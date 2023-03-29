using AnyOf.SourceGenerator;
using AnyOfGenerator;

namespace AnyOfCodeGeneratorTester;

class Program
{
    static void Main(string[] args)
    {
        var generator = new AnyOfCodeGenerator();

        generator.Generate(new OutputOptions
        {
            Type = OutputType.File,
            SupportsNullable = false,
            Folder = "../../../../AnyOf"
        });
    }
}