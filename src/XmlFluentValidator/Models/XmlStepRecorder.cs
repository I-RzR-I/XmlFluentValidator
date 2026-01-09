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

using System.Collections.Generic;
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
    public sealed class XmlStepRecorder
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
        ///     The maximum for: length, in range max value
        /// </summary>
        /// <value>
        ///     The maximum value.
        /// </value>
        /// =================================================================================================
        public int? Max { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the value exact length.
        /// </summary>
        /// <value>
        ///     The length of the value exact.
        /// </value>
        /// =================================================================================================
        public int? ValueExactLength { get; set; }

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

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the element/attribute path.
        /// </summary>
        /// <value>
        ///     The element/attribute path.
        /// </value>
        /// =================================================================================================
        public string Path { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the type of the data.
        /// </summary>
        /// <value>
        ///     The type of the data.
        /// </value>
        /// =================================================================================================
        public XmlValidationDataTypeKind DataType { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether this object is inclusive validation.
        /// </summary>
        /// <value>
        ///     True if this object is inclusive validation, false if not.
        /// </value>
        /// =================================================================================================
        public bool IsInclusiveValidation { get; set; } = true;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the in range enumerator.
        /// </summary>
        /// <value>
        ///     The in range enumerator.
        /// </value>
        /// =================================================================================================
        public IEnumerable<string> InRangeEnumerator { get; set; } = new List<string>();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets information of the annotation documentation.
        /// </summary>
        /// <value>
        ///     Information of the annotation documentation.
        /// </value>
        /// =================================================================================================
        public string AnnotationDocumentation { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the is nullable.
        /// </summary>
        /// <value>
        ///     The is nullable.
        /// </value>
        /// =================================================================================================
        public bool? IsNullable { get; set; }
    }
}