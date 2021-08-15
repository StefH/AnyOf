using System;
using AnyOfTypes;

namespace ClassLibrary
{
    public class Class1
    {
        public AnyOf<int?, bool> X(AnyOf<string?, int> value)
        {
            if (value.IsFirst)
            {
                return value.First == "x";
            }

            return value.Second;
        }
    }
}
