// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2026-01-09 19:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-09 19:20
// ***********************************************************************
//  <copyright file="StringExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Linq;
using DomainCommonExtensions.DataTypeExtensions;

#endregion

namespace XmlFluentValidator.Extensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A string extensions.
    /// </summary>
    /// =================================================================================================
    internal static class StringExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A string extension method that query if 'source' is true.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     True if true, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsTrue(this string source)
        {
            var normalized = source.IfNullThenEmpty().ToLower();

            var trueValues = new[] { "true", "1", "t", "y", "+" };

            return trueValues.Contains(normalized);
        }
    }
}