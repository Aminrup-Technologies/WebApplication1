<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="view_emp_mastertbldata.aspx.cs" Inherits="WebApplication1.bussiness.production.view_emp_mastertbldata" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h5>View Employee Master Data ||
                        <asp:Button ID="Button1" runat="server" Text="Export" OnClick="ExportExcel" CssClass="btn btn-primary btn-sm" /></h5>
                </div>

                <div class="title_right">
                    <div class="col-md-5 col-sm-5 form-group pull-right top_search"></div>
                </div>

            </div>

            <div class="clearfix"></div>

            <!-- Small modal -->
            <asp:Button ID="ShowPopup" runat="server" Text="Button" class="btn btn-primary" Visible="false" data-toggle="modal" data-target=".bs-example-modal-sm" />
            <div id="MyPopup" class="modal fade bs-example-modal-sm" tabindex="-1" role="dialog" aria-hidden="true">
                <div class="modal-dialog modal-sm">
                    <div class="modal-content">

                        <div class="modal-header">
                            <h4 class="modal-title" id="myModalLabel2"></h4>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">×</span>
                            </button>
                        </div>
                        <div class="modal-body">
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary btn-sm" data-dismiss="modal">Close</button>
                        </div>

                    </div>
                </div>
            </div>

            <div class="row">
                <div class="card-box col-md-12 col-sm-12 small" style="width: 100%; height: 450px; overflow: scroll">
                    <asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="ID" HeaderStyle-Width="5%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Region Code" HeaderStyle-Width="5%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_WorkRegion" runat="server" Text='<%# Eval("WorkRegion") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Company Code" HeaderStyle-Width="5%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_WorkCompany" runat="server" Text='<%# Eval("WorkCompany") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Status" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Button ID="btn_workstatus" runat="server" Text='<%# Eval("WorkStatus") %>' Font-Size="Smaller" CssClass="btn btn-sm btn-info" CommandName="Swap_WorkStatus" CommandArgument="<%# Container.DataItemIndex %>" />
                                    <asp:Label ID="lbl_WorkStatus" runat="server" Text='<%# Eval("WorkStatus") %>' Visible="false" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="User ID" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_LoginID" runat="server" Text='<%# Eval("LoginID") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Password" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_LoginPassword" runat="server" Text='<%# Eval("LoginPassword") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Workman" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Button ID="btn_viewdetails" runat="server" Text='<%# Eval("WorkmanSL") %>' Font-Size="Smaller" CssClass="btn btn-sm btn-info" CommandName="View_Details" CommandArgument="<%# Container.DataItemIndex %>" />
                                    <asp:Label ID="lbl_WorkmanSL" runat="server" Text='<%# Eval("WorkmanSL") %>' Visible="false" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_FullName" runat="server" Text='<%# Eval("FullName") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Father Name" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Fathername" runat="server" Text='<%# Eval("Fathername") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Mobile" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_MobileNo" runat="server" Text='<%# Eval("MobileNo") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Work Site" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_WorkSite" runat="server" Text='<%# Eval("WorkSite") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Skill Category" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SkillCategory" runat="server" Text='<%# Eval("SkillCategory") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Designation" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SkillDesignation" runat="server" Text='<%# Eval("SkillDesignation") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Safety PassNo" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SafetyPassNo" runat="server" Text='<%# Eval("SafetyPassNo") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Gate PassNo" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_GatePassNo" runat="server" Text='<%# Eval("GatePassNo") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                        </Columns>
                        <EmptyDataTemplate>
                            <div class="grid">No Data Found</div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function ShowPopup(title, body) {
            $("#MyPopup .modal-title").html(title);
            $("#MyPopup .modal-body").html(body);
            $("#MyPopup").modal("show");
        }
    </script>
</asp:Content>
