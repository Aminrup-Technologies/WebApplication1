<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="str_masters.aspx.cs" Inherits="WebApplication1.bussiness.production.str_masters" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="col-md-6" id="storemaster_panel" runat="server" visible="true">
            <div class="x_panel">
                <div class="x_title">
                    <h2>Manage Store</h2>
                    <ul class="nav navbar-right panel_toolbox">
                        <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                        </li>
                    </ul>
                    <div class="clearfix"></div>
                </div>
                <div class="x_content">
                    <a class="btn btn-app" href="str_add_warehouse.aspx">
                        <div id="div2" class="badge bg-red" runat="server">
                            <asp:Label ID="Label2" runat="server" Text="WIP" Visible="true"></asp:Label>
                        </div>
                        <i class="fa fa-cubes"></i>Add Warehouse
                    </a>
                    <a class="btn btn-app" href="str_add_cstores.aspx">
                        <div id="div11" class="badge bg-red" runat="server">
                            <asp:Label ID="Label18" runat="server" Text="WIP" Visible="true"></asp:Label>
                        </div>
                        <i class="fa fa-cubes"></i>Add Central Store
                    </a>
                    <a class="btn btn-app" href="str_add_sitestore.aspx">
                        <div id="div12" class="badge bg-red" runat="server">
                            <asp:Label ID="Label19" runat="server" Text="WIP" Visible="true"></asp:Label>
                        </div>
                        <i class="fa fa-cubes"></i>Add Site Store
                    </a>
                    <a class="btn btn-app" href="#">
                        <div id="div1" class="badge bg-red" runat="server">
                            <asp:Label ID="Label1" runat="server" Text="WIP" Visible="true"></asp:Label>
                        </div>
                        <i class="fa fa-user"></i>Store Managers
                    </a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
