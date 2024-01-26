<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="jobs_and_manpower.aspx.cs" Inherits="WebApplication1.bussiness.production.jobs_and_manpower" %>
<%@ Register Src="~/bussiness/production/jobstatus_flow.ascx" TagName="jobflow" TagPrefix="jf" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>Daily JOB's, Manpower & Billing</h3>
                </div>

                <div class="title_right">
                    <div class="col-md-5 col-sm-5 form-group row pull-right top_search"></div>
                </div>
            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>JOBID : <asp:Label ID="lbl_activejobid" runat="server" Text="N/A" ForeColor="Blue" Font-Bold="true"></asp:Label>&nbsp; Real-Time Flow</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <jf:jobflow ID="jobstatusflow" runat="server" />
                        </div>
                    </div>
                </div>


                <div class="col-md-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Daily JOB  Management</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <a class="btn btn-app" href="create_jobid.aspx">
                                <span class="badge bg-orange">
                                    <asp:Label ID="lbl_activejobcount" runat="server" Text="Label"></asp:Label></span>
                                <i class="fa fa-edit"></i>Create ID

                            </a> 
                            <a class="btn btn-app" href="job_permitupload.aspx">
                                <span class="badge bg-orange">
                                    <asp:Label ID="lbl_prmtupldcount" runat="server" Text="Label"></asp:Label></span>
                                <i class="fa fa-edit"></i>Permit Upload
                            </a>
                            <a class="btn btn-app" href="job_inpunch.aspx">
                                <span class="badge bg-orange">
                                    <asp:Label ID="lbl_inpunchcount" runat="server" Text="Label"></asp:Label></span>
                                <i class="fa fa-edit"></i>IN Punch
                            </a>
                            <a class="btn btn-app" href="job_outpunch.aspx">
                                <span class="badge bg-orange">
                                    <asp:Label ID="lbl_outpndgcount" runat="server" Text="Label"></asp:Label></span>
                                <i class="fa fa-users"></i>OUT Punch

                            </a>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>JOBID For Memo & Biliing</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <a class="btn btn-app" href="vw_supplyjobs.aspx">
                                <span class="badge bg-green">
                                    <asp:Label ID="lbl_splyjobscount" runat="server" Text="Label"></asp:Label></span>
                                <i class="fa fa-edit"></i>Supply Memo
                            </a>
                            <a class="btn btn-app" href="vw_lineitemjobs.aspx">
                                <span class="badge bg-green">
                                    <asp:Label ID="lbl_lijobscount" runat="server" Text="Label"></asp:Label></span>
                                <i class="fa fa-bullhorn"></i>Line Item Memo

                            </a>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
