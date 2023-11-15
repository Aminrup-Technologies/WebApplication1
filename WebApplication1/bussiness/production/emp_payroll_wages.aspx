<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="emp_payroll_wages.aspx.cs" Inherits="WebApplication1.bussiness.production.emp_payroll_wages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="">
			<div class="page-title">
				<div class="title_left">
					<h3>Employee Payroll Wages</h3>
				</div>

				<div class="title_right">
					<div class="col-md-5 col-sm-5 form-group pull-right top_search">
						<!--<div class="input-group">
					<input type="text" class="form-control" placeholder="Search for...">
					<span class="input-group-btn">
					  <button class="btn btn-default" type="button">Go!</button>
					</span>
				  </div>-->
					</div>
				</div>

			</div>

			<div class="clearfix"></div>

			<div class="row">
				<div class="col-md-12 col-sm-12  ">
					<div class="x_panel">
						<div class="x_title">
							<h2>Add :Payroll Wages</h2>
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
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select State Name <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_WorkStates" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_WorkStates_SelectedIndexChanged"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" ControlToValidate="DDL_WorkStates"></asp:RequiredFieldValidator>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select Region <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_Region" runat="server" CssClass="form-control form-control-sm rounded" OnSelectedIndexChanged="DDL_Region_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" ControlToValidate="DDL_Region"></asp:RequiredFieldValidator>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select Company Name <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_Company" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Company_SelectedIndexChanged"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="SUBMIT" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" ControlToValidate="DDL_Company"></asp:RequiredFieldValidator>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Enter Category Type <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDl_Category_type" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDl_Category_type_SelectedIndexChanged"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDl_Category_type" SetFocusOnError="true"></asp:RequiredFieldValidator>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Enter Wages Rate  <span class="text text-danger">*</span></label>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_WagesRate" class="form-control form-control-sm rounded" placeholder="Enter WagesRate..." oninput="calculateTotalRate()" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_wagesrate" SetFocusOnError="true"></asp:RequiredFieldValidator>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<label>Enter VDA Rate <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_VDARate" class="form-control form-control-sm rounded" placeholder="Enter VDARate..." oninput="calculateTotalRate()" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_wagesrate" SetFocusOnError="true"></asp:RequiredFieldValidator>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<label>Enter Total Rate <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_totalRate" class="form-control form-control-sm rounded" placeholder="Enter Total_Rate..." runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_totalRate" SetFocusOnError="true"></asp:RequiredFieldValidator>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<label>Effective Date <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_effdt" runat="server" class="date-picker form-control rounded" placeholder="yyyy-mm-dd" required="required" onfocus="this.type='date'" onmouseover="this.type='date'" onclick="this.type = 'date'" onblur="this.type='text'" onmouseout="timeFunctionLong(this)"></asp:TextBox>
									<script>
										function timeFunctionLong(txt_effdt) {
											setTimeout(function () {
												txt_effdt.type = 'text';
											}, 60000);
										}
									</script>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_effdt" SetFocusOnError="true"></asp:RequiredFieldValidator>
								</div>

							</div>

							<%--button   start--%>
							<div class="col-md-6 center-margin">
								<div class="ln_solid"></div>
								<div class="item form-group row">
									<div class="col-md-6 col-sm-12">
										<asp:Label ID="Label1" runat="server" Text="Click SUBMIT to Save Data!!"></asp:Label>
									</div>
									<div class="col-md-6 col-sm-12">
										<button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
										<button type="reset" class="btn btn-warning btn-sm">Reset</button>
										<asp:Button ID="btn_submit" runat="server" Text="Submit" class="btn btn-success btn-sm" OnClick="btn_submit_Click" />
									</div>
								</div>
							</div>
							<%--button   end--%>
						</div>

					</div>
				</div>

				<div class="col-md-12 col-sm-12  ">
					<div class="x_panel">
						<div class="x_title">
							<h2>View and Manage : Employee Payroll Categories</h2>
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
										<%--<p class="text-muted font-13 m-b-30">Gridview with action</p>--%>
										<asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found">
											<Columns>
												<asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="2%">
													<ItemTemplate>
														<asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
													</ItemTemplate>
													<HeaderStyle CssClass="text text-center" />
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Country Code" Visible="false" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_Country_Code" runat="server" Text='<%# Eval("Country_Code") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="text text-center" />
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Country Code" Visible="false" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_State_Code" runat="server" Text='<%# Eval("State_Code") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="text text-center" />
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Country Code" Visible="false" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_WorkRegion_Code" runat="server" Text='<%# Eval("WorkRegion_Code") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="text text-center" />
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Country Code" HeaderStyle-Width="8%">
													<ItemTemplate>
														<asp:Label ID="lbl_Company_Code" runat="server" Text='<%# Eval("Company_Code") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="text text-center" />
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Country Code" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_Category_DB" runat="server" Text='<%# Eval("Category_DB") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="text text-center" />
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Country Code" HeaderStyle-Width="20%">
													<ItemTemplate>
														<asp:Label ID="lbl_Category_Type" runat="server" Text='<%# Eval("Category_Type") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="text text-center" />
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Country Code" HeaderStyle-Width="10%" Visible="false">
													<ItemTemplate>
														<asp:Label ID="lbl_Category_Code" runat="server" Text='<%# Eval("Category_Code") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="text text-center" />
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Wages Code" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_Wages_DB" runat="server" Text='<%# Eval("Wages_DB") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="text text-center" />
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Wages Rate" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_Wages_Rate" runat="server" Text='<%# Eval("Wages_Rate") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="text text-center" />
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="VDA RAte" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_VDA_Rate" runat="server" Text='<%# Eval("VDA_Rate") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="text text-center" />
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Total Wages" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_Total_Wages" runat="server" Text='<%# Eval("Total_Wages") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="text text-center" />
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Effective Form" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_Active_Date" runat="server" Text='<%# Eval("Active_Date","{0:dd-MM-yyyy}") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="text text-center" />
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Status" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_Status" runat="server" Text='<%# Eval("Status") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="text text-center" />
													<ItemStyle CssClass="text text-center" />
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

	<script>
		function calculateTotalRate() {
			// Get the values of the WagesRate and VDARate TextBox controls
			var wagesRate = parseFloat(document.getElementById('<%= txt_WagesRate.ClientID %>').value) || 0;
		var vdARate = parseFloat(document.getElementById('<%= txt_VDARate.ClientID %>').value) || 0;

		// Calculate the total rate
		var totalRate = wagesRate + vdARate;

		// Set the result in the Total_Rate TextBox control
		document.getElementById('<%= txt_totalRate.ClientID %>').value = totalRate;
	}
	</script>
</asp:Content>
