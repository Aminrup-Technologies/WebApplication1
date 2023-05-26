<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="add_workorder_lineitems.aspx.cs" Inherits="WebApplication1.bussiness.production.workorder_line_litems" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="container">
			<%--<div class="page-title">
				<div class="title_left">
					<h3>Work Order Line Items</h3>
				</div>

				<div class="title_right">
					<div class="col-md-5 col-sm-5 form-group pull-right top_search"></div>
				</div>
			</div>

			<div class="clearfix"></div>--%>

			<div class="row">
				<div class="col-md-12 col-sm-12 ">
					<div class="x_panel">
						<div class="x_title">
							<h2>Add : Line Items Details</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>

						<div class="x_content">
							<div class="row">

								<div class="col-md-3 col-sm-12 form-group">
									<label>Select Work Region<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group">
									<asp:DropDownList ID="DDL_WorkRegion" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_WorkRegion_SelectedIndexChanged"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_WorkRegion" SetFocusOnError="true" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
								</div>


								<div class="col-md-3 col-sm-12 form-group">
									<label>Select Work Company<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group">
									<asp:DropDownList ID="DDL_Company" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Company_SelectedIndexChanged"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_Company" SetFocusOnError="true" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select DepartmentName <span class="text text-danger">*</span></label>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_Departments" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Departments_SelectedIndexChanged"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_Departments" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
								</div>


								<div class="col-md-3 col-sm-12 form-group">
									<label>Select Work Order<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group">
									<asp:DropDownList ID="DDL_Workorder" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Workorder_SelectedIndexChanged"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_Workorder" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
								</div>


								<div class="col-md-3 col-sm-12 form-group">
									<label>Enter Line Item No<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group">
									<asp:TextBox ID="txt_lineitemno" class="form-control form-control-sm rounded" placeholder="Enter LINE Number" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_lineitemno" SetFocusOnError="true" InitialValue="" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4">Example : 10</small>
								</div>


								<div class="col-md-3 col-sm-12 form-group">
									<label>Enter Serial Number<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group">
									<asp:TextBox ID="txt_serialno" class="form-control form-control-sm rounded" placeholder="Enter LINE Number" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_serialno" SetFocusOnError="true" InitialValue="" ></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4">Example : 10</small>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>Enter Service Number <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group">
									<asp:TextBox ID="txt_service_number" class="form-control form-control-sm rounded" placeholder="Enter service number" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_service_description" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4">Example : USCESI02A102</small>
								</div>


								<div class="col-md-3 col-sm-12 form-group">
									<label>Enter Service Description <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group">
									<asp:TextBox ID="txt_service_description" class="form-control form-control-sm rounded" placeholder="Enter Service Description" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_service_description" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4">Example : ELE & MECH MAINT OF LAB INSTRUMENTS</small>
								</div>


								<div class="col-md-2 col-sm-12 form-group">
									<label>Enter Order Quantity <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-2 col-sm-12 form-group">
									<asp:TextBox ID="txt_order_quantity" class="form-control form-control-sm rounded" placeholder="Enter Line Item Order Quantity" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_order_quantity" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4">Example : 1</small>
								</div>


								<div class="col-md-2 col-sm-12 form-group">
									<label>Enter Rate <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-2 col-sm-12 form-group">
									<asp:TextBox ID="txt_rate" class="form-control form-control-sm rounded" placeholder="Enter Rate" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_rate" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4">Example : 125000</small>
								</div>


								<div class="col-md-2 col-sm-12 form-group">
									<label>Enter Per Unit <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-2 col-sm-12 form-group">
									<asp:TextBox ID="txt_perunit" class="form-control form-control-sm rounded" placeholder="Enter PER Unit value" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_perunit" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4">Example : 1 - NOS</small>
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
										<asp:Button ID="btn_submit" runat="server" Text="Submit" class="btn btn-success btn-sm" OnClick="btn_submit_Click"/>
									</div>
								</div>
							</div>
							<%--button end--%>
						</div>
					</div>
				</div>


				<div class="col-md-12 col-sm-12">
					<div class="x_panel">
						<div class="x_title">
							<h2>View and Manage : Work order Line Items</h2>
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
												<asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="3%">
													<ItemTemplate>
														<asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Work Order Number" HeaderStyle-Width="7%">
													<ItemTemplate>
														<asp:Label ID="lbl_WO_Number" runat="server" Text='<%# Eval("WO_Number") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Item Number" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_ItemNO" runat="server" Text='<%# Eval("ItemNO") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Serial Number" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_LineNumber" runat="server" Text='<%# Eval("LineNumber") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="UOM Number" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_ServiceNumber" runat="server" Text='<%# Eval("ServiceNumber") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Item Description" HeaderStyle-Width="20%">
													<ItemTemplate>
														<asp:Label ID="lbl_Service_Description" runat="server" Text='<%# Eval("Service_Description") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Rate" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_Rate" runat="server" Text='<%# Eval("Rate") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Per Unit Value" HeaderStyle-Width="2%">
													<ItemTemplate>
														<asp:Label ID="lbl_PerUnit_Value" runat="server" Text='<%# Eval("PerUnit_Value") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Order Quantity" HeaderStyle-Width="3%">
													<ItemTemplate>
														<asp:Label ID="lbl_Order_Quantity" runat="server" Text='<%# Eval("Order_Quantity") %>' />
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
