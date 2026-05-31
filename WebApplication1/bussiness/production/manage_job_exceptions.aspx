<%@ Page Title="Job Exceptions Dashboard" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="manage_job_exceptions.aspx.cs" Inherits="WebApplication1.bussiness.production.manage_job_exceptions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                                    <div class="col-md-3">
                                        <label>From Date</label>
                                        <asp:TextBox ID="txtFromDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>To Date</label>
                                        <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Work Region</label>
                                        <asp:DropDownList ID="ddlRegion" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-3" style="margin-top: 24px;">
                                        <asp:Button ID="btnSearch" runat="server" Text="Filter Data" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-danger" OnClick="btnClear_Click" />
                                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-warning" PostBackUrl="~/bussiness/production/anlys_jobsdeta.aspx" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row top_tiles" style="margin: 10px 0;">
                    <asp:HiddenField ID="hfSelectedCategory" runat="server" Value="ALL" />

                    <asp:Repeater ID="rptSummary" runat="server" OnItemCommand="rptSummary_ItemCommand">
                        <ItemTemplate>
                            <div class="col-lg-3 col-md-4 col-sm-6 col-xs-12">
                                <asp:LinkButton ID="lnkCard" runat="server" CommandName="FilterCategory" CommandArgument='<%# Eval("Workflow_Stage") %>' CssClass='<%# Eval("Workflow_Stage").ToString() == hfSelectedCategory.Value ? "clickable-card active-filter-card" : "clickable-card" %>'>
                                    <div class="tile-stats" style="padding:15px; border-left: 4px solid #E74C3C;">
                                        <div class="count"><%# Eval("Total_Jobs") %></div>
                                        <h3 style="font-size:13px; font-weight:bold; white-space:normal;"><%# Eval("Workflow_Stage") %></h3>
                                    </div>
                                </asp:LinkButton>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <div class="x_panel">
                    <div class="x_title d-flex justify-content-between align-items-center">
                        <h2>Exception Ledger
                            <asp:Label ID="lblCurrentFilter" runat="server" CssClass="text-danger"></asp:Label></h2>

                        <asp:LinkButton ID="btnExportExcel" runat="server" CssClass="btn btn-success btn-sm pull-right" OnClick="btnExportExcel_Click">
                            <i class="fa fa-file-excel-o"></i> Export Ledger
                        </asp:LinkButton>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content table-responsive">
                        <asp:GridView ID="gvExceptions" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered dt-responsive nowrap" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="SL No">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="JOBID" HeaderText="JOB ID" />
                                <asp:BoundField DataField="Job_Date" HeaderText="Date" />
                                <asp:BoundField DataField="JOB_Region" HeaderText="Region" />

                                <asp:BoundField DataField="Worksite" HeaderText="Worksite" />
                                <asp:BoundField DataField="Creator_Details" HeaderText="Created By" />
                                <asp:BoundField DataField="Approver_Details" HeaderText="Assigned Approver" />

                                <asp:BoundField DataField="Workflow_Stage" HeaderText="Current State" />

                                <asp:TemplateField HeaderText="Aging">
                                    <ItemTemplate>
                                        <span class="badge badge-danger"><%# Eval("Days_Aging") %> Days</span>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="Recommended_Admin_Action" HeaderText="Action Needed" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#E74C3C" />

                                <asp:TemplateField HeaderText="Resolve">
                                    <ItemTemplate>
                                        <a href='job_360_view.aspx?jobid=<%# Eval("JOBID") %>' target="_blank" class="btn btn-primary btn-sm"><i class="fa fa-external-link"></i>View</a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
