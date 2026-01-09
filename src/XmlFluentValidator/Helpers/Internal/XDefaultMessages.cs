// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2026-01-05 19:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-05 19:56
// ***********************************************************************
//  <copyright file="XDefaultMessages.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace XmlFluentValidator.Helpers.Internal
{
    internal static class XDefaultMessages
    {
        public const string UnsupportedRegexPattern = "Unsupported regex construct: ({0})!";
        public const string InvalidPath = "The path ({0}) is invalid!";
        public const string StepWithOutPath = "Step has no path!";
        public const string XmlValidationRuleKind = "The validation rule kind is out of range!";
        public const string RegexMissing = "Regex must not be empty!";
        public const string AttributeMissing = "Attribute is missing!";
        public const string AttributeNameMissing = "Attribute name is missing!";

        public const string ElementLengthApplyToNotString = "Length applied to non-string element ({0})!";
        public const string AttributeLengthApplyToNotString = "Length applied to non-string attribute ({0})!";

        public const string ElementLengthApplyToNotInteger = "Length applied to non-integer element ({0})!";
        public const string AttributeLengthApplyToNotInteger = "Length applied to non-integer attribute ({0})!";

        public const string ElementExactLengthApplyToNotString = "Exact length applied to non-string element ({0})!";
        public const string AttributeExactLengthApplyToNotString = "Exact length applied to non-string attribute ({0})!";

        public const string ElementRegexApplyToNotString = "Regex applied to non-string element ({0})!";
        public const string AttributeRegexApplyToNotString = "Regex applied to non-string attribute ({0})!";

        public const string ElementRangeApplyToNotInteger = "Integer range applied to non-integer element ({0})!";
        public const string AttributeRangeApplyToNotInteger = "Integer range applied to non-integer attribute ({0})!";

        public const string AttributeConflictWithPath = "Attribute ({0}) type conflict on ({1})";
        
        public const string EmitElementConflictHaveValueAndChildElement = "Element ({0}) cannot have both value and child elements!";
        public const string EmitElementConflictHaveFacetAndChildExist = "Element ({0}) cannot have facets when child elements exist!";
        public const string EmitInvalidComplexType = "Invalid complex type state for element ({0})!";

        public const string ElementCantBeNullableAndOptional = "Element ({0}) cannot be both nillable and optional!";
        public const string ElementCantBeNullableAndEnum = "Element ({0}) cannot be nillable when enumeration is defined!";
    }
}

