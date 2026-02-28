<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="deta_employeemaster.aspx.cs" Inherits="WebApplication1.bussiness.production.deta_employeemaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="row">

            <div class="col-md-6" id="pnl_addemployee" runat="server" visible="true">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Employee Master Data </h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <a class="btn btn-app" href="emp_registration.aspx" id="box_AddEmployee" runat="server" visible="true">
                            <div id="AddEmployee" class="badge bg-green" runat="server">
                                <asp:Label ID="Label17" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Add Employee
                        </a>
                        <a class="btn btn-app" href="emp_bulkregistration.aspx" id="box_BulkRegistration" runat="server" visible="true">
                            <div id="BulkRegistration" class="badge bg-green" runat="server">
                                <asp:Label ID="lbl_factorsstatus" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Bulk Registration

                        </a>
                        <a class="btn btn-app" href="view_emp_mastertbldata_v2.aspx" id="box_ViewMasterData" runat="server" visible="true">
                            <div id="ViewMasterData" class="badge bg-green" runat="server">
                                <asp:Label ID="lbl_bankdatastatus" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>View Employees
                        </a>
                        <a class="btn btn-app" href="viewupdate_empmustertabledata.aspx" id="box_UpdateEmpMasterData" runat="server" visible="false">
                            <div id="UpdateEmpMasterData" class="badge bg-green" runat="server">
                                <asp:Label ID="Label1" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>Update Employees
                        </a>
                    </div>
                </div>
            </div>

            <div class="col-md-6" id="pnl_updtemployee" runat="server" visible="true">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Update Employee Master Data </h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <a class="btn btn-app" href="bulk_employeeupdate.aspx" id="bulkupdate" runat="server" visible="true">
                            <div id="Div2" class="badge bg-green" runat="server">
                                <asp:Label ID="Label2" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Bulk Update
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
