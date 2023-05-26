<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="csm_approvals.aspx.cs" Inherits="WebApplication1.bussiness.production.csm_approvals" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>CSM Approvals</h3>
                </div>

                <div class="title_right">
                    <div class="col-md-5 col-sm-5 form-group row pull-right top_search"></div>
                </div>
            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12" id="sftysupvbox" runat="server" visible="true">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Saftey Supervisors</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <a class="btn btn-app" href="vw_tbtforapproval.aspx" id="tbtbx1" runat="server">
                                <span class="badge bg-red">
                                    <asp:Label ID="lbl_tbtbx1count" runat="server" Text="0"></asp:Label></span>
                                <i class="fa fa-edit"></i>TBT Talk

                            </a>
                            <a class="btn btn-app" href="vw_sopapproval.aspx?vw=sopvw1" id="sopbx1" runat="server">
                                <span class="badge bg-red"><asp:Label ID="lbl_sopbx1count" runat="server" Text="0"></asp:Label></span>
                                <i class="fa fa-users"></i>SOP Training

                            </a>
                            <a class="btn btn-app">
                                <i class="fa fa-check-square-o"></i>JHA

                            </a>
                        </div>
                    </div>
                </div>

                <div class="col-md-12" id="sftyofcrbox" runat="server" visible="true">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Safety Officers</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <a class="btn btn-app" href="vw_tbt_masterapproval.aspx">
                                <span class="badge bg-red"><asp:Label ID="lbl_tbtbx2count" runat="server" Text="0"></asp:Label></span>
                                <i class="fa fa-edit"></i>TBT Talk

                            </a>
                            <a class="btn btn-app" href="vw_sopapproval.aspx?vw=sopvw2" id="sopbx2" runat="server">
                                <span class="badge bg-red"><asp:Label ID="lbl_sopbx2count" runat="server" Text="0"></asp:Label></span>
                                <i class="fa fa-users"></i>SOP Training

                            </a>
                            <a class="btn btn-app">
                                <i class="fa fa-check-square-o"></i>JHA

                            </a>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
