// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-26 19:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-26 19:51
// ***********************************************************************
//  <copyright file="EnumExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.CommonExtensions;

#endregion

namespace XmlFluentValidator.Extensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An enum extensions.
    /// </summary>
    /// =================================================================================================
    internal static class EnumExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A T? extension method that query if 'sourceEnumValue' is equal to.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="sourceEnumValue">The sourceEnumValue to act on.</param>
        /// <param name="compareEnumValue">The compare enum value.</param>
        /// <returns>
        ///     True if equal to, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsEqualTo<T>(this T? sourceEnumValue, T? compareEnumValue)
            where T : struct
        {
            if (sourceEnumValue.IsNull() && compareEnumValue.IsNull())
                return true;

            if (sourceEnumValue.IsNull() || compareEnumValue.IsNull())
                return false;

            return sourceEnumValue!.Equals(compareEnumValue);
        }
    }
}