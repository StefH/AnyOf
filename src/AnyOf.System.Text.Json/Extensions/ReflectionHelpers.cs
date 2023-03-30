using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AnyOfTypes.System.Text.Json.Models;

namespace AnyOfTypes.System.Text.Json.Extensions;

internal static class ReflectionHelpers
{
    [ThreadStatic]
    private static readonly Dictionary<KeyValuePair<Type, Type>, bool> ImplicitCastCache = new();

    public static T GetPropertyValue<T>(this object instance, string name)
    {
        var value = GetNullablePropertyValue(instance, name);
        if (value is null)
        {
            throw new InvalidOperationException($"The public property '{name}' has a null value.");
        }

        return (T)value;
    }

    public static object? GetNullablePropertyValue(this object instance, string name)
    {
        var type = instance.GetType();
        var propertyInfo = type.GetProperty(name);
        if (propertyInfo is null)
        {
            throw new InvalidOperationException($"The type '{type}' does not contain public property '{name}'.");
        }

        return propertyInfo.GetValue(instance);
    }

    public static Type GetElementTypeX(this Type enumerableType)
    {
        return enumerableType.IsArray == true ? enumerableType.GetElementType() : enumerableType.GetGenericArguments().First();
    }

    public static ListDetails CastToTypedList(this IList source, Type elementType)
    {
        var listType = typeof(List<>).MakeGenericType(elementType);
        var list = (IList)Activator.CreateInstance(listType);

        foreach (var item in source)
        {
            list.Add(item);
        }

        return new ListDetails
        {
            List = list,
            ListType = listType
        };
    }

    /// <summary>
    /// https://stackoverflow.com/questions/17676838/how-to-check-if-type-can-be-converted-to-another-type-in-c-sharp
    /// https://stackoverflow.com/questions/2224266/how-to-tell-if-type-a-is-implicitly-convertible-to-type-b
    /// </summary>
    public static bool IsImplicitlyCastableTo(this Type? from, Type? to)
    {
        if (from is null || to is null)
        {
            return false;
        }

        if (from == to)
        {
            return true;
        }

        var key = new KeyValuePair<Type, Type>(from, to);
        if (ImplicitCastCache.TryGetValue(key, out bool result))
        {
            return result;
        }

#if !NETSTANDARD1_3
        if (to.IsAssignableFrom(from))
        {
            return ImplicitCastCache[key] = true;
        }
#endif
        if (from.GetMethods(BindingFlags.Public | BindingFlags.Static).Any(m => m.ReturnType == to && (m.Name == "op_Implicit" || m.Name == "op_Explicit")))
        {
            return ImplicitCastCache[key] = true;
        }

        bool changeType = false;
        try
        {
            var val = Activator.CreateInstance(from);
            _ = Convert.ChangeType(val, to);
            changeType = true;
        }
        catch
        {
            // Ignore
        }

        return ImplicitCastCache[key] = changeType;
    }
}
