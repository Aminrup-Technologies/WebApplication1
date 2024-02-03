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


        <%--function ValidateModuleList(source, args) {
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
		}--%>

		<%--function ToggleValidatorSftAlert(chkSftyAlert) {
		    var valName4 = document.getElementById("<%=valSftAlert.ClientID%>");
		    ValidatorEnable(valName4, chkSftyAlert.checked);
		}--%>

        function showhide() {
            var div = document.getElementById("newpost");
            if (div.style.display !== "none") {
                div.style.display = "none";
            }
            else {
                div.style.display = "block";
            }
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
                            <h2>(DOC#ATS/CSM/TBT-01)</h2>
                            <div class="clearfix"></div>
                        </div>

                        <div class="x_content">
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

                            <ul class="nav nav-tabs bar_tabs" id="myTab" role="tablist">
                                <li class="nav-item">
                                    <a class="nav-link active" id="jobid-tab" data-toggle="tab" href="#jobid" role="tab" aria-controls="jobid" aria-selected="true">JOBID</a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" id="tbt-tab" data-toggle="tab" href="#tbt" role="tab" aria-controls="tbt" aria-selected="false">Basic</a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" id="annexure-tab" data-toggle="tab" href="#annexure" role="tab" aria-controls="annexure" aria-selected="false">Annexure</a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" id="photo-tab" data-toggle="tab" href="#photo" role="tab" aria-controls="photo" aria-selected="false">Photo</a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" id="action-tab" data-toggle="tab" href="#action" role="tab" aria-controls="action" aria-selected="false">Actionable</a>
                                </li>
                            </ul>
                            <div class="clearfix">&nbsp;</div>
                            <div class="tab-content" id="myTabContent">
                                <div class="tab-pane fade show active" id="jobid" role="tabpanel" aria-labelledby="jobid-tab">

                                    <div class="form-group row">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">Select JOBID <span class="required">*</span></label>
                                        <div class="col-md-6 col-sm-6 ">
                                            <asp:DropDownList ID="DDL_JOBID" runat="server" class="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_JOBID_SelectedIndexChanged"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="CreateTBTID" ControlToValidate="DDL_JOBID" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="--Select--"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="form-group row" id="JOBIDDetails_Row" runat="server" visible="false">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                                            JOBID Details <span class="required">*</span>
                                        </label>
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
                                        <label for="middle-name" class="col-form-label col-md-3 col-sm-3 label-align">Middle Name / Initial</label>
                                        <div class="col-md-6 col-sm-6 ">
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

                                    <div class="form-group row" id="locationrow" runat="server" visible="false">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align">Location</label>
                                        <div class="col-md-6 col-sm-6 ">
                                            <asp:TextBox ID="txt_location" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row" id="deptrow" runat="server" visible="false">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align">
                                            Department <span class="required">*</span>
                                        </label>
                                        <div class="col-md-6 col-sm-6 ">
                                            <asp:TextBox ID="txt_dept" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row" id="jobsupv" runat="server" visible="false">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align">
                                            Supervisor <span class="required">*</span>
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
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="CreateTBTID" ControlToValidate="txt_linemanager" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="form-group row" id="sftyofcrrow" runat="server" visible="false">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align">
                                            Safety Officer <span class="required">*</span>
                                        </label>
                                        <div class="col-md-6 col-sm-6 ">
                                            <asp:DropDownList ID="DDL_SftyOfcr" runat="server" class="form-control form-control-sm rounded"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="CreateTBTID" ControlToValidate="DDL_SftyOfcr" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="form-group row" id="sftysupvrow" runat="server" visible="false">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align">
                                            Safety Supervisor <span class="required">*</span>
                                        </label>
                                        <div class="col-md-6 col-sm-6 ">
                                            <asp:DropDownList ID="DDL_SftySupv" runat="server" class="form-control form-control-sm rounded"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="CreateTBTID" ControlToValidate="DDL_SftySupv" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
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

                                    <div class="col-md-6 center-margin" id="CreateTBTIDRow" runat="server" visible="false">
                                        <div class="ln_solid"></div>
                                        <div class="item form-group row">
                                            <div class="col-md-6 col-sm-12">
                                                <asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to ADD Data!!"></asp:Label>
                                            </div>
                                            <div class="col-md-6 col-sm-12">
                                                <button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
                                                <button type="reset" class="btn btn-warning btn-sm">Reset</button>
                                                <asp:Button ID="btn_submit" runat="server" Text="Generate TBT ID" ValidationGroup="CreateTBTID" Enabled="true" CausesValidation="true" CssClass="btn btn-success btn-sm" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" id="TBTID_CreatedMsg" runat="server" visible="false">
                                        <div class="col-md-12 col-sm-12" style="vertical-align: middle; text-align: center;">
                                            <asp:Image ID="Img_Success1" runat="server" ImageUrl="~/erp_images/success_gif.gif" Width="100px" Height="100px" />
                                            <asp:Label ID="lbl_tbtidcreatedmsg" runat="server" Text="TBT ID Created..!" Font-Bold="true" Font-Size="Large"></asp:Label>
                                            : [<asp:Label ID="lbl_TBTID" runat="server" Text="" Visible="false" Font-Bold="true"></asp:Label>]
                                        </div>
                                    </div>

                                </div>

                                <div class="tab-pane fade" id="tbt" role="tabpanel" aria-labelledby="tbt-tab">
                                    tbt details
                                </div>

                                <div class="tab-pane fade" id="annexure" role="tabpanel" aria-labelledby="annexure-tab">

                                    <div class="form-group row">
                                        <label class="col-form-label col-md-12 col-sm-12 label-align" for="BoxName1">ITEMS DISCUSSED : <small>(Indicate if not discussed)</small><span class="required">*</span></label>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="BoxName1">1. Safety contact and review of action items from last meeting :</label>
                                        <div class="col-md-6 col-sm-6 ">
                                            <div class="col-md-3 col-sm-3">
                                                <input type="checkbox" id="BoxName1" name="BoxName1" onclick="ShowCheckboxDiv('BoxName', 9)" />
                                                Yes
                                            </div>
                                        </div>
                                        <div class="clearfix"></div>
                                        <div class="col-md-12 col-sm-12  form-group" id="BoxName1Div" style="display: none;">
                                            N/A
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="BoxName2">2. Items of General Safety Importance to the Total Work Site : (Ask employees to mention any incident / Near Miss during the past day which may have or have resuted into damage to property or injury to Company or Contractor Personnel)</label>
                                        <div class="col-md-6 col-sm-6 ">
                                            <div class="col-md-3 col-sm-3">
                                                <input type="checkbox" id="BoxName2" name="BoxName2" onclick="ShowCheckboxDiv('BoxName', 9)" />Yes
                                            </div>
                                        </div>
                                        <div class="clearfix"></div>
                                        <div class="col-md-12 col-sm-12  form-group" id="BoxName2Div" style="display: none;">
                                            <asp:Button ID="btn_newincidententry" runat="server" Text="New Incident" Enabled="false" CssClass="btn btn-primary btn-sm" />
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="BoxName3">3. Items of safety interest to this Group:(Eg. Red Stripes, Orange Stripes, Green Stripe, Safety alert tips for safety communication, hazards or safety conditions applicable to this group’s work area)</label>
                                        <div class="col-md-6 col-sm-6 ">
                                            <div class="col-md-3 col-sm-3">
                                                <input type="checkbox" id="BoxName3" name="BoxName3" onclick="ShowCheckboxDiv('BoxName', 9); ToggleValidatorSI(this);" checked="checked" />Yes
                                            </div>
                                        </div>
                                        <div class="clearfix"></div>
                                        <div class="col-md-12 col-sm-12  form-group" id="BoxName3Div" style="display: block;">
                                            <asp:TextBox ID="txt_sftyintrst" runat="server" class="form-control form-control-sm rounded" ReadOnly="false" TextMode="MultiLine" Rows="3"></asp:TextBox><br />
                                            <asp:RequiredFieldValidator ID="valSI" ControlToValidate="txt_sftyintrst" runat="server" ErrorMessage="*Required" ForeColor="Red" ValidationGroup="Group1" />
                                        </div>
                                    </div>

                                </div>

                                <div class="tab-pane fade" id="photo" role="tabpanel" aria-labelledby="photo-tab">
                                    Photograph
                                </div>

                                <div class="tab-pane fade" id="action" role="tabpanel" aria-labelledby="action-tab">
                                    Actionable
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
