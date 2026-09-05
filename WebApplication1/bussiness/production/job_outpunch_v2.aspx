<%@ Page Title="JOB OUT-Punch V2" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="job_outpunch_v2.aspx.cs" Inherits="WebApplication1.bussiness.production.job_outpunch_v2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/clockpicker/0.0.7/bootstrap-clockpicker.min.css">
    <style type="text/css">
        /* Base typography and layout */
        .top-label {
            font-weight: 600;
            margin-bottom: 6px;
            color: #2a3f54;
            font-size: 13px;
            letter-spacing: 0.3px;
            display: inline-block;
        }

        .data-label {
            font-weight: 700;
            color: #2c3e50;
            font-size: 14px;
            display: block;
            margin-top: 2px;
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

            .modern-input[readonly] {
                background-color: #f8f9fa !important;
                cursor: not-allowed;
            }

        /* Job Details Card */
        .modern-info-card {
            background-color: #fcfcfd;
            border: 1px solid #e9ecef;
            padding: 0;
            border-radius: 8px;
            margin-bottom: 20px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.02);
            overflow: hidden;
        }

        /* Action Card (Punch Out Form) */
        .modern-action-card {
            background-color: #fffdf5;
            border: 1px solid #ffeeba;
            border-left: 5px solid #E74C3C; /* Red for OUT */
            padding: 20px;
            border-radius: 6px;
            box-shadow: 0 4px 10px rgba(0,0,0,0.05);
        }

        /* Buttons */
        .btn-modern {
            border-radius: 20px;
            padding: 8px 20px;
            font-weight: 600;
            letter-spacing: 0.5px;
            font-size: 13px;
            transition: all 0.2s ease;
        }

            .btn-modern:hover {
                transform: translateY(-2px);
                box-shadow: 0 4px 8px rgba(0,0,0,0.15);
            }

        /* GridView Container (Cleaned up to let Bootstrap handle the table) */
        .modern-grid-container {
            border: 1px solid #e9ecef;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 2px 4px rgba(0,0,0,0.02);
            background: #fff;
        }

            .modern-grid-container th {
                background-color: #f8f9fa;
                color: #34495e;
                font-weight: 600;
                border-bottom-width: 1px;
            }

        /* Clockpicker fix */
        .clockpicker-popover {
            z-index: 10000 !important;
            border-radius: 8px;
            box-shadow: 0 10px 20px rgba(0,0,0,0.15);
            border: none;
        }

        /* Loader CSS */
        #loadingOverlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(255,255,255,0.85);
            z-index: 9999;
            backdrop-filter: blur(2px);
        }

        .spinner-container {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            text-align: center;
        }

        .loader {
            border: 6px solid #f3f3f3;
            border-top: 6px solid #E74C3C; /* Red to indicate Exit process */
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
            font-size: 16px;
            font-weight: 600;
            color: #2a3f54;
        }

        /* Custom Radio Button styling */
        .custom-radio-list td {
            padding-right: 15px;
        }

        /* Enlarge Touch Targets for Radio Buttons */
        .custom-radio-list label {
            cursor: pointer;
            font-size: 15px;
            font-weight: 600;
            color: #34495e;
            padding: 8px 20px;
            background-color: #f1f5f9;
            border-radius: 6px;
            border: 1px solid #e2e8f0;
            transition: all 0.2s ease;
        }

        .custom-radio-list input[type="radio"] {
            display: none; /* Hide the tiny default circle */
        }

            .custom-radio-list input[type="radio"]:checked + label {
                background-color: #1ABB9C; /* Green highlight when selected */
                color: #ffffff;
                border-color: #1ABB9C;
                box-shadow: 0 3px 8px rgba(26, 187, 156, 0.3);
            }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="https://unpkg.com/leaflet@1.9.4/dist/leaflet.css" />
