namespace WebApplication1.bussiness.production
{
    /// <summary>
    /// Frozen JOB status literals from the M1–M6 / CR-009 baseline.
    /// Values are identical to the former per-page copies. No new codes.
    /// SQL query text, inbox filters, and KPI CASE expressions stay literal.
    /// Status6 / Status0 are unofficial 360 Cancel / Delete — kept, not expanded.
    /// </summary>
    public static class JobStatusConstants
    {
        public const string Created = "Created";
        public const string Entry = "Entry";
        public const string Exit = "Exit";
        public const string OutPunchDone = "Out-Punch Done";
        public const string Status1 = "1";
        public const string Status3 = "3";
        public const string Status4 = "4";
        public const string Status5 = "5";
        public const string Status6 = "6";
        public const string Status0 = "0";
    }
}
