// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-09 21:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-09 21:51
// ***********************************************************************
//  <copyright file="XmlValidatorCompositeRule.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using XmlFluentValidator.Abstractions;
using XmlFluentValidator.Enums;
using XmlFluentValidator.Models;
using XmlFluentValidator.Models.Result;

#endregion

namespace XmlFluentValidator.Rules
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An XML validator composite rule.
    /// </summary>
    /// <seealso cref="T:XmlFluentValidator.Abstractions.IXmlValidatorRule"/>
    /// =================================================================================================
    public class XmlValidatorCompositeRule : IXmlValidatorRule
    {
        #region PRIVATE FIELDS
        
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The condition custom message.
        /// </summary>
        /// =================================================================================================
        private string _conditionMessage;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The default message.
        /// </summary>
        /// =================================================================================================
        private string _defaultMessage;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The custom message.
        /// </summary>
        /// =================================================================================================
        private string _customMessage;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The severity.
        /// </summary>
        /// =================================================================================================
        private XmlMessageSeverity _severity = XmlMessageSeverity.Error;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The condition.
        /// </summary>
        /// =================================================================================================
        private Func<XDocument, bool> _condition;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) XPath.
        /// </summary>
        /// =================================================================================================
        private readonly string _xPath;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the steps.
        /// </summary>
        /// =================================================================================================
        private readonly IEnumerable<Func<XDocument, IEnumerable<XmlValidationFailureResult>>> _steps;

        #endregion

        #region INTERNAL / PUBLIC

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) name of the display.
        /// </summary>
        /// =================================================================================================
        internal readonly string DisplayName;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the recorded steps.
        /// </summary>
        /// =================================================================================================
        internal readonly IList<XmlStepRecorder> RecordedSteps = new List<XmlStepRecorder>();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the xpath.
        /// </summary>
        /// <value>
        ///     The xpath.
        /// </value>
        /// =================================================================================================
        public string XPath => _xPath;

        #endregion

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="XmlValidatorCompositeRule"/> class.
        /// </summary>
        /// <param name="xPath">(Immutable) XPath.</param>
        /// <param name="steps">The steps.</param>
        /// <param name="displayName">Name of the display.</param>
        /// =================================================================================================
        public XmlValidatorCompositeRule(
            string xPath,
            IEnumerable<Func<XDocument, IEnumerable<XmlValidationFailureResult>>> steps,
            string displayName)
        {
            _xPath = xPath;
            _steps = steps;
            DisplayName = displayName;
        }

        /// <inheritdoc/>
        public void Evaluate(XmlValidationContext ctx)
        {
            if (_condition.IsNotNull() && _condition(ctx.Document).IsFalse())
            {
                if (_severity.AreEquals(XmlMessageSeverity.Error))
                {
                    ctx.Failures.Add(new XmlValidationFailureResult()
                    {
                        Severity = _severity,
                        Path = _xPath,
                        Name = DisplayName,
                        Message = _defaultMessage.IfNullOrWhiteSpace(_conditionMessage.IfNullOrWhiteSpace("Condition evaluation failed!"))
                    });
                }

                return;
            }

            foreach (var step in _steps.NotNull())
            {
                var failures = step(ctx.Document);

                foreach (var f in failures.NotNull())
                {
                    if (f.Message.IsMissing())
                    {
                        //  For WithMessage
                        if (_customMessage.IsPresent())
                            f.Message = _customMessage;

                        // For WithMessageForAll (same message for all failure results)
                        if (_defaultMessage.IsPresent())
                            f.Message = _defaultMessage;
                    }

                    ctx.Failures.Add(f);
                }
            }
        }

        #region INTERNAL

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Sets a condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// =================================================================================================
        internal void SetCondition(Func<XDocument, bool> condition)
        {
            _condition = condition;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Sets condition message.
        /// </summary>
        /// <param name="conditionMessage">Message describing the condition.</param>
        /// <param name="severity">(Optional) The severity.</param>
        /// =================================================================================================
        internal void SetConditionMessage(string conditionMessage,
            XmlMessageSeverity severity = XmlMessageSeverity.Error)
        {
            _conditionMessage = conditionMessage;
            _severity = severity;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Sets custom message.
        /// </summary>
        /// <param name="customMessage">Message describing the custom.</param>
        /// =================================================================================================
        internal void SetCustomMessage(string customMessage)
        {
            _customMessage = customMessage;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Sets default message.
        /// </summary>
        /// <param name="defaultMessage">The default message.</param>
        /// =================================================================================================
        internal void SetDefaultMessage(string defaultMessage)
        {
            _defaultMessage = defaultMessage;
        }

        #endregion
    }
}