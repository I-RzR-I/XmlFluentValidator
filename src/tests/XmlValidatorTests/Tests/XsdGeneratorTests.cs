// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlValidatorTests
//  Author           : RzR
//  Created On       : 2025-12-21 18:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-21 18:12
// ***********************************************************************
//  <copyright file="XsdGeneratorTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using DomainCommonExtensions.DataTypeExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using XmlFluentValidator;
using XmlFluentValidator.Models.Message;

namespace XmlValidatorTests.Tests
{
    [TestClass]
    public class XsdGeneratorTests
    {
        [TestMethod]
        public void Validate_Pass_Test_3()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""ABC-001""  qty=""5"">Widget</item>
        <item sku=""ABC-002""  qty=""5"">Widget</item>
    </items>
</order>");

            var validator = new XmlValidator()
                .ForPath("/order/items/item")
                    .WithAttributeRequired("sku")
                    .WithAttribute("sku", v => v.IsPresent(), "Item SKU is required.")
                    .WithElementCount(c => c >= 2)
                    .WithMessage("Order must contain at least {Min} item(s).", MessageArguments.From(("Min", 1)))
                .Done()
                .ForPath("/order/id")
                    .WithElementValue(v => int.TryParse(v, out var n) && n > 0)
                    .WithMessage("Order Id '{Path}' is invalid", null)
                .Done();

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Errors.Count.IsZero());

            var xsdInstance = XsdGenerator.Instance;
            var xsdSchema = xsdInstance.Generate(validator, "order", null);
            var validator2 = validator.UseSchema(xsdInstance.GetSchemaSet(xsdSchema));
            var xsdAsString = xsdInstance.GetSchemaString(xsdSchema);
            var result2 = validator2.Validate(xml);

            Assert.IsTrue(result2.IsValid);

        }
    }
}