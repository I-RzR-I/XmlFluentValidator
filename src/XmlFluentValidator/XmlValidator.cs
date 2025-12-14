// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-09 21:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-09 23:39
// ***********************************************************************
//  <copyright file="XmlValidator.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using XmlFluentValidator.Abstractions;
using XmlFluentValidator.Enums;
using XmlFluentValidator.Models.Result;
using XmlFluentValidator.Rules;

#endregion

namespace XmlFluentValidator
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An XML validator.
    /// </summary>
    /// <seealso cref="T:XmlFluentValidator.Abstractions.IXmlValidator"/>
    /// =================================================================================================
    public sealed class XmlValidator : IXmlValidator
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) name of the root element.
        /// </summary>
        /// =================================================================================================
        private readonly string _rootElementName;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the rules.
        /// </summary>
        /// =================================================================================================
        private readonly IList<IXmlValidatorRule> _rules = new List<IXmlValidatorRule>();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Set the schema belongs to.
        /// </summary>
        /// =================================================================================================
        private XmlSchemaSet _schemaSet;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     True to stop on schema errors.
        /// </summary>
        /// =================================================================================================
        private bool _stopOnSchemaErrors;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The current rule.
        /// </summary>
        /// =================================================================================================
        private XmlValidatorCompositeRule _currentRule;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the rules.
        /// </summary>
        /// <value>
        ///     The rules.
        ///      Expose rules for XSD generator to inspect
        /// </value>
        /// =================================================================================================
        internal IEnumerable<IXmlValidatorRule> Rules => _rules;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the current rule.
        /// </summary>
        /// <value>
        ///     The current rule.
        /// </value>
        /// =================================================================================================
        internal XmlValidatorCompositeRule CurrentRule => _currentRule!;
        
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="XmlValidator"/> class.
        /// </summary>
        /// =================================================================================================
        public XmlValidator() { }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="XmlValidator"/> class.
        /// </summary>
        /// <param name="rootElementName">Name of the root element.</param>
        /// =================================================================================================
        public XmlValidator(string rootElementName)
        {
            _rootElementName = rootElementName;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder ForPath(string xpath)
        {
            var rb = new XmlValidatorRuleBuilder(xpath, this);
            _currentRule = rb.BuildEntryCompositeRule();
            _rules.Add(_currentRule);

            return rb;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder ForAttribute(string xpath)
        {
            var rb = new XmlValidatorRuleBuilder(xpath, this, true);
            _currentRule = rb.BuildEntryCompositeRule();
            _rules.Add(_currentRule);

            return rb;
        }

        /// <inheritdoc />
        public IXmlValidatorRuleBuilder ForElement(string elementPath)
        {
            if (string.IsNullOrWhiteSpace(_rootElementName))
                throw new InvalidOperationException("Root element name must be set in XmlValidator.");

            // Build XPath relative to root
            var xpath = $"/{_rootElementName}/{elementPath}";

            // Detect attribute syntax
            string attributeName = null;
            if (elementPath.Contains("@"))
            {
                var parts = elementPath.Split('@');
                elementPath = parts[0];
                attributeName = parts[1];
                xpath = $"/{_rootElementName}/{elementPath}";
            }

            var rb = new XmlValidatorRuleBuilder(xpath, this);

            // If attribute specified, pre‑configure builder to target attribute
            if (attributeName.IsPresent())
            {
                rb.TargetAttribute(attributeName);
            }

            _currentRule = rb.BuildEntryCompositeRule();
            _rules.Add(_currentRule);

            return rb;
        }

        /// <inheritdoc />
        public IXmlValidator UseSchema(XmlSchemaSet schemaSet, bool stopOnSchemaErrors = true)
        {
            _schemaSet = schemaSet;
            _stopOnSchemaErrors = stopOnSchemaErrors;

            return this;
        }

        /// <inheritdoc />
        public IXmlValidator GlobalRule(Func<XDocument, bool> predicate, string message = null)
        {
            _rules.Add(new XmlValidatorGlobalRule(predicate, message));

            return this;
        }

        /// <inheritdoc />
        public XmlValidationResult Validate(XDocument doc)
        {
            var failures = new List<XmlValidationFailureResult>();

            if (_schemaSet.IsNotNull())
            {
                var settings = new XmlReaderSettings
                {
                    ValidationType = ValidationType.Schema,
                    Schemas = _schemaSet,
                    DtdProcessing = DtdProcessing.Ignore
                };

                settings.ValidationEventHandler += (s, e) =>
                {
                    failures.Add(new XmlValidationFailureResult()
                    {
                        Severity = XmlMessageSeverity.Error,
                        Path = "$.schema",
                        Message = e.Message
                    });
                };

                using var reader = XmlReader.Create(doc.CreateReader(), settings);
                while (reader.Read()) { }
                if (_stopOnSchemaErrors && failures.Any(f => f.Severity == XmlMessageSeverity.Error))
                {
                    return new XmlValidationResult
                    {
                        IsValid = false,
                        Errors = failures
                    };
                }
            }

            var ctx = new XmlValidationContext(doc, failures);
            foreach (var r in _rules)
                r.Evaluate(ctx);

            return new XmlValidationResult
            {
                IsValid = failures.Count.IsZero(),
                Errors = failures
            };
        }
    }
}