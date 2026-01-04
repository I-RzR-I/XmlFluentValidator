// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2026-01-04 22:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-04 22:45
// ***********************************************************************
//  <copyright file="IntExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.DataTypeExtensions;

#endregion

namespace XmlFluentValidator.Extensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An int extensions.
    /// </summary>
    /// =================================================================================================
    internal static class IntExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An int extension method that query if this object is in range.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <param name="inclusive">True to inclusive.</param>
        /// <returns>
        ///     True if in range, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsInRange(this int source, int minValue, int maxValue, bool inclusive)
        {
            if (minValue > maxValue)
                return false;

            return inclusive.IsTrue()
                ? source >= minValue && source <= maxValue
                : source > minValue && source < maxValue;
        }
    }
}