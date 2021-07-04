using System;

namespace AnyOfGenerator
{
    internal static class IntExtensions
    {
        public static string Ordinalize(this int value)
        {
            switch (value)
            {
                case 1:
                    return "First";

                case 2:
                    return "Second";

                case 3:
                    return "Third";

                case 4:
                    return "Fourth";

                case 5:
                    return "Fifth";
            }

            throw new NotSupportedException();
        }
    }
}