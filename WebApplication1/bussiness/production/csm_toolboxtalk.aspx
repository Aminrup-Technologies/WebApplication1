<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="csm_toolboxtalk.aspx.cs" Inherits="WebApplication1.bussiness.production.toolboxtalk" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
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
    </script>

    <script type="text/javascript">
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

		<%--function ToggleValidatorSftAlert(chkSftyAlert) {
		    var valName4 = document.getElementById("<%=valSftAlert.ClientID%>");
		    ValidatorEnable(valName4, chkSftyAlert.checked);
		}--%>
    </script>

    <script>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>Tool Box Talk</h3>
                </div>
            </div>
            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>DOC#ATS/CSM/TBT-01</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row" id="jobselectionpanel" runat="server" visible="true">

                                <div class="col-md-6 col-sm-12 form-group">
                                    <label>Select JOB ID<span class="text text-danger">*</span></label>
                                </div>

                                <div class="col-md-6 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_JOBID" runat="server" class="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_JOBID_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="CreateTBTID" ControlToValidate="DDL_JOBID" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="--Select--"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-12 col-sm-12  form-group" id="JOBIDDetails_Row" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">Selected JOBID Details</label>
                                    <div class="col-md-6 col-sm-6">
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

                                <div class="col-md-12 col-sm-12  form-group" id="locationrow" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">Location<span class="text text-danger">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_location" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-12 col-sm-12  form-group" id="deptrow" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">Department<span class="text text-danger">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_dept" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-12 col-sm-12  form-group" id="jobsupv" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">JOB Supervisor<span class="text text-danger">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_supv" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-12 col-sm-12  form-group" id="LineManagerRow" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">TSL Line Manager<span class="text text-danger">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_linemanager" runat="server" Style="text-transform: uppercase" class="form-control form-control-sm rounded"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="CreateTBTID" ControlToValidate="txt_linemanager" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="col-md-12 col-sm-12  form-group" id="sftyofcrrow" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">Safety Officer<span class="text text-danger">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:DropDownList ID="DDL_SftyOfcr" runat="server" class="form-control form-control-sm rounded"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="CreateTBTID" ControlToValidate="DDL_SftyOfcr" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="col-md-12 col-sm-12  form-group" id="sftysupvrow" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">Safety Supervisor<span class="text text-danger">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:DropDownList ID="DDL_SftySupv" runat="server" class="form-control form-control-sm rounded"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="CreateTBTID" ControlToValidate="DDL_SftySupv" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="col-md-12 col-sm-12  form-group" id="supvrow" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">Contractor Representative / Area In Charge<span class="text text-danger">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_incharge" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-12 col-sm-12  form-group" id="ContractEmployeesrow" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">Contract Employees :<span class="text text-danger">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_cntrctemp" runat="server" class="form-control form-control-sm rounded"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="CreateTBTID" ControlToValidate="txt_cntrctemp" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-12 center-margin" id="Div1" runat="server" visible="false">
                                <div class="ln_solid"></div>
                            </div>

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


                            <%--ADD button-1 start--%>
                            <div class="col-md-6 center-margin" id="CreateTBTIDRow" runat="server" visible="false">
                                <div class="ln_solid"></div>
                                <div class="item form-group row">
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to ADD Data!!"></asp:Label>
                                    </div>
                                    <div class="col-md-6 col-sm-12">
                                        <button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
                                        <button type="reset" class="btn btn-warning btn-sm">Reset</button>
                                        <asp:Button ID="btn_submit" runat="server" Text="Generate TBT ID" ValidationGroup="CreateTBTID" Enabled="true" CausesValidation="true" CssClass="btn btn-success btn-sm" OnClick="btn_submit_Click" />
                                    </div>
                                </div>
                            </div>
                            <%--ADD button-1 end--%>

                            <div class="row" id="TBTID_CreatedMsg" runat="server" visible="false">
                                <div class="col-md-12 col-sm-12" style="vertical-align: middle; text-align: center;">
                                    <asp:Image ID="Img_Success1" runat="server" ImageUrl="~/erp_images/success_gif.gif" Width="100px" Height="100px" />
                                    <asp:Label ID="lbl_tbtidcreatedmsg" runat="server" Text="TBT ID Created..!" Font-Bold="true" Font-Size="Large"></asp:Label>
                                    : [<asp:Label ID="lbl_TBTID" runat="server" Text="" Visible="false" Font-Bold="true"></asp:Label>]
                                </div>
                            </div>

                            <div class="col-md-12 center-margin" id="TBTID_CreatedMsgHR" runat="server" visible="false">
                                <div class="ln_solid"></div>
                            </div>

                            <div class="section row" id="panel2heading" runat="server" visible="false">
                                <div class="col-md-12 col-sm-12">ITEMS DISCUSSED : <small>(Indicate if not discussed)</small></div>
                            </div>
                            <div class="clearfix"></div>

                            <div class="row" id="TBTPanel2" runat="server" visible="false">
                                <div class="col-md-12 col-sm-12 form-group">
                                    <asp:Label ID="Label1" class="col-form-label col-md-9 col-sm-9" runat="server" Font-Bold="true">1. Safety contact and review of action items from last meeting :</asp:Label>
                                    <div class="col-md-3 col-sm-3">
                                        <input type="checkbox" id="BoxName1" name="BoxName1" onclick="ShowCheckboxDiv('BoxName', 9)" />
                                        Yes
                                    </div>
                                </div>


                                <div class="col-md-12 col-sm-12  form-group" id="BoxName1Div" style="display: none;">
                                    <div class="col-md-12 col-sm-12">
                                        N/A
                                    </div>
                                </div>


                                <div class="col-md-12 col-sm-12  form-group">
                                    <asp:Label ID="Label2" class="col-form-label col-md-9 col-sm-9" runat="server" Font-Bold="true">2. Items of General Safety Importance to the Total Work Site : (Ask employees to mention any incident / Near Miss during the past day which may have or have resuted into damage to property or injury to Company or Contractor Personnel)</asp:Label>
                                    <div class="col-md-3 col-sm-3">
                                        <input type="checkbox" id="BoxName2" name="BoxName2" onclick="ShowCheckboxDiv('BoxName', 9)" />
                                        Yes
                                    </div>
                                </div>

                                <div class="col-md-12 col-sm-12  form-group" id="BoxName2Div" style="display: none;">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Button ID="btn_newincidententry" runat="server" Text="New Incident" Enabled="false" CssClass="btn btn-primary btn-sm" />
                                    </div>
                                </div>

                                <div class="col-md-12 col-sm-12  form-group">
                                    <asp:Label ID="Label3" class="col-form-label col-md-9 col-sm-9" runat="server" Font-Bold="true">3. Items of safety interest to this Group:(Eg. Red Stripes, Orange Stripes, Green Stripe, Safety alert tips for safety communication, hazards or safety conditions applicable to this group’s work area).</asp:Label>
                                    <div class="col-md-3 col-sm-3">
                                        <input type="checkbox" id="BoxName3" name="BoxName3" onclick="ShowCheckboxDiv('BoxName', 9); ToggleValidatorSI(this);" checked="checked" />
                                        Yes
                                    </div>
                                </div>

                                <div class="col-md-12 col-sm-12  form-group" id="BoxName3Div" style="display: block;">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:TextBox ID="txt_sftyintrst" runat="server" class="form-control form-control-sm rounded" ReadOnly="false" TextMode="MultiLine" Rows="3"></asp:TextBox><br />
                                        <asp:RequiredFieldValidator ID="valSI" ControlToValidate="txt_sftyintrst" runat="server" ErrorMessage="*Required" ForeColor="Red" ValidationGroup="Group1" />
                                    </div>
                                </div>

                                <div class="col-md-12 col-sm-12  form-group">
                                    <asp:Label ID="Label4" class="col-form-label col-md-9 col-sm-9" runat="server" Font-Bold="true">4. Standard Operation Procedures (SOP) relevant to this Group :</asp:Label>
                                    <div class="col-md-3 col-sm-3">
                                        <input type="checkbox" id="BoxName4" name="BoxName4" onclick="ShowCheckboxDiv('BoxName', 9); ToggleValidatorSOP(this);" checked="checked" />
                                        Yes
                                    </div>
                                </div>

                                <div class="col-md-12 col-sm-12  form-group" id="BoxName4Div" style="display: block;">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:TextBox ID="txt_sopno" runat="server" class="form-control form-control-sm rounded" ReadOnly="false"></asp:TextBox><br />
                                        <asp:RequiredFieldValidator ID="valSOP" ControlToValidate="txt_sopno" runat="server" ErrorMessage="*Required" ForeColor="Red" ValidationGroup="Group1" />
                                    </div>
                                </div>


                                <div class="col-md-12 col-sm-12  form-group">
                                    <asp:Label ID="Label5" class="col-form-label col-md-9 col-sm-9" runat="server" Font-Bold="true">5. Reminders to Employees of their Personal Responsibilities to ensure and Mantain :</asp:Label>
                                    <div class="col-md-3 col-sm-3">
                                        <input type="checkbox" id="BoxName5" name="BoxName5" onclick="ShowCheckboxDiv('BoxName', 9); ToggleValidatorPRSNL(this);" checked="checked" />
                                        Yes
                                    </div>
                                </div>

                                <div class="col-md-12 col-sm-12  form-group" id="BoxName5Div" style="display: block;">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:CheckBoxList ID="chkbxrspons" runat="server" Font-Size="Small" CssClass="checkbox rounded" RepeatColumns="2" Width="100%"></asp:CheckBoxList><br />
                                        <asp:CustomValidator runat="server" ID="cvmodulelist" ClientValidationFunction="ValidateModuleList" Display="Dynamic" ErrorMessage="Please select state"></asp:CustomValidator>
                                    </div>
                                    <br />
                                </div>


                                <div class="col-md-12 col-sm-12  form-group">
                                    <asp:Label ID="Label8" class="col-form-label col-md-9 col-sm-9" runat="server" Font-Bold="true">6. Hazardous materials relevant to this Group Work's Area :</asp:Label>
                                    <div class="col-md-3 col-sm-3">
                                        <input type="checkbox" id="BoxName6" name="BoxName6" onclick="ShowCheckboxDiv('BoxName', 9); ToggleValidatorHazard(this);" checked="checked" />
                                        Yes
                                    </div>
                                </div>

                                <div class="col-md-12 col-sm-12  form-group" id="BoxName6Div" style="display: block;">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:TextBox ID="txt_hazards" runat="server" class="form-control form-control-sm rounded" ReadOnly="false" TextMode="MultiLine" Rows="3"></asp:TextBox><br />
                                        <asp:RequiredFieldValidator ID="valHazard" ControlToValidate="txt_hazards" runat="server" ErrorMessage="*Required" ForeColor="Red" ValidationGroup="Group1" />
                                    </div>
                                </div>



                                <div class="col-md-12 col-sm-12  form-group">
                                    <asp:Label ID="Label6" class="col-form-label col-md-9 col-sm-9" runat="server" Font-Bold="true">7. Safety Message Handouts / Circulars to be shared with Contract Employees :</asp:Label>
                                    <div class="col-md-3 col-sm-3">
                                        <input type="checkbox" id="BoxName7" name="BoxName7" onclick="ShowCheckboxDiv('BoxName', 9); ToggleValidatorSftMsg(this);" checked="checked" />
                                        Yes
                                    </div>
                                </div>

                                <div class="col-md-12 col-sm-12  form-group" id="BoxName7Div" style="display: block;">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:TextBox ID="txt_sftmsg" runat="server" class="form-control form-control-sm rounded" ReadOnly="false" TextMode="MultiLine" Rows="3"></asp:TextBox><br />
                                        <asp:RequiredFieldValidator ID="valSftMsg" ControlToValidate="txt_sftmsg" runat="server" ErrorMessage="*Required" ForeColor="Red" ValidationGroup="Group1" />
                                    </div>
                                </div>

                                <%--<div class="col-md-12 col-sm-12  form-group">
                                    <asp:Label ID="Label9" class="col-form-label col-md-9 col-sm-9" runat="server" Font-Bold="true">8. Safety Alert Tips relevant to this Work Area :</asp:Label>
                                    <div class="col-md-3 col-sm-3">
                                        <input type="checkbox" id="BoxName8" name="BoxName8" onclick="ShowCheckboxDiv('BoxName', 9); ToggleValidatorSftAlert(this);" checked="checked" />
                                        Yes
                                    </div>
                                </div>

                                <div class="col-md-12 col-sm-12  form-group" id="BoxName8Div" style="display: block;">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:TextBox ID="txt_sftalert" runat="server" class="form-control form-control-sm rounded" ReadOnly="false" TextMode="MultiLine" Rows="3"></asp:TextBox><br />
                                        <asp:RequiredFieldValidator ID="valSftAlert" ControlToValidate="txt_sftalert" runat="server" ErrorMessage="*Required" ForeColor="Red" ValidationGroup="Group1" />
                                    </div>
                                </div>--%>

                                <div class="col-md-12 col-sm-12  form-group">
                                    <asp:Label ID="Label7" class="col-form-label col-md-9 col-sm-9" runat="server" Font-Bold="true">9. Actions resulting from this meeting and point rasied by Contract Employees and Supervisor :</asp:Label>
                                    <div class="col-md-3 col-sm-3">
                                        <input type="checkbox" id="BoxName9" name="BoxName9" onclick="ShowCheckboxDiv('BoxName', 9)" />
                                        Yes
                                    </div>
                                </div>

                                <div class="col-md-12 col-sm-12  form-group" id="BoxName9Div" style="display: none;">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Button ID="btn_addfeedback" runat="server" Text="Add Actionable" Enabled="false" CssClass="btn btn-primary btn-sm" />
                                    </div>
                                </div>
                            </div>


                            <%--ADD button-2 start--%>
                            <div class="col-md-6 center-margin" id="SavePanel2Data" runat="server" visible="false">
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
                            <%--ADD button-2 end--%>

                            <div class="row" id="TBT_ItemsSavedMsg" runat="server" visible="false">
                                <div class="col-md-12 col-sm-12" style="vertical-align: middle; text-align: center;">
                                    <asp:Image ID="Img_Success2" runat="server" ImageUrl="~/erp_images/success_gif.gif" Width="100px" Height="100px" />
                                    <asp:Label ID="lbl_tbtitemssavedmsg" runat="server" Text="TBT Items Data Saved..!" Font-Bold="true" Font-Size="Large"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-12 center-margin" id="TBT_ItemsSavedMsgHR" runat="server" visible="false">
                                <div class="ln_solid"></div>
                            </div>

                            <div class="col-md-12 center-margin" id="Div2" runat="server" visible="false">
                                <div class="ln_solid"></div>
                            </div>

                            <div class="row" id="TBTPhotographRow" runat="server" visible="false">
                                <div class="col-md-6 col-sm-6 form-group" id="uploadbuttonrow1" runat="server" visible="true">
                                    <label>Upload Photograph (.jpeg / .jpg) <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-6 col-sm-6 form-group" id="uploadbuttonrow2" runat="server" visible="true">
                                    <button type="button" class="btn btn-primary btn-sm" id="btnShowPopup" data-toggle="modal" data-target="#myModal">
                                        <i class="fa fa-plus-circle"></i>&nbsp;Meeting Photograph
                                    </button>
                                </div>

                                <div class="col-md-6 col-sm-12 form-group" id="UploadedPhotoRow1" runat="server" visible="false">
                                    <label>Uploaded Photograph<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-6 col-sm-12 form-group" id="UploadedPhotoRow2" runat="server" visible="false">
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
                            <div class="col-md-6 center-margin" id="SavePanel3Data" runat="server" visible="false">
                                <div class="ln_solid"></div>
                                <div class="item form-group row">
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Label ID="Label12" runat="server" Text="Click PROCEED to SAVE Data!!"></asp:Label>
                                    </div>
                                    <div class="col-md-6 col-sm-12">
                                        <button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
                                        <button type="reset" class="btn btn-warning btn-sm">Reset</button>
                                        <asp:Button ID="btn_saveTBTPhoto" runat="server" Text="PROCEED" Enabled="true" CssClass="btn btn-success btn-sm" OnClick="btn_saveTBTPhoto_Click" />
                                    </div>
                                </div>
                            </div>
                            <%--ADD button-3 end--%>


                            <div class="row" id="TBTPhotoUploaded" runat="server" visible="false">
                                <div class="col-md-12 col-sm-12" style="vertical-align: middle; text-align: center;">
                                    <asp:Image ID="Img_Success" runat="server" ImageUrl="~/erp_images/success_gif.gif" Width="100px" Height="100px" />
                                    <asp:Label ID="Label11" runat="server" Text="Photograph Uploaded Successfully...!" Font-Bold="true" Font-Size="Large"></asp:Label>
                                    <asp:Label ID="lbl_tbtphotoid" runat="server" Text=""></asp:Label>
                                </div>
                            </div>

                            <%--ADD button-4 start--%>
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
                            <%--ADD button-4 end--%>
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
                            <asp:Button ID="btn_back" runat="server" Text="Go Back" CssClass="btn btn-warning btn-sm" ValidationGroup="Submit" CausesValidation="false" PostBackUrl="~/bussiness/production/csms_mainview.aspx" />
                            <asp:Button ID="btn_retry" runat="server" class="btn btn-success btn-sm" Text="Retry" CausesValidation="false" OnClientClick="reloadPage(); return false;" />
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
