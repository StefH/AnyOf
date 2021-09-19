using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AnyOfTypes.System.Text.Json
{
    internal struct PropertyMap
    {
        public PropertyDetails SourceProperty { get; set; }

        public PropertyDetails TargetProperty { get; set; }
    }

    internal struct PropertyDetails
    {
        public string Name { get; set; }

        public bool CanRead { get; set; }

        public bool CanWrite { get; set; }

        public bool IsPublic { get; set; }

        public bool IsValueType { get; set; }

        public Type? PropertyType { get; set; }
    }

    internal static class Mapper
    {
        public static IList<PropertyMap> GetMatchingProperties(IEnumerable<PropertyDetails> sourceProperties, IEnumerable<PropertyDetails> targetProperties)
        {
            return
            (
                from s in sourceProperties
                from t in targetProperties
                where s.Name == t.Name &&
                    s.CanRead &&
                    t.CanWrite &&
                    s.IsPublic &&
                    t.IsPublic &&
                    s.PropertyType == t.PropertyType &&
                    (
                        (s.IsValueType && t.IsValueType) || (s.PropertyType == typeof(string) && t.PropertyType == typeof(string))
                    )
                select new PropertyMap
                {
                    SourceProperty = s,
                    TargetProperty = t
                }
            ).ToList();
        }

        public static IList<PropertyMap> GetMatchingProperties(Type sourceType, Type targetType)
        {
            return GetMatchingProperties(Map(sourceType.GetProperties()), Map(targetType.GetProperties()));
        }

        private static IEnumerable<PropertyDetails> Map(PropertyInfo[] properties)
        {
            return properties.Select(p => new PropertyDetails
            {
                CanRead = p.CanRead,
                CanWrite = p.CanWrite,
                IsPublic = p.PropertyType.IsPublic,
                IsValueType = p.PropertyType.IsValueType,
                Name = p.Name,
                PropertyType = p.PropertyType
            });
        }

        public static Type? FindBestType(IEnumerable<PropertyDetails> sourceType, Type[] targetTypes)
        {
            Type? mostSuitableType = null;
            int countOfMaxMatchingProperties = -1;

            foreach (var targetType in targetTypes)
            {
                var propMap = GetMatchingProperties(sourceType, Map(targetType.GetProperties()));
                if (propMap.Count > countOfMaxMatchingProperties)
                {
                    mostSuitableType = targetType;
                    countOfMaxMatchingProperties = propMap.Count;
                }
            }

            return mostSuitableType;
        }
    }
}