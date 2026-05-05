<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="jobapp_controller.aspx.cs" Inherits="WebApplication1.bussiness.production.jobapp_controller" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h5>View Blocked JOB ID -
                        <asp:Label ID="lbl_month" runat="server"></asp:Label><asp:Label ID="lbl_monthcode" Visible="false" runat="server"></asp:Label>,
                        <asp:Label ID="lbl_year" runat="server"></asp:Label></h5>
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
                <div class="col-md-12 col-sm-12  ">
                    <div class="x_panel">
                        <div class="x_title" runat="server" visible="false">
                            <h2>Search View Parameters</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row">

                                <div class="col-12 text-center">
                                    <div class="btn-group" role="group" aria-label="">
                                        <asp:Button ID="btn_prevmonth" runat="server" Text="Prev Month" CssClass="btn btn-success btn-sm" OnClick="btn_prevmonth_Click" />
                                        <asp:Button ID="btn_currentdata" runat="server" Text="Current Month" CssClass="btn btn-primary btn-sm" OnClick="btn_currentdata_Click" />
                                        <asp:Button ID="btn_nextmonth" runat="server" Text="Next Month" CssClass="btn btn-success btn-sm" OnClick="btn_nextmonth_Click" />
                                    </div>
                                </div>

                                <div class="col-12">
                                    <hr />
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-6 form-group">
                                    <label>Search By Levels<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-6 form-group">
                                    <asp:DropDownList ID="ddlViewLevel" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="ddlViewLevel_SelectedIndexChanged">
                                        <asp:ListItem Text="Region Level" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Region + Incharge" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Detailed View" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                            </div>

                            <div class="col-md-6 center-margin">
                                <div class="ln_solid"></div>
                                <div class="item form-group row">
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to Save Data!!"></asp:Label>
                                    </div>
                                    <div class="col-md-6 col-sm-12">
                                        <button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
                                        <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="btn btn-warning btn-sm" PostBackUrl="~/bussiness/production/jobapp_controller.aspx" />
                                        <asp:Button ID="btn_submit" runat="server" Text="Submit" CssClass="btn btn-success btn-sm" OnClick="btn_submit_Click" />
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="card-box col-md-12 col-sm-12 small" style="width: 100%; height: auto; overflow: scroll;">
                    <asp:GridView ID="gvPendingApprovalsold" runat="server" AutoGenerateColumns="True" Visible="false" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive" AllowPaging="True" PageSize="10"></asp:GridView>
                </div>
            </div>

            <div class="row">
                <div class="card-box col-md-12 col-sm-12 small" style="width: 100%; height: 450px; overflow: scroll;">
                    <div class="row">
                        <div class="card-box col-md-12 col-sm-12 small" style="width: 100%; height: 450px; overflow: scroll;">
                            <asp:GridView ID="gvPendingApprovals" runat="server" AutoGenerateColumns="False" Visible="false"
                                class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive"
                                AllowPaging="True" PageSize="100" DataKeyNames="Id"
                                OnRowCommand="gvPendingApprovals_RowCommand" OnPageIndexChanging="gvPendingApprovals_PageIndexChanging">
                                <Columns>
                                    <%-- Select Checkbox --%>
                                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkHeader" runat="server" onclick="toggleSelectAll(this, 'gvPendingApprovals');" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" CssClass="rowCheckbox" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%-- Core Job Identifiers --%>
                                    <asp:BoundField DataField="JOBID" HeaderText="Job ID" SortExpression="JOBID" ItemStyle-Font-Bold="true" />
                                    <asp:BoundField DataField="CreatedDate" HeaderText="Shift Created On" SortExpression="CreatedDate" DataFormatString="{0:dd-MMM-yyyy HH:mm}" />
                                    <asp:BoundField DataField="WorkOrderNo" HeaderText="Work Order" SortExpression="WorkOrderNo" />

                                    <%-- Location Context --%>
                                    <asp:BoundField DataField="JOB_Company" HeaderText="Company" SortExpression="JOB_Company" />
                                    <asp:BoundField DataField="JOB_Region" HeaderText="Region" SortExpression="JOB_Region" />
                                    <asp:BoundField DataField="JOB_Location" HeaderText="Location" SortExpression="JOB_Location" />

                                    <%-- Bottleneck Context --%>
                                    <asp:BoundField DataField="JOB_InchargeName" HeaderText="Approver (In-Charge)" SortExpression="JOB_InchargeName" ItemStyle-Font-Bold="true" />
                                    <asp:BoundField DataField="ManpowerCount" HeaderText="Manpower" SortExpression="ManpowerCount" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" />

                                    <%-- Visual Status Badge --%>
                                    <asp:TemplateField HeaderText="Current State" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <span class="badge badge-danger" style="font-size: 11px; padding: 5px;">Blocked (72h Expired)</span>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%-- Action Button --%>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Button ID="btnUnblock" runat="server" Text="Unblock (24h)" CssClass="btn btn-success btn-sm"
                                                CommandName="Unblock" CommandArgument='<%# Eval("Id") %>'
                                                OnClientClick="return confirm('Are you sure you want to unblock this job and grant the Approver a 24-Hour grace period?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                            <br />
                            <asp:Button ID="Button1" runat="server" Text="Bulk Unblock Selected (24h)" CssClass="btn btn-danger btn-sm"
                                OnClick="btnBulkUnblock_Click" OnClientClick="return confirm('Are you sure you want to unblock the selected jobs for 24 Hours?');" />
                        </div>
                    </div>
                    <asp:Button ID="btnBulkUnblock" runat="server" Text="Bulk Unblock (24h)" CssClass="btn btn-danger btn-sm" OnClick="btnBulkUnblock_Click" OnClientClick="return confirm('Are you sure you want to unblock selected jobs for 24 Hours?');" />
                </div>
            </div>

        </div>
    </div>
    <script type="text/javascript">
        function toggleSelectAll(headerCheckbox) {
            var gridView = document.getElementById('<%= gvPendingApprovals.ClientID %>');
            var checkboxes = gridView.getElementsByTagName("input");

            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].type == "checkbox" && checkboxes[i] != headerCheckbox) {
                    checkboxes[i].checked = headerCheckbox.checked;
                }
            }
        }
    </script>
</asp:Content>
