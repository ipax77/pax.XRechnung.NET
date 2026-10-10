# Official ZUGFeRD 2.3.3 / Factur-X 1.07.3 EN16931 resources

Source URL: https://www.ferd-net.de/fileadmin/user_upload/FeRD/Downloads/ZF233_EN_01.zip
Retrieved and verified: 2026-10-10
ZIP SHA-256: `cca2ac49a5012ad82c2c42342968486f012d49b1d9f027119c03158c83dfe659`

All normative resources below were extracted directly from the publisher-hosted official FeRD ZIP. The ZIP-relative directory structure and filenames are preserved exactly under this directory. No files were renamed, reserialized, regenerated, or modified; XSD import paths and the compiled stylesheet's document() reference are unchanged. Raw-byte SHA-256 checks against the ZIP passed for all eight copied technical files. Git attributes disable text conversion for the copied official files.

Four XSDs are embedded: the EN16931 root plus its entire transitive xs:import/xs:include/xs:redefine dependency closure. The closure was traversed from the official root; it contains exactly the four XSDs listed below, with no additional dependencies elsewhere in the ZIP. The official _XSLT_EN16931 stylesheet and its code-list XML are the other two embedded runtime resources. The separate .sch and its source-side code list are packaged as untouched reference files, not used at runtime.

No Mustangproject resources, code, test fixtures, or LICENSE/NOTICE files remain in this validation addition. The existing ZUGFeRD-csharp writer stays at Version23 / Comfort / CII. Its independent package license is unaffected.

## Extraction paths and raw SHA-256

Paths are relative both to ZF233_EN_01.zip and this resource directory.

| Exact ZIP entry / local relative path | Embedded | SHA-256 |
|---|---|---|
| `Schema/3. Factur-X_1.07.3_EN16931/_XSLT_EN16931/FACTUR-X_EN16931_codedb.xml` | Yes | `33d04741e2970478d2e1129854b8960aef972e16e0352dd16108d35f94ad147a` |
| `Schema/3. Factur-X_1.07.3_EN16931/_XSLT_EN16931/FACTUR-X_EN16931.xslt` | Yes | `af0473fbae13ca9d46c7c70a5dc2ef95985544e779d9be8a7fc84c2555864cea` |
| `Schema/3. Factur-X_1.07.3_EN16931/Factur-X_1.07.3_EN16931_codedb.xml` | No; reference only | `6eb005e7ff2802f728aa9a3332e89dc4a5f88ef2359ccddb3125e2e5ee43caa9` |
| `Schema/3. Factur-X_1.07.3_EN16931/Factur-X_1.07.3_EN16931_urn_un_unece_uncefact_data_standard_QualifiedDataType_100.xsd` | Yes | `5a3ce756cfa8d4f2ff3165d68123cfb7fbf3c7b64664edb0de38cca64d5c413b` |
| `Schema/3. Factur-X_1.07.3_EN16931/Factur-X_1.07.3_EN16931_urn_un_unece_uncefact_data_standard_ReusableAggregateBusinessInformationEntity_100.xsd` | Yes | `0c024357d6e5f96a0699cbb425d21bb8db4c65450ad9c265c30a8aad50c46e46` |
| `Schema/3. Factur-X_1.07.3_EN16931/Factur-X_1.07.3_EN16931_urn_un_unece_uncefact_data_standard_UnqualifiedDataType_100.xsd` | Yes | `9590182879865144dfc6ae069ef7499defaa7d31cb62e30ccacf9146ace5306d` |
| `Schema/3. Factur-X_1.07.3_EN16931/Factur-X_1.07.3_EN16931.sch` | No; reference only | `74781c081d3a2cbeed85c23e8637bd2ac8fef97416c86db60548699dfcb0c801` |
| `Schema/3. Factur-X_1.07.3_EN16931/Factur-X_1.07.3_EN16931.xsd` | Yes | `d71afa234624a8ee03ec876caa757702806c68e2da5f77f67ae1df91964fc485` |

## Official invoice fixture

ZIP entry: `Examples/3. EN16931/EN16931_Einfach/EN16931_Einfach.xml`
Extracted unchanged to: `src/pax.XRechnung.NET.tests/Resources/zugferd/Examples/3. EN16931/EN16931_Einfach/EN16931_Einfach.xml`
Fixture SHA-256: `8cdc72bb82ac87d8a320f8324352a7b290a88a604b6349be6cc3b5e8bf39baab`
Embedded in the test assembly and WASM probe as `zugferd-invoice.xml`, not the production library.

## License and attribution

`Documentation/1. FACTUR-X 1.07.3 EN.pdf`, page 9 (Disclaimer), identifies Apache License 2.0 terms for the specification and its appendices. Its copyright attribution is ©2025 FNFE-MPE | FeRD. The dedicated `NOTICE-FeRD.txt` records that attribution and source; `LICENSE-FeRD-Apache-2.0.txt` contains the full license, obtained from https://www.apache.org/licenses/LICENSE-2.0.txt. These are package-provided notices, not invented ZIP entries or Mustang notices. See the official specification for the disclaimer and references to third-party EN16931 documents. The official example also retains the FeRD right-of-use notice in its original XML comments.

## Reproduce verification

From the repository root, in PowerShell 7:

```powershell
Invoke-WebRequest 'https://www.ferd-net.de/fileadmin/user_upload/FeRD/Downloads/ZF233_EN_01.zip' -OutFile artifacts/provenance/ZF233_EN_01.zip
./tools/Verify-ZugferdArtifacts.ps1 -ZipPath artifacts/provenance/ZF233_EN_01.zip
```

The verifier checks the pinned ZIP hash, compares every copied technical file against raw decompressed ZIP bytes, and walks the complete root-schema dependency closure. Production tests separately check the SHA-256 of all six actual embedded manifest resources and the availability of every declared XSD dependency. Validation rejects DTDs, ignores invoice-supplied schemaLocation and xml-stylesheet directives, blocks external Saxon functions/protocols, and only resolves the exact embedded stylesheet code-list URI.
