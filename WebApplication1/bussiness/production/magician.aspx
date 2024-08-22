<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="magician.aspx.cs" Inherits="WebApplication1.bussiness.production.magician" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<div class="right_col" role="main">
        <div class="row">

            <div class="col-md-6" id="Div_rolespermissions" runat="server" visible="true">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Roles & Permission Master Data </h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <a class="btn btn-app" href="#">
                            <div id="div_roles" class="badge bg-green" runat="server">
                                <asp:Label ID="lbl_roles" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Role Type
                        </a>
                        <a class="btn btn-app" href="#">
                            <div id="div_rolepermissions" class="badge bg-green" runat="server">
                                <asp:Label ID="lbl_rolepermissions" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Role Permissions

                        </a>
                        <a class="btn btn-app" href="#">
                            <div id="div_accesspermissions" class="badge bg-green" runat="server">
                                <asp:Label ID="lbl_accesspermissions" runat="server" Text="N/A" Visible="false"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>Access Permissions
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>--%>


    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-6" id="Div_rolespermissions" runat="server" visible="true">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Roles & Permission Master Data</h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <a id="div_roles" runat="server" class="btn btn-app" href="~/bussiness/production/manage_rolls.aspx">
                            <div  class="badge bg-green" runat="server">
                                <asp:Label ID="lbl_roles" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Role Type
                        </a>
                        <a id="div_rolepermissions" runat="server" class="btn btn-app" href="~/bussiness/production/manage_rollsaccess.aspx">
                            <div  class="badge bg-green" runat="server">
                                <asp:Label ID="lbl_rolepermissions" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Role Permissions
                        </a>
                        <a id="div_accesspermissions" runat="server" class="btn btn-app" href="#">
                            <div  class="badge bg-green" runat="server">
                                <asp:Label ID="lbl_accesspermissions" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>Access Permissions
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
