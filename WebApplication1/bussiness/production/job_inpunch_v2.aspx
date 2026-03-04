<%@ Page Title="JOB IN-Punch V2" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="job_inpunch_v2.aspx.cs" Inherits="WebApplication1.bussiness.production.job_inpunch_v2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .top-label {
            font-weight: 600;
            margin-bottom: 5px;
            color: #333;
        }

        .data-label {
            font-weight: bold;
            color: #0056b3;
        }

        .req-star {
            color: red;
        }

        #loadingOverlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.4);
            z-index: 9999;
        }

        .spinner-container {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            text-align: center;
            color: white;
        }

        .loader {
            border: 6px solid #f3f3f3;
            border-top: 6px solid #007bff;
            border-radius: 50%;
            width: 60px;
            height: 60px;
            animation: spin 1s linear infinite;
            margin: auto;
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }

        .loading-text {
            margin-top: 15px;
            font-size: 18px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="container">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Step 3: Manpower IN-Punch <small>Smart Scanner</small></h2>
                            <div class="clearfix"></div>
                        </div>

                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>

                                <div class="x_content bg-light p-3 mb-3" style="border-radius: 5px; border: 1px solid #ddd;" id="InpunchPanel_Row" runat="server">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-12 form-group">
                                            <label class="top-label">Select Active JOB ID <span class="req-star">*</span></label>
                                            <asp:DropDownList ID="DDL_JOBID" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_JOBID_SelectedIndexChanged"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="ADDTOLIST" ControlToValidate="DDL_JOBID" runat="server" ErrorMessage="Required" CssClass="text-danger" Display="Dynamic" InitialValue="--Select--"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>

                                <div class="x_content" id="JOBIDDetails_Row" runat="server" visible="false">
                                    <div class="row" style="background-color: #fdfdfe; border: 1px solid #e5e5e5; padding: 15px; border-radius: 4px; margin-bottom: 20px; box-shadow: 0 1px 2px rgba(0,0,0,0.05);">
                                        <div class="col-md-12">
                                            <h5 class="text-primary"><i class="fa fa-info-circle"></i>JOB Details</h5>
                                            <div class="ln_solid mt-1 mb-3"></div>
                                        </div>

                                        <div class="col-md-4 col-sm-6 mb-2">
                                            <label class="text-muted">JOB ID:</label>
                                            <asp:Label ID="lbl_jobid" runat="server" CssClass="data-label"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-sm-6 mb-2">
                                            <label class="text-muted">Date:</label>
                                            <asp:Label ID="lbl_jobiddate" runat="server" CssClass="data-label"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-sm-6 mb-2">
                                            <label class="text-muted">Permit No:</label>
                                            <asp:Label ID="lbl_permitno" runat="server" CssClass="data-label text-danger"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-sm-6 mb-2">
                                            <label class="text-muted">Site:</label>
                                            <asp:Label ID="lbl_jobsite" runat="server" CssClass="data-label"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-sm-6 mb-2">
                                            <label class="text-muted">Location:</label>
                                            <asp:Label ID="lbl_jobloc" runat="server" CssClass="data-label"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-sm-6 mb-2">
                                            <label class="text-muted">Shift:</label>
                                            <asp:Label ID="lbl_jobshift" runat="server" CssClass="data-label text-success"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-sm-6 mb-2">
                                            <label class="text-muted">Work Order:</label>
                                            <asp:Label ID="lbl_wrkordr" runat="server" CssClass="data-label"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-sm-6 mb-2">
                                            <label class="text-muted">In-Charge:</label>
                                            <asp:Label ID="lbl_inchargename" runat="server" CssClass="data-label"></asp:Label>
                                        </div>

                                        <asp:Label ID="lbl_jobcreatorname" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_creatorwrk" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_creatorregion" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_creatorcompany" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_crtrsitename" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_crtrsitecode" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_jobrgn" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_jobcompay" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_jobsitecode" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_inchargewrk" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_dept" runat="server" Visible="false"></asp:Label>
                                    </div>

                                    <div class="ln_solid"></div>

                                    <div class="row align-items-center" id="WorkmanInput_Row" runat="server" visible="false" style="background-color: #fff8e1; padding: 15px; border-radius: 5px; border: 1px dashed #ffc107;">
                                        <div class="col-md-12 mb-2">
                                            <span class="text-warning font-weight-bold"><i class="fa fa-clock-o"></i>Set IN-Punch Time for Scanned Workers</span>
                                        </div>
                                        <div class="col-md-3 col-sm-6 form-group">
                                            <label class="top-label">IN Date <span class="req-star">*</span></label>
                                            <asp:TextBox ID="txt_date" runat="server" CssClass="form-control form-control-sm rounded" type="date"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3 col-sm-6 form-group">
                                            <label class="top-label">IN Time <span class="req-star">*</span></label>
                                            <asp:TextBox ID="txt_time" runat="server" CssClass="form-control form-control-sm rounded" type="time"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4 col-sm-6 form-group">
                                            <label class="top-label text-primary"><i class="fa fa-barcode"></i>Scan / Enter Workman ID</label>
                                            <asp:TextBox ID="txt_empworkman" CssClass="form-control form-control-sm rounded" runat="server" AutoPostBack="true" placeholder="Enter ID & Press Enter" OnTextChanged="txt_empworkman_TextChanged" Style="border: 2px solid #007bff;"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row mt-3" id="EmployeeData_Row" runat="server" visible="false" style="background-color: #e8f5e9; padding: 15px; border-radius: 5px;">
                                        <div class="col-md-3 col-sm-6 form-group">
                                            <label class="top-label">Employee Name</label>
                                            <asp:TextBox ID="txt_empname" runat="server" ReadOnly="true" CssClass="form-control form-control-sm rounded font-weight-bold text-success"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3 col-sm-6 form-group">
                                            <label class="top-label">Designation</label><br />
                                            <asp:Label ID="lbl_designation" runat="server" CssClass="badge bg-blue"></asp:Label>
                                        </div>
                                        <div class="col-md-3 col-sm-6 form-group">
                                            <label class="top-label">Gatepass & Validity</label><br />
                                            <span class="text-danger font-weight-bold">[<asp:Label ID="lbl_gpno" runat="server"></asp:Label>]</span> : 
                                                <asp:Label ID="lbl_gpvalidty" runat="server"></asp:Label>
                                            (<asp:Label ID="lbl_gpdays" runat="server"></asp:Label>
                                            Days)
    
                                            <asp:Button ID="btnShowPopup2" runat="server" Text="Update Pass" CssClass="btn btn-warning btn-xs ml-2" Visible="false" OnClientClick="$('#myModal2').modal('show'); return false;" />
                                        </div>
                                        <div class="col-md-3 col-sm-6 form-group text-right">
                                            <br />
                                            <asp:Button ID="btn_submit" runat="server" Text="Add to Roster" ValidationGroup="ADDTOLIST" CssClass="btn btn-success btn-sm" OnClick="btn_submit_Click" />
                                            <asp:Button ID="btn_reset" runat="server" Text="Clear" CssClass="btn btn-warning btn-sm" OnClick="btn_reset_Click" />
                                        </div>

                                        <asp:Label ID="lbl_workhours" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_designationcode" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_category" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_categorycode" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_pocategoryname" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_pocategorycode" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_sftyno" runat="server" Visible="false"></asp:Label>
                                        <asp:TextBox ID="txt_worksite" runat="server" Visible="false"></asp:TextBox>
                                        <asp:Label ID="lbl_worksitecode" runat="server" Visible="false"></asp:Label>
                                    </div>

                                    <div class="row mt-4" id="ViewState_TableRow" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <h5 class="text-success"><i class="fa fa-users"></i>Staged for IN-Punch</h5>
                                            <asp:GridView ID="GridView1" runat="server" Width="100%" CssClass="table table-striped table-bordered table-sm" AutoGenerateColumns="false" EmptyDataText="No Data Found" OnRowDeleting="GridView1_RowDeleting">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="SL" ItemStyle-Width="5%" ItemStyle-CssClass="text-center">
                                                        <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Workman ID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_wrk" runat="server" Text='<%# Bind("wrk") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_name" runat="server" Text='<%# Bind("name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="IN Time">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_in" runat="server" Text='<%# Bind("in") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Action" ItemStyle-Width="5%" ItemStyle-CssClass="text-center">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btndelete" runat="server" CommandName="Delete" CssClass="text-danger" ToolTip="Remove from list"><i class="fa fa-trash"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_wrkhrs" runat="server" Text='<%# Bind("wrkhrs") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_category" runat="server" Text='<%# Bind("category") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_categorycode" runat="server" Text='<%# Bind("categorycode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_po_category" runat="server" Text='<%# Bind("po_category") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_po_categorycode" runat="server" Text='<%# Bind("po_categorycode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_designation" runat="server" Text='<%# Bind("designation") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_designationcode" runat="server" Text='<%# Bind("designationcode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_gpno" runat="server" Text='<%# Bind("gpno") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_sftyno" runat="server" Text='<%# Bind("sftyno") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_wrksitename" runat="server" Text='<%# Bind("wrksitename") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_wrksitecode" runat="server" Text='<%# Bind("wrksitecode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>

                                    <div class="row mt-4" id="ExistingWorkers_Row" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <h5 class="text-info"><i class="fa fa-check-square-o"></i>Already IN-Punched Members (Active in Database)</h5>
                                            <div class="card-box table-responsive">
                                                <asp:GridView ID="gvExistingWorkers" runat="server" Width="100%" CssClass="table table-bordered table-sm" AutoGenerateColumns="false" BackColor="#fdfdfe" DataKeyNames="Id" OnRowDeleting="gvExistingWorkers_RowDeleting">
                                                    <HeaderStyle BackColor="#d1ecf1" ForeColor="#0c5460" CssClass="text-center" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="SL" ItemStyle-Width="5%" ItemStyle-CssClass="text-center">
                                                            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="EmployeeWrk" HeaderText="Workman ID" ItemStyle-CssClass="font-weight-bold" />
                                                        <asp:BoundField DataField="EmployeeName" HeaderText="Name" />
                                                        <asp:BoundField DataField="Inpunch_Time" HeaderText="IN Time" ItemStyle-CssClass="text-success" />

                                                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="10%" ItemStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="btnDeleteDB" runat="server" CommandName="Delete" CssClass="btn btn-danger btn-xs" OnClientClick="return confirm('Are you sure you want to remove this worker from the active job?');" ToolTip="Remove from active JOB"><i class="fa fa-trash"></i></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="SendAttendance_Buttons" runat="server" visible="false">
                                        <div class="ln_solid"></div>
                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btn_finalsubmit" runat="server" Text="Finalize IN-Punch" CssClass="btn btn-primary btn-lg" OnClientClick="showLoader();" OnClick="btn_finalsubmit_Click" />
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <div id="loadingOverlay" style="display: none;">
                            <div class="spinner-container">
                                <div class="loader"></div>
                                <div class="loading-text">Finalizing IN-Punch...</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="myModal2" data-backdrop="static">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-warning">
                    <h4 class="modal-title">Update Gatepass / Safety Pass</h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6 form-group">
                            <label>New Gatepass No:</label>
                            <asp:TextBox ID="txt_nwgpno" runat="server" CssClass="form-control form-control-sm rounded"></asp:TextBox>
                        </div>
                        <div class="col-md-6 form-group">
                            <label>New GP Validity:</label>
                            <asp:TextBox ID="txt_nwgpvalidity" runat="server" CssClass="form-control form-control-sm rounded" type="date"></asp:TextBox>
                        </div>
                        <div class="col-md-6 form-group">
                            <label>New Safety No:</label>
                            <asp:TextBox ID="txt_nwsftyno" runat="server" CssClass="form-control form-control-sm rounded"></asp:TextBox>
                        </div>
                        <div class="col-md-6 form-group">
                            <label>New Safety Validity:</label>
                            <asp:TextBox ID="txt_nwsftyvalidity" runat="server" CssClass="form-control form-control-sm rounded" type="date"></asp:TextBox>
                        </div>
                        <div class="col-md-12 form-group">
                            <label>New PV Validity:</label>
                            <asp:TextBox ID="txt_nwpvvalidity" runat="server" CssClass="form-control form-control-sm rounded" type="date"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btn_gtpsedit" runat="server" Text="Save Changes" CssClass="btn btn-success btn-sm" OnClick="btn_gtpsedit_Click" />
                    <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function showPNotify(title, text, type) {
            new PNotify({
                title: title, text: text, type: type, styling: 'bootstrap3', delay: 5000
            });
        }
        function showLoader() { document.getElementById("loadingOverlay").style.display = "block"; }
        function hideLoader() { document.getElementById("loadingOverlay").style.display = "none"; }

        // Future Date/Time Prevention Logic
        function AttachInPunchDateListeners() {
            const inPunchDate = document.getElementById('<%= txt_date.ClientID %>');
            const inPunchTime = document.getElementById('<%= txt_time.ClientID %>');

            if (!inPunchDate || !inPunchTime) return;

            function validateInPunch() {
                let now = new Date();
                let currentDate = now.toISOString().split("T")[0];
                let currentHours = now.getHours();
                let currentMinutes = now.getMinutes();

                let selectedDate = inPunchDate.value;
                let selectedTime = inPunchTime.value;

                if (!selectedDate || !selectedTime) return;

                if (selectedTime.includes(":")) {
                    let timeParts = selectedTime.split(":");
                    let selectedHours = parseInt(timeParts[0], 10);
                    let selectedMinutes = parseInt(timeParts[1], 10);

                    if (selectedDate > currentDate) {
                        showPNotify('Warning', 'You cannot punch IN for a future date.', 'error');
                        inPunchDate.value = currentDate; // Reset to today
                        return;
                    }

                    if (selectedDate === currentDate) {
                        if (selectedHours > currentHours || (selectedHours === currentHours && selectedMinutes > currentMinutes)) {
                            showPNotify('Warning', 'You cannot punch IN for a future time today.', 'error');
                            inPunchTime.value = ""; // Reset
                        }
                    }
                }
            }

            inPunchDate.addEventListener("change", validateInPunch);
            inPunchTime.addEventListener("change", validateInPunch);
        }

        // CLIENT-SIDE LOCK: Prevents rapid double-scanning while AJAX is processing
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(function () {
            var scannerInput = document.getElementById('<%= txt_empworkman.ClientID %>');
            if (scannerInput) {
                scannerInput.readOnly = true;
                scannerInput.style.backgroundColor = "#e9ecef";
            }
        });

            prm.add_endRequest(function () {
                hideLoader(); // Ensures spinner vanishes if an error occurs

                AttachInPunchDateListeners(); // Re-attach date validations

                var scannerInput = document.getElementById('<%= txt_empworkman.ClientID %>');
                if (scannerInput) {
                    scannerInput.readOnly = false;
                    scannerInput.style.backgroundColor = "#fff";
                    scannerInput.focus();
                }
            });

            // Initial Load
            document.addEventListener("DOMContentLoaded", AttachInPunchDateListeners);
    </script>
</asp:Content>
