﻿using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace AnyOfTypes2
{
    public enum AnyOfType
    {
        Undefined = 0, First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth, Ninth, Tenth
    }
}

#nullable enable
namespace AnyOfTypes2
{
    [DebuggerDisplay("AnyOfType = {_currentType}; Type = {_currentValueType?.Name}; Value = '{ToString()}'")]
    public struct AnyOf<TFirst, TSecond>
    {
        private readonly int _numberOfTypes;
        private readonly object? _currentValue; // object -> object?
        private readonly Type _currentValueType;
        private readonly AnyOfType _currentType;

        private readonly TFirst _first;
        private readonly TSecond _second;

        public bool IsUndefined => _currentType == AnyOfType.Undefined;
        public bool IsFirst => _currentType == AnyOfType.First;
        public bool IsSecond => _currentType == AnyOfType.Second;

        public static implicit operator AnyOf<TFirst, TSecond>(TFirst value) => new AnyOf<TFirst, TSecond>(value);

        public static implicit operator TFirst(AnyOf<TFirst, TSecond> @this) => @this.First; // !

        public AnyOf(TFirst value)
        {
            _numberOfTypes = 2;
            _currentType = AnyOfType.First;
            _currentValue = value;
            _currentValueType = typeof(TFirst);
            _first = value;
            _second = default!; // !
        }

        public TFirst First // ?
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
            _first = default!; // !
        }

        public TSecond Second // 
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

        public object? CurrentValue // object -> object?
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
            hash.Add(typeof(TFirst));
            hash.Add(typeof(TSecond));
            return hash.ToHashCode();
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

        public override bool Equals(object? obj) // object -> object?
        {
            return obj is AnyOf<TFirst, TSecond> o && Equals(o);
        }

        public override string? ToString() // string -> string?
        {
            return IsUndefined ? null : $"{_currentValue}";
        }
    }
}
#nullable disable