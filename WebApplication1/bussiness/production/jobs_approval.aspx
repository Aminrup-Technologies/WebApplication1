<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="jobs_approval.aspx.cs" Inherits="WebApplication1.bussiness.production.jobs_approval" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="row">

            <div class="col-md-6" id="pnl_employee" runat="server" visible="true">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>JOB-ID Approval Management </h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <a class="btn btn-app" href="generate_appvrsite_atdncsheet.aspx" id="box_vwatten" runat="server" visible="true">
                            <div id="vwatten" class="badge bg-green" runat="server">
                                <asp:Label ID="Label17" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Monthly Attendance
                        </a>
                        <a class="btn btn-app" href="view_jobsforapproval.aspx" id="box_ViewJobforApp" runat="server" visible="true">
                            <div id="ViewJobforApp" class="badge bg-green" runat="server">
                                <asp:Label ID="lbl_factorsstatus" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-edit"></i>Pending JOB's

                        </a>
                        <a class="btn btn-app" href="view_approvedjobs.aspx" id="box_ApprovedJOBS" runat="server" visible="true">
                            <div id="ApprovedJOBS" class="badge bg-green" runat="server">
                                <asp:Label ID="lbl_bankdatastatus" runat="server" Text="Ok" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>Approved JOB's
                        </a>
                        <a class="btn btn-app" href="#" id="box_RejectedJOBS_inch" runat="server" visible="true">
                            <div id="RejectedJOBS_inch" class="badge bg-orange" runat="server">
                                <asp:Label ID="Label1" runat="server" Text="WIP" Visible="true"></asp:Label>
                            </div>
                            <i class="fa fa-users"></i>Rejected JOB's
                        </a>
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
