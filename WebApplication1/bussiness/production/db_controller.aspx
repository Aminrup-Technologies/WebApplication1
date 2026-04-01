<%@ Page Title="DB Controller" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="db_controller.aspx.cs" Inherits="WebApplication1.bussiness.production.db_controller" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .nav-tabs-custom > .nav-tabs > li.active > a {
            border-top-color: #3c8dbc;
            border-top: 3px solid #3498DB;
            font-weight: bold;
        }

        .tab-pane {
            padding: 20px;
            background: #fff;
            border: 1px solid #ddd;
            border-top: none;
        }

        .control-panel {
            background: #f8f9fa;
            border-radius: 4px;
            padding: 15px;
            margin-bottom: 20px;
        }
    </style>
    <link href='https://cdn.jsdelivr.net/npm/fullcalendar@5.11.3/main.min.css' rel='stylesheet' />
    <script src='https://cdn.jsdelivr.net/npm/fullcalendar@5.11.3/main.min.js'></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="container">
            <div class="page-title">
                <div class="title_left">
                    <h3>DB Controller <small>Master Data Management</small></h3>
                </div>
            </div>
            <div class="clearfix"></div>

            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

            <div class="row">
                <div class="col-md-12">
                    <div class="nav-tabs-custom">
                        <ul class="nav nav-tabs">
                            <li class="active"><a href="#tab_region_safety" data-toggle="tab"><i class="fa fa-shield text-danger"></i>Work Region</a></li>
                            <li><a href="#tab_wo_config" data-toggle="tab"><i class="fa fa-briefcase text-primary"></i>WO/PO Controller</a></li>
                            <li><a href="#tab_wo_matrix" data-toggle="tab"><i class="fa fa-cogs text-warning"></i>WO Rule Matrix</a></li>
                            <li><a href="#tab_doc_master" data-toggle="tab"><i class="fa fa-file-text-o text-success"></i>Document Master</a></li>              
                            <li><a href="#tab_smart_calendar" data-toggle="tab"><i class="fa fa-calendar text-info"></i>Smart Calendar</a></li>
                            <li><a href="#tab_backdate" data-toggle="tab"><i class="fa fa-history text-danger"></i>Backdate Exceptions</a></li>
                        </ul>

                        <div class="tab-content">

                            <div class="tab-pane active" id="tab_region_safety">
                                <asp:UpdatePanel ID="upRegion" runat="server">
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="control-panel">
                                                    <label>Select Target Region <span class="text-danger">*</span></label>
                                                    <asp:DropDownList ID="ddl_Regions" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_Regions_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row" id="RegionConfigRow" runat="server" visible="false">
                                            <div class="col-md-6">
                                                <h5 class="text-info"><i class="fa fa-map-marker"></i>Location Constraints</h5>
                                                <div class="well well-sm mt-3" style="background-color: #f9f9f9; padding: 15px; border-radius: 4px;">
                                                    <div class="form-group mb-0">
                                                        <label>Require GPS Tagging for JOB Creation?</label>
                                                        <asp:DropDownList ID="ddl_req_gps" runat="server" CssClass="form-control form-control-sm w-50 mt-1">
                                                            <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                            <asp:ListItem Value="No">No</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <small class="text-muted d-block mt-2">If set to Yes, users must allow browser geolocation to create a job in this region.</small>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-6 mt-3">
                                                <h5 class="text-primary"><i class="fa fa-money"></i> Allowed Billing Types</h5>
                                                <p class="small text-muted">Select which JOB Types are permitted for this Region.</p>
                                                <div class="well well-sm" style="background-color: #f9f9f9; padding: 15px; border-radius: 4px;">
                                                    <asp:CheckBoxList ID="cbl_BillingTypes" runat="server" RepeatColumns="1" CellPadding="5" CellSpacing="5" CssClass="table table-borderless table-sm"></asp:CheckBoxList>
                                                </div>
                                            </div>

                                            <div class="col-md-12 mt-2">
                                                <asp:Button ID="btn_SaveRegion" runat="server" Text="Update Region Rules" CssClass="btn btn-success" OnClick="btn_SaveRegion_Click" />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div class="tab-pane" id="tab_wo_config">
                                <asp:UpdatePanel ID="upWO" runat="server">
                                    <ContentTemplate>
                                        <div class="control-panel">
                                            <div class="row">
                                                <div class="col-md-3 form-group">
                                                    <label>1. Region</label>
                                                    <asp:DropDownList ID="ddl_wo_region" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_wo_region_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                                <div class="col-md-3 form-group">
                                                    <label>2. Company</label>
                                                    <asp:DropDownList ID="ddl_wo_company" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_wo_company_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                                <div class="col-md-3 form-group">
                                                    <label>3. Department</label>
                                                    <asp:DropDownList ID="ddl_wo_dept" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_wo_dept_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                                <div class="col-md-3 form-group">
                                                    <label>4. Work Order No.</label>
                                                    <asp:DropDownList ID="ddl_wo_number" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_wo_number_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row" id="WOConfigRow" runat="server" visible="false">
                                            <div class="col-md-4 form-group">
                                                <label>Contract Nature</label>
                                                <asp:DropDownList ID="ddl_contract_nature" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="ARC">ARC</asp:ListItem>
                                                    <asp:ListItem Value="Lumpsum">Lumpsum</asp:ListItem>
                                                    <asp:ListItem Value="Supply">Supply</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-4 form-group">
                                                <label>Billing Nature</label>
                                                <asp:DropDownList ID="ddl_billing_nature" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="Billing">Billing</asp:ListItem>
                                                    <asp:ListItem Value="Non-Billing">Non-Billing</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-4 form-group">
                                                <label>Execution Type</label>
                                                <asp:DropDownList ID="ddl_execution_type" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="Manpower Supply">Manpower Supply</asp:ListItem>
                                                    <asp:ListItem Value="Equipment">Equipment</asp:ListItem>
                                                    <asp:ListItem Value="Turnkey">Turnkey</asp:ListItem>
                                                    <asp:ListItem Value="Line Items JOB">Line Items JOB</asp:ListItem>
                                                    <asp:ListItem Value="One Time">One Time</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-12 mt-3 text-right">
                                                <asp:Button ID="btn_SaveWO" runat="server" Text="Update WO Parameters" CssClass="btn btn-primary" OnClick="btn_SaveWO_Click" />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div class="tab-pane" id="tab_doc_master">
                                <asp:UpdatePanel ID="upDoc" runat="server">
                                    <ContentTemplate>
                                        <div class="row control-panel align-items-end mb-3">
                                            <div class="col-md-3">
                                                <label>Category</label>
                                                <asp:DropDownList ID="ddl_newdoc_category" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="CSM">CSM Document</asp:ListItem>
                                                    <asp:ListItem Value="Permit">Safety Permit</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-6">
                                                <label>Document Name</label>
                                                <asp:TextBox ID="txt_newdoc_name" runat="server" CssClass="form-control" placeholder="e.g., Electrical Safety Clearance"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 mt-4">
                                                <asp:Button ID="btn_AddDoc" runat="server" Text="Add to Dictionary" CssClass="btn btn-dark btn-block" OnClick="btn_AddDoc_Click" />
                                            </div>
                                        </div>

                                        <asp:GridView ID="gv_DocMaster" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered" DataKeyNames="Doc_ID">
                                            <Columns>
                                                <asp:BoundField DataField="Doc_ID" HeaderText="ID" ItemStyle-Width="5%" />
                                                <asp:BoundField DataField="Doc_Category" HeaderText="Category" ItemStyle-Width="15%" ItemStyle-Font-Bold="true" />
                                                <asp:BoundField DataField="Doc_Name" HeaderText="Document Name" />
                                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <span class='badge <%# Convert.ToBoolean(Eval("IsActive")) ? "bg-green" : "bg-red" %>'>
                                                            <%# Convert.ToBoolean(Eval("IsActive")) ? "Active" : "Disabled" %>
                                                        </span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div class="tab-pane" id="tab_wo_matrix">
                                <asp:UpdatePanel ID="upMatrix" runat="server">
                                    <ContentTemplate>
                                        <div class="control-panel">
                                            <h5 class="text-warning"><i class="fa fa-sliders"></i>&nbsp;Define Work Order Rules</h5>
                                            <p class="text-muted small">Set up the required UI behaviors for specific combinations of Billing Nature and Execution Type.</p>

                                            <div class="row align-items-center">
                                                <div class="col-md-2 form-group">
                                                    <label>Billing Nature <span class="text-danger">*</span></label>
                                                    <asp:DropDownList ID="ddl_matrix_billing" runat="server" CssClass="form-control">
                                                        <asp:ListItem Value="Billing">Billing</asp:ListItem>
                                                        <asp:ListItem Value="Non-Billing">Non-Billing</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-2 form-group">
                                                    <label>Execution Type <span class="text-danger">*</span></label>
                                                    <asp:DropDownList ID="ddl_matrix_execution" runat="server" CssClass="form-control">
                                                        <asp:ListItem Value="Manpower Supply">Manpower Supply</asp:ListItem>
                                                        <asp:ListItem Value="Equipment">Equipment</asp:ListItem>
                                                        <asp:ListItem Value="Turnkey">Turnkey</asp:ListItem>
                                                        <asp:ListItem Value="Line Items JOB">Line Items JOB</asp:ListItem>
                                                        <asp:ListItem Value="One Time">One Time</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>

                                                <div class="col-md-2 form-group">
                                                    <label>Routing Destination <span class="text-danger">*</span></label>
                                                    <asp:DropDownList ID="ddl_matrix_routing" runat="server" CssClass="form-control">
                                                        <asp:ListItem Value="1">1 - Permit Upload</asp:ListItem>
                                                        <asp:ListItem Value="3">3 - Direct In-Punch</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>

                                                <div class="col-md-4 form-group">
                                                    <label>Enforced Rules (Check to Require)</label>
                                                    <div class="form-control" style="height: auto; padding: 8px 12px; background-color: #f4f6f9;">
                                                        <asp:CheckBox ID="chk_req_permit" runat="server" Text=" Permit No." CssClass="mr-3" Font-Bold="true" Checked="true" />&nbsp;&nbsp;
                                                        <asp:CheckBox ID="chk_req_csm" runat="server" Text=" CSM Docs" CssClass="mr-3" Font-Bold="true" Checked="true" />&nbsp;&nbsp;
                                                        <asp:CheckBox ID="chk_req_attendance" runat="server" Text=" Attendance" Font-Bold="true" Checked="true" />
                                                        <span style="border-left: 2px solid #ccc; margin-left: 5px; margin-right: 10px;"></span>
                                                        <asp:CheckBox ID="chk_auto_title" runat="server" Text=" Auto-Generate Title" Font-Bold="true" ForeColor="DarkBlue" />
                                                    </div>
                                                </div>
                                                <div class="col-md-2 form-group mt-4">
                                                    <asp:Button ID="btn_SaveMatrix" runat="server" Text="Save Rule" CssClass="btn btn-warning btn-block" OnClick="btn_SaveMatrix_Click" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row mt-3">
                                            <div class="col-md-12">
                                                <asp:GridView ID="gv_RuleMatrix" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered">
                                                    <Columns>
                                                        <asp:BoundField DataField="Billing_Nature" HeaderText="Billing Nature" ItemStyle-Width="15%" ItemStyle-Font-Bold="true" />
                                                        <asp:BoundField DataField="Execution_Type" HeaderText="Execution Type" ItemStyle-Width="15%" ItemStyle-Font-Bold="true" />

                                                        <asp:TemplateField HeaderText="Require Permit?" ItemStyle-Width="12%" ItemStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <span class='badge <%# Convert.ToBoolean(Eval("Req_PermitNo")) ? "bg-green" : "bg-red" %>'>
                                                                    <%# Convert.ToBoolean(Eval("Req_PermitNo")) ? "Yes" : "No" %>
                                                                </span>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Require CSM Docs?" ItemStyle-Width="12%" ItemStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <span class='badge <%# Convert.ToBoolean(Eval("Req_CSM_Docs")) ? "bg-green" : "bg-red" %>'>
                                                                    <%# Convert.ToBoolean(Eval("Req_CSM_Docs")) ? "Yes" : "No" %>
                                                                </span>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Require Attendance?" ItemStyle-Width="12%" ItemStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <span class='badge <%# Convert.ToBoolean(Eval("Req_Attendance")) ? "bg-green" : "bg-red" %>'>
                                                                    <%# Convert.ToBoolean(Eval("Req_Attendance")) ? "Yes" : "No" %>
                                                                </span>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Auto JOB Title?" ItemStyle-Width="12%" ItemStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <span class='badge <%# Convert.ToBoolean(Eval("Auto_Generate_Title")) ? "bg-green" : "bg-red" %>'>
                                                                    <%# Convert.ToBoolean(Eval("Auto_Generate_Title")) ? "Yes" : "No" %>
                                                                </span>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Routing Destination" ItemStyle-Width="22%" ItemStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <span class='badge <%# Eval("Default_MasterStatusCode").ToString() == "3" ? "bg-purple" : "bg-blue" %>'>
                                                                    <%# Eval("Default_MasterStatusCode").ToString() == "3" ? "3 - Direct In-Punch" : "1 - Permit Upload" %>
                                                                </span>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div class="alert alert-info text-center">No Matrix Rules have been defined yet.</div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div class="tab-pane" id="tab_smart_calendar">
                                <div class="row mb-3">
                                    <div class="col-md-4">
                                        <label>1. Select Company to Manage</label>
                                        <asp:DropDownList ID="ddl_cal_company" runat="server" CssClass="form-control" onchange="loadCompanyEvents();"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-9">
                                        <div class="control-panel">
                                            <div class="col-md-9">
                                                <div class="control-panel" style="position: relative; min-height: 650px;">
                                                    <div id="calendar-loading" style="display: none; position: absolute; top: 0; left: 0; width: 100%; height: 100%; background: rgba(255,255,255,0.8); z-index: 10; text-align: center; padding-top: 20%;">
                                                        <i class="fa fa-spinner fa-spin fa-3x text-primary"></i>
                                                        <h4 class="text-primary mt-2">Loading Calendar...</h4>
                                                    </div>

                                                    <div id='smart-calendar'></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <div class="control-panel" id="external-events">
                                            <h5 class="text-info"><i class="fa fa-tags"></i>Drag & Drop Tags</h5>
                                            <p class="small text-muted">Drag a tag onto a date to set the Attendance Rule for this company.</p>

                                            <hr />
                                            <div class='fc-event tag-draggable' data-code='FL' style='background-color: #e74c3c; border-color: #e74c3c; padding: 10px; margin-bottom: 10px; color: white; cursor: grab; border-radius: 4px;'>
                                                <i class="fa fa-star"></i>Festival Leave (FL)
                                            </div>
                                            <div class='fc-event tag-draggable' data-code='OD' style='background-color: #3498db; border-color: #3498db; padding: 10px; margin-bottom: 10px; color: white; cursor: grab; border-radius: 4px;'>
                                                <i class="fa fa-bed"></i>Weekly Off (OD)
                                            </div>
                                            <div class='fc-event tag-draggable' data-code='NH' style='background-color: #9b59b6; border-color: #9b59b6; padding: 10px; margin-bottom: 10px; color: white; cursor: grab; border-radius: 4px;'>
                                                <i class="fa fa-flag"></i>National Holiday (NH)
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="tab-pane" id="tab_backdate">
                                <asp:UpdatePanel ID="upBackdate" runat="server">
                                    <ContentTemplate>
                                        <div class="control-panel">
                                            <h5 class="text-danger"><i class="fa fa-history"></i>Backdate Limit Exceptions</h5>
                                            <p class="text-muted small"><strong>Global Default:</strong> All users are restricted to 2 days of backdating. Use this form to grant exceptions.</p>

                                            <div class="row align-items-center">
                                                <div class="col-md-3 form-group">
                                                    <label>Workman SL <span class="text-danger">*</span></label>
                                                    <asp:TextBox ID="txt_exc_workman" runat="server" CssClass="form-control" placeholder="e.g. 104523"></asp:TextBox>
                                                </div>
                                                <div class="col-md-3 form-group">
                                                    <label>Max Backdate <span class="text-danger">*</span></label>
                                                    <asp:DropDownList ID="ddl_exc_days" runat="server" CssClass="form-control">
                                                        <asp:ListItem Value="3">3 Days</asp:ListItem>
                                                        <asp:ListItem Value="5">5 Days</asp:ListItem>
                                                        <asp:ListItem Value="15">15 Days</asp:ListItem>
                                                        <asp:ListItem Value="30">30 Days</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-4 form-group">
                                                    <label>Remarks / Reason <span class="text-danger">*</span></label>
                                                    <asp:TextBox ID="txt_exc_remarks" runat="server" CssClass="form-control" placeholder="e.g. Approved by HR (Ticket #123)"></asp:TextBox>
                                                </div>
                                                <div class="col-md-2 form-group mt-4">
                                                    <asp:Button ID="btn_SaveException" runat="server" Text="Save Rule" CssClass="btn btn-danger btn-block" OnClick="btn_SaveException_Click" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row mt-3">
                                            <div class="col-md-12">
                                                <asp:GridView ID="gv_BackdateExceptions" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered" OnRowCommand="gv_BackdateExceptions_RowCommand" DataKeyNames="Employee_Workman">
                                                    <Columns>
                                                        <asp:BoundField DataField="Employee_Workman" HeaderText="Workman SL" ItemStyle-Width="15%" ItemStyle-Font-Bold="true" />
                                                        <asp:TemplateField HeaderText="Backdate Limit" ItemStyle-Width="20%">
                                                            <ItemTemplate>
                                                                <span class="badge bg-blue"><%# Eval("Max_Backdate_Days") %> Days</span>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" ItemStyle-Width="35%" />

                                                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="15%" ItemStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <span class='badge <%# Convert.ToBoolean(Eval("IsActive")) ? "bg-green" : "bg-red" %>'>
                                                                    <%# Convert.ToBoolean(Eval("IsActive")) ? "Active" : "Inactive" %>
                                                                </span>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="15%" ItemStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="btn_ToggleStatus" runat="server"
                                                                    CommandName="ToggleStatus"
                                                                    CommandArgument='<%# Eval("Employee_Workman") + "|" + Eval("IsActive") %>'
                                                                    CssClass='<%# Convert.ToBoolean(Eval("IsActive")) ? "btn btn-xs btn-outline-danger" : "btn btn-xs btn-outline-success" %>'
                                                                    OnClientClick='<%# Convert.ToBoolean(Eval("IsActive")) ? "return confirm(\"Deactivate this exception?\");" : "return confirm(\"Reactivate this exception?\");" %>'>
                                                                    <i class='<%# Convert.ToBoolean(Eval("IsActive")) ? "fa fa-ban" : "fa fa-check" %>'></i> 
                                                                    <%# Convert.ToBoolean(Eval("IsActive")) ? "Deactivate" : "Reactivate" %>
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div class="alert alert-info text-center">No exceptions defined. All users are currently on the default 2-day limit.</div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function showPNotify(title, text, type) {
            new PNotify({ title: title, text: text, type: type, styling: 'bootstrap3', delay: 4000 });
        }

        var calendar; // Global calendar object

        document.addEventListener('DOMContentLoaded', function () {
            var Calendar = FullCalendar.Calendar;
            var Draggable = FullCalendar.Draggable;

            // 1. Initialize Draggable RHS Tags
            var containerEl = document.getElementById('external-events');
            new Draggable(containerEl, {
                itemSelector: '.tag-draggable',
                eventData: function (eventEl) {
                    return {
                        title: eventEl.innerText,
                        backgroundColor: eventEl.style.backgroundColor,
                        borderColor: eventEl.style.borderColor,
                        extendedProps: {
                            tagCode: eventEl.getAttribute('data-code')
                        }
                    };
                }
            });

            // 2. Initialize Calendar
            var calendarEl = document.getElementById('smart-calendar');
            calendar = new Calendar(calendarEl, {
                initialView: 'dayGridMonth',
                droppable: true,

                // --- FIX: Force a fixed rendering size ---
                height: 600,
                contentHeight: 550,
                stickyHeaderDates: true, // Keeps the days of the week visible

                // --- NEW: Loading Spinner Hook ---
                loading: function (isLoading) {
                    var loader = document.getElementById('calendar-loading');
                    if (loader) {
                        if (isLoading) {
                            loader.style.display = 'block';
                        } else {
                            loader.style.display = 'none';
                        }
                    }
                },

                // Fetch existing rules from DB
                events: function (info, successCallback, failureCallback) {
                    var compCode = document.getElementById('<%= ddl_cal_company.ClientID %>').value;
                    if (!compCode) { successCallback([]); return; }

                    $.ajax({
                        type: "POST",
                        url: "db_controller.aspx/GetCalendarEvents",
                        data: JSON.stringify({ companyCode: compCode }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            successCallback(JSON.parse(response.d));
                        }
                    });
                },

                // Triggered when a tag is dropped on a date
                drop: function (info) {
                    var droppedDate = info.dateStr;
                    var droppedTagCode = info.draggedEl.getAttribute('data-code');
                    var companyCode = document.getElementById('<%= ddl_cal_company.ClientID %>').value;

                    if (!companyCode) {
                        showPNotify('Warning', 'Please select a company first!', 'warning');
                        info.revert();
                        return;
                    }

                    // Show spinner manually during save
                    var loader = document.getElementById('calendar-loading');
                    if (loader) loader.style.display = 'block';

                    // Save to DB
                    $.ajax({
                        type: "POST",
                        url: "db_controller.aspx/SaveCalendarEvent",
                        data: JSON.stringify({ companyCode: companyCode, calDate: droppedDate, tagCode: droppedTagCode }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            if (loader) loader.style.display = 'none';
                            if (response.d === "Success") {
                                showPNotify('Saved', 'Calendar Rule applied successfully.', 'success');
                                calendar.refetchEvents(); // Refresh grid to prevent duplicates
                            } else {
                                showPNotify('Error', response.d, 'error');
                            }
                        },
                        error: function () {
                            if (loader) loader.style.display = 'none';
                            showPNotify('Error', 'Failed to save tag.', 'error');
                        }
                    });
                },

                // --- NEW: Delete functionality on Click ---
                eventClick: function (info) {
                    // Confirm with the admin before deleting
                    if (confirm("Are you sure you want to remove the '" + info.event.title + "' rule for " + info.event.startStr + "?")) {
                        var companyCode = document.getElementById('<%= ddl_cal_company.ClientID %>').value;

                        // Show spinner manually during delete
                        var loader = document.getElementById('calendar-loading');
                        if (loader) loader.style.display = 'block';

                        $.ajax({
                            type: "POST",
                            url: "db_controller.aspx/DeleteCalendarEvent",
                            data: JSON.stringify({ companyCode: companyCode, calDate: info.event.startStr }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                if (loader) loader.style.display = 'none';
                                if (response.d === "Success") {
                                    info.event.remove(); // Visually remove from calendar instantly
                                    showPNotify('Deleted', 'Rule removed successfully.', 'success');
                                } else {
                                    showPNotify('Error', response.d, 'error');
                                }
                            },
                            error: function () {
                                if (loader) loader.style.display = 'none';
                                showPNotify('Error', 'Failed to communicate with server.', 'error');
                            }
                        });
                    }
                }
            });

            calendar.render();
        });

        // Refetch events when dropdown changes
        function loadCompanyEvents() {
            if (calendar) { calendar.refetchEvents(); }
        }

        // Refetch events when dropdown changes
        function loadCompanyEvents() {
            if (calendar) { calendar.refetchEvents(); }
        }

        // --- FIX V2: Force Calendar to resize AFTER Bootstrap animation finishes ---
        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            // Check if the clicked tab is the Smart Calendar
            var targetTab = $(e.target).attr("href");

            if (targetTab === '#tab_smart_calendar') {
                if (calendar) {
                    // Wait 200ms for Bootstrap's CSS transition to fully complete
                    setTimeout(function () {
                        calendar.updateSize(); // Forces FullCalendar to recalculate its dimensions
                        calendar.render();     // Redraw the grid
                    }, 200);
                }
            }
        });
    </script>
</asp:Content>
