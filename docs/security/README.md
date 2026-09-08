# Security documentation

This folder is the **operations and threat-model** companion to [architecture](../architecture/README.md). Architecture explains **how auth works**. Security here explains **what can go wrong**, **how to verify**, and **how to change it safely**.

## Documents

| Doc | Status | Purpose |
|-----|--------|---------|
| [threat-model.md](threat-model.md) | Verified | Session, overlay, impersonation, MFA, config |
| [verification.md](verification.md) | Verified | UAT / smoke checks without inventing Admin rows |
| [change-control.md](change-control.md) | Verified | Pointers to governance matrix and CODEOWNERS |

## Do not confuse

| Topic | Canonical doc |
|-------|----------------|
| Login / Session | `docs/architecture/authentication-flow.md` |
| `CanAccess` | `docs/architecture/authorization-flow.md` |
| Switch User | `docs/architecture/switch-user.md` |
| Overlay schema | `docs/architecture/permission-overlay.md` |
| Governance | `docs/governance/security_change_matrix.md` |
| Documentation process | `docs/governance/documentation_governance.md` |
