# Duplicate Compile items

**Evidence:** `CONTRIBUTING.md` language section; C# 6 codedom in `Web.config.example`.

## Symptom

MSBuild fails with duplicate `Compile` include for the same `.cs` in `WebApplication1.csproj`.

## Cause

WebForms project lists each code file. Adding a class under `App_Code` **and** a second `<Compile Include>` for the same path, or copying a page without removing the old include, duplicates the item.

## Fix

Keep Compile items **unique**. Do not add C# 7+ syntax (`out var`, throw expressions, etc.).

## Related

- [developer handbook](../developer/handbook.md)
