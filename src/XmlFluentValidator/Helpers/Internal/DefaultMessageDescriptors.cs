// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-17 18:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-18 20:24
// ***********************************************************************
//  <copyright file="DefaultMessageDescriptors.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using XmlFluentValidator.Models.Message;

#endregion

namespace XmlFluentValidator.Helpers.Internal
{
    internal static class DefaultMessageDescriptors
    {
        public static readonly MessageDescriptor NodeRequired =
            new("NODE_REQUIRED", $"Required node not found at '{{{MessageArgs.Path}}}'");

        public static readonly MessageDescriptor AttributeRequired =
            new("ATTR_REQUIRED", $"Attribute '{{{MessageArgs.Attribute}}}' is required at '{{{MessageArgs.Path}}}'");

        public static readonly MessageDescriptor ElementRequired =
            new("ELEMENT_REQUIRED", $"Element '{{{MessageArgs.Element}}}' is required at '{{{MessageArgs.Path}}}'");

        public static readonly MessageDescriptor ValueMustMatchPattern =
            new("VALUE_PATTERN",
                $"Value '{{{MessageArgs.Actual}}}' must match pattern '{{{MessageArgs.Pattern}}}' at '{{{MessageArgs.Path}}}'");

        public static readonly MessageDescriptor AttributeValueMustMatchPattern =
            new("VALUE_PATTERN",
                $"Attribute '{{{MessageArgs.Name}}}' value '{{{MessageArgs.Value}}}' does not match regex '{{{MessageArgs.Pattern}}}'.");

        public static readonly MessageDescriptor ValueMustMatchPatternFailed =
            new("VALUE_PATTERN_FAILED", "Value must match pattern failed.");

        public static readonly MessageDescriptor ValueMustNotBeEmpty =
            new("VALUE_NOT_EMPTY", $"Value must not be empty at '{{{MessageArgs.Path}}}'");

        public static readonly MessageDescriptor CustomRuleFailed =
            new("CUSTOM_FAILED", $"Validation failed: {{{MessageArgs.Reason}}} at '{{{MessageArgs.Path}}}'");

        public static readonly MessageDescriptor ConditionFailed =
            new("CONDITION_FAILED", $"Validation failed: {{{MessageArgs.Reason}}} at '{{{MessageArgs.Path}}}'");

        public static readonly MessageDescriptor GenericFailure =
            new("GENERIC_FAILED", $"Validation failed: {{{MessageArgs.Reason}}} at '{{{MessageArgs.Path}}}'");

        public static readonly MessageDescriptor ElementMissing =
            new("ELEMENT_MISSING", $"Required element is missing at '{{{MessageArgs.Path}}}'.");

        public static readonly MessageDescriptor AttributeMissing =
            new("ATTRIBUTE_MISSING", "Required element is missing.");

        public static readonly MessageDescriptor AttributeMissingWithPath =
            new("ATTRIBUTE_MISSING", $"Required element is missing at '{{{MessageArgs.Path}}}'.");

        public static readonly MessageDescriptor ValueEmpty =
            new("VALUE_EMPTY",
                $"Required value for element '{{{MessageArgs.Element}}}' is empty at '{{{MessageArgs.Path}}}'.");

        public static readonly MessageDescriptor CountFailed =
            new("COUNT_FAILED", $"Count predicate failed (count='{{{MessageArgs.Count}}}').");

        public static readonly MessageDescriptor ValueFailed =
            new("VALUE_FAILED", $"Count predicate failed (value='{{{MessageArgs.Value}}}').");

        public static readonly MessageDescriptor AttributeInvalid =
            new("ATTRIBUTE_INVALID", $"Attribute '{{{MessageArgs.Name}}}' invalid at '{{{MessageArgs.Path}}}'.");

        public static readonly MessageDescriptor ElementPredicateFailed =
            new("ELEMENT_PREDICATE_FAILED", "Element failed predicate.");

        public static readonly MessageDescriptor NoElementPredicateFailed =
            new("NO_ELEMENT_PREDICATE_FAILED", "No element satisfied predicate");

        public static readonly MessageDescriptor ElementOptional =
            new("ELEMENT_OPTIONAL", "Element is optional");

        public static readonly MessageDescriptor ElementInRangeWithValue =
            new("ELEMENT_IN_RANGE_VALUE",
                $"Element value '{{{MessageArgs.Value}}}' must be between '{{{MessageArgs.Minimum}}}' and '{{{MessageArgs.Maximum}}}'.");

        public static readonly MessageDescriptor AttributeInRangeWithValue =
            new("ATTRIBUTE_IN_RANGE_VALUE",
                $"Attribute '{{{MessageArgs.Name}}}' with value '{{{MessageArgs.Value}}}' must be between '{{{MessageArgs.Minimum}}}' and '{{{MessageArgs.Maximum}}}'.");

        public static readonly MessageDescriptor ElementAttributeInRangeFailed =
            new("ELEMENT_ATTRIBUTE_IN_RANGE", "Value must be between in range.");

        public static readonly MessageDescriptor ValueIsNotInt =
            new("VALUE_NOT_INT", "Value '{Value}' is not a valid integer.");

        public static readonly MessageDescriptor ElementUniqueDuplicateFailed =
            new("ELEMENT_UNIQUE_FAILED", $"Duplicate values found: '{{{MessageArgs.Duplicates}}}'");

        public static readonly MessageDescriptor AttributeUniqueDuplicateFailed =
            new("ATTRIBUTE_UNIQUE_FAILED",
                $"Duplicate attribute '{{{MessageArgs.Name}}}' values found: '{{{MessageArgs.Duplicates}}}'");

        public static readonly MessageDescriptor ElementUniqueFailed =
            new("ELEMENT_UNIQUE_DUPLICATE_FAILED", "Element is not unique.");

        public static readonly MessageDescriptor AttributeUniqueFailed =
            new("ATTRIBUTE_UNIQUE_DUPLICATE_FAILED", "Element is not unique.");

        public static readonly MessageDescriptor CrossValidationFailed =
            new("CROSS_VALIDATION_FAILED", "Cross validation failed.");

        public static readonly MessageDescriptor CrossValidationWithDataFailed =
            new("CROSS_VALIDATION_WITH_DATA_FAILED",
                $"Cross validation failed between element '{{{MessageArgs.Name}}}' and attribute '{{{MessageArgs.Attribute}}}'.");

        public static readonly MessageDescriptor ElementCrossValidationFailed =
            new("ELEMENT_VALIDATION_FAILED", $"Custom validation failed for element '{{{MessageArgs.Name}}}'.");

        public static readonly MessageDescriptor CustomValidationFailed =
            new("CUSTOM_VALIDATION_FAILED", "Custom validation failed.");

        public static readonly MessageDescriptor CustomValidationRuleFailed =
            new("CUSTOM_VALIDATION_RULE_FAILED", "Custom validation rule failed.");

        public static readonly MessageDescriptor CustomValidationRuleWithDataFailed =
            new("CUSTOM_VALIDATION_RULE_WITH_DATA_FAILED",
                $"Custom rule '{{{MessageArgs.RuleName}}}' failed for element '{{{MessageArgs.Name}}}'.");

        public static readonly MessageDescriptor ElementMaxOccursFailed =
            new("ELEMENT_MAX_OCCURS_FAILED", "Element must not occur more than X times.");

        public static readonly MessageDescriptor ElementMaxOccursWithDataFailed =
            new("ELEMENT_MAX_OCCURS_WITH_DATA_FAILED",
                $"Element must not occur more than {{{MessageArgs.Maximum}}} times.");
    }
}