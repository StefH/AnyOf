using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using AnyOfTypes.System.Text.Json.Extensions;
using Nelibur.ObjectMapper;

namespace AnyOfTypes.System.Text.Json
{
    public class AnyOfJsonConverter : JsonConverter<object?>
    {
        public override object? Read(ref Utf8JsonReader reader, Type? typeToConvert, JsonSerializerOptions options)
        {
            object? value;

            var jsonElement = GetConverter<JsonElement>(options).Read(ref reader, typeof(object), options);

            switch (jsonElement.ValueKind)
            {
                case JsonValueKind.Array:
                    value = FindBestArrayMatch(jsonElement, typeToConvert ?? typeof(object), options);

                    //var list = new List<object?>();

                    //foreach (var arrayElement in jsonElement.EnumerateArray())
                    //{
                    //    var valuex = FindBestObjectMatch(arrayElement, typeToConvert ?? typeof(object), options);

                    //    if (arrayElement.ValueKind == JsonValueKind.Object)
                    //    {
                    //        value = FindBestObjectMatch(arrayElement, typeToConvert ?? typeof(object), options);
                    //    }
                    //    else
                    //    {
                    //        value = GetNonObjectValue(arrayElement);
                    //    }

                    //    list.Add(value);
                    //}

                    //Type? listType = null;
                    //if (list.Any())
                    //{
                    //    listType = list.First()?.GetType();
                    //}

                    //listType ??= typeof(object);

                    //var arr = Array.CreateInstance(listType, list.Count());
                    //Array.Copy(list.ToArray(), arr, list.Count);

                    //value = arr;
                    break;

                case JsonValueKind.Object:
                    value = FindBestObjectMatch(jsonElement, typeToConvert ?? typeof(object), options);
                    break;

                default:
                    value = AnyOfJsonConverter.GetNonObjectValue(jsonElement);
                    break;
            }

            if (typeToConvert is null)
            {
                return value;
            }

            if (value is null)
            {
                return Activator.CreateInstance(typeToConvert);
            }

            return Activator.CreateInstance(typeToConvert, value);
        }

        private static object? GetNonObjectValue(JsonElement reader)
        {

            switch (reader.ValueKind)
            {
                case JsonValueKind.String:
                    if (reader.TryGetDateTime(out var date))
                    {
                        return date;
                    }

                    return reader.GetString();

                case JsonValueKind.Number:
                    if (reader.TryGetInt32(out var i))
                    {
                        return i;
                    }

                    if (reader.TryGetInt64(out var l))
                    {
                        return l;
                    }

                    if (reader.TryGetUInt32(out var ui))
                    {
                        return ui;
                    }

                    if (reader.TryGetUInt64(out var ul))
                    {
                        return ul;
                    }

                    return reader.GetDecimal();

                case JsonValueKind.True:
                    return true;

                case JsonValueKind.False:
                    return false;

                case JsonValueKind.Null:
                    return null;

                default:
                    throw new JsonException($"The ValueKind '{reader.ValueKind}' is not supported.");
            }
        }

        private (object value, Type type) ReadNumber(ref Utf8JsonReader reader)
        {
            ReadOnlySpan<byte> buffer = reader.GetRawString();
            if (Utf8Parser.TryParse(buffer, out int iValue, out int bytesConsumed) && bytesConsumed == buffer.Length)
            {
                return (iValue, typeof(int));
            }

            if (Utf8Parser.TryParse(buffer, out long lValue, out bytesConsumed) && bytesConsumed == buffer.Length)
            {
                return (lValue, typeof(long));
            }

            if (Utf8Parser.TryParse(buffer, out ulong ulValue, out bytesConsumed) && bytesConsumed == buffer.Length)
            {
                return (ulValue, typeof(ulong));
            }

            if (Utf8Parser.TryParse(buffer, out double dblValue, out bytesConsumed) && bytesConsumed == buffer.Length)
            {
                return (dblValue, typeof(double));
            }

            throw new JsonException();
        }

        private object? FindBestArrayMatch(JsonElement jsonElement, Type typeToConvert, JsonSerializerOptions options)
        {
            var list = new List<object?>();

            Type? elementType = null;
            foreach (var arrayElement in jsonElement.EnumerateArray())
            {
                object? value;
                if (arrayElement.ValueKind == JsonValueKind.Object)
                {
                    value = FindBestObjectMatch(arrayElement, typeToConvert ?? typeof(object), options);
                    elementType = value?.GetType();
                }
                else
                {
                    value = AnyOfJsonConverter.GetNonObjectValue(arrayElement);
                    elementType = value?.GetType();
                }

                list.Add(value);
            }

            if (elementType is null)
            {
                return null;
            }

            var (newList, newListType) = CastToTypedList(list, elementType);

            foreach (var knownIEnumerableType in typeToConvert.GetGenericArguments().Where(t => typeof(IEnumerable).IsAssignableFrom(t)))
            {
                if (GetElementType(knownIEnumerableType) == elementType)
                {
                    TinyMapper.Bind(newListType, knownIEnumerableType);
                    return TinyMapper.Map(newListType, knownIEnumerableType, newList);
                }
            }

