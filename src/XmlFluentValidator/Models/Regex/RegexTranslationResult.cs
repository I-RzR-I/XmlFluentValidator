// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-26 23:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-26 23:46
// ***********************************************************************
//  <copyright file="RegexTranslationResult.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

#endregion

namespace XmlFluentValidator.Models.Regex
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Encapsulates the result of a RegEx translation.
    /// </summary>
    /// =================================================================================================
    public class RegexTranslationResult
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the XSD pattern.
        /// </summary>
        /// <value>
        ///     The XSD pattern.
        /// </value>
        /// =================================================================================================
        public string XsdPattern { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the warnings.
        /// </summary>
        /// <value>
        ///     The warnings.
        /// </value>
        /// =================================================================================================
        public IReadOnlyList<string> Warnings { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="RegexTranslationResult"/> class.
        /// </summary>
        /// <param name="xsdPattern">The XSD pattern.</param>
        /// <param name="warnings">The warnings.</param>
        /// =================================================================================================
        public RegexTranslationResult(string xsdPattern, IReadOnlyList<string> warnings)
        {
            XsdPattern = xsdPattern;
            Warnings = warnings;
        }
    }
}