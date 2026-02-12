<%@ Page Title="HRMS | View Employee Master" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="view_emp_mastertbldata.aspx.cs" Inherits="WebApplication1.bussiness.production.view_emp_mastertbldata" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .thumbnail {
            position: relative;
            overflow: hidden;
            width: 100px; /* Set the initial width of the thumbnail */
            height: 100px; /* Set the initial height of the thumbnail */
            transition: width 0.3s, height 0.3s; /* Add smooth transition effect */
        }

            .thumbnail:hover {
                width: 150px; /* Set the enlarged width on hover */
                height: 150px; /* Set the enlarged height on hover */
            }

        .thumbnail-image {
            width: 100%;
            height: 100%;
            object-fit: cover; /* Ensure the image covers the entire container */
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Master Data <small>View & Manage Employees</small></h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li>
                                    <asp:Button ID="Button1" runat="server" Text="Export Excel" OnClick="ExportExcel" CssClass="btn btn-success btn-sm" />
                                </li>
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>

                        <div class="x_content">
                            <p class="text-muted font-13 m-b-30">
                                Use the search box to filter records instantly. Click headers to sort.
                            </p>

                            <div class="table-responsive">
                                <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped table-bordered dt-responsive nowrap" Width="100%" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnPreRender="GridView1_PreRender" OnRowCommand="GridView1_RowCommand" ClientIDMode="Static">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SL" HeaderStyle-Width="20px">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Status" HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Button ID="btn_workstatus" runat="server" Text='<%# Eval("WorkStatus") %>'
                                                    CssClass='<%# Eval("WorkStatus").ToString() == "Active" ? "btn btn-xs btn-success" : "btn btn-xs btn-danger" %>'
                                                    CommandName="Swap_WorkStatus" CommandArgument='<%# Eval("Id") + "," + Eval("WorkmanSL") + "," + Eval("WorkStatus") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Employee Details">
                                            <ItemTemplate>
                                                <span class="badge badge-info" style="font-size: 0.9em; margin-right: 5px;"><%# Eval("WorkmanSL") %></span>

                                                <strong style="font-size: 1.1em; color: #2A3F54; text-transform: uppercase;">
                                                    <%# Eval("FullName") %>
                                                </strong>
                                                <br />

                                                <div style="margin-top: 4px; color: #73879C; font-size: 0.9em;">
                                                    <i class="fa fa-briefcase"></i>
                                                    <span><%# Eval("SkillDesignation") %></span>

                                                    <span style="color: #ccc; margin: 0 5px;">|</span>

                                                    <span style="font-weight: 600; color: #1ABB9C;">
                                                        <%# Eval("SkillCategory") %>
                                                    </span>
                                                </div>

                                                <small style="color: #73879C;">
                                                    <i class="fa fa-user"></i>F: <%# Eval("Fathername") %>
                                                    <span style="margin-left: 10px; font-weight: bold; color: #d9534f;">
                                                        <i class="fa fa-tint"></i><%# Eval("BloodGroup") %>
                                                    </span>
                                                </small>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Important Dates">
                                            <ItemTemplate>
                                                <div style="font-size: 0.9em;">
                                                    <i class="fa fa-calendar-check-o text-success"></i>DOJ: <%# Eval("DOJ", "{0:dd-MMM-yyyy}") %><br />
                                                    <i class="fa fa-clock-o text-danger"></i>DOR: <span><%# Eval("DOR", "{0:dd-MMM-yyyy}") %></span>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Contact Info">
                                            <ItemTemplate>
                                                <i class="fa fa-mobile" style="font-size: 1.2em;"></i><%# Eval("MobileNo") %><br />
                                                <small>
                                                    <%# string.IsNullOrEmpty(Eval("UANNo").ToString()) ? 
                                                    "<i class='fa fa-envelope-o'></i> " + Eval("Email") : 
                                                    "<strong>UAN:</strong> " + Eval("UANNo") %>
                                                </small>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Site Access">
                                            <ItemTemplate>
                                                <span class="label label-primary"><%# Eval("WorkSite") %></span>
                                                <div style="margin-top: 5px; font-size: 0.85em;">
                                                    <strong>SP:</strong> <%# Eval("SafetyPassNo") %><br />

                                                    <span class="text-danger">Exp: <%# Eval("SafetyPassExpiry", "{0:dd-MMM-yy}") %>
                                                    </span>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btn_viewdetails" runat="server" CssClass="btn btn-info btn-xs"
                                                    CommandName="View_Details"
                                                    CommandArgument='<%# Eval("WorkmanSL") %>'>
                <i class="fa fa-eye"></i> View
                                                </asp:LinkButton>
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

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <script src="https://cdn.datatables.net/1.13.4/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.13.4/js/dataTables.bootstrap4.min.js"></script>

    <script src="https://cdn.datatables.net/buttons/2.3.6/js/dataTables.buttons.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.3.6/js/buttons.html5.min.js"></script>

    <script type="text/javascript">
        // Use 'jqNew' instead of '$' to avoid conflict with Master Page's jQuery 1.7.2
        var jqNew = $.noConflict(true);

        jqNew(document).ready(function () {
            jqNew('#<%= GridView1.ClientID %>').DataTable({
                "paging": true,
                "lengthChange": true,
                "searching": true,
                "ordering": true,
                "info": true,
                "autoWidth": false,
                "responsive": true,
                "pageLength": 10,
                "dom": 'Bfrtip', // Defines where buttons appear
                "buttons": [
                    {
                        extend: 'excelHtml5',
                        text: '<i class="fa fa-file-excel-o"></i> Export Excel',
                        className: 'btn btn-success btn-sm', // Matches your theme
                        title: 'Employee_Master_Data'
                    },
                    {
                        extend: 'pdfHtml5',
                        text: '<i class="fa fa-file-pdf-o"></i> PDF',
                        className: 'btn btn-danger btn-sm'
                    },
                    'copy', 'print'
                ],
                "language": {
                    "search": "_INPUT_",
                    "searchPlaceholder": "Search Employees..."
                }
            });
        });
    </script>
</asp:Content>
