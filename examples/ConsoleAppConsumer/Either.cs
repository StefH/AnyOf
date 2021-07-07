using System;
using System.Collections.Generic;
using System.Diagnostics;
using AnyOfExampleGenerator;

namespace Union
{
    public enum CurrType
    {
        Neither = 0,
        Primary,
        Alternate,
    }

    [DebuggerDisplay("{ToString()}")]
    public struct Either<TP, TA>
    {
        private readonly CurrType currType;
        private readonly TP primary;
        private readonly TA alternate;

        public bool IsNeither => currType == CurrType.Neither;
        public bool IsPrimary => currType == CurrType.Primary;
        public bool IsAlternate => currType == CurrType.Alternate;

        public static implicit operator Either<TP, TA>(TP val) => new Either<TP, TA>(val);

        public static implicit operator Either<TP, TA>(TA val) => new Either<TP, TA>(val);

        public static implicit operator TP(Either<TP, TA> @this) => @this.Primary;

        public static implicit operator TA(Either<TP, TA> @this) => @this.Alternate;

        public override string ToString()
        {
            string description = IsNeither ? "" : $": {(IsPrimary ? typeof(TP).Name : typeof(TA).Name)}";
            return $"{currType}{description}";
        }

        public Either(TP val)
        {
            currType = CurrType.Primary;
            primary = val;
            alternate = default;
        }

        public Either(TA val)
        {
            currType = CurrType.Alternate;
            alternate = val;
            primary = default;
        }

        public TP Primary
        {
            get
            {
                Validate(CurrType.Primary);
                return primary;
            }
        }

        public TA Alternate
        {
            get
            {
                Validate(CurrType.Alternate);
                return alternate;
            }
        }

        public CurrType CurrentType
        {
            get
            {
                return currType;
            }
        }

        private void Validate(CurrType desiredType)
        {
            if (desiredType != currType)
            {
                throw new InvalidOperationException($"Attempting to get {desiredType} when {currType} is set");
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Either<TP, TA> either &&
                   currType == either.currType &&
                   EqualityComparer<TP>.Default.Equals(primary, either.primary) &&
                   EqualityComparer<TA>.Default.Equals(alternate, either.alternate);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(currType, primary, alternate);
        }
    }
}