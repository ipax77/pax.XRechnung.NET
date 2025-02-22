from lxml import etree

def validate_schematron(xml_file, schematron_xslt):
    xml_doc = etree.parse(xml_file)
    xslt_doc = etree.parse(schematron_xslt)
    transform = etree.XSLT(xslt_doc)
    result = transform(xml_doc)

    print(result)  # Output validation result
    return result

# Convert PEPPOL-EN16931-UBL.sch to XSLT first (use schxslt CLI)
xml_invoice = "invoicetest.xml"
schematron_xslt = "PEPPOL-EN16931-UBL.xslt"  # Generate using schxslt first

validate_schematron(xml_invoice, schematron_xslt)