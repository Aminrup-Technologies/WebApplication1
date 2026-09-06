namespace WebApplication1.bussiness.production
{
    public static class JobStatusConstants
    {
        public const string EntryExitCreated = "Created";
        public const string EntryExitEntry = "Entry";
        public const string EntryExitExit = "Exit";

        public const string StatusActive = "Active";
        public const string StatusOutPunchDone = "Out-Punch Done";

        public const string CodeCreated = "1";
        public const string CodeInPunch = "3";
        public const string CodeClosed = "4";
        public const string CodeApproved = "5";

        // Existing legacy values only.
        public const string CodeCancelled = "6";
        public const string CodeDeleted = "0";
    }
}
