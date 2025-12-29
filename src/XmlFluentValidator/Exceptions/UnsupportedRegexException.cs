// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-26 23:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-26 23:45
// ***********************************************************************
//  <copyright file="UnsupportedRegexException.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;

#endregion

namespace XmlFluentValidator.Exceptions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Exception for signalling unsupported RegEx errors.
    /// </summary>
    /// <seealso cref="T:Exception"/>
    /// =================================================================================================
    public class UnsupportedRegexException : Exception
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="UnsupportedRegexException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// =================================================================================================
        public UnsupportedRegexException(string message) : base(message)
        {
        }
    }
}

