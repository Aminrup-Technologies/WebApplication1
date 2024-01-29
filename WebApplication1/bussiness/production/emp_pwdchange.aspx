<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="emp_pwdchange.aspx.cs" Inherits="WebApplication1.bussiness.production.emp_pwdchange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="container">
            <div class="row">
                <div class="col-md-12 col-sm-12  ">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Sub Heading</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row">
                                <div class="form-group">
                                    <div class="col-md-2 col-sm-3 col-xs-3 form-group">
                                        <label>Login ID:<span class="text text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-4 col-sm-3 col-xs-3 form-group">
                                        <asp:TextBox ID="txt_atsloginid" runat="server" class="form-control form-control-sm rounded" Text="N/A" Font-Bold="true" ForeColor="Blue" ReadOnly="true"></asp:TextBox>
                                    </div>

                                    <div class="col-md-2 form-group">
                                        <label>Workmen Sl:<span class="text text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-4 form-group">
                                        <asp:TextBox ID="txt_atsworkmenno" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" Font-Bold="true" ForeColor="Blue"></asp:TextBox>

                                    </div>

                                    <div class="col-md-2 form-group">
                                        <label>OLD Password:<span class="text text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-4 form-group">
                                        <asp:TextBox ID="txt_oldpass" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="OLDPASS" ForeColor="IndianRed" ErrorMessage="OLD Password as input in required" InitialValue="" Display="Dynamic" ControlToValidate="txt_oldpass" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    </div>

                                    <div class="col-md-2 form-group">
                                        &nbsp;
                                    </div>
                                    <div class="col-md-4 form-group">
                                        &nbsp;
                                    </div>

                                    <div class="col-md-2 form-group">
                                        <label><span class="text text-danger">*</span>Verify OLD Password</label>
                                    </div>
                                    <div class="col-md-4 form-group">
                                        <asp:Button ID="btn_validateoldpassword" runat="server" Text="Verify & Proceed" ValidationGroup="OLDPASS" CausesValidation="true" CssClass="btn btn-sm btn-success" OnClick="btn_validateoldpassword_Click" />
                                    </div>

                                    <div class="col-md-2 form-group" id="newpwd_row1" runat="server" visible="false">
                                        <label>New Password:<span class="text text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-4 form-group" id="newpwd_row2" runat="server" visible="false">
                                        <asp:TextBox ID="txt_newpass1" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_P1" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txt_newpass1" InitialValue="" ValidationGroup="ChnagePassword" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="REV_P1" runat="server" ErrorMessage="AlphaNumeric Password Policy" ForeColor="Red" ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$" ControlToValidate="txt_newpass1"></asp:RegularExpressionValidator>
                                    </div>

                                    <div class="col-md-2 form-group" id="newpwd_row3" runat="server" visible="false">
                                        <label>New Password:<span class="text text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-4 form-group" id="newpwd_row4" runat="server" visible="false">
                                        <asp:TextBox ID="txt_newpass2" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_P2" runat="server" ErrorMessage="*" ForeColor="Red" InitialValue="" ControlToValidate="txt_newpass2" ValidationGroup="ChnagePassword" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="CV_P1" runat="server" ErrorMessage="CompareValidator" ControlToCompare="txt_newpass1" ForeColor="Red" ControlToValidate="txt_newpass2"></asp:CompareValidator>
                                    </div>

                                    <div class="col-md-2 form-group" id="newpwd_row5" runat="server" visible="false">
                                        <label>Security Q1:<span class="text text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-10 form-group" id="newpwd_row6" runat="server" visible="false">
                                        <asp:DropDownList ID="DDL_SQ1" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFV_P3" runat="server" ErrorMessage="Selection Required" ValidationGroup="ChnagePassword" ControlToValidate="DDL_SQ1" ForeColor="Red" InitialValue="Please Select Option" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    </div>

                                    <div class="col-md-2 form-group" id="newpwd_row7" runat="server" visible="false">
                                        <label>Answer to Q1 :<span class="text text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-10 form-group" id="newpwd_row8" runat="server" visible="false">
                                        <asp:TextBox ID="txt_SQAns1" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_P4" runat="server" ErrorMessage="Input Required" ControlToValidate="txt_SQAns1" ValidationGroup="ChnagePassword" InitialValue="" SetFocusOnError="true" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>

                                    <div class="col-md-2 form-group" id="newpwd_row9" runat="server" visible="false">
                                        <label>Security Q2 :<span class="text text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-10 form-group" id="newpwd_row10" runat="server" visible="false">
                                        <asp:DropDownList ID="DDL_SQ2" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFV_P5" runat="server" ErrorMessage="Selection Required" ControlToValidate="DDL_SQ2" ForeColor="Red" Display="Dynamic" InitialValue="Please Select Option" SetFocusOnError="true" ValidationGroup="ChnagePassword"></asp:RequiredFieldValidator>
                                    </div>

                                    <div class="col-md-2 form-group" id="newpwd_row11" runat="server" visible="false">
                                        <label>Answer to Q2 :<span class="text text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-10 form-group" id="newpwd_row12" runat="server" visible="false">
                                        <asp:TextBox ID="txt_SQAns2" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_P6" runat="server" ErrorMessage="Input Required" ControlToValidate="txt_SQAns2" ValidationGroup="ChnagePassword" InitialValue="" SetFocusOnError="true" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>

                                    </div>

                                    <div class="col-md-8 form-group">
                                        <asp:Label ID="lbl_msgpass" runat="server" Text="Your login credentails has expired, Please change to continue visitng" Font-Bold="true" ForeColor="Red"></asp:Label>
                                    </div>

                                    <div class="col-md-4 form-group">
                                        <asp:Button ID="btn_relogin" CausesValidation="false" runat="server" Text="Re-Login" Enabled="false" CssClass="btn btn-primary btn-sm" OnClick="btn_relogin_Click" />
                                    </div>
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
                                        <asp:Button ID="btn_closecvpass" runat="server" Text="SKIP NOW" Visible="true" CausesValidation="false" class="btn btn-danger btn-sm" data-dismiss="modal" Enabled="true" />
                                        <asp:Button ID="btn_discardsvpass" runat="server" Text="Discard Changes" CausesValidation="false" CssClass="btn btn-warning btn-sm" OnClick="btn_discardsvpass_Click" />
                                        <asp:Button ID="btn_svpass" runat="server" CssClass="btn btn-success btn-sm" Text="Save Changes" Enabled="false" OnClick="btn_svpass_Click" CausesValidation="true" ValidationGroup="ChnagePassword" />
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
                            <h2>View and Manage</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row">
                                <div class="col-md-12 col-sm-12">
                                    <div class="card-box table-responsive">
                                        <span>Hello, How are you?</span>
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
</asp:Content>
