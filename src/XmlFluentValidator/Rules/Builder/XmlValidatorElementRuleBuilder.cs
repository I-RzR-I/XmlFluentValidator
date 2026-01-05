// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-29 19:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-29 19:46
// ***********************************************************************
//  <copyright file="XmlValidatorElementRuleBuilder.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using DomainCommonExtensions.Utilities.Ensure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using XmlFluentValidator.Abstractions;
using XmlFluentValidator.Enums;
using XmlFluentValidator.Extensions;
using XmlFluentValidator.Helpers.Internal;
using XmlFluentValidator.Models;
using XmlFluentValidator.Models.Message;

// ReSharper disable UnusedParameter.Local
// ReSharper disable CheckNamespace
// ReSharper disable UseCollectionExpression

#endregion

namespace XmlFluentValidator.Rules
{
    public partial class XmlValidatorRuleBuilder
    {
        /// <inheritdoc />
        public IXmlValidatorRuleBuilder WithElementMustExist(
            string message = null)
        {
            _steps.Add(doc =>
            {
                var nodes = doc.XPathSelectElements(_xpath);
                var ok = _targetIsAttribute ? nodes.Any(e => e.HasAttributes) : nodes.Any();
                if (ok.IsFalse())
                {
                    var failure = BuildFailureMessage(message, DefaultMessageDescriptors.NodeRequired,
                        MessageArguments.From((MessageArgs.Path, _xpath)));

                    return new[] { failure };
                }

                return Enumerable.Empty<FailureMessageDescriptor>();
            });

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder WithElementCount(
            Func<int, bool> predicate,
            string message = null)
        {
            _steps.Add(doc =>
            {
                var count = doc.XPathSelectElements(_xpath).Count();
                if (predicate(count).IsFalse())
                {
                    var failure = BuildFailureMessage(message, DefaultMessageDescriptors.CountFailed,
                        MessageArguments.From((MessageArgs.Count, count)));

                    return new[] { failure };
                }

                return Enumerable.Empty<FailureMessageDescriptor>();
            });

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder WithElementValue(
            Func<string, bool> predicate, 
            string message = null)
        {
            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath).ToList();
                var fails = new List<FailureMessageDescriptor>();

                foreach (var e in elems)
                {
                    var val = e?.Value;
                    if (predicate(val.IfNullThenEmpty()).IsFalse())
                    {
                        var failure = BuildFailureMessage(message, DefaultMessageDescriptors.ValueFailed,
                            MessageArguments.From((MessageArgs.Value, val)));
                        fails.Add(failure);

                        if (_stopOnFailure)
                            break;
                    }
                }

                return fails;
            });

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder WithElementOptional(
            string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.ElementOptional,
                Descriptor = DefaultMessageDescriptors.ElementOptional,
                Path = _xpath
            });

            _steps.Add(doc => new List<FailureMessageDescriptor>());

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder WithElementRequired(
            string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.ElementRequired,
                Descriptor = DefaultMessageDescriptors.ElementRequired,
                Path = _xpath
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath).ToList();
                var fails = new List<FailureMessageDescriptor>();
                if (elems.IsNullOrEmptyEnumerable())
                {
                    var failure = BuildFailureMessage(message, DefaultMessageDescriptors.ElementMissing,
                        MessageArguments.From((MessageArgs.Path, _xpath)));
                    fails.Add(failure);
                }
                else
                {
                    foreach (var e in elems)
                    {
                        var val = e.Value;
                        if (val.IsMissing())
                        {
                            var path = GetElementPath(e);
                            var failure = BuildFailureMessage(message, DefaultMessageDescriptors.ElementRequired,
                                MessageArguments.From(
                                    (MessageArgs.Element, e.Name.LocalName), 
                                    (MessageArgs.Path, path)), path);
                            fails.Add(failure);

                            if (_stopOnFailure)
                                break;
                        }
                    }
                }

