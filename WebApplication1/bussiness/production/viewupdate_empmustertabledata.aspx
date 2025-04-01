<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="viewupdate_empmustertabledata.aspx.cs" Inherits="WebApplication1.bussiness.production.viewupdate_empmustertabledata" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>Employee Muster Data Update</h3>
                </div>
            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <%--<div class="x_title">
							<h2>View & Update Employee Muster Data</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>--%>

                        <div class="x_content">
                            <div class="row">

                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <label>Work Country</label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:DropDownList ID="DDL_WorkCountry" runat="server" CssClass="form-control form-control-sm rounded" Enabled="false"></asp:DropDownList>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <label>Work State</label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:DropDownList ID="DDL_WorkStates" runat="server" CssClass="form-control form-control-sm rounded" Enabled="false"></asp:DropDownList>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <label>Work Region</label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:DropDownList ID="DDL_WorkRegion" runat="server" CssClass="form-control form-control-sm rounded" Enabled="false"></asp:DropDownList>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <label>Company Name</label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:DropDownList ID="DDL_Company" runat="server" CssClass="form-control form-control-sm rounded" Enabled="false"></asp:DropDownList>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <label>Employee Workman SL</label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:TextBox ID="txt_workman" class="form-control form-control-sm rounded" runat="server" MaxLength="5" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="col-md-12 center-margin">
                                    <div class="ln_solid"></div>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <label>First Name <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:TextBox ID="txt_empfname" class="form-control form-control-sm rounded" placeholder="First Name" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_empfname" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <label>Middle Name</label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:TextBox ID="txt_empmdname" class="form-control form-control-sm rounded" placeholder="Middle Name" runat="server"></asp:TextBox>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <label>Last Name <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:TextBox ID="txt_emplstname" class="form-control form-control-sm rounded" placeholder="Last Name" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_emplstname" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <label>Full Name <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:TextBox ID="txt_fullanme" class="form-control form-control-sm rounded" placeholder="Full Name" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_fullanme" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>


                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label style="font-weight: bold; color: blue;">Father Name <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:TextBox ID="txt_empfathername" class="form-control form-control-sm rounded" placeholder="Enter Employee Father's Name" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_empfathername" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label style="font-weight: bold; color: blue;">Mother Name <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:TextBox ID="txt_empmothername" class="form-control form-control-sm rounded" placeholder="txt_Mothername..." runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5a" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_empmothername" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>Employee Date Of Birth <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:TextBox ID="txt_DOB" runat="server" class="date-picker form-control rounded" placeholder="yyyy-mm-dd" required="required" onfocus="this.type='date'" onmouseover="this.type='date'" onclick="this.type = 'date'" onblur="this.type='text'" onmouseout="timeFunctionLong(this)"></asp:TextBox>
                                    <script>
                                        function timeFunctionLong(txt_DOB) {
                                            setTimeout(function () {
                                                txt_DOB.type = 'text';
                                            }, 60000);
                                        }
                                    </script>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>Employee Blood Group<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:TextBox ID="txt_bloodgroup" class="form-control form-control-sm rounded" placeholder="Enter Blood Group" runat="server" MaxLength="4"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_bloodgroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>


                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>Mobile Number<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:TextBox ID="txt_Mobile_Number" class="form-control form-control-sm rounded" placeholder="Enter Employee Contact" runat="server" MaxLength="10"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_Mobile_Number" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>Highest Qualification<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:DropDownList ID="DDL_HighestEdu" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" Display="Dynamic" InitialValue="--Select--" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_HighestEdu" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-12 center-margin">
                                    <div class="ln_solid"></div>
                                </div>


                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>Employee Date Of Joining <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:TextBox ID="txt_DOJ" runat="server" class="date-picker form-control rounded" placeholder="yyyy-mm-dd" required="required" onfocus="this.type='date'" onmouseover="this.type='date'" onclick="this.type = 'date'" onblur="this.type='text'" onmouseout="timeFunctionLong(this)"></asp:TextBox>
                                    <script>
                                        function timeFunctionLong(txt_DOJ) {
                                            setTimeout(function () {
                                                txt_DOJ.type = 'text';
                                            }, 60000);
                                        }
                                    </script>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>Employee Work-site <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:DropDownList ID="DDL_Worksites" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" InitialValue="Please Select Option" ControlToValidate="DDL_Worksites"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>Employee Skill Category <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:DropDownList ID="DDL_SkillCategory" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_SkillCategory_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" InitialValue="Please Select Option" ControlToValidate="DDL_SkillCategory"></asp:RequiredFieldValidator>
                                </div>


                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>Employee Skill Designation<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:DropDownList ID="DDL_SkillDesignation" runat="server" Enabled="false" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" InitialValue="Please Select Option" ControlToValidate="DDL_SkillDesignation"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>Select Employee Role<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:DropDownList ID="DDL_EmployeeType" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_EmployeeType_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" InitialValue="Please Select Option" ControlToValidate="DDL_EmployeeType"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>Select Role - Permissions<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:DropDownList ID="DDL_RolePermissions" runat="server" CssClass="form-control form-control-sm rounded" Enabled="false"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" InitialValue="Please Select Option" ControlToValidate="DDL_RolePermissions"></asp:RequiredFieldValidator>
                                </div>


                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>Employee Work Hours<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:DropDownList ID="DDL_WorkHours" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" InitialValue="Please Select Option" ControlToValidate="DDL_WorkHours"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>Employee OT Factor<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:DropDownList ID="DDL_OTFactor" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" InitialValue="Please Select Option" ControlToValidate="DDL_OTFactor"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-12 center-margin">
                                    <div class="ln_solid"></div>
                                </div>




                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>Safety Pass No<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:TextBox ID="txt_rfidno" class="form-control form-control-sm rounded" placeholder="Enter Saftey Pass No" runat="server" ReadOnly="true" BackColor="#efefef"></asp:TextBox>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>Safety Pass Validity<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:TextBox ID="txt_rfidvalidity" runat="server" CssClass="form-control form-control-sm rounded" ReadOnly="true" BackColor="#efefef"></asp:TextBox>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>Gate pass No<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:TextBox ID="txt_gpno" class="form-control form-control-sm rounded" placeholder="Enter Gate Pass No" runat="server" ReadOnly="true" BackColor="#efefef"></asp:TextBox>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>Gate pass Validity<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:TextBox ID="txt_gpvalidity" runat="server" CssClass="form-control form-control-sm rounded" ReadOnly="true" BackColor="#efefef"></asp:TextBox>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>Police Verification Validity<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:TextBox ID="txt_pvvalidity" runat="server" CssClass="form-control form-control-sm rounded" ReadOnly="true" BackColor="#efefef"></asp:TextBox>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>UPDATE<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <button type="button" class="btn btn-primary btn-sm" id="btnShowPopup2" data-toggle="modal" data-target="#myModal2">
                                        Update GP
                                    </button>
                                </div>




                                <div class="col-md-12 center-margin">
                                    <div class="ln_solid"></div>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>UAN No<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:TextBox ID="txt_uanno" class="form-control form-control-sm rounded" placeholder="Enter Employee UAN No" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_uanno" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>ESIC No<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:TextBox ID="txt_esicno" class="form-control form-control-sm rounded" placeholder="Enter Employee ESIC No" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_esicno" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>



                                <div class="col-md-12 center-margin">
                                    <div class="ln_solid"></div>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>Bank Name<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
									<asp:TextBox ID="txt_banknanme" class="form-control form-control-sm rounded" placeholder="Enter Bank Name" runat="server" ReadOnly="true"></asp:TextBox>
								</div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>Bank Account No<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:TextBox ID="txt_accountno" class="form-control form-control-sm rounded" placeholder="Enter bank account number" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>Bank IFSC Code<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:TextBox ID="txt_ifsccode" class="form-control form-control-sm rounded" placeholder="Enter bank IFSC Code" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>Bank Branch Name<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <asp:TextBox ID="txt_branchnm" class="form-control form-control-sm rounded" placeholder="Enter bank branch name" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>UPDATE</label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6  form-group">
                                    <button type="button" class="btn btn-primary btn-sm" id="btnShowPopup" data-toggle="modal" data-target="#myModal">Update Bank</button>
                                </div>
                            </div>


                            <%--button   start--%>
                            <div class="col-md-6 center-margin">
                                <div class="ln_solid"></div>
                                <div class="item form-group row">
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Label ID="lbl_msg" runat="server" Text="Click UPDATE to Save Data!!"></asp:Label>
                                    </div>
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Button ID="btn_cancel" runat="server" Text="BACK" class="btn btn-danger btn-sm" OnClick="btn_cancel_Click" CausesValidation="false" />
                                        <asp:Button ID="btn_save" runat="server" Text="UPDATE ALL" Enabled="true" class="btn btn-success btn-sm" OnClick="btn_save_Click" />
                                    </div>
                                </div>
                            </div>
                            <%--button   end--%>

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
            </div>

            <%--- Up-loader Modal --%>
            <div class="modal fade" id="myModal">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">Update Salary Account Details</h4>
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-6 col-sm-12 form-group">
                                            <label>Enter Bank Name :<span class="text text-danger"></span></label>
                                        </div>
                                        <div class="col-md-6 col-sm-12 form-group">
                                            <%--<asp:TextBox ID="txt_nwbankname" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
											<asp:RequiredFieldValidator ID="RFV1" runat="server" ValidationGroup="BANK" Display="Dynamic" ForeColor="Red" ControlToValidate="txt_nwbankname" ErrorMessage="**"></asp:RequiredFieldValidator>--%>
                                        </div>

                                        <div class="col-md-6 col-sm-12 form-group">
                                            <asp:DropDownList ID="DDL_BankName" runat="server" CssClass="form-control form-control-sm rounded" Enabled="false"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RFV_BankName" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" InitialValue="Please Select Option" ControlToValidate="DDL_BankName"></asp:RequiredFieldValidator>
                                        </div>

                                        <div class="col-md-6 col-sm-12 form-group">
                                            <label>Enter Account Number :<span class="text text-danger"></span></label>
                                        </div>
                                        <div class="col-md-6 col-sm-12 form-group">
                                            <asp:TextBox ID="txt_nwaccno" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RFV2" runat="server" ValidationGroup="BANK" Display="Dynamic" ForeColor="Red" ErrorMessage="**" ControlToValidate="txt_nwaccno"></asp:RequiredFieldValidator>
                                        </div>

                                        <div class="col-md-6 col-sm-12 form-group">
                                            <label>Re-Enter Account Number :<span class="text text-danger"></span></label>
                                        </div>
                                        <div class="col-md-6 col-sm-12 form-group">
                                            <asp:TextBox ID="txt_nwcnfaccno" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RFV3" runat="server" ValidationGroup="BANK" ErrorMessage="**" ControlToValidate="txt_nwcnfaccno"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="CV1" runat="server" ValidationGroup="BANK" Display="Dynamic" ForeColor="Red" ErrorMessage="Enter same Account Number" ControlToCompare="txt_nwaccno" ControlToValidate="txt_nwcnfaccno"></asp:CompareValidator>
                                        </div>

                                        <div class="col-md-6 col-sm-12 form-group">
                                            <label>Enter IFSC Code :<span class="text text-danger"></span></label>
                                        </div>
                                        <div class="col-md-6 col-sm-12 form-group">
                                            <asp:TextBox ID="txt_nwifsc" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RFV4" runat="server" ValidationGroup="BANK" Display="Dynamic" ForeColor="Red" ErrorMessage="**" ControlToValidate="txt_nwifsc"></asp:RequiredFieldValidator>
                                        </div>

                                        <div class="col-md-6 col-sm-12 form-group">
                                            <label>Enter Branch Name :<span class="text text-danger"></span></label>
                                        </div>
                                        <div class="col-md-6 col-sm-12 form-group">
                                            <asp:TextBox ID="txt_nwbranchname" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btn_bankedit" runat="server" ValidationGroup="BANK" Enabled="true" CausesValidation="true" Text="Make Changes" CssClass="btn btn-info btn-sm" OnClick="btn_bankedit_Click" />
                            <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
            <%--- Up-loader Modal -------- END --%>


            <%--- Up-loader Modal --%>
            <div class="modal fade" id="myModal2">
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
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="GPDATA" ControlToValidate="txt_nwgpno" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
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
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="GPDATA" ControlToValidate="txt_nwgpvalidity" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
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
                                    <asp:Label ID="Label1" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btn_gtpsedit" runat="server" CausesValidation="true" ValidationGroup="GPDATA" Text="Make Changes" CssClass="btn btn-info btn-sm" OnClick="btn_gtpsedit_Click" />
                            <asp:Button ID="btn_cancelgpedit" runat="server" CausesValidation="false" Text="Cancel" CssClass="btn btn-warning btn-sm" OnClick="btn_cancelgpedit_Click" />
                            <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
            <%--- Up-loader Modal -------- END --%>
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

        function ShowPopup2() {
            $("#myModal2").modal("show");
        }
    </script>
</asp:Content>
