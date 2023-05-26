using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace WebApplication1.bussiness.production.rpts
{
    public partial class tbttalk_rpt : System.Web.UI.Page
    {
        DB_Utility_OH4Y dbcl = new DB_Utility_OH4Y();
        DataTable dt = new DataTable();

        static string imglink = "~\\images\\No_Image.jpg";
        static string imgfilename = "N/A";

        protected void Page_Load(object sender, EventArgs e)
        {
            string tbtid = Request.QueryString["TBTID"];
            //string tbtid = "TBT002983";

            BindTBTCheckLists();
            Bind_TBTIDDetails(tbtid);

        }

        protected void BindTBTCheckLists()
        {
            string cmdString = "select * from tlb_personalresponsibilities order by Id";
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            SqlCommand cmd = new SqlCommand(cmdString, dbcl.Conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            chkbxrspons.DataTextField = "PS_ItemText";
            chkbxrspons.DataValueField = "PS_ItemID";
            chkbxrspons.DataSource = dt;
            chkbxrspons.DataBind();
            dbcl.DisconnectDb();
        }

        private void Bind_TBTIDDetails(string tbtid)
        {
            try
            {
                string query = "select * from tbl_toolboxtalkdata where TBT_ID=@TBT_ID";
                SqlParameter[] pram = {
                                          new SqlParameter("@TBT_ID",tbtid),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    string Panel1status = dt.Rows[0]["Panel1_Status"].ToString();
                    string Panel2status = dt.Rows[0]["Panel2_Status"].ToString();
                    string Panel3status = dt.Rows[0]["TBTPhoto"].ToString();
                    string filename = dt.Rows[0]["TBT_PhotoFile"].ToString();

                    string jobid = dt.Rows[0]["Ref_JOBID"].ToString();
                    lbl_jobid.Text = jobid;
                    lbl_tbtid.Text = tbtid;

                    string tbtdate = dt.Rows[0]["TBT_Date"].ToString();
                    lbl_tbtdate.Text = DateBinder(tbtdate);
                    string jobdate = dt.Rows[0]["Ref_JOBDate"].ToString();
                    lbl_jobdate.Text = DateBinder(tbtdate);

                    lbl_tbtrgn.Text = dt.Rows[0]["TBT_Region"].ToString();
                    lbl_tbtdept.Text = dt.Rows[0]["TBT_Dept"].ToString();

                    lbl_jobrgn.Text = dt.Rows[0]["Ref_JOBRegion"].ToString();
                    lbl_jobdept.Text = "N/A";

                    lbl_tbtloc.Text = dt.Rows[0]["TBT_Location"].ToString();
                    //lbl_jobloc.Text ="N/A";

                    lbl_tbtsupvname.Text = dt.Rows[0]["TBT_SupvName"].ToString();
                    lbl_tbtsupvwrk.Text = dt.Rows[0]["TBT_SupvWrk"].ToString();

                    lbl_jobsupvname.Text = dt.Rows[0]["Ref_JOBSupvName"].ToString();
                    lbl_jobsupvwrk.Text = dt.Rows[0]["Ref_JOBSupvWrk"].ToString();

                    lbl_siteincharge.Text = dt.Rows[0]["AreaInchargeName"].ToString();
                    lbl_inchargewrk.Text = dt.Rows[0]["AreaInchargeWrk"].ToString();

                    lbl_linemngr.Text = dt.Rows[0]["LineManager"].ToString();

                    string sftysupvname = dt.Rows[0]["SafetySupvName"].ToString();
                    lbl_tbtsfysupvname.Text = sftysupvname;
                    lbl_sftysupvname.Text = sftysupvname;
                    lbl_tbtsfysupvwrk.Text = dt.Rows[0]["SafetySupvWrk"].ToString();
                    string sftysupvappdt = dt.Rows[0]["SafetySupvAppDate"].ToString();
                    lbl_sftysupvdt.Text = sftysupvappdt;
                    string sftysupvrmrks = dt.Rows[0]["SafetySupvAppRmrks"].ToString();
                    lbl_sftysupvrmrks.Text = sftysupvrmrks;
                    string sftysupvappstatus = dt.Rows[0]["SafetySupvApprovalStatus"].ToString();
                    lbl_sftysupvapp.Text = sftysupvappstatus;
                    lbl_sftysupvapp1.Text = sftysupvappstatus;

                    if (sftysupvappstatus == "Approved")
                    {
                        lbl_sftysupvapp.ForeColor = System.Drawing.Color.Green;
                        lbl_sftysupvapp1.ForeColor = System.Drawing.Color.Green;
                        app1.Visible = true;
                        pen1.Visible = false;
                    }
                    else
                    {
                        lbl_sftysupvapp.ForeColor = System.Drawing.Color.Red;
                        lbl_sftysupvapp1.ForeColor = System.Drawing.Color.Red;
                        app1.Visible = false;
                        pen1.Visible = true;
                    }
                    string sftyofcname = dt.Rows[0]["SafetyOfficerName"].ToString();
                    lbl_tbtsfyofname.Text = sftyofcname;
                    lbl_sftyofcrname.Text = sftyofcname;
                    lbl_tbtsfyofcwrk.Text = dt.Rows[0]["SafetyOfficerWrk"].ToString();
                    string sftyofcappdt = dt.Rows[0]["SO_ApprovalDate"].ToString();
                    lbl_sftyofcrdt.Text = sftyofcappdt;
                    string sftyofcrmrks = dt.Rows[0]["SO_Remarks"].ToString();
                    lbl_sftyofcrrmrks.Text = sftyofcrmrks;
                    string sftyofcrappstatus = dt.Rows[0]["SO_ApprovalStatus"].ToString();
                    lbl_sftyofcrapp.Text = sftyofcrappstatus;
                    lbl_sftyofcapp.Text = sftyofcrappstatus;
                    if (sftyofcrappstatus == "Approved")
                    {
                        lbl_sftyofcrapp.ForeColor = System.Drawing.Color.Green;
                        lbl_sftyofcapp.ForeColor = System.Drawing.Color.Green;
                        app2.Visible = true;
                        pen2.Visible = false;
                    }
                    else if (sftyofcrappstatus == "Pending")
                    {
                        lbl_sftyofcrapp.ForeColor = System.Drawing.Color.Red;
                        lbl_sftyofcapp.ForeColor = System.Drawing.Color.Red;
                        app2.Visible = false;
                        pen2.Visible = true;
                    }
                    else if (sftyofcrappstatus == "Rejected")
                    {
                        lbl_sftyofcrapp.ForeColor = System.Drawing.Color.Black;
                        lbl_sftyofcapp.ForeColor = System.Drawing.Color.Black;
                        app2.Visible = false;
                        pen2.Visible = true;
                    }


                    string tbtphoto = dt.Rows[0]["TBT_PhotoFile"].ToString();
                    TBT_Img.ImageUrl = "~//erp_images//TBTPhoto//" + tbtphoto;


                    string slno1 = dt.Rows[0]["PrevActionItem"].ToString();
                    if (slno1 == "Yes")
                    {
                        Image1.Visible = true;
                        Image2.Visible = false;

                        DataTable dt1 = new DataTable();
                        PrevActionableGrid.DataSource = dt1;
                        PrevActionableGrid.DataBind();
                    }
                    else
                    {
                        Image1.Visible = false;
                        Image2.Visible = true;
                        DataTable dt1 = new DataTable();
                        PrevActionableGrid.DataSource = dt1;
                        PrevActionableGrid.DataBind();
                    }

                    string slno2 = dt.Rows[0]["NewIncidentItem"].ToString();
                    if (slno2 == "Yes")
                    {
                        Image3.Visible = true;
                        Image4.Visible = false;

                        DataTable dt2 = new DataTable();
                        PastIncidentGrid.DataSource = dt2;
                        PastIncidentGrid.DataBind();
                    }
                    else
                    {
                        Image3.Visible = false;
                        Image4.Visible = true;

                        DataTable dt2 = new DataTable();
                        PastIncidentGrid.DataSource = dt2;
                        PastIncidentGrid.DataBind();
                    }

                    string slno3 = dt.Rows[0]["SafetyInterest"].ToString();
                    if (slno3 == "Yes")
                    {
                        Image5.Visible = true;
                        Image6.Visible = false;

                        lbl_sftyintrst.Text = dt.Rows[0]["SafetyInterestItems"].ToString();

                    }

                    string slno4 = dt.Rows[0]["SOPYesNo"].ToString();
                    if (slno4 == "Yes")
                    {
                        Image7.Visible = true;
                        Image8.Visible = false;
                        lbl_tbtsopno.Text = dt.Rows[0]["SOPNumber"].ToString();
                    }

                    string slno5 = dt.Rows[0]["EmpPrsnlResponsibilty"].ToString();
                    if (slno5 == "Yes")
                    {
                        Image9.Visible = true;
                        Image10.Visible = false;

                        //Checkbox for personal responsiblities
                        string chkditemsfromdb = dt.Rows[0]["EmpPersnlItems"].ToString();

                        string[] items = chkditemsfromdb.Split(',');
                        for (int i = 0; i <= items.GetUpperBound(0); i++)
                        {
                            ListItem currentcheckbox = chkbxrspons.Items.FindByText(items[i].ToString());
                            if (currentcheckbox != null)
                            {
                                currentcheckbox.Selected = true;
                            }
                        }
                    }

                    string slno6 = dt.Rows[0]["HazardMaterial"].ToString();
                    if (slno6 == "Yes")
                    {
                        Image11.Visible = true;
                        Image12.Visible = false;

                        lbl_hazards.Text = dt.Rows[0]["HazardMaterialItems"].ToString();
                    }

                    string slno7 = dt.Rows[0]["SafetyMessage"].ToString();
                    if (slno7 == "Yes")
                    {
                        Image13.Visible = true;
                        Image14.Visible = false;

                        lbl_sftmsg.Text = dt.Rows[0]["SafetyMessageItems"].ToString();
                    }

                    string slno8 = dt.Rows[0]["SafetyAlert"].ToString();
                    if (slno8 == "Yes")
                    {
                        Image15.Visible = true;
                        Image16.Visible = false;

                        lbl_sftyalert.Text = dt.Rows[0]["SafetyAlertItems"].ToString();
                    }

                    string slno9 = dt.Rows[0]["Ref_ActionItem"].ToString();
                    if (slno9 == "Yes")
                    {
                        Image17.Visible = true;
                        Image18.Visible = false;

                        DataTable dt3 = new DataTable();
                        NewActionableGrid.DataSource = dt3;
                        NewActionableGrid.DataBind();
                    }
                    else if (slno9 != "Yes")
                    {
                        Image17.Visible = false;
                        Image18.Visible = true;

                        DataTable dt3 = new DataTable();
                        NewActionableGrid.DataSource = dt3;
                        NewActionableGrid.DataBind();
                    }

                    Bind_JOBIDDetails(jobid);
                }
                else
                {
                    //If no data is found against the TBTID
                }
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private void Bind_JOBIDDetails(string jobid)
        {
            try
            {
                string query = "select * from tbl_jobs where JOBID=@JOBID";
                SqlParameter[] pram = {
                                          new SqlParameter("@JOBID",jobid),
                                      };
                dt = dbcl.SPreturn_dt(query, pram);
                if (dt.Rows.Count > 0)
                {
                    lbl_jobworksite.Text = dt.Rows[0]["JOB_Site"].ToString();

                    lbl_inchargewrk.Text = dt.Rows[0]["JOB_InchargeWrk"].ToString();
                    string incharge = dt.Rows[0]["JOB_InchargeName"].ToString();

                    lbl_jobdept.Text = dt.Rows[0]["JOB_Dept"].ToString();
                    string loc = dt.Rows[0]["JOB_Location"].ToString();

                    lbl_jobshift.Text = dt.Rows[0]["JOB_Shift"].ToString();
                    lbl_permitno.Text = dt.Rows[0]["JOB_PermitNo"].ToString();

                    lbl_jobtitle.Text = dt.Rows[0]["JOB_Title"].ToString();
                    //CheckforAttachedAttendnace();
                    Bind_Attendnace(jobid);
                }
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private void Bind_Attendnace(string jobid)
        {
            string ddljobid = lbl_jobid.Text.ToString();
            dbcl.Sqlconnection();
            dbcl.ConnectDb();
            string CmdString = "Select * from tbl_attendance where JOBID='" + jobid + "' order by Id";
            SqlCommand cmd = new SqlCommand(CmdString, dbcl.Conn);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                TBTMemebersGrid.DataSource = dr;
                TBTMemebersGrid.DataBind();
            }
            else
            {
                DataTable dt4 = new DataTable();
                TBTMemebersGrid.DataSource = dt4;
                TBTMemebersGrid.DataBind();
            }
            dbcl.Conn.Close();
        }
        private string DateBinder(string date)
        {
            string newdate = "";
            DateTime oDate = Convert.ToDateTime(date);
            string day = "";
            string month = "";
            if (oDate.Day < 10)
            {
                day = "0" + oDate.Day.ToString();
            }
            else
            {
                day = oDate.Day.ToString();
            }
            if (oDate.Month < 10)
            {
                month = "0" + oDate.Month.ToString();
            }
            else
            {
                month = oDate.Month.ToString();
            }
            return newdate = day + "-" + month + "-" + oDate.Year;
        }
    }
}