// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-26 23:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-26 23:45
// ***********************************************************************
//  <copyright file="XUnsupportedRegexException.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace XmlFluentValidator.Exceptions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Exception for signalling unsupported RegEx errors.
    /// </summary>
    /// <seealso cref="T:Exception"/>
    /// =================================================================================================
    public class XUnsupportedRegexException : XmlFluentValidatorException
    {
        /// <inheritdoc />
        public XUnsupportedRegexException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public XUnsupportedRegexException(string message, params object[] args)
            : base(message, args)
        { }
    }
}

