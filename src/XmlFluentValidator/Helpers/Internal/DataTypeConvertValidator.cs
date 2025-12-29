// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-25 15:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-26 20:13
// ***********************************************************************
//  <copyright file="DataTypeConvertValidator.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Globalization;
using DomainCommonExtensions.DataTypeExtensions;
using XmlFluentValidator.Enums;

// ReSharper disable RedundantAssignment

#endregion

namespace XmlFluentValidator.Helpers.Internal
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A data type convert validator.
    /// </summary>
    /// =================================================================================================
    internal static class DataTypeConvertValidator
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Determine if we can be converted.
        /// </summary>
        /// <param name="kind">The kind.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     True if we can be converted, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool CanBeConverted(XmlValidationDataTypeKind kind, string value)
        {
            var convertResult = false;
            switch (kind)
            {
                case XmlValidationDataTypeKind.Date:
                    convertResult = DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out _);
                    break;
                case XmlValidationDataTypeKind.DateTime:
                    convertResult = DateTime.TryParseExact(value, "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out _);
                    break;
                case XmlValidationDataTypeKind.Bool:
                    convertResult = bool.TryParse(value, out _);
                    break;
                case XmlValidationDataTypeKind.String:
                    convertResult = true;
                    break;
                case XmlValidationDataTypeKind.Float:
                    convertResult = float.TryParse(value, out _);
                    break;
                case XmlValidationDataTypeKind.Decimal:
                    convertResult = decimal.TryParse(value, out _);
                    break;
                case XmlValidationDataTypeKind.Integer:
                    convertResult = long.TryParse(value, out _);
                    break;
                case XmlValidationDataTypeKind.Double:
                    convertResult = double.TryParse(value, out _);
                    break;
                case XmlValidationDataTypeKind.AnyUri:
                    if (value.IsPresent() && Uri.IsWellFormedUriString(value, UriKind.Absolute).IsFalse())
                        convertResult = false;
                    else
                        convertResult = true;
                    break;
                default:
                    convertResult = false;
                    break;
            }

            return convertResult;
        }
    }
}