                return fails;
            });

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder WithElementMatchesRegex(
            string pattern, 
            string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.ElementRegex,
                Pattern = pattern,
                Descriptor = DefaultMessageDescriptors.ValueMustMatchPatternFailed,
                Path = _xpath
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<FailureMessageDescriptor>();
                foreach (var e in elems)
                {
                    var val = e.Value.IfNullThenEmpty();
                    if (Regex.IsMatch(val, pattern).IsFalse())
                    {
                        var path = GetElementPath(e);
                        var failure = BuildFailureMessage(message, DefaultMessageDescriptors.ValueMustMatchPattern,
                            MessageArguments.From(
                                (MessageArgs.Actual, val), 
                                (MessageArgs.Pattern, pattern), 
                                (MessageArgs.Path, path)), path);

                        fails.Add(failure);

                        if (_stopOnFailure)
                            break;
                    }
                }

                return fails;
            });

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder WithElementInRange(
            int min, 
            int max, 
            bool isInclusive = true,
            string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.ElementRangeInt,
                Min = min,
                Max = max,
                Descriptor = DefaultMessageDescriptors.ElementAttributeInRangeFailed,
                Path = _xpath,
                IsInclusiveValidation = isInclusive
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<FailureMessageDescriptor>();
                foreach (var e in elems)
                {
                    if (int.TryParse(e.Value, out var n))
                    {
                        if (n.IsInRange(min, max, isInclusive).IsFalse())
                        {
                            var path = GetElementPath(e);
                            var failure = BuildFailureMessage(message, DefaultMessageDescriptors.ElementInRangeWithValue,
                                MessageArguments.From(
                                    (MessageArgs.Value, n), 
                                    (MessageArgs.Minimum, min), 
                                    (MessageArgs.Maximum, max)), path);

                            fails.Add(failure);

                            if (_stopOnFailure)
                                break;
                        }
                    }
                    else
                    {
                        var path = GetElementPath(e);
                        var failure = BuildFailureMessage(message, DefaultMessageDescriptors.ValueIsNotInt,
                            MessageArguments.From((MessageArgs.Value, e.Value)), path);

                        fails.Add(failure);
                    }
                }

                return fails;
            });

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder WithElementUnique(
            string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.ElementUnique,
                Descriptor = DefaultMessageDescriptors.ElementUniqueFailed,
                Path = _xpath
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath).ToList();
                var values = elems.Select(e => e.Value).ToList();
                var duplicates = values.GroupBy(v => v).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
                if (duplicates.Any())
                {
                    var failure = BuildFailureMessage(message, DefaultMessageDescriptors.ElementUniqueDuplicateFailed,
                        MessageArguments.From((MessageArgs.Duplicates, string.Join(", ", duplicates))));

                    return new[] { failure };
                }

                return Enumerable.Empty<FailureMessageDescriptor>();
            });

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder WithElementMaxOccurs(
            int max, 
            string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder
            {
                Kind = XmlValidationRuleKind.ElementMaxOccurs,
                MaxOccurs = max,
                Descriptor = DefaultMessageDescriptors.ElementMaxOccursFailed,
                Path = _xpath
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<FailureMessageDescriptor>();
                if (elems.Count() > max)
                {
                    var failure = BuildFailureMessage(message, DefaultMessageDescriptors.ElementMaxOccursWithDataFailed,
                        MessageArguments.From((MessageArgs.Maximum, max)), _xpath);

                    fails.Add(failure);
                }

                return fails;
            });

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder WithElementValueLength(
            int min,
            int? max = null, 
            string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder
            {
                Kind = XmlValidationRuleKind.ElementValueLength,
                Min = min,
                Max = max,
                Descriptor = DefaultMessageDescriptors.ElementValueWithMinMaxLengthDataFailed,
                Path = _xpath
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<FailureMessageDescriptor>();
                foreach (var element in elems)
                {
                    var value = element?.Value.IfNullThenEmpty();
                    if (max.IsNull())
                    {
                        if (value!.Length < min)
                        {
                            var failure = BuildFailureMessage(message, DefaultMessageDescriptors.ElementValueWithMinLengthDataWithParamsFailed,
                                MessageArguments.From((MessageArgs.Minimum, min)), _xpath);

                            fails.Add(failure);
                        }
                    }
                    else
                    {
                        if (value!.Length.IsBetween(min, (int)max!).IsFalse())
                        {
                            var failure = BuildFailureMessage(message, DefaultMessageDescriptors.ElementValueWithMinMaxLengthDataWithParamsFailed,
                                MessageArguments.From(
                                    (MessageArgs.Minimum, min), 
                                    (MessageArgs.Maximum, max)), _xpath);

                            fails.Add(failure);
                        }
                    }
                }

                return fails;
            });

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder WithElementDataType(
            XmlValidationDataTypeKind dataType, 
            string message = null)
        {
            DomainEnsure.IsValidEnum<XmlValidationDataTypeKind>(dataType, nameof(dataType));

            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder
            {
                Kind = XmlValidationRuleKind.ElementDataType,
                Descriptor = DefaultMessageDescriptors.ElementDataTypeValidationFailed,
                Path = _xpath,
                DataType = dataType
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<FailureMessageDescriptor>();
                foreach (var element in elems)
                {
                    var value = element?.Value.IfNullThenEmpty();
                    var isValid = DataTypeConvertValidator.CanBeConverted(dataType, value);
                    if (isValid.IsFalse())
                    {
                        var failure = BuildFailureMessage(message, DefaultMessageDescriptors.ElementDataTypeWithParamsValidationFailed,
                            MessageArguments.From(
                                (MessageArgs.DataType, dataType.GetDescription()), 
                                (MessageArgs.Value, value)), _xpath);

                        fails.Add(failure);
                    }
                }

                return fails;
            });

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder WithElementEnumerator(
            string[] rangeEnumerator, 
            string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder
            {
                Kind = XmlValidationRuleKind.ElementEnumeration,
                Descriptor = DefaultMessageDescriptors.ElementInEnumValidationFailed,
                Path = _xpath,
                InRangeEnumerator = rangeEnumerator,
                AnnotationDescription = $"ENUM: {rangeEnumerator.NotNull().ListToString(",")}"
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<FailureMessageDescriptor>();
                foreach (var element in elems)
                {
                    var value = element?.Value.IfNullThenEmpty();
                    var isInRange = rangeEnumerator.IsInRangeStringValue(value);
                    if (isInRange.IsFalse())
                    {
                        var failure = BuildFailureMessage(message, DefaultMessageDescriptors.ElementInEnumWithValueValidationFailed,
                            MessageArguments.From(
                                (MessageArgs.Value, value)), 
                            _xpath);

                        fails.Add(failure);
                    }
                }

                return fails;
            });

            return this;
        }
    }
}