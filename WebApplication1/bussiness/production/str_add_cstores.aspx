<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="str_add_cstores.aspx.cs" Inherits="WebApplication1.bussiness.production.str_add_cstores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="container">
            <%--<div class="page-title">
                <div class="title_left">
                    <h5>Main Heading</h5>
                </div>
            </div>

            <div class="clearfix"></div>--%>

            <div class="row">
                <div class="col-md-12 col-sm-12  ">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Add New Central Store Against Work Region</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="col-md-3">
                                <div class="mb-3">
                                    <asp:Label ID="LBL_DDL_WorkCountry" runat="server" AssociatedControlID="DDL_WorkCountry" Text="Select Country :" ForeColor="Blue" Font-Bold="true" Font-Size="Small"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RFV_DDL_WorkCountry" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="DDL_WorkCountry" Display="Dynamic" InitialValue="0" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    <div class="input-group-sm">
                                        <asp:DropDownList ID="DDL_WorkCountry" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="false" ></asp:DropDownList>

                                    </div>

                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="mb-3">
                                    <asp:Label ID="LVL_DDL_State" runat="server" AssociatedControlID="DDL_State" Text="Select State :" ForeColor="Blue" Font-Bold="true" Font-Size="Small"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RFV_DDL_Supervisor" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="DDL_State" Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
                                    <div class="input-group-sm">
                                        <asp:DropDownList ID="DDL_State" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" >
                                            <asp:ListItem Enabled="False" Selected="True" Value="0">--select--</asp:ListItem>
                                        </asp:DropDownList>

                                    </div>

                                </div>
                            </div>

                             <div class="col-md-3">
                                <div class="mb-3">
                                    <asp:Label ID="LBL_TB_StoreName" runat="server" AssociatedControlID="TB_StoreName" Text="Enter Store Name :" ForeColor="Blue" Font-Bold="true" Font-Size="Small"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RFV_TB_StoreName" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="TB_StoreName" Display="Dynamic" InitialValue="" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    <div class="input-group-sm">
                                        <asp:TextBox ID="TB_StoreName" runat="server" CssClass="form-control form-control-sm rounded"></asp:TextBox>
                                    </div>

                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="mb-3">
                                    <asp:Label ID="LBL_TB_StoreCode" runat="server" AssociatedControlID="TB_StoreCode" Text="Enter Store Code :" ForeColor="Blue" Font-Bold="true" Font-Size="Small"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RFV_TB_StoreCode" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="TB_StoreCode" Display="Dynamic" InitialValue="" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    <div class="input-group-sm">
                                        <asp:TextBox ID="TB_StoreCode" runat="server" CssClass="form-control form-control-sm rounded" ></asp:TextBox>
                                    </div>

                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="mb-3">
                                    <asp:Label ID="LBL_TB_Manager1_Wrk" runat="server" AssociatedControlID="TB_Manager1_Wrk" Text="Assign Manager 1 :" ForeColor="Blue" Font-Bold="true" Font-Size="Small"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RFV_TB_Manager1_Wrk" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="TB_Manager1_Wrk" Display="Dynamic" InitialValue="" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    <div class="input-group-sm">
                                        <asp:TextBox ID="TB_Manager1_Wrk" runat="server" CssClass="form-control form-control-sm rounded"></asp:TextBox>
                                    </div>

                                </div>
                            </div>

                             <div class="col-md-3">
                                <div class="mb-3">
                                    <asp:Label ID="LBL_TB_Manager2_Wrk" runat="server" AssociatedControlID="TB_Manager2_Wrk" Text="Assign Manager 2 :" ForeColor="Blue" Font-Bold="true" Font-Size="Small"></asp:Label>
                                    <div class="input-group-sm">
                                        <asp:TextBox ID="TB_Manager2_Wrk" runat="server" CssClass="form-control form-control-sm rounded"></asp:TextBox>
                                    </div>

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
                                <button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
                                <button type="reset" class="btn btn-warning btn-sm">Reset</button>
                                <asp:Button ID="btn_submit" runat="server" Text="Submit" CssClass="btn btn-success btn-sm" CausesValidation="true" ValidationGroup="Submit"/>
                            </div>
                        </div>
                    </div>
                    <%--button end--%>
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
