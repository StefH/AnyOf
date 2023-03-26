using System;

namespace AnyOfGenerator;

internal static class IntExtensions
{
    private static readonly string[] Order = { "Zeroth", "First", "Second", "Third", "Fourth", "Fifth", "Sixth", "Seventh", "Eighth", "Ninth", "Tenth" };

    public static string Ordinalize(this int value)
    {
        if (value < Order.Length)
        {
            return Order[value];
        }

        throw new NotSupportedException();
    }
}