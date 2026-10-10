# Browser acceptance probe

This small .NET 10 Blazor WebAssembly app calls the actual library API with valid,
repeated, incorrect-total, missing-reference, invalid-currency, warning-only,
schema-invalid, malformed, unsupported, and DTD-containing inputs. It embeds a
public fixture; invoices and rules are not fetched at runtime.

From the repository root, publish and serve the resulting static files:

```powershell
dotnet publish tools/Validator.WasmProbe -c Release -o artifacts/wasm-probe
python -m http.server 5190 --directory artifacts/wasm-probe/wwwroot
```

Open http://localhost:5190 and click **Run validation**. The JSON must contain
`passed: true`; every case reports its elapsed time and structured findings.
After the app loads, disable networking in browser developer tools and run it
again. All cases must still pass with the same findings. Disabling networking
after load verifies local validation, not installation or offline page reload.

The app loads all globalization data, matching BeInX. Publish uses trimming and
the default interpreter; AOT is not enabled. It is kept outside the main solution
so normal library builds do not require the WASM workload.

## Recorded results, 2026-10-10

- .NET SDK 10.0.400, browser runtime 10.0.11, Release trimming, Chromium.
- All ten cases passed through the public API, online and offline. Warm-run
  findings were identical; local HTTP status fields were empty.
- Cold first validation: 10,663 ms; repeated validation: 120 ms; subsequent
  business-rule cases: 95–110 ms. Warm offline calls: 99–102 ms.
- Final rerun after compiling the shared schema cache under its initialization
  lock: all ten cases still passed online and offline with identical findings;
  cold validation 10,473 ms, repeat 104 ms, warm offline validation 90 ms.
- Framework assets: 48,606,989 bytes raw / 13,408,722 bytes Brotli. Matching
  minimal Blazor template with full globalization: 8,216,145 bytes raw /
  2,493,999 bytes Brotli. Approximate incremental compressed size: 10.9 MB.
- The isolated initial engine probe produced the same codes, severities, text,
  and paths on desktop .NET and WASM. The production tests additionally cover
  33 standard fixtures, DTOs, all three severity overrides, and concurrent calls.
- Using the exact 2025-03-24 rules read from the running WSL KoSIT installation,
  five cases matched Java 1.5.0 in acceptance, codes, severity, and normalized
  paths: valid, repeated valid, incorrect payable total (`BR-CO-16`), missing
  buyer reference (`BR-DE-15`), and invalid currency (`BR-CL-04`, `BR-CO-15`).
  Those historical rules were used only in the isolated comparison workspace.
- The 2026 rules additionally report informational `BR-DE-TMP-32` on the
  original fixture. This does not invalidate the invoice.

The timings and asset sizes are measurements of this probe, not a performance
guarantee for BeInX. Cold compilation executes synchronously on the browser
thread. No BeInX source, Docker configuration, or running service was changed.
