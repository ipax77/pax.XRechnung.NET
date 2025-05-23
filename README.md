![NuGet Version](https://img.shields.io/nuget/v/pax.XRechnung.NET)
[![.NET](https://github.com/ipax77/pax.XRechnung.NET/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ipax77/pax.XRechnung.NET/actions/workflows/dotnet.yml)

# Introduction

**pax.XRechnung.NET** is a .NET library that helps validate, map, and generate [XRechnung](https://xeinkauf.de/xrechnung/) XML invoices, following [specification 3.0.2](https://xeinkauf.de/app/uploads/2024/07/302-XRechnung-2024-06-20.pdf).

## Features
- ✅ **Validation**: Ensure XML invoices conform to XRechnung 3.0.2 schema and Schematron rules.
- 🔁 **Mapping**: Convert XML invoices into strongly typed DTOs.
- 🧾 **Generation**: Create compliant XML invoices from C# objects.


## Getting started

### Installation

```bash
dotnet add package pax.XRechnung.NET
```

## Usage

**Validate XML schema**
```csharp
    var xmlText = "<Invoice>...</invoice>";
    var serializer = new XmlSerializer(typeof(XmlInvoice));
    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlText));
    stream.Position = 0;
    var xmlInvoice = (XmlInvoice?)serializer.Deserialize(stream);
    Assert.IsNotNull(xmlInvoice);
    var validationResult = XmlInvoiceValidator.Validate(xmlInvoice);
    Assert.IsTrue(validationresult.IsValid);
```

### Handle Sample Invoice
```csharp
public static InvoiceBaseDto GetInvoiceBaseDto()
{
    return new()
    {
        GlobalTaxCategory = "S",
        GlobalTaxScheme = "VAT",
        GlobalTax = 19.0,
        Id = "1",
        IssueDate = new DateTime(2025, 05, 01),
        InvoiceTypeCode = "380",
        DocumentCurrencyCode = "EUR",
        SellerParty = new PartyBaseDto()
        {
            Name = "Seller Name",
            StreetName = "Test Street",
            City = "Test City",
            PostCode = "123456",
            CountryCode = "DE",
            Telefone = "1234/54321",
            Email = "seller@example.com",
            RegistrationName = "Seller Name",
            TaxId = "DE12345678"
        },
        BuyerParty = new PartyBaseDto()
        {
            Name = "Buyer Name",
            StreetName = "Test Street",
            City = "Test City",
            PostCode = "123456",
            CountryCode = "DE",
            Telefone = "1234/54321",
            Email = "buyer@example.com",
            RegistrationName = "Buyer Name",
            BuyerReference = "04011000-12345-34",
        },
        PaymentMeans = new PaymentMeansBaseDto()
        {
            Iban = "DE12 1234 1234 1234 1234 12",
            Bic = "BICABCDE",
            Name = "Bank Name",
            PaymentMeansTypeCode = "30",
        },
        PaymentTermsNote = "Zahlbar innerhalb von 14 Tagen nach Erhalt der Rechnung.",
        PayableAmount = 119.0,
        InvoiceLines = [
            new InvoiceLineBaseDto()
            {
                Id = "1",
                Quantity = 1.0,
                QuantityCode = "HUR",
                UnitPrice = 100.0,
                Name = "Test Job"
            }
        ]
    };
}
```

**Serialize DTO to XML**
```csharp
    var invoiceBaseDto = GetInvoiceBaseDto();
    var mapper = new InvoiceMapper();
    var xmlInvoice = mapper.ToXml(invoiceBaseDto);
    var xmlText = XmlInvoiceWriter.Serialize(xmlInvoice);
```

**Validate schematron - requires [Kosit validator](#java-schematron-validator)**
```csharp
    var invoiceBaseDto = GetInvoiceBaseDto();
    var mapper = new InvoiceMapper();
    XmlInvoice xmlInvoice = mapper.ToXml(invoiceBaseDto);
    var result = await XmlInvoiceValidator.ValidateSchematron(xmlInvoice);
    var resultText = string.Join(Environment.NewLine, result.Validations.Select(s => $"{s.Severity}:\t{s.Message}"));
    Assert.IsTrue(result.Validations.Count == 0, resultText);
    Assert.IsTrue(result.IsValid, resultText);
```
**The InvoiceBaseDto is designed to be easily extended see [BaseDtoExtensionTests](src/pax.XRechnung.NET.tests/BaseDtoExtensionTests.cs)**

## Java Schematron Validator
Requires a running [Kosit](https://github.com/itplr-kosit/validator) validation server. Setup: [xrechnung usage](https://github.com/itplr-kosit/validator-configuration-xrechnung/blob/master/docs/usage.md)

Server start:
`java -jar .\validationtool-1.5.0-standalone.jar -s .\scenarios.xml  -r ${PWD} -D`

# ChangeLog

<details open="open"><summary>v0.3.1</summary>

>- **Breaking Changes**
>- Code Lists fix to receive meta information
>- DTO rework to be more flexible and robust:
>- PaymentMeansTypeCode to PaymentMeans
>- BuyerReference to BuyerParty

</details>

<details><summary>v0.3.0</summary>

>- **Breaking Changes**
>- DTO rework to be more flexible and robust.
>- InvoiceAnnotationDto now available with Required fields and CodeList validation

</details>

<details><summary>v0.2.0</summary>

>- **Breaking Changes**
>- Fixed/Renamed XmlInvoice properties and dependencies. All existing properties are now xml schema conform.
>- Added Kosit schematron validation. See [Java Schematron Validator](#java-schematron-validator)
>- Replaced all DTOs with InvoiceBaseDto

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

