using AnyOfTypes;
using FluentAssertions;
using Xunit;

namespace AnyOfTests
{
    public class AnyOfTest
    {
        [Fact]
        public void AnyOf_Equals_Method()
        {
            // Arrange
            var anyOfIntAndStringValue1 = new AnyOf<int, string>(42);
            var anyOfIntAndStringValue2 = new AnyOf<int, string>(42);
            var anyOfIntAndStringValue3 = new AnyOf<int, string>(5);
            var anyOfIntAndBoolValue = new AnyOf<int, bool>(42);
            var normalInt = 42;

            // Assert
            anyOfIntAndStringValue1.Equals(anyOfIntAndStringValue2).Should().BeTrue();
            anyOfIntAndStringValue1.Equals(anyOfIntAndStringValue3).Should().BeFalse();
            anyOfIntAndStringValue1.Equals(anyOfIntAndBoolValue).Should().BeFalse();
            anyOfIntAndStringValue1.Equals(normalInt).Should().BeFalse();
        }

        [Fact]
        public void AnyOf_Equals_Operator()
        {
            // Arrange
            var anyOfIntAndStringValue1 = new AnyOf<int, string>(42);
            var anyOfIntAndStringValue2 = new AnyOf<int, string>(42);
            var anyOfIntAndStringValue3 = new AnyOf<int, string>(5);
            var anyOfIntAndStringValue4 = new AnyOf<int, string>("x");
            var anyOfIntAndBoolValue = new AnyOf<int, bool>(42);
            var normalInt = 42;
            var normalString = "x";

            // Assert
            (anyOfIntAndStringValue1 == anyOfIntAndStringValue2).Should().BeTrue();
            (anyOfIntAndStringValue1 == anyOfIntAndStringValue3).Should().BeFalse();
            (anyOfIntAndStringValue1 == anyOfIntAndStringValue4).Should().BeFalse();
            (anyOfIntAndStringValue1 == anyOfIntAndBoolValue).Should().BeFalse();
            (anyOfIntAndStringValue1 == normalInt).Should().BeFalse();
            (anyOfIntAndStringValue1 == normalString).Should().BeFalse();
            (normalInt == anyOfIntAndStringValue1).Should().BeTrue();
            (normalString == anyOfIntAndStringValue4).Should().BeTrue();
        }
    }
}
