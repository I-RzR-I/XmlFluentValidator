// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-09 20:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-09 20:33
// ***********************************************************************
//  <copyright file="XmlValidationResult.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;

#endregion

namespace XmlFluentValidator.Models.Result
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Encapsulates the result of an XML validation.
    /// </summary>
    /// =================================================================================================
    public class XmlValidationResult
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether this object is valid.
        /// </summary>
        /// <value>
        ///     True if this object is valid, false if not.
        /// </value>
        /// =================================================================================================
        public bool IsValid { get; internal set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the errors.
        /// </summary>
        /// <value>
        ///     The errors.
        /// </value>
        /// =================================================================================================
        public IReadOnlyList<XmlValidationFailureResult> Errors { get; internal set; }
    }
}