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