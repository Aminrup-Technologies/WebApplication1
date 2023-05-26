<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="pyrl_managedashbrd.aspx.cs" Inherits="WebApplication1.bussiness.production.pyrl_managedashbrd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>Employee Payroll Inputs</h3>
                </div>

                <div class="title_right">
                    <div class="col-md-5 col-sm-5 form-group row pull-right top_search"></div>
                </div>
            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12" id="Supvkpirow" runat="server" visible="true">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Employee Payroll Muster</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <a class="btn btn-app" href="viewupdate_emppayrolldata.aspx">
                                <i class="fa fa-edit"></i>Factors

                            </a>
                            <a class="btn btn-app" href="vw_emp_paymentbankdetails.aspx">
                                    <asp:Label ID="Label1" runat="server" Text="0" Visible="false"></asp:Label>
                                <i class="fa fa-users"></i>Bank Details

                            </a>
                        </div>
                    </div>
                </div>


                <div class="col-md-12" id="Deductions" runat="server" visible="true">
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
                                    <asp:Label ID="Label22" runat="server" Text="0" Visible="false"></asp:Label>
                                <i class="fa fa-edit"></i>Add New

                            </a>
                            <a class="btn btn-app" href="pyrl_deductionlist.aspx">
                                    <asp:Label ID="Label23" runat="server" Text="0" Visible="false"></asp:Label>
                                <i class="fa fa-users"></i>View / Manage

                            </a>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
