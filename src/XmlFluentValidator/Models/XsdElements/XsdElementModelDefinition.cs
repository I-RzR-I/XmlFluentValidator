// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-26 12:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-26 15:39
// ***********************************************************************
//  <copyright file="XsdElementModelDefinition.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using XmlFluentValidator.Enums;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

#endregion

namespace XmlFluentValidator.Models.XsdElements
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An XSD element model definition.
    /// </summary>
    /// =================================================================================================
    public class XsdElementModelDefinition
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the element name.
        /// </summary>
        /// <value>
        ///     The element name.
        /// </value>
        /// =================================================================================================
        public string Name { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the element path.
        /// </summary>
        /// <value>
        ///     The element path.
        /// </value>
        /// =================================================================================================
        public string Path { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the minimum occurs.
        /// </summary>
        /// <value>
        ///     The minimum occurs.
        /// </value>
        /// =================================================================================================
        public int? MinOccurs { get; set; }

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
        ///     Gets or sets the type of the value.
        /// </summary>
        /// <value>
        ///     The type of the value.
        /// </value>
        /// =================================================================================================
        public XmlValidationDataTypeKind? LengthValueType { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the type of the value.
        /// </summary>
        /// <value>
        ///     The type of the value.
        /// </value>
        /// =================================================================================================
        public XmlValidationDataTypeKind? ValueType { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the element constraints.
        /// </summary>
        /// <value>
        ///     The element constraints.
        /// </value>
        /// =================================================================================================
        public XsdValueConstraintModelDefinition Constraints { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the element attributes.
        /// </summary>
        /// <value>
        ///     The element attributes.
        /// </value>
        /// =================================================================================================
        public Dictionary<string, XsdAttributeModelDefinition> Attributes { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the element children.
        /// </summary>
        /// <value>
        ///     The element children.
        /// </value>
        /// =================================================================================================
        public Dictionary<string, XsdElementModelDefinition> Children { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The element documentation.
        /// </summary>
        /// =================================================================================================
        public string Documentation { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether this object is nullable.
        /// </summary>
        /// <value>
        ///     True if this object is nullable, false if not.
        /// </value>
        /// =================================================================================================
        public bool? IsNullable { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="XsdElementModelDefinition"/> class.
        /// </summary>
        /// =================================================================================================
        public XsdElementModelDefinition()
        {
            Constraints = new XsdValueConstraintModelDefinition();
            Attributes = new Dictionary<string, XsdAttributeModelDefinition>();
            Children = new Dictionary<string, XsdElementModelDefinition>();
        }
    }
}