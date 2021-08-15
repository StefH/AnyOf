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
    [DebuggerDisplay("AnyOfType = {_currentType}; Type = {_currentValueType?.Name}; Value = '{ToString()}'")]
    public struct AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>
    {
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

        public bool IsUndefined => _currentType == AnyOfType.Undefined;
        public bool IsFirst => _currentType == AnyOfType.First;
        public bool IsSecond => _currentType == AnyOfType.Second;
        public bool IsThird => _currentType == AnyOfType.Third;
        public bool IsFourth => _currentType == AnyOfType.Fourth;
        public bool IsFifth => _currentType == AnyOfType.Fifth;
        public bool IsSixth => _currentType == AnyOfType.Sixth;
        public bool IsSeventh => _currentType == AnyOfType.Seventh;
        public bool IsEighth => _currentType == AnyOfType.Eighth;

        public static implicit operator AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(TFirst value) => new AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);

        public static implicit operator TFirst(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> @this) => @this.First;

        public AnyOf(TFirst value)
        {
            _numberOfTypes = 8;
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
        }

        public TFirst First
        {
            get
            {
               Validate(AnyOfType.First);
               return _first;
            }
        }

        public static implicit operator AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(TSecond value) => new AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);

        public static implicit operator TSecond(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> @this) => @this.Second;

        public AnyOf(TSecond value)
        {
            _numberOfTypes = 8;
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
        }

        public TSecond Second
        {
            get
            {
               Validate(AnyOfType.Second);
               return _second;
            }
        }

        public static implicit operator AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(TThird value) => new AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);

        public static implicit operator TThird(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> @this) => @this.Third;

        public AnyOf(TThird value)
        {
            _numberOfTypes = 8;
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
        }

        public TThird Third
        {
            get
            {
               Validate(AnyOfType.Third);
               return _third;
            }
        }

        public static implicit operator AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(TFourth value) => new AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);

        public static implicit operator TFourth(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> @this) => @this.Fourth;

        public AnyOf(TFourth value)
        {
            _numberOfTypes = 8;
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
        }

        public TFourth Fourth
        {
            get
            {
               Validate(AnyOfType.Fourth);
               return _fourth;
            }
        }

        public static implicit operator AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(TFifth value) => new AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);

        public static implicit operator TFifth(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> @this) => @this.Fifth;

        public AnyOf(TFifth value)
        {
            _numberOfTypes = 8;
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
        }

        public TFifth Fifth
        {
            get
            {
               Validate(AnyOfType.Fifth);
               return _fifth;
            }
        }

        public static implicit operator AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(TSixth value) => new AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);

        public static implicit operator TSixth(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> @this) => @this.Sixth;

        public AnyOf(TSixth value)
        {
            _numberOfTypes = 8;
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
        }

        public TSixth Sixth
        {
            get
            {
               Validate(AnyOfType.Sixth);
               return _sixth;
            }
        }

        public static implicit operator AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(TSeventh value) => new AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);

        public static implicit operator TSeventh(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> @this) => @this.Seventh;

        public AnyOf(TSeventh value)
        {
            _numberOfTypes = 8;
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
        }

        public TSeventh Seventh
        {
            get
            {
               Validate(AnyOfType.Seventh);
               return _seventh;
            }
        }

        public static implicit operator AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(TEighth value) => new AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);

        public static implicit operator TEighth(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> @this) => @this.Eighth;

        public AnyOf(TEighth value)
        {
            _numberOfTypes = 8;
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
        }

        public TEighth Eighth
        {
            get
            {
               Validate(AnyOfType.Eighth);
               return _eighth;
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
            var hash = new HashCode();
            hash.Add(_currentValue);
            hash.Add(_currentType);
                        hash.Add(_first);
                        hash.Add(_second);
                        hash.Add(_third);
                        hash.Add(_fourth);
                        hash.Add(_fifth);
                        hash.Add(_sixth);
                        hash.Add(_seventh);
                        hash.Add(_eighth);
                        hash.Add(typeof(TFirst));
                        hash.Add(typeof(TSecond));
                        hash.Add(typeof(TThird));
                        hash.Add(typeof(TFourth));
                        hash.Add(typeof(TFifth));
                        hash.Add(typeof(TSixth));
                        hash.Add(typeof(TSeventh));
                        hash.Add(typeof(TEighth));
            return hash.ToHashCode();
        }

        private bool Equals(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> other)
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
            EqualityComparer<TEighth>.Default.Equals(_eighth, other._eighth);
        }

        public static bool operator ==(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> obj1, AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> obj1, AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> obj2)
        {
            return !obj1.Equals(obj2);
        }

        public override bool Equals(object obj)
        {
            return obj is AnyOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> o && Equals(o);
        }

        public override string ToString()
        {
            return IsUndefined ? null : $"{_currentValue}";
        }
    }
}
