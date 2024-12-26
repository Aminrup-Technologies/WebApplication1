<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="csm_tbtform.aspx.cs" Inherits="WebApplication1.bussiness.production.csm_tbtform" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function ShowPopup(title, body) {
            $("#MyPopup .modal-title").html(title);
            $("#MyPopup .modal-body").html(body);
            $("#MyPopup").modal("show");
        }

        function ShowCheckboxDiv(IdBaseName, NumberOfBoxes) {
            for (let x = 1; x <= NumberOfBoxes; x++) {
                const CheckThisBox = IdBaseName + x;
                const BoxDiv = IdBaseName + x + 'Div';

                const checkbox = document.getElementById(CheckThisBox);

                // Check if the checkbox element is found
                if (checkbox !== null && checkbox !== undefined) {
                    if (checkbox.checked) {
                        document.getElementById(BoxDiv).style.display = "block";
                    } else {
                        document.getElementById(BoxDiv).style.display = "none";
                    }
                } else {
                    console.error(`Checkbox element with ID ${CheckThisBox} not found.`);
                }
            }
            return false;
        }


        function ValidateModuleList(source, args) {
            var chkListModules = document.getElementById('<%= chkbxrspons.ClientID %>');
            var chkListinputs = chkListModules.getElementsByTagName("input");
            for (var i = 0; i < chkListinputs.length; i++) {
                if (chkListinputs[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
        }

        function ToggleValidatorSOP(chkSOP) {
            var valName = document.getElementById("<%=valSOP.ClientID%>");
            ValidatorEnable(valName, chkSOP.checked);
        }
        function ToggleValidatorPRSNL(chkPRSNL) {
            var valName5 = document.getElementById("<%=cvmodulelist.ClientID%>");
            ValidatorEnable(valName5, chkPRSNL.checked);
        }
        function ToggleValidatorSI(chkSI) {
            var valName1 = document.getElementById("<%=valSI.ClientID%>");
            ValidatorEnable(valName1, chkSI.checked);
        }

        function ToggleValidatorHazard(chkHZD) {
            var valName2 = document.getElementById("<%=valHazard.ClientID%>");
            ValidatorEnable(valName2, chkHZD.checked);
        }

        function ToggleValidatorSftMsg(chkSftyMsg) {
            var valName3 = document.getElementById("<%=valSftMsg.ClientID%>");
            ValidatorEnable(valName3, chkSftyMsg.checked);
        }

        function OpenJOBIDTab() {
            $('#jobid-tab').tab('show');
        }

        function OpenItemsTab() {
            $('#annexure-tab').tab('show');
        }

        function OpenActionableTab() {
            $('#action-tab').tab('show');
        }
        function OpenPhotoTab() {
            $('#photo-tab').tab('show');
        }
        function OpenFinalTab() {
            $('#final-tab').tab('show');
        }
    </script>


    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>Tool Box Talk</h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 col-sm-12 ">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>(DOC#ERP/CSM/TBT-01)</h2>
                            <div class="clearfix"></div>
                        </div>

                        <div class="x_content">

                            <!-- Small modal -->
                            <asp:Button ID="ShowPopup" runat="server" Text="Button" CssClass="btn btn-primary" Visible="false" data-toggle="modal" data-target=".bs-example-modal-sm" />
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


                            <ul class="nav nav-tabs bar_tabs" id="myTab" role="tablist">
                                <li class="nav-item">
                                    <a class="nav-link active" id="jobid-tab" data-toggle="tab" href="#jobid" role="tab" aria-controls="jobid" aria-selected="true">JOBID</a>
                                </li>

                                <li class="nav-item">
                                    <a class="nav-link" id="annexure-tab" data-toggle="tab" href="#annexure" role="tab" aria-controls="annexure" aria-selected="false">ITEMS DISCUSSED</a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" id="photo-tab" data-toggle="tab" href="#photo" role="tab" aria-controls="photo" aria-selected="false">Photo</a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" id="action-tab" data-toggle="tab" href="#action" role="tab" aria-controls="action" aria-selected="false">Actionable</a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" id="final-tab" data-toggle="tab" href="#final" role="tab" aria-controls="final" aria-selected="false">Final</a>
                                </li>
                            </ul>

                            <div class="clearfix">&nbsp;</div>

                            <div class="tab-content" id="myTabContent">
                                <div class="tab-pane fade show active" id="jobid" role="tabpanel" aria-labelledby="jobid-tab">

                                    <div class="form-group row">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align">Select JOBID <span class="required">*</span></label>
                                        <div class="col-md-6 col-sm-6">
                                            <asp:DropDownList ID="DDL_JOBID" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_JOBID_SelectedIndexChanged"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="CreateTBTID" ControlToValidate="DDL_JOBID" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="--Select--"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="form-group row" id="JOBIDDetails_Row" runat="server" visible="false">
                                        <asp:Label CssClass="col-form-label col-md-3 col-sm-3 label-align" AssociatedControlID="lbl_jobid" runat="server" ForeColor="IndianRed" Font-Bold="true">
                                            JOBID Details <span class="required">*</span>
                                        </asp:Label>
                                        <div class="col-md-6 col-sm-6 ">
                                            JOB_ID :<asp:Label ID="lbl_jobid" runat="server" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>;
										JOB Date :<asp:Label ID="lbl_jobiddate" runat="server" Text="Label" ForeColor="Blue" Font-Bold="true"></asp:Label>;
											<asp:Label ID="lbl_jobcreatorname" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
                                            <asp:Label ID="lbl_creatorwrk" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>;
										JOB Site :<asp:Label ID="lbl_jobsite" runat="server" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lbl_jobsitecode" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
                                            <asp:Label ID="lbl_creatorregion" runat="server" Visible="false" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lbl_creatorcompany" runat="server" Visible="false" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lbl_crtrsitename" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
                                            <asp:Label ID="lbl_crtrsitecode" runat="server" Visible="false" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>;
										JOB Work Order :<asp:Label ID="lbl_wrkordr" runat="server" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>;
										JOB Permit No :<asp:Label ID="lbl_permitno" runat="server" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>;
											<asp:Label ID="lbl_jobrgn" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
                                            <asp:Label ID="lbl_jobcompay" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
                                            <asp:Label ID="lbl_inchargename" runat="server" Text="Label" ForeColor="blue" Font-Bold="true" Visible="false"></asp:Label>
                                            <asp:Label ID="lbl_inchargewrk" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
                                            <asp:Label ID="lbl_jobloc" runat="server" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>;
										JOB Shift :<asp:Label ID="lbl_jobshift" runat="server" Text="Label" ForeColor="Blue" Font-Bold="true"></asp:Label>;
											<asp:Label ID="lbl_dept" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="form-group row" id="AttachedAttendanceRow" runat="server" visible="false">
                                        <asp:Label for="middle-name" CssClass="col-form-label col-md-3 col-sm-3 label-align" runat="server" ForeColor="IndianRed" Font-Bold="true">Engaged Manpower Details</asp:Label>
                                        <div class="col-md-6 col-sm-6 ">
                                            <asp:GridView ID="EmpGrid" runat="server" Width="100%" CssClass="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" ShowFooter="true" OnRowDataBound="EmpGrid_RowDataBound" OnRowCreated="EmpGrid_RowCreated">
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

                                    <div class="form-group row" id="locationrow" runat="server" visible="false">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align">JOB Location</label>
                                        <div class="col-md-6 col-sm-6 ">
                                            <asp:TextBox ID="txt_location" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row" id="deptrow" runat="server" visible="false">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align">
                                            JOB Department <span class="required">*</span>
                                        </label>
                                        <div class="col-md-6 col-sm-6 ">
                                            <asp:TextBox ID="txt_dept" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row" id="jobsupv" runat="server" visible="false">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align">
                                            JOB Supervisor <span class="required">*</span>
                                        </label>
                                        <div class="col-md-6 col-sm-6 ">
                                            <asp:TextBox ID="txt_supv" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row" id="LineManagerRow" runat="server" visible="false">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align">
                                            Line Manager <span class="required">*</span>
                                        </label>
                                        <div class="col-md-6 col-sm-6 ">
                                            <asp:TextBox ID="txt_linemanager" runat="server" Style="text-transform: uppercase" class="form-control form-control-sm rounded"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="CreateTBTID" ControlToValidate="txt_linemanager" runat="server" ErrorMessage="Line Manager name is Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="form-group row" id="sftyofcrrow" runat="server" visible="false">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align">
                                            Site Safety Officer <span class="required">*</span>
                                        </label>
                                        <div class="col-md-6 col-sm-6 ">
                                            <asp:DropDownList ID="DDL_SftyOfcr" runat="server" class="form-control form-control-sm rounded"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="CreateTBTID" ControlToValidate="DDL_SftyOfcr" runat="server" ErrorMessage="Safety Officwer Selection Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="form-group row" id="sftysupvrow" runat="server" visible="false">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align">
                                            Site Safety Supervisor <span class="required">*</span>
                                        </label>
                                        <div class="col-md-6 col-sm-6 ">
                                            <asp:DropDownList ID="DDL_SftySupv" runat="server" class="form-control form-control-sm rounded"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="CreateTBTID" ControlToValidate="DDL_SftySupv" runat="server" ErrorMessage="Safety Supervisor Seelction Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="form-group row" id="supvrow" runat="server" visible="false">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align">
                                            Contractor Representative <span class="required">*</span>
                                        </label>
                                        <div class="col-md-6 col-sm-6 ">
                                            <asp:TextBox ID="txt_incharge" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row" id="ContractEmployeesrow" runat="server" visible="false">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align">
                                            Contract Employees <span class="required">*</span>
                                        </label>
                                        <div class="col-md-6 col-sm-6 ">
                                            <asp:TextBox ID="txt_cntrctemp" runat="server" class="form-control form-control-sm rounded"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="CreateTBTID" ControlToValidate="txt_cntrctemp" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="col-md-6 center-margin" id="Panel1_Buttons" runat="server" visible="false">
                                        <div class="ln_solid"></div>
                                        <div class="item form-group row">
                                            <div class="col-md-6 col-sm-12">
                                                <asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to ADD Data!!"></asp:Label>
                                            </div>
                                            <div class="col-md-6 col-sm-12">
                                                <asp:Button ID="btn_submit" runat="server" Text="Generate TBT ID" ValidationGroup="CreateTBTID" Enabled="true" CausesValidation="true" CssClass="btn btn-success btn-sm" OnClick="btn_submit_Click" />
                                                <asp:Button ID="btn_back" runat="server" Text="Cancel" CssClass="btn btn-danger btn-sm" CausesValidation="false" PostBackUrl="~/bussiness/production/csms_mainview.aspx" />
                                                <button type="reset" class="btn btn-warning btn-sm">Reset</button>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" id="Panel1_Success" runat="server" visible="false">
                                        <div class="col-md-12 col-sm-12" style="vertical-align: middle; text-align: center;">
                                            <asp:Image ID="Img_Success1" runat="server" ImageUrl="~/erp_images/success_gif.gif" Width="100px" Height="100px" Visible="false" />
                                            <asp:Image ID="Img_Cross1" runat="server" ImageUrl="~/erp_images/crossgif.gif" Width="100px" Height="100px" Visible="false" />
                                            <asp:Label ID="lbl_panel1_msg" runat="server" Text="TBT ID Created..!" Font-Bold="true" Font-Size="Large"></asp:Label>
                                            : [<asp:Label ID="lbl_TBTID" runat="server" Text="" Visible="false" Font-Bold="true"></asp:Label>]
                                        </div>
                                    </div>

                                </div>

                                <div class="tab-pane fade" id="annexure" role="tabpanel" aria-labelledby="annexure-tab">

                                    <div class="form-group row" id="Annexure0" runat="server">
                                        <asp:Label class="col-form-label col-md-12 col-sm-12" for="BoxName1" runat="server" ForeColor="IndianRed" Font-Bold="true">ITEMS DISCUSSED : <small>(Indicate if not discussed)</small><span class="required">*</span></asp:Label>
                                    </div>

                                    <div class="form-group row" id="Annexure1" runat="server">
                                        <label class="col-form-label col-md-10 col-sm-12" for="BoxName1">1. Safety contact and review of action items from last meeting :</label>
                                        <div class="col-md-2 col-sm-12">
                                            <div class="col-md-2 col-sm-12">
                                                <input type="checkbox" id="BoxName1" name="BoxName1" onclick="ShowCheckboxDiv('BoxName', 8)" checked="checked" />Yes
                                            </div>
                                        </div>
                                        <div class="clearfix">&nbsp;</div>
                                        <div class="col-md-12 col-sm-12 form-group" id="BoxName1Div" style="display: block;">
                                            <asp:GridView ID="GridView2" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowDeleting="GridView1_RowDeleting">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sl" Visible="True" HeaderStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid" />
                                                        <ItemStyle CssClass="grid" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="ID" HeaderStyle-Width="20%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_AcnID" runat="server" Text='<%# Bind("AcnID") %>'></asp:Label><br />
                                                            <asp:Label ID="lbl_RefCSMFormID" runat="server" Text='<%# Bind("RefCSMFormID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid" />
                                                        <ItemStyle CssClass="grid" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Type" HeaderStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_AcnType" runat="server" Text='<%# Bind("AcnType") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid" />
                                                        <ItemStyle CssClass="grid" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Description" HeaderStyle-Width="40%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_AcnDescription" runat="server" Text='<%# Bind("AcnDescription") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Assigned To" HeaderStyle-Width="20%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_AssignedToName" runat="server" Text='<%# Bind("AssignedToName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Status" Visible="true" HeaderStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_CurrentStatus" runat="server" Text='<%# Bind("CurrentStatus") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid" />
                                                    </asp:TemplateField>

                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <div class="grid">No Data Found</div>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </div>
                                    </div>

                                    <div class="form-group row" id="Annexure2" runat="server">
                                        <label class="col-form-label col-md-10 col-sm-12" for="BoxName2">2. Items of General Safety Importance to the Total Work Site : (Ask employees to mention any incident / Near Miss during the past day which may have or have resuted into damage to property or injury to Company or Contractor Personnel)</label>
                                        <div class="col-md-2 col-sm-12">
                                            <div class="col-md-2 col-sm-12">
                                                <input type="checkbox" id="BoxName2" name="BoxName2" onclick="ShowCheckboxDiv('BoxName', 8)" />
                                                Yes
                                            </div>
                                        </div>
                                        <div class="clearfix">&nbsp;</div>
                                        <div class="col-md-12 col-sm-12 form-group" id="BoxName2Div" style="display: none;">
                                            <div>
                                                <asp:Button ID="btn_newincidententry" runat="server" Text="New Incident" Enabled="false" CssClass="btn btn-primary btn-sm" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group row" id="Annexure3" runat="server">
                                        <label class="col-form-label col-md-10 col-sm-12" for="BoxName3">3. Items of safety interest to this Group:(Eg. Red Stripes, Orange Stripes, Green Stripe, Safety alert tips for safety communication, hazards or safety conditions applicable to this group’s work area)</label>
                                        <div class="col-md-2 col-sm-12">
                                            <div class="col-md-3 col-sm-3">
                                                <input type="checkbox" id="BoxName3" name="BoxName3" onclick="ShowCheckboxDiv('BoxName', 8); ToggleValidatorSI(this);" checked="checked" />Yes
                                            </div>
                                        </div>
                                        <div class="clearfix">&nbsp;</div>
                                        <div class="col-md-10 col-sm-12  form-group" id="BoxName3Div" style="display: block;">
                                            <asp:TextBox ID="txt_sftyintrst" runat="server" class="form-control form-control-sm rounded" ReadOnly="false" TextMode="MultiLine" Rows="3"></asp:TextBox><br />
                                            <asp:RequiredFieldValidator ID="valSI" ControlToValidate="txt_sftyintrst" runat="server" ErrorMessage="*Required" ForeColor="Red" ValidationGroup="Group1" />
                                        </div>
                                    </div>

                                    <div class="form-group row" id="Annexure4" runat="server">
                                        <label class="col-form-label col-md-10 col-sm-12" for="BoxName4">
                                            4. Standard Operation Procedures (SOP) relevant to this Group :
                                        </label>
                                        <div class="col-md-2 col-sm-12">
                                            <div class="col-md-3 col-sm-3">
                                                <input type="checkbox" id="BoxName4" name="BoxName4" onclick="ShowCheckboxDiv('BoxName', 8); ToggleValidatorSOP(this);" checked="checked" />
                                                Yes
                                            </div>
                                        </div>

                                        <div class="clearfix">&nbsp;</div>

                                        <div class="col-md-10 col-sm-12" id="BoxName4Div" style="display: block;">
                                            <asp:TextBox ID="txt_sopno" runat="server" class="form-control form-control-sm rounded" ReadOnly="false"></asp:TextBox><br />
                                            <asp:RequiredFieldValidator ID="valSOP" ControlToValidate="txt_sopno" runat="server" ErrorMessage="*Required" ForeColor="Red" ValidationGroup="Group1" />
                                        </div>
                                    </div>

                                    <div class="form-group row" id="Annexure5" runat="server">
                                        <label class="col-form-label col-md-10 col-sm-12" for="BoxName5">
                                            5. Reminders to Employees of their Personal Responsibilities to ensure and Maintain :
                                        </label>
                                        <div class="col-md-2 col-sm-12">
                                            <div class="col-md-3 col-sm-3">
                                                <input type="checkbox" id="BoxName5" name="BoxName5" onclick="ShowCheckboxDiv('BoxName', 8); ToggleValidatorPRSNL(this);" checked="checked" />
                                                Yes
                                            </div>
                                        </div>

                                        <div class="clearfix">&nbsp;</div>

                                        <div class="col-md-12 col-sm-12" id="BoxName5Div" style="display: block;">
                                            <asp:CheckBoxList ID="chkbxrspons" runat="server" Font-Size="Small" CssClass="checkbox rounded" RepeatColumns="2" Width="100%"></asp:CheckBoxList><br />
                                            <asp:CustomValidator runat="server" ID="cvmodulelist" ClientValidationFunction="ValidateModuleList" Display="Dynamic" ErrorMessage="Please select state"></asp:CustomValidator>
                                            <br />
                                        </div>
                                    </div>

                                    <div class="form-group row" id="Annexure6" runat="server">
                                        <label class="col-form-label col-md-10 col-sm-12" for="BoxName6">
                                            6. Hazardous materials relevant to this Group Work's Area :
                                        </label>
                                        <div class="col-md-2 col-sm-12">
                                            <div class="col-md-3 col-sm-3">
                                                <input type="checkbox" id="BoxName6" name="BoxName6" onclick="ShowCheckboxDiv('BoxName', 8); ToggleValidatorHazard(this);" checked="checked" />
                                                Yes
                                            </div>
                                        </div>

                                        <div class="clearfix">&nbsp;</div>

                                        <div class="col-md-10 col-sm-12" id="BoxName6Div" style="display: block;">
                                            <asp:TextBox ID="txt_hazards" runat="server" class="form-control form-control-sm rounded" ReadOnly="false" TextMode="MultiLine" Rows="3"></asp:TextBox><br />
                                            <asp:RequiredFieldValidator ID="valHazard" ControlToValidate="txt_hazards" runat="server" ErrorMessage="*Required" ForeColor="Red" ValidationGroup="Group1" />
                                        </div>
                                    </div>

                                    <div class="form-group row" id="Annexure7" runat="server">
                                        <label class="col-form-label col-md-10 col-sm-12" for="BoxName7">
                                            7. Safety Message Handouts / Circulars to be shared with Contract Employees :
                                        </label>
                                        <div class="col-md-2 col-sm-12">
                                            <div class="col-md-3 col-sm-3">
                                                <input type="checkbox" id="BoxName7" name="BoxName7" onclick="ShowCheckboxDiv('BoxName', 8); ToggleValidatorSftMsg(this);" checked="checked" />
                                                Yes
                                            </div>
                                        </div>
                                        <div class="clearfix">&nbsp;</div>
                                        <div class="col-md-10 col-sm-12" id="BoxName7Div" style="display: block;">
                                            <asp:TextBox ID="txt_sftmsg" runat="server" CssClass="form-control form-control-sm rounded" ReadOnly="false" TextMode="MultiLine" Rows="3"></asp:TextBox><br />
                                            <asp:RequiredFieldValidator ID="valSftMsg" ControlToValidate="txt_sftmsg" runat="server" ErrorMessage="*Required" ForeColor="Red" ValidationGroup="Group1" />
                                        </div>
                                    </div>

                                    <%--<div class="form-group row" id="Annexure9" runat="server">
                                        <label class="col-form-label col-md-10 col-sm-12" for="BoxName8">
                                            8. Actions resulting from this meeting and points raised by Contract Employees and Supervisor :
                                        </label>
                                        <div class="col-md-2 col-sm-12">
                                            <div class="col-md-3 col-sm-3">
                                                <input type="checkbox" id="BoxName8" name="BoxName8" onclick="ShowCheckboxDiv('BoxName', 8)" checked="checked" />
                                                Yes
                                            </div>
                                        </div>
                                        <div class="clearfix">&nbsp;</div>
                                        <div class="col-md-10 col-sm-12" id="BoxName8Div" style="display: block;">
                                            <asp:Button ID="btn_addfeedback" runat="server" Text="Add Actionable" Enabled="true" CssClass="btn btn-primary btn-sm" />
                                        </div>
                                    </div>--%>

                                    <div class="col-md-6 center-margin" id="Panel2_Buttons" runat="server" visible="false">
                                        <div class="ln_solid"></div>
                                        <div class="item form-group row">
                                            <div class="col-md-6 col-sm-12">
                                                <asp:Label ID="Label10" runat="server" Text="Click PROCEED to ADD Data!!"></asp:Label>
                                            </div>
                                            <div class="col-md-6 col-sm-12">
                                                <button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
                                                <button type="reset" class="btn btn-warning btn-sm">Reset</button>
                                                <asp:Button ID="btn_savetbtdata" runat="server" Text="PROCEED" Enabled="true" CausesValidation="true" CssClass="btn btn-success btn-sm" ValidationGroup="Group1" OnClick="btn_savetbtdata_Click" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" id="Panel2_Success" runat="server" visible="false">
                                        <div class="col-md-12 col-sm-12" style="vertical-align: middle; text-align: center;">
                                            <asp:Image ID="Img_Success2" runat="server" ImageUrl="~/erp_images/success_gif.gif" Width="100px" Height="100px" />
                                            <asp:Image ID="Img_Cross2" runat="server" ImageUrl="~/erp_images/crossgif.gif" Width="100px" Height="100px" Visible="false" />
                                            <asp:Label ID="lbl_panel2_msg" runat="server" Text="TBT Items Data Saved..!" Font-Bold="true" Font-Size="Large"></asp:Label>
                                        </div>
                                    </div>

                                </div>

                                <div class="tab-pane fade" id="photo" role="tabpanel" aria-labelledby="photo-tab">
                                    <div class="row" id="TBTPhotographRow" runat="server" visible="true">
                                        <div class="col-md-6 col-sm-6 form-group" id="uploadbuttonrow1" runat="server" visible="false">
                                            <label>Upload Photograph (.jpeg / .jpg) <span class="text text-danger">*</span></label>
                                        </div>
                                        <div class="col-md-6 col-sm-6 form-group" id="uploadbuttonrow2" runat="server" visible="false">
                                            <button type="button" class="btn btn-primary btn-sm" id="btnShowPopup" data-toggle="modal" data-target="#myModal">
                                                <i class="fa fa-plus-circle"></i>&nbsp;Meeting Photograph
                                            </button>
                                        </div>

                                        <div class="col-md-6 col-sm-12 form-group" id="UploadedPhotoRow1" runat="server" visible="true">
                                            <label>Uploaded Photograph<span class="text text-danger">*</span></label>
                                        </div>
                                        <div class="col-md-6 col-sm-12 form-group" id="UploadedPhotoRow2" runat="server" visible="true">
                                            <asp:Image ID="ImgDisplay" runat="server" Height="200px" Width="320px" class="img-thumbnail" />
                                        </div>
                                    </div>

                                    <%--- Up-loader Modal --%>
                                    <div class="modal fade" id="myModal">
                                        <div class="modal-dialog">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h4 class="modal-title">Upload TBT Meeting Photograph</h4>
                                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="form-group">
                                                                <label>Capture / Choose TBT Meeting Photograph</label>
                                                                <div class="input-group">
                                                                    <div class="custom-file col-md-8">
                                                                        <asp:FileUpload ID="TBM_FileUploader" CssClass="custom-file-input" runat="server" AllowMultiple="false" />
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
                                    <div class="col-md-6 center-margin" id="Panel3_Buttons" runat="server" visible="false">
                                        <div class="ln_solid"></div>
                                        <div class="item form-group row">
                                            <div class="col-md-6 col-sm-12">
                                                <asp:Label ID="Label12" runat="server" Text="Click PROCEED to SAVE Data!!"></asp:Label>
                                            </div>
                                            <div class="col-md-6 col-sm-12">
                                                <button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
                                                <button type="reset" class="btn btn-warning btn-sm">Reset</button>
                                                <asp:Button ID="btn_saveTBTPhoto" runat="server" Text="NEXT" Enabled="true" CssClass="btn btn-success btn-sm" OnClick="btn_saveTBTPhoto_Click" />
                                            </div>
                                        </div>
                                    </div>
                                    <%--ADD button-3 end--%>

                                    <div class="row" id="Panel3_Success" runat="server" visible="false">
                                        <div class="col-md-12 col-sm-12" style="vertical-align: middle; text-align: center;">
                                            <asp:Image ID="Img_Success3" runat="server" ImageUrl="~/erp_images/success_gif.gif" Width="100px" Height="100px" />
                                            <asp:Image ID="Img_Cross3" runat="server" ImageUrl="~/erp_images/crossgif.gif" Width="100px" Height="100px" Visible="false" />
                                            <asp:Label ID="lbl_panel3_msg" runat="server" Text="Photograph Uploaded Successfully...!" Font-Bold="true" Font-Size="Large"></asp:Label>
                                            <asp:Label ID="lbl_tbtphotoid" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="tab-pane fade" id="action" role="tabpanel" aria-labelledby="action-tab">
                                    <div class="form-group row" id="Acn_Type_row" runat="server" visible="true">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align">Select Actionable Type<span class="required" style="color: red;">*</span></label>
                                        <div class="col-md-6 col-sm-6">
                                            <asp:DropDownList ID="DDL_AcType" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RFV_ACN_1" ValidationGroup="Create_ACN" ControlToValidate="DDL_AcType" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="form-group row" id="Acn_Descp_row" runat="server" visible="true">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align">Enter Actionable Description<span class="required">*</span></label>
                                        <div class="col-md-6 col-sm-6">
                                            <asp:TextBox ID="txt_acdescp" runat="server" CssClass="form-control form-control-sm rounded" ReadOnly="false" TextMode="MultiLine" Rows="3"></asp:TextBox><br />
                                            <asp:RequiredFieldValidator ID="RFV_ACN_2" ControlToValidate="txt_acdescp" runat="server" ErrorMessage="*Required" InitialValue="" ForeColor="Red" ValidationGroup="Create_ACN" />
                                        </div>
                                    </div>

                                    <div class="form-group row" id="Acn_lvl_row" runat="server" visible="true">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align">Select Priority Level<span class="required" style="color: red;">*</span></label>
                                        <div class="col-md-6 col-sm-6">
                                            <asp:DropDownList ID="DDL_Priority" runat="server" class="form-control form-control-sm rounded">
                                                <asp:ListItem Text="--Select--" Value="0" Selected="True">--Select--</asp:ListItem>
                                                <asp:ListItem Text="High" Value="1">High</asp:ListItem>
                                                <asp:ListItem Text="Medium" Value="2">Medium</asp:ListItem>
                                                <asp:ListItem Text="Low" Value="3">Low</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="Create_ACN" ControlToValidate="DDL_Priority" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="0"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="form-group row" id="Acn_remarks_row" runat="server" visible="true">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align">Enter Actionable Remarks<span class="required">*</span></label>
                                        <div class="col-md-6 col-sm-6">
                                            <asp:TextBox ID="txt_acn_rmrks" runat="server" CssClass="form-control form-control-sm rounded" ReadOnly="false" TextMode="MultiLine" Rows="3"></asp:TextBox><br />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="txt_acn_rmrks" runat="server" ErrorMessage="*Required" InitialValue="" ForeColor="Red" ValidationGroup="Create_ACN" />
                                        </div>
                                    </div>

                                    <div class="col-md-6 center-margin" id="Acn_btns_row" runat="server" visible="false">
                                        <div class="ln_solid"></div>
                                        <div class="item form-group row">
                                            <div class="col-md-6 col-sm-12">
                                                <asp:Label ID="lbl_acnbtn_msg" runat="server" Text="Click to SAVE the actionable!"></asp:Label>
                                            </div>
                                            <div class="col-md-6 col-sm-12">
                                                <button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
                                                <button type="reset" class="btn btn-warning btn-sm">Reset</button>
                                                <asp:Button ID="btn_sv_acn" runat="server" Text="ADD TO LIST" Enabled="true" CausesValidation="true" CssClass="btn btn-success btn-sm" ValidationGroup="Create_ACN" OnClick="btn_sv_acn_Click" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group row" id="Acn_tempgrid_row" runat="server" visible="true">
                                        <label class="col-form-label col-md-12 col-sm-12 label-align">Added Actionable Items</label>
                                        <div class="col-md-12 col-sm-12">
                                            <asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowDeleting="GridView1_RowDeleting">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sl" Visible="True" HeaderStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid" />
                                                        <ItemStyle CssClass="grid" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Type" HeaderStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_ActionableType" runat="server" Text='<%# Bind("ActionableType") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid" />
                                                        <ItemStyle CssClass="grid" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Actionable" HeaderStyle-Width="30%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_ActionableDescription" runat="server" Text='<%# Bind("ActionableDescription") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="30%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_ActionableRemarks" runat="server" Text='<%# Bind("ActionableRemarks") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Priority" Visible="true" HeaderStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Priority" runat="server" Text='<%# Bind("Priority") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid" />
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

                                    <div class="col-md-6 center-margin" id="Panel4_Buttons" runat="server" visible="false">
                                        <div class="ln_solid"></div>
                                        <div class="item form-group row">
                                            <div class="col-md-6 col-sm-12">
                                                <asp:Label ID="lbl_Panel4btn" runat="server" Text="Click to Add Actionables"></asp:Label>
                                            </div>
                                            <div class="col-md-6 col-sm-12">
                                                <button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
                                                <asp:Button ID="btn_skip" runat="server" Text="SKIP" CssClass="btn btn-warning btn-sm" OnClick="btn_skip_Click"/>
                                                <asp:Button ID="btn_insertacns" runat="server" Text="SAVE ALL" Enabled="true" CausesValidation="true" CssClass="btn btn-success btn-sm" OnClick="btn_insertacns_Click" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" id="Panel4_Success" runat="server" visible="false">
                                        <div class="col-md-12 col-sm-12" style="vertical-align: middle; text-align: center;">
                                            <asp:Image ID="Img_Success4" runat="server" ImageUrl="~/erp_images/success_gif.gif" Width="100px" Height="100px" />
                                            <asp:Image ID="Img_Cross4" runat="server" ImageUrl="~/erp_images/crossgif.gif" Width="100px" Height="100px" Visible="false" />
                                            <asp:Label ID="lbl_panel4_msg" runat="server" Text="Actionables Added Successfully...!" Font-Bold="true" Font-Size="Large"></asp:Label>
                                        </div>
                                    </div>


                                </div>

                                <div class="tab-pane fade" id="final" role="tabpanel" aria-labelledby="final-tab">
                                    <div class="row col-md-12 col-sm-12" runat="server" visible="true">
                                        <div class="col-md-12 col-sm-12 form-group" id="statustable" runat="server" visible="true">
                                            <table class="table table-responsive table-striped table-bordered" style="width: 100%;">
                                                <thead class="thead-dark">
                                                    <tr>
                                                        <th class="text-center">Sl no.</th>
                                                        <th>Steps Name</th>
                                                        <th class="text-center">Status</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td class="text-center">1</td>
                                                        <td>JOBID Selection</td>
                                                        <td>
                                                            <asp:Label ID="Label1" runat="server" Font-Bold="true"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="text-center">2</td>
                                                        <td>Annexure Data</td>
                                                        <td>
                                                            <asp:Label ID="Label2" runat="server" Font-Bold="true"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="text-center">3</td>
                                                        <td>TBT Photograph</td>
                                                        <td>
                                                            <asp:Label ID="Label3" runat="server" Font-Bold="true"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="text-center">4</td>
                                                        <td>Actionable Items</td>
                                                        <td>
                                                            <asp:Label ID="Label4" runat="server" Font-Bold="true"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="text-center"></td>
                                                        <td>Overall</td>
                                                        <td>
                                                            <asp:Label ID="LabelOverall" runat="server" Font-Bold="true"></asp:Label></td>
                                                    </tr>
                                                </tbody>
                                            </table>

                                        </div>
                                    </div>
                                    <div class="col-md-6 center-margin" id="TBTFinalStep" runat="server" visible="false">
                                        <div class="ln_solid"></div>
                                        <div class="item form-group row">
                                            <div class="col-md-6 col-sm-12">
                                                <asp:Label ID="Label13" runat="server" Text="Click FINISH to Close!!"></asp:Label>
                                            </div>
                                            <div class="col-md-6 col-sm-12">
                                                <asp:Button ID="Button1" runat="server" class="btn btn-danger btn-sm" CausesValidation="false" Text="Home" PostBackUrl="~/bussiness/production/homepage.aspx" />
                                                <button type="reset" class="btn btn-warning btn-sm">Reset</button>
                                                <asp:Button ID="btn_finalstep" runat="server" Text="FINISH" Enabled="true" CssClass="btn btn-success btn-sm" OnClick="btn_finalstep_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>


                            </div>
                        </div>

                    </div>

                </div>
            </div>
        </div>

        <div id="Default_Buttons" runat="server" visible="false">
            <div class="col-md-6 center-margin">
                <div class="ln_solid"></div>
                <div class="item form-group row">
                    <div class="col-md-6 col-sm-12">
                        <asp:Label ID="lbl_pagemsg" runat="server" Text="No Action to perform, Try Below!!"></asp:Label>
                    </div>
                    <div class="col-md-6 col-sm-12">
                        <asp:Button ID="btn_home" runat="server" class="btn btn-danger btn-sm" CausesValidation="false" Text="Home" PostBackUrl="~/bussiness/production/homepage.aspx" />
                        <asp:Button ID="Button2" runat="server" Text="Go Back" CssClass="btn btn-warning btn-sm" ValidationGroup="Submit" CausesValidation="false" PostBackUrl="~/bussiness/production/csms_mainview.aspx" />
                        <asp:Button ID="btn_retry" runat="server" class="btn btn-success btn-sm" Text="Refresh" CausesValidation="false" OnClientClick="reloadPage(); return false;" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
