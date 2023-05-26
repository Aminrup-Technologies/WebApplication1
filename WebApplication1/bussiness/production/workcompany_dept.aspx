<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="workcompany_dept.aspx.cs" Inherits="WebApplication1.bussiness.production.company_dept" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="">
			<div class="row">
				<div class="col-md-12 col-sm-12" id="add_panel" runat="server" visible="true">
					<div class="x_panel">
						<div class="x_title">
							<h2>Add : Your Company - Department</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
								<%--<li><a class="close-link"><i class="fa fa-close"></i></a></li>--%>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">
							<div class="row">

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select Country Name <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_WorkCountry" CssClass="form-control form-control-sm rounded" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL_WorkCountry_SelectedIndexChanged"></asp:DropDownList>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select State Name <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_WorkStates" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_WorkStates_SelectedIndexChanged"></asp:DropDownList>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select Work Region <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_Region" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Region_SelectedIndexChanged"></asp:DropDownList>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select Company <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_Company" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
								</div>


								<div class="col-md-3 col-sm-12  form-group">
									<label>Company Department Name<span class="text text-danger">*</span></label>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_company_deptname" class="form-control form-control-sm rounded" placeholder="Enter Department Name..." runat="server"></asp:TextBox>
									<small class="form-text text-muted ml-4">Example : Raw Material Handling</small>
								</div>



								<div class="col-md-3 col-sm-12  form-group">
									<label>Company Department Code<span class="text text-danger">*</span></label>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_company_deptcode" class="form-control form-control-sm rounded" placeholder="Enter Department Short Code..." runat="server"></asp:TextBox>
									<small class="form-text text-muted ml-4">Example : RMH</small>
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

				<div class="col-md-12 col-sm-12  ">
					<div class="x_panel">
						<div class="x_title">
							<h2>View and Manage : Company-Departments</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
								<%--<li><a class="close-link"><i class="fa fa-close"></i></a></li>--%>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">
							<div class="row">

								<div class="col-md-12 col-sm-12">
									<div class="card-box table-responsive">
										<asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
											<Columns>
												<asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Region" Visible="false">
													<ItemTemplate>
														<asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Region" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_Work_Region_Code" runat="server" Text='<%# Eval("Work_Region_Code") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Company Name" HeaderStyle-Width="15%" Visible="false">
													<ItemTemplate>
														<asp:Label ID="lbl_Company_Name" runat="server" Text='<%# Eval("Company_Name") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Company Code" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_Company_Code" runat="server" Text='<%# Eval("Company_Code") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />

												</asp:TemplateField>

												<asp:TemplateField HeaderText="DB Code" HeaderStyle-Width="8%">
													<ItemTemplate>
														<asp:Label ID="lbl_DB_Code" runat="server" Text='<%# Eval("DB_Code") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Department Name" HeaderStyle-Width="20%">
													<ItemTemplate>
														<asp:Label ID="lbl_Company_Department" runat="server" Text='<%# Eval("Company_Department") %>' />
													</ItemTemplate>
													<EditItemTemplate>
														<asp:TextBox ID="txt_Company_Department" class="form-control form-control-sm rounded" Text='<%# Eval("Company_Department") %>' Width="100%" runat="server"></asp:TextBox>
														<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="Dynamic" ErrorMessage="*" ForeColor="Red" ControlToValidate="txt_Company_Department" SetFocusOnError="true"></asp:RequiredFieldValidator>
													</EditItemTemplate>
													<ItemStyle CssClass="text text-left" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Department Code" HeaderStyle-Width="20%">
													<ItemTemplate>
														<asp:Label ID="lbl_CompDept_Code" runat="server" Text='<%# Eval("CompDept_Code") %>' />
													</ItemTemplate>
													<EditItemTemplate>
														<asp:TextBox ID="txt_CompDept_Code" class="form-control form-control-sm rounded" Text='<%# Eval("CompDept_Code") %>' Width="100%" runat="server"></asp:TextBox>
														<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ErrorMessage="*" ForeColor="Red" ControlToValidate="txt_CompDept_Code" SetFocusOnError="true"></asp:RequiredFieldValidator>
													</EditItemTemplate>
													<ItemStyle CssClass="text text-left" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Action" HeaderStyle-Width="5%" Visible="false">
													<EditItemTemplate>
														<asp:ImageButton ID="btnupdate" runat="server" CommandName="Update" Height="15px" ImageUrl="~/erp_images/fi-sr-disk.svg" Width="15px" ToolTip="Save" ImageAlign="Middle"/>
														&nbsp;
														<asp:ImageButton ID="Btncancale" runat="server" CommandName="Cancel" Height="15px" ImageUrl="~/erp_images/fi-sr-cross-circle.svg" Width="15px" ToolTip="Cancel Update" ImageAlign="Middle"/>
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
			if (document.getElementById('<%=DDL_WorkCountry.ClientID%>').selectedIndex == 0) {
				document.getElementById('<%=DDL_WorkCountry.ClientID%>').focus();
				ShowPopup("Error :", "Work Country selection required...!");
				return false;
			}

			if (document.getElementById('<%=DDL_WorkStates.ClientID%>').selectedIndex == 0) {
				ShowPopup("Error :", "Work State selection required");
				document.getElementById('<%=DDL_WorkStates.ClientID%>').focus();
				return false;
			}

			if (document.getElementById('<%=DDL_Region.ClientID%>').selectedIndex == 0) {
				ShowPopup("Error :", "Work Region selection required");
				document.getElementById('<%=DDL_Region.ClientID%>').focus();
				return false;
			}

			if (document.getElementById('<%=DDL_Company.ClientID%>').selectedIndex == 0) {
				ShowPopup("Error :", "Selection required");
				document.getElementById('<%=DDL_Company.ClientID%>').focus();
				return false;
			}

			if (document.getElementById('<%=txt_company_deptname.ClientID%>').value == "") {
				ShowPopup("Error :", "Data required");
				document.getElementById('<%=txt_company_deptname.ClientID%>').focus();
				return false;
			}

			if (document.getElementById('<%=txt_company_deptcode.ClientID%>').value == "") {
				ShowPopup("Error :", "Data required");
				document.getElementById('<%=txt_company_deptcode.ClientID%>').focus();
				return false;
			}
		}
	</script>
</asp:Content>
