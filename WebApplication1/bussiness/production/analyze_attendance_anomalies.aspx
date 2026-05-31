<%@ Page Title="Attendance Anomalies Audit" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="analyze_attendance_anomalies.aspx.cs" Inherits="WebApplication1.bussiness.production.analyze_attendance_anomalies" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- DataTables CSS -->
    <link href="../vendors/datatables.net-bs/css/dataTables.bootstrap.min.css" rel="stylesheet">
    <style>
        .clickable-card {
            transition: transform 0.2s;
            cursor: pointer;
            text-decoration: none !important;
            color: inherit;
        }

            .clickable-card:hover {
                transform: translateY(-5px);
                box-shadow: 0 6px 12px rgba(0,0,0,0.15);
            }

        .active-filter-card .tile-stats {
            background-color: #f1f7fd;
            border: 2px solid #3498db;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-12">
                        <div class="x_panel">
                            <div class="x_content">
                                <div class="row">
                                    <div class="col-md-2">
                                        <label>From Date</label>
                                        <asp:TextBox ID="txtFromDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <label>To Date</label>
                                        <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <label>Work Region</label>
                                        <asp:DropDownList ID="ddlRegion" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-6" style="margin-top: 24px;">
                                        <asp:Button ID="btnSearch" runat="server" Text="Filter Audit" CssClass="btn btn-sm btn-primary" OnClick="btnSearch_Click" />
                                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-sm btn-danger" OnClick="btnClear_Click" />
                                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-sm btn-warning" PostBackUrl="~/bussiness/production/anlys_jobsdeta.aspx" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row top_tiles" style="margin: 10px 0;">
                    <asp:HiddenField ID="hfSelectedAnomaly" runat="server" Value="ALL" />

                    <asp:Repeater ID="rptSummary" runat="server" OnItemCommand="rptSummary_ItemCommand">
                        <ItemTemplate>
                            <div class="col-lg-3 col-md-4 col-sm-6 col-xs-12">
                                <asp:LinkButton ID="lnkCard" runat="server" CommandName="FilterCategory" CommandArgument='<%# Eval("Anomaly_Type") %>' CssClass='<%# Eval("Anomaly_Type").ToString() == hfSelectedAnomaly.Value ? "clickable-card active-filter-card" : "clickable-card" %>'>
                                    <div class="tile-stats" style="padding:15px; border-left: 4px solid <%# Eval("Severity_Color") %>;">
                                        <div class="count"><%# Eval("Anomaly_Count") %></div>
                                        <h3 style="font-size:13px; font-weight:bold; white-space:normal;"><%# Eval("Anomaly_Type") %></h3>
                                        <p style="margin-bottom:0; color:<%# Eval("Severity_Color") %>; font-weight:bold;"><%# Eval("Severity_Level") %></p>
                                    </div>
                                </asp:LinkButton>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <div class="x_panel">
                    <div class="x_title d-flex justify-content-between align-items-center">
                        <h2>Anomaly Ledger
                            <asp:Label ID="lblCurrentFilter" runat="server" CssClass="text-danger"></asp:Label></h2>

                        <!-- EXPORT BUTTON -->
                        <asp:LinkButton ID="btnExportExcel" runat="server" CssClass="btn btn-success btn-sm pull-right" OnClick="btnExportExcel_Click" OnClientClick="showPNotify('Exporting', 'Generating Excel file...', 'info');">
                            <i class="fa fa-file-excel-o"></i> Export Ledger
                        </asp:LinkButton>
                        <div class="clearfix"></div>
                    </div>

                    <div class="x_content table-responsive">
                        <asp:GridView ID="gvAnomalies" runat="server" AutoGenerateColumns="False"
                            CssClass="table table-striped table-bordered table-hover dt-responsive nowrap" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="SL" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="1%">
                                    <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Job Reference" HeaderStyle-Width="12%">
                                    <ItemTemplate>
                                        <a href='job_360_view.aspx?jobid=<%# Eval("JOBID") %>' target="_blank" class="text-primary font-weight-bold" style="font-size: 13px; text-decoration: underline;">
                                            <i class="fa fa-briefcase"></i><%# Eval("JOBID") %>
                                        </a>
                                        <br />
                                        <span class="text-muted small">
                                            <i class="fa fa-calendar"></i>Date: <strong><%# Eval("Job_Date") %></strong>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Employee & Site" HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <a href='view_empmonthlyatt.aspx?empwrk=<%# Eval("EmployeeWrk") %>&month=<%# Eval("CreatedMonth") %>&year=<%# Eval("CreatedYear") %>'
                                            target="_blank" class="font-weight-bold" style="color: #2C3E50; text-decoration: underline; font-size: 13px;">
                                            <i class="fa fa-user"></i><%# Eval("Employee_Details") %>
                                        </a>
                                        <br />
                                        <span class="text-muted small">
                                            <i class="fa fa-map-marker"></i><%# Eval("Worksite") %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Punches & Code" HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <span class="label label-warning" style="font-size: 11px;">Code: <%# Eval("AttendanceCode") %></span>
                                        <br />
                                        <span class="text-muted" style="display: inline-block; margin-top: 4px;">
                                            <i class="fa fa-sign-in text-success"></i>IN: <%# Eval("IN_Time") %><br />
                                            <i class="fa fa-sign-out text-danger"></i>OUT: <%# Eval("OUT_Time") %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Hours & OT Breakdown" HeaderStyle-Width="18%">
                                    <ItemTemplate>
                                        <div style="line-height: 1.6;">
                                            <!-- Registered Hours -->
                                            <span class="small text-muted" title="Registered Shift Hours">
                                                <i class="fa fa-clock-o"></i><b>Reg Hrs:</b> <%# Eval("Registered_Hours") %>
                                            </span>
                                            |
                                            <!-- Actually Worked Hours -->
                                            <span class="small text-primary" title="Actually Worked Hours">
                                                <i class="fa fa-hourglass-half"></i><b>Worked:</b> <%# Eval("WorkedHours") %>
                                            </span>
                                            <br />
                                            <!-- System Calculated OT -->
                                            <span class="small" title="System Calculated Overtime">
                                                <i class="fa fa-calculator text-muted"></i><b>Sys OT:</b> <%# Eval("System_OT") %>
                                            </span>
                                            |
                                            <!-- Manually Given OT (Highlighted in Red for auditing) -->
                                            <span class="small text-danger font-weight-bold" title="Manually Given Overtime">
                                                <i class="fa fa-edit"></i><b>Given OT:</b> <%# Eval("Final_OT") %>
                                            </span>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Audit Trail" HeaderStyle-Width="18%">
                                    <ItemTemplate>
                                        <span class="small"><i class="fa fa-pencil"></i><b>Created:</b> <%# Eval("Creator_Details") %></span><br />
                                        <span class="small"><i class="fa fa-check-circle text-success"></i><b>Approved:</b> <%# Eval("Approver_Details") %></span>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Anomaly Detected" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <span class="text-danger font-weight-bold"><i class="fa fa-warning"></i><%# Eval("Anomaly_Type") %></span>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <EmptyDataTemplate>
                                <div class="alert alert-success text-center" style="margin-top: 10px;">
                                    <i class="fa fa-check-circle"></i>No attendance anomalies detected for this period.
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>

            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <!-- DataTables Script Initialization -->
    <script src="../vendors/datatables.net/js/jquery.dataTables.min.js"></script>
    <script src="../vendors/datatables.net-bs/js/dataTables.bootstrap.min.js"></script>
    <script>
        function initializeDataTable() {
            if ($.fn.DataTable.isDataTable('#<%= gvAnomalies.ClientID %>')) {
                $('#<%= gvAnomalies.ClientID %>').DataTable().destroy();
            }
            $('#<%= gvAnomalies.ClientID %>').DataTable({
                "order": [[2, "desc"]] // Sort by Date by default
            });
        }

        // Re-initialize after UpdatePanel postback
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            initializeDataTable();
        });

        $(document).ready(function () {
            initializeDataTable();
        });
    </script>
</asp:Content>
