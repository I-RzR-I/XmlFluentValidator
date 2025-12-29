// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-29 19:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-29 19:46
// ***********************************************************************
//  <copyright file="XmlValidatorAttributeRuleBuilder.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

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
using XmlFluentValidator.Helpers.Internal;
using XmlFluentValidator.Models;
using XmlFluentValidator.Models.Message;

// ReSharper disable CheckNamespace
// ReSharper disable UseCollectionExpression

#endregion

namespace XmlFluentValidator.Rules
{
    public partial class XmlValidatorRuleBuilder
    {
        /// <inheritdoc />
        public IXmlValidatorRuleBuilder WithAttribute(
            string name, 
            Func<string, bool> predicate, 
            string message = null)
        {
            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<FailureMessageDescriptor>();
                foreach (var e in elems)
                {
                    var attr = e.Attribute(name);
                    var ok = predicate(attr?.Value);
                    if (ok.IsFalse())
                    {
                        var path = GetElementPath(e) + "/@" + name;
                        var failure = BuildFailureMessage(message, DefaultMessageDescriptors.AttributeInvalid,
                            MessageArguments.From(
                                (MessageArgs.Name, name), 
                                (MessageArgs.Path, path)), path);
                        fails.Add(failure);

                        if (_stopOnFailure.IsTrue())
                            break;
                    }
                }

                return fails;
            });

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder WithAttributeRequired(
            string name, 
            string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.AttributeRequired,
                AttributeName = name,
                Descriptor = DefaultMessageDescriptors.AttributeMissing,
                Path = _xpath
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<FailureMessageDescriptor>();
                foreach (var e in elems)
                {
                    var attr = e.Attribute(name);
                    if (attr.IsNull() || attr!.Value.IsMissing())
                    {
                        var path = GetElementPath(e) + "/@" + name;
                        var failure = BuildFailureMessage(message, DefaultMessageDescriptors.AttributeRequired,
                            MessageArguments.From(
                                (MessageArgs.Attribute, name), 
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
        public IXmlValidatorRuleBuilder WithAttributeMatchesRegex(
            string name, 
            string pattern, 
            string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.AttributeRegex,
                AttributeName = name,
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
                    var attr = e.Attribute(name);
                    if (attr.IsNotNull() && Regex.IsMatch(attr!.Value, pattern).IsFalse())
                    {
                        var path = GetElementPath(e) + "/@" + name;
                        var failure = BuildFailureMessage(message, DefaultMessageDescriptors.AttributeValueMustMatchPattern,
                            MessageArguments.From(
                                (MessageArgs.Name, name), 
                                (MessageArgs.Actual, attr.Value),
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
        public IXmlValidatorRuleBuilder WithAttributeInRange(
            string name, 
            int min, 
            int max, 
            string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.AttributeRangeInt,
                AttributeName = name,
                Min = min,
                Max = max,
                Descriptor = DefaultMessageDescriptors.ElementAttributeInRangeFailed,
                Path = _xpath
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<FailureMessageDescriptor>();
                foreach (var e in elems)
                {
                    var attr = e.Attribute(name);
                    if (attr.IsNotNull() && int.TryParse(attr!.Value, out var n))
                    {
                        if (n < min || n > max)
                        {
                            var failure = BuildFailureMessage(message, DefaultMessageDescriptors.AttributeInRangeWithValue,
                                MessageArguments.From(
                                    (MessageArgs.Name, name), 
                                    (MessageArgs.Value, n), 
                                    (MessageArgs.Minimum, min), 
                                    (MessageArgs.Maximum, max)));

                            fails.Add(failure);

                            if (_stopOnFailure)
                                break;
                        }
                    }
                    else
                    {
                        var path = GetElementPath(e) + "/@" + name;
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
        public IXmlValidatorRuleBuilder WithAttributeUnique(
            string name, 
            string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.AttributeUnique,
                AttributeName = name,
                Descriptor = DefaultMessageDescriptors.AttributeUniqueFailed,
                Path = _xpath
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath).ToList();
                var values = elems.Select(e => e.Attribute(name)?.Value).Where(v => v.IsNotNull()).ToList();
                var duplicates = values.GroupBy(v => v).Where(g => g.Count() > 1).Select(g => g.Key!).ToList();
                if (duplicates.Any())
                {
                    var failure = BuildFailureMessage(message, DefaultMessageDescriptors.AttributeUniqueDuplicateFailed,
                        MessageArguments.From(
                            (MessageArgs.Name, name),
                            (MessageArgs.Path, _xpath), 
                            (MessageArgs.Duplicates, string.Join(", ", duplicates))), _xpath);

                    return new[] { failure };
                }

                return Enumerable.Empty<FailureMessageDescriptor>();
            });

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder WithAttributeValueLength(
            string name,
            int min, 
            int? max = null, 
            string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder
            {
                Kind = XmlValidationRuleKind.AttributeValueLength,
                Min = min,
                Max = max,
                Descriptor = DefaultMessageDescriptors.AttributeValueWithMinMaxLengthDataFailed,
                Path = _xpath,
                AttributeName = name
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<FailureMessageDescriptor>();
                foreach (var element in elems)
                {
                    var attr = element.Attribute(name);
                    if (attr.IsNotNull() && attr!.Value.IsPresent())
                    {
                        if (max.IsNull())
                        {
                            if (attr!.Value!.Length < min)
                            {
                                var failure = BuildFailureMessage(message, DefaultMessageDescriptors.AttributeValueWithMinLengthDataWithParamsFailed,
                                    MessageArguments.From((MessageArgs.Minimum, min)), _xpath);

                                fails.Add(failure);
                            }
                        }
                        else
                        {
                            if (attr!.Value.Length.IsBetween(min, (int)max!).IsFalse())
                            {
                                var failure = BuildFailureMessage(message, DefaultMessageDescriptors.AttributeValueWithMinMaxLengthDataWithParamsFailed,
                                    MessageArguments.From(
                                        (MessageArgs.Minimum, min),
                                        (MessageArgs.Maximum, max)), _xpath);

                                fails.Add(failure);
                            }
                        }
                    }
                    else
                    {
                        var path = GetElementPath(element) + "/@" + name;
                        var failure = BuildFailureMessage(message, DefaultMessageDescriptors.AttributeValueWithMinMaxLengthDataFailed, path: path);

                        fails.Add(failure);
                    }
                }

                return fails;
            });

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder WithAttributeDataType(
            string name, 
            XmlValidationDataTypeKind dataType, 
            string message = null)
        {
            DomainEnsure.IsValidEnum<XmlValidationDataTypeKind>(dataType, nameof(dataType));

            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder
            {
                Kind = XmlValidationRuleKind.AttributeDataType,
                Descriptor = DefaultMessageDescriptors.AttributeDataTypeValidationFailed,
                Path = _xpath,
                AttributeName = name,
                DataType = dataType
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<FailureMessageDescriptor>();
                foreach (var element in elems)
                {
                    var attr = element.Attribute(name);
                    if (attr.IsNotNull() && attr!.Value.IsPresent())
                    {
                        var value = attr!.Value.IfNullThenEmpty();
                        var isValid = DataTypeConvertValidator.CanBeConverted(dataType, value);
                        if (isValid.IsFalse())
                        {
                            var failure = BuildFailureMessage(message, DefaultMessageDescriptors.AttributeDataTypeWithParamsValidationFailed,
                                MessageArguments.From(
                                    (MessageArgs.Attribute, name), 
                                    (MessageArgs.DataType, dataType.GetDescription()), 
                                    (MessageArgs.Value, value)), _xpath);

                            fails.Add(failure);
                        }
                    }
                    else
                    {
                        var path = GetElementPath(element) + "/@" + name;
                        var failure = BuildFailureMessage(message, DefaultMessageDescriptors.AttributeDataTypeValidationFailed, path: path);

                        fails.Add(failure);
                    }
                }

                return fails;
            });

            return this;
        }
    }
}