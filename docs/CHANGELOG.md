### **v1.0.0.0** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 05-02-2026
-> [DEV] - Publish release v1.0.0.0. <br />

### **v1.0.0-rc.2** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 20-01-2025
-> [DEV] - Add the `fixed` element/attribute validation and xsd emit. <br />
-> [DEV] - Adjust the element/attribute `Required` to the element and element with value validation. <br />
-> [DEV] - Adjust the docs from the readme and using.

### **v1.0.0-rc.1** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 10-01-2025
-> [DEV] - Add the `null` (`nil`) element validation and xsd emit. <br />
-> [DEV] - Adjust the docs from readme and using.

### v**1.0.0-alpha.4** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 06-01-2025
-> [DEV] - Add possibilities to set inclusive/exclusive length validation (`WithElementInRange`, `WithAttributeInRange`);<br />
-> [DEV] - Add new validation rule for exact value length (`WithElementExactLength`, `WithAttributeExactLength`);<br />
-> [DEV] - Add validation for range values (`WithElementEnumerator`, `WithAttributeEnumerator`);<br />
-> [DEV] - Add element/attribute documentation(annotation) (`WithElementDocumentation`, `WithAttributeDocumentation`);<br />

### v**1.0.0-alpha.3** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 29-12-2025
-> [DEV] - Add XSD generator (`XsdGenerator`) based on defined XML validation rules;<br />
-> [DEV] - Add XSD to string export;<br />
-> [DEV] - Add small XSD regex translator (from .NET to XSD);<br />
-> [DEV] - Add new validation value length methods: `WithElementValueLength`, `WithAttributeValueLength`;<br />
-> [DEV] - Add new validation data type methods: `WithElementDataType`, `WithAttributeDataType`;<br />
-> [DEV] - Rename almost all defined methods by adding `WithElement*` or `WithAttribute*` in dependence on which validation is applied;<br />

### v**1.0.0-alpha.2** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 18-12-2025
-> [DEV] - Refactor all message formats and workflow;<br />
-> [DEV] - Changed the message priority `WithMessageForAll` -> `WithMessage` -> validation message from method;<br />

### v**1.0.0-alpha** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 15-12-2025
-> [DEV] - Add init validation flow; <br />
-> [DEV] - Add validation methods: `MustExist`, `Count`, `Value`, `Attribute`, `All`, `Any`, `When`, `Optional`, `Required`, `MatchesRegex`, `InRange`, `Unique`, `MaxOccurs`, `RequiredAttribute`, `AttributeMatchesRegex`, `AttributeInRange`, `AttributeUnique`, `ElementAttributeCrossRule`, `CustomElementRule`, `Custom`, `UseCustomRule`, `WithName`, `WithSeverity`, `StopOnFailure`, `WithMessage`, `WithMessage`, `WithMessageForAll`, `Done`.
