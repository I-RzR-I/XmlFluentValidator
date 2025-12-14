# USING

📖 Methods Description

Element Rules

| Method | Description | Example |
|--------|-------------|---------|
| .MustExist([message]) | The element must exist. | .ForPath("items/item").MustExist() |
| .Count(predicate, [message]) | Limits how many times an element can appear. | .ForPath("items/item").Count(x => x > 1) |
| .Value(predicate, [message]) | Set validation rule for element value. | .ForPath("items/item").Value(v => v.Contains("@")) |
| .Attribute(name, predicate, [message]) | Set validation rule for element attribute. | .ForPath("items/item").Attribute("sku", v => v.IsPresent()) |
| .All(predicate, [message]) |   | .ForPath("items/item").All(e => e.IsNotNull()) |
| .Any(predicate, [message]) |   | .ForPath("items/item").Any(e => e.IsNotNull()) |
| .When(predicate, [message]) | When the given condition is true, continue validaction or stop  | .ForPath("items/item").When(doc => doc.IsNotNull()) |
| .Optional([message]) | Marks the element as optional. Generates minOccurs="0" in XSD. | .ForPath("customer/phone").Optional() |
| .Required([message]) | Marks the element as mandatory. Generates minOccurs="1" in XSD. | .ForPath("id").Required() |
| .MatchesRegex(pattern, [message]) | Validates that element text matches a regex. Adds <xs:pattern> restriction. | .ForPath("email").MatchesRegex(@"&Hat;&lbrack;&Hat;&commat;&bsol;&#115;&rsqb;&plus;&commat;&lbrack;&Hat;&commat;&bsol;&#115;&rsqb;&plus;&bsol;&period;&lbrack;&Hat;&commat;&bsol;&#115;&rsqb;&plus;&dollar;") |
| .InRange(min, max, [message]) | Validates that element text is numeric and within range. Adds <xs:minInclusive> and <xs:maxInclusive>. | .ForPath("age").InRange(18, 65) |
| .Unique([message]) | Ensures element values are unique within their container. Generates <xs:unique> constraint. | .ForPath("items/item/code").Unique() |
| .MaxOccurs(count, [message]) | Limits how many times an element can appear. Generates maxOccurs="count". | .ForPath("items/item").MaxOccurs(10) |

---

Attribute Rules

| Method | Description | Example |
|--------|-------------|---------|
| .RequiredAttribute(name, [message]) | Attribute must exist. Generates use="required". | .ForElement("item").RequiredAttribute("sku") |
| .MatchesRegex(pattern, [message]) | Attribute value must match regex. Adds <xs:pattern>. | .ForElement("item@sku").MatchesRegex(@"^[A-Z]{3}-\d{3}$") |
| .AttributeInRange(name, min, max, [message]) | Attribute numeric value must be within range. Adds <xs:minInclusive> and <xs:maxInclusive>. | .ForElement("item@qty").AttributeInRange("qty", 1, 100) |
| .UniqueAttribute(name, [message]) | Attribute values must be unique. Generates <xs:unique> constraint. | .ForElement("item@sku").UniqueAttribute("sku") |

---

Custom & Cross Rules

| Method | Description | Example |
|--------|-------------|---------|
| .ElementAttributeCrossRule(attrName, predicate, message) | Validates relationship between element text and attribute value. Documented in <xs:annotation>. | .ForElement("item").ElementAttributeCrossRule("qty", (elem, attr) => elem == attr, "Element must equal qty") |
| .CustomElementRule(predicate, message) | Can be used custom defined(registered) rule or defined in specific context. | .ForPath("/order/items/item").CustomElementRule(    DefaultCustomRule.ElementEqualsAttribute("qty")) |
| .Custom(handler, message) | Injects arbitrary validation logic. Documented in <xs:annotation>. | .ForPath("items/item").Custom(ctx => { / custom check / }, "Custom rule") |
| .UseCustomRule(ruleName, message) | Set the custom rule name for execution | .ForPath("/order/items/item").UseCustomRule("ElementEqualsAttribute2", "Item value must equal qty attribute.") |

---

Global Rules

| Method | Description | Example |
|--------|-------------|---------|
| .GlobalRule(handler, message) | Applies a check across the entire document. Documented at root in <xs:annotation>. | .GlobalRule(doc => doc.XPathSelectElements("/order/items/item").Count() <= 10, "Max 10 items") |

---

Control Flow

| Method | Description | Example |
|--------|-------------|---------|
| .Done() | Ends the current element rule chain and returns to validator. | .ForElement("id").Required().InRange(1,9999).Done() |

---

