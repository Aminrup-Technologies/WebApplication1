<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="emp_profilepic.aspx.cs" Inherits="WebApplication1.bussiness.production.emp_profilepic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>Work Permit Upload</h3>
                </div>
            </div>
            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Permit Attachment Status & View</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row">
                                <div class="col-md-3 col-sm-12 form-group" id="pdfuploadbuttonrow1" runat="server" visible="true">
                                    <label>Upload Permit (.pdf / .jpg) <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12 form-group" id="pdfuploadbuttonrow2" runat="server" visible="true">
                                    <button type="button" class="btn btn-primary btn-sm" id="btnShowPopup" data-toggle="modal" data-target="#myModal">
                                        <i class="fa fa-plus-circle"></i>&nbsp;Upload Permit File
                                    </button>
                                </div>
                            </div>
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


                            <%--- Up-loader Modal --%>
                            <div class="modal fade" id="myModal">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h4 class="modal-title">Upload File</h4>
                                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                                        </div>
                                        <div class="modal-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label>Choose file</label>
                                                        <div class="input-group">
                                                            <div class="custom-file col-md-8">
                                                                <asp:FileUpload ID="FileUpload1" CssClass="custom-file-input" runat="server" />
                                                                <label class="custom-file-label"></label>
                                                            </div>
                                                            <asp:Label ID="lbl_fileyesno" runat="server" Text="Label" Visible="false"></asp:Label>
                                                            <div class="input-group-append col-md-4">
                                                                <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-primary btn-sm" CausesValidation="false" Text="Upload" OnClick="ImportPermit" />
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
                            <%--- Up-loader Modal -------- END --%>



                            <div class="row" id="Photouploaded" runat="server" visible="false">
                                <div class="col-md-12 col-sm-12" style="vertical-align: middle; text-align: center;">
                                    <asp:Image ID="Img_Success" runat="server" ImageUrl="~/erp_images/success_gif.gif" Width="100px" Height="100px" />
                                    <asp:Label ID="Label5" runat="server" Text="Photograph Uploaded Successfully...!" Font-Bold="true" Font-Size="Large"></asp:Label>
                                </div>
                                <div class="row">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Panel ID="PanelViewPhoto" runat="server" Visible="false">
                                            <div class="col-sm-12 col-xs-12">
                                                <asp:Image ID="ImgDisplay" runat="server" Height="200px" Width="320px" class="img-thumbnail" />
                                            </div>
                                        </asp:Panel>
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

        function ShowPopup1() {
            $("#myModal").modal("show");
        }
    </script>
</asp:Content>
