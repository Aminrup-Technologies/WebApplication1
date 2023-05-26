<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="csms_mainview.aspx.cs" Inherits="WebApplication1.bussiness.production.csms_mainview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>Contractor Safety Management (CSM) Dashboard</h3>
                </div>

                <div class="title_right">
                    <div class="col-md-5 col-sm-5 form-group row pull-right top_search"></div>
                </div>
            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12" id="Supvkpirow" runat="server" visible="true">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Supervisor - KPI Documentation</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <a class="btn btn-app" href="csm_toolboxtalk.aspx">
                                <span class="badge bg-red">Daily : 1
                                    <asp:Label ID="lbl_tbttodaycount" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-edit"></i>TBT Talk

                            </a>
                            <a class="btn btn-app" href="csm_soptraining.aspx">
                                <span class="badge bg-red">Daily : 1
                                    <asp:Label ID="Label1" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-users"></i>SOP Training

                            </a>
                            <a class="btn btn-app" href="ppe_request.aspx">
                                <span class="badge bg-orange">Weekly : 1
                                    <asp:Label ID="Label2" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-male"></i>PPE Audit

                            </a>
                        </div>
                    </div>
                </div>

                <div class="col-md-12" id="SafetySupvKPIrow" runat="server" visible="true">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Safety Supervisor : KPI Documentation</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">

                            <a class="btn btn-app" href="csm_globaltraining.aspx?type=STD">
                                <span class="badge bg-red">Daily : 1
                                    <asp:Label ID="Label10" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-users"></i>Standard Training

                            </a>

                            <a class="btn btn-app" href="csm_globaltraining.aspx?type=SOP">
                                <span class="badge bg-orange">Weekly : 1
                                    <asp:Label ID="Label3" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-users"></i>SOP Training

                            </a>

                            <a class="btn btn-app" href="csm_globaltraining.aspx?type=JHA">
                                <span class="badge bg-orange">Weekly : 1
                                    <asp:Label ID="Label4" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-users"></i>JHA Training

                            </a>

                            <a class="btn btn-app" href="csm_globaltraining.aspx?type=FVS">
                                <span class="badge bg-orange">Weekly : 1
                                    <asp:Label ID="Label6" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-users"></i>5S Training

                            </a>

                            <a class="btn btn-app" href="csm_globaltraining.aspx?type=EPT">
                                <span class="badge bg-orange">Weekly : 1
                                    <asp:Label ID="Label7" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-users"></i>EPRP Training

                            </a>

                            <a class="btn btn-app" href="csm_globaltraining.aspx?type=PPE">
                                <span class="badge bg-orange">Weekly : 1
                                    <asp:Label ID="Label11" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-users"></i>PPE Training

                            </a>

                            <a class="btn btn-app" href="#">
                                <span class="badge bg-orange">Weekly : 1
                                    <asp:Label ID="Label19" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-edit"></i>PPE Audit

                            </a>

                            <a class="btn btn-app" href="#">
                                <span class="badge bg-orange">Weekly : 1
                                    <asp:Label ID="Label20" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-cogs"></i>Tools & Tackles Audit

                            </a>
                            <a class="btn btn-app" href="#">
                                <span class="badge bg-orange">Weekly : 1
                                    <asp:Label ID="Label21" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-sort-amount-asc"></i>5S Audit (Store / Office)

                            </a>

                            <a class="btn btn-app" href="csm_globaltraining.aspx?type=HSE">
                                <span class="badge bg-purple">Monthly : 1
                                    <asp:Label ID="Label8" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-users"></i>HSE Policy Training

                            </a>
                        </div>
                    </div>
                </div>

                <div class="col-md-12" id="StoreKPIrow" runat="server" visible="true">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Store Keeper : KPI Documentation</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <a class="btn btn-app" href="#">
                                <span class="badge bg-red">Weekly : 1
                                    <asp:Label ID="Label18" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-edit"></i>Tools & Tackles Audit

                            </a>
                        </div>
                    </div>
                </div>

                <div class="col-md-12" id="SafetyOfficerKPIrow" runat="server" visible="true">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Safety Officer : KPI Documentation</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <a class="btn btn-app" href="#">
                                <span class="badge bg-red">Weekly : 1
                                    <asp:Label ID="Label17" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-users"></i>Field Severity Audit

                            </a>
                            <a class="btn btn-app" href="csm_globaltraining.aspx?type=HRA">
                                <span class="badge bg-red">Weekly : 1
                                    <asp:Label ID="Label14" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-users"></i>HIRA Training
                            </a>
                            <a class="btn btn-app" href="#">
                                <span class="badge bg-purple">Monthly : 1
                                    <asp:Label ID="Label13" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-edit"></i>Line Walk
                            </a>
                            <a class="btn btn-app" href="#">
                                <span class="badge bg-purple">Monthly : 1
                                    <asp:Label ID="Label15" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-users"></i>Mass Meeting
                            </a>
                            <a class="btn btn-app" href="#">
                                <span class="badge bg-purple">Monthly : 1
                                    <asp:Label ID="Label16" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-users"></i>Safety Committee Meeting
                            </a>
                        </div>
                    </div>
                </div>

                <div class="col-md-12" id="siteinchargekpirow" runat="server" visible="true">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Site-Incharge : KPI Documentation</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <a class="btn btn-app" href="#">
                                <span class="badge bg-red">Monthly : 1
                                    <asp:Label ID="Label5" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-edit"></i>Line Walk

                            </a>
                            <a class="btn btn-app" href="#">
                                <span class="badge bg-red">Monthly : 1
                                    <asp:Label ID="Label12" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-users"></i>Mass Meeting

                            </a>
                        </div>
                    </div>
                </div>

                <div class="col-md-12" id="GeneralKPI" runat="server" visible="true">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Other : KPI Documentation</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <a class="btn btn-app" href="csm_incidentreporting.aspx">
                                <span class="badge bg-red">*
                                    <asp:Label ID="Label22" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-edit"></i>Incident Reporting

                            </a>
                            <a class="btn btn-app" href="#">
                                <span class="badge bg-blue">*
                                    <asp:Label ID="Label23" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-users"></i>Self Safety Initiatve

                            </a>
                            <a class="btn btn-app" href="#">
                                <span class="badge bg-orange">*
                                    <asp:Label ID="Label24" runat="server" Text="0" Visible="false"></asp:Label></span>
                                <i class="fa fa-users"></i>Compliance

                            </a>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
