// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-26 12:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-26 15:39
// ***********************************************************************
//  <copyright file="XsdValueConstraintModelDefinition.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System.Collections.Generic;

namespace XmlFluentValidator.Models.XsdElements
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An XSD value constraint model definition.
    /// </summary>
    /// =================================================================================================
    public class XsdValueConstraintModelDefinition
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the REGEX pattern.
        /// </summary>
        /// <value>
        ///     The REGEX pattern.
        /// </value>
        /// =================================================================================================
        public string Pattern { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the minimum length of the element/attribute.
        /// </summary>
        /// <value>
        ///     The minimum length of the element/attribute.
        /// </value>
        /// =================================================================================================
        public int? MinLength { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the maximum length of the element/attribute.
        /// </summary>
        /// <value>
        ///     The maximum length of the element/attribute.
        /// </value>
        /// =================================================================================================
        public int? MaxLength { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the minimum inclusive value.
        /// </summary>
        /// <value>
        ///     The minimum inclusive of the element/attribute.
        /// </value>
        /// =================================================================================================
        public int? MinInclusive { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the maximum inclusive value.
        /// </summary>
        /// <value>
        ///     The maximum inclusive of the element/attribute.
        /// </value>
        /// =================================================================================================
        public int? MaxInclusive { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the minimum exclusive value.
        /// </summary>
        /// <value>
        ///     The minimum exclusive of the element/attribute.
        /// </value>
        /// =================================================================================================
        public int? MinExclusive { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the maximum exclusive value.
        /// </summary>
        /// <value>
        ///     The maximum exclusive of the element/attribute.
        /// </value>
        /// =================================================================================================
        public int? MaxExclusive { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the enumeration values.
        /// </summary>
        /// <value>
        ///     The enumeration values.
        /// </value>
        /// =================================================================================================
        public IEnumerable<string> EnumerationValues { get; set; }
    }
}