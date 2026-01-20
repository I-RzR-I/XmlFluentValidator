# USING

This document explains how to use the XmlFluentValidator fluent API, grouped by rule category.
Each rule can participate in runtime validation, XSD generation, or both.

```csharp
var validator = new XmlValidator()
    .ForPath("order/id")
        .WithElementRequired()
        .WithElementInRange(1, 9999)
        .Done()
    .ForPath("order/items/item")
        .WithElementMaxOccurs(10)
        .Done();

var result = validator.Validate(xmlDocument);
```

## 📖 Methods Description

### Element Rules

| Method                                                    | Description                                                                         | Example                                                             |
| --------------------------------------------------------- | ----------------------------------------------------------------------------------- | ------------------------------------------------------------------- |
| `.WithElementMustExist([message])`                        | The element must exist.                                                             | `.ForPath("items/item").WithElementMustExist()`                     |
| `.WithElementOptional([message])`                         | Marks element as optional. Generates `minOccurs="0"`.                               | `.ForPath("customer/phone").WithElementOptional()`                  |
| `.WithElementRequired([message])`                         | Marks element as mandatory. Generates `minOccurs="1"`.                              | `.ForPath("id").WithElementRequired()`                              |
| `.WithElementValueRequired([message])`                    | Marks element and value as mandatory. Generates `minOccurs="1"`.                    | `.ForPath("id").WithElementValueRequired()`                         |
| `.WithElementCount(predicate, [message])`                 | Limits how many times an element can appear (runtime).                              | `.ForPath("items/item").WithElementCount(c => c <= 5)`              |
| `.WithElementMaxOccurs(max, [message])`                   | Limits element occurrences. Generates `maxOccurs`.                                  | `.ForPath("items/item").WithElementMaxOccurs(10)`                   |
| `.WithElementValue(predicate, [message])`                 | Validates element text value.                                                       | `.ForPath("email").WithElementValue(v => v.Contains("@"))`          |
| `.WithElementMatchesRegex(pattern, [message])`            | Validates element text using regex. Generates `<xs:pattern>`.                       | `.ForPath("email").WithElementMatchesRegex(@"^\S+@\S+\.\S+$")`      |
| `.WithElementInRange(min, max, [isInclusive], [message])` | Validates numeric value range. Generates `<xs:minInclusive>` / `<xs:maxInclusive>`. | `.ForPath("age").WithElementInRange(18, 65)`                        |
| `.WithElementValueLength(min, [max], [message])`          | Validates string length. Generates `<xs:minLength>` / `<xs:maxLength>`.             | `.ForPath("code").WithElementValueLength(3, 10)`                    |
| `.WithElementExactLength(length, [message])`              | Enforces exact string length. Generates `<xs:length>`. 							  | `.ForPath("countryCode").WithElementExactLength(2)`                 |
| `.WithElementUnique([message])`                           | Ensures unique values within scope.                         						  | `.ForPath("items/item/code").WithElementUnique()`                   |
| `.WithElementDataType(type, [message])`                   | Enforces XSD data type.                                                             | `.ForPath("createdAt").WithElementDataType(XmlDataType.DateTime)`   |
| `.WithElementEnumerator(values, [message])`               | Restricts values to enumeration. Generates `<xs:enumeration>`.                      | `.ForPath("status").WithElementEnumerator(new[] { "New", "Paid" })` |
| `.WithElementNullable([isNullable], [message])`           | Controls `xs:nillable`.                                                             | `.ForPath("description").WithElementNullable(true)`                 |
| `.WithElementDocumentation(text)`                         | Adds `<xs:annotation>` documentation.                                               | `.ForPath("id").WithElementDocumentation("Order identifier")`       |
| `.WithElementFixedValue(fixedValue, [message])`           | Adds `<xs:fixed>` fixed value restrinction.                                         | `.ForPath("Order/StoreName").WithElementFixedValue("STOREX")`       |

### Attribute Rules

