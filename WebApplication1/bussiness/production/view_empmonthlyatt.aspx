<%@ Page Title="Employee Monthly Analytics" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="view_empmonthlyatt.aspx.cs" Inherits="WebApplication1.bussiness.production.view_empmonthlyatt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Modernized Gentelella Panel Styling */
        .modern-panel {
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.05);
            border: none;
        }

            .modern-panel .x_title {
                border-bottom: 2px solid #E6E9ED;
                padding-bottom: 10px;
            }

        /* Heatmap Styling */
        .heatmap-container {
            display: flex;
            flex-wrap: wrap;
            gap: 6px;
            margin-top: 15px;
            margin-bottom: 20px;
        }

        .heatmap-box {
            width: 38px;
            height: 38px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 12px;
            font-weight: bold;
            color: white;
            border-radius: 6px;
            cursor: help;
            box-shadow: inset 0 -2px 0 rgba(0,0,0,0.15);
            transition: transform 0.2s;
        }

            .heatmap-box:hover {
                transform: scale(1.1);
            }

        /* Shift Intensity Colors */
        .bg-gray {
            background-color: #BDC3C7;
            color: #fff;
        }
        /* Missing Punch / No Data */
        .bg-info {
            background-color: #3498DB;
            color: #fff;
        }
        /* Sunday / Weekly Off */
        .bg-black {
            background-color: #1A1A1A !important;
            color: #fff !important;
        }
        /* Explicit ABSENT Code */
        .bg-green {
            background-color: #2ECC71;
            color: #fff;
        }
        /* Normal Shift */
        .bg-yellow {
            background-color: #F1C40F;
            color: #333;
        }
        /* OT Shift */
        .bg-red {
            background-color: #E74C3C;
            color: #fff;
        }
        /* Compliance/Double Shift */

        /* Custom Calendar Styling */
        .att-calendar {
            width: 100%;
            max-width: 100%;
            background: #fff;
            border-collapse: separate;
            border-spacing: 2px;
        }

            .att-calendar th {
                text-align: center;
                background: #2C3E50;
                color: white;
                padding: 12px;
                border-radius: 4px;
            }

            .att-calendar td {
                height: 65px;
                text-align: center;
                vertical-align: middle;
                font-weight: bold;
                border-radius: 4px;
                border: 1px solid #f0f0f0;
                transition: 0.2s;
                cursor: help;
            }

                .att-calendar td:hover {
                    filter: brightness(0.9);
                }

        .highlight-row {
            background-color: #FFF3CD !important; /* Soft Warning Yellow */
            box-shadow: inset 5px 0 0 #F1C40F;
            transition: background-color 0.8s ease;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="page-title">
            <div class="title_left">
                <h4><i class="fa fa-line-chart"></i>Monthly Work Analytics</h4>
            </div>
        </div>
        <div class="clearfix"></div>

        <!-- SEARCH FILTERS -->
        <div class="row">
            <div class="col-md-12 col-sm-12">
                <div class="x_panel">
                    <div class="x_content">
                        <div class="row">
                            <div class="col-md-2 form-group">
                                <label>Month / Year</label>
                            </div>
                            <div class="col-md-2 form-group">
                                <asp:DropDownList ID="DDL_Day" runat="server" Visible="false"></asp:DropDownList>
                                <asp:DropDownList ID="DDL_Month" CssClass="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
                            </div>
                            <div class="col-md-2 form-group">
                                <asp:DropDownList ID="DDL_Year" CssClass="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
                            </div>

                            <div class="col-md-2 form-group">
                                <label>Search By</label>
                            </div>
                            <div class="col-md-2 form-group">
                                <asp:DropDownList ID="DDL_SearchType" runat="server" class="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_SearchType_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Value="0">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">By Employee Name</asp:ListItem>
                                    <asp:ListItem Value="2">By Workman SL</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <div class="col-md-2" id="Nameinputrow1" runat="server" visible="false">
                                <label style="margin-top: 5px;">Employee Name</label>
                            </div>
                            <div class="col-md-2" id="Nameinputrow2" runat="server" visible="false">
                                <asp:TextBox ID="txt_empname" class="form-control form-control-sm" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row mt-2">
                            <div class="col-md-2" id="WorkmanInput_Row1" runat="server" visible="false">
                                <label style="margin-top: 5px;">Workman SL</label>
                            </div>
                            <div class="col-md-2" id="WorkmanInput_Row2" runat="server" visible="false">
                                <asp:TextBox ID="txt_empworkman" class="form-control form-control-sm" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row mt-3 text-center">
                            <div class="col-md-12">
                                <asp:Button ID="btn_search" runat="server" Text="Analyze Month" CssClass="btn btn-success" OnClick="btn_search_Click" />
                                <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="btn btn-warning" OnClick="btn_reset_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- ANALYTICS DASHBOARD -->
        <asp:Panel ID="pnlAnalytics" runat="server" Visible="false">

            <!-- TIER 1: KPI CARDS -->
            <div class="row top_tiles">
                <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                    <div class="tile-stats">
                        <div class="count">
                            <asp:Label ID="lblTotalShifts" runat="server"></asp:Label>
                        </div>
                        <h3>Total Shifts</h3>
                    </div>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                    <div class="tile-stats">
                        <div class="count">
                            <asp:Label ID="lblRegHours" runat="server"></asp:Label>
                        </div>
                        <h3>Regular Hours</h3>
                    </div>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                    <div class="tile-stats">
                        <div class="count text-danger">
                            <asp:Label ID="lblOTHours" runat="server"></asp:Label>
                        </div>
                        <h3>Overtime Hours</h3>
                    </div>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                    <div class="tile-stats">
                        <div class="count text-warning">
                            <asp:Label ID="lblRestViolations" runat="server"></asp:Label>
                        </div>
                        <h3>Rest Violations (< 8h Gap)</h3>
                    </div>
                </div>
            </div>

            <!-- TIER 2 & TIER 4: TIMELINE & CALENDAR -->
            <div class="row">
                <!-- Shift Timeline (Heatmap) -->
                <div class="col-md-7">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Shift Intensity Timeline <small>(Hover for details)</small></h2>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="heatmap-container">
                                <asp:Repeater ID="rptHeatmap" runat="server">
                                    <ItemTemplate>
                                        <div class='heatmap-box <%# Eval("ColorClass") %> clickable-day' title='<%# Eval("TooltipDetails") %>' data-date='<%# Eval("FullDate") %>'>
                                            <%# Eval("DayNum") %>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <p class="text-muted small">
                                <span class="badge bg-black">Absent</span>
                                <span class="badge bg-gray">Off/No Data</span>
                                <span class="badge bg-green">Standard Shift</span>
                                <span class="badge bg-yellow">OT Shift (9-12h)</span>
                                <span class="badge bg-red">Compliance Risk (12h+)</span>
                            </p>
                        </div>
                    </div>
                </div>

                <!-- Monthly Attendance Calendar -->
                <div class="col-md-5">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Attendance Calendar</h2>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content text-center">
                            <asp:Calendar ID="AttendanceCalendar" runat="server" CssClass="att-calendar"
                                OnDayRender="AttendanceCalendar_DayRender" ShowGridLines="true"
                                NextPrevFormat="ShortMonth" DayNameFormat="Short"></asp:Calendar>
                        </div>
                    </div>
                </div>
            </div>

            <!-- TIER 3: DETAILED LEDGER WITH GAPS & BLACK BOX -->
            <div class="row">
                <div class="col-md-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Monthly Ledger & Rest Gaps</h2>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content table-responsive">

                            <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped table-bordered dt-responsive nowrap"
                                AutoGenerateColumns="false" Width="100%"
                                OnRowDataBound="GridView1_RowDataBound"
                                OnRowEditing="GridView1_RowEditing"
                                OnRowCancelingEdit="GridView1_RowCancelingEdit"
                                OnRowUpdating="GridView1_RowUpdating"
                                OnRowDeleting="GridView1_RowDeleting"
                                DataKeyNames="Id">
                                <Columns>
                                    <asp:TemplateField HeaderText="Date & Job">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lbl_JOBID" runat="server" Text='<%# Eval("JOBID") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lbl_JOB_Region" runat="server" Text='<%# Eval("JOB_Region") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lbl_EmployeeWrk" runat="server" Text='<%# Eval("EmployeeWrk") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lbl_WourkHours" runat="server" Text='<%# Eval("WourkHours") %>' Visible="false"></asp:Label>

                                            <strong><%# Eval("CreatedDate", "{0:dd-MMM-yyyy}") %></strong><br />
                                            <span class="text-muted small"><i class="fa fa-briefcase"></i><%# Eval("JOBID") %></span><br />

                                            <div class="btn-group" style="margin-top: 5px;">
                                                <a href='jobapprovalpage.aspx?JOBID=<%# Eval("JOBID") %>' target="_blank" class="btn btn-primary btn-xs" title="Approve Timesheet (V1)">
                                                    <i class="fa fa-check-square-o"></i>Approve
                                                </a>
                                                <a href='job_360_view.aspx?jobid=<%# Eval("JOBID") %>' target="_blank" class="btn btn-info btn-xs" title="360 Control Tower (V2)">
                                                    <i class="fa fa-search"></i>360
                                                </a>
                                            </div>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lbl_JOBID" runat="server" Text='<%# Eval("JOBID") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lbl_JOB_Region" runat="server" Text='<%# Eval("JOB_Region") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lbl_EmployeeWrk" runat="server" Text='<%# Eval("EmployeeWrk") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lbl_WourkHours" runat="server" Text='<%# Eval("WourkHours") %>' Visible="false"></asp:Label>

                                            <strong><%# Eval("CreatedDate", "{0:dd-MMM-yyyy}") %></strong><br />
                                            <span class="text-muted small"><i class="fa fa-briefcase"></i><%# Eval("JOBID") %></span>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Punches">
                                        <ItemTemplate>
                                            <span class="text-success small"><i class="fa fa-sign-in"></i>IN: <%# Eval("Inpunch_Time", "{0:dd-MMM hh:mm tt}") %></span><br />
                                            <span class="text-danger small"><i class="fa fa-sign-out"></i>OUT: <%# Eval("Outpunch_Time", "{0:dd-MMM hh:mm tt}") %></span><br />

                                            <span class='<%# Convert.ToInt32(Eval("Rest_Gap_Hours") == DBNull.Value ? 99 : Eval("Rest_Gap_Hours")) < 8 ? "label label-danger mt-1" : "label label-success mt-1" %>' style="display: inline-block; margin-top: 3px;">
                                                <i class="fa fa-bed"></i>Gap: <%# Eval("Rest_Gap_Hours") == DBNull.Value ? "N/A" : Eval("Rest_Gap_Hours") + " Hrs" %>
                                            </span>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <label class="small mb-0">IN Punch</label>
                                            <asp:TextBox ID="txt_Inpunch_Time" runat="server" CssClass="form-control input-sm mb-1" Text='<%# Eval("Inpunch_Time", "{0:yyyy-MM-dd hh:mm:ss tt}") %>'></asp:TextBox>
                                            <label class="small mb-0">OUT Punch</label>
                                            <asp:TextBox ID="txt_Outpunch_Time" runat="server" CssClass="form-control input-sm" Text='<%# Eval("Outpunch_Time", "{0:yyyy-MM-dd hh:mm:ss tt}") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Hours">
                                        <ItemTemplate>
                                            <span class="small"><b>Worked:</b> <%# Eval("WorkedHours") %> h</span><br />
                                            <span class="small text-danger"><b>OT Given:</b> <%# Eval("ProvidedOT") %> h</span>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <label class="small mb-0">OT Given</label>
                                            <asp:TextBox ID="txt_ProvidedOT" runat="server" CssClass="form-control input-sm mb-1" Text='<%# Eval("ProvidedOT") %>'></asp:TextBox>
                                            <label class="small mb-0">Lunch Factor?</label>
                                            <asp:DropDownList ID="DDL_LunchYesNo" runat="server" CssClass="form-control input-sm">
                                                <asp:ListItem Value="Yes" Selected="True">Yes</asp:ListItem>
                                                <asp:ListItem Value="No">No</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <span class="label <%# Eval("StatusBadgeClass") %>"><%# Eval("AttendanceStatus") %></span>
                                            <span class="font-weight-bold ml-2">[<%# Eval("AttendanceCode") %>]</span>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <label class="small mb-0">Status</label>
                                            <asp:DropDownList ID="DDL_AttendanceStatus" runat="server" CssClass="form-control input-sm mb-1"></asp:DropDownList>
                                            <label class="small mb-0">Code</label>
                                            <asp:DropDownList ID="DDL_AttendanceCode" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-warning btn-xs" ToolTip="Edit Shift">
                    <i class="fa fa-pencil"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-danger btn-xs" ToolTip="Delete Shift" OnClientClick="return confirm('Are you sure you want to completely DELETE this record?');">
                    <i class="fa fa-trash"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" CssClass="btn btn-success btn-xs mb-1" ToolTip="Save Changes" Style="width: 100%;">
                    <i class="fa fa-save"></i> Save
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-default btn-xs" ToolTip="Cancel Edit" Style="width: 100%;">
                    <i class="fa fa-times"></i> Cancel
                                            </asp:LinkButton>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="alert alert-warning">No records found for this month.</div>
                                </EmptyDataTemplate>
                            </asp:GridView>

                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            // Listen for clicks on the Heatmap or the Calendar
            $(document).on('click', '.clickable-day', function () {
                var targetDate = $(this).attr('data-date');
                if (!targetDate) return;

                var targetRow = $('#row-' + targetDate);

                // If a corresponding row exists in the Ledger
                if (targetRow.length > 0) {

                    // 1. Smoothly scroll the page down to the ledger row
                    $('html, body').animate({
                        scrollTop: targetRow.offset().top - 150 // Offset to clear the fixed top navigation bar
                    }, 600);

                    // 2. Remove highlight from any previously clicked rows
                    $('.table tr').removeClass('highlight-row');

                    // 3. Add the highlight animation to the targeted row
                    targetRow.addClass('highlight-row');

                    // 4. Remove the highlight after 3 seconds so it doesn't stay yellow forever
                    setTimeout(function () {
                        targetRow.removeClass('highlight-row');
                    }, 3000);
                }
                else {
                    // Optional: Show a subtle PNotify toast if they click a rest day with no ledger data
                    if (typeof new PNotify === 'function') {
                        new PNotify({
                            title: 'No Data',
                            text: 'No timesheet record exists for ' + targetDate,
                            type: 'info',
                            styling: 'bootstrap3',
                            delay: 2000
                        });
                    } else {
                        alert('No timesheet record exists for ' + targetDate);
                    }
                }
            });
        });
    </script>
</asp:Content>
