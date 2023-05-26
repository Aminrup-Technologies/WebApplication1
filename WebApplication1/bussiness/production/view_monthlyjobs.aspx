<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="view_monthlyjobs.aspx.cs" Inherits="WebApplication1.bussiness.production.view_monthlyjobs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title" runat="server" visible="false">
                <div class="title_left">
                    <h5>view Monthly JOB & Other Status</h5>
                </div>

                <div class="title_right">
                    <div class="col-md-5 col-sm-5 form-group pull-right top_search"></div>
                </div>

            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12  ">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>View Monthly JOBs Summary :: Search Parameters</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row">

                                <div class="col-md-2 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <label>Select Work Region <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <asp:DropDownList ID="DDL_Region" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Region_SelectedIndexChanged"></asp:DropDownList>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <label>Select Company <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <asp:DropDownList ID="DDL_Company" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <label>Select Year <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <asp:DropDownList ID="DDL_Year" CssClass="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
                                </div>


                                <div class="col-md-2 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <label>Select Month <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <asp:DropDownList ID="DDL_Month" CssClass="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
                                    <asp:DropDownList ID="DDL_Day" CssClass="form-control form-control-sm rounded" runat="server" Visible="false"></asp:DropDownList>
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

                <div class="col-md-12 col-sm-12">
                        <asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-condensed table-sm dt-responsive nowrap small" ShowFooter="true" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowCommand="GridView1_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl" Visible="True" HeaderStyle-Width="2%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="JOB Date" HeaderStyle-Width="10%" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_CreatedDate" runat="server" Text='<%# Bind("CreatedDate","{0:dd-MM-yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Total JOB ID Created" HeaderStyle-Width="8%" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_total_count" runat="server" Text='<%# Bind("total_count") %>' DataFormatString="{0:D}" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Total Manpower" HeaderStyle-Width="8%" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_total_manpower" runat="server" Text='<%# Bind("total_manpower") %>' DataFormatString="{0:D}" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Total OT" HeaderStyle-Width="8%" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_total_overtime" runat="server" Text='<%# Bind("total_overtime") %>' DataFormatString="{0:D}" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Idle JOBS" HeaderStyle-Width="8%" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Idlejob_count" runat="server" ForeColor="Orange" Text='<%# Bind("Idlejob_count") %>' DataFormatString="{0:D}"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Open JOBS" HeaderStyle-Width="8%" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_openjobs_count" runat="server" ForeColor="Blue" Text='<%# Bind("openjobs_count") %>' DataFormatString="{0:D}"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Closed JOBS" HeaderStyle-Width="8%" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_closedjobs_count" runat="server" ForeColor="Black" Text='<%# Bind("closedjobs_count") %>' DataFormatString="{0:D}"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Pending for Approval" HeaderStyle-Width="8%" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_pending_count" runat="server" ForeColor="MediumBlue" Text='<%# Bind("pending_count") %>' DataFormatString="{0:D}"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Returned by Incharge" HeaderStyle-Width="8%" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_returned_count" runat="server" ForeColor="IndianRed" Text='<%# Bind("returned_count") %>' DataFormatString="{0:D}"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Rejected by Incharge" HeaderStyle-Width="8%" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_rejected_count" runat="server" ForeColor="Red" Text='<%# Bind("rejected_count") %>' DataFormatString="{0:D}"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Approved JOB ID's" HeaderStyle-Width="8%" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_approved_count" runat="server" ForeColor="Green" Text='<%# Bind("approved_count") %>' DataFormatString="{0:D}"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Manpower Jobs" HeaderStyle-Width="8%" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_manpowerjobs" runat="server" ForeColor="DarkViolet" Text='<%# Bind("manpowerjobs") %>' DataFormatString="{0:D}"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Line Item Jobs" HeaderStyle-Width="8%" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_lineitemjobs" runat="server" ForeColor="DarkBlue" Text='<%# Bind("lineitemjobs") %>' DataFormatString="{0:D}"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Unassigned JOBS" HeaderStyle-Width="8%" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_notassigned" runat="server" ForeColor="OrangeRed" Text='<%# Bind("notassigned") %>' DataFormatString="{0:D}"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="NON - Billing Jobs" HeaderStyle-Width="8%" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_nonbillingjobs" runat="server" ForeColor="Black" Text='<%# Bind("nonbillingjobs") %>' DataFormatString="{0:D}"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="View Details" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgBtn_ViewDetails" runat="server" CommandArgument='<%#Eval("CreatedDate") %>' CommandName="View_Details" Height="20px" ImageUrl="~/erp_images/view.svg" Width="20px"/>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text text-center" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="text text-center" />
                            <FooterStyle CssClass="text text-center font-weight-bold bg-info"  />
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
            if (document.getElementById('<%=DDL_Region.ClientID%>').selectedIndex == 0) {
                document.getElementById('<%=DDL_Region.ClientID%>').focus();
                ShowPopup("Error :", "Work Region selection required...!");
                return false;
            }

            if (document.getElementById('<%=DDL_Company.ClientID%>').selectedIndex == 0) {
                document.getElementById('<%=DDL_Company.ClientID%>').focus();
                ShowPopup("Error :", "Work Company selection required...!");
                return false;
            }

            if (document.getElementById('<%=DDL_Year.ClientID%>').selectedIndex == 0) {
                document.getElementById('<%=DDL_Year.ClientID%>').focus();
                ShowPopup("Error :", "Calender Year selection required...!");
                return false;
            }
        }
    </script>
</asp:Content>
