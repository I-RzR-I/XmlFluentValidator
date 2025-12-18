// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-17 19:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-18 18:57
// ***********************************************************************
//  <copyright file="MessageArguments.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;

#endregion

namespace XmlFluentValidator.Models.Message
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Arguments for message. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="T:System.Collections.Generic.Dictionary{String,Object}"/>
    /// =================================================================================================
    public sealed class MessageArguments : Dictionary<string, object>
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Get message arguments from the given pairs.
        /// </summary>
        /// <param name="pairs">A variable-length parameters list containing pairs.</param>
        /// <returns>
        ///     The MessageArguments.
        /// </returns>
        /// =================================================================================================
        public static MessageArguments From(params (string Key, object Value)[] pairs)
        {
            var args = new MessageArguments();

            foreach (var (k, v) in pairs)
                args[k] = v;

            return args;
        }
    }
}