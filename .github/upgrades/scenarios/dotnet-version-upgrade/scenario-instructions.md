# .NET Version Upgrade

## Upgrade Options
- **Strategy**: All-At-Once

## Strategy
**Selected**: All-At-Once
**Rationale**: 3 SDK-style projects on net8.0, linear dependency chain, no API incompatibilities.

### Execution Constraints
- All projects retargeted together; no tier ordering
- Update TFMs, then packages, restore, build and fix errors in one pass
- Validate full solution build with 0 errors before testing
- Test fixture projects (netstandard2.0) stay unchanged

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: net10.0

## Source Control
- **Source Branch**: bugfix/null-ref-errors
- **Working Branch**: upgrade-dotnet-10
- **Commit Strategy**: After Each Task
- **Branch Sync**: Auto (Merge)
