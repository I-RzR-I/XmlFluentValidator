// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2026-01-05 21:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-05 21:41
// ***********************************************************************
//  <copyright file="XEmitInvalidOperationException.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace XmlFluentValidator.Exceptions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Exception for signalling emit invalid operation errors.
    /// </summary>
    /// <seealso cref="T:XmlFluentValidator.Exceptions.XmlFluentValidatorException"/>
    /// =================================================================================================
    public class XEmitInvalidOperationException : XmlFluentValidatorException
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="XEmitInvalidOperationException"/> class.
        /// </summary>
        /// =================================================================================================
        public XEmitInvalidOperationException()
        { }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="XEmitInvalidOperationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// =================================================================================================
        public XEmitInvalidOperationException(string message) : base(message)
        { }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="XEmitInvalidOperationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">A variable-length parameters list containing arguments.</param>
        /// =================================================================================================
        public XEmitInvalidOperationException(string message, params object[] args) : base(message, args)
        { }

    }
}

