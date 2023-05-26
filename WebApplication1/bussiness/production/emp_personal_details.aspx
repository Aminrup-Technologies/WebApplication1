<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="emp_personal_details.aspx.cs" Inherits="WebApplication1.bussiness.production.personal_details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="">
			<div class="page-title">
				<div class="title_left">
					<h3>Employee Personal Details</h3>
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
							<h2>Add : Employee Personal Details</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
								<%--<li><a class="close-link"><i class="fa fa-close"></i></a></li>--%>
							</ul>
							<div class="clearfix"></div>
						</div>

						<div class="x_content">
							<div class="row">
								<div class="col-md-12 center-margin">
									<div class="item form-group row">
										<div class="col-md-6 col-sm-12 text text-capitalize text-primary">Employee : Personal Data</div>
									</div>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Employee First Name <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_fname" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_fname" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4"></small>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Employee Middle Name <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_Mname" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_Mname" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4"></small>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Employee Last Name <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_lastname" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_lastname" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4"></small>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Employee Father Name <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_fthrname" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_fthrname" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4"></small>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Employee Mother Name <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_mthrname" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_mthrname" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4"></small>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Date of Birth <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_dob" runat="server" class="date-picker form-control" placeholder="" required="required" onfocus="this.type='date'" onmouseover="this.type='date'" onclick="this.type = 'date'" onblur="this.type='text'" onmouseout="timeFunctionLong(this)"></asp:TextBox>
									<script>
										function timeFunctionLong(txt_dob) {
											setTimeout(function () {
												txt_dob.type = 'text';
											}, 60000);
										}
									</script>
									<small class="form-text text-muted ml-4"></small>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Gender <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_gender" runat="server" CssClass="form-control form-control-sm rounded">
										<asp:ListItem Selected="True" Value="0">--Select--</asp:ListItem>
										<asp:ListItem Value="1">Male</asp:ListItem>
										<asp:ListItem Value="2">Female</asp:ListItem>
									</asp:DropDownList>
									<small class="form-text text-muted ml-4"></small>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Religion <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_religion" runat="server" CssClass="form-control form-control-sm rounded">
										<asp:ListItem Selected="True" Value="0">--select--</asp:ListItem>
										<asp:ListItem Value="1">Hindu</asp:ListItem>
										<asp:ListItem Value="2">Muslim</asp:ListItem>
										<asp:ListItem Value="3">Shikh</asp:ListItem>
										<asp:ListItem Value="4">Christian</asp:ListItem>
									</asp:DropDownList>
									<small class="form-text text-muted ml-4"></small>
								</div>

								<div class="col-md-3 col-sm-12  form-group">
									<label>Martial status<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_martial_status" runat="server" CssClass="form-control form-control-sm rounded">
										<asp:ListItem Selected="True" Value="0">--select--</asp:ListItem>
										<asp:ListItem Value="1">Single</asp:ListItem>
										<asp:ListItem Value="2">Married</asp:ListItem>
									</asp:DropDownList>
									<small class="form-text text-muted ml-4"></small>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<label>Blood Group<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control form-control-sm rounded">
										<asp:ListItem Selected="True" Value="0">--select--</asp:ListItem>
										<asp:ListItem Value="1">A+</asp:ListItem>
										<asp:ListItem Value="2">B+</asp:ListItem>
										<asp:ListItem Value="3">AB+</asp:ListItem>
										<asp:ListItem Value="4">AB-</asp:ListItem>
										<asp:ListItem Value="5">O+</asp:ListItem>
										<asp:ListItem Value="6">O-</asp:ListItem>
									</asp:DropDownList>
									<small class="form-text text-muted ml-4"></small>
								</div>

								<div class="col-md-12 center-margin">
									<div class="ln_solid"></div>
									<div class="item form-group row">
										<div class="col-md-6 col-sm-12 text text-capitalize text-primary">Employee : Emergency Contact Information</div>
									</div>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>Contact Person Name<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_name" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_name" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4"></small>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>Relationship with Employee <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_rrlsip_name" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_rrlsip_name" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4"></small>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>Contact Number<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_contact_number" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_contact_number" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4"></small>
								</div>

								<div class="col-md-12 center-margin">
									<div class="ln_solid"></div>
									<div class="item form-group row">
										<div class="col-md-6 col-sm-12 text text-capitalize text-primary">Employee : Contact Information</div>
									</div>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>Email ID<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_email_id" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_email_id" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4"></small>
								</div>
								<div class="col-md-3 col-sm-12 form-group">
									<label>Mobile Number<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_mobile_no" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_mobile_no" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4"></small>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>Employee Photo Graph<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_ph_graph" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_ph_graph" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4"></small>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>Employee Signature<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_sign" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_sign" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4"></small>
								</div>

								<div class="col-md-12 center-margin">
									<div class="ln_solid"></div>
									<div class="item form-group row">
										<div class="col-md-6 col-sm-12 text text-capitalize text-primary">Employee : Present Address Section</div>
									</div>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>Present Street Address <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group">
									<asp:TextBox ID="txt_present_addr" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_present_addr" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4"></small>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>Present City <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group">
									<asp:TextBox ID="TextBox1" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_city" SetFocusOnError="true"></asp:RequiredFieldValidator>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>Present District <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="TextBox2" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_distrct" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4"></small>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>Present State <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="TextBox3" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_stat" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4"></small>
								</div>


								<div class="col-md-3 col-sm-12 form-group">
									<label>Present Pincode<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="TextBox4" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_pincode" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4"></small>
								</div>

								<div class="col-md-12 center-margin">
									<div class="ln_solid"></div>
									<div class="item form-group row">
										<div class="col-md-6 col-sm-12 text text-capitalize text-primary">Employee : Permanent Address Section</div>
									</div>
								</div>


								<div class="col-md-3 col-sm-12 form-group">
									<label>Permanent Street Address <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group">
									<asp:TextBox ID="txt_str_adr" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_str_adr" SetFocusOnError="true"></asp:RequiredFieldValidator>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>Permanent City <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group">
									<asp:TextBox ID="txt_city" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_city" SetFocusOnError="true"></asp:RequiredFieldValidator>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>Permanent District <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_distrct" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_distrct" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4"></small>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>Permanent State <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_stat" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_stat" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4"></small>
								</div>


								<div class="col-md-3 col-sm-12 form-group">
									<label>Permanent Pincode<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_pincode" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_pincode" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4"></small>
								</div>

								<div class="col-md-3 col-sm-12 form-group">
									<label>Address Proof <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12 form-group">
									<asp:TextBox ID="txt_pr" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_pr" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4"></small>
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
										<button type="submit" class="btn btn-success btn-sm">Submit</button>
									</div>
								</div>
							</div>
							<%--button   end--%>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

