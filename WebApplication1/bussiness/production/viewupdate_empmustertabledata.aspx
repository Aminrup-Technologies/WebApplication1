<%@ Page Title="Master Control Center" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="viewupdate_empmustertabledata.aspx.cs" Inherits="WebApplication1.bussiness.production.viewupdate_empmustertabledata" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /* Compact Admin Theme */
        body {
            font-size: 13px;
        }

        .form-control {
            height: 30px;
            padding: 2px 6px;
            font-size: 12px;
        }

        label {
            font-size: 12px;
            margin-bottom: 2px;
            font-weight: 600;
            color: #444;
        }

        .form-group {
            margin-bottom: 8px;
        }

        .x_panel {
            padding: 10px;
        }

        /* Tab Styling */
        .nav-tabs.bar_tabs > li.active {
            border-top: 3px solid #1ABB9C;
            margin-top: 0;
            background: #fff;
        }

        .tab-pane {
            padding-top: 15px;
        }

        .badge-verified {
            background-color: #26b99a;
            color: white;
            padding: 3px 6px;
            border-radius: 3px;
            font-size: 11px;
        }

        .section-header {
            border-bottom: 2px solid #E6E9ED;
            padding-bottom: 5px;
            margin-bottom: 10px;
            color: #2a3f54;
            font-weight: 700;
            font-size: 14px;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            // 1. Get saved tab from HiddenField
            var activeTab = $('#<%= hfActiveTab.ClientID %>').val();

            // 2. Logic to activate the correct tab
            if (activeTab && activeTab != "") {
                // Remove default active classes
                $('.nav-tabs li').removeClass('active');
                $('.tab-pane').removeClass('active in show');

                // Add active class to the specific tab link's parent <li>
                $('.nav-tabs a[href="' + activeTab + '"]').parent('li').addClass('active');

                // Show the corresponding content div
                $(activeTab).addClass('active in show');
            } else {
                // Default to first tab if nothing saved
                $('.nav-tabs li:first').addClass('active');
                $('.tab-pane:first').addClass('active in show');
            }

            // 3. Save tab ID on click
            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                var target = $(e.target).attr("href");
                $('#<%= hfActiveTab.ClientID %>').val(target);
            });
        });

        function ShowPopup(title, body) {
            $("#popTitle").text(title);
            $("#popBody").html(body);
            $("#MyPopup").modal("show");
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfActiveTab" runat="server" />

    <div class="right_col" role="main">
        <div class="page-title">
            <div class="title_left">
                <h3><i class="fa fa-users"></i>Employee Master Control</h3>
            </div>
        </div>
        <div class="clearfix"></div>

        <div class="row">
            <div class="col-md-12 col-sm-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            <asp:Label ID="lbl_EmpNameHeader" runat="server" Text="Employee Name" />
                            <small>(ID:
                                <asp:Label ID="lbl_EmpID" runat="server" ForeColor="#1ABB9C" />)</small></h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>

                    <div class="x_content">
                        <ul class="nav nav-tabs bar_tabs" id="masterTab" role="tablist">
                            <li class="active"><a data-toggle="tab" href="#tab_personal"><i class="fa fa-user"></i>Personal</a></li>
                            <li><a data-toggle="tab" href="#tab_job"><i class="fa fa-briefcase"></i>Job & Skills</a></li>
                            <li><a data-toggle="tab" href="#tab_payroll"><i class="fa fa-money"></i>Payroll</a></li>
                            <li><a data-toggle="tab" href="#tab_bank"><i class="fa fa-bank"></i>Bank/Statutory</a></li>
                            <li><a data-toggle="tab" href="#tab_compliance"><i class="fa fa-shield"></i>Compliance</a></li>
                            <li><a data-toggle="tab" href="#tab_docs"><i class="fa fa-file-pdf-o"></i>Documents</a></li>
                            <li><a data-toggle="tab" href="#tab_admin" class="text-danger"><i class="fa fa-lock"></i><b>Admin/Exit</b></a></li>
                        </ul>

                        <div class="tab-content">
                            <div id="tab_personal" class="tab-pane fade in active">
                                <h4 class="section-header">Identity Details</h4>
                                <div class="row">
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>First Name *</label><asp:TextBox ID="txt_fname" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Middle Name</label><asp:TextBox ID="txt_mname" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Last Name *</label><asp:TextBox ID="txt_lname" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Full Name (Auto)</label><asp:TextBox ID="txt_fullname" runat="server" CssClass="form-control" ReadOnly="true" BackColor="#f2f2f2" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Father's Name</label><asp:TextBox ID="txt_father" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Mother's Name</label><asp:TextBox ID="txt_mother" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>DOB *</label><asp:TextBox ID="txt_dob" runat="server" CssClass="form-control" TextMode="Date" />
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Blood Group</label><asp:TextBox ID="txt_blood" runat="server" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Mobile No *</label><asp:TextBox ID="txt_mobile" runat="server" CssClass="form-control" MaxLength="10" />
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Email ID</label><asp:TextBox ID="txt_email" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Education</label><asp:DropDownList ID="DDL_Education" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div id="tab_job" class="tab-pane fade">

                                <h4 class="section-header"><i class="fa fa-sitemap"></i>Organization Hierarchy</h4>
                                <div class="row">
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Work Country</label>
                                        <asp:TextBox ID="txt_Country" runat="server" CssClass="form-control" ReadOnly="true" BackColor="#f9f9f9" />
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Work State</label>
                                        <asp:TextBox ID="txt_State" runat="server" CssClass="form-control" ReadOnly="true" BackColor="#f9f9f9" />
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Work Region</label>
                                        <asp:TextBox ID="txt_Region" runat="server" CssClass="form-control" ReadOnly="true" BackColor="#f9f9f9" />
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Company Code</label>
                                        <asp:TextBox ID="txt_Company" runat="server" CssClass="form-control" ReadOnly="true" BackColor="#f9f9f9" />
                                    </div>
                                </div>

                                <h4 class="section-header mt-2"><i class="fa fa-briefcase"></i>Job Mapping</h4>
                                <div class="row">
                                    <div class="col-md-4 col-sm-6 form-group">
                                        <label>Worksite</label>
                                        <asp:DropDownList ID="DDL_Worksite" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-4 col-sm-6 form-group">
                                        <label>Skill Category</label>
                                        <asp:DropDownList ID="DDL_SkillCat" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDL_SkillCat_SelectedIndexChanged" />
                                    </div>
                                    <div class="col-md-4 col-sm-6 form-group">
                                        <label>Skill Designation</label>
                                        <asp:DropDownList ID="DDL_Designation" runat="server" CssClass="form-control" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Role Type</label>
                                        <asp:DropDownList ID="DDL_UserRole" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Permission Level</label>
                                        <asp:DropDownList ID="DDL_Role" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Work Hours</label>
                                        <asp:DropDownList ID="DDL_WorkHours" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Date of Joining</label>
                                        <asp:TextBox ID="txt_doj" runat="server" CssClass="form-control" TextMode="Date" />
                                    </div>
                                </div>

                                <h4 class="section-header mt-2"><i class="fa fa-clock-o"></i>System Access Log</h4>
                                <div class="row">
                                    <div class="col-md-4 col-sm-6 form-group">
                                        <label>Login Active?</label>
                                        <asp:Label ID="lbl_LoginStatus" runat="server" CssClass="form-control" Style="border: none; font-weight: bold;"></asp:Label>
                                    </div>
                                    <div class="col-md-4 col-sm-6 form-group">
                                        <label>Last Login Time</label>
                                        <asp:TextBox ID="txt_LastLogin" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="col-md-4 col-sm-6 form-group">
                                        <label>Last Logout Time</label>
                                        <asp:TextBox ID="txt_LastLogout" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                            </div>

                            <div id="tab_payroll" class="tab-pane fade">
                                <h4 class="section-header">Earnings Configuration</h4>
                                <div class="row">
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Fixed Salary</label><asp:TextBox ID="txt_fixed_amt" runat="server" CssClass="form-control" Text="0.00" />
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>DA / VDA</label><asp:TextBox ID="txt_da_vda" runat="server" CssClass="form-control" Text="0.00" />
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>HRA</label><asp:TextBox ID="txt_hra" runat="server" CssClass="form-control" Text="0.00" />
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Conv. Allowance</label><asp:TextBox ID="txt_conv" runat="server" CssClass="form-control" Text="0.00" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Medical Allow.</label><asp:TextBox ID="txt_medical" runat="server" CssClass="form-control" Text="0.00" />
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Washing Allow.</label><asp:TextBox ID="txt_washing" runat="server" CssClass="form-control" Text="0.00" />
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Att. Bonus</label><asp:TextBox ID="txt_att_bonus" runat="server" CssClass="form-control" Text="0.00" />
                                    </div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>OT Factor</label><asp:DropDownList ID="DDL_OTFactor" runat="server" CssClass="form-control" />
                                    </div>
                                </div>
                            </div>

                            <div id="tab_bank" class="tab-pane fade">
                                <div class="row">
                                    <div class="col-md-6">
                                        <h4 class="section-header">Statutory Info</h4>
                                        <div class="form-group">
                                            <label>UAN Number</label><asp:TextBox ID="txt_uan" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            <label>ESIC Number</label><asp:TextBox ID="txt_esic" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>
                                    <div class="col-md-6" style="border-left: 1px solid #ddd;">
                                        <h4 class="section-header">Bank Details (Salary)</h4>
                                        <div class="form-group">
                                            <label>Bank Name</label><asp:DropDownList ID="DDL_BankName" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            <label>Account Number</label><asp:TextBox ID="txt_acc_no" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            <label>IFSC Code</label><asp:TextBox ID="txt_ifsc" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="tab_compliance" class="tab-pane fade">
                                <h4 class="section-header">Gate & Safety</h4>
                                <div class="row">
                                    <div class="col-md-4 form-group">
                                        <label>Safety Pass No</label><asp:TextBox ID="txt_rfid" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-4 form-group">
                                        <label>Safety Validity</label><asp:TextBox ID="txt_rfid_val" runat="server" CssClass="form-control" TextMode="Date" />
                                    </div>
                                    <div class="col-md-4 form-group">
                                        <label>PV Expiry</label><asp:TextBox ID="txt_pv_val" runat="server" CssClass="form-control" TextMode="Date" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4 form-group">
                                        <label>Gatepass No</label><asp:TextBox ID="txt_gp_no" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-4 form-group">
                                        <label>Gatepass Expiry</label><asp:TextBox ID="txt_gp_val" runat="server" CssClass="form-control" TextMode="Date" />
                                    </div>
                                </div>
                                <h4 class="section-header mt-3">Account Security</h4>
                                <div class="row">
                                    <div class="col-md-4">
                                        <label>Plain Password</label><asp:TextBox ID="txt_plain_pass" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="col-md-4">
                                        <label>Security Q1</label><asp:TextBox ID="txt_sq1" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="col-md-4">
                                        <label>Answer 1</label><asp:TextBox ID="txt_sqans1" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                            </div>

                            <div id="tab_docs" class="tab-pane fade">
                                <h4 class="section-header">Document Verification</h4>
                                <div class="alert alert-info" style="padding: 5px;">
                                    <i class="fa fa-info-circle"></i><strong>Rejection Note:</strong> Enter reason below before rejecting.
                                    <asp:TextBox ID="txt_DocAdminNote" runat="server" CssClass="form-control mt-1" placeholder="e.g. Image blurry, please scan original..." />
                                </div>
                                <table class="table table-striped table-hover table-condensed">
                                    <thead>
                                        <tr>
                                            <th>Document Type</th>
                                            <th>Status</th>
                                            <th>Uploaded On</th>
                                            <th>Actions</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>Aadhaar Card</td>
                                            <td>
                                                <asp:Label ID="lbl_AadhaarStat" runat="server" /></td>
                                            <td>
                                                <asp:Label ID="lbl_AadhaarDate" runat="server" /></td>
                                            <td>
                                                <asp:HyperLink ID="lnk_ViewAadhaar" runat="server" Target="_blank" CssClass="btn btn-primary btn-xs"><i class="fa fa-eye"></i></asp:HyperLink>
                                                <asp:Button ID="btn_RejectAadhaar" runat="server" Text="Reject" CssClass="btn btn-danger btn-xs" OnClick="btn_RejectDoc_Click" CommandArgument="Aadhaar" OnClientClick="return confirm('Reject Aadhaar?');" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>PAN Card</td>
                                            <td>
                                                <asp:Label ID="lbl_PanStat" runat="server" /></td>
                                            <td>
                                                <asp:Label ID="lbl_PanDate" runat="server" /></td>
                                            <td>
                                                <asp:HyperLink ID="lnk_ViewPan" runat="server" Target="_blank" CssClass="btn btn-primary btn-xs"><i class="fa fa-eye"></i></asp:HyperLink>
                                                <asp:Button ID="btn_RejectPan" runat="server" Text="Reject" CssClass="btn btn-danger btn-xs" OnClick="btn_RejectDoc_Click" CommandArgument="Pan" OnClientClick="return confirm('Reject PAN?');" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Bank Proof</td>
                                            <td>
                                                <asp:Label ID="lbl_BankStat" runat="server" /></td>
                                            <td>
                                                <asp:Label ID="lbl_BankDate" runat="server" /></td>
                                            <td>
                                                <asp:HyperLink ID="lnk_ViewBank" runat="server" Target="_blank" CssClass="btn btn-primary btn-xs"><i class="fa fa-eye"></i></asp:HyperLink>
                                                <asp:Button ID="btn_RejectBank" runat="server" Text="Reject" CssClass="btn btn-danger btn-xs" OnClick="btn_RejectDoc_Click" CommandArgument="Bank" OnClientClick="return confirm('Reject Bank Doc?');" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>

                            <div id="tab_admin" class="tab-pane fade">
                                <div class="admin-alert">
                                    <h4 class="text-danger section-header"><i class="fa fa-exclamation-triangle"></i>Account & Exit Management</h4>
                                    <div class="row">
                                        <div class="col-md-4 form-group">
                                            <label>Current Status</label>
                                            <asp:Label ID="lbl_CurrentStatus" runat="server" CssClass="badge badge-info p-2 d-block" Text="Loading..."></asp:Label>
                                        </div>
                                        <div class="col-md-4 form-group">
                                            <label>Set Status Action</label>
                                            <asp:DropDownList ID="DDL_AdminStatus" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDL_AdminStatus_SelectedIndexChanged">
                                                <asp:ListItem Text="Active" Value="Active" />
                                                <asp:ListItem Text="Blocked (Admin)" Value="Blocked" />
                                                <asp:ListItem Text="Exited (Final)" Value="Exited" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-4 form-group">
                                            <label>Login Access</label>
                                            <asp:DropDownList ID="DDL_LoginAccess" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Enabled" Value="1" />
                                                <asp:ListItem Text="Disabled" Value="0" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <asp:Panel ID="pnl_Exit" runat="server" Visible="false">
                                        <div class="row">
                                            <div class="col-md-4 form-group">
                                                <label>Date of Relief (DOR) *</label>
                                                <asp:TextBox ID="txt_DOR" runat="server" CssClass="form-control" TextMode="Date" />
                                            </div>
                                            <div class="col-md-4 form-group">
                                                <label>Exit Type *</label>
                                                <asp:DropDownList ID="DDL_ExitType" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="-- Select --" Value="" />
                                                    <asp:ListItem Text="Resigned" Value="Resigned" />
                                                    <asp:ListItem Text="Terminated" Value="Terminated" />
                                                    <asp:ListItem Text="Absconded" Value="Absconded" />
                                                    <asp:ListItem Text="Retired" Value="Retired" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                    <div class="row mt-2">
                                        <div class="col-md-12">
                                            <label>Reason for Action / Change Log <span class="text-danger">*</span></label>
                                            <asp:TextBox ID="txt_AdminReason" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Required for Audit Trail..." />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="ln_solid"></div>
                        <div class="row">
                            <div class="col-md-6">
                                <asp:Label ID="lbl_msg" runat="server" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="col-md-6 text-right">
                                <asp:Button ID="btn_Cancel" runat="server" Text="Back to List" CssClass="btn btn-default btn-sm" PostBackUrl="view_emp_mastertbldata.aspx" CausesValidation="false" />
                                <asp:Button ID="btn_SaveAll" runat="server" Text="SAVE ALL CHANGES" CssClass="btn btn-success btn-sm" OnClick="btn_SaveAll_Click" OnClientClick="return confirm('Are you sure you want to commit these changes to the Database?');" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="MyPopup" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="popTitle"></h4>
                </div>
                <div class="modal-body" id="popBody"></div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
