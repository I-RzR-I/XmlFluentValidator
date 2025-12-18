// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-09 20:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-18 20:45
// ***********************************************************************
//  <copyright file="XmlValidationContext.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Xml.Linq;
using DomainCommonExtensions.CommonExtensions.TypeParam;
using XmlFluentValidator.Abstractions.Message;
using XmlFluentValidator.Models.Result;

#endregion

namespace XmlFluentValidator
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An XML validation context.
    /// </summary>
    /// =================================================================================================
    public class XmlValidationContext
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the message factory.
        /// </summary>
        /// <value>
        ///     The message factory.
        /// </value>
        /// =================================================================================================
        public IXmlValidationMessageFactory MessageFactory { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the document.
        /// </summary>
        /// <value>
        ///     The document.
        /// </value>
        /// =================================================================================================
        public XDocument Document { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the failures.
        /// </summary>
        /// <value>
        ///     The failures.
        /// </value>
        /// =================================================================================================
        public IList<XmlValidationFailureResult> Failures { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="XmlValidationContext"/> class.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="failures">The failures.</param>
        /// <param name="messageFactory">The message factory.</param>
        /// =================================================================================================
        public XmlValidationContext(XDocument doc, IList<XmlValidationFailureResult> failures, 
            IXmlValidationMessageFactory messageFactory)
        {
            Document = doc;
            Failures = failures;
            MessageFactory = messageFactory.IfIsNull(new DefaultXmlValidationMessageFactory());
        }
    }
}