
using pax.XRechnung.NET.BaseDtos;

namespace pax.XRechnung.NET.tests;

[TestClass]
public class BaseDtoTests
{
    [TestMethod]
    public void FromXml_MapsCorrectlyToDto()
    {
        var xml = SchematronValidationTests.GetStandardXmlInvoice();
        var mapper = new InvoiceMapper<InvoiceBaseDto>();

        var dto = mapper.FromXml(xml);

        Assert.AreEqual(xml.Id.Content, dto.Id);
        Assert.AreEqual(xml.DocumentCurrencyCode, dto.DocumentCurrencyCode);
        Assert.AreEqual(xml.SellerParty.Party.PartyName.Name, dto.SellerParty.Name);
        Assert.AreEqual(xml.InvoiceLines.Count, dto.InvoiceLines.Count);
    }

    [TestMethod]
    public void Roundtrip_ProducesEquivalentXml()
    {
        var original = SchematronValidationTests.GetStandardXmlInvoice();
        var mapper = new InvoiceMapper<InvoiceBaseDto>();

        var dto = mapper.FromXml(original);
        var roundtripXml = mapper.ToXml(dto);

        Assert.AreEqual(original.Id.Content, roundtripXml.Id.Content);
        Assert.AreEqual(original.InvoiceLines.Count, roundtripXml.InvoiceLines.Count);
        Assert.AreEqual(original.SellerParty.Party.PartyName.Name, roundtripXml.SellerParty.Party.PartyName.Name);
    }
}
