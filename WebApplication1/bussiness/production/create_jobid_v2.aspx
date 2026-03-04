<%@ Page Title="Create JOBID V2" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="create_jobid_v2.aspx.cs" Inherits="WebApplication1.bussiness.production.create_jobid_v2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .top-label { font-weight: 600; margin-bottom: 5px; color: #333; }
        .req-star { color: red; }
        .doc-list label { margin-left: 5px; font-weight: normal; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="container">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Step 1: Create JOB ID <small>Smart Workflow</small></h2>
                            <div class="clearfix"></div>
                        </div>

                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="x_content bg-light p-2 mb-3" style="border-radius: 5px;">
                                    <div class="row" id="Div1" runat="server" visible="true">
                                        <div class="col-md-6 col-sm-12 text-center" style="vertical-align: middle;">
                                            <asp:Button ID="btn_dateswap" runat="server" Text="Today" CssClass="btn btn-success btn-sm" OnClick="btn_dateswap_Click" CausesValidation="false" />
                                        </div>
                                        <div class="col-md-6 col-sm-12 text-center" style="vertical-align: middle; font-size: 16px;">
                                            <span style="font-weight: bold; color: darkblue;">Date: </span>
                                            <asp:Label ID="lbl_jobdate" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label> | 
                                            <span style="font-weight: bold; color: darkblue;">Day: </span>
                                            <asp:Label ID="lbl_jobday" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="x_content">
                                    <div id="jobid_creation" runat="server" visible="true">
                                        
                                        <div class="row">
                                            <div class="col-md-3 col-sm-6 col-xs-12 form-group">
                                                <label class="top-label">Work Region <span class="req-star">*</span></label>
                                                <asp:DropDownList ID="DDL_Region" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Region_SelectedIndexChanged"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ValidationGroup="Submit" runat="server" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required" ControlToValidate="DDL_Region" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
                                                
                                                <div id="div_gps_status" runat="server" visible="false" class="mt-1 small">
                                                    <span id="gps_indicator" class="text-warning"><i class="fa fa-spinner fa-spin"></i> Acquiring GPS...</span>
                                                </div>
                                                <asp:HiddenField ID="hf_latitude" runat="server" />
                                                <asp:HiddenField ID="hf_longitude" runat="server" />
                                            </div>

                                            <div class="col-md-3 col-sm-6 col-xs-12 form-group">
                                                <label class="top-label">Work Order <span class="req-star">*</span></label>
                                                <asp:DropDownList ID="DDL_Workorder" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Workorder_SelectedIndexChanged"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="Submit" runat="server" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required" ControlToValidate="DDL_Workorder" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
                                                
                                                <div class="mt-1" id="div_wo_badges" runat="server" visible="false">
                                                    <asp:Label ID="lbl_ContractNature" runat="server" CssClass="badge bg-blue"></asp:Label>
                                                    <asp:Label ID="lbl_BillingNature" runat="server" CssClass="badge bg-green"></asp:Label>
                                                </div>
                                            </div>

                                            <div class="col-md-3 col-sm-6 col-xs-12 form-group">
                                                <label class="top-label">Work-Site <span class="req-star">*</span></label>
                                                <asp:DropDownList ID="DDL_Worksite" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Worksite_SelectedIndexChanged"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Submit" runat="server" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required" ControlToValidate="DDL_Worksite" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="col-md-3 col-sm-6 col-xs-12 form-group">
                                                <label class="top-label">Approver (Site Incharge) <span class="req-star">*</span></label>
                                                <asp:DropDownList ID="DDL_Approver" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ValidationGroup="Submit" runat="server" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required" ControlToValidate="DDL_Approver" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>

                                        <div class="row mt-2">
                                            <div class="col-md-3 col-sm-6 col-xs-12 form-group" id="div_BillingType" runat="server">
                                                <label class="top-label">JOB Type (Billing) <span class="req-star">*</span></label>
                                                <asp:DropDownList ID="DDL_BillingType" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RFV1" ValidationGroup="Submit" runat="server" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required" ControlToValidate="DDL_BillingType" InitialValue="Please Select Option"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="col-md-6 col-sm-12 col-xs-12 form-group" id="div_NonBillingAlert" runat="server" visible="false">
                                                <div class="alert alert-info" style="padding: 6px; margin-bottom: 0;">
                                                    <i class="fa fa-info-circle"></i> <strong>Non-Billing Job:</strong> Permit upload will be bypassed.
                                                </div>
                                            </div>

                                            <div class="col-md-3 col-sm-6 col-xs-12 form-group">
                                                <label class="top-label">Attendance Code <span class="req-star">*</span></label>
                                                <asp:DropDownList ID="DDL_AttenCode" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                            </div>

                                            <div class="col-md-3 col-sm-6 col-xs-12 form-group">
                                                <label class="top-label">Location <span class="req-star">*</span></label>
                                                <asp:DropDownList ID="DDL_Location" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="row mt-2">
                                            <div class="col-md-3 col-sm-6 col-xs-12 form-group">
                                                <label class="top-label">Work Permit No <span class="req-star">*</span></label>
                                                <asp:TextBox ID="txt_permitno" runat="server" MaxLength="100" CssClass="form-control form-control-sm rounded"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="Submit" runat="server" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required" ControlToValidate="txt_permitno"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="col-md-3 col-sm-6 col-xs-12 form-group">
                                                <label class="top-label">JOB Shift <span class="req-star">*</span></label>
                                                <asp:TextBox ID="txt_jobshift" runat="server" CssClass="form-control form-control-sm rounded" MaxLength="1" placeholder="A, B, C, G"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="Submit" runat="server" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required" ControlToValidate="txt_jobshift"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="col-md-6 col-sm-12 col-xs-12 form-group">
                                                <label class="top-label">JOB Title <span class="req-star">*</span></label>
                                                <asp:TextBox ID="txt_jobtitle" runat="server" CssClass="form-control form-control-sm rounded" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="Submit" runat="server" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required" ControlToValidate="txt_jobtitle"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>

                                        <div class="row mt-2" id="div_documents" runat="server" visible="false">
                                            <div class="col-md-12">
                                                <div class="ln_solid"></div>
                                                <label class="top-label text-primary"><i class="fa fa-file-text-o"></i> Required Digital Documents</label>
                                                <p class="text-muted small">Mandatory documents are locked. Select any additional requirements.</p>
                                                
                                                <div class="well well-sm doc-list" style="background-color: #f9f9f9;">
                                                    <asp:CheckBoxList ID="CBL_Documents" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" CssClass="table table-borderless table-condensed"></asp:CheckBoxList>
                                                </div>
                                            </div>
                                        </div>

                                        <asp:TextBox ID="txt_workregion" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txt_company" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txt_dept" runat="server" Visible="false"></asp:TextBox>

                                    </div>

                                    <div id="jobid_creation_buttons" runat="server" visible="true">
                                        <div class="ln_solid"></div>
                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="btn btn-danger btn-sm" OnClick="btn_cancel_Click" CausesValidation="false" />
                                                <asp:Button ID="btn_submit" runat="server" Text="Create JOB & Continue" ValidationGroup="Submit" CssClass="btn btn-success btn-sm" OnClick="btn_submit_Click" />
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

        // HTML5 Geolocation API
        function requestGPSLocation() {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function (position) {
                    // Populate hidden fields
                    document.getElementById('<%= hf_latitude.ClientID %>').value = position.coords.latitude;
                    document.getElementById('<%= hf_longitude.ClientID %>').value = position.coords.longitude;
                    
                    // Update UI to show success
                    let indicator = document.getElementById('gps_indicator');
                    if(indicator) {
                        indicator.innerHTML = "<i class='fa fa-check text-success'></i> GPS Captured";
                        indicator.className = "text-success";
                    }
                }, function (error) {
                    showPNotify('GPS Error', 'Please enable location services for this region.', 'error');
                    let indicator = document.getElementById('gps_indicator');
                    if(indicator) {
                        indicator.innerHTML = "<i class='fa fa-times text-danger'></i> Location Denied";
                        indicator.className = "text-danger";
                    }
                });
            } else {
                showPNotify('Warning', 'Geolocation is not supported by this browser.', 'notice');
            }
        }
    </script>
</asp:Content>