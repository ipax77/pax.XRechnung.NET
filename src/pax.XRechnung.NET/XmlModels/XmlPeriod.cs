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
    public XmlDate? StartDate { get; set; }
    /// <summary>
    /// StartTime
    /// </summary>
    [XmlElement("StartTime", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public XmlTime? StartTime { get; set; }
    /// <summary>
    /// EndDate
    /// </summary>
    [XmlElement("EndDate", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public XmlDate? EndDate { get; set; }
    /// <summary>
    /// EndTime
    /// </summary>
    [XmlElement("EndTime", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public XmlTime? EndTime { get; set; }
    /// <summary>
    /// DurationMeasure
    /// </summary>
    [XmlElement("DurationMeasure", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public decimal? DurationMeasure { get; set; }
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