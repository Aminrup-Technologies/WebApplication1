<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="pyrl_deductionsview.aspx.cs" Inherits="WebApplication1.bussiness.production.pyrl_deductionsview" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h5>View Employee Master Data</h5>
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
                    <asp:GridView ID="DeductionGrid" runat="server" Width="100%" class="table table-bordered table-hover table-striped table-responsive small dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found">
                            <Columns>
                                <asp:TemplateField HeaderText="Workman" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_WorkmanSL" runat="server" Text='<%# Bind("WorkmanSL") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Advance" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Advance" runat="server" Text='<%# Bind("Advance") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="text text-center bg-primary" />
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Remaining Advance" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Rem_Advance" runat="server" Text='<%# Bind("Rem_Advance") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="text text-center bg-primary" />
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Current Advance" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_SkillDesignation" runat="server" Text='<%# Bind("Cur_Advance") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="text text-center bg-primary" />
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Total Fines" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Fathername" runat="server" Text='<%# Bind("Fines") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="text text-center bg-danger" />
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Remaining Fines" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_MobileNo" runat="server" Text='<%# Bind("Rem_Fines") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="text text-center bg-danger" />
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Current Fines" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_SkillDesignation" runat="server" Text='<%# Bind("Cur_Fines") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="text text-center bg-danger" />
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Others" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Fathername" runat="server" Text='<%# Bind("Others") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="text text-center bg-warning" />
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Remaining Others" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_MobileNo" runat="server" Text='<%# Bind("Rem_Others") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="text text-center bg-warning" />
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Current Others" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_SkillDesignation" runat="server" Text='<%# Bind("Cur_Others") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="text text-center bg-warning" />
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                            </Columns>
                            <HeaderStyle CssClass="text text-center" />
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
