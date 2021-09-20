using AnyOfTypes.System.Text.Json.Matcher;

namespace AnyOf.System.Text.Json.Matcher.Models
{
    internal struct PropertyMap
    {
        public PropertyDetails SourceProperty { get; set; }

        public PropertyDetails TargetProperty { get; set; }
    }
}