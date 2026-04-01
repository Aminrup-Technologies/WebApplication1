<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="jobs_and_manpower_v2.aspx.cs" Inherits="WebApplication1.bussiness.production.jobs_and_manpower_v2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .btn-app {
            position: relative;
        }

            .btn-app .badge {
                position: absolute;
                top: -3px;
                right: -10px;
                font-size: 14px;
                font-weight: 700;
                padding: 5px 8px;
                border-radius: 50%;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title" style="display: flex; justify-content: space-between; align-items: center;">
                <div class="title_left">
                    <h3>Daily JOB's, Manpower & Billing (NEW Version)</h3>
                </div>
                <div class="title_right">
                    <a href="jobs_and_manpower.aspx"
                        style="display: inline-block; padding: 8px 16px; cursor: pointer; background-color: #6c757d; color: white; border: none; border-radius: 4px; font-weight: bold; text-decoration: none;">Switch to OLD Version
                    </a>
                </div>
            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-6 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Daily JOB Management <small>Smart Workflow</small></h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <a class="btn btn-app" href="create_jobid_v2.aspx">
                                <span class="badge bg-green" id="badge_activejobs" runat="server">0</span>
                                <i class="fa fa-plus-square"></i>Create ID
                            </a>
                            <a class="btn btn-app" href="job_permitupload_v2.aspx">
                                <span class="badge bg-green" id="badge_permits" runat="server">0</span>
                                <i class="fa fa-upload"></i>Permit Upload
                            </a>
                            <a class="btn btn-app" href="job_inpunch_v2.aspx">
                                <span class="badge bg-green" id="badge_inpunches" runat="server">0</span>
                                <i class="fa fa-sign-in"></i>IN Punch
                            </a>
                            <a class="btn btn-app" href="job_outpunch_v2.aspx">
                                <span class="badge bg-green" id="badge_outpunches" runat="server">0</span>
                                <i class="fa fa-sign-out"></i>OUT Punch
                            </a>
                        </div>
                    </div>
                </div>

                <div class="col-md-6 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>JOBID For Memo & Billing</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <a class="btn btn-app" href="vw_supplyjobs.aspx">
                                <span class="badge bg-green" id="badge_splyjobs" runat="server">0</span>
                                <i class="fa fa-file-text-o"></i>Supply Memo
                            </a>
                            <a class="btn btn-app" href="vw_lineitemjobs.aspx">
                                <span class="badge bg-green" id="badge_lijobs" runat="server">0</span>
                                <i class="fa fa-list-alt"></i>Line Item Memo
                            </a>
                        </div>
                    </div>
                </div>

                <div class="col-md-6 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>JOB Management <small>Smart Window</small></h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <a class="btn btn-app" href="manage_jobid_v2.aspx">
                                <span class="badge bg-green" id="Span1" runat="server">0</span>
                                <i class="fa fa-file-text-o"></i>View & Manage
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
