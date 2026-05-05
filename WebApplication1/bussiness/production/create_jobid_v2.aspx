<%@ Page Title="Create JOBID V2" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="create_jobid_v2.aspx.cs" Inherits="WebApplication1.bussiness.production.create_jobid_v2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /* Base typography and layout */
        .top-label {
            font-weight: 600;
            margin-bottom: 6px;
            color: #2a3f54; /* Gentelella dark text */
            font-size: 13px;
            letter-spacing: 0.3px;
            display: inline-block;
        }

        .req-star {
            color: #E74C3C;
            font-weight: bold;
            margin-left: 2px;
        }

        /* Modern Panel Styling */
        .modern-panel {
            border: none !important;
            border-radius: 10px;
            box-shadow: 0 4px 10px rgba(0,0,0,0.05), 0 1px 3px rgba(0,0,0,0.03);
            background: #ffffff;
            margin-bottom: 20px;
            transition: box-shadow 0.3s ease;
        }

        .modern-title {
            border-bottom: 1px solid #f0f2f5 !important;
            padding: 16px 20px !important;
        }

            .modern-title h2 {
                font-weight: 600;
                color: #34495e;
                font-size: 18px;
            }

        /* Top Header Button */
        .modern-header-btn {
            display: inline-block;
            background: linear-gradient(145deg, #6c757d, #5a6268);
            color: white;
            border: none;
            border-radius: 20px;
            padding: 6px 16px;
            font-size: 13px;
            font-weight: 600;
            text-decoration: none;
            box-shadow: 0 3px 6px rgba(0,0,0,0.1);
            transition: all 0.3s ease;
        }

            .modern-header-btn:hover {
                background: linear-gradient(145deg, #5a6268, #4e555b);
                color: white;
                transform: translateY(-2px);
                box-shadow: 0 5px 12px rgba(0,0,0,0.15);
                text-decoration: none;
            }

        /* Modern Inputs */
        .modern-input {
            border: 1px solid #dce1e5 !important;
            border-radius: 6px !important;
            padding: 8px 12px;
            height: auto !important;
            font-size: 14px;
            color: #495057;
            box-shadow: inset 0 1px 2px rgba(0,0,0,0.02);
            transition: border-color 0.2s ease, box-shadow 0.2s ease;
        }

            .modern-input:focus {
                border-color: #1ABB9C !important;
                box-shadow: 0 0 0 3px rgba(26, 187, 156, 0.15) !important;
                outline: none;
            }

        /* Date Picker Bar */
        .modern-date-bar {
            background: #f8f9fa;
            border: 1px solid #e9ecef;
            border-radius: 8px;
            padding: 15px;
            margin-bottom: 25px;
            box-shadow: inset 0 2px 4px rgba(0,0,0,0.01);
        }

        /* Alerts and Info Boxes */
        .modern-alert {
            border-radius: 8px;
            border-left: 4px solid #3498DB;
            background-color: #ebf5fb;
            color: #2980b9;
            padding: 10px 15px;
            display: flex;
            align-items: center;
        }

            .modern-alert i {
                margin-right: 10px;
                font-size: 18px;
            }

        /* Document Checkbox List */
        .modern-doc-list {
            background-color: #f8f9fa;
            border: 1px solid #e9ecef;
            border-radius: 8px;
            padding: 15px;
        }

            .modern-doc-list label {
                margin-left: 8px;
                font-weight: 500;
                color: #5A738E;
                cursor: pointer;
            }

            .modern-doc-list input[type="checkbox"] {
                transform: scale(1.1);
                cursor: pointer;
            }

        /* Action Buttons */
        .modern-actions .btn {
            border-radius: 20px;
            padding: 8px 20px;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            font-size: 12px;
            transition: all 0.2s ease;
        }

            .modern-actions .btn:hover {
                transform: translateY(-2px);
                box-shadow: 0 4px 8px rgba(0,0,0,0.15);
            }

        /* GPS Retry Button */
        .btn-gps-retry {
            padding: 1px 8px;
            font-size: 11px;
            background: #ffffff;
            border: 1px solid #ced4da;
            border-radius: 12px;
            color: #6c757d;
            transition: all 0.2s;
        }

            .btn-gps-retry:hover {
                background: #e2e6ea;
                color: #495057;
                border-color: #adb5bd;
            }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="container">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel modern-panel">
                        <div class="x_title modern-title" style="display: flex; justify-content: space-between; align-items: center;">
                            <h2 style="margin: 0;">Step 1: Create JOB ID <small style="color: #1ABB9C; font-weight: 600;">Smart Workflow</small></h2>
                            <a href="create_jobid.aspx" class="modern-header-btn">
                                <i class="fa fa-history" style="margin-right: 5px;"></i>Switch to OLD Version
                            </a>
                            <div class="clearfix"></div>
                        </div>

                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>

                                <div class="x_content">
                                    <div class="modern-date-bar" id="Div1" runat="server" visible="true">
                                        <div class="row">
                                            <div class="col-md-12 text-center" style="vertical-align: middle; font-size: 15px;">
                                                <span style="font-weight: 600; color: #34495e;">Select JOB Date: </span>
                                                <asp:TextBox ID="txt_jobdate" runat="server" TextMode="Date" CssClass="form-control modern-input d-inline-block" Style="width: auto; font-weight: 600; margin: 0 10px;" AutoPostBack="true" OnTextChanged="txt_jobdate_TextChanged"></asp:TextBox>
                                                <span style="font-weight: 600; color: #34495e;">Day: </span>
                                                <asp:Label ID="lbl_jobday" runat="server" Text="" ForeColor="#1ABB9C" Font-Bold="true"></asp:Label>

                                                <div id="div_existing_jobs" runat="server" visible="false" class="mt-3">
                                                    <div class="alert alert-danger" style="display: inline-block; padding: 8px 15px; margin-bottom: 0; border-radius: 6px;">
                                                        <i class="fa fa-exclamation-triangle"></i><strong>Warning:</strong> You already have Active JOB(s) for this date: 
                                                        <asp:Label ID="lbl_existing_jobs_list" runat="server" CssClass="badge bg-red" Style="font-size: 13px;"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="jobid_creation" runat="server" visible="true">
                                        <div class="row">
                                            <div class="col-md-3 col-sm-6 col-xs-12 form-group">
                                                <label class="top-label">Work Region <span class="req-star">*</span></label>
                                                <asp:DropDownList ID="DDL_Region" runat="server" CssClass="form-control modern-input" AutoPostBack="true" OnSelectedIndexChanged="DDL_Region_SelectedIndexChanged"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ValidationGroup="Submit" runat="server" CssClass="text-danger small" Display="Dynamic" ErrorMessage="Required" ControlToValidate="DDL_Region" InitialValue="Please Select Option"></asp:RequiredFieldValidator>

                                                <div id="div_gps_status" runat="server" visible="false" class="mt-2" style="display: flex; align-items: center; justify-content: space-between; background: #f8f9fa; padding: 6px 10px; border-radius: 6px; border: 1px solid #e9ecef;">
                                                    <span id="gps_indicator" class="text-warning font-weight-bold" style="font-size: 12px;">
                                                        <i class="fa fa-spinner fa-spin" style="margin-right: 4px;"></i>Acquiring GPS...
                                                    </span>
                                                    <button type="button" class="btn btn-gps-retry m-0" onclick="requestGPSLocation(true);" title="Retry GPS Connection">
                                                        <i class="fa fa-refresh"></i>Retry
                                                    </button>
                                                </div>

                                                <asp:HiddenField ID="hf_latitude" runat="server" />
                                                <asp:HiddenField ID="hf_longitude" runat="server" />
                                            </div>

                                            <div class="col-md-3 col-sm-6 col-xs-12 form-group">
                                                <label class="top-label">Work Order <span class="req-star">*</span></label>
                                                <asp:DropDownList ID="DDL_Workorder" runat="server" CssClass="form-control modern-input" AutoPostBack="true" OnSelectedIndexChanged="DDL_Workorder_SelectedIndexChanged"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="Submit" runat="server" CssClass="text-danger small" Display="Dynamic" ErrorMessage="Required" ControlToValidate="DDL_Workorder" InitialValue="Please Select Option"></asp:RequiredFieldValidator>

                                                <div class="mt-1" id="div_wo_badges" runat="server" visible="false" style="margin-top: 5px;">
                                                    <asp:Label ID="lbl_ContractNature" runat="server" CssClass="badge bg-blue"></asp:Label>
                                                    <asp:Label ID="lbl_BillingNature" runat="server" CssClass="badge bg-green"></asp:Label>
                                                </div>
                                            </div>

                                            <div class="col-md-3 col-sm-6 col-xs-12 form-group" id="div_BillingType" runat="server">
                                                <label class="top-label">JOB Type (Billing) <span class="req-star">*</span></label>
                                                <asp:DropDownList ID="DDL_BillingType" runat="server" CssClass="form-control modern-input" AutoPostBack="true" OnSelectedIndexChanged="DDL_BillingType_SelectedIndexChanged"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RFV1" ValidationGroup="Submit" runat="server" CssClass="text-danger small" Display="Dynamic" ErrorMessage="Required" ControlToValidate="DDL_BillingType" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="col-md-6 col-sm-12 col-xs-12 form-group" id="div_NonBillingAlert" runat="server" visible="false">
                                                <div class="modern-alert" style="margin-top: 24px;">
                                                    <i class="fa fa-info-circle"></i>
                                                    <div><strong>Non-Billing Job:</strong> Permit upload will be bypassed.</div>
                                                </div>
                                            </div>
                                            <div class="col-md-3 col-sm-6 col-xs-12 form-group">
                                                <label class="top-label">Attendance Code <span class="req-star">*</span></label>
                                                <asp:DropDownList ID="DDL_AttenCode" runat="server" CssClass="form-control modern-input"></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="row mt-3">
                                            <div class="col-md-3 col-sm-6 col-xs-12 form-group">
                                                <label class="top-label">Work-Site <span class="req-star">*</span></label>
                                                <asp:DropDownList ID="DDL_Worksite" runat="server" CssClass="form-control modern-input" AutoPostBack="true" OnSelectedIndexChanged="DDL_Worksite_SelectedIndexChanged"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Submit" runat="server" CssClass="text-danger small" Display="Dynamic" ErrorMessage="Required" ControlToValidate="DDL_Worksite" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="col-md-3 col-sm-6 col-xs-12 form-group">
                                                <label class="top-label">Approver (Site Incharge) <span class="req-star">*</span></label>
                                                <asp:DropDownList ID="DDL_Approver" runat="server" CssClass="form-control modern-input"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ValidationGroup="Submit" runat="server" CssClass="text-danger small" Display="Dynamic" ErrorMessage="Required" ControlToValidate="DDL_Approver" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="col-md-3 col-sm-6 col-xs-12 form-group">
                                                <label class="top-label">Location <span class="req-star">*</span></label>
                                                <asp:DropDownList ID="DDL_Location" runat="server" CssClass="form-control modern-input"></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="row mt-3">
                                            <div class="col-md-3 col-sm-6 col-xs-12 form-group">
                                                <label class="top-label">Work Permit No <span class="req-star">*</span></label>
                                                <asp:TextBox
                                                    ID="txt_permitno" runat="server"
                                                    MaxLength="100" CssClass="form-control modern-input"
                                                    onkeypress="return validatePermitNo(event);" onkeyup="updatePermitCount();">
                                                </asp:TextBox>
                                                <div style="display: flex; justify-content: space-between; margin-top: 4px;">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="Submit" runat="server" CssClass="text-danger small" Display="Dynamic" ErrorMessage="Required" ControlToValidate="txt_permitno"></asp:RequiredFieldValidator>
                                                    <small id="permitCount" class="text-muted" style="margin-left: auto;">0 / 100</small>
                                                </div>
                                            </div>

                                            <div class="col-md-3 col-sm-6 col-xs-12 form-group">
                                                <label class="top-label">JOB Shift <span class="req-star">*</span></label>
                                                <asp:TextBox ID="txt_jobshift" runat="server" CssClass="form-control modern-input" MaxLength="1" placeholder="A, B, C, G"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="Submit" runat="server" CssClass="text-danger small" Display="Dynamic" ErrorMessage="Required" ControlToValidate="txt_jobshift"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="col-md-6 col-sm-12 col-xs-12 form-group">
                                                <label class="top-label">JOB Title <span class="req-star">*</span></label>
                                                <asp:TextBox
                                                    ID="txt_jobtitle"
                                                    runat="server"
                                                    CssClass="form-control modern-input"
                                                    MaxLength="200"
                                                    onkeypress="return validateJobTitle(event);"
                                                    onkeyup="updateCharCount();"></asp:TextBox>

                                                <div style="display: flex; justify-content: space-between; margin-top: 4px;">
                                                    <div>
                                                        <asp:CustomValidator ID="cv_jobtitle_words" runat="server" ControlToValidate="txt_jobtitle" ValidationGroup="Submit" CssClass="text-danger small" Display="Dynamic" ErrorMessage="Must be > 3 words" ClientValidationFunction="validateWordCount"></asp:CustomValidator>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="Submit" runat="server" CssClass="text-danger small" Display="Dynamic" ErrorMessage="Required" ControlToValidate="txt_jobtitle"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <small id="charCount" class="text-muted">0 / 200</small>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row mt-4" id="div_documents" runat="server" visible="false">
                                            <div class="col-md-12">
                                                <div class="ln_solid" style="margin-top: 5px; margin-bottom: 15px;"></div>
                                                <label class="top-label" style="color: #2980b9; font-size: 14px;"><i class="fa fa-file-text-o" style="margin-right: 6px;"></i>Required Digital Documents</label>
                                                <p class="text-muted small" style="margin-bottom: 10px;">Mandatory documents are locked. Select any additional requirements.</p>

                                                <div class="modern-doc-list">
                                                    <asp:CheckBoxList ID="CBL_Documents" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" CssClass="table table-borderless table-condensed mb-0"></asp:CheckBoxList>
                                                </div>
                                            </div>
                                        </div>

                                        <asp:TextBox ID="txt_workregion" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txt_company" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txt_dept" runat="server" Visible="false"></asp:TextBox>

                                    </div>

                                    <div id="jobid_creation_buttons" runat="server" visible="true">
                                        <div class="ln_solid" style="margin-top: 30px; margin-bottom: 20px;"></div>
                                        <div class="row modern-actions">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="btn_cancel_Click" CausesValidation="false" />
                                                <asp:Button ID="btn_reset" runat="server" Text="Reset Form" CssClass="btn btn-warning" Style="color: #fff;" OnClick="btn_reset_Click" CausesValidation="false" />
                                                <asp:Button ID="btn_submit" runat="server" Text="Create JOB & Continue" ValidationGroup="Submit" CssClass="btn btn-success" OnClick="btn_submit_Click" />
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function showPNotify(title, text, type) {
            new PNotify({
                title: title,
                text: text,
                type: type,
                styling: 'bootstrap3',
                delay: 4000
            });
        }

        function validateWordCount(sender, args) {
            var val = args.Value.trim();
            val = val.replace(/\s+/g, ' ');
            var wordCount = val === "" ? 0 : val.split(' ').length;

            if (wordCount <= 3) {
                args.IsValid = false;
            } else {
                args.IsValid = true;
            }
        }

        function validateJobTitle(e) {
            var key = e.keyCode || e.which;
            if (key === 13)
                return false;
            var char = String.fromCharCode(key);
            var regex = /^[a-zA-Z0-9 .,\-()&\/]*$/;
            if (!regex.test(char))
                return false;
            return true;
        }

        function validatePermitNo(e) {
            var key = e.keyCode || e.which;
            var char = String.fromCharCode(key);
            if (key === 8 || key === 46 || key === 37 || key === 39)
                return true;
            var regex = /^[a-zA-Z0-9,\/]*$/;
            if (!regex.test(char))
                return false;
            return true;
        }

        function updatePermitCount() {
            var textbox = document.querySelector('[id$="txt_permitno"]');
            if (textbox) {
                var count = textbox.value.length;
                var counterEl = document.getElementById("permitCount");
                if (counterEl) counterEl.innerHTML = count + " / 100";
            }
        }

        function updateCharCount() {
            var textbox = document.querySelector('[id$="txt_jobtitle"]');
            if (textbox) {
                var count = textbox.value.length;
                var counterEl = document.getElementById("charCount");
                if (counterEl) counterEl.innerHTML = count + " / 200";
            }
        }

        // ENHANCED HTML5 Geolocation API with Retry Logic
        function requestGPSLocation(isRetry = false) {
            let indicator = document.getElementById('gps_indicator');
            let latField = document.querySelector('.js-lat-field');
            let lonField = document.querySelector('.js-lon-field');
        
            // If user clicked retry, visually reset the indicator to processing state
            if (isRetry && indicator) {
                indicator.innerHTML = "<i class='fa fa-spinner fa-spin' style='margin-right:4px;'></i> Acquiring GPS...";
                indicator.className = "text-warning font-weight-bold";
            }

            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function (position) {
                
                    // Safely assign values only if the fields exist in the DOM
                    if (latField && lonField) {
                        latField.value = position.coords.latitude;
                        lonField.value = position.coords.longitude;
                    }

                    if (indicator) {
                        indicator.innerHTML = "<i class='fa fa-check-circle text-success' style='margin-right:4px;'></i> GPS Captured Successfully";
                        indicator.className = "text-success font-weight-bold";
                    }
                
                    // Show a tiny success toast if they explicitly requested a retry
                    if(isRetry) showPNotify('Success', 'GPS connection re-established.', 'success');

                }, function (error) {
                    showPNotify('GPS Error', 'Please enable location services or check your connection.', 'error');
                    if (indicator) {
                        indicator.innerHTML = "<i class='fa fa-times-circle text-danger' style='margin-right:4px;'></i> Location Denied / Failed";
                        indicator.className = "text-danger font-weight-bold";
                    }
                });
            } else {
                showPNotify('Warning', 'Geolocation is not supported by this browser.', 'notice');
            }
        }
    </script>
</asp:Content>
