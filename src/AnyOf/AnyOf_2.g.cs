//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/StefH/AnyOf.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace AnyOfTypes
{
    [DebuggerDisplay("{_thisType}, AnyOfType = {_currentType}; Type = {_currentValueType?.Name}; Value = '{ToString()}'")]
    public struct AnyOf<TFirst, TSecond>
    {
        private readonly string _thisType => $"AnyOf<{typeof(TFirst).Name}, {typeof(TSecond).Name}>";
        private readonly int _numberOfTypes;
        private readonly object _currentValue;
        private readonly Type _currentValueType;
        private readonly AnyOfType _currentType;

        private readonly TFirst _first;
        private readonly TSecond _second;

        public readonly AnyOfType[] AnyOfTypes => new[] { AnyOfType.First, AnyOfType.Second };
        public readonly Type[] Types => new[] { typeof(TFirst), typeof(TSecond) };
        public bool IsUndefined => _currentType == AnyOfType.Undefined;
        public bool IsFirst => _currentType == AnyOfType.First;
        public bool IsSecond => _currentType == AnyOfType.Second;

        public static implicit operator AnyOf<TFirst, TSecond>(TFirst value) => new AnyOf<TFirst, TSecond>(value);

        public static implicit operator TFirst(AnyOf<TFirst, TSecond> @this) => @this.First;

        public AnyOf(TFirst value)
        {
            _numberOfTypes = 2;
            _currentType = AnyOfType.First;
            _currentValue = value;
            _currentValueType = typeof(TFirst);
            _first = value;
            _second = default;
        }

        public TFirst First
        {
            get
            {
                Validate(AnyOfType.First);
                return _first;
            }
        }

        public static implicit operator AnyOf<TFirst, TSecond>(TSecond value) => new AnyOf<TFirst, TSecond>(value);

        public static implicit operator TSecond(AnyOf<TFirst, TSecond> @this) => @this.Second;

        public AnyOf(TSecond value)
        {
            _numberOfTypes = 2;
            _currentType = AnyOfType.Second;
            _currentValue = value;
            _currentValueType = typeof(TSecond);
            _second = value;
            _first = default;
        }

        public TSecond Second
        {
            get
            {
                Validate(AnyOfType.Second);
                return _second;
            }
        }

        private void Validate(AnyOfType desiredType)
        {
            if (desiredType != _currentType)
            {
                throw new InvalidOperationException($"Attempting to get {desiredType} when {_currentType} is set");
            }
        }

        public AnyOfType CurrentType
        {
            get
            {
                return _currentType;
            }
        }

        public object CurrentValue
        {
            get
            {
                return _currentValue;
            }
        }

        public Type CurrentValueType
        {
            get
            {
                return _currentValueType;
            }
        }

        public override int GetHashCode()
        {
            var fields = new object[]
            {
                _numberOfTypes,
                _currentValue,
                _currentType,
                _first,
                _second,
                typeof(TFirst),
                typeof(TSecond),
            };
            return HashCodeCalculator.GetHashCode(fields);
        }

        private bool Equals(AnyOf<TFirst, TSecond> other)
        {
            return _currentType == other._currentType &&
                   _numberOfTypes == other._numberOfTypes &&
                   EqualityComparer<object>.Default.Equals(_currentValue, other._currentValue) &&
            EqualityComparer<TFirst>.Default.Equals(_first, other._first) &&
            EqualityComparer<TSecond>.Default.Equals(_second, other._second);
        }

        public static bool operator ==(AnyOf<TFirst, TSecond> obj1, AnyOf<TFirst, TSecond> obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(AnyOf<TFirst, TSecond> obj1, AnyOf<TFirst, TSecond> obj2)
        {
            return !obj1.Equals(obj2);
        }

        public override bool Equals(object obj)
        {
            return obj is AnyOf<TFirst, TSecond> o && Equals(o);
        }

        public override string ToString()
        {
            return IsUndefined ? null : $"{_currentValue}";
        }
    }

    public class AnyOfConverter<TFirst, TSecond> : TypeConverter
    {
        private readonly Lazy<AnyOf<TFirst, TSecond>> _theType = new Lazy<AnyOf<TFirst, TSecond>>(() => new AnyOf<TFirst, TSecond>());

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return
                sourceType == typeof(AnyOf<TFirst, TSecond>) ||
                _theType.Value.Types.Contains(sourceType) ||
                base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return
                destinationType == typeof(AnyOf<TFirst, TSecond>) ||
                _theType.Value.Types.Contains(destinationType) ||
                base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
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

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is null)
            {
                return null;
            }

            if (value is TFirst first)
            {
                return new AnyOf<TFirst, TSecond>(first);
            }

            if (value is TSecond second)
            {
                return new AnyOf<TFirst, TSecond>(second);
            }

            if (value is AnyOf<TFirst, TSecond> anyOfValue)
            {
                if (destinationType == typeof(AnyOf<TFirst, TSecond>))
                {
                    return value;
                }

                if (destinationType == typeof(TFirst))
                {
                    return anyOfValue.First;
                }

                if (destinationType == typeof(TSecond))
                {
                    return anyOfValue.Second;
                }
            }

            // Fall back to the base implementation if the value cannot be converted.
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}