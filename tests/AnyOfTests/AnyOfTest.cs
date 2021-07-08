using System;
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
            var anyOfIntAndStringTypeWithIntValue1 = new AnyOf<int, string>(42);
            var anyOfIntAndStringTypeWithIntValue2 = new AnyOf<int, string>(42);
            var anyOfIntAndStringTypeWithIntValue3 = new AnyOf<int, string>(5);
            var anyOfIntAndStringTypeWithStringValue = new AnyOf<int, string>("x");
            var anyOfIntAndBoolTypeWithIntValue = new AnyOf<int, bool>(42);
            var anyOfIntAndBoolTypeWithBoolValue = new AnyOf<int, bool>(true);
            var normalBool = true;
            var normalInt = 42;
            var normalString = "x";

            // Assert
            (anyOfIntAndStringTypeWithIntValue1 == anyOfIntAndStringTypeWithIntValue2).Should().BeTrue();
            (anyOfIntAndStringTypeWithIntValue1 == anyOfIntAndBoolTypeWithIntValue).Should().BeTrue();
            
            (anyOfIntAndStringTypeWithIntValue1 == normalInt).Should().BeTrue();
            (anyOfIntAndStringTypeWithIntValue1 == normalString).Should().BeFalse();
            (anyOfIntAndBoolTypeWithBoolValue == normalBool).Should().BeTrue();
            (anyOfIntAndStringTypeWithStringValue == normalString).Should().BeTrue();
            
            (anyOfIntAndStringTypeWithIntValue1 == anyOfIntAndStringTypeWithIntValue3).Should().BeFalse();
            (anyOfIntAndStringTypeWithIntValue1 == anyOfIntAndBoolTypeWithIntValue).Should().BeTrue();
            (anyOfIntAndStringTypeWithIntValue1 == anyOfIntAndStringTypeWithStringValue).Should().BeFalse();
        }

        [Fact]
        public void AnyOf_Equals_Operator_Throws_Exception()
        {
            // Arrange
            var anyOfIntAndStringTypeWithIntValue1 = new AnyOf<int, string>(42);
            var anyOfIntAndBoolTypeWithBoolValue = new AnyOf<int, bool>(true);

            // Assert
            Func<bool> a = () => anyOfIntAndStringTypeWithIntValue1 == anyOfIntAndBoolTypeWithBoolValue;
            a.Should().Throw<Exception>();
        }
    }
}