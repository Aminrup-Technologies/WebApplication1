# R0 Production Deployment Runbook — v2.2 Platform Freeze

Audience: IIS / deployment engineer  
Release: `v2.2-platform-freeze`  
Deployment commit: `1dff8644b3abb5f4de6caa22ef32afd19ae18a1e`  
Executable payload: `238bd2f` (wizard shell + frozen V2 / JOB360 / helpers)  
Governance docs on this tag: `docs/CR-001_EXECUTION_BASELINE.md`  
Schema / stored-procedure changes in this release: **none**

Do **not** deploy `origin/Jul_to_Sep_2026_Suport_N_Dev_Works` tip. That head has moved past the freeze (login/memo work at `933445a` and later).

---

## 0. Preconditions

- [ ] Change freeze: no executable CRs in flight for this IIS site.
- [ ] Deployment window agreed with operations (InProc session: recycle logs everyone out).
- [ ] SQL `ats_erp` reachable from the IIS host; no schema scripts to run.
- [ ] Permit file share / folder for `~/erp_images/Permits/` exists and is writable by the app-pool identity.
- [ ] Production `Web.config`, `connections.config`, and `appsettings.secrets.config` are **on the server**, not in git.
- [ ] Previous production package location known (rollback source).

---

## 1. Identify and stop the site (brief)

Record:

| Item | Value (fill at deploy) |
|------|------------------------|
| IIS site name | |
| Application pool | |
| Physical path | |
| Previous package backup path | |
| Deployed by / date | |

- [ ] Open IIS Manager. Note site physical path and app-pool name.
- [ ] Optionally stop the site or set app pool to Stop so files are not locked.

---

## 2. IIS backup (mandatory)

`DeleteExistingFiles` is `True` in `WebApplication1/Properties/PublishProfiles/deploy.pubxml`. A publish can wipe the site folder. Backup first.

