using AnyOf.SourceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace AnyOfGenerator;

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
        BuildHashCodeCalculatorClass(options, context);
        BuildAnyOfTypesEnumClass(options, context);
        // BuildBaseClass(options, context);

        for (int numberOfTypes = 2; numberOfTypes <= Max; numberOfTypes++)
        {
            BuildTxClass(options, context, numberOfTypes);
        }
    }

    private static string[] GetTypeNames(int numberOfTypes)
    {
        return Enumerable.Range(0, numberOfTypes).Select(idx => $"{(idx + 1).Ordinalize()}").ToArray();
    }

    private static void BuildHashCodeCalculatorClass(OutputOptions options, GeneratorExecutionContext? context)
    {
        const string filename = "HashCodeCalculator.g.cs";

        var sb = new StringBuilder(AddHeader());

        sb.AppendLine(
            """
            using System.Collections.Generic;
            using System.Linq;

            namespace AnyOfTypes
            {
                // Code is based on https://github.com/Informatievlaanderen/hashcode-calculator
                internal static class HashCodeCalculator
                {
                    public static int GetHashCode(IEnumerable<object> hashFieldValues)
                    {
                        const int offset = unchecked((int)2166136261);
                        const int prime = 16777619;
            
                        static int HashCodeAggregator(int hashCode, object value) => value == null
                            ? (hashCode ^ 0) * prime
                            : (hashCode ^ value.GetHashCode()) * prime;
            
                        return hashFieldValues.Aggregate(offset, HashCodeAggregator);
                    }
                }
            }
            """);

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

    private static void BuildAnyOfTypesEnumClass(OutputOptions options, GeneratorExecutionContext? context)
    {
        const string filename = "AnyOfTypes.g.cs";
        var typeNames = GetTypeNames(Max);

        var sb = new StringBuilder(AddHeader());
        sb.Append(
            $$"""
            namespace AnyOfTypes
            {

                public enum AnyOfType
                {
                    Undefined = 0, {{string.Join(", ", typeNames.Select((x, i) => $"{x} = {i + 1}"))}}
                }
            }
            """);

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

    private static void BuildBaseClass(OutputOptions options, GeneratorExecutionContext? context)
    {
        var nullable = options.SupportsNullable ? "?" : string.Empty;

        var sb = new StringBuilder();
        sb.Append(AddHeader());

        if (options.SupportsNullable)
        {
            sb.AppendLine("#nullable enable");
        }

        sb.AppendLine(
            $$"""
            using System;
            using System.Diagnostics;
            using System.Collections.Generic;

            namespace AnyOfTypes
            {
                public struct AnyOfBase
                {
                    public virtual int NumberOfTypes { get; private set; }
                    private virtual object{nullable} _currentValue;
                    private virtual readonly Type _currentValueType;
                    private virtual readonly AnyOfType _currentType;

            {{AddProperty(@"AnyOfType", "CurrentType", "_currentType")}}

            {{AddProperty($"object{nullable}", "CurrentValue", "_currentValue")}}

            {{AddProperty(@"Type", "CurrentValueType", "_currentValueType")}}
                }
            }
            """);

        if (options.SupportsNullable)
        {
            sb.AppendLine("#nullable disable");
        }

        var code = sb.ToString();
        var filename = @"AnyOfBase.g.cs";
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
        var genericTypesAsCommaSeparatedString = string.Join(", ", typeNames.Select(t => $"T{t}"));
        var typesAsCommaSeparatedString = $"{string.Join(", ", typeNames.Select(t => $"typeof(T{t})"))}";
        var thisType = $"AnyOf<{string.Join(", ", typeNames.Select(t => $"{{typeof(T{t}).Name}}"))}>";

        var nullable = options.SupportsNullable ? "?" : string.Empty;
        var @default = options.SupportsNullable ? "!" : string.Empty;

        var sb = new StringBuilder(AddHeader());

        if (options.SupportsNullable)
        {
            sb.AppendLine("#nullable enable");
        }

        sb.AppendLine(
            $$"""
            using System;
            using System.Diagnostics;
            using System.Collections.Generic;

            namespace AnyOfTypes
            {
                [DebuggerDisplay("{_thisType}, AnyOfType = {_currentType}; Type = {_currentValueType?.Name}; Value = '{ToString()}'")]
                public struct AnyOf<{{genericTypesAsCommaSeparatedString}}> : IEquatable<AnyOf<{{genericTypesAsCommaSeparatedString}}>>
                {
                    private readonly string _thisType => $"{{thisType}}";
                    private readonly int _numberOfTypes;
                    private readonly object{{nullable}} _currentValue;
                    private readonly Type _currentValueType;
                    private readonly AnyOfType _currentType;

            {{string.Join(
                Environment.NewLine,
                typeNames.Select(t => $"        private readonly T{t} _{t.ToLowerInvariant()};"))}}

                    public readonly AnyOfType[] AnyOfTypes => new[] { {{string.Join(", ", typeNames.Select(t => $"AnyOfType.{t}"))}} };
                    public readonly Type[] Types => new[] { {{typesAsCommaSeparatedString}} };

                    public bool IsUndefined => _currentType == AnyOfType.Undefined;

            {{string.Join(
                Environment.NewLine,
                typeNames.Select(t => $"        public bool Is{t} => _currentType == AnyOfType.{t};"))}}

            """);


        Array.ForEach(typeNames, typeName =>
        {
            sb.AppendLine(
                $$"""
                        public static implicit operator AnyOf<{{genericTypesAsCommaSeparatedString}}>(T{{typeName}} value) => new AnyOf<{{genericTypesAsCommaSeparatedString}}>(value);

                        public static implicit operator T{{typeName}}(AnyOf<{{genericTypesAsCommaSeparatedString}}> @this) => @this.{{typeName}};

                        public AnyOf(T{{typeName}} value)
                        {
                            _numberOfTypes = {{numberOfTypes}};
                            _currentType = AnyOfType.{{typeName}};
                            _currentValue = value;
                            _currentValueType = typeof(T{{typeName}});
                {{string.Join(
                    Environment.NewLine,
                    typeNames.Select(t => t == typeName
                        ? $"            _{t.ToLowerInvariant()} = value;"
                        : $"            _{t.ToLowerInvariant()} = default{@default};"))}}
                        }

                        public T{{typeName}} {{typeName}}
                        {
                            get
                            {
                                Validate(AnyOfType.{{typeName}});
                                return _{{typeName.ToLowerInvariant()}};
                            }
                        }

                """);
        });

        sb.AppendLine(
            $$"""
                    private void Validate(AnyOfType desiredType)
                    {
                        if (desiredType != _currentType)
                        {
                            throw new InvalidOperationException($"Attempting to get {desiredType} when {_currentType} is set");
                        }
                    }

            {{AddProperty("AnyOfType", "CurrentType", "_currentType")}}

            {{AddProperty($"object{nullable}", "CurrentValue", "_currentValue")}}

            {{AddProperty("Type", "CurrentValueType", "_currentValueType")}}

                    public override int GetHashCode()
                    {
                        var fields = new object[]
                        {
                            _numberOfTypes,
                            _currentValue,
                            _currentType,
            {{string.Join(
                Environment.NewLine,
                typeNames.Select(t => $"                _{t.ToLowerInvariant()},"))}}
            {{string.Join(
                Environment.NewLine,
                typeNames.Select(t => $"                typeof(T{t}),"))}}
                        };
                        return HashCodeCalculator.GetHashCode(fields);
                    }

                    public bool Equals(AnyOf<{{genericTypesAsCommaSeparatedString}}> other)
                    {
                        return _currentType == other._currentType &&
                            _numberOfTypes == other._numberOfTypes &&
                            EqualityComparer<object{{nullable}}>.Default.Equals(_currentValue, other._currentValue) &&
            {{string.Join(
                Environment.NewLine,
                typeNames.Select(t => $"                EqualityComparer<T{t}>.Default.Equals(_{t.ToLowerInvariant()}, other._{t.ToLowerInvariant()}){(t == typeNames.Last() ? ";" : " &&")}"))}}
                    }

                    public static bool operator ==(AnyOf<{{genericTypesAsCommaSeparatedString}}> obj1, AnyOf<{{genericTypesAsCommaSeparatedString}}> obj2)
                    {
                        return EqualityComparer<AnyOf<{{genericTypesAsCommaSeparatedString}}>>.Default.Equals(obj1, obj2);
                    }

                    public static bool operator !=(AnyOf<{{genericTypesAsCommaSeparatedString}}> obj1, AnyOf<{{genericTypesAsCommaSeparatedString}}> obj2)
                    {
                        return !(obj1 == obj2);
                    }

                    public override bool Equals(object{{nullable}} obj)
                    {
                        return obj is AnyOf<{{genericTypesAsCommaSeparatedString}}> o && Equals(o);
                    }

                    public override string{{nullable}} ToString()
                    {
                        return IsUndefined ? null : $"{_currentValue}";
                    }
                }
            }
            """);

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

    private static string AddProperty(string type, string name, string privateField) =>
        $$"""
                public {{type}} {{name}}
                {
                    get
                    {
                        return {{privateField}};
                    }
                }
        """;

    private static string AddHeader() =>
        """
        //------------------------------------------------------------------------------
        // <auto-generated>
        //     This code was generated by https://github.com/StefH/AnyOf.
        //
        //     Changes to this file may cause incorrect behavior and will be lost if
        //     the code is regenerated.
        // </auto-generated>
        //------------------------------------------------------------------------------

        """;
}