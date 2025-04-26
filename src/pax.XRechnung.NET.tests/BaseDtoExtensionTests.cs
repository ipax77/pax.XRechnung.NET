
using pax.XRechnung.NET.BaseDtos;
using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET.tests;

[TestClass]
public class BaseDtoExtensionTests
{
    public static InvoiceExtendedDto GetInvoiceBaseDto()
    {
        return new()
        {
            GlobalTaxCategory = "S",
            GlobalTaxScheme = "VAT",
            GlobalTax = 19.0,
            Id = "1",
            IssueDate = DateTime.UtcNow,
            InvoiceTypeCode = "380",
            DocumentCurrencyCode = "EUR",
            BuyerReference = "04011000-12345-34",
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
            },
            PaymentMeans = new PaymentMeansBaseDto()
            {
                Iban = "DE12 1234 1234 1234 1234 12",
                Bic = "BICABCDE",
                Name = "Bank Name"
            },
            PaymentMeansTypeCode = "30",
            PaymentTermsNote = "Zahlbar innerhalb 14 Tagen nach Erhalt der Rechnung.",
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

    [TestMethod]
    public void InvoiceBaseDtoMapTest()
    {
        var invoiceExtendedDto = GetInvoiceBaseDto();
        invoiceExtendedDto.AdditionalDocumentReferences = [
            new()
            {
                Id = "1",
                DocumentDescription = "pdf",
                MimeCode = "application/pdf",
                FileName = "invoice.pdf",
                Content = "R0lGODlhAQABAAAAACw=",
            }
        ];
        var mapper = new InvoiceExtendedMapper();
        var xmlInvoice = mapper.ToXml(invoiceExtendedDto);
        var doc = xmlInvoice.AdditionalDocumentReferences.FirstOrDefault();
        Assert.IsNotNull(doc);
        Assert.IsNotNull(doc.Attachment);
        Assert.AreEqual(doc.Attachment.EmbeddedDocumentBinaryObject.Content,
            invoiceExtendedDto.AdditionalDocumentReferences[0].Content);
    }

    [TestMethod]
    public void InvoiceBaseDtoSchemaIsValidTest()
    {
        var invoiceExtendedDto = GetInvoiceBaseDto();
        invoiceExtendedDto.AdditionalDocumentReferences = [
            new()
            {
                Id = "1",
                DocumentDescription = "pdf",
                MimeCode = "application/pdf",
                FileName = "invoice.pdf",
                Content = "R0lGODlhAQABAAAAACw=",
            }
        ];
        var mapper = new InvoiceExtendedMapper();
        var xmlInvoice = mapper.ToXml(invoiceExtendedDto);
        var result = XmlInvoiceValidator.Validate(xmlInvoice);
        Assert.IsTrue(result.IsValid);
    }
}

public class InvoiceExtendedMapper : InvoiceMapperBase<InvoiceExtendedDto>
{
    private readonly InvoiceMapper baseMapper = new();


    public override InvoiceExtendedDto FromXml(XmlInvoice xmlInvoice)
    {
        var dto = baseMapper.FromXml(xmlInvoice) as InvoiceExtendedDto
            ?? throw new InvalidCastException("base.FromXml did not return InvoiceExtendedDto.");

        dto.AdditionalDocumentReferences = xmlInvoice.AdditionalDocumentReferences
            .Select(x => new AdditionalDocumentReferenceDto
            {
                Id = x.Id.Content,
                DocumentDescription = x.DocumentDescription ?? string.Empty,
                MimeCode = x.Attachment?.EmbeddedDocumentBinaryObject.MimeCode ?? string.Empty,
                FileName = x.Attachment?.EmbeddedDocumentBinaryObject.FileName ?? string.Empty,
                Content = x.Attachment?.EmbeddedDocumentBinaryObject.Content ?? string.Empty,
            }).ToList();

        return dto;
    }

    public override XmlInvoice ToXml(InvoiceExtendedDto dto)
    {
        var xml = baseMapper.ToXml(dto);

        if (dto.AdditionalDocumentReferences.Count != 0)
        {
            xml.AdditionalDocumentReferences = dto.AdditionalDocumentReferences.Select(x => new XmlAdditionalDocumentReference
            {
                Id = new() { Content = x.Id },
                DocumentDescription = x.DocumentDescription,
                Attachment = new()
                {
                    EmbeddedDocumentBinaryObject = new()
                    {
                        MimeCode = x.MimeCode,
                        FileName = x.FileName,
                        Content = x.Content
                    }
                }
            }).ToList();
        }

        return xml;
    }
}



public class InvoiceExtendedDto : InvoiceBaseDto
{
    /// <summary>
    /// Additional documents attached to the invoice (e.g., contract, timesheet)
    /// </summary>
    public List<AdditionalDocumentReferenceDto> AdditionalDocumentReferences { get; set; } = [];
}

public class AdditionalDocumentReferenceDto
{
    public string Id { get; set; } = string.Empty;
    public string DocumentDescription { get; set; } = string.Empty;
    public string MimeCode { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}