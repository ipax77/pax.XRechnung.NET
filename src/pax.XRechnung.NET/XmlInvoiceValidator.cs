using System.Xml;
using System.Xml.Schema;
using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET;

/// <summary>
/// XmlInvoiceValidator
/// </summary>
public static class XmlInvoiceValidator
{
    /// <summary>
    /// ValidateXmlInvoice
    /// </summary>
    /// <param name="xmlInvoice"></param>
    /// <returns></returns>
    public static ValidationResult ValidateXmlInvoice(XmlInvoice xmlInvoice)
    {
        ArgumentNullException.ThrowIfNull(xmlInvoice);
        var xml = XmlInvoiceWriter.Serialize(xmlInvoice);
        return ValidateXml(xml);
    }

    /// <summary>
    /// Validate xml Invoice
    /// </summary>
    /// <param name="xmlFilePath">xml file path</param>
    /// <returns></returns>
    public static ValidationResult ValidateFile(string xmlFilePath)
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

            return new ValidationResult(validationEventArgs);
        }
        catch (ArgumentNullException e)
        {
            return new ValidationResult()
            {
                Error = $"Argument error: {e.Message}"
            };
        }
        catch (FileNotFoundException e)
        {
            return new ValidationResult()
            {
                Error = $"File error: {e.Message}"
            };
        }
        catch (XmlException e)
        {
            return new ValidationResult()
            {
                Error = $"XML error: {e.Message}"
            };
        }
        catch (Exception e)
        {
            return new ValidationResult()
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
    public static ValidationResult ValidateXml(string xmlText)
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

            return new ValidationResult(validationEventArgs);
        }
        catch (Exception e)
        {
            var validationResult = new ValidationResult()
            {
                Error = e.Message
            };
            return validationResult;
            throw;
        }
    }

    private static string GetRawXmlText(string xmlText)
    {
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

