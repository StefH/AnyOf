using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using AnyOfTypes.System.Text.Json.Extensions;
using AnyOfTypes.System.Text.Json.Matcher;
using AnyOfTypes.System.Text.Json.Matcher.Models;
using Nelibur.ObjectMapper;
using System.Diagnostics.CodeAnalysis;

namespace AnyOfTypes.System.Text.Json;

public class AnyOfJsonConverter : JsonConverter<object?>
{
    private readonly bool _ignoreCase;

    public AnyOfJsonConverter(bool ignoreCase = true)
    {
        _ignoreCase = ignoreCase;
    }

    public override object? Read(ref Utf8JsonReader reader, Type? typeToConvert, JsonSerializerOptions options)
    {
        object? value;

        var jsonElement = GetConverter<JsonElement>(options).Read(ref reader, typeof(object), options);

        switch (jsonElement.ValueKind)
        {
            case JsonValueKind.Array:
                value = FindBestArrayMatch(jsonElement, typeToConvert, options);
                break;

            case JsonValueKind.Object:
                if (!TryFindBestObjectMatch(jsonElement, typeToConvert?.GetGenericArguments() ?? Type.EmptyTypes, options, out value))
                {
                    value = ToObject(jsonElement, typeof(object), options);
                }
                //value = FindBestObjectMatch(jsonElement, typeToConvert?.GetGenericArguments() ?? Type.EmptyTypes, options);
                break;

            default:
                value = GetSimpleValue(jsonElement);
                break;
        }

        if (typeToConvert is null)
        {
            return value;
        }

        if (value is null)
        {
            return Activator.CreateInstance(typeToConvert);
        }

        return Activator.CreateInstance(typeToConvert, value);
    }

    private static object? GetSimpleValue(JsonElement reader)
    {

        switch (reader.ValueKind)
        {
            case JsonValueKind.String:
                if (reader.TryGetDateTime(out var date))
                {
                    return date;
                }

                return reader.GetString();

            case JsonValueKind.Number:
                if (reader.TryGetInt32(out var i))
                {
                    return i;
                }

                if (reader.TryGetInt64(out var l))
                {
                    return l;
                }

                if (reader.TryGetUInt32(out var ui))
                {
                    return ui;
                }

                if (reader.TryGetUInt64(out var ul))
                {
                    return ul;
                }

                return reader.GetDecimal();

            case JsonValueKind.True:
                return true;

            case JsonValueKind.False:
                return false;

            case JsonValueKind.Null:
                return null;

            default:
                throw new JsonException($"The ValueKind '{reader.ValueKind}' is not supported.");
        }
    }

    private object? FindBestArrayMatch(JsonElement jsonElement, Type? typeToConvert, JsonSerializerOptions options)
    {
        var enumerableTypes = typeToConvert?.GetGenericArguments().Where(t => typeof(IEnumerable).IsAssignableFrom(t)).ToArray() ?? Type.EmptyTypes;
        var types = enumerableTypes.Select(t => t.GetElementTypeX()).ToArray();

        var list = new List<object?>();

        Type? elementType = null;
        foreach (var arrayElement in jsonElement.EnumerateArray())
        {
            object? value;
            if (arrayElement.ValueKind == JsonValueKind.Object)
            {
                if (!TryFindBestObjectMatch(arrayElement, types, options, out value))
                {
                    value = ToObject(arrayElement, typeof(object), options);
                }
                //value = FindBestObjectMatch(arrayElement, types, options);
            }
            else
            {
                value = GetSimpleValue(arrayElement);
            }

            if (elementType is null)
            {
                elementType = value?.GetType();
            }

            list.Add(value);
        }

        if (elementType is null)
        {
            return null;
        }

        var typedListDetails = list.CastToTypedList(elementType);

        foreach (var knownIEnumerableType in enumerableTypes)
        {
            if (knownIEnumerableType.GetElementTypeX() == elementType)
            {
                TinyMapper.Bind(typedListDetails.ListType, knownIEnumerableType);
                return TinyMapper.Map(typedListDetails.ListType, knownIEnumerableType, typedListDetails.List);
            }
        }

        return null;
    }

    private bool TryFindBestObjectMatch(JsonElement objectElement, Type[] types, JsonSerializerOptions options, [NotNullWhen(true)] out object? value)
    {
        var properties = new List<PropertyDetails>();
        foreach (var element in objectElement.EnumerateObject())
        {
            var propertyDetails = new PropertyDetails
            {
                CanRead = true,
                CanWrite = true,
                IsPublic = true,
                Name = element.Name
            };

            object? val;
            switch (element.Value.ValueKind)
            {
                case JsonValueKind.Object:
                    if (!TryFindBestObjectMatch(element.Value, types, options, out val))
                    {
                        val = ToObject(element.Value, typeof(object), options);
                    }
                    
                    break;

                default:
                    val = GetSimpleValue(element.Value);
                    break;
            }

            propertyDetails.PropertyType = val?.GetType();
            propertyDetails.IsValueType = val?.GetType().IsValueType == true;

            properties.Add(propertyDetails);
        }

        var mostSuitableType = MatchFinder.FindBestType(_ignoreCase, properties, types);
        if (mostSuitableType is not null)
        {
            value = ToObject(objectElement, mostSuitableType, options);
            return true;
        }

        value = default;
        return false; //throw new JsonException("No suitable type found.");
    }

    public override void Write(Utf8JsonWriter writer, object? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        var currentValue = value.GetNullablePropertyValue("CurrentValue");
        if (currentValue is null)
        {
            writer.WriteNullValue();
            return;
        }

        var currentType = value.GetPropertyValue<Type>("CurrentValueType");
        JsonSerializer.Serialize(writer, currentValue, currentType, options);
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType.FullName?.StartsWith("AnyOfTypes.AnyOf`") == true;
    }

    /// <summary>
    /// - https://stackoverflow.com/questions/58138793/system-text-json-jsonelement-toobject-workaround
    /// - https://stackoverflow.com/a/58193164/255966
    /// </summary>
    private static object ToObject(JsonElement element, Type returnType, JsonSerializerOptions? options = null)
    {
        var json = element.GetRawText();
        return JsonSerializer.Deserialize(json, returnType, options);
    }

    /// <summary>
    /// https://github.com/dahomey-technologies/Dahomey.Json/blob/master/src/Dahomey.Json/Util/Utf8JsonReaderExtensions.cs
    /// </summary>
    private static JsonConverter<T> GetConverter<T>(JsonSerializerOptions options)
    {
        return (JsonConverter<T>)options.GetConverter(typeof(T));
    }
}