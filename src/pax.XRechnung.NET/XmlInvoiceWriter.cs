
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET;

/// <summary>
/// XmlWriterOptions
/// </summary>
public static class XmlInvoiceWriter
{
    private static XmlSerializerNamespaces? _namespaces;
    private static XmlWriterSettings? _writerSettings;

    /// <summary>
    /// CommonAggregateComponents Schema
    /// </summary>
    public const string CommonAggregateComponents =
        "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
    /// <summary>
    /// CommonBasicComponents Schema
    /// </summary>
    public const string CommonBasicComponents =
        "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
    /// <summary>
    /// CommonExtensionComponents Schema
    /// </summary>
    public const string CommonExtensionComponents =
        "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2";
    /// <summary>
    /// Invoice Schema
    /// </summary>
    public const string InvoiceSchema =
        "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2";

    private static XmlSerializerNamespaces GetNamespaces()
    {
        if (_namespaces == null)
        {
            _namespaces = new XmlSerializerNamespaces();
            _namespaces.Add("ubl", InvoiceSchema);
            _namespaces.Add("cac", CommonAggregateComponents);
            _namespaces.Add("cbc", CommonBasicComponents);
            _namespaces.Add("cec", CommonExtensionComponents);
        }
        return _namespaces;
    }

    private static XmlWriterSettings GetSettings()
    {
        if (_writerSettings == null)
        {
            _writerSettings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = false
            };
        }
        return _writerSettings;
    }

    /// <summary>
    /// Serialize to xml string
    /// </summary>
    /// <param name="invoice"></param>
    /// <returns></returns>
    public static string Serialize(XmlInvoice invoice)
    {
        var xml = SerializeToXDocument(invoice, GetNamespaces());

        RemoveNullElements(xml);
        FormatDateTimeElements(xml);

        return WriteToString(xml, GetSettings());
    }

    private static void RemoveNullElements(XDocument xml)
    {
        var xsiNamespace = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
        xml.Descendants()
           .Where(e => (string?)e.Attribute(xsiNamespace + "nil") == "true")
           .Remove();
    }

    private static void FormatDateTimeElements(XDocument xml)
    {
        string[] dateTimeFormats = [
            "yyyy-MM-ddTHH:mm:ss.fffffffZ",
            "yyyy-MM-ddTHH:mm:ss.ffffffZ",
            "yyyy-MM-ddTHH:mm:ss.fffffZ"
        ];

        foreach (var element in xml.Descendants())
        {
            if (DateTime.TryParseExact(element.Value, dateTimeFormats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var dateTime))
            {
                element.Value = dateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
        }
    }

    private static XDocument SerializeToXDocument(XmlInvoice invoice, XmlSerializerNamespaces namespaces)
    {
        using var stream = new MemoryStream();
        var serializer = new XmlSerializer(typeof(XmlInvoice));
        serializer.Serialize(stream, invoice, namespaces);
        stream.Seek(0, SeekOrigin.Begin);
        var xDocument = XDocument.Load(stream);
        xDocument.Declaration = new XDeclaration("1.0", "UTF-8", null);
        return xDocument;
    }

    private static string WriteToString(XDocument xml, XmlWriterSettings settings)
    {
        var memory = new MemoryStream();
        xml.Save(memory);
        return Encoding.UTF8.GetString(memory.ToArray());
    }
}
