<%@ Page Title="DB Controller" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="db_controller.aspx.cs" Inherits="WebApplication1.bussiness.production.db_controller" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /* Base typography and layout */
        .top-label {
            font-weight: 600;
            color: #2a3f54;
            font-size: 13px;
            letter-spacing: 0.3px;
            margin-bottom: 5px;
            display: block;
        }

        /* Modern Tabs Styling */
        .nav-tabs-custom {
            margin-bottom: 20px;
            background: transparent;
            box-shadow: none;
        }

            .nav-tabs-custom > .nav-tabs {
                border-bottom-color: #e9ecef;
                display: flex;
                flex-wrap: wrap;
                background: #ffffff;
                border-radius: 8px 8px 0 0;
                padding: 10px 10px 0 10px;
            }

                .nav-tabs-custom > .nav-tabs > li {
                    margin-bottom: -1px;
                }

                    .nav-tabs-custom > .nav-tabs > li > a {
                        color: #5A738E;
                        font-weight: 600;
                        border-radius: 6px 6px 0 0;
                        padding: 12px 20px;
                        border: 1px solid transparent;
                        margin-right: 5px;
                        transition: all 0.2s ease;
                    }

                        .nav-tabs-custom > .nav-tabs > li > a:hover {
                            color: #2a3f54;
                            background-color: #f8f9fa;
                            border-color: #e9ecef #e9ecef #ddd;
                        }

                    .nav-tabs-custom > .nav-tabs > li.active > a,
                    .nav-tabs-custom > .nav-tabs > li.active > a:hover,
                    .nav-tabs-custom > .nav-tabs > li.active > a:focus {
                        color: #1ABB9C;
                        background-color: #fff;
                        border-color: #e9ecef #e9ecef transparent;
                        border-top: 3px solid #1ABB9C;
                    }

        /* Tab Content Area */
        .tab-pane {
            padding: 25px;
            background: #fff;
            border: 1px solid #e9ecef;
            border-top: none;
            border-radius: 0 0 8px 8px;
            box-shadow: 0 4px 10px rgba(0,0,0,0.03);
        }

        /* Modern Control Panel Wrapper */
        .control-panel {
            background: #fdfdfe;
            border-radius: 8px;
            padding: 20px;
            margin-bottom: 25px;
            border: 1px solid #e9ecef;
            box-shadow: 0 2px 4px rgba(0,0,0,0.02);
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

        /* Standard Buttons */
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

        /* GridView Container & Mobile Scroll */
        .modern-grid-container {
            border: 1px solid #e9ecef;
            border-radius: 8px;
            background: #fff;
            margin-bottom: 20px;
        }

        .modern-table-wrapper {
            overflow-x: auto;
            -webkit-overflow-scrolling: touch;
            width: 100%;
            border-radius: 8px;
        }

        .modern-grid-container th {
            background-color: #f8f9fa !important;
            color: #34495e !important;
            font-weight: 700;
            border: 1px solid #e9ecef !important;
            padding: 12px 8px !important;
            white-space: nowrap;
        }

        .modern-grid-container td {
            vertical-align: middle !important;
            padding: 10px 8px !important;
            border: 1px solid #e9ecef !important;
        }

        .trigger-check {
            font-weight: 600;
            color: #2a3f54;
        }

        .trigger-check input[type="checkbox"] {
            margin-right: 6px;
            transform: scale(1.2);
            vertical-align: middle;
        }
    </style>
    <link href='https://cdn.jsdelivr.net/npm/fullcalendar@5.11.3/main.min.css' rel='stylesheet' />
    <script src='https://cdn.jsdelivr.net/npm/fullcalendar@5.11.3/main.min.js'></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="container-fluid pl-0 pr-0">
            <div class="page-title mb-3 border-bottom pb-2">
                <div class="title_left">
                    <h3 style="color: #2a3f54; font-weight: 600;"><i class="fa fa-database text-primary mr-2" style="color: #1ABB9C !important;"></i>DB Controller <small style="font-size: 14px; color: #73879C; font-weight: 500;">Master Data Management</small></h3>
                </div>
            </div>
            <div class="clearfix"></div>

            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

            <div class="row">
                <div class="col-md-12">
                    <div class="nav-tabs-custom">
                        <ul class="nav nav-tabs">
                            <li class="active"><a href="#tab_region_safety" data-toggle="tab"><i class="fa fa-shield text-danger mr-1"></i>Work Region</a></li>
                            <li><a href="#tab_wo_config" data-toggle="tab"><i class="fa fa-briefcase text-primary mr-1"></i>WO/PO Controller</a></li>
                            <li><a href="#tab_wo_matrix" data-toggle="tab"><i class="fa fa-cogs text-warning mr-1"></i>WO Rule Matrix</a></li>
                            <li><a href="#tab_doc_master" data-toggle="tab"><i class="fa fa-file-text-o text-success mr-1"></i>Document Master</a></li>
                            <li><a href="#tab_smart_calendar" data-toggle="tab"><i class="fa fa-calendar text-info mr-1"></i>Smart Calendar</a></li>
                            <li><a href="#tab_backdate" data-toggle="tab"><i class="fa fa-history text-danger mr-1"></i>Backdate Exceptions</a></li>
                            <li><a href="#tab_triggers" data-toggle="tab"><i class="fa fa-bell text-success mr-1"></i>Notification Triggers</a></li>
                        </ul>

                        <div class="tab-content">

                            <div class="tab-pane active" id="tab_region_safety">
                                <asp:UpdatePanel ID="upRegion" runat="server">
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-md-5">
                                                <div class="control-panel">
                                                    <label class="top-label">Select Target Region <span class="text-danger">*</span></label>
                                                    <asp:DropDownList ID="ddl_Regions" runat="server" CssClass="form-control modern-input" AutoPostBack="true" OnSelectedIndexChanged="ddl_Regions_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row" id="RegionConfigRow" runat="server" visible="false">
                                            <div class="col-md-6 mb-3">
                                                <div class="control-panel h-100 mb-0">
                                                    <h5 style="color: #3498DB; font-weight: 600; margin-bottom: 15px;"><i class="fa fa-map-marker mr-2"></i>Location Constraints</h5>
                                                    <div style="background-color: #f8f9fa; padding: 15px; border-radius: 6px; border: 1px solid #e9ecef;">
                                                        <div class="form-group mb-0">
                                                            <label class="top-label">Require GPS Tagging for JOB Creation?</label>
                                                            <asp:DropDownList ID="ddl_req_gps" runat="server" CssClass="form-control modern-input w-50 mt-1">
                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <small class="text-muted d-block mt-2"><i class="fa fa-info-circle"></i>If set to Yes, users must allow browser geolocation to create a job in this region.</small>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-6 mb-3">
                                                <div class="control-panel h-100 mb-0">
                                                    <h5 style="color: #1ABB9C; font-weight: 600; margin-bottom: 15px;"><i class="fa fa-money mr-2"></i>Allowed Billing Types</h5>
                                                    <p class="small text-muted mb-2">Select which JOB Types are permitted for this Region.</p>
                                                    <div style="background-color: #f8f9fa; padding: 15px; border-radius: 6px; border: 1px solid #e9ecef;">
                                                        <asp:CheckBoxList ID="cbl_BillingTypes" runat="server" RepeatColumns="1" CellPadding="5" CellSpacing="5" CssClass="table table-borderless table-sm mb-0"></asp:CheckBoxList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-12 mt-3 text-right border-top pt-3">
                                                <asp:Button ID="btn_SaveRegion" runat="server" Text="Update Region Rules" CssClass="btn btn-success btn-modern" OnClick="btn_SaveRegion_Click" />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div class="tab-pane" id="tab_wo_config">
                                <asp:UpdatePanel ID="upWO" runat="server">
                                    <ContentTemplate>
                                        <div class="control-panel">
                                            <h5 style="color: #2a3f54; font-weight: 600; margin-bottom: 20px;"><i class="fa fa-briefcase mr-2"></i>Configure Work Order Parameters</h5>
                                            <div class="row">
                                                <div class="col-md-3 col-sm-6 form-group">
                                                    <label class="top-label">1. Region</label>
                                                    <asp:DropDownList ID="ddl_wo_region" runat="server" CssClass="form-control modern-input" AutoPostBack="true" OnSelectedIndexChanged="ddl_wo_region_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                                <div class="col-md-3 col-sm-6 form-group">
                                                    <label class="top-label">2. Company</label>
                                                    <asp:DropDownList ID="ddl_wo_company" runat="server" CssClass="form-control modern-input" AutoPostBack="true" OnSelectedIndexChanged="ddl_wo_company_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                                <div class="col-md-3 col-sm-6 form-group">
                                                    <label class="top-label">3. Department</label>
                                                    <asp:DropDownList ID="ddl_wo_dept" runat="server" CssClass="form-control modern-input" AutoPostBack="true" OnSelectedIndexChanged="ddl_wo_dept_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                                <div class="col-md-3 col-sm-6 form-group">
                                                    <label class="top-label">4. Work Order No.</label>
                                                    <asp:DropDownList ID="ddl_wo_number" runat="server" CssClass="form-control modern-input" AutoPostBack="true" OnSelectedIndexChanged="ddl_wo_number_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-panel" id="WOConfigRow" runat="server" visible="false" style="background-color: #f0f9f6; border: 1px solid #c8e6c9;">
                                            <div class="row align-items-end">
                                                <div class="col-md-3 form-group mb-md-0">
                                                    <label class="top-label">Contract Nature</label>
                                                    <asp:DropDownList ID="ddl_contract_nature" runat="server" CssClass="form-control modern-input">
                                                        <asp:ListItem Value="ARC">ARC</asp:ListItem>
                                                        <asp:ListItem Value="Lumpsum">Lumpsum</asp:ListItem>
                                                        <asp:ListItem Value="Supply">Supply</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-3 form-group mb-md-0">
                                                    <label class="top-label">Billing Nature</label>
                                                    <asp:DropDownList ID="ddl_billing_nature" runat="server" CssClass="form-control modern-input">
                                                        <asp:ListItem Value="Billing">Billing</asp:ListItem>
                                                        <asp:ListItem Value="Non-Billing">Non-Billing</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-3 form-group mb-md-0">
                                                    <label class="top-label">Execution Type</label>
                                                    <asp:DropDownList ID="ddl_execution_type" runat="server" CssClass="form-control modern-input">
                                                        <asp:ListItem Value="Manpower Supply">Manpower Supply</asp:ListItem>
                                                        <asp:ListItem Value="Equipment">Equipment</asp:ListItem>
                                                        <asp:ListItem Value="Turnkey">Turnkey</asp:ListItem>
                                                        <asp:ListItem Value="Line Items JOB">Line Items JOB</asp:ListItem>
                                                        <asp:ListItem Value="One Time">One Time</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-3 text-right mt-3 mt-md-0">
                                                    <asp:Button ID="btn_SaveWO" runat="server" Text="Save Parameters" CssClass="btn btn-primary btn-modern btn-block" OnClick="btn_SaveWO_Click" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row mt-4">
                                            <div class="col-md-12">
                                                <h6 style="color: #2a3f54; font-weight: 600; border-bottom: 1px solid #e9ecef; padding-bottom: 10px;">
                                                    <i class="fa fa-list mr-2"></i>Active Work Order Configurations
                                                </h6>
                                                <div class="modern-grid-container">
                                                    <div class="modern-table-wrapper">
                                                        <asp:GridView ID="gv_WO_Summary" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-sm mb-0" GridLines="Both">
                                                            <Columns>
                                                                <asp:BoundField DataField="Company_Name" HeaderText="Company" />
                                                                <asp:BoundField DataField="Department_Name" HeaderText="Department" />
                                                                <asp:BoundField DataField="WO_Number" HeaderText="WO Number" ItemStyle-Font-Bold="true" />
                                                                <asp:BoundField DataField="Contract_Nature" HeaderText="Contract Nature" />
                                                                <asp:BoundField DataField="Billing_Nature" HeaderText="Billing Nature" />
                                                                <asp:BoundField DataField="Execution_Type" HeaderText="Execution Type" />
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <div class="alert alert-info text-center m-3">No Active Work Orders Found.</div>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div class="tab-pane" id="tab_wo_matrix">
                                <asp:UpdatePanel ID="upMatrix" runat="server">
                                    <ContentTemplate>
                                        <div class="control-panel">
                                            <h5 style="color: #f39c12; font-weight: 600;"><i class="fa fa-sliders mr-2"></i>Define Work Order Rules</h5>
                                            <p class="text-muted small mb-4">Set up the required UI behaviors for specific combinations of Billing Nature and Execution Type.</p>

                                            <div class="row align-items-center">
                                                <div class="col-md-2 col-sm-6 form-group">
                                                    <label class="top-label">Billing Nature <span class="text-danger">*</span></label>
                                                    <asp:DropDownList ID="ddl_matrix_billing" runat="server" CssClass="form-control modern-input">
                                                        <asp:ListItem Value="Billing">Billing</asp:ListItem>
                                                        <asp:ListItem Value="Non-Billing">Non-Billing</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-2 col-sm-6 form-group">
                                                    <label class="top-label">Execution Type <span class="text-danger">*</span></label>
                                                    <asp:DropDownList ID="ddl_matrix_execution" runat="server" CssClass="form-control modern-input">
                                                        <asp:ListItem Value="Manpower Supply">Manpower Supply</asp:ListItem>
                                                        <asp:ListItem Value="Equipment">Equipment</asp:ListItem>
                                                        <asp:ListItem Value="Turnkey">Turnkey</asp:ListItem>
                                                        <asp:ListItem Value="Line Items JOB">Line Items JOB</asp:ListItem>
                                                        <asp:ListItem Value="One Time">One Time</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>

                                                <div class="col-md-2 col-sm-12 form-group">
                                                    <label class="top-label">Routing Destination <span class="text-danger">*</span></label>
                                                    <asp:DropDownList ID="ddl_matrix_routing" runat="server" CssClass="form-control modern-input">
                                                        <asp:ListItem Value="1">1 - Permit Upload</asp:ListItem>
                                                        <asp:ListItem Value="3">3 - Direct In-Punch</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>

                                                <div class="col-md-4 col-sm-12 form-group">
                                                    <label class="top-label">Enforced Rules (Check to Require)</label>
                                                    <div style="background-color: #f8f9fa; padding: 10px 15px; border-radius: 6px; border: 1px solid #e9ecef;">
                                                        <asp:CheckBox ID="chk_req_permit" runat="server" Text=" Permit No." CssClass="mr-2" Font-Bold="true" Checked="true" />
                                                        <asp:CheckBox ID="chk_req_csm" runat="server" Text=" CSM Docs" CssClass="mr-2" Font-Bold="true" Checked="true" />
                                                        <asp:CheckBox ID="chk_req_attendance" runat="server" Text=" Attendance" Font-Bold="true" Checked="true" />
                                                        <div class="w-100 mt-2 mb-2" style="border-top: 1px solid #ddd;"></div>
                                                        <asp:CheckBox ID="chk_auto_title" runat="server" Text=" Auto-Generate Title" Font-Bold="true" ForeColor="#2980b9" />
                                                    </div>
                                                </div>
                                                <div class="col-md-2 col-sm-12 form-group mt-md-4">
                                                    <asp:Button ID="btn_SaveMatrix" runat="server" Text="Save Rule" CssClass="btn btn-warning btn-modern btn-block text-white" OnClick="btn_SaveMatrix_Click" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row mt-4">
                                            <div class="col-md-12">
                                                <div class="modern-grid-container">
                                                    <div class="modern-table-wrapper">
                                                        <asp:GridView ID="gv_RuleMatrix" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-sm mb-0" GridLines="Both">
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
                                                                        <span class='badge <%# Eval("Default_MasterStatusCode").ToString() == "3" ? "bg-purple" : "bg-blue" %>' style="font-size: 12px; padding: 4px 8px;">
                                                                            <%# Eval("Default_MasterStatusCode").ToString() == "3" ? "3 - Direct In-Punch" : "1 - Permit Upload" %>
                                                                        </span>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <div class="alert alert-info text-center m-3">No Matrix Rules have been defined yet.</div>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div class="tab-pane" id="tab_doc_master">
                                <asp:UpdatePanel ID="upDoc" runat="server">
                                    <ContentTemplate>
                                        <div class="control-panel">
                                            <h5 style="color: #27ae60; font-weight: 600; margin-bottom: 15px;"><i class="fa fa-file-text-o mr-2"></i>Add New Document Requirement</h5>
                                            <div class="row align-items-end">
                                                <div class="col-md-3 col-sm-6 form-group mb-md-0 mb-3">
                                                    <label class="top-label">Category</label>
                                                    <asp:DropDownList ID="ddl_newdoc_category" runat="server" CssClass="form-control modern-input">
                                                        <asp:ListItem Value="CSM">CSM Document</asp:ListItem>
                                                        <asp:ListItem Value="Permit">Safety Permit</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-6 col-sm-6 form-group mb-md-0 mb-3">
                                                    <label class="top-label">Document Name</label>
                                                    <asp:TextBox ID="txt_newdoc_name" runat="server" CssClass="form-control modern-input" placeholder="e.g., Electrical Safety Clearance"></asp:TextBox>
                                                </div>
                                                <div class="col-md-3 col-sm-12 form-group mb-md-0">
                                                    <asp:Button ID="btn_AddDoc" runat="server" Text="Add to Dictionary" CssClass="btn btn-dark btn-modern btn-block" OnClick="btn_AddDoc_Click" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row mt-4">
                                            <div class="col-md-12">
                                                <div class="modern-grid-container">
                                                    <div class="modern-table-wrapper">
                                                        <asp:GridView ID="gv_DocMaster" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-sm mb-0" DataKeyNames="Doc_ID" GridLines="Both">
                                                            <Columns>
                                                                <asp:BoundField DataField="Doc_ID" HeaderText="ID" ItemStyle-Width="5%" ItemStyle-CssClass="text-center" />
                                                                <asp:BoundField DataField="Doc_Category" HeaderText="Category" ItemStyle-Width="15%" ItemStyle-Font-Bold="true" />
                                                                <asp:BoundField DataField="Doc_Name" HeaderText="Document Name" />
                                                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="10%" ItemStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <span class='badge <%# Convert.ToBoolean(Eval("IsActive")) ? "bg-green" : "bg-red" %>'>
                                                                            <%# Convert.ToBoolean(Eval("IsActive")) ? "Active" : "Disabled" %>
                                                                        </span>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div class="tab-pane" id="tab_smart_calendar">
                                <div class="row mb-3">
                                    <div class="col-md-4 col-sm-6">
                                        <label class="top-label">1. Select Company to Manage</label>
                                        <asp:DropDownList ID="ddl_cal_company" runat="server" CssClass="form-control modern-input" onchange="loadCompanyEvents();"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-9 col-sm-12 mb-3">
                                        <div class="control-panel" style="position: relative; min-height: 650px; padding: 15px;">
                                            <div id="calendar-loading" style="display: none; position: absolute; top: 0; left: 0; width: 100%; height: 100%; background: rgba(255,255,255,0.85); z-index: 10; text-align: center; padding-top: 20%; border-radius: 8px;">
                                                <i class="fa fa-spinner fa-spin fa-3x" style="color: #1ABB9C;"></i>
                                                <h4 class="mt-3" style="color: #2a3f54; font-weight: 600;">Syncing Calendar...</h4>
                                            </div>
                                            <div id='smart-calendar'></div>
                                        </div>
                                    </div>

                                    <div class="col-md-3 col-sm-12">
                                        <div class="control-panel" id="external-events">
                                            <h5 style="color: #3498DB; font-weight: 600;"><i class="fa fa-tags mr-2"></i>Drag & Drop Tags</h5>
                                            <p class="small text-muted mb-3">Drag a tag onto a date to set the Attendance Rule for this company.</p>

                                            <div class='fc-event tag-draggable' data-code='FL' style='background-color: #e74c3c; border-color: #e74c3c; padding: 12px; margin-bottom: 10px; color: white; cursor: grab; border-radius: 6px; font-weight: 600; box-shadow: 0 2px 4px rgba(231, 76, 60, 0.2);'>
                                                <i class="fa fa-star mr-2"></i>Festival Leave (FL)
                                            </div>
                                            <div class='fc-event tag-draggable' data-code='OD' style='background-color: #3498db; border-color: #3498db; padding: 12px; margin-bottom: 10px; color: white; cursor: grab; border-radius: 6px; font-weight: 600; box-shadow: 0 2px 4px rgba(52, 152, 219, 0.2);'>
                                                <i class="fa fa-bed mr-2"></i>Weekly Off (OD)
                                            </div>
                                            <div class='fc-event tag-draggable' data-code='NH' style='background-color: #9b59b6; border-color: #9b59b6; padding: 12px; margin-bottom: 10px; color: white; cursor: grab; border-radius: 6px; font-weight: 600; box-shadow: 0 2px 4px rgba(155, 89, 182, 0.2);'>
                                                <i class="fa fa-flag mr-2"></i>National Holiday (NH)
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="tab-pane" id="tab_backdate">
                                <asp:UpdatePanel ID="upBackdate" runat="server">
                                    <ContentTemplate>
                                        <div class="control-panel">
                                            <h5 style="color: #E74C3C; font-weight: 600; margin-bottom: 10px;"><i class="fa fa-history mr-2"></i>Backdate Limit Exceptions</h5>
                                            <p class="text-muted small mb-4"><strong>Global Default:</strong> All users are restricted to 2 days of backdating. Use this form to grant operational exceptions.</p>

                                            <div class="row align-items-end">
                                                <div class="col-md-3 col-sm-6 form-group mb-md-0 mb-3">
                                                    <label class="top-label">Workman SL <span class="text-danger">*</span></label>
                                                    <asp:TextBox ID="txt_exc_workman" runat="server" CssClass="form-control modern-input" placeholder="e.g. 104523"></asp:TextBox>
                                                </div>
                                                <div class="col-md-2 col-sm-6 form-group mb-md-0 mb-3">
                                                    <label class="top-label">Max Backdate <span class="text-danger">*</span></label>
                                                    <asp:DropDownList ID="ddl_exc_days" runat="server" CssClass="form-control modern-input">
                                                        <asp:ListItem Value="3">3 Days</asp:ListItem>
                                                        <asp:ListItem Value="5">5 Days</asp:ListItem>
                                                        <asp:ListItem Value="15">15 Days</asp:ListItem>
                                                        <asp:ListItem Value="30">30 Days</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-5 col-sm-12 form-group mb-md-0 mb-3">
                                                    <label class="top-label">Remarks / Reason <span class="text-danger">*</span></label>
                                                    <asp:TextBox ID="txt_exc_remarks" runat="server" CssClass="form-control modern-input" placeholder="e.g. Approved by HR (Ticket #123)"></asp:TextBox>
                                                </div>
                                                <div class="col-md-2 col-sm-12 form-group mb-0">
                                                    <asp:Button ID="btn_SaveException" runat="server" Text="Save Rule" CssClass="btn btn-danger btn-modern btn-block" OnClick="btn_SaveException_Click" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row mt-4">
                                            <div class="col-md-12">
                                                <div class="modern-grid-container">
                                                    <div class="modern-table-wrapper">
                                                        <asp:GridView ID="gv_BackdateExceptions" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-sm mb-0" OnRowCommand="gv_BackdateExceptions_RowCommand" DataKeyNames="Employee_Workman" GridLines="Both">
                                                            <Columns>
                                                                <asp:BoundField DataField="Employee_Workman" HeaderText="Workman SL" ItemStyle-Width="15%" ItemStyle-Font-Bold="true" ItemStyle-CssClass="text-center" />
                                                                <asp:TemplateField HeaderText="Backdate Limit" ItemStyle-Width="15%" ItemStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <span class="badge bg-blue" style="font-size: 13px; padding: 4px 8px;"><i class="fa fa-calendar-minus-o mr-1"></i><%# Eval("Max_Backdate_Days") %> Days</span>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" ItemStyle-Width="40%" />

                                                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="10%" ItemStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <span class='badge <%# Convert.ToBoolean(Eval("IsActive")) ? "bg-green" : "bg-red" %>'>
                                                                            <%# Convert.ToBoolean(Eval("IsActive")) ? "Active" : "Inactive" %>
                                                                        </span>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="20%" ItemStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="btn_ToggleStatus" runat="server"
                                                                            CommandName="ToggleStatus"
                                                                            CommandArgument='<%# Eval("Employee_Workman") + "|" + Eval("IsActive") %>'
                                                                            CssClass='<%# Convert.ToBoolean(Eval("IsActive")) ? "btn btn-xs btn-outline-danger m-0" : "btn btn-xs btn-outline-success m-0" %>'
                                                                            OnClientClick='<%# Convert.ToBoolean(Eval("IsActive")) ? "return confirm(\"Deactivate this exception?\");" : "return confirm(\"Reactivate this exception?\");" %>'
                                                                            Style="border-radius: 12px; font-weight: 600;">
                                                                            <i class='<%# Convert.ToBoolean(Eval("IsActive")) ? "fa fa-ban" : "fa fa-check" %>'></i> 
                                                                            <%# Convert.ToBoolean(Eval("IsActive")) ? "Deactivate" : "Reactivate" %>
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <div class="alert alert-info text-center m-3">No exceptions defined. All users are currently on the default 2-day limit.</div>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div class="tab-pane" id="tab_triggers">
                                <asp:UpdatePanel ID="upTriggers" runat="server">
                                    <ContentTemplate>
                                        <div class="alert alert-warning" id="pnl_triggers_missing" runat="server" visible="false">
                                            <i class="fa fa-warning mr-1"></i>
                                            Notification trigger table was not found. Run <code>scripts/add_notification_triggers.sql</code> on this database, then reload this page.
                                        </div>

                                        <div id="pnl_triggers_ready" runat="server" visible="false">
                                            <div class="control-panel" style="background-color: #f0f9f6; border: 1px solid #c8e6c9;">
                                                <h5 style="color: #1ABB9C; font-weight: 600; margin-bottom: 8px;"><i class="fa fa-globe mr-2"></i>Portal level</h5>
                                                <p class="small text-muted mb-3">Portal Off turns off notification channels (jobs, helpdesk, payroll, and similar). Authentication OTPs stay active unless you disable those modules below. Changes save when you tick or untick a box.</p>
                                                <div class="row align-items-end">
                                                    <div class="col-md-3 col-sm-6 form-group mb-md-0">
                                                        <label class="top-label">Email</label>
                                                        <asp:CheckBox ID="chk_portal_email" runat="server" Text=" Enabled" CssClass="trigger-check" AutoPostBack="true" OnCheckedChanged="chk_Portal_CheckedChanged" />
                                                    </div>
                                                    <div class="col-md-3 col-sm-6 form-group mb-md-0">
                                                        <label class="top-label">WhatsApp</label>
                                                        <asp:CheckBox ID="chk_portal_whatsapp" runat="server" Text=" Enabled" CssClass="trigger-check" AutoPostBack="true" OnCheckedChanged="chk_Portal_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-panel">
                                                <h5 style="color: #2a3f54; font-weight: 600; margin-bottom: 8px;"><i class="fa fa-lock mr-2"></i>Authentication OTP</h5>
                                                <p class="small text-muted mb-3">Active by default. Login MFA, password reset, and profile verification codes are not turned off by Portal Off. Changes save when you tick or untick a box.</p>
                                                <div class="modern-grid-container">
                                                    <div class="modern-table-wrapper">
                                                        <asp:GridView ID="gv_TriggerAuth" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-sm mb-0" GridLines="Both" DataKeyNames="TriggerKey">
                                                            <Columns>
                                                                <asp:BoundField DataField="DisplayName" HeaderText="OTP" ItemStyle-Font-Bold="true" />
                                                                <asp:TemplateField HeaderText="Email" ItemStyle-Width="18%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chk_row_email" runat="server" CssClass="trigger-check" AutoPostBack="true" OnCheckedChanged="chk_TriggerRow_CheckedChanged" Checked='<%# Convert.ToBoolean(Eval("EmailEnabled")) %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="WhatsApp" ItemStyle-Width="18%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chk_row_whatsapp" runat="server" CssClass="trigger-check" AutoPostBack="true" OnCheckedChanged="chk_TriggerRow_CheckedChanged" Checked='<%# Convert.ToBoolean(Eval("WhatsAppEnabled")) %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <div class="alert alert-info text-center m-3">No authentication OTP rows found.</div>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-panel">
                                                <h5 style="color: #2a3f54; font-weight: 600; margin-bottom: 8px;"><i class="fa fa-cubes mr-2"></i>Notification modules</h5>
                                                <p class="small text-muted mb-3">A notification send goes out only when Portal and the module are both Enabled for that channel. Changes save when you tick or untick a box.</p>
                                                <div class="modern-grid-container">
                                                    <div class="modern-table-wrapper">
                                                        <asp:GridView ID="gv_TriggerModules" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-sm mb-0" GridLines="Both" DataKeyNames="TriggerKey">
                                                            <Columns>
                                                                <asp:BoundField DataField="DisplayName" HeaderText="Module" ItemStyle-Font-Bold="true" />
                                                                <asp:TemplateField HeaderText="Email" ItemStyle-Width="18%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chk_row_email" runat="server" CssClass="trigger-check" AutoPostBack="true" OnCheckedChanged="chk_TriggerRow_CheckedChanged" Checked='<%# Convert.ToBoolean(Eval("EmailEnabled")) %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="WhatsApp" ItemStyle-Width="18%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chk_row_whatsapp" runat="server" CssClass="trigger-check" AutoPostBack="true" OnCheckedChanged="chk_TriggerRow_CheckedChanged" Checked='<%# Convert.ToBoolean(Eval("WhatsAppEnabled")) %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <div class="alert alert-info text-center m-3">No module trigger rows found.</div>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
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
        </div>
    </div>

    <script type="text/javascript">
        function showPNotify(title, text, type) {
            new PNotify({ title: title, text: text, type: type, styling: 'bootstrap3', delay: 4000 });
        }

        function showTriggerTab() {
            var tab = document.querySelector('.nav-tabs a[href="#tab_triggers"]');
            if (tab) {
                $(tab).tab('show');
            }
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

                height: 600,
                contentHeight: 550,
                stickyHeaderDates: true,

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

                drop: function (info) {
                    var droppedDate = info.dateStr;
                    var droppedTagCode = info.draggedEl.getAttribute('data-code');
                    var companyCode = document.getElementById('<%= ddl_cal_company.ClientID %>').value;

                    if (!companyCode) {
                        showPNotify('Warning', 'Please select a company first!', 'warning');
                        info.revert();
                        return;
                    }

                    var loader = document.getElementById('calendar-loading');
                    if (loader) loader.style.display = 'block';

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
                                calendar.refetchEvents();
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

                eventClick: function (info) {
                    if (confirm("Are you sure you want to remove the '" + info.event.title + "' rule for " + info.event.startStr + "?")) {
                        var companyCode = document.getElementById('<%= ddl_cal_company.ClientID %>').value;
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
                                    info.event.remove();
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

        // Force Calendar to resize AFTER Bootstrap animation finishes
        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            var targetTab = $(e.target).attr("href");
            if (targetTab === '#tab_smart_calendar') {
                if (calendar) {
                    setTimeout(function () {
                        calendar.updateSize();
                        calendar.render();
                    }, 200);
                }
            }
        });
    </script>
</asp:Content>
