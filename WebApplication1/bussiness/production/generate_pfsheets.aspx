<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="generate_pfsheets.aspx.cs" Inherits="WebApplication1.bussiness.production.generate_pfsheets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .page-title {
            width: 100%;
            height: auto;
            padding: 10px 0;
        }

        .title_left {
            width: 50%;
            float: left;
            display: block;
            text-align: left; /* Center the content on small screens */
        }

            .title_left h5 {
                margin: 9px 0;
            }

        .title_right {
            width: 50%;
            float: right;
            display: block;
            text-align: right; /* Center the content on small screens */
        }

            .title_right .pull-right {
                margin: 10px 0;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h5>Generate EPFO Challan Sheets</h5>
                </div>

                <div class="title_right">
                    <div class="pull-right">
                        <asp:Button ID="btnExport" runat="server" Enabled="false" Text="Export To Excel" CssClass="btn btn-success btn-sm" OnClick="btn_excelexport_Click" />
                    </div>
                </div>

                <div class="clearfix"></div>

                <div class="row">
                    <div class="col-md-12 col-sm-12  ">
                        <div class="x_panel">
                            <div class="x_content">
                                <div class="row">

                                    <div class="col-md-3 col-sm-12  form-group">
                                        <label>Select Company <span class="text text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-3 col-sm-12  form-group">
                                        <asp:DropDownList ID="DDL_Company" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                    </div>

                                    <div class="col-md-3 col-sm-12  form-group">
                                        <label>Select Year <span class="text text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-3 col-sm-12  form-group">
                                        <asp:DropDownList ID="DDL_Year" CssClass="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
                                    </div>


                                    <div class="col-md-3 col-sm-12  form-group">
                                        <label>Select Month <span class="text text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-3 col-sm-12  form-group">
                                        <asp:DropDownList ID="DDL_Month" CssClass="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
                                    </div>
                                </div>

                                <%--button   start--%>
                                <div class="col-md-6 center-margin">
                                    <div class="ln_solid"></div>
                                    <div class="item form-group row">
                                        <div class="col-md-6 col-sm-12">
                                            <asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to Save Data!!"></asp:Label>
                                        </div>
                                        <div class="col-md-6 col-sm-12">
                                            <asp:Button ID="btn_cancel" runat="server" Text="Back" CssClass="btn btn-danger btn-sm" OnClick="btn_cancel_Click" />
                                            <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="btn btn-warning btn-sm" OnClick="btn_reset_Click" />
                                            <asp:Button ID="btn_submit" runat="server" Text="Submit" CssClass="btn btn-success btn-sm" OnClientClick="return ValidateFormField()" OnClick="btn_submit_Click" />
                                        </div>
                                    </div>
                                </div>
                                <%--button   end--%>

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

                    <div class="col-md-12 col-sm-12  ">
                        <div id="Grid" class="card-box table-responsive">
                            <asp:Label ID="lblTotalData" runat="server"></asp:Label>
                        </div>
                        <hr />
                        <asp:HiddenField ID="hfGridHtml" runat="server" />

                        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
                        <script type="text/javascript">
                            $(function () {
                                $("[id*=btnExport]").click(function () {
                                    $("[id*=hfGridHtml]").val($("#Grid").html());
                                });
                            });
                        </script>
                    </div>

                    <div class="col-md-12 col-sm-12  ">
                        <div>&nbsp;</div>
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

        function ValidateFormField() {
            if (document.getElementById('<%=DDL_Company.ClientID%>').selectedIndex == 0) {
                document.getElementById('<%=DDL_Company.ClientID%>').focus();
                ShowPopup("Error :", "Work Company selection required...!");
                return false;
            }

            if (document.getElementById('<%=DDL_Month.ClientID%>').selectedIndex == 0) {
                document.getElementById('<%=DDL_Month.ClientID%>').focus();
                ShowPopup("Error :", "Calender Month selection required...!");
                return false;
            }
        }
    </script>
</asp:Content>
