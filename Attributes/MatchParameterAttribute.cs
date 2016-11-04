﻿using System;

namespace Attributes
{
    // Should be applied to .ctors.
    // Matches a .ctor parameter with property. Needs to get a default value from property field, and use this value for calling .ctor.
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = true)]
    public class MatchParameterWithPropertyAttribute : Attribute
    {
        public string Parameter { get; set; }
        public string Field { get; set; }

        public MatchParameterWithPropertyAttribute(string parameter, string field)
        {
            Parameter = parameter;
            Field = field;
        }
    }
}
