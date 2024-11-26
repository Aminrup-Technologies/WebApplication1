<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="anlys_musterdeta.aspx.cs" Inherits="WebApplication1.bussiness.production.anlys_musterdeta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="row">

            <div class="col-md-6" id="pnl_employee" runat="server" visible="true">
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
                        <a class="btn btn-app" href="#" id="box_AddEmployee" runat="server" visible="true">
                            <div id="AddEmployee" class="badge bg-green" runat="server">
                                <asp:Label ID="Label17" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Work Regions
                        </a>
                        <a class="btn btn-app" href="#" id="box_BulkRegistration" runat="server" visible="true">
                            <div id="BulkRegistration" class="badge bg-green" runat="server">
                                <asp:Label ID="lbl_factorsstatus" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Work Companies

                        </a>
                        <a class="btn btn-app" href="#" id="box_ViewMasterData" runat="server" visible="true">
                            <div id="ViewMasterData" class="badge bg-green" runat="server">
                                <asp:Label ID="lbl_bankdatastatus" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>Active P.O.
                        </a>
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
