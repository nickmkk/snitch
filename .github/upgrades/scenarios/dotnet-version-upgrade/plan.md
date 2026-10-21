# .NET 10 Upgrade Plan

## Overview

**Target**: Upgrade Depends.Core, Snitch, and Snitch.Tests from net8.0 to net10.0
**Scope**: 3 SDK-style projects, ~2.3k LOC; test fixture projects (netstandard2.0) are out of scope

### Selected Strategy
**All-At-Once** — All projects upgraded simultaneously in a single operation.
**Rationale**: 3 projects, all on .NET 8, SDK-style, linear dependency chain (Snitch.Tests → Snitch → Depends.Core), no API incompatibilities reported.

## Tasks

### 01-prerequisites: Verify .NET 10 SDK and update global.json

The repo-level `global.json` pins SDK `8.0.407` with `rollForward: latestPatch`, which blocks building net10.0. The machine has SDK 10.0.401 installed. Update `global.json` to a .NET 10 SDK so local builds and the CI workflow (`actions/setup-dotnet` reads global.json) use .NET 10.

**Done when**: `global.json` references a 10.0 SDK and `dotnet --version` in the repo resolves to a 10.0.x SDK.

---

### 02-upgrade-all-projects: Retarget all projects to net10.0 and update packages

Change `TargetFramework(s)` from net8.0 to net10.0 in Depends.Core, Snitch, and Snitch.Tests. Address package findings from the assessment: Microsoft.Build 17.11.4 has a security vulnerability (suggested 18.9.6) in Depends.Core; Microsoft.CodeAnalysis.NetAnalyzers 8.0.0 in Snitch should align with .NET 10; xunit 2.9.2 and Verify.Xunit 14.11.4 in Snitch.Tests are flagged deprecated.

Research: verify Microsoft.Build 18.x compatibility with Buildalyzer 7.0.2 (MSBuild APIs used by Depends.Core), review whether the deprecated xunit/Verify.Xunit packages can be bumped without a major xunit v3 migration, and fix any new analyzer warnings surfaced by the newer SDK.

**Done when**: All three projects target net10.0, the Microsoft.Build vulnerability is resolved, and the solution builds with 0 errors and no new warnings.

---

### 03-final-validation: Build and test the full solution

Run a full solution build and the Snitch.Tests suite (which exercises Snitch against the netstandard2.0 fixture projects) on .NET 10. Document any deferred recommendations (e.g., xunit v3 migration if not done).

**Done when**: Solution builds cleanly and all tests pass.
