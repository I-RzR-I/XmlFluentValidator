// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-09 20:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-09 21:28
// ***********************************************************************
//  <copyright file="IXmlValidatorRuleBuilder.cs" company="RzR SOFT & TECH">
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
using XmlFluentValidator.Enums;

#endregion

namespace XmlFluentValidator.Abstractions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for XML validator rule builder.
    /// </summary>
    /// =================================================================================================
    public interface IXmlValidatorRuleBuilder
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Must exist. The element must exist.
        /// </summary>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder MustExist(string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Counts.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder Count(Func<int, bool> predicate, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Value.
        ///     Set validation rule for element value.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder Value(Func<string, bool> predicate, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Attributes.
        ///     Set validation rule for element attribute.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder Attribute(string name, Func<string, bool> predicate, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Alls.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder All(Func<XElement, bool> predicate, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Any.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder Any(Func<XElement, bool> predicate, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     When the given condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder When(Func<XDocument, bool> condition, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Optional.
        ///     Specify the element validation as not required.
        /// </summary>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder Optional(string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Required.
        ///     Set element validation rule as required.
        /// </summary>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder Required(string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Matches RegEx.
        ///     Set element validation rule as regular expression.
        /// </summary>
        /// <param name="pattern">Specifies the RegEx pattern.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder MatchesRegex(string pattern, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     In range.
        ///     Set element validation rule as in range between minimum and maximum value.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder InRange(int min, int max, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Uniques the given message.
        ///     Set element validation rule as unique value.
        /// </summary>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder Unique(string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Maximum occurs.
        /// </summary>
        /// <param name="max">The maximum.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder MaxOccurs(int max, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Required attribute.
        ///     Set attribute validation rule as unique value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder RequiredAttribute(string name, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Attribute matches RegEx.
        ///     Set attribute validation rule as regular expression.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="pattern">Specifies the RegEx pattern.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder AttributeMatchesRegex(string name, string pattern, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Attribute in range.
        ///     Set attribute validation rule as in range between minimum and maximum value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder AttributeInRange(string name, int min, int max, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Attribute unique.
        ///     Set specific attribute as required.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder AttributeUnique(string name, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Element attribute cross rule.
        ///     Set cross validation for element and specific element attribute.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder ElementAttributeCrossRule(string attributeName, Func<string, string, bool> predicate,
            string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Custom element rule.
        ///     Can be used custom defined(registered) rule or defined in specific context.
        /// </summary>
        /// <remarks>
        ///     The custom rule must be registered before use.
        ///     <code>
        ///         <![CDATA[CustomRuleRegistry.Register(name, predicate)]]>
        ///     </code>
        /// </remarks>
        /// <param name="predicate">The predicate.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder CustomElementRule(Func<XElement, IDictionary<string, string>, bool> predicate,
            string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Customs.
        ///     Set the custom (user defined) validation method.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder Custom(Action<XmlValidationContext> handler, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Use custom rule.
        ///     Set the custom rule name for execution.
        /// </summary>
        /// <param name="ruleName">Name of the rule.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <remarks>
        ///     The custom rule must be registered before use.
        ///     <code>
        ///         <![CDATA[CustomRuleRegistry.Register(name, predicate)]]>
        ///     </code>
        /// </remarks>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder UseCustomRule(string ruleName, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     With name. 
        ///     Set the specific name for path.
        /// </summary>
        /// <param name="displayName">Name of the display.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder WithName(string displayName);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     With severity. 
        ///     Set the validation message severity.
        /// </summary>
        /// <param name="severity">The severity.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder WithSeverity(XmlMessageSeverity severity);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Stops on failure.
        ///     Short-circuit within this rule chain.
        /// </summary>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder StopOnFailure();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     With message. 
        ///     Set custom validation message.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder WithMessage(string template, object context);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     With message. Set custom validation message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder WithMessage(string message);


        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     With message for all.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder WithMessageForAll(string message);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the done.
        ///     Ends the current element rule chain and returns to validator.
        /// </summary>
        /// <returns>
        ///     An XmlValidator.
        /// </returns>
        /// =================================================================================================
        XmlValidator Done();
    }
}