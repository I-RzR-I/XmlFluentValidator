// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-25 13:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-26 23:54
// ***********************************************************************
//  <copyright file="XmlValidationDataTypeKind.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.ComponentModel;

#endregion

namespace XmlFluentValidator.Enums
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Values that represent XML validation data type kinds.
    /// </summary>
    /// =================================================================================================
    public enum XmlValidationDataTypeKind
    {
        /// <summary>
        ///     An enum constant representing the date option.
        /// </summary>
        [Description("date")]
        Date,

        /// <summary>
        ///     An enum constant representing the date time option.
        /// </summary>
        [Description("dateTime")]
        DateTime,

        /// <summary>
        ///     An enum constant representing the bool option.
        /// </summary>
        [Description("boolean")]
        Bool,

        /// <summary>
        ///     An enum constant representing the string option.
        /// </summary>
        [Description("string")]
        String,

        /// <summary>
        ///     An enum constant representing the float option.
        /// </summary>
        [Description("float")]
        Float,

        /// <summary>
        ///     An enum constant representing the decimal option.
        /// </summary>
        [Description("decimal")]
        Decimal,

        /// <summary>
        ///     An enum constant representing the integer option.
        /// </summary>
        [Description("integer")]
        Integer,

        /// <summary>
        ///     An enum constant representing the double option.
        /// </summary>
        [Description("double")] 
        Double,

        /// <summary>
        ///     An enum constant representing any URI option.
        /// </summary>
        [Description("anyURI")] 
        AnyUri
    }
}