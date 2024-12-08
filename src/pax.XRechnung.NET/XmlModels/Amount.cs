
using System.Xml.Serialization;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// Amount ISO 15000-5:2014
/// </summary>
public class Amount
{
    /// <summary>
    /// Value
    /// </summary>
    [XmlText]
    public decimal Value { get; set; }

    /// <summary>
    /// Currency
    /// </summary>
    [XmlAttribute("currencyID")]
    public string CurrencyID { get; set; } = string.Empty;
}
