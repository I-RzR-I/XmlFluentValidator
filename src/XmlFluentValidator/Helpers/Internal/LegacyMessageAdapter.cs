// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-17 15:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-18 18:36
// ***********************************************************************
//  <copyright file="LegacyMessageAdapter.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.DataTypeExtensions;
using XmlFluentValidator.Enums;
using XmlFluentValidator.Models.Message;

#endregion

namespace XmlFluentValidator.Helpers.Internal
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A legacy message adapter.
    /// </summary>
    /// =================================================================================================
    internal static class LegacyMessageAdapter
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     From raw.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="path">(Optional) Element path.</param>
        /// <param name="severity">(Optional) The severity.</param>
        /// <returns>
        ///     A MessageDescriptor.
        /// </returns>
        /// =================================================================================================
        public static MessageDescriptor FromRaw(string message, string path = null, XmlMessageSeverity severity = XmlMessageSeverity.Error)
        {
            return new MessageDescriptor("VALIDATION_FAILED", message.IfNullOrWhiteSpace($"Validation failed {(path.IsPresent() ? $"at '{path}'" : "")}"),
                severity);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     From raw.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <param name="path">(Optional) Element path.</param>
        /// <param name="severity">(Optional) The severity.</param>
        /// <returns>
        ///     A MessageDescriptor.
        /// </returns>
        /// =================================================================================================
        public static MessageDescriptor FromRaw(string code, string message, string path = null,
            XmlMessageSeverity severity = XmlMessageSeverity.Error)
        {
            return new MessageDescriptor(code, message.IfNullOrWhiteSpace($"Validation failed {(path.IsPresent() ? $"at '{path}'" : "")}"), severity);
        }
    }
}