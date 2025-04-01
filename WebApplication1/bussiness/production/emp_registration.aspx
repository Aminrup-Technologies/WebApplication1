<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="emp_registration.aspx.cs" Inherits="WebApplication1.bussiness.production.emp_registration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>New Employee Registration</h3>
                </div>
            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Add : New Employee Registration</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>

                        <div class="x_content">
                            <div class="row">

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
                                    <asp:DropDownList ID="DDL_Company" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_company_name_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" InitialValue="Please Select Option" ControlToValidate="DDL_Company"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Employee Workman SL<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_workman" class="form-control form-control-sm rounded" placeholder="Enter Employee First Name" runat="server" MaxLength="5" OnTextChanged="txt_workman_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_workman" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4">Example : For KPO : K201 / For Angul : A201</small>
                                </div>

                                <div class="col-md-12 center-margin">
                                    <div class="ln_solid"></div>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Employee First Name <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_empfname" class="form-control form-control-sm rounded" placeholder="Enter Employee First Name" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_empfname" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4">Example : Aman</small>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Employee Middle Name</label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_empmdname" class="form-control form-control-sm rounded" placeholder="Enter Employee Middle Name" runat="server"></asp:TextBox>
                                    <small class="form-text text-muted ml-4">Example : Kumar</small>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Employee Last Name <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_emplstname" class="form-control form-control-sm rounded" placeholder="Enter Employee Last Name" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_emplstname" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4">Example : Singh</small>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Employee Full Name <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_fullanme" class="form-control form-control-sm rounded" placeholder="Enter Employee Last Name" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_fullanme" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4">Example : Singh</small>
                                </div>


                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Employee Father Name <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_empfathername" class="form-control form-control-sm rounded" placeholder="Enter Employee Father's Name" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_empfathername" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4">Example : Raghav Kumar Singh</small>
                                </div>

                                <%--<div class="col-md-3 col-sm-12 form-group">
									<label>Employee Mother Name <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:TextBox ID="txt_empmothername" class="form-control form-control-sm rounded" placeholder="txt_Mothername..." runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_empmothername" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4">Example : Rima Singh</small>
								</div>--%>

                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Employee Date Of Birth <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_DOB" runat="server" class="date-picker form-control rounded" placeholder="yyyy-mm-dd" required="required" onfocus="this.type='date'" onmouseover="this.type='date'" onclick="this.type = 'date'" onblur="this.type='text'" onmouseout="timeFunctionLong(this)"></asp:TextBox>
                                    <script>
                                        function timeFunctionLong(txt_DOB) {
                                            setTimeout(function () {
                                                txt_DOB.type = 'text';
                                            }, 60000);
                                        }
                                    </script>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Employee Blood Group<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_bloodgroup" class="form-control form-control-sm rounded" placeholder="Enter Blood Group" runat="server" MaxLength="4"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_bloodgroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4">Example : B+</small>
                                </div>


                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Mobile Number<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_Mobile_Number" class="form-control form-control-sm rounded" placeholder="Enter Employee Contact" runat="server" MaxLength="10"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_Mobile_Number" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4">Example : </small>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Highest Qualification<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_HighestEdu" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" Display="Dynamic" InitialValue="--Select--" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_HighestEdu" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4">Example : </small>
                                </div>

                                <div class="col-md-12 center-margin">
                                    <div class="ln_solid"></div>
                                </div>


                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Employee Date Of Joining <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_DOJ" runat="server" class="date-picker form-control rounded" placeholder="yyyy-mm-dd" required="required" onfocus="this.type='date'" onmouseover="this.type='date'" onclick="this.type = 'date'" onblur="this.type='text'" onmouseout="timeFunctionLong(this)"></asp:TextBox>
                                    <script>
                                        function timeFunctionLong(txt_DOJ) {
                                            setTimeout(function () {
                                                txt_DOJ.type = 'text';
                                            }, 60000);
                                        }
                                    </script>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Employee Work-site <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_Worksites" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" InitialValue="Please Select Option" ControlToValidate="DDL_Worksites"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Employee Skill Category <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_SkillCategory" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_SkillCategory_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" InitialValue="Please Select Option" ControlToValidate="DDL_SkillCategory"></asp:RequiredFieldValidator>
                                </div>


                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Employee Skill Designation<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_SkillDesignation" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" InitialValue="Please Select Option" ControlToValidate="DDL_SkillDesignation"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Select Employee Role<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_EmployeeType" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_EmployeeType_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" InitialValue="Please Select Option" ControlToValidate="DDL_EmployeeType"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Select Role - Permissions<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_RolePermissions" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" InitialValue="Please Select Option" ControlToValidate="DDL_RolePermissions"></asp:RequiredFieldValidator>
                                </div>


                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Employee Work Hours<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_WorkHours" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" InitialValue="Please Select Option" ControlToValidate="DDL_WorkHours"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Employee OT Factor<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_OTFactor" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" InitialValue="Please Select Option" ControlToValidate="DDL_OTFactor"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-12 center-margin">
                                    <div class="ln_solid"></div>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Safety Pass No<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_rfidno" class="form-control form-control-sm rounded" placeholder="Enter Saftey Pass No" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_rfidno" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4">Example : </small>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Safety Pass Validity<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_rfidvalidity" runat="server" class="date-picker form-control rounded" placeholder="yyyy-mm-dd" required="required" onfocus="this.type='date'" onmouseover="this.type='date'" onclick="this.type = 'date'" onblur="this.type='text'" onmouseout="timeFunctionLong(this)"></asp:TextBox>
                                    <script>
                                        function timeFunctionLong(txt_rfidvalidity) {
                                            setTimeout(function () {
                                                txt_rfidvalidity.type = 'text';
                                            }, 60000);
                                        }
                                    </script>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Gate pass No<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_gpno" class="form-control form-control-sm rounded" placeholder="Enter Gate Pass No" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_gpno" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4">Example : </small>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Gate pass Validity<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_gpvalidity" runat="server" class="date-picker form-control rounded" placeholder="yyyy-mm-dd" required="required" onfocus="this.type='date'" onmouseover="this.type='date'" onclick="this.type = 'date'" onblur="this.type='text'" onmouseout="timeFunctionLong(this)"></asp:TextBox>
                                    <script>
                                        function timeFunctionLong(txt_gpvalidity) {
                                            setTimeout(function () {
                                                txt_gpvalidity.type = 'text';
                                            }, 60000);
                                        }
                                    </script>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Police Verification Validity<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_pvvalidity" runat="server" class="date-picker form-control rounded" placeholder="yyyy-mm-dd" required="required" onfocus="this.type='date'" onmouseover="this.type='date'" onclick="this.type = 'date'" onblur="this.type='text'" onmouseout="timeFunctionLong(this)"></asp:TextBox>
                                    <script>
                                        function timeFunctionLong(txt_pvvalidity) {
                                            setTimeout(function () {
                                                txt_pvvalidity.type = 'text';
                                            }, 60000);
                                        }
                                    </script>
                                </div>

                                <div class="col-md-12 center-margin">
                                    <div class="ln_solid"></div>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>UAN No<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_uanno" class="form-control form-control-sm rounded" placeholder="Enter Employee UAN No" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_uanno" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4">Example : </small>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>ESIC No<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_esicno" class="form-control form-control-sm rounded" placeholder="Enter Employee ESIC No" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_esicno" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4">Example : </small>
                                </div>

                                <div class="col-md-12 center-margin">
                                    <div class="ln_solid"></div>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Bank Name<span class="text text-danger">*</span></label>
                                </div>
                                <%--<div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_banknanme" class="form-control form-control-sm rounded" placeholder="Enter Bank Name" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_banknanme" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4">Example : State bank of India</small>
                                </div>--%>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_BankName" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RFV_BankName" runat="server" ErrorMessage="Selection is required" Display="Dynamic" ForeColor="Red" InitialValue="Please Select Option" ControlToValidate="DDL_BankName"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Bank Account No<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_accountno" class="form-control form-control-sm rounded" placeholder="Enter bank account number" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_accountno" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4">Example : 003256478952</small>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Bank IFSC Code<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_ifsccode" class="form-control form-control-sm rounded" placeholder="Enter bank IFSC Code" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_ifsccode" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4">Example : SBIN0001765</small>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Bank Branch<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_bankbranch" class="form-control form-control-sm rounded" placeholder="Enter bank branch name" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_bankbranch" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4">Example : Bhagalpur</small>
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
                                        <asp:Button ID="btn_submit" runat="server" Text="Submit" class="btn btn-success btn-sm" OnClick="btn_submit_Click" />
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

    <script type="text/javascript">
        function ShowPopup(title, body) {
            $("#MyPopup .modal-title").html(title);
            $("#MyPopup .modal-body").html(body);
            $("#MyPopup").modal("show");
        }
    </script>
</asp:Content>
