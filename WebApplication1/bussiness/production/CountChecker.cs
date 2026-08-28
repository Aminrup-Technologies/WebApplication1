using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace WebApplication1.bussiness.production
{

    public class CountChecker
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();


        public Int32 Find_ActiveJOBCountforINPunch(string workman, string region)
        {
            Int32 count = 0;
            try
            {
                string cmdString = "";
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                cmdString = "SELECT COUNT([JOBID]) FROM [tbl_jobs] WHERE [Creator_Workman] = @Creator_Workman AND [JOBID_Status] = 'Active' AND [CreatedDate] >= DATEADD(DAY, -3, GETDATE()) and CSM_Documents='Yes'";
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Creator_Workman", workman);
                count = Convert.ToInt32(cmd.ExecuteScalar());
                dbcl.Conn.Close();
                return count;
            }
            catch (SqlException ex)
            {
                count = 0;
                // Handle the exception, log it, or throw a custom exception.
                // For example, you can log the exception details to the console or a log file.
                Console.WriteLine("SQL Exception: " + ex.Message);
                //throw; // rethrow the exception if needed
            }
            catch (Exception ex)
            {
                count = 0;
                // Handle other types of exceptions if necessary
                Console.WriteLine("Exception: " + ex.Message);
                //throw; // rethrow the exception if needed
            }
            return count;
        }

        public Int32 Find_ActiveJOBCountforJOBINPunch(string workman, string region)
        {
            Int32 count = 0;
            try
            {
                string cmdString = "";
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                cmdString = "SELECT COUNT([JOBID]) FROM [tbl_jobs] WHERE [CreatedDate] >= DATEADD(DAY, -3, GETDATE()) and [Creator_Workman] = @Creator_Workman AND [JOBID_Status] = 'Active'";
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Creator_Workman", workman);
                count = Convert.ToInt32(cmd.ExecuteScalar());
                dbcl.Conn.Close();
                return count;
            }
            catch (SqlException ex)
            {
                count = 0;
                // Handle the exception, log it, or throw a custom exception.
                // For example, you can log the exception details to the console or a log file.
                Console.WriteLine("SQL Exception: " + ex.Message);
                //throw; // rethrow the exception if needed
            }
            catch (Exception ex)
            {
                count = 0;
                // Handle other types of exceptions if necessary
                Console.WriteLine("Exception: " + ex.Message);
                //throw; // rethrow the exception if needed
            }
            return count;
        }


        public Int32 Find_ActiveJOBCountforSOP(string workman, string region)
        {
            Int32 count = 0;
            try
            {
                string cmdString = "";
                dbcl.Sqlconnection();
                dbcl.ConnectDb();
                cmdString = "SELECT COUNT([JOBID]) FROM [tbl_jobs] WHERE [CreatedDate] >= DATEADD(DAY, -3, GETDATE()) and [Creator_Workman] = @Creator_Workman AND [JOBID_Status] = 'Active' AND CSM_Documents='Yes' and SOP_Count=0 and SOPID is null";
                SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Creator_Workman", workman);
                count = Convert.ToInt32(cmd.ExecuteScalar());
                dbcl.Conn.Close();
                return count;
            }
            catch (SqlException ex)
            {
                count = 0;
                // Handle the exception, log it, or throw a custom exception.
                // For example, you can log the exception details to the console or a log file.
                Console.WriteLine("SQL Exception: " + ex.Message);
                //throw; // rethrow the exception if needed
            }
            catch (Exception ex)
            {
                count = 0;
                // Handle other types of exceptions if necessary
                Console.WriteLine("Exception: " + ex.Message);
                //throw; // rethrow the exception if needed
            }
            return count;
        }


        public Int32 Find_ActiveJOBCountforOUTPunch(string workman, string region)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdString = "select COUNT(JOBID) from tbl_jobs where [CreatedDate] >= DATEADD(DAY, -3, GETDATE()) and Creator_Region=@Creator_Region and Creator_Workman=@Creator_Workman and JOBID_Status='Active' and FinalUpldStatus='Yes'";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Creator_Workman", workman);
            cmd.Parameters.AddWithValue("@Creator_Region", region);
            Int32 tbmcount = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.Conn.Close();
            return tbmcount;
        }

        public Int32 Find_PPEAuditDraftModeCount(string empwrk)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdString = "select count(Id) from tbl_manualppeaudit where DraftMode='Yes' and EmpWrk=@EmpWrk";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@EmpWrk", empwrk);
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.Conn.Close();
            return count;
        }

        public Int32 Find_PermitUploadCount(string JOBID)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdString = "select FileCount from tbl_jobs where JOBID=@JOBID";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@JOBID", JOBID);
            Int32 tbmcount = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.Conn.Close();
            return tbmcount;
        }

        public Int32 Find_PendingPermitUpload(string workman)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdString = "select COUNT(JOBID) from tbl_jobs where [CreatedDate] >= DATEADD(DAY, -3, GETDATE()) and Creator_Workman=@Creator_Workman and JOBID_Status='Active' and EntryExit='Entry'";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Creator_Workman", workman);
            Int32 tbmcount = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.Conn.Close();
            return tbmcount;
        }
        public Int32 Find_PendingPermitUploadStatus(string workman)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdString = "select COUNT(JOBID) from tbl_jobs where [CreatedDate] >= DATEADD(DAY, -3, GETDATE()) and Creator_Workman=@Creator_Workman and JOB_Status='In-Punch Done' and FinalUpldStatus='No'";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Creator_Workman", workman);
            Int32 tbmcount = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.Conn.Close();
            return tbmcount;
        }


        public Int32 Check_WorkmanExitstence(string workman)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdString = "select COUNT(WorkmanSL) from tbl_Employee_Mustertable where WorkmanSL=@WorkmanSL";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@WorkmanSL", workman);
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.Conn.Close();
            return count;
        }

        public Int32 Check_EmployeePunchOUT(string workman)
        {
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string cmdString = "select COUNT(EmployeeWrk) from tbl_attendance where EmployeeWrk=@EmployeeWrk and AttendanceStatus='Entry' and Outpunch_Time is NULL ";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@EmployeeWrk", workman);
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.Conn.Close();
            return count;
        }

        public Int32 CheckforPendingOUT(string jobid, string supvwrkman)
        {
            dbcl.Sqlconnection();
            string cmdstring = "select count (JOBID) from tbl_attendance where JOBID= '" + jobid + "' and Creator_Workman='" + supvwrkman + "'  and AttendanceStatus = 'Entry'";
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdstring, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.DisconnectDb();
            return count;
        }

        public Int32 CheckforPendingPermit(string jobid, string supvwrkman)
        {
            dbcl.Sqlconnection();
            string cmdstring = "select count (JOBID) from tbl_jobs where JOBID=@JOBID and Creator_Workman=@Creator_Workman and FinalUpldStatus = 'No'";
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdstring, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 120;
            cmd.Parameters.AddWithValue("@JOBID", jobid);
            cmd.Parameters.AddWithValue("@Creator_Workman", supvwrkman);
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.DisconnectDb();
            return count;
        }


        public Int32 GetPendingJOBApprovalCount(string inchargewrk)
        {
            dbcl.Sqlconnection();
            string cmdstring = "select count (Id) from tbl_jobs where [CreatedDate] >= DATEADD(DAY, -30, GETDATE()) and JOB_InchargeWrk='" + inchargewrk + "' and JOB_Status='Out-Punch Done' and EntryExit='Exit'";
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdstring, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 120;
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.DisconnectDb();
            return count;
        }

        public Int32 GetApprovedJOBCount(string inchargewrk)
        {
            dbcl.Sqlconnection();
            string cmdstring = "select count (Id) from tbl_jobs where JOB_InchargeWrk='" + inchargewrk + "' and Incharge_Approval='Approved'";
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdstring, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.DisconnectDb();
            return count;
        }

        public Int32 GetSubmittedTBTCount(string supvwrk)
        {
            dbcl.Sqlconnection();
            string cmdstring = "select count (Id) from tbl_toolboxtalkdata where Ref_JOBSupvWrk='" + supvwrk + "' and TBT_SupvWrk='"+supvwrk+"'";
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdstring, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.DisconnectDb();
            return count;
        }

        public Int32 GetPendingTBTCount1(string wrk)
        {
            dbcl.Sqlconnection();
            string cmdstring = "select count (Id) from tbl_toolboxtalkdata where SafetySupvWrk='" + wrk + "' and SafetySupvApprovalStatus='Pending'";
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdstring, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.DisconnectDb();
            return count;
        }

        public Int32 GetApprovedTBTCount1(string wrk)
        {
            dbcl.Sqlconnection();
            string cmdstring = "select count (Id) from tbl_toolboxtalkdata where SafetySupvWrk='" + wrk + "' and SafetySupvApprovalStatus='Approved'";
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdstring, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.DisconnectDb();
            return count;
        }

        public Int32 GetPendingTBTCount2(string wrk)
        {
            dbcl.Sqlconnection();
            string cmdstring = "select count (Id) from tbl_toolboxtalkdata where SafetyOfficerWrk='" + wrk + "' and SO_ApprovalStatus='Pending' and SafetySupvApprovalStatus='Approved'";
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdstring, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.DisconnectDb();
            return count;
        }

        public Int32 GetApprovedTBTCount2(string wrk)
        {
            dbcl.Sqlconnection();
            string cmdstring = "select count (Id) from tbl_toolboxtalkdata where SafetyOfficerWrk='" + wrk + "' and SO_ApprovalStatus='Approved' and SafetySupvApprovalStatus='Approved'";
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdstring, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.DisconnectDb();
            return count;
        }



        //------------------- find count of TBt aganist the JOBID ------------------------------//
        public Int32 GetTBTCount(string JOBID)
        {
            dbcl.Sqlconnection();
            string cmdstring = "select count (Id) from tbl_toolboxtalkdata where Ref_JOBID='" + JOBID + "'";
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdstring, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.DisconnectDb();
            return count;
        }

        public bool TBTRecordExist(string JOBID, string jobDate)
        {
            dbcl.Sqlconnection();  // Assuming dbcl is an instance of a class that manages database connections

            string cmdstring = "IF EXISTS (SELECT 1 FROM tbl_toolboxtalkdata WHERE Ref_JOBID = @JOBID AND Ref_JOBDate = @JobDate) SELECT 1 ELSE SELECT 0";
            dbcl.ConnectDb();

            try
            {
                using (SqlCommand cmd = new SqlCommand(cmdstring, dbcl.Conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@JOBID", JOBID);
                    cmd.Parameters.AddWithValue("@JobDate", jobDate);

                    // ExecuteScalar will return 1 if a record exists, 0 otherwise
                    int exeresult = Convert.ToInt32(cmd.ExecuteScalar());
                    dbcl.DisconnectDb();

                    return exeresult == 1;
                }
            }
            catch (Exception)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public Int32 GetSOPCount(string JOBID)
        {
            dbcl.Sqlconnection();
            string cmdstring = "select count (Ref_JOBID) from tbl_soptraining where Ref_JOBID='" + JOBID + "'";
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdstring, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.DisconnectDb();
            return count;
        }


        public Int32 GetPendingSOPCount1(string wrk)
        {
            dbcl.Sqlconnection();
            string cmdstring = "select count (Id) from tbl_soptraining where SafetySupvWrk='" + wrk + "' and SafetySupvApprovalStatus='Pending'";
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdstring, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.DisconnectDb();
            return count;
        }

        public Int32 GetPendingSOPCount2(string wrk)
        {
            dbcl.Sqlconnection();
            string cmdstring = "select count (Id) from tbl_soptraining where SafetyOfficerWrk='" + wrk + "' and SO_ApprovalStatus='Pending' and SafetySupvApprovalStatus='Approved'";
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdstring, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.DisconnectDb();
            return count;
        }
    }
}