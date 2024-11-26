<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="anlys_jobsdeta.aspx.cs" Inherits="WebApplication1.bussiness.production.anlys_jobsdeta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="row">

            <div class="col-md-6" id="pnl_employee" runat="server" visible="true">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>JOB & Manpower Data </h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <a class="btn btn-app" href="view_monthlyjobs.aspx" id="box_monthlysummary" runat="server" visible="true">
                            <div id="monthlysummary" class="badge bg-green" runat="server">
                                <asp:Label ID="Label17" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Monthly Summary
                        </a>
                        <a class="btn btn-app" href="view_dailyjobs.aspx" id="box_dailysummary" runat="server" visible="true">
                            <div id="dailysummary" class="badge bg-green" runat="server">
                                <asp:Label ID="lbl_factorsstatus" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Daily Summary

                        </a>
                        <a class="btn btn-app" href="swap_jobdate.aspx" id="box_swapjobdate" runat="server" visible="true">
                            <div id="swapjobdate" class="badge bg-green" runat="server">
                                <asp:Label ID="lbl_bankdatastatus" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>Swap JOB Date
                        </a>
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
