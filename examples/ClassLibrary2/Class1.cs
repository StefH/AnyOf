using AnyOfTypes;

namespace ClassLibrary2
{
    public class Class2
    {
        public AnyOf<string?, int> X2(AnyOf<int?, bool> value)
        {
            if (value.IsFirst)
            {
                return $"str = {value.First}";
            }

            return value.Second ? 1 : 0;
        }
    }
}