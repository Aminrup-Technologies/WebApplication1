<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="pyrl_upldadvc.aspx.cs" Inherits="WebApplication1.bussiness.production.pyrl_upldadvc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Styling for the overlay */
        #overlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgb(0, 0, 0, 0.5); /* Adjust the opacity to make it more or less transparent */
            z-index: 9999;
            opacity: 0.5;
            display: none;
        }

        /* Styling for the loading GIF (optional) */
        #loadingGif {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            z-index: 10000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div id="overlay" style="display: none;">
            <!-- You can add a loading GIF here -->
            <!-- For example: <img id="loadingGif" src="loading.gif" alt="Loading..."> -->
        </div>

        <div class="container">
            <div class="page-title">
                <div class="title_left">
                    <h5>Deductions : Bulk Upload using EXCEL</h5>
                </div>
            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12  ">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Add deductions manually using excel upload</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row">

                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Upload Excel Sheet<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12 form-group">
                                    <button type="button" class="btn btn-primary btn-sm" id="ShowUploaderPopup" data-toggle="modal" data-target="#myModal">
                                        <i class="fa fa-plus-circle"></i>&nbsp;Import Excel
                                    </button>
                                </div>

                                <div class="modal fade" id="myModal">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h4 class="modal-title">Import Excel File</h4>
                                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                            </div>
                                            <div class="modal-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="form-group">
                                                            <label>Choose excel file</label>
                                                            <div class="input-group">
                                                                <div class="custom-file col-md-8">
                                                                    <asp:FileUpload ID="FileUpload1" CssClass="custom-file-input" runat="server" />
                                                                    <label class="custom-file-label"></label>
                                                                </div>
                                                                <label id="filename"></label>
                                                                <div class="input-group-append col-md-4">
                                                                    <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-primary btn-sm" Text="Upload" OnClick="ImportExcel" />
                                                                </div>
                                                            </div>
                                                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <%--button start--%>
                            <div class="col-md-6 center-margin">
                                <div class="ln_solid"></div>
                                <div class="item form-group row">
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to Save Data!!"></asp:Label>
                                    </div>
                                    <div class="col-md-6 col-sm-12">
                                        <button type="button" id="cancelButton" class="btn btn-danger btn-sm" onclick="redirectToHomepage()">Cancel</button>
                                        <button type="reset" class="btn btn-warning btn-sm">Reset</button>
                                        <asp:Button ID="btn_submit" runat="server" Text="SAVE" CssClass="btn btn-success btn-sm"/>
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

                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>View Uploaded Data</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row">
                                <div class="col-md-12 col-sm-12">
                                    <div class="card-box table-responsive"></div>
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
        function ShowUploaderPopup() {
            $("#btnShowPopup").click();
        }
        function redirectToHomepage() {
            document.getElementById("overlay").style.display = "block";
            setTimeout(function () {
                window.location.href = "pyrl_managedashbrd.aspx";
            }, 2000);
        }
        function ShowUploaderModal() {
            $("#myModal").modal("show");
        }
    </script>
</asp:Content>
