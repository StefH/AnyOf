//using System.Diagnostics;
//using System;

//namespace AnyOfTypes
//{
//    public enum AnyOfType
//    {
//        Undefined = 0, First, Second, Third
//    }
//}



//namespace AnyOfTypes
//{
//    [DebuggerDisplay("{ToString()}")]
//    public struct AnyOf<TFirst, TSecond>
//    {
//        private readonly Type _currentValueType;
//        private readonly AnyOfType _currentType;
//        private readonly TFirst _first;
//        private readonly TSecond _second;
//        public bool IsUndefined => _currentType == AnyOfType.Undefined;
//        public bool IsFirst => _currentType == AnyOfType.First;
//        public bool IsSecond => _currentType == AnyOfType.Second;
//        public static implicit operator AnyOf<TFirst, TSecond>(TFirst value) => new AnyOf<TFirst, TSecond>(value);
//        public static implicit operator TFirst(AnyOf<TFirst, TSecond> @this) => @this.First;
//        public AnyOf(TFirst value)
//        {
//            _currentType = AnyOfType.First;
//            _currentValueType = typeof(TFirst);
//            _first = value;
//            _second = default;
//        }
//        public TFirst First
//        {
//            get
//            {
//                Validate(AnyOfType.First);
//                return _first;
//            }
//        }
//        public static implicit operator AnyOf<TFirst, TSecond>(TSecond value) => new AnyOf<TFirst, TSecond>(value);
//        public static implicit operator TSecond(AnyOf<TFirst, TSecond> @this) => @this.Second;
//        public AnyOf(TSecond value)
//        {
//            _currentType = AnyOfType.Second;
//            _currentValueType = typeof(TSecond);
//            _second = value;
//            _first = default;
//        }
//        public TSecond Second
//        {
//            get
//            {
//                Validate(AnyOfType.Second);
//                return _second;
//            }
//        }
//        private void Validate(AnyOfType desiredType)
//        {
//            if (desiredType != _currentType)
//            {
//                throw new InvalidOperationException($"Attempting to get {desiredType} when {_currentType} is set");
//            }
//        }
//        public AnyOfType CurrentType
//        {
//            get
//            {
//                return _currentType;
//            }
//        }
//        public Type CurrentValueType
//        {
//            get
//            {
//                return _currentValueType;
//            }
//        }
//        public override string ToString()
//        {
//            string description = IsUndefined ? string.Empty : $" : {_currentValueType.Name}";
//            return $"{_currentType}{description}";
//        }
//    }
//}