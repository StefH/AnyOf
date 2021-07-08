using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
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
            Generate(context);
        }

        public void Test()
        {
            Generate(null);
        }

        private void Generate(GeneratorExecutionContext? context)
        {
            BuildEnum(context);

            for (int numberOfTypes = 2; numberOfTypes <= Max; numberOfTypes++)
            {
                BuildTxClass(context, numberOfTypes);
            }
        }

        private static string[] GetTypeNames(int numberOfTypes)
        {
            return Enumerable.Range(0, numberOfTypes).Select(idx => $"{(idx + 1).Ordinalize()}").ToArray();
        }

        private static void BuildEnum(GeneratorExecutionContext? context)
        {
            var typeNames = GetTypeNames(Max);
            var typesAsString = string.Join(", ", typeNames);

            var src = new StringBuilder();
            src.AppendLine("namespace AnyOfTypes");
            src.AppendLine("{");

            src.AppendLine("    public enum AnyOfType");
            src.AppendLine("    {");
            src.AppendLine($"        Undefined = 0, {typesAsString}");
            src.AppendLine("    }");
            src.AppendLine("}");

            string code = src.ToString();

            if (context is null)
            {
                Console.WriteLine(code);
            }
            else
            {
                context?.AddSource($"AnyOfTypes_Generated", SourceText.From(code, Encoding.UTF8));
            }
        }

        private static void BuildTxClass(GeneratorExecutionContext? context, int numberOfTypes)
        {
            var typeNames = GetTypeNames(numberOfTypes);
            var typesAsString = string.Join(", ", typeNames.Select(t => $"T{t}"));

            var src = new StringBuilder();
            src.AppendLine("using System;");
            src.AppendLine("using System.Diagnostics;");
            src.AppendLine("using System.Collections.Generic;");
            src.AppendLine();

            src.AppendLine("namespace AnyOfTypes");
            src.AppendLine("{");

            src.AppendLine("    [DebuggerDisplay(\"AnyOfType = {_currentType}; Type = {_currentValueType?.Name}; Value = '{ToString()}'\")]");
            src.AppendLine($"    public struct AnyOf<{typesAsString}>");
            src.AppendLine("    {");

            src.AppendLine("        private readonly int _numberOfTypes;");
            src.AppendLine("        private readonly object _currentValue;");
            src.AppendLine("        private readonly Type _currentValueType;");
            src.AppendLine("        private readonly AnyOfType _currentType;");
            src.AppendLine();

            Array.ForEach(typeNames, t => src.AppendLine($"        private readonly T{t} _{t.ToLowerInvariant()};"));
            src.AppendLine();

            src.AppendLine("        public bool IsUndefined => _currentType == AnyOfType.Undefined;");
            Array.ForEach(typeNames, t => src.AppendLine($"        public bool Is{t} => _currentType == AnyOfType.{t};"));
            src.AppendLine();

            Array.ForEach(typeNames, t =>
            {
                src.AppendLine($"        public static implicit operator AnyOf<{typesAsString}>(T{t} value) => new AnyOf<{typesAsString}>(value);");
                src.AppendLine();

                src.AppendLine($"        public static implicit operator T{t}(AnyOf<{typesAsString}> @this) => @this.{t};");
                src.AppendLine();

                src.AppendLine($"        public AnyOf(T{t} value)");
                src.AppendLine("        {");
                src.AppendLine($"            _numberOfTypes = {numberOfTypes};");
                src.AppendLine($"            _currentType = AnyOfType.{t};");
                src.AppendLine($"            _currentValue = value;");
                src.AppendLine($"            _currentValueType = typeof(T{t});");
                src.AppendLine($"            _{t.ToLowerInvariant()} = value;");
                Array.ForEach(typeNames.Except(new[] { t }).ToArray(), dt => src.AppendLine($"            _{dt.ToLowerInvariant()} = default;"));
                src.AppendLine("        }");
                src.AppendLine();

                src.AppendLine($"        public T{t} {t}");
                src.AppendLine("        {");
                src.AppendLine("            get");
                src.AppendLine("            {");
                src.AppendLine($"               Validate(AnyOfType.{t});");
                src.AppendLine($"               return _{t.ToLowerInvariant()};");
                src.AppendLine("            }");
                src.AppendLine("        }");
                src.AppendLine();
            });

            src.AppendLine("        private void Validate(AnyOfType desiredType)");
            src.AppendLine("        {");
            src.AppendLine("            if (desiredType != _currentType)");
            src.AppendLine("            {");
            src.AppendLine("                throw new InvalidOperationException($\"Attempting to get {desiredType} when {_currentType} is set\");");
            src.AppendLine("            }");
            src.AppendLine("        }");
            src.AppendLine();

            AddProperty(src, "AnyOfType", "CurrentType", "_currentType");

            AddProperty(src, "object", "CurrentValue", "_currentValue");

            AddProperty(src, "Type", "CurrentValueType", "_currentValueType");

            src.AppendLine("        public override int GetHashCode()");
            src.AppendLine("        {");
            src.AppendLine("            var hash = new HashCode();");
            src.AppendLine("            hash.Add(_currentValue);");
            src.AppendLine("            hash.Add(_currentType);");
            Array.ForEach(typeNames, t => src.AppendLine($"                        hash.Add(_{t.ToLowerInvariant()});"));
            src.AppendLine("            return hash.ToHashCode();");
            src.AppendLine("        }");
            src.AppendLine();

            src.AppendLine($"        private bool Equals(AnyOf<{typesAsString}> other)");
            src.AppendLine("        {");
            src.AppendLine("            return _currentType == other._currentType &&");
            src.AppendLine("                   _numberOfTypes == other._numberOfTypes &&");
            src.AppendLine("                   EqualityComparer<object>.Default.Equals(_currentValue, other._currentValue) &&");
            Array.ForEach(typeNames, t => src.AppendLine($"            EqualityComparer<T{t}>.Default.Equals(_{t.ToLowerInvariant()}, other._{t.ToLowerInvariant()}){(t == typeNames.Last() ? ";" : " &&")}"));
            src.AppendLine("        }");
            src.AppendLine();

            src.AppendLine($"        public static bool operator ==(AnyOf<{typesAsString}> obj1, AnyOf<{typesAsString}> obj2)");
            src.AppendLine("        {");
            src.AppendLine($"            return obj1.Equals(obj2);");
            src.AppendLine("        }");
            src.AppendLine();

            src.AppendLine($"        public static bool operator !=(AnyOf<{typesAsString}> obj1, AnyOf<{typesAsString}> obj2)");
            src.AppendLine("        {");
            src.AppendLine($"            return !obj1.Equals(obj2);");
            src.AppendLine("        }");
            src.AppendLine();

            src.AppendLine("        public override bool Equals(object obj)");
            src.AppendLine("        {");
            src.AppendLine($"            return obj is AnyOf<{typesAsString}> o && Equals(o);");
            src.AppendLine("        }");
            src.AppendLine();

            src.AppendLine("        public override string ToString()");
            src.AppendLine("        {");
            src.AppendLine("            return IsUndefined ? null : $\"{_currentValue}\";");
            src.AppendLine("        }");

            src.AppendLine("    }");

            src.AppendLine("}");

            var code = src.ToString();

            if (context is null)
            {
                Console.WriteLine(code);
            }
            else
            {
                context?.AddSource($"AnyOf_{numberOfTypes}_Generated", SourceText.From(code, Encoding.UTF8));
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
    }
}