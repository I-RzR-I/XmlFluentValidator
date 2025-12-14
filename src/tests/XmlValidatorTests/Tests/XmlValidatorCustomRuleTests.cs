// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlValidatorTests
//  Author           : RzR
//  Created On       : 2025-12-11 22:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-11 22:11
// ***********************************************************************
//  <copyright file="XmlValidatorCustomRuleTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using XmlFluentValidator;
using XmlFluentValidator.FluentExtensions;
using XmlFluentValidator.Rules;

namespace XmlValidatorTests.Tests
{
    [TestClass]
    public class XmlValidatorCustomRuleTests
    {
        [TestMethod]
        public void CustomRule_Test_1()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""ABC-101"" qty=""10"" type=""discount"">10</item>
    </items>
</order>");


            var validator = new XmlValidator("order")
                .ForPath("/order/items/item")
                .CustomElementRule(
                    DefaultCustomRule.ElementEqualsAttribute("qty"),
                    "Item element value must equal qty attribute.")
                .CustomElementRule(
                    DefaultCustomRule.AttributePrefixMax("sku", "ABC", 10),
                    "Items with SKU starting 'ABC' must not exceed 10.")
                .CustomElementRule(
                    DefaultCustomRule.AttributeConditional("type", "discount", e => int.Parse(e.Value) <= 50),
                    "Discount items must have value ≤ 50.")
                .Done();

            var validatorResult = validator.Validate(xml);

            Assert.IsNotNull(validator);
            Assert.IsNotNull(validatorResult);
            Assert.IsTrue(validatorResult.IsValid);
        }

        [TestMethod]
        public void CustomRule_Test_2()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""ABC-101"" qty=""10"" type=""discount"">10</item>
    </items>
</order>");

            CustomRuleRegistry.Register("ElementEqualsAttribute2", (elem, attrs) =>
            {
                var attr = attrs.ContainsKey("qty") ? attrs["qty"] : null;
                return elem.Value == attr;
            });

            CustomRuleRegistry.Register("SkuPrefixMax10", (elem, attrs) =>
            {
                var sku = attrs.ContainsKey("sku") ? attrs["sku"] : null;
                var qty = attrs.ContainsKey("qty") ? int.Parse(attrs["qty"] ?? "0") : 0;
                return !(sku != null && sku.StartsWith("ABC") && qty > 10);
            });


            var validator = new XmlValidator()
                    .ForPath("/order/items/item")
                    .UseCustomRule("ElementEqualsAttribute2", "Item value must equal qty attribute.")
                    .UseCustomRule("SkuPrefixMax10", "Items with SKU starting 'ABC' must not exceed 10.")
                .Done();

            var validatorResult = validator.Validate(xml);

            Assert.IsNotNull(validator);
            Assert.IsNotNull(validatorResult);
            Assert.IsTrue(validatorResult.IsValid);
        }
    }
}