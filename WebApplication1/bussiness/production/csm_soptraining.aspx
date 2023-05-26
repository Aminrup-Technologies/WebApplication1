<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="csm_soptraining.aspx.cs" Inherits="WebApplication1.bussiness.production.csm_soptraining" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="">
			<div class="page-title">
				<div class="title_left">
					<h3>Online SOP Training</h3>
				</div>
			</div>
			<div class="clearfix"></div>

			<div class="row">
				<div class="col-md-12 col-sm-12">
					<div class="x_panel">
						<div class="x_title">
							<h2>DOC#ATS/CSM/SOP-01</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">
							<div class="row" id="jobselectionpanel" runat="server" visible="true">

								<div class="col-md-6 col-sm-12 form-group">
									<label>Select JOB ID<span class="text text-danger">*</span></label>
								</div>

								<div class="col-md-6 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_JOBID" runat="server" class="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_JOBID_SelectedIndexChanged"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="CreateTBTID" ControlToValidate="DDL_JOBID" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="--Select--"></asp:RequiredFieldValidator>
								</div>

								<div class="col-md-12 col-sm-12  form-group" id="JOBIDDetails_Row" runat="server" visible="false">
									<label class="col-form-label col-md-3 col-sm-3">Selected JOBID Details</label>
									<div class="col-md-9 col-sm-6">
										JOB_ID :<asp:Label ID="lbl_jobid" runat="server" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>;
										JOB Date :<asp:Label ID="lbl_jobiddate" runat="server" Text="Label" ForeColor="Blue" Font-Bold="true"></asp:Label>;
										JOB Supervisor :<asp:Label ID="lbl_jobcreatorname" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="true"></asp:Label>
										<asp:Label ID="lbl_creatorwrk" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>;
										JOB Site :<asp:Label ID="lbl_jobsite" runat="server" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>
										<asp:Label ID="lbl_jobsitecode" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
										<asp:Label ID="lbl_creatorregion" runat="server" Visible="false" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>
										<asp:Label ID="lbl_creatorcompany" runat="server" Visible="false" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>
										<asp:Label ID="lbl_crtrsitename" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
										<asp:Label ID="lbl_crtrsitecode" runat="server" Visible="false" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>;
										JOB Work Order :<asp:Label ID="lbl_wrkordr" runat="server" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>;
										JOB Permit No :<asp:Label ID="lbl_permitno" runat="server" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>;
											<asp:Label ID="lbl_jobrgn" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>;
										JOB Title :<asp:Label ID="lbl_jobtitle" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="true"></asp:Label>
										<asp:Label ID="lbl_jobcompay" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>;
										Site In-Charge :<asp:Label ID="lbl_inchargename" runat="server" Text="Label" ForeColor="blue" Font-Bold="true" Visible="true"></asp:Label>
										<asp:Label ID="lbl_inchargewrk" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>;
										JOB Work Location :<asp:Label ID="lbl_jobloc" runat="server" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>;
										JOB Shift :<asp:Label ID="lbl_jobshift" runat="server" Text="Label" ForeColor="Blue" Font-Bold="true"></asp:Label>;
											<asp:Label ID="lbl_dept" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
									</div>
								</div>

								<div class="col-md-12 col-sm-12  form-group" id="AttachedAttendanceRow" runat="server" visible="false">
									<div class="card-box table-responsive small center">
										<asp:GridView ID="EmpGrid" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" ShowFooter="true" OnRowDataBound="EmpGrid_RowDataBound" OnRowCreated="EmpGrid_RowCreated">
											<Columns>
												<asp:TemplateField HeaderText="Sl. NO" Visible="True" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Workman" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_EmployeeWrk" runat="server" Text='<%# Bind("EmployeeWrk") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="30%">
													<ItemTemplate>
														<asp:Label ID="lbl_EmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>' DataFormatString="{0:D}" ForeColor="Blue" Font-Bold="true"></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Employee Designation" HeaderStyle-Width="30%">
													<ItemTemplate>
														<asp:Label ID="lbl_EmpDesignation" runat="server" Text='<%# Bind("EmpDesignation") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Gatepass No" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_GatePassNo" runat="server" Text='<%# Bind("GatePassNo") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

											</Columns>
											<HeaderStyle CssClass="text text-center" />
											<EditRowStyle CssClass="bg-blue-sky" />
											<FooterStyle CssClass="text text-center font-weight-bold" />
											<SelectedRowStyle CssClass="bg-blue-sky" />
											<EmptyDataTemplate>
												<div class="grid">No Data Found</div>
											</EmptyDataTemplate>
										</asp:GridView>

									</div>
								</div>


								<div class="col-md-6 col-sm-12  form-group" id="inchargerow" runat="server" visible="false">
									<label class="col-form-label col-md-6 col-sm-3">Contractor Representative <span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:TextBox ID="txt_incharge" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
									</div>
								</div>

								<div class="col-md-6 col-sm-12  form-group" id="ContractEmployeesrow" runat="server" visible="false">
									<label class="col-form-label col-md-6 col-sm-3">Contract Employees :<span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:TextBox ID="txt_cntrctemp" runat="server" class="form-control form-control-sm rounded"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="CreateSOPID" ControlToValidate="txt_cntrctemp" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
									</div>
								</div>

								<div class="col-md-12 col-sm-12  form-group" id="locationrow" runat="server" visible="false">
									<label class="col-form-label col-md-3 col-sm-3 label-align">Location<span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:TextBox ID="txt_location" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
									</div>
								</div>

								<div class="col-md-12 col-sm-12  form-group" id="deptrow" runat="server" visible="false">
									<label class="col-form-label col-md-3 col-sm-3 label-align">Department<span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:TextBox ID="txt_dept" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
									</div>
								</div>

								<div class="col-md-6 col-sm-12  form-group" id="sftyofcrrow" runat="server" visible="false">
									<label class="col-form-label col-md-6 col-sm-3">Safety Officer<span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:DropDownList ID="DDL_SftyOfcr" runat="server" class="form-control form-control-sm rounded"></asp:DropDownList>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="CreateSOPID" ControlToValidate="DDL_SftyOfcr" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
									</div>
								</div>

								<div class="col-md-6 col-sm-12  form-group" id="sftysupvrow" runat="server" visible="false">
									<label class="col-form-label col-md-6 col-sm-3">Safety Supervisor<span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:DropDownList ID="DDL_SftySupv" runat="server" class="form-control form-control-sm rounded"></asp:DropDownList>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="CreateSOPID" ControlToValidate="DDL_SftySupv" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
									</div>
								</div>

								<div class="col-md-6 col-sm-12  form-group" id="sopinput_1" runat="server" visible="false">
									<label class="col-form-label col-md-6 col-sm-3">SOP Number <span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:TextBox ID="txt_sopno" runat="server" class="form-control form-control-sm rounded" ReadOnly="false"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="CreateSOPID" ControlToValidate="txt_sopno" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
									</div>
								</div>

								<div class="col-md-6 col-sm-12  form-group" id="sopinput_2" runat="server" visible="false">
									<label class="col-form-label col-md-6 col-sm-3">SOP Title <span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:TextBox ID="txt_sopdesc" runat="server" class="form-control form-control-sm rounded" ReadOnly="false"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="CreateSOPID" ControlToValidate="txt_sopdesc" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
									</div>
								</div>

								<div class="col-md-6 col-sm-12  form-group" id="sopinput_4" runat="server" visible="false">
									<label class="col-form-label col-md-6 col-sm-3">SOP Trainer / Faculty <span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:TextBox ID="txt_sopfaculty" runat="server" class="form-control form-control-sm rounded" ReadOnly="false"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator7" ValidationGroup="CreateSOPID" ControlToValidate="txt_sopfaculty" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
									</div>
								</div>

								<div class="col-md-6 col-sm-12  form-group" id="sopinput_3" runat="server" visible="false">
									<label class="col-form-label col-md-6 col-sm-3">SOP Training Duration <span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:TextBox ID="txt_duration" runat="server" class="form-control form-control-sm rounded" ReadOnly="false"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator8" ValidationGroup="CreateSOPID" ControlToValidate="txt_duration" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
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
							<!-- Small modal -->

							<%--ADD button-1 start--%>
							<div class="col-md-12 center-margin" id="CreateSOPIDRow" runat="server" visible="false">
								<div class="ln_solid"></div>
								<div class="row center">
									<div class="col-md-6 center">
										<asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to ADD Data!!"></asp:Label>
									</div>
									<div class="col-md-6 center">
										<asp:Button ID="btn_home" runat="server" Text="Home" CssClass="btn btn-danger btn-sm" PostBackUrl="~/bussiness/production/homepage.aspx" />
										<asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="btn btn-warning btn-sm" PostBackUrl="~/bussiness/production/csm_soptraining.aspx" />
										<asp:Button ID="btn_submit" runat="server" Text="Generate SOP ID" ValidationGroup="CreateSOPID" Enabled="true" CausesValidation="true" CssClass="btn btn-success btn-sm" OnClick="btn_submit_Click" />
									</div>
								</div>
							</div>
							<%--ADD button-1 end--%>

							<div class="row center" id="ID_CreatedMsg" runat="server" visible="false">
								<div class="col-md-12 col-sm-12" style="vertical-align: middle; text-align: center;">
									<asp:Image ID="Img_Success1" runat="server" ImageUrl="~/erp_images/success_gif.gif" Width="100px" Height="100px" />
									<asp:Label ID="lbl_idcreatedmsg" runat="server" Text="SOP ID Created..!" Font-Bold="true" Font-Size="Large"></asp:Label>
									<asp:Label ID="lbl_ID" runat="server" Text="" Visible="false"></asp:Label>
								</div>
							</div>

							<div class="col-md-12 center-margin" id="ID_CreatedMsgHR" runat="server" visible="false">
								<div class="ln_solid"></div>
							</div>

							<div class="row" id="PhotographRow" runat="server" visible="false">
								<div class="col-md-6 col-sm-6 form-group" id="uploadbuttonrow1" runat="server" visible="true">
									<label>Upload Photograph (.jpeg / .jpg) <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-6 col-sm-6 form-group" id="uploadbuttonrow2" runat="server" visible="true">
									<button type="button" class="btn btn-primary btn-sm" id="btnShowPopup" data-toggle="modal" data-target="#myModal">
										<i class="fa fa-plus-circle"></i>&nbsp;Training Photograph
									</button>
								</div>

								<div class="col-md-6 col-sm-12 form-group" id="UploadedPhotoRow1" runat="server" visible="false">
									<label>Uploaded Photograph<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-6 col-sm-12 form-group" id="UploadedPhotoRow2" runat="server" visible="false">
									<asp:Image ID="ImgDisplay" runat="server" Height="200px" Width="320px" class="img-thumbnail" />
								</div>
							</div>

							<%--- Up-loader Modal --%>
							<div class="modal fade" id="myModal">
								<div class="modal-dialog">
									<div class="modal-content">
										<div class="modal-header">
											<h4 class="modal-title">Upload Training Photograph</h4>
											<button type="button" class="close" data-dismiss="modal">&times;</button>
										</div>
										<div class="modal-body">
											<div class="row">
												<div class="col-md-12">
													<div class="form-group">
														<label>Capture / Choose Photograph</label>
														<div class="input-group">
															<div class="custom-file col-md-8">
																<asp:FileUpload ID="FileUploader" CssClass="custom-file-input" runat="server" AllowMultiple="false" />
																<label class="custom-file-label"></label>
															</div>
															<asp:Label ID="lbl_fileyesno" runat="server" Text="Label" Visible="false"></asp:Label>
															<div class="input-group-append col-md-4">
																<asp:Button ID="btnUpload" runat="server" CssClass="btn btn-primary btn-sm" CausesValidation="false" Text="Upload" OnClick="btnUpload_Click" />
															</div>
														</div>
														<asp:Label ID="lblMessage" runat="server"></asp:Label>
													</div>
												</div>
											</div>
										</div>
										<div class="modal-footer">
											<button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
										</div>
									</div>
								</div>
							</div>
							<%--- Up-loader Modal -------- END --%>


							<%--ADD button-3 start--%>
							<div class="col-md-6 center-margin" id="SavePanel3Data" runat="server" visible="false">
								<div class="ln_solid"></div>
								<div class="item form-group row">
									<div class="col-md-6 col-sm-12">
										<asp:Label ID="Label12" runat="server" Text="Click PROCEED to SAVE Data!!"></asp:Label>
									</div>
									<div class="col-md-6 col-sm-12">
										<button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
										<button type="reset" class="btn btn-warning btn-sm">Reset</button>
										<asp:Button ID="btn_saveTBTPhoto" runat="server" Text="PROCEED" Enabled="true" CssClass="btn btn-success btn-sm" OnClick="btn_saveTBTPhoto_Click" />
									</div>
								</div>
							</div>
							<%--ADD button-3 end--%>


							<div class="row" id="PhotoUploaded" runat="server" visible="false">
								<div class="col-md-12 col-sm-12" style="vertical-align: middle; text-align: center;">
									<asp:Image ID="Img_Success" runat="server" ImageUrl="~/erp_images/success_gif.gif" Width="100px" Height="100px" />
									<asp:Label ID="Label11" runat="server" Text="Photograph Uploaded Successfully...!" Font-Bold="true" Font-Size="Large"></asp:Label>
									<asp:Label ID="lbl_photoid" runat="server" Text=""></asp:Label>
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
