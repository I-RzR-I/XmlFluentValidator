// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlValidatorTests
//  Author           : RzR
//  Created On       : 2025-12-10 19:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-10 19:54
// ***********************************************************************
//  <copyright file="XmlValidatorTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.DataTypeExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using XmlFluentValidator;
using XmlFluentValidator.Enums;
using XmlFluentValidator.Models.Message;
using XmlFluentValidator.Models.Result;

#endregion

namespace XmlValidatorTests.Tests
{
    [TestClass]
    public class XmlValidatorTests
    {
        [TestMethod]
        public void Validate_Pass_Test_1()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""ABC-001"" qty=""2"">Widget</item>
    </items>
</order>");

            var validator = new XmlValidator()
                .ForPath("/order/id")
                .WithName("Order Id")
                .MustExist("Order id is required.")
                .Value(v => int.TryParse(v, out var n) && n > 0, "Order id must be a positive integer.")
                .Done()
                .ForPath("/order/customer/email")
                .MustExist("Customer email is required.")
                .Value(v => v.Contains("@"), "Customer email must be valid.")
                .Done()
                .ForPath("/order/items/item")
                .Count(c => c >= 1, "At least one item is required.")
                .Attribute("sku", v => v.IsPresent(), "Item SKU is required.")
                .Attribute("qty", v => int.TryParse(v, out var n) && n > 0, "Quantity must be a positive integer.")
                .Done();

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Errors.Count.IsZero());
        }

        [TestMethod]
        public void Validate_Pass_Test_2()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""ABC-001"" qty=""5"">Widget</item>
    </items>
    <discountCode>asoih9723</discountCode>
</order>");

            var validator = new XmlValidator()
                .ForPath("/order/discountCode")
                .Required("Ele required").WithMessage("Ele X required x2")
                .When(doc =>
                {
                    //return false;
                    //return doc.XPathSelectElement("/order/discountCode").Value.StartsWith("AZ");
                    return doc.XPathEvaluate("string(/order/discountCode)") is string s && !string.IsNullOrEmpty(s);
                }, "Internal condition message").WithMessage("Message2")
                .All(e => e.Value.Length <= 10, "Discount code must be at most 10 characters.")
                .Done()
                .ForPath("/order/items/item")
                .WithMessageForAll("Message for all")
                .Custom(ctx =>
                {
                    var items = ctx.Document.XPathSelectElements("/order/items/item").ToList();
                    var totalQty = items.Sum(i => int.TryParse(i.Attribute("qty")?.Value, out var q) ? q : 0);
                    if (totalQty > 100)
                    {
                        ctx.Failures.Add(new XmlValidationFailureResult()
                        {
                            Severity = XmlMessageSeverity.Warning,
                            Path = "/order/items",
                            Message = "Total quantity is unusually high."
                        });
                    }
                })
                .Done();

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Errors.Count.IsZero());
        }

        [TestMethod]
        public void Validate_Fail_Test_1()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""ABC-001"" qty=""5"">Widget</item>
    </items>
    <discountCode>asoih9723</discountCode>
</order>");

            var validator = new XmlValidator()
                .ForPath("/order/discountCode")
                .Required("Ele required").WithMessage("Ele X required x2")
                .Done()
                .ForPath("/order/discountCode")
                .When(doc =>
                {
                    return false;
                }, "Internal condition message").WithMessage("Message2")
                .Done()
                .ForPath("/order/discountCode")
                .All(e => e.Value.Length <= 10, "Discount code must be at most 10 characters.")
                .Done();

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Message2", result.Errors.FirstOrDefault()?.Message);
        }

        [TestMethod]
        public void Validate_Fail_Test_2()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""ABC-001"" qty=""5"">Widget</item>
    </items>
    <discountCode>asoih9723</discountCode>
</order>");

            var validator = new XmlValidator()
                .ForPath("/order/discountCode")
                .Required("Ele required").WithMessage("Ele X required x2")
                .Done()
                .ForPath("/order/discountCode")
                .When(doc =>
                {
                    return false;
                }, "Internal condition message")
                .All(e => e.Value.Length <= 10, "Discount code must be at most 10 characters.")
                .Done()
                .ForPath("/order/discountCode")
                .Done();

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Internal condition message", result.Errors.FirstOrDefault()?.Message);
        }

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
        <item sku=""ABC-001"" qty=""5"">Widget</item>
    </items>
