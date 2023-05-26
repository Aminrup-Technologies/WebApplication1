<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="vw_monthlyatten.aspx.cs" Inherits="WebApplication1.bussiness.production.vw_monthlyatten" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title" id="toprow" runat="server" visible="false">
                <div class="title_left">
                    <h5>View Monthly Attendance :
						<asp:Label ID="lbl_month" runat="server"></asp:Label><asp:Label ID="lbl_monthcode" Visible="false" runat="server"></asp:Label>,
						<asp:Label ID="lbl_year" runat="server"></asp:Label></h5>
                </div>

                <div class="title_right">
                    <div class="col-md-5 col-sm-5 form-group pull-right top_search"></div>
                </div>

            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12  ">
                    <div class="x_panel">
                        <div class="x_content">
                            <div class="row">

                                <div class="col-12 text-center">
                                    <div class="btn-group" role="group" aria-label="">
                                        <asp:Button ID="btn_prevmonth" runat="server" Text="Prev Month" CssClass="btn btn-success btn-sm" OnClick="btn_prevmonth_Click" />
                                        <asp:Button ID="btn_currentdata" runat="server" Text="Current Month" CssClass="btn btn-primary btn-sm" OnClick="btn_currentdata_Click" />
                                        <asp:Button ID="btn_nextmonth" runat="server" Text="Next Month" CssClass="btn btn-success btn-sm" OnClick="btn_nextmonth_Click" />
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

                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-6 col-sm-6 profile_details">
                    <div class="well profile_view col-sm-12 col-lg-12">
                        <div class="col-sm-12">
                            <h4 class="brief"><i>Attendance :
							<asp:Label ID="lbl_calmonth" runat="server" Text="0"></asp:Label>,
							<asp:Label ID="lbl_calyear" runat="server" Text="0"></asp:Label></i></h4>

                            <div class="right col-md-6 col-sm-6 text-center">
                                <asp:Label ID="lbl_totalpresent" runat="server" Text="0" ForeColor="Green" Font-Size="90px"></asp:Label>
                                /
							<asp:Label ID="lbl_caldays" runat="server" Font-Size="Medium" Font-Bold="true" ForeColor="Black" Text="0"></asp:Label>
                            </div>
                            <div class="left col-md-6 col-sm-6">
                                <h2>Days Worked :
								<asp:Label ID="lbl_dayswrkd" runat="server" Text="0" Font-Bold="true"></asp:Label></h2>
                                <span><strong style="color: green;">P : </strong>
                                    <asp:Label ID="lbl_presentdayscount" runat="server" Text="0" Font-Bold="true"></asp:Label>
                                </span>&nbsp;|&nbsp;
							<span><strong style="color: darkorange;">OD : </strong>
                                <asp:Label ID="lbl_oddayscount" runat="server" Text="0" Font-Bold="true"></asp:Label>
                            </span>&nbsp;|&nbsp;
							<span><strong style="color: darkblue;">NH : </strong>
                                <asp:Label ID="lbl_nhcount" runat="server" Text="0" Font-Bold="true"></asp:Label>
                            </span>&nbsp;|&nbsp;
							<span><strong style="color: blue;">FL : </strong>
                                <asp:Label ID="lbl_flcount" runat="server" Text="0" Font-Bold="true"></asp:Label>
                            </span>
                                <hr />
                                <h2>Total OT :
								<asp:Label ID="lbl_totalot" runat="server" Text="32" ForeColor="Brown" Font-Bold="true"></asp:Label>
                                    Hours</h2>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-6 col-sm-6 profile_details" id="PaymentView" runat="server" visible="false">
                    <div class="well profile_view col-sm-12 col-lg-12">
                        <div class="col-sm-12">
                            <h4 class="brief"><i>Payment :
							<asp:Label ID="lbl_paymonth" runat="server" Text="0"></asp:Label>,
							<asp:Label ID="lbl_payyear" runat="server" Text="0"></asp:Label></i> <span id="realtime" runat="server" visible="false">(Real Time)</span> <span id="finalized" runat="server" visible="false">(Finalized)</span></h4>

                            <div class="right col-md-4 col-sm-6 text-center">
                                ₹.
								<asp:Label ID="lbl_grosspay" runat="server" Text="0" ForeColor="Green" Font-Size="30px" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="left col-md-8 col-sm-6">
                                <h2>Pay 1 :₹
								<asp:Label ID="lbl_netpay" runat="server" Text="0" Font-Bold="true"></asp:Label>
                                    || Pay 2:₹
								<asp:Label ID="lbl_netpay2" runat="server" Text="0" Font-Bold="true"></asp:Label></h2>
                                <span><strong style="color: green;">Basic : </strong>₹
									<asp:Label ID="lbl_basic" runat="server" Text="0" Font-Bold="true"></asp:Label>
                                </span>&nbsp;|&nbsp;
							<span><strong style="color: darkorange;">OT : </strong>₹
								<asp:Label ID="lbl_otpay" runat="server" Text="0" Font-Bold="true"></asp:Label>
                            </span>&nbsp;|&nbsp;
							<span><strong style="color: darkblue;">Allow. : </strong>₹
								<asp:Label ID="lbl_allowances" runat="server" Text="0" Font-Bold="true"></asp:Label>
                            </span>
                                <hr />
                                <h2>Total Deductions :
								<asp:Label ID="lbl_ttldeductions" runat="server" Text="0" ForeColor="Brown" Font-Bold="true"></asp:Label></h2>
                                <span><strong style="color: blue;">PF : </strong>₹
								<asp:Label ID="lbl_pfpay" runat="server" Text="0" Font-Bold="true"></asp:Label>
                                </span>&nbsp;|&nbsp;
								<span><strong style="color: darkorange;">ESIC : </strong>₹
								<asp:Label ID="lbl_esicpay" runat="server" Text="0" Font-Bold="true"></asp:Label>
                                </span>&nbsp;|&nbsp;
								<span><strong style="color: darkorange;">Other : </strong>₹
								<asp:Label ID="lbl_advance" runat="server" Text="0" Font-Bold="true"></asp:Label>
                                </span>
                            </div>

                            <div></div>
                            <div id="salary_pdfrow" runat="server" visible="false">
                                <asp:Button runat="server" Text="Pay Slip" ID="btnExport" CssClass="btn btn-primary btn-sm" OnClick="btnExport_Click" />
                            </div>

                        </div>
                    </div>
                </div>

            </div>

            <div class="row">
                <div class="card-box col-md-12 col-sm-12 small" style="width: 100%; height: 100%; overflow: scroll;">
                    <asp:GridView ID="GridView1" runat="server" Width="100%" CssClass="table table-striped table-hover table-bordered table-sm dt-responsive" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl" Visible="True" HeaderStyle-Width="1%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Id" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Date" Visible="true" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_CreatedDate" runat="server" Text='<%# Bind("CreatedDate","{0:dd-MM-yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="JOBID" Visible="true" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_JOBID" runat="server" Text='<%# Bind("JOBID") %>' Visible="true" />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Supervisor Name" HeaderStyle-Width="12%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Creator_Name" runat="server" Text='<%# Bind("Creator_Name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Approver" HeaderStyle-Width="12%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_JOB_InchargeName" runat="server" Text='<%# Bind("JOB_InchargeName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="IN-Punch Time" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Inpunch_Time" runat="server" Text='<%# Bind("Inpunch_Time","{0:dd-MM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="OUT-Punch Time" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Outpunch_Time" runat="server" Text='<%# Bind("Outpunch_Time","{0:dd-MM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Worked Minutes" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_WorkedTime" runat="server" Text='<%# Bind("WorkedTime") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="W" HeaderStyle-Width="1%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_WorkedHours" runat="server" Text='<%# Bind("WorkedHours") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Lunch (Y/N)" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_LunchFactor" runat="server" Text='<%# Bind("LunchFactor") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="C OT" HeaderStyle-Width="2%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Calc_OT" runat="server" Text='<%# Bind("Calc_OT") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="P OT" HeaderStyle-Width="4%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_ProvidedOT" runat="server" Text='<%# Bind("ProvidedOT") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="A Status" HeaderStyle-Width="3%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_AttendanceStatus" runat="server" Text='<%# Bind("AttendanceStatus") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="A Code" HeaderStyle-Width="3%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_AttendanceCode" runat="server" Text='<%# Bind("AttendanceCode") %>'></asp:Label>
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

            <br />
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
