// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2026-01-05 20:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-05 20:21
// ***********************************************************************
//  <copyright file="XValidationRuleOutOfRangeException.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.DataTypeExtensions;
using XmlFluentValidator.Helpers.Internal;

// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace XmlFluentValidator.Exceptions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Exception for signalling validation rule out of range errors.
    /// </summary>
    /// <seealso cref="T:XmlFluentValidator.Exceptions.XmlFluentValidatorException"/>
    /// =================================================================================================
    public class XValidationRuleOutOfRangeException : XmlFluentValidatorException
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="XValidationRuleOutOfRangeException" />
        ///     class.
        /// </summary>
        /// =================================================================================================
        public XValidationRuleOutOfRangeException()
            : base(XDefaultMessages.XmlValidationRuleKind)
        { }

        /// <inheritdoc />
        protected XValidationRuleOutOfRangeException(string message)
            : base(message.IfNullOrWhiteSpace(XDefaultMessages.XmlValidationRuleKind))
        { }

        /// <inheritdoc />
        protected XValidationRuleOutOfRangeException(string message, params object[] args) : base(message, args)
        { }
    }
}