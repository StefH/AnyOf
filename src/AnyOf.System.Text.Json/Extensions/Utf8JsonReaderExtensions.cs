using System;
using System.Buffers;
using System.Linq;
using System.Text.Json;

namespace AnyOfTypes.System.Text.Json.Extensions
{
    internal static class Utf8JsonReaderExtensions
    {
        public static ReadOnlySpan<byte> GetRawString(this Utf8JsonReader reader)
        {
            return reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
        }
    }
}