// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-09 20:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-09 20:57
// ***********************************************************************
//  <copyright file="IXmlValidatorRule.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace XmlFluentValidator.Abstractions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for XML validator rule.
    /// </summary>
    /// =================================================================================================
    public interface IXmlValidatorRule
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Evaluates the given context.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// =================================================================================================
        void Evaluate(XmlValidationContext ctx);
    }
}