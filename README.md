# AnyOf
Use the `AnyOf<First, TSecond, ...>` type to handle multiple defined types as input parameters for methods.

This project uses code generation to generate up to 10 AnyOf-types:

- `AnyOf<TFirst, TSecond>`
- `AnyOf<TFirst, TSecond, TThird>`
- `AnyOf<TFirst, TSecond, TThird, TFourth>`
- ...

# Install
## The normal version:
[![NuGet Badge](https://img.shields.io/nuget/v/AnyOf)](https://www.nuget.org/packages/AnyOf)

## The source-generator version:
[![NuGet Badge](https://img.shields.io/nuget/v/AnyOf.SourceGenerator)](https://www.nuget.org/packages/AnyOf.SourceGenerator)

## AnyOf.Newtonsoft.Json
This package can be used to serialize/deserialize (with Newtonsoft.Json) an object which contains an AnyOf-type.<br>
For more details see [wiki : AnyOf.Newtonsoft.Json](https://github.com/StefH/AnyOf/wiki/AnyOf.Newtonsoft.Json)

[![NuGet Badge](https://img.shields.io/nuget/v/AnyOf.Newtonsoft.Json)](https://www.nuget.org/packages/AnyOf.Newtonsoft.Json)

## AnyOf.System.Text.Json
This package can be used to serialize/deserialize (with System.Text.Json) an object which contains an AnyOf-type.<br>
For more details see [wiki : AnyOf.System.Text.Json](https://github.com/StefH/AnyOf/wiki/AnyOf.System.Text.Json)

[![NuGet Badge](https://img.shields.io/nuget/v/AnyOf.System.Text.Json)](https://www.nuget.org/packages/AnyOf.System.Text.Json)

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

### Sponsors

[Entity Framework Extensions](https://entityframework-extensions.net/?utm_source=StefH&utm_medium=AnyOf) and [Dapper Plus](https://dapper-plus.net/?utm_source=StefH) are major sponsors and proud to contribute to the development of **AnyOf**, **AnyOf.Newtonsoft.Json** and **AnyOf.System.Text.Json**.

[![Entity Framework Extensions](https://raw.githubusercontent.com/StefH/resources/main/sponsor/entity-framework-extensions-sponsor.png)](https://entityframework-extensions.net/bulk-insert?utm_source=StefH)

[![Dapper Plus](https://raw.githubusercontent.com/StefH/resources/main/sponsor/dapper-plus-sponsor.png)](https://dapper-plus.net/bulk-insert?utm_source=StefH)