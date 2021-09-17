using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Dahomey.Json.Serialization;
using Dahomey.Json.Serialization.Converters;
using Dahomey.Json.Util;

namespace AnyOfTypes.System.Text.Json
{
    public class AnyOfJsonConverter : BaseObjectConverter
    {
        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            _ = ReadJsonNode(ref reader, typeToConvert, options, out object? anyOfValue);
            return Activator.CreateInstance(typeToConvert, anyOfValue);
        }

        private JsonNode ReadJsonNode(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options, out object? anyOfValue)
        {
            anyOfValue = null;

            switch (reader.TokenType)
            {
                case JsonTokenType.StartObject:
                    using (new DepthHandler(options))
                    {
                        anyOfValue = ReadObject(ref reader, typeToConvert, options);
                        return false;
                    }

                case JsonTokenType.StartArray:
                    using (new DepthHandler(options))
                    {
                        return ReadArray(ref reader, options);
                    }

                case JsonTokenType.String:
                    return reader.GetString();

                case JsonTokenType.Number:
#if NETSTANDARD2_0
                    var stringValue = Encoding.ASCII.GetString(reader.GetRawString().ToArray());
#else
                    var stringValue = Encoding.ASCII.GetString(reader.GetRawString());
#endif
                    var number = new JsonNumber(stringValue);

                    anyOfValue = number.GetInt32();

                    return number;

                case JsonTokenType.True:
                    anyOfValue = true;
                    return true;

                case JsonTokenType.False:
                    anyOfValue = false;
                    return false;

                case JsonTokenType.Null:
                    return new JsonNull();

                default:
                    throw new JsonException();
            }
        }

        private JsonArray ReadArray(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            var array = new JsonArray();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                JsonNode node = ReadJsonNode(ref reader, typeof(JsonNode), options, out _);
                array.Add(node);
            }

            return array;
        }

        private object? ReadObject(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var obj = new JsonObject();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                string? propertyName = reader.GetString();
                if (propertyName == null)
                {
                    throw new JsonException("Property name cannot be null");
                }

                reader.Read();
                JsonNode propertyValue = ReadJsonNode(ref reader, typeof(JsonNode), options, out _);

                obj.Add(propertyName, propertyValue);
            }

            Type? mostSuitableType = null;
            int countOfMaxMatchingProperties = -1;

            // Take the names of elements from json data
            var jObjectKeys = GetKeys(obj);

            var existingValue = Activator.CreateInstance(typeToConvert);
            if (existingValue is null)
            {
                throw new JsonException($"Could not create instance of type {typeToConvert}.");
            }

            // Trying to find the right "KnownType"
            foreach (var knownType in GetPropertyValue<Type[]>(existingValue, "Types"))
            {
                // Select properties
                var notIgnoreProps = knownType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

                // Get serializable property names
                var jsonNameFields = notIgnoreProps.Select(prop =>
                {
                    return prop.Name;
                });

                var jKnownTypeKeys = new HashSet<string>(jsonNameFields);

                // By intersecting the sets of names we determine the most suitable inheritor
                int count = jObjectKeys.Intersect(jKnownTypeKeys).Count();

                if (count == jKnownTypeKeys.Count)
                {
                    mostSuitableType = knownType;
                    break;
                }

                if (count > countOfMaxMatchingProperties)
                {
                    countOfMaxMatchingProperties = count;
                    mostSuitableType = knownType;
                }
            }

            if (mostSuitableType != null)
            {
                //var x = JsonObject.

                

                var mostSuitableTypeInstance = Activator.CreateInstance(mostSuitableType);
                if (mostSuitableTypeInstance is null)
                {
                    throw new JsonException($"Could not create instance of type {mostSuitableType}.");
                }

                return mostSuitableTypeInstance;
            }

            throw new JsonException($"Could not deserialize {typeToConvert}, no suitable type found.");
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

            Console.WriteLine(objectType);
            return objectType.FullName.StartsWith("AnyOfTypes.AnyOf`");
        }

        private HashSet<string> GetKeys(JsonObject obj)
        {
            return new HashSet<string>(((IEnumerable<KeyValuePair<string, JsonNode>>)obj).Select(k => k.Key));
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