// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2026-01-05 19:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-05 19:51
// ***********************************************************************
//  <copyright file="XInvalidPathException.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

// ReSharper disable ClassNeverInstantiated.Global
// 
namespace XmlFluentValidator.Exceptions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Exception for signalling invalid path errors.
    /// </summary>
    /// <seealso cref="T:XmlFluentValidator.Exceptions.XmlFluentValidatorException"/>
    /// =================================================================================================
    public class XInvalidPathException : XmlFluentValidatorException
    {
        /// <inheritdoc />
        protected XInvalidPathException(string message) 
            : base(message)
        { }

        /// <inheritdoc />
        protected XInvalidPathException(string message, params object[] args) 
            : base(message, args)
        { }
    }
}

