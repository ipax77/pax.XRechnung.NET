namespace pax.XRechnung.NET.Dtos;

/// <summary>
/// AccountingSupplierParty
/// </summary>
public record SellerDto
{
    /// <summary>
    ///  identifier
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Die Kennung des Bildungsschemas für den identifier,  ISO/IEC 6523
    /// </summary>
    public string? SchemeId { get; set; }
    /// <summary>
    /// Der vollständige Name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Handelsname des Käufers/Verkäufers (optional).
    /// </summary>
    public string? TradingName { get; set; }
    /// <summary>
    ///  Die Hauptzeile in einer Anschrift. Üblicherweise ist dies entweder Strasse und Hausnummer oder der 
    ///  Text „Postfach“ gefolgt von der Postfachnummer
    /// </summary>
    public string? StreetName { get; set; }
    /// <summary>
    /// Eine zusätzliche Adresszeile in einer Anschrift, die verwendet werden kann, um weitere Einzelheiten 
    /// in Ergänzung zur Hauptzeile anzugeben.
    /// </summary>
    public string? AdditionalStreetName { get; set; }
    /// <summary>
    /// Eine zusätzliche Adresszeile in einer Anschrift, die verwendet werden kann, um weitere Einzelheiten 
    /// in Ergänzung zur Hauptzeile anzugeben.
    /// </summary>
    public string? BlockName { get; set; }
    /// <summary>
    ///  Die Bezeichnung der Stadt oder Gemeinde, in der sich die Verkäuferanschrift befindet.
    /// </summary>
    public string City { get; set; } = string.Empty;
    /// <summary>
    ///  Die Postleitzahl
    /// </summary>
    public string PostCode { get; set; } = string.Empty;
    /// <summary>
    ///  Ein Code, mit dem das Land bezeichnet wird
    /// </summary>
    public string Country { get; set; } = "DE";
    /// <summary>
    /// Angaben zu Ansprechpartner oder Kontaktstelle (wie z. B. Name einer Person, Abteilungs- oder Bürobezeichnung)
    /// </summary>
    public string ContactName { get; set; } = string.Empty;
    /// <summary>
    ///  Telefonnummer des Ansprechpartners oder der Kontaktstelle
    /// </summary>
    public string ContactTelephone { get; set; } = string.Empty;
    /// <summary>
    /// Eine E-Mail-Adresse des Ansprechpartners oder der Kontaktstelle
    /// </summary>
    public string ContactEmail { get; set; } = string.Empty;
    /// <summary>
    /// Steuerliche Kennung des Verkäufers (optional).
    /// </summary>
    public string? TaxRegistrationIdentifier { get; set; }
    /// <summary>
    /// tax-related identifier
    /// </summary>
    public string TaxCompanyId { get; set; } = string.Empty;
    /// <summary>
    /// tax scheme
    /// </summary>
    public string TaxSchemeId { get; set; } = string.Empty;
    /// <summary>
    /// Seller Name
    /// </summary>
    public string RegistrationName { get; set; } = string.Empty;
}
