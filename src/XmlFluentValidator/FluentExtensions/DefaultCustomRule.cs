// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-09 23:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-09 23:37
// ***********************************************************************
//  <copyright file="DefaultCustomRule.cs" company="RzR SOFT & TECH">
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
using DomainCommonExtensions.CommonExtensions;

#endregion

namespace XmlFluentValidator.FluentExtensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Default defined custom rules.
    /// </summary>
    /// =================================================================================================
    public static class DefaultCustomRule
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Rule: If attribute equals a value, element must satisfy predicate.
        /// </summary>
        /// <param name="attrName">Name of the attribute.</param>
        /// <param name="expectedValue">The expected value.</param>
        /// <param name="elementPredicate">The element predicate.</param>
        /// <returns>
        ///     A function delegate that yields a bool.
        /// </returns>
        /// =================================================================================================
        public static Func<XElement, IDictionary<string, string>, bool>
            AttributeConditional(string attrName, string expectedValue, Func<XElement, bool> elementPredicate)
        {
            return (elem, attrs) =>
            {
                var attr = attrs.TryGetValue(attrName, out var outAttributeValue) 
                    ? outAttributeValue
                    : null;

                if (attr == expectedValue)
                    return elementPredicate(elem);
                return true; // passes if condition not met
            };
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Rule: Element value must equal attribute value.
        /// </summary>
        /// <param name="attrName">Name of the attribute.</param>
        /// <returns>
        ///     A function delegate that yields a bool.
        /// </returns>
        /// =================================================================================================
        public static Func<XElement, IDictionary<string, string>, bool>
            ElementEqualsAttribute(string attrName)
        {
            return (elem, attrs) =>
            {
                var attr = attrs.TryGetValue(attrName, out var outAttributeValue) 
                    ? outAttributeValue 
                    : null;

                return elem.Value == attr;
            };
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Rule: If attribute starts with prefix, numeric element value must be &lt;= max.
        /// </summary>
        /// <param name="attrName">Name of the attribute.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="max">The maximum.</param>
        /// <returns>
        ///     A function delegate that yields a bool.
        /// </returns>
        /// =================================================================================================
        public static Func<XElement, IDictionary<string, string>, bool>
            AttributePrefixMax(string attrName, string prefix, int max)
        {
            return (elem, attrs) =>
            {
                var attr = attrs.TryGetValue(attrName, out var outAttributeValue)
                    ? outAttributeValue 
                    : null;

                if (attr.IsNotNull() && attr!.StartsWith(prefix)) 
                    return int.TryParse(elem.Value, out var v) && v <= max;

                return true;
            };
        }
    }
}