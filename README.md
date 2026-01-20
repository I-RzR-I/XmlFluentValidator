> **Note** This repository is developed for .netstandard2.0+ <br />
> A fluent, runtime-first XML validation engine with XSD generation and documentation support.

[![NuGet Version](https://img.shields.io/nuget/v/XmlFluentValidator.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/XmlFluentValidator/)
[![Nuget Downloads](https://img.shields.io/nuget/dt/XmlFluentValidator.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/XmlFluentValidator)

### 🔎 Overview

**XmlFluentValidator** is a flexible XML validation library designed for scenarios where:
- Validation rules must be **defined programmatically**
- XML needs to be **validated at runtime**
- **XSD schemas** must be generated from code-based rules
- Some rules cannot be expressed in XSD but still need to be **documented**

The library combines a **fluent builder API**, runtime validation, and schema generation into a single, cohesive solution.

**What you can do**
- Define XML validation rules using a fluent, readable API
- Validate XML documents at runtime
- Generate XSD schemas directly from validation rules
- Document runtime-only rules (e.g., custom logic) using `<xs:annotation>`
- Mix schema validation and custom validation seamlessly

---

### ✨  Key Features

**Element Validation**
- Required / optional elements
- Regex validation
- Numeric and length ranges
- Enumeration (enum-like constraints)
- MaxOccurs and cardinality rules
- Data type enforcement
- Nullable (`xs:nillable`) support
- Uniqueness constraints
- Fixed(constant) value

**Attribute Validation**
- Required attributes
- Regex, range, length, and enumeration rules
- Attribute uniqueness
- Attribute-level documentation
- Element–attribute cross-validation
- Fixed(constant) value

**Advanced Capabilities**
- Deeply nested paths (`/customer/address/street`)
- Attribute shorthand (`items/item@sku`)
- Cross-field validation
- Global document rules
- Custom, user-defined validation logic
- Rule severity levels
- Short-circuiting (`StopOnFailure`)
- Automatic XSD documentation generation

---

### 📖 Fluent Validator API Reference
- `.ForPath(xpath)` → For path. Rule for specific path.
- `.ForAttribute(xpath)` → For attribute. Rule for specific attribute.
- `.ForElement(elementPath)` → For element. Rule for specific element.
- `.UseSchema(schemaSet, [stopOnSchemaErrors])` → .Use schema (for validation).
- `.GlobalRule(predicate, [message])` → Global rule.
- `.Validate(doc)` → Validates the given document.

### 📖 Fluent API Valdiation Rules Reference

**Element Rules**
- `.WithElementMustExist([message])` → Must exist. The element must exist.
- `.WithElementCount(predicate, [message])` → Counts.
- `.WithElementValue(predicate, [message])` → Set validation rule for element value.
- `.WithElementOptional([message])` → Specify the element validation as not required.
- `.WithElementRequired([message])` → Set element validation rule as required.
- `.WithElementValueRequired([message])` → Set element and value validation rule as required.
- `.WithElementMatchesRegex(pattern, [message])` → Matches RegEx. Set element validation rule as regular expression.
- `.WithElementInRange(min, max, [isInclusive], [message])` → In range. Set element validation rule as in range between minimum and maximum value.
- `.WithElementUnique([message])` →  Set element validation rule as unique value.
- `.WithElementMaxOccurs(max, [message])` → Set element validation rule as maximum occurs.
- `.WithElementValueLength(min, [max], [message])` → With element value length.
- `.WithElementDataType(dataType, [message])` → With element data type.
- `.WithElementEnumerator(rangeEnumerator, [message])` → With element enumerator (like enum values).
- `.WithElementExactLength(length, [message])` → With element exact length.
- `.WithElementDocumentation(documentation)` → With element documentation. Set annotation/documentaion.
- `.WithElementNullable([isNullable], [message])` → With element nullable.
- `.WithElementFixedValue(fixedValue, [message])` → With element fixed(xonstant) value.

**Attribute Rules**
- `.WithAttribute(name, predicate, [message])` → Set validation rule for element attribute.
- `.WithAttributeRequired(name, [message])` → IsRequired attribute. Set attribute validation rule as required.
- `.WithAttributeValueRequired(name, [message])` → IsRequired attribute. Set attribute and value validation rule as required.
- `.WithAttributeMatchesRegex(name, pattern, [message])` → Attribute matches RegEx. Set attribute validation rule as regular expression.
- `.WithAttributeInRange(name, min, max, [isInclusive], [message])` → Attribute in range. Set attribute validation rule as in range between minimum and maximum value.
- `.WithAttributeUnique(name, [message])` → Attribute unique. Set specific attribute as unique.
- `.ElementAttributeCrossRule(name, predicate, [message])` → Element attribute cross rule. Set cross validation for element and specific element attribute.
- `.WithAttributeValueLength(name, min, [max], [message])` → With attribute value length.
- `.WithAttributeDataType(name, dataType, [message])` → With attribute data type.
- `.WithAttributeEnumerator(name, rangeEnumerator, [message])` → With attribute enumerator.
- `.WithAttributeExactLength(name, length, [message])` → With attribute exact length.
- `.WithAttributeDocumentation(name, documentation)` → With attribute documentation.
- `.WithAttributeExactLength(name, fixedValue, [message])` → With attribute fixed(xonstant) value.

**Messages**
- `.WithMessage(template, arguments)` → With message. Set custom validation message.
- `.WithMessage(message)` → With message. Set custom validation message.
- `.WithMessageForAll(message)` → With message for all.

**Other rules**
- `.All(predicate, [message])` → All.
- `.Any(predicate, [message])` → Any.
- `.When(condition, [message])` → When the given condition.
- `.CustomElementRule(predicate, [message])` → Custom element rule. an be used custom defined(registered) rule or defined in specific context.
- `.Custom(handler, [message])` → Customs. Set the custom (user defined) validation method.
- `.UseCustomRule(ruleName, [message])` → Use custom rule. Set the custom rule name for execution.
- `.WithName(displayName)` →  With name. Set the specific name for path.
- `.WithSeverity(severity)` → With severity. Set the validation message severity.
- `.StopOnFailure()` → Stops on failure. Short-circuit within this rule chain.
- `.Done()` → Gets the done action. Ends the current element rule chain and returns to validator.

--- 

#### Custom Rules
```csharp
.Custom(ctx =>
{
    var val = ctx.Document.XPathSelectElement("/order/id")?.Value;
    if (val == "0000")
        ctx.Failures.Add(new XmlValidationFailure
        {
            Severity = Severity.Error,
            Path = "/order/id",
            Message = "ID cannot be 0000"
        });
}, "ID must not be 0000")
```

Global Rules
```csharp
.GlobalRule(doc =>
{
    return doc.XPathSelectElements("/order/items/item").Count() <= 10;
}, "Order must not contain more than 10 items")
```

> To get acquainted with a more detailed description, please check the content table at [the first point](docs/usage.md).

No additional components or packs are required for use. So, it only needs to be added/installed in the project and can be used instantly.

**In case you wish to use it in your project, u can install the package from <a href="https://www.nuget.org/packages/XmlFluentValidator" target="_blank">nuget.org</a>** or specify what version you want:


> `Install-Package XmlFluentValidator -Version x.x.x.x`

## Content
1. [USING](docs/usage.md)
1. [CHANGELOG](docs/CHANGELOG.md)
1. [BRANCH-GUIDE](docs/branch-guide.md)