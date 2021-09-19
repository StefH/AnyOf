using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AnyOfTypes.System.Text.Json
{
    internal class PropertyMap
    {
        //public PropertyInfo SourceProperty { get; set; }

        //public PropertyInfo TargetProperty { get; set; }

        public P SourceProperty { get; set; }

        public P TargetProperty { get; set; }
    }

    internal class P
    {
        public string Name { get; set; }

        public bool CanRead { get; set; }

        public bool CanWrite { get; set; }

        public bool IsPublic { get; set; }

        public bool IsValueType { get; set; }

        public Type PropertyType { get; set; }
    }

    internal static class Mapper
    {
        public static IList<PropertyMap> GetMatchingProperties(IEnumerable<P> sourceProperties, IEnumerable<P> targetProperties)
        {
            var properties = (from s in sourceProperties
                              from t in targetProperties
                              where s.Name == t.Name &&
                                    s.CanRead &&
                                    t.CanWrite &&
                                    s.PropertyType.IsPublic &&
                                    t.PropertyType.IsPublic &&
                                    s.PropertyType == t.PropertyType &&
                                    (
                                      (s.PropertyType.IsValueType && t.PropertyType.IsValueType) ||
                                      (s.PropertyType == typeof(string) && t.PropertyType == typeof(string))
                                    )
                              select new PropertyMap
                              {
                                  SourceProperty = s,
                                  TargetProperty = t
                              }).ToList();

            return properties;
        }

        public static IList<PropertyMap> GetMatchingProperties(Type sourceType, Type targetType)
        {
            return GetMatchingProperties(Map(sourceType.GetProperties()), Map(targetType.GetProperties()));
        }

        private static IEnumerable<P> Map(PropertyInfo[] properties)
        {
            return properties.Select(s => new P
            {
                CanRead = s.CanRead,
                CanWrite = s.CanWrite,
                IsPublic = s.PropertyType.IsPublic,
                IsValueType = s.PropertyType.IsValueType,
                Name = s.Name,
                PropertyType = s.PropertyType
            });
        }

        public static string GetClassName(Type sourceType, Type targetType)
        {
            var className = "Copy_";

            className += sourceType.FullName.Replace(".", "_");
            className += "_";
            className += targetType.FullName.Replace(".", "_");

            return className;
        }

        private static Dictionary<string, PropertyMap[]> _maps = new Dictionary<string, PropertyMap[]>();

        public static void AddPropertyMap<T, TU>()
        {
            var props = GetMatchingProperties(typeof(T), typeof(TU));
            var className = GetClassName(typeof(T), typeof(TU));

            _maps.Add(className, props.ToArray());

        }

        //public static void CopyMatchingCachedProperties(object source, object target)
        //{
        //    var className = GetClassName(source.GetType(), target.GetType());

        //    var propMap = _maps[className];
        //    for (var i = 0; i < propMap.Length; i++)
        //    {
        //        var prop = propMap[i];

        //        var sourceValue = prop.SourceProperty.GetValue(source, null);
        //        prop.TargetProperty.SetValue(target, sourceValue, null);
        //    }
        //}

        public static Type? FindBestType(IEnumerable<P> sourceType, Type[] targetTypes)
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

        //public static void CopyProperties(object source, object target)
        //{
        //    var sourceType = source.GetType();
        //    var targetType = target.GetType();

        //    var propMap = GetMatchingProperties(sourceType, targetType);
        //    for (var i = 0; i < propMap.Count; i++)
        //    {
        //        var prop = propMap[i];

        //        var sourceValue = prop.SourceProperty.GetValue(source, null);

        //        prop.TargetProperty.SetValue(target, sourceValue, null);
        //    }
        //}
    }
}