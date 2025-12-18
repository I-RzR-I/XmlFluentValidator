// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-17 17:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-17 17:38
// ***********************************************************************
//  <copyright file="FailureMessageDescriptor.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace XmlFluentValidator.Models.Message
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A failure message descriptor. This class cannot be inherited.
    /// </summary>
    /// =================================================================================================
    public sealed class FailureMessageDescriptor
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the message descriptor.
        /// </summary>
        /// <value>
        ///     The descriptor.
        /// </value>
        /// =================================================================================================
        public MessageDescriptor Descriptor { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the xpath to element or attribute.
        /// </summary>
        /// <value>
        ///     The xpath to element or attribute.
        /// </value>
        /// =================================================================================================
        public string Path { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        /// =================================================================================================
        public string Name { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>
        ///     The arguments.
        /// </value>
        /// =================================================================================================
        public MessageArguments Arguments { get; set; }
    }
}

