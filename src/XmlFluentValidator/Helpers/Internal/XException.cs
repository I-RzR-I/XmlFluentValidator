// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2026-01-05 19:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-05 19:39
// ***********************************************************************
//  <copyright file="Xml Exception.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using XmlFluentValidator.Exceptions;

#endregion

namespace XmlFluentValidator.Helpers.Internal
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An exception throw helper.
    /// </summary>
    /// =================================================================================================
    internal static class XException
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Throws the T exception.
        /// </summary>
        /// <typeparam name="T">Generic exception type parameter.</typeparam>
        /// =================================================================================================
        public static void Throw<T>()
            where T : XmlFluentValidatorException
        {
            var exception = (T)Activator.CreateInstance(typeof(T))!;

            throw exception;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Throws the T exception.
        /// </summary>
        /// <typeparam name="T">Generic exception type parameter.</typeparam>
        /// <param name="message">The exception message.</param>
        /// =================================================================================================
        public static void Throw<T>(string message)
            where T : XmlFluentValidatorException
        {
            var exception = (T)Activator.CreateInstance(typeof(T), message)!;

            throw exception;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Throws the T exception.
        /// </summary>
        /// <typeparam name="T">Generic exception type parameter.</typeparam>
        /// <param name="message">The exception message.</param>
        /// <param name="args">A variable-length parameters list containing arguments.</param>
        /// =================================================================================================
        public static void Throw<T>(string message, params object[] args)
            where T : XmlFluentValidatorException
        {
            var exception = (T)Activator.CreateInstance(typeof(T), message, args)!;

            throw exception;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Throws the T exception.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="factory">The factory.</param>
        /// <param name="message">The exception message.</param>
        /// =================================================================================================
        public static void Throw<T>(
            Func<string, T> factory, string message)
            where T : XmlFluentValidatorException
        {
            throw factory(message);
        }
    }
}