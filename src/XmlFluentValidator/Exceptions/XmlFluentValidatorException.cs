// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2026-01-05 17:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-05 17:45
// ***********************************************************************
//  <copyright file="XmlFluentValidatorException.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using DomainCommonExtensions.DataTypeExtensions;

// ReSharper disable MemberCanBeProtected.Global

#endregion

namespace XmlFluentValidator.Exceptions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Exception for signalling XML fluent validator errors.
    /// </summary>
    /// <seealso cref="T:Exception"/>
    /// =================================================================================================
    public class XmlFluentValidatorException : Exception
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="XmlFluentValidatorException"/> class.
        /// </summary>
        /// =================================================================================================
        public XmlFluentValidatorException() : base(FormatMessage(null))
        { }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="XmlFluentValidatorException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// =================================================================================================
        protected XmlFluentValidatorException(string message)
            : base(FormatMessage(message))
        { }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="XmlFluentValidatorException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">A variable-length parameters list containing arguments.</param>
        /// =================================================================================================
        protected XmlFluentValidatorException(string message, params object[] args)
            : base(FormatMessage(message, args))
        { }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Format message.
        /// </summary>
        /// <param name="errorMessage">Message describing the error.</param>
        /// <returns>
        ///     The formatted message.
        /// </returns>
        /// =================================================================================================
        protected static string FormatMessage(string errorMessage)
            => errorMessage.IfNullOrWhiteSpace("XML Fluent Validator ⇔ Unexcepted error occurred!");

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Format message.
        /// </summary>
        /// <param name="errorMessage">Message describing the error.</param>
        /// <param name="formatArgs">
        ///     A variable-length parameters list containing format arguments.
        /// </param>
        /// <returns>
        ///     The formatted message.
        /// </returns>
        /// =================================================================================================
        protected static string FormatMessage(string errorMessage, params object[] formatArgs)
        {
            var message = errorMessage.IfNullOrWhiteSpace(FormatMessage(null));

            return message.FormatWith(args: formatArgs);
        }
    }
}