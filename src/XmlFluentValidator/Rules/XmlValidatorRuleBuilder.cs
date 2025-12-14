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
using XmlFluentValidator.Models;
using XmlFluentValidator.Models.Result;

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
        private string _attributeName;
        private bool _targetIsAttribute;
        private Func<object, string> _messageFormatter;
        private string _message;
        private XmlMessageSeverity _severity = XmlMessageSeverity.Error;

        private readonly string _xpath;
        private readonly XmlValidator _validator;
        private readonly IList<Func<XDocument, IEnumerable<XmlValidationFailureResult>>>
            _steps = new List<Func<XDocument, IEnumerable<XmlValidationFailureResult>>>();
        
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
                    var failMessage = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace("Required node not found."));

                    return new[]
                    {
                        Failure(failMessage, path: _xpath)
                    };
                }

                return Enumerable.Empty<XmlValidationFailureResult>();
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
                    var failMessage = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace($"Count predicate failed (count={count})."));
                    
                    return new[]
                    {
                        Failure(failMessage, _xpath)
                    };
                }

                return Enumerable.Empty<XmlValidationFailureResult>();
            });

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder Value(Func<string, bool> predicate, string message = null)
        {
            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath).ToList();
                var fails = new List<XmlValidationFailureResult>();

                foreach (var e in elems)
                {
                    //var val = _targetIsAttribute ? e?.Value : e?.Value;
                    var val = e?.Value;
                    if (predicate(val.IfNullThenEmpty()).IsFalse())
                    {
                        var failMessage = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace("Invalid value."));
                        fails.Add(Failure(failMessage, GetElementPath(e)));

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
                var fails = new List<XmlValidationFailureResult>();
                foreach (var e in elems)
                {
                    var attr = e.Attribute(name);
                    var ok = predicate(attr?.Value);
                    if (ok.IsFalse())
                    {
                        var failMessage = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace($"Attribute '{name}' invalid."));
                        fails.Add(Failure(failMessage, GetElementPath(e) + "/@" + name));

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
                var fails = new List<XmlValidationFailureResult>();
                foreach (var e in elems)
                {
                    if (predicate(e).IsFalse())
                    {
                        var failMessage = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace("Element failed predicate."));
                        fails.Add(Failure(failMessage, GetElementPath(e)));

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
                    ? Enumerable.Empty<XmlValidationFailureResult>()
                    : new[] { Failure(message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace("No element satisfied predicate.")), _xpath) };
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
                var newMessage = FailureMessage(message, _xpath);
                rule.SetConditionMessage(newMessage, _severity);

                if (newMessage.IsPresent())
                {
                    rule.RecordedSteps.Add(new XmlStepRecorder()
                    {
                        Kind = XmlValidationRuleKind.Condition,
                        Message = newMessage
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
                Message = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace(FailureMessage("Element is optional", _xpath)))
            });

            _steps.Add(doc => new List<XmlValidationFailureResult>());

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder Required(string message = null)
        {
            var rule = _validator.CurrentRule;
            rule.RecordedSteps.Add(new XmlStepRecorder()
            {
                Kind = XmlValidationRuleKind.ElementRequired,
                Message = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace(FailureMessage(message, _xpath)))
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath).ToList();
                var fails = new List<XmlValidationFailureResult>();
                if (elems.IsNullOrEmptyEnumerable())
                {
                    var failMessage = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace("Required element is missing."));
                    fails.Add(Failure(failMessage, _xpath));
                }
                else
                {
                    foreach (var e in elems)
                    {
                        var val = e.Value;
                        if (val.IsMissing())
                        {
                            var failMessage = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace("Required value is empty."));
                            fails.Add(Failure(failMessage, GetElementPath(e)));

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
                Message = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace(FailureMessage(message, _xpath)))
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<XmlValidationFailureResult>();
                foreach (var e in elems)
                {
                    var val = e.Value.IfNullThenEmpty();
                    if (Regex.IsMatch(val, pattern).IsFalse())
                    {
                        var failMessage = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace($"Value '{val}' does not match regex '{pattern}'."));
                        //fails.Add(Failure(message.IfNullOrWhiteSpace($"Value '{val}' does not match regex '{pattern}'."), GetElementPath(e)));
                        fails.Add(Failure(failMessage, GetElementPath(e)));

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
                Message = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace(FailureMessage(message, _xpath)))
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<XmlValidationFailureResult>();
                foreach (var e in elems)
                {
                    if (int.TryParse(e.Value, out var n))
                    {
                        if (n < min || n > max)
                        {
                            var failMessage = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace($"Value {n} must be between {min} and {max}."));
                            fails.Add(Failure(failMessage, GetElementPath(e)));

                            if (_stopOnFailure)
                                break;
                        }
                    }
                    else
                    {
                        var failMessage = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace($"Value '{e.Value}' is not a valid integer."));
                        fails.Add(Failure(failMessage, GetElementPath(e)));
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
                Message = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace(FailureMessage(message, _xpath)))
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath).ToList();
                var values = elems.Select(e => e.Value).ToList();
                var duplicates = values.GroupBy(v => v).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
                if (duplicates.Any())
                {
                    var failMessage = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace($"Duplicate values found: {string.Join(", ", duplicates)}"));

                    return new[]
                    {
                        Failure(failMessage, _xpath)
                    };
                }

                return Enumerable.Empty<XmlValidationFailureResult>();
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
                Message = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace(FailureMessage(message, _xpath)))
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<XmlValidationFailureResult>();
                foreach (var e in elems)
                {
                    var attr = e.Attribute(name);
                    if (attr.IsNull() || attr!.Value.IsMissing())
                    {
                        var failMessage = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace($"Attribute '{name}' is required."));
                        fails.Add(Failure(failMessage, GetElementPath(e) + "/@" + name));

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
                Message = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace(FailureMessage(message, _xpath)))
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<XmlValidationFailureResult>();
                foreach (var e in elems)
                {
                    var attr = e.Attribute(name);
                    if (attr.IsNotNull() && Regex.IsMatch(attr!.Value, pattern).IsFalse())
                    {
                        var failMessage = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace($"Attribute '{name}' value '{attr.Value}' does not match regex '{pattern}'."));
                        fails.Add(Failure(failMessage, GetElementPath(e) + "/@" + name));

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
                Message = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace(FailureMessage(message, _xpath)))
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<XmlValidationFailureResult>();
                foreach (var e in elems)
                {
                    var attr = e.Attribute(name);
                    if (attr.IsNotNull() && int.TryParse(attr!.Value, out var n))
                    {
                        if (n < min || n > max)
                        {
                            var failMessage = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace("Attribute '{name}' must be between {min} and {max}."));
                            fails.Add(Failure(failMessage, GetElementPath(e) + "/@" + name));

                            if (_stopOnFailure)
                                break;
                        }
                    }
                    else
                    {
                        var failMessage = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace($"Attribute '{name}' is not a valid integer."));
                        fails.Add(Failure(failMessage, GetElementPath(e) + "/@" + name));
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
                Message = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace(FailureMessage(message, _xpath)))
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath).ToList();
                var values = elems.Select(e => e.Attribute(name)?.Value).Where(v => v != null).ToList();
                var duplicates = values.GroupBy(v => v).Where(g => g.Count() > 1).Select(g => g.Key!).ToList();
                if (duplicates.Any())
                {
                    var failMessage = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace($"Duplicate attribute '{name}' values found: {string.Join(", ", duplicates)}"));

                    return new[]
                    {
                        Failure(failMessage, _xpath)
                    };
                }

                return Enumerable.Empty<XmlValidationFailureResult>();
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
                Message = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace(FailureMessage(message, _xpath)))
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<XmlValidationFailureResult>();
                foreach (var e in elems)
                {
                    var attrVal = e.Attribute(attributeName)?.Value;
                    var elemVal = e.Value;
                    if (predicate(elemVal, attrVal).IsFalse())
                    {
                        var failMessage = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace($"Cross validation failed between element '{e.Name}' and attribute '{attributeName}'."));
                        fails.Add(Failure(failMessage, GetElementPath(e)));

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
                Message = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace(FailureMessage(message, _xpath)))
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<XmlValidationFailureResult>();
                foreach (var e in elems)
                {
                    var attrs = e.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value);
                    if (predicate(e, attrs).IsFalse())
                    {
                        var failMessage = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace($"Custom validation failed for element '{e.Name}'."));
                        fails.Add(Failure(failMessage, GetElementPath(e)));

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
                Message = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace(FailureMessage(message, _xpath)))
            });

            _steps.Add(doc =>
            {
                var fails = new List<XmlValidationFailureResult>();
                var ctx = new XmlValidationContext(doc, fails);

                handler(ctx);

                return fails;
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
                Message = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace(FailureMessage(message, _xpath)))
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<XmlValidationFailureResult>();
                foreach (var e in elems)
                {
                    var attrs = e.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value);
                    if (predicate(e, attrs).IsFalse())
                    {
                        var failMessage = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace($"Custom rule '{ruleName}' failed for element '{e.Name}'."));
                        fails.Add(Failure(failMessage, GetElementPath(e)));

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
                Message = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace(FailureMessage(message, _xpath)))
            });

            _steps.Add(doc =>
            {
                var elems = doc.XPathSelectElements(_xpath);
                var fails = new List<XmlValidationFailureResult>();
                if (elems.Count() > max)
                {
                    var failMessage = message.IfNullOrWhiteSpace(_message.IfNullOrWhiteSpace($"Element must not occur more than {max} times."));
                    fails.Add(Failure(failMessage, _xpath));
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
        public IXmlValidatorRuleBuilder WithMessage(string template, object context)
        {
            _messageFormatter = (_) => XmlValidationMessageResult.Format(template, context);

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder WithMessage(string message)
        {
            _message = message.IfNullOrWhiteSpace(FailureMessage(message, _xpath));
            var rule = _validator.CurrentRule;
            rule.SetCustomMessage(_message);

            return this;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder WithMessageForAll(string message)
        {
            _message = message.IfNullOrWhiteSpace(FailureMessage(message, _xpath));
            var rule = _validator.CurrentRule;
            rule.SetDefaultMessage(_message);

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

        private XmlValidationFailureResult Failure(string message, string path)
        {
            var finalMessage = FailureMessage(message, path);

            return new XmlValidationFailureResult()
            {
                Severity = _severity,
                Path = path,
                Message = finalMessage,
                Name = _displayName
            };
        }

        private string FailureMessage(string message, string path = null)
        {
            var finalMessage = message.IfNullOrWhiteSpace($"Validation failed {(path.IsPresent() ? $"at '{path}'" : "")}.");
            if (_messageFormatter.IsNotNull())
            {
                finalMessage = _messageFormatter(new
                {
                    Path = path,
                    Raw = finalMessage,
                    Name = _displayName
                });
            }

            return finalMessage;
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