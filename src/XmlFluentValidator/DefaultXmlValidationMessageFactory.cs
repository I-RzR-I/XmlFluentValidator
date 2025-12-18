// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-17 18:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-18 19:53
// ***********************************************************************
//  <copyright file="DefaultXmlValidationMessageFactory.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using XmlFluentValidator.Abstractions.Message;
using XmlFluentValidator.Enums;
using XmlFluentValidator.Helpers.Internal;
using XmlFluentValidator.Models.Message;
using XmlFluentValidator.Models.Result;

#endregion

namespace XmlFluentValidator
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A default XML validation message factory.
    /// </summary>
    /// <seealso cref="T:XmlFluentValidator.Abstractions.Message.IXmlValidationMessageFactory"/>
    /// =================================================================================================
    public class DefaultXmlValidationMessageFactory : IXmlValidationMessageFactory
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the templates.
        /// </summary>
        /// =================================================================================================
        private readonly ILocalizedMessageTemplateSource _templates;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultXmlValidationMessageFactory"/>
        ///     class.
        /// </summary>
        /// <param name="templates">(Optional) The templates.</param>
        /// =================================================================================================
        public DefaultXmlValidationMessageFactory(ILocalizedMessageTemplateSource templates = null)
        {
            _templates = templates;
        }
        
        /// <inheritdoc/>
        public XmlValidationFailureResult Create(MessageDescriptor descriptor, string path,
            string name = null, MessageArguments args = null, XmlMessageSeverity? severityOverride = null)
        {
            var template = _templates?.Get(descriptor.Code) ?? descriptor.DefaultTemplate;
            var message = XmlMessageFormatter.Format(template, args);

            return new XmlValidationFailureResult
            {
                Code = descriptor.Code,
                Path = path,
                Name = name,
                Severity = severityOverride ?? descriptor.DefaultSeverity,
                Message = message
            };
        }
    }
}