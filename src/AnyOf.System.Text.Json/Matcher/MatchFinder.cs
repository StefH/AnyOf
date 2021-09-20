using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AnyOf.System.Text.Json.Extensions;
using AnyOf.System.Text.Json.Matcher.Models;

namespace AnyOfTypes.System.Text.Json.Matcher
{
    internal static class MatchFinder
    {
        public static Type? FindBestType(IEnumerable<PropertyDetails> sourceProperties, Type[] targetTypes, bool returnNullIfNoMatchFound = true)
        {
            Type? mostSuitableType = null;
            int countOfMaxMatchingProperties = -1;

            foreach (var targetType in targetTypes)
            {
                var propMap = GetMatchingProperties(sourceProperties, Map(targetType.GetProperties()));
                if (propMap.Count > countOfMaxMatchingProperties)
                {
                    mostSuitableType = targetType;
                    countOfMaxMatchingProperties = propMap.Count;
                }
            }

            return countOfMaxMatchingProperties == 0 && returnNullIfNoMatchFound ? null : mostSuitableType;
        }

        private static IList<PropertyMap> GetMatchingProperties(IEnumerable<PropertyDetails> sourceProperties, IEnumerable<PropertyDetails> targetProperties)
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
                    (s.PropertyType == t.PropertyType || s.PropertyType.IsImplicitlyCastableTo(t.PropertyType) || t.PropertyType.IsImplicitlyCastableTo(s.PropertyType)) &&
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
                IsPublic = p.PropertyType.GetTypeInfo().IsPublic,
                IsValueType = p.PropertyType.GetTypeInfo().IsValueType,
                Name = p.Name,
                PropertyType = p.PropertyType
            });
        }
    }
}