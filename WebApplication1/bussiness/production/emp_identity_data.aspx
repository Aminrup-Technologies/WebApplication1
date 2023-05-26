<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="emp_identity_data.aspx.cs" Inherits="WebApplication1.bussiness.production.emp_identity_data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>Employee Identity Data</h3>
                </div>

                <div class="title_right">
                    <div class="col-md-5 col-sm-5 form-group pull-right top_search"></div>
                </div>

            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12  ">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Add :Identity Data</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                                <%--<li><a class="close-link"><i class="fa fa-close"></i></a></li>--%>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">

                            <div class="row">
                                <hr />

                                <div class="col-md-12 center-margin">
									<div class="item form-group row">
										<div class="col-md-6 col-sm-12 text text-capitalize text-primary">Employee : Voter ID Information</div>
									</div>
								</div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Voter ID Number <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12 form-group">
                                 <asp:TextBox ID="txt_voter_id" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_voter_id" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4"></small>
                                </div>


                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Voter ID Photo <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12 form-group">
                                    <asp:FileUpload ID="FileUpload1" runat="server" />  
                                    <small class="form-text text-muted ml-4">Kindly upload a scanned cop</small>
                                </div>
                                
                                

                                <div class="col-md-12 center-margin">
									<div class="item form-group row">
										<div class="col-md-6 col-sm-12 text text-capitalize text-primary">Employee : Aadhaar Card Information</div>
									</div>
								</div>

                                <div class="col-md-3 col-sm-12  form-group">
                                 <label>Aadhaar Number <span class="text text-danger">*</span></label>     
                                 </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <<asp:TextBox ID="txt_aadhaar_no" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_aadhaar_no" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4"></small>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Aadhaar Photo<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:FileUpload ID="FileUpload2" runat="server" />
                                  <small class="form-text text-muted ml-4"></small>
                                </div>


                                <div class="col-md-12 center-margin">
									<div class="item form-group row">
										<div class="col-md-6 col-sm-12 text text-capitalize text-primary">Employee : PAN Card Information</div>
									</div>
								</div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>PAN Number <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <<asp:TextBox ID="txt_pan" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_pan" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4"></small>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>PAN Photo <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:FileUpload ID="FileUpload3" runat="server" />
                                   <small class="form-text text-muted ml-4"></small>
                                </div>


                                <div class="col-md-12 center-margin">
									<div class="item form-group row">
										<div class="col-md-6 col-sm-12 text text-capitalize text-primary">Employee : Passport Number Information</div>
									</div>
								</div>
                            
                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Passport Number <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_passport_no" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_passport_no" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4"></small>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Passport Photo <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:FileUpload ID="FileUpload4" runat="server" />
                                    <small class="form-text text-muted ml-4">Example : HS</small>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Passport Validity <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_passport_validity" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_passport_validity" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4"></small>
                                </div>

                                <div class="col-md-12 center-margin">
									<div class="item form-group row">
										<div class="col-md-6 col-sm-12 text text-capitalize text-primary">Employee : Driving Licence Card Information</div>
									</div>
								</div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Driving licence Number <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_dl_no" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_dl_no" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4"></small>
                                </div>
                                 <div class="col-md-3 col-sm-12  form-group">
                                    <label>Driving licence Photo <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:FileUpload ID="FileUpload5" runat="server" />
                                    <small class="form-text text-muted ml-4"></small>
                                </div>
                                 <div class="col-md-3 col-sm-12  form-group">
                                    <label>Driving Licence Validity <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_dl_validity" class="form-control form-control-sm rounded" placeholder="" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_dl_validity" SetFocusOnError="true"></asp:RequiredFieldValidator>
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
                                        <asp:Button ID="btn_submit" runat="server" Text="Submit" class="btn btn-success btn-sm"/>
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
