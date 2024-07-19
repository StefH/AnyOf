using System;

namespace AnyOfTypes.System.Text.Json.Models;

internal struct PropertyDetails
{
    public string Name { get; set; }

    public bool CanRead { get; set; }

    public bool CanWrite { get; set; }

    public bool IsPublic { get; set; }

    public bool IsValueType { get; set; }

    public Type? PropertyType { get; set; }
}