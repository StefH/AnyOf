﻿using System;
using System.Collections;

namespace AnyOfTypes.System.Text.Json.Models;

internal struct ListDetails
{
    public IList List { get; set; }

    public Type ListType { get; set; }
}