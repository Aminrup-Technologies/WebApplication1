<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="vw_manpowerusage.aspx.cs" Inherits="WebApplication1.bussiness.production.vw_manpowerusage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript">
			function ShowPopup(title, body) {
				$("#MyPopup .modal-title").html(title);
				$("#MyPopup .modal-body").html(body);
				$("#MyPopup").modal("show");
			}
		</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="container">
			<%--<div class="page-title">
				<div class="title_left">
					<h5>Main Heading</h5>
				</div>
			</div>--%>

			<%--<div class="clearfix"></div>--%>

			<div class="row">
				<div class="col-md-12 col-sm-12  ">
					<div class="x_panel">
						<div class="x_title">
							<h2>Manpower Utilization View</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">
							<div class="row">

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select Calender / JOB date <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_date" runat="server" CssClass="form-control form-control-sm rounded" class='date' type="date" name="date"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="GetResult" ControlToValidate="txt_date" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
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
										<asp:Button ID="btn_submit" runat="server" Text="Submit" ValidationGroup="GetResult" CssClass="btn btn-success btn-sm" OnClick="btn_submit_Click" />
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
							<h2>View and Manage : Manpower Duplicate Entries</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">

							<div class="row">
								<div class="col-md-4 col-sm-6">
									<div class="card-box col-md-12 col-sm-12 small">
										<asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-sm" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowCommand="GridView1_RowCommand">
											<Columns>
												<asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="JOB Date" HeaderStyle-Width="30%" Visible="false">
													<ItemTemplate>
														<asp:Label ID="lbl_CreatedDate" runat="server" Text='<%# Eval("CreatedDate","{0:dd-MM-yyyy}") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Workman" HeaderStyle-Width="50%" Visible="true">
													<ItemTemplate>
														<asp:Label ID="lbl_EmployeeName" runat="server" Text='<%# Eval("EmployeeName") %>' />
														<asp:Label ID="lbl_EmployeeWrk" runat="server" Text='<%# Eval("EmployeeWrk") %>' Visible="false" />
														<asp:Button ID="btn_viewdetails" runat="server" Text='<%# Eval("EmployeeWrk") %>' Font-Size="Smaller" CssClass="btn btn-sm btn-info" CommandName="View_Details" CommandArgument="<%# Container.DataItemIndex %>" />
													</ItemTemplate>
													<ItemStyle CssClass="text text-left" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Count" HeaderStyle-Width="10%" Visible="true">
													<ItemTemplate>
														<asp:Label ID="lbl_DuplicateCount" runat="server" Text='<%# Eval("DuplicateCount") %>' Font-Bold="true" ForeColor="Black" />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>
											</Columns>

											<EmptyDataTemplate>
												<div class="grid">No Data Found</div>
											</EmptyDataTemplate>
										</asp:GridView>
									</div>
								</div>

								<div class="col-md-8 col-sm-6" id="SecondGrid" runat="server" visible="false">
									<div class="card-box col-md-12 col-sm-12 small">
										<asp:GridView ID="GridView2" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-sm" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found">
											<Columns>
												<asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="2%">
													<ItemTemplate>
														<asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="JOB Date" HeaderStyle-Width="10%" Visible="true">
													<ItemTemplate>
														<asp:Label ID="lbl_CreatedDate" runat="server" Text='<%# Eval("CreatedDate","{0:dd-MM-yyyy}") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="JOBID" Visible="true" HeaderStyle-Width="8%">
													<ItemTemplate>
														<asp:Label ID="lbl_JOBID" runat="server" Text='<%# Bind("JOBID") %>' Font-Bold="true" ForeColor="Blue"></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="WRK" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_EmployeeWrk" runat="server" Text='<%# Bind("EmployeeWrk") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="IN Punch Time" HeaderStyle-Width="20%">
													<ItemTemplate>
														<asp:Label ID="lbl_Inpunch_Time" runat="server" Text='<%# Bind("Inpunch_Time","{0:dd-MM-yyyy hh:mm:ss tt}") %>' Font-Bold="true"></asp:Label>
													</ItemTemplate>
													<EditItemTemplate>
														<asp:TextBox ID="txt_Inpunch_Time" runat="server" class="form-control form-control-sm rounded small" Text='<%# DataBinder.Eval(Container.DataItem,"Inpunch_Time", "{0:yyyy-MM-dd hh:mm:ss tt}") %> ' Width="100%"></asp:TextBox>
													</EditItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="OUT Punch Time" HeaderStyle-Width="20%">
													<ItemTemplate>
														<asp:Label ID="lbl_Outpunch_Time" runat="server" Text='<%# Bind("Outpunch_Time","{0:dd-MM-yyyy hh:mm:ss tt}") %>' Font-Bold="true"></asp:Label>
													</ItemTemplate>
													<EditItemTemplate>
														<asp:TextBox ID="txt_Outpunch_Time" runat="server" class="form-control form-control-sm rounded small" Text='<%# DataBinder.Eval(Container.DataItem,"Outpunch_Time", "{0:yyyy-MM-dd hh:mm:ss tt}") %> ' Width="100%"></asp:TextBox>
													</EditItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Lunch" HeaderStyle-Width="5%">
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

												<asp:TemplateField HeaderText="POT" HeaderStyle-Width="6%">
													<ItemTemplate>
														<asp:Label ID="lbl_ProvidedOT" runat="server" Text='<%# Bind("ProvidedOT") %>'></asp:Label>
													</ItemTemplate>
													<EditItemTemplate>
														<asp:TextBox ID="txt_ProvidedOT" runat="server" class="form-control form-control-sm rounded" Text='<%# DataBinder.Eval(Container.DataItem,"ProvidedOT") %>' Width="100%"></asp:TextBox>
														<asp:CustomValidator ID="CustomValidator2" runat="server" ValidationGroup="Update" ControlToValidate="txt_ProvidedOT" Display="Dynamic" ErrorMessage="Value must be less than or equal to 16" ForeColor="IndianRed" ClientValidationFunction="validateProvidedOT"></asp:CustomValidator>
													</EditItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>
												<asp:TemplateField HeaderText="Atten Status" HeaderStyle-Width="3%" Visible="false">
													<ItemTemplate>
														<asp:Label ID="lbl_AttendanceStatus" runat="server" Text='<%# Bind("AttendanceStatus") %>'></asp:Label>
													</ItemTemplate>
													<EditItemTemplate>
														<asp:DropDownList ID="DDL_AttendanceStatus" class="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
													</EditItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Code" HeaderStyle-Width="3%">
													<ItemTemplate>
														<asp:Label ID="lbl_AttendanceCode" runat="server" Text='<%# Bind("AttendanceCode") %>'></asp:Label>
													</ItemTemplate>
													<EditItemTemplate>
														<asp:DropDownList ID="DDL_AttendanceCode" class="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
													</EditItemTemplate>
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
</asp:Content>
