> **Note** This repository is developed for .netstandard2.0+

[![NuGet Version](https://img.shields.io/nuget/v/XmlFluentValidator.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/XmlFluentValidator/)
[![Nuget Downloads](https://img.shields.io/nuget/dt/XmlFluentValidator.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/XmlFluentValidator)

🔎 Overview

This library allows you to:
- Define XML validation rules with a fluent builder syntax.
- Validate XML documents at runtime.
- Generate XSD schemas that reflect those rules.

TODO:
- Document runtime-only rules (like custom logic) in <xs:annotation> blocks.

---

✨ Features

- Element rules: Required, regex, numeric ranges, uniqueness.
- Attribute rules: Required, regex, numeric ranges, uniqueness.
- Nested paths: Arbitrary depth (customer/address/street).
- Attribute shorthand: items/item@sku.
- Custom rules: Inject arbitrary validation logic.
- Cross rules: Validate relationships between element values and attributes.
- Global rules: Apply checks across the entire document.

---

📖 Fluent API Reference

Element Rules
- `.WithElementRequired()` → element must exist.
- `.WithElementOptional()` → element may be absent.
- `.WithElementMatchesRegex(pattern)` → element text must match regex.
- `.WithElementInRange(min, max)` → element text must be numeric within range.
- `.WithElementUnique()` → element values must be unique.
- etc

Attribute Rules
- `.WithAttributeRequired(name)` → attribute must exist.
- `.WithAttributeMatchesRegex(pattern)` → attribute value must match regex.
- `.WithAttributeInRange(name, min, max)` → attribute numeric range.
- `.WithAttributeUnique(name)` → attribute values must be unique.
- etc

Custom Rules
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