using pax.XRechnung.NET.XmlModels;
using System.Xml;
using System.Xml.Schema;

namespace pax.XRechnung.NET;

/// <summary>
/// XmlInvoiceValidator
/// </summary>
public static partial class XmlInvoiceValidator
{
    /// <summary>
    /// Validate XmlInvoice
    /// </summary>
    /// <param name="xmlInvoice"></param>
    /// <returns></returns>
    public static InvoiceValidationResult Validate(XmlInvoice xmlInvoice)
    {
        ArgumentNullException.ThrowIfNull(xmlInvoice);
        var xml = XmlInvoiceWriter.Serialize(xmlInvoice);
        return ValidateXmlText(xml);
    }

    /// <summary>
    /// Validate xml Invoice
    /// </summary>
    /// <param name="xmlFilePath">xml file path</param>
    /// <returns></returns>
    public static InvoiceValidationResult Validate(string xmlFilePath)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(xmlFilePath);
            if (!File.Exists(xmlFilePath))
            {
                throw new FileNotFoundException(xmlFilePath);
            }

            var settings = new XmlReaderSettings
            {
                Schemas = XmlInvoiceWriter.GetSchemaSet(),
                ValidationType = ValidationType.Schema
            };

            List<ValidationEventArgs> validationEventArgs = [];
            settings.ValidationEventHandler += (sender, e) =>
            {
                validationEventArgs.Add(e);
            };

            using var reader = XmlReader.Create(xmlFilePath, settings);
            while (reader.Read()) { }

            return new InvoiceValidationResult(validationEventArgs);
        }
        catch (ArgumentNullException e)
        {
            return new InvoiceValidationResult()
            {
                Error = $"Argument error: {e.Message}"
            };
        }
        catch (FileNotFoundException e)
        {
            return new InvoiceValidationResult()
            {
                Error = $"File error: {e.Message}"
            };
        }
        catch (XmlException e)
        {
            return new InvoiceValidationResult()
            {
                Error = $"XML error: {e.Message}"
            };
        }
        catch (Exception e)
        {
            return new InvoiceValidationResult()
            {
                Error = $"Unexpected error: {e.Message}"
            };
            throw;
        }
    }

    /// <summary>
    /// Validate xml Invoice
    /// </summary>
    /// <param name="xmlText">xml string</param>
    public static InvoiceValidationResult ValidateXmlText(string xmlText)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(xmlText);

            XmlDocument document = new()
            {
                Schemas = XmlInvoiceWriter.GetSchemaSet()
            };
            var rawXmlText = GetRawXmlText(xmlText);
            document.LoadXml(rawXmlText);

            List<ValidationEventArgs> validationEventArgs = [];
            document.Validate((sender, e) =>
            {
                validationEventArgs.Add(e);
            });

            return new InvoiceValidationResult(validationEventArgs);
        }
        catch (Exception e)
        {
            var validationResult = new InvoiceValidationResult()
            {
                Error = e.Message
            };
            return validationResult;
            throw;
        }
    }

    internal static string GetRawXmlText(string xmlText)
    {
        // remove BOM if exists
        xmlText = xmlText.TrimStart('\uFEFF');
        if (xmlText.StartsWith("<?xml", StringComparison.Ordinal))
        {
            var declarationEndIndex = xmlText.IndexOf("?>", StringComparison.Ordinal);
            if (declarationEndIndex >= 0)
            {
                return xmlText[(declarationEndIndex + 2)..].TrimStart();
            }
        }
        return xmlText;
    }
}

