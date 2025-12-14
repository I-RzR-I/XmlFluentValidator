// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-09 20:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-09 21:03
// ***********************************************************************
//  <copyright file="IXmlValidator.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Xml.Linq;
using System.Xml.Schema;
using XmlFluentValidator.Models.Result;

#endregion

namespace XmlFluentValidator.Abstractions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for XML validator.
    /// </summary>
    /// =================================================================================================
    public interface IXmlValidator
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     For path. Rule for specific path
        /// </summary>
        /// <param name="xpath">The xpath.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder ForPath(string xpath);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     For attribute.
        ///     xpath to element; attribute name set in rule
        /// </summary>
        /// <param name="xpath">The xpath.</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder ForAttribute(string xpath);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     For element.
        ///     Rule for specific element.
        /// </summary>
        /// <param name="elementPath">The element path (xpath).</param>
        /// <returns>
        ///     An IXmlValidatorRuleBuilder.
        /// </returns>
        /// =================================================================================================
        IXmlValidatorRuleBuilder ForElement(string elementPath);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Use schema.
        /// </summary>
        /// <param name="schemaSet">Set the schema belongs to.</param>
        /// <param name="stopOnSchemaErrors">(Optional) True to stop on schema errors.</param>
        /// <returns>
        ///     An XmlValidator.
        /// </returns>
        /// =================================================================================================
        IXmlValidator UseSchema(XmlSchemaSet schemaSet, bool stopOnSchemaErrors = true);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Global rule.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     An IXmlValidator.
        /// </returns>
        /// =================================================================================================
        IXmlValidator GlobalRule(Func<XDocument, bool> predicate, string message = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Validates the given document.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <returns>
        ///     An XmlValidationResult.
        /// </returns>
        /// =================================================================================================
        XmlValidationResult Validate(XDocument doc);
    }
}