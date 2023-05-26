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
            string cmdString = "";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            cmdString = "select COUNT(JOBID) from tbl_jobs where Creator_Workman=@Creator_Workman and JOBID_Status='Active'";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Creator_Workman", workman);
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.Conn.Close();
            return count;
        }

        public Int32 Find_ActiveJOBCountforOUTPunch(string workman, string region)
        {
            string cmdString = "";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            if (region == "AGL")
            {
                cmdString = "select COUNT(JOBID) from tbl_jobs where Creator_Workman=@Creator_Workman and JOBID_Status='Active'";
            }
            else
            {
                cmdString = "select COUNT(JOBID) from tbl_jobs where Creator_Workman=@Creator_Workman and JOBID_Status='Active'";
            }
            //string cmdString = "select COUNT(JOBID) from tbl_jobs where Creator_Workman=@Creator_Workman and JOBID_Status='Active' and FinalUpldStatus='Yes'";
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Creator_Workman", workman);
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
            string cmdString = "select COUNT(JOBID) from tbl_jobs where Creator_Workman=@Creator_Workman and JOBID_Status='Active' and EntryExit='Entry'";
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
            string cmdString = "select COUNT(JOBID) from tbl_jobs where Creator_Workman=@Creator_Workman and JOBID_Status='Active' and FinalUpldStatus='No'";
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
            string cmdstring = "select count (Id) from tbl_attendance where JOBID= '" + jobid + "' and Creator_Workman='" + supvwrkman + "'  and AttendanceStatus = 'Entry'";
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
            string cmdstring = "select count (Id) from tbl_jobs where JOBID= '" + jobid + "' and Creator_Workman='" + supvwrkman + "'  and FinalUpldStatus = 'No'";
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdstring, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            dbcl.DisconnectDb();
            return count;
        }


        public Int32 GetPendingJOBApprovalCount(string inchargewrk)
        {
            dbcl.Sqlconnection();
            string cmdstring = "select count (Id) from tbl_jobs where JOB_InchargeWrk='" + inchargewrk + "' and JOB_Status='Out-Punch Done' and EntryExit='Exit'";
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdstring, dbcl.Conn);
            cmd.CommandType = CommandType.Text;
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

        public Int32 GetSOPCount(string JOBID)
        {
            dbcl.Sqlconnection();
            string cmdstring = "select count (Id) from tbl_soptraining where Ref_JOBID='" + JOBID + "'";
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