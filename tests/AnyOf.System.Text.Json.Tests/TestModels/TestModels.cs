using System.Collections.Generic;
using AnyOfTypes;

namespace AnyOf.System.Text.Json.Tests.TestModels
{
    public class TestComplexTypes
    {
        public AnyOf<A, B> AorB { get; set; }
    }

    public class TestComplexTypes2
    {
        public AnyOf<A2, B> AorB { get; set; }
    }

    public class TestSimpleTypes
    {
        public AnyOf<int, string> IntOrString { get; set; }

        public AnyOf<int, string>? NullableIntOrString { get; set; }
    }

    public class TestMixedTypes
    {
        public AnyOf<int, string, A, B> IntOrStringOrAOrB { get; set; }
    }

    public class TestComplexArray
    {
        public AnyOf<int[], List<string>, List<A>, IEnumerable<B>> X { get; set; }
    }

    public class A
    {
        public int Id { get; set; }
    }

    public class A2
    {
        public int id { get; set; }
    }

    public class B
    {
        public string Guid { get; set; }
    }
}