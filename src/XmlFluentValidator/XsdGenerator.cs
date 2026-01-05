// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-26 13:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-26 19:23
// ***********************************************************************
//  <copyright file="XsdGenerator.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.CommonExtensions.TypeParam;
using DomainCommonExtensions.DataTypeExtensions;
using DomainCommonExtensions.Utilities.Ensure;
using XmlFluentValidator.Enums;
using XmlFluentValidator.Extensions;
using XmlFluentValidator.Helpers.Internal.Xsd;
using XmlFluentValidator.Models;
using XmlFluentValidator.Models.XsdElements;
using XmlFluentValidator.Rules;

// ReSharper disable RedundantCaseLabel

#endregion

namespace XmlFluentValidator
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An XSD generator.
    /// </summary>
    /// =================================================================================================
    public sealed class XsdGenerator
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the xs namespace.
        /// </summary>
        /// =================================================================================================
        private const string XsNs = "http://www.w3.org/2001/XMLSchema";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the instance.
        /// </summary>
        /// =================================================================================================
        public static readonly XsdGenerator Instance = new XsdGenerator();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Prevents a default instance of the <see cref="XsdGenerator"/> class from being
        ///     created.
        /// </summary>
        /// =================================================================================================
        private XsdGenerator() { }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generates the XML schema (XSD).
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="validator">The validator.</param>
        /// <param name="rootElementName">Name of the root element.</param>
        /// <param name="targetNs">Target ns.</param>
        /// <returns>
        ///     An XmlSchema.
        /// </returns>
        /// =================================================================================================
        public XmlSchema Generate(XmlValidator validator, string rootElementName, string targetNs)
        {
            DomainEnsure.IsNotNull(validator, nameof(validator));

            var schema = new XmlSchema
            {
                Version = "1.0",
                TargetNamespace = targetNs.IfNullOrWhiteSpace(XsNs),
                ElementFormDefault = XmlSchemaForm.Qualified
            };

            schema.Namespaces.Add("xs", XsNs);

            var allRecordedSteps = new List<XmlStepRecorder>();
            var rules = validator.Rules.OfType<XmlValidatorCompositeRule>();

            foreach (var rule in rules.NotNull())
                allRecordedSteps.AddRange(rule.RecordedSteps);

            var rootElement = XsdModelCompilerHelper.Instance.Compile(allRecordedSteps, rootElementName);

            schema.Items.Add(EmitElement(rootElement, true));

            return schema;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets XML schema set (XmlSchemaSet) from XML schema (XmlSchema).
        /// </summary>
        /// <param name="schemaSource">The XSD schema source.</param>
        /// <returns>
        ///     The XML schema set.
        /// </returns>
        /// =================================================================================================
        public XmlSchemaSet GetSchemaSet(XmlSchema schemaSource)
            => schemaSource.ToSchemaSet();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets XML schema (XSD) as string.
        /// </summary>
        /// <param name="schemaSource">The schema source.</param>
        /// <returns>
        ///     The XML schema (XSD) string.
        /// </returns>
        /// =================================================================================================
        public string GetSchemaString(XmlSchema schemaSource)
            => schemaSource.XmlSchemaToString();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Emit XML schema element.
        /// </summary>
        /// <param name="elementDefinition">The element definition.</param>
        /// <param name="isRoot">True if is root, false if not.</param>
        /// <returns>
        ///     An XmlSchemaElement.
        /// </returns>
        /// =================================================================================================
        private XmlSchemaElement EmitElement(XsdElementModelDefinition elementDefinition, bool isRoot)
        {
            var schemaElement = new XmlSchemaElement
            {
                Name = elementDefinition.Name
            };

            if (isRoot.IsFalse())
            {
                schemaElement.MinOccurs = elementDefinition.MinOccurs ?? 1;
                schemaElement.MaxOccursString = elementDefinition.MaxOccurs == int.MaxValue
                    ? "unbounded"
                    : elementDefinition.MaxOccurs?.ToString();
            }

            if (elementDefinition.Children.Any() || elementDefinition.Attributes.Any())
                schemaElement.SchemaType = EmitComplexType(elementDefinition);
            else
                schemaElement.SchemaType = EmitSimpleType(elementDefinition);

            return schemaElement;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Emit XML schema simple type.
        /// </summary>
        /// <param name="elementDefinition">The element definition.</param>
        /// <returns>
        ///     An XmlSchemaSimpleType.
        /// </returns>
        /// =================================================================================================
        private XmlSchemaSimpleType EmitSimpleType(XsdElementModelDefinition elementDefinition)
        {
            var typeOfValue = (elementDefinition.ValueType ?? elementDefinition.LengthValueType)
                .IfIsNull(XmlValidationDataTypeKind.String);

            var restriction = new XmlSchemaSimpleTypeRestriction
            {
                BaseTypeName = BuildBaseTypeName(typeOfValue)
            };

            EmitFacets(restriction.Facets, elementDefinition.Constraints);

            return new XmlSchemaSimpleType
            {
                Content = restriction
            };
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Emit complex type.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the requested operation is invalid.
        /// </exception>
        /// <param name="elementDefinition">The element definition.</param>
        /// <returns>
        ///     An XmlSchemaComplexType.
        /// </returns>
        /// =================================================================================================
        private XmlSchemaComplexType EmitComplexType(XsdElementModelDefinition elementDefinition)
        {
            var hasValue = elementDefinition.ValueType.HasValue;
            var hasChildren = elementDefinition.Children.Any();
            var hasAttributes = elementDefinition.Attributes.Any();
            var hasFacets = elementDefinition.Constraints.IsNull();

            // -------------------------------
            // Enforce invalid combinations
            // -------------------------------
            if (hasChildren.IsTrue() && hasValue.IsTrue())
                throw new InvalidOperationException(
                    $"Element '{elementDefinition.Name}' cannot have both value and child elements.");

            if (hasChildren.IsTrue() && hasFacets.IsTrue())
                throw new InvalidOperationException(
                    $"Element '{elementDefinition.Name}' cannot have facets when child elements exist.");

            // -------------------------------
            // Complex with children
            // -------------------------------

            if (hasChildren.IsTrue())
            {
                var complexType = new XmlSchemaComplexType();
                var seq = new XmlSchemaSequence();

                foreach (var child in elementDefinition.Children.Values)
                    seq.Items.Add(EmitElement(child, false));

                complexType.Particle = seq;

                foreach (var attr in elementDefinition.Attributes.Values)
                    complexType.Attributes.Add(EmitAttribute(attr));

                return complexType;
            }

            // -------------------------------
            // Value + attributes
            // -------------------------------

            if (hasValue.IsTrue() && hasAttributes.IsTrue())
            {
                var simpleContent = new XmlSchemaSimpleContent();

                // Use extension ONLY when no facets exist
                if (hasFacets.IsFalse())
                {
                    var typeOfValue = (elementDefinition.ValueType ?? elementDefinition.LengthValueType)
                        .IfIsNull(XmlValidationDataTypeKind.String);
                    var extension = new XmlSchemaSimpleContentExtension
                    {
                        BaseTypeName = BuildBaseTypeName(typeOfValue)
                    };

                    foreach (var attr in elementDefinition.Attributes.Values)
                        extension.Attributes.Add(EmitAttribute(attr));

                    simpleContent.Content = extension;
                }
                else
                {
                    var typeOfValue = (elementDefinition.ValueType ?? elementDefinition.LengthValueType)
                        .IfIsNull(XmlValidationDataTypeKind.String);
                    var restriction = new XmlSchemaSimpleContentRestriction
                    {
                        BaseTypeName = BuildBaseTypeName(typeOfValue)
                    };

                    EmitFacets(restriction.Facets, elementDefinition.Constraints);

                    simpleContent.Content = restriction;
                }

                return new XmlSchemaComplexType
                {
                    ContentModel = simpleContent
                };
            }

            // -------------------------------
            // Attributes only (no value, no children)
            // -------------------------------

            if (hasAttributes.IsTrue())
            {
                var complexType = new XmlSchemaComplexType();

                foreach (var attr in elementDefinition.Attributes.Values)
                    complexType.Attributes.Add(EmitAttribute(attr));

                return complexType;
            }

            // -------------------------------
            // Should never reach here
            // -------------------------------
            throw new InvalidOperationException(
                $"Invalid complex type state for element '{elementDefinition.Name}'.");
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Emit element/attribute facets.
        /// </summary>
        /// <param name="facets">The facets.</param>
        /// <param name="constraint">The constraint.</param>
        /// =================================================================================================
        private void EmitFacets(XmlSchemaObjectCollection facets,
            XsdValueConstraintModelDefinition constraint)
        {
            if (constraint.Pattern.IsPresent())
            {
                facets.Add(new XmlSchemaPatternFacet() { Value = constraint.Pattern });
            }

            if (constraint.MinLength.HasValue)
            {
                facets.Add(new XmlSchemaMinLengthFacet() { Value = constraint.MinLength.Value.ToString() });
            }

            if (constraint.MaxLength.HasValue)
            {
                facets.Add(new XmlSchemaMaxLengthFacet() { Value = constraint.MaxLength.Value.ToString() });
            }

            if (constraint.ExactLength.HasValue)
            {
                facets.Add(new XmlSchemaLengthFacet() { Value = constraint.ExactLength.Value.ToString() });
            }

            if (constraint.MinInclusive.HasValue)
            {
                facets.Add(new XmlSchemaMinInclusiveFacet() { Value = constraint.MinInclusive.Value.ToString() });
            }

            if (constraint.MaxInclusive.HasValue)
            {
                facets.Add(new XmlSchemaMaxInclusiveFacet() { Value = constraint.MaxInclusive.Value.ToString() });
            }

            if (constraint.MinExclusive.HasValue)
            {
                facets.Add(new XmlSchemaMinExclusiveFacet() { Value = constraint.MinExclusive.Value.ToString() });
            }

            if (constraint.MaxExclusive.HasValue)
            {
                facets.Add(new XmlSchemaMaxExclusiveFacet() { Value = constraint.MaxExclusive.Value.ToString() });
            }

            if (constraint.EnumerationValues.IsNotNullOrEmptyEnumerable())
            {
                foreach (var enumValue in constraint.EnumerationValues.NotNull())
                {
                    facets.Add(new XmlSchemaEnumerationFacet() { Value = enumValue });
                }
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Emit XML schema attribute.
        /// </summary>
        /// <param name="attributeDefinition">The attribute definition.</param>
        /// <returns>
        ///     An XmlSchemaAttribute.
        /// </returns>
        /// =================================================================================================
        private XmlSchemaAttribute EmitAttribute(XsdAttributeModelDefinition attributeDefinition)
        {
            var typeOfValue = attributeDefinition.ValueType.IfIsNull(XmlValidationDataTypeKind.String);

            DomainEnsure.IsNotNull(attributeDefinition, nameof(attributeDefinition));

            var xmlAttribute = new XmlSchemaAttribute
            {
                Name = attributeDefinition.Name,
                Use = attributeDefinition.IsRequired.IsTrue()
                    ? XmlSchemaUse.Required
                    : XmlSchemaUse.Optional
            };

            // Attributes must always be simple types
            var restriction = new XmlSchemaSimpleTypeRestriction
            {
                BaseTypeName = BuildBaseTypeName(typeOfValue)
            };

            EmitFacets(restriction.Facets, attributeDefinition.Constraints);

            xmlAttribute.SchemaType = new XmlSchemaSimpleType
            {
                Content = restriction,
            };

            return xmlAttribute;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Builds base type name.
        /// </summary>
        /// <param name="dataTypeKind">The data type kind.</param>
        /// <returns>
        ///     An XmlQualifiedName.
        /// </returns>
        /// =================================================================================================
        private static XmlQualifiedName BuildBaseTypeName(XmlValidationDataTypeKind? dataTypeKind)
        {
            string xsdType;
            switch (dataTypeKind)
            {
                case XmlValidationDataTypeKind.Date:
                    xsdType = XmlValidationDataTypeKind.Date.GetDescription();
                    break;
                case XmlValidationDataTypeKind.DateTime:
                    xsdType = XmlValidationDataTypeKind.DateTime.GetDescription();
                    break;
                case XmlValidationDataTypeKind.Bool:
                    xsdType = XmlValidationDataTypeKind.Bool.GetDescription();
                    break;
                case XmlValidationDataTypeKind.Float:
                    xsdType = XmlValidationDataTypeKind.Float.GetDescription();
                    break;
                case XmlValidationDataTypeKind.Decimal:
                    xsdType = XmlValidationDataTypeKind.Decimal.GetDescription();
                    break;
                case XmlValidationDataTypeKind.Integer:
                    xsdType = XmlValidationDataTypeKind.Integer.GetDescription();
                    break;
                case XmlValidationDataTypeKind.Double:
                    xsdType = XmlValidationDataTypeKind.Double.GetDescription();
                    break;
                case XmlValidationDataTypeKind.AnyUri:
                    xsdType = XmlValidationDataTypeKind.AnyUri.GetDescription();
                    break;
                case XmlValidationDataTypeKind.String:
                case null:
                default:
                    xsdType = XmlValidationDataTypeKind.String.GetDescription();
                    break;
            }

            var type = new XmlQualifiedName(xsdType, XsNs);

            return type;
        }
    }
}
