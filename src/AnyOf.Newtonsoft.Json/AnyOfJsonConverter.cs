using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RestEaseClientGeneratorConsoleApp
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

            var currentValue = GetPropertyValue(value, "CurrentValue");
            serializer.Serialize(writer, currentValue);
        }

        /// <summary>
        /// See https://stackoverflow.com/questions/8030538/how-to-implement-custom-jsonconverter-in-json-net/17247339#17247339
        /// </summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            Type mostSuitableType = null;
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
                    string jsonFieldName = null;
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
                                jsonFieldName = (string)argument.Value;
                            }
                        }
                    }

                    // Otherwise, take the name of the property
                    if (string.IsNullOrEmpty(jsonFieldName))
                    {
                        jsonFieldName = prop.Name;
                    }

                    return jsonFieldName;
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
                object target = Activator.CreateInstance(mostSuitableType);

                using (JsonReader jObjectReader = CopyReaderForObject(reader, jObject))
                {
                    serializer.Populate(jObjectReader, target);
                }

                return Activator.CreateInstance(objectType, target);
            }

            throw new SerializationException($"Could not deserialize {objectType}, no suitable type found.");
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

        private static T GetPropertyValue<T>(object value, string name)
        {
            return (T)GetPropertyValue(value, name);
        }

        private static object GetPropertyValue(object value, string name)
        {
            return value.GetType().GetProperty(name).GetValue(value);
        }
    }
}