// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-09 21:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-09 21:45
// ***********************************************************************
//  <copyright file="XmlValidatorGlobalRule.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Xml.Linq;
using DomainCommonExtensions.DataTypeExtensions;
using XmlFluentValidator.Abstractions;
using XmlFluentValidator.Enums;
using XmlFluentValidator.Models.Result;

#endregion

namespace XmlFluentValidator.Rules
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An XML validator global rule.
    /// </summary>
    /// <seealso cref="T:XmlFluentValidator.Abstractions.IXmlValidatorRule"/>
    /// =================================================================================================
    public class XmlValidatorGlobalRule : IXmlValidatorRule
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the predicate.
        /// </summary>
        /// =================================================================================================
        private readonly Func<XDocument, bool> _predicate;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the message.
        /// </summary>
        /// =================================================================================================
        private readonly string _message;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the message.
        /// </summary>
        /// <value>
        ///     The message.
        /// </value>
        /// =================================================================================================
        internal string Message => _message;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="XmlValidatorGlobalRule"/> class.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="message">The message.</param>
        /// =================================================================================================
        public XmlValidatorGlobalRule(Func<XDocument, bool> predicate, string message)
        {
            _predicate = predicate;
            _message = message;
        }

        /// <inheritdoc/>
        public void Evaluate(XmlValidationContext ctx)
        {
            if (_predicate(ctx.Document).IsFalse())
            {
                ctx.Failures.Add(new XmlValidationFailureResult()
                {
                    Severity = XmlMessageSeverity.Error,
                    Path = "$.global",
                    Message = _message.IfNullOrWhiteSpace("Global validation rule failed.")
                });
            }
        }
    }
}