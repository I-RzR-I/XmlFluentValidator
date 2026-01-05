// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2026-01-05 20:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-05 20:35
// ***********************************************************************
//  <copyright file="XMissingArgException.cs" company="RzR SOFT & TECH">
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
    ///     Exception for signalling step path errors.
    /// </summary>
    /// <seealso cref="T:XmlFluentValidator.Exceptions.XmlFluentValidatorException" />
    /// =================================================================================================
    public class XMissingArgException : XmlFluentValidatorException
    {
        /// <inheritdoc />
        protected XMissingArgException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        protected XMissingArgException(string message, params object[] args)
            : base(message, args)
        { }
    }
}