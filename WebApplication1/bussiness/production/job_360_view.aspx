<%@ Page Title="360-Degree JOB View" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="job_360_view.aspx.cs" Inherits="WebApplication1.bussiness.production.job_360_view" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /* Custom Stepper adapted for your Admin Theme */
        .stepper-wrapper {
            display: flex;
            justify-content: space-between;
            margin-bottom: 20px;
            margin-top: 10px;
            align-items: flex-start;
        }

        .stepper-item {
            position: relative;
            display: flex;
            flex-direction: column;
            align-items: center;
            flex: 1;
            text-align: center;
        }

            .stepper-item::before {
                position: absolute;
                content: "";
                border-bottom: 3px solid #e1e5eb;
                width: 100%;
                top: 20px;
                left: -50%;
                z-index: 2;
            }

            .stepper-item::after {
                position: absolute;
                content: "";
                border-bottom: 3px solid #e1e5eb;
                width: 100%;
                top: 20px;
                left: 50%;
                z-index: 2;
            }

            .stepper-item .step-counter {
                position: relative;
                z-index: 5;
                display: flex;
                justify-content: center;
                align-items: center;
                width: 40px;
                height: 40px;
                border-radius: 50%;
                background: #e1e5eb;
                color: #73879C;
                font-weight: bold;
                margin-bottom: 8px;
                border: 2px solid white;
                box-shadow: 0 0 5px rgba(0,0,0,0.1);
            }

            /* Status Colors */
            .stepper-item.completed .step-counter {
                background-color: #1ABB9C;
                color: white;
                border-color: #1ABB9C;
            }

            .stepper-item.completed::before, .stepper-item.completed::after {
                border-color: #1ABB9C;
            }

            .stepper-item.active .step-counter {
                background-color: #3498DB;
                color: white;
                border-color: #3498DB;
                box-shadow: 0 0 10px rgba(52,152,219,0.4);
            }

            .stepper-item.failed .step-counter {
                background-color: #E74C3C;
                color: white;
            }

            .stepper-item.skipped .step-counter {
                background-color: #f8f9fa;
                color: #adb5bd;
                border: 2px dashed #ced4da;
                box-shadow: none;
            }

            .stepper-item:first-child::before {
                content: none;
            }

            .stepper-item:last-child::after {
                content: none;
            }

        .step-name {
            font-size: 13px;
            font-weight: 600;
            color: #73879C;
            text-align: center;
        }

        .stepper-item.active .step-name {
            color: #3498DB;
        }

        .stepper-item.skipped .step-name {
            color: #adb5bd;
            text-decoration: line-through;
        }

        /* Details & TAT Panel styling */
        .step-details {
            font-size: 11.5px;
            color: #555;
            margin-top: 5px;
            line-height: 1.4;
            background: #f9f9f9;
            padding: 5px 10px;
            border-radius: 5px;
            border: 1px solid #eee;
            display: inline-block;
            min-width: 140px;
            box-shadow: 0 1px 3px rgba(0,0,0,0.05);
        }

        .stepper-item.skipped .step-details {
            background: transparent;
            border: none;
            font-style: italic;
            box-shadow: none;
        }

        .tat-badge {
            background-color: #e8f4f8;
            color: #2980b9;
            padding: 2px 6px;
            border-radius: 12px;
            font-size: 10px;
            font-weight: 700;
            display: inline-block;
            margin-top: 4px;
            border: 1px dashed #b3d4e6;
        }

        .step-details strong {
            color: #333;
        }

        /* General Table & Panel Styling */
        .detail-table th {
            background-color: #f7f7f7;
            width: 35%;
            color: #555;
        }

        .modern-panel {
            border-radius: 8px;
            border: none;
            box-shadow: 0 2px 10px rgba(0,0,0,0.05);
        }

        .border-top-info {
            border-top: 4px solid #17a2b8 !important;
        }

        .cockpit-tabs > li > a {
            font-weight: 600;
            color: #2a3f54;
        }

        .cockpit-tab-content {
            background: #fff;
            border: 1px solid #ddd;
            border-top: none;
            padding: 15px 10px 5px 10px;
        }

        /* Site Docs (CSM) remains a server stepper node for EvaluateSmartLifecycle; not shown on the frozen Overview labels. */
        .step-csm-hidden {
            display: none !important;
        }

        /* Close & Send is a visual label only (no runat=server). Completion follows existing step5 until a later CR splits Close from MAX(LastModified). */
        #step5.completed + .step-close-send .step-counter {
            background-color: #1ABB9C;
            color: white;
            border-color: #1ABB9C;
        }

        #step5.completed + .step-close-send::before,
        #step5.completed + .step-close-send::after {
            border-color: #1ABB9C;
        }

        .admin-console-card .x_panel {
            min-height: 150px;
        }

        .audit-timeline {
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
            gap: 8px;
            padding: 12px 8px 4px 8px;
        }

        .audit-node {
            flex: 1;
            text-align: center;
            position: relative;
        }

        .audit-node .audit-dot {
            width: 12px;
            height: 12px;
            border-radius: 50%;
            margin: 0 auto 8px auto;
            background: #ccc;
            border: 2px solid #ccc;
            position: relative;
            z-index: 1;
        }

        .audit-node:not(:last-child)::after {
            content: "";
            position: absolute;
            top: 5px;
            left: 50%;
            width: 100%;
            height: 2px;
            background: #e6e6e6;
            z-index: 0;
        }

        .audit-node.completed:not(:last-child)::after {
            background: #1ABB9C;
        }

        .audit-node.completed .audit-dot {
            background: #1ABB9C;
            border-color: #1ABB9C;
        }

        .audit-node .audit-name {
            font-weight: 700;
            font-size: 12px;
            color: #2a3f54;
        }

        .audit-node .audit-when {
            font-size: 11px;
            color: #73879C;
        }

        @media (max-width: 768px) {
            .stepper-wrapper {
                flex-direction: column;
                align-items: stretch;
            }

            .stepper-item::before,
            .stepper-item::after {
                display: none;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="page-title">
            <div class="title_left">
                <h3><i class="fa fa-globe"></i>JOB 360-Degree Lifecycle View</h3>
            </div>
        </div>
        <div class="clearfix"></div>

        <div class="row">
            <div class="col-md-12 col-sm-12">
                <div class="x_panel modern-panel">
                    <div class="x_content novalidate">
                        <div class="field item form-group mt-2">
                            <label class="col-form-label col-md-3 col-sm-3 label-align text-primary font-weight-bold">
                                Search JOBID<span class="required">*</span>
                            </label>
                            <div class="col-md-5 col-sm-5">
                                <asp:TextBox ID="txt_jobid" class="form-control form-control-sm rounded text-uppercase" runat="server" placeholder="e.g. JOB260424322"></asp:TextBox>
                            </div>
                            <div class="col-md-4 col-sm-4">
                                <asp:Button ID="btn_search" runat="server" Text="Track Job" CssClass="btn btn-success btn-sm" OnClick="btn_search_Click" />
                                <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="btn btn-secondary btn-sm" OnClick="btn_reset_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="MainDashboardRow" runat="server" visible="false">

            <ul class="nav nav-tabs cockpit-tabs" role="tablist">
                <li role="presentation" class="active"><a href="#cockpit_overview" class="cockpit-tab" aria-controls="cockpit_overview" role="tab" data-toggle="tab">Overview</a></li>
                <li role="presentation"><a href="#cockpit_details" class="cockpit-tab" aria-controls="cockpit_details" role="tab" data-toggle="tab">Details</a></li>
                <li role="presentation"><a href="#cockpit_permits" class="cockpit-tab" aria-controls="cockpit_permits" role="tab" data-toggle="tab">Permits</a></li>
                <li role="presentation"><a href="#cockpit_manpower" class="cockpit-tab" aria-controls="cockpit_manpower" role="tab" data-toggle="tab">Manpower</a></li>
                <li role="presentation"><a href="#cockpit_csm" class="cockpit-tab" aria-controls="cockpit_csm" role="tab" data-toggle="tab">CSM</a></li>
                <li role="presentation"><a href="#cockpit_admin" class="cockpit-tab" aria-controls="cockpit_admin" role="tab" data-toggle="tab">Admin</a></li>
            </ul>
            <div class="tab-content cockpit-tab-content">

            <div role="tabpanel" class="tab-pane active" id="cockpit_overview">
            <div class="row" id="ActionBarRow" runat="server" visible="false">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel modern-panel" style="background-color: #f8f9fa; border-left: 5px solid #2a3f54;">
                        <div class="x_content mb-0 pb-0">
                            <div class="d-flex flex-wrap align-items-center">
                                <h5 class="mr-4 mb-2 font-weight-bold text-dark"><i class="fa fa-cogs"></i>Available Actions:</h5>

                                <%-- Standard Pipeline Actions --%>
                                <asp:LinkButton ID="btn_Act_UploadPermit" runat="server" CssClass="btn btn-primary btn-sm mb-2 mr-2" OnClick="btn_Act_UploadPermit_Click" Visible="false" CausesValidation="false"
                                    ToolTip="Proceed to upload mandatory safety permits for this shift."><i class="fa fa-paperclip"></i> Upload Permit</asp:LinkButton>

                                <asp:LinkButton ID="btn_Act_InPunch" runat="server" CssClass="btn btn-info btn-sm mb-2 mr-2" OnClick="btn_Act_InPunch_Click" Visible="false" CausesValidation="false"
                                    ToolTip="Open the scanner to IN-Punch manpower for this shift."><i class="fa fa-sign-in"></i> IN-Punch Manpower</asp:LinkButton>

                                <asp:LinkButton ID="btn_Act_AddDocs" runat="server" CssClass="btn btn-info btn-sm mb-2 mr-2" OnClick="btn_Act_AddDocs_Click" Visible="false" CausesValidation="false"
                                    ToolTip="Upload daily site compliance documents (TBT/SOP)."><i class="fa fa-file-text"></i> Add Site Docs</asp:LinkButton>

                                <asp:LinkButton ID="btn_Act_OutPunch" runat="server" CssClass="btn btn-warning btn-sm mb-2 mr-2 text-dark" OnClick="btn_Act_OutPunch_Click" Visible="false" CausesValidation="false"
                                    ToolTip="Proceed to OUT-Punch workers and close this shift."><i class="fa fa-sign-out"></i> OUT-Punch Shift</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel modern-panel">
                        <div class="x_title">
                            <h2><i class="fa fa-line-chart"></i>Smart Pipeline & Progress Status</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content mt-3">
                            <div class="stepper-wrapper" id="stepperContainer" runat="server">
                                <div class="stepper-item" id="step1" runat="server">
                                    <div class="step-counter"><i class="fa fa-file-text-o"></i></div>
                                    <div class="step-name">Create</div>
                                    <asp:Literal ID="lit_step1_details" runat="server"></asp:Literal>
                                </div>
                                <div class="stepper-item" id="step3" runat="server">
                                    <div class="step-counter"><i class="fa fa-sign-in"></i></div>
                                    <div class="step-name">IN-Punch</div>
                                    <asp:Literal ID="lit_step3_details" runat="server"></asp:Literal>
                                </div>
                                <div class="stepper-item" id="step2" runat="server">
                                    <div class="step-counter"><i class="fa fa-paperclip"></i></div>
                                    <div class="step-name">Permit Upload</div>
                                    <asp:Literal ID="lit_step2_details" runat="server"></asp:Literal>
                                </div>
                                <div class="stepper-item" id="step5" runat="server">
                                    <div class="step-counter"><i class="fa fa-sign-out"></i></div>
                                    <div class="step-name">OUT-Punch</div>
                                    <asp:Literal ID="lit_step5_details" runat="server"></asp:Literal>
                                </div>
                                <div class="stepper-item step-close-send">
                                    <div class="step-counter"><i class="fa fa-flag-checkered"></i></div>
                                    <div class="step-name">Close &amp; Send</div>
                                </div>
                                <div class="stepper-item" id="step6" runat="server">
                                    <div class="step-counter"><i class="fa fa-check-square-o"></i></div>
                                    <div class="step-name">Approval</div>
                                    <asp:Literal ID="lit_step6_details" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <div class="step-csm-hidden" aria-hidden="true">
                                <div class="stepper-item" id="step4" runat="server">
                                    <div class="step-counter"><i class="fa fa-folder-open-o"></i></div>
                                    <div class="step-name">Site Docs (CSM)</div>
                                    <asp:Literal ID="lit_step4_details" runat="server"></asp:Literal>
                                </div>
                            </div>

                            <hr />

                            <div class="alert alert-warning text-dark mb-0" id="divBottleneck" runat="server" style="font-size: 14px;">
                                <i class="fa fa-exclamation-triangle fa-lg"></i><strong>Current Status: </strong>
                                <asp:Label ID="lbl_bottleneck" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            </div>

            <div role="tabpanel" class="tab-pane" id="cockpit_details">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel modern-panel">
                        <div class="x_title">
                            <h2><i class="fa fa-info-circle"></i>Core Details</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li>
                                    <asp:LinkButton ID="btn_EditCoreDetails" runat="server" CssClass="btn btn-sm btn-outline-primary m-0" OnClick="btn_EditCoreDetails_Click" Visible="false"><i class="fa fa-edit"></i> Edit</asp:LinkButton>
                                </li>
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <table class="table table-bordered table-sm detail-table">
                                <tr>
                                    <th>JOB Date</th>
                                    <td>
                                        <asp:Label ID="lbl_jobdate" runat="server" CssClass="font-weight-bold text-primary"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th>Created By</th>
                                    <td>
                                        <asp:Label ID="lbl_creator" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th>Work Site</th>
                                    <td>
                                        <asp:Label ID="lbl_worksite" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th>Job Title</th>
                                    <td>
                                        <asp:Label ID="lbl_title" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th>Shift</th>
                                    <td>
                                        <asp:Label ID="lbl_shift" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th>Work Order No</th>
                                    <td>
                                        <asp:Label ID="lbl_wo" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th>Current JOB Status</th>
                                    <td>
                                        <asp:Label ID="lbl_jobstatus" runat="server" Font-Bold="true"></asp:Label></td>
                                </tr>
                            </table>

                            <!-- Extended Admin/Financial Details (From DB Dump) -->
                            <h6 class="text-info font-weight-bold"><i class="fa fa-lock"></i>Admin & Financial Ledger</h6>
                            <table class="table table-bordered table-sm detail-table" style="background-color: #fdfdfe;">
                                <tr>
                                    <th>Billing Status</th>
                                    <td>
                                        <asp:Label ID="lbl_billingstatus" runat="server" CssClass="badge bg-secondary"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th>Level 1 Billing Code</th>
                                    <td>
                                        <asp:Label ID="lbl_l1billing" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th>EMC Number</th>
                                    <td>
                                        <asp:Label ID="lbl_emc" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th>Lumpsum Amount</th>
                                    <td>
                                        <asp:Label ID="lbl_lumpsum" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th>TBT Verified By</th>
                                    <td>
                                        <asp:Label ID="lbl_tbtverifiedby" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th>Original Permit No</th>
                                    <td>
                                        <asp:Label ID="lbl_originalpermit" runat="server" CssClass="text-muted"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th>Unblocked Until</th>
                                    <td>
                                        <asp:Label ID="lbl_unblockeduntil" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th>GPS Location (Creation)</th>
                                    <td>
                                        <asp:Label ID="lbl_gps" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>

            </div>
            </div>

            <div role="tabpanel" class="tab-pane" id="cockpit_permits">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel modern-panel">
                        <div class="x_title">
                            <h2><i class="fa fa-folder-open-o"></i>Attached Permits (<asp:Label ID="lbl_filecount" runat="server" Text="0"></asp:Label>)</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="table-responsive">
                                <asp:GridView ID="gvPermits" runat="server" Width="100%" CssClass="table table-striped jambo_table table-bordered table-sm" AutoGenerateColumns="false" EmptyDataText="<div class='p-3 text-center text-muted'>No Permits Attached</div>" OnRowCommand="gvPermits_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="File Details">
                                            <ItemTemplate>
                                                <strong><%# Eval("Name") %></strong><br />
                                                <small class="text-muted"><%# Eval("UploadType") %> (<%# Eval("Extension") %>)</small>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Uploaded By">
                                            <ItemTemplate>
                                                <%# Eval("Submitter_Name") %>
                                                <br />
                                                <small>[<%# Eval("Submitter_Wrk") %>]</small>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TimeStamp" HeaderText="Uploaded On" DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" />
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" runat="server" CommandName="DownloadDoc" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-primary btn-sm" ToolTip="Download File"><i class="fa fa-download"></i></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text-center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="headings" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
            </div>

            <div role="tabpanel" class="tab-pane" id="cockpit_manpower">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel modern-panel">
                        <div class="x_title" style="display: flex; justify-content: space-between; align-items: center;">
                            <h2 style="margin: 0;"><i class="fa fa-users"></i>Manpower & Attendance Ledger</h2>

                            <div style="display: flex; align-items: center;">
                                <asp:CheckBox ID="chk_ShowDeleted" runat="server" AutoPostBack="true"
                                    OnCheckedChanged="chk_ShowDeleted_CheckedChanged"
                                    Text="&nbsp;Show Deleted Records"
                                    CssClass="text-danger font-weight-bold mr-3"
                                    Style="font-size: 13px; cursor: pointer;" />

                                <ul class="nav navbar-right panel_toolbox m-0">
                                    <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                                </ul>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="card-box table-responsive">
                                <asp:GridView ID="gvManpower" runat="server" Width="100%" CssClass="table table-striped jambo_table table-bordered table-sm dt-responsive nowrap" AutoGenerateColumns="false" EmptyDataText="<div class='p-3 text-center text-muted'>No Manpower IN-Punched Yet</div>" OnRowCommand="gvManpower_RowCommand" OnRowDataBound="gvManpower_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                            <ItemStyle CssClass="text-center" Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Employee Details">
                                            <ItemTemplate>
                                                <span class="font-weight-bold" style='<%# Eval("DeleteStatus").ToString() == "1" ? "color: #e74c3c; text-decoration: line-through;": "color: #2a3f54;" %>'>
                                                    <%# Eval("EmployeeName") %>
                                                </span>
                                                <span runat="server" visible='<%# Eval("DeleteStatus").ToString() == "1" %>' class="badge bg-danger ml-1" style="font-size: 9px; padding: 2px 5px;">DELETED</span>
                                                <br />
                                                <small class="text-muted"><i class="fa fa-id-card-o"></i><%# Eval("EmployeeWrk") %> | <%# Eval("EmpDesignation") %></small>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="IN-Punch">
                                            <ItemTemplate>
                                                <strong><%# Eval("Inpunch_Time", "{0:hh:mm tt}") %></strong><br />
                                                <small class="text-muted">Sys: <%# Eval("TimeStamp", "{0:dd-MMM hh:mm tt}") %></small>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OUT-Punch">
                                            <ItemTemplate>
                                                <%# Eval("Outpunch_Time") == DBNull.Value ? "<span class='badge bg-warning text-dark'>Pending</span>" : "<strong>" + Convert.ToDateTime(Eval("Outpunch_Time")).ToString("hh:mm tt") + "</strong><br /><small class='text-muted'>Sys: " + (Eval("LastModified") != DBNull.Value ? Convert.ToDateTime(Eval("LastModified")).ToString("dd-MMM hh:mm tt") : "") + "</small>" %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="WorkedHours" HeaderText="Worked Hrs" ItemStyle-CssClass="text-center font-weight-bold" />
                                        <asp:BoundField DataField="Calc_OT" HeaderText="Sys OT" ItemStyle-CssClass="text-center" />
                                        <asp:BoundField DataField="ProvidedOT" HeaderText="Final OT" ItemStyle-CssClass="text-center font-weight-bold text-success" />
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <span class="badge <%# Eval("AttendanceStatus").ToString() == "Present" || Eval("AttendanceStatus").ToString() == "Approved" ? "bg-green" : (Eval("AttendanceStatus").ToString() == "Invalidated" ? "bg-dark" : "bg-red") %>"><%# Eval("AttendanceStatus") %></span>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text-center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Admin Action" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEditWorker" runat="server" CommandName="EditWorker" CommandArgument='<%# Eval("Id") %>'
                                                    CssClass="btn btn-warning btn-xs text-dark" ToolTip="Modify Time/OT"
                                                    Visible='<%# IsAdmin() && Eval("DeleteStatus").ToString() != "1" %>'>
            <i class="fa fa-pencil"></i> Edit
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnInvalidate" runat="server" CommandName="InvalidateWorker" CommandArgument='<%# Eval("EmployeeWrk") %>'
                                                    CssClass="btn btn-danger btn-xs" ToolTip="Invalidate Worker" OnClientClick="return confirm('ADMIN ACTION: Remove this worker from the roster?');"
                                                    Visible='<%# IsAdmin() && Eval("DeleteStatus").ToString() != "1" %>'>
            <i class="fa fa-trash"></i> Drop
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text-center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="headings" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            </div>

            <div role="tabpanel" class="tab-pane" id="cockpit_csm">
            <div class="row">
                <div class="col-md-6 col-sm-12">
                    <div class="x_panel modern-panel border-top-info">
                        <div class="x_title">
                            <h2><i class="fa fa-bullhorn text-info"></i>Toolbox Talk (TBT) Records</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="table-responsive">
                                <asp:GridView ID="gvTBT" runat="server" Width="100%" CssClass="table table-striped jambo_table table-bordered table-sm" AutoGenerateColumns="false" EmptyDataText="<div class='p-3 text-center text-muted'>No TBT Records Found</div>">
                                    <Columns>
                                        <asp:BoundField DataField="TBT_ID" HeaderText="TBT ID" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#2a3f54" />
                                        <asp:BoundField DataField="TimeStamp" HeaderText="Submitted On" DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" />
                                        <asp:BoundField DataField="TBT_SupvName" HeaderText="Conducted By" />
                                        <asp:BoundField DataField="ContractEmployees" HeaderText="Attendees" ItemStyle-CssClass="text-center font-weight-bold" />
                                        <asp:TemplateField HeaderText="Safety Status">
                                            <ItemTemplate>
                                                <span class="badge <%# Eval("SafetySupvApprovalStatus").ToString() == "Approved" ? "bg-green" : "bg-warning text-dark" %>">
                                                    <%# Eval("SafetySupvApprovalStatus") %>
                                                </span>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text-center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="headings" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6 col-sm-12">
                    <div class="x_panel modern-panel border-top-info">
                        <div class="x_title">
                            <h2><i class="fa fa-book text-info"></i>Standard Operating Procedure (SOP)</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="table-responsive">
                                <asp:GridView ID="gvSOP" runat="server" Width="100%" CssClass="table table-striped jambo_table table-bordered table-sm" AutoGenerateColumns="false" EmptyDataText="<div class='p-3 text-center text-muted'>No SOP Records Found</div>">
                                    <Columns>
                                        <asp:BoundField DataField="SOP_ID" HeaderText="SOP ID" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#2a3f54" />
                                        <asp:BoundField DataField="SOPTitle" HeaderText="SOP Title" />
                                        <asp:BoundField DataField="SOPTrainer" HeaderText="Trainer" />
                                        <asp:BoundField DataField="SOPDuration" HeaderText="Duration" ItemStyle-CssClass="text-center" />
                                        <asp:TemplateField HeaderText="Safety Status">
                                            <ItemTemplate>
                                                <span class="badge <%# Eval("SafetySupvApprovalStatus").ToString() == "Approved" ? "bg-green" : "bg-warning text-dark" %>">
                                                    <%# Eval("SafetySupvApprovalStatus") %>
                                                </span>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text-center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="headings" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            </div>

            <div role="tabpanel" class="tab-pane" id="cockpit_admin">
            <div class="row admin-console-card">
                <div class="col-md-4 col-sm-12">
                    <div class="x_panel modern-panel" style="border-left: 5px solid #2a3f54;">
                        <div class="x_title">
                            <h2><i class="fa fa-database"></i>Raw Inspector</h2>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <p class="text-muted">Admin only. Existing modal. Permits exclude blob.</p>
                            <asp:LinkButton ID="btn_Act_ViewRawData" runat="server" CssClass="btn btn-dark btn-sm text-white" OnClick="btn_Act_ViewRawData_Click" Visible="false" CausesValidation="false"
                                ToolTip="IMPACT: Opens a developer-level view of the raw database rows linked to this JOBID for deep debugging."><i class="fa fa-database"></i> Raw DB Inspector</asp:LinkButton>
                        </div>
                    </div>
                </div>
                <div class="col-md-4 col-sm-12">
                    <div class="x_panel modern-panel" style="border-left: 5px solid #3498DB;">
                        <div class="x_title">
                            <h2><i class="fa fa-pencil-square-o"></i>Edit Core</h2>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <p class="text-muted">Admin only. Shift and title. Existing modal and save SQL.</p>
                            <asp:LinkButton ID="btn_Act_EditCoreAdmin" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btn_EditCoreDetails_Click" Visible="false" CausesValidation="false"><i class="fa fa-edit"></i> Edit Core Details</asp:LinkButton>
                        </div>
                    </div>
                </div>
                <div class="col-md-4 col-sm-12">
                    <div class="x_panel modern-panel" style="background-color: #f8f9fa; border-left: 5px solid #2a3f54;">
                        <div class="x_title">
                            <h2><i class="fa fa-cogs"></i>Overrides</h2>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content mb-0 pb-0">
                            <div class="d-flex flex-wrap align-items-center">
                                <%-- Exception Handling & Overrides (IDs and bindings unchanged) --%>
                                <asp:LinkButton ID="btn_Act_Unblock" runat="server" CssClass="btn btn-dark btn-sm mb-2 mr-2" OnClick="btn_Act_Unblock_Click"
                                    OnClientClick="if(!confirm('Are you sure you want to Unblock this JOB and grant a 24-hour grace period?')) return false;" Visible="false" CausesValidation="false"
                                    ToolTip="IMPACT: Removes the 72-hour system block and grants a 24-hour grace period for corrections."><i class="fa fa-unlock"></i> Unblock JOB</asp:LinkButton>

                                <asp:LinkButton ID="btn_Act_Resubmit" runat="server" CssClass="btn btn-secondary btn-sm mb-2 mr-2" OnClick="btn_Act_Resubmit_Click" Visible="false" CausesValidation="false"
                                    ToolTip="IMPACT: Resets rejected/cancelled jobs back to 'In-Punch Done' state and invalidates existing attendance so the supervisor can correct it."><i class="fa fa-refresh"></i> Fix & Resubmit</asp:LinkButton>

                                <asp:LinkButton ID="btn_Act_ForceOut" runat="server" CssClass="btn btn-danger btn-sm mb-2 mr-2" OnClick="btn_Act_ForceOut_Click"
                                    OnClientClick="if(!confirm('WARNING: This will forcefully clock out all manpower. Proceed?')) return false;" Visible="false" CausesValidation="false"
                                    ToolTip="IMPACT: Automatically clocks out all active workers using their standard shift hours and forces the job to close."><i class="fa fa-stop-circle"></i> Force OUT-Punch</asp:LinkButton>

                                <asp:LinkButton ID="btn_Act_SwapDate" runat="server" CssClass="btn btn-secondary btn-sm mb-2 mr-2" OnClick="btn_Act_SwapDate_Click" Visible="false" CausesValidation="false"
                                    ToolTip="IMPACT: Changes the official creation date of this job (Allowed only if no attendance is logged)."><i class="fa fa-calendar"></i> Swap Date</asp:LinkButton>

                                <asp:LinkButton ID="btn_Act_Delete" runat="server" CssClass="btn btn-danger btn-sm mb-2 mr-2" OnClick="btn_Act_Delete_Click"
                                    OnClientClick="if(!confirm('Delete this JOB permanently? This cannot be undone.')) return false;" Visible="false" CausesValidation="false"
                                    ToolTip="IMPACT: Permanently soft-deletes this job and completely removes it from all active operational workflows."><i class="fa fa-trash"></i> Delete JOB</asp:LinkButton>

                                <asp:LinkButton ID="btn_Act_ForcePermitBypass" runat="server" CssClass="btn btn-warning btn-sm mb-2 mr-2 text-dark" OnClick="btn_Act_ForcePermitBypass_Click"
                                    OnClientClick="if(!confirm('EMERGENCY BYPASS: Bypass the safety permit requirement?')) return false;" Visible="false" CausesValidation="false"
                                    ToolTip="IMPACT: Administratively bypasses the safety permit requirement, unlocking the job for IN-Punching immediately."><i class="fa fa-shield"></i> Bypass Permits</asp:LinkButton>

                                <asp:LinkButton ID="btn_Act_ResetToCreated" runat="server" CssClass="btn btn-danger btn-sm mb-2 mr-2" OnClick="btn_Act_ResetToCreated_Click"
                                    OnClientClick="if(!confirm('ROLLBACK: Reset the job to Step 1 and delete attached permits?')) return false;" Visible="false" CausesValidation="false"
                                    ToolTip="IMPACT: Deletes all uploaded permits and rolls the job back to Step 1 (Created state)."><i class="fa fa-backward"></i> Reset to Created</asp:LinkButton>

                                <asp:LinkButton ID="btn_Act_CancelShift" runat="server" CssClass="btn btn-dark btn-sm mb-2 mr-2" OnClick="btn_Act_CancelShift_Click"
                                    OnClientClick="if(!confirm('CANCEL SHIFT: Mark this job as Void/Cancelled?')) return false;" Visible="false" CausesValidation="false"
                                    ToolTip="IMPACT: Voids the ghost shift completely and safely archives the record."><i class="fa fa-times-circle"></i> Cancel/Void Shift</asp:LinkButton>

                                <asp:LinkButton ID="btn_Act_AdminRollback" runat="server" CssClass="btn btn-warning btn-sm mb-2 mr-2 text-dark" OnClick="btn_Act_AdminRollback_Click"
                                    OnClientClick="if(!confirm('ADMIN OVERRIDE: Revoke this approval and roll the JOB back to the Supervisor for corrections?')) return false;" Visible="false" CausesValidation="false"
                                    ToolTip="IMPACT: Revokes Final Approval, changes status to 'Returned', and rolls the job back to the supervisor for critical payroll corrections.">
    <i class="fa fa-undo text-danger"></i> Admin Rollback
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel modern-panel">
                        <div class="x_title">
                            <h2><i class="fa fa-history"></i>Audit Timeline</h2>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <p class="text-muted mb-2">Read-only. Existing timestamps only. Close &amp; Send remains on job_outpunch_v2.</p>
                            <div class="audit-timeline">
                                <div class="audit-node" id="audit_created" runat="server">
                                    <div class="audit-dot"></div>
                                    <div class="audit-name">Created</div>
                                    <asp:Label ID="lbl_audit_created" runat="server" CssClass="audit-when"></asp:Label>
                                </div>
                                <div class="audit-node" id="audit_in" runat="server">
                                    <div class="audit-dot"></div>
                                    <div class="audit-name">First IN</div>
                                    <asp:Label ID="lbl_audit_in" runat="server" CssClass="audit-when"></asp:Label>
                                </div>
                                <div class="audit-node" id="audit_permit" runat="server">
                                    <div class="audit-dot"></div>
                                    <div class="audit-name">Permit</div>
                                    <asp:Label ID="lbl_audit_permit" runat="server" CssClass="audit-when"></asp:Label>
                                </div>
                                <div class="audit-node" id="audit_out" runat="server">
                                    <div class="audit-dot"></div>
                                    <div class="audit-name">OUT</div>
                                    <asp:Label ID="lbl_audit_out" runat="server" CssClass="audit-when"></asp:Label>
                                </div>
                                <div class="audit-node" id="audit_close" runat="server">
                                    <div class="audit-dot"></div>
                                    <div class="audit-name">Close &amp; Send</div>
                                    <asp:Label ID="lbl_audit_close" runat="server" CssClass="audit-when"></asp:Label>
                                </div>
                                <div class="audit-node" id="audit_approval" runat="server">
                                    <div class="audit-dot"></div>
                                    <div class="audit-name">Approval</div>
                                    <asp:Label ID="lbl_audit_approval" runat="server" CssClass="audit-when"></asp:Label>
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
    <!-- Worker Modification Modal -->
    <div class="modal fade" id="modalEditWorker" tabindex="-1" data-backdrop="static">
        <div class="modal-dialog">
            <div class="modal-content modern-panel">
                <div class="modal-header bg-warning">
                    <h4 class="modal-title text-dark font-weight-bold"><i class="fa fa-clock-o"></i>Modify Worker Attendance</h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body p-4">
                    <asp:HiddenField ID="hf_EditWorkerId" runat="server" />
                    <h5 class="text-primary font-weight-bold" id="lbl_EditWorkerName" runat="server"></h5>
                    <hr />
                    <div class="row">
                        <div class="col-md-6 form-group">
                            <label class="top-label">IN Time</label>
                            <asp:TextBox ID="txt_EditInTime" runat="server" CssClass="form-control modern-input" type="datetime-local"></asp:TextBox>
                        </div>
                        <div class="col-md-6 form-group">
                            <label class="top-label">OUT Time</label>
                            <asp:TextBox ID="txt_EditOutTime" runat="server" CssClass="form-control modern-input" type="datetime-local"></asp:TextBox>
                        </div>
                        <div class="col-md-6 form-group">
                            <label class="top-label">Final OT (Hours)</label>
                            <asp:TextBox ID="txt_EditOT" runat="server" CssClass="form-control modern-input text-danger font-weight-bold" type="number" step="0.5"></asp:TextBox>
                        </div>
                        <div class="col-md-6 form-group">
                            <label class="top-label">Status</label>
                            <asp:DropDownList ID="ddl_EditStatus" runat="server" CssClass="form-control modern-input">
                                <asp:ListItem Value="Present">Present</asp:ListItem>
                                <asp:ListItem Value="Exit">Exit</asp:ListItem>
                                <asp:ListItem Value="Absent">Absent</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                    <asp:Button ID="btn_SaveWorkerEdit" runat="server" Text="Save Changes" CssClass="btn btn-success" OnClick="btn_SaveWorkerEdit_Click" />
                </div>
            </div>
        </div>
    </div>
    <!-- Core Details Modification Modal -->
    <div class="modal fade" id="modalEditCoreDetails" tabindex="-1" data-backdrop="static">
        <div class="modal-dialog">
            <div class="modal-content modern-panel">
                <div class="modal-header" style="background-color: #3498DB; color: white;">
                    <h4 class="modal-title font-weight-bold"><i class="fa fa-pencil-square-o"></i>Edit Core Details</h4>
                    <button type="button" class="close text-white" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body p-4">
                    <div class="row">
                        <div class="col-md-12 form-group">
                            <label class="top-label">JOB Shift <span class="req-star">*</span></label>
                            <asp:TextBox ID="txt_EditShift" runat="server" CssClass="form-control modern-input" MaxLength="1"></asp:TextBox>
                            <small class="text-muted d-block mt-1">Example: A, B, C, or G</small>
                        </div>
                        <div class="col-md-12 form-group mt-3">
                            <label class="top-label">JOB Title <span class="req-star">*</span></label>
                            <asp:TextBox ID="txt_EditTitle" runat="server" CssClass="form-control modern-input" TextMode="MultiLine" Rows="3"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="modal-footer" style="border-top: 1px solid #e9ecef;">
                    <button type="button" class="btn btn-secondary btn-modern" data-dismiss="modal">Cancel</button>
                    <asp:Button ID="btn_SaveCoreDetails" runat="server" Text="Save Changes" CssClass="btn btn-success btn-modern" OnClick="btn_SaveCoreDetails_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- RAW DATABASE INSPECTOR MODAL -->
    <div class="modal fade" id="modalRawData" tabindex="-1" data-backdrop="static">
        <div class="modal-dialog" style="width: 95%; max-width: 1400px;">
            <!-- Extra Wide for DB Columns -->
            <div class="modal-content modern-panel">
                <div class="modal-header bg-dark text-white">
                    <h4 class="modal-title font-weight-bold"><i class="fa fa-terminal"></i>Database Inspector: Raw Table Records</h4>
                    <button type="button" class="close text-white" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body p-3" style="background-color: #f4f6f9;">

                    <!-- Bootstrap Nav Tabs -->
                    <ul class="nav nav-tabs" role="tablist">
                        <li role="presentation" class="active">
                            <a href="#tab_jobs" aria-controls="tab_jobs" role="tab" data-toggle="tab" style="font-weight: bold; color: #2a3f54;"><i class="fa fa-briefcase"></i>tbl_jobs</a>
                        </li>
                        <li role="presentation">
                            <a href="#tab_attendance" aria-controls="tab_attendance" role="tab" data-toggle="tab" style="font-weight: bold; color: #2a3f54;"><i class="fa fa-users"></i>tbl_attendance</a>
                        </li>
                        <li role="presentation">
                            <a href="#tab_permits" aria-controls="tab_permits" role="tab" data-toggle="tab" style="font-weight: bold; color: #2a3f54;"><i class="fa fa-paperclip"></i>tbl_jobspermit</a>
                        </li>
                        <li role="presentation">
                            <a href="#tab_csm" aria-controls="tab_csm" role="tab" data-toggle="tab" style="font-weight: bold; color: #2a3f54;"><i class="fa fa-shield"></i>tbl_tbt & tbl_sop</a>
                        </li>
                    </ul>

                    <!-- Tab Panes -->
                    <div class="tab-content" style="background: white; border: 1px solid #ddd; border-top: none; padding: 15px;">

                        <!-- Tab 1: Master Job Table -->
                        <div role="tabpanel" class="tab-pane active" id="tab_jobs">
                            <div class="table-responsive" style="max-height: 500px; overflow-y: auto; overflow-x: auto; white-space: nowrap;">
                                <asp:GridView ID="gvRawJobs" runat="server" CssClass="table table-bordered table-sm table-striped table-hover" AutoGenerateColumns="true"></asp:GridView>
                            </div>
                        </div>

                        <!-- Tab 2: Attendance Table -->
                        <div role="tabpanel" class="tab-pane" id="tab_attendance">
                            <div class="table-responsive" style="max-height: 500px; overflow-y: auto; overflow-x: auto; white-space: nowrap;">
                                <asp:GridView ID="gvRawAttendance" runat="server" CssClass="table table-bordered table-sm table-striped table-hover" AutoGenerateColumns="true"></asp:GridView>
                            </div>
                        </div>

                        <!-- Tab 3: Permits Table -->
                        <div role="tabpanel" class="tab-pane" id="tab_permits">
                            <div class="table-responsive" style="max-height: 500px; overflow-y: auto; overflow-x: auto; white-space: nowrap;">
                                <asp:GridView ID="gvRawPermits" runat="server" CssClass="table table-bordered table-sm table-striped table-hover" AutoGenerateColumns="true"></asp:GridView>
                            </div>
                        </div>

                        <!-- Tab 4: CSM Tables -->
                        <div role="tabpanel" class="tab-pane" id="tab_csm">
                            <h6 class="text-primary font-weight-bold">Table: tbl_tbt</h6>
                            <div class="table-responsive mb-4" style="max-height: 250px; overflow-y: auto; overflow-x: auto; white-space: nowrap;">
                                <asp:GridView ID="gvRawTBT" runat="server" CssClass="table table-bordered table-sm table-striped table-hover" AutoGenerateColumns="true"></asp:GridView>
                            </div>
                            <h6 class="text-primary font-weight-bold">Table: tbl_sop</h6>
                            <div class="table-responsive" style="max-height: 250px; overflow-y: auto; overflow-x: auto; white-space: nowrap;">
                                <asp:GridView ID="gvRawSOP" runat="server" CssClass="table table-bordered table-sm table-striped table-hover" AutoGenerateColumns="true"></asp:GridView>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary btn-modern" data-dismiss="modal">Close Inspector</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('[title]').tooltip({
                placement: 'bottom',
                trigger: 'hover'
            });
        });

        $(document).ready(function () {
            var stored = sessionStorage.getItem('job360-cockpit-tab');
            if (stored && stored.indexOf('#cockpit_') === 0) {
                $('.cockpit-tabs a[href="' + stored + '"]').tab('show');
            }
            $('.cockpit-tabs a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                sessionStorage.setItem('job360-cockpit-tab', $(e.target).attr('href'));
            });
        });

        $(document).ready(function () {
            // --- Session Keep-Alive Heartbeat ---
            // Pings the server every 10 minutes (600,000 ms) to prevent IIS from destroying the session
            // while the Admin is busy reading long Raw Data Inspector tables.

            setInterval(function () {
                // We use a lightweight GET request to the current page.
                // Appending a timestamp prevents browser caching.
                $.get(location.href + (location.href.indexOf('?') > -1 ? '&' : '?') + "keepAlive=" + new Date().getTime())
                 .done(function () {
                     console.log("Session heartbeat sent successfully.");
                 })
                 .fail(function () {
                     console.warn("Session heartbeat failed. Network disconnected?");
                 });
            }, 600000); // 10 minutes
        });
    </script>
</asp:Content>
