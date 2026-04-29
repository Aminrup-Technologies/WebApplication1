<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="jobs_and_manpower_v2.aspx.cs" Inherits="WebApplication1.bussiness.production.jobs_and_manpower_v2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /* Base button positioning */
        .btn-app {
            position: relative;
        }
        
        /* MODERNIZATION OVERRIDES FOR GENTELELLA */
        
        /* Panel Enhancements: Soft shadows and rounded corners */
        .modern-panel {
            border: none !important;
            border-radius: 10px;
            box-shadow: 0 4px 6px rgba(0,0,0,0.04), 0 1px 3px rgba(0,0,0,0.06);
            transition: box-shadow 0.3s ease;
            background: #ffffff;
            margin-bottom: 20px;
        }
        .modern-panel:hover {
            box-shadow: 0 8px 15px rgba(0,0,0,0.08), 0 3px 6px rgba(0,0,0,0.05);
        }

        /* Title Enhancements */
        .modern-title {
            border-bottom: 1px solid #f2f2f2 !important;
            padding: 16px 20px !important;
        }
        .modern-title h2 {
            font-weight: 600;
            color: #34495e;
            font-size: 18px;
        }
        .modern-title small {
            color: #95a5a6;
            font-weight: 400;
            letter-spacing: 0.5px;
        }

        /* Action Buttons Enhancements */
        .btn-app.modern-btn {
            border-radius: 10px;
            border: 1px solid #ebeeef;
            background: #ffffff;
            color: #5A738E;
            box-shadow: 0 2px 4px rgba(0,0,0,0.02);
            transition: all 0.25s cubic-bezier(0.4, 0, 0.2, 1);
            min-width: 110px;
            height: auto;
            padding: 18px 12px 14px 12px;
            margin: 0 10px 10px 0;
        }
        .btn-app.modern-btn:hover {
            background: #fafbfc;
            color: #2a3f54;
            border-color: #d8dee3;
            transform: translateY(-4px);
            box-shadow: 0 6px 12px rgba(0,0,0,0.08);
        }
        .btn-app.modern-btn i {
            font-size: 26px;
            margin-bottom: 10px;
            display: block;
            color: #1ABB9C; /* Gentelella Primary Green */
            transition: transform 0.2s ease;
        }
        .btn-app.modern-btn:hover i {
            transform: scale(1.1);
        }

        /* Badge Enhancements */
        .btn-app.modern-btn .badge {
            position: absolute;
            top: -8px;
            right: -8px;
            font-size: 13px;
            font-weight: 700;
            padding: 5px 8px;
            border-radius: 50%;
            box-shadow: 0 2px 5px rgba(0,0,0,0.2);
            border: 2px solid #ffffff;
        }

        /* Top Header Button */
        .modern-header-btn {
            display: inline-block;
            background: linear-gradient(145deg, #6c757d, #5a6268);
            color: white;
            border: none;
            border-radius: 20px;
            padding: 8px 20px;
            font-size: 14px;
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
        
        /* Modern Page Title Alignment */
        .modern-page-header {
            display: flex; 
            justify-content: space-between; 
            align-items: center;
            margin-bottom: 20px;
            padding-bottom: 10px;
            border-bottom: 1px solid #e6e9ed;
        }
        .modern-page-header h3 {
            margin: 0;
            color: #2a3f54;
            font-weight: 600;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            
            <div class="page-title modern-page-header">
                <div class="title_left">
                    <h3>Daily JOB's, Manpower & Billing <span style="color: #1ABB9C; font-size: 16px;">(NEW Version)</span></h3>
                </div>
                <div class="title_right text-right">
                    <a href="jobs_and_manpower.aspx" class="modern-header-btn">
                        <i class="fa fa-history" style="margin-right: 5px;"></i> Switch to OLD Version
                    </a>
                </div>
            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-6 col-sm-12">
                    <div class="x_panel modern-panel">
                        <div class="x_title modern-title">
                            <h2>Daily JOB Management <small>Smart Workflow</small></h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content" style="padding-top: 15px;">
                            <a class="btn btn-app modern-btn" href="create_jobid_v2.aspx">
                                <span class="badge bg-green" id="badge_activejobs" runat="server">0</span>
                                <i class="fa fa-plus-square"></i>Create ID
                            </a>
                            <a class="btn btn-app modern-btn" href="job_permitupload_v2.aspx">
                                <span class="badge bg-green" id="badge_permits" runat="server">0</span>
                                <i class="fa fa-upload"></i>Permit Upload
                            </a>
                            <a class="btn btn-app modern-btn" href="job_inpunch_v2.aspx">
                                <span class="badge bg-green" id="badge_inpunches" runat="server">0</span>
                                <i class="fa fa-sign-in"></i>IN Punch
                            </a>
                            <a class="btn btn-app modern-btn" href="job_outpunch_v2.aspx">
                                <span class="badge bg-green" id="badge_outpunches" runat="server">0</span>
                                <i class="fa fa-sign-out"></i>OUT Punch
                            </a>
                        </div>
                    </div>
                </div>

                <div class="col-md-6 col-sm-12">
                    <div class="x_panel modern-panel">
                        <div class="x_title modern-title">
                            <h2>JOBID For Memo & Billing</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content" style="padding-top: 15px;">
                            <a class="btn btn-app modern-btn" href="vw_supplyjobs.aspx">
                                <span class="badge bg-green" id="badge_splyjobs" runat="server">0</span>
                                <i class="fa fa-file-text-o"></i>Supply Memo
                            </a>
                            <a class="btn btn-app modern-btn" href="vw_lineitemjobs.aspx">
                                <span class="badge bg-green" id="badge_lijobs" runat="server">0</span>
                                <i class="fa fa-list-alt"></i>Line Item Memo
                            </a>
                        </div>
                    </div>
                </div>

                <div class="col-md-6 col-sm-12">
                    <div class="x_panel modern-panel">
                        <div class="x_title modern-title">
                            <h2>JOB Management <small>Smart Window</small></h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content" style="padding-top: 15px;">
                            <a class="btn btn-app modern-btn" href="manage_jobid_v2.aspx">
                                <span class="badge bg-green" id="Span1" runat="server">0</span>
                                <i class="fa fa-database"></i>View & Manage
                            </a>
                        </div>
                    </div>
                </div>
            </div>
            
        </div>
    </div>
</asp:Content>