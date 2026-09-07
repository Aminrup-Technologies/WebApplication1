# ERP release governance

Future ERP releases copy `docs/release/TEMPLATE/` to `docs/release/<version>/` and fill it during UAT. Do not invent IIS or SQL results. Sign **GO** or **NO GO** only.

The v2.2 Security Foundation pack on the UAT orchestration branch is the first filled example. That UAT PR must never be squash-merged; the template in this folder is what lands on the default branch.
