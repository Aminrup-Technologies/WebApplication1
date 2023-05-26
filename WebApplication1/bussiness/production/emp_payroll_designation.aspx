<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="emp_payroll_designation.aspx.cs" Inherits="WebApplication1.bussiness.production.emp_payroll_desgination" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="">
			<div class="page-title">
				<div class="title_left">
					<h3>Employee Payroll Designation</h3>
				</div>

				<div class="title_right">
					<div class="col-md-5 col-sm-5 form-group pull-right top_search">
					</div>
				</div>

			</div>

			<div class="clearfix"></div>

			<div class="row">
				<div class="col-md-12 col-sm-12  ">
					<div class="x_panel">
						<div class="x_title">
							<h2>Add : Emp_payroll_Desg</h2>
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
									<asp:DropDownList ID="DDL_WorkCountry" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_WorkCountry_SelectedIndexChanged"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" ControlToValidate="DDL_WorkCountry"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4">Example : India</small>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select State Name <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_WorkStates" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_WorkStates_SelectedIndexChanged"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" ControlToValidate="DDL_WorkStates"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4">Example : India</small>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select WorkRegion <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_Region" runat="server" CssClass="form-control form-control-sm rounded" OnSelectedIndexChanged="DDL_Region_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" ControlToValidate="DDL_Region"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4">Example : India</small>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select Company Name <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_Company" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Company_SelectedIndexChanged"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" ControlToValidate="DDL_Company"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4">Example : India</small>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<label>Select Skill Category <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDl_Category_type" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDl_Category_type_SelectedIndexChanged"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" ControlToValidate="DDl_Category_type"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4">Example : India</small>
								</div>



								<div class="col-md-3 col-sm-12  form-group">
									<label>Enter Designation Name  <span class="text text-danger">*</span></label>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_designation_Name" class="form-control form-control-sm rounded" placeholder="txt_designation_Name..." runat="server" TextMode="SingleLine"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_designation_Name" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4">Example : HS</small>
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
										<asp:Button ID="btn_submit" runat="server" Text="Submit" class="btn btn-success btn-sm" OnClick="btn_submit_Click" />
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
							<h2>View and Manage : Employee Payroll Category-Designation</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">
							<div class="row">

								<div class="col-md-12 col-sm-12">
									<div class="card-box table-responsive">
										<asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found">
											<Columns>
												<asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="2%">
													<ItemTemplate>
														<asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
													</ItemTemplate>
													<HeaderStyle CssClass="text-center" />
													<HeaderStyle CssClass="text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="State Code" Visible="false" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_State_Code" runat="server" Text='<%# Eval("State_Code") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="text-center" />
													<HeaderStyle CssClass="text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Work Region Code" Visible="false" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_WorkRegion_Code" runat="server" Text='<%# Eval("WorkRegion_Code") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="text-center" />
													<HeaderStyle CssClass="text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Company Code" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_Company_Code" runat="server" Text='<%# Eval("Company_Code") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="text-center" />
													<HeaderStyle CssClass="text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Category DB" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_Category_DB" runat="server" Text='<%# Eval("Category_DB") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="text-center" />
													<HeaderStyle CssClass="text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Category Name" HeaderStyle-Width="15%">
													<ItemTemplate>
														<asp:Label ID="lbl_Category_Type" runat="server" Text='<%# Eval("Category_Type") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="text-center" />
													<HeaderStyle CssClass="text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Designation DB" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_Designation_DB" runat="server" Text='<%# Eval("Designation_DB") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="text-center" />
													<HeaderStyle CssClass="text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Designation Name" HeaderStyle-Width="20%">
													<ItemTemplate>
														<asp:Label ID="lbl_Designation_Type" runat="server" Text='<%# Eval("Designation_Type") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="text-center" />
													<HeaderStyle CssClass="text-center" />
												</asp:TemplateField>

											</Columns>
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

			if (document.getElementById('<%=txt_designation_Name.ClientID%>').value == "") {
				ShowPopup("Error :", "Data required");
				document.getElementById('<%=txt_designation_Name.ClientID%>').focus();
				return false;
			}
		}
	</script>
</asp:Content>
