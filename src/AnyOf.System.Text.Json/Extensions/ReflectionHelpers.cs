using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AnyOf.System.Text.Json.Extensions
{
    public static class ReflectionHelpers
    {
        [ThreadStatic]
        static readonly Dictionary<KeyValuePair<Type, Type>, bool> ImplicitCastCache = new Dictionary<KeyValuePair<Type, Type>, bool>();

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
                Convert.ChangeType(val, to);
                changeType = true;
            }
            catch
            {
                // Ignore
            }

            return ImplicitCastCache[key] = changeType;
        }
    }
}