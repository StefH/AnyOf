using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using AnyOf.SourceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace AnyOfGenerator
{
    [Generator]
    public class AnyOfCodeGenerator : ISourceGenerator
    {
        private const int Max = 10;

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required
        }

        public void Execute(GeneratorExecutionContext context)
        {
            bool supportsNullable = context.ParseOptions switch
            {
                CSharpParseOptions csharpParseOptions => csharpParseOptions.LanguageVersion >= LanguageVersion.CSharp8,

                // VisualBasicParseOptions visualBasicParseOptions => visualBasicParseOptions.LanguageVersion >= Microsoft.CodeAnalysis.VisualBasic.LanguageVersion.VisualBasic16,

                _ => throw new NotSupportedException("Only C# is supported."),
            };

            bool nullableEnabled = context.Compilation.Options.NullableContextOptions != NullableContextOptions.Disable;

            Generate(new OutputOptions { Type = OutputType.Context, SupportsNullable = supportsNullable }, context);
        }

        public void Generate(OutputOptions options)
        {
            Generate(options, null);
        }

        private static void Generate(OutputOptions options, GeneratorExecutionContext? context)
        {
            BuildEnum(options, context);

            for (int numberOfTypes = 2; numberOfTypes <= Max; numberOfTypes++)
            {
                BuildTxClass(options, context, numberOfTypes);
            }
        }

        private static string[] GetTypeNames(int numberOfTypes)
        {
            return Enumerable.Range(0, numberOfTypes).Select(idx => $"{(idx + 1).Ordinalize()}").ToArray();
        }

        private static void BuildEnum(OutputOptions options, GeneratorExecutionContext? context)
        {
            const string filename = "AnyOfTypes.g.cs";
            var typeNames = GetTypeNames(Max);
            var typesAsString = string.Join(", ", typeNames);

            var sb = new StringBuilder();
            sb.Append(AddHeader());

            sb.AppendLine("namespace AnyOfTypes");
            sb.AppendLine("{");

            sb.AppendLine("    public enum AnyOfType");
            sb.AppendLine("    {");
            sb.AppendLine($"        Undefined = 0, {typesAsString}");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            string code = sb.ToString();

            switch (options.Type)
            {
                case OutputType.Console:
                    Console.WriteLine(code);
                    break;

                case OutputType.File:
                    File.WriteAllText(Path.Combine(options.Folder, filename), code);
                    break;

                default:
                    context?.AddSource(filename, SourceText.From(code, Encoding.UTF8));
                    break;
            }
        }

        private static void BuildTxClass(OutputOptions options, GeneratorExecutionContext? context, int numberOfTypes)
        {
            var typeNames = GetTypeNames(numberOfTypes);
            var typesAsString = string.Join(", ", typeNames.Select(t => $"T{t}"));

            var nullable = options.SupportsNullable ? "?" : string.Empty;
            var @default = options.SupportsNullable ? "!" : string.Empty;

            var sb = new StringBuilder();
            sb.Append(AddHeader());

            if (options.SupportsNullable)
            {
                sb.AppendLine("#nullable enable");
            }
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Diagnostics;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine();

            sb.AppendLine("namespace AnyOfTypes");
            sb.AppendLine("{");

            sb.AppendLine("    [DebuggerDisplay(\"AnyOfType = {_currentType}; Type = {_currentValueType?.Name}; Value = '{ToString()}'\")]");
            sb.AppendLine($"    public struct AnyOf<{typesAsString}>");
            sb.AppendLine("    {");

            sb.AppendLine("        private readonly int _numberOfTypes;");
            sb.AppendLine($"        private readonly object{nullable} _currentValue;");
            sb.AppendLine("        private readonly Type _currentValueType;");
            sb.AppendLine("        private readonly AnyOfType _currentType;");
            sb.AppendLine();

            Array.ForEach(typeNames, t => sb.AppendLine($"        private readonly T{t} _{t.ToLowerInvariant()};"));
            sb.AppendLine();

            sb.AppendLine("        public bool IsUndefined => _currentType == AnyOfType.Undefined;");
            Array.ForEach(typeNames, t => sb.AppendLine($"        public bool Is{t} => _currentType == AnyOfType.{t};"));
            sb.AppendLine();

            Array.ForEach(typeNames, t =>
            {
                sb.AppendLine($"        public static implicit operator AnyOf<{typesAsString}>(T{t} value) => new AnyOf<{typesAsString}>(value);");
                sb.AppendLine();

                sb.AppendLine($"        public static implicit operator T{t}(AnyOf<{typesAsString}> @this) => @this.{t};");
                sb.AppendLine();

                sb.AppendLine($"        public AnyOf(T{t} value)");
                sb.AppendLine("        {");
                sb.AppendLine($"            _numberOfTypes = {numberOfTypes};");
                sb.AppendLine($"            _currentType = AnyOfType.{t};");
                sb.AppendLine($"            _currentValue = value;");
                sb.AppendLine($"            _currentValueType = typeof(T{t});");
                sb.AppendLine($"            _{t.ToLowerInvariant()} = value;");
                Array.ForEach(typeNames.Except(new[] { t }).ToArray(), dt => sb.AppendLine($"            _{dt.ToLowerInvariant()} = default{@default};"));
                sb.AppendLine("        }");
                sb.AppendLine();

                sb.AppendLine($"        public T{t} {t}");
                sb.AppendLine("        {");
                sb.AppendLine("            get");
                sb.AppendLine("            {");
                sb.AppendLine($"               Validate(AnyOfType.{t});");
                sb.AppendLine($"               return _{t.ToLowerInvariant()};");
                sb.AppendLine("            }");
                sb.AppendLine("        }");
                sb.AppendLine();
            });

            sb.AppendLine("        private void Validate(AnyOfType desiredType)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (desiredType != _currentType)");
            sb.AppendLine("            {");
            sb.AppendLine("                throw new InvalidOperationException($\"Attempting to get {desiredType} when {_currentType} is set\");");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();

            AddProperty(sb, "AnyOfType", "CurrentType", "_currentType");

            AddProperty(sb, $"object{nullable}", "CurrentValue", "_currentValue");

            AddProperty(sb, "Type", "CurrentValueType", "_currentValueType");

            sb.AppendLine("        public override int GetHashCode()");
            sb.AppendLine("        {");
            sb.AppendLine("            var hash = new HashCode();");
            sb.AppendLine("            hash.Add(_currentValue);");
            sb.AppendLine("            hash.Add(_currentType);");
            Array.ForEach(typeNames, t => sb.AppendLine($"            hash.Add(_{t.ToLowerInvariant()});"));
            Array.ForEach(typeNames, t => sb.AppendLine($"            hash.Add(typeof(T{t}));"));
            sb.AppendLine("            return hash.ToHashCode();");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine($"        private bool Equals(AnyOf<{typesAsString}> other)");
            sb.AppendLine("        {");
            sb.AppendLine("            return _currentType == other._currentType &&");
            sb.AppendLine("                   _numberOfTypes == other._numberOfTypes &&");
            sb.AppendLine($"                   EqualityComparer<object{nullable}>.Default.Equals(_currentValue, other._currentValue) &&");
            Array.ForEach(typeNames, t => sb.AppendLine($"            EqualityComparer<T{t}>.Default.Equals(_{t.ToLowerInvariant()}, other._{t.ToLowerInvariant()}){(t == typeNames.Last() ? ";" : " &&")}"));
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine($"        public static bool operator ==(AnyOf<{typesAsString}> obj1, AnyOf<{typesAsString}> obj2)");
            sb.AppendLine("        {");
            sb.AppendLine($"            return obj1.Equals(obj2);");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine($"        public static bool operator !=(AnyOf<{typesAsString}> obj1, AnyOf<{typesAsString}> obj2)");
            sb.AppendLine("        {");
            sb.AppendLine($"            return !obj1.Equals(obj2);");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine($"        public override bool Equals(object{nullable} obj)");
            sb.AppendLine("        {");
            sb.AppendLine($"            return obj is AnyOf<{typesAsString}> o && Equals(o);");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine($"        public override string{nullable} ToString()");
            sb.AppendLine("        {");
            sb.AppendLine("            return IsUndefined ? null : $\"{_currentValue}\";");
            sb.AppendLine("        }");

            sb.AppendLine("    }");

            sb.AppendLine("}");
            if (options.SupportsNullable)
            {
                sb.AppendLine("#nullable disable");
            }

            var code = sb.ToString();
            var filename = $"AnyOf_{numberOfTypes}.g.cs";
            switch (options.Type)
            {
                case OutputType.Console:
                    Console.WriteLine(code);
                    break;
                case OutputType.File:
                    File.WriteAllText(Path.Combine(options.Folder, filename), code);
                    break;
                default:
                    context?.AddSource(filename, SourceText.From(code, Encoding.UTF8));
                    break;
            }
        }

        private static void AddProperty(StringBuilder src, string type, string name, string privateField)
        {
            src.AppendLine($"        public {type} {name}");
            src.AppendLine("        {");
            src.AppendLine("            get");
            src.AppendLine("            {");
            src.AppendLine($"               return {privateField};");
            src.AppendLine("            }");
            src.AppendLine("        }");
            src.AppendLine();
        }

        private static StringBuilder AddHeader()
        {
            var sb = new StringBuilder();
            sb.AppendLine("//------------------------------------------------------------------------------");
            sb.AppendLine("// <auto-generated>");
            sb.AppendLine("//     This code was generated by https://github.com/StefH/AnyOf.");
            sb.AppendLine("//");
            sb.AppendLine("//     Changes to this file may cause incorrect behavior and will be lost if");
            sb.AppendLine("//     the code is regenerated.");
            sb.AppendLine("// </auto-generated>");
            sb.AppendLine("//------------------------------------------------------------------------------");
            sb.AppendLine();
            return sb;
        }
    }
}