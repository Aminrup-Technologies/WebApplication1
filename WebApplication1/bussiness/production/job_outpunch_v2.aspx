<%@ Page Title="JOB OUT-Punch V2" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="job_outpunch_v2.aspx.cs" Inherits="WebApplication1.bussiness.production.job_outpunch_v2" %>

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

        /* Loader CSS */
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
            border-top: 6px solid #d9534f; /* Red for exit */
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
            font-weight: bold;
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
                            <h2>Step 4: Manpower OUT-Punch <small>Smart Workflow</small></h2>
                            <div class="clearfix"></div>
                        </div>

                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>

                                <div class="x_content bg-light p-3 mb-3" style="border-radius: 5px; border: 1px solid #ddd;" id="OUTpunchPanel_Row" runat="server" visible="true">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-12 form-group">
                                            <label class="top-label">Select Active JOB ID <span class="req-star">*</span></label>
                                            <asp:DropDownList ID="DDL_JOBID" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_JOBID_SelectedIndexChanged"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="DDL_JOBID" runat="server" ErrorMessage="Required" CssClass="text-danger" Display="Dynamic" InitialValue="--Select--"></asp:RequiredFieldValidator>
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

                                    <div class="row" id="CSMRow" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="alert alert-info">
                                                <i class="fa fa-shield"></i><strong>Safety Compliance:</strong>
                                                TBT Count:
                                                <asp:Label ID="lbl_tbtcount" runat="server" CssClass="badge bg-green" Text="0"></asp:Label>
                                                | 
                                                SOP Count:
                                                <asp:Label ID="lbl_sopcount" runat="server" CssClass="badge bg-green" Text="0"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" id="IncompleteCSM" runat="server" visible="false">
                                        <div class="col-md-12 text-center p-4">
                                            <i class="fa fa-times-circle text-danger" style="font-size: 50px;"></i>
                                            <h4 class="text-danger mt-2">Incomplete Safety Documents!</h4>
                                            <p>You must complete Toolbox Talk (TBT) and SOP Training before punching out workers.</p>
                                            <a href="csm_tbtform.aspx" class="btn btn-primary btn-sm">Complete TBT</a>
                                            <a href="csm_soptraining.aspx" class="btn btn-info btn-sm">Complete SOP</a>
                                        </div>
                                    </div>

                                    <div class="row" id="PunchOutForm_Row" runat="server" visible="false" style="background-color: #fff3cd; padding: 15px; border-radius: 5px; border: 1px solid #ffeeba;">
                                        <div class="col-md-12">
                                            <h5 class="text-danger"><i class="fa fa-sign-out"></i>Process Exit For:
                                                <asp:Label ID="txt_empname" runat="server" Font-Bold="true"></asp:Label></h5>
                                        </div>

                                        <div class="col-md-2 col-sm-6 form-group">
                                            <label class="top-label">IN Time</label>
                                            <asp:TextBox ID="txt_inpunchtime" runat="server" CssClass="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2 col-sm-6 form-group">
                                            <label class="top-label">OUT Date <span class="req-star">*</span></label>
                                            <asp:TextBox ID="txt_date" runat="server" CssClass="form-control form-control-sm rounded" type="date"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2 col-sm-6 form-group">
                                            <label class="top-label">OUT Time <span class="req-star">*</span></label>
                                            <asp:TextBox ID="txt_time" runat="server" CssClass="form-control form-control-sm rounded" type="time"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2 col-sm-6 form-group">
                                            <label class="top-label">Lunch <span class="req-star">*</span></label>
                                            <asp:RadioButtonList ID="RBTN_LunchFactor" runat="server" RepeatDirection="Horizontal" CssClass="table-borderless">
                                                <asp:ListItem Selected="True" Value="Yes">Yes&nbsp;</asp:ListItem>
                                                <asp:ListItem Value="No">No</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class="col-md-2 col-sm-6 form-group">
                                            <label class="top-label">Attn Code <span class="req-star">*</span></label>
                                            <asp:DropDownList ID="DDL_AttenCode" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2 col-sm-6 form-group">
                                            <label class="top-label">Over Time (Hrs) <span class="req-star">*</span></label>
                                            <asp:TextBox ID="txt_ot" runat="server" CssClass="form-control form-control-sm rounded" Text="0" type="number" max="16" min="0"></asp:TextBox>
                                            <asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="PunchOUT_Button" ControlToValidate="txt_ot" CssClass="text-danger" Display="Dynamic" ClientValidationFunction="validateInput"></asp:CustomValidator>
                                        </div>
                                        <div class="col-md-12 text-right mt-2">
                                            <asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="btn btn-warning btn-sm" CausesValidation="false" OnClick="btn_cancel_Click" />
                                            <asp:Button ID="btn_punchout" runat="server" Text="Confirm Punch OUT" ValidationGroup="PunchOUT_Button" CssClass="btn btn-danger btn-sm" OnClientClick="showLoader();" OnClick="btn_punchout_Click" />
                                        </div>

                                        <asp:Label ID="lbl_Id" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_empworkman" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_workhours" runat="server" Visible="false"></asp:Label>
                                    </div>

                                    <div class="row mt-4" id="ViewState_TableRow" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <h5 class="text-primary"><i class="fa fa-users"></i>Workers Currently IN</h5>
                                            <div class="card-box table-responsive">
                                                <asp:GridView ID="GridView1" runat="server" Width="100%" CssClass="table table-striped table-bordered table-sm" AutoGenerateColumns="false" EmptyDataText="All workers have been punched out." OnRowDeleting="GridView1_RowDeleting">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="SL" ItemStyle-Width="5%" ItemStyle-CssClass="text-center">
                                                            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Id" runat="server" Text='<%# Bind("Id") %>'></asp:Label></ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Workman ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_EmployeeWrk" runat="server" Text='<%# Bind("EmployeeWrk") %>'></asp:Label></ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_EmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label></ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="IN Time">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Inpunch_Time" runat="server" Text='<%# Bind("Inpunch_Time") %>'></asp:Label></ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="15%" ItemStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <asp:Button ID="PunchOUT" runat="server" CssClass="btn btn-sm btn-danger" Text="Punch OUT" CommandArgument='<%# Eval("Id") %>' OnClick="PunchOUT_Click" />
                                                                <asp:LinkButton ID="btndelete" runat="server" CommandName="Delete" CssClass="btn btn-default btn-sm ml-2" OnClientClick="return confirm('Are you sure you want to delete this record entirely?');" ToolTip="Delete Record"><i class="fa fa-trash text-danger"></i></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" id="NoPenidngPunch" runat="server" visible="false">
                                        <div class="col-md-12 text-center p-4">
                                            <i class="fa fa-check-circle text-success" style="font-size: 60px;"></i>
                                            <h3 class="text-success mt-2">Job Successfully Closed!</h3>
                                            <p style="font-size: 16px;">All workers have been punched out. This JOB ID is now marked as <strong>Exit</strong> and ready for Memo generation.</p>
                                        </div>
                                    </div>

                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <div id="loadingOverlay" style="display: none;">
                            <div class="spinner-container">
                                <div class="loader"></div>
                                <div class="loading-text">Processing Exit...</div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function showPNotify(title, text, type) {
            new PNotify({
                title: title, text: text, type: type, styling: 'bootstrap3', delay: 4000
            });
        }

        function showLoader() { document.getElementById("loadingOverlay").style.display = "block"; }
        function hideLoader() { document.getElementById("loadingOverlay").style.display = "none"; }

        function validateInput(sender, args) {
            var inputValue = document.getElementById('<%= txt_ot.ClientID %>');
            if (inputValue && inputValue.value.trim() !== '') {
                var numericValue = parseInt(inputValue.value);
                if (isNaN(numericValue) || numericValue > 16 || numericValue < 0) {
                    args.IsValid = false;
                } else {
                    args.IsValid = true;
                }
            } else {
                args.IsValid = false;
            }
        }

        // Re-attach listeners after UpdatePanel partial postback
        function AttachDateListeners() {
            const outPunchDate = document.getElementById('<%= txt_date.ClientID %>');
            const outPunchTime = document.getElementById('<%= txt_time.ClientID %>');

            if (!outPunchDate || !outPunchTime) return;

            function validateOutPunch() {
                let now = new Date();
                let currentDate = now.toISOString().split("T")[0];
                let currentHours = now.getHours();
                let currentMinutes = now.getMinutes();

                let selectedDate = outPunchDate.value;
                let selectedTime = outPunchTime.value;

                if (!selectedDate || !selectedTime) return;

                if (selectedTime.includes(":")) {
                    let timeParts = selectedTime.split(":");
                    let selectedHours = parseInt(timeParts[0], 10);
                    let selectedMinutes = parseInt(timeParts[1], 10);

                    if (selectedDate > currentDate) {
                        showPNotify('Warning', 'You cannot punch out for a future date.', 'error');
                        outPunchDate.value = currentDate;
                        return;
                    }

                    if (selectedDate === currentDate) {
                        if (selectedHours > currentHours || (selectedHours === currentHours && selectedMinutes > currentMinutes)) {
                            showPNotify('Warning', 'You cannot punch out beyond the current time.', 'error');
                            outPunchTime.value = "";
                        }
                    }
                }
            }

            outPunchDate.addEventListener("change", validateOutPunch);
            outPunchTime.addEventListener("change", validateOutPunch);
        }

        // Run on initial load and clear loader after every AJAX call
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            hideLoader();
            AttachDateListeners();
        });
        document.addEventListener("DOMContentLoaded", AttachDateListeners);
    </script>
</asp:Content>