| Method                                                            | Description                                                 | Example                                                                        |
| ----------------------------------------------------------------- | ----------------------------------------------------------- | ------------------------------------------------------------------------------ |
| `.WithAttribute(name, predicate, [message])`                      | Adds a custom validation rule for attribute value.          | `.ForPath("item").WithAttribute("sku", v => !string.IsNullOrEmpty(v))`         |
| `.WithAttributeRequired(name, [message])`                         | Attribute must exist. Generates `use="required"`.           | `.ForPath("item").WithAttributeRequired("sku")`                                |
| `.WithAttributeValueRequired(name, [message])`                    | Attribute and value must exist. Generates `use="required"`. | `.ForPath("item").WithAttributeValueRequired("sku")`                           |
| `.WithAttributeMatchesRegex(name, pattern, [message])`            | Attribute value must match regex. Generates `<xs:pattern>`. | `.ForPath("item@sku").WithAttributeMatchesRegex("sku", @"^[A-Z]{3}-\d{3}$")`   |
| `.WithAttributeInRange(name, min, max, [isInclusive], [message])` | Attribute numeric value must be within range.               | `.ForPath("item@qty").WithAttributeInRange("qty", 1, 100)`                     |
| `.WithAttributeValueLength(name, min, [max], [message])`          | Validates attribute string length.                          | `.ForPath("item@sku").WithAttributeValueLength("sku", 5, 10)`                  |
| `.WithAttributeExactLength(name, length, [message])`              | Enforces exact attribute length.                            | `.ForPath("item@cc").WithAttributeExactLength("cc", 2)`                        |
| `.WithAttributeUnique(name, [message])`                           | Attribute values must be unique. 							  | `.ForPath("item@sku").WithAttributeUnique("sku")`                  			   |
| `.WithAttributeDataType(name, type, [message])`                   | Enforces attribute XSD data type.                           | `.ForPath("item@qty").WithAttributeDataType("qty", XmlDataType.Int)`           |
| `.WithAttributeEnumerator(name, values, [message])`               | Restricts attribute values to enumeration.                  | `.ForPath("item@type").WithAttributeEnumerator("type", new[] { "A", "B" })`    |
| `.WithAttributeDocumentation(name, text)`                         | Adds documentation to attribute in XSD.                     | `.ForPath("item@sku").WithAttributeDocumentation("sku", "Stock keeping unit")` |
| `.WithAttributeFixedValue(name, fixedValue, [message])`           | Adds `<xs:fixed>` fixed value restrinction.                 | `.ForPath("Order/StoreName").WithAttributeFixedValue("id", "xsd123")`          |

### Collection & Logical Rules

| Method                        | Description                                          | Example                                                  |
| ----------------------------- | ---------------------------------------------------- | -------------------------------------------------------- |
| `.All(predicate, [message])`  | All matched elements must satisfy condition.         | `.ForPath("items/item").All(e => e.IsNotNull())`         |
| `.Any(predicate, [message])`  | At least one matched element must satisfy condition. | `.ForPath("items/item").Any(e => e.Value == "A")`        |
| `.When(condition, [message])` | Conditional execution of subsequent rules.           | `.ForPath("discount").When(doc => doc.HasValue("/vip"))` |

### Custom & Cross Rules

| Method                                                       | Description                                                                    | Example                                                                                       |
| ------------------------------------------------------------ | ------------------------------------------------------------------------------ | --------------------------------------------------------------------------------------------- |
| `.ElementAttributeCrossRule(attrName, predicate, [message])` | Validates relationship between element value and attribute. Documented in XSD. | `.ForPath("item").ElementAttributeCrossRule("qty", (e, a) => e == a, "Value must equal qty")` |
| `.CustomElementRule(predicate, [message])`                   | Uses a registered reusable custom rule.                                        | `.ForPath("item").CustomElementRule(DefaultCustomRule.ElementEqualsAttribute("qty"))`         |
| `.Custom(handler, [message])`                                | Inline custom validation logic. Documented in XSD.                             | `.ForPath("item").Custom(ctx => { /* logic */ }, "Custom rule")`                              |
| `.UseCustomRule(ruleName, [message])`                        | Executes named custom rule.                                                    | `.ForPath("item").UseCustomRule("ElementEqualsAttribute2")`                                   |

### Global Rules

| Method                              | Description                            | Example                                                                               |
| ----------------------------------- | -------------------------------------- | ------------------------------------------------------------------------------------- |
| `.GlobalRule(predicate, [message])` | Applies validation to entire document. | `.GlobalRule(doc => doc.XPathSelectElements("//item").Count() <= 10, "Max 10 items")` |

### Messages & Severity

| Method                         | Description                                | Example                                                |
| ------------------------------ | ------------------------------------------ | ------------------------------------------------------ |
| `.WithMessage(message)`        | Overrides validation message.              | `.WithElementRequired().WithMessage("ID is required")` |
| `.WithMessage(template, args)` | Message template with arguments.           | `.WithMessage("Value {0} is invalid", val)`            |
| `.WithMessageForAll(message)`  | Applies same message to all chained rules. | `.WithMessageForAll("Invalid item")`                   |
| `.WithSeverity(severity)`      | Sets severity level.                       | `.WithSeverity(Severity.Warning)`                      |
| `.WithName(displayName)`       | Friendly display name for path.            | `.WithName("Order Identifier")`                        |

### Control Flow

| Method             | Description                                       | Example                                            |
| ------------------ | ------------------------------------------------- | -------------------------------------------------- |
| `.StopOnFailure()` | Stops rule chain execution on first failure.      | `.WithElementRequired().StopOnFailure()`           |
| `.Done()`          | Ends current rule chain and returns to validator. | `.ForPath("id").Required().InRange(1,9999).Done()` |

---
