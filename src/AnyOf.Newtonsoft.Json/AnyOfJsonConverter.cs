using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Linq;
using AnyOf.System.Text.Json.Matcher.Models;
using AnyOfTypes.System.Text.Json.Matcher;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AnyOfTypes.Newtonsoft.Json
{
    public class AnyOfJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                if (serializer.NullValueHandling == NullValueHandling.Include)
                {
                    serializer.Serialize(writer, value);
                }
                return;
            }

            var currentValue = GetNullablePropertyValue(value, "CurrentValue");
            if (currentValue == null)
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
            if (reader.TokenType == JsonToken.Null)
            {
                return Activator.CreateInstance(objectType);
            }

            if (reader.TokenType != JsonToken.StartObject)
            {
                var jValue = new JValue(reader.Value);

                object? value = null;
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

                return Activator.CreateInstance(objectType, value);
            }

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

            var bestType = MatchFinder.FindBestType(properties, objectType?.GetGenericArguments() ?? new Type[0]);
            if (bestType is not null)
            {
                var target = Activator.CreateInstance(bestType);

                using (JsonReader jObjectReader = CopyReaderForObject(reader, jObject))
                {
                    serializer.Populate(jObjectReader, target);
                }

                return Activator.CreateInstance(objectType, target);
            }

            throw new SerializationException($"Could not deserialize '{objectType}', no suitable type found.");

            Type? mostSuitableType = null;
            int countOfMaxMatchingProperties = -1;

            // Take the names of elements from json data
            var jObjectKeys = GetKeys(jObject);

            // Take the public properties of the parent class
            var objectTypeProps = objectType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(p => p.Name).ToHashSet();

            // Trying to find the right "KnownType"
            foreach (var knownType in GetPropertyValue<Type[]>(existingValue, "Types"))
            {
                // Select properties of the inheritor, except properties from the parent class and properties with "ignore" attributes
                var notIgnoreProps = knownType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => !objectTypeProps.Contains(p.Name) && p.CustomAttributes.All(a => a.AttributeType != typeof(JsonIgnoreAttribute)));

                // Get serializable property names
                var jsonNameFields = notIgnoreProps.Select(prop =>
                {
                    var jsonPropertyAttribute = prop.CustomAttributes.FirstOrDefault(a => a.AttributeType == typeof(JsonPropertyAttribute));
                    if (jsonPropertyAttribute != null)
                    {
                        // Take the name of the json element from the attribute constructor
                        int constructorArgumentsCount = jsonPropertyAttribute.ConstructorArguments.Count;
                        if (constructorArgumentsCount > 0)
                        {
                            var argument = jsonPropertyAttribute.ConstructorArguments.First();
                            if (argument.ArgumentType == typeof(string) && !string.IsNullOrEmpty(argument.Value as string))
                            {
                                return (string)argument.Value;
                            }
                        }
                    }

                    // Otherwise, take the name of the property
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
                var target = Activator.CreateInstance(mostSuitableType);

                using (JsonReader jObjectReader = CopyReaderForObject(reader, jObject))
                {
                    serializer.Populate(jObjectReader, target);
                }

                return Activator.CreateInstance(objectType, target);
            }

            throw new SerializationException($"Could not deserialize '{objectType}', no suitable type found.");
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.FullName.StartsWith("AnyOfTypes.AnyOf`");
        }

        private HashSet<string> GetKeys(JObject obj)
        {
            return new HashSet<string>(((IEnumerable<KeyValuePair<string, JToken>>)obj).Select(k => k.Key));
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