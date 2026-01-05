// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.XmlValidatorTests
//  Author           : RzR
//  Created On       : 2025-12-24 22:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-24 22:21
// ***********************************************************************
//  <copyright file="XsdGeneratorOrderTests.cs" company="RzR SOFT & TECH">
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
using XmlFluentValidator.Enums;

namespace XmlValidatorTests.Tests
{
    [TestClass]
    public class XsdGeneratorOrderTests
    {
        [TestMethod]
        public void Generate_Xsd_AndValidate_Test()
        {
            var xml = XDocument.Parse(@"
<Order>
    <OrderID>12345</OrderID>
    <Customer>
        <CustomerID>67890</CustomerID>
        <Name>John Doe</Name>
        <Email>johndoe@example.com</Email>
        <Address>
            <Street>123 Maple Street</Street>
            <City>Springfield</City>
            <State>IL</State>
            <PostalCode>62701</PostalCode>
            <Country>USA</Country>
        </Address>
    </Customer>
    <OrderDate>2025-12-24</OrderDate>
    <OrderMoment>2025-12-24T10:26:01.448</OrderMoment>
    <Items>
        <Item>
            <ItemID sku=""P-000154"">1001</ItemID>
            <ProductName>Wireless Headphones</ProductName>
            <Quantity>2</Quantity>
            <Price priceType=""standard"">99.99</Price>
        </Item>
        <Item>
            <ItemID sku=""P-009154"">1002</ItemID>
            <ProductName>Bluetooth Speaker</ProductName>
            <Quantity>1</Quantity>
            <Price priceType=""discount"">49.99</Price>
        </Item>
    </Items>
    <TotalAmount>249.97</TotalAmount>
    <ShippingCost>99.99</ShippingCost>
    <ShippingMethod>Standard</ShippingMethod>
    <Payment>
        <Method>Credit Card</Method>
        <TransactionID>abc123xyz456</TransactionID>
        <AmountPaid>249.97</AmountPaid>
    </Payment>
    <Status>Shipped</Status>
</Order>");

            var validator = new XmlValidator()
                .ForPath("Order/OrderID")
                    .WithElementRequired()
                    .WithElementMaxOccurs(1)
                .Done()
                .ForPath("Order/Customer")
                    .WithElementRequired()
                    .WithElementMaxOccurs(1)
                .Done()
                .ForPath("Order/Customer/CustomerID")
                    .WithElementRequired()
                    .WithElementMaxOccurs(1)
                    .WithElementMatchesRegex("[0-9]")
                    .WithElementDataType(XmlValidationDataTypeKind.Integer)
                .Done()
                .ForPath("Order/Customer/Name")
                    .WithElementRequired()
                .Done()
                .ForPath("Order/Customer/Email")
                    .WithElementRequired()
                .Done()
                .ForPath("Order/Customer/Address")
                    .WithElementRequired()
                    .WithElementMaxOccurs(1)
                .Done()
                .ForPath("Order/Customer/Address/Street")
                    .WithElementRequired()
                .Done()
                .ForPath("Order/Customer/Address/City")
                    .WithElementRequired()
                .Done()
                .ForPath("Order/Customer/Address/State")
                    .WithElementRequired()
                .Done()
                .ForPath("Order/Customer/Address/PostalCode")
                    .WithElementRequired()
                .Done()
                .ForPath("Order/Customer/Address/Country")
                    .WithElementRequired()
                    .WithElementMatchesRegex("[a-zA-Z0-9]")
                    .WithElementValueLength(3, 99)
                    .WithElementDataType(XmlValidationDataTypeKind.String)
                .Done()
                .ForPath("Order/OrderDate")
                    .WithElementRequired()
                    .WithElementMaxOccurs(1)
                    .WithElementDataType(XmlValidationDataTypeKind.Date)
                .Done()
                .ForPath("Order/OrderMoment")
                    .WithElementRequired()
                    .WithElementMaxOccurs(1)
                    .WithElementDataType(XmlValidationDataTypeKind.DateTime)
                .Done()
                .ForPath("Order/Items")
                    .WithElementRequired()
                .Done()
                .ForPath("Order/Items/Item")
                    .WithElementRequired()
                .Done()
                .ForPath("Order/Items/Item/ItemID")
                    .WithElementRequired()
                    .WithAttributeRequired("sku")
                    .WithAttributeValueLength("sku", 4)
                .Done()
                .ForPath("Order/Items/Item/ProductName")
                    .WithElementRequired()
                .Done()
                .ForPath("Order/Items/Item/Quantity")
                    .WithElementRequired()
                    .WithElementInRange(1, 100)
                .Done()
                .ForPath("Order/Items/Item/Price")
                    .WithElementRequired()
                    .WithAttributeRequired("priceType")
                    .WithElementDataType(XmlValidationDataTypeKind.Decimal)
                    .WithAttributeDataType("priceType", XmlValidationDataTypeKind.String)
                    .WithAttributeEnumerator("priceType", new[] { "standard", "discount" })
                .Done()
                .ForPath("Order/TotalAmount")
                    .WithElementRequired()
                    .WithElementMaxOccurs(1)
                    .WithElementDataType(XmlValidationDataTypeKind.Decimal)
                .Done()
                .ForPath("Order/ShippingCost")
                    .WithElementRequired()
                    .WithElementMaxOccurs(1)
                .Done()
                .ForPath("Order/ShippingMethod")
                    .WithElementRequired()
                    .WithElementMaxOccurs(1)
                .Done()
                .ForPath("Order/Payment")
                    .WithElementRequired()
                    .WithElementMaxOccurs(1)
                .Done()
                .ForPath("Order/Payment/Method")
                    .WithElementRequired()
                    .WithElementMaxOccurs(1)
                    .WithElementEnumerator(new[] { "Credit Card", "Cash" })
                .Done()
                .ForPath("Order/Payment/TransactionID")
                    .WithElementRequired()
                    .WithElementMaxOccurs(1)
                .Done()
                .ForPath("Order/Payment/AmountPaid")
                    .WithElementRequired()
                    .WithElementMaxOccurs(1)
                    .WithElementDataType(XmlValidationDataTypeKind.Decimal)
                .Done()
                .ForPath("Order/Status")
                    .WithElementRequired()
                    .WithElementMaxOccurs(1)
                .Done();

            var result = validator.Validate(xml);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Errors.Count.IsZero());

            var xsdInstance = XsdGenerator.Instance;
            var xsd1 = xsdInstance.Generate(validator, "Order", null);
            var x12 = validator.UseSchema(xsdInstance.GetSchemaSet(xsd1));
            var xsdAsString = xsdInstance.GetSchemaString(xsd1);
            var result3 = x12.Validate(xml);

            Assert.IsTrue(result3.IsValid);
        }
    }
}