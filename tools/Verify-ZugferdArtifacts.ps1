param(
    [string] $ZipPath = (Join-Path $PSScriptRoot '../artifacts/provenance/ZF233_EN_01.zip')
)
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.IO.Compression.FileSystem
$expectedZipHash = 'cca2ac49a5012ad82c2c42342968486f012d49b1d9f027119c03158c83dfe659'
if ((Get-FileHash -LiteralPath $ZipPath -Algorithm SHA256).Hash.ToLowerInvariant() -ne $expectedZipHash) {
    throw 'The official ZIP does not match the pinned SHA-256.'
}
$resourceDirectory = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../src/pax.XRechnung.NET/Resources/Zugferd/2.3.3'))
$schemaPrefix = 'Schema/3. Factur-X_1.07.3_EN16931/'
$zip = [IO.Compression.ZipFile]::OpenRead([IO.Path]::GetFullPath($ZipPath))
try {
    $entries = @($zip.Entries | Where-Object {
        $_.FullName.StartsWith($schemaPrefix, [StringComparison]::Ordinal) -and
        $_.FullName -match '\.(xsd|xslt|xml|sch)$'
    } | Sort-Object FullName)
    foreach ($entry in $entries) {
        $stream = $entry.Open()
        try { $officialHash = [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($stream)).ToLowerInvariant() }
        finally { $stream.Dispose() }
        $file = Join-Path $resourceDirectory $entry.FullName
        $copiedHash = (Get-FileHash -LiteralPath $file -Algorithm SHA256).Hash.ToLowerInvariant()
        if ($officialHash -ne $copiedHash) { throw "Official artifact mismatch: $($entry.FullName)" }
        [pscustomobject]@{ Path = $entry.FullName; SHA256 = $officialHash; Match = $true }
    }
    # Walk import/include/redefine paths from the official root, rather than assuming a dependency count.
    $seen = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    $pending = [Collections.Generic.Queue[string]]::new()
    $pending.Enqueue($schemaPrefix + 'Factur-X_1.07.3_EN16931.xsd')
    while ($pending.Count -gt 0) {
        $name = $pending.Dequeue()
        if (!$seen.Add($name)) { continue }
        if ($null -eq $zip.GetEntry($name)) { throw "Missing official XSD dependency: $name" }
        [xml] $schema = Get-Content -Raw -LiteralPath (Join-Path $resourceDirectory $name)
        $ns = [Xml.XmlNamespaceManager]::new($schema.NameTable)
        $ns.AddNamespace('xs', 'http://www.w3.org/2001/XMLSchema')
        foreach ($dependency in $schema.SelectNodes('/xs:schema/xs:import[@schemaLocation] | /xs:schema/xs:include[@schemaLocation] | /xs:schema/xs:redefine[@schemaLocation]', $ns)) {
            $baseUri = [Uri]::new('https://pax.invalid/' + $name)
            $uri = [Uri]::new($baseUri, $dependency.schemaLocation)
            if ($uri.Host -ne 'pax.invalid') { throw 'Unexpected external schema dependency' }
            $pending.Enqueue([Uri]::UnescapeDataString($uri.AbsolutePath.TrimStart('/')))
        }
    }
    if ($seen.Count -ne @($entries | Where-Object FullName -Like '*.xsd').Count) {
        throw 'The extracted XSD set differs from the complete dependency closure.'
    }
    Write-Output "Verified $($entries.Count) untouched files, including all $($seen.Count) XSDs in the official root's dependency closure."
}
finally { $zip.Dispose() }
