<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="job_inpunch.aspx.cs" Inherits="WebApplication1.bussiness.production.job_inpunch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="">
			<div class="page-title">
				<div class="title_left">
					<h3>JOB Manpower Entry</h3>
				</div>
			</div>
			<div class="clearfix"></div>

			<div class="row">
				<div class="col-md-12 col-sm-12">
					<div class="x_panel">
						<div class="x_title">
							<h2>In-Punch Page<small>(Manpower Attendance)</small></h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">
							<div class="novalidate" id="InpunchPanel_Row" runat="server" visible="true">
								<span class="section">Add Manpower aganist JOBID</span>

								<div class="field item form-group">
									<label class="col-form-label col-md-3 col-sm-3 label-align">Select JOB ID<span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:DropDownList ID="DDL_JOBID" runat="server" class="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_JOBID_SelectedIndexChanged"></asp:DropDownList>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="ADDTOLIST" ControlToValidate="DDL_JOBID" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="--Select--"></asp:RequiredFieldValidator>
									</div>
								</div>

								<div class="field item form-group" id="JOBIDDetails_Row" runat="server" visible="false">
									<label class="col-form-label col-md-3 col-sm-3">Selected JOBID Details</label>
									<div class="col-md-6 col-sm-6">
										JOB_ID :<asp:Label ID="lbl_jobid" runat="server" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>;
									JOB Date :<asp:Label ID="lbl_jobiddate" runat="server" Text="Label"></asp:Label>;
									JOB Supervisor :<asp:Label ID="lbl_jobcreatorname" runat="server" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>
										<asp:Label ID="lbl_creatorwrk" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>;
									JOB Site :
									<asp:Label ID="lbl_jobsite" runat="server" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>
										<asp:Label ID="lbl_jobsitecode" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
										<asp:Label ID="lbl_creatorregion" runat="server" Visible="false" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>
										<asp:Label ID="lbl_creatorcompany" runat="server" Visible="false" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>
										<asp:Label ID="lbl_crtrsitename" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
										<asp:Label ID="lbl_crtrsitecode" runat="server" Visible="false" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>;
									JOB Work Order :
									<asp:Label ID="lbl_wrkordr" runat="server" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>;
									JOB Permit No :
									<asp:Label ID="lbl_permitno" runat="server" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>;
													<asp:Label ID="lbl_jobrgn" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
										<asp:Label ID="lbl_jobcompay" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
										In-charge Name :
									<asp:Label ID="lbl_inchargename" runat="server" Text="Label" ForeColor="blue" Font-Bold="true"></asp:Label>;
													<asp:Label ID="lbl_inchargewrk" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
										JOB Location :
									<asp:Label ID="lbl_jobloc" runat="server" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>;
									JOB Shift :
									<asp:Label ID="lbl_jobshift" runat="server" Text="Label" ForeColor="Green" Font-Bold="true"></asp:Label>
										<asp:Label ID="lbl_dept" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
									</div>
								</div>

								<div class="field item form-group" id="AttachedAttendanceRow" runat="server" visible="false">
									<div class="card-box table-responsive small">
										<asp:GridView ID="GridView2" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowDeleting="GridView2_RowDeleting">
											<Columns>
												<asp:TemplateField HeaderText="Sl. NO" Visible="True" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="ID" HeaderStyle-Width="10%" Visible="false">
													<ItemTemplate>
														<asp:Label ID="lbl_Id" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Workman" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_EmployeeWrk" runat="server" Text='<%# Bind("EmployeeWrk") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Name" HeaderStyle-Width="30%">
													<ItemTemplate>
														<asp:Label ID="lbl_EmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="IN Time" HeaderStyle-Width="50%">
													<ItemTemplate>
														<asp:Label ID="lbl_Inpunch_Time" runat="server" Text='<%# Bind("Inpunch_Time") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Action" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:ImageButton ID="btndelete" runat="server" CommandName="Delete" Height="15px" ImageUrl="~/erp_images/fi-sr-trash.svg" Width="15px" ToolTip="Delete" OnClientClick="return confirm('Do you want to DELETE...?')" ImageAlign="Middle" />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

											</Columns>
											<HeaderStyle CssClass="text text-center" />
											<EditRowStyle CssClass="bg-blue-sky" />
											<SelectedRowStyle CssClass="bg-blue-sky" />
											<EmptyDataTemplate>
												<div class="grid">No Data Found</div>
											</EmptyDataTemplate>
										</asp:GridView>

									</div>
								</div>

								<div class="field item form-group" id="WorkmanInput_Row" runat="server" visible="false">
									<label class="col-form-label col-md-3 col-sm-3 label-align">Employee Workman<span class="required">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:TextBox ID="txt_empworkman" class="form-control form-control-sm rounded" runat="server" AutoPostBack="true" placeholder="Enter Employee Workman" OnTextChanged="txt_empworkman_TextChanged"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="ADDTOLIST" ControlToValidate="txt_empworkman" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
									</div>
								</div>

								<div class="field item form-group" id="PendingOUTMsg" runat="server" visible="false">
									<label class="col-form-label col-md-3 col-sm-3  label-align">Error Message <span class="required">:</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:Label ID="lbl_pendingmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
									</div>
								</div>

								<div class="field item form-group" id="EmployeeName_Row" runat="server" visible="false">
									<label class="col-form-label col-md-3 col-sm-3  label-align">Employee Name <span class="required text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:TextBox ID="txt_empname" runat="server" ReadOnly="true" class="form-control form-control-sm rounded" Font-Bold="true" ForeColor="Black"></asp:TextBox>
										<asp:Label ID="lbl_workhours" runat="server" Text="0" Visible="false"></asp:Label>
										<asp:Label ID="lbl_designation" runat="server" Text="" Visible="true" ForeColor="Blue" Font-Bold="true"></asp:Label><asp:Label ID="lbl_designationcode" runat="server" Text="" Visible="false"></asp:Label>
										<asp:Label ID="lbl_category" runat="server" Text="" Visible="false"></asp:Label><asp:Label ID="lbl_categorycode" runat="server" Text="" Visible="false"></asp:Label>
										<asp:Label ID="lbl_pocategoryname" runat="server" Text="" Visible="false"></asp:Label><asp:Label ID="lbl_pocategorycode" runat="server" Text="" Visible="false"></asp:Label>
									</div>
								</div>

								<div class="field item form-group" id="GPValidty_row" runat="server" visible="false">
									<label class="col-form-label col-md-3 col-sm-3 label-align">Gatepass No & Validty <span class="required text-danger">*</span></label>
									<div class="col-md-4 col-sm-4">
										[<asp:Label ID="lbl_gpno" runat="server" Text="" Visible="true" ForeColor="Brown" Font-Bold="true"></asp:Label>]&nbsp;:&nbsp;
										<asp:Label ID="lbl_gpvalidty" runat="server" Text="" Visible="true" Font-Bold="true"></asp:Label>&nbsp;(<asp:Label ID="lbl_gpdays" runat="server" Text="" Font-Bold="true"></asp:Label>
										Days)
										&nbsp;<br />
										<asp:Image ID="Image1" runat="server" ImageUrl="~/erp_images/rightarrow.gif" Width="80px" Height="70px" />
										<asp:Label ID="lbl_sftyno" runat="server" Text="" Visible="false" ForeColor="Brown" Font-Bold="true"></asp:Label>
										<button type="button" class="btn btn-warning btn-sm" id="btnShowPopup2" data-toggle="modal" data-target="#myModal2" runat="server" visible="false">Update</button>
									</div>
								</div>

								<div class="field item form-group" id="EmployeeWorksite_Row" runat="server" visible="false">
									<label class="col-form-label col-md-3 col-sm-3 label-align">Employee Worksite <span class="required">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:TextBox ID="txt_worksite" runat="server" ReadOnly="true" class="form-control form-control-sm rounded"></asp:TextBox>
										<asp:Label ID="lbl_worksitecode" runat="server" Text="" Visible="false"></asp:Label>
									</div>
								</div>


								<div class="field item form-group" id="InPunchDate_Row" runat="server" visible="false">
									<label class="col-form-label col-md-3 col-sm-3 label-align">Date<span class="required">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:TextBox ID="txt_date" runat="server" CssClass="form-control form-control-sm rounded" class='date' type="date" name="date"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="ADDTOLIST" ControlToValidate="txt_date" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
									</div>
								</div>

								<div class="field item form-group" id="InPunchTime_Row" runat="server" visible="false">
									<label class="col-form-label col-md-3 col-sm-3  label-align">Time<span class="required">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:TextBox ID="txt_time" runat="server" CssClass="form-control form-control-sm rounded" class='time' type="time" name="time"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="ADDTOLIST" ControlToValidate="txt_time" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
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
							<div class="col-md-6 center-margin" id="Addto_buttons" runat="server" visible="false">
								<div class="ln_solid"></div>
								<div class="item form-group row">
									<div class="col-md-6 col-sm-12">
										<asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to ADD Data!!"></asp:Label>
									</div>
									<div class="col-md-6 col-sm-12">
										<button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
										<asp:Button ID="btn_reset" runat="server" Text="RESET" CausesValidation="true" CssClass="btn btn-warning btn-sm" OnClick="btn_reset_Click" />
										<asp:Button ID="btn_submit" runat="server" Text="ADD TO LIST" ValidationGroup="ADDTOLIST" CausesValidation="true" CssClass="btn btn-success btn-sm" OnClick="btn_submit_Click" />
									</div>
								</div>
							</div>
							<%--ADD button end--%>

							<div class="col-md-12 col-sm-12" id="ViewState_TableRow" runat="server" visible="false">
								<div class="card-box table-responsive">
									<asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowDeleting="GridView1_RowDeleting">
										<Columns>
											<asp:TemplateField HeaderText="Sl" Visible="True" HeaderStyle-Width="5%">
												<ItemTemplate>
													<asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
												</ItemTemplate>
												<HeaderStyle CssClass="grid" />
												<ItemStyle CssClass="grid" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="WSL" HeaderStyle-Width="10%">
												<ItemTemplate>
													<asp:Label ID="lbl_wrk" runat="server" Text='<%# Bind("wrk") %>'></asp:Label>
												</ItemTemplate>
												<HeaderStyle CssClass="grid" />
												<ItemStyle CssClass="grid" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="Name" HeaderStyle-Width="20%">
												<ItemTemplate>
													<asp:Label ID="lbl_name" runat="server" Text='<%# Bind("name") %>'></asp:Label>
												</ItemTemplate>
												<HeaderStyle CssClass="grid" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="Hours" Visible="false">
												<ItemTemplate>
													<asp:Label ID="lbl_wrkhrs" runat="server" Text='<%# Bind("wrkhrs") %>'></asp:Label>
												</ItemTemplate>
												<HeaderStyle CssClass="grid" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="Hours" Visible="false">
												<ItemTemplate>
													<asp:Label ID="lbl_category" runat="server" Text='<%# Bind("category") %>'></asp:Label>
												</ItemTemplate>
												<HeaderStyle CssClass="grid" />
											</asp:TemplateField>
											<asp:TemplateField HeaderText="Hours" Visible="false">
												<ItemTemplate>
													<asp:Label ID="lbl_categorycode" runat="server" Text='<%# Bind("categorycode") %>'></asp:Label>
												</ItemTemplate>
												<HeaderStyle CssClass="grid" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="Hours" Visible="false">
												<ItemTemplate>
													<asp:Label ID="lbl_po_category" runat="server" Text='<%# Bind("po_category") %>'></asp:Label>
												</ItemTemplate>
												<HeaderStyle CssClass="grid" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="Hours" Visible="false">
												<ItemTemplate>
													<asp:Label ID="lbl_po_categorycode" runat="server" Text='<%# Bind("po_categorycode") %>'></asp:Label>
												</ItemTemplate>
												<HeaderStyle CssClass="grid" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="Hours" Visible="false">
												<ItemTemplate>
													<asp:Label ID="lbl_designation" runat="server" Text='<%# Bind("designation") %>'></asp:Label>
												</ItemTemplate>
												<HeaderStyle CssClass="grid" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="Hours" Visible="false">
												<ItemTemplate>
													<asp:Label ID="lbl_designationcode" runat="server" Text='<%# Bind("designationcode") %>'></asp:Label>
												</ItemTemplate>
												<HeaderStyle CssClass="grid" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="Hours" Visible="false">
												<ItemTemplate>
													<asp:Label ID="lbl_gpno" runat="server" Text='<%# Bind("gpno") %>'></asp:Label>
												</ItemTemplate>
												<HeaderStyle CssClass="grid" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="Hours" Visible="false">
												<ItemTemplate>
													<asp:Label ID="lbl_sftyno" runat="server" Text='<%# Bind("sftyno") %>'></asp:Label>
												</ItemTemplate>
												<HeaderStyle CssClass="grid" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="SiteName" Visible="false">
												<ItemTemplate>
													<asp:Label ID="lbl_wrksitename" runat="server" Text='<%# Bind("wrksitename") %>'></asp:Label>
												</ItemTemplate>
												<HeaderStyle CssClass="grid" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="SiteCode" Visible="false">
												<ItemTemplate>
													<asp:Label ID="lbl_wrksitecode" runat="server" Text='<%# Bind("wrksitecode") %>'></asp:Label>
												</ItemTemplate>
												<HeaderStyle CssClass="grid" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="IN" HeaderStyle-Width="20%">
												<ItemTemplate>
													<asp:Label ID="lbl_in" runat="server" Text='<%# Bind("in") %>'></asp:Label>
												</ItemTemplate>
												<HeaderStyle CssClass="grid" />
												<ItemStyle CssClass="grid" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="Action" HeaderStyle-Width="5%">
												<ItemTemplate>
													<asp:ImageButton ID="btndelete" runat="server" CommandName="Delete" Height="15px" ImageUrl="~/erp_images/fi-sr-trash.svg" Width="15px" ToolTip="Delete" OnClientClick="return confirm('Do you want to DELETE...?')" ImageAlign="Middle" />
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

							<div class="row" id="AttendanceSentMesg" runat="server" visible="false">
								<div class="col-md-12 col-sm-12" style="vertical-align: middle; text-align: center;">
									<asp:Image ID="Img_Success" runat="server" ImageUrl="~/erp_images/success_gif.gif" Width="100px" Height="100px" />
									<asp:Label ID="Label1" runat="server" Text="Manpower Attendance Sent..!" Font-Bold="true" Font-Size="Large"></asp:Label>
								</div>
							</div>

							<%--button start--%>
							<div class="col-md-6 center-margin" id="inpunched_buttons" runat="server" visible="false">
								<div class="ln_solid"></div>
								<div class="col-md-12 col-sm-12 center" style="text-align: center;">
									<asp:Button ID="btn_tbtpage" runat="server" Text="Online TBT" CssClass="btn btn-primary btn-sm" Visible="false" OnClick="btn_tbtpage_Click" />
									<asp:Button ID="btn_soppage" runat="server" Text="Online SOP" CssClass="btn btn-primary btn-sm" Visible="false" PostBackUrl="~/bussiness/production/csm_soptraining.aspx" />
									<asp:Button ID="btn_hmpg" runat="server" Text="Home" CssClass="btn btn-primary btn-sm" Visible="true" OnClick="btn_hmpg_Click" />
								</div>
							</div>
							<%--button end--%>

							<%--Final SAVE button start--%>
							<div class="col-md-6 center-margin" id="SendAttendance_Buttons" runat="server" visible="false">
								<div class="ln_solid"></div>
								<div class="item form-group row">
									<div class="col-md-6 col-sm-12">
										<asp:Label ID="lbl_finalmsg" runat="server" Text="Click SUBMIT to SEND Data!!"></asp:Label>
									</div>
									<div class="col-md-6 col-sm-12">
										<button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
										<asp:Button ID="btn_finalsubmit" runat="server" Text="SAVE" CssClass="btn btn-success btn-sm" OnClick="btn_finalsubmit_Click" />
									</div>
								</div>
							</div>
							<%--Final SAVE button end--%>
						</div>
					</div>
				</div>
			</div>
		</div>

		<%--- Up-loader Modal ---------START----%>
		<div class="modal fade" id="myModal2" data-backdrop="static">
			<div class="modal-dialog">
				<div class="modal-content">
					<div class="modal-header">
						<h4 class="modal-title">Update Gatepass Details</h4>
						<button type="button" class="close" data-dismiss="modal">&times;</button>
					</div>
					<div class="modal-body">
						<div class="row">
							<div class="col-md-12">
								<div class="form-group">

									<div class="col-md-6 col-sm-12 form-group">
										<label>Gatepass No :<span class="text text-danger"></span></label>
									</div>
									<div class="col-md-6 col-sm-12 form-group">
										<asp:Label ID="lbl_oldgpno" runat="server" Text="N/A" CssClass="form-control form-control-sm rounded" BackColor="#e6e6e6"></asp:Label>
									</div>

									<div class="col-md-6 col-sm-12 form-group" id="nwgprow1" runat="server" visible="false">
										<label style="font-weight: bold; color: darkblue;">Enter New Gatepass No :<span class="text text-danger"></span></label>
									</div>
									<div class="col-md-6 col-sm-12 form-group" id="nwgprow2" runat="server" visible="false">
										<asp:TextBox ID="txt_nwgpno" runat="server" CssClass="form-control form-control-sm rounded" BorderColor="Blue" BorderWidth="2px"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="GPDATA" ControlToValidate="txt_nwgpno" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
									</div>

									<div class="col-md-6 col-sm-12 form-group">
										<label>Gatepass Validity :<span class="text text-danger"></span></label>
									</div>
									<div class="col-md-6 col-sm-12 form-group">
										<asp:Label ID="lbl_oldgpvalidity" runat="server" Text="N/A" CssClass="form-control form-control-sm rounded" BackColor="#e6e6e6"></asp:Label>
									</div>

									<div class="col-md-6 col-sm-12 form-group" id="nwgpvalrow1" runat="server" visible="false">
										<label style="font-weight: bold; color: darkblue;">New Gatepass Validity :<span class="text text-danger"></span></label>
									</div>
									<div class="col-md-6 col-sm-12 form-group" id="nwgpvalrow2" runat="server" visible="false">
										<asp:TextBox ID="txt_nwgpvalidity" runat="server" CssClass="form-control form-control-sm rounded" class='date' type="date" name="date" BorderColor="Blue" BorderWidth="2px"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="GPDATA" ControlToValidate="txt_nwgpvalidity" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
									</div>

									<div class="col-md-6 col-sm-12 form-group">
										<label>Safety No :<span class="text text-danger"></span></label>
									</div>
									<div class="col-md-6 col-sm-12 form-group">
										<asp:Label ID="lbl_oldsftyno" runat="server" Text="N/A" CssClass="form-control form-control-sm rounded" BackColor="#e6e6e6"></asp:Label>
									</div>

									<div class="col-md-6 col-sm-12 form-group" id="nwsftyrow1" runat="server" visible="false">
										<label style="font-weight: bold; color: darkblue;">Enter New Safety No :<span class="text text-danger"></span></label>
									</div>
									<div class="col-md-6 col-sm-12 form-group" id="nwsftyrow2" runat="server" visible="false">
										<asp:TextBox ID="txt_nwsftyno" runat="server" CssClass="form-control form-control-sm rounded" BorderColor="Blue" BorderWidth="2px"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator7" ValidationGroup="GPDATA" ControlToValidate="txt_nwsftyno" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
									</div>

									<div class="col-md-6 col-sm-12 form-group">
										<label>Safety Validity :<span class="text text-danger"></span></label>
									</div>
									<div class="col-md-6 col-sm-12 form-group">
										<asp:Label ID="lbl_oldsftyval" runat="server" Text="N/A" CssClass="form-control form-control-sm rounded"></asp:Label>
									</div>

									<div class="col-md-6 col-sm-12 form-group" id="nwrfidrow1" runat="server" visible="false">
										<label style="font-weight: bold; color: darkblue;">New Safety Validity :<span class="text text-danger"></span></label>
									</div>
									<div class="col-md-6 col-sm-12 form-group" id="nwrfidrow2" runat="server" visible="false">
										<asp:TextBox ID="txt_nwsftyvalidity" runat="server" CssClass="form-control form-control-sm rounded" class='date' type="date" name="date" BorderColor="Blue" BorderWidth="2px"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator8" ValidationGroup="GPDATA" ControlToValidate="txt_nwsftyvalidity" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
									</div>

									<div class="col-md-6 col-sm-12 form-group">
										<label>PV Validity :<span class="text text-danger"></span></label>
									</div>
									<div class="col-md-6 col-sm-12 form-group">
										<asp:Label ID="lbl_oldpvvalidity" runat="server" Text="N/A" CssClass="form-control form-control-sm rounded"></asp:Label>
									</div>

									<div class="col-md-6 col-sm-12 form-group" id="nwpvrow1" runat="server" visible="false">
										<label style="font-weight: bold; color: darkblue;">New PV Validity :<span class="text text-danger"></span></label>
									</div>
									<div class="col-md-6 col-sm-12 form-group" id="nwpvrow2" runat="server" visible="false">
										<asp:TextBox ID="txt_nwpvvalidity" runat="server" CssClass="form-control form-control-sm rounded" class='date' type="date" name="date" BorderColor="Blue" BorderWidth="2px"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator9" ValidationGroup="GPDATA" ControlToValidate="txt_nwpvvalidity" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
									</div>
								</div>
								<asp:Label ID="Label2" runat="server"></asp:Label>
							</div>
						</div>
					</div>
					<div class="modal-footer">
						<asp:Button ID="btn_gtpsedit" runat="server" CausesValidation="true" ValidationGroup="GPDATA" Text="Make Changes" CssClass="btn btn-info btn-sm" OnClick="btn_gtpsedit_Click" />
						<asp:Button ID="btn_cancel" runat="server" CausesValidation="false" Text="Cancel" CssClass="btn btn-warning btn-sm" OnClick="btn_cancel_Click" />
						<button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">Close</button>
					</div>
				</div>
			</div>
		</div>
		<%--- Up-loader Modal -------- END --%>
	</div>
	<script type="text/javascript">
		function ShowPopup(title, body) {
			$("#MyPopup .modal-title").html(title);
			$("#MyPopup .modal-body").html(body);
			$("#MyPopup").modal("show");
		}

		function ShowPopup2() {
			$("#myModal2").modal("show");
		}
	</script>
</asp:Content>
