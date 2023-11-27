<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="swap_jobdate.aspx.cs" Inherits="WebApplication1.bussiness.production.swap_jobdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="">
			<div class="page-title">
				<div class="title_left">
					<h4>View JOB Details by JOBID</h4>
				</div>
			</div>
			<div class="clearfix"></div>

			<div class="row">
				<div class="col-md-12 col-sm-12">
					<div class="x_panel">
						<div class="x_content">
							<div class="novalidate">

								<div class="field item form-group" id="JOBIDinputrow" runat="server" visible="true">
									<label class="col-form-label col-md-3 col-sm-3 label-align">Enter JOBID<span class="required">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:TextBox ID="txt_jobid" class="form-control form-control-sm rounded" runat="server" placeholder="JOB00___"></asp:TextBox>
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
										<asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to Fetch Data!!"></asp:Label>
									</div>
									<div class="col-md-6 col-sm-12">
										<asp:Button ID="btn_search" runat="server" Text="SEARCH" CssClass="btn btn-success btn-sm" OnClick="btn_search_Click" />
										<asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="btn btn-danger btn-sm" OnClick="btn_cancel_Click" />
										<asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="btn btn-warning btn-sm" OnClick="btn_reset_Click" />
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>

				<div class="col-md-12 col-sm-12" id="detailsrow1" runat="server" visible="false">
					<div class="x_panel">
						<div class="x_title">
							<h2>Attached Manpower Data & Details</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
								<%--<li><a class="close-link"><i class="fa fa-close"></i></a></li>--%>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">
							<div class="row">
								<div class="col-md-3 col-sm-6  form-group">
									<label>JOB ID </label>
								</div>
								<div class="col-md-3 col-sm-6  form-group">
									<asp:TextBox ID="txt_jobiddb" class="form-control form-control-sm rounded" runat="server" ReadOnly="true"></asp:TextBox>
								</div>


								<div class="col-md-3 col-sm-12  form-group">
									<label>JOB DATE</label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_jobdate" class="form-control form-control-sm rounded" runat="server" ReadOnly="true"></asp:TextBox>
									<asp:Label ID="lbl_jobday" runat="server" Text=""></asp:Label>
								</div>

								<div class="col-md-3 col-sm-12  form-group" id="swapdate1" runat="server" visible="false">
									<label style="color: blue; font-weight: bold; background-color: aquamarine;">Select New JOB DATE</label>
								</div>
								<div class="col-md-3 col-sm-12  form-group" id="swapdate2" runat="server" visible="false">
									<asp:TextBox ID="txt_date" runat="server" CssClass="form-control form-control-sm rounded" class='date' type="date" name="date"></asp:TextBox>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Work Order No </label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_workorderno" class="form-control form-control-sm rounded" runat="server" ReadOnly="true"></asp:TextBox>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>JOB ID Status </label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:Label ID="lbl_jobidsstatus" runat="server" Text="N/A" Font-Bold="true"></asp:Label>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Work Site Name </label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_worksitename" class="form-control form-control-sm rounded" runat="server" ReadOnly="true"></asp:TextBox>
									<asp:Label ID="lbl_worksitedbcode" runat="server" Text="Label" Visible="true"></asp:Label>
								</div>

								<div class="col-md-3 col-sm-12 form-group" id="worksite_row1" runat="server" visible="false">
									<label style="color: blue; font-weight: bold; background-color: aquamarine;">Select New Work-Site<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group" id="worksite_row2" runat="server" visible="false">
									<asp:DropDownList ID="DDL_Worksite" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Worksite_SelectedIndexChanged"></asp:DropDownList>
									<asp:Label ID="lbl_worksitecode" runat="server" Text=""></asp:Label>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Site In-Charge Name </label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_inchargename" class="form-control form-control-sm rounded" runat="server" ReadOnly="true"></asp:TextBox>
									<asp:Label ID="lbl_inchargewrk" runat="server" Text="Label" Visible="true"></asp:Label>
								</div>

								<div class="col-md-3 col-sm-12 form-group" id="approver_row1" runat="server" visible="false">
									<label style="color: blue; font-weight: bold; background-color: aquamarine;">Select New Approver<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group" id="approver_row2" runat="server" visible="false">
									<asp:DropDownList ID="DDL_Approver" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>JOB Department </label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_jobdept" class="form-control form-control-sm rounded" runat="server" ReadOnly="true"></asp:TextBox>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>JOB Work Location </label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_jobloc" class="form-control form-control-sm rounded" runat="server" ReadOnly="true"></asp:TextBox>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>JOB Shift </label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_jobshift" class="form-control form-control-sm rounded" runat="server" ReadOnly="true"></asp:TextBox>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>JOB Title </label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_jobtitle" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" TextMode="MultiLine" Rows="3"></asp:TextBox>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Work Permit No </label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_permitno" class="form-control form-control-sm rounded" runat="server" ReadOnly="true"></asp:TextBox>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Upload Status </label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:Label ID="lbl_prmtupldstatus" runat="server" Text="" Font-Bold="true"></asp:Label>
								</div>

								<div class="col-md-3 col-sm-12 form-group" id="FileCount_Row1" runat="server" visible="true">
									<asp:Label ID="Label6" runat="server" Text="Total File Count"></asp:Label>
								</div>
								<div class="col-md-3 col-sm-12 form-group" id="FileCount_Row2" runat="server" visible="true">
									<asp:Label ID="lbl_filecount" runat="server" Text="0" Font-Bold="true"></asp:Label>
								</div>

								<div class="col-md-3 col-sm-12 form-group" id="PrmtUpldDate1" runat="server" visible="true">
									<asp:Label ID="Label8" runat="server" Text="Upload Date"></asp:Label>
								</div>
								<div class="col-md-3 col-sm-12 form-group" id="PrmtUpldDate2" runat="server" visible="true">
									<asp:Label ID="lbl_permituploaddate" runat="server" Text="" Font-Bold="true"></asp:Label>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Approval Status </label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:Label ID="lbl_approvalstatus" runat="server" Text="" Font-Bold="true"></asp:Label>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Approver Remarks </label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_approverrmrks" class="form-control form-control-sm rounded" runat="server" ReadOnly="true"></asp:TextBox>
								</div>
							</div>

							<%--form buttons div ---- start--%>
							<div class="col-md-6 center-margin" runat="server" visible="true">
								<div class="ln_solid"></div>
								<div class="item form-group row">
									<div class="col-md-6 col-sm-12">
										<asp:Button ID="btn_update" runat="server" Text="Update" CssClass="btn btn-success btn-sm" Enabled="false" OnClick="btn_update_Click" />
										<asp:Button ID="btn_updatedate" runat="server" Text="Swap Date" CssClass="btn btn-primary btn-sm" Enabled="true" Visible="true" OnClick="btn_updatedate_Click" />
										<asp:Button ID="btn_cancelupdate" runat="server" Text="Cancel" CssClass="btn btn-danger btn-sm" Enabled="false" Visible="false" OnClick="btn_cancelupdate_Click" />
										<asp:Button ID="btn_clear" runat="server" Text="Reset" CssClass="btn btn-warning btn-sm" Enabled="true" Visible="true" OnClick="btn_clear_Click" />
									</div>

									<div class="col-md-6 col-sm-12">
										<asp:Label ID="Label1" runat="server" Text=""></asp:Label>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>

				<div class="col-md-12 col-sm-12" id="detailsrow2" runat="server" visible="false">
					<div class="x_panel">
						<div class="x_title">
							<h2>Attached Permit Data</h2>
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
										<p class="text-muted font-13 m-b-30">
											Last attached permit file deleted on :
											<asp:Label ID="lbl_permitdeleteddate" runat="server" Text=""></asp:Label>, by
											<asp:Label ID="lbl_permitdeletedby" runat="server" Text=""></asp:Label>
										</p>
										<asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowDeleting="GridView1_RowDeleting">
											<Columns>
												<asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="DB ID" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="JOBID" Visible="true">
													<ItemTemplate>
														<asp:Label ID="lbl_JOBID" runat="server" Text='<%# Bind("JOBID") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="File Name" HeaderStyle-Width="60%">
													<ItemTemplate>
														<asp:Label ID="lbl_Name" runat="server" Text='<%# Eval("Name") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="View Permit" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("Id") %>' OnClick="DownloadFile" ToolTip="Click to Download" CommandName="Download"><span class="glyphicon glyphicon-save" aria-hidden='true'></span></asp:LinkButton>
														<asp:Label ID="lbl_dbid" runat="server" Visible="false" Text='<%# Bind("Id") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Delete" HeaderStyle-Width="5%" Visible="false">
													<ItemTemplate>
														<asp:ImageButton ID="btndelete" runat="server" CommandName="Delete" Height="15px" ImageUrl="~/erp_images/fi-sr-trash.svg" Width="15px" ToolTip="Delete" OnClientClick="return confirm('Do you want to DELETE...?')" ImageAlign="Middle" />
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

				<div class="col-md-12 col-sm-12" id="detailsrow3" runat="server" visible="false">
					<div class="x_panel">
						<div class="x_title">
							<h2>Attached Manpower Data & Details</h2>
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
										<asp:GridView ID="GridView2" runat="server" Font-Size="Smaller" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowCancelingEdit="GridView2_RowCancelingEdit" OnRowDeleting="GridView2_RowDeleting" OnRowEditing="GridView2_RowEditing" OnRowUpdating="GridView2_RowUpdating" OnRowDataBound="GridView2_RowDataBound">
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

												<asp:TemplateField HeaderText="JOBID" Visible="false" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_JOBID" runat="server" Text='<%# Bind("JOBID") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="JOB Date / Attendance Date" HeaderStyle-Width="7%">
													<ItemTemplate>
														<asp:Label ID="lbl_CreatedDate" runat="server" Text='<%# Bind("CreatedDate","{0:dd-MM-yyyy}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="12%">
													<ItemTemplate>
														<asp:Label ID="lbl_EmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
														[<asp:Label ID="lbl_EmployeeWrk" runat="server" Text='<%# Bind("EmployeeWrk") %>'></asp:Label>]
													</ItemTemplate>
													<ItemStyle CssClass="text text-left" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Site" HeaderStyle-Width="12%" Visible="false">
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

												<asp:TemplateField HeaderText="IN-Punch Time" HeaderStyle-Width="12%">
													<ItemTemplate>
														<asp:Label ID="lbl_Inpunch_Time" runat="server" Text='<%# Bind("Inpunch_Time","{0:dd-MM-yyyy hh:mm:ss tt}") %>'></asp:Label>
													</ItemTemplate>
													<EditItemTemplate>
														<asp:TextBox ID="txt_Inpunch_Time" runat="server" class="form-control form-control-sm rounded small" Text='<%# DataBinder.Eval(Container.DataItem,"Inpunch_Time", "{0:yyyy-MM-dd hh:mm:ss tt}") %> ' Width="100%"></asp:TextBox>
													</EditItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="OUT-Punch Time" HeaderStyle-Width="12%">
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

												<asp:TemplateField HeaderText="P OT" HeaderStyle-Width="4%">
													<ItemTemplate>
														<asp:Label ID="lbl_ProvidedOT" runat="server" Text='<%# Bind("ProvidedOT") %>'></asp:Label>
													</ItemTemplate>
													<EditItemTemplate>
														<asp:TextBox ID="txt_ProvidedOT" runat="server" TextMode="MultiLine" Rows="1" Columns="5" class="form-control form-control-sm rounded" Text='<%# DataBinder.Eval(Container.DataItem,"ProvidedOT") %>' Width="100%"></asp:TextBox>
														<asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="txt_ProvidedOT" Display="Dynamic" ErrorMessage="Value must be less than or equal to 16" ForeColor="IndianRed" ValidationGroup="Update" ClientValidationFunction="validateProvidedOT"></asp:CustomValidator>
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
														<asp:ImageButton ID="btnupdate" runat="server" CommandName="Update" Height="15px" ImageUrl="~/erp_images/fi-sr-disk.svg" Width="15px" ToolTip="Save" ImageAlign="Middle" />
														<asp:ImageButton ID="Btncancale" runat="server" CommandName="Cancel" Height="15px" ImageUrl="~/erp_images/fi-sr-cross-circle.svg" Width="15px" ToolTip="Cancel Update" ImageAlign="Middle" />
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
	</div>
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
