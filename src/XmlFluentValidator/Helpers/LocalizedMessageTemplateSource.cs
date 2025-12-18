// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-17 20:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-18 18:33
// ***********************************************************************
//  <copyright file="LocalizedMessageTemplateSource.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Resources;
using DomainCommonExtensions.DataTypeExtensions;
using XmlFluentValidator.Abstractions.Message;

#endregion

namespace XmlFluentValidator.Helpers
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A localized message template source.
    /// </summary>
    /// <seealso cref="T:XmlFluentValidator.Abstractions.Message.ILocalizedMessageTemplateSource"/>
    /// =================================================================================================
    public class LocalizedMessageTemplateSource : ILocalizedMessageTemplateSource
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the manager.
        /// </summary>
        /// =================================================================================================
        private readonly ResourceManager _manager;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="LocalizedMessageTemplateSource"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// =================================================================================================
        public LocalizedMessageTemplateSource(ResourceManager manager)
        {
            _manager = manager;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a string using the given code.
        /// </summary>
        /// <param name="code">The code to get.</param>
        /// <returns>
        ///     A string.
        /// </returns>
        /// =================================================================================================
        public string Get(string code)
        {
            return _manager.GetString(code).IfNullThenEmpty();
        }
    }
}