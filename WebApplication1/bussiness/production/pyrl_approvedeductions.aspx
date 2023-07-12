<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="pyrl_approvedeductions.aspx.cs" Inherits="WebApplication1.bussiness.production.pyrl_approvedeductions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h5>View Active Deductions</h5>
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
                <div class="card-box col-md-12 col-sm-12" style="width: 100%; height: 450px; overflow: scroll;">
                    <asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found">
                        <Columns>
                            <asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="DBID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="DBID" HeaderStyle-Width="10%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_WorkmanSL" runat="server" Text='<%# Eval("WorkmanSL") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Full Name" HeaderStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_FullName" runat="server" Text='<%# Eval("FullName") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center small" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Skill Category" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SkillCategory" runat="server" Text='<%# Eval("SkillCategory") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center small" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Skill Designation" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SkillDesignation" runat="server" Text='<%# Eval("SkillDesignation") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center small" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Present" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Present" runat="server" Text='<%# Eval("Present") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center small" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Over Time" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_OverTime" runat="server" Text='<%# Eval("OverTime") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center small" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Basic Salary" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_BasicSalary" runat="server" Text='<%# Eval("BasicSalary") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center small" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Fixed Rate Salary" HeaderStyle-Width="15%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_FixedRateSalary" runat="server" Text='<%# Eval("FixedRateSalary") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center small" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Actual Gross" HeaderStyle-Width="15%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_ActualGross" runat="server" Text='<%# Eval("ActualGross") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center small" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="ESIC Gross" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_ESICGross" runat="server" Text='<%# Eval("ESICGross") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center small" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="PF Pay" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_PFPay" runat="server" Text='<%# Eval("PFPay") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center small" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="ESIC Pay" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_ESICPay" runat="server" Text='<%# Eval("ESICPay") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center small" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Net Pay1" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_NetPay1" runat="server" Text='<%# Eval("NetPay1") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center small" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Advance" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Approved_Advance" runat="server" Font-Bold="true" Text='<%# Eval("Approved_Advance") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Rem Advance" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Remaining_Advance" runat="server" Text='<%# Eval("Remaining_Advance") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Cur Advance" HeaderStyle-Width="3%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_CurrentMonth_Advance" runat="server" Text='<%# Eval("CurrentMonth_Advance") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Deducted Advance" HeaderStyle-Width="3%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Deducted_Advance" runat="server" Text='<%# Eval("Deducted_Advance") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" BackColor="OrangeRed" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Approved Fines" HeaderStyle-Width="7%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Approved_Fines" runat="server" Text='<%# Eval("Approved_Fines") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Remaining Fines" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Remaining_Fines" runat="server" Font-Bold="true" Text='<%# Eval("Remaining_Fines") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="CurrentMonth Fines" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_CurrentMonth_Fines" runat="server" Text='<%# Eval("CurrentMonth_Fines") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Deducted Fines" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Deducted_Fines" runat="server" Text='<%# Eval("Deducted_Fines") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" BackColor="OrangeRed" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Approved Others" HeaderStyle-Width="7%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Approved_Others" runat="server" Text='<%# Eval("Approved_Others") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Remaining Others" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Remaining_Others" runat="server" Font-Bold="true" Text='<%# Eval("Remaining_Others") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="CurrentMonth Others" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_CurrentMonth_Others" runat="server" Text='<%# Eval("CurrentMonth_Others") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Deducted Others" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Deducted_Others" runat="server" Text='<%# Eval("Deducted_Others") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" BackColor="OrangeRed" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Total Deduction" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_TotalDeduction" runat="server" Text='<%# Eval("TotalDeduction") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" BackColor="IndianRed" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="NetPay Final" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_NetPayFinal" runat="server" Text='<%# Eval("NetPayFinal") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" BackColor="LightGreen" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Net Pay2" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_NetPay2" runat="server" Text='<%# Eval("NetPay2") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" BackColor="LightGreen" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Total Pay" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Total_Pay" runat="server" Text='<%# Eval("Total_Pay") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" BackColor="Green" />
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
