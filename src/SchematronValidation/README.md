
# Schamatron validation

## Requironments
`https://github.com/schxslt/schxslt/releases` xslt-only
`https://github.com/Saxonica/Saxon-HE/releases/` SaxonHE12-5J.zip

* Convert *.sch to xstl
```bash
java -jar SaxonHE12-5J/saxon-he-12.5.jar -s:PEPPOL-EN16931-UBL.sch -xsl:schxslt-1.10.1-xslt-only/schxslt-1.10.1/2.
0/compile/compile-2.0.xsl -o:PEPPOL-EN16931-UBL.xslt
```
