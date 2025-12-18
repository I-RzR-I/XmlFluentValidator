// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-09 20:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-09 20:13
// ***********************************************************************
//  <copyright file="XmlStepRecorder.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using XmlFluentValidator.Enums;
using XmlFluentValidator.Models.Message;

#endregion

namespace XmlFluentValidator.Models
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An XML step recorder.
    /// </summary>
    /// =================================================================================================
    public class XmlStepRecorder
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the kind.
        /// </summary>
        /// <value>
        ///     The kind.
        /// </value>
        /// =================================================================================================
        public XmlValidationRuleKind Kind { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the name of the attribute.
        /// </summary>
        /// <value>
        ///     The name of the attribute.
        /// </value>
        /// =================================================================================================
        public string AttributeName { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the pattern.
        /// </summary>
        /// <value>
        ///     The pattern.
        /// </value>
        /// =================================================================================================
        public string Pattern { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the minimum.
        /// </summary>
        /// <value>
        ///     The minimum value.
        /// </value>
        /// =================================================================================================
        public int? Min { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the maximum.
        /// </summary>
        /// <value>
        ///     The maximum value.
        /// </value>
        /// =================================================================================================
        public int? Max { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the maximum occurs.
        /// </summary>
        /// <value>
        ///     The maximum occurs.
        /// </value>
        /// =================================================================================================
        public int? MaxOccurs { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the descriptor.
        /// </summary>
        /// <value>
        ///     The descriptor.
        /// </value>
        /// =================================================================================================
        public MessageDescriptor Descriptor { get; set; }
    }
}