# Build evidence — Security Foundation v2.2 RC

**Solution:** `WebApplication1.sln` (Visual Studio 14 / VS2015)  
**Project:** `WebApplication1\WebApplication1.csproj`  
**Target:** .NET Framework 4.8  
**Branch:** `uat/security-foundation-v2.2`  
**Expected commit at rebuild:** at or after `965934f`

Do not assume success. Paste the Output window. Linux CI on this branch is not a VS2015 build.

## Operator

| Metric | Value |
| --- | --- |
| Tester | |
| Date / time | |
| Machine | |
| Visual Studio | 2015 (14.0.25420.1 expected) |
| Configuration | Debug / Release (circle one) |
| Platform | Any CPU |
| Git SHA rebuilt | |
| Clean Solution | |
| Rebuild Solution | |

## Expected static facts (already verified in repo)

| Metric | Value |
| --- | --- |
| Duplicate `Compile` items | 0 (339 unique at `965934f`) |
| `AuthorizationService.cs` | one csproj include |
| Pages in csproj | `SwitchUser.aspx`, `PermissionInspector.aspx`, `AccessAnalyzer.aspx` |

## Build output (paste)

```
(paste last ~30 lines of the Output window, including “Rebuild All succeeded/failed”)
```

| Metric | Value |
| --- | --- |
| Succeeded | |
| Failed | |
| Skipped | |
| Errors | |
| Warnings | |
| Missing references | none / list |

## Missing references / namespace conflicts

```
(paste if any)
```

## Gate

| Item | PASS / FAIL |
| --- | --- |
| Rebuild succeeded | |
| 0 errors | |
| No missing references | |
| No duplicate compile items in output | |

Stop if Rebuild failed. Do not recycle IIS on a failed build.
