<%@ Page Title="ATS : Create JOBID" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="create_jobid.aspx.cs" Inherits="WebApplication1.bussiness.production.create_jobid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="container">
			<div class="page-title">
				<div class="title_left">
					<h5>JOB ID Creation Page : **Permit Number</h5>
				</div>
			</div>

			<div class="clearfix"></div>

			<div class="row">
				<div class="col-md-12 col-sm-12">
					<div class="x_panel">
						<div class="x_title">
							<h2>Create : JOB ID against Work Permit & Upload Permit File</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>

						<div class="x_content">
							<div class="row" id="Div1" runat="server" visible="true">
								<div class="col-md-6 col-sm-12" style="vertical-align: middle; text-align: center;">
									<asp:Button ID="btn_dateswap" runat="server" Text="Today" class="btn btn-success btn-sm" OnClick="btn_dateswap_Click"/>
								</div>
								<div class="col-md-6 col-sm-12" style="vertical-align: middle; text-align: center;">
									<asp:Image ID="Image1" runat="server" ImageUrl="~/erp_images/rightarrow.gif" Width="80px" Height="70px" />
									<span style="font-weight:bold; color:darkblue;">JOB Date : </span><asp:Label ID="lbl_jobdate" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
									<span style="font-weight:bold; color:darkblue;">JOB Day : </span><asp:Label ID="lbl_jobday" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
								</div>

								<div class="col-md-12 col-sm-12 center-margin">
									<div class="ln_solid"></div>
								</div>
							</div>
						</div>

						<div class="x_content">
							<%--Form Row start--%>
							<div class="row form-horizontal form-label-left" id="jobid_creation" runat="server" visible="false">

								<div class="col-md-3 col-sm-6 col-xs-6 form-group">
									<label>Select Work Region<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-6 col-xs-6 form-group">
									<asp:DropDownList ID="DDL_Region" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Region_SelectedIndexChanged"></asp:DropDownList>
									<asp:Label ID="Label2" runat="server" Text=""></asp:Label>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator7" ValidationGroup="Submit" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_Region" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
								</div>

								<div class="col-md-3 col-sm-6 col-xs-6 form-group">
									<label>Select Work Order<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-6 col-xs-6 form-group">
									<asp:DropDownList ID="DDL_Workorder" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Workorder_SelectedIndexChanged"></asp:DropDownList>
									<asp:Label ID="lbl_wotype" runat="server" Text=""></asp:Label>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="Submit" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_Workorder" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
								</div>

								<div class="col-md-3 col-sm-12 form-group" runat="server" visible="false">
									<label>Work Region<span class="text text-danger"></span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group" runat="server" visible="false">
									<asp:TextBox ID="txt_workregion" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
								</div>

								<div class="col-md-3 col-sm-12 form-group" runat="server" visible="false">
									<label>Company<span class="text text-danger"></span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group" runat="server" visible="false">
									<asp:TextBox ID="txt_company" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>JOB Type</label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_BillingType" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RFV1" ValidationGroup="Submit" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_BillingType" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Attendance Type</label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_AttenCode" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RFV3" ValidationGroup="Submit" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_AttenCode" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>Select Work-Site<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group">
									<asp:DropDownList ID="DDL_Worksite" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Worksite_SelectedIndexChanged"></asp:DropDownList>
									<asp:Label ID="lbl_worksitecode" runat="server" Text=""></asp:Label>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Submit" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_Worksite" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>Select Approver<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group">
									<asp:DropDownList ID="DDL_Approver" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator8" ValidationGroup="Submit" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_Approver" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>Department<span class="text text-danger"></span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group">
									<asp:TextBox ID="txt_dept" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>Location<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group">
									<asp:DropDownList ID="DDL_Location" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="Submit" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_Location" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>Work Permit No<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group">
									<asp:TextBox ID="txt_permitno" runat="server" class="form-control form-control-sm rounded"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="Submit" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_permitno" SetFocusOnError="true" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator runat="server" ControlToValidate="txt_permitno" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Only Numeric" ID="RegularExpressionValidator1" ValidationExpression="^[0-9 /,]+$"></asp:RegularExpressionValidator>
									<small class="form-text text-muted ml-4">Example : 5896523, If more than one then separate with comma(,)</small>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>JOB Shift<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group">
									<asp:TextBox ID="txt_jobshift" runat="server" class="form-control form-control-sm rounded" MaxLength="1"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="Submit" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_jobshift" SetFocusOnError="true" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator runat="server" ControlToValidate="txt_jobshift" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Only Characters" ID="RegularExpressionValidator2" ValidationExpression="^[a-zA-Z]+$"></asp:RegularExpressionValidator>
									<small class="form-text text-muted ml-4">Example : G / A / B / C etc..</small>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>JOB Tile<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group">
									<asp:TextBox ID="txt_jobtitle" runat="server" class="form-control form-control-sm rounded" TextMode="MultiLine" Rows="3"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="Submit" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_jobtitle" SetFocusOnError="true" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator runat="server" ControlToValidate="txt_jobtitle" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ErrorMessage="NO special characters" ID="rfvname" ValidationExpression="^[a-zA-Z0-9 ,]+$"></asp:RegularExpressionValidator>
								</div>
							</div>
							<%--Form Row END--%>

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


							<%--button start--%>
							<div class="col-md-6 center-margin" id="jobid_creation_buttons" runat="server" visible="false">
								<div class="ln_solid"></div>
								<div class="item form-group row">
									<div class="col-md-6 col-sm-12 pb-2" style="text-align: center;">
										<asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to Save Data!!"></asp:Label>
									</div>
									<div class="col-md-6 col-sm-12" style="text-align: center;">
										<button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
										<button type="reset" class="btn btn-warning btn-sm">Reset</button>
										<asp:Button ID="btn_submit" runat="server" Text="Submit" ValidationGroup="Submit" CssClass="btn btn-success btn-sm" OnClick="btn_submit_Click" />
									</div>
								</div>
							</div>
							<%--button end--%>


							<div class="row" id="jobid_created" runat="server" visible="false">
								<div class="col-md-12 col-sm-12" style="vertical-align: middle; text-align: center;">
									<asp:Image ID="Img_Success" runat="server" ImageUrl="~/erp_images/success_gif.gif" Width="100px" Height="100px" />
									<asp:Label ID="Label1" runat="server" Text="JOB ID Created Successfully...!" Font-Bold="true" Font-Size="Large"></asp:Label>
								</div>
							</div>

							<%--button start--%>
							<div class="col-md-6 center-margin" id="jobid_created_buttons" runat="server" visible="false">
								<div class="ln_solid"></div>
								<div class="col-md-12 col-sm-12 center" style="text-align: center;">
									<asp:Button ID="btn_upload" runat="server" Text="Permit Upload" CssClass="btn btn-primary btn-sm" Visible="false" OnClick="btn_upload_Click" />
									<asp:Button ID="btn_inpunch" runat="server" Text="In-Punch Page" CssClass="btn btn-info btn-sm" Visible="true" OnClick="btn_inpunch_Click" />
								</div>
							</div>
							<%--button end--%>
						</div>
					</div>
				</div>
			</div>
			<%--Body Row END--%>
		</div>
	</div>
	<script type="text/javascript">
		function ShowPopup(title, body) {
			$("#MyPopup .modal-title").html(title);
			$("#MyPopup .modal-body").html(body);
			$("#MyPopup").modal("show");
		}

		function ShowPopup1() {
			$("#myModal").modal("show");
		}
	</script>
</asp:Content>
