using System;
using AnyOfExampleGenerator;
using AnyOfTypes;
using Union;

namespace ConsoleAppConsumer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            E(42);
            E("test");
            Console.WriteLine(new string('-', 50));

            Y(42);
            Y("test");
            Console.WriteLine(new string('-', 50));

            X(42);
            X("test");
            Console.WriteLine(new string('-', 50));

            X3(42);
            X3("test");
            X3(DateTime.Now);
            Console.WriteLine(new string('-', 50));
        }

        private static void E(Either<int, string> value)
        {
            switch (value.CurrentType)
            {
                case CurrType.Primary:
                    Console.WriteLine("Primary with value " + value.Primary);
                    break;

                case CurrType.Alternate:
                    Console.WriteLine("Alternate with value " + value.Alternate);
                    break;

                default:
                    Console.WriteLine("????");
                    break;
            }
        }

        private static void Y(AnyOfExample<int, string> value)
        {
            switch (value.CurrentType)
            {
                case AnyOfTypeExample.T1:
                    Console.WriteLine("AnyOfType = T1 with value " + value.T1Property);
                    break;

                case AnyOfTypeExample.T2:
                    Console.WriteLine("AnyOfType = T2 with value " + value.T2Property);
                    break;

                default:
                    Console.WriteLine("????");
                    break;
            }
        }

        private static void X(AnyOf<int, string> value)
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
                    break;

                case AnyOfType.Second:
                    Console.WriteLine("AnyOfType = Second with value " + value.Second);
                    break;

                default:
                    Console.WriteLine("????");
                    break;
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