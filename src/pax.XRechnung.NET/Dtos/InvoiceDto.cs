namespace pax.XRechnung.NET;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class InvoiceDto
{
    public required string Id { get; set; }
    public required string InvoiceTypeCode { get; set; }
    public required string BuyerReference { get; set; }
    public required string DocumentCurrencyCode { get; set; }
    public required DateTime IssueDate { get; set; }
    public DateTime? DueDate { get; set; }
    public required string Note { get; set; }
    public required PartyDto SellerParty { get; set; }
    public required PartyDto BuyerParty { get; set; }
    public required PaymentMeansDto PaymentMeans { get; set; }
    public required MonetaryTotalDto LegalMonetaryTotal { get; set; }
    public required TaxTotalDto TaxTotal { get; set; }
}

public class PartyDto
{
    public required string EndpointId { get; set; }
    public required string PartyName { get; set; }
    public required AddressDto PostalAddress { get; set; }
    public required TaxSchemeDto PartyTaxScheme { get; set; }
    public required LegalEntityDto PartyLegalEntity { get; set; }
    public required ContactDto Contact { get; set; }
}

public class AddressDto
{
    public required string StreetName { get; set; }
    public required string City { get; set; }
    public required string PostCode { get; set; }
}

public class TaxSchemeDto
{
    public required string CompanyId { get; set; }
    public required string TaxSchemeId { get; set; }
}

public class LegalEntityDto
{
    public required string RegistrationName { get; set; }
}

public class ContactDto
{
    public required string Name { get; set; }
    public required string Telephone { get; set; }
    public required string Email { get; set; }
}

public class PaymentMeansDto
{
    public required string PaymentMeansTypeCode { get; set; }
    public required List<FinancialAccountDto> PayeeFinancialAccount { get; set; }
}

public class FinancialAccountDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
}

public class MonetaryTotalDto
{
    public required AmountDto LineExtensionAmount { get; set; }
    public required AmountDto TaxExclusiveAmount { get; set; }
    public required AmountDto TaxInclusiveAmount { get; set; }
    public required AmountDto PayableAmount { get; set; }
}

public class AmountDto
{
    public required decimal Value { get; set; }
    public required string CurrencyID { get; set; }
}

public class TaxTotalDto
{
    public required AmountDto TaxAmount { get; set; }
    public required List<TaxSubTotalDto> TaxSubTotal { get; set; }
}

public class TaxSubTotalDto
{
    public required AmountDto TaxableAmount { get; set; }
    public required AmountDto TaxAmount { get; set; }
    public required TaxCategoryDto TaxCategory { get; set; }
}

public class TaxCategoryDto
{
    public required string Id { get; set; }
    public required decimal Percent { get; set; }
    public required TaxSchemeDto TaxScheme { get; set; }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
