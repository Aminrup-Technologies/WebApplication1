<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="overview.aspx.cs" Inherits="WebApplication1.bussiness.production.overview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="container">
            <div class="page-title">
                <div class="title_left">
                    <h5>Organization Works Overview</h5>
                </div>
            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12  ">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Current Month JOB's view </h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row">

                                <div class="col-md-3 col-sm-12 col-lg-6 form-group">
                                    <iframe
                                        width="100%"
                                        height="400"
                                        seamless
                                        frameborder="1"
                                        scrolling="no"
                                        src="https://www.mystifying-yalow.150-242-202-11.plesk.page/superset/explore/p/3m12Pr0ZNPg/?standalone=1&height=400"></iframe>
                                </div>
                                <div class="col-md-3 col-sm-12 col-lg-6 form-group">
                                    <iframe
                                        width="100%"
                                        height="400"
                                        seamless
                                        frameborder="1"
                                        scrolling="no"
                                        src="https://www.mystifying-yalow.150-242-202-11.plesk.page/superset/explore/p/QWgKJ9A2m4p/?standalone=1&height=400"></iframe>
                                </div>

                                <div class="col-md-3 col-sm-12 col-lg-12 form-group">
                                    <iframe
                                        width="100%"
                                        height="600"
                                        seamless
                                        frameborder="0"
                                        scrolling="no"
                                        src="https://www.mystifying-yalow.150-242-202-11.plesk.page/superset/explore/p/4QVKoJp2eNj/?standalone=1&height=400"></iframe>
                                </div>

                            </div>


                            <%--button start--%>
                            <div class="col-md-6 center-margin" id="buttons" runat="server" visible="false">
                                <div class="ln_solid"></div>
                                <div class="item form-group row">
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to Save Data!!"></asp:Label>
                                    </div>
                                    <div class="col-md-6 col-sm-12">
                                        <button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
                                        <button type="reset" class="btn btn-warning btn-sm">Reset</button>
                                        <asp:Button ID="btn_submit" runat="server" Text="Submit" CssClass="btn btn-success btn-sm" OnClientClick="return ValidateFormField()" />
                                    </div>
                                </div>
                            </div>
                            <%--button end--%>

                            <!-- Small modal -->
                            <asp:Button ID="ShowPopup" runat="server" Text="Button" class="btn btn-primary" Visible="false" data-toggle="modal" data-target=".bs-example-modal-sm" />
                            <div id="MyPopup" class="modal fade bs-example-modal-sm" tabindex="-1" role="dialog" aria-hidden="true">
                                <div class="modal-dialog modal-sm">
                                    <div class="modal-content">

                                        <div class="modal-header">
                                            <h4 class="modal-title" id="myModalLabel2"></h4>
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                <span aria-hidden="true">×</span>
                                            </button>
                                        </div>
                                        <div class="modal-body">
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary btn-sm" data-dismiss="modal">Close</button>
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <!-- Small Modal - END---->
                        </div>

                    </div>
                </div>

                <div class="col-md-12 col-sm-12" id="grid" runat="server" visible="false">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>View and Manage : Work Order Data</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row">
                                <div class="col-md-12 col-sm-12">
                                    <div class="card-box table-responsive">
                                        <span>Hello, How are you?</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function ShowPopup(title, body) {
            $("#MyPopup .modal-title").html(title);
            $("#MyPopup .modal-body").html(body);
            $("#MyPopup").modal("show");
        }
    </script>
</asp:Content>
