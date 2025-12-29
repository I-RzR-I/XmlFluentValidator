// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-26 14:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-26 16:20
// ***********************************************************************
//  <copyright file="XsdElementBuilderHelper.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using DomainCommonExtensions.Resources.Enums;
using DomainCommonExtensions.Utilities.Ensure;
using XmlFluentValidator.Models.XsdElements;

// ReSharper disable UseCollectionExpression

#endregion

namespace XmlFluentValidator.Helpers.Internal.Xsd;

/// -------------------------------------------------------------------------------------------------
/// <summary>
///     An XSD element builder helper.
/// </summary>
/// =================================================================================================
internal class XsdElementBuilderHelper
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     (Immutable) full pathname of the by file.
    /// </summary>
    /// =================================================================================================
    private readonly Dictionary<string, XsdElementModelDefinition> _byPath = new(StringComparer.Ordinal);

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     The instance.
    /// </summary>
    /// =================================================================================================
    public static readonly XsdElementBuilderHelper Instance = new XsdElementBuilderHelper();

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Prevents a default instance of the <see cref="XsdElementBuilderHelper"/> class from being
    ///     created.
    /// </summary>
    /// =================================================================================================
    private XsdElementBuilderHelper() { }

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Gets or create.
    /// </summary>
    /// <exception cref="ArgumentException">
    ///     Thrown when one or more arguments have unsupported or illegal values.
    /// </exception>
    /// <param name="path">The Element/attribute path.</param>
    /// <returns>
    ///     The element from get or create.
    /// </returns>
    /// =================================================================================================
    public XsdElementModelDefinition GetOrCreate(string path)
    {
        if (_byPath.TryGetValue(path, out var existing))
            return existing;

        var parts = path.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length.IsZero())
            DomainEnsure.ThrowException($"Invalid path ({nameof(path)})", ExceptionType.ArgumentException);

        XsdElementModelDefinition parent = null;
        var built = new List<string>();

        foreach (var part in parts.NotNull())
        {
            built.Add(part);
            var currentPath = string.Join("/", built);

            if (_byPath.TryGetValue(currentPath, out var elementDefinition))
            {
                parent = elementDefinition;
                continue;
            }

            elementDefinition = new XsdElementModelDefinition
            {
                Name = part,
                Path = currentPath
            };

            _byPath[currentPath] = elementDefinition;

            parent?.Children.Add(part, elementDefinition);
            parent = elementDefinition;
        }

        return parent;
    }

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Gets a root element definition.
    /// </summary>
    /// <param name="rootName">Name of the root element.</param>
    /// <returns>
    ///     The root.
    /// </returns>
    /// =================================================================================================
    public XsdElementModelDefinition GetRoot(string rootName) 
        => _byPath[rootName];
}