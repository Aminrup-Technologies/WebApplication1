<%@ Page Title="360-Degree JOB View" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="job_360_view.aspx.cs" Inherits="WebApplication1.bussiness.production.job_360_view" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Custom Stepper adapted for your Admin Theme */
        .stepper-wrapper { display: flex; justify-content: space-between; margin-bottom: 20px; margin-top: 10px; align-items: flex-start; }
        .stepper-item { position: relative; display: flex; flex-direction: column; align-items: center; flex: 1; text-align: center; }
        .stepper-item::before { position: absolute; content: ""; border-bottom: 3px solid #e1e5eb; width: 100%; top: 20px; left: -50%; z-index: 2; }
        .stepper-item::after { position: absolute; content: ""; border-bottom: 3px solid #e1e5eb; width: 100%; top: 20px; left: 50%; z-index: 2; }
        .stepper-item .step-counter { position: relative; z-index: 5; display: flex; justify-content: center; align-items: center; width: 40px; height: 40px; border-radius: 50%; background: #e1e5eb; color: #73879C; font-weight: bold; margin-bottom: 8px; border: 2px solid white; box-shadow: 0 0 5px rgba(0,0,0,0.1); }
        
        /* Status Colors */
        .stepper-item.completed .step-counter { background-color: #1ABB9C; color: white; border-color: #1ABB9C; }
        .stepper-item.completed::before, .stepper-item.completed::after { border-color: #1ABB9C; }
        .stepper-item.active .step-counter { background-color: #3498DB; color: white; border-color: #3498DB; box-shadow: 0 0 10px rgba(52,152,219,0.4); }
        .stepper-item.failed .step-counter { background-color: #E74C3C; color: white; }
        .stepper-item.skipped .step-counter { background-color: #f8f9fa; color: #adb5bd; border: 2px dashed #ced4da; box-shadow: none; }
        
        .stepper-item:first-child::before { content: none; }
        .stepper-item:last-child::after { content: none; }
        
        .step-name { font-size: 13px; font-weight: 600; color: #73879C; text-align:center; }
        .stepper-item.active .step-name { color: #3498DB; }
        .stepper-item.skipped .step-name { color: #adb5bd; text-decoration: line-through; }
        
        /* Details & TAT Panel styling */
        .step-details { font-size: 11.5px; color: #555; margin-top: 5px; line-height: 1.4; background: #f9f9f9; padding: 5px 10px; border-radius: 5px; border: 1px solid #eee; display: inline-block; min-width: 140px; box-shadow: 0 1px 3px rgba(0,0,0,0.05); }
        .stepper-item.skipped .step-details { background: transparent; border: none; font-style: italic; box-shadow:none; }
        
        .tat-badge { background-color: #e8f4f8; color: #2980b9; padding: 2px 6px; border-radius: 12px; font-size: 10px; font-weight: 700; display: inline-block; margin-top: 4px; border: 1px dashed #b3d4e6; }
        .step-details strong { color: #333; }
        
        /* General Table & Panel Styling */
        .detail-table th { background-color: #f7f7f7; width: 35%; color: #555; }
        .modern-panel { border-radius: 8px; border: none; box-shadow: 0 2px 10px rgba(0,0,0,0.05); }
        .border-top-info { border-top: 4px solid #17a2b8 !important; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="page-title">
            <div class="title_left">
                <h3><i class="fa fa-globe"></i> JOB 360-Degree Lifecycle View</h3>
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
            
            <div class="row" id="ActionBarRow" runat="server" visible="false">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel modern-panel" style="background-color: #f8f9fa; border-left: 5px solid #2a3f54;">
                        <div class="x_content mb-0 pb-0">
                            <div class="d-flex flex-wrap align-items-center">
                                <h5 class="mr-4 mb-2 font-weight-bold text-dark"><i class="fa fa-cogs"></i> Available Actions:</h5>
                                
                                <asp:LinkButton ID="btn_Act_UploadPermit" runat="server" CssClass="btn btn-primary btn-sm mb-2 mr-2" OnClick="btn_Act_UploadPermit_Click" Visible="false"><i class="fa fa-paperclip"></i> Upload Permit</asp:LinkButton>
                                <asp:LinkButton ID="btn_Act_InPunch" runat="server" CssClass="btn btn-info btn-sm mb-2 mr-2" OnClick="btn_Act_InPunch_Click" Visible="false"><i class="fa fa-sign-in"></i> IN-Punch Manpower</asp:LinkButton>
                                <asp:LinkButton ID="btn_Act_AddDocs" runat="server" CssClass="btn btn-info btn-sm mb-2 mr-2" OnClick="btn_Act_AddDocs_Click" Visible="false"><i class="fa fa-file-text"></i> Add Site Docs</asp:LinkButton>
                                <asp:LinkButton ID="btn_Act_OutPunch" runat="server" CssClass="btn btn-warning btn-sm mb-2 mr-2 text-dark" OnClick="btn_Act_OutPunch_Click" Visible="false"><i class="fa fa-sign-out"></i> OUT-Punch Shift</asp:LinkButton>

                                <asp:LinkButton ID="btn_Act_Unblock" runat="server" CssClass="btn btn-dark btn-sm mb-2 mr-2" OnClick="btn_Act_Unblock_Click" OnClientClick="return confirm('Are you sure you want to Unblock this JOB and grant a 24-hour grace period?');" Visible="false"><i class="fa fa-unlock"></i> Unblock JOB</asp:LinkButton>
                                <asp:LinkButton ID="btn_Act_Resubmit" runat="server" CssClass="btn btn-secondary btn-sm mb-2 mr-2" OnClick="btn_Act_Resubmit_Click" Visible="false"><i class="fa fa-refresh"></i> Fix & Resubmit</asp:LinkButton>
                                <asp:LinkButton ID="btn_Act_ForceOut" runat="server" CssClass="btn btn-danger btn-sm mb-2 mr-2" OnClick="btn_Act_ForceOut_Click" OnClientClick="return confirm('WARNING: This will forcefully clock out all manpower. Proceed?');" Visible="false"><i class="fa fa-stop-circle"></i> Force OUT-Punch</asp:LinkButton>

                                <asp:LinkButton ID="btn_Act_SwapDate" runat="server" CssClass="btn btn-secondary btn-sm mb-2 mr-2" OnClick="btn_Act_SwapDate_Click" Visible="false"><i class="fa fa-calendar"></i> Swap Date</asp:LinkButton>
                                <asp:LinkButton ID="btn_Act_Delete" runat="server" CssClass="btn btn-danger btn-sm mb-2 mr-2" OnClick="btn_Act_Delete_Click" OnClientClick="return confirm('Delete this JOB permanently? This cannot be undone.');" Visible="false"><i class="fa fa-trash"></i> Delete JOB</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel modern-panel">
                        <div class="x_title">
                            <h2><i class="fa fa-line-chart"></i> Smart Pipeline & Progress Status</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content mt-3">
                            <div class="stepper-wrapper" id="stepperContainer" runat="server">
                                <div class="stepper-item" id="step1" runat="server">
                                    <div class="step-counter"><i class="fa fa-file-text-o"></i></div>
                                    <div class="step-name">Created</div>
                                    <asp:Literal ID="lit_step1_details" runat="server"></asp:Literal>
                                </div>
                                <div class="stepper-item" id="step2" runat="server">
                                    <div class="step-counter"><i class="fa fa-paperclip"></i></div>
                                    <div class="step-name">Upload Permit</div>
                                    <asp:Literal ID="lit_step2_details" runat="server"></asp:Literal>
                                </div>
                                <div class="stepper-item" id="step3" runat="server">
                                    <div class="step-counter"><i class="fa fa-sign-in"></i></div>
                                    <div class="step-name">IN-Punched</div>
                                    <asp:Literal ID="lit_step3_details" runat="server"></asp:Literal>
                                </div>
                                <div class="stepper-item" id="step4" runat="server">
                                    <div class="step-counter"><i class="fa fa-folder-open-o"></i></div>
                                    <div class="step-name">Site Docs (CSM)</div>
                                    <asp:Literal ID="lit_step4_details" runat="server"></asp:Literal>
                                </div>
                                <div class="stepper-item" id="step5" runat="server">
                                    <div class="step-counter"><i class="fa fa-sign-out"></i></div>
                                    <div class="step-name">OUT-Punched</div>
                                    <asp:Literal ID="lit_step5_details" runat="server"></asp:Literal>
                                </div>
                                <div class="stepper-item" id="step6" runat="server">
                                    <div class="step-counter"><i class="fa fa-check-square-o"></i></div>
                                    <div class="step-name">Final Approval</div>
                                    <asp:Literal ID="lit_step6_details" runat="server"></asp:Literal>
                                </div>
                            </div>

                            <hr />
                            
                            <div class="alert alert-warning text-dark mb-0" id="divBottleneck" runat="server" style="font-size: 14px;">
                                <i class="fa fa-exclamation-triangle fa-lg"></i> <strong>Current Status: </strong> 
                                <asp:Label ID="lbl_bottleneck" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-6 col-sm-12">
                    <div class="x_panel modern-panel">
                        <div class="x_title">
                            <h2><i class="fa fa-info-circle"></i> Core Details</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <table class="table table-bordered table-sm detail-table">
                                <tr><th>JOB Date</th><td><asp:Label ID="lbl_jobdate" runat="server" CssClass="font-weight-bold text-primary"></asp:Label></td></tr>
                                <tr><th>Created By</th><td><asp:Label ID="lbl_creator" runat="server"></asp:Label></td></tr>
                                <tr><th>Work Site</th><td><asp:Label ID="lbl_worksite" runat="server"></asp:Label></td></tr>
                                <tr><th>Job Title</th><td><asp:Label ID="lbl_title" runat="server"></asp:Label></td></tr>
                                <tr><th>Shift</th><td><asp:Label ID="lbl_shift" runat="server"></asp:Label></td></tr>
                                <tr><th>Work Order No</th><td><asp:Label ID="lbl_wo" runat="server"></asp:Label></td></tr>
                                <tr><th>Current JOB Status</th><td><asp:Label ID="lbl_jobstatus" runat="server" Font-Bold="true"></asp:Label></td></tr>
                            </table>
                        </div>
                    </div>
                </div>

                <div class="col-md-6 col-sm-12">
                    <div class="x_panel modern-panel">
                        <div class="x_title">
                            <h2><i class="fa fa-folder-open-o"></i> Attached Permits (<asp:Label ID="lbl_filecount" runat="server" Text="0"></asp:Label>)</h2>
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
                                                <%# Eval("Submitter_Name") %> <br />
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

            <div class="row">
                <div class="col-md-6 col-sm-12">
                    <div class="x_panel modern-panel border-top-info">
                        <div class="x_title">
                            <h2><i class="fa fa-bullhorn text-info"></i> Toolbox Talk (TBT) Records</h2>
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
                            <h2><i class="fa fa-book text-info"></i> Standard Operating Procedure (SOP)</h2>
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

            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel modern-panel">
                        <div class="x_title">
                            <h2><i class="fa fa-users"></i> Manpower & Attendance Ledger</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="card-box table-responsive">
                                <asp:GridView ID="gvManpower" runat="server" Width="100%" CssClass="table table-striped jambo_table table-bordered table-sm dt-responsive nowrap" AutoGenerateColumns="false" EmptyDataText="<div class='p-3 text-center text-muted'>No Manpower IN-Punched Yet</div>">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                            <ItemStyle CssClass="text-center" Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Employee Details">
                                            <ItemTemplate>
                                                <span class="text-primary font-weight-bold"><%# Eval("EmployeeName") %></span> <br/>
                                                <small class="text-muted"><i class="fa fa-id-card-o"></i> <%# Eval("EmployeeWrk") %> | <%# Eval("EmpDesignation") %></small>
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
                                                <span class="badge <%# Eval("AttendanceStatus").ToString() == "Present" || Eval("AttendanceStatus").ToString() == "Approved" ? "bg-green" : "bg-red" %>"><%# Eval("AttendanceStatus") %></span>
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
    </div>
</asp:Content>