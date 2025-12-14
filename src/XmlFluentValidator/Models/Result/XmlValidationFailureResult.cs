// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-09 20:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-09 20:34
// ***********************************************************************
//  <copyright file="XmlValidationFailureResult.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using XmlFluentValidator.Enums;

#endregion

namespace XmlFluentValidator.Models.Result
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Encapsulates the result of an XML validation failure.
    /// </summary>
    /// =================================================================================================
    public class XmlValidationFailureResult
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the severity.
        /// </summary>
        /// <value>
        ///     The severity.
        /// </value>
        /// =================================================================================================
        public XmlMessageSeverity Severity { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the full pathname of the file.
        /// </summary>
        /// <value>
        ///     The full pathname of the file.
        /// </value>
        /// =================================================================================================
        public string Path { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        /// <value>
        ///     The message.
        /// </value>
        /// =================================================================================================
        public string Message { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        /// =================================================================================================
        public string Name { get; set; }
    }
}