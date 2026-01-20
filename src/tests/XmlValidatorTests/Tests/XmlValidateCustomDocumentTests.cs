// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlValidatorTests
//  Author           : RzR
//  Created On       : 2026-01-20 18:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-20 20:27
// ***********************************************************************
//  <copyright file="XmlValidateCustomDocumentTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Xml.Linq;
using DomainCommonExtensions.DataTypeExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlFluentValidator;
using XmlFluentValidator.Enums;

#endregion

namespace XmlValidatorTests.Tests
{
    [TestClass]
    public class XmlValidateCustomDocumentTests
    {
        [TestMethod]
        public void Validate_Form_Report()
        {
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<Report Period=""0000-00"" ID=""00000000000000000000"">
<Form name=""usr-form-01"">
	<Row id=""1"" Quantity=""10"" Price=""11.01"" Discount=""0"" />
	<Row id=""2"" Quantity=""1"" Price=""101.01"" Discount=""0"" />
	<Row id=""3"" Quantity=""2"" Price=""1.01"" Discount=""1.5"" />
</Form>
<Form name=""usr-form-02"">
	<Row id=""1"" Quantity=""10"" Price=""11.01"" />
	<Row id=""2"" Quantity=""44"" Price=""811.00"" />
</Form>
</Report>";

            var validator = new XmlValidator("Report")
                .ForPath("Report")
                    .WithElementRequired()
                    .WithElementMaxOccurs(1)

                    .WithAttributeRequired("Period")
                    .WithAttributeRequired("ID")
                .Done()
                .ForPath("Report/Form")
                    .WithElementRequired()
                    .WithElementMaxOccurs(2)

                    .WithAttributeRequired("name")
                .Done()
                .ForPath("Report/Form/Row")
                    .WithElementRequired()

                    .WithAttributeRequired("id")
                    .WithAttributeValueRequired("Quantity")
                    .WithAttributeValueRequired("Price")
                    .WithAttributeDataType("Price", XmlValidationDataTypeKind.Decimal)
                .Done();

            var validationResult = validator.Validate(XDocument.Parse(xml));

            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
            Assert.IsTrue(validationResult.Errors.Count.IsZero());

            var xsdInstance = XsdGenerator.Instance;
            var xsd1 = xsdInstance.Generate(validator, "Report", null);
            var x12 = validator.UseSchema(xsdInstance.GetSchemaSet(xsd1));
            var xsdAsString = xsdInstance.GetSchemaString(xsd1);
            var result3 = x12.Validate(XDocument.Parse(xml));

            Assert.IsTrue(result3.IsValid);
        }
    }
}