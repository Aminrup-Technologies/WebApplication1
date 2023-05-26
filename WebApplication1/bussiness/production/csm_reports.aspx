<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="csm_reports.aspx.cs" Inherits="WebApplication1.bussiness.production.csm_reports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>CSM Reporting</h3>
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
                            <h2>Daily Communications</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <asp:LinkButton ID="LB_tbt" class="btn btn-app" OnClick="LB_tbt_Click" runat="server"><span class="badge bg-green">
                                    <asp:Label ID="lbl_tbtcount" runat="server" Text="0"></asp:Label></span>
                                <i class="fa fa-edit"></i>TBT Talk</asp:LinkButton>
                            <a class="btn btn-app">
                                <span class="badge bg-green">0</span>
                                <i class="fa fa-users"></i>SOP Training

                            </a>
                            <a class="btn btn-app">
                                <i class="fa fa-check-square-o"></i>JHA

                            </a>
                        </div>
                    </div>
                </div>

                <div class="col-md-12" id="Safety_Audits" runat="server" visible="false">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Safety Audits</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <a class="btn btn-app">
                                <span class="badge bg-green">0</span>
                                <i class="fa fa-edit"></i>FSO

                            </a>
                            <a class="btn btn-app">
                                <span class="badge bg-green">0</span>
                                <i class="fa fa-users"></i>Trainings

                            </a>
                            <a class="btn btn-app">
                                <i class="fa fa-male"></i>PPE
                  </a>
                        </div>
                    </div>
                </div>

                <div class="col-md-12" id="Reportings" runat="server" visible="false">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Compliance & Reporting</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <a class="btn btn-app">
                                <span class="badge bg-green">0</span>
                                <i class="fa fa-edit"></i>NC

                            </a>
                            <a class="btn btn-app">
                                <span class="badge bg-green">0</span>
                                <i class="fa fa-bullhorn"></i>Incident

                            </a>
                            <a class="btn btn-app">
                                <i class="fa fa-check-square-o"></i>SSI

                            </a>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
