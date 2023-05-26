<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="atsworksite_incharges.aspx.cs" Inherits="WebApplication1.bussiness.production.atsworksite_incharges" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="">
			<div class="page-title">
				<div class="title_left">
					<h3>WorkSite -  InCharges</h3>
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
							<h2>Assign Worksite to Employees</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">
							<div class="row">

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select Worksite <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_worksites" runat="server" CssClass="form-control form-control-sm rounded" OnSelectedIndexChanged="DDL_worksites_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RFV_1" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" ControlToValidate="DDL_worksites" InitialValue="Please Select Option" ValidationGroup="Submit_Btn"></asp:RequiredFieldValidator>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Enter Employee Workman <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_empworkman" class="form-control form-control-sm rounded" runat="server" AutoPostBack="true" placeholder="Enter Employee Workman" OnTextChanged="txt_empworkman_TextChanged"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RFV_2" runat="server" ErrorMessage="Required" Display="Dynamic" ForeColor="Red" ControlToValidate="txt_empworkman" InitialValue="" ValidationGroup="Submit_Btn"></asp:RequiredFieldValidator>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Employee Full Name</label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_empname" runat="server" ReadOnly="true" class="form-control form-control-sm rounded"></asp:TextBox>
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
										<asp:Button ID="btn_submit" runat="server" Text="Submit" class="btn btn-success btn-sm" Enabled="true" ValidationGroup="Submit_Btn" OnClick="btn_submit_Click" />
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

				<div class="col-md-12 col-sm-12  ">
					<div class="x_panel">
						<div class="x_title">
							<h2>View and Manage : Assigned Worksites</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">
							<div class="row">
								<div class="col-md-12 col-sm-12">
									<div class="card-box table-responsive">
										<asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
											<Columns>
												<asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="2%">
													<ItemTemplate>
														<asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="DB Code" Visible="false">
													<ItemTemplate>
														<asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="DB Code" Visible="true" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_DB_Code" runat="server" Text='<%# Eval("DB_Code") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Worksite Name" Visible="true" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_Worksite_Name" runat="server" Text='<%# Eval("Worksite_Name") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Employee Workman" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_Employee_Workman" runat="server" Text='<%# Eval("Employee_Workman") %>' />
													</ItemTemplate>
													<EditItemTemplate>
														<asp:TextBox ID="txt_Employee_Workman" runat="server" AutoPostBack="true" OnTextChanged="txt_Employee_Workman_TextChanged" class="form-control form-control-sm rounded" Text='<%# Eval("Employee_Workman") %>'></asp:TextBox>
													</EditItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="15%">
													<ItemTemplate>
														<asp:Label ID="lbl_Employee_Name" runat="server" Text='<%# Eval("Employee_Name") %>' />
													</ItemTemplate>
													<EditItemTemplate>
														<asp:TextBox ID="txt_Employee_Name" ReadOnly="true" runat="server" class="form-control form-control-sm rounded" Text='<%# Eval("Employee_Name") %>'></asp:TextBox>
													</EditItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Assign Date" HeaderStyle-Width="7%">
													<ItemTemplate>
														<asp:Label ID="lbl_Assign_Date" runat="server" Text='<%# Eval("Assign_Date","{0:dd-MM-yyyy}") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Withdraw Date" HeaderStyle-Width="7%">
													<ItemTemplate>
														<asp:Label ID="lbl_Withdraw_Date" runat="server" Text='<%# Eval("Withdraw_Date","{0:dd-MM-yyyy}") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Status" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_Status" runat="server" Text='<%# Eval("Status") %>' />
													</ItemTemplate>
													<EditItemTemplate>
														<asp:DropDownList ID="DDL_Status" runat="server" class="form-control form-control-sm rounded" SelectedValue='<%# Bind("Status") %>'>
															<asp:ListItem Value="Active">Active</asp:ListItem>
															<asp:ListItem Value="Inactive">Inactive</asp:ListItem>
														</asp:DropDownList>
													</EditItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="JOB Approver" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_JOB_Approver" runat="server" Text='<%# Eval("JOB_Approver") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Action" HeaderStyle-Width="5%">
													<EditItemTemplate>
														<asp:ImageButton ID="btnupdate" runat="server" CommandName="Update" Height="15px" ImageUrl="~/erp_images/fi-sr-disk.svg" Width="15px" ToolTip="Save" ImageAlign="Middle" />
														&nbsp;
														<asp:ImageButton ID="Btncancale" runat="server" CommandName="Cancel" Height="15px" ImageUrl="~/erp_images/fi-sr-cross-circle.svg" Width="15px" ToolTip="Cancel Update" ImageAlign="Middle" />
													</EditItemTemplate>
													<ItemTemplate>
														<asp:ImageButton ID="btnedit" runat="server" CommandName="Edit" Height="15px" ImageUrl="~/erp_images/fi-sr-pencil.svg" Width="15px" ToolTip="Update" ImageAlign="Middle" />
														&nbsp;
														<asp:ImageButton ID="btndelete" runat="server" CommandName="Delete" Enabled="true" Height="15px" ImageUrl="~/erp_images/fi-sr-trash.svg" Width="15px" ToolTip="Delete" OnClientClick="return confirm('Do you want to DELETE...?')" ImageAlign="Middle" />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>
											</Columns>
											<HeaderStyle CssClass="text text-center" />
											<EditRowStyle CssClass="bg-blue-sky" />

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
	</div>

	<script type="text/javascript">
		function ShowPopup(title, body) {
			$("#MyPopup .modal-title").html(title);
			$("#MyPopup .modal-body").html(body);
			$("#MyPopup").modal("show");
		}
	</script>
</asp:Content>
