// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-17 20:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-18 19:51
// ***********************************************************************
//  <copyright file="IXmlValidationMessageFactory.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using XmlFluentValidator.Enums;
using XmlFluentValidator.Models.Message;
using XmlFluentValidator.Models.Result;

#endregion

namespace XmlFluentValidator.Abstractions.Message
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for XML validation message factory.
    /// </summary>
    /// =================================================================================================
    public interface IXmlValidationMessageFactory
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a new XmlValidationFailureResult.
        /// </summary>
        /// <param name="descriptor">The message descriptor.</param>
        /// <param name="path">The xpath to element/attribute.</param>
        /// <param name="name">(Optional) The name.</param>
        /// <param name="args">(Optional) The arguments.</param>
        /// <param name="severityOverride">(Optional) The severity override.</param>
        /// <returns>
        ///     An XmlValidationFailureResult.
        /// </returns>
        /// =================================================================================================
        XmlValidationFailureResult Create(MessageDescriptor descriptor, string path,
            string name = null, MessageArguments args = null, XmlMessageSeverity? severityOverride = null);
    }
}