</order>");

            var validator = new XmlValidator()
                .ForPath("/order/items/item")
                .Count(c => c >= 1)
                .WithMessage("Order must contain at least {Min} item(s).", MessageArguments.From(("Min", 1)))
                .Done()
                .ForPath("/order/id")
                .Value(v => int.TryParse(v, out var n) && n > 0)
                .WithMessage("Order Id '{XPath}' is invalid: {Raw}", null)
                .Done();


            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Errors.Count.IsZero());
        }

        [TestMethod]
        public void Validate_Pass_Test_3_1()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""ABC-001"" qty=""9"">Widget</item>
    </items>
</order>");

            var validator = new XmlValidator()
                .ForPath("/order/items/item")
                .Count(c => c >= 1)
                .WithMessage("Order must contain at least {Min} item(s).", MessageArguments.From(("Min", 1)))
                .Done()
                .ForPath("/order/items/item")
                .Attribute("qty", v => int.TryParse(v, out var n) && n > 0)
                .WithMessage("Item quantity at {XPath} must be positive. Found: {Raw}", null)
                .Done()
                .ForPath("/order/id")
                .Value(v => int.TryParse(v, out var n) && n > 0)
                .WithMessage("Order Id '{XPath}' is invalid: {Raw}", null)
                .Done();


            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Errors.Count.IsZero());
        }

        [TestMethod]
        public void Validate_Fail_ForPath_Attribute_Test_4()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""ABC-001"" qty=""9"">Widget</item>
    </items>
</order>");

            var validator = new XmlValidator()
                .ForPath("/order/customer/email")
                .MustExist()
                .MatchesRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                .Done()

                .ForPath("/order/items/item/@qty")
                .InRange(1, 100)
                .Done()

                .ForPath("/order/items/item/@sku")
                .Unique()
                .Done();

            Assert.ThrowsException<InvalidOperationException>(() => validator.Validate(xml));
        }

        [TestMethod]
        public void Validate_Pass_Test_4_1()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""ABC-001"" qty=""9"">Widget</item>
    </items>
</order>");

            var validator = new XmlValidator()
                .ForPath("/order/customer/email")
                .MustExist()
                .MatchesRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                .Done()

                .ForPath("/order/items/item")
                .AttributeInRange("qty", 1, 100)
                .Done()

                .ForPath("/order/items/item")
                .AttributeUnique("sku")
                .Done();

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Errors.Count.IsZero());
        }

        [TestMethod]
        public void Validate_Pass_Test_5()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""ABC-001"" qty=""9"">Widget</item>
    </items>
</order>");

            var validator = new XmlValidator()
                .ForPath("/order/id")
                .Required()
                .Done()

                .ForPath("/order/items/item")
                .Attribute("sku", v => v.IsPresent())
                .Required("Item SKU is required.")
                .Done()

                .ForPath("/order/customer/email")
                .Required()
                .MatchesRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                .Done();

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Errors.Count.IsZero());
        }

        [TestMethod]
        public void Validate_Pass_Test_6()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""ABC-001"" qty=""9"">Widget</item>
    </items>
</order>");

            var validator = new XmlValidator()
                .ForPath("/order/items/item")
                .RequiredAttribute("sku") // must exist and not be empty
                .AttributeMatchesRegex("sku", @"^[A-Z]{3}-\d{3}$") // format check
                .AttributeInRange("qty", 1, 100) // numeric range
                .AttributeUnique("sku") // no duplicates allowed
                .Done();

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Errors.Count.IsZero());
        }

        [TestMethod]
        public void Validate_Pass_Test_6_x()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""ABC-001"" qty=""9"">Widget</item>
    </items>
</order>");

            var validator = new XmlValidator()
                .ForPath("/order/items/item")
                .RequiredAttribute("sku") // must exist and not be empty
                .AttributeMatchesRegex("sku", @"^[A-Z]{3}-\d{3}$") // format check
                .AttributeInRange("qty", 1, 100) // numeric range
                .AttributeUnique("sku") // no duplicates allowed
                .Done();

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Errors.Count.IsZero());
        }

        [TestMethod]
        public void Validate_Pass_Test_7()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""ABC-001"" qty=""9"">9</item>
    </items>
    <discount type=""test"">
      
    </discount>
