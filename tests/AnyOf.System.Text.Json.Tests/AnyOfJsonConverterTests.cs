using System.Collections.Generic;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace AnyOfTypes.System.Text.Json.Tests
{
    public class AnyOfJsonConverterTests
    {
        public class TestComplexTypes
        {
            public AnyOf<A, B> AorB { get; set; }
        }

        public class TestSimpleTypes
        {
            public AnyOf<int, string> IntOrString { get; set; }
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

        public class B
        {
            public string Guid { get; set; }
        }

        [Fact]
        public void Serialize_AnyOf_With_SimpleTypes()
        {
            // Arrange
            var test = new TestSimpleTypes
            {
                IntOrString = 1
            };

            // Act
            var options = new JsonSerializerOptions
            {
                WriteIndented = false
            };
            options.Converters.Add(new AnyOfJsonConverter());

            var json = JsonSerializer.Serialize(test, options);

            // Assert
            json.Should().Be("{\"IntOrString\":1}");
        }

        [Fact]
        public void Serialize_AnyOf_With_ComplexTypes()
        {
            // Arrange
            var test = new TestComplexTypes
            {
                AorB = new A
                {
                    Id = 1
                }
            };

            // Act
            var options = new JsonSerializerOptions
            {
                WriteIndented = false
            };
            options.Converters.Add(new AnyOfJsonConverter());

            var json = JsonSerializer.Serialize(test, options);

            // Assert
            json.Should().Be("{\"AorB\":{\"Id\":1}}");
        }

        [Fact]
        public void Serialize_AnyOf_With_MixedTypes()
        {
            // Arrange
            var test = new TestMixedTypes
            {
                IntOrStringOrAOrB = 1
            };

            // Act
            var options = new JsonSerializerOptions
            {
                WriteIndented = false
            };
            options.Converters.Add(new AnyOfJsonConverter());

            var json = JsonSerializer.Serialize(test, options);

            // Assert
            json.Should().Be("{\"IntOrStringOrAOrB\":1}");
        }

        [Fact]
        public void Serialize_AnyOf_With_IntArray()
        {
            // Arrange
            var test = new TestComplexArray
            {
                X = new int[] { 42 }
            };

            // Act
            var options = new JsonSerializerOptions
            {
                WriteIndented = false
            };
            options.Converters.Add(new AnyOfJsonConverter());

            var json = JsonSerializer.Serialize(test, options);

            // Assert
            json.Should().Be("{\"X\":[42]}");
        }

        [Fact]
        public void Serialize_AnyOf_With_ObjectList()
        {
            // Arrange
            var test = new TestComplexArray
            {
                X = new List<A> { new A { Id= 1 }, new A { Id = 2 } }
            };

            // Act
            var options = new JsonSerializerOptions
            {
                WriteIndented = false
            };
            options.Converters.Add(new AnyOfJsonConverter());

            var json = JsonSerializer.Serialize(test, options);

            // Assert
            json.Should().Be("{\"X\":[{\"Id\":1},{\"Id\":2}]}");
        }

        [Fact]
        public void Deserialize_AnyOf_With_SimpleTypes()
        {
            // Arrange
            var expected = new TestSimpleTypes
            {
                IntOrString = 1
            };

            // Act
            var options = new JsonSerializerOptions();
            options.Converters.Add(new AnyOfJsonConverter());

            var result = JsonSerializer.Deserialize<TestSimpleTypes>("{\"IntOrString\":1}", options);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Deserialize_AnyOf_With_ComplexTypes()
        {
            // Arrange
            var expected = new A
            {
                Id = 1
            };

            // Act
            var options = new JsonSerializerOptions();
            options.Converters.Add(new AnyOfJsonConverter());

            var result = JsonSerializer.Deserialize<TestComplexTypes>("{\"AorB\":{\"Id\":1}}", options);

            // Assert
            result.AorB.First.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Deserialize_AnyOf_With_MixedTypes()
        {
            // Arrange
            var expected = new TestMixedTypes
            {
                IntOrStringOrAOrB = 1
            };

            // Act
            var options = new JsonSerializerOptions();
            options.Converters.Add(new AnyOfJsonConverter());

            var result = JsonSerializer.Deserialize<TestMixedTypes>("{\"IntOrStringOrAOrB\":1}", options);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Deserialize_AnyOf_With_IntArray()
        {
            // Arrange
            var expected = new int[] { 42 };

            // Act
            var options = new JsonSerializerOptions();
            options.Converters.Add(new AnyOfJsonConverter());

            var result = JsonSerializer.Deserialize<TestComplexArray>("{\"X\":[42]}", options);

            // Assert
            result.X.First.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Deserialize_AnyOf_With_StringList()
        {
            // Arrange
            var expected = new [] { "a", "b" };

            // Act
            var options = new JsonSerializerOptions();
            options.Converters.Add(new AnyOfJsonConverter());

            var result = JsonSerializer.Deserialize<TestComplexArray>("{\"X\":[\"a\", \"b\"]}", options);

            // Assert
            result.X.Second.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Deserialize_AnyOf_With_ObjectList()
        {
            // Arrange
            var expected = new[] { "a", "b" };

            // Act
            var options = new JsonSerializerOptions();
            options.Converters.Add(new AnyOfJsonConverter());

            var result = JsonSerializer.Deserialize<TestComplexArray>("{\"X\":[{\"Id\":1},{\"Id\":2}]}", options);

            // Assert
            result.X.First.Should().BeEquivalentTo(expected);
        }
    }
}