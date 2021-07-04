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

                case 6:
                    return "Sixth";

                case 7:
                    return "Seventh";

                case 8:
                    return "Eighth";

                case 9:
                    return "Ninth";

                case 10:
                    return "Tenth";
            }

            throw new NotSupportedException();
        }
    }
}