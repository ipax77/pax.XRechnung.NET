namespace pax.XRechnung.NET.BaseDtos;

/// <summary>
/// Base DTO for simplified XRechnung invoices.
/// Supports a single currency and a single VAT category/scheme for all lines.
/// Amounts are expected to be net (exclusive of tax).
/// </summary>
public partial class InvoiceBaseDto
{
    /// <summary>
    /// Global tax category (e.g., "S" for Standard rate).
    /// Applied to all invoice lines.
    /// </summary>
    public string GlobalTaxCategory { get; set; } = "S";
    /// <summary>
    /// Global tax scheme (e.g., "VAT").
    /// Applied to all invoice lines.
    /// </summary>
    public string GlobalTaxScheme { get; set; } = "VAT";
    /// <summary>
    /// Global tax used for all lines
    /// </summary>
    public double GlobalTax { get; set; } = 19.0;
    /// <summary>
    /// Eine eindeutige Kennung der Rechnung, die diese im System des Verkäufers identifiziert.
    /// </summary>
    public string Id { get; set; } = string.Empty;
    /// <summary>
    /// Das Datum, an dem die Rechnung ausgestellt wurde
    /// </summary>
    public DateTime IssueDate { get; set; }
    /// <summary>
    /// das Fälligkeitsdatum des Rechnungsbetrages
    /// </summary>
    public DateTime? DueDate { get; set; }
    /// <summary>
    /// ISO 4217 currency code (e.g., "EUR"). Used on all amounts
    /// </summary>
    public string DocumentCurrencyCode { get; set; } = "EUR";
    /// <summary>
    /// Ein vom Erwerber zugewiesener und für interne Lenkungszwecke benutzter Bezeichner
    /// </summary>
    public string BuyerReference { get; set; } = string.Empty;
    /// <summary>
    /// Seller
    /// </summary>
    public PartyBaseDto SellerParty { get; set; } = new();
    /// <summary>
    /// Buyer
    /// </summary>
    public PartyBaseDto BuyerParty { get; set; } = new();
    /// <summary>
    /// Bank account info for payment
    /// </summary>
    public PaymentMeansBaseDto PaymentMeans { get; set; } = new();
    /// <summary>
    /// Payment type code, e.g. "30"
    /// </summary>
    public string PaymentMeansTypeCode { get; set; } = "30";
    /// <summary>
    /// Payment terms note
    /// </summary>
    public string PaymentTermsNote { get; set; } = string.Empty;
    /// <summary>
    /// Total net amount
    /// </summary>
    public double LineExtensionAmount { get; set; }
    /// <summary>
    /// Tax exclusive amount (same as LineExtensionAmount here)
    /// </summary>
    public double TaxExclusiveAmount { get; set; }
    /// <summary>
    /// Tax inclusive amount (net + VAT)
    /// </summary>
    public double TaxInclusiveAmount { get; set; }
    /// <summary>
    /// Final payable amount
    /// </summary>
    public double PayableAmount { get; set; }
    /// <summary>
    /// Total tax amount
    /// </summary>
    public double TaxAmount { get; set; }

    /// <summary>
    /// Total taxable amount (should match LineExtensionAmount)
    /// </summary>
    public decimal TaxableAmount { get; set; }
    /// <summary>
    /// Invoice lines
    /// </summary>
    public List<InvoiceLineBaseDto> InvoiceLines { get; set; } = [];
}

/// <summary>
/// IBAN and BIC info
/// </summary>
public partial class PaymentMeansBaseDto
{
    /// <summary>
    /// Iban
    /// </summary>
    public string Iban { get; set; } = string.Empty;
    /// <summary>
    /// Bic
    /// </summary>
    public string Bic { get; set; } = string.Empty;
    /// <summary>
    /// Bank Name
    /// </summary>
    public string Name { get; set; } = string.Empty;
}


/// <summary>
/// Invoice Line
/// </summary>
public partial class InvoiceLineBaseDto
{
    /// <summary>
    /// Eindeutige Bezeichnung für die betreffende Rechnungsposition.
    /// </summary>
    public string Id { get; set; } = string.Empty;
    /// <summary>
    /// Ein Textvermerk, der unstrukturierte Informationen enthält, die für die Rechnungsposition maßgeblich sind.
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Die Menge zu dem in der betreffenden Zeile in Rechnung gestellten Einzelposten (Waren oder Dienstleistungen).
    /// </summary>
    public double Quantity { get; set; }
    /// <summary>
    /// Quantity unitCode UN_ECE_Recommendation_N_20_3
    /// </summary>
    public string QuantityCode { get; set; } = "HUR";
    /// <summary>
    /// Net price per unit.
    /// </summary>
    public double UnitPrice { get; set; }

    // InvoicePeriod
    /// <summary>
    /// Start
    /// </summary>
    public DateTime? StartDate { get; set; }
    /// <summary>
    /// End
    /// </summary>
    public DateTime? EndDate { get; set; }

    // Item
    /// <summary>
    /// Beschreibung des Postens. (Optional)
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Name des Postens. (Erforderlich)
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Total net amount for this line (Quantity × UnitPrice).
    /// </summary>
    public double LineTotal => Math.Round(Quantity * UnitPrice, 2);
}

/// <summary>
/// Seller / Buyer Party
/// </summary>
public class PartyBaseDto
{
    /// <summary>
    /// Website
    /// </summary>
    public string? Website { get; set; }
    /// <summary>
    /// Logo Id referencing a AdditionalDocument
    /// </summary>
    public string? LogoReferenceId { get; set; }
    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// StreetName
    /// </summary>
    public string? StreetName { get; set; }
    /// <summary>
    /// City
    /// </summary>
    public string City { get; set; } = string.Empty;
    /// <summary>
    /// PostCode
    /// </summary>
    public string PostCode { get; set; } = string.Empty;
    /// <summary>
    /// CountryCode
    /// </summary>
    public string CountryCode { get; set; } = "DE";
    /// <summary>
    /// Telefone
    /// </summary>
    public string Telefone { get; set; } = string.Empty;
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Registration Name
    /// </summary>
    public string RegistrationName { get; set; } = string.Empty;
    /// <summary>
    /// VAT number or company tax ID.
    /// </summary>
    public string TaxId { get; set; } = string.Empty;
}