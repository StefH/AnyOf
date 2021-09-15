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

namespace AnyOfTypes
{
    [DebuggerDisplay("{thisType}, AnyOfType = {_currentType}; Type = {_currentValueType?.Name}; Value = '{ToString()}'")]
    public struct AnyOf<TFirst, TSecond, TThird, TFourth>
    {
        private readonly string thisType => $"AnyOf<{typeof(TFirst).Name}, {typeof(TSecond).Name}, {typeof(TThird).Name}, {typeof(TFourth).Name}>";
        private readonly int _numberOfTypes;
        private readonly object _currentValue;
        private readonly Type _currentValueType;
        private readonly AnyOfType _currentType;

        private readonly TFirst _first;
        private readonly TSecond _second;
        private readonly TThird _third;
        private readonly TFourth _fourth;

        public readonly AnyOfType[] AnyOfTypes => new [] { AnyOfType.First, AnyOfType.Second, AnyOfType.Third, AnyOfType.Fourth };
        public readonly Type[] Types => new [] { typeof(TFirst), typeof(TSecond), typeof(TThird), typeof(TFourth) };
        public bool IsUndefined => _currentType == AnyOfType.Undefined;
        public bool IsFirst => _currentType == AnyOfType.First;
        public bool IsSecond => _currentType == AnyOfType.Second;
        public bool IsThird => _currentType == AnyOfType.Third;
        public bool IsFourth => _currentType == AnyOfType.Fourth;

        public static implicit operator AnyOf<TFirst, TSecond, TThird, TFourth>(TFirst value) => new AnyOf<TFirst, TSecond, TThird, TFourth>(value);

        public static implicit operator TFirst(AnyOf<TFirst, TSecond, TThird, TFourth> @this) => @this.First;

        public AnyOf(TFirst value)
        {
            _numberOfTypes = 4;
            _currentType = AnyOfType.First;
            _currentValue = value;
            _currentValueType = typeof(TFirst);
            _first = value;
            _second = default;
            _third = default;
            _fourth = default;
        }

        public TFirst First
        {
            get
            {
               Validate(AnyOfType.First);
               return _first;
            }
        }

        public static implicit operator AnyOf<TFirst, TSecond, TThird, TFourth>(TSecond value) => new AnyOf<TFirst, TSecond, TThird, TFourth>(value);

        public static implicit operator TSecond(AnyOf<TFirst, TSecond, TThird, TFourth> @this) => @this.Second;

        public AnyOf(TSecond value)
        {
            _numberOfTypes = 4;
            _currentType = AnyOfType.Second;
            _currentValue = value;
            _currentValueType = typeof(TSecond);
            _second = value;
            _first = default;
            _third = default;
            _fourth = default;
        }

        public TSecond Second
        {
            get
            {
               Validate(AnyOfType.Second);
               return _second;
            }
        }

        public static implicit operator AnyOf<TFirst, TSecond, TThird, TFourth>(TThird value) => new AnyOf<TFirst, TSecond, TThird, TFourth>(value);

        public static implicit operator TThird(AnyOf<TFirst, TSecond, TThird, TFourth> @this) => @this.Third;

        public AnyOf(TThird value)
        {
            _numberOfTypes = 4;
            _currentType = AnyOfType.Third;
            _currentValue = value;
            _currentValueType = typeof(TThird);
            _third = value;
            _first = default;
            _second = default;
            _fourth = default;
        }

        public TThird Third
        {
            get
            {
               Validate(AnyOfType.Third);
               return _third;
            }
        }

        public static implicit operator AnyOf<TFirst, TSecond, TThird, TFourth>(TFourth value) => new AnyOf<TFirst, TSecond, TThird, TFourth>(value);

        public static implicit operator TFourth(AnyOf<TFirst, TSecond, TThird, TFourth> @this) => @this.Fourth;

        public AnyOf(TFourth value)
        {
            _numberOfTypes = 4;
            _currentType = AnyOfType.Fourth;
            _currentValue = value;
            _currentValueType = typeof(TFourth);
            _fourth = value;
            _first = default;
            _second = default;
            _third = default;
        }

        public TFourth Fourth
        {
            get
            {
               Validate(AnyOfType.Fourth);
               return _fourth;
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
                _third,
                _fourth,
                typeof(TFirst),
                typeof(TSecond),
                typeof(TThird),
                typeof(TFourth),
            };
            return HashCodeCalculator.GetHashCode(fields);
        }

        private bool Equals(AnyOf<TFirst, TSecond, TThird, TFourth> other)
        {
            return _currentType == other._currentType &&
                   _numberOfTypes == other._numberOfTypes &&
                   EqualityComparer<object>.Default.Equals(_currentValue, other._currentValue) &&
            EqualityComparer<TFirst>.Default.Equals(_first, other._first) &&
            EqualityComparer<TSecond>.Default.Equals(_second, other._second) &&
            EqualityComparer<TThird>.Default.Equals(_third, other._third) &&
            EqualityComparer<TFourth>.Default.Equals(_fourth, other._fourth);
        }

        public static bool operator ==(AnyOf<TFirst, TSecond, TThird, TFourth> obj1, AnyOf<TFirst, TSecond, TThird, TFourth> obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(AnyOf<TFirst, TSecond, TThird, TFourth> obj1, AnyOf<TFirst, TSecond, TThird, TFourth> obj2)
        {
            return !obj1.Equals(obj2);
        }

        public override bool Equals(object obj)
        {
            return obj is AnyOf<TFirst, TSecond, TThird, TFourth> o && Equals(o);
        }

        public override string ToString()
        {
            return IsUndefined ? null : $"{_currentValue}";
        }
    }
}
