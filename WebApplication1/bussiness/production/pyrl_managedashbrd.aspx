<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="pyrl_managedashbrd.aspx.cs" Inherits="WebApplication1.bussiness.production.pyrl_managedashbrd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <%--<div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>Employee Payroll Inputs</h3>
                </div>
            </div>

            <div class="title_right">
                <div class="col-md-5 col-sm-5 form-group row pull-right top_search"></div>
            </div>
        </div>--%>

        <div class="clearfix"></div>

        <div class="row">
            <div class="col-md-12" id="Supvkpirow" runat="server" visible="true">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Employee Payroll Management : </h2>
                        <ul class="nav navbar-right panel_toolbox"></ul>
                        <div id="StateSelector" runat="server" class="col-md-2 col-sm-6 form-group" visible="false">
                            <asp:DropDownList ID="DDL_WorkStates" runat="server" CssClass="form-control form-control-sm rounded" OnSelectedIndexChanged="DDL_WorkStates_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div id="RegionSelector" runat="server" visible="false" class="col-md-2 col-sm-6  form-group">
                            <asp:DropDownList ID="DDL_WorkRegion" CssClass="form-control form-control-sm rounded" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL_WorkRegion_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div id="RegionComSelector" runat="server" visible="false" class="col-md-2 col-sm-6  form-group">
                            <asp:DropDownList ID="DDL_Company" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Company_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="clearfix"></div>
                    </div>
                </div>
            </div>

            <div class="col-md-6" id="Div1" runat="server" visible="true">
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
                        <a class="btn btn-app" href="viewupdate_emppayrolldata.aspx">
                            <div id="div_factorsstatus" class="badge bg-green" runat="server">
                                <asp:Label ID="lbl_factorsstatus" runat="server" Text="***" Visible="false"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Payroll Factors

                        </a>
                        <a class="btn btn-app" href="vw_emp_paymentbankdetails.aspx">
                            <div id="div_bankdatastatus" class="badge bg-green" runat="server">
                                <asp:Label ID="lbl_bankdatastatus" runat="server" Text="N/A" Visible="false"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>Bank Details

                        </a>
                    </div>
                </div>
            </div>

            <div class="col-md-6" id="Deductions" runat="server" visible="true">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Employee Deductions</h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <a class="btn btn-app" href="pyrl_deductions.aspx">
                            <div id="div_inactivedeductions" class="badge bg-green" runat="server">
                                <asp:Label ID="lbl_inactivedeductions" runat="server" Text="0" Visible="false"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Add New

                        </a>
                        <a class="btn btn-app" href="pyrl_deductionlist.aspx">
                            <div id="div_activedeductions" class="badge bg-orange" runat="server">
                                <asp:Label ID="lbl_activedeductions" runat="server" Text="0" Visible="false"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>View / Manage
                        </a>
                    </div>
                </div>
            </div>

            <div class="col-md-6" id="f16_row" runat="server" visible="true">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Attendance / Form 16</h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <a class="btn btn-app" href="generate_siteatdncsheet.aspx">
                            <i class="fa fa-edit"></i>Worksite

                        </a>
                        <a class="btn btn-app" href="generate_atdncsheet.aspx">
                            <asp:Label ID="Label1" runat="server" Text="0" Visible="false"></asp:Label>
                            <i class="fa fa-users"></i>Workregion

                        </a>
                    </div>
                </div>
            </div>

            <div class="col-md-6" id="f17_row" runat="server" visible="true">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Form 17</h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <a class="btn btn-app" href="generate_trialform17.aspx" id="KPO_F17" runat="server">
                            <asp:Label ID="Label22" runat="server" Text="0" Visible="false"></asp:Label>
                            <i class="fa fa-edit"></i>KPO F17
                        </a>

                        <a class="btn btn-app" href="gen_agl_F17.aspx" id="AGL_F17" runat="server">
                            <asp:Label ID="Label9" runat="server" Text="0" Visible="false"></asp:Label>
                            <i class="fa fa-edit"></i>AGL F17
                        </a>

                        <a class="btn btn-app" href="gen_jsr_F17.aspx" id="JSR_F17" runat="server">
                            <asp:Label ID="Label11" runat="server" Text="0" Visible="false"></asp:Label>
                            <i class="fa fa-edit"></i>JSR F17
                        </a>

                        <a class="btn btn-app" href="gen_ninl_F17.aspx" id="NINL_F17" runat="server">
                            <asp:Label ID="Label10" runat="server" Text="0" Visible="false"></asp:Label>
                            <i class="fa fa-edit"></i>NINL F17
                        </a>

                        <a class="btn btn-app" href="gen_ats_f17.aspx" id="ATS_F17" runat="server">
                            <span class="badge bg-green">New</span><i class="fa fa-edit"></i>ATS Global F17</a>

                        <a class="btn btn-app" href="#">
                            <asp:Label ID="Label23" runat="server" Text="0" Visible="false"></asp:Label>
                            <i class="fa fa-users"></i>Approve Deductions

                        </a>
                        <a class="btn btn-app" href="#">
                            <asp:Label ID="Label24" runat="server" Text="0" Visible="false"></asp:Label>
                            <i class="fa fa-users"></i>Final 17
                        </a>
                    </div>
                </div>
            </div>

            <div class="col-md-6" id="rpts_row" runat="server" visible="true">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Reports / Exports</h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <a class="btn btn-app" href="generate_trialform17.aspx">
                            <asp:Label ID="Label3" runat="server" Text="0" Visible="false"></asp:Label>
                            <i class="fa fa-edit"></i>Form-17

                        </a>
                        <a class="btn btn-app" href="generate_f29sheet.aspx">
                            <asp:Label ID="Label4" runat="server" Text="0" Visible="false"></asp:Label>
                            <i class="fa fa-users"></i>Form-29

                        </a>
                        <a class="btn btn-app" href="generate_banksheets.aspx">
                            <asp:Label ID="Label5" runat="server" Text="0" Visible="false"></asp:Label>
                            <i class="fa fa-users"></i>Bank Sheets
                        </a>
                    </div>
                </div>
            </div>

            <div class="col-md-6" id="emp_comprow" runat="server" visible="true">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Employee Compliances</h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <a class="btn btn-app" href="generate_pfsheets.aspx">
                            <asp:Label ID="Label6" runat="server" Text="0" Visible="false"></asp:Label>
                            <i class="fa fa-edit"></i>PF
                        </a>
                        <a class="btn btn-app" href="generate_esicsheets.aspx">
                            <asp:Label ID="Label7" runat="server" Text="0" Visible="false"></asp:Label>
                            <i class="fa fa-users"></i>ESIC
                        </a>
                        <a class="btn btn-app" href="#">
                            <asp:Label ID="Label8" runat="server" Text="0" Visible="false"></asp:Label>
                            <i class="fa fa-users"></i>TAX
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
