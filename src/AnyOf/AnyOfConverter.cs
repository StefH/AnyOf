using System;
using System.ComponentModel;
using System.Globalization;

namespace AnyOfTypes;

public class AnyOfConverter<TFirst, TSecond> : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(AnyOf<TFirst, TSecond>) || sourceType == typeof(TFirst) || sourceType == typeof(TSecond) || base.CanConvertFrom(context, sourceType);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType != null && (destinationType == typeof(AnyOf<TFirst, TSecond>) || destinationType == typeof(TFirst) || destinationType == typeof(TSecond) || base.CanConvertTo(context, destinationType));
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
    {
        if (value == null)
        {
            return null;
        }

        if (value is AnyOf<TFirst, TSecond> anyOfValue)
        {
            return anyOfValue;
        }

        if (value is TFirst first)
        {
            return new AnyOf<TFirst, TSecond>(first);
        }

        if (value is TSecond second)
        {
            return new AnyOf<TFirst, TSecond>(second);
        }

        // Fall back to the base implementation if the value cannot be converted.
        return base.ConvertFrom(context, culture, value);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (value is null)
        {
            return null;
        }

        if (destinationType == typeof(AnyOf<TFirst, TSecond>))
        {
            return value;
        }

        if (destinationType == typeof(TFirst))
        {
            return ((AnyOf<TFirst, TSecond>)value).First;
        }

        if (destinationType == typeof(TSecond))
        {
            return ((AnyOf<TFirst, TSecond>)value).Second;
        }

        //if (value is AnyOf<TFirst, TSecond> anyOfValue)
        //{
        //    if (destinationType == typeof(AnyOf<TFirst, TSecond>))
        //    {
        //        return value;
        //    }

        //    if (destinationType == typeof(TFirst))
        //    {
        //        return anyOfValue.First;
        //    }

        //    if (destinationType == typeof(TSecond))
        //    {
        //        return anyOfValue.Second;
        //    }
        //}

        // Fall back to the base implementation if the value cannot be converted.
        return base.ConvertTo(context, culture, value, destinationType);
    }
}