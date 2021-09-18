using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace AnyOfTypes.Newtonsoft.Json.Tests
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
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.None
            };
            options.Converters.Add(new AnyOfJsonConverter());

            var json = JsonConvert.SerializeObject(test, options);

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
    }
}