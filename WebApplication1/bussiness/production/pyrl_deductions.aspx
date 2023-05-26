<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="pyrl_deductions.aspx.cs" Inherits="WebApplication1.bussiness.production.pyrl_deductions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="container">
			<div class="page-title">
				<div class="title_left">
					<h3>Add / Manage Deductions</h3>
				</div>
			</div>
			<div class="clearfix"></div>

			<div class="row">
				<div class="col-md-12 col-sm-12">
					<div class="x_panel">
						<div class="x_content">
							<div class="novalidate">
								<div class="field item form-group">
									<label class="col-form-label col-md-3 col-sm-3 label-align">Name / Workman SL<span class="required">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:DropDownList ID="DDL_SearchType" runat="server" class="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_SearchType_SelectedIndexChanged">
											<asp:ListItem Selected="True" Value="0">--Select--</asp:ListItem>
											<asp:ListItem Value="1" Enabled="false">By Employee Name</asp:ListItem>
											<asp:ListItem Value="2">By Workman SL</asp:ListItem>
										</asp:DropDownList>
									</div>
								</div>

								<div class="field item form-group" id="Nameinputrow" runat="server" visible="false">
									<label class="col-form-label col-md-3 col-sm-3 label-align">Employee Name<span class="required">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:TextBox ID="txt_empname" class="form-control form-control-sm rounded" runat="server" placeholder="Enter Employee Name"></asp:TextBox>
									</div>
								</div>

								<div class="field item form-group" id="WorkmanInput_Row" runat="server" visible="false">
									<label class="col-form-label col-md-3 col-sm-3 label-align">Employee Workman<span class="required">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:TextBox ID="txt_empworkman" class="form-control form-control-sm rounded" runat="server" placeholder="Enter Employee Workman"></asp:TextBox>
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


							<%--ADD button start--%>
							<div class="col-md-6 center-margin">
								<div class="ln_solid"></div>
								<div class="item form-group row">
									<div class="col-md-6 col-sm-12">
										<asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to ADD Data!!"></asp:Label>
									</div>
									<div class="col-md-6 col-sm-12">
										<asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="btn btn-danger btn-sm" OnClick="btn_cancel_Click" />
										<asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="btn btn-warning btn-sm" OnClick="btn_reset_Click" />
										<asp:Button ID="btn_search" runat="server" Text="SEARCH" CssClass="btn btn-success btn-sm" OnClick="btn_search_Click" />
									</div>
								</div>
							</div>
							<%--ADD button end--%>
						</div>
					</div>
				</div>
			</div>

			<div class="row">
				<div class="col-md-5 col-sm-6 profile_details">
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

				<div class="col-md-7 col-sm-6 profile_details">
					<div class="well profile_view col-sm-12 col-lg-12">
						<div class="col-sm-12">
							<h4 class="brief"><i>Payment :
							<asp:Label ID="lbl_paymonth" runat="server" Text="0"></asp:Label>,
							<asp:Label ID="lbl_payyear" runat="server" Text="0"></asp:Label></i> <span id="realtime" runat="server" visible="false">(Real Time)</span> <span id="finalized" runat="server" visible="false">(Finalized)</span></h4>

							<%--<div class="right col-md-4 col-sm-6 text-center">
								₹.
								<asp:Label ID="lbl_grosspay" runat="server" Text="0" ForeColor="Green" Font-Size="30px" Font-Bold="true"></asp:Label>
							</div>--%>
							<div class="left col-md-12 col-sm-6">
								<h2>Total Earnings :
								<span><strong style="color: blue;">Actual Gross : </strong><asp:Label ID="lbl_ttlactlgros" runat="server" Text="0" ForeColor="Brown" Font-Bold="true"></asp:Label></span>&nbsp;|&nbsp;
								<span><strong style="color: blue;">ESIC Gross : </strong><asp:Label ID="lbl_esic4gross" runat="server" Text="0" ForeColor="Brown" Font-Bold="true"></asp:Label></span>
								</h2>
								<span><strong style="color: blue;">Basic : </strong>₹<asp:Label ID="lbl_basic" runat="server" Text="0" Font-Bold="true"></asp:Label></span>&nbsp;|&nbsp;
								<span><strong style="color: orange;">OT : </strong>₹<asp:Label ID="lbl_otpay" runat="server" Text="0" Font-Bold="true"></asp:Label></span>&nbsp;|&nbsp;
								<Span><strong style="color: darkblue;">Fixed Salary : </strong><asp:Label ID="lbl_fxdsalry" runat="server" Text="0" Font-Bold="true"></asp:Label></Span>&nbsp;|&nbsp;
								<Span><strong style="color: black;">Gross Actual : </strong><asp:Label ID="lbl_actualgross" runat="server" Text="0" Font-Bold="true"></asp:Label></Span>&nbsp;|&nbsp;
								<Span><strong style="color: blue;">Gross ESIC : </strong><asp:Label ID="lbl_esicgros" runat="server" Text="0" Font-Bold="true"></asp:Label></Span>&nbsp;|&nbsp;
								<br />
								<Span><strong style="color: green;">Actual NetPay 1 : </strong><asp:Label ID="lbl_netpayb4" runat="server" Text="0" Font-Bold="true"></asp:Label></Span>&nbsp;|&nbsp;
								<Span><strong style="color: green;">Actual NetPay 2 : </strong><asp:Label ID="lbl_netpay2b4" runat="server" Text="0" Font-Bold="true"></asp:Label></Span>&nbsp;||&nbsp;
								<Span><strong style="color: blue;">Other Pay : </strong><asp:Label ID="lbl_otherpay" runat="server" Text="0" Font-Bold="true"></asp:Label></Span>
								<br />
								<Span><strong style="color: green;">DA/VDA 1 : </strong><asp:Label ID="lbl_davdapay" runat="server" Text="0" Font-Bold="true"></asp:Label></Span>&nbsp;|&nbsp;
								<Span><strong style="color: green;">HRA : </strong><asp:Label ID="lbl_hrapay" runat="server" Text="0" Font-Bold="true"></asp:Label></Span>&nbsp;|&nbsp;
								<Span><strong style="color: blue;">Conv. : </strong><asp:Label ID="lbl_convpay" runat="server" Text="0" Font-Bold="true"></asp:Label></Span>&nbsp;|&nbsp;
								<Span><strong style="color: green;">Med. : </strong><asp:Label ID="lbl_medpay" runat="server" Text="0" Font-Bold="true" Visible="true"></asp:Label></Span>&nbsp;|&nbsp;
								<Span><strong style="color: green;">Wash : </strong><asp:Label ID="lbl_washpay" runat="server" Text="0" Font-Bold="true" Visible="true"></asp:Label></Span>&nbsp;|&nbsp;
								<Span><strong style="color: blue;">Att. : </strong><asp:Label ID="lbl_attpay" runat="server" Text="0" Font-Bold="true" Visible="true"></asp:Label></Span>&nbsp;|&nbsp;
								<Span><strong style="color: blue;">SPCL. : </strong><asp:Label ID="lbl_spclway" runat="server" Text="0" Font-Bold="true" Visible="true"></asp:Label></Span>&nbsp;|&nbsp;
								<Span><strong style="color: blue;">Misc. : </strong><asp:Label ID="lbl_miscpay" runat="server" Text="0" Font-Bold="true" Visible="true"></asp:Label></Span>
								<hr />
								<h2>Total Deductions :
								<asp:Label ID="lbl_ttldeductions" runat="server" Text="0" ForeColor="Brown" Font-Bold="true"></asp:Label></h2>
								<span><strong style="color: blue;">PF : </strong>₹<asp:Label ID="lbl_pfpay" runat="server" Text="0" Font-Bold="true"></asp:Label></span>&nbsp;|&nbsp;
								<span><strong style="color: darkorange;">ESIC : </strong>₹<asp:Label ID="lbl_esicpay" runat="server" Text="0" Font-Bold="true"></asp:Label></span>&nbsp;|&nbsp;
								<span><strong style="color: darkorange;">Advance : </strong>₹<asp:Label ID="lbl_advance" runat="server" Text="0" Font-Bold="true"></asp:Label></span>&nbsp;|---|&nbsp;
								<Span><strong style="color: green;">Final NetPay 1 : </strong><asp:Label ID="lbl_netpayfnl" runat="server" Text="0" Font-Bold="true" Font-Size="Medium"></asp:Label></Span>&nbsp;|&nbsp;
								<Span><strong style="color: green;">Final NetPay 2 : </strong><asp:Label ID="lbl_netpay2fnl" runat="server" Text="0" Font-Bold="true" Font-Size="Medium"></asp:Label></Span>&nbsp;|&nbsp;
							</div>
						</div>
					</div>
				</div>

			</div>

			<div class="row" id="deduction_panel" runat="server" visible="false">
				<div class="col-md-12 col-sm-12">
					<div class="card-box table-responsive">
						<asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found">
							<Columns>
								<asp:TemplateField HeaderText="Sl. NO" Visible="True" HeaderStyle-Width="5%">
									<ItemTemplate>
										<asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
									</ItemTemplate>
									<ItemStyle CssClass="text text-center" />
								</asp:TemplateField>

								<asp:TemplateField HeaderText="EMP ID" HeaderStyle-Width="5%">
									<ItemTemplate>
										<asp:Label ID="lbl_WorkmanSL" runat="server" Text='<%# Bind("WorkmanSL") %>' />
									</ItemTemplate>
									<ItemStyle CssClass="text text-center" />
								</asp:TemplateField>

								<asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="10%">
									<ItemTemplate>
										<asp:Label ID="lbl_FullName" runat="server" Text='<%# Bind("FullName") %>' />
									</ItemTemplate>
									<ItemStyle CssClass="text text-center" />
								</asp:TemplateField>

								<asp:TemplateField HeaderText="Father Name" HeaderStyle-Width="10%">
									<ItemTemplate>
										<asp:Label ID="lbl_Fathername" runat="server" Text='<%# Bind("Fathername") %>' />
									</ItemTemplate>
									<ItemStyle CssClass="text text-center" />
								</asp:TemplateField>

								<asp:TemplateField HeaderText="Mobile" HeaderStyle-Width="10%">
									<ItemTemplate>
										<asp:Label ID="lbl_MobileNo" runat="server" Text='<%# Bind("MobileNo") %>' />
									</ItemTemplate>
									<ItemStyle CssClass="text text-center" />
								</asp:TemplateField>

								<asp:TemplateField HeaderText="Designation" HeaderStyle-Width="10%">
									<ItemTemplate>
										<asp:Label ID="lbl_SkillDesignation" runat="server" Text='<%# Bind("SkillDesignation") %>' />
									</ItemTemplate>
									<ItemStyle CssClass="text text-center" />
								</asp:TemplateField>

								<asp:TemplateField HeaderText="Skill Category" HeaderStyle-Width="10%">
									<ItemTemplate>
										<asp:Label ID="lbl_SkillCategory" runat="server" Text='<%# Eval("SkillCategory") %>' />
									</ItemTemplate>
									<HeaderStyle CssClass="GridHeaderText-Center" />
									<ItemStyle CssClass="grid" />
								</asp:TemplateField>

								<asp:TemplateField HeaderText="F 16" HeaderStyle-Width="5%">
									<ItemTemplate>
										<asp:Label ID="lbl_F16_YesNo" runat="server" Text='<%# Eval("F16_YesNo") %>' />
									</ItemTemplate>
									<HeaderStyle CssClass="GridHeaderText-Center" />
									<ItemStyle CssClass="grid" />
								</asp:TemplateField>

								<asp:TemplateField HeaderText="Fixed Y/N" HeaderStyle-Width="5%" Visible="true">
									<ItemTemplate>
										<asp:Label ID="lbl_FixedSalary_YesNo" runat="server" Text='<%# Bind("FixedSalary_YesNo") %>'></asp:Label>
									</ItemTemplate>
									<HeaderStyle CssClass="grid" />
									<ItemStyle CssClass="grid" />
								</asp:TemplateField>

								<asp:TemplateField HeaderText="Fixed Salary" HeaderStyle-Width="5%" Visible="true">
									<ItemTemplate>
										<asp:Label ID="lbl_FixedAmount" runat="server" Text='<%# Bind("FixedAmount") %>'></asp:Label>
									</ItemTemplate>
									<HeaderStyle CssClass="grid" />
									<ItemStyle CssClass="grid" />
								</asp:TemplateField>

								<asp:TemplateField HeaderText="WH" HeaderStyle-Width="3%" Visible="true">
									<ItemTemplate>
										<asp:Label ID="lbl_WorkHours" runat="server" Text='<%# Bind("WorkHours") %>'></asp:Label>
									</ItemTemplate>
									<HeaderStyle CssClass="grid" />
									<ItemStyle CssClass="grid" />
								</asp:TemplateField>

								<asp:TemplateField HeaderText="OT F" HeaderStyle-Width="3%" Visible="true">
									<ItemTemplate>
										<asp:Label ID="lbl_OTFactor" runat="server" Text='<%# Bind("OTFactor") %>'></asp:Label>
									</ItemTemplate>
									<HeaderStyle CssClass="grid" />
									<ItemStyle CssClass="grid" />
								</asp:TemplateField>

								<asp:TemplateField HeaderText="OT M" HeaderStyle-Width="3%" Visible="true">
									<ItemTemplate>
										<asp:Label ID="lbl_OTMultiplier" runat="server" Text='<%# Bind("OTMultiplier") %>'></asp:Label>
									</ItemTemplate>
									<HeaderStyle CssClass="grid" />
									<ItemStyle CssClass="grid" />
								</asp:TemplateField>

								<asp:TemplateField HeaderText="OT Div" HeaderStyle-Width="3%" Visible="true">
									<ItemTemplate>
										<asp:Label ID="lbl_OT_Divisibility" runat="server" Text='<%# Bind("OT_Divisibility") %>'></asp:Label>
									</ItemTemplate>
									<HeaderStyle CssClass="grid" />
									<ItemStyle CssClass="grid" />
								</asp:TemplateField>

								<asp:TemplateField HeaderText="DA / VDA" HeaderStyle-Width="2%" Visible="true">
									<ItemTemplate>
										<asp:Label ID="lbl_DA_VDA" runat="server" Text='<%# Bind("DA_VDA") %>'></asp:Label>
									</ItemTemplate>
									<HeaderStyle CssClass="grid" />
									<ItemStyle CssClass="grid" />
								</asp:TemplateField>

								<asp:TemplateField HeaderText="HRA" HeaderStyle-Width="5%" Visible="true">
									<ItemTemplate>
										<asp:Label ID="lbl_HRA" runat="server" Text='<%# Bind("HRA") %>'></asp:Label>
									</ItemTemplate>
									<HeaderStyle CssClass="grid" />
									<ItemStyle CssClass="grid" />
								</asp:TemplateField>

								<asp:TemplateField HeaderText="Conv." HeaderStyle-Width="5%" Visible="true">
									<ItemTemplate>
										<asp:Label ID="lbl_Conv_Allowance" runat="server" Text='<%# Bind("Conv_Allowance") %>'></asp:Label>
									</ItemTemplate>
									<HeaderStyle CssClass="grid" />
									<ItemStyle CssClass="grid" />
								</asp:TemplateField>

								<asp:TemplateField HeaderText="Medical" HeaderStyle-Width="5%" Visible="true">
									<ItemTemplate>
										<asp:Label ID="lbl_Medical_Allowance" runat="server" Text='<%# Bind("Medical_Allowance") %>'></asp:Label>
									</ItemTemplate>
									<HeaderStyle CssClass="grid" />
									<ItemStyle CssClass="grid" />
								</asp:TemplateField>

								<asp:TemplateField HeaderText="ATT" HeaderStyle-Width="5%" Visible="true">
									<ItemTemplate>
										<asp:Label ID="lbl_ATT_Allowance" runat="server" Text='<%# Bind("ATT_Allowance") %>'></asp:Label>
									</ItemTemplate>
									<HeaderStyle CssClass="grid" />
									<ItemStyle CssClass="grid" />
								</asp:TemplateField>

								<asp:TemplateField HeaderText="SPCL" HeaderStyle-Width="5%" Visible="true">
									<ItemTemplate>
										<asp:Label ID="lbl_SPCL_Allowance" runat="server" Text='<%# Bind("SPCL_Allowance") %>'></asp:Label>
									</ItemTemplate>
									<HeaderStyle CssClass="grid" />
									<ItemStyle CssClass="grid" />
								</asp:TemplateField>

								<asp:TemplateField HeaderText="Misc" HeaderStyle-Width="5%" Visible="true">
									<ItemTemplate>
										<asp:Label ID="lbl_Misc_Earnings" runat="server" Text='<%# Bind("Misc_Earnings") %>'></asp:Label>
									</ItemTemplate>
									<HeaderStyle CssClass="grid" />
									<ItemStyle CssClass="grid" />
								</asp:TemplateField>

								<asp:TemplateField HeaderText="Wash" HeaderStyle-Width="5%" Visible="true">
									<ItemTemplate>
										<asp:Label ID="lbl_Washing_Allowance" runat="server" Text='<%# Bind("Washing_Allowance") %>'></asp:Label>
									</ItemTemplate>
									<HeaderStyle CssClass="grid" />
									<ItemStyle CssClass="grid" />
								</asp:TemplateField>

							</Columns>
							<HeaderStyle CssClass="text text-center" />
							<EmptyDataTemplate>
								<div class="grid">No Data Found</div>
							</EmptyDataTemplate>
						</asp:GridView>

					</div>
				</div>

				<div class="col-md-12 col-sm-12">
					<div class="card-box table-responsive">
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


				<%--ADD button start--%>
				<div class="col-md-6 center-margin" id="deduction_vwbtns" runat="server" visible="false">
					<div class="ln_solid"></div>
					<div class="item form-group row">
						<div class="col-md-6 col-sm-12">
							<asp:Label ID="Label1" runat="server" Text="Click to VIEW / ADD Data!!"></asp:Label>
						</div>
						<div class="col-md-6 col-sm-12">
							<button type="button" class="btn btn-primary btn-sm" id="btnShowPopup1" data-toggle="modal" data-target="#myModal">
								ADVACE
							</button>
							<button type="button" class="btn btn-danger btn-sm" id="btnShowPopup2" data-toggle="modal" data-target="#myModal2">
								FINES
							</button>
							<button type="button" class="btn btn-warning btn-sm" id="btnShowPopup3" data-toggle="modal" data-target="#myModal3">
								OTHERS
							</button>

							<button type="button" class="btn btn-danger btn-sm" id="btn_Logout" data-toggle="modal" data-target="#MyModal_DedDelete">CLEAR DEDUCTIONS</button>
						</div>
					</div>
				</div>
				<%--ADD button end--%>
			</div>
		</div>

		<%--- Logout Modal --%>
		<div class="modal fade" id="MyModal_DedDelete" data-backdrop="static">
			<div class="modal-dialog">
				<div class="modal-content">
					<div class="modal-header">
						<h4 class="modal-title">Confirm your DELETE</h4>
						<button type="button" class="close" data-dismiss="modal">&times;</button>
					</div>
					<div class="modal-body">
						<div class="row">
							<div class="col-md-12">
								<div class="form-group">
									<div class="col-md-6 col-sm-12 form-group">
										<label><span class="text text-danger">Click to CLEAR all Deductions</span></label>
									</div>
									<div class="col-md-6 col-sm-12 form-group">
										<asp:Button ID="btn_yes" runat="server" CausesValidation="false" Text="Yes" CssClass="btn btn-danger btn-sm" OnClick="btn_yes_Click" />
										<button type="button" class="btn btn-sm btn-success" data-dismiss="modal">No</button>
									</div>
								</div>
							</div>
						</div>
					</div>
					<div class="modal-footer">
						<button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
					</div>
				</div>
			</div>
		</div>
		<%--- Logout Modal -------- END --%>

		<div class="modal fade" id="myModal" data-backdrop="static">
			<div class="modal-dialog">
				<div class="modal-content">
					<div class="modal-header">
						<h4 class="modal-title">Add & Manage Advance Deductions</h4>
						<button type="button" class="close" data-dismiss="modal">&times;</button>
					</div>
					<div class="modal-body">
						<div class="row">
							<div class="col-md-12">
								<div class="form-group">
									<div class="col-md-6 col-sm-12 form-group">
										<label>Enter Total Advance Amount :<span class="text text-danger"></span></label>
									</div>
									<div class="col-md-6 col-sm-12 form-group">
										<asp:TextBox ID="txt_advanceamnt" runat="server" TextMode="Number" class="form-control form-control-sm rounded" ReadOnly="false"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RFV1" runat="server" ValidationGroup="ADVANCE" Display="Dynamic" ForeColor="Red" ControlToValidate="txt_advanceamnt" ErrorMessage="**"></asp:RequiredFieldValidator>
									</div>

									<div class="col-md-6 col-sm-12 form-group">
										<label>Enter Remaining Amount :<span class="text text-danger"></span></label>
									</div>
									<div class="col-md-6 col-sm-12 form-group">
										<asp:TextBox ID="txt_remamnt" runat="server" TextMode="Number" class="form-control form-control-sm rounded" ReadOnly="false"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RFV2" runat="server" ValidationGroup="ADVANCE" Display="Dynamic" ForeColor="Red" ErrorMessage="**" ControlToValidate="txt_remamnt"></asp:RequiredFieldValidator>
									</div>

									<div class="col-md-6 col-sm-12 form-group">
										<label>Enter Current Deduction :<span class="text text-danger"></span></label>
									</div>
									<div class="col-md-6 col-sm-12 form-group">
										<asp:TextBox ID="txt_curradvamnt" runat="server" TextMode="Number" class="form-control form-control-sm rounded" ReadOnly="false"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RFV3" runat="server" ValidationGroup="ADVANCE" ErrorMessage="**" ControlToValidate="txt_curradvamnt"></asp:RequiredFieldValidator>
									</div>
								</div>
								<asp:Label ID="lblMessage1" runat="server"></asp:Label>
							</div>
						</div>
					</div>
					<div class="modal-footer">
						<asp:Button ID="btn_svadvance" runat="server" Text="Save Changes" CssClass="btn btn-sm btn-primary" OnClick="btn_svadvance_Click" />
						<button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">Close</button>
					</div>
				</div>
			</div>
		</div>


		<div class="modal fade" id="myModal2" data-backdrop="static">
			<div class="modal-dialog">
				<div class="modal-content">
					<div class="modal-header">
						<h4 class="modal-title">Add & Manage Fines</h4>
						<button type="button" class="close" data-dismiss="modal">&times;</button>
					</div>
					<div class="modal-body">
						<div class="row">
							<div class="col-md-12">
								<div class="form-group">
									<div class="col-md-6 col-sm-12 form-group">
										<label>Total Fine Amount :<span class="text text-danger"></span></label>
									</div>
									<div class="col-md-6 col-sm-12 form-group">
										<asp:TextBox ID="txt_totalfine" runat="server" TextMode="Number" class="form-control form-control-sm rounded" ReadOnly="false"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="ADVANCE" Display="Dynamic" ForeColor="Red" ControlToValidate="txt_totalfine" ErrorMessage="**"></asp:RequiredFieldValidator>
									</div>

									<div class="col-md-6 col-sm-12 form-group">
										<label>Remaining Fine :<span class="text text-danger"></span></label>
									</div>
									<div class="col-md-6 col-sm-12 form-group">
										<asp:TextBox ID="txt_remfine" runat="server" TextMode="Number" class="form-control form-control-sm rounded" ReadOnly="false"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="ADVANCE" Display="Dynamic" ForeColor="Red" ErrorMessage="**" ControlToValidate="txt_remfine"></asp:RequiredFieldValidator>
									</div>

									<div class="col-md-6 col-sm-12 form-group">
										<label>Current Fine :<span class="text text-danger"></span></label>
									</div>
									<div class="col-md-6 col-sm-12 form-group">
										<asp:TextBox ID="txt_currfine" runat="server" TextMode="Number" class="form-control form-control-sm rounded" ReadOnly="false"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="ADVANCE" ErrorMessage="**" ControlToValidate="txt_currfine"></asp:RequiredFieldValidator>
									</div>
								</div>
								<asp:Label ID="lblMessage2" runat="server"></asp:Label>
							</div>
						</div>
					</div>
					<div class="modal-footer">
						<asp:Button ID="btn_svfines" runat="server" Text="Save Changes" OnClick="btn_svfines_Click" CssClass="btn btn-sm btn-info" />
						<button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">Close</button>
					</div>
				</div>
			</div>
		</div>

		<div class="modal fade" id="myModal3" data-backdrop="static">
			<div class="modal-dialog">
				<div class="modal-content">
					<div class="modal-header">
						<h4 class="modal-title">Add & Manage Other Deductions</h4>
						<button type="button" class="close" data-dismiss="modal">&times;</button>
					</div>
					<div class="modal-body">
						<div class="row">
							<div class="col-md-12">
								<div class="form-group">
									<div class="col-md-6 col-sm-12 form-group">
										<label>Total other Amount :<span class="text text-danger"></span></label>
									</div>
									<div class="col-md-6 col-sm-12 form-group">
										<asp:TextBox ID="txt_ttlothers" runat="server" TextMode="Number" CssClass="form-control form-control-sm rounded" ReadOnly="false"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="ADVANCE" Display="Dynamic" ForeColor="Red" ControlToValidate="txt_ttlothers" ErrorMessage="**"></asp:RequiredFieldValidator>
									</div>

									<div class="col-md-6 col-sm-12 form-group">
										<label>Enter others Remaining :<span class="text text-danger"></span></label>
									</div>
									<div class="col-md-6 col-sm-12 form-group">
										<asp:TextBox ID="txt_ttlothersrem" runat="server" TextMode="Number" CssClass="form-control form-control-sm rounded" ReadOnly="false"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="ADVANCE" Display="Dynamic" ForeColor="Red" ErrorMessage="**" ControlToValidate="txt_ttlothersrem"></asp:RequiredFieldValidator>
									</div>

									<div class="col-md-6 col-sm-12 form-group">
										<label>Current others :<span class="text text-danger"></span></label>
									</div>
									<div class="col-md-6 col-sm-12 form-group">
										<asp:TextBox ID="txt_ttlcurrothers" runat="server" TextMode="Number" CssClass="form-control form-control-sm rounded" ReadOnly="false"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="ADVANCE" ErrorMessage="**" ControlToValidate="txt_ttlcurrothers"></asp:RequiredFieldValidator>
									</div>
								</div>
								<asp:Label ID="lblMessage3" runat="server"></asp:Label>
							</div>
						</div>
					</div>
					<div class="modal-footer">
						<asp:Button ID="btn_svothers" runat="server" Text="Save Changes" CssClass="btn btn-info btn-sm" OnClick="btn_svothers_Click" />
						<button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">Close</button>
					</div>
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
		function ShowPopup1() {
			$("#myModal").modal("show");
		}
	</script>
</asp:Content>
