<%@ Page Title="Permit Upload V2" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="job_permitupload_v2.aspx.cs" Inherits="WebApplication1.bussiness.production.job_permitupload_v2" %>

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

        .req-star {
            color: #E74C3C;
            font-weight: bold;
            margin-left: 2px;
        }

        .data-label {
            font-weight: 700;
            color: #2c3e50;
            font-size: 14px;
            display: block;
            margin-top: 2px;
        }

        /* Modern Panel Styling (Consistent with Step 1) */
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

        /* Job Details Card */
        .modern-info-card {
            background-color: #fcfcfd;
            border: 1px solid #e9ecef;
            padding: 20px;
            border-radius: 8px;
            margin-bottom: 20px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.02);
        }
        .modern-info-card .text-muted {
            font-size: 12px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            margin-bottom: 0;
        }

        /* Status Bar */
        .modern-status-bar {
            background-color: #f4f6f9;
            border-left: 4px solid #1ABB9C;
            border-radius: 6px;
            padding: 20px;
            box-shadow: 0 2px 5px rgba(0,0,0,0.03);
            display: flex;
            align-items: center;
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
        }
        .modern-grid-container .table {
            margin-bottom: 0;
        }
        .modern-grid-container th {
            background-color: #f8f9fa;
            color: #34495e;
            font-weight: 600;
            border-bottom-width: 1px;
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
                            <h2 style="margin: 0;">Step 2: Permit Attachment <small style="color:#1ABB9C; font-weight: 600;">Smart Workflow</small></h2>
                            <a href="job_permitupload.aspx" class="modern-header-btn">
                                <i class="fa fa-history" style="margin-right: 5px;"></i> Switch to OLD Version
                            </a>
                            <div class="clearfix"></div>
                        </div>

                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnUpload" />
                            </Triggers>
                            <ContentTemplate>

                                <div class="x_content bg-light p-3 mb-4" style="border-radius: 8px; border: 1px solid #e9ecef;">
                                    <div class="row">
                                        <div class="col-md-5 col-sm-12 form-group mb-0">
                                            <label class="top-label">Select Active JOB ID <span class="req-star">*</span></label>
                                            <asp:DropDownList ID="DDL_JOBID" runat="server" CssClass="form-control modern-input" AutoPostBack="true" OnSelectedIndexChanged="DDL_JOBID_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <div class="x_content" id="jobid_details_row" runat="server" visible="false">
                                    <div class="row modern-info-card">
                                        <div class="col-md-12">
                                            <h5 style="color: #2980b9; font-weight: 600; margin-top: 0;"><i class="fa fa-info-circle" style="margin-right: 6px;"></i>JOB Details</h5>
                                            <div class="ln_solid mt-2 mb-3"></div>
                                        </div>

                                        <div class="col-md-4 col-sm-6 mb-3">
                                            <label class="text-muted">JOB ID:</label>
                                            <asp:Label ID="lbl_jobid" runat="server" CssClass="data-label"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-sm-6 mb-3">
                                            <label class="text-muted">Date:</label>
                                            <asp:Label ID="lbl_jobiddate" runat="server" CssClass="data-label"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-sm-6 mb-3">
                                            <label class="text-muted">Permit No:</label>
                                            <asp:Label ID="lbl_permitno" runat="server" CssClass="data-label text-danger"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-sm-6 mb-3">
                                            <label class="text-muted">Site:</label>
                                            <asp:Label ID="lbl_jobsite" runat="server" CssClass="data-label"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-sm-6 mb-3">
                                            <label class="text-muted">Location:</label>
                                            <asp:Label ID="lbl_jobloc" runat="server" CssClass="data-label"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-sm-6 mb-3">
                                            <label class="text-muted">Shift:</label>
                                            <asp:Label ID="lbl_jobshift" runat="server" CssClass="data-label text-success"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-sm-6 mb-3">
                                            <label class="text-muted">Work Order:</label>
                                            <asp:Label ID="lbl_wrkordr" runat="server" CssClass="data-label"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-sm-6 mb-3">
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

                                    <div class="row modern-status-bar mt-2 mb-4">
                                        <div class="col-md-3 col-sm-6 mb-2 mb-md-0">
                                            <label class="top-label" style="margin-bottom: 2px;">Permit Status:</label><br />
                                            <asp:Label ID="lbl_permitstatus" runat="server" Text="Pending" CssClass="badge bg-red" Font-Size="14px" style="padding: 6px 10px;"></asp:Label>
                                        </div>
                                        <div class="col-md-3 col-sm-6 mb-2 mb-md-0">
                                            <label class="top-label" style="margin-bottom: 2px;">Files Uploaded:</label><br />
                                            <asp:Label ID="lbl_filecount" runat="server" Text="0" Font-Bold="true" Font-Size="18px" ForeColor="#34495e"></asp:Label>
                                        </div>
                                        <div class="col-md-6 col-sm-12 text-md-right text-left mt-3 mt-md-0">
                                            <button type="button" class="btn btn-primary btn-modern" data-toggle="modal" data-target="#myModal">
                                                <i class="fa fa-upload" style="margin-right: 5px;"></i> Upload Permit File
                                            </button>
                                            <asp:Button ID="btn_inpunch" runat="server" Text="Proceed to IN-Punch" CssClass="btn btn-success btn-modern" Visible="false" OnClick="btn_inpunch_Click" />
                                        </div>
                                    </div>
                                </div>

                                <div class="x_content" id="GridTable_Row" runat="server" visible="false">
                                    <h5 style="color: #2a3f54; font-weight: 600; margin-bottom: 15px;"><i class="fa fa-file-image-o" style="margin-right: 6px;"></i>Attached Files Log</h5>
                                    <div class="modern-grid-container card-box table-responsive">
                                        <asp:GridView ID="GridView1" runat="server" Width="100%" CssClass="table table-hover table-striped table-sm" AutoGenerateColumns="false" EmptyDataText="No files uploaded yet" OnRowDeleting="GridView1_RowDeleting" GridLines="None">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL" ItemStyle-Width="5%" ItemStyle-CssClass="text-center">
                                                    <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="File Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Name" runat="server" Text='<%# Eval("Name") %>' Font-Weight="Bold" ForeColor="#34495e" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Type" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <span class="badge bg-green"><%# Eval("UploadType") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Uploaded On" ItemStyle-Width="20%">
                                                    <ItemTemplate><%# Eval("TimeStamp") %></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="12%" ItemStyle-CssClass="text-center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnDownload" runat="server" CommandArgument='<%# Eval("Id") %>' OnClick="DownloadFile" CssClass="btn btn-info btn-xs" ToolTip="Download"><i class="fa fa-download"></i></asp:LinkButton>
                                                        <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-danger btn-xs" OnClientClick="return confirm('Are you sure you want to delete this file?');" ToolTip="Delete"><i class="fa fa-trash"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>

                                <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
                                    <div class="modal-dialog">
                                        <div class="modal-content" style="border-radius: 10px; border: none; box-shadow: 0 10px 30px rgba(0,0,0,0.1);">
                                            <div class="modal-header" style="background-color: #f8f9fa; border-radius: 10px 10px 0 0; border-bottom: 1px solid #e9ecef;">
                                                <h4 class="modal-title" style="font-weight: 600; color: #34495e;">Attach New File</h4>
                                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                            </div>
                                            <div class="modal-body p-4">
                                                <div class="form-group">
                                                    <label class="top-label">Document Type <span class="req-star">*</span></label>
                                                    <asp:DropDownList ID="DDL_UploadType" runat="server" CssClass="form-control modern-input mb-4">
                                                        <asp:ListItem Value="PDF File">PDF Document (.pdf)</asp:ListItem>
                                                        <asp:ListItem Value="Photograph">Photograph (.jpg, .png)</asp:ListItem>
                                                    </asp:DropDownList>

                                                    <label class="top-label">Select File <span class="req-star">*</span></label>
                                                    <asp:FileUpload ID="FileUpload1" CssClass="form-control modern-input" runat="server" style="padding: 5px 12px;" />
                                                    <small class="text-muted mt-2 d-block"><i class="fa fa-info-circle"></i> Images will be automatically compressed to save space.</small>
                                                </div>
                                            </div>
                                            <div class="modal-footer" style="border-top: 1px solid #e9ecef;">
                                                <button type="button" class="btn btn-secondary btn-modern" data-dismiss="modal">Close</button>
                                                <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-primary btn-modern" CausesValidation="false" Text="Upload File" OnClick="ImportPermit" />
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
        function closeUploadModal() {
            $('#myModal').modal('hide');
        }
    </script>
</asp:Content>