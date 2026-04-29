<%@ Page Title="JOB IN-Punch V2" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="job_inpunch_v2.aspx.cs" Inherits="WebApplication1.bussiness.production.job_inpunch_v2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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

        /* Modern Panel Styling (Consistent with Steps 1 & 2) */
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

        /* Scanner Specific Styles */
        .modern-scan-box {
            background-color: #fdfdfe;
            padding: 20px;
            border-radius: 8px;
            border: 1px dashed #1ABB9C;
            box-shadow: 0 4px 6px rgba(26, 187, 156, 0.05);
            margin-bottom: 20px;
        }
        .scanner-input {
            border: 2px solid #1ABB9C !important;
            font-size: 16px;
            font-weight: bold;
            text-align: center;
            letter-spacing: 1px;
            padding: 10px;
        }
        .scanner-input:focus {
            box-shadow: 0 0 0 4px rgba(26, 187, 156, 0.2) !important;
        }

        /* Employee Success Card */
        .modern-success-card {
            background-color: #f0f9f6;
            border: 1px solid #c8e6c9;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.02);
        }

        /* Job Details Card */
        .modern-info-card {
            background-color: #fcfcfd;
            border: 1px solid #e9ecef;
            padding: 20px;
            border-radius: 8px;
            margin-bottom: 20px;
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

        /* GridView Container */
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

        /* Loader Overlay */
        #loadingOverlay {
            position: fixed;
            top: 0; left: 0; width: 100%; height: 100%;
            background: rgba(255,255,255,0.85);
            z-index: 9999;
            backdrop-filter: blur(2px);
        }
        .spinner-container {
            position: absolute;
            top: 50%; left: 50%;
            transform: translate(-50%, -50%);
            text-align: center;
        }
        .loader {
            border: 6px solid #f3f3f3;
            border-top: 6px solid #1ABB9C; /* Gentelella Green */
            border-radius: 50%;
            width: 60px; height: 60px;
            animation: spin 1s linear infinite;
            margin: auto;
        }
        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }
        .loading-text {
            margin-top: 15px;
            font-size: 16px;
            font-weight: 600;
            color: #2a3f54;
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
                            <h2 style="margin: 0;">Step 3: Manpower IN-Punch <small style="color:#1ABB9C; font-weight: 600;">Smart Scanner</small></h2>
                            <a href="job_inpunch.aspx" class="modern-header-btn">
                                <i class="fa fa-history" style="margin-right: 5px;"></i> Switch to OLD Version
                            </a>
                            <div class="clearfix"></div>
                        </div>

                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>

                                <div class="x_content bg-light p-3 mb-4" style="border-radius: 8px; border: 1px solid #e9ecef;" id="InpunchPanel_Row" runat="server">
                                    <div class="row">
                                        <div class="col-md-5 col-sm-12 form-group mb-0">
                                            <label class="top-label">Select Active JOB ID <span class="req-star">*</span></label>
                                            <asp:DropDownList ID="DDL_JOBID" runat="server" CssClass="form-control modern-input" AutoPostBack="true" OnSelectedIndexChanged="DDL_JOBID_SelectedIndexChanged"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="ADDTOLIST" ControlToValidate="DDL_JOBID" runat="server" ErrorMessage="Required" CssClass="text-danger small" Display="Dynamic" InitialValue="--Select--"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>

                                <div class="x_content" id="JOBIDDetails_Row" runat="server" visible="false">
                                    <div class="row modern-info-card" style="padding: 0;">
                                        <div class="col-md-12" style="background-color: #f8f9fa; border-bottom: 1px solid #e9ecef; padding: 15px 20px; border-radius: 8px 8px 0 0;">
                                            <h5 class="m-0" style="color: #2980b9; font-weight: 600;"><i class="fa fa-briefcase" style="margin-right: 8px;"></i>Active JOB Overview</h5>
                                        </div>

                                        <div class="col-md-12 p-4">
                                            <div class="row">
                                                <div class="col-md-3 col-sm-6 mb-3">
                                                    <small class="text-muted text-uppercase font-weight-bold"><i class="fa fa-hashtag"></i> JOB ID</small><br />
                                                    <asp:Label ID="lbl_jobid" runat="server" CssClass="data-label" Style="font-size: 16px;"></asp:Label>
                                                </div>
                                                <div class="col-md-3 col-sm-6 mb-3">
                                                    <small class="text-muted text-uppercase font-weight-bold"><i class="fa fa-calendar"></i> Date</small><br />
                                                    <asp:Label ID="lbl_jobiddate" runat="server" CssClass="data-label" Style="font-size: 15px;"></asp:Label>
                                                </div>
                                                <div class="col-md-3 col-sm-6 mb-3">
                                                    <small class="text-muted text-uppercase font-weight-bold"><i class="fa fa-file-text-o"></i> Work Order</small><br />
                                                    <asp:Label ID="lbl_wrkordr" runat="server" CssClass="data-label" Style="font-size: 15px;"></asp:Label>
                                                </div>
                                                <div class="col-md-3 col-sm-6 mb-3">
                                                    <small class="text-muted text-uppercase font-weight-bold"><i class="fa fa-shield"></i> Permit No</small><br />
                                                    <asp:Label ID="lbl_permitno" runat="server" CssClass="badge bg-red" Style="font-size: 13px; margin-top: 4px; padding: 5px 8px;"></asp:Label>
                                                </div>

                                                <div class="col-md-3 col-sm-6 mb-2">
                                                    <small class="text-muted text-uppercase font-weight-bold"><i class="fa fa-building-o"></i> Site / Location</small><br />
                                                    <span class="data-label" style="display: inline-block;">
                                                        <asp:Label ID="lbl_jobsite" runat="server"></asp:Label>
                                                        <span class="text-muted font-weight-normal mx-1">|</span>
                                                        <asp:Label ID="lbl_jobloc" runat="server" CssClass="font-weight-normal"></asp:Label>
                                                    </span>
                                                </div>
                                                <div class="col-md-3 col-sm-6 mb-2">
                                                    <small class="text-muted text-uppercase font-weight-bold"><i class="fa fa-clock-o"></i> Shift</small><br />
                                                    <asp:Label ID="lbl_jobshift" runat="server" CssClass="badge bg-green" Style="font-size: 13px; margin-top: 4px; padding: 5px 8px;"></asp:Label>
                                                </div>
                                                <div class="col-md-6 col-sm-12 mb-2">
                                                    <small class="text-muted text-uppercase font-weight-bold"><i class="fa fa-user-circle"></i> Site In-Charge</small><br />
                                                    <asp:Label ID="lbl_inchargename" runat="server" CssClass="data-label"></asp:Label>
                                                </div>
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

                                    <div class="row align-items-center modern-scan-box" id="WorkmanInput_Row" runat="server" visible="false">
                                        <div class="col-md-12 mb-3">
                                            <span style="color: #1ABB9C; font-weight: 600; font-size: 15px;"><i class="fa fa-clock-o" style="margin-right: 6px;"></i>Set IN-Punch Time for Scanned Workers</span>
                                        </div>
                                        <div class="col-md-3 col-sm-6 form-group">
                                            <label class="top-label">IN Date <span class="req-star">*</span></label>
                                            <asp:TextBox ID="txt_date" runat="server" CssClass="form-control modern-input" type="date"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3 col-sm-6 form-group">
                                            <label class="top-label">IN Time <span class="req-star">*</span></label>
                                            <asp:TextBox ID="txt_time" runat="server" CssClass="form-control modern-input" type="time"></asp:TextBox>
                                        </div>
                                        <div class="col-md-5 col-sm-12 form-group">
                                            <label class="top-label" style="color: #2980b9;"><i class="fa fa-barcode" style="margin-right: 5px;"></i>Scan / Enter Workman ID</label>
                                            <asp:TextBox ID="txt_empworkman" CssClass="form-control modern-input scanner-input" runat="server" AutoPostBack="true" placeholder="Enter ID & Press Enter" OnTextChanged="txt_empworkman_TextChanged"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row mt-3" id="EmployeeData_Row" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="modern-success-card">
                                                <div class="row align-items-center">

                                                    <div class="col-md-4" style="border-right: 2px solid #a5d6a7;">
                                                        <h4 style="color: #2e7d32; font-weight: 700; margin-top: 0;"><i class="fa fa-user" style="margin-right: 8px;"></i>
                                                            <asp:TextBox ID="txt_empname" runat="server" ReadOnly="true" CssClass="border-0 bg-transparent p-0 m-0" Style="outline: none; width: 85%; color: #2e7d32; font-weight: bold;"></asp:TextBox></h4>
                                                        <div class="mt-2">
                                                            <asp:Label ID="lbl_designation" runat="server" CssClass="badge bg-blue" Style="font-size: 12px; padding: 5px 8px;"></asp:Label>
                                                            <asp:Label ID="lbl_category" runat="server" CssClass="badge bg-secondary" Style="font-size: 12px; padding: 5px 8px;"></asp:Label>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-5">
                                                        <div class="row">
                                                            <div class="col-sm-6 text-center">
                                                                <small class="text-muted text-uppercase font-weight-bold">Gatepass No</small><br />
                                                                <span class="text-dark font-weight-bold" style="font-size: 15px;">[<asp:Label ID="lbl_gpno" runat="server"></asp:Label>]</span>
                                                            </div>
                                                            <div class="col-sm-6 text-center">
                                                                <small class="text-muted text-uppercase font-weight-bold">Expiry & Status</small><br />
                                                                <asp:Label ID="lbl_gpvalidty" runat="server" CssClass="text-dark font-weight-bold" Style="display: block; margin-bottom: 4px;"></asp:Label>
                                                                <asp:Label ID="lbl_gpdays" runat="server" Style="font-size: 12px; padding: 4px 8px;"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-3 text-right" style="border-left: 2px solid #a5d6a7;">
                                                        <asp:Button ID="btnShowPopup2" runat="server" Text="Renew Pass" CssClass="btn btn-danger btn-modern btn-block mb-2" Visible="false" OnClientClick="$('#myModal2').modal('show'); return false;" />
                                                        <asp:Button ID="btn_submit" runat="server" Text="Add to Roster" ValidationGroup="ADDTOLIST" CssClass="btn btn-success btn-modern btn-block mb-2" OnClick="btn_submit_Click" />
                                                        <asp:Button ID="btn_reset" runat="server" Text="Clear & Scan Next" CssClass="btn btn-outline-secondary btn-modern btn-block" OnClick="btn_reset_Click" />
                                                    </div>

                                                </div>

                                                <asp:Label ID="lbl_workhours" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lbl_designationcode" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lbl_categorycode" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lbl_pocategoryname" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lbl_pocategorycode" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lbl_sftyno" runat="server" Visible="false"></asp:Label>
                                                <asp:TextBox ID="txt_worksite" runat="server" Visible="false"></asp:TextBox>
                                                <asp:Label ID="lbl_worksitecode" runat="server" Visible="false"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row mt-4" id="ViewState_TableRow" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <h5 style="color: #1ABB9C; font-weight: 600; margin-bottom: 15px;"><i class="fa fa-users" style="margin-right: 6px;"></i>Staged for IN-Punch</h5>
                                            <div class="modern-grid-container card-box table-responsive">
                                                <asp:GridView ID="GridView1" runat="server" Width="100%" CssClass="table table-striped table-hover table-sm" AutoGenerateColumns="false" EmptyDataText="No Data Found" OnRowDeleting="GridView1_RowDeleting" GridLines="None">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="SL" ItemStyle-Width="5%" ItemStyle-CssClass="text-center">
                                                            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Workman ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_wrk" runat="server" Text='<%# Bind("wrk") %>' Font-Weight="Bold" ForeColor="#34495e"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_name" runat="server" Text='<%# Bind("name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="IN Time">
                                                            <ItemTemplate>
                                                                <span class="badge bg-green"><asp:Label ID="lbl_in" runat="server" Text='<%# Bind("in") %>'></asp:Label></span>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="8%" ItemStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="btndelete" runat="server" CommandName="Delete" CssClass="btn btn-danger btn-xs" ToolTip="Remove from list"><i class="fa fa-trash"></i></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_wrkhrs" runat="server" Text='<%# Bind("wrkhrs") %>'></asp:Label>
                                                                <asp:Label ID="lbl_category" runat="server" Text='<%# Bind("category") %>'></asp:Label>
                                                                <asp:Label ID="lbl_categorycode" runat="server" Text='<%# Bind("categorycode") %>'></asp:Label>
                                                                <asp:Label ID="lbl_po_category" runat="server" Text='<%# Bind("po_category") %>'></asp:Label>
                                                                <asp:Label ID="lbl_po_categorycode" runat="server" Text='<%# Bind("po_categorycode") %>'></asp:Label>
                                                                <asp:Label ID="lbl_designation" runat="server" Text='<%# Bind("designation") %>'></asp:Label>
                                                                <asp:Label ID="lbl_designationcode" runat="server" Text='<%# Bind("designationcode") %>'></asp:Label>
                                                                <asp:Label ID="lbl_gpno" runat="server" Text='<%# Bind("gpno") %>'></asp:Label>
                                                                <asp:Label ID="lbl_sftyno" runat="server" Text='<%# Bind("sftyno") %>'></asp:Label>
                                                                <asp:Label ID="lbl_wrksitename" runat="server" Text='<%# Bind("wrksitename") %>'></asp:Label>
                                                                <asp:Label ID="lbl_wrksitecode" runat="server" Text='<%# Bind("wrksitecode") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row mt-4" id="ExistingWorkers_Row" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <h5 style="color: #3498DB; font-weight: 600; margin-bottom: 15px;"><i class="fa fa-check-square-o" style="margin-right: 6px;"></i>Already IN-Punched Members <small class="text-muted">(Active in Database)</small></h5>
                                            <div class="modern-grid-container card-box table-responsive">
                                                <asp:GridView ID="gvExistingWorkers" runat="server" Width="100%" CssClass="table table-hover table-sm" AutoGenerateColumns="false" BackColor="#ffffff" DataKeyNames="Id" OnRowDeleting="gvExistingWorkers_RowDeleting" GridLines="None">
                                                    <HeaderStyle BackColor="#ebf5fb" ForeColor="#2980b9" CssClass="text-center" Font-Bold="true" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="SL" ItemStyle-Width="5%" ItemStyle-CssClass="text-center">
                                                            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="EmployeeWrk" HeaderText="Workman ID" ItemStyle-CssClass="font-weight-bold text-dark" />
                                                        <asp:BoundField DataField="EmployeeName" HeaderText="Name" />
                                                        <asp:BoundField DataField="Inpunch_Time" HeaderText="IN Time" ItemStyle-CssClass="text-success font-weight-bold" />

                                                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="10%" ItemStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="btnDeleteDB" runat="server" CommandName="Delete" CssClass="btn btn-outline-danger btn-xs" OnClientClick="return confirm('Are you sure you want to remove this worker from the active job?');" ToolTip="Remove from active JOB"><i class="fa fa-times"></i> Remove</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="SendAttendance_Buttons" runat="server" visible="false">
                                        <div class="ln_solid" style="margin-top: 30px; margin-bottom: 20px;"></div>
                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btn_finalsubmit" runat="server" Text="Finalize IN-Punch" CssClass="btn btn-primary btn-modern btn-lg" style="padding: 10px 30px; font-size: 15px;" OnClientClick="showLoader();" OnClick="btn_finalsubmit_Click" />
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
            <div class="modal-content" style="border-radius: 10px; border: none; box-shadow: 0 10px 30px rgba(0,0,0,0.1);">
                <div class="modal-header" style="background-color: #f39c12; border-radius: 10px 10px 0 0; color: white;">
                    <h4 class="modal-title" style="font-weight: 600;"><i class="fa fa-warning" style="margin-right: 8px;"></i>Update Gatepass / Safety Pass</h4>
                    <button type="button" class="close" data-dismiss="modal" style="color: white; opacity: 0.8;">&times;</button>
                </div>
                <div class="modal-body p-4">
                    <div class="row">
                        <div class="col-md-6 form-group">
                            <label class="top-label">New Gatepass No:</label>
                            <asp:TextBox ID="txt_nwgpno" runat="server" CssClass="form-control modern-input"></asp:TextBox>
                        </div>
                        <div class="col-md-6 form-group">
                            <label class="top-label">New GP Validity:</label>
                            <asp:TextBox ID="txt_nwgpvalidity" runat="server" CssClass="form-control modern-input" type="date"></asp:TextBox>
                        </div>
                        <div class="col-md-6 form-group">
                            <label class="top-label">New Safety No:</label>
                            <asp:TextBox ID="txt_nwsftyno" runat="server" CssClass="form-control modern-input"></asp:TextBox>
                        </div>
                        <div class="col-md-6 form-group">
                            <label class="top-label">New Safety Validity:</label>
                            <asp:TextBox ID="txt_nwsftyvalidity" runat="server" CssClass="form-control modern-input" type="date"></asp:TextBox>
                        </div>
                        <div class="col-md-12 form-group mb-0">
                            <label class="top-label">New PV Validity:</label>
                            <asp:TextBox ID="txt_nwpvvalidity" runat="server" CssClass="form-control modern-input" type="date"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="modal-footer" style="border-top: 1px solid #e9ecef;">
                    <button type="button" class="btn btn-secondary btn-modern" data-dismiss="modal">Close</button>
                    <asp:Button ID="btn_gtpsedit" runat="server" Text="Save Changes" CssClass="btn btn-success btn-modern" OnClick="btn_gtpsedit_Click" />
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