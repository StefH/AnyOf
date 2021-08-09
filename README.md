# AnyOf
Use the `AnyOf<First, TSecond, ...>` type to handle multiple defined types as input parameters for methods.

This project uses code generation to generate up to 10 AnyOf-types:

- `AnyOf<TFirst, TSecond>`
- `AnyOf<TFirst, TSecond, TThird>`
- `AnyOf<TFirst, TSecond, TThird, TFourth>`
- ...

# Install
[![NuGet Badge](https://buildstats.info/nuget/AnyOf)](https://www.nuget.org/packages/AnyOf)

You can install from NuGet using the following command in the package manager window:

`Install-Package AnyOf`

Or via the Visual Studio NuGet package manager or if you use the `dotnet` command:

`dotnet add package AnyOf`

#### :pencil2: Using in a Library project
When you use this Source Generator as a package reference in your library project, make sure that you define this NuGet package as a Private Asset (`<PrivateAssets>`) and define the correct `<IncludeAssets>`. This is needed to indicate that this dependency is purely used as a development dependency and that you don’t want to expose that to projects that will consume your package.

``` xml
<PackageReference Include="AnyOf" Version="0.0.16">
    <!-- 👇 -->
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
</PackageReference>
```

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
