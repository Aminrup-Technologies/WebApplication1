<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="view_empmonthlyatt.aspx.cs" Inherits="WebApplication1.bussiness.production.view_empmonthlyatt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="">
			<div class="page-title">
				<div class="title_left">
					<h4>Employee Attendance & Payslip View </h4>
				</div>
			</div>
			<div class="clearfix"></div>

			<div class="row">
				<div class="col-md-12 col-sm-12">
					<div class="x_panel">
						<%--<div class="x_title">
							<h2>Search Employee by Name / Workman</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>--%>
						<div class="x_content">
							<div class="row">
								<%--<span class="section">Select Search Type</span>--%>

								<div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
									<label>Select Calender Input<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
									<asp:DropDownList ID="DDL_Day" CssClass="form-control form-control-sm rounded" runat="server" Visible="false"></asp:DropDownList>
									<asp:DropDownList ID="DDL_Month" CssClass="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
								</div>
								<div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
									<asp:DropDownList ID="DDL_Year" CssClass="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
								</div>

								<div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
									&nbsp;
								</div>

								<div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
									<label>Name / Workman SL<span class="required">*</span></label>
								</div>

								<div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
									<asp:DropDownList ID="DDL_SearchType" runat="server" class="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_SearchType_SelectedIndexChanged">
										<asp:ListItem Selected="True" Value="0">--Select--</asp:ListItem>
										<asp:ListItem Value="1">By Employee Name</asp:ListItem>
										<asp:ListItem Value="2">By Workman SL</asp:ListItem>
									</asp:DropDownList>
								</div>

								<div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group" id="Nameinputrow1" runat="server" visible="false">
									<label class="col-form-label col-md-3 col-sm-3 label-align">Employee Name<span class="required">*</span></label>
								</div>

								<div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group" id="Nameinputrow2" runat="server" visible="false">
									<asp:TextBox ID="txt_empname" class="form-control form-control-sm rounded" runat="server" placeholder="Enter Employee Name"></asp:TextBox>
								</div>

								<div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group" id="WorkmanInput_Row1" runat="server" visible="false">
									<label class="col-form-label col-md-3 col-sm-3 label-align">Employee Workman<span class="required">*</span></label>
								</div>

								<div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group" id="WorkmanInput_Row2" runat="server" visible="false">
									<asp:TextBox ID="txt_empworkman" class="form-control form-control-sm rounded" runat="server" placeholder="Enter Employee Workman"></asp:TextBox>
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

							<div class="col-md-12 col-sm-12">
								<div class="card-box table-responsive">
									<asp:GridView ID="GridView1" runat="server" Font-Size="Smaller" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="GridView1_RowEditing" OnRowDataBound="GridView1_RowDataBound" OnRowUpdating="GridView1_RowUpdating" OnRowCommand="GridView1_RowCommand" OnRowDeleting="GridView1_RowDeleting">
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

											<asp:TemplateField HeaderText="JOBID" Visible="true">
												<ItemTemplate>
													<asp:Button ID="btn_viewdetails" runat="server" Text='<%# Eval("JOBID") %>' Font-Size="Smaller" CommandArgument='<%#Eval("JOBID") %>' CssClass="btn btn-sm btn-info" CommandName="Approve" />
													<asp:Label ID="lbl_JOBID" runat="server" Text='<%# Eval("JOBID") %>' Visible="false" />
												</ItemTemplate>
												<ItemStyle CssClass="text text-center" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="Date" Visible="true" HeaderStyle-Width="8%">
												<ItemTemplate>
													<asp:Label ID="lbl_CreatedDate" runat="server" Text='<%# Bind("CreatedDate","{0:dd-MM-yyyy}") %>'></asp:Label>
												</ItemTemplate>
												<ItemStyle CssClass="text text-center" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="Work Man" HeaderStyle-Width="2%">
												<ItemTemplate>
													<asp:Label ID="lbl_EmployeeWrk" runat="server" Text='<%# Bind("EmployeeWrk") %>'></asp:Label>
												</ItemTemplate>
												<ItemStyle CssClass="text text-center" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="12%">
												<ItemTemplate>
													<asp:Label ID="lbl_EmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
												</ItemTemplate>
												<ItemStyle CssClass="text text-left" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="Site" HeaderStyle-Width="12%">
												<ItemTemplate>
													<asp:Label ID="lbl_Employee_Worksite" runat="server" Text='<%# Bind("Employee_Worksite") %>'></asp:Label>
												</ItemTemplate>
												<%--<EditItemTemplate>
														<asp:DropDownList ID="DropDownList1" class="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
													</EditItemTemplate>--%>
												<ItemStyle CssClass="text text-center small" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="Designation" HeaderStyle-Width="8%" Visible="false">
												<ItemTemplate>
													<asp:Label ID="lbl_EmpDesignation" runat="server" Text='<%# Bind("EmpDesignation") %>'></asp:Label>
												</ItemTemplate>
												<ItemStyle CssClass="text text-center small" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="H" HeaderStyle-Width="1%">
												<ItemTemplate>
													<asp:Label ID="lbl_WourkHours" runat="server" Text='<%# Bind("WourkHours") %>'></asp:Label>
												</ItemTemplate>
												<ItemStyle CssClass="text text-center" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="IN-Punch Time" HeaderStyle-Width="15%">
												<ItemTemplate>
													<asp:Label ID="lbl_Inpunch_Time" runat="server" Text='<%# Bind("Inpunch_Time","{0:dd-MM-yyyy hh:mm:ss tt}") %>'></asp:Label>
												</ItemTemplate>
												<EditItemTemplate>
													<asp:TextBox ID="txt_Inpunch_Time" runat="server" class="form-control form-control-sm rounded small" Text='<%# DataBinder.Eval(Container.DataItem,"Inpunch_Time", "{0:yyyy-MM-dd hh:mm:ss tt}") %> ' Width="100%"></asp:TextBox>
												</EditItemTemplate>
												<ItemStyle CssClass="text text-center" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="OUT-Punch Time" HeaderStyle-Width="15%">
												<ItemTemplate>
													<asp:Label ID="lbl_Outpunch_Time" runat="server" Text='<%# Bind("Outpunch_Time","{0:dd-MM-yyyy hh:mm:ss tt}") %>'></asp:Label>
												</ItemTemplate>
												<EditItemTemplate>
													<asp:TextBox ID="txt_Outpunch_Time" runat="server" class="form-control form-control-sm rounded small" Text='<%# DataBinder.Eval(Container.DataItem,"Outpunch_Time", "{0:yyyy-MM-dd hh:mm:ss tt}") %> ' Width="100%"></asp:TextBox>
												</EditItemTemplate>
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
												<EditItemTemplate>
													<asp:DropDownList ID="DDL_LunchYesNo" runat="server" class="form-control form-control-sm rounded" SelectedValue='<%# Bind("LunchFactor") %>'>
														<asp:ListItem Text="Yes" Value="Yes">Yes</asp:ListItem>
														<asp:ListItem Text="No" Value="No">No</asp:ListItem>
													</asp:DropDownList>
												</EditItemTemplate>
												<ItemStyle CssClass="text text-center" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="C OT" HeaderStyle-Width="2%">
												<ItemTemplate>
													<asp:Label ID="lbl_Calc_OT" runat="server" Text='<%# Bind("Calc_OT") %>'></asp:Label>
												</ItemTemplate>
												<ItemStyle CssClass="text text-center" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="POT" HeaderStyle-Width="7%">
												<ItemTemplate>
													<asp:Label ID="lbl_ProvidedOT" runat="server" Text='<%# Bind("ProvidedOT") %>'></asp:Label>
												</ItemTemplate>
												<EditItemTemplate>
													<asp:TextBox ID="txt_ProvidedOT" runat="server" class="form-control form-control-sm rounded" Text='<%# DataBinder.Eval(Container.DataItem,"ProvidedOT") %>' Width="100%"></asp:TextBox>
													<asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="txt_ProvidedOT" ValidationGroup="Update" Display="Dynamic" ErrorMessage="Value must be less than or equal to 16" ClientValidationFunction="validateProvidedOT" SetFocusOnError="true" ForeColor="Red"></asp:CustomValidator>
												</EditItemTemplate>
												<ItemStyle CssClass="text text-center" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="A Status" HeaderStyle-Width="3%">
												<ItemTemplate>
													<asp:Label ID="lbl_AttendanceStatus" runat="server" Text='<%# Bind("AttendanceStatus") %>'></asp:Label>
												</ItemTemplate>
												<EditItemTemplate>
													<asp:DropDownList ID="DDL_AttendanceStatus" class="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
												</EditItemTemplate>
												<ItemStyle CssClass="text text-center" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="A Code" HeaderStyle-Width="3%">
												<ItemTemplate>
													<asp:Label ID="lbl_AttendanceCode" runat="server" Text='<%# Bind("AttendanceCode") %>'></asp:Label>
												</ItemTemplate>
												<EditItemTemplate>
													<asp:DropDownList ID="DDL_AttendanceCode" class="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
												</EditItemTemplate>
												<ItemStyle CssClass="text text-center" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="Action" HeaderStyle-Width="3%">
												<EditItemTemplate>
													<asp:ImageButton ID="btnupdate" runat="server" CommandName="Update" Height="15px" ImageUrl="~/erp_images/fi-sr-disk.svg" ValidationGroup="Update" CommandArgument="<%# Container.DataItemIndex %>" Width="15px" ToolTip="Save" ImageAlign="Middle" />
													<asp:ImageButton ID="Btncancale" runat="server" CommandName="Cancel" Height="15px" ImageUrl="~/erp_images/fi-sr-cross-circle.svg" Width="15px" ToolTip="Cancel Update" ImageAlign="Middle" CausesValidation="false" />
												</EditItemTemplate>
												<ItemTemplate>
													<asp:ImageButton ID="btnedit" runat="server" Visible="true" CommandName="Edit" Height="15px" ImageUrl="~/erp_images/fi-sr-pencil.svg" Width="15px" ToolTip="Update" ImageAlign="Middle" />
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
				</div>
			</div>
		</div>
	</div>
	<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

	<script type="text/javascript">
		function ShowPopup(title, body) {
			$("#MyPopup .modal-title").html(title);
			$("#MyPopup .modal-body").html(body);
			$("#MyPopup").modal("show");
		}
	</script>

	<script>
		function validateProvidedOT(sender, args) {
			var textBox = $("#" + sender.controltovalidate);
			var inputValue = textBox.val();
			if (inputValue.trim() !== '') {
				var numericValue = parseInt(inputValue);
				if (isNaN(numericValue) || numericValue > 16) {
					args.IsValid = false;
				} else {
					args.IsValid = true;
				}
			} else {
				args.IsValid = false;
			}
		}
	</script>
</asp:Content>
