// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-09 19:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-09 19:59
// ***********************************************************************
//  <copyright file="XmlValidationRuleKind.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace XmlFluentValidator.Enums
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Values that represent XML validation rule kinds.
    /// </summary>
    /// =================================================================================================
    public enum XmlValidationRuleKind
    {
        /* Element text rules */
        /// <summary>
        ///     An enum constant representing the element required option.
        /// </summary>
        ElementRequired,

        /// <summary>
        ///     An enum constant representing the element optional option.
        /// </summary>
        ElementOptional,

        /// <summary>
        ///     An enum constant representing the element RegEx option.
        /// </summary>
        ElementRegex,

        /// <summary>
        ///     An enum constant representing the element range int option.
        /// </summary>
        ElementRangeInt,

        /// <summary>
        ///     An enum constant representing the element unique option.
        /// </summary>
        ElementUnique,

        /// <summary>
        ///     An enum constant representing the element Maximum occurs option.
        /// </summary>
        ElementMaxOccurs,

        /// <summary>
        ///     An enum constant representing the element value length option.
        /// </summary>
        ElementValueLength,

        /// <summary>
        ///     An enum constant representing the element data type option.
        /// </summary>
        ElementDataType, 
        
        /// <summary>
        ///     An enum constant representing the element enumeration option.
        /// </summary>
        ElementEnumeration,
        
        /// <summary>
        ///     An enum constant representing the element value exact length option.
        /// </summary>
        ElementValueExactLength,

        /// <summary>
        ///     An enum constant representing the element documentation option.
        /// </summary>
        ElementDocumentation,

        /// <summary>
        ///     An enum constant representing the element nullable option.
        /// </summary>
        ElementNullable,

        /* Attribute rules */
        /// <summary>
        ///     An enum constant representing the attribute required option.
        /// </summary>
        AttributeRequired,

        /// <summary>
        ///     An enum constant representing the attribute RegEx option.
        /// </summary>
        AttributeRegex,

        /// <summary>
        ///     An enum constant representing the attribute range int option.
        /// </summary>
        AttributeRangeInt,

        /// <summary>
        ///     An enum constant representing the attribute unique option.
        /// </summary>
        AttributeUnique,

        /// <summary>
        ///     An enum constant representing the attribute value length option.
        /// </summary>
        AttributeValueLength,

        /// <summary>
        ///     An enum constant representing the attribute data type option.
        /// </summary>
        AttributeDataType,

        /// <summary>
        ///     An enum constant representing the attribute enumeration option.
        /// </summary>
        AttributeEnumeration,

        /// <summary>
        ///     An enum constant representing the attribute value exact length option.
        /// </summary>
        AttributeValueExactLength,

        /// <summary>
        ///     An enum constant representing the attribute documentation option.
        /// </summary>
        AttributeDocumentation,

        /* Runtime-only */
        /// <summary>
        ///     An enum constant representing the custom element option.
        /// </summary>
        CustomElement,

        /// <summary> 
        ///     An enum constant representing the element attribute cross option.
        /// </summary>
        ElementAttributeCross,

        /// <summary>
        ///     An enum constant representing the condition option.
        /// </summary>
        Condition
    }
}