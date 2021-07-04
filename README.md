# AnyOf
Use the `AnyOf<First, TSecond, ...>` type to handle multiple defined types as input parameters for methods.

This project uses code generation to generate multiple AnyOf-types:

- `AnyOf<TFirst, TSecond>`
- `AnyOf<TFirst, TSecond, TThird>`
- `AnyOf<TFirst, TSecond, TThird, TFourth>`
- `AnyOf<TFirst, TSecond, TThird, TFourth, TFifth>`

# Install
[![NuGet Badge](https://buildstats.info/nuget/AnyOf)](https://www.nuget.org/packages/AnyOf)

You can install from NuGet using the following command in the package manager window:

`Install-Package AnyOf`

Or via the Visual Studio NuGet package manager or if you use the `dotnet` command:

`dotnet add package AnyOf`

# Usage
``` c#
using System;
using AnyOfTypes;

namespace ConsoleAppConsumer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            X(42);
            X("test");
            Console.WriteLine(new string('-', 50));
        }

        private static void X(AnyOf<int, string> value)
        {
            Console.WriteLine("ToString " + value.ToString());
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
                    Console.WriteLine("???");
                    break;
            }
        }
    }
}
```