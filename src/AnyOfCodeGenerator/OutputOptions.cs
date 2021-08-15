namespace AnyOf.SourceGenerator
{
    public class OutputOptions
    {
        public OutputType Type { get; set; } = OutputType.Context;

        public bool SupportsNullable { get; set; } = true;

        public string Folder { get; set; }
    }
}