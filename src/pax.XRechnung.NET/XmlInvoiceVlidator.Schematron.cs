using System.Xml;
using System.Xml.Xsl;

namespace pax.XRechnung.NET;

public static partial class XmlInvoiceValidator
{
    private const string schematronRoute = "pax.XRechnung.NET.Resources.Schematrons";

    /// <summary>
    /// Validate xml string against Schmatrons
    /// </summary>
    /// <param name="xml"></param>
    public static ValidationResult ValidateSchematron(string xml)
    {
        try
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xml);

            using XmlReader skeletonReader = XmlInvoiceWriter
                .LoadEmbeddedResource(schematronRoute + ".iso_schematron_skeleton_for_saxon.xsl");

            using XmlReader schematronReader = XmlInvoiceWriter.LoadEmbeddedResource(schematronRoute + ".PEPPOL-EN16931-UBL.sch");

            XslCompiledTransform schematronToXslt = new();
            using MemoryStream xsltStream = new();
            using XmlWriter xsltWriter = XmlWriter.Create(xsltStream);

            schematronToXslt.Load(skeletonReader);
            schematronToXslt.Transform(schematronReader, xsltWriter);

            xsltStream.Position = 0;

            XslCompiledTransform validationXslt = new();
            using XmlReader xsltReader = XmlReader.Create(xsltStream);
            validationXslt.Load(xsltReader);

            using MemoryStream resultStream = new();
            using XmlWriter resultWriter = XmlWriter.Create(resultStream);
            validationXslt.Transform(xmlDocument, resultWriter);

            resultStream.Position = 0;
            using StreamReader reader = new(resultStream);
            string validationOutput = reader.ReadToEnd();

            bool isValid = !validationOutput.Contains("<failed-assert", StringComparison.OrdinalIgnoreCase);

            return new ValidationResult
            {
                IsValid = isValid,
                Error = isValid ? null : validationOutput
            };
        }
        catch (Exception ex)
        {
            return new ValidationResult
            {
                IsValid = false,
                Error = ex.Message
            };
            throw;
        }
    }
}