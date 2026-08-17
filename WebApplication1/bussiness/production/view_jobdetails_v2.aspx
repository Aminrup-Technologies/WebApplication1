<%@ Page Title="View JOB Details" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="view_jobdetails_v2.aspx.cs" Inherits="WebApplication1.bussiness.production.view_jobdetails_v2" MaintainScrollPositionOnPostBack="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /* Ultra-Compact Typography and Layout */
        .top-label {
            font-weight: 600;
            color: #2a3f54;
            font-size: 12px;
            letter-spacing: 0.3px;
            margin-bottom: 2px;
            display: block;
        }

        /* Modern Inputs - Compact */
        .modern-input {
            border: 1px solid #dce1e5 !important;
            border-radius: 4px !important;
            padding: 4px 8px;
            font-size: 13px;
            color: #495057;
            height: 30px !important;
            box-shadow: inset 0 1px 2px rgba(0,0,0,0.02);
        }
        textarea.modern-input { height: auto !important; }
        
        .modern-input[readonly] {
            background-color: #f8f9fa !important;
            color: #2c3e50;
            font-weight: 600;
            border-color: #e9ecef !important;
        }

        /* Info Card Container */
        .modern-info-card {
            background-color: #ffffff;
            border: 1px solid #e9ecef;
            padding: 15px;
            border-radius: 6px;
            margin-bottom: 15px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.02);
        }

        /* Standard Buttons */
        .btn-modern {
            border-radius: 15px;
            padding: 4px 15px;
            font-weight: 600;
            letter-spacing: 0.3px;
            font-size: 12px;
            margin-bottom: 0;
            transition: all 0.2s ease;
        }
        .btn-modern:hover {
            transform: translateY(-1px);
            box-shadow: 0 3px 6px rgba(0,0,0,0.1);
        }

        /* WhatsApp Button */
        .btn-whatsapp { background-color: #25D366; color: white; border: none; }
        .btn-whatsapp:hover { background-color: #128C7E; color: white; }

        /* GridView Container */
        .modern-grid-container {
            border: 1px solid #e9ecef;
            border-radius: 6px;
            background: #fff;
            margin-bottom: 15px;
            overflow: hidden;
        }
        .modern-grid-container th {
            background-color: #f8f9fa !important;
            color: #34495e !important;
            font-weight: 700;
            font-size: 12px;
            border: 1px solid #e9ecef !important;
            padding: 8px 6px !important;
            white-space: nowrap;
        }
        .modern-grid-container td {
            vertical-align: middle !important;
            padding: 6px !important;
            font-size: 12px;
            border: 1px solid #e9ecef !important;
        }
        
        /* Section Titles */
        .compact-section-title {
            font-size: 14px;
            font-weight: 600;
            color: #2a3f54;
            margin-bottom: 10px;
            border-bottom: 1px dashed #e9ecef;
            padding-bottom: 5px;
        }
    </style>

    <script type="text/javascript">
        function ShowPopup(title, body) {
            $("#MyPopup .modal-title").html(title);
            $("#MyPopup .modal-body").html(body);
            $("#MyPopup").modal("show");
        }

        function openWhatsApp() {
            var whatsappMessage = $('#<%=HF_Msg.ClientID%>').val();
            whatsappMessage = window.encodeURIComponent(whatsappMessage);
            window.open("whatsapp://send?text=" + whatsappMessage, '_blank');
        }

        function validateProvidedOT(sender, args) {
            var textBox = $("#" + sender.controltovalidate);
            var inputValue = textBox.val();
            if (inputValue.trim() !== '') {
                var numericValue = parseInt(inputValue);
                if (isNaN(numericValue) || numericValue > 16) {
                    args.IsValid = false;
                } else {
                    args.IsValid = true;
                }
            } else {
                args.IsValid = false;
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="container-fluid pl-0 pr-0">
            <asp:HiddenField ID="HF_Msg" runat="server" />

            <div class="page-title" style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 15px; border-bottom: 2px solid #1ABB9C; padding-bottom: 10px; flex-wrap: wrap;">
                <div class="title_left mb-2 mb-md-0" style="flex: 1;">
                    <h3 style="margin: 0; color: #2a3f54; font-weight: 600; font-size: 18px;">
                        <i class="fa fa-eye text-primary mr-2" style="color: #1ABB9C !important;"></i>View JOB Details
                    </h3>
                </div>
                <div class="title_right d-flex justify-content-end align-items-center" style="display: flex; gap: 8px; flex-wrap: wrap;">
                    <asp:Button ID="btn_update" runat="server" Text="Edit Data" CssClass="btn btn-success btn-modern" Enabled="false" OnClick="btn_update_Click" />
                    <asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="btn btn-danger btn-modern" Enabled="false" Visible="false" OnClick="btn_cancel_Click" />
                    <asp:LinkButton ID="btn_share_whatsapp" runat="server" CssClass="btn btn-whatsapp btn-modern shadow-sm" OnClick="btn_share_whatsapp_Click" OnClientClick="document.body.style.cursor='wait';">
                        <i class="fa fa-share-alt mr-1"></i> Share Details
                    </asp:LinkButton>
                    
                    <asp:Button ID="btn_back" runat="server" Text="Back to List" CssClass="btn btn-outline-secondary btn-modern" OnClick="btn_back_Click" />
                </div>
            </div>
            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12">
                    <div class="modern-info-card">
                        <div class="compact-section-title"><i class="fa fa-info-circle mr-1 text-info"></i> Basic Data</div>
                        <div class="row">
                            <div class="col-md-12 form-group mb-2">
                                <label class="top-label text-primary">JOB Title</label>
                                <asp:TextBox ID="txt_jobtitle" class="form-control modern-input" runat="server" ReadOnly="true" TextMode="MultiLine" Rows="1"></asp:TextBox>
                            </div>
                            
                            <div class="col-md-2 col-sm-4 form-group mb-2">
                                <label class="top-label">JOB ID</label>
                                <asp:TextBox ID="txt_jobid" class="form-control modern-input text-primary font-weight-bold" runat="server" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col-md-2 col-sm-4 form-group mb-2">
                                <label class="top-label">Execution Date</label>
                                <asp:TextBox ID="txt_jobdate" class="form-control modern-input" runat="server" ReadOnly="true"></asp:TextBox>
                                <asp:Label ID="lbl_jobday" runat="server" Visible="false"></asp:Label>
                            </div>
                            <div class="col-md-2 col-sm-4 form-group mb-2">
                                <label class="top-label">Execution Day</label>
                                <asp:TextBox ID="txt_jobday" class="form-control modern-input" runat="server" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col-md-2 col-sm-4 form-group mb-2">
                                <label class="top-label">JOB Shift</label>
                                <asp:TextBox ID="txt_jobshift" class="form-control modern-input" runat="server" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col-md-2 col-sm-4 form-group mb-2">
                                <label class="top-label">Work Order No</label>
                                <asp:TextBox ID="txt_workorderno" class="form-control modern-input text-danger" runat="server" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col-md-2 col-sm-4 form-group mb-2">
                                <label class="top-label">Work Permit No</label>
                                <asp:TextBox ID="txt_permitno" runat="server" ReadOnly="true" MaxLength="100" class="form-control modern-input"></asp:TextBox>
                            </div>

                            <div class="col-md-3 col-sm-6 form-group mb-1">
                                <label class="top-label">Work Site Name</label>
                                <asp:TextBox ID="txt_worksitename" class="form-control modern-input" runat="server" ReadOnly="true"></asp:TextBox>
                                <asp:Label ID="lbl_worksitedbcode" runat="server" Visible="false"></asp:Label>
                            </div>
                            <div class="col-md-2 col-sm-6 form-group mb-1">
                                <label class="top-label">Department</label>
                                <asp:TextBox ID="txt_jobdept" class="form-control modern-input" runat="server" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col-md-2 col-sm-6 form-group mb-1">
                                <label class="top-label">Location</label>
                                <asp:TextBox ID="txt_jobloc" class="form-control modern-input" runat="server" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col-md-3 col-sm-6 form-group mb-1">
                                <label class="top-label">Site In-Charge</label>
                                <asp:TextBox ID="txt_inchargename" class="form-control modern-input text-info" runat="server" ReadOnly="true"></asp:TextBox>
                                <asp:Label ID="lbl_inchargewrk" runat="server" Visible="false"></asp:Label>
                            </div>
                            <div class="col-md-2 col-sm-6 form-group mb-1">
                                <label class="top-label">Executor</label>
                                <asp:TextBox ID="txt_jobsupv" class="form-control modern-input text-info" runat="server" ReadOnly="true"></asp:TextBox>
                                <asp:Label ID="lbl_creatorwrk" runat="server" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4 col-sm-12">
                    <div class="compact-section-title"><i class="fa fa-paperclip mr-1 text-success"></i> Attached Permits</div>
                    <div class="modern-grid-container">
                        <div class="modern-table-wrapper">
                            <asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-sm mb-0" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Permit Attached" OnRowDeleting="GridView1_RowDeleting" GridLines="Both">
                                <Columns>
                                    <asp:TemplateField HeaderText="SL" HeaderStyle-Width="5%" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DB ID" HeaderStyle-Width="10%" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="JOBID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_JOBID" runat="server" Text='<%# Bind("JOBID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="File Name" HeaderStyle-Width="40%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Name" runat="server" Text='<%# Eval("Name") %>' Font-Weight="Bold" ForeColor="#2c3e50" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="View" HeaderStyle-Width="10%" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("Id") %>' OnClick="DownloadFile" CssClass="btn btn-info btn-xs" ToolTip="Download"><i class="fa fa-download"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Time" HeaderStyle-Width="20%" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_TimeStamp" runat="server" Text='<%# Eval("TimeStamp") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>

                <div class="col-md-8 col-sm-12">
                    <div class="compact-section-title d-flex justify-content-between align-items-center">
                        <span><i class="fa fa-users mr-1 text-warning"></i> Deployed Manpower</span>
                        <div runat="server" id="attachmanpowerrow" visible="false">
                            <asp:Button ID="btn_attachmanpower" runat="server" Text="+ Add Manpower" CssClass="btn btn-primary btn-xs mb-0" OnClick="btn_attachmanpower_Click" />
                        </div>
                        <div runat="server" id="ResendApp_Div" visible="false">
                            <asp:Button ID="btn_resendapp" runat="server" Text="Resend for Approval" CssClass="btn btn-warning btn-xs mb-0" OnClick="btn_resendapp_Click" />
                        </div>
                    </div>
                    <div class="modern-grid-container">
                        <div class="modern-table-wrapper">
                            <asp:GridView ID="GridView2" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-sm mb-0" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Manpower Added" OnRowCancelingEdit="GridView2_RowCancelingEdit" OnRowDeleting="GridView2_RowDeleting" OnRowEditing="GridView2_RowEditing" OnRowDataBound="GridView2_RowDataBound" OnRowUpdating="GridView2_RowUpdating" GridLines="Both">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl" HeaderStyle-Width="3%" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Id" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Region" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_JOB_Region" runat="server" Text='<%# Bind("JOB_Region") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="JOBID" HeaderStyle-Width="8%" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_JOBID" runat="server" Text='<%# Bind("JOBID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="JOB Date" HeaderStyle-Width="8%" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_CreatedDate" runat="server" Text='<%# Bind("CreatedDate","{0:dd-MM-yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="WRK" HeaderStyle-Width="6%" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_EmployeeWrk" runat="server" Font-Bold="true" Text='<%# Bind("EmployeeWrk") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_EmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="H" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_WourkHours" runat="server" Text='<%# Bind("WourkHours") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="IN Punch" HeaderStyle-Width="15%" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Inpunch_Time" runat="server" Text='<%# Bind("Inpunch_Time","{0:dd-MM-yyyy hh:mm tt}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txt_Inpunch_Time" runat="server" class="form-control form-control-sm modern-input" Text='<%# DataBinder.Eval(Container.DataItem,"Inpunch_Time", "{0:yyyy-MM-dd hh:mm:ss tt}") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="OUT Punch" HeaderStyle-Width="15%" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Outpunch_Time" runat="server" Text='<%# Bind("Outpunch_Time","{0:dd-MM-yyyy hh:mm tt}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txt_Outpunch_Time" runat="server" class="form-control form-control-sm modern-input" Text='<%# DataBinder.Eval(Container.DataItem,"Outpunch_Time", "{0:yyyy-MM-dd hh:mm:ss tt}") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Lunch" HeaderStyle-Width="6%" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_LunchFactor" runat="server" Text='<%# Bind("LunchFactor") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="DDL_LunchYesNo" runat="server" class="form-control form-control-sm modern-input" SelectedValue='<%# Bind("LunchFactor") %>'>
                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                <asp:ListItem Value="No">No</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="POT" HeaderStyle-Width="5%" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ProvidedOT" runat="server" Text='<%# Bind("ProvidedOT") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txt_ProvidedOT" runat="server" class="form-control form-control-sm modern-input text-center" Text='<%# DataBinder.Eval(Container.DataItem,"ProvidedOT") %>'></asp:TextBox>
                                            <asp:CustomValidator ID="CustomValidator2" runat="server" ValidationGroup="Update" ControlToValidate="txt_ProvidedOT" Display="Dynamic" ErrorMessage="<=16" ForeColor="IndianRed" ClientValidationFunction="validateProvidedOT"></asp:CustomValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Status" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_AttendanceStatus" runat="server" Text='<%# Bind("AttendanceStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="DDL_AttendanceStatus" class="form-control form-control-sm modern-input" runat="server"></asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Code" HeaderStyle-Width="6%" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_AttendanceCode" runat="server" Text='<%# Bind("AttendanceCode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="DDL_AttendanceCode" class="form-control form-control-sm modern-input" runat="server"></asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action" HeaderStyle-Width="8%" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnedit" runat="server" CommandName="Edit" CssClass="btn btn-outline-primary btn-xs" ToolTip="Edit"><i class="fa fa-pencil"></i></asp:LinkButton>
                                            <asp:LinkButton ID="btndelete" runat="server" Enabled="false" CommandName="Delete" CssClass="btn btn-outline-danger btn-xs" OnClientClick="return confirm('Do you want to DELETE?')" ToolTip="Delete"><i class="fa fa-trash"></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="btnupdate" runat="server" CommandName="Update" CssClass="btn btn-success btn-xs" ValidationGroup="Update" ToolTip="Save"><i class="fa fa-save"></i></asp:LinkButton>
                                            <asp:LinkButton ID="Btncancale" runat="server" CommandName="Cancel" CssClass="btn btn-secondary btn-xs" ToolTip="Cancel"><i class="fa fa-times"></i></asp:LinkButton>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="MyPopup" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog modal-sm">
            <div class="modal-content" style="border-radius: 8px;">
                <div class="modal-header bg-light">
                    <h5 class="modal-title font-weight-bold" id="myModalLabel2"></h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                </div>
                <div class="modal-body text-center p-4"></div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary btn-modern" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>