- [ ] Copy the entire IIS physical path to a dated folder, for example `D:\ATS_Backups\pre-v2.2-YYYYMMDD-HHMM\`.
- [ ] Explicitly copy (do not skip):
  - `Web.config`
  - `connections.config`
  - `appsettings.secrets.config`
  - `erp_images\` (permits and uploads)
- [ ] Confirm backup `Web.config` still contains production `DbConn` via `connections.config`.
- [ ] Do **not** replace production `machineKey` or connection strings from `Web.config.example`.

---

## 3. Checkout the immutable tag

On the build/publish machine (not by pulling latest Jul):

```bat
git fetch --tags origin
git checkout v2.2-platform-freeze
git rev-parse HEAD
git describe --tags --exact-match
```

Expected:

```
1dff8644b3abb5f4de6caa22ef32afd19ae18a1e
v2.2-platform-freeze
```

Fail the release if either line differs.

---

## 4. Publish profile

Repo profile: `WebApplication1/Properties/PublishProfiles/deploy.pubxml`

| Setting | Repo value | Production action |
|---------|------------|-------------------|
| `WebPublishMethod` | FileSystem | Keep |
| `LastUsedBuildConfiguration` | Release | Keep |
| `publishUrl` | `D:\PersonalRnD\AminrupWorks\ATS\ATS_Publish` | **Do not use.** Point at a staging folder, then copy to IIS, or set to the IIS physical path **after** backup. |
| `DeleteExistingFiles` | `True` | Dangerous on live IIS. Prefer publish to staging with delete, then robocopy to IIS **excluding** `Web.config`, `connections.config`, `appsettings.secrets.config`, `erp_images`. |
| `ExcludeApp_Data` | `True` | Keep |

Suggested publish (Visual Studio or MSBuild):

```bat
msbuild WebApplication1\WebApplication1.csproj /t:Rebuild /p:Configuration=Release /p:Platform="Any CPU" /p:DeployOnBuild=true /p:PublishProfile=deploy
```

Then copy Release output to IIS, preserving secrets:

```bat
robocopy C:\ATS_Publish_v22 <IIS_PHYSICAL_PATH> /E /XD erp_images /XF Web.config connections.config appsettings.secrets.config
```

- [ ] Build configuration is **Release**.
- [ ] Target framework remains **.NET Framework 4.8**.
- [ ] Staging folder contents include `bin\`, `bussiness\`, `Content\supervisor-wizard.css`, `Content\job360-cockpit.css`.
- [ ] Confirm `supervisor_wizard.aspx` is in the published `bussiness\production\` folder.

---

## 5. Web.config validation (production files stay on IIS)

After copy, on the **live** site folder:

- [ ] `Web.config` still present (not overwritten by example).
- [ ] `<connectionStrings configSource="connections.config" />` still resolves.
- [ ] `connections.config` `DbConn` still points at production `ats_erp`.
- [ ] `appsettings.secrets.config` still present (`SmtpPass`, `Msg91AuthKey`, etc.).
- [ ] `<compilation targetFramework="4.8"` (debug should be `false` on production if that is the existing standard).
- [ ] `<sessionState mode="InProc" timeout="20" />` — recycle will drop sessions.
- [ ] Forms auth `loginUrl="~/login.aspx"` and `defaultUrl="~/bussiness/production/homepage_v2.aspx"` unchanged.
- [ ] `httpCookies httpOnlyCookies="true"` unchanged.
- [ ] Do not paste `Web.config.example` `machineKey` onto production.

Quick SQL check (from the IIS host, using the live connection string):

```sql
SELECT TOP 1 JOBID FROM tbl_jobs ORDER BY CreatedDate DESC;
```

No DDL. No data repair as part of this release.

---

## 6. App pool recycle and cache/session

- [ ] Start the site if it was stopped.
- [ ] Recycle the application pool once after files are in place.
- [ ] Expect all InProc sessions to end; users must log in again.
- [ ] Confirm no leftover `Temporary ASP.NET Files` lock errors in Event Viewer / stdout.
- [ ] Browser: hard refresh or instruct users to refresh after login (static CSS: `job360-cockpit.css`, `supervisor-wizard.css`).

---

## 7. Post-deployment verification (engineering smoke)

Perform as a supervisor with a valid `USERID` + `WORKMAN` session. Do not use Admin-only JOB360 overrides for the happy path.

| # | Check | Pass? |
|---|--------|-------|
| 1 | `login.aspx` authenticates; lands on `homepage_v2.aspx` | ☐ |
| 2 | Create a permit-required JOBID; it appears on IN-Punch (no `MasterStatusCode='3'` gate) | ☐ |
| 3 | IN-Punch at least one worker | ☐ |
| 4 | Permit upload; leave and return; JOB still in permit inbox (`Active` + `Entry`) | ☐ |
| 5 | Second permit file; `FileCount` increases | ☐ |
| 6 | OUT remaining workers; last OUT shows Close & Send on `job_outpunch_v2` | ☐ |
| 7 | Close & Send writes Out-Punch Done / code `4` / `Exit`; job on approval list | ☐ |
| 8 | Hub: Pending OUT dropped; Pending Permit dropped after first successful upload | ☐ |
| 9 | JOB360 search with **raw** JOBID loads the cockpit (six tabs) | ☐ |
| 10 | JOB360 Permit / IN / OUT hops open V2 without decode error (encoded outbound) | ☐ |
| 11 | `supervisor_wizard.aspx` loads; Open Step reaches existing V2 URLs | ☐ |
| 12 | Non-admin cannot complete Force OUT / Delete / Cancel (click-time `IsAdmin()`) | ☐ |
| 13 | No 500s in IIS logs for the smoke JOBID | ☐ |

Record the smoke JOBID: __________________

---

## 8. Rollback (application-only)

This release did not change schema or stored-procedure contracts. Do not run reverse SQL. Do not delete JOBIDs created during the failed window unless operations explicitly request data cleanup.

1. Stop or idle the app pool.
2. Restore the backup folder over the IIS physical path (binaries, markup, **and** the backed-up `Web.config` / secrets if they were touched).
3. Recycle the app pool.
4. Repeat smoke checks 1–8 on the previous package.
5. Rollback SHA for this freeze (docs-only revert of governance is `1dff864`; executable payload rollback is the previous IIS package, not `git revert` of live data).

Previous production package: __________________

---

## 9. Sign-off

| Role | Name | Date | Signature |
|------|------|------|-----------|
| Deployment engineer | | | |
| Operations | | | |
| Client SPOC (after business UAT) | | | |

After smoke PASS, proceed to `docs/R0_BUSINESS_UAT_SIGNOFF.xlsx`. Log production observations in `docs/R0_HYPERCARE_ISSUE_REGISTER.md`. Do not start Phase B during hypercare.
