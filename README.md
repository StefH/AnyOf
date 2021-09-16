# AnyOf
Use the `AnyOf<First, TSecond, ...>` type to handle multiple defined types as input parameters for methods.

This project uses code generation to generate up to 10 AnyOf-types:

- `AnyOf<TFirst, TSecond>`
- `AnyOf<TFirst, TSecond, TThird>`
- `AnyOf<TFirst, TSecond, TThird, TFourth>`
- ...

# Install
## The Source Generator version:
[![NuGet Badge](https://buildstats.info/nuget/AnyOf.SourceGenerator)](https://www.nuget.org/packages/AnyOf.SourceGenerator)

## The normal version:
[![NuGet Badge](https://buildstats.info/nuget/AnyOf)](https://www.nuget.org/packages/AnyOf)

## AnyOf.Newtonsoft.Json
This package can be used to serialize/deserialize an object which contains an AnyOf-type.<br>
For more details see [wiki : AnyOf.Newtonsoft.Json](https://github.com/StefH/AnyOf/wiki/AnyOf.Newtonsoft.Json)

[![NuGet Badge](https://buildstats.info/nuget/AnyOf.Newtonsoft.Json)](https://www.nuget.org/packages/AnyOf.Newtonsoft.Json)

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
            Console.WriteLine(ReturnSomething().CurrentValue);

            X(42);
            X("test");
        }

        // This method returns an string, int or bool in a random way.
        private static AnyOf<string, int, bool> ReturnSomething()
        {
            return new Random().Next(3) switch
            {
                1 => "test",
                2 => 42,
                _ => true,
            };
        }

        // This method accepts only an int and a string.
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
                    Console.WriteLine("???");
                    break;
            }
        }
    }
}
```
