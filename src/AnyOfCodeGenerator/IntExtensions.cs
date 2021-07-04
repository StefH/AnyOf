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
            }

            return string.Empty;
        }
    }
}