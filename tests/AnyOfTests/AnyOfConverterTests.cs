using System;
using System.ComponentModel;
using AnyOfTypes;
using FluentAssertions;
using Moq;
using Xunit;

namespace AnyOfTests;

public class AnyOfConverterTests
{
    private readonly ITypeDescriptorContext _typeDescriptorContext = Mock.Of<ITypeDescriptorContext>();

    [Theory]
    [InlineData(typeof(string), true)]
    [InlineData(typeof(int), true)]
    [InlineData(typeof(AnyOf<string, int>), true)]
    [InlineData(typeof(bool), false)]
    [InlineData(typeof(double), false)]
    [InlineData(typeof(Tuple<string, int>), false)]
    [InlineData(typeof(AnyOf<string, double>), false)]
    public void CanConvertFrom_ShouldReturnExpectedValue(Type sourceType, bool expectedValue)
    {
        // Arrange
        var converter = new AnyOfConverter<string, int>();

        // Act
        bool result = converter.CanConvertFrom(_typeDescriptorContext, sourceType);

        // Assert
        result.Should().Be(expectedValue);
    }

    [Fact]
    public void ConvertFrom_Null_ReturnsNull()
    {
        // Arrange
        var converter = new AnyOfConverter<string, int>();

        // Act
        var actual = converter.ConvertFrom(_typeDescriptorContext, null!, null);

        // Assert
        actual.Should().BeNull();
    }

    [Theory]
    [InlineData(42, 42)]
    [InlineData("foo", "foo")]
    public void ConvertFrom_ShouldReturnExpectedValue(object value, object expectedValue)
    {
        // Arrange
        var converter = new AnyOfConverter<string, int>();

        // Act
        var result = (AnyOf<string, int>)converter.ConvertFrom(_typeDescriptorContext, null!, value)!;

        // Assert
        result.CurrentValue.Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(42, 42)]
    [InlineData("foo", "foo")]
    public void ConvertFrom_UsingTypeDescriptor_ShouldReturnExpectedValue(object value, object expectedValue)
    {
        // Arrange
        var converter = TypeDescriptor.GetConverter(typeof(AnyOfConverter<string, int>));

        // Act
        var result = (AnyOf<string, int>)converter.ConvertFrom(_typeDescriptorContext, null!, value)!;

        // Assert
        result.CurrentValue.Should().Be(expectedValue);
    }

    [Fact]
    public void ConvertFrom_AnyOf_ShouldReturnExpectedValue()
    {
        // Arrange
        var converter = new AnyOfConverter<string, int>();
        var value = new AnyOf<string, int>("foo");

        // Act
        var result = (AnyOf<string, int>)converter.ConvertFrom(_typeDescriptorContext, null!, value);

        // Assert
        result.CurrentValue.Should().Be("foo");
    }

    [Theory]
    [InlineData(typeof(string), true)]
    [InlineData(typeof(int), true)]
    [InlineData(typeof(object), false)]
    [InlineData(typeof(bool), false)]
    [InlineData(typeof(double), false)]
    public void CanConvertTo_ShouldReturnTrue_WhenDestinationTypeCanBeConverted(Type destinationType, bool expectedResult)
    {
        // Arrange
        var converter = new AnyOfConverter<string, int>();

        // Act
        var result = converter.CanConvertTo(_typeDescriptorContext, destinationType);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void ConvertTo_Null_ReturnsNull()
    {
        // Arrange
        var converter = new AnyOfConverter<string, int>();

        // Act
        var actual = converter.ConvertTo(_typeDescriptorContext, null, null, typeof(string));

        // Assert
        actual.Should().BeNull();
    }

    [Theory]
    [InlineData("foo", "foo")]
    [InlineData(42, 42)]
    public void ConvertTo_ShouldConvertValueToAnyOf(object value, object expectedValue)
    {
        // Arrange
        var converter = new AnyOfConverter<string, int>();

        // Act
        var result = converter.ConvertTo(_typeDescriptorContext, null, value, typeof(AnyOf<string, int>));

        // Assert
        result.Should().Be(expectedValue);
    }

    [Fact]
    public void ConvertTo_ShouldConvertAnyOfToAnyOf()
    {
        // Arrange
        var converter = new AnyOfConverter<string, int>();
        var value = new AnyOf<string, int>(42);

        // Act
        var result = (AnyOf<string, int>)converter.ConvertTo(_typeDescriptorContext, null, value, typeof(AnyOf<string, int>));

        // Assert
        result.Second.Should().Be(42);
    }
}