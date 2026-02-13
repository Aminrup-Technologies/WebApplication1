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

    <div id="MyPopup" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-md">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">×</span>
                    </button>
                    <h4 class="modal-title" id="myModalLabel2">Notification</h4>
                </div>

                <div class="modal-body">
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>

            </div>
        </div>
    </div>

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
                                                    CssClass='<%# Eval("WorkStatus").ToString() == "Active" ? "btn btn-sm btn-success" : "btn btn-sm btn-danger" %>'
                                                    CommandName="Swap_WorkStatus" CommandArgument='<%# Eval("Id") + "," + Eval("WorkmanSL") + "," + Eval("WorkStatus") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Employee Details" HeaderStyle-Width="30%">
                                            <ItemTemplate>
                                                <div style="margin-bottom: 4px;">
                                                    <span class="badge badge-info" title="Employee Code" style="margin-right: 2px;">
                                                        <%# Eval("WorkmanSL") %>
                                                    </span>

                                                    <span class="badge" style="background-color: #E7E7E7; color: #555; border: 1px solid #ccc; margin-right: 5px;" title="System Login ID">
                                                        <i class="fa fa-key"></i><%# Eval("LoginID") %>
                                                    </span>

                                                    <strong style="font-size: 1.1em; color: #2A3F54; text-transform: uppercase; vertical-align: middle;">
                                                        <%# Eval("FullName") %>
                                                    </strong>
                                                </div>

                                                <div style="color: #73879C; font-size: 0.9em; margin-bottom: 2px;">
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

                                        <asp:TemplateField HeaderText="Contact Info" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <div style="line-height: 1.6;">
                                                    <i class="fa fa-phone-square" style="font-size: 1.1em; color: #26B99A;"></i>
                                                    <span style="font-weight: 600; color: #555;">
                                                        <%# Eval("MobileNo") %>
                                                    </span>
                                                    <br />

                                                    <i class="fa fa-envelope" style="font-size: 1em; color: #3498DB;"></i>
                                                    <span style="font-size: 0.9em; color: #73879C;">
                                                        <%# string.IsNullOrEmpty(Eval("Email").ToString()) ? "N/A" : Eval("Email") %>
                                                    </span>
                                                </div>
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

                                        <asp:TemplateField HeaderText="Action" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <div class="btn-group btn-group-sm">
                                                    <asp:LinkButton ID="btn_viewdetails" runat="server" CssClass="btn btn-info"
                                                        CommandName="View_Details" CommandArgument='<%# Eval("WorkmanSL") %>'
                                                        ToolTip="View Profile">
                                                        <i class="fa fa-eye"></i>
                                                    </asp:LinkButton>
                                                    &nbsp;&nbsp;
                                                    <asp:LinkButton ID="btn_resetpwd" runat="server" CssClass="btn btn-warning"
                                                        CommandName="Reset_Password"
                                                        CommandArgument='<%# Eval("WorkmanSL") + "|" + (Eval("Email") ?? "") + "|" + Eval("FullName") + "|" + Eval("LoginID") %>'
                                                        OnClientClick="return confirm('Are you sure you want to reset the password for this user?');"
                                                        ToolTip="Reset Password & Email">
                                                        <i class="fa fa-key"></i>
                                                    </asp:LinkButton>
                                                </div>
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
        // -------------------------------------------------------------------------
        // CRITICAL: Handle jQuery Conflict
        // -------------------------------------------------------------------------
        // jqNew = jQuery 3.6.0 (For DataTables)
        // $     = Master Page jQuery (For Modals & Theme)
        var jqNew = $.noConflict(true);

        // -------------------------------------------------------------------------
        // 1. Initialize DataTables using jqNew (The new jQuery)
        // -------------------------------------------------------------------------
        jqNew(document).ready(function () {
            var table = jqNew('#<%= GridView1.ClientID %>').DataTable({
                "paging": true,
                "lengthChange": true,
                "searching": true,
                "ordering": true,
                "info": true,
                "autoWidth": false,
                "responsive": true,
                "pageLength": 10,
                "dom": 'Bfrtip',
                "buttons": [
                    {
                        extend: 'excelHtml5',
                        text: '<i class="fa fa-file-excel-o"></i> Export Excel',
                        className: 'btn btn-success btn-sm',
                        title: 'Employee_Master_Data',
                        exportOptions: {
                            columns: ':not(:last-child)' // Hide Action column
                        }
                    },
                    {
                        extend: 'pdfHtml5',
                        text: '<i class="fa fa-file-pdf-o"></i> PDF',
                        className: 'btn btn-danger btn-sm',
                        orientation: 'landscape',
                        pageSize: 'LEGAL',
                        exportOptions: {
                            columns: ':not(:last-child)'
                        }
                    },
                    'copy', 'print'
                ],
                "language": {
                    "search": "_INPUT_",
                    "searchPlaceholder": "Search Employees..."
                }
            });
        });

        // -------------------------------------------------------------------------
        // 2. Popup Function (Use '$' for Global Bootstrap)
        // -------------------------------------------------------------------------
        function ShowPopup(title, body) {
            // FIX: Use '$' here because the Master Page loads Bootstrap 
            // and attaches it to the global jQuery variable, not jqNew.
            $("#MyPopup .modal-title").html(title);
            $("#MyPopup .modal-body").html(body);
            $("#MyPopup").modal("show");
        }

        // FUNCTION TO COPY TEXT
        function CopyShareText() {
            // 1. Get the hidden text
            var copyText = document.getElementById("txtShare");

            // 2. Show it temporarily so we can select it (required for some browsers)
            copyText.style.display = "block";

            // 3. Select and Copy
            copyText.select();
            copyText.setSelectionRange(0, 99999); // For mobile devices
            document.execCommand("copy");

            // 4. Hide it again
            copyText.style.display = "none";

            // 5. Visual Feedback (Optional: simple alert or button text change)
            alert("Credentials copied! You can now paste them in WhatsApp/Teams.");
        }
    </script>
</asp:Content>
