// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2026-01-05 20:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-05 20:29
// ***********************************************************************
//  <copyright file="XApplyInvalidOperationException.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

// ReSharper disable ClassNeverInstantiated.Global

namespace XmlFluentValidator.Exceptions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Exception for signalling apply invalid operation errors.
    /// </summary>
    /// <seealso cref="T:XmlFluentValidator.Exceptions.XmlFluentValidatorException"/>
    /// =================================================================================================
    public class XApplyInvalidOperationException : XmlFluentValidatorException
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="XApplyInvalidOperationException"/> class.
        /// </summary>
        /// =================================================================================================
        public XApplyInvalidOperationException()
        { }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="XApplyInvalidOperationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// =================================================================================================
        public XApplyInvalidOperationException(string message) : base(message)
        { }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="XApplyInvalidOperationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">A variable-length parameters list containing arguments.</param>
        /// =================================================================================================
        public XApplyInvalidOperationException(string message, params object[] args) : base(message, args)
        { }
    }
}