            return null;
        }

        public static (IList, Type) CastToTypedList(IList source, Type elementType)
        {
            var listType = typeof(List<>).MakeGenericType(elementType);
            IList list = (IList) Activator.CreateInstance(listType);
            foreach (var item in source) 
            {
                list.Add(item);
            }

            return (list, listType);
        }

        private static Type? GetElementType(Type enumerableType)
        {
            return enumerableType.IsArray ? enumerableType.GetElementType() : enumerableType.GetGenericArguments().First();
        }

        private static object? FindBestObjectMatch(JsonElement jsonElement, Type typeToConvert, JsonSerializerOptions options)
        {
            var properties = new List<P>();
            foreach (var e in jsonElement.EnumerateObject())
            {
                var p = new P
                {
                    CanRead = true,
                    CanWrite = true,
                    IsPublic = true,
                    Name = e.Name
                };

                object? val;
                switch (e.Value.ValueKind)
                {
                    case JsonValueKind.Object:
                        val = FindBestObjectMatch(e.Value, typeToConvert,options);
                        break;

                    default:
                        val = GetNonObjectValue(e.Value);
                        break;

                }

                p.PropertyType = val?.GetType();
                p.IsValueType = val?.GetType().IsValueType == true;

                properties.Add(p);
            }

            //var properties = jsonElement.EnumerateObject().Select(e => new P
            //{
            //    CanRead = true,
            //    CanWrite = true,
            //    IsPublic = true,
            //    Name = e.Name,
            //    IsValueType = e.Value.GetType().IsValueType,
            //    PropertyType = e.Value.GetType()
            //});

            var mostSuitableType = Mapper.FindBestType(properties, typeToConvert.GetGenericArguments());




            //Type? mostSuitableType = null;
            //int countOfMaxMatchingProperties = -1;




            //// Take the names of elements from json data
            //var jObjectKeys = jsonElement.EnumerateObject().Select(p => p.Name);

            //foreach (var knownType in typeToConvert.GetGenericArguments())
            //{
            //    // Select properties
            //    var notIgnoreProps = knownType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            //    // Get serializable property names
            //    var jsonNameFields = notIgnoreProps.Select(prop => prop.Name);

            //    var jKnownTypeKeys = new HashSet<string>(jsonNameFields);

            //    // By intersecting the sets of names we determine the most suitable inheritor
            //    int count = jObjectKeys.Intersect(jKnownTypeKeys).Count();

            //    if (count == jKnownTypeKeys.Count)
            //    {
            //        mostSuitableType = knownType;
            //        break;
            //    }

            //    if (count > countOfMaxMatchingProperties)
            //    {
            //        countOfMaxMatchingProperties = count;
            //        mostSuitableType = knownType;
            //    }
            //}

            if (mostSuitableType != null)
            {
                return ToObject(jsonElement, mostSuitableType, options);
            }

            throw new JsonException($"Could not deserialize '{typeToConvert}', no suitable type found.");
        }

        public override void Write(Utf8JsonWriter writer, object? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            var currentValue = GetNullablePropertyValue(value, "CurrentValue");
            if (currentValue == null)
            {
                writer.WriteNullValue();
                return;
            }

            var currentType = GetPropertyValue<Type>(value, "CurrentValueType");
            JsonSerializer.Serialize(writer, currentValue, currentType, options);
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType.FullName is null)
            {
                return false;
            }

            return objectType.FullName.StartsWith("AnyOfTypes.AnyOf`");
        }

        /// <summary>
        /// - https://stackoverflow.com/questions/58138793/system-text-json-jsonelement-toobject-workaround
        /// - https://stackoverflow.com/a/58193164/255966
        /// </summary>
        private static object? ToObject(JsonElement element, Type returnType, JsonSerializerOptions? options = null)
        {
            var json = element.GetRawText();
            return JsonSerializer.Deserialize(json, returnType, options);
        }

        /// <summary>
        /// https://github.com/dahomey-technologies/Dahomey.Json/blob/master/src/Dahomey.Json/Util/Utf8JsonReaderExtensions.cs
        /// </summary>
        private static JsonConverter<T> GetConverter<T>(JsonSerializerOptions options)
        {
            return (JsonConverter<T>)options.GetConverter(typeof(T));
        }

        private static T GetPropertyValue<T>(object instance, string name)
        {
            var value = GetNullablePropertyValue(instance, name);
            if (value is null)
            {
                throw new JsonException($"The public property '{name}' has a null value.");
            }

            return (T)value;
        }

        private static object? GetNullablePropertyValue(object instance, string name)
        {
            var type = instance.GetType();
            var propertyInfo = type.GetProperty(name);
            if (propertyInfo is null)
            {
                throw new JsonException($"The type '{type}' does not contain public property '{name}'.");
            }

            return propertyInfo.GetValue(instance);
        }
    }
}