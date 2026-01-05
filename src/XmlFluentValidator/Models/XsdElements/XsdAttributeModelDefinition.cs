// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-26 12:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-26 15:38
// ***********************************************************************
//  <copyright file="XsdAttributeModelDefinition.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using XmlFluentValidator.Enums;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

#endregion

namespace XmlFluentValidator.Models.XsdElements
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An XSD attribute model definition.
    /// </summary>
    /// =================================================================================================
    public class XsdAttributeModelDefinition
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the attribute name.
        /// </summary>
        /// <value>
        ///     The name of the attribute.
        /// </value>
        /// =================================================================================================
        public string Name { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether the is required.
        /// </summary>
        /// <value>
        ///     True if is required, false if not.
        /// </value>
        /// =================================================================================================
        public bool IsRequired { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the data type of the value
        /// </summary>
        /// <value>
        ///     The type of the value.
        /// </value>
        /// =================================================================================================
        public XmlValidationDataTypeKind? ValueType { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the attribute value constraints.
        /// </summary>
        /// <value>
        ///     The attribute constraints.
        /// </value>
        /// =================================================================================================
        public XsdValueConstraintModelDefinition Constraints { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="XsdAttributeModelDefinition"/> class.
        /// </summary>
        /// =================================================================================================
        public XsdAttributeModelDefinition()
        {
            Constraints = new XsdValueConstraintModelDefinition();
        }
    }
}