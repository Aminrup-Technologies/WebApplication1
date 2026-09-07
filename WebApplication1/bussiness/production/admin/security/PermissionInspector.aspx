<%@ Page Title="Permission Inspector" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="PermissionInspector.aspx.cs" Inherits="WebApplication1.bussiness.production.admin.security.PermissionInspector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .perm-inspector .perm-photo { width: 88px; height: 88px; object-fit: cover; border-radius: 8px; border: 1px solid #ddd; }
        .perm-inspector .perm-yes { color: #26B99A; font-weight: 700; }
        .perm-inspector .perm-no { color: #d9534f; font-weight: 700; }
        .perm-inspector .perm-stat { font-size: 28px; font-weight: 700; margin: 0; }
        .perm-inspector .perm-warning { margin-bottom: 12px; }
        .perm-inspector .identity-meta { color: #73879C; margin-top: 4px; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col perm-inspector" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>Permission Inspector <small>read-only</small></h3>
                </div>
            </div>
            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Search an active employee</h2>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <p class="text-muted">Workman SL (exact or contains), First Name, and/or Full Name. Active employees only. This page does not change permissions.</p>
                            <div class="novalidate">
                                <div class="field item form-group">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">Workman SL</label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_workman" runat="server" CssClass="form-control form-control-sm rounded" MaxLength="20" placeholder="Exact or partial Workman SL"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="field item form-group">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">First Name</label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_firstname" runat="server" CssClass="form-control form-control-sm rounded" MaxLength="80" placeholder="First name contains"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="field item form-group">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">Full Name</label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_fullname" runat="server" CssClass="form-control form-control-sm rounded" MaxLength="120" placeholder="Full name contains"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 center-margin">
                                <div class="ln_solid"></div>
                                <div class="item form-group row">
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Label ID="lbl_msg" runat="server" Text="Enter at least one search field, then SEARCH."></asp:Label>
                                    </div>
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="btn btn-warning btn-sm" OnClick="btn_reset_Click" CausesValidation="false" />
                                        <asp:Button ID="btn_search" runat="server" Text="SEARCH" CssClass="btn btn-success btn-sm" OnClick="btn_search_Click" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12 col-sm-12">
                                <div class="card-box table-responsive">
                                    <asp:GridView ID="gv_results" runat="server" Width="100%" CssClass="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowCommand="gv_results_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl. NO" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text-center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Workman" HeaderStyle-Width="8%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_WorkmanSL" runat="server" Text='<%# Eval("WorkmanSL") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text-center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="First Name" HeaderStyle-Width="12%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_FirstName" runat="server" Text='<%# Eval("FirstName") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text-center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Full Name" HeaderStyle-Width="18%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_FullName" runat="server" Text='<%# Eval("FullName") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text-center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Login ID" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_LoginID" runat="server" Text='<%# Eval("LoginID") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text-center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" HeaderStyle-Width="8%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_WorkStatus" runat="server" Text='<%# Eval("WorkStatus") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text-center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Role Type" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_UserRoleType" runat="server" Text='<%# Eval("User_RoleType") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text-center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Region" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_WorkRegion" runat="server" Text='<%# Eval("WorkRegion") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text-center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Company" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_WorkCompany" runat="server" Text='<%# Eval("WorkCompany") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text-center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action" HeaderStyle-Width="9%">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btn_inspect" runat="server" CssClass="btn btn-info btn-sm" CommandName="Inspect" CommandArgument='<%# Eval("LoginID") %>'>Inspect</asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text-center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <asp:Panel ID="pnl_inspect" runat="server" Visible="false">
                <div class="row">
                    <div class="col-md-12 col-sm-12">
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>Identity</h2>
                                <div class="clearfix"></div>
                            </div>
                            <div class="x_content">
                                <div class="row">
                                    <div class="col-md-2 col-sm-3 text-center">
                                        <asp:Image ID="img_photo" runat="server" CssClass="perm-photo" AlternateText="Employee photo" />
                                    </div>
                                    <div class="col-md-10 col-sm-9">
                                        <h4 class="mb-0">
                                            <asp:Label ID="lbl_fullName" runat="server" Font-Bold="true"></asp:Label>
                                            <small>
                                                <asp:Label ID="lbl_workman" runat="server"></asp:Label>
                                            </small>
                                        </h4>
                                        <p class="identity-meta">
                                            USERTYPE:
                                            <asp:Label ID="lbl_userType" runat="server" Font-Bold="true"></asp:Label>
                                            · Region:
                                            <asp:Label ID="lbl_region" runat="server"></asp:Label>
                                            · Site:
                                            <asp:Label ID="lbl_site" runat="server"></asp:Label>
                                        </p>
                                        <div class="table-responsive">
                                            <table class="table table-bordered table-sm">
                                                <tr>
                                                    <th>LoginID</th>
                                                    <td><asp:Label ID="lbl_loginId" runat="server"></asp:Label></td>
                                                    <th>UserRoleDB</th>
                                                    <td><asp:Label ID="lbl_userRoleDb" runat="server"></asp:Label></td>
                                                    <th>RolePermissionDB</th>
                                                    <td><asp:Label ID="lbl_rolePermissionDb" runat="server"></asp:Label></td>
                                                </tr>
                                            </table>
                                        </div>
                                        <p class="text-muted">Evaluated as a normal login of this employee. The inspector Session is restored afterward.</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-7 col-sm-12">
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>Effective permissions</h2>
                                <div class="clearfix"></div>
                            </div>
                            <div class="x_content">
                                <div class="table-responsive">
                                    <asp:GridView ID="gv_permissions" runat="server" Width="100%" CssClass="table table-striped table-bordered table-sm" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No permission rows." OnRowDataBound="gv_permissions_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="Code" HeaderText="Permission" />
                                            <asp:TemplateField HeaderText="Allowed" ItemStyle-CssClass="text-center" HeaderStyle-Width="12%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_allowed" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Layer" HeaderText="Layer" HeaderStyle-Width="12%" />
                                            <asp:BoundField DataField="Source" HeaderText="Source" />
                                            <asp:BoundField DataField="OverlayWouldAllow" HeaderText="OverlayWouldAllow" />
                                            <asp:BoundField DataField="LegacyWouldAllow" HeaderText="LegacyWouldAllow" />
                                            <asp:BoundField DataField="Detail" HeaderText="Why" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

                        <div class="x_panel">
                            <div class="x_title">
                                <h2>Source breakdown</h2>
                                <div class="clearfix"></div>
                            </div>
                            <div class="x_content">
                                <asp:Repeater ID="rpt_breakdown" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-bordered table-sm">
                                            <thead>
                                                <tr>
                                                    <th>Layer</th>
                                                    <th>Kind</th>
                                                    <th>Actual source</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("Layer") %></td>
                                            <td><%# Eval("Kind") %></td>
                                            <td><%# Eval("Value") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                            </tbody>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <asp:Label ID="lbl_breakdownEmpty" runat="server" CssClass="text-muted" Visible="false" Text="No contributing layers for this employee."></asp:Label>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-5 col-sm-12">
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>Legacy diagnostics</h2>
                                <div class="clearfix"></div>
                            </div>
                            <div class="x_content">
                                <div class="table-responsive">
                                    <asp:GridView ID="gv_legacy" runat="server" Width="100%" CssClass="table table-striped table-bordered table-sm" AutoGenerateColumns="false" OnRowDataBound="gv_legacy_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="Check" HeaderText="Legacy Check" />
                                            <asp:TemplateField HeaderText="Result" ItemStyle-CssClass="text-center" HeaderStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_result" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

                        <div class="x_panel">
                            <div class="x_title">
                                <h2>Overlay status</h2>
                                <div class="clearfix"></div>
                            </div>
                            <div class="x_content">
                                <div id="pnl_overlayWarning" runat="server" class="alert alert-warning perm-warning" visible="false">
                                    Overlay tables are not reachable. Grants fail closed (empty). Legacy Session / USERTYPE / config / hardcoded gates still apply.
                                </div>
                                <div class="row">
                                    <div class="col-xs-6">
                                        <p class="text-muted">PermissionRepository health</p>
                                        <p class="perm-stat"><asp:Label ID="lbl_overlayHealth" runat="server"></asp:Label></p>
                                    </div>
                                    <div class="col-xs-3">
                                        <p class="text-muted">Direct grants</p>
                                        <p class="perm-stat"><asp:Label ID="lbl_directCount" runat="server"></asp:Label></p>
                                    </div>
                                    <div class="col-xs-3">
                                        <p class="text-muted">Group grants</p>
                                        <p class="perm-stat"><asp:Label ID="lbl_groupCount" runat="server"></asp:Label></p>
                                    </div>
                                </div>
                                <div class="table-responsive">
                                    <table class="table table-bordered table-sm">
                                        <tr>
                                            <th>Overlay availability</th>
                                            <td><asp:Label ID="lbl_overlayAvailability" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <th>Direct codes</th>
                                            <td><asp:Label ID="lbl_directCodes" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <th>Group membership</th>
                                            <td><asp:Label ID="lbl_groupCodes" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <th>Group-inherited codes</th>
                                            <td><asp:Label ID="lbl_groupPermCodes" runat="server"></asp:Label></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>

                        <div class="x_panel">
                            <div class="x_title">
                                <h2>Cache verification</h2>
                                <div class="clearfix"></div>
                            </div>
                            <div class="x_content">
                                <div class="table-responsive">
                                    <table class="table table-bordered table-sm">
                                        <tr>
                                            <th>Cache status</th>
                                            <td><asp:Label ID="lbl_cacheStatus" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <th>This load</th>
                                            <td><asp:Label ID="lbl_cacheHit" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <th>Last refresh (UTC)</th>
                                            <td><asp:Label ID="lbl_cacheRefresh" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <th>Cache TTL</th>
                                            <td><asp:Label ID="lbl_cacheTtl" runat="server"></asp:Label></td>
                                        </tr>
                                    </table>
                                </div>
                                <p class="text-muted">Read-only. This page does not invalidate or write the cache.</p>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
