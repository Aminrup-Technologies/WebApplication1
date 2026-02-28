<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="view_emp_mastertbldata_v2.aspx.cs" Inherits="WebApplication1.bussiness.production.view_emp_mastertbldata_v2" MaintainScrollPositionOnPostback="true"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .thumbnail {
            position: relative;
            overflow: hidden;
            width: 100px;
            height: 100px;
            transition: width 0.3s, height 0.3s;
        }

            .thumbnail:hover {
                width: 150px;
                height: 150px;
            }

        .thumbnail-image {
            width: 100%;
            height: 100%;
            object-fit: cover;
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
                <div class="modal-body"></div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hfStatusEmpDbId" runat="server" />
    <asp:HiddenField ID="hfStatusEmpCode" runat="server" />
    <asp:HiddenField ID="hfCurrentStatus" runat="server" />
    <div id="StatusChangeModal" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #f39c12; color: white;">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">×</span>
                    </button>
                    <h4 class="modal-title"><i class="fa fa-exclamation-triangle"></i>Confirm Status Change</h4>
                </div>
                <div class="modal-body">
                    <p id="lblStatusChangeDesc" style="font-size: 1.1em; margin-bottom: 15px;"></p>

                    <div id="divReasonSelection" style="display: none; padding: 10px; background: #f9f9f9; border: 1px solid #ddd; border-radius: 4px;">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Change Type <span class="text-danger">*</span></label>
                                    <select id="ddlChangeType" runat="server" class="form-control" onchange="toggleReasons()">
                                        <option value="">-- Select Type --</option>
                                        <option value="Temporary">Temporary (Leave/Suspension)</option>
                                        <option value="Permanent">Permanent (Exit/Resignation)</option>
                                    </select>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Specific Reason <span class="text-danger">*</span></label>
                                    <select id="ddlSpecificReason" runat="server" class="form-control">
                                        <option value="">-- Select Reason --</option>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div id="divPermanentDates" style="display: none; background: #fff; padding: 10px; border: 1px dashed #ccc; margin-bottom: 10px;">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label>Date of Resignation (DOR) <span class="text-danger">*</span></label>
                                        <asp:TextBox ID="txtDOR" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label>Last Working Day (DOE) <span class="text-danger">*</span></label>
                                        <asp:TextBox ID="txtDOE" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <label>Remarks (Optional)</label>
                            <asp:TextBox ID="txtStatusRemarks" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Add any HR notes here..."></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnConfirmStatusChange" runat="server" Text="Save Status" CssClass="btn btn-warning" OnClick="btnConfirmStatusChange_Click" OnClientClick="return validateStatusChange();" />
                    <button type="button" class="btn btn-default " data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hfResetEmpWrk" runat="server" />
    <asp:HiddenField ID="hfResetEmpName" runat="server" />
    <asp:HiddenField ID="hfResetLoginID" runat="server" />
    <asp:HiddenField ID="hfIsEmailEditMode" runat="server" Value="false" />
    <asp:HiddenField ID="hfResetMethod" runat="server" Value="email" />
    <div id="EmailConfirmModal" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #3498DB; color: white;">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                    <h4 class="modal-title"><i class="fa fa-key"></i>Reset Employee Password</h4>
                </div>
                <div class="modal-body">
                    <p>Please select how you want to reset the password for <b><span id="lblResetEmpName"></span></b>.</p>

                    <div class="form-group" style="margin-bottom: 20px;">
                        <div class="radio">
                            <label style="font-size: 1.1em; color: #2A3F54;">
                                <input type="radio" name="resetMethod" id="rbEmailFlow" value="email" checked onchange="toggleResetMethod()">
                                <strong>Option 1:</strong> Send Password via Email
                            </label>
                        </div>
                        <div class="radio">
                            <label style="font-size: 1.1em; color: #2A3F54;">
                                <input type="radio" name="resetMethod" id="rbManualFlow" value="manual" onchange="toggleResetMethod()">
                                <strong>Option 2:</strong> Generate Manually (Show on screen to copy)
                            </label>
                        </div>
                    </div>

                    <div id="divEmailFlow" style="background: #f9f9f9; padding: 15px; border: 1px solid #ddd; border-radius: 4px;">
                        <div class="form-group">
                            <label>Current Registered Email</label>
                            <asp:TextBox ID="txtCurrentEmail" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>

                        <div class="checkbox" style="margin-bottom: 15px;">
                            <label>
                                <input type="checkbox" id="chkUpdateEmail" onchange="toggleEmailEdit()">
                                <b>Update this email address</b></label>
                        </div>

                        <div id="divUpdateEmail" style="display: none; border-top: 1px dashed #ccc; padding-top: 15px;">
                            <h5 style="margin-top: 0; color: #d9534f; font-weight: bold;">Update Email via OTP</h5>

                            <div class="form-group">
                                <label>New Email Address <span class="text-danger">*</span></label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtNewEmail" runat="server" CssClass="form-control" placeholder="example@domain.com"></asp:TextBox>
                                    <span class="input-group-btn">
                                        <asp:Button ID="btnSendOTP" runat="server" Text="Send OTP" CssClass="btn btn-primary" OnClick="btnSendOTP_Click" OnClientClick="return validateNewEmail();" />
                                    </span>
                                </div>
                            </div>

                            <div class="form-group">
                                <label>Enter 6-Digit OTP <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtOTP" runat="server" CssClass="form-control" MaxLength="6" placeholder="------"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div id="divManualFlow" style="display: none; background: #fff3cd; padding: 15px; border: 1px solid #ffeeba; border-radius: 4px; color: #856404;">
                        <i class="fa fa-info-circle" style="font-size: 1.2em; margin-right: 5px;"></i>
                        <strong>Manual Reset Selected:</strong> The password will be generated immediately and displayed on the next screen. No email will be sent to the employee.
                    </div>

                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnConfirmAndReset" runat="server" Text="Confirm & Proceed" CssClass="btn btn-success" OnClick="btnConfirmAndReset_Click" OnClientClick="return validateResetSubmit();" />
                    <button type="button" class="btn btn-default btn-danger" data-dismiss="modal">Cancel</button>
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
                                                    <span class="badge badge-info" title="Employee Code" style="margin-right: 2px;"><%# Eval("WorkmanSL") %></span>
                                                    <span class="badge" style="background-color: #E7E7E7; color: #555; border: 1px solid #ccc; margin-right: 5px;" title="System Login ID">
                                                        <i class="fa fa-key"></i><%# Eval("LoginID") %>
                                                    </span>
                                                    <strong style="font-size: 1.1em; color: #2A3F54; text-transform: uppercase; vertical-align: middle;">
                                                        <%# Eval("FullName") %>
                                                    </strong>
                                                </div>
                                                <div style="color: #73879C; font-size: 0.9em; margin-bottom: 2px;">
                                                    <i class="fa fa-briefcase"></i><span><%# Eval("SkillDesignation") %></span>
                                                    <span style="color: #ccc; margin: 0 5px;">|</span>
                                                    <span style="font-weight: 600; color: #1ABB9C;"><%# Eval("SkillCategory") %></span>
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
                                                    <span style="font-weight: 600; color: #555;"><%# Eval("MobileNo") %></span><br />
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
                                                    <span class="text-danger">Exp: <%# Eval("SafetyPassExpiry", "{0:dd-MMM-yy}") %></span>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Action" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <div class="btn-group btn-group-sm">
                                                    <asp:LinkButton ID="btn_viewdetails" runat="server" CssClass="btn btn-info"
                                                        CommandName="View_Details" CommandArgument='<%# Eval("WorkmanSL") %>' ToolTip="View Profile">
                                                        <i class="fa fa-eye"></i>
                                                    </asp:LinkButton>
                                                    &nbsp;&nbsp;
                                                    <asp:LinkButton ID="btn_resetpwd" runat="server" CssClass="btn btn-warning"
                                                        CommandName="Reset_Password"
                                                        CommandArgument='<%# Eval("WorkmanSL") + "|" + Eval("Email") + "|" + Eval("FullName") + "|" + Eval("LoginID") %>'
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
        var jqNew = $.noConflict(true);

        jqNew(document).ready(function () {
            var table = jqNew('#<%= GridView1.ClientID %>').DataTable({
                "paging": true, "lengthChange": true, "searching": true, "ordering": true,
                "info": true, "autoWidth": false, "responsive": true, "pageLength": 10,
                "dom": 'Bfrtip',
                "buttons": [
                    { extend: 'excelHtml5', text: '<i class="fa fa-file-excel-o"></i> Export Excel', className: 'btn btn-success btn-sm', title: 'Employee_Master_Data', exportOptions: { columns: ':not(:last-child)' } },
                    { extend: 'pdfHtml5', text: '<i class="fa fa-file-pdf-o"></i> PDF', className: 'btn btn-danger btn-sm', orientation: 'landscape', pageSize: 'LEGAL', exportOptions: { columns: ':not(:last-child)' } },
                    'copy', 'print'
                ],
                "language": { "search": "_INPUT_", "searchPlaceholder": "Search Employees..." }
            });
        });

        function ShowPopup(title, body) {
            $("#MyPopup .modal-title").html(title);
            $("#MyPopup .modal-body").html(body);
            $("#MyPopup").modal("show");
        }

        function toggleReasons() {
            var type = document.getElementById('<%= ddlChangeType.ClientID %>').value;
            var reasonDropdown = document.getElementById('<%= ddlSpecificReason.ClientID %>');
            var dateSection = document.getElementById('divPermanentDates');

            reasonDropdown.innerHTML = '<option value="">-- Select Reason --</option>';

            if (type === 'Temporary') {
                dateSection.style.display = 'none';
                reasonDropdown.innerHTML += '<option value="Medical Leave">Medical Leave</option>';
                reasonDropdown.innerHTML += '<option value="Maternity/Paternity">Maternity/Paternity Leave</option>';
                reasonDropdown.innerHTML += '<option value="Suspension">Suspension</option>';
                reasonDropdown.innerHTML += '<option value="Sabbatical">Sabbatical</option>';
                reasonDropdown.innerHTML += '<option value="LWP">Leave Without Pay (LWP)</option>';
            } else if (type === 'Permanent') {
                dateSection.style.display = 'block';
                reasonDropdown.innerHTML += '<option value="Resignation">Resignation</option>';
                reasonDropdown.innerHTML += '<option value="Termination">Termination</option>';
                reasonDropdown.innerHTML += '<option value="Absconding">Absconding</option>';
                reasonDropdown.innerHTML += '<option value="Retirement">Retirement</option>';
                reasonDropdown.innerHTML += '<option value="Deceased">Deceased</option>';
            } else {
                dateSection.style.display = 'none';
            }
        }

        function validateStatusChange() {
            var type = document.getElementById('<%= ddlChangeType.ClientID %>').value;
            var reason = document.getElementById('<%= ddlSpecificReason.ClientID %>').value;
            var isDeactivating = document.getElementById('divReasonSelection').style.display !== 'none';

            if (isDeactivating) {
                if (type === '' || reason === '') { alert("Please select both a Change Type and a Specific Reason."); return false; }
                if (type === 'Permanent') {
                    var dor = document.getElementById('<%= txtDOR.ClientID %>').value;
                    var doe = document.getElementById('<%= txtDOE.ClientID %>').value;
                    if (dor === '' || doe === '') { alert("Please provide both DOR and DOE."); return false; }
                }
            }
            return true;
        }

        function openStatusModal(empCode, newStatus, isDeactivating) {
            jqNew("#lblStatusChangeDesc").html("You are about to change the status of Employee <b>" + empCode + "</b> to <b>" + newStatus + "</b>.");
            if (isDeactivating) { jqNew("#divReasonSelection").show(); } else { jqNew("#divReasonSelection").hide(); }
            $("#StatusChangeModal").modal("show");
        }

        function toggleResetMethod() {
            var isEmail = document.getElementById('rbEmailFlow').checked;
            document.getElementById('divEmailFlow').style.display = isEmail ? 'block' : 'none';
            document.getElementById('divManualFlow').style.display = isEmail ? 'none' : 'block';
            document.getElementById('<%= hfResetMethod.ClientID %>').value = isEmail ? 'email' : 'manual';
        }

        function toggleEmailEdit() {
            var isEdit = document.getElementById('chkUpdateEmail').checked;
            document.getElementById('divUpdateEmail').style.display = isEdit ? 'block' : 'none';
            document.getElementById('<%= hfIsEmailEditMode.ClientID %>').value = isEdit ? 'true' : 'false';
        }

        function validateNewEmail() {
            var email = document.getElementById('<%= txtNewEmail.ClientID %>').value;
            if (email.trim() === '') { alert("Please enter a new email address."); return false; }
            return true;
        }

        function validateResetSubmit() {
            var method = document.getElementById('<%= hfResetMethod.ClientID %>').value;
            
            if (method === 'email') {
                var isEditMode = document.getElementById('<%= hfIsEmailEditMode.ClientID %>').value;
                if (isEditMode === 'true') {
                    var newEmail = document.getElementById('<%= txtNewEmail.ClientID %>').value;
                    if(newEmail.trim() === '') {
                        alert("Please enter a new email address or uncheck 'Update Email'.");
                        return false;
                    }
                    var otp = document.getElementById('<%= txtOTP.ClientID %>').value;
                    if (otp.trim() === '' || otp.length < 6) { 
                        alert("Please enter the 6-digit OTP sent to the new email."); 
                        return false; 
                    }
                } else {
                    var currentEmail = document.getElementById('<%= txtCurrentEmail.ClientID %>').value;
                    if (currentEmail === '' || currentEmail === 'N/A') {
                        alert("There is no email on file. Please select 'Option 2: Generate Manually' or check the box to update the email.");
                        return false;
                    }
                }
            }
            return confirm('Are you sure you want to proceed with the password reset?');
        }

        function openEmailModal(name, forceEditMode) {
            document.getElementById('lblResetEmpName').innerText = name;
            var currentEmail = document.getElementById('<%= txtCurrentEmail.ClientID %>').value;
            
            // Reset to defaults
            document.getElementById('rbEmailFlow').checked = true;
            document.getElementById('chkUpdateEmail').checked = false;

            // Handle UI State
            if (forceEditMode) {
                document.getElementById('chkUpdateEmail').checked = true;
            } else if (currentEmail === '' || currentEmail === 'N/A') {
                // If there is no email on file, automatically select Option 2 for the user!
                document.getElementById('rbManualFlow').checked = true;
            }

            toggleResetMethod();
            toggleEmailEdit();
            $("#EmailConfirmModal").modal("show");
        }

        function CopyShareText() {
            var copyText = document.getElementById("txtShare");
            copyText.style.display = "block";
            copyText.select();
            copyText.setSelectionRange(0, 99999);
            document.execCommand("copy");
            copyText.style.display = "none";
            alert("Credentials copied! You can now paste them in WhatsApp/Teams.");
        }
    </script>
</asp:Content>
