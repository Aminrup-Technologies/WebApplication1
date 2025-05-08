<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="add_nwhelpdsk.aspx.cs" Inherits="WebApplication1.bussiness.production.add_nwhelpdsk" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        <style > .custom-grid {
            width: 100%;
            border-collapse: collapse;
            font-size: 14px;
        }

        .custom-grid th, .custom-grid td {
            padding: 12px 10px;
            text-align: left;
            vertical-align: middle;
        }

        .grid-header {
            background-color: #000;
            color: #fff;
            font-weight: bold;
        }

        .btn-view {
            background-color: #007bff;
            border: none;
            color: white;
            padding: 5px 10px;
            border-radius: 4px;
            font-size: 12px;
            cursor: pointer;
        }
        .btn-view:hover {
            background-color: #0056b3;
        }

        .grid-row:hover {
            background-color: #f9f9f9;
        }

        @media screen and (max-width: 768px) {
            .custom-grid td, .custom-grid th {
                font-size: 13px;
                padding: 10px 6px;
            }
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="container">


            <div class="row">
                <div class="col-md-12 col-sm-12  ">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Raise New Grievance Request</h2>
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
                                        <asp:Button ID="btn_submit" runat="server" Text="Submit" CssClass="btn btn-success btn-sm" OnClick="btn_submit_Click" ValidationGroup="Submit" CausesValidation="true" />
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
                            <h2>View and Manage : Grievances</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row">
                                <div class="col-md-12 col-sm-12">
                                    <div class="card-box table-responsive">
                                        <asp:GridView ID="gvGrievances" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-responsive-md custom-grid"
                                            HeaderStyle-CssClass="grid-header" RowStyle-CssClass="grid-row" DataKeyNames="ticket_id" OnRowCommand="gvGrievances_RowCommand">
                                            <Columns>
                                                <asp:BoundField DataField="ticket_id" HeaderText="Ticket ID" />
                                                <asp:BoundField DataField="CreatedOn" HeaderText="Created On" DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" />
                                                <asp:BoundField DataField="CreatedByName" HeaderText="Created By" />
                                                <asp:BoundField DataField="CreatorRegion" HeaderText="Region" />
                                                <asp:BoundField DataField="root1_value" HeaderText="Root 1" />
                                                <asp:BoundField DataField="root2_value" HeaderText="Root 2" />
                                                <asp:BoundField DataField="root3_value" HeaderText="Root 3" />
                                                <asp:BoundField DataField="priority_level" HeaderText="Priority" />
                                                <asp:BoundField DataField="status" HeaderText="Status" />
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnView" runat="server" Text="View" CssClass="btn btn-view" CommandName="ViewTicket" CommandArgument='<%# Eval("ticket_id") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
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
