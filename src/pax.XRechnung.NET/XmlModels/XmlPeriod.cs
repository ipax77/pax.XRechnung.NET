using System.Xml;
using System.Xml.Serialization;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// PeriodType
/// </summary>
public class XmlPeriod
{
    /// <summary>
    /// StartDate
    /// </summary>
    [XmlElement("StartDate", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string? StartDate { get; set; }
    /// <summary>
    /// StartTime
    /// </summary>
    [XmlElement("StartTime", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string? StartTime { get; set; }
    /// <summary>
    /// EndDate
    /// </summary>
    [XmlElement("EndDate", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string? EndDate { get; set; }
    /// <summary>
    /// EndTime
    /// </summary>
    [XmlElement("EndTime", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string? EndTime { get; set; }
    /// <summary>
    /// DurationMeasure
    /// </summary>
    [XmlElement("DurationMeasure", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string? DurationMeasure { get; set; }
    /// <summary>
    /// DescriptionCode
    /// </summary>
    [XmlElement("DescriptionCode", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string? DescriptionCode { get; set; }
    /// <summary>
    /// Description
    /// </summary>
    [XmlElement("Description", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string? Description { get; set; }
}