<script src="https://unpkg.com/leaflet@1.9.4/dist/leaflet.js"></script>
    <div class="right_col" role="main">
        <div class="container">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel modern-panel">
                        <div class="x_title modern-title" style="display: flex; justify-content: space-between; align-items: center;">
                            <h2 style="margin: 0;">Step 4: Manpower OUT-Punch <small style="color: #1ABB9C; font-weight: 600;">Smart Workflow</small></h2>
                            <a href="job_outpunch.aspx" class="modern-header-btn">
                                <i class="fa fa-history" style="margin-right: 5px;"></i>Switch to OLD Version
                            </a>
                            <div class="clearfix"></div>
                        </div>

                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>

                                <div class="x_content bg-light p-3 mb-4" style="border-radius: 8px; border: 1px solid #e9ecef;" id="OUTpunchPanel_Row" runat="server" visible="true">
                                    <div class="row">
                                        <div class="col-md-5 col-sm-12 form-group mb-0">
                                            <label class="top-label">Select Active JOB ID <span class="req-star">*</span></label>
                                            <asp:DropDownList ID="DDL_JOBID" runat="server" CssClass="form-control modern-input" AutoPostBack="true" OnSelectedIndexChanged="DDL_JOBID_SelectedIndexChanged"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="DDL_JOBID" runat="server" ErrorMessage="Required" CssClass="text-danger small" Display="Dynamic" InitialValue="--Select--"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>

                                <div class="x_content" id="JOBIDDetails_Row" runat="server" visible="false">
                                    <div class="row modern-info-card">
                                        <div class="col-md-12" style="background-color: #f8f9fa; border-bottom: 1px solid #e9ecef; padding: 15px 20px;">
                                            <h5 class="m-0" style="color: #2980b9; font-weight: 600;"><i class="fa fa-briefcase" style="margin-right: 8px;"></i>Active JOB Overview</h5>
                                        </div>

                                        <div class="col-md-12 p-4">
                                            <div class="row">
                                                <div class="col-md-3 col-sm-6 mb-3">
                                                    <small class="text-muted text-uppercase font-weight-bold"><i class="fa fa-hashtag"></i>JOB ID</small><br />
                                                    <asp:Label ID="lbl_jobid" runat="server" CssClass="data-label" Style="font-size: 16px;"></asp:Label>
                                                </div>
                                                <div class="col-md-3 col-sm-6 mb-3">
                                                    <small class="text-muted text-uppercase font-weight-bold"><i class="fa fa-calendar"></i>Date</small><br />
                                                    <asp:Label ID="lbl_jobiddate" runat="server" CssClass="data-label" Style="font-size: 15px;"></asp:Label>
                                                </div>
                                                <div class="col-md-3 col-sm-6 mb-3">
                                                    <small class="text-muted text-uppercase font-weight-bold"><i class="fa fa-file-text-o"></i>Work Order</small><br />
                                                    <asp:Label ID="lbl_wrkordr" runat="server" CssClass="data-label" Style="font-size: 15px;"></asp:Label>
                                                </div>
                                                <div class="col-md-3 col-sm-6 mb-3">
                                                    <small class="text-muted text-uppercase font-weight-bold"><i class="fa fa-shield"></i>Permit No</small><br />
                                                    <asp:Label ID="lbl_permitno" runat="server" CssClass="badge bg-red" Style="font-size: 13px; margin-top: 4px; padding: 5px 8px;"></asp:Label>
                                                </div>

                                                <div class="col-md-3 col-sm-6 mb-2">
                                                    <small class="text-muted text-uppercase font-weight-bold"><i class="fa fa-building-o"></i>Site / Location</small><br />
                                                    <span class="data-label" style="display: inline-block;">
                                                        <asp:Label ID="lbl_jobsite" runat="server"></asp:Label>
                                                        <span class="text-muted font-weight-normal mx-1">|</span>
                                                        <asp:Label ID="lbl_jobloc" runat="server" CssClass="font-weight-normal"></asp:Label>
                                                    </span>
                                                </div>
                                                <div class="col-md-3 col-sm-6 mb-2">
                                                    <small class="text-muted text-uppercase font-weight-bold"><i class="fa fa-clock-o"></i>Shift</small><br />
                                                    <asp:Label ID="lbl_jobshift" runat="server" CssClass="badge bg-green" Style="font-size: 13px; margin-top: 4px; padding: 5px 8px;"></asp:Label>
                                                </div>
                                                <div class="col-md-6 col-sm-12 mb-2">
                                                    <small class="text-muted text-uppercase font-weight-bold"><i class="fa fa-user-circle"></i>Site In-Charge</small><br />
                                                    <asp:Label ID="lbl_inchargename" runat="server" CssClass="data-label"></asp:Label>
                                                </div>

                                                <div class="col-md-12 mt-3" id="div_saved_map" runat="server" style="display: none;">
                                                    <small class="text-muted text-uppercase font-weight-bold"><i class="fa fa-map-marker" style="color: #e74c3c;"></i>GPS Location (Created At)</small>
                                                    <div id="savedMapPreview" style="height: 150px; width: 100%; border-radius: 8px; margin-top: 5px; border: 1px solid #ced4da; box-shadow: inset 0 1px 3px rgba(0,0,0,0.1);"></div>
                                                </div>

                                                <asp:HiddenField ID="hf_db_lat" runat="server" />
                                                <asp:HiddenField ID="hf_db_lon" runat="server" />
                                            </div>
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
                                            <div class="alert alert-info" style="border-radius: 8px; padding: 12px 20px; display: flex; align-items: center;">
                                                <i class="fa fa-shield" style="font-size: 20px; margin-right: 10px;"></i>
                                                <div style="flex-grow: 1;">
                                                    <strong>Safety Compliance:</strong>
                                                    <span class="ml-2">TBT Count:
                                                        <asp:Label ID="lbl_tbtcount" runat="server" CssClass="badge bg-green mx-1" Text="0"></asp:Label></span>
                                                    <span class="text-muted mx-2">|</span>
                                                    <span>SOP Count:
                                                        <asp:Label ID="lbl_sopcount" runat="server" CssClass="badge bg-green mx-1" Text="0"></asp:Label></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" id="IncompleteCSM" runat="server" visible="false">
                                        <div class="col-md-12 text-center p-5" style="background: #fff5f5; border: 1px dashed #e74c3c; border-radius: 8px; margin-bottom: 20px;">
                                            <i class="fa fa-times-circle text-danger" style="font-size: 50px;"></i>
                                            <h4 class="text-danger mt-3" style="font-weight: 600;">Incomplete Safety Documents!</h4>
                                            <p class="text-muted" style="font-size: 15px;">You must complete Toolbox Talk (TBT) and SOP Training before punching out workers.</p>
                                            <div class="mt-3">
                                                <a href="csm_tbtform.aspx" class="btn btn-primary btn-modern"><i class="fa fa-pencil-square-o"></i>Complete TBT</a>
                                                <a href="csm_soptraining.aspx" class="btn btn-info btn-modern"><i class="fa fa-graduation-cap"></i>Complete SOP</a>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row mt-3 mb-4" id="PunchOutForm_Row" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="modern-action-card">
                                                <div class="row border-bottom pb-3 mb-4 align-items-center">
                                                    <div class="col-md-12">
                                                        <h4 class="text-danger m-0" style="font-weight: 700;">
                                                            <i class="fa fa-sign-out" style="margin-right: 8px;"></i>Process Exit For: 
                                                            <asp:Label ID="txt_empname" runat="server" CssClass="text-dark ml-2"></asp:Label>
                                                        </h4>
                                                    </div>
                                                </div>

                                                <div class="row align-items-end">
                                                    <div class="col-lg-2 col-md-4 col-12 form-group mb-3">
                                                        <label class="text-muted font-weight-bold mb-1" style="font-size: 12px; text-transform: uppercase;">IN Time</label>
                                                        <asp:TextBox ID="txt_inpunchtime" runat="server" CssClass="form-control modern-input" ReadOnly="true"></asp:TextBox>
                                                    </div>

                                                    <div class="col-lg-2 col-md-4 col-12 form-group mb-3">
                                                        <label class="top-label">OUT Date <span class="req-star">*</span></label>
                                                        <asp:TextBox ID="txt_date" runat="server" CssClass="form-control modern-input" type="date" onkeydown="return false;"></asp:TextBox>
                                                    </div>

                                                    <div class="col-lg-2 col-md-4 col-12 form-group mb-3">
                                                        <label class="top-label">OUT Time <span class="req-star">*</span></label>
                                                        <asp:TextBox ID="txt_time" runat="server" CssClass="form-control modern-input time-picker-custom" AutoCompleteType="Disabled" placeholder="HH:MM"></asp:TextBox>
                                                    </div>

                                                    <div class="col-lg-2 col-md-4 col-12 form-group mb-3">
                                                        <label class="top-label d-block text-center">Lunch <span class="req-star">*</span></label>
                                                        <div class="form-control modern-input border-0 bg-transparent p-0 text-center" style="box-shadow: none;">
                                                            <asp:RadioButtonList ID="RBTN_LunchFactor" runat="server" RepeatDirection="Horizontal" CssClass="custom-radio-list mx-auto" Style="display: inline-block;">
                                                                <asp:ListItem Selected="True" Value="Yes">Yes</asp:ListItem>
                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-2 col-md-4 col-12 form-group mb-3">
                                                        <label class="top-label">Attn Code <span class="req-star">*</span></label>
                                                        <asp:DropDownList ID="DDL_AttenCode" runat="server" CssClass="form-control modern-input"></asp:DropDownList>
                                                    </div>

                                                    <div class="col-lg-2 col-md-4 col-12 form-group mb-3">
                                                        <label class="top-label text-center d-block">Over Time (Hrs) <span class="req-star">*</span></label>
                                                        <asp:TextBox ID="txt_ot" runat="server" CssClass="form-control modern-input text-center" Text="0" type="number" max="16" min="0" Style="font-weight: bold; color: #E74C3C;"></asp:TextBox>
                                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="PunchOUT_Button" ControlToValidate="txt_ot" CssClass="text-danger small d-block text-center mt-1" Display="Dynamic" ClientValidationFunction="validateInput"></asp:CustomValidator>
                                                    </div>
                                                </div>

                                                <div class="row mt-3">
                                                    <div class="col-md-12 border-top pt-4 d-flex flex-column flex-sm-row justify-content-sm-end">
                                                        <asp:Button ID="btn_cancel" runat="server" Text="Cancel"
                                                            CssClass="btn btn-outline-secondary btn-modern mb-3 mb-sm-0 mr-sm-2"
                                                            Style="padding: 10px 25px; font-size: 15px;"
                                                            CausesValidation="false" OnClick="btn_cancel_Click" />

                                                        <asp:Button ID="btn_punchout" runat="server" Text="Confirm Punch OUT"
                                                            ValidationGroup="PunchOUT_Button"
                                                            CssClass="btn btn-danger btn-modern"
                                                            Style="padding: 10px 25px; font-size: 15px; box-shadow: 0 4px 10px rgba(231, 76, 60, 0.3);"
                                                            OnClientClick="showLoader();" OnClick="btn_punchout_Click" />
                                                    </div>
                                                </div>

                                                <asp:Label ID="lbl_Id" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lbl_empworkman" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lbl_workhours" runat="server" Visible="false"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row mt-4" id="ViewState_TableRow" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <h5 style="color: #1ABB9C; font-weight: 600; margin-bottom: 15px;"><i class="fa fa-users" style="margin-right: 6px;"></i>Workers Currently IN</h5>
                                            <div class="modern-grid-container card-box table-responsive">
                                                <asp:GridView ID="GridView1" runat="server" Width="100%" CssClass="table table-striped table-hover table-sm" AutoGenerateColumns="false" EmptyDataText="All workers have been punched out." OnRowDeleting="GridView1_RowDeleting" GridLines="None">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Worker Info" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="35%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Id" runat="server" Text='<%# Bind("Id") %>' Visible="false"></asp:Label>
                                                                <div style="font-weight: 700; color: #2c3e50; font-size: 14px; margin-bottom: 3px;">
                                                                    <%# Container.DataItemIndex + 1 %>.
                                                                    <asp:Label ID="lbl_EmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                                                                </div>
                                                                <div style="font-size: 12px; color: #64748b;">
                                                                    <span class="badge" style="background-color: #f1f5f9; color: #475569; border: 1px solid #e2e8f0; padding: 3px 6px;">
                                                                        <%# Eval("EmployeeWrk") %>
                                                                    </span>
                                                                    <span style="margin: 0 4px; color: #cbd5e1;">|</span>
                                                                    <span style="font-weight: 500;">
                                                                        <%# Eval("EmpDesignation") != DBNull.Value ? Eval("EmpDesignation") : "Worker" %>
                                                                    </span>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Time Log" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="35%">
                                                            <ItemTemplate>
                                                                <div style="margin-bottom: 5px;">
                                                                    <span class="badge bg-green text-white" style="font-size: 10px; width: 40px; padding: 4px 0; display: inline-block; text-align: center;">IN</span>
                                                                    <span style="font-size: 13px; font-weight: 600; color: #34495e; margin-left: 5px;">
                                                                        <%# Eval("Inpunch_Time", "{0:dd-MMM HH:mm}") %>
                                                                    </span>
                                                                </div>
                                                                <div>
                                                                    <span class='<%# Eval("AttendanceStatus").ToString() == "Exit" ? "badge bg-red text-white" : "badge bg-secondary text-white" %>' style="font-size: 10px; width: 40px; padding: 4px 0; display: inline-block; text-align: center;">OUT</span>
                                                                    <span style="font-size: 13px; font-weight: 600; margin-left: 5px; color: <%# Eval("AttendanceStatus").ToString() == "Exit" ? "#E74C3C" : "#94a3b8" %>;">
                                                                        <%# Eval("AttendanceStatus").ToString() == "Exit" ? Eval("Outpunch_Time", "{0:dd-MMM HH:mm}") : "Pending..." %>
                                                                    </span>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Hrs/OT" ItemStyle-CssClass="d-none d-md-table-cell hidden-xs hidden-sm" HeaderStyle-CssClass="d-none d-md-table-cell hidden-xs hidden-sm" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="15%">
                                                            <ItemTemplate>
                                                                <div style="font-size: 13px; font-weight: bold; color: #34495e; margin-bottom: 3px;">
                                                                    Hrs: <%# Eval("AttendanceStatus").ToString() == "Exit" ? Eval("WorkedHours") + "h" : "-" %>
                                                                </div>
                                                                <div style="font-size: 13px; font-weight: bold; color: #c0392b;">
                                                                    OT: <%# Eval("AttendanceStatus").ToString() == "Exit" ? Eval("ProvidedOT") + "h" : "-" %>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Action" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="15%">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="PunchOUT" runat="server"
                                                                    CssClass='<%# Eval("AttendanceStatus").ToString() == "Exit" ? "btn btn-sm btn-default btn-block m-0" : "btn btn-sm btn-danger btn-block m-0" %>'
                                                                    Style="font-weight: 600; padding: 5px 0; margin-bottom: 5px !important;"
                                                                    CommandArgument='<%# Eval("Id") %>'
                                                                    OnClick="PunchOUT_Click">
                                                                    <i class='<%# Eval("AttendanceStatus").ToString() == "Exit" ? "fa fa-pencil" : "fa fa-sign-out" %>'></i> 
                                                                    <%# Eval("AttendanceStatus").ToString() == "Exit" ? "Edit" : "OUT" %>
                                                                </asp:LinkButton>

                                                                <asp:LinkButton ID="btndelete" runat="server" CommandName="Delete"
                                                                    CssClass="btn btn-sm btn-default btn-block m-0"
                                                                    Style="padding: 5px 0;"
                                                                    OnClientClick="return confirm('Delete this record?');" ToolTip="Delete Record">
                                                                    <i class="fa fa-trash text-danger"></i>
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" id="NoPenidngPunch" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="text-center p-4 shadow" style="background-color: #f0f9f6; border: 2px solid #1ABB9C; border-radius: 8px; margin-top: 20px; position: sticky; bottom: 10px; z-index: 100;">
                                                <div class="d-flex align-items-center justify-content-center flex-wrap">
                                                    <i class="fa fa-check-circle text-success mr-3" style="font-size: 30px;"></i>
                                                    <h4 class="text-success m-0 mr-3" style="font-weight: 700;">All Punches Completed</h4>
                                                    <button type="button" class="btn btn-success m-0 mt-2 mt-sm-0"
                                                        style="border-radius: 30px; padding: 10px 30px; font-weight: bold; font-size: 15px; box-shadow: 0 4px 10px rgba(26, 187, 156, 0.3);"
                                                        onclick="showCloseConfirmModal(); return false;">
                                                        Close &amp; Send
                                                    </button>
                                                </div>
                                            </div>
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

    <div class="modal fade" id="confirmCloseModal" tabindex="-1" role="dialog" aria-labelledby="confirmCloseTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #1ABB9C; color: #fff;">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="color: #fff; opacity: 1;">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="confirmCloseTitle"><i class="fa fa-send"></i> Close shift and send for approval?</h4>
                </div>
                <div class="modal-body">
                    <p class="mb-2">All workers are OUT-punched. Closing this shift writes the same legacy status as automatic close:</p>
                    <ul>
                        <li><code>JOB_Status = Out-Punch Done</code></li>
                        <li><code>MasterStatusCode = 4</code></li>
                        <li><code>EntryExit = Exit</code></li>
                    </ul>
                    <p class="text-muted small mb-0">The job will appear in the Site In-Charge approval queue and drop off the pending-OUT dashboard.</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" id="btn_ReviewAgain" data-dismiss="modal">Review Again</button>
                    <asp:Button ID="btn_FinalizeShift" runat="server"
                        Text="Close &amp; Send"
                        CssClass="btn btn-success"
                        CausesValidation="false"
                        UseSubmitBehavior="true"
                        OnClientClick="return lockCloseAndSend(this);"
                        OnClick="btn_FinalizeShift_Click" />
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="successCloseModal" tabindex="-1" role="dialog" aria-labelledby="successCloseTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #26B99A; color: #fff;">
                    <h4 class="modal-title" id="successCloseTitle"><i class="fa fa-check-circle"></i> Shift submitted for approval</h4>
                </div>
                <div class="modal-body text-center">
                    <p>JOB <strong id="closeSuccessJobId"></strong> is closed and sent to the Site In-Charge.</p>
                    <p class="text-muted small">Status is Out-Punch Done / MasterStatusCode 4 / EntryExit Exit. No pending-OUT orphan remains.</p>
                </div>
                <div class="modal-footer" style="text-align: center;">
                    <a id="closeSuccess360" class="btn btn-success" href="#"><i class="fa fa-eye"></i> Open JOB 360</a>
                    <a id="closeSuccessDash" class="btn btn-info" href="jobs_and_manpower_v2.aspx"><i class="fa fa-th-large"></i> JOB Dashboard</a>
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

        function showLoader(message) {
            var overlay = document.getElementById("loadingOverlay");
            if (overlay) overlay.style.display = "block";
            var textEl = overlay ? overlay.querySelector(".loading-text") : null;
            if (textEl) textEl.textContent = message || "Processing Exit...";
        }
        function hideLoader() { document.getElementById("loadingOverlay").style.display = "none"; }

        function lockCloseAndSend(btn) {
            // UAT-040A: first click posts; later clicks are no-ops until the page reloads.
            if (window.__closeAndSendLocked) return false;
            window.__closeAndSendLocked = true;
            showLoader("Closing shift...");
            btn.value = "Closing...";
            var reviewBtn = document.getElementById("btn_ReviewAgain");
            if (reviewBtn) reviewBtn.disabled = true;
            var headerClose = document.querySelector("#confirmCloseModal .close");
            if (headerClose) headerClose.disabled = true;
            setTimeout(function () { btn.disabled = true; }, 0);
            return true;
        }

        function showCloseConfirmModal() {
            hideLoader();
            if (window.jQuery) {
                $('#confirmCloseModal').modal({ backdrop: 'static', keyboard: false });
            }
        }

        function showCloseSuccessModal(jobid) {
            hideLoader();
            if (!window.jQuery) return;
            $('#confirmCloseModal').modal('hide');
            var safeId = jobid || '';
            var jobEl = document.getElementById('closeSuccessJobId');
            if (jobEl) jobEl.textContent = safeId;
            var viewEl = document.getElementById('closeSuccess360');
            if (viewEl) viewEl.setAttribute('href', 'job_360_view.aspx?jobid=' + encodeURIComponent(safeId));
            $('#successCloseModal').modal({ backdrop: 'static', keyboard: false });
        }

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

        // 1. Initialize the interactive clock
        function InitClockPicker() {
            if ($.fn.clockpicker) {
                $('.time-picker-custom').clockpicker({
                    placement: 'bottom',
                    align: 'left',
                    autoclose: true,
                    'default': 'now',
                    donetext: 'Done',
                    afterDone: function () {
                        $('.time-picker-custom').trigger('change');
                    }
                });
            }
        }

        // 2. Main Validation Logic
        function validateOutPunch() {
            const outPunchDate = document.getElementById('<%= txt_date.ClientID %>');
            const outPunchTime = document.getElementById('<%= txt_time.ClientID %>');

            if (!outPunchDate || !outPunchTime) return;

            let selectedDate = outPunchDate.value;
            let selectedTime = outPunchTime.value;

            if (!selectedDate || !selectedTime) return;

            let now = new Date();
            let year = now.getFullYear();
            let month = String(now.getMonth() + 1).padStart(2, '0');
            let day = String(now.getDate()).padStart(2, '0');

            let currentDate = `${year}-${month}-${day}`;
            let currentHours = now.getHours();
            let currentMinutes = now.getMinutes();

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

        // 3. Attach listeners cleanly
        function AttachDateListeners() {
            InitClockPicker();
            $('#<%= txt_date.ClientID %>').off('change').on('change', validateOutPunch);
            $('#<%= txt_time.ClientID %>').off('change').on('change', validateOutPunch);
        }

        // 4. THE FIX: Dynamic Dependency Loader
        function BootUpSystem() {
            // Check if the Master Page has finished loading jQuery
            if (window.jQuery) {
                // Check if clockpicker is already loaded to prevent duplicate network calls
                if (!$.fn.clockpicker) {
                    // Dynamically load the library only AFTER jQuery is ready
                    $.getScript("https://cdnjs.cloudflare.com/ajax/libs/clockpicker/0.0.7/bootstrap-clockpicker.min.js")
                        .done(function () {
                            AttachDateListeners();
                        })
                        .fail(function () {
                            console.error("Failed to load ClockPicker script from CDN.");
                        });
                } else {
                    AttachDateListeners();
                }
            } else {
                // If jQuery isn't ready, wait 50ms and try again
                setTimeout(BootUpSystem, 50);
            }
        }

        // 5. UpdatePanel and Page Load Handlers
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            hideLoader();
            AttachDateListeners(); // Dependencies are already loaded on partial postbacks
        });

        // Start the boot sequence
        document.addEventListener("DOMContentLoaded", BootUpSystem);

        let savedMap = null;

        function renderSavedMap() {
            // 1. Grab coordinates from the ASP.NET HiddenFields
            let latField = document.querySelector('input[id$="hf_db_lat"]');
            let lonField = document.querySelector('input[id$="hf_db_lon"]');

            if (latField && lonField && latField.value && lonField.value) {
                let lat = parseFloat(latField.value);
                let lon = parseFloat(lonField.value);

                let mapDiv = document.getElementById('savedMapPreview');

                if (mapDiv) {
                    // Leaflet Bug Fix: Sometimes map tiles blur if div changes size. 
                    // Invalidate sizes forces a fresh render.
                    if (savedMap !== null) {
                        savedMap.setView([lat, lon], 16);
                        savedMap.invalidateSize();
                    } else {
                        // Initialize Map
                        savedMap = L.map('savedMapPreview').setView([lat, lon], 16);

                        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                            attribution: '© OpenStreetMap'
                        }).addTo(savedMap);

                        // Add the Red Pin Marker
                        L.marker([lat, lon]).addTo(savedMap)
                         .bindPopup("<b>JOB Creation Location</b><br>Lat: " + lat + "<br>Lon: " + lon)
                         .openPopup();
                    }
                }
            }
        }
    </script>
</asp:Content>
