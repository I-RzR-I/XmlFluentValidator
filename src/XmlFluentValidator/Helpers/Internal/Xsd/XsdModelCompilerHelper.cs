// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlFluentValidator
//  Author           : RzR
//  Created On       : 2025-12-26 14:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-26 18:36
// ***********************************************************************
//  <copyright file="XsdModelCompilerHelper.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using XmlFluentValidator.Enums;
using XmlFluentValidator.Exceptions;
using XmlFluentValidator.Extensions;
using XmlFluentValidator.Models;
using XmlFluentValidator.Models.XsdElements;

#endregion

namespace XmlFluentValidator.Helpers.Internal.Xsd
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An XSD model compiler helper.
    /// </summary>
    /// =================================================================================================
    internal class XsdModelCompilerHelper
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the builder.
        /// </summary>
        /// =================================================================================================
        private readonly XsdElementBuilderHelper _builder;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the instance.
        /// </summary>
        /// =================================================================================================
        public static readonly XsdModelCompilerHelper Instance = new XsdModelCompilerHelper();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Prevents a default instance of the <see cref="XsdModelCompilerHelper"/> class from being
        ///     created.
        /// </summary>
        /// =================================================================================================
        private XsdModelCompilerHelper()
        {
            _builder = XsdElementBuilderHelper.Instance;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Compiles recorded steps to element definition.
        /// </summary>
        /// <param name="steps">The recorder steps.</param>
        /// <param name="rootPath">The root element path.</param>
        /// <returns>
        ///     An XsdElementModelDefinition.
        /// </returns>
        /// =================================================================================================
        public XsdElementModelDefinition Compile(IEnumerable<XmlStepRecorder> steps, string rootPath)
        {
            foreach (var step in steps)
                ApplyStep(step);

            return _builder.GetRoot(rootPath);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Applies the step described by xml recorded step.
        /// </summary>
        /// <exception cref="XMissingArgException" />
        /// <exception cref="XValidationRuleOutOfRangeException" />
        /// <param name="step">The xml recorded step.</param>
        /// =================================================================================================
        private void ApplyStep(XmlStepRecorder step)
        {
            if (step.Path.IsMissing())
                XException.Throw<XMissingArgException>(XDefaultMessages.StepWithOutPath);

            var element = _builder.GetOrCreate(step.Path);

            switch (step.Kind)
            {
                case XmlValidationRuleKind.ElementRequired:
                    element.MinOccurs = 1;
                    break;

                case XmlValidationRuleKind.ElementOptional:
                    element.MinOccurs = 0;
                    break;

                case XmlValidationRuleKind.ElementMaxOccurs:
                    element.MaxOccurs = step.MaxOccurs;
                    break;

                case XmlValidationRuleKind.ElementRegex:
                    ApplyElementRegex(element, step);
                    break;

                case XmlValidationRuleKind.ElementRangeInt:
                    ApplyElementRange(element, step);
                    break;

                case XmlValidationRuleKind.ElementValueLength:
                    ApplyElementLength(element, step);
                    break;

                case XmlValidationRuleKind.ElementDataType:
                    ApplyElementDataType(element, step);
                    break;

                case XmlValidationRuleKind.ElementEnumeration:
                    ApplyElementEnumeration(element, step);
                    break;

                case XmlValidationRuleKind.ElementValueExactLength:
                    ApplyElementExactLength(element, step);
                    break;

                case XmlValidationRuleKind.AttributeRequired:
                    ApplyAttributeRequired(element, step);
                    break;

                case XmlValidationRuleKind.AttributeRegex:
                    ApplyAttributeRegex(element, step);
                    break;

                case XmlValidationRuleKind.AttributeRangeInt:
                    ApplyAttributeRange(element, step);
                    break;

                case XmlValidationRuleKind.AttributeValueLength:
                    ApplyAttributeLength(element, step);
                    break;

                case XmlValidationRuleKind.AttributeDataType:
                    ApplyAttributeDataType(element, step);
                    break;

                case XmlValidationRuleKind.AttributeEnumeration:
                    ApplyAttributeEnumeration(element, step);
                    break;

                case XmlValidationRuleKind.AttributeValueExactLength:
                    ApplyAttributeExactLength(element, step);
                    break;

                case XmlValidationRuleKind.CustomElement:
                case XmlValidationRuleKind.Condition:
                case XmlValidationRuleKind.ElementAttributeCross:
                case XmlValidationRuleKind.ElementUnique:
                case XmlValidationRuleKind.AttributeUnique:
                    element.Documentation ??= step.Descriptor?.DefaultTemplate;
                    break;

                default:
                    XException.Throw<XValidationRuleOutOfRangeException>();
                    break;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Applies the element exact length.
        /// </summary>
        /// <exception cref="XApplyInvalidOperationException" />
        /// <param name="element">The element definition.</param>
        /// <param name="step">The xml recorded step.</param>
        /// =================================================================================================
        private void ApplyElementExactLength(XsdElementModelDefinition element, XmlStepRecorder step)
        {
            element.LengthValueType ??= XmlValidationDataTypeKind.String;

            if (element.LengthValueType.IsEqualTo(XmlValidationDataTypeKind.String).IsFalse())
                XException.Throw<XApplyInvalidOperationException>(XDefaultMessages.ElementExactLengthApplyToNotString, element.Path);

            if (step.ValueExactLength.IsNotNull())
                element.Constraints.ExactLength = step.ValueExactLength;

            if (step.AnnotationDescription.IsPresent())
                element.Documentation = step.AnnotationDescription;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Applies the attribute exact length.
        /// </summary>
        /// <exception cref="XMissingArgException" />
        /// <exception cref="XApplyInvalidOperationException" />
        /// <param name="element">The element definition.</param>
        /// <param name="step">The xml recorded step.</param>
        /// =================================================================================================
        private void ApplyAttributeExactLength(XsdElementModelDefinition element, XmlStepRecorder step)
        {
            if (step.AttributeName.IsMissing())
                XException.Throw<XMissingArgException>(XDefaultMessages.AttributeNameMissing);

            var attr = GetOrCreateAttribute(element, step.AttributeName);
            attr.ValueType ??= XmlValidationDataTypeKind.String;

            if (attr.ValueType.IsEqualTo(XmlValidationDataTypeKind.String).IsFalse())
                XException.Throw<XApplyInvalidOperationException>(XDefaultMessages.AttributeExactLengthApplyToNotString, attr.Name);

            if (step.ValueExactLength.IsNotNull())
                attr.Constraints.ExactLength = step.ValueExactLength;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Applies the element enumeration.
        /// </summary>
        /// <param name="element">The element definition.</param>
        /// <param name="step">The xml recorded step.</param>
        /// =================================================================================================
        private void ApplyElementEnumeration(XsdElementModelDefinition element, XmlStepRecorder step)
        {
            if (step.InRangeEnumerator.IsNotNullOrEmptyEnumerable())
                element.Constraints.EnumerationValues = step.InRangeEnumerator;

            if (step.AnnotationDescription.IsPresent())
                element.Documentation = step.AnnotationDescription;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Applies the attribute enumeration.
        /// </summary>
        /// <exception cref="XMissingArgException" />
        /// <param name="element">The element definition.</param>
        /// <param name="step">The xml recorded step.</param>
        /// =================================================================================================
        private void ApplyAttributeEnumeration(XsdElementModelDefinition element, XmlStepRecorder step)
        {
            if (step.AttributeName.IsMissing())
                XException.Throw<XMissingArgException>(XDefaultMessages.AttributeNameMissing);

            var attr = GetOrCreateAttribute(element, step.AttributeName);

            if (step.InRangeEnumerator.IsNotNullOrEmptyEnumerable())
                attr.Constraints.EnumerationValues = step.InRangeEnumerator;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Applies the attribute data type.
        /// </summary>
        /// <exception cref="XMissingArgException" />
        /// <exception cref="XApplyInvalidOperationException" />
        /// <param name="element">The element definition.</param>
        /// <param name="step">The xml recorded step.</param>
        /// =================================================================================================
        private void ApplyAttributeDataType(XsdElementModelDefinition element, XmlStepRecorder step)
        {
            if (step.AttributeName.IsMissing())
                XException.Throw<XMissingArgException>(XDefaultMessages.AttributeNameMissing);

            var attr = GetOrCreateAttribute(element, step.AttributeName);

            var type = step.DataType;

            if (attr.ValueType.HasValue && attr.ValueType.IsEqualTo(type).IsFalse())
                XException.Throw<XApplyInvalidOperationException>(XDefaultMessages.AttributeConflictWithPath.FormatWith(attr.Name, element.Path));

            attr.ValueType = type;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Applies the element data type.
        /// </summary>
        /// <param name="element">The element definition.</param>
        /// <param name="step">The xml recorded step.</param>
        /// =================================================================================================
        private void ApplyElementDataType(XsdElementModelDefinition element, XmlStepRecorder step)
        {
            var type = step.DataType;

            if (element.ValueType.HasValue.IsFalse())
            {
                element.ValueType = type;
                return;
            }

            // Type already exists — must extend
            if (element.ValueType.IsEqualTo(type).IsFalse())
            {
                element.ValueType ??= element.LengthValueType;
                element.ValueType = type;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Applies the element RegEx.
        /// </summary>
        /// <exception cref="XApplyInvalidOperationException" />
        /// <param name="element">The element definition.</param>
        /// <param name="step">The xml recorded step.</param>
        /// =================================================================================================
        private static void ApplyElementRegex(XsdElementModelDefinition element, XmlStepRecorder step)
        {
            element.LengthValueType ??= XmlValidationDataTypeKind.String;

            if (element.LengthValueType.IsEqualTo(XmlValidationDataTypeKind.String).IsFalse())
                XException.Throw<XApplyInvalidOperationException>(XDefaultMessages.ElementRegexApplyToNotString.FormatWith(element.Path));

            element.Constraints.Pattern = XsdRegexTranslatorHelper.Translate(step.Pattern).XsdPattern;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Applies the element range.
        /// </summary>
        /// <exception cref="XApplyInvalidOperationException" />
        /// <param name="element">The element definition.</param>
        /// <param name="step">The xml recorded step.</param>
        /// =================================================================================================
        private static void ApplyElementRange(XsdElementModelDefinition element, XmlStepRecorder step)
        {
            element.LengthValueType ??= XmlValidationDataTypeKind.Integer;

            if (element.LengthValueType.IsEqualTo(XmlValidationDataTypeKind.Integer).IsFalse())
                XException.Throw<XApplyInvalidOperationException>(XDefaultMessages.ElementRangeApplyToNotInteger.FormatWith(element.Path));

            if (step.Min.HasValue.IsTrue())
            {
                if (step.IsInclusiveValidation.IsTrue())
                    element.Constraints.MinInclusive = step.Min;
                else
                    element.Constraints.MinExclusive = step.Min;
            }

            if (step.Max.HasValue.IsTrue())
            {
                if (step.IsInclusiveValidation.IsTrue())
                    element.Constraints.MaxInclusive = step.Max;
                else
                    element.Constraints.MaxExclusive = step.Max;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Applies the element length.
        /// </summary>
        /// <exception cref="XApplyInvalidOperationException" />
        /// <param name="element">The element definition.</param>
        /// <param name="step">The xml recorded step.</param>
        /// =================================================================================================
        private static void ApplyElementLength(XsdElementModelDefinition element, XmlStepRecorder step)
        {
            element.LengthValueType ??= XmlValidationDataTypeKind.String;

            if (element.LengthValueType.IsEqualTo(XmlValidationDataTypeKind.String).IsFalse())
                XException.Throw<XApplyInvalidOperationException>(XDefaultMessages.ElementLengthApplyToNotString.FormatWith(element.Path));

            element.Constraints.MinLength = step.Min;
            element.Constraints.MaxLength = step.Max;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Applies the attribute required.
        /// </summary>
        /// <exception cref="XMissingArgException" />
        /// <param name="element">The element definition.</param>
        /// <param name="step">The xml recorded step.</param>
        /// =================================================================================================
        private static void ApplyAttributeRequired(XsdElementModelDefinition element, XmlStepRecorder step)
        {
            if (step.AttributeName.IsMissing())
                XException.Throw<XMissingArgException>(XDefaultMessages.AttributeNameMissing);

            var attr = GetOrCreateAttribute(element, step.AttributeName);
            attr.IsRequired = true;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Applies the attribute RegEx.
        /// </summary>
        /// <exception cref="XMissingArgException" />
        /// <param name="element">The element definition.</param>
        /// <param name="step">The xml recorded step.</param>
        /// =================================================================================================
        private static void ApplyAttributeRegex(XsdElementModelDefinition element, XmlStepRecorder step)
        {
            if (step.AttributeName.IsMissing())
                XException.Throw<XMissingArgException>(XDefaultMessages.AttributeNameMissing);

            var attr = GetOrCreateAttribute(element, step.AttributeName);

            attr.ValueType ??= XmlValidationDataTypeKind.String;

            if (attr.ValueType.IsEqualTo(XmlValidationDataTypeKind.String).IsFalse())
                XException.Throw<XApplyInvalidOperationException>(XDefaultMessages.AttributeRegexApplyToNotString.FormatWith(attr.Name));

            attr.Constraints.Pattern = XsdRegexTranslatorHelper.Translate(step.Pattern).XsdPattern;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Applies the attribute range.
        /// </summary>
        /// <exception cref="XMissingArgException" />
        /// <exception cref="XApplyInvalidOperationException" />
        /// <param name="element">The element definition.</param>
        /// <param name="step">The xml recorded step.</param>
        /// =================================================================================================
        private static void ApplyAttributeRange(XsdElementModelDefinition element, XmlStepRecorder step)
        {
            if (step.AttributeName.IsMissing())
                XException.Throw<XMissingArgException>(XDefaultMessages.AttributeNameMissing);

            var attr = GetOrCreateAttribute(element, step.AttributeName);

            attr.ValueType ??= XmlValidationDataTypeKind.Integer;

            if (attr.ValueType.IsEqualTo(XmlValidationDataTypeKind.Integer).IsFalse())
                XException.Throw<XApplyInvalidOperationException>(XDefaultMessages.AttributeRangeApplyToNotInteger.FormatWith(attr.Name));

            if (step.Min.HasValue.IsTrue())
            {
                if (step.IsInclusiveValidation.IsTrue())
                    attr.Constraints.MinInclusive = step.Min;
                else
                    attr.Constraints.MinExclusive = step.Min;
            }

            if (step.Max.HasValue.IsTrue())
            {
                if (step.IsInclusiveValidation.IsTrue())
                    attr.Constraints.MaxInclusive = step.Max;
                else
                    attr.Constraints.MaxExclusive = step.Max;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Applies the attribute length.
        /// </summary>
        /// <exception cref="XMissingArgException" />
        /// <exception cref="XApplyInvalidOperationException" />
        /// <param name="element">The element definition.</param>
        /// <param name="step">The xml recorded step.</param>
        /// =================================================================================================
        private static void ApplyAttributeLength(XsdElementModelDefinition element, XmlStepRecorder step)
        {
            if (step.AttributeName.IsMissing())
                XException.Throw<XMissingArgException>(XDefaultMessages.AttributeNameMissing);

            var attr = GetOrCreateAttribute(element, step.AttributeName);

            attr.ValueType ??= XmlValidationDataTypeKind.String;

            if (attr.ValueType.IsEqualTo(XmlValidationDataTypeKind.String).IsFalse())
                XException.Throw<XApplyInvalidOperationException>(XDefaultMessages.AttributeLengthApplyToNotString.FormatWith(attr.Name));

            attr.Constraints.MinLength = step.Min;
            attr.Constraints.MaxLength = step.Max;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or create attribute.
        /// </summary>
        /// <param name="element">The element definition.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        ///     The or create attribute.
        /// </returns>
        /// =================================================================================================
        private static XsdAttributeModelDefinition GetOrCreateAttribute(
            XsdElementModelDefinition element,
            string name)
        {
            if (element.Attributes.TryGetValue(name, out var attr).IsFalse())
            {
                attr = new XsdAttributeModelDefinition { Name = name };
                element.Attributes.Add(name, attr);
            }

            return attr;
        }
    }
}