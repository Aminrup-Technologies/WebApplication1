<%--
What: ASPX Markup for Manage JOB IDs
Why: Provided complete revised version as per architecture standards.
When: 2026-06-21
Changed: No structural changes; maintained the ultra-compact filter ribbon UI.
--%>

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

                    <div class="page-title" style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 15px; border-bottom: 2px solid #1ABB9C; padding-bottom: 10px; flex-wrap: wrap;">

                        <div class="title_left mb-2 mb-md-0" style="flex: 1;">
                            <h3 style="margin: 0; color: #2a3f54; font-weight: 600; display: flex; align-items: center; flex-wrap: wrap; gap: 10px;">
                                <span><i class="fa fa-list-alt" style="margin-right: 8px; color: #1ABB9C;"></i>Manage JOB IDs</span>
                                <span class="badge shadow-sm" style="background: #e0f2f1; color: #1ABB9C; font-size: 13px; padding: 6px 12px; border: 1px solid #b2dfdb; font-weight: 600;">
                                    <i class="fa fa-calendar mr-1"></i>
                                    <asp:Label ID="lbl_month" runat="server"></asp:Label>
                                    <asp:Label ID="lbl_monthcode" Visible="false" runat="server"></asp:Label>
                                    <asp:Label ID="lbl_year" runat="server"></asp:Label>
                                </span>
                            </h3>
                        </div>

                        <div class="title_right d-flex justify-content-end align-items-center" style="display: flex; gap: 15px; flex-wrap: wrap;">
                            <div class="btn-group shadow-sm" role="group">
                                <asp:LinkButton ID="btn_prevmonth" runat="server" CssClass="btn btn-default btn-sm" OnClick="btn_prevmonth_Click" Style="font-weight: 600; color: #5a738e; background: #fff;">
                                    <i class="fa fa-chevron-left"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btn_currentdata" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btn_currentdata_Click" Style="background-color: #1ABB9C; border-color: #1ABB9C; font-weight: 600;">
                                    Current
                                </asp:LinkButton>
                                <asp:LinkButton ID="btn_nextmonth" runat="server" CssClass="btn btn-default btn-sm" OnClick="btn_nextmonth_Click" Style="font-weight: 600; color: #5a738e; background: #fff;">
                                    <i class="fa fa-chevron-right"></i>
                                </asp:LinkButton>
                            </div>
                            <a href="manage_jobid.aspx" class="modern-header-btn" style="padding: 5px 12px; font-size: 12px;">
                                <i class="fa fa-history"></i>OLD Version
                            </a>
                        </div>
                    </div>
                    <div class="clearfix"></div>

                    <div class="filter-ribbon shadow-sm" style="background: #ffffff; padding: 12px 15px; border-radius: 8px; border: 1px solid #e9ecef; margin-bottom: 20px;">
                        <div class="row" style="display: flex; align-items: center; flex-wrap: wrap;">

                            <div class="col-md-3 col-sm-6 mb-2 mb-md-0">
                                <div class="input-group" style="margin-bottom: 0;">
                                    <span class="input-group-addon" style="background: #f8f9fa; border-color: #dce1e5; color: #1ABB9C;" title="Filter by Status"><i class="fa fa-info-circle"></i></span>
                                    <asp:DropDownList ID="DDL_JobStatus" CssClass="form-control modern-input" runat="server" Style="height: 34px !important; border-left: none;">
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
                            </div>

                            <div class="col-md-3 col-sm-6 mb-2 mb-md-0">
                                <div class="input-group" style="margin-bottom: 0;">
                                    <span class="input-group-addon" style="background: #f8f9fa; border-color: #dce1e5; color: #1ABB9C;" title="Filter by Job Type"><i class="fa fa-tags"></i></span>
                                    <asp:DropDownList ID="DDL_BillingType" runat="server" CssClass="form-control modern-input" Style="height: 34px !important; border-left: none;"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-8 mb-2 mb-sm-0">
                                <div class="input-group" style="margin-bottom: 0;">
                                    <span class="input-group-addon" style="background: #f8f9fa; border-color: #dce1e5; color: #1ABB9C;" title="Quick Text Search"><i class="fa fa-search"></i></span>
                                    <asp:TextBox ID="txt_quicksearch" runat="server" CssClass="form-control modern-input" placeholder="Type to filter grid instantly..." onkeyup="filterGrid()" Style="height: 34px !important; border-left: none;"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-md-2 col-sm-4 text-right">
                                <asp:LinkButton ID="btn_reset" runat="server" CssClass="btn btn-default btn-sm" OnClick="btn_reset_Click" Style="border-radius: 20px; font-weight: 600; padding: 6px 12px; margin-bottom: 0; border-color: #dce1e5;" title="Clear Filters">
                                    <i class="fa fa-refresh text-warning"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btn_submit" runat="server" CssClass="btn btn-success btn-sm shadow-sm" OnClick="btn_submit_Click" Style="border-radius: 20px; font-weight: 600; padding: 6px 15px; margin-bottom: 0; background-color: #1ABB9C; border-color: #1ABB9C;">
                                    <i class="fa fa-filter mr-1"></i> Fetch
                                </asp:LinkButton>
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
                                                    </strong>
                                                    <br />
                                                    <span class="text-muted" style="font-size: 12px; white-space: nowrap;"><i class="fa fa-map-marker mr-1"></i><%# Eval("JOB_Location") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Site Approver" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_JOB_InchargeName" runat="server" Font-Bold="true" ForeColor="#34495e" Text='<%# Eval("JOB_InchargeName") %>' Style="white-space: nowrap;" /><br />
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
                                                    <asp:Label ID="lbl_JOB_PermitNo" runat="server" Font-Bold="true" Text='<%# Eval("JOB_PermitNo") %>' Style="padding: 4px 8px; font-size: 12px; white-space: nowrap;" />
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
                                                <i class="fa fa-info-circle fa-2x mb-2"></i>
                                                <br />
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

        // Show Loading State during Async Postbacks
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_beginRequest(function (sender, args) {
            // Change cursor to waiting
            document.body.style.cursor = 'wait';

            // Find the submit button and disable it temporarily
            var fetchBtn = document.getElementById('<%= btn_submit.ClientID %>');
            if (fetchBtn) {
                fetchBtn.innerHTML = '<i class="fa fa-spinner fa-spin mr-1"></i> Loading...';
                fetchBtn.style.pointerEvents = 'none';
                fetchBtn.style.opacity = '0.6';
            }
        });

        prm.add_endRequest(function (sender, args) {
            // Restore cursor and button
            document.body.style.cursor = 'default';

            var fetchBtn = document.getElementById('<%= btn_submit.ClientID %>');
            if (fetchBtn) {
                fetchBtn.innerHTML = '<i class="fa fa-filter mr-1"></i> Fetch Data';
                fetchBtn.style.pointerEvents = 'auto';
                fetchBtn.style.opacity = '1';
            }

            // Re-apply quick search filter
            filterGrid();
        });
    </script>
</asp:Content>