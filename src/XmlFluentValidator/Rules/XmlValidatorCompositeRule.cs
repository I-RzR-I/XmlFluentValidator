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
using XmlFluentValidator.Enums.Internal;
using XmlFluentValidator.Helpers.Internal;
using XmlFluentValidator.Models;
using XmlFluentValidator.Models.Message;
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
    public sealed class XmlValidatorCompositeRule : IXmlValidatorRule
    {
        #region PRIVATE FIELDS

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the message descriptors.
        /// </summary>
        /// =================================================================================================
        private readonly Dictionary<CompositeRuleMessageDescriptorType, MessageDescriptor> _messageDescriptors
            = new Dictionary<CompositeRuleMessageDescriptorType, MessageDescriptor>();

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
        private readonly IEnumerable<Func<XDocument, IEnumerable<FailureMessageDescriptor>>> _steps;

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
            IEnumerable<Func<XDocument, IEnumerable<FailureMessageDescriptor>>> steps,
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
                    var desc = _messageDescriptors.TryGetValue(CompositeRuleMessageDescriptorType.Default)
                               ?? _messageDescriptors.TryGetValue(CompositeRuleMessageDescriptorType.Custom)
                               ?? _messageDescriptors.TryGetValue(CompositeRuleMessageDescriptorType.Condition)
                               ?? DefaultMessageDescriptors.ConditionFailed;
                    var failure = ctx.MessageFactory.Create(desc, path: _xPath, name: DisplayName,
                        MessageArguments.From((MessageArgs.Path, _xPath)));

                    ctx.Failures.Add(failure);
                }

                return;
            }

            foreach (var step in _steps.NotNull())
            {
                var failures = step(ctx.Document);

                foreach (var f in failures.NotNull())
                {
                    XmlValidationFailureResult failure = null;
                    if (f.Descriptor.IsNull())
                    {
                        var desc = _messageDescriptors.TryGetValue(CompositeRuleMessageDescriptorType.Default)
                                   ?? _messageDescriptors.TryGetValue(CompositeRuleMessageDescriptorType.Custom)
                                   ?? _messageDescriptors.TryGetValue(CompositeRuleMessageDescriptorType.Condition)
                                   ?? DefaultMessageDescriptors.GenericFailure;
                        failure = ctx.MessageFactory.Create(desc, path: f.Path, name: f.Name, MessageArguments.From((MessageArgs.Path, _xPath)));
                    }
                    else
                    {
                        if (_messageDescriptors.TryGetValue(CompositeRuleMessageDescriptorType.Default, out var descriptor))
                            failure = ctx.MessageFactory.Create(descriptor, path: f.Path, name: f.Name, MessageArguments.From((MessageArgs.Path, _xPath)));
                        else if (_messageDescriptors.TryGetValue(CompositeRuleMessageDescriptorType.Custom, out var messageDescriptor))
                            failure = ctx.MessageFactory.Create(messageDescriptor, path: f.Path, name: f.Name, MessageArguments.From((MessageArgs.Path, _xPath)));
                        else
                            failure = ctx.MessageFactory.Create(f.Descriptor, path: f.Path, name: f.Name, f.Arguments);
                    }
                    ctx.Failures.Add(failure);
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
        /// <param name="conditionDescriptor">Message describing the condition.</param>
        /// <param name="severity">(Optional) The severity.</param>
        /// =================================================================================================
        internal void SetConditionMessage(MessageDescriptor conditionDescriptor,
            XmlMessageSeverity severity = XmlMessageSeverity.Error)
        {
            _messageDescriptors[CompositeRuleMessageDescriptorType.Condition] = conditionDescriptor;
            _severity = severity;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Sets custom message.
        /// </summary>
        /// <param name="customDescriptor">Message describing the custom.</param>
        /// =================================================================================================
        internal void SetCustomMessage(MessageDescriptor customDescriptor)
        {
            _messageDescriptors[CompositeRuleMessageDescriptorType.Custom] = customDescriptor;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Sets default message.
        /// </summary>
        /// <param name="defaultDescriptor">The default message.</param>
        /// =================================================================================================
        internal void SetDefaultMessage(MessageDescriptor defaultDescriptor)
        {
            _messageDescriptors[CompositeRuleMessageDescriptorType.Default] = defaultDescriptor;
        }

        #endregion
    }
}