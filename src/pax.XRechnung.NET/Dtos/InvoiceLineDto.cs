namespace pax.XRechnung.NET.Dtos;

/// <summary>
///  Eine Gruppe von Informationselementen, die Informationen über einzelne Rechnungspositionen enthalten.
/// </summary>
public record InvoiceLineDto
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
    /// Eine vom Verkäufer angegebene Kennung für ein Objekt, auf das sich die Rechnungsposition bezieht.
    /// </summary>
    public string? ObjectIdentifier { get; set; }
    /// <summary>
    /// Die Kennung des Bildungsmusters der Objektkennung.
    /// Anmerkung: Sofern das Bildungsmuster für den Empfänger nicht klar sein sollte, sollte ein Bildungsmuster 
    /// angegeben werden, welches aus der UNTDID 1153 Codeliste gewählt werden muss.
    /// </summary>
    public string? ObjectIdentifierSchema { get; set; }

    /// <summary>
    /// Die Menge zu dem in der betreffenden Zeile in Rechnung gestellten Einzelposten (Waren oder Dienstleistungen).
    /// </summary>
    public decimal InvoicedQuantity { get; set; }
    /// <summary>
    ///  Die für die "Invoiced quantity" (BT-129) geltende Maßeinheit. Die Maßeinheit muss unter Anwendung der in UN/ECE
    ///  Rec No 20 Intro 2.a) beschriebenen Methode aus den Listen UN/ECE Recommendation No. 20 „Codes for 
    ///  Units of Measure Used in International Trade“ und UN/ECE Recommendation No 21 „Codes for Passengers, Types of
    ///  Cargo, Packages and Packaging Materials (with Complementary Codes for Package Names)“ ausgewählt werden.
    /// </summary>
    public string InvoicedQuantityCode { get; set; } = string.Empty;
    /// <summary>
    ///  Der Gesamtbetrag der Rechnungsposition. Dies ist der Betrag ohne Umsatzsteuer, aber einschließlich aller für 
    ///  die Rechnungsposition geltenden Nachlässe und Abgaben sowie sonstiger anfallender Steuern.
    /// </summary>
    public decimal LineExtensionAmount { get; set; }
    /// <summary>
    /// Eine vom Erwerber ausgegebene Kennung für eine referenzierte Position einer Bestellung/eines Auftrags.
    /// </summary>
    public string? ReferencedPurchaseOrderLineReference { get; set; }

    /// <summary>
    /// Ein Textwert, der angibt, an welcher Stelle die betreffenden Daten in den Finanzkonten des Erwerbers zu buchen sind.
    /// </summary>
    public string? BuyerAccountingReference { get; set; }
    /// <summary>
    /// Beschreibung des Postens. (Optional)
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Name des Postens. (Erforderlich)
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Verkäufer-Kennung des Postens. (Optional)
    /// </summary>
    public string? SellersIdentifier { get; set; }

    /// <summary>
    /// Käufer-Kennung des Postens. (Optional)
    /// </summary>
    public string? BuyersIdentifier { get; set; }

    /// <summary>
    /// Standard-Kennung des Postens. (Optional)
    /// </summary>
    public string? StandardIdentifier { get; set; }

    /// <summary>
    /// Klassifizierungskennungen des Postens (0..*).
    /// </summary>
    public List<string> ClassificationIdentifiers { get; set; } = [];

    /// <summary>
    /// Ursprungsland des Postens. (Optional)
    /// </summary>
    public string? CountryOfOrigin { get; set; }

    /// <summary>
    /// Liste von Attributen für die Unterposition (0..*).
    /// </summary>
    public List<ItemAttributeDto> Attributes { get; set; } = [];

    /// <summary>
    /// Der Code der für den in Rechnung gestellten Posten geltenden Umsatzsteuerkategorie.
    /// </summary>
    public string TaxId { get; set; } = string.Empty;

    /// <summary>
    /// Der Prozentsatz der Umsatzsteuer, der für den in Rechnung gestellten Posten gilt
    /// </summary>
    public decimal TaxPercent { get; set; } = 19;
    /// <summary>
    /// TaxScheme
    /// </summary>
    public string TaxScheme { get; set; } = "VAT";
    /// <summary>
    /// Der Preis eines Postens, ohne Umsatzsteuer, nach Abzug des für diese Rechnungsposition geltenden Rabatts.
    /// </summary>
    public decimal PriceAmount { get; set; }

    /// <summary>
    /// Der gesamte zur Berechnung des Netto-Postenpreises vom Brutto-Postenpreis subtrahierte Rabatt.
    /// </summary>
    public decimal? PriceDiscount { get; set; }

    /// <summary>
    /// Der Postenpreis ohne Umsatzsteuer vor Abzug des Postenpreisrabatts.
    /// </summary>
    public decimal? GrossPrice { get; set; }

    /// <summary>
    /// Die Anzahl von Einheiten, für die der Postenpreis gilt.
    /// </summary>
    public decimal? PriceBaseQuantity { get; set; }

    /// <summary>
    /// Der Code der zu Grunde gelegten Maßeinheit.
    /// </summary>
    public string? PriceBaseQuantityUnitOfMeasureCode { get; set; }
    /// <summary>
    /// Weitere untergeordnete Rechnungszeilen.
    /// </summary>
    public List<InvoiceLineDto> InvoiceLines { get; set; } = [];
}
