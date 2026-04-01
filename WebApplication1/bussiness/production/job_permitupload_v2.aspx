<%@ Page Title="Permit Upload V2" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="job_permitupload_v2.aspx.cs" Inherits="WebApplication1.bussiness.production.job_permitupload_v2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .top-label {
            font-weight: 600;
            margin-bottom: 5px;
            color: #333;
        }

        .data-label {
            font-weight: bold;
            color: #0056b3;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="container">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Step 2: Permit Attachment <small>Smart Workflow</small></h2> &nbsp;
                            <a href="job_permitupload.aspx"
                                style="display: inline-block; padding: 6px 12px; cursor: pointer; background-color: #6c757d; color: white; border: none; border-radius: 4px; font-weight: bold; text-decoration: none; font-size: 14px;">Switch to OLD Version
                            </a>
                            <div class="clearfix"></div>
                        </div>

                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnUpload" />
                            </Triggers>
                            <ContentTemplate>

                                <div class="x_content bg-light p-3 mb-3" style="border-radius: 5px; border: 1px solid #ddd;">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-12 form-group">
                                            <label class="top-label">Select / Active JOB ID <span class="text-danger">*</span></label>
                                            <asp:DropDownList ID="DDL_JOBID" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_JOBID_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <div class="x_content" id="jobid_details_row" runat="server" visible="false">
                                    <div class="row" style="background-color: #fdfdfe; border: 1px solid #e5e5e5; padding: 15px; border-radius: 4px; margin-bottom: 20px; box-shadow: 0 1px 2px rgba(0,0,0,0.05);">
                                        <div class="col-md-12">
                                            <h5 class="text-primary"><i class="fa fa-info-circle"></i>JOB Details</h5>
                                            <div class="ln_solid mt-1 mb-3"></div>
                                        </div>

                                        <div class="col-md-4 col-sm-6 mb-2">
                                            <label class="text-muted">JOB ID:</label>
                                            <asp:Label ID="lbl_jobid" runat="server" CssClass="data-label"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-sm-6 mb-2">
                                            <label class="text-muted">Date:</label>
                                            <asp:Label ID="lbl_jobiddate" runat="server" CssClass="data-label"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-sm-6 mb-2">
                                            <label class="text-muted">Permit No:</label>
                                            <asp:Label ID="lbl_permitno" runat="server" CssClass="data-label text-danger"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-sm-6 mb-2">
                                            <label class="text-muted">Site:</label>
                                            <asp:Label ID="lbl_jobsite" runat="server" CssClass="data-label"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-sm-6 mb-2">
                                            <label class="text-muted">Location:</label>
                                            <asp:Label ID="lbl_jobloc" runat="server" CssClass="data-label"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-sm-6 mb-2">
                                            <label class="text-muted">Shift:</label>
                                            <asp:Label ID="lbl_jobshift" runat="server" CssClass="data-label text-success"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-sm-6 mb-2">
                                            <label class="text-muted">Work Order:</label>
                                            <asp:Label ID="lbl_wrkordr" runat="server" CssClass="data-label"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-sm-6 mb-2">
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

                                    <div class="row mt-4 p-3" style="background-color: #f4f6f9; border-left: 4px solid #1ABB9C;">
                                        <div class="col-md-3 col-sm-6 mb-2">
                                            <label class="top-label">Permit Status:</label><br />
                                            <asp:Label ID="lbl_permitstatus" runat="server" Text="Pending" CssClass="badge bg-red" Font-Size="14px"></asp:Label>
                                        </div>
                                        <div class="col-md-3 col-sm-6 mb-2">
                                            <label class="top-label">Files Uploaded:</label><br />
                                            <asp:Label ID="lbl_filecount" runat="server" Text="0" Font-Bold="true" Font-Size="16px"></asp:Label>
                                        </div>
                                        <div class="col-md-6 col-sm-12 text-right">
                                            <br />
                                            <button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#myModal">
                                                <i class="fa fa-upload"></i>Upload Permit File
                                            </button>
                                            <asp:Button ID="btn_inpunch" runat="server" Text="Proceed to IN-Punch" CssClass="btn btn-success btn-sm" Visible="false" OnClick="btn_inpunch_Click" />
                                        </div>
                                    </div>
                                </div>

                                <div class="x_content mt-4" id="GridTable_Row" runat="server" visible="false">
                                    <h5 class="text-primary"><i class="fa fa-file-image-o"></i>Attached Files</h5>
                                    <div class="card-box table-responsive">
                                        <asp:GridView ID="GridView1" runat="server" Width="100%" CssClass="table table-striped table-bordered table-sm" AutoGenerateColumns="false" EmptyDataText="No files uploaded yet" OnRowDeleting="GridView1_RowDeleting">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL" ItemStyle-Width="5%" ItemStyle-CssClass="text-center">
                                                    <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' /></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="File Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Name" runat="server" Text='<%# Eval("Name") %>' /></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Type" ItemStyle-Width="15%">
                                                    <ItemTemplate><%# Eval("UploadType") %></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Uploaded On" ItemStyle-Width="20%">
                                                    <ItemTemplate><%# Eval("TimeStamp") %></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="10%" ItemStyle-CssClass="text-center">
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
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h4 class="modal-title">Attach New File</h4>
                                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                            </div>
                                            <div class="modal-body">
                                                <div class="form-group">
                                                    <label class="top-label">Document Type <span class="req-star">*</span></label>
                                                    <asp:DropDownList ID="DDL_UploadType" runat="server" CssClass="form-control mb-3">
                                                        <asp:ListItem Value="PDF File">PDF Document (.pdf)</asp:ListItem>
                                                        <asp:ListItem Value="Photograph">Photograph (.jpg, .png)</asp:ListItem>
                                                    </asp:DropDownList>

                                                    <label class="top-label">Select File <span class="req-star">*</span></label>
                                                    <asp:FileUpload ID="FileUpload1" CssClass="form-control" runat="server" />
                                                    <small class="text-muted mt-1 d-block">Note: Images will be automatically compressed to save space.</small>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <button type="button" class="btn btn-secondary btn-sm" data-dismiss="modal">Close</button>
                                                <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-primary btn-sm" CausesValidation="false" Text="Upload File" OnClick="ImportPermit" />
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
