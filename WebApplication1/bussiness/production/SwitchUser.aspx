<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="SwitchUser.aspx.cs" Inherits="WebApplication1.bussiness.production.SwitchUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>Switch User</h3>
                </div>
            </div>
            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div id="pnl_return" runat="server" visible="false" class="x_panel">
                        <div class="x_title">
                            <h2>Return to original account</h2>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <p>
                                You are viewing as
                                <asp:Label ID="lbl_currentTarget" runat="server" Font-Bold="true"></asp:Label>
                                (Workman
                                <asp:Label ID="lbl_currentTargetWorkman" runat="server" Font-Bold="true"></asp:Label>).
                                Original admin:
                                <asp:Label ID="lbl_returnOriginal" runat="server" Font-Bold="true"></asp:Label>.
                            </p>
                            <asp:Button ID="btn_return" runat="server" Text="Return to my account" CssClass="btn btn-warning btn-sm" OnClick="btn_return_Click" OnClientClick="return confirm('Return to your original admin account?');" />
                        </div>
                    </div>

                    <div id="pnl_search" runat="server" visible="false" class="x_panel">
                        <div class="x_title">
                            <h2>Search an active employee</h2>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <p class="text-muted">Search uses Workman SL, First Name, and/or Full Name. Only Active employees are returned. Nested impersonation is not allowed.</p>
                            <div class="novalidate">
                                <div class="field item form-group">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">Workman SL</label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_workman" runat="server" CssClass="form-control form-control-sm rounded" MaxLength="20" placeholder="Exact Workman SL"></asp:TextBox>
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
                                    <asp:GridView ID="gv_results" runat="server" Width="100%" CssClass="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowCommand="gv_results_RowCommand" OnRowDataBound="gv_results_RowDataBound">
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
                                                    <asp:LinkButton ID="btn_switch" runat="server" CssClass="btn btn-warning btn-sm" CommandName="SwitchUser" CommandArgument='<%# Eval("LoginID") %>' OnClientClick="return confirm('Switch to this user? Use Return to restore your admin session.');">Switch</asp:LinkButton>
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
        </div>
    </div>
</asp:Content>
