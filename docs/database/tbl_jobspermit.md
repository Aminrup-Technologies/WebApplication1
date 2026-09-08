# tbl_jobspermit (call contract)

**Baseline:** `v2.2.3-governance-final`  
**Status:** Columns from SELECT/UPDATE only. Insert path is the permit-upload page (file bytes live on this table — JOB360 comments warn against `SELECT *` of the binary column).

## Evidenced columns

| Column | Evidence |
| --- | --- |
| `Id`, `JOBID` | Keys in WHERE |
| `Name`, `TimeStamp` | List SELECT |
| `Submitter_Wrk`, `Submitter_Name` | Filter / JOB360 list |
| `UploadType`, `Extension` | JOB360 SELECT |
| `DownloadStatus` | JOB360 UPDATE `=1` |
| `DeleteStatus`, `DeletedOn`, `DeletedBy` | V2 soft-delete UPDATE |
| File bytes | **Inferred** column name not cited here; JOB360 loads a column list “excluding the raw binary data column” |

Legacy `view_jobdetails.aspx.cs` still has string-concat `DELETE`. V2 uses parameterized soft-delete. Prefer V2.

## Related

- [tbl_jobs.md](tbl_jobs.md)
- [jobid.md](../modules/jobid.md)
