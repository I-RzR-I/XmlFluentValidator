// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-17 20:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-18 18:34
// ***********************************************************************
//  <copyright file="ILocalizedMessageTemplateSource.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace XmlFluentValidator.Abstractions.Message
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for localized message template source.
    /// </summary>
    /// =================================================================================================
    public interface ILocalizedMessageTemplateSource
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a localized string using the given code.
        /// </summary>
        /// <param name="code">The code to get.</param>
        /// <returns>
        ///     A string.
        /// </returns>
        /// =================================================================================================
        string Get(string code);
    }
}