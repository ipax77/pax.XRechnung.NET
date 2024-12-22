[![.NET](https://github.com/ipax77/pax.XRechnung.NET/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ipax77/pax.XRechnung.NET/actions/workflows/dotnet.yml)

# Introduction

`pax.XRechnung.NET` is a .NET library for validating and mapping [XRechnung](https://xeinkauf.de/xrechnung/) XML invoices.

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
InvoiceDto invoiceDto = new()
{
    Id = "1",
    IssueDate = DateTime.UtcNow,
    DueDate = DateTime.UtcNow.AddDays(14),
    InvoiceTypeCode = "380",
    Note = "Test Note",
    DocumentCurrencyCode = "EUR",
    BuyerReference = "123",
    AdditionalDocumentReference = new()
    {
        Id = "invoice 123",
        DocumentDescription = "human readable pdf invoice",
        MimeCode = "application/pdf",
        FileName = "invoice.pdf",
        Content = "ZWYNCjE0OTk0Nw0KJSVFT0Y=",
    },
    Seller = new()
    {
        Email = "seller@email.com",
        Name = "Seller",
        StreetName = "TestStreet",
        City = "TestCity",
        PostCode = "12345",
        ContactName = "ContactName",
        ContactTelephone = "12345",
        ContactEmail = "contact@email.com",
        TaxCompanyId = "DE1234567",
        TaxSchemeId = "VAT",
        RegistrationName = "Seller Name",
    },
    Buyer = new()
    {
        Email = "buyer@email.com",
        Name = "Buyer",
        StreetName = "TestStreet1",
        City = "TestCity1",
        PostCode = "54321",
        ContactName = "ContactName1",
        ContactTelephone = "54321",
        ContactEmail = "contact1@email.com"
    },
    PaymentMeans = new()
    {
        PaymentMeansTypeCode = "30",
        IBAN = "DE21081508151234123412",
        BankName = "Test Bank"
    },
    TaxTotal = new()
    {
        TaxAmount = 4.27M,
        TaxableAmount = 22.45M,
        TaxCategoryId = "S",
        Percent = 19.0M,
        TaxScheme = "VAT"
    },
    LegalMonetaryTotal = new()
    {
        LineExtensionAmount = 22.45M,
        TaxExclusiveAmount = 22.45M,
        TaxInclusiveAmount = 26.72M,
        PayableAmount = 26.72M,
    },
    InvoiceLines = [
        new() {
            Id = "1",
            Note = "Test Note",
            Name = "Test",
            Description ="Test Desc",
            TaxId = "S",
            TaxPercent = 19.0M,
            TaxScheme = "VAT",
            InvoicedQuantity = 1,
            InvoicedQuantityCode = "XPP",
            LineExtensionAmount = 22.45M,
            PriceAmount = 22.45M,
        },
    ]
};
try
{
    var validationResult = XmlInvoiceValidator.Validate(invoiceDto);
    if (!validationResult.IsValid)
    {
        var error = valitationResult.Error ?? string.Join(", ", validationResult.Validations.Select(s => s.Message);
        Console.WriteLine($"Validation errors: {error}");
    }
    else
    {
        var xmlText = XmlInvoiceWriter.Serialize(invoiceDto);
        Console.WriteLine("Invoice serialized successfully.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
```

### Handle xml file
```csharp
var filePath = "path/to/xml";
try
{
    var serializer = new XmlSerializer(typeof(XmlInvoice));
    using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
    var xmlInvoice = (XmlInvoice?)serializer.Deserialize(stream);
    var validationResult = XmlInvoiceValidator.Validate(xmlInvoice);

    if (!validationResult.IsValid)
    {
        var error = valitationResult.Error ?? string.Join(", ", validationResult.Validations.Select(s => s.Message);
        Console.WriteLine($"Validation errors: {error}");
    }
    else
    {
        var invoiceDto = XmlInvoiceMapper.MapToInvoiceDto(xmlInvoice);
        Console.WriteLine("Invoice mapped successfully.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
```

# Known Limitations / ToDo
* **Partial DTO Implementation**: Currently, the DTOs only support a subset of XRechnung properties. Missing properties will need manual extension.
* **Incomplete XML Validation**: Not all XML element names or attributes are verified for strict compliance with XRechnung standards.
# ChangeLog

<details open="open"><summary>v0.0.1</summary>

>- Initial release
>- Support for invoice validation and serialization
>- Partial DTO implementation

</details>

