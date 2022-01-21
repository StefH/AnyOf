using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AnyOfTypes.System.Text.Json.Extensions;
using AnyOfTypes.System.Text.Json.Matcher;
using AnyOfTypes.System.Text.Json.Matcher.Models;
using Nelibur.ObjectMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AnyOfTypes.Newtonsoft.Json
{
    public class AnyOfJsonConverter : JsonConverter
    {
        private readonly bool _ignoreCase;

        public AnyOfJsonConverter(bool ignoreCase = true)
        {
            _ignoreCase = ignoreCase;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is null)
            {
                if (serializer.NullValueHandling == NullValueHandling.Include)
                {
                    serializer.Serialize(writer, value);
                }
                return;
            }

            var currentValue = value.GetNullablePropertyValue("CurrentValue");
            if (currentValue is null)
            {
                if (serializer.NullValueHandling == NullValueHandling.Include)
                {
                    serializer.Serialize(writer, currentValue);
                }
                return;
            }

            serializer.Serialize(writer, currentValue);
        }

        /// <summary>
        /// See
        /// - https://stackoverflow.com/questions/8030538/how-to-implement-custom-jsonconverter-in-json-net
        /// - https://stackoverflow.com/a/59286262/255966
        /// </summary>
        public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object? value;
            switch (reader.TokenType)
            {
                case JsonToken.Null:
                    value = null;
                    break;

                case JsonToken.StartObject:
                    value = FindBestObjectMatch(reader, objectType?.GetGenericArguments() ?? new Type[0], serializer);
                    break;

                case JsonToken.StartArray:
                    value = FindBestArrayMatch(reader, objectType, existingValue, serializer);
                    break;

                default:
                    value = GetSimpleValue(reader, existingValue);
                    break;
            }

            if (value is null)
            {
                return Activator.CreateInstance(objectType);
            }

            return Activator.CreateInstance(objectType, value);
        }

        private static object? GetSimpleValue(JsonReader reader, object existingValue)
        {
            var jValue = new JValue(reader.Value);

            object? value;
            switch (reader.TokenType)
            {
                case JsonToken.String:
                    value = (string)jValue;
                    break;

                case JsonToken.Date:
                    value = (DateTime)jValue;
                    break;

                case JsonToken.Boolean:
                    value = (bool)jValue;
                    break;

                case JsonToken.Integer:
                    value = (int)jValue;
                    break;

                case JsonToken.Float:
                    value = (double)jValue;
                    break;

                default:
                    value = jValue.Value;
                    break;
            }

            if (value is null)
            {
                return existingValue;
            }

            return value;
        }

        private object? FindBestArrayMatch(JsonReader reader, Type? typeToConvert, object existingValue, JsonSerializer serializer)
        {
            var enumerableTypes = typeToConvert?.GetGenericArguments().Where(t => typeof(IEnumerable).IsAssignableFrom(t)).ToArray() ?? new Type[0];
            var elementTypes = enumerableTypes.Select(t => t.GetElementTypeX()).ToArray();

            var list = new List<object?>();
            Type? elementType = null;

            while (reader.Read() && reader.TokenType != JsonToken.EndArray)
            {
                object? value;
                if (reader.TokenType == JsonToken.StartObject)
                {
                    value = FindBestObjectMatch(reader, elementTypes, serializer);
                }
                else
                {
                    value = GetSimpleValue(reader, existingValue);
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

            var typedListDetails = list.CastToTypedList(elementType);

            foreach (var knownIEnumerableType in enumerableTypes)
            {
                if (knownIEnumerableType.GetElementTypeX() == elementType)
                {
                    TinyMapper.Bind(typedListDetails.ListType, knownIEnumerableType);
                    return TinyMapper.Map(typedListDetails.ListType, knownIEnumerableType, typedListDetails.List);
                }
            }

            return null;
        }

        private object? FindBestObjectMatch(JsonReader reader, Type[] types, JsonSerializer serializer)
        {
            var properties = new List<PropertyDetails>();
            var jObject = JObject.Load(reader);
            foreach (var element in jObject)
            {
                var propertyDetails = new PropertyDetails
                {
                    CanRead = true,
                    CanWrite = true,
                    IsPublic = true,
                    Name = element.Key
                };

                var val = element.Value.ToObject<object?>();
                propertyDetails.PropertyType = val?.GetType();
                propertyDetails.IsValueType = val?.GetType().GetTypeInfo().IsValueType == true;

                properties.Add(propertyDetails);
            }

            var bestType = MatchFinder.FindBestType(_ignoreCase, properties, types);
            if (bestType is not null)
            {
                var target = Activator.CreateInstance(bestType);

                using (JsonReader jObjectReader = CopyReaderForObject(reader, jObject))
                {
                    serializer.Populate(jObjectReader, target);
                }

                return target;
            }

            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.FullName.StartsWith("AnyOfTypes.AnyOf`");
        }

        private static JsonReader CopyReaderForObject(JsonReader reader, JObject jObject)
        {
            var jObjectReader = jObject.CreateReader();
            jObjectReader.CloseInput = reader.CloseInput;
            jObjectReader.Culture = reader.Culture;
            jObjectReader.DateFormatString = reader.DateFormatString;
            jObjectReader.DateParseHandling = reader.DateParseHandling;
            jObjectReader.DateTimeZoneHandling = reader.DateTimeZoneHandling;
            jObjectReader.FloatParseHandling = reader.FloatParseHandling;
            jObjectReader.MaxDepth = reader.MaxDepth;
            jObjectReader.SupportMultipleContent = reader.SupportMultipleContent;
            return jObjectReader;
        }
    }
}