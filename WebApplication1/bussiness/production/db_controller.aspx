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
                            <li class="active"><a href="#tab_region_safety" data-toggle="tab"><i class="fa fa-shield text-danger"></i>Region Safety & Compliance</a></li>
                            <li><a href="#tab_wo_config" data-toggle="tab"><i class="fa fa-briefcase text-primary"></i>Work Order Controller</a></li>
                            <li><a href="#tab_doc_master" data-toggle="tab"><i class="fa fa-file-text-o text-success"></i>Document Master</a></li>
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
                                            <div class="col-md-5">
                                                <h5 class="text-info"><i class="fa fa-toggle-on"></i>Region Toggles</h5>
                                                <div class="form-group">
                                                    <label>Require GPS Tagging (Create JOB)?</label>
                                                    <asp:DropDownList ID="ddl_req_gps" runat="server" CssClass="form-control form-control-sm w-50">
                                                        <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                        <asp:ListItem Value="No">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="form-group">
                                                    <label>Require Strict TBT Number?</label>
                                                    <asp:DropDownList ID="ddl_req_tbt" runat="server" CssClass="form-control form-control-sm w-50">
                                                        <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                        <asp:ListItem Value="No">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="col-md-7">
                                                <h5 class="text-danger"><i class="fa fa-check-square-o"></i>Mandatory CSM / Permit Documents</h5>
                                                <p class="text-muted small">Select documents that supervisors MUST upload for this region before IN/OUT punch.</p>
                                                <div class="well well-sm" style="max-height: 250px; overflow-y: auto;">
                                                    <asp:CheckBoxList ID="cbl_RegionDocs" runat="server" RepeatColumns="2" CellPadding="5" CellSpacing="5" CssClass="table table-borderless table-sm"></asp:CheckBoxList>
                                                </div>
                                            </div>

                                            <div class="col-md-12 mt-3 text-right">
                                                <asp:Button ID="btn_SaveRegion" runat="server" Text="Update Region Compliance" CssClass="btn btn-success" OnClick="btn_SaveRegion_Click" />
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
                                                    <asp:ListItem Value="Manpower">Manpower</asp:ListItem>
                                                    <asp:ListItem Value="Equipment">Equipment</asp:ListItem>
                                                    <asp:ListItem Value="Turnkey">Turnkey</asp:ListItem>
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

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        function showPNotify(title, text, type) {
            new PNotify({ title: title, text: text, type: type, styling: 'bootstrap3', delay: 4000 });
        }
    </script>
</asp:Content>
