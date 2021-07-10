using System;
using AnyOfTypes;

namespace ConsoleAppConsumer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(ReturnSomething().CurrentValue);
            Console.WriteLine(new string('-', 50));

            var xInt1 = X(42);
            var hc = xInt1.GetHashCode();

            var xInt2 = X(42);
            var xInt3 = X(5);

            if (xInt1 == xInt2)
            {
                Console.WriteLine("---> xInt1 == xInt2");
            }
            if (xInt1 != xInt3)
            {
                Console.WriteLine("---> xInt1 != xInt3");
            }
            if (xInt1.Equals(xInt2))
            {
                Console.WriteLine("---> xInt1 Equals xInt2");
            }
            if (!xInt1.Equals(xInt3))
            {
                Console.WriteLine("---> xInt1 !Equals xInt3");
            }

            X("test");
            Console.WriteLine(new string('-', 50));

            X3(42);
            X3("test");
            X3(DateTime.Now);
            Console.WriteLine(new string('-', 50));
        }

        private static AnyOf<string, int, bool> ReturnSomething()
        {
            return new Random().Next(3) switch
            {
                1 => "test",
                2 => 42,
                _ => true,
            };
        }

        private static AnyOf<int, string> X(AnyOf<int, string> value)
        {
            Console.WriteLine("ToString " + value.ToString());
            Console.WriteLine("CurrentValue " + value.CurrentValue);
            Console.WriteLine("IsUndefined " + value.IsUndefined);
            Console.WriteLine("IsFirst " + value.IsFirst);
            Console.WriteLine("IsSecond " + value.IsSecond);

            switch (value.CurrentType)
            {
                case AnyOfType.First:
                    Console.WriteLine("AnyOfType = First with value " + value.First);
                    return value;

                case AnyOfType.Second:
                    Console.WriteLine("AnyOfType = Second with value " + value.Second);
                    return value;

                default:
                    throw new Exception("???");
            }
        }

        private static void X3(AnyOf<int, string, DateTime> value)
        {
            Console.WriteLine("ToString " + value.ToString());
            Console.WriteLine("CurrentValue " + value.CurrentValue);
            Console.WriteLine("IsUndefined " + value.IsUndefined);
            Console.WriteLine("IsFirst " + value.IsFirst);
            Console.WriteLine("IsSecond " + value.IsSecond);
            Console.WriteLine("IsThird " + value.IsThird);

            switch (value.CurrentType)
            {
                case AnyOfType.First:
                    Console.WriteLine("AnyOfType = First with value " + value.First);
                    break;

                case AnyOfType.Second:
                    Console.WriteLine("AnyOfType = Second with value " + value.Second);
                    break;

                case AnyOfType.Third:
                    Console.WriteLine("AnyOfType = Third with value " + value.Third);
                    break;

                default:
                    Console.WriteLine("????");
                    break;
            }
        }
    }
}