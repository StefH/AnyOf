using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using AnyOf.System.Text.Json.Matcher.Models;
using AnyOfTypes.System.Text.Json.Matcher;
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

                    value = FindBestArrayMatch(jsonElement, typeToConvert, options);
                    break;

                case JsonValueKind.Object:
                    value = FindBestObjectMatch(jsonElement, typeToConvert?.GetGenericArguments() ?? new Type[0], options);
                    break;

                default:
                    value = GetSimpleValue(jsonElement);
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

        private static object? GetSimpleValue(JsonElement reader)
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

        private object? FindBestArrayMatch(JsonElement jsonElement, Type? typeToConvert, JsonSerializerOptions options)
        {
            var enumerableTypes = typeToConvert?.GetGenericArguments().Where(t => typeof(IEnumerable).IsAssignableFrom(t)).ToArray() ?? new Type[0];
            var types = enumerableTypes.Select(t => GetElementType(t)).ToArray();

            var list = new List<object?>();

            Type? elementType = null;
            foreach (var arrayElement in jsonElement.EnumerateArray())
            {
                object? value;
                if (arrayElement.ValueKind == JsonValueKind.Object)
                {
                    value = FindBestObjectMatch(arrayElement, types, options);
                }
                else
                {
                    value = GetSimpleValue(arrayElement);
                }

                if (elementType is null)
                {
                    elementType = value?.GetType();
                }

                list.Add(value);
            }

            if (elementType is null)
            {
                return null;
            }

            var (newList, newListType) = CastToTypedList(list, elementType);

            foreach (var knownIEnumerableType in enumerableTypes)
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
            var list = (IList)Activator.CreateInstance(listType);
            foreach (var item in source)
            {
                list.Add(item);
            }

            return (list, listType);
        }

        private static object? FindBestObjectMatch(JsonElement objectElement, Type[] types, JsonSerializerOptions options)
        {
            var properties = new List<PropertyDetails>();
            foreach (var element in objectElement.EnumerateObject())
            {
                var propertyDetails = new PropertyDetails
                {
                    CanRead = true,
                    CanWrite = true,
                    IsPublic = true,
                    Name = element.Name
                };

                object? val;
                switch (element.Value.ValueKind)
                {
                    case JsonValueKind.Object:
                        val = FindBestObjectMatch(element.Value, types, options);
                        break;

                    default:
                        val = GetSimpleValue(element.Value);
                        break;
                }

                propertyDetails.PropertyType = val?.GetType();
                propertyDetails.IsValueType = val?.GetType().IsValueType == true;

                properties.Add(propertyDetails);
            }

            var mostSuitableType = MatchFinder.FindBestType(properties, types);
            if (mostSuitableType is not null)
            {
                return ToObject(objectElement, mostSuitableType, options);
            }

            throw new JsonException("No suitable type found.");
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

        private static Type GetElementType(Type enumerableType)
        {
            return enumerableType.IsArray ? enumerableType.GetElementType() : enumerableType.GetGenericArguments().First();
        }
    }
}