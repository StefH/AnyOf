using AnyOf.SourceGenerator;
using AnyOfGenerator;

namespace AnyOfCodeGeneratorTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new AnyOfCodeGenerator();

            c.Generate(OutputType.File);
        }
    }
}