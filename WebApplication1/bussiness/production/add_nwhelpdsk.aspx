<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="add_nwhelpdsk.aspx.cs" Inherits="WebApplication1.bussiness.production.add_nwhelpdsk" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="container">
            <%--<div class="page-title">
                <div class="title_left">
                    <h5>Raise New Support Request</h5>
                </div>
            </div>

            <div class="clearfix"></div>--%>

            <div class="row">
                <div class="col-md-12 col-sm-12  ">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Raise New Grievances Request</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row">
                                <div class="col-md-3 col-sm-6  form-group">
                                    <label>Support Category<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-6  form-group">
                                    <asp:DropDownList ID="DDL_RootCategory" CssClass="form-control form-control-sm rounded" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL_RootCategory_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RFV_1" ValidationGroup="Submit" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_RootCategory" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-3 col-sm-6  form-group">
                                    <label>Grievances Topic<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-6  form-group">
                                    <asp:DropDownList ID="DDL_ChildCategory" CssClass="form-control form-control-sm rounded" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL_ChildCategory_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RFV_2" ValidationGroup="Submit" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_ChildCategory" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-3 col-sm-6  form-group">
                                    <label>Select Subject</label>
                                </div>
                                <div class="col-md-3 col-sm-6  form-group">
                                    <asp:DropDownList ID="DDL_Subject" CssClass="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
                                </div>

                                <div class="col-md-3 col-sm-6  form-group">
                                    <label>Support Level</label>
                                </div>
                                <div class="col-md-3 col-sm-6  form-group">
                                    <asp:DropDownList ID="DDL_HelpLevel" CssClass="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RFV_3" ValidationGroup="Submit" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_HelpLevel" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-3 col-sm-6  form-group">
                                    <label>Description<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-9 col-sm-6  form-group">
                                    <asp:TextBox ID="txt_descp" runat="server" class="form-control form-control-sm rounded" TextMode="MultiLine" Rows="3" Text="N/A"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RFV_4" ValidationGroup="Submit" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_descp" SetFocusOnError="true" InitialValue="N/A" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
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
                                        <asp:Button ID="btn_cancel" runat="server" class="btn btn-danger btn-sm collapse-link" CausesValidation="false" Text="Cancel" PostBackUrl="~/bussiness/production/homepage.aspx" />
                                        <asp:Button ID="btn_reset" runat="server" class="btn btn-warning btn-sm" Text="Reset" CausesValidation="false" OnClientClick="reloadPage(); return false;" />
                                        <asp:Button ID="btn_submit" runat="server" Text="Submit" CssClass="btn btn-success btn-sm" OnClick="btn_submit_Click" ValidationGroup="Submit" CausesValidation="true"/>
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
