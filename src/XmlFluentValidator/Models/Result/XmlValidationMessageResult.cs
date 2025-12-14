// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-09 20:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-09 20:25
// ***********************************************************************
//  <copyright file="XmlValidationMessageResult.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace XmlFluentValidator.Models.Result
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Encapsulates the result of an XML validation message.
    /// </summary>
    /// =================================================================================================
    public class XmlValidationMessageResult
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

            foreach (var p in props)
            {
                var token = "{" + p.Name + "}";
                var value = p.GetValue(context)?.ToString() ?? "";
                msg = msg.Replace(token, value);
            }

            return msg;
        }
    }
}