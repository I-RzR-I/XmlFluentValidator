// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-09 22:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-09 22:00
// ***********************************************************************
//  <copyright file="XmlValidatorRuleBuilder.cs" company="RzR SOFT & TECH">
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using XmlFluentValidator.Abstractions;
using XmlFluentValidator.Enums;
using XmlFluentValidator.Helpers.Internal;
using XmlFluentValidator.Models;
using XmlFluentValidator.Models.Message;
using XmlFluentValidator.Models.Result;

// ReSharper disable UseCollectionExpression

#endregion

namespace XmlFluentValidator.Rules
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An XML validator rule builder.
    /// </summary>
    /// <seealso cref="T:XmlFluentValidator.Abstractions.IXmlValidatorRuleBuilder"/>
    ///
    /// ### <inheritdoc cref="IXmlValidatorRuleBuilder"/>
    /// =================================================================================================
    public class XmlValidatorRuleBuilder : IXmlValidatorRuleBuilder
    {
        private string _displayName;
        private bool _stopOnFailure;
        private bool _targetIsAttribute;
        private string _attributeName;
        private XmlMessageSeverity _severity = XmlMessageSeverity.Error;

        private readonly string _xpath;
        private readonly XmlValidator _validator;
        private readonly IList<Func<XDocument, IEnumerable<FailureMessageDescriptor>>>
            _steps = new List<Func<XDocument, IEnumerable<FailureMessageDescriptor>>>();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="XmlValidatorRuleBuilder"/> class.
        /// </summary>
        /// <param name="xpath">The xpath.</param>
        /// <param name="validator">The validator.</param>
        /// =================================================================================================
        public XmlValidatorRuleBuilder(string xpath, XmlValidator validator)
        {
            _xpath = xpath;
            _validator = validator;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="XmlValidatorRuleBuilder"/> class.
        /// </summary>
        /// <param name="xpath">The xpath.</param>
        /// <param name="validator">The validator.</param>
        /// <param name="targetIsAttribute">(Optional) True if target is attribute.</param>
        /// =================================================================================================
        public XmlValidatorRuleBuilder(string xpath, XmlValidator validator, bool targetIsAttribute = false)
        {
            _xpath = xpath;
            _validator = validator;
            _targetIsAttribute = targetIsAttribute;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder MustExist(string message = null)
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
        public IXmlValidatorRuleBuilder Count(Func<int, bool> predicate, string message = null)
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
        public IXmlValidatorRuleBuilder Value(Func<string, bool> predicate, string message = null)
        {
            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath).ToList();
                var fails = new List<FailureMessageDescriptor>();

                foreach (var e in elems)
                {
                    //var val = _targetIsAttribute ? e?.Value : e?.Value;
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
        public IXmlValidatorRuleBuilder Attribute(string name, Func<string, bool> predicate, string message = null)
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
                            MessageArguments.From((MessageArgs.Name, name), (MessageArgs.Path, path)), path);
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
        public IXmlValidatorRuleBuilder All(Func<XElement, bool> predicate, string message = null)
        {
            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<FailureMessageDescriptor>();
                foreach (var e in elems)
                {
                    if (predicate(e).IsFalse())
                    {
                        var path = GetElementPath(e);
                        var failure = BuildFailureMessage(message, DefaultMessageDescriptors.ElementPredicateFailed, path: path);

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
        public IXmlValidatorRuleBuilder Any(Func<XElement, bool> predicate, string message = null)
        {
            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath).ToList();
                var ok = elems.Any(predicate);

                return ok
                    ? Enumerable.Empty<FailureMessageDescriptor>()
                    : new[] { BuildFailureMessage(message, DefaultMessageDescriptors.NoElementPredicateFailed, path: _xpath) };
            });

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder When(Func<XDocument, bool> condition, string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.SetCondition(condition);

            if (message.IsPresent())
            {
                var newMessage = LegacyMessageAdapter.FromRaw(message: message, path: _xpath);
                rule.SetConditionMessage(LegacyMessageAdapter.FromRaw(message: message, path: _xpath), _severity);

                if (newMessage.IsNotNull())
                {
                    rule.RecordedSteps.Add(new XmlStepRecorder()
                    {
                        Kind = XmlValidationRuleKind.Condition,
                        Descriptor = newMessage
                    });
                }
            }

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder Optional(string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.ElementOptional,
                Descriptor = DefaultMessageDescriptors.ElementOptional
            });

            _steps.Add(doc => new List<FailureMessageDescriptor>());

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder Required(string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.ElementRequired,
                Descriptor = DefaultMessageDescriptors.ElementRequired
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
                                MessageArguments.From((MessageArgs.Element, e.Name.LocalName), (MessageArgs.Path, path)), path);
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
        public IXmlValidatorRuleBuilder MatchesRegex(string pattern, string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.ElementRegex,
                Pattern = pattern,
                Descriptor = DefaultMessageDescriptors.ValueMustMatchPatternFailed
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
                            MessageArguments.From((MessageArgs.Actual, val), (MessageArgs.Pattern, pattern), (MessageArgs.Path, path)), path);

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
        public IXmlValidatorRuleBuilder InRange(int min, int max, string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.ElementRangeInt,
                Min = min,
                Max = max,
                Descriptor = DefaultMessageDescriptors.ElementAttributeInRangeFailed
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<FailureMessageDescriptor>();
                foreach (var e in elems)
                {
                    if (int.TryParse(e.Value, out var n))
                    {
                        if (n < min || n > max)
                        {
                            var path = GetElementPath(e);
                            var failure = BuildFailureMessage(message, DefaultMessageDescriptors.ElementInRangeWithValue,
                                MessageArguments.From((MessageArgs.Value, n), (MessageArgs.Minimum, min), (MessageArgs.Maximum, max)), path);

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
        public IXmlValidatorRuleBuilder Unique(string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.ElementUnique,
                Descriptor = DefaultMessageDescriptors.ElementUniqueFailed
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
        public IXmlValidatorRuleBuilder RequiredAttribute(string name, string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.AttributeRequired,
                AttributeName = name,
                Descriptor = DefaultMessageDescriptors.AttributeMissing
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
                            MessageArguments.From((MessageArgs.Attribute, name), (MessageArgs.Path, path)), path);

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
        public IXmlValidatorRuleBuilder AttributeMatchesRegex(string name, string pattern, string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.AttributeRegex,
                AttributeName = name,
                Pattern = pattern,
                Descriptor = DefaultMessageDescriptors.ValueMustMatchPatternFailed
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
                            MessageArguments.From((MessageArgs.Name, name), (MessageArgs.Actual, attr.Value), (MessageArgs.Pattern, pattern), (MessageArgs.Path, path)), path);

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
        public IXmlValidatorRuleBuilder AttributeInRange(string name, int min, int max, string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.AttributeRangeInt,
                AttributeName = name,
                Min = min,
                Max = max,
                Descriptor = DefaultMessageDescriptors.ElementAttributeInRangeFailed
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
                                MessageArguments.From((MessageArgs.Name, name), (MessageArgs.Value, n), (MessageArgs.Minimum, min), (MessageArgs.Maximum, max)));

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
        public IXmlValidatorRuleBuilder AttributeUnique(string name, string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.AttributeUnique,
                AttributeName = name,
                Descriptor = DefaultMessageDescriptors.AttributeUniqueFailed
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath).ToList();
                var values = elems.Select(e => e.Attribute(name)?.Value).Where(v => v != null).ToList();
                var duplicates = values.GroupBy(v => v).Where(g => g.Count() > 1).Select(g => g.Key!).ToList();
                if (duplicates.Any())
                {
                    var failure = BuildFailureMessage(message, DefaultMessageDescriptors.AttributeUniqueDuplicateFailed,
                        MessageArguments.From((MessageArgs.Name, name), (MessageArgs.Path, _xpath), (MessageArgs.Duplicates, string.Join(", ", duplicates))), _xpath);

                    return new[] { failure };
                }

                return Enumerable.Empty<FailureMessageDescriptor>();
            });

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder ElementAttributeCrossRule(string attributeName, Func<string, string, bool> predicate, string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.ElementAttributeCross,
                AttributeName = attributeName,
                Descriptor = DefaultMessageDescriptors.CrossValidationFailed
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<FailureMessageDescriptor>();
                foreach (var e in elems)
                {
                    var attrVal = e.Attribute(attributeName)?.Value;
                    var elemVal = e.Value;
                    if (predicate(elemVal, attrVal).IsFalse())
                    {
                        var path = GetElementPath(e);
                        var failure = BuildFailureMessage(message, DefaultMessageDescriptors.CrossValidationWithDataFailed,
                            MessageArguments.From((MessageArgs.Name, e.Name), (MessageArgs.Attribute, path)), path);

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
        public IXmlValidatorRuleBuilder CustomElementRule(Func<XElement, IDictionary<string, string>, bool> predicate, string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.CustomElement,
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<FailureMessageDescriptor>();
                foreach (var e in elems)
                {
                    var attrs = e.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value);
                    if (predicate(e, attrs).IsFalse())
                    {
                        var path = GetElementPath(e);
                        var failure = BuildFailureMessage(message, DefaultMessageDescriptors.ElementCrossValidationFailed,
                            MessageArguments.From((MessageArgs.Name, e.Name)), path);

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
        public IXmlValidatorRuleBuilder Custom(Action<XmlValidationContext> handler, string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.CustomElement,
                Descriptor = DefaultMessageDescriptors.CustomValidationFailed
            });

            _steps.Add(doc =>
            {
                var fails = new List<XmlValidationFailureResult>();
                var ctx = new XmlValidationContext(doc, fails, null);

                handler(ctx);

                return Enumerable.Empty<FailureMessageDescriptor>();
            });

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder UseCustomRule(string ruleName, string message = null)
        {
            var predicate = CustomRuleRegistry.Get(ruleName);
            if (predicate.IsNull())
                throw new InvalidOperationException($"Custom rule '{ruleName}' not found.");

            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.CustomElement,
                Descriptor = DefaultMessageDescriptors.CustomValidationRuleFailed
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<FailureMessageDescriptor>();
                foreach (var e in elems)
                {
                    var attrs = e.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value);
                    if (predicate(e, attrs).IsFalse())
                    {
                        var path = GetElementPath(e);
                        var failure = BuildFailureMessage(message, DefaultMessageDescriptors.CustomValidationRuleWithDataFailed,
                            MessageArguments.From((MessageArgs.RuleName, ruleName), (MessageArgs.Name, e.Name)), path);

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
        public IXmlValidatorRuleBuilder MaxOccurs(int max, string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder
            {
                Kind = XmlValidationRuleKind.ElementMaxOccurs,
                MaxOccurs = max,
                Descriptor = DefaultMessageDescriptors.ElementMaxOccursFailed
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
        public IXmlValidatorRuleBuilder WithName(string displayName)
        {
            _displayName = displayName;

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder WithSeverity(XmlMessageSeverity severity)
        {
            _severity = severity;

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder StopOnFailure()
        {
            _stopOnFailure = true;

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder WithMessage(string template, MessageArguments arguments)
        {
            var rule = _validator.CurrentRule;
            var failure = XmlMessageFormatter.Format(template, arguments);

            rule.SetCustomMessage(LegacyMessageAdapter.FromRaw(failure));

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder WithMessage(string message)
        {
            var rule = _validator.CurrentRule;
            rule.SetCustomMessage(LegacyMessageAdapter.FromRaw(message: message, path: _xpath));

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder WithMessageForAll(string message)
        {
            var rule = _validator.CurrentRule;

            rule.SetDefaultMessage(LegacyMessageAdapter.FromRaw(message: message, path: _xpath));

            return this;
        }

        /// <inheritdoc />
        public XmlValidator Done() => _validator;

        #region INTERNAL

        internal IXmlValidatorRuleBuilder TargetAttribute(string name)
        {
            _targetIsAttribute = true;
            _attributeName = name;

            return this;
        }

        internal IXmlValidatorRule BuildEntryRule()
        {
            return new XmlValidatorCompositeRule(_xpath, _steps, _displayName);
        }

        internal XmlValidatorCompositeRule BuildEntryCompositeRule()
        {
            return new XmlValidatorCompositeRule(_xpath, _steps, _displayName);
        }

        #endregion

        #region PRIVATE

        private FailureMessageDescriptor BuildFailureMessage(string message, MessageDescriptor defaultDescriptor,
            MessageArguments arguments = null, string path = null)
        {
            var xpath = path.IsPresent() ? path : _xpath;
            var failureDescriptor = message.IsPresent()
                ? LegacyMessageAdapter.FromRaw(message, xpath, _severity)
                : defaultDescriptor;
            
            return new FailureMessageDescriptor()
            {
                Path = xpath,
                Name = _displayName,
                Descriptor = failureDescriptor,
                Arguments = arguments
            };
        }

        private static string GetElementPath(XElement e)
        {
            var segments = new List<string>();
            var current = e;

            while (current.IsNotNull())
            {
                var siblings = current!.Parent?.Elements(current.Name).ToList();
                var index = siblings.IsNull() ? 1 : siblings!.IndexOf(current) + 1; // 1-based for readability
                segments.Add($"{current.Name.LocalName}[{index}]");
                current = current.Parent;
            }
            segments.Reverse();

            return "/" + string.Join("/", segments);
        }

        #endregion
    }
}