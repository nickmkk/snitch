# 02-upgrade-all-projects: Retarget all projects to net10.0 and update packages

Change `TargetFramework(s)` from net8.0 to net10.0 in Depends.Core, Snitch, and Snitch.Tests. Address package findings from the assessment: Microsoft.Build 17.11.4 has a security vulnerability (suggested 18.9.6) in Depends.Core; Microsoft.CodeAnalysis.NetAnalyzers 8.0.0 in Snitch should align with .NET 10; xunit 2.9.2 and Verify.Xunit 14.11.4 in Snitch.Tests are flagged deprecated.

Research: verify Microsoft.Build 18.x compatibility with Buildalyzer 7.0.2 (MSBuild APIs used by Depends.Core), review whether the deprecated xunit/Verify.Xunit packages can be bumped without a major xunit v3 migration, and fix any new analyzer warnings surfaced by the newer SDK.

**Done when**: All three projects target net10.0, the Microsoft.Build vulnerability is resolved, and the solution builds with 0 errors and no new warnings.

## Research

- Depends.Core.csproj: `TargetFrameworks` net8.0 → net10.0; Microsoft.Build 17.11.4 (vulnerable) → 18.9.6 (latest supported for net10.0).
- Snitch.csproj: `TargetFramework` net8.0 → net10.0; Microsoft.CodeAnalysis.NetAnalyzers 8.0.0 → 10.0.402. Has `LangVersion` 10.0 (kept as-is to avoid scope creep).
- Snitch.Tests.csproj: `TargetFramework` net8.0 → net10.0. xunit 2.9.2 / Verify.Xunit 14.11.4 are deprecated (successors: xunit.v3 / Verify.XunitV3) — that is a test framework migration, deferred (no vulnerability, works on net10.0).
- Assessment: 0 API issues in all projects. No stubs. Fixture projects (netstandard2.0) untouched.
