using AnyOfTypes;

namespace ClassLibrary1
{
    public class Class1
    {
        public AnyOf<int?, bool> X1(AnyOf<string?, int> value)
        {
            if (value.IsFirst)
            {
                return value.First == "x";
            }

            return value.Second;
        }
    }
}