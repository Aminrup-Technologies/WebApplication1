<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="deta_mustering.aspx.cs" Inherits="WebApplication1.bussiness.production.deta_mustering" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="row">

            <div class="col-md-6" id="pnl_basicdata" runat="server" visible="true">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Organizational Master Data </h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <a class="btn btn-app" href="work_country.aspx" id="box_WorkCountry" runat="server" visible="true">
                            <div id="WorkCountry" class="badge bg-green" runat="server">
                                <asp:Label ID="Label17" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Work Country
                        </a>
                        <a class="btn btn-app" href="work_states.aspx" id="box_WorkState" runat="server" visible="true">
                            <div id="WorkState" class="badge bg-green" runat="server">
                                <asp:Label ID="lbl_factorsstatus" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Work States

                        </a>
                        <a class="btn btn-app" href="work_region.aspx" id="box_WorkRegion" runat="server" visible="true">
                            <div id="WorkRegion" class="badge bg-green" runat="server">
                                <asp:Label ID="lbl_bankdatastatus" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>Work Regions
                        </a>
                    </div>
                </div>
            </div>

            <div class="col-md-6" id="pnl_company" runat="server" visible="true">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Company Master Data </h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <a class="btn btn-app" href="work_company.aspx" id="box_WorkCompany" runat="server" visible="true">
                            <div id="WorkCompany" class="badge bg-green" runat="server">
                                <asp:Label ID="Label1" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Work Company
                        </a>
                        <a class="btn btn-app" href="workcompany_dept.aspx" id="box_CompanyDept" runat="server" visible="true">
                            <div id="CompanyDept" class="badge bg-green" runat="server">
                                <asp:Label ID="Label2" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Departments

                        </a>
                        <a class="btn btn-app" href="workcomp_deptheads.aspx" id="box_DeptHeads" runat="server" visible="true">
                            <div id="DeptHeads" class="badge bg-green" runat="server">
                                <asp:Label ID="Label3" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>Dept. Heads
                        </a>
                        <a class="btn btn-app" href="workcomp_dept_locations.aspx" id="box_DeptLocations" runat="server" visible="true">
                            <div id="DeptLocations" class="badge bg-green" runat="server">
                                <asp:Label ID="Label4" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>Locations
                        </a>
                    </div>
                </div>
            </div>

            <div class="col-md-6" id="pnl_po" runat="server" visible="true">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Work-Order / P.O. Master Data </h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <a class="btn btn-app" href="add_workorders.aspx" id="box_AddWorkorder" runat="server" visible="true">
                            <div id="AddWorkorder" class="badge bg-green" runat="server">
                                <asp:Label ID="Label5" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Add PO
                        </a>
                        <a class="btn btn-app" href="add_wo_skillcategory.aspx" id="box_AddWO_SkillCategory" runat="server" visible="true">
                            <div id="AddWO_SkillCategory" class="badge bg-green" runat="server">
                                <asp:Label ID="Label6" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>PO Category

                        </a>
                        <a class="btn btn-app" href="add_workorderdata.aspx" id="box_AddItemNo" runat="server" visible="true">
                            <div id="AddItemNo" class="badge bg-green" runat="server">
                                <asp:Label ID="Label7" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>PO Items
                        </a>
                        <a class="btn btn-app" href="add_workorder_lineitems.aspx" id="box_AddLineItems" runat="server" visible="true">
                            <div id="AddLineItems" class="badge bg-green" runat="server">
                                <asp:Label ID="Label8" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>PO LineItem
                        </a>
                        <a class="btn btn-app" href="upload_wrkordrlineitems.aspx" id="box_UploadLineItems" runat="server" visible="true">
                            <div id="UploadLineItems" class="badge bg-green" runat="server">
                                <asp:Label ID="Label9" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>Upload LineItem
                        </a>
                    </div>
                </div>
            </div>

            <div class="col-md-6" id="pnl_payroll" runat="server" visible="true">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Payroll Master Data </h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <a class="btn btn-app" href="emp_payrollcategory.aspx" id="box_PayrollSkills" runat="server" visible="true">
                            <div id="PayrollSkills" class="badge bg-green" runat="server">
                                <asp:Label ID="Label10" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Categories
                        </a>
                        <a class="btn btn-app" href="emp_payroll_wages.aspx" id="box_PayrollWages" runat="server" visible="true">
                            <div id="PayrollWages" class="badge bg-green" runat="server">
                                <asp:Label ID="Label11" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Wages

                        </a>
                        <a class="btn btn-app" href="emp_payroll_designation.aspx" id="box_PayrollDesg" runat="server" visible="true">
                            <div id="PayrollDesg" class="badge bg-green" runat="server">
                                <asp:Label ID="Label12" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>Designation
                        </a>
                    </div>
                </div>
            </div>

            <div class="col-md-6" id="pnl_sites" runat="server" visible="true">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Work-sites Master Data </h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <a class="btn btn-app" href="ats_work_sites.aspx" id="box_Worksites" runat="server" visible="true">
                            <div id="Worksites" class="badge bg-green" runat="server">
                                <asp:Label ID="Label13" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Worksites
                        </a>
                        <a class="btn btn-app" href="atsworksite_incharges.aspx" id="box_SiteIncharges" runat="server" visible="true">
                            <div id="SiteIncharges" class="badge bg-green" runat="server">
                                <asp:Label ID="Label14" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>In-Charges

                        </a>
                    </div>
                </div>
            </div>

            <div class="col-md-6" id="pnl_controller" runat="server" visible="true">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>DB Controllers </h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <a class="btn btn-app" href="db_controller.aspx" id="box_jobs" runat="server" visible="true">
                            <div id="Div2" class="badge bg-green" runat="server">
                                <asp:Label ID="Label15" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>JOB's
                        </a>
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
