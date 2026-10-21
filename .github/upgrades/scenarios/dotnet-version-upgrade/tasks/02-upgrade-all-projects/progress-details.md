# 02-upgrade-all-projects — Progress Details

## Changes
- **Depends.Core.csproj**: net8.0 → net10.0; Microsoft.Build 17.11.4 → 18.9.6 (vuln fix); NuGet.ProjectModel/NuGet.Resolver 6.11.1 → 6.14.3 (vuln fix); added explicit pins for transitive packages flagged by NuGet audit: Microsoft.Build.Tasks.Core / Microsoft.Build.Utilities.Core 18.9.6 (were 17.0.1 via Buildalyzer), System.Security.Cryptography.Pkcs / Xml 10.0.13.
- **Snitch.csproj**: net8.0 → net10.0; Microsoft.CodeAnalysis.NetAnalyzers 8.0.0 → 10.0.402.
- **Snitch.Tests.csproj**: net8.0 → net10.0.
- **Pre-existing warning fixes (Snitch)**:
  - `PackageToRemove.cs`, `ProjectAnalyzer.cs`: `out Package pd` → `out var pd` (CS8600).
  - `ProjectPackage.cs`: target-typed `new` → explicit type, trailing whitespace removed (SA1000/SA1028).
  - `AnalyzeCommand.cs`: `System` usings ordered first (SA1208).
  - `AnalyzerResultExtensions.cs`: `NuGetFrameworkFullComparer.Instance` (CS0618); null result from `GetNearest` now throws a descriptive `InvalidOperationException` instead of an `ArgumentNullException` from the dictionary indexer (CS8604). Comparer now also applied to the outer dictionary.

## Validation
- `dotnet build Snitch.sln`: 0 errors, 0 warnings (baseline on net8.0 was 17 warnings).

## Deferred
- xunit 2.9.2 / Verify.Xunit 14.11.4 are deprecated (successors xunit.v3 / Verify.XunitV3) — separate test-framework migration, not required for .NET 10.
