<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="anlys_employees.aspx.cs" Inherits="WebApplication1.bussiness.production.anlys_employees" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="row">

            <div class="col-md-6" id="pnl_employee" runat="server" visible="true">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Employee Summary </h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <a class="btn btn-app" href="view_empmonthlyatt.aspx" id="box_empsummary" runat="server" visible="true">
                            <div id="empsummary" class="badge bg-green" runat="server">
                                <asp:Label ID="Label17" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Employee Summary
                        </a>
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