</order>");

            var validator = new XmlValidator()
                .ForPath("/order/items/item")
                .ElementAttributeCrossRule("qty",
                    (elemVal, attrVal) =>
                        int.TryParse(attrVal, out var qty) &&
                        int.TryParse(elemVal, out var val) &&
                        qty == val,
                    "Item quantity attribute must equal element value.")
                .Done()
                .ForPath("/order/items/item")
                .CustomElementRule(
                    (elem, attrs) =>
                    {
                        var sku = attrs.ContainsKey("sku") ? attrs["sku"] : null;
                        var qty = attrs.ContainsKey("qty") ? int.Parse(attrs["qty"] ?? "0") : 0;

                        // Business rule: if SKU starts with "ABC", qty must be ? 10
                        if (sku != null && sku.StartsWith("ABC") && qty > 10)
                            return false;

                        return true;
                    },
                    "Items with SKU starting 'ABC' must not exceed quantity 10.")
                .Done()
                .ForPath("/order/discount")
                .ElementAttributeCrossRule("type",
                    (elemVal, attrVal) =>
                        attrVal == "percentage" ? int.Parse(elemVal) <= 50 : true,
                    "Percentage discounts must not exceed 50%.")
                .Done();

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Errors.Count.IsZero());
        }

        [TestMethod]
        public void Validate_Pass_Test_7_1()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""ABC-001"" qty=""9"">9</item>
    </items>
    <discount type=""percentage"">
        5
    </discount>
</order>");

            var validator = new XmlValidator()
                .ForPath("/order/items/item")
                .ElementAttributeCrossRule("qty",
                    (elemVal, attrVal) =>
                        int.TryParse(attrVal, out var qty) &&
                        int.TryParse(elemVal, out var val) &&
                        qty == val,
                    "Item quantity attribute must equal element value.")
                .Done()
                .ForPath("/order/items/item")
                .CustomElementRule(
                    (elem, attrs) =>
                    {
                        var sku = attrs.ContainsKey("sku") ? attrs["sku"] : null;
                        var qty = attrs.ContainsKey("qty") ? int.Parse(attrs["qty"] ?? "0") : 0;

                        // Business rule: if SKU starts with "ABC", qty must be ? 10
                        if (sku != null && sku.StartsWith("ABC") && qty > 10)
                            return false;

                        return true;
                    },
                    "Items with SKU starting 'ABC' must not exceed quantity 10.")
                .Done()
                .ForPath("/order/discount")
                .ElementAttributeCrossRule("type",
                    (elemVal, attrVal) =>
                        attrVal == "percentage" ? int.Parse(elemVal) <= 50 : true,
                    "Percentage discounts must not exceed 50%.")
                .Done();

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Errors.Count.IsZero());
        }

        [TestMethod]
        public void Validate_Pass_Test_8()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <email>john@example.com</email>
    </customer>
    <items>
        <item sku=""ABC-001"" qty=""9"">19</item>
    </items>
    <discount type=""percentage"">
        5
    </discount>
    <totalQty>
        9
    </totalQty>
</order>");

            var validator = new XmlValidator()
                    .ForPath("/order/items/item")
                    .RequiredAttribute("qty")
                    .Done()
                    .GlobalRule(doc =>
                    {
                        var items = doc.XPathSelectElements("/order/items/item");
                        var sum = items.Sum(i => int.TryParse(i.Attribute("qty")?.Value, out var q) ? q : 0);
                        var total = int.TryParse(doc.XPathEvaluate("string(/order/totalQty)")?.ToString(), out var t) ? t : 0;
                        return sum == total;
                    }, "Sum of item quantities must equal <totalQty> element.");

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Errors.Count.IsZero());
        }

        [TestMethod]
        public void Validate_Pass_Test_8_1()
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
</order>");

            var validator = new XmlValidator()
                    .ForPath("/order/items/item")
                    .RequiredAttribute("qty")
                    .Done()
                    .GlobalRule(doc =>
                    {
                        var total = int.Parse(doc.XPathEvaluate("string(/order/totalAmount)").ToString());
                        var hasPremium = doc.XPathSelectElements("/order/items/item")
                            .Any(i => i.Attribute("type")?.Value == "premium");
                        return total <= 1000 || hasPremium;
                    }, "Orders over 1000 must include at least one premium item.");

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Errors.Count.IsZero());
        }

        [TestMethod]
        public void Validate_Pass_Test_8_2()
        {
            var xml = XDocument.Parse(@"
<order>
    <id>123</id>
    <customer>
        <id>123</id>
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
                    .ForPath("/order/items/item")
                    .RequiredAttribute("qty")
                    .Done()
                    .GlobalRule(doc =>
                    {
                        var customerId = doc.XPathEvaluate("string(/order/customer/id)").ToString();
                        var billingId = doc.XPathEvaluate("string(/order/billing/customerId)").ToString();
                        return customerId == billingId;
                    }, "Customer ID must match Billing customerId.");

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Errors.Count.IsZero());
        }

        [TestMethod]
        public void Validate_Fail_OnItemSku_Test_8_3()
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
    }
}