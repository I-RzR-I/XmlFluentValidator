// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-17 18:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-18 19:55
// ***********************************************************************
//  <copyright file="XmlMessageFormatter.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.DataTypeExtensions;

// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace XmlFluentValidator.Helpers.Internal
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Encapsulates the result of an XML validation message.
    /// </summary>
    /// =================================================================================================
    internal class XmlMessageFormatter
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Formats.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        ///     The formatted value.
        /// </returns>
        /// =================================================================================================
        public static string Format(string template, object context)
        {
            var props = context.GetType().GetProperties();
            var msg = template;

            foreach (var p in props.NotNull())
            {
                var token = "{" + p.Name + "}";
                var value = p.GetValue(context)?.ToString().IfNullThenEmpty();
                msg = msg.Replace(token, value);
            }

            return msg;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Formats.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>
        ///     The formatted value.
        /// </returns>
        /// =================================================================================================
        public static string Format(string template, IReadOnlyDictionary<string, object> args)
        {
            if (template.IsMissing() || args.IsNullOrEmptyEnumerable())
                return template;

            var result = template;
            foreach (var kv in args.NotNull())
            {
                var token = "{" + kv.Key + "}";
                result = result.Replace(token, kv.Value?.ToString().IfNullThenEmpty());
            }

            return result;
        }
    }
}