<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="view_dailyjobs.aspx.cs" Inherits="WebApplication1.bussiness.production.view_dailyjobs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h5>View Daily JOB with Other Status</h5>
                </div>

                <div class="title_right">
                    <div class="col-md-5 col-sm-5 form-group pull-right top_search"></div>
                </div>

            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12 " runat="server" visible="true">
                    <div class="x_panel">
                        <div class="x_title" runat="server" visible="false">
                            <h2>Search Parameters</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row">

                                <div class="col-md-2 col-sm-6 col-xs-6 col-lg-2 form-group" id="rgnrow1" runat="server" visible="false">
                                    <label>Select Work Region <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6 col-lg-2 form-group" id="rgnrow2" runat="server" visible="false">
                                    <asp:DropDownList ID="DDL_Region" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Region_SelectedIndexChanged"></asp:DropDownList>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 col-lg-2 form-group" id="comprow1" runat="server" visible="false">
                                    <label>Select Company <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6 col-lg-2 form-group" id="comprow2" runat="server" visible="false">
                                    <asp:DropDownList ID="DDL_Company" runat="server" CssClass="form-control form-control-sm rounded" OnSelectedIndexChanged="DDL_Company_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </div>

                                <div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <label>Select Work-Site<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <asp:DropDownList ID="DDL_Worksite" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                </div>

                                <div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <label>Select Calender Date<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <asp:DropDownList ID="DDL_Day" CssClass="form-control form-control-sm rounded" runat="server" Visible="true"></asp:DropDownList>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <asp:DropDownList ID="DDL_Month" CssClass="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <asp:DropDownList ID="DDL_Year" CssClass="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
                                </div>


                            </div>

                            <%--button   start--%>
                            <div class="col-md-6 center-margin">
                                <div class="ln_solid"></div>
                                <div class="item form-group row">
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to Save Data!!"></asp:Label>
                                    </div>
                                    <div class="col-md-6 col-sm-12">
                                        <button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
                                        <button type="reset" class="btn btn-warning btn-sm">Reset</button>
                                        <asp:Button ID="btn_submit" runat="server" Text="Submit" CssClass="btn btn-success btn-sm" OnClientClick="return ValidateFormField()" OnClick="btn_submit_Click" />
                                    </div>
                                </div>
                            </div>
                            <%--button   end--%>

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
                        </div>

                    </div>
                </div>

                <div class="col-lg-12">
                    <asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-condensed table-sm dt-responsive small" ShowFooter="true" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand" OnRowDeleting="GridView1_RowDeleting">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl" Visible="True" HeaderStyle-Width="2%">
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

                            <asp:TemplateField HeaderText="JOB Date" HeaderStyle-Width="5%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_CreatedDate" runat="server" Text='<%# Bind("CreatedDate","{0:dd-MM-yyyy}") %>'></asp:Label> [<asp:Label ID="lbl_JOB_Shift" Font-Bold="true" runat="server" Text='<%# Bind("JOB_Shift") %>'></asp:Label>] Shift
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="JOBID" HeaderStyle-Width="5%" Visible="true">
                                <ItemTemplate>
                                    <asp:Button ID="btn_viewdetails" runat="server" Text='<%# Eval("JOBID") %>' Font-Size="Smaller" CssClass="btn btn-sm btn-info" CommandName="View_Details" CommandArgument="<%# Container.DataItemIndex %>" />
                                    <asp:Label ID="lbl_JOBID" runat="server" Text='<%# Eval("JOBID") %>' Visible="false" />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="JOBID Status" HeaderStyle-Width="5%" Visible="false">
                                <ItemTemplate>
                                    <asp:Button ID="btn_jobidstatus" runat="server" Text='<%# Eval("JOBID_Status") %>' Font-Size="Smaller" CssClass="btn btn-sm btn-info" CommandName="Swap_JOBIDStatus" CommandArgument="<%# Container.DataItemIndex %>" />
                                    <asp:Label ID="lbl_JOBID_Status" runat="server" Text='<%# Eval("JOBID_Status") %>' Visible="false" />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="JOB Supervisor" HeaderStyle-Width="8%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Creator_Name" runat="server" Font-Bold="true" Text='<%# Bind("Creator_Name") %>'></asp:Label> [<asp:Label ID="lbl_Creator_Workman" Font-Bold="true" runat="server" Text='<%# Eval("Creator_Workman") %>' />]
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <%--<asp:TemplateField HeaderText="DBID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Creator_Workman" Font-Bold="true" runat="server" Text='<%# Eval("Creator_Workman") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>--%>

                            <asp:TemplateField HeaderText="JOB Status" HeaderStyle-Width="2%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_EntryExit" runat="server" Text='<%# Bind("EntryExit") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="JOB Work Site & Location" HeaderStyle-Width="8%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_JOB_Site" runat="server" Font-Bold="true" Text='<%# Bind("JOB_Site") %>'></asp:Label> [<asp:Label ID="lbl_JOB_Location" runat="server" Text='<%# Bind("JOB_Location") %>'></asp:Label>]
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="JOB Site Incharge" HeaderStyle-Width="10%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_JOB_InchargeName" runat="server" Font-Bold="true" Text='<%# Bind("JOB_InchargeName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Approval Status" HeaderStyle-Width="5%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Incharge_Approval" runat="server" Font-Bold="true" Text='<%# Bind("Incharge_Approval") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="JOB Workorder - Permit no." HeaderStyle-Width="5%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_WorkOrderNo" runat="server" Text='<%# Bind("WorkOrderNo") %>'></asp:Label> [<asp:Label ID="lbl_JOB_PermitNo" runat="server" Font-Bold="true" Text='<%# Bind("JOB_PermitNo") %>'></asp:Label>]
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <%--<asp:TemplateField HeaderText="JOB Permit No" HeaderStyle-Width="5%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_JOB_PermitNo" runat="server" Font-Bold="true" Text='<%# Bind("JOB_PermitNo") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>--%>

                            <asp:TemplateField HeaderText="Upload Status" HeaderStyle-Width="5%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_FinalUpldStatus" runat="server" Font-Bold="true" Text='<%# Bind("FinalUpldStatus") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <%--<asp:TemplateField HeaderText="JOB Location" HeaderStyle-Width="5%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_JOB_Location" runat="server" Text='<%# Bind("JOB_Location") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>--%>

                            <asp:TemplateField HeaderText="JOB Title" HeaderStyle-Width="15%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_JOB_Title" runat="server" Text='<%# Eval("JOB_Title").ToString().Length > 50? (Eval("JOB_Title") as string).Substring(0,50) + " ..." : Eval("JOB_Title")  %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <%--<asp:TemplateField HeaderText="JOB Shift" HeaderStyle-Width="2%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_JOB_Shift" Font-Bold="true" runat="server" Text='<%# Bind("JOB_Shift") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>--%>

                            <asp:TemplateField HeaderText="Manpower" HeaderStyle-Width="2%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_ManpowerCount" runat="server" Text='<%# Bind("ManpowerCount") %>' Font-Bold="true" ForeColor="Black"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Billing Type" HeaderStyle-Width="5%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_BillingType" runat="server" Text='<%# Bind("BillingType") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Billing Type" HeaderStyle-Width="2%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_BillingCode" runat="server" Text='<%# Bind("BillingCode") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Atten Code" HeaderStyle-Width="2%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_AttendanceCode" Font-Bold="true" runat="server" Text='<%# Bind("AttendanceCode") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="2%" Visible="true">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btndelete" runat="server" CommandName="Delete" CommandArgument="<%# Container.DataItemIndex %>" Height="15px" ImageUrl="~/erp_images/fi-sr-trash.svg" Width="15px" ToolTip="Delete" OnClientClick="return confirm('Do you want to DELETE...?')" ImageAlign="Middle" />
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

        function ValidateFormField() {

            if (document.getElementById('<%=DDL_Year.ClientID%>').selectedIndex == 0) {
                document.getElementById('<%=DDL_Year.ClientID%>').focus();
                ShowPopup("Error :", "Calender Year selection required...!");
                return false;
            }
        }
    </script>
</asp:Content>
