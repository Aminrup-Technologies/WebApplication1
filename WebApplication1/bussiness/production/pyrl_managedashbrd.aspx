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

       <%-- <div class="clearfix"></div>--%>

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
                        <div id="DataLocker" runat="server" visible="true" class="col-md-2 col-sm-6  form-group">
                            <asp:Button ID="btn_datalocker" runat="server" Text="Lock" CssClass="btn btn-success btn-sm" OnClick="btn_datalocker_Click" />
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
                        <a class="btn btn-app" href="emp_payroll_wages.aspx">
                            <div id="div_payrollwages" class="badge bg-green" runat="server">
                                <asp:Label ID="Label17" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Payroll Wages
                        </a>
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
                        <a class="btn btn-app" href="#">
                            <div id="div4" class="badge bg-red" runat="server">
                                <asp:Label ID="Label13" runat="server" Text="WIP" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>Uplaod Bank Data
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

                        <a class="btn btn-app" href="#" data-toggle="modal" data-target="#Deductiondel_modal">
                            <div id="div2" class="badge bg-green" runat="server">
                                <asp:Label ID="Label2" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>Clear All
                        </a>

                        <a class="btn btn-app" href="pyrl_upldadvc.aspx">
                            <div id="div3" class="badge bg-green" runat="server">
                                <asp:Label ID="Label12" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>Upload Deductions
                        </a>
                    </div>

                    <!-- Large modal : Confirm to Clear all deductions-------START------>
                    <div class="modal fade bs-pass-modal-sm" id="Deductiondel_modal" data-backdrop="static">
                        <div class="modal-dialog modal-lg">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title" id="myModalLabel2A">Acknowledge Confirmation</h4>
                                    <button type="button" class="close" data-dismiss="modal">
                                        <span aria-hidden="true">×</span>
                                    </button>
                                </div>
                                <div class="modal-body">
                                    <div class="container-fluid">
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <div class="form-group">
                                                <div class="col-md-12 form-group">
                                                    <span class="text text-danger">
                                                        <asp:Label ID="Label4" runat="server" Text="Please confirm,<br /> If you want to clear all deductions against the selcted State, Work Region and Work Company Employees.<br />Thank You" Font-Bold="true" ForeColor="IndianRed"></asp:Label>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">CANCEL</button>
                                    <asp:Button ID="btn_clrded" runat="server" Text="CONFIRM" CausesValidation="false" CssClass="btn btn-success btn-sm" OnClick="btn_clrded_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Large modal : Personal Information-------END------>

                    <!-- Small modal -->
                    <asp:Button ID="ShowPopup" runat="server" Text="Button" class="btn btn-primary" Visible="false" data-toggle="modal" data-target=".bs-example-modal-sm" />
                    <div id="MyPopup" class="modal fade bs-example-modal-sm" tabindex="-1" role="dialog" aria-hidden="true">
                        <div class="modal-dialog modal-sm">
                            <div class="modal-content">

                                <div class="modal-header">
                                    <h4 class="modal-title" id="myModalLabel2"></h4>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">×</span>
                                    </button>
                                </div>
                                <div class="modal-body">
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-secondary btn-sm" data-dismiss="modal">Close</button>
                                </div>

                            </div>
                        </div>
                    </div>
                    <!-- Small Modal - END---->

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
                            <div id="div_activeworksites" class="badge bg-orange" runat="server">
                                <asp:Label ID="lbl_activeworksites" runat="server" Text="0" Visible="false"></asp:Label>
                            </div>
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

                        <a class="btn btn-app" href="pyrl_approvedeductions.aspx">
                            <div id="div5" class="badge bg-red" runat="server">
                                <asp:Label ID="Label14" runat="server" Text="WIP" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>Approve Deductions
                        </a>
                        <a class="btn btn-app" href="#">
                            <div id="div6" class="badge bg-red" runat="server">
                                <asp:Label ID="Label15" runat="server" Text="WIP" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>Final F17
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
                        <a class="btn btn-app" href="#">
                            <div id="div8" class="badge bg-red" runat="server">
                                <asp:Label ID="Label3" runat="server" Text="N/A" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Form-17

                        </a>

                        <a class="btn btn-app" href="pyrl_generate_f29.aspx">
                            <div id="div7" class="badge bg-green" runat="server">
                                <asp:Label ID="Label16" runat="server" Text="New" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>Form-29
                        </a>

                        <a class="btn btn-app" href="generate_banksheets.aspx">
                            <div id="div9" class="badge bg-green" runat="server">
                                <asp:Label ID="Label5" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
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

            <div class="col-md-6" id="control_panel" runat="server" visible="false">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Payroll Controller</h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <a class="btn btn-app" href="#">
                            <asp:Label ID="Label18" runat="server" Text="0" Visible="true"></asp:Label>
                            <i class="fa fa-edit"></i>Rollback
                        </a>
                        <a class="btn btn-app" href="#">
                            <asp:Label ID="Label19" runat="server" Text="0" Visible="true"></asp:Label>
                            <i class="fa fa-users"></i>Paroll Visibility
                        </a>
                    </div>
                </div>
            </div>

        </div>
    </div>
    <script type="text/javascript">
        function ShowPopup(title, body) {
            $("#MyPopup .modal-title").html(title);
            $("#MyPopup .modal-body").html(body);
            $("#MyPopup").modal("show");
        }

        function ShowPopup1() {
            $("#myModal").modal("show");
        }

        function ShowPasswordModal() {
            $("#Deductiondel_modal").modal("show");
        }
    </script>
</asp:Content>
