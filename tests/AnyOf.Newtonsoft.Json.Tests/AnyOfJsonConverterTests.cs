using System.Collections.Generic;
using System.Text.Json;
using AnyOf.System.Text.Json.Tests.TestModels;
using AnyOfTypes.Newtonsoft.Json;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace AnyOf.Newtonsoft.Json.Tests;

public class AnyOfJsonConverterTests
{
    [Fact]
    public void Serialize_AnyOf_With_SimpleTypes()
    {
        // Arrange
        var test = new TestSimpleTypes
        {
            IntOrString = 1
        };

        // Act
        var options = new JsonSerializerSettings
        {
            Formatting = Formatting.None
        };
        options.Converters.Add(new AnyOfJsonConverter());

        var json = JsonConvert.SerializeObject(test, options);

        // Assert
        json.Should().Be("{\"IntOrString\":1}");
    }

    [Theory]
    [InlineData(NullValueHandling.Include, "{\"NullableIntOrString\":null}")]
    //[InlineData(NullValueHandling.Ignore, "{}")] --> fails !
    public void Serialize_AnyOf_With_TestNullableTypes_NullValueHandling(NullValueHandling nullValueHandling, string expected)
    {
        // Arrange
        var test = new TestNullableTypes
        {
            NullableIntOrString = (int?)null
        };

        // Act
        var serializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            NullValueHandling = nullValueHandling
        };
        serializerSettings.Converters.Add(new AnyOfJsonConverter());

        var json = JsonConvert.SerializeObject(test, serializerSettings);

        // Assert
        json.Should().Be(expected);
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
    public void Deserialize_AnyOf_With_SimpleTypes()
    {
        // Arrange
        var expected = new TestSimpleTypes
        {
            IntOrString = 1
        };

        // Act
        var options = new JsonSerializerSettings();
        options.Converters.Add(new AnyOfJsonConverter());

        var result = JsonConvert.DeserializeObject<TestSimpleTypes>("{\"IntOrString\":1}", options);

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
        var options = new JsonSerializerSettings();
        options.Converters.Add(new AnyOfJsonConverter());

        var result = JsonConvert.DeserializeObject<TestComplexTypes>("{\"AorB\":{\"Id\":1}}", options);

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

        var result = JsonConvert.DeserializeObject<TestComplexTypes2>("{\"AorB\":{\"Id\":1}}", options);

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
        var expected = new int[] { 42 };

        // Act
        var options = new JsonSerializerSettings();
        options.Converters.Add(new AnyOfJsonConverter());

        var result = JsonConvert.DeserializeObject<TestComplexArray>("{\"X\":[42]}", options);

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

        var result = JsonConvert.DeserializeObject<TestComplexArray>("{\"X\":[\"a\", \"b\"]}", options);

        // Assert
        result.X.Second.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Deserialize_AnyOf_With_StringOrStringList()
    {
        // Arrange
        var expected = new[] { "a", "b" };

        // Act
        var options = new JsonSerializerSettings();
        options.Converters.Add(new AnyOfJsonConverter());

        var json = "{ \"field\": [ \"a\", \"b\" ] }";
        var result = JsonConvert.DeserializeObject<TestStringOrStringArray>(json, options)!;

        // Assert
        result.Field.Second.Should().BeEquivalentTo(expected);
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

        var result = JsonConvert.DeserializeObject<TestComplexArray>("{\"X\":[{\"Id\":1},{\"Id\":2}]}", options);

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

        var result = JsonConvert.DeserializeObject<TestComplexArray>("{\"X\":[{\"Guid\":\"a\"},{\"Guid\":\"b\"}]}", options);

        // Assert
        result.X.Fourth.Should().BeEquivalentTo(expected);
    }
}
