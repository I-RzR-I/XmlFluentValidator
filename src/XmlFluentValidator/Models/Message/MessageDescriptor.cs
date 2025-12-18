// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-17 18:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-18 19:47
// ***********************************************************************
//  <copyright file="MessageDescriptor.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using XmlFluentValidator.Enums;

#endregion

namespace XmlFluentValidator.Models.Message
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A message descriptor.
    /// </summary>
    /// =================================================================================================
    public class MessageDescriptor
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the message code.
        /// </summary>
        /// <value>
        ///     The code.
        /// </value>
        /// =================================================================================================
        public string Code { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the default message/template.
        /// </summary>
        /// <value>
        ///     The default template.
        /// </value>
        /// =================================================================================================
        public string DefaultTemplate { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the default message severity.
        /// </summary>
        /// <value>
        ///     The default severity.
        /// </value>
        /// =================================================================================================
        public XmlMessageSeverity DefaultSeverity { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageDescriptor"/> class.
        /// </summary>
        /// <param name="code">The message code.</param>
        /// <param name="template">The message/template.</param>
        /// <param name="severity">(Optional) The severity.</param>
        /// =================================================================================================
        public MessageDescriptor(string code, string template, XmlMessageSeverity severity = XmlMessageSeverity.Error)
        {
            Code = code;
            DefaultTemplate = template;
            DefaultSeverity = severity;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageDescriptor"/> class.
        /// </summary>
        /// <param name="template">The message/template.</param>
        /// <param name="severity">(Optional) The severity.</param>
        /// =================================================================================================
        public MessageDescriptor(string template, XmlMessageSeverity severity = XmlMessageSeverity.Error)
        {
            Code = null;
            DefaultTemplate = template;
            DefaultSeverity = severity;
        }
    }
}