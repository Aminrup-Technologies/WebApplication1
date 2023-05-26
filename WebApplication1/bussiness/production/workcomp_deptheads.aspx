<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="workcomp_deptheads.aspx.cs" Inherits="WebApplication1.bussiness.production.work_head_dept" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="">
			<div class="page-title">
				<div class="title_left">
					<h3>Department Representative</h3>
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
							<h2>Add : Your company Representative</h2>
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
									<asp:DropDownList ID="DDL_WorkCountry" CssClass="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_WorkCountry" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4">Example : Select India</small>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select State Name <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_WorkStates" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_WorkStates" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4">Example : India</small>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select Region <span class="text text-danger">*</span></label>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_Region" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
									<small class="form-text text-muted ml-4">Example : India</small>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select companyName <span class="text text-danger">*</span></label>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_companyName" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
									<small class="form-text text-muted ml-4">Example : India</small>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select DepartmentName <span class="text text-danger">*</span></label>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_dept_name" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
									<small class="form-text text-muted ml-4">Example : India</small>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>company  Department Name <span class="text text-danger">*</span></label>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_company_deptname" class="form-control form-control-sm rounded" placeholder="Enter Country Name..." runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_company_deptname" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4">Example : India</small>
								</div>



								<div class="col-md-3 col-sm-12  form-group">
									<label>Designation <span class="text text-danger">*</span></label>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_designation" class="form-control form-control-sm rounded" placeholder="Enter Country Short Code..." runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_designation" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4">Example : IN</small>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Mobile No <span class="text text-danger">*</span></label>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="TextBox1" class="form-control form-control-sm rounded" placeholder="Enter Country Short Code..." runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_designation" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4">Example : IN</small>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<label>Email_Id <span class="text text-danger">*</span></label>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="TextBox2" class="form-control form-control-sm rounded" placeholder="Enter Country Short Code..." runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_designation" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4">Example : IN</small>
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
										<asp:Button ID="btn_submit" runat="server" Text="Submit" CssClass="btn btn-success btn-sm" Enabled="false" />
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
							<h2>View and Manage : Department Heads</h2>
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
										<asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found">
											<Columns>
												<asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<%--<asp:TemplateField HeaderText="Country Name" HeaderStyle-Width="20%">
													<ItemTemplate>
														<asp:Label ID="lbl_Country_Name" runat="server" Text='<%# Eval("Country_Name") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>--%>

												<asp:TemplateField HeaderText="Country Code" Visible="false" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_Country_Code" runat="server" Text='<%# Eval("Country_Code") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<%--<asp:TemplateField HeaderText="State Name" HeaderStyle-Width="20%">
													<ItemTemplate>
														<asp:Label ID="lbl_State_Name" runat="server" Text='<%# Eval("State_Name") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>--%>

												<asp:TemplateField HeaderText="State Code" Visible="false"  HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_State_Code" runat="server" Text='<%# Eval("State_Code") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<%--<asp:TemplateField HeaderText="Work Region Name" HeaderStyle-Width="20%">
													<ItemTemplate>
														<asp:Label ID="lbl_Work_Region_Name" runat="server" Text='<%# Eval("Work_Region_Name") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>--%>

												<asp:TemplateField HeaderText="Region Code" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_Work_Region_Code" runat="server" Text='<%# Eval("Work_Region_Code") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Company Name" HeaderStyle-Width="15%">
													<ItemTemplate>
														<asp:Label ID="lbl_Company_Name" runat="server" Text='<%# Eval("Company_Name") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Company Code" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_Company_Code" runat="server" Text='<%# Eval("Company_Code") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Department Name" HeaderStyle-Width="20%">
													<ItemTemplate>
														<asp:Label ID="lbl_Department_Name" runat="server" Text='<%# Eval("Department_Name") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Department Code" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_Department_Code" runat="server" Text='<%# Eval("Department_Code") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Rep_Name" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_Rep_Name" runat="server" Text='<%# Eval("Rep_Name") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Designation" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_Rep_Designation" runat="server" Text='<%# Eval("Rep_Designation") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Mobile" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_Rep_Mobile" runat="server" Text='<%# Eval("Rep_Mobile") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Email" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_Rep_Email" runat="server" Text='<%# Eval("Rep_Email") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Memo Approver" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_Memo_Approver" runat="server" Text='<%# Eval("Memo_Approver") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Challam Approver" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_Challam_Approver" runat="server" Text='<%# Eval("Challam_Approver") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
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
</asp:Content>
