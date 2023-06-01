<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="pyrl_gnrtdashbrd.aspx.cs" Inherits="WebApplication1.bussiness.production.pyrl_gnrtdashbrd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>Employee Payroll Outputs</h3>
                </div>

                <div class="title_right">
                    <div class="col-md-5 col-sm-5 form-group row pull-right top_search"></div>
                </div>
            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12" id="f16_row" runat="server" visible="true">
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


                <div class="col-md-12" id="f17_row" runat="server" visible="true">
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


                <div class="col-md-12" id="f29_row" runat="server" visible="true">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Form 29</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <a class="btn btn-app" href="generate_f29sheet.aspx">
                                    <asp:Label ID="Label2" runat="server" Text="0" Visible="false"></asp:Label>
                                <i class="fa fa-edit"></i>F29 Report

                            </a>
                        </div>
                    </div>
                </div>

                <div class="col-md-12" id="rpts_row" runat="server" visible="true">
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
                                <i class="fa fa-edit"></i>F 17

                            </a>
                            <a class="btn btn-app" href="generate_f29sheet.aspx">
                                    <asp:Label ID="Label4" runat="server" Text="0" Visible="false"></asp:Label>
                                <i class="fa fa-users"></i>F 29

                            </a>
                            <a class="btn btn-app" href="generate_banksheets.aspx">
                                    <asp:Label ID="Label5" runat="server" Text="0" Visible="false"></asp:Label>
                                <i class="fa fa-users"></i>Bank Sheets

                            </a>
                        </div>
                    </div>
                </div>

                <div class="col-md-12" id="emp_comprow" runat="server" visible="true">
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
    </div>
</asp:Content>
