using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using System.Configuration;
using System.Drawing.Drawing2D;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.bussiness.production
{
    public partial class pyrl_deductions : System.Web.UI.Page
    {
        DB_Utility_OH4Y DbCL = new DB_Utility_OH4Y();
        Payroll_OH4Y PayRoll = new Payroll_OH4Y();

        public static decimal GorssBreaker = 20500;

        DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USERID"] == null || Session["RolePermissionDB"] == null || Session["UserRoleDB"] == null|| Session["USERNAME"] == null || Session["WORKMAN"] == null || Session["REGION"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
                else
                {
                    DDL_SearchType.Focus();
                }
            }
        }

        protected void DDL_SearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_SearchType.SelectedIndex != 0)
            {
                if (DDL_SearchType.SelectedIndex == 1)
                {
                    Nameinputrow.Visible = true;
                    WorkmanInput_Row.Visible = false;
                }
                else if (DDL_SearchType.SelectedIndex == 2)
                {
                    WorkmanInput_Row.Visible = true;
                    Nameinputrow.Visible = false;
                }
            }
            else
            {
                Nameinputrow.Visible = false;
                WorkmanInput_Row.Visible = false;

                DDL_SearchType.Focus();
                string title = "Notifications :";
                string body = "Please select search type";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            if (DDL_SearchType.SelectedIndex != 0)
            {
                if (DDL_SearchType.SelectedIndex == 1)
                {
                    if (txt_empname.Text.ToString() != "")
                    {
                        string query = "select * from tbl_Employee_Mustertable where WorkRegion = '" + Session["REGION"].ToString() + "' and FullName like '" + txt_empname.Text.ToString() + "%'";
                        GridBinder(query);

                        if (GridView1.Rows.Count == 1)
                        {
                            CurrentDataBinder();

                            string CmdString = "select WorkmanSL,FullName,Advance,Rem_Advance,Cur_Advance,Fines, Rem_Fines, Cur_Fines,Others, Rem_Others, Cur_Others from tbl_Employee_Mustertable where WorkmanSL='" + txt_empworkman.Text.ToString() + "'";
                            Deduction_GridBinder(CmdString);

                            deduction_panel.Visible = true;
                            deduction_vwbtns.Visible = true;

                            FineAmountLoader(txt_empworkman.Text.ToString());
                            AdvanceAmountLoader(txt_empworkman.Text.ToString());
                            OtherDeductionsLoader(txt_empworkman.Text.ToString());
                        }
                        else
                        {
                            deduction_vwbtns.Visible = true;
                            string title = "Opps :";
                            string body = "More then one items found, Kindly search for specific employee.";
                            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                        }
                    }
                    else
                    {
                        string title = "Notifications :";
                        string body = "Please enter Employee First Name";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                }
                else if (DDL_SearchType.SelectedIndex == 2)
                {
                    if (txt_empworkman.Text.ToString() != "")
                    {
                        string query = "select * from tbl_Employee_Mustertable where WorkRegion = '" + Session["REGION"].ToString() + "' and WorkmanSL = '" + txt_empworkman.Text.ToString() + "'";
                        GridBinder(query);

                        if (GridView1.Rows.Count == 1)
                        {
                            CurrentDataBinder();
                            string CmdString = "select WorkmanSL,FullName,Advance,Rem_Advance,Cur_Advance,Fines, Rem_Fines, Cur_Fines,Others, Rem_Others, Cur_Others from tbl_Employee_Mustertable where WorkmanSL='" + txt_empworkman.Text.ToString() + "'";
                            Deduction_GridBinder(CmdString);

                            deduction_panel.Visible = true;
                            deduction_vwbtns.Visible = true;

                            FineAmountLoader(txt_empworkman.Text.ToString());
                            AdvanceAmountLoader(txt_empworkman.Text.ToString());
                            OtherDeductionsLoader(txt_empworkman.Text.ToString());
                        }
                    }
                    else
                    {
                        string title = "Notifications :";
                        string body = "Please enter Employee Workman SL";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
                    }
                }
            }
            else
            {
                string title = "Notifications :";
                string body = "Please select search type..!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        private void GridBinder(string CmdString)
        {
            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            SqlCommand cmd = new SqlCommand(CmdString, DbCL.Conn);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            GridView1.DataSource = ds;
            GridView1.DataBind();
            DbCL.Conn.Close();
        }

        private void Deduction_GridBinder(string CmdString)
        {
            DbCL.Sqlconnection();
            DbCL.ConnectDb();
            SqlCommand cmd = new SqlCommand(CmdString, DbCL.Conn);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            DeductionGrid.DataSource = ds;
            DeductionGrid.DataBind();
            DbCL.Conn.Close();
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            Response.Redirect("pyrl_deductions.aspx");

        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("pyrl_managedashbrd.aspx");
        }


        //---------------------- The below codes addds new loans / deductions /fines for the first time ------------------//

        private void AdvanceAmountLoader(string emp_workmen)
        {
            //--------The below variables fetch the live db values --------------//
            int dbotp_total_advance = 0;
            int dbotp_remaining_advance = 0;
            int dbotp_current_advance = 0;

            try
            {
                string query = "select Advance,Rem_Advance,Cur_Advance from tbl_Employee_Mustertable where WorkmanSL='" + emp_workmen + "'";
                DbCL.Sqlconnection();
                DbCL.ConnectDb();
                SqlCommand cmd = new SqlCommand(query, DbCL.Conn);
                SqlDataReader Rdr;
                Rdr = cmd.ExecuteReader();
                if (Rdr.Read())
                {
                    dbotp_total_advance = Convert.ToInt32(Rdr["Advance"]);
                    txt_advanceamnt.Text = dbotp_total_advance.ToString().Trim();

                    dbotp_remaining_advance = Convert.ToInt32(Rdr["Rem_Advance"]);
                    txt_remamnt.Text = dbotp_remaining_advance.ToString().Trim();

                    dbotp_current_advance = Convert.ToInt32(Rdr["Cur_Advance"]);
                    txt_curradvamnt.Text = dbotp_current_advance.ToString().Trim();
                }
                DbCL.Conn.Close();

                btn_svadvance.Enabled = true;
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }
        private void AdvanceDeductions_Calculator(string emp_workmen)
        {
            //-------------The below variables fetch the inputted values from the HR --------------//
            int inputed_total_advance = Convert.ToInt32(txt_advanceamnt.Text.ToString());
            int inputed_current_advance = Convert.ToInt32(txt_curradvamnt.Text.ToString());

            //--------The below variables values will be saved into the db --------------//
            int dbinp_total_advance = 0;
            int dbinp_remaining_advance = 0;
            int dbinp_current_advance = 0;

            //--------The below variables fetch the live db values --------------//
            int dbotp_total_advance = 0;
            int dbotp_remaining_advance = 0;
            int dbotp_current_advance = 0;

            try
            {
                string query = "select Advance,Rem_Advance,Cur_Advance from tbl_Employee_Mustertable where WorkmanSL='" + emp_workmen + "'";
                DbCL.Sqlconnection();
                DbCL.ConnectDb();
                SqlCommand cmd = new SqlCommand(query, DbCL.Conn);
                SqlDataReader Rdr;
                Rdr = cmd.ExecuteReader();
                if (Rdr.Read())
                {
                    dbotp_total_advance = Convert.ToInt32(Rdr["Advance"]);
                    txt_advanceamnt.Text = dbotp_total_advance.ToString().Trim();

                    dbotp_remaining_advance = Convert.ToInt32(Rdr["Rem_Advance"]);
                    txt_remamnt.Text = dbotp_remaining_advance.ToString().Trim();

                    dbotp_current_advance = Convert.ToInt32(Rdr["Cur_Advance"]);
                    txt_curradvamnt.Text = dbotp_current_advance.ToString().Trim();

                    //if (dbotp_current_advance != 0)
                    //{
                    //    inputed_current_advance = dbotp_current_advance;
                    //}
                    dbinp_total_advance = dbotp_total_advance;
                }
                DbCL.Conn.Close();
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }



            // ------------------------ LOGIC ----------------------------------------//
            if (dbotp_total_advance == 0 && dbotp_remaining_advance == 0 && dbotp_current_advance == 0)
            {
                dbinp_total_advance = inputed_total_advance;
                dbinp_remaining_advance = inputed_total_advance;
                dbinp_current_advance = inputed_current_advance;
            }
            else if (dbotp_remaining_advance == 0 && inputed_current_advance > 0)
            {
                dbinp_remaining_advance = 0;
                dbinp_current_advance = inputed_current_advance - inputed_current_advance;
            }
            else if (dbotp_remaining_advance == 0)
            {
                dbinp_remaining_advance = inputed_total_advance - inputed_current_advance;
                dbinp_current_advance = inputed_current_advance;
            }
            else if (dbotp_remaining_advance > 0)
            {
                dbinp_remaining_advance = dbotp_remaining_advance - inputed_current_advance;
                dbinp_current_advance = inputed_current_advance;

                if (dbinp_remaining_advance < 0)
                {
                    dbinp_remaining_advance = 0;
                    dbinp_current_advance = 0;
                }

            }

            txt_advanceamnt.Text = dbotp_total_advance.ToString().Trim();
            txt_remamnt.Text = dbotp_remaining_advance.ToString().Trim();
            txt_curradvamnt.Text = dbotp_current_advance.ToString().Trim();
            string wrkman = txt_empworkman.Text.ToString();
            try
            {
                DbCL.Sqlconnection();
                DbCL.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = DbCL.Conn;
                string CmdString = "UPDATE tbl_Employee_Mustertable set Advance=@Advance, Rem_Advance=@Rem_Advance, Cur_Advance=@Cur_Advance where WorkmanSL=@WorkmanSL";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Advance", dbinp_total_advance);
                cmd.Parameters.AddWithValue("@Rem_Advance", dbinp_remaining_advance);
                cmd.Parameters.AddWithValue("@Cur_Advance", dbinp_current_advance);
                cmd.Parameters.AddWithValue("@WorkmanSL", wrkman);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                string CmdString1 = "select WorkmanSL,FullName,Advance,Rem_Advance,Cur_Advance,Fines, Rem_Fines, Cur_Fines,Others, Rem_Others, Cur_Others from tbl_Employee_Mustertable where WorkmanSL='" + wrkman + "'";
                Deduction_GridBinder(CmdString1);

                string title = "Notifications :";
                string body = "Deductions added to Adance Section of Rs." + dbinp_total_advance.ToString() + ", Remaining Amount of Rs." + dbinp_remaining_advance.ToString() + ", Cuurent Deduction of Rs." + dbinp_current_advance.ToString() + "";

                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }
        private void FineAmountLoader(string emp_workmen)
        {
            //--------The below variables fetch the live db values --------------//
            int dbotp_total_fines = 0;
            int dbotp_remaining_fines = 0;
            int dbotp_current_fines = 0;

            try
            {
                string query = "select Fines, Rem_Fines, Cur_Fines from tbl_Employee_Mustertable where WorkmanSL='" + emp_workmen + "'";
                DbCL.Sqlconnection();
                DbCL.ConnectDb();
                SqlCommand cmd = new SqlCommand(query, DbCL.Conn);
                SqlDataReader Rdr;
                Rdr = cmd.ExecuteReader();
                if (Rdr.Read())
                {
                    dbotp_total_fines = Convert.ToInt32(Rdr["Fines"]);
                    txt_totalfine.Text = dbotp_total_fines.ToString();

                    dbotp_remaining_fines = Convert.ToInt32(Rdr["Rem_Fines"]);
                    txt_remfine.Text = dbotp_remaining_fines.ToString();

                    dbotp_current_fines = Convert.ToInt32(Rdr["Cur_Fines"]);
                    txt_currfine.Text = dbotp_current_fines.ToString();
                }
                DbCL.Conn.Close();
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }
        private void FinesDeductions_Calculator(string emp_workmen)
        {
            //-------------The below variables fetch the inputted values from the HR --------------//
            int inputed_total_fines = Convert.ToInt32(txt_totalfine.Text.ToString());
            int inputed_current_fines = Convert.ToInt32(txt_currfine.Text.ToString());

            //--------The below variables values will be saved into the db --------------//
            int dbinp_total_fines = 0;
            int dbinp_remaining_fines = 0;
            int dbinp_current_fines = 0;

            //--------The below variables fetch the live db values --------------//
            int dbotp_total_fines = 0;
            int dbotp_remaining_fines = 0;
            int dbotp_current_fines = 0;


            // ------------------------ LOGIC ----------------------------------------//
            if (dbotp_total_fines == 0 && dbotp_remaining_fines == 0 && dbotp_current_fines == 0)
            {
                dbinp_total_fines = inputed_total_fines;
                dbinp_remaining_fines = inputed_total_fines;
                dbinp_current_fines = inputed_current_fines;
            }
            else if (dbotp_remaining_fines == 0 && inputed_current_fines > 0)
            {
                dbinp_remaining_fines = 0;
                dbinp_current_fines = inputed_current_fines - inputed_current_fines;
            }
            else if (dbotp_remaining_fines == 0)
            {
                dbinp_remaining_fines = inputed_total_fines - inputed_current_fines;
                dbinp_current_fines = inputed_current_fines;
            }
            else if (dbotp_remaining_fines > 0)
            {
                dbinp_remaining_fines = dbotp_remaining_fines - inputed_current_fines;
                dbinp_current_fines = inputed_current_fines;

                if (dbinp_remaining_fines < 0)
                {
                    dbinp_remaining_fines = 0;
                    dbinp_current_fines = 0;
                }
            }

            txt_totalfine.Text = dbotp_total_fines.ToString();
            txt_remfine.Text = dbotp_remaining_fines.ToString();
            txt_currfine.Text = dbotp_current_fines.ToString();

            try
            {
                DbCL.Sqlconnection();
                DbCL.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = DbCL.Conn;
                string CmdString = "UPDATE tbl_Employee_Mustertable set Fines=@Fines, Rem_Fines=@Rem_Fines, Cur_Fines=@Cur_Fines where WorkmanSL=@WorkmanSL";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Fines", dbinp_total_fines);
                cmd.Parameters.AddWithValue("@Rem_Fines", dbinp_remaining_fines);
                cmd.Parameters.AddWithValue("@Cur_Fines", dbinp_current_fines);
                cmd.Parameters.AddWithValue("@WorkmanSL", txt_empworkman.Text.ToString());
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                string CmdString1 = "select WorkmanSL,FullName,Advance,Rem_Advance,Cur_Advance,Fines, Rem_Fines, Cur_Fines,Others, Rem_Others, Cur_Others from tbl_Employee_Mustertable where WorkmanSL='" + txt_empworkman.Text.ToString() + "'";
                Deduction_GridBinder(CmdString1);

                string title = "Notifications :";
                string body = "Deductions added to Fines Section of Rs." + dbinp_total_fines.ToString() + " Remaining Amount of Rs." + dbinp_remaining_fines.ToString() + " Cuurent Deduction of Rs." + dbinp_current_fines.ToString() + "";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }
        private void OtherDeductionsLoader(string emp_workmen)
        {
            //--------The below variables fetch the live db values --------------//
            int dbotp_total_others = 0;
            int dbotp_remaining_others = 0;
            int dbotp_current_others = 0;

            try
            {
                string query = "select Others, Rem_Others, Cur_Others from tbl_Employee_Mustertable where WorkmanSL='" + emp_workmen + "'";
                DbCL.Sqlconnection();
                DbCL.ConnectDb();
                SqlCommand cmd = new SqlCommand(query, DbCL.Conn);
                SqlDataReader Rdr;
                Rdr = cmd.ExecuteReader();
                if (Rdr.Read())
                {
                    dbotp_total_others = Convert.ToInt32(Rdr["Others"]);
                    dbotp_remaining_others = Convert.ToInt32(Rdr["Rem_Others"]);
                    dbotp_current_others = Convert.ToInt32(Rdr["Cur_Others"]);
                }
                DbCL.Conn.Close();
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }
        private void OthersDeductions_Calculator(string emp_workmen)
        {
            //-------------The below variables fetch the inputted values from the HR --------------//
            int inputed_total_others = Convert.ToInt32(txt_ttlothers.Text.ToString()); ;
            int inputed_current_others = Convert.ToInt32(txt_ttlcurrothers.Text.ToString()); ;

            //--------The below variables values will be saved into the db --------------//
            int dbinp_total_others = 0;
            int dbinp_remaining_others = 0;
            int dbinp_current_others = 0;

            //--------The below variables fetch the live db values --------------//
            int dbotp_total_others = 0;
            int dbotp_remaining_others = 0;
            int dbotp_current_others = 0;

            try
            {
                string query = "select Others, Rem_Others, Cur_Others from tbl_Employee_Mustertable where WorkmanSL='" + emp_workmen + "'";
                DbCL.Sqlconnection();
                DbCL.ConnectDb();
                SqlCommand cmd = new SqlCommand(query, DbCL.Conn);
                SqlDataReader Rdr;
                Rdr = cmd.ExecuteReader();
                if (Rdr.Read())
                {
                    dbotp_total_others = Convert.ToInt32(Rdr["Others"]);
                    txt_ttlothers.Text = dbotp_total_others.ToString();

                    dbotp_remaining_others = Convert.ToInt32(Rdr["Rem_Others"]);
                    txt_ttlothersrem.Text = dbotp_remaining_others.ToString();

                    dbotp_current_others = Convert.ToInt32(Rdr["Cur_Others"]);
                    txt_ttlothers.Text = dbotp_current_others.ToString();

                    if (dbotp_current_others != 0)
                    {
                        inputed_current_others = dbotp_current_others;
                    }
                    dbinp_total_others = dbotp_total_others;
                }
                DbCL.Conn.Close();
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }

            // ------------------------ LOGIC ----------------------------------------//
            if (dbotp_total_others == 0 && dbotp_remaining_others == 0 && dbotp_current_others == 0)
            {
                dbinp_total_others = inputed_total_others;
                dbinp_remaining_others = inputed_total_others;
                dbinp_current_others = inputed_current_others;
            }
            else if (dbotp_remaining_others == 0 && inputed_current_others > 0)
            {
                dbinp_remaining_others = 0;
                dbinp_current_others = inputed_current_others - inputed_current_others;
            }
            else if (dbotp_remaining_others == 0)
            {
                dbinp_remaining_others = inputed_total_others - inputed_current_others;
                dbinp_current_others = inputed_current_others;
            }
            else if (dbotp_remaining_others > 0)
            {
                dbinp_remaining_others = dbotp_remaining_others - inputed_current_others;
                dbinp_current_others = inputed_current_others;

                if (dbinp_remaining_others < 0)
                {
                    dbinp_remaining_others = 0;
                    dbinp_current_others = 0;
                }

            }

            txt_ttlothers.Text = dbotp_total_others.ToString();
            txt_ttlothersrem.Text = dbotp_remaining_others.ToString();
            txt_ttlothers.Text = dbotp_current_others.ToString();


            try
            {
                DbCL.Sqlconnection();
                DbCL.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = DbCL.Conn;
                string CmdString = "UPDATE tbl_Employee_Mustertable set Others=@Others, Rem_Others=@Rem_Others, Cur_Others=@Cur_Others where WorkmanSL=@WorkmanSL";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Others", dbinp_total_others);
                cmd.Parameters.AddWithValue("@Rem_Others", dbinp_remaining_others);
                cmd.Parameters.AddWithValue("@Cur_Others", dbinp_current_others);
                cmd.Parameters.AddWithValue("@WorkmanSL", txt_empworkman.Text.ToString());
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                string CmdString1 = "select WorkmanSL,FullName,Advance,Rem_Advance,Cur_Advance,Fines, Rem_Fines, Cur_Fines,Others, Rem_Others, Cur_Others from tbl_Employee_Mustertable where WorkmanSL='" + txt_empworkman.Text.ToString() + "'";
                Deduction_GridBinder(CmdString1);

                string title = "Notifications :";
                string body = "Deductions added to Other Section of Rs." + dbinp_total_others.ToString() + " Remaining Amount of Rs." + dbinp_remaining_others.ToString() + " Cuurent Deduction of Rs." + dbinp_current_others.ToString() + "";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void btn_yes_Click(object sender, EventArgs e)
        {
            Clear_Deductions();
        }

        private void Clear_Deductions()
        {
            try
            {
                DbCL.Sqlconnection();
                DbCL.ConnectDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = DbCL.Conn;
                string CmdString = "UPDATE tbl_Employee_Mustertable set Advance=@Advance, Rem_Advance=@Rem_Advance, Cur_Advance=@Cur_Advance, Rem_Fines=@Rem_Fines, Cur_Fines=@Cur_Fines,Others=@Others, Rem_Others=@Rem_Others, Cur_Others=@Cur_Others,Fines=@Fines where WorkmanSL=@WorkmanSL";
                cmd.CommandText = CmdString;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Advance", 0);
                cmd.Parameters.AddWithValue("@Rem_Advance", 0);
                cmd.Parameters.AddWithValue("@Cur_Advance", 0);
                cmd.Parameters.AddWithValue("@Fines", 0);
                cmd.Parameters.AddWithValue("@Rem_Fines", 0);
                cmd.Parameters.AddWithValue("@Cur_Fines", 0);
                cmd.Parameters.AddWithValue("@Others", 0);
                cmd.Parameters.AddWithValue("@Rem_Others", 0);
                cmd.Parameters.AddWithValue("@Cur_Others", 0);
                cmd.Parameters.AddWithValue("@WorkmanSL", txt_empworkman.Text.ToString());
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                string title = "Notifications :";
                string body = "All deductions has been cleared";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);

                FineAmountLoader(txt_empworkman.Text.ToString());
                AdvanceAmountLoader(txt_empworkman.Text.ToString());
                OtherDeductionsLoader(txt_empworkman.Text.ToString());

                CurrentDataBinder();

                string CmdString1 = "select WorkmanSL,FullName, Advance,Rem_Advance,Cur_Advance,Fines, Rem_Fines, Cur_Fines,Others, Rem_Others, Cur_Others from tbl_Employee_Mustertable where WorkmanSL='" + txt_empworkman.Text.ToString() + "'";
                Deduction_GridBinder(CmdString1);
            }
            catch (Exception ex)
            {
                string title = "Notifications :";
                string body = "Error : " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
            }
        }

        protected void btn_svadvance_Click(object sender, EventArgs e)
        {
            AdvanceDeductions_Calculator(txt_empworkman.Text.ToString());
            CurrentDataBinder();
            AdvanceAmountLoader(txt_empworkman.Text.ToString());
        }

        protected void btn_svfines_Click(object sender, EventArgs e)
        {
            FinesDeductions_Calculator(txt_empworkman.Text.ToString());
            CurrentDataBinder();
            FineAmountLoader(txt_empworkman.Text.ToString());
        }

        protected void btn_svothers_Click(object sender, EventArgs e)
        {
            OthersDeductions_Calculator(txt_empworkman.Text.ToString());
            CurrentDataBinder();
            OtherDeductionsLoader(txt_empworkman.Text.ToString());
        }


        //--------------------------- Attendnace & Payment --------------------------//

        private void CurrentDataBinder()
        {
            string Year = DateTime.Now.Year.ToString();
            string Month = DateTime.Now.AddMonths(-1).ToString("MM");
            string MonthName = DateTime.Now.AddMonths(-1).ToString("MMMM");

            finalized.Visible = false;
            realtime.Visible = true;
            PaymentDataBinder(Year, Month, MonthName);
        }

        private void PaymentDataBinder(string Year, string Month, string Monthname)
        {
            realtime.Visible = true;
            finalized.Visible = false;

            Int32 int_month = Convert.ToInt32(Month);
            lbl_calmonth.Text = lbl_paymonth.Text = Monthname;

            Int32 int_year = Convert.ToInt32(Year);
            lbl_calyear.Text = lbl_payyear.Text = int_year.ToString();

            string empwrk = txt_empworkman.Text.ToString();
            string empskill = "";
            string empregion = "";
            PayRoll.FindEmployeeSkillType(empwrk, ref empskill, ref empregion);

            //----------------- Function call to find out the Daily Pay Rate aganist the Employee Skill Category----------------//
            decimal dailyrate = 0.0m;
            PayRoll.FindPayCadre(empskill, empregion, ref dailyrate);

            //----------------- Function call to find out the Calender Working Days --------------------------------------------//
            Int32 calwrkdays = 0;
            PayRoll.FindCalWorkDays(Year, Month, empregion, ref calwrkdays);


            //----------------- Function call to find out Employee Payroll Factors in BULK ------------------//
            string FixedSalary_YesNo = "";
            decimal FixedAmount = .0m;
            Int32 WorkHours = 0;
            Int32 OTFactor = 0;
            string OTMultiplier = "";
            decimal DA_VDA = .0m;
            decimal HRA = .0m;
            decimal Conv_Allowance = .0m;
            decimal Medical_Allowance = .0m;
            decimal Washing_Allowance = .0m;
            decimal ATT_Allowance = .0m;
            decimal SPCL_Allowance = .0m;
            decimal Misc_Earnings = .0m;
            Int32 OT_Divisibility = 0;
            int Advance = 0;
            int Fines = 0;
            int Others = 0;

            PayRoll.EmployeePayrollFactors(empwrk, ref FixedSalary_YesNo, ref FixedAmount, ref WorkHours, ref OTFactor, ref OTMultiplier, ref DA_VDA, ref HRA, ref Conv_Allowance, ref Medical_Allowance, ref Washing_Allowance, ref ATT_Allowance, ref SPCL_Allowance, ref Misc_Earnings, ref OT_Divisibility, ref Advance, ref Fines, ref Others);

            Int32 caldays = DateTime.DaysInMonth(int_year, int_month);
            lbl_caldays.Text = caldays.ToString();

            decimal ttl_days = 0;
            PayRoll.FindEmployeeTotalDaysByMonth(Month, Year, empwrk, ref ttl_days);
            lbl_totalpresent.Text = ttl_days.ToString();
            lbl_dayswrkd.Text = ttl_days.ToString();

            Int32 ttl_p = 0;
            PayRoll.FindEmployeeTotalPresentByMonth(Month, Year, empwrk, ref ttl_p);
            lbl_presentdayscount.Text = ttl_p.ToString();

            Int32 ttl_od = 0;
            PayRoll.FindEmployeeTotalODByMonth(Month, Year, empwrk, ref ttl_od);
            lbl_oddayscount.Text = ttl_od.ToString();

            Int32 ttl_nh = 0;
            PayRoll.FindEmployeeTotalNHByMonth(Month, Year, empwrk, ref ttl_nh);
            lbl_nhcount.Text = ttl_nh.ToString();

            Int32 ttl_fl = 0;
            PayRoll.FindEmployeeTotalFLByMonth(Month, Year, empwrk, ref ttl_fl);
            lbl_flcount.Text = ttl_fl.ToString();

            decimal ttl_ot = .0m;
            PayRoll.FindEmployeeTotalOTByMonth(Month, Year, empwrk, ref ttl_ot);
            lbl_totalot.Text = ttl_ot.ToString();

            //Here goes the code for real time salary calculations
            //this will shows real time but not ACTUAL DATA

            //-------------Basic Salary or Basic Wages  -----------   Daily PayRate x Present Days
            decimal BasicSalary = 0.0m;
            PayRoll.BasicSalaryCalculation(ttl_p, dailyrate, ref BasicSalary);


            decimal fxdrt = .0m;
            decimal fnlfdr = .0m;
            decimal FixRateSalary = .0m;
            if (FixedSalary_YesNo == "Yes")
            {
                //----------------- Calculation for Employee who are in Fixed Salary----------------//
                //fxdrt = Math.Ceiling(Math.Round(FixedAmount / calwrkdays, 2));
                fxdrt = FixedAmount / calwrkdays;
                fnlfdr = Math.Round(fxdrt, 2);

                //------------ Wage of Fixed rate ---------------   ( FixedAmount / CalenderDays ) x  PresentDays
                FixRateSalary = Math.Round(fnlfdr * ttl_p, 0);
            }

            decimal DaVdaPay = .0m;
            decimal HRAPay = .0m;
            decimal ConvPay = .0m;
            decimal MedPay = .0m;
            decimal WashPay = .0m;
            decimal AttPay = .0m;
            decimal SPCLPay = .0m;
            decimal MiscPay = .0m;

            //---------------- DA/VDA Alowances Cal-------------------------------------//
            PayRoll.EmployeeOthersPayCalculations1(ttl_p, calwrkdays, DA_VDA, ref DaVdaPay);
            lbl_davdapay.Text = DaVdaPay.ToString();

            //---------------- HRA Alowances Cal-------------------------------------//
            PayRoll.EmployeeOthersPayCalculations2(ttl_p, calwrkdays, HRA, ref HRAPay);
            lbl_hrapay.Text = HRAPay.ToString();

            //---------------- Conv Alowances Cal-------------------------------------//
            PayRoll.EmployeeOthersPayCalculations3(ttl_p, calwrkdays, Conv_Allowance, ref ConvPay);
            lbl_davdapay.Text = ConvPay.ToString();

            //---------------- Medical Alowances Cal-------------------------------------//
            PayRoll.EmployeeOthersPayCalculations4(ttl_p, calwrkdays, Medical_Allowance, ref MedPay);
            lbl_medpay.Text = MedPay.ToString();

            //---------------- Wash Alowances Cal-------------------------------------//
            PayRoll.EmployeeOthersPayCalculations5(ttl_p, calwrkdays, Washing_Allowance, ref WashPay);
            lbl_washpay.Text = WashPay.ToString();

            //---------------- Att Alowances Cal-------------------------------------//
            PayRoll.EmployeeOthersPayCalculations6(ttl_p, calwrkdays, ATT_Allowance, ref AttPay);
            lbl_attpay.Text = AttPay.ToString();

            //---------------- SPCL Alowances Cal-------------------------------------//
            PayRoll.EmployeeOthersPayCalculations7(ttl_p, calwrkdays, SPCL_Allowance, ref SPCLPay);
            lbl_spclway.Text = SPCLPay.ToString();

            //---------------- MISC Alowances Cal-------------------------------------//
            PayRoll.EmployeeOthersPayCalculations8(ttl_p, calwrkdays, Misc_Earnings, ref MiscPay);
            lbl_miscpay.Text = MiscPay.ToString();

            //--------------- PF Calucations --------------------//
            decimal PFPay = 0.0m;
            PayRoll.PFPayCalculation(BasicSalary, ref PFPay);

            //-------------------Total Allowances --------------------//
            decimal ttl_allow = DaVdaPay + HRAPay + ConvPay + MedPay + WashPay + AttPay + SPCLPay + MiscPay;



            //---------------- OT Pay -------- Gross Rate
            decimal otpay = 0.0m;
            decimal actualgross = 0.0m;

            if (FixedSalary_YesNo == "Yes")  ///Check whether the employee is in Fixed or Daily Rate Payroll
            {
                if (OTMultiplier == "Gross")
                {
                    if (ttl_ot >= 1)
                    {
                        decimal otdays = ttl_ot / OT_Divisibility;
                        // OTPay on FixedRate
                        otpay = Math.Round(fnlfdr * otdays * OTFactor, 0);
                    }
                    else
                    {
                    }

                    //actualgross = FixRateSalary + otpay + DaVdaPay + HRAPay + ConvPay + MedPay + AttPay + SPCLPay + MiscPay;
                    //actualgross = FixRateSalary + otpay + DaVdaPay + HRAPay + MedPay + AttPay + SPCLPay + MiscPay; ---------- Commented on 01.05.2022
                    actualgross = FixRateSalary + otpay + DaVdaPay + MedPay + AttPay + SPCLPay + MiscPay;
                }
                else
                {
                    if (ttl_ot >= 1)
                    {
                        decimal otdays = ttl_ot / OT_Divisibility;
                        //OTPay on DailyRate
                        otpay = Math.Round(dailyrate * otdays * OTFactor, 0);
                    }
                    else
                    {

                    }
                    //actualgross = FixRateSalary + otpay + DaVdaPay + HRAPay + ConvPay + MedPay + AttPay + SPCLPay + MiscPay;
                    //actualgross = FixRateSalary + otpay + DaVdaPay + HRAPay + MedPay + AttPay + SPCLPay + MiscPay;
                    actualgross = FixRateSalary + otpay + DaVdaPay + MedPay + AttPay + SPCLPay + MiscPay;
                }
            }
            else
            {
                if (OTMultiplier == "Gross")
                {
                    if (ttl_ot >= 1)
                    {
                        decimal otdays = ttl_ot / OT_Divisibility;
                        // OTPay on FixedRate
                        otpay = Math.Round(fnlfdr * otdays * OTFactor, 0);
                    }
                    else
                    {

                    }

                    //actualgross = FixRateSalary + otpay + DaVdaPay + HRAPay + ConvPay + MedPay + AttPay + SPCLPay + MiscPay;
                    //actualgross = FixRateSalary + otpay + DaVdaPay + HRAPay + MedPay + AttPay + SPCLPay + MiscPay;
                    actualgross = FixRateSalary + otpay + DaVdaPay + MedPay + AttPay + SPCLPay + MiscPay;
                }
                else
                {
                    if (ttl_ot >= 1)
                    {
                        decimal otdays = ttl_ot / OT_Divisibility;
                        //OTPay on DailyRate
                        otpay = Math.Round(dailyrate * otdays * OTFactor, 0);
                    }
                    else
                    {

                    }
                    //actualgross = BasicSalary + otpay + DaVdaPay + HRAPay + ConvPay + MedPay + AttPay + SPCLPay + MiscPay;
                    //actualgross = BasicSalary + otpay + DaVdaPay + HRAPay + MedPay + AttPay + SPCLPay + MiscPay;
                    actualgross = BasicSalary + otpay + DaVdaPay + MedPay + AttPay + SPCLPay + MiscPay;
                }
            }

            //----------------Gross Calculation & NET Payment 2 -------------------------------//
            decimal grossesic = 0.0m;

            decimal washgross = actualgross + WashPay + ConvPay;

            if (washgross > GorssBreaker)
            {
                decimal minus = WashPay + ConvPay;
                //grossesic = GorssBreaker-WashPay;
                grossesic = GorssBreaker - minus;
            }
            else
            {
                //grossesic = washgross - WashPay;
                decimal minus = WashPay + ConvPay;
                grossesic = washgross - minus;
            }
            decimal otherpay = grossesic - BasicSalary;

            //--------------------- ESIC pay---------------------------------------------------//

            decimal esicpay = 0.0m;
            esicpay = Math.Round(grossesic * 0.0075m, 0);


            //--------------------- NET Payment -------------------------------------------------//
            decimal netpay1 = 0.0m;
            decimal newgross = grossesic;
            netpay1 = Math.Round(newgross - PFPay - esicpay + WashPay + ConvPay, 0);

            decimal ttldeductions = Advance + Fines + Others;

            //decimal netpayfinal = netpay1 - ttldeductions;          -- Comented on 04-08-2022
            //The above is the NetPay 1 Final Payment

            decimal netpay1final = .0m;
            decimal netpay2 = 0.0m;
            if (FixedSalary_YesNo == "Yes")
            {
                netpay2 = actualgross - newgross + HRAPay;
                //netpay2 = newgross-actualgross;
            }
            else
            {
                decimal p = netpay1;
                netpay2 = actualgross - p - PFPay - esicpay + HRAPay;
            }

            //to deduct max amount from Pay2, if it is greater then or equal to the total deduction amount

            decimal check = netpay2 - ttldeductions;
            decimal netpay2_finalaftrded = .0m;
            if (empregion == "KPO")
            {
                if (check > 0)
                {
                    netpay2_finalaftrded = netpay2 - ttldeductions;
                    netpay1final = netpay1;
                }
                else
                {
                    netpay2_finalaftrded = netpay2;
                    netpay1final = netpay1 - ttldeductions;
                }
            }
            else if (empregion == "AGL")
            {
                netpay2_finalaftrded = netpay2;
                netpay1final = netpay1 - ttldeductions;
            }


            lbl_basic.Text = BasicSalary.ToString();
            lbl_otpay.Text = otpay.ToString();

            lbl_fxdsalry.Text = FixRateSalary.ToString();

            decimal disp_actual_gross = actualgross;
            lbl_actualgross.Text = disp_actual_gross.ToString();

            lbl_esicgros.Text = grossesic.ToString();


            lbl_netpayb4.Text = netpay1.ToString();
            lbl_ttlactlgros.Text = actualgross.ToString();
            lbl_esic4gross.Text = grossesic.ToString();

            decimal pay2b4 = actualgross - grossesic;
            lbl_netpay2b4.Text = pay2b4.ToString();

            decimal earning = BasicSalary + otpay;
            decimal ttlded = PFPay + esicpay + Advance + Fines + Others;
            lbl_ttldeductions.Text = ttlded.ToString();

            lbl_pfpay.Text = PFPay.ToString();
            lbl_esicpay.Text = esicpay.ToString();

            otherpay = grossesic - earning - ttlded;
            //lbl_allowances.Text = otherpay.ToString();
            decimal cal = grossesic - BasicSalary;
            lbl_otherpay.Text = cal.ToString();

            lbl_advance.Text = ttldeductions.ToString();

            lbl_netpayfnl.Text = netpay1final.ToString();
            lbl_netpay2fnl.Text = netpay2_finalaftrded.ToString();
        }

    }
}