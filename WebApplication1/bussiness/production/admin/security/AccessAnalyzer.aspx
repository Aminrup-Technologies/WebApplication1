<%@ Page Title="Access Analyzer" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="AccessAnalyzer.aspx.cs" Inherits="WebApplication1.bussiness.production.admin.security.AccessAnalyzer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .access-analyzer .perm-stat { font-size: 26px; font-weight: 700; margin: 0; }
        .access-analyzer .kpi-label { color: #73879C; margin-bottom: 4px; }
        @media print {
            .no-print, .nav_menu, .left_col, footer, .sidebar-footer { display: none !important; }
            .right_col { margin-left: 0 !important; width: 100% !important; }
            .access-analyzer { color: #000; }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col access-analyzer" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>Access Analyzer <small>read-only</small></h3>
                </div>
            </div>
            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Migration dashboard</h2>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row">
                                <div class="col-md-2 col-sm-4 col-xs-6">
                                    <p class="kpi-label">Pages using AuthorizationService</p>
                                    <p class="perm-stat"><asp:Label ID="lbl_kpiPages" runat="server"></asp:Label></p>
                                </div>
                                <div class="col-md-2 col-sm-4 col-xs-6">
                                    <p class="kpi-label">Overlay grants</p>
                                    <p class="perm-stat"><asp:Label ID="lbl_kpiOverlay" runat="server"></asp:Label></p>
                                </div>
                                <div class="col-md-2 col-sm-4 col-xs-6">
                                    <p class="kpi-label">Config allowlist users</p>
                                    <p class="perm-stat"><asp:Label ID="lbl_kpiConfig" runat="server"></asp:Label></p>
                                </div>
                                <div class="col-md-2 col-sm-4 col-xs-6">
                                    <p class="kpi-label">Hardcoded users</p>
                                    <p class="perm-stat"><asp:Label ID="lbl_kpiHardcoded" runat="server"></asp:Label></p>
                                </div>
                                <div class="col-md-2 col-sm-4 col-xs-6">
                                    <p class="kpi-label">Module exceptions</p>
                                    <p class="perm-stat"><asp:Label ID="lbl_kpiModule" runat="server"></asp:Label></p>
                                </div>
                                <div class="col-md-2 col-sm-4 col-xs-6">
                                    <p class="kpi-label">Active employees scanned</p>
                                    <p class="perm-stat"><asp:Label ID="lbl_kpiScanned" runat="server"></asp:Label></p>
                                </div>
                            </div>
                            <p class="text-muted">Production page callers remain 0 until PR D/E. Hardcoded and module counts fill after Permission or Legacy analysis (`DescribeIdentity`).</p>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row no-print">
                <div class="col-md-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Mode</h2>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <asp:RadioButtonList ID="rbl_mode" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="rbl_mode_SelectedIndexChanged">
                                <asp:ListItem Text="Permission" Value="permission" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="User" Value="user"></asp:ListItem>
                                <asp:ListItem Text="Legacy" Value="legacy"></asp:ListItem>
                                <asp:ListItem Text="Overlay" Value="overlay"></asp:ListItem>
                            </asp:RadioButtonList>
                            <span style="margin-left: 16px;">
                                <asp:Button ID="btn_csv" runat="server" Text="Export CSV" CssClass="btn btn-default btn-sm" OnClick="btn_csv_Click" />
                                <asp:Button ID="btn_print" runat="server" Text="Print" CssClass="btn btn-default btn-sm" OnClick="btn_print_Click" />
                            </span>
                            <div>
                                <asp:Label ID="lbl_msg" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <asp:Panel ID="pnl_permission" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>Who has this permission?</h2>
                                <div class="clearfix"></div>
                            </div>
                            <div class="x_content">
                                <div class="form-group no-print">
                                    <label>Permission</label>
                                    <asp:DropDownList ID="ddl_permission" runat="server" CssClass="form-control form-control-sm" Width="320"></asp:DropDownList>
                                    <asp:Button ID="btn_analyzePermission" runat="server" Text="Analyze" CssClass="btn btn-success btn-sm" OnClick="btn_analyzePermission_Click" style="margin-top:8px;" />
                                </div>
                                <p>
                                    Overlay: <asp:Label ID="lbl_permOverlay" runat="server" Font-Bold="true"></asp:Label>
                                    · Config: <asp:Label ID="lbl_permConfig" runat="server" Font-Bold="true"></asp:Label>
                                    · Hardcoded: <asp:Label ID="lbl_permHardcoded" runat="server" Font-Bold="true"></asp:Label>
                                    · Module-only: <asp:Label ID="lbl_permModule" runat="server" Font-Bold="true"></asp:Label>
                                </p>
                                <div class="table-responsive">
                                    <asp:GridView ID="gv_permission" runat="server" Width="100%" CssClass="table table-striped table-bordered table-sm" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="Run Analyze to list holders.">
                                        <Columns>
                                            <asp:BoundField DataField="WorkmanSL" HeaderText="Employee" />
                                            <asp:BoundField DataField="FullName" HeaderText="Name" />
                                            <asp:BoundField DataField="UserType" HeaderText="USERTYPE" />
                                            <asp:BoundField DataField="Display" HeaderText="Source" />
                                            <asp:BoundField DataField="Source" HeaderText="Source enum" />
                                            <asp:BoundField DataField="Detail" HeaderText="Why" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnl_user" runat="server" Visible="false">
                <div class="row">
                    <div class="col-md-12">
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>User summary</h2>
                                <div class="clearfix"></div>
                            </div>
                            <div class="x_content">
                                <p class="text-muted no-print">Same Active-employee search as Permission Inspector. This view is a source summary, not the full inspect console.</p>
                                <div class="novalidate no-print">
                                    <div class="field item form-group">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align">Workman SL</label>
                                        <div class="col-md-6 col-sm-6">
                                            <asp:TextBox ID="txt_workman" runat="server" CssClass="form-control form-control-sm rounded" MaxLength="20"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="field item form-group">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align">First Name</label>
                                        <div class="col-md-6 col-sm-6">
                                            <asp:TextBox ID="txt_firstname" runat="server" CssClass="form-control form-control-sm rounded" MaxLength="80"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="field item form-group">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align">Full Name</label>
                                        <div class="col-md-6 col-sm-6">
                                            <asp:TextBox ID="txt_fullname" runat="server" CssClass="form-control form-control-sm rounded" MaxLength="120"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="no-print">
                                    <asp:Button ID="btn_searchUser" runat="server" Text="SEARCH" CssClass="btn btn-success btn-sm" OnClick="btn_searchUser_Click" />
                                </div>
                                <div class="table-responsive">
                                    <asp:GridView ID="gv_userSearch" runat="server" Width="100%" CssClass="table table-striped table-bordered table-sm" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowCommand="gv_userSearch_RowCommand">
                                        <Columns>
                                            <asp:BoundField DataField="WorkmanSL" HeaderText="Workman" />
                                            <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                                            <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                                            <asp:BoundField DataField="LoginID" HeaderText="Login ID" />
                                            <asp:BoundField DataField="User_RoleType" HeaderText="USERTYPE" />
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btn_summarize" runat="server" CssClass="btn btn-info btn-sm" CommandName="Summarize" CommandArgument='<%# Eval("LoginID") %>'>Summarize</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <p>
                                    <asp:Label ID="lbl_userIdentity" runat="server" Font-Bold="true"></asp:Label>
                                    Effective permission count:
                                    <asp:Label ID="lbl_userGranted" runat="server" Font-Bold="true"></asp:Label>
                                </p>
                                <div class="table-responsive">
                                    <asp:GridView ID="gv_userPerms" runat="server" Width="100%" CssClass="table table-striped table-bordered table-sm" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="Select an employee.">
                                        <Columns>
                                            <asp:BoundField DataField="Code" HeaderText="Permission" />
                                            <asp:BoundField DataField="Display" HeaderText="Source" />
                                            <asp:BoundField DataField="Source" HeaderText="Source enum" />
                                            <asp:BoundField DataField="Detail" HeaderText="Why" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnl_legacy" runat="server" Visible="false">
                <div class="row">
                    <div class="col-md-12">
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>Legacy exposure</h2>
                                <div class="clearfix"></div>
                            </div>
                            <div class="x_content">
                                <p class="text-muted">Employees whose <em>winning</em> `AuthorizationService` source is still Config, Hardcoded, or Module Exception. Overlay winners are omitted.</p>
                                <div class="no-print">
                                    <asp:Button ID="btn_analyzeLegacy" runat="server" Text="Run legacy report" CssClass="btn btn-success btn-sm" OnClick="btn_analyzeLegacy_Click" />
                                </div>
                                <div class="table-responsive">
                                    <asp:GridView ID="gv_legacy" runat="server" Width="100%" CssClass="table table-striped table-bordered table-sm" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="Run the legacy report.">
                                        <Columns>
                                            <asp:BoundField DataField="WorkmanSL" HeaderText="Employee" />
                                            <asp:BoundField DataField="FullName" HeaderText="Name" />
                                            <asp:BoundField DataField="UserType" HeaderText="USERTYPE" />
                                            <asp:BoundField DataField="Code" HeaderText="Permission" />
                                            <asp:BoundField DataField="Display" HeaderText="Legacy Type" />
                                            <asp:BoundField DataField="Source" HeaderText="Source enum" />
                                            <asp:BoundField DataField="Detail" HeaderText="Why" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnl_overlay" runat="server" Visible="false">
                <div class="row">
                    <div class="col-md-12">
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>Overlay adoption</h2>
                                <div class="clearfix"></div>
                            </div>
                            <div class="x_content">
                                <div id="pnl_overlayWarning" runat="server" class="alert alert-warning" visible="false">
                                    Overlay tables are not reachable. Inventory is empty (fail closed).
                                </div>
                                <div class="row">
                                    <div class="col-xs-4">
                                        <p class="kpi-label">Overlay grants</p>
                                        <p class="perm-stat"><asp:Label ID="lbl_overlayGrants" runat="server"></asp:Label></p>
                                    </div>
                                    <div class="col-xs-4">
                                        <p class="kpi-label">Groups</p>
                                        <p class="perm-stat"><asp:Label ID="lbl_overlayGroups" runat="server"></asp:Label></p>
                                    </div>
                                    <div class="col-xs-4">
                                        <p class="kpi-label">Legacy users (last scan)</p>
                                        <p class="perm-stat"><asp:Label ID="lbl_overlayLegacyUsers" runat="server"></asp:Label></p>
                                    </div>
                                </div>
                                <h4>Direct grants</h4>
                                <div class="table-responsive">
                                    <asp:GridView ID="gv_direct" runat="server" Width="100%" CssClass="table table-striped table-bordered table-sm" AutoGenerateColumns="true" ShowHeaderWhenEmpty="true" EmptyDataText="None"></asp:GridView>
                                </div>
                                <h4>Group grants</h4>
                                <div class="table-responsive">
                                    <asp:GridView ID="gv_groupGrants" runat="server" Width="100%" CssClass="table table-striped table-bordered table-sm" AutoGenerateColumns="true" ShowHeaderWhenEmpty="true" EmptyDataText="None"></asp:GridView>
                                </div>
                                <h4>Unused permissions</h4>
                                <asp:Label ID="lbl_unused" runat="server"></asp:Label>
                                <h4>Orphan groups</h4>
                                <div class="table-responsive">
                                    <asp:GridView ID="gv_orphans" runat="server" Width="100%" CssClass="table table-striped table-bordered table-sm" AutoGenerateColumns="true" ShowHeaderWhenEmpty="true" EmptyDataText="None"></asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <div class="row">
                <div class="col-md-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Permission distribution matrix</h2>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <p class="text-muted">Filled from the last Active-employee `DescribeIdentity` scan (Permission or Legacy mode).</p>
                            <div class="table-responsive">
                                <asp:GridView ID="gv_matrix" runat="server" Width="100%" CssClass="table table-striped table-bordered table-sm" AutoGenerateColumns="true" ShowHeaderWhenEmpty="true" EmptyDataText="Run Permission or Legacy analysis."></asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
