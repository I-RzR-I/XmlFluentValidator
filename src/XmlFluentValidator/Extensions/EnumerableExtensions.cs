// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2026-01-05 15:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-05 15:18
// ***********************************************************************
//  <copyright file="EnumerableExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Linq;
using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.DataTypeExtensions;

// ReSharper disable PossibleMultipleEnumeration

#endregion

namespace XmlFluentValidator.Extensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An enumerable extensions.
    /// </summary>
    /// =================================================================================================
    internal static class EnumerableExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IEnumerable&lt;string&gt; extension method that query if 'source' is in range string
        ///     value.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <param name="searchValue">The search value.</param>
        /// <returns>
        ///     True if in range string value, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsInRangeStringValue(this IEnumerable<string> source, string searchValue)
        {
            if (source.IsNullOrEmptyEnumerable() || searchValue.IsMissing())
                return false;

            return source.NotNull().Contains(searchValue);
        }
    }
}