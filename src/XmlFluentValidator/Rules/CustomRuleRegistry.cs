// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-09 20:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-09 20:46
// ***********************************************************************
//  <copyright file="CustomRuleRegistry.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Xml.Linq;

#endregion

namespace XmlFluentValidator.Rules
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A custom rule registry.
    /// </summary>
    /// =================================================================================================
    public class CustomRuleRegistry
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the rules.
        /// </summary>
        /// =================================================================================================
        private static readonly IDictionary<string, Func<XElement, IDictionary<string, string>, bool>> Rules
            = new Dictionary<string, Func<XElement, IDictionary<string, string>, bool>>();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Registers this object.
        /// </summary>
        /// <param name="name">The name to get.</param>
        /// <param name="predicate">The predicate.</param>
        /// =================================================================================================
        public static void Register(string name, Func<XElement, IDictionary<string, string>, bool> predicate)
        {
            Rules[name] = predicate;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a func&lt; x element, i dictionary&lt;string,string?&gt;,bool&gt;? using
        ///     the given name.
        /// </summary>
        /// <param name="name">The name to get.</param>
        /// <returns>
        ///     A Func&lt;XElement,IDictionary&lt;string,string?&gt;,bool&gt;?
        /// </returns>
        /// =================================================================================================
        public static Func<XElement, IDictionary<string, string>, bool> Get(string name)
        {
            return Rules.TryGetValue(name, out var rule) ? rule : null;
        }
    }
}