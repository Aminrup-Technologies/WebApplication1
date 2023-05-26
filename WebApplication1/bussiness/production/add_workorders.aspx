<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="add_workorders.aspx.cs" Inherits="WebApplication1.bussiness.production.add_workorders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="container">
			<div class="page-title">
				<div class="title_left">
					<h5>Work Order Addition Page || <asp:Button ID="Button1" runat="server" Text="ExportData" CssClass="btn btn-warning btn-sm" CausesValidation="false" OnClick="ExportExcel"/></h5>
				</div>
			</div>

			<div class="clearfix"></div>

			<div class="row">
				<div class="col-md-12 col-sm-12  ">
					<div class="x_panel">
						<div class="x_title">
							<h2>Add : Work Order Information</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">
							<div class="row">

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select Work Region <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_WorkRegion" CssClass="form-control form-control-sm rounded" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL_WorkRegion_SelectedIndexChanged"></asp:DropDownList>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select Company Name<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_Company" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Company_SelectedIndexChanged"></asp:DropDownList>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select DepartmentName <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_Departments" runat="server" CssClass="form-control form-control-sm rounded" OnSelectedIndexChanged="DDL_Departments_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Enter Work Order Type<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_workordertype" class="form-control form-control-sm rounded" placeholder="Enter work order type..." MaxLength="3" runat="server"></asp:TextBox>
									<small class="form-text text-muted ml-4">Example : ARC</small>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Enter Work Order No<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_workorderno" class="form-control form-control-sm rounded" placeholder="Enter work order number" runat="server"></asp:TextBox>
									<small class="form-text text-muted ml-4">Example : 3000126427</small>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Status<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group rounded">
									<asp:DropDownList ID="DDL_Status" runat="server" CssClass="form-control form-control-sm rounded">
										<asp:ListItem Value="0" Selected="True">Please select option</asp:ListItem>
										<asp:ListItem Value="Active">Active</asp:ListItem>
										<asp:ListItem Value="Inactive">Inactive</asp:ListItem>
									</asp:DropDownList>
								</div>
							</div>


							<%--button start--%>
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
							<%--button end--%>

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
					<div class="x_panel">
						<div class="x_title">
							<h2>View and Manage : Work Order Data</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">
							<div class="row">

								<div class="col-md-12 col-sm-12">
									<div class="card-box table-responsive">
										<asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" OnRowDataBound="GridView1_RowDataBound">
											<Columns>
												<asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="2%">
													<ItemTemplate>
														<asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Id" Visible="false">
													<ItemTemplate>
														<asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Region Code" HeaderStyle-Width="3%">
													<ItemTemplate>
														<asp:Label ID="lbl_Work_Region_Code" runat="server" Text='<%# Eval("Work_Region_Code") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Company Code" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_Company_Code" runat="server" Text='<%# Eval("Company_Code") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Department Code" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_Dept_DBCode" runat="server" Text='<%# Eval("Dept_DBCode") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Department Name" HeaderStyle-Width="15%">
													<ItemTemplate>
														<asp:Label ID="lbl_Department_Name" runat="server" Text='<%# Eval("Department_Name") %>' />
													</ItemTemplate>
													<EditItemTemplate>
														<asp:DropDownList ID="DDL_NewDept" runat="server" class="form-control form-control-sm rounded"></asp:DropDownList>
													</EditItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="DB Code" Visible="false">
													<ItemTemplate>
														<asp:Label ID="lbl_DB_Code" runat="server" Text='<%# Eval("DB_Code") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Type" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_WO_Type" runat="server" Text='<%# Eval("WO_Type") %>' />
													</ItemTemplate>
													<EditItemTemplate>
														<asp:TextBox ID="txt_WO_Type" class="form-control form-control-sm rounded" Text='<%# Eval("WO_Type") %>' Width="100%" runat="server"></asp:TextBox>
														<asp:RequiredFieldValidator ID="RFV_1" runat="server" Display="Dynamic" ErrorMessage="*" ForeColor="Red" ControlToValidate="txt_WO_Type" SetFocusOnError="true"></asp:RequiredFieldValidator>
													</EditItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Work Order Number" HeaderStyle-Width="7%">
													<ItemTemplate>
														<asp:Label ID="lbl_WO_Number" runat="server" Text='<%# Eval("WO_Number") %>' />
													</ItemTemplate>
													<EditItemTemplate>
														<asp:TextBox ID="txt_WO_Number" class="form-control form-control-sm rounded" Text='<%# Eval("WO_Number") %>' Width="100%" runat="server"></asp:TextBox>
														<asp:RequiredFieldValidator ID="RFV_2" runat="server" Display="Dynamic" ErrorMessage="*" ForeColor="Red" ControlToValidate="txt_WO_Number" SetFocusOnError="true"></asp:RequiredFieldValidator>
													</EditItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Status" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_WO_Status" runat="server" Text='<%# Eval("WO_Status") %>' />
													</ItemTemplate>
													<EditItemTemplate>
														<asp:DropDownList ID="DDL_WO_Status" runat="server" class="form-control form-control-sm rounded" SelectedValue='<%# Bind("WO_Status") %>'>
															<asp:ListItem Value="0" Selected="True">Please select option</asp:ListItem>
															<asp:ListItem Value="Active">Active</asp:ListItem>
															<asp:ListItem Value="Inactive">Inactive</asp:ListItem>
														</asp:DropDownList>
													</EditItemTemplate>
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
														<asp:ImageButton ID="btndelete" runat="server" CommandName="Delete" Height="15px" ImageUrl="~/erp_images/fi-sr-trash.svg" Width="15px" ToolTip="Delete" OnClientClick="return confirm('Do you want to DELETE...?')" ImageAlign="Middle" />
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

		function ValidateFormField() {
			if (document.getElementById('<%=DDL_WorkRegion.ClientID%>').selectedIndex == 0) {
				ShowPopup("Error :", "Work Region selection required");
				document.getElementById('<%=DDL_WorkRegion.ClientID%>').focus();
				return false;
			}

			if (document.getElementById('<%=DDL_Company.ClientID%>').selectedIndex == 0) {
				ShowPopup("Error :", "Selection required");
				document.getElementById('<%=DDL_Company.ClientID%>').focus();
				return false;
			}

			if (document.getElementById('<%=DDL_Departments.ClientID%>').selectedIndex == 0) {
				ShowPopup("Error :", "Selection required");
				document.getElementById('<%=DDL_Departments.ClientID%>').focus();
				return false;
			}

			if (document.getElementById('<%=txt_workordertype.ClientID%>').value == "") {
				ShowPopup("Error :", "Data required");
				document.getElementById('<%=txt_workordertype.ClientID%>').focus();
				return false;
			}

			if (document.getElementById('<%=txt_workorderno.ClientID%>').value == "") {
				ShowPopup("Error :", "Data required");
				document.getElementById('<%=txt_workorderno.ClientID%>').focus();
				return false;
			}
			if (document.getElementById('<%=DDL_Status.ClientID%>').selectedIndex == 0) {
				ShowPopup("Error :", "Selection required");
				document.getElementById('<%=DDL_Status.ClientID%>').focus();
				return false;
			}
		}
	</script>

	<script type="text/javascript">
		function ShowPopup(title, body) {
			$("#MyPopup .modal-title").html(title);
			$("#MyPopup .modal-body").html(body);
			$("#MyPopup").modal("show");
		}
	</script>
</asp:Content>
