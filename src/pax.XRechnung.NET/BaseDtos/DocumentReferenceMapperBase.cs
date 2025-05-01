using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET.BaseDtos;

/// <summary>
/// Base class for mapping AdditionalDocumentReference
/// </summary>
public abstract class DocumentReferenceMapperBase<T> where T : IDocumentReferenceBaseDto, new()
{
    /// <summary>
    /// Map From XML
    /// </summary>
    /// <param name="xmlDoc"></param>
    /// <returns></returns>
    public virtual IDocumentReferenceBaseDto FromXml(XmlAdditionalDocumentReference xmlDoc)
    {
        ArgumentNullException.ThrowIfNull(xmlDoc);
        var dto = new T
        {
            Id = xmlDoc.Id.Content,
            DocumentDescription = xmlDoc.DocumentDescription ?? string.Empty,
            MimeCode = xmlDoc.Attachment?.EmbeddedDocumentBinaryObject.MimeCode ?? string.Empty,
            FileName = xmlDoc.Attachment?.EmbeddedDocumentBinaryObject.FileName ?? string.Empty,
            Content = xmlDoc.Attachment?.EmbeddedDocumentBinaryObject.Content ?? string.Empty,
        };
        return dto;
    }

    /// <summary>
    /// Map To XML
    /// </summary>
    /// <param name="docDto"></param>
    /// <returns></returns>
    public virtual XmlAdditionalDocumentReference ToXml(IDocumentReferenceBaseDto docDto)
    {
        ArgumentNullException.ThrowIfNull(docDto);
        return new()
        {
            Id = new() { Content = docDto.Id },
            DocumentDescription = string.IsNullOrEmpty(docDto.DocumentDescription) ? null : docDto.DocumentDescription,
            Attachment = new()
            {
                EmbeddedDocumentBinaryObject = new()
                {
                    MimeCode = docDto.MimeCode,
                    FileName = docDto.FileName,
                    Content = docDto.Content
                }
            }
        };
    }
}
