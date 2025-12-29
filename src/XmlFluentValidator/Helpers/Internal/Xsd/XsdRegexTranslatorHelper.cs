// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-26 23:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-26 23:28
// ***********************************************************************
//  <copyright file="XsdRegexTranslatorHelper.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using DomainCommonExtensions.DataTypeExtensions;
using DomainCommonExtensions.Utilities.Ensure;
using XmlFluentValidator.Exceptions;
using XmlFluentValidator.Models.Regex;

#endregion

namespace XmlFluentValidator.Helpers.Internal.Xsd
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An XSD RegEx translator helper.
    /// </summary>
    /// =================================================================================================
    internal static class XsdRegexTranslatorHelper
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the shorthand replacements.
        /// </summary>
        /// =================================================================================================
        private static readonly Dictionary<string, string> ShorthandReplacements = new()
        {
            { @"\d", "[0-9]" },
            { @"\D", "[^0-9]" },
            { @"\w", "[A-Za-z0-9_]" },
            { @"\W", "[^A-Za-z0-9_]" },
            { @"\s", "[ \t\r\n]" },
            { @"\S", "[^ \t\r\n]" },
            { @"\A", string.Empty },
            { @"\z", string.Empty }
        };

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the unsupported patterns.
        /// </summary>
        /// =================================================================================================
        private static readonly string[] UnsupportedPatterns =
        {
            @"\(\?=",     // lookahead
            @"\(\?!",     // negative lookahead
            @"\(\?<=",    // lookbehind
            @"\(\?<!",    // negative lookbehind
            @"\\\d+",     // backreference
            @"\*\?",      // lazy *
            @"\+\?",      // lazy +
            @"\?\?",      // lazy ?
            @"\(\?[imnsx]" // inline flags
        };

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Translates.
        /// </summary>
        /// <param name="regexValue">The RegEx value.</param>
        /// <returns>
        ///     A RegexTranslationResult.
        /// </returns>
        /// =================================================================================================
        public static RegexTranslationResult Translate(string regexValue)
        {
            DomainEnsure.IsNotNullOrEmpty(regexValue, nameof(regexValue), "Regex must not be empty");

            bool caseInsensitive = regexValue.StartsWith("(?i)");
            var regex = caseInsensitive ?
                regexValue.Length > 4
                    ? regexValue.Substring(4)
                    : string.Empty
                : regexValue;

            RejectUnsupported(regexValue);

            var warnings = new List<string>();
            var result = regex;

            // Remove anchors
            result = Regex.Replace(result, @"^\^|\$$", string.Empty);

            // Convert non-capturing groups
            result = result.Replace("(?:", "(");

            // Expand shorthand classes
            foreach (var kv in ShorthandReplacements)
                result = result.Replace(kv.Key, kv.Value);

            if (caseInsensitive.IsTrue())
            {
                result = ExpandToCaseInsensitive(result);
                warnings.Add("Case-insensitive expansion applied.");
            }

            return new RegexTranslationResult(result, warnings);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Expand to case-insensitive.
        /// </summary>
        /// <param name="pattern">Specifies the pattern.</param>
        /// <returns>
        ///     A string.
        /// </returns>
        /// =================================================================================================
        private static string ExpandToCaseInsensitive(string pattern)
        {
            var sb = new StringBuilder();
            foreach (var c in pattern)
            {
                if (char.IsLetter(c))
                    sb.Append($"[{char.ToLowerInvariant(c)}{char.ToUpperInvariant(c)}]");
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Reject unsupported.
        /// </summary>
        /// <exception cref="UnsupportedRegexException">
        ///     Thrown when an Unsupported RegEx error condition occurs.
        /// </exception>
        /// <param name="regex">The RegEx.</param>
        /// =================================================================================================
        private static void RejectUnsupported(string regex)
        {
            foreach (var pattern in UnsupportedPatterns)
            {
                if (Regex.IsMatch(regex, pattern))
                    throw new UnsupportedRegexException($"Unsupported regex construct: {pattern}");
            }
        }
    }
}