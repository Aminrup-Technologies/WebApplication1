<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="GrievanceReq_View.aspx.cs" Inherits="WebApplication1.bussiness.production.GrievanceReq_View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Header Styling */
        .x_title {
            background-color: #000; /* Black background */
            color: #fff; /* White font */
            padding: 15px;
            border-radius: 5px 5px 0 0;
        }
        .x_title h2 {
            color: #fff;
            font-weight: bold;
        }
        /* Grid Styling */
        .custom-grid th {
            background-color: #343a40;
            color: #fff;
            text-align: center;
            font-size: 14px;
        }
        .custom-grid td {
            text-align: center;
            font-size: 13px;
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
        @media (max-width: 768px) {
            .custom-grid td, .custom-grid th {
                font-size: 12px;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="right_col" role="main">
    <div class="container">
        <div class="row">
            <div class="col-md-12 col-sm-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Grievance Requests - View and Manage</h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up" style="color:white;"></i></a></li>
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
                                </div> <!-- card-box -->
                            </div> <!-- col -->
                        </div> <!-- row -->
                    </div> <!-- x_content -->
                </div> <!-- x_panel -->
            </div> <!-- col -->
        </div> <!-- row -->
    </div> <!-- container -->
</div> <!-- right_col -->
</asp:Content>
