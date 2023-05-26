<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="csm_globaltraining.aspx.cs" Inherits="WebApplication1.bussiness.production.csm_globaltraining" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="container">
			<div class="page-title">
				<div class="title_left">
					<h4>CSM Training Documentation</h4>
				</div>
			</div>
			<div class="clearfix"></div>

			<div class="row">
				<div class="col-md-12 col-sm-12">
					<div class="x_panel">
						<div class="x_title">
							<h2>P1 : JOBID Attachment Panel</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">
							<div class="row" id="jobselectionpanel" runat="server" visible="true">
								<div class="col-md-6 col-sm-12  form-group" id="JOBIDYesNoRow" runat="server" visible="true">
									<label class="col-form-label col-md-6 col-sm-3">Training to JOB Executing Group :<span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:RadioButtonList ID="RBTN_JOBYesNo" CssClass="" runat="server" CellPadding="5" Height="15px" CellSpacing="5" RepeatColumns="2" AutoPostBack="true" RepeatDirection="Horizontal" Width="100%" OnSelectedIndexChanged="RBTN_JOBYesNo_SelectedIndexChanged">
											<asp:ListItem Value="0" Text="No">No</asp:ListItem>
											<asp:ListItem Value="1" Text="Yes">Yes</asp:ListItem>
										</asp:RadioButtonList>
										<asp:RequiredFieldValidator ID="RFV1" ValidationGroup="CreateID" ControlToValidate="RBTN_JOBYesNo" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
									</div>
								</div>

								<div class="col-md-6 col-sm-12  form-group" id="JOBIDInputRow" runat="server" visible="false">
									<label class="col-form-label col-md-6 col-sm-3">Input Valid JOBID :<span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:TextBox ID="txt_jobid" runat="server" class="form-control form-control-sm rounded" OnTextChanged="txt_jobid_TextChanged" AutoPostBack="true"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RFV2" ValidationGroup="CreateID" ControlToValidate="txt_jobid" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
									</div>
								</div>

								<div class="col-md-12 col-sm-12  form-group" id="JOBIDDetails_Row" runat="server" visible="false">
									<label class="col-form-label col-md-3 col-sm-3">JOBID Details</label>
									<div class="col-md-9 col-sm-6">
										JOB Region :
										<asp:Label ID="lbl_jobrgn" runat="server" Text="N/A" ForeColor="Black" Font-Bold="true" Visible="true"></asp:Label>;
										JOB ID :<asp:Label ID="lbl_jobid" runat="server" Text="N/A" ForeColor="Black" Font-Bold="true"></asp:Label>;
										JOB Title :<asp:Label ID="lbl_jobtitle" runat="server" Text="N/A" ForeColor="Black" Font-Bold="true" Visible="true"></asp:Label>;
										JOB Date :<asp:Label ID="lbl_jobiddate" runat="server" Text="N/A" ForeColor="Blue" Font-Bold="true"></asp:Label>;
										JOB Shift :<asp:Label ID="lbl_jobshift" runat="server" Text="N/A" ForeColor="Blue" Font-Bold="true"></asp:Label>;
										JOB Supervisor :<asp:Label ID="lbl_jobcreatorname" runat="server" Text="N/A" ForeColor="Black" Font-Bold="true" Visible="true"></asp:Label>-
										(<asp:Label ID="lbl_creatorwrk" runat="server" Text="N/A" ForeColor="Black" Font-Bold="true" Visible="true"></asp:Label>);
										JOB Site :<asp:Label ID="lbl_jobsite" runat="server" Text="N/A" ForeColor="Black" Font-Bold="true"></asp:Label>
										<asp:Label ID="lbl_jobsitecode" runat="server" Text="N/A" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
										<asp:Label ID="lbl_creatorregion" runat="server" Visible="false" Text="N/A" ForeColor="Black" Font-Bold="true"></asp:Label>
										<asp:Label ID="lbl_creatorcompany" runat="server" Visible="false" Text="N/A" ForeColor="Black" Font-Bold="true"></asp:Label>
										<asp:Label ID="lbl_crtrsitename" runat="server" Text="N/A" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
										<asp:Label ID="lbl_crtrsitecode" runat="server" Visible="false" Text="N/A" ForeColor="Black" Font-Bold="true"></asp:Label>;
										JOB Work Order :<asp:Label ID="lbl_wrkordr" runat="server" Text="N/A" ForeColor="Black" Font-Bold="true"></asp:Label>;
										JOB Permit No :<asp:Label ID="lbl_permitno" runat="server" Text="N/A" ForeColor="Black" Font-Bold="true"></asp:Label>;
										<asp:Label ID="lbl_jobcompay" runat="server" Text="N/A" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
										Site In-Charge :<asp:Label ID="lbl_inchargename" runat="server" Text="N/A" ForeColor="blue" Font-Bold="true" Visible="true"></asp:Label>-
										(<asp:Label ID="lbl_inchargewrk" runat="server" Text="N/A" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>);
										JOB Work Location :<asp:Label ID="lbl_jobloc" runat="server" Text="N/A" ForeColor="Black" Font-Bold="true"></asp:Label>;
										<asp:Label ID="lbl_dept" runat="server" Text="N/A" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
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

							</div>
						</div>
					</div>


					<div class="x_panel" id="traningbasicdata" runat="server" visible="false">
						<div class="x_title">
							<h2>P2 : Training Basic Details</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">
							<div class="row" id="Div1" runat="server" visible="true">

								<div class="col-md-6 col-sm-12  form-group" id="worksiterow" runat="server" visible="true">
									<label class="col-form-label col-md-6 col-sm-3">Worksite<span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:DropDownList ID="DDL_Worksite" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Worksite_SelectedIndexChanged"></asp:DropDownList>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="CreateID" ControlToValidate="DDL_Worksite" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
										<asp:Label ID="lbl_worksitecode" runat="server" Text=""></asp:Label>
									</div>
								</div>


								<div class="col-md-6 col-sm-12  form-group" id="approverrow" runat="server" visible="true">
									<label class="col-form-label col-md-6 col-sm-3">Site In-Charge<span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:DropDownList ID="DDL_Approver" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="false"></asp:DropDownList>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator9" ValidationGroup="CreateID" ControlToValidate="DDL_Approver" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
									</div>
								</div>

								<div class="col-md-6 col-sm-12  form-group" id="Locationrow" runat="server" visible="true">
									<label class="col-form-label col-md-6 col-sm-3">Training Location<span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:DropDownList ID="DDL_Location" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="false"></asp:DropDownList>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator10" ValidationGroup="CreateID" ControlToValidate="DDL_Location" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
									</div>
								</div>

								<div class="col-md-6 col-sm-12  form-group" id="sftyofcrrow" runat="server" visible="true">
									<label class="col-form-label col-md-6 col-sm-3">Safety Officer<span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:DropDownList ID="DDL_SftyOfcr" runat="server" class="form-control form-control-sm rounded"></asp:DropDownList>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="CreateID" ControlToValidate="DDL_SftyOfcr" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
									</div>
								</div>

								<div class="col-md-6 col-sm-12  form-group" id="sftysupvrow" runat="server" visible="true">
									<label class="col-form-label col-md-6 col-sm-3">Safety Supervisor<span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:DropDownList ID="DDL_SftySupv" runat="server" class="form-control form-control-sm rounded"></asp:DropDownList>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="CreateID" ControlToValidate="DDL_SftySupv" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
									</div>
								</div>

								<div class="col-md-6 col-sm-12  form-group" id="sopinput_4" runat="server" visible="true">
									<label class="col-form-label col-md-6 col-sm-3">Trainer / Faculty <span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:TextBox ID="txt_faculty" runat="server" class="form-control form-control-sm rounded" ReadOnly="false"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator7" ValidationGroup="CreateID" ControlToValidate="txt_faculty" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
									</div>
								</div>

								<div class="col-md-6 col-sm-12  form-group" id="sopinput_1" runat="server" visible="true">
									<label class="col-form-label col-md-6 col-sm-3">Trainng Source<span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:TextBox ID="txt_sopno" runat="server" class="form-control form-control-sm rounded" ReadOnly="false"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="CreateID" ControlToValidate="txt_sopno" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
									</div>
								</div>

								<div class="col-md-6 col-sm-12  form-group" id="sopinput_2" runat="server" visible="true">
									<label class="col-form-label col-md-6 col-sm-3">Training Title <span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:TextBox ID="txt_sopdesc" runat="server" class="form-control form-control-sm rounded" ReadOnly="false"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="CreateID" ControlToValidate="txt_sopdesc" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
									</div>
								</div>

								<div class="col-md-6 col-sm-12  form-group" id="ContractEmployeesrow" runat="server" visible="true">
									<label class="col-form-label col-md-6 col-sm-3">Contract Employees (Head Count) :<span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:TextBox ID="txt_cntrctemp" runat="server" class="form-control form-control-sm rounded" TextMode="Number" MaxLength="2"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="CreateID" ControlToValidate="txt_cntrctemp" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
									</div>
								</div>

								<div class="col-md-6 col-sm-12  form-group" id="sopinput_3" runat="server" visible="true">
									<label class="col-form-label col-md-6 col-sm-3">Training Duration (In Mins) <span class="text text-danger">*</span></label>
									<div class="col-md-6 col-sm-6">
										<asp:TextBox ID="txt_duration" runat="server" class="form-control form-control-sm rounded" ReadOnly="false" TextMode="Number" MaxLength="2"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator8" ValidationGroup="CreateID" ControlToValidate="txt_duration" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
									</div>
								</div>

								<%--ADD button-1 start--%>
								<div class="col-md-12 center-margin" id="CreateSOPIDRow" runat="server" visible="true">
									<div class="ln_solid"></div>
									<div class="col-md-12 center">
										<div class="col-md-6 center">
											<asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to ADD Data!!"></asp:Label>
										</div>
										<div class="col-md-6 center">
											<asp:Button ID="btn_home" runat="server" Text="Home" CssClass="btn btn-danger btn-sm" PostBackUrl="~/bussiness/production/homepage.aspx" />
											<asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="btn btn-warning btn-sm" PostBackUrl="~/bussiness/production/csm_soptraining.aspx" />
											<asp:Button ID="btn_submit" runat="server" Text="Generate TRN ID" ValidationGroup="CreateID" Enabled="true" CausesValidation="true" CssClass="btn btn-success btn-sm" OnClick="btn_submit_Click" />
										</div>
									</div>
								</div>
								<%--ADD button-1 end--%>

								<div class="row center" id="ID_CreatedMsg" runat="server" visible="false">
									<div class="col-md-12 col-sm-12" style="vertical-align: middle; text-align: center;">
										<asp:Image ID="Img_Success1" runat="server" ImageUrl="~/erp_images/success_gif.gif" Width="100px" Height="100px" />
										<asp:Label ID="lbl_idcreatedmsg" runat="server" Text="Training ID Created..!" Font-Bold="true" Font-Size="Large"></asp:Label>
										:
									<asp:Label ID="lbl_ID" runat="server" Text="" Visible="true"></asp:Label>
									</div>
								</div>
							</div>
						</div>
					</div>

					<div>
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
					</div>

					<br />

					<div class="x_panel" id="traningagenda" runat="server" visible="false">
						<div class="x_title">
							<h2>P3 : Training Agenda / Points Discussed by Trainer</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">

							<div class="row" id="Div3" runat="server" visible="true">
								<div class="col-md-12 col-sm-12  form-group" id="Div4" runat="server" visible="true">
									<label class="col-md-3 col-sm-6">Training Agenda Point <span class="text text-danger">*</span></label>
									<div class="col-md-9 col-sm-6">
										<asp:TextBox ID="txt_trnagenda" runat="server" class="form-control form-control-sm rounded" ReadOnly="false"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator11" ValidationGroup="ADDAGENDA" ControlToValidate="txt_trnagenda" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
									</div>
								</div>

								<%--ADD button-1 start--%>
								<div class="col-md-12 center-margin" id="Div5" runat="server" visible="true">
									<div class="ln_solid"></div>
									<div class="col-md-12 center">
										<div class="col-md-6 center">
											<asp:Label ID="Label1" runat="server" Text="Click SUBMIT to ADD Data!!"></asp:Label>
										</div>
										<div class="col-md-6 center">
											<asp:Button ID="Button1" runat="server" Text="Home" CssClass="btn btn-danger btn-sm" PostBackUrl="~/bussiness/production/homepage.aspx" />
											<asp:Button ID="Button2" runat="server" Text="Reset" CssClass="btn btn-warning btn-sm" PostBackUrl="~/bussiness/production/csm_globaltraining.aspx"/>
											<asp:Button ID="btn_AddAgenda" runat="server" Text="ADD TO LIST" ValidationGroup="ADDAGENDA" Enabled="true" CausesValidation="true" OnClick="btn_AddAgenda_Click" CssClass="btn btn-success btn-sm"/>
										</div>
									</div>
								</div>
								<%--ADD button-1 end--%>
							</div>

							<br />

							<div class="col-md-12 col-sm-12  form-group" id="Div2" runat="server" visible="true">
								<div class="card-box table-responsive small center col-md-12 col-sm-12">
									<asp:GridView ID="AgendaGrid" AutoGenerateColumns="false" Visible="true" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" ShowFooter="true" OnRowEditing="AgendaGrid_RowEditing" OnRowCancelingEdit="AgendaGrid_RowCancelingEdit" OnRowUpdating="AgendaGrid_RowUpdating" OnRowDeleting="AgendaGrid_RowDeleting">
										<Columns>
											<asp:TemplateField HeaderText="Sl. NO" Visible="True" HeaderStyle-Width="10%">
												<ItemTemplate>
													<asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
												</ItemTemplate>
												<ItemStyle CssClass="text text-center" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="Agenda Detailed Description" HeaderStyle-Width="70%">
												<EditItemTemplate>
													<asp:TextBox ID="txt_trnagenda" class="form-control form-control-sm rounded" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"agendadescp") %> ' Width="100%"></asp:TextBox>
													<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_trnagenda"
														ErrorMessage="Enter Details" InitialValue="" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
												</EditItemTemplate>
												<ItemTemplate>
													<asp:Label ID="lbl_agendadescp" runat="server" Text='<%# Bind("agendadescp") %>'></asp:Label>
												</ItemTemplate>
												<ItemStyle CssClass="text text-left" />
											</asp:TemplateField>

											<asp:TemplateField HeaderText="Action" HeaderStyle-Width="20%">
												<EditItemTemplate>
													<asp:ImageButton ID="btnupdate" runat="server" CommandName="Update" Height="15px" ImageUrl="~/erp_images/fi-sr-disk.svg" Width="15px" ToolTip="Save" ImageAlign="Middle" />&nbsp;
														<asp:ImageButton ID="Btncancale" runat="server" CommandName="Cancel" Height="15px" ImageUrl="~/erp_images/fi-sr-cross-circle.svg" Width="15px" ToolTip="Cancel Update" ImageAlign="Middle" />
												</EditItemTemplate>
												<ItemTemplate>
													<asp:ImageButton ID="btnedit" runat="server" CommandName="Edit" Height="15px" ImageUrl="~/erp_images/fi-sr-pencil.svg" Width="15px" ToolTip="Update" ImageAlign="Middle" />&nbsp;
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
										<FooterStyle CssClass="text text-center font-weight-bold" />
									</asp:GridView>
								</div>
							</div>

						</div>
					</div>

					<div class="x_panel" id="traningactionable" runat="server" visible="false">
						<div class="x_title">
							<h2>P4 : Training Actionables</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">
						</div>
					</div>

					<div class="x_panel" id="traningphotograph" runat="server" visible="false">
						<div class="x_title">
							<h2>P5 : Training Photograph</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">
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
