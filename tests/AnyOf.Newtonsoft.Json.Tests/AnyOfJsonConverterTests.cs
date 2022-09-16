using System.Collections.Generic;
using AnyOf.System.Text.Json.Tests.TestModels;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

// ReSharper disable once CheckNamespace
namespace AnyOfTypes.Newtonsoft.Json.Tests;

public class AnyOfJsonConverterTests
{
    [Fact]
    public void Serialize_AnyOf_With_List()
    {
        // Arrange
        var test = new TestList
        {
            List = new List<AnyOf<int, string>> { 1, "s" },
            NullableList = new List<AnyOf<int, string>> { 2, "a" },
            ListWithNullable = new List<AnyOf<int, string>?> { 3, "n", null }
        };

        // Act
        var options = new JsonSerializerSettings
        {
            Formatting = Formatting.None
        };
        options.Converters.Add(new AnyOfJsonConverter());

        var json = JsonConvert.SerializeObject(test, options);

        // Assert
        json.Should().Be("{\"List\":[1,\"s\"],\"NullableList\":[2,\"a\"],\"ListWithNullable\":[3,\"n\",null]}");
    }

    [Fact]
    public void Serialize_AnyOf_With_SimpleTypes()
    {
        // Arrange
        var test = new TestSimpleTypes
        {
            IntOrString = 1,
            NullableIntOrString = "s",
            NullableStringOrStrings = new[] { "a", "b" }
        };

        // Act
        var options = new JsonSerializerSettings
        {
            Formatting = Formatting.None
        };
        options.Converters.Add(new AnyOfJsonConverter());

        var json = JsonConvert.SerializeObject(test, options);

        // Assert
        json.Should().Be("{\"IntOrString\":1,\"NullableIntOrString\":\"s\",\"NullableStringOrStrings\":[\"a\",\"b\"]}");
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
        var options = new JsonSerializerSettings
        {
            Formatting = Formatting.None
        };
        options.Converters.Add(new AnyOfJsonConverter());

        var json = JsonConvert.SerializeObject(test, options);

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
        var options = new JsonSerializerSettings
        {
            Formatting = Formatting.None
        };
        options.Converters.Add(new AnyOfJsonConverter());

        var json = JsonConvert.SerializeObject(test, options);

        // Assert
        json.Should().Be("{\"IntOrStringOrAOrB\":1}");
    }

    [Fact]
    public void Deserialize_AnyOf_With_List()
    {
        // Arrange
        var expected = new TestList
        {
            List = new List<AnyOf<int, string>> { 1, "s" },
            NullableList = new List<AnyOf<int, string>> { 2, "a" },
            ListWithNullable = new List<AnyOf<int, string>?> { 3, "n", null }
        };

        // Act
        var options = new JsonSerializerSettings();
        options.Converters.Add(new AnyOfJsonConverter());

        var result = JsonConvert.DeserializeObject<TestList>("{\"List\":[1,\"s\"],\"NullableList\":[2,\"a\"],\"ListWithNullable\":[3,\"n\",null]}", options);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Deserialize_AnyOf_With_SimpleTypes()
    {
        // Arrange
        var json = "{\"IntOrString\":1,\"NullableIntOrString\":\"s\",\"NullableStringOrStrings\":[\"a\",\"b\"]}";
        var array = new[] { "a", "b" };
        var expected = new TestSimpleTypes
        {
            IntOrString = 1,
            NullableIntOrString = "s",
            NullableStringOrStrings = array
        };

        // Act
        var options = new JsonSerializerSettings();
        options.Converters.Add(new AnyOfJsonConverter());

        var result = JsonConvert.DeserializeObject<TestSimpleTypes>(json, options)!;

        // Assert
        result.IntOrString.Should().Be(expected.IntOrString);
        result.NullableIntOrString.Should().Be(expected.NullableIntOrString);
        result.NullableStringOrStrings!.Value.Second.Should().Contain(array);
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
        var options = new JsonSerializerSettings();
        options.Converters.Add(new AnyOfJsonConverter());

        var result = JsonConvert.DeserializeObject<TestComplexTypes>("{\"AorB\":{\"Id\":1}}", options)!;

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
        var options = new JsonSerializerSettings();
        options.Converters.Add(new AnyOfJsonConverter());

        var result = JsonConvert.DeserializeObject<TestComplexTypes2>("{\"AorB\":{\"Id\":1}}", options)!;

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
        var options = new JsonSerializerSettings();
        options.Converters.Add(new AnyOfJsonConverter());

        var result = JsonConvert.DeserializeObject<TestMixedTypes>("{\"IntOrStringOrAOrB\":1}", options);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Deserialize_AnyOf_With_IntArray()
    {
        // Arrange
        var expected = new[] { 42 };

        // Act
        var options = new JsonSerializerSettings();
        options.Converters.Add(new AnyOfJsonConverter());

        var result = JsonConvert.DeserializeObject<TestComplexArray>("{\"X\":[42]}", options)!;

        // Assert
        result.X.First.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Deserialize_AnyOf_With_StringList()
    {
        // Arrange
        var expected = new[] { "a", "b" };

        // Act
        var options = new JsonSerializerSettings();
        options.Converters.Add(new AnyOfJsonConverter());

        var result = JsonConvert.DeserializeObject<TestComplexArray>("{\"X\":[\"a\", \"b\"]}", options)!;

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
        var options = new JsonSerializerSettings();
        options.Converters.Add(new AnyOfJsonConverter());

        var result = JsonConvert.DeserializeObject<TestComplexArray>("{\"X\":[{\"Id\":1},{\"Id\":2}]}", options)!;

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
        var options = new JsonSerializerSettings();
        options.Converters.Add(new AnyOfJsonConverter());

        var result = JsonConvert.DeserializeObject<TestComplexArray>("{\"X\":[{\"Guid\":\"a\"},{\"Guid\":\"b\"}]}", options)!;

        // Assert
        result.X.Fourth.Should().BeEquivalentTo(expected);
    }
}