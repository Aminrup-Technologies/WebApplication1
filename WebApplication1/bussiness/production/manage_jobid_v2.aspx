<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="manage_jobid_v2.aspx.cs" Inherits="WebApplication1.bussiness.production.manage_jobid_v2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /* Base typography and layout */
        .top-label {
            font-weight: 600;
            color: #2a3f54;
            font-size: 13px;
            letter-spacing: 0.3px;
        }

        /* Modern Panel Styling */
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

        /* Grid Action Buttons */
        .grid-action-btn {
            padding: 4px 10px;
            font-size: 12px;
            margin: 2px;
            border-radius: 15px;
            font-weight: 600;
            letter-spacing: 0.3px;
            transition: all 0.2s ease;
            white-space: nowrap; /* Prevent button text wrapping on mobile */
        }
        .grid-action-btn:hover {
            transform: translateY(-1px);
            box-shadow: 0 2px 5px rgba(0,0,0,0.15);
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

        /* Responsive GridView Container & Grid Lines */
        .modern-grid-container {
            border: 1px solid #e9ecef;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.02);
            background: #fff;
            margin-bottom: 20px;
        }
        
        /* Mobile Scrolling Wrapper */
        .modern-table-wrapper {
            overflow-x: auto;
            -webkit-overflow-scrolling: touch;
            width: 100%;
            border-radius: 8px;
        }
        
        /* Enforce minimum width to prevent text squishing on mobile */
        .modern-table-wrapper table {
            min-width: 1000px;
            margin-bottom: 0;
        }

        .modern-grid-container th {
            background-color: #f8f9fa !important;
            color: #34495e !important;
            font-weight: 700;
            border: 1px solid #e9ecef !important; /* Explicit Grid Lines */
            padding: 12px 8px !important;
            white-space: nowrap; /* Keep headers on one line */
        }

        .modern-grid-container td {
            vertical-align: middle !important;
            padding: 10px 8px !important;
            border: 1px solid #e9ecef !important; /* Explicit Grid Lines */
        }
        
        /* Keep badges on one line */
        .badge {
            white-space: nowrap;
        }
        
        /* Month Navigation Pill */
        .nav-pill-group .btn {
            border-radius: 0;
            padding: 8px 20px;
            font-weight: 600;
            font-size: 13px;
        }
        .nav-pill-group .btn:first-child {
            border-top-left-radius: 20px;
            border-bottom-left-radius: 20px;
        }
        .nav-pill-group .btn:last-child {
            border-top-right-radius: 20px;
            border-bottom-right-radius: 20px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="container-fluid pl-0 pr-0">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <div class="page-title" style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 20px; border-bottom: 1px solid #e6e9ed; padding-bottom: 10px; flex-wrap: wrap;">
                        <div class="title_left mb-2 mb-md-0">
                            <h3 style="margin: 0; color: #2a3f54; font-weight: 600;">
                                <i class="fa fa-list-alt text-primary" style="margin-right: 8px; color: #1ABB9C !important;"></i>Manage JOB IDs - 
                                <span style="color: #1ABB9C;">
                                    <asp:Label ID="lbl_month" runat="server"></asp:Label>
                                    <asp:Label ID="lbl_monthcode" Visible="false" runat="server"></asp:Label>,
                                    <asp:Label ID="lbl_year" runat="server"></asp:Label>
                                </span>
                            </h3>
                        </div>
                        <div class="title_right text-md-right text-left">
                            <a href="manage_jobid.aspx" class="modern-header-btn">
                                <i class="fa fa-history" style="margin-right: 5px;"></i> Switch to OLD Version
                            </a>
                        </div>
                    </div>
                    <div class="clearfix"></div>

                    <div class="row">
                        <div class="col-md-12 col-sm-12">
                            <div class="x_panel modern-panel">

                                <div class="x_title modern-title border-bottom-0 pb-0">
                                    <h2 style="color: #2980b9; font-weight: 600;"><i class="fa fa-filter" style="margin-right: 6px;"></i>Search & Filter Records</h2>
                                    <ul class="nav navbar-right panel_toolbox">
                                        <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                                    </ul>
                                    <div class="clearfix"></div>
                                </div>

                                <div class="x_content pt-3">
                                    
                                    <div class="row mb-4">
                                        <div class="col-12 d-flex justify-content-center">
                                            <div class="btn-group nav-pill-group shadow-sm" role="group" aria-label="Month Navigation">
                                                <asp:LinkButton ID="btn_prevmonth" runat="server" CssClass="btn btn-outline-secondary" OnClick="btn_prevmonth_Click">
                                                    <i class="fa fa-chevron-left mr-1"></i> Prev Month
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btn_currentdata" runat="server" CssClass="btn btn-primary" OnClick="btn_currentdata_Click" style="background-color: #1ABB9C; border-color: #1ABB9C; color: white;">
                                                    <i class="fa fa-calendar mr-1"></i> Current Month
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btn_nextmonth" runat="server" CssClass="btn btn-outline-secondary" OnClick="btn_nextmonth_Click">
                                                    Next Month <i class="fa fa-chevron-right ml-1"></i>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row align-items-end" style="background-color: #f8f9fa; padding: 20px 15px; border-radius: 8px; border: 1px solid #e9ecef;">

                                        <div class="col-md-3 col-sm-6 form-group mb-md-0 mb-3">
                                            <label class="top-label"><i class="fa fa-info-circle mr-1"></i> JOB Status</label>
                                            <asp:DropDownList ID="DDL_JobStatus" CssClass="form-control modern-input" runat="server">
                                                <asp:ListItem Value="0" Selected="True">-- All Statuses --</asp:ListItem>
                                                <asp:ListItem Value="1">JOBID - Active</asp:ListItem>
                                                <asp:ListItem Value="2">JOBID - Blocked</asp:ListItem>
                                                <asp:ListItem Value="3">Permit - Pending</asp:ListItem>
                                                <asp:ListItem Value="4">Permit - Uploaded</asp:ListItem>
                                                <asp:ListItem Value="5">Approval - Pending</asp:ListItem>
                                                <asp:ListItem Value="6">Approval - Done</asp:ListItem>
                                                <asp:ListItem Value="7">Approval - Returned</asp:ListItem>
                                                <asp:ListItem Value="8">Approval - Rejected</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>

                                        <div class="col-md-3 col-sm-6 form-group mb-md-0 mb-3">
                                            <label class="top-label"><i class="fa fa-tags mr-1"></i> JOB Type</label>
                                            <asp:DropDownList ID="DDL_BillingType" runat="server" CssClass="form-control modern-input"></asp:DropDownList>
                                        </div>

                                        <div class="col-md-3 col-sm-12 form-group mb-md-0 mb-3">
                                            <label class="top-label"><i class="fa fa-search mr-1"></i> Quick Search</label>
                                            <asp:TextBox ID="txt_quicksearch" runat="server" CssClass="form-control modern-input" placeholder="Type to filter grid..." onkeyup="filterGrid()"></asp:TextBox>
                                        </div>

                                        <div class="col-md-3 col-sm-12 text-md-right text-center mt-2 mt-md-0">
                                            <asp:LinkButton ID="btn_reset" runat="server" CssClass="btn btn-outline-warning btn-modern mr-2" OnClick="btn_reset_Click">
                                                <i class="fa fa-refresh mr-1"></i> Reset
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btn_submit" runat="server" CssClass="btn btn-success btn-modern shadow-sm" OnClick="btn_submit_Click">
                                                <i class="fa fa-filter mr-1"></i> Fetch Data
                                            </asp:LinkButton>
                                        </div>

                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12 col-sm-12">
                            <div class="modern-grid-container">
                                <div class="modern-table-wrapper" style="max-height: 550px;">
                                    <asp:GridView ID="GridView1" runat="server" class="table table-striped table-hover table-bordered table-sm mb-0" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand" GridLines="Both">
                                        <Columns>
                                            <asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="2%" ItemStyle-CssClass="text-center text-muted">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_slno" runat="server" Font-Bold="true" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="DBID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' />
                                                    <asp:Label ID="lbl_Creator_Workman" runat="server" Text='<%# Eval("Creator_Workman") %>' />
                                                    <asp:Label ID="lbl_JOBID" runat="server" Text='<%# Eval("JOBID") %>' />
                                                    <asp:Label ID="lbl_JOBID_Status" runat="server" Text='<%# Eval("JOBID_Status") %>' />
                                                    <asp:Label ID="lbl_FinalUpldStatus" runat="server" Text='<%# Eval("FinalUpldStatus") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Date" HeaderStyle-Width="8%" ItemStyle-CssClass="text-center font-weight-bold" ItemStyle-ForeColor="#34495e">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_CreatedDate" runat="server" Text='<%# Eval("CreatedDate","{0:dd-MMM-yyyy}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="JOBID" HeaderStyle-Width="10%" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Button ID="btn_viewdetails" runat="server" Text='<%# Eval("JOBID") %>' CssClass="btn btn-info grid-action-btn" CommandName="View_Details" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="View Full Details" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="System Status" HeaderStyle-Width="10%" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Button ID="btn_jobidstatus" runat="server" Text='<%# Eval("JOBID_Status") %>' CssClass="btn grid-action-btn" CommandName="Swap_JOBIDStatus" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Click to Toggle Status" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Site / Location" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <strong style="color: #2c3e50;">
                                                        <asp:Label ID="lbl_JOB_Site" runat="server" Text='<%# Eval("JOB_Site") %>' />
                                                    </strong><br />
                                                    <span class="text-muted" style="font-size: 12px; white-space: nowrap;"><i class="fa fa-map-marker mr-1"></i><%# Eval("JOB_Location") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Site Approver" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_JOB_InchargeName" runat="server" Font-Bold="true" ForeColor="#34495e" Text='<%# Eval("JOB_InchargeName") %>' style="white-space: nowrap;" /><br />
                                                    <asp:Label ID="lbl_Incharge_Approval" runat="server" CssClass="small font-weight-bold" Text='<%# Eval("Incharge_Approval") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Shift" HeaderStyle-Width="4%" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_JOB_Shift" runat="server" CssClass="badge bg-secondary" Text='<%# Eval("JOB_Shift") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Permit No" HeaderStyle-Width="10%" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_JOB_PermitNo" runat="server" Font-Bold="true" Text='<%# Eval("JOB_PermitNo") %>' style="padding: 4px 8px; font-size: 12px; white-space: nowrap;" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="MP" HeaderStyle-Width="4%" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <span class="badge bg-blue" style="font-size: 12px; padding: 4px 6px;"><i class="fa fa-users mr-1"></i><%# Eval("ManpowerCount") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="JOB Title" HeaderStyle-Width="17%">
                                                <ItemTemplate>
                                                    <span title='<%# Eval("JOB_Title") %>' class="text-muted" style="font-size: 12px;">
                                                        <%# Eval("JOB_Title").ToString().Length > 45 ? Eval("JOB_Title").ToString().Substring(0,45) + "..." : Eval("JOB_Title") %>
                                                    </span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Action" HeaderStyle-Width="5%" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btndelete" runat="server" CommandName="DeleteRec" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn btn-outline-danger grid-action-btn" ToolTip="Permanently Delete JOB" OnClientClick="return confirm('WARNING: Do you want to permanently DELETE this JOB and all associated data (Attendance, Permits)?');">
                                                        <i class="fa fa-trash"></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div class="alert alert-info text-center mt-3" style="background-color: #ebf5fb; color: #2980b9; border: none; padding: 20px;">
                                                <i class="fa fa-info-circle fa-2x mb-2"></i><br />
                                                <strong>No Data Found</strong> for the selected month and filters.
                                            </div>
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

    <script type="text/javascript">
        function showPNotify(title, text, type) {
            new PNotify({
                title: title, text: text, type: type, styling: 'bootstrap3', delay: 4000
            });
        }

        // =========================================================
        // CLIENT-SIDE QUICK SEARCH FILTER
        // =========================================================
        function filterGrid() {
            var input = document.getElementById('<%= txt_quicksearch.ClientID %>');
            var filter = input.value.toUpperCase();
            var table = document.getElementById('<%= GridView1.ClientID %>');

            if (!table) return;
            var tr = table.getElementsByTagName("tr");

            for (var i = 1; i < tr.length; i++) {
                var rowContainsFilter = false;
                var td = tr[i].getElementsByTagName("td");

                for (var j = 0; j < td.length; j++) {
                    if (td[j]) {
                        var txtValue = td[j].textContent || td[j].innerText;
                        if (txtValue.toUpperCase().indexOf(filter) > -1) {
                            rowContainsFilter = true;
                            break; 
                        }
                    }
                }

                if (rowContainsFilter) {
                    tr[i].style.display = "";
                } else {
                    tr[i].style.display = "none";
                }
            }
        }

        // Re-apply the filter automatically if the UpdatePanel refreshes the grid
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            filterGrid();
        });
    </script>
</asp:Content>