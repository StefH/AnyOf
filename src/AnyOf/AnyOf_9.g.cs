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
    [DebuggerDisplay("{_thisType}, AnyOfType = {_currentType}; Type = {_currentValueType?.Name}; Value = '{ToString()}'")]
    public struct AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth> : IEquatable<AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>>
    {
        private readonly string _thisType => $"AnyOf<{typeof(TFirst).Name}, {typeof(TSecond).Name}, {typeof(TThird).Name}, {typeof(TFourth).Name}, {typeof(TFifth).Name}, {typeof(TSixth).Name}, {typeof(TSeventh).Name}, {typeof(TEighth).Name}, {typeof(TNinth).Name}>";
        private readonly int _numberOfTypes;
        private readonly object _currentValue;
        private readonly Type _currentValueType;
        private readonly AnyOfType _currentType;

        private readonly TFirst _first;
        private readonly TSecond _second;
        private readonly TThird _third;
        private readonly TFourth _fourth;
        private readonly TFifth _fifth;
        private readonly TSixth _sixth;
        private readonly TSeventh _seventh;
        private readonly TEighth _eighth;
        private readonly TNinth _ninth;

        public readonly AnyOfType[] AnyOfTypes => new[] { AnyOfType.First, AnyOfType.Second, AnyOfType.Third, AnyOfType.Fourth, AnyOfType.Fifth, AnyOfType.Sixth, AnyOfType.Seventh, AnyOfType.Eighth, AnyOfType.Ninth };
        public readonly Type[] Types => new[] { typeof(TFirst), typeof(TSecond), typeof(TThird), typeof(TFourth), typeof(TFifth), typeof(TSixth), typeof(TSeventh), typeof(TEighth), typeof(TNinth) };
        public bool IsUndefined => _currentType == AnyOfType.Undefined;
        public bool IsFirst => _currentType == AnyOfType.First;
        public bool IsSecond => _currentType == AnyOfType.Second;
        public bool IsThird => _currentType == AnyOfType.Third;
        public bool IsFourth => _currentType == AnyOfType.Fourth;
        public bool IsFifth => _currentType == AnyOfType.Fifth;
        public bool IsSixth => _currentType == AnyOfType.Sixth;
        public bool IsSeventh => _currentType == AnyOfType.Seventh;
        public bool IsEighth => _currentType == AnyOfType.Eighth;
        public bool IsNinth => _currentType == AnyOfType.Ninth;

        public static implicit operator AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(TFirst value) => new AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(value);

        public static implicit operator TFirst(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth> @this) => @this.First;

        public AnyOf(TFirst value)
        {
            _numberOfTypes = 9;
            _currentType = AnyOfType.First;
            _currentValue = value;
            _currentValueType = typeof(TFirst);
            _first = value;
            _second = default;
            _third = default;
            _fourth = default;
            _fifth = default;
            _sixth = default;
            _seventh = default;
            _eighth = default;
            _ninth = default;
        }

        public TFirst First
        {
            get
            {
               Validate(AnyOfType.First);
               return _first;
            }
        }

        public static implicit operator AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(TSecond value) => new AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(value);

        public static implicit operator TSecond(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth> @this) => @this.Second;

        public AnyOf(TSecond value)
        {
            _numberOfTypes = 9;
            _currentType = AnyOfType.Second;
            _currentValue = value;
            _currentValueType = typeof(TSecond);
            _second = value;
            _first = default;
            _third = default;
            _fourth = default;
            _fifth = default;
            _sixth = default;
            _seventh = default;
            _eighth = default;
            _ninth = default;
        }

        public TSecond Second
        {
            get
            {
               Validate(AnyOfType.Second);
               return _second;
            }
        }

        public static implicit operator AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(TThird value) => new AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(value);

        public static implicit operator TThird(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth> @this) => @this.Third;

        public AnyOf(TThird value)
        {
            _numberOfTypes = 9;
            _currentType = AnyOfType.Third;
            _currentValue = value;
            _currentValueType = typeof(TThird);
            _third = value;
            _first = default;
            _second = default;
            _fourth = default;
            _fifth = default;
            _sixth = default;
            _seventh = default;
            _eighth = default;
            _ninth = default;
        }

        public TThird Third
        {
            get
            {
               Validate(AnyOfType.Third);
               return _third;
            }
        }

        public static implicit operator AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(TFourth value) => new AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(value);

        public static implicit operator TFourth(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth> @this) => @this.Fourth;

        public AnyOf(TFourth value)
        {
            _numberOfTypes = 9;
            _currentType = AnyOfType.Fourth;
            _currentValue = value;
            _currentValueType = typeof(TFourth);
            _fourth = value;
            _first = default;
            _second = default;
            _third = default;
            _fifth = default;
            _sixth = default;
            _seventh = default;
            _eighth = default;
            _ninth = default;
        }

        public TFourth Fourth
        {
            get
            {
               Validate(AnyOfType.Fourth);
               return _fourth;
            }
        }

        public static implicit operator AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(TFifth value) => new AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(value);

        public static implicit operator TFifth(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth> @this) => @this.Fifth;

        public AnyOf(TFifth value)
        {
            _numberOfTypes = 9;
            _currentType = AnyOfType.Fifth;
            _currentValue = value;
            _currentValueType = typeof(TFifth);
            _fifth = value;
            _first = default;
            _second = default;
            _third = default;
            _fourth = default;
            _sixth = default;
            _seventh = default;
            _eighth = default;
            _ninth = default;
        }

        public TFifth Fifth
        {
            get
            {
               Validate(AnyOfType.Fifth);
               return _fifth;
            }
        }

        public static implicit operator AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(TSixth value) => new AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(value);

        public static implicit operator TSixth(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth> @this) => @this.Sixth;

        public AnyOf(TSixth value)
        {
            _numberOfTypes = 9;
            _currentType = AnyOfType.Sixth;
            _currentValue = value;
            _currentValueType = typeof(TSixth);
            _sixth = value;
            _first = default;
            _second = default;
            _third = default;
            _fourth = default;
            _fifth = default;
            _seventh = default;
            _eighth = default;
            _ninth = default;
        }

        public TSixth Sixth
        {
            get
            {
               Validate(AnyOfType.Sixth);
               return _sixth;
            }
        }

        public static implicit operator AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(TSeventh value) => new AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(value);

        public static implicit operator TSeventh(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth> @this) => @this.Seventh;

        public AnyOf(TSeventh value)
        {
            _numberOfTypes = 9;
            _currentType = AnyOfType.Seventh;
            _currentValue = value;
            _currentValueType = typeof(TSeventh);
            _seventh = value;
            _first = default;
            _second = default;
            _third = default;
            _fourth = default;
            _fifth = default;
            _sixth = default;
            _eighth = default;
            _ninth = default;
        }

        public TSeventh Seventh
        {
            get
            {
               Validate(AnyOfType.Seventh);
               return _seventh;
            }
        }

        public static implicit operator AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(TEighth value) => new AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(value);

        public static implicit operator TEighth(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth> @this) => @this.Eighth;

        public AnyOf(TEighth value)
        {
            _numberOfTypes = 9;
            _currentType = AnyOfType.Eighth;
            _currentValue = value;
            _currentValueType = typeof(TEighth);
            _eighth = value;
            _first = default;
            _second = default;
            _third = default;
            _fourth = default;
            _fifth = default;
            _sixth = default;
            _seventh = default;
            _ninth = default;
        }

        public TEighth Eighth
        {
            get
            {
               Validate(AnyOfType.Eighth);
               return _eighth;
            }
        }

        public static implicit operator AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(TNinth value) => new AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(value);

        public static implicit operator TNinth(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth> @this) => @this.Ninth;

        public AnyOf(TNinth value)
        {
            _numberOfTypes = 9;
            _currentType = AnyOfType.Ninth;
            _currentValue = value;
            _currentValueType = typeof(TNinth);
            _ninth = value;
            _first = default;
            _second = default;
            _third = default;
            _fourth = default;
            _fifth = default;
            _sixth = default;
            _seventh = default;
            _eighth = default;
        }

        public TNinth Ninth
        {
            get
            {
               Validate(AnyOfType.Ninth);
               return _ninth;
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
                _fifth,
                _sixth,
                _seventh,
                _eighth,
                _ninth,
                typeof(TFirst),
                typeof(TSecond),
                typeof(TThird),
                typeof(TFourth),
                typeof(TFifth),
                typeof(TSixth),
                typeof(TSeventh),
                typeof(TEighth),
                typeof(TNinth),
            };
            return HashCodeCalculator.GetHashCode(fields);
        }

        public bool Equals(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth> other)
        {
            return _currentType == other._currentType &&
                   _numberOfTypes == other._numberOfTypes &&
                   EqualityComparer<object>.Default.Equals(_currentValue, other._currentValue) &&
                    EqualityComparer<TFirst>.Default.Equals(_first, other._first) &&
                    EqualityComparer<TSecond>.Default.Equals(_second, other._second) &&
                    EqualityComparer<TThird>.Default.Equals(_third, other._third) &&
                    EqualityComparer<TFourth>.Default.Equals(_fourth, other._fourth) &&
                    EqualityComparer<TFifth>.Default.Equals(_fifth, other._fifth) &&
                    EqualityComparer<TSixth>.Default.Equals(_sixth, other._sixth) &&
                    EqualityComparer<TSeventh>.Default.Equals(_seventh, other._seventh) &&
                    EqualityComparer<TEighth>.Default.Equals(_eighth, other._eighth) &&
                    EqualityComparer<TNinth>.Default.Equals(_ninth, other._ninth);
        }

        public static bool operator ==(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth> obj1, AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth> obj2)
        {
            return EqualityComparer<AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>>.Default.Equals(obj1, obj2);
        }

        public static bool operator !=(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth> obj1, AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth> obj2)
        {
            return !(obj1 == obj2);
        }

        public override bool Equals(object obj)
        {
            return obj is AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth> o && Equals(o);
        }

        public override string ToString()
        {
            return IsUndefined ? null : $"{_currentValue}";
        }
    }
}
