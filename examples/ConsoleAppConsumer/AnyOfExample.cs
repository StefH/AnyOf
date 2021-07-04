using System;
using System.Diagnostics;

namespace AnyOfExampleGenerator
{
    public enum AnyOfTypeExample
    {
        Undefined = 0,
        T1,
        T2,
    }

    [DebuggerDisplay("{currentType}: {ToString()}")]
    public struct AnyOfExample<T1, T2>
    {
        private readonly Type _currentValueType;
        private readonly AnyOfTypeExample _currentType;
        private readonly T1 _t1;
        private readonly T2 _t2;

        public bool IsUndefined => _currentType == AnyOfTypeExample.Undefined;
        public bool IsT1 => _currentType == AnyOfTypeExample.T1;
        public bool IST2 => _currentType == AnyOfTypeExample.T2;

        public static implicit operator AnyOfExample<T1, T2>(T1 val) => new AnyOfExample<T1, T2>(val);

        public static implicit operator AnyOfExample<T1, T2>(T2 val) => new AnyOfExample<T1, T2>(val);

        public static implicit operator T1(AnyOfExample<T1, T2> @this) => @this.T1Property;

        public static implicit operator T2(AnyOfExample<T1, T2> @this) => @this.T2Property;

        public override string ToString()
        {
            string description = IsUndefined ? string.Empty : $": {(IsT1 ? typeof(T1).Name : typeof(T2).Name)}";
            return $"{_currentType}{description}";
        }

        public AnyOfExample(T1 value)
        {
            _currentType = AnyOfTypeExample.T1;
            _currentValueType = typeof(T1);
            _t1 = value;
            _t2 = default;
        }

        public AnyOfExample(T2 val)
        {
            _currentType = AnyOfTypeExample.T2;
            _currentValueType = typeof(T2);
            _t2 = val;
            _t1 = default;
        }

        public T1 T1Property
        {
            get
            {
                Validate(AnyOfTypeExample.T1);
                return _t1;
            }
        }

        public T2 T2Property
        {
            get
            {
                Validate(AnyOfTypeExample.T2);
                return _t2;
            }
        }

        public AnyOfTypeExample CurrentType
        {
            get
            {
                return _currentType;
            }
        }

        private void Validate(AnyOfTypeExample desiredType)
        {
            if (desiredType != _currentType)
            {
                throw new InvalidOperationException($"Attempting to get {desiredType} when {_currentType} is set");
            }
        }
    }
}
