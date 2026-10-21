# 01-prerequisites: Verify .NET 10 SDK and update global.json

The repo-level `global.json` pins SDK `8.0.407` with `rollForward: latestPatch`, which blocks building net10.0. The machine has SDK 10.0.401 installed. Update `global.json` to a .NET 10 SDK so local builds and the CI workflow (`actions/setup-dotnet` reads global.json) use .NET 10.

**Done when**: `global.json` references a 10.0 SDK and `dotnet --version` in the repo resolves to a 10.0.x SDK.
