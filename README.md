![NuGet Version](https://img.shields.io/nuget/v/pax.XRechnung.NET)
[![.NET](https://github.com/ipax77/pax.XRechnung.NET/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ipax77/pax.XRechnung.NET/actions/workflows/dotnet.yml)

# Introduction

`pax.XRechnung.NET` is a .NET library for validating and mapping [XRechnung](https://xeinkauf.de/xrechnung/) XML invoices based on [specification 3.0.2](https://xeinkauf.de/app/uploads/2024/07/302-XRechnung-2024-06-20.pdf).

## Features
- Validate XRechnung XML invoices
- Map XML invoices to DTOs for easier manipulation
- Generate compliant XML invoices from structured DTOs

## Getting started

### Installation

```bash
dotnet add package pax.XRechnung.NET
```

## Usage

### Handle Sample Invoice
```csharp

```

# Known Limitations / ToDo
* The XmlInvoice might miss some specified properties

# Java Schematron Validator
Requires a running [Kosit](https://github.com/itplr-kosit/validator) validation server. Setup: [xrechnung usage](https://github.com/itplr-kosit/validator-configuration-xrechnung/blob/master/docs/usage.md)

Server start:
`java -jar .\validationtool-1.5.0-standalone.jar -s .\scenarios.xml  -r ${PWD} -D`

Validation:
```csharp
    var invoiceDto = GetStandardInvoiceDto();
    var xml = XmlInvoiceWriter.Serialize(invoiceDto);
    var validationResult = XmlInvoiceValidator.ValidateSchematron(xml).GetAwaiter().GetResult();

    var message = validationResult.Error != null ? validationResult.Error
        : string.Join(Environment.NewLine, validationResult.Validations.Select(s => s.Message));

    Assert.IsTrue(validationResult.IsValid, message);
```

# ChangeLog

<details open="open"><summary>v0.2.0</summary>

>- **Breaking Changes**
>- Fixed/Renamed XmlInvoice properties and dependencies. All existing properties are now xml schema conform.
>- Added Kosit schematron validation. See [Java Schematron Validator](#java-schematron-validator)
>- Minimal InvoiceDto
>- TODO:
    * Remove Dto from validation/serialization (only xml)
    * Make InvoiceDto based on InvoiceBaseDto
    * Update README sample usage
    * Validation only for the test project!?

</details>

<details><summary>v0.1.0</summary>

>- **Breaking Changes**
>- Added FinancialInstitutionBranch to FinancialAccountType (XmlPaymentInstructions)
>- Seller/Buyer cleanup and reference XmlParty
>- Changed XmlAdditionalDocumentReference to XmlAdditionalDocumentReferences as list

</details>

<details><summary>v0.0.1</summary>

>- Initial release
>- Support for invoice validation and serialization
>- Partial DTO implementation

</details>

