<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="vw_sopapproval.aspx.cs" Inherits="WebApplication1.bussiness.production.vw_sopapproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h5>View & Approve TBT : [<asp:Label ID="lbl_month" runat="server"></asp:Label><asp:Label ID="lbl_monthcode" Visible="false" runat="server"></asp:Label>,
						<asp:Label ID="lbl_year" runat="server"></asp:Label>]</h5>
                </div>

                <div class="title_right">
                    <div class="col-md-5 col-sm-5 form-group pull-right top_search"></div>
                </div>

            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12  ">
                    <div class="x_panel">
                        <%--<div class="x_title">
							<h2>Search View Parameters</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>--%>
                        <div class="x_content">
                            <div class="row">
                                <div class="col-md-3 col-sm-6 col-xs-6 form-group">
                                    <label>By Department <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-6 form-group">
                                    <asp:DropDownList ID="DDL_TBTDept" CssClass="form-control form-control-sm rounded" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <%--button   start--%>
                            <div class="col-md-12 center-margin">
                                <div class="ln_solid"></div>
                                <div class="row center">
                                    <div class="col-6 text-center">
                                        <div class="btn-group" role="group" aria-label="">
                                            <asp:Button ID="btn_prevmonth" runat="server" Text="Prev Month" CssClass="btn btn-success btn-sm" OnClick="btn_prevmonth_Click" />
                                            <asp:Button ID="btn_currentdata" runat="server" Text="Current Month" CssClass="btn btn-primary btn-sm" OnClick="btn_currentdata_Click" />
                                            <asp:Button ID="btn_nextmonth" runat="server" Text="Next Month" CssClass="btn btn-success btn-sm" OnClick="btn_nextmonth_Click" />
                                        </div>
                                    </div>
                                    <div class="col-md-6 text-center">
                                        <button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
                                        <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="btn btn-warning btn-sm" OnClick="btn_reset_Click" />
                                        <asp:Button ID="btn_submit" runat="server" Text="Submit" CssClass="btn btn-success btn-sm" OnClick="btn_submit_Click" />
                                    </div>
                                </div>
                            </div>
                            <%--button   end--%>
                        </div>

                    </div>
                </div>
            </div>

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
            <!-- Small Modal - END---->

            <div class="row">
                <div class="card-box col-md-12 col-sm-12 small" style="width: 100%; height: 450px; overflow: scroll;">
                    <asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowCommand="GridView1_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="1%">
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

                            <asp:TemplateField HeaderText="Date" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SOP_Date" runat="server" Text='<%# Eval("SOP_Date","{0:dd-MM-yyyy}") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="TBT ID" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Button ID="btn_viewsopdetails" runat="server" Text='<%# Eval("SOP_ID") %>' Font-Size="Smaller" CssClass="btn btn-sm btn-info" CommandName="View_Details" CommandArgument="<%# Container.DataItemIndex %>" />
                                    <asp:Label ID="lbl_SOP_ID" runat="server" Text='<%# Eval("SOP_ID") %>' Visible="false" />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="JOBID" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Button ID="btn_viewjobdetails" runat="server" Text='<%# Eval("Ref_JOBID") %>' Font-Size="Smaller" CssClass="btn btn-sm btn-primary" CommandName="View_JOBDetails" CommandArgument="<%# Container.DataItemIndex %>" />
                                    <asp:Label ID="lbl_Ref_JOBID" runat="server" Text='<%# Eval("Ref_JOBID") %>' Visible="false" />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Supervisor / Submitter Name" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SOP_SubmitterName" runat="server" Text='<%# Eval("SOP_SubmitterName") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="SOP Number" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SOPNumber" runat="server" Text='<%# Eval("SOPNumber") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="SOP Title" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SOPTitle" runat="server" Text='<%# Eval("SOPTitle") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Approval Status" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SafetySupvApprovalStatus" runat="server" Text='<%# Eval("SafetySupvApprovalStatus") %>' />
                                </ItemTemplate>
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
