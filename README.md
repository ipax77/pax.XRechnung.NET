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

**Validate schema and Schematron locally (.NET and Blazor WASM)**
```csharp
    var invoiceBaseDto = GetInvoiceBaseDto();
    var mapper = new InvoiceMapper();
    XmlInvoice xmlInvoice = mapper.ToXml(invoiceBaseDto);
    var result = await XmlInvoiceValidator.ValidateSchematron(xmlInvoice);
    var resultText = string.Join(Environment.NewLine, result.Validations.Select(s => $"{s.RuleSeverity}:\t{s.Code}: {s.Message}"));
    Assert.IsTrue(result.IsValid, resultText);
```
**The InvoiceBaseDto is designed to be easily extended see [BaseDtoExtensionTests](src/pax.XRechnung.NET.tests/BaseDtoExtensionTests.cs)**

`ValidateSchematron` now runs locally when no server URI is supplied. It supports
standard XRechnung 3.0 UBL Invoice documents with customization ID
`urn:cen.eu:en16931:2017#compliant#urn:xeinkauf.de:kosit:xrechnung_3.0`.
The embedded rules are pinned to KoSIT configuration **2026-08-31**, using
EN 16931 **1.3.16** and XRechnung Schematron **2.6.0**. Extension, CVD, UBL
CreditNote, CII, and other profiles require an external validator.

The validator uses **[The Saxon XSLT and XQuery Processor from Saxonica Limited](https://www.saxonica.com/)**
(SaxonCS-HE 13.0.0). It works offline after application assets load and caches
compiled stylesheets. `Code`, `Path`, and `RuleSeverity` preserve business-rule
details. Warnings and information do not invalidate an invoice. Because
`XmlSeverityType` has only Error and Warning, informational messages have
`Severity == Warning` and `RuleSeverity == "information"`. Processing failures
set `Error`; ordinary rule failures appear in `Validations`. Local results have
an empty `HttpStatusCode`.

Release-published, trimmed .NET 10 Blazor WASM was tested in Chromium, including
validation with networking disabled. This is not a guarantee for all browsers
or AOT builds. The measured cold validation took about **10.7 seconds**, and
cached validations about **100 ms**, for the small test fixture on this machine.
The validation executes on the calling thread, so cold compilation can block
the browser UI. The probe's framework assets total **13.4 MB Brotli-compressed**,
compared with **2.5 MB** for a minimal matching Blazor app. These are artifact
sizes, not measured BeInX download sizes. See [the reproducible browser probe](tools/Validator.WasmProbe/README.md).

For a consumer such as BeInX, invoke `ValidateSchematron(invoice)` even when the
configured server URL is empty. If an external validator is configured, pass
its URI explicitly:

```csharp
var result = await XmlInvoiceValidator.ValidateSchematron(
    xmlInvoice,
    string.IsNullOrWhiteSpace(config.SchematronValidationUri)
        ? null
        : new Uri(config.SchematronValidationUri));
```

This library change does not modify BeInX's export flow. That flow currently
skips Schematron validation when no server URL is configured.

## Java Schematron Validator
External validation remains available through an explicit URI, or directly
through `InvoiceKositValidator.Validate(xmlText)`, whose default is still
`http://localhost:8080`. It requires a running [KoSIT](https://github.com/itplr-kosit/validator)
validation server. Setup: [XRechnung usage](https://github.com/itplr-kosit/validator-configuration-xrechnung/blob/master/docs/usage.md).

```csharp
var result = await XmlInvoiceValidator.ValidateSchematron(
    xmlInvoice, new Uri("http://localhost:9090"));
```

The supplied URI must expose KoSIT's raw XML/HTML response; a JSON API wrapper
requires its own client. Browser HTTP validation also requires the server to
permit the application's origin through CORS.

Server start:
`java -jar .\validationtool-1.5.0-standalone.jar -s .\scenarios.xml  -r ${PWD} -D`

# ChangeLog

<details open="open"><summary>v0.3.2</summary>

>- Update to dotnet 10

</details>

<details><summary>v0.3.1</summary>

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

