## Summary

<!-- What changed and why. -->

## Security Foundation Checklist

Security Foundation v2.2 (`v2.2-security-foundation`) is the platform baseline. See `CONTRIBUTING.md` and `SECURITY_FOUNDATION_BASELINE.md`.

- [ ] Uses AuthorizationService
- [ ] No new hardcoded WorkmanSL gates
- [ ] No new Session privilege checks
- [ ] Overlay considered
- [ ] Release evidence updated (if applicable)

## Documentation Impact

Mandatory mapping: `docs/governance/documentation_impact_matrix.md`. Charter: `docs/ERP_DOCUMENTATION_CHARTER.md`. If evidence is missing, add a row to `docs/EVIDENCE_GAP_REGISTER.md` — do not invent schema.

- [ ] Impact matrix reviewed (N/A if docs-only with no index change)
- [ ] Module / architecture / database docs updated where the matrix requires it
- [ ] `docs/ERP_DOCUMENTATION_INDEX.md` lists any new document
- [ ] Coverage dashboard recounted if an index area changed (`docs/DOCUMENTATION_COVERAGE.md`)
- [ ] Evidence gaps filed or closed (`docs/EVIDENCE_GAP_REGISTER.md`)
- [ ] Release add-on template filled if this PR is part of a release (`docs/release/TEMPLATE/`)

## Notes

<!-- If a box is unchecked, explain. Overlay considered = new platform permission catalogued in tlb_permissions / granted via tlb_employee_permissions, or an explicit reason the overlay does not apply. Release evidence = copy/fill docs/release/TEMPLATE/ when this PR is part of a release. -->
