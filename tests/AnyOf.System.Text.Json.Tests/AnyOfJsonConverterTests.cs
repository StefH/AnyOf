using System.Collections.Generic;
using System.Text.Json;
using AnyOf.System.Text.Json.Tests.TestModels;
using FluentAssertions;
using Xunit;

// ReSharper disable once CheckNamespace
namespace AnyOfTypes.System.Text.Json.Tests
{
    public class AnyOfJsonConverterTests
    {
        [Fact]
        public void Serialize_AnyOf_With_SimpleTypes()
        {
            // Arrange
            var test = new TestSimpleTypes
            {
                IntOrString = 1,
                NullableIntOrString = "s"
            };

            // Act
            var options = new JsonSerializerOptions
            {
                WriteIndented = false
            };
            options.Converters.Add(new AnyOfJsonConverter());

            var json = JsonSerializer.Serialize(test, options);

            // Assert
            json.Should().Be("{\"IntOrString\":1,\"NullableIntOrString\":\"s\"}");
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
                X = new[] { 42 }
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
                X = new List<A> { new A { Id = 1 }, new A { Id = 2 } }
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
                IntOrString = 1,
                NullableIntOrString = "s"
            };

            // Act
            var options = new JsonSerializerOptions();
            options.Converters.Add(new AnyOfJsonConverter());

            var result = JsonSerializer.Deserialize<TestSimpleTypes>("{\"IntOrString\":1,\"NullableIntOrString\":\"s\"}", options);

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

            var result = JsonSerializer.Deserialize<TestComplexTypes>("{\"AorB\":{\"Id\":1}}", options)!;

            // Assert
            result.AorB.First.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Deserialize_AnyOf_With_ComplexTypes_DifferentCasing()
        {
            // Arrange
            var expected = new A2
            {
                id = 1
            };

            // Act
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new AnyOfJsonConverter());

            var result = JsonSerializer.Deserialize<TestComplexTypes2>("{\"AorB\":{\"Id\":1}}", options)!;

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

            var result = JsonSerializer.Deserialize<TestComplexArray>("{\"X\":[42]}", options)!;

            // Assert
            result.X.First.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Deserialize_AnyOf_With_StringList()
        {
            // Arrange
            var expected = new[] { "a", "b" };

            // Act
            var options = new JsonSerializerOptions();
            options.Converters.Add(new AnyOfJsonConverter());

            var result = JsonSerializer.Deserialize<TestComplexArray>("{\"X\":[\"a\", \"b\"]}", options)!;

            // Assert
            result.X.Second.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Deserialize_AnyOf_With_ObjectList_A()
        {
            // Arrange
            var expected = new List<A>
            {
                new A
                {
                    Id = 1
                },
                new A
                {
                    Id = 2
                }
            };

            // Act
            var options = new JsonSerializerOptions();
            options.Converters.Add(new AnyOfJsonConverter());

            var result = JsonSerializer.Deserialize<TestComplexArray>("{\"X\":[{\"Id\":1},{\"Id\":2}]}", options)!;

            // Assert
            result.X.Third.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Deserialize_AnyOf_With_ObjectList_B()
        {
            // Arrange
            var expected = new List<B>
            {
                new B
                {
                    Guid = "a"
                },
                new B
                {
                    Guid = "b"
                }
            };

            // Act
            var options = new JsonSerializerOptions();
            options.Converters.Add(new AnyOfJsonConverter());

            var result = JsonSerializer.Deserialize<TestComplexArray>("{\"X\":[{\"Guid\":\"a\"},{\"Guid\":\"b\"}]}", options)!;

            // Assert
            result.X.Fourth.Should().BeEquivalentTo(expected);
        }
    }
}