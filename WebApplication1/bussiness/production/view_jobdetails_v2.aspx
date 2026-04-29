<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="view_jobdetails_v2.aspx.cs" Inherits="WebApplication1.bussiness.production.view_jobdetails_v2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /* Base typography and layout */
        .top-label {
            font-weight: 600;
            color: #2a3f54;
            font-size: 13px;
            letter-spacing: 0.3px;
            margin-bottom: 4px;
            display: block;
        }

        /* Modern Panel Styling */
        .modern-panel {
            border: none !important;
            border-radius: 10px;
            box-shadow: 0 4px 10px rgba(0,0,0,0.05), 0 1px 3px rgba(0,0,0,0.03);
            background: #ffffff;
            margin-bottom: 20px;
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

        /* Modern Inputs */
        .modern-input {
            border: 1px solid #dce1e5 !important;
            border-radius: 6px !important;
            padding: 8px 12px;
            font-size: 14px;
            color: #495057;
            box-shadow: inset 0 1px 2px rgba(0,0,0,0.02);
        }

            .modern-input[readonly] {
                background-color: #f8f9fa !important;
                color: #2c3e50;
                font-weight: 600;
                border-color: #e9ecef !important;
            }

        /* Info Card Container */
        .modern-info-card {
            background-color: #fdfdfe;
            border: 1px solid #e9ecef;
            padding: 20px;
            border-radius: 8px;
            margin-bottom: 20px;
        }

        /* Standard Buttons */
        .btn-modern {
            border-radius: 20px;
            padding: 6px 18px;
            font-weight: 600;
            letter-spacing: 0.5px;
            font-size: 13px;
            transition: all 0.2s ease;
        }

            .btn-modern:hover {
                transform: translateY(-2px);
                box-shadow: 0 4px 8px rgba(0,0,0,0.15);
            }

        /* WhatsApp Button */
        .btn-whatsapp {
            background-color: #25D366;
            color: white;
            border: none;
        }

            .btn-whatsapp:hover {
                background-color: #128C7E;
                color: white;
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
    </style>

    <script type="text/javascript">
        function ShowPopup(title, body) {
            $("#MyPopup .modal-title").html(title);
            $("#MyPopup .modal-body").html(body);
            $("#MyPopup").modal("show");
        }

        // REVERTED TO OLD CLIENT-SIDE WHATSAPP SHARING
        function openWhatsApp() {
            var whatsappMessage = $('#<%=HF_Msg.ClientID%>').val();
            whatsappMessage = window.encodeURIComponent(whatsappMessage);
            window.open("whatsapp://send?text=" + whatsappMessage, '_blank');
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="container-fluid pl-0 pr-0">
            <div class="page-title mb-3 border-bottom pb-2">
                <div class="title_left">
                    <h3 style="color: #2a3f54; font-weight: 600;"><i class="fa fa-eye text-primary mr-2" style="color: #1ABB9C !important;"></i>View JOBID Details</h3>
                </div>
            </div>
            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel modern-panel">
                        <div class="x_title modern-title">
                            <h2 style="color: #2980b9;"><i class="fa fa-info-circle mr-2"></i>JOB Basic Details</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">

                            <asp:HiddenField ID="HF_Msg" runat="server" />

                            <div class="modern-info-card">
                                <div class="row">
                                    <div class="col-md-12 form-group mb-4">
                                        <label class="top-label text-primary">JOB Title</label>
                                        <asp:TextBox ID="txt_jobtitle" class="form-control modern-input" runat="server" ReadOnly="true" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                    </div>

                                    <div class="col-md-3 col-sm-6 form-group mb-3">
                                        <label class="top-label">JOB ID</label>
                                        <asp:TextBox ID="txt_jobid" class="form-control modern-input text-primary" runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group mb-3">
                                        <label class="top-label">Execution Date</label>
                                        <asp:TextBox ID="txt_jobdate" class="form-control modern-input" runat="server" ReadOnly="true"></asp:TextBox>
                                        <asp:Label ID="lbl_jobday" runat="server" Visible="false"></asp:Label>
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group mb-3">
                                        <label class="top-label">Execution Day</label>
                                        <asp:TextBox ID="txt_jobday" class="form-control modern-input" runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group mb-3">
                                        <label class="top-label">JOB Shift</label>
                                        <asp:TextBox ID="txt_jobshift" class="form-control modern-input" runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>

                                    <div class="col-md-3 col-sm-6 form-group mb-3">
                                        <label class="top-label">Work Order No</label>
                                        <asp:TextBox ID="txt_workorderno" class="form-control modern-input text-danger" runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group mb-3">
                                        <label class="top-label">Work Permit No</label>
                                        <asp:TextBox ID="txt_permitno" runat="server" ReadOnly="true" MaxLength="100" class="form-control modern-input"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_permitno" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Only numeric values separated by commas or N/A" ValidationExpression="^((?i:na|n/a)|\d+(\s*,\s*\d+)*)$"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group mb-3">
                                        <label class="top-label">Work Site Name</label>
                                        <asp:TextBox ID="txt_worksitename" class="form-control modern-input" runat="server" ReadOnly="true"></asp:TextBox>
                                        <asp:Label ID="lbl_worksitedbcode" runat="server" Visible="false"></asp:Label>
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group mb-3">
                                        <label class="top-label">JOB Department</label>
                                        <asp:TextBox ID="txt_jobdept" class="form-control modern-input" runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>

                                    <div class="col-md-3 col-sm-6 form-group mb-3">
                                        <label class="top-label">JOB Work Location</label>
                                        <asp:TextBox ID="txt_jobloc" class="form-control modern-input" runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group mb-3">
                                        <label class="top-label">JOB Executor</label>
                                        <asp:TextBox ID="txt_jobsupv" class="form-control modern-input text-info" runat="server" ReadOnly="true"></asp:TextBox>
                                        <asp:Label ID="lbl_creatorwrk" runat="server" Visible="false"></asp:Label>
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group mb-3">
                                        <label class="top-label">Site In-Charge Name</label>
                                        <asp:TextBox ID="txt_inchargename" class="form-control modern-input text-info" runat="server" ReadOnly="true"></asp:TextBox>
                                        <asp:Label ID="lbl_inchargewrk" runat="server" Visible="false"></asp:Label>
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group mb-3">
                                        <label class="top-label">Approver Remarks</label>
                                        <asp:TextBox ID="txt_approverrmrks" class="form-control modern-input" runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row align-items-center mt-3 border-top pt-3">
                                <div class="col-md-6 mb-2 mb-md-0">
                                    <asp:Button ID="btn_update" runat="server" Text="Update" CssClass="btn btn-success btn-modern" Enabled="false" OnClick="btn_update_Click" />
                                    <asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="btn btn-danger btn-modern" Enabled="false" Visible="false" OnClick="btn_cancel_Click" />
                                    <asp:Button ID="btn_back" runat="server" Text="Back to List" CssClass="btn btn-outline-secondary btn-modern ml-2" Enabled="true" OnClick="btn_back_Click" />
                                </div>
                                <div class="col-md-6 text-md-right text-left">
                                    <asp:Label ID="lbl_msg" runat="server" Text=""></asp:Label>

                                    <asp:LinkButton ID="btn_share_whatsapp" runat="server"
                                        CssClass="btn btn-whatsapp btn-modern shadow-sm"
                                        OnClick="btn_share_whatsapp_Click"
                                        OnClientClick="showLoader();">
        <i class="fa fa-share-alt mr-1" style="font-size: 16px;"></i> Share with Approver
                                    </asp:LinkButton>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel modern-panel">
                        <div class="x_title modern-title">
                            <h2 style="color: #34495e;"><i class="fa fa-paperclip mr-2"></i>Attached Permit Data</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="alert alert-secondary small py-2 mb-3">
                                <i class="fa fa-info-circle mr-1"></i>Last attached permit file deleted on: <strong>
                                    <asp:Label ID="lbl_permitdeleteddate" runat="server" Text="N/A"></asp:Label></strong> by <strong>
                                        <asp:Label ID="lbl_permitdeletedby" runat="server" Text="N/A"></asp:Label></strong>
                            </div>

                            <div class="modern-grid-container">
                                <div class="modern-table-wrapper">
                                    <asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-sm mb-0" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Permit Attached" OnRowDeleting="GridView1_RowDeleting" GridLines="Both">
                                        <Columns>
                                            <asp:TemplateField HeaderText="SL" HeaderStyle-Width="5%" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DB ID" HeaderStyle-Width="10%" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' /></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="JOBID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_JOBID" runat="server" Text='<%# Bind("JOBID") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="File Name" HeaderStyle-Width="40%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Name" runat="server" Text='<%# Eval("Name") %>' Font-Weight="Bold" ForeColor="#2c3e50" /></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="View" HeaderStyle-Width="10%" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("Id") %>' OnClick="DownloadFile" CssClass="btn btn-info btn-xs" ToolTip="Download"><i class="fa fa-download"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Time" HeaderStyle-Width="20%" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_TimeStamp" runat="server" Text='<%# Eval("TimeStamp") %>' /></ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel modern-panel">
                        <div class="x_title modern-title">
                            <h2 style="color: #34495e;"><i class="fa fa-users mr-2"></i>Attached Manpower Data</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="modern-grid-container">
                                <div class="modern-table-wrapper">
                                    <asp:GridView ID="GridView2" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-sm mb-0" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Manpower Added" OnRowCancelingEdit="GridView2_RowCancelingEdit" OnRowDeleting="GridView2_RowDeleting" OnRowEditing="GridView2_RowEditing" OnRowDataBound="GridView2_RowDataBound" OnRowUpdating="GridView2_RowUpdating" GridLines="Both">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl" HeaderStyle-Width="3%" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Id" runat="server" Text='<%# Bind("Id") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Region" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_JOB_Region" runat="server" Text='<%# Bind("JOB_Region") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="JOBID" HeaderStyle-Width="8%" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_JOBID" runat="server" Text='<%# Bind("JOBID") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="JOB Date" HeaderStyle-Width="8%" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_CreatedDate" runat="server" Text='<%# Bind("CreatedDate","{0:dd-MM-yyyy}") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="WRK" HeaderStyle-Width="6%" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_EmployeeWrk" runat="server" Font-Bold="true" Text='<%# Bind("EmployeeWrk") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_EmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="H" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_WourkHours" runat="server" Text='<%# Bind("WourkHours") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="IN Punch" HeaderStyle-Width="15%" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Inpunch_Time" runat="server" Text='<%# Bind("Inpunch_Time","{0:dd-MM-yyyy hh:mm tt}") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txt_Inpunch_Time" runat="server" class="form-control form-control-sm modern-input" Text='<%# DataBinder.Eval(Container.DataItem,"Inpunch_Time", "{0:yyyy-MM-dd hh:mm:ss tt}") %>'></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="OUT Punch" HeaderStyle-Width="15%" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Outpunch_Time" runat="server" Text='<%# Bind("Outpunch_Time","{0:dd-MM-yyyy hh:mm tt}") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txt_Outpunch_Time" runat="server" class="form-control form-control-sm modern-input" Text='<%# DataBinder.Eval(Container.DataItem,"Outpunch_Time", "{0:yyyy-MM-dd hh:mm:ss tt}") %>'></asp:TextBox></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Lunch" HeaderStyle-Width="6%" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_LunchFactor" runat="server" Text='<%# Bind("LunchFactor") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="DDL_LunchYesNo" runat="server" class="form-control form-control-sm modern-input" SelectedValue='<%# Bind("LunchFactor") %>'>
                                                        <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                        <asp:ListItem Value="No">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="POT" HeaderStyle-Width="5%" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_ProvidedOT" runat="server" Text='<%# Bind("ProvidedOT") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txt_ProvidedOT" runat="server" class="form-control form-control-sm modern-input text-center" Text='<%# DataBinder.Eval(Container.DataItem,"ProvidedOT") %>'></asp:TextBox>
                                                    <asp:CustomValidator ID="CustomValidator2" runat="server" ValidationGroup="Update" ControlToValidate="txt_ProvidedOT" Display="Dynamic" ErrorMessage="<=16" ForeColor="IndianRed" ClientValidationFunction="validateProvidedOT"></asp:CustomValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Status" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_AttendanceStatus" runat="server" Text='<%# Bind("AttendanceStatus") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="DDL_AttendanceStatus" class="form-control form-control-sm modern-input" runat="server"></asp:DropDownList></EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Code" HeaderStyle-Width="6%" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_AttendanceCode" runat="server" Text='<%# Bind("AttendanceCode") %>'></asp:Label></ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="DDL_AttendanceCode" class="form-control form-control-sm modern-input" runat="server"></asp:DropDownList></EditItemTemplate>
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

                            <div class="row mt-3">
                                <div class="col-md-6" runat="server" id="ResendApp_Div" visible="false">
                                    <div class="alert alert-warning d-flex align-items-center py-2">
                                        <asp:Button ID="btn_resendapp" runat="server" Text="Re-Send for Approval" CssClass="btn btn-warning btn-modern m-0 mr-3" OnClick="btn_resendapp_Click" />
                                        <asp:Label ID="lbl_resenddiv_msg" runat="server" CssClass="mb-0 font-weight-bold">JOB Rejected. Click to Resend.</asp:Label>
                                    </div>
                                </div>
                                <div class="col-md-6 text-right" runat="server" id="attachmanpowerrow" visible="false">
                                    <asp:Label ID="Label1" runat="server" CssClass="text-muted mr-3">Click to Attach Manpower</asp:Label>
                                    <asp:Button ID="btn_attachmanpower" runat="server" Text="Attach Manpower" CssClass="btn btn-primary btn-modern" OnClick="btn_attachmanpower_Click" />
                                </div>
                            </div>

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

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
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
