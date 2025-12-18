// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlValidatorTests
//  Author           : RzR
//  Created On       : 2025-12-14 11:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-14 11:45
// ***********************************************************************
//  <copyright file="XmlValidatorMessageTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using DomainCommonExtensions.DataTypeExtensions;
using XmlFluentValidator;

#endregion

namespace XmlValidatorTests.Tests
{
    [TestClass]
    public class XmlValidatorMessageTests
    {
        [TestMethod]
        public void Validate_ForElement_MatchesRegex_WithMessage_Fail_OnItemSku_Test()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <id>123</id>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""A-001"" qty=""9"">19</item>
        <item sku=""ABC-001"" qty=""9"" type=""premium"">1900</item>
    </items>
    <discount >
        5
    </discount>
    <totalQty>
        9
    </totalQty>
    <totalAmount>
        9000
    </totalAmount>
    <billing>
        <customerId>123</customerId>
    </billing>
</order>");

            var validator = new XmlValidator("order")
                .ForElement("items/item@sku")
                .Required()
                .MatchesRegex(@"^[A-Z]{3}-\d{3}$").WithMessage("SKU validation fails.")

                .Done();

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("SKU validation fails.", result.Errors.FirstOrDefault()?.Message);
        }

        [TestMethod]
        public void Validate_ForElement_MatchesRegex_Message_Fail_OnItemSku_Test()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <id>123</id>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""A-001"" qty=""9"">19</item>
        <item sku=""ABC-001"" qty=""9"" type=""premium"">1900</item>
    </items>
    <discount >
        5
    </discount>
    <totalQty>
        9
    </totalQty>
    <totalAmount>
        9000
    </totalAmount>
    <billing>
        <customerId>123</customerId>
    </billing>
</order>");

            var validator = new XmlValidator("order")
                .ForElement("items/item@sku")
                .Required()
                .MatchesRegex(@"^[A-Z]{3}-\d{3}$", "SKU internal message").WithMessage("SKU validation fails.")

                .Done();

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("SKU validation fails.", result.Errors.FirstOrDefault()?.Message);
        }

        [TestMethod]
        public void Validate_ForElement_MatchesRegex_Message_Fail_OnItemSku_0_Test()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <id>123</id>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""A-001"" qty=""9"">19</item>
        <item sku=""ABC-001"" qty=""9"" type=""premium"">1900</item>
    </items>
    <discount >
        5
    </discount>
    <totalQty>
        9
    </totalQty>
    <totalAmount>
        9000
    </totalAmount>
    <billing>
        <customerId>123</customerId>
    </billing>
</order>");

            var validator = new XmlValidator("order")
                .ForElement("items/item@sku")
                .Required()
                .MatchesRegex(@"^[A-Z]{3}-\d{3}$", "SKU internal message")

                .Done();

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("SKU internal message", result.Errors.FirstOrDefault()?.Message);
        }

        [TestMethod]
        public void Validate_ForPath_MustExist_Fail_WithMessage_Test()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""ABC-001"" qty=""9"">19</item>
        <item sku=""ABC-001"" qty=""9"" type=""premium"">1900</item>
    </items>
    <discount type=""percentage"">
        5
    </discount>
    <totalQty>
        9
    </totalQty>
    <totalAmount>
        9000
    </totalAmount>
    <billing>
        <customerId>123</customerId>
    </billing>
</order>");

            var validator = new XmlValidator()
                .ForPath("/order/customer/id")
                .MustExist().WithMessage("Id is missing")
                .Done();

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Id is missing", result.Errors.FirstOrDefault()?.Message);
        }

        [TestMethod]
        public void Validate_ForPath_MustExist_Fail_Message_Test()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""ABC-001"" qty=""9"">19</item>
        <item sku=""ABC-001"" qty=""9"" type=""premium"">1900</item>
    </items>
    <discount type=""percentage"">
        5
    </discount>
    <totalQty>
        9
    </totalQty>
    <totalAmount>
        9000
    </totalAmount>
    <billing>
        <customerId>123</customerId>
    </billing>
</order>");

            var validator = new XmlValidator()
                .ForPath("/order/customer/id")
                .MustExist("No id").WithMessage("Id is missing")
                .Done();

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Id is missing", result.Errors.FirstOrDefault()?.Message);
        }

        [TestMethod]
        public void Validate_ForPath_MustExist_Fail_Message_0_Test()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""ABC-001"" qty=""9"">19</item>
        <item sku=""ABC-001"" qty=""9"" type=""premium"">1900</item>
    </items>
    <discount type=""percentage"">
        5
    </discount>
    <totalQty>
        9
    </totalQty>
    <totalAmount>
        9000
    </totalAmount>
    <billing>
        <customerId>123</customerId>
    </billing>
</order>");

            var validator = new XmlValidator()
                .ForPath("/order/customer/id")
                .MustExist("No id")
                .Done();

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("No id", result.Errors.FirstOrDefault()?.Message);
        }

        [TestMethod]
        public void Validate_ForPath_MustExist_Fail_No_Message_Test()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""ABC-001"" qty=""9"">19</item>
        <item sku=""ABC-001"" qty=""9"" type=""premium"">1900</item>
    </items>
    <discount type=""percentage"">
        5
    </discount>
    <totalQty>
        9
    </totalQty>
    <totalAmount>
        9000
    </totalAmount>
    <billing>
        <customerId>123</customerId>
    </billing>
</order>");

            var validator = new XmlValidator()
                .ForPath("/order/customer/id")
                .MustExist()
                .Done();

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.FirstOrDefault()?.Message.IsPresent());
        }

        [TestMethod]
        public void Validate_ForPath_MustExist_Fail_WithMessageForAll_Test()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <id></id>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""ABC-001"" qty=""9"">19</item>
        <item sku=""ABC-001"" qty=""9"" type=""premium"">1900</item>
    </items>
    <discount type=""percentage"">
        5
    </discount>
    <totalQty>
        9
    </totalQty>
    <totalAmount>
        9000
    </totalAmount>
    <billing>
        <customerId>123</customerId>
    </billing>
</order>");

            var validator = new XmlValidator()
                .ForPath("/order/customer/id").WithMessage("Temp")
                .WithMessageForAll("Id is not valid")
                .Required("Req message").WithMessage("Req with message")
                .Value(val =>
                {
                    var isNumber = int.TryParse(val, out _);

                    return isNumber;
                })
                .Done();

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Count == 2);
            Assert.IsTrue(result.Errors.All(x => x?.Message == "Id is not valid"));
        }
    }
}