
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using pax.XRechnung.NET.Dtos;
using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET;

/// <summary>
/// XmlWriterOptions
/// </summary>
public static class XmlInvoiceWriter
{
    private static XmlSerializerNamespaces? _namespaces;
    private static XmlWriterSettings? _writerSettings;
    private static XmlSchemaSet? xmlSchemaSet;
    private static readonly object schemaLock = new();

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

    /// <summary>
    /// XmlSchemaSet
    /// </summary>
    /// <returns>XmlSchemaSet</returns>
    public static XmlSchemaSet GetSchemaSet()
    {
        lock (schemaLock)
        {
            if (xmlSchemaSet is not null)
            {
                return xmlSchemaSet;
            }
            var schemaRoute = "pax.XRechnung.NET.Resources.XmlSchemas.ubl._2._1.xsd";
            var schemaSet = new XmlSchemaSet();
            AddSchema(InvoiceSchema, schemaRoute + ".maindoc." + "UBL-Invoice-2.1.xsd", schemaSet);
            AddSchema(CommonAggregateComponents, schemaRoute + ".common." + "UBL-CommonAggregateComponents-2.1.xsd", schemaSet);
            AddSchema(CommonBasicComponents, schemaRoute + ".common." + "UBL-CommonBasicComponents-2.1.xsd", schemaSet);
            AddSchema(CommonExtensionComponents, schemaRoute + ".common." + "UBL-CommonExtensionComponents-2.1.xsd", schemaSet);
            AddSchema("urn:oasis:names:specification:ubl:schema:xsd:QualifiedDataTypes-2", schemaRoute + ".common." + "UBL-QualifiedDataTypes-2.1.xsd", schemaSet);
            AddSchema("urn:oasis:names:specification:ubl:schema:xsd:UnqualifiedDataTypes-2", schemaRoute + ".common." + "UBL-UnqualifiedDataTypes-2.1.xsd", schemaSet);
            AddSchema("urn:un:unece:uncefact:data:specification:CoreComponentTypeSchemaModule:2", schemaRoute + ".common." + "CCTS_CCT_SchemaModule-2.1.xsd", schemaSet);
            AddSchema("urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2", schemaRoute + ".common." + "UBL-ExtensionContentDataType-2.1.xsd", schemaSet);
            xmlSchemaSet = schemaSet;
            return xmlSchemaSet;
        }
    }

    private static void AddSchema(string targetNameSpace, string source, XmlSchemaSet xmlSchemaSet)
    {
        using var reader = LoadEmbeddedResource(source);
        xmlSchemaSet.Add(targetNameSpace, reader);
    }

    private static XmlReader LoadEmbeddedResource(string ressourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var stream = assembly.GetManifestResourceStream(ressourceName);
        if (stream == null)
            throw new FileNotFoundException($"Embedded resource not found: {ressourceName}");

        return XmlReader.Create(stream);
    }

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
                Encoding = new UTF8Encoding(false),
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
    public static string Serialize(InvoiceDto invoice)
    {
        var xmlInvoice = XmlInvoiceMapper.MapToXmlInvoice(invoice);
        var xml = SerializeToXDocument(xmlInvoice, GetNamespaces());

        RemoveNullElements(xml);
        FormatDateTimeElements(xml);

        return WriteToString(xml);
    }

    /// <summary>
    /// Serialize to xml string
    /// </summary>
    /// <param name="invoice"></param>
    /// <returns></returns>
    public static string Serialize(XmlInvoice invoice)
    {
        ArgumentNullException.ThrowIfNull(invoice);
        invoice.IssueDate = XmlInvoiceMapper.GetXmlDate(invoice.IssueDate);
        invoice.DueDate = XmlInvoiceMapper.GetXmlDate(invoice.DueDate);
        var xml = SerializeToXDocument(invoice, GetNamespaces());

        RemoveNullElements(xml);
        FormatDateTimeElements(xml);

        return WriteToString(xml);
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
            // 2025-02-02T00:00:00Z
            "yyyy-MM-ddTHH:mm:ssZ"
        ];

        foreach (var element in xml.Descendants())
        {
            if (DateTime.TryParseExact(element.Value, dateTimeFormats,
                CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var dateTime))
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

    private static string WriteToString(XDocument xml)
    {
        using var memory = new MemoryStream();
        using var writer = new StreamWriter(memory, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        xml.Save(writer);
        writer.Flush();
        return Encoding.UTF8.GetString(memory.ToArray());
    }

}
