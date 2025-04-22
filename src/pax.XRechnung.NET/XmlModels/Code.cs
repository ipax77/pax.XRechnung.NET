namespace pax.XRechnung.NET.XmlModels;

using System.Xml.Serialization;

/// <summary>
/// Mit diesem Datentyp wird ein Code abgebildet, der in einer Codeliste spezifiziert ist. 
/// </summary>
public class Code
{
    /// <summary>
    /// Content
    /// </summary>
    [XmlText]
    public string Content { get; set; } = string.Empty;
}