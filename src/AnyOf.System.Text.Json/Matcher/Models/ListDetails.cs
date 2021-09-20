using System;
using System.Collections;

namespace AnyOf.System.Text.Json.Matcher.Models
{
    internal struct ListDetails
    {
        public IList List { get; set; }

        public Type ListType { get; set; }
    }
}
