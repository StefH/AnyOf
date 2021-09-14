using System;
using AnyOfTypes;

namespace ConsoleAppConsumerNET472
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(ReturnSomething().CurrentValue);
            Console.WriteLine(ReturnSomething().CurrentValue);
            Console.WriteLine(ReturnSomething().CurrentValue);

            //var c = new ClassLibrary.Class1();
            //Console.WriteLine("call X with \"x\" = " + c.X("x"));
            //Console.WriteLine("call X with 123 = " + c.X(123));
        }

        private static AnyOf<string, int, bool> ReturnSomething()
        {
            switch (new Random().Next(3))
            {
                case 1:
                    return "test";

                case 2:
                    return 42;

                default:
                    return true;
            }
        }
    }
}