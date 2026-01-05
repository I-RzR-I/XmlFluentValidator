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
using XmlFluentValidator.Models.Message;

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
        #region ELEMENT

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Must exist. The element must exist.
        /// </summary>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder WithElementMustExist(string message = null);

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
        IXmlValidatorRuleBuilder WithElementCount(Func<int, bool> predicate, string message = null);

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
        IXmlValidatorRuleBuilder WithElementValue(Func<string, bool> predicate, string message = null);

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
        IXmlValidatorRuleBuilder WithElementOptional(string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     IsRequired.
        ///     Set element validation rule as required.
        /// </summary>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder WithElementRequired(string message = null);

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
        IXmlValidatorRuleBuilder WithElementMatchesRegex(string pattern, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     In range. Set element validation rule as in range between minimum and maximum value.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="isInclusive">(Optional) True if is inclusive, false if not.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder WithElementInRange(int min, int max,
            bool isInclusive = true, string message = null);

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
        IXmlValidatorRuleBuilder WithElementUnique(string message = null);

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
        IXmlValidatorRuleBuilder WithElementMaxOccurs(int max, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     With element value length.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">(Optional) The maximum.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder WithElementValueLength(int min, int? max = null, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     With element data type.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder WithElementDataType(XmlValidationDataTypeKind dataType, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     With element enumerator.
        /// </summary>
        /// <param name="rangeEnumerator">The range enumerator.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder WithElementEnumerator(
            string[] rangeEnumerator,
            string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     With element exact length.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder WithElementExactLength(
            int length,
            string message = null);

        #endregion

        #region ATTRIBUTE

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
        IXmlValidatorRuleBuilder WithAttribute(string name, Func<string, bool> predicate, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     IsRequired attribute.
        ///     Set attribute validation rule as unique value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder WithAttributeRequired(string name, string message = null);

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
        IXmlValidatorRuleBuilder WithAttributeMatchesRegex(string name, string pattern, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Attribute in range. Set attribute validation rule as in range between minimum and maximum
        ///     value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        /// <param name="isInclusive">(Optional) True if is inclusive, false if not.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder WithAttributeInRange(string name, int min, int max,
            bool isInclusive = true, string message = null);

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
        IXmlValidatorRuleBuilder WithAttributeUnique(string name, string message = null);

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
        ///     With attribute value length.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">(Optional) The maximum.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder WithAttributeValueLength(string name, int min, int? max = null, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     With attribute data type.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder WithAttributeDataType(string name, XmlValidationDataTypeKind dataType, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     With attribute enumerator.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="rangeEnumerator">The range enumerator.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder WithAttributeEnumerator(
            string name,
            string[] rangeEnumerator,
            string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     With attribute exact length.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="length">The length.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder WithAttributeExactLength(
            string name,
            int length,
            string message = null);

        #endregion

        #region MESSAGES

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     With message. 
        ///     Set custom validation message.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="arguments">The context, message arguments.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder WithMessage(string template, MessageArguments arguments);

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

        #endregion

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