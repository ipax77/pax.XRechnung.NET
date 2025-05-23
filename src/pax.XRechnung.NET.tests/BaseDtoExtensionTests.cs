
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
            PaymentTermsNote = "Zahlbar innerhalb 14 Tagen nach Erhalt der Rechnung.",
            PayableAmount = 119.0,
            InvoiceLines = [
                new InvoiceLineExtendedDto()
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
        var lineDto = invoiceExtendedDto.InvoiceLines.FirstOrDefault() as InvoiceLineExtendedDto;
        Assert.IsNotNull(lineDto);
        lineDto.Attributes.Add(new() { Name = "TestName", Value = "TestValue" });
        var mapper = new InvoiceExtendedMapper();
        var xmlInvoice = mapper.ToXml(invoiceExtendedDto);
        var xmlLine = xmlInvoice.InvoiceLines.FirstOrDefault();
        Assert.IsNotNull(xmlLine);
        var attribute = xmlLine.Item.Attributes.FirstOrDefault();
        Assert.IsNotNull(attribute);
        Assert.AreEqual("TestName", attribute.Name);
        Assert.AreEqual("TestValue", attribute.Value);
    }

    [TestMethod]
    public void InvoiceExtendedDto_RoundtripMapping_WorksCorrectly()
    {
        var originalDto = GetInvoiceBaseDto();
        var originalLine = originalDto.InvoiceLines.FirstOrDefault();
        Assert.IsNotNull(originalLine);
        originalLine.Attributes.Add(new ItemAttributeDto { Name = "TestName", Value = "TestValue" });

        var mapper = new InvoiceExtendedMapper();

        var xmlInvoice = mapper.ToXml(originalDto);
        var roundtrippedDto = mapper.FromXml(xmlInvoice);
        var roundtrippedLine = roundtrippedDto.InvoiceLines.FirstOrDefault();

        Assert.IsNotNull(roundtrippedLine);
        var attribute = roundtrippedLine.Attributes.FirstOrDefault(a => a.Name == "TestName");
        Assert.IsNotNull(attribute);
        Assert.AreEqual("TestValue", attribute.Value);
    }
}

public class InvoiceExtendedMapper : InvoiceMapperBase<InvoiceExtendedDto, DocumentReferenceBaseDto, PartyBaseDto,
 PartyBaseDto, PaymentMeansBaseDto, InvoiceLineExtendedDto>
{
    public InvoiceExtendedMapper()
    : base(
        new DocumentReferenceMapper(),
        new InvoiceSellerPartyMapper(),
        new InvoiceBuyerPartyMapper(),
        new PaymentMeansMapper(),
        new InvoiceLineExtendedMapper()
    )
    {
    }
}

public class InvoiceLineExtendedMapper : InvoiceLineMapperBase<InvoiceLineExtendedDto>
{
    public override InvoiceLineExtendedDto FromXml(XmlInvoiceLine xmlLine)
    {
        var dto = base.FromXml(xmlLine) as InvoiceLineExtendedDto;
        ArgumentNullException.ThrowIfNull(dto, "unable to cast line to InvoiceLineExtendedDto");

        dto.Attributes = xmlLine.Item.Attributes.Select(s => new ItemAttributeDto()
        {
            Name = s.Name,
            Value = s.Value
        }).ToList();

        return dto;
    }

    public override XmlInvoiceLine ToXml(IInvoiceLineBaseDto dtoLine,
                                         string currencyId,
                                         string taxCategory,
                                         string taxScheme,
                                         double tax)
    {
        var xmlLine = base.ToXml(dtoLine, currencyId, taxCategory, taxScheme, tax);
        if (dtoLine is InvoiceLineExtendedDto extDtoLine)
        {
            xmlLine.Item.Attributes = extDtoLine.Attributes.Select(s => new XmlItemAttributes()
            {
                Name = s.Name,
                Value = s.Value
            }).ToList();
        }
        return xmlLine;
    }
}


public class InvoiceExtendedDto : IInvoiceBaseDto
{
    public string GlobalTaxCategory { get; set; } = "S";
    public string GlobalTaxScheme { get; set; } = "VAT";
    public double GlobalTax { get; set; } = 19.0;
    public string Id { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string InvoiceTypeCode { get; set; } = "380";
    public string? Note { get; set; }
    public string DocumentCurrencyCode { get; set; } = "EUR";
    public List<DocumentReferenceBaseDto> AdditionalDocumentReferences { get; set; } = [];
    public PartyBaseDto SellerParty { get; set; } = new PartyBaseDto();
    public PartyBaseDto BuyerParty { get; set; } = new PartyBaseDto();
    public PaymentMeansBaseDto PaymentMeans { get; set; } = new PaymentMeansBaseDto();
    public string PaymentTermsNote { get; set; } = string.Empty;
    public double PayableAmount { get; set; }
    public List<InvoiceLineExtendedDto> InvoiceLines { get; set; } = [];

    IPartyBaseDto IInvoiceBaseDto.SellerParty { get => SellerParty; set => SellerParty = (PartyBaseDto)value; }
    IPartyBaseDto IInvoiceBaseDto.BuyerParty { get => BuyerParty; set => BuyerParty = (PartyBaseDto)value; }
    IPaymentMeansBaseDto IInvoiceBaseDto.PaymentMeans { get => PaymentMeans; set => PaymentMeans = (PaymentMeansBaseDto)value; }
    List<IInvoiceLineBaseDto> IInvoiceBaseDto.InvoiceLines
    {
        get => InvoiceLines.Cast<IInvoiceLineBaseDto>().ToList();
        set => InvoiceLines = value.Cast<InvoiceLineExtendedDto>().ToList();
    }
    List<IDocumentReferenceBaseDto> IInvoiceBaseDto.AdditionalDocumentReferences
    {
        get => AdditionalDocumentReferences.Cast<IDocumentReferenceBaseDto>().ToList();
        set => AdditionalDocumentReferences = value.Cast<DocumentReferenceBaseDto>().ToList();
    }
}

public class InvoiceLineExtendedDto : IInvoiceLineBaseDto
{
    public string Id { get; set; } = string.Empty;
    public string? Note { get; set; }
    public double Quantity { get; set; }
    public string QuantityCode { get; set; } = "HUR";
    public double UnitPrice { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<ItemAttributeDto> Attributes { get; set; } = [];
    public double LineTotal => Math.Round(UnitPrice * Quantity, 2);
}

public class ItemAttributeDto
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}