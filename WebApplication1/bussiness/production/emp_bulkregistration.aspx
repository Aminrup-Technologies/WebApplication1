<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="emp_bulkregistration.aspx.cs" Inherits="WebApplication1.bussiness.production.emp_bulkregistration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="">
			<div class="page-title">
				<div class="title_left">
					<h3>New Employee Registration</h3>
				</div>

				<div class="title_right">
					<div class="col-md-5 col-sm-5 form-group pull-right top_search"></div>
				</div>
			</div>

			<div class="clearfix"></div>

			<div class="row">
				<div class="col-md-12 col-sm-12">
					<div class="x_panel">
						<div class="x_title">
							<h2>Bulk Employee Data Upload</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>

						<div class="x_content">
							<div class="row">

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select Work Country <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_WorkCountry" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_WorkCountry_SelectedIndexChanged"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Selection is required" Display="Dynamic" InitialValue="Please Select Option" ForeColor="Red" ControlToValidate="DDL_WorkCountry"></asp:RequiredFieldValidator>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select Work State <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_WorkStates" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_WorkStates_SelectedIndexChanged"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Selection is required" Display="Dynamic" InitialValue="Please Select Option" ForeColor="Red" ControlToValidate="DDL_WorkStates"></asp:RequiredFieldValidator>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select Work Region <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_WorkRegion" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Work_Region_SelectedIndexChanged"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Selection is required" Display="Dynamic" InitialValue="Please Select Option" ForeColor="Red" ControlToValidate="DDL_WorkRegion"></asp:RequiredFieldValidator>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Select Company Name <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_Company" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" InitialValue="Please Select Option" ControlToValidate="DDL_Company"></asp:RequiredFieldValidator>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>Upload Excel Sheet<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group">
									<button type="button" class="btn btn-primary btn-sm" id="btnShowPopup" data-toggle="modal" data-target="#myModal">
										<i class="fa fa-plus-circle"></i> Import Excel
									</button>
								</div>
                                <div class="col-md-3 col-sm-12 form-group">
									<asp:Button ID="btn_dwn_dedexcel" runat="server" Text="Template Downlaod" CssClass="btn btn-sm btn-warning" OnClick="btn_dwn_dedexcel_Click" />
                                </div>

								<div class="modal fade" id="myModal">
									<div class="modal-dialog">
										<div class="modal-content">
											<div class="modal-header">
												<h4 class="modal-title">Import Excel File</h4>
												<button type="button" class="close" data-dismiss="modal">&times;</button>
											</div>
											<div class="modal-body">
												<div class="row">
													<div class="col-md-12">
														<div class="form-group">
															<label>Choose excel file</label>
															<div class="input-group">
																<div class="custom-file col-md-8">
																	<asp:FileUpload ID="FileUpload1" CssClass="custom-file-input" runat="server" />
																	<label class="custom-file-label"></label>
																</div>
																<label id="filename"></label>
																<div class="input-group-append col-md-4">
																	<asp:Button ID="btnUpload" runat="server" CssClass="btn btn-primary btn-sm" Text="Upload" OnClick="ImportExcel" />
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
										<asp:Button ID="btn_submit" runat="server" Text="Submit" class="btn btn-success btn-sm" OnClick="btn_submit_Click"/>
									</div>
								</div>
							</div>
							<%--button   end--%>
						</div>
					</div>
				</div>
			</div>
		</div>

		<div class="col-md-12 col-sm-12">
			<div class="x_panel">
				<div class="x_title">
					<h2>View Upload Excel Sheet Data</h2>
					<ul class="nav navbar-right panel_toolbox">
						<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
					</ul>
					<div class="clearfix"></div>
				</div>
				<div class="x_content">
					<div class="row">

						<div class="col-md-12 col-sm-12">
							<div class="card-box table-responsive">
								<p class="text-muted font-13 m-b-30">Click on the above SUBMIT button to save the uploaded data</p>
								<asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap">
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

	<script type="text/javascript">
		function ShowPopup() {
			$("#btnShowPopup").click();
		}
	</script>
</asp:Content>
