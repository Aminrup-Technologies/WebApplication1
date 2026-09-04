<%@ Page Title="Master Control Center" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="viewupdate_empmustertabledata_v2.aspx.cs" Inherits="WebApplication1.bussiness.production.viewupdate_empmustertabledata_v2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
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
            margin-bottom: 10px;
        }

        .x_panel {
            padding: 10px;
        }

        .x_title h2 {
            font-size: 18px;
        }

        .nav-tabs.bar_tabs > li.active {
            border-top: 3px solid #1ABB9C;
            margin-top: 0;
            background: #fff;
        }

        .nav-tabs.bar_tabs > li > a {
            padding: 8px 15px;
            font-weight: 600;
        }

        .tab-pane {
            padding-top: 15px;
        }

        .section-header {
            border-bottom: 2px solid #E6E9ED;
            padding-bottom: 5px;
            margin-bottom: 15px;
            color: #2a3f54;
            font-weight: 700;
            font-size: 13px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            background: #f9f9f9;
            padding: 8px;
        }

        .badge-verified {
            background-color: #26b99a;
            color: white;
            padding: 3px 6px;
            border-radius: 3px;
            font-size: 11px;
        }

        .badge-pending {
            background-color: #f0ad4e;
            color: white;
            padding: 3px 6px;
            border-radius: 3px;
            font-size: 11px;
        }

        .admin-alert {
            background-color: #fff3f3;
            border: 1px solid #ffcccc;
            padding: 10px;
            border-radius: 4px;
        }

        .readonly-text {
            background-color: #f5f5f5 !important;
            color: #555;
        }

        .badge-verified {
            background-color: #26b99a;
            color: white;
            padding: 3px 6px;
            border-radius: 3px;
            font-size: 11px;
        }

        .badge-pending {
            background-color: #f0ad4e;
            color: white;
            padding: 3px 6px;
            border-radius: 3px;
            font-size: 11px;
        }

        .badge-rejected {
            background-color: #d9534f;
            color: white;
            padding: 3px 6px;
            border-radius: 3px;
            font-size: 11px;
        }

        .badge-missing {
            background-color: #999999;
            color: white;
            padding: 3px 6px;
            border-radius: 3px;
            font-size: 11px;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            // --- 1. TAB PERSISTENCE & UI SYNC LOGIC ---
            var activeTab = $('#<%= hfActiveTab.ClientID %>').val();
            if (activeTab && activeTab != "") {
                $('.nav-tabs li').removeClass('active');
                $('.tab-pane').removeClass('active in show');
                $('.nav-tabs a[href="' + activeTab + '"]').parent('li').addClass('active');
                $(activeTab).addClass('active in show');
            } else {
                $('.nav-tabs li:first').addClass('active');
                $('.tab-pane:first').addClass('active in show');
            }

            $('.nav-tabs a[data-toggle="tab"]').on('click', function () {
                $('.nav-tabs li').removeClass('active');
                $(this).parent('li').addClass('active');
                var target = $(this).attr("href");
                $('#<%= hfActiveTab.ClientID %>').val(target);
            });

            // --- 2. AUTO-GENERATE FULL NAME LOGIC ---
            $('#<%= txt_fname.ClientID %>, #<%= txt_mname.ClientID %>, #<%= txt_lname.ClientID %>').on('input keyup', function () {
                var firstName = $('#<%= txt_fname.ClientID %>').val().trim();
                var middleName = $('#<%= txt_mname.ClientID %>').val().trim();
                var lastName = $('#<%= txt_lname.ClientID %>').val().trim();

                var fullName = firstName;
                if (middleName.length > 0) { fullName += " " + middleName; }
                if (lastName.length > 0) { fullName += " " + lastName; }

                $('#<%= txt_fullname.ClientID %>').val(fullName);
            });

            // --- 3. STATUS UI TOGGLES ---
            toggleStatusUI();
        });

        // Admin Tab Functions
        function toggleStatusUI() {
            var status = $('#DDL_AdminStatus').val();
            if (status === "InActive") {
                $("#divReasonSelection").slideDown(200);
                toggleReasons();
            } else {
                $("#divReasonSelection").slideUp(200);
            }
        }

        function toggleReasons() {
            var type = document.getElementById('<%= ddlChangeType.ClientID %>').value;
            var reasonDropdown = document.getElementById('ddlSpecificReason');
            var dateSection = document.getElementById('divPermanentDates');
            var currentReason = $('#hfSavedReason').val();

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

            if (currentReason) { $(reasonDropdown).val(currentReason); }
        }

        function saveReason() {
            $('#hfSavedReason').val($('#ddlSpecificReason').val());
        }

        function ShowPopup(title, body) {
            $("#popTitle").text(title);
            $("#popBody").html(body);
            $("#MyPopup").modal("show");
        }

        function ShowAccessDenied() {
            if (typeof PNotify !== 'undefined') {
                new PNotify({
                    title: 'Access Restricted <i class="fa fa-lock"></i>',
                    text: 'You do not have the required permissions to view Payroll & Allowances data.',
                    type: 'error',
                    styling: 'bootstrap3',
                    delay: 3000
                });
            } else {
                alert('Access Restricted: You do not have permission to view Payroll data.');
            }
        }

        function openImageModal(imgSrc) {
            document.getElementById('enlargedProfilePic').src = imgSrc;
            $('#ImageModal').modal('show');
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
                            <small>(Workman SL:
                                <asp:Label ID="lbl_EmpID" runat="server" ForeColor="#1ABB9C" Font-Bold="true" />)</small>
                        </h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>

                    <div class="x_content">
                        <ul class="nav nav-tabs bar_tabs" id="masterTab" role="tablist">
                            <li class="active"><a data-toggle="tab" href="#tab_personal"><i class="fa fa-user"></i>Personal & Contact</a></li>
                            <li><a data-toggle="tab" href="#tab_job"><i class="fa fa-briefcase"></i>Job & Skills</a></li>
                            <li id="nav_payroll" runat="server">
                                <a data-toggle="tab" href="#tab_payroll" id="lnk_payroll" runat="server" clientidmode="Static">
                                    <i class="fa fa-money" id="icon_payroll" runat="server"></i>Payroll
                                </a>
                            </li>
                            <li><a data-toggle="tab" href="#tab_bank"><i class="fa fa-bank"></i>Statutory & Bank</a></li>
                            <li><a data-toggle="tab" href="#tab_compliance"><i class="fa fa-shield"></i>Compliance</a></li>
                            <li><a data-toggle="tab" href="#tab_docs"><i class="fa fa-file-pdf-o"></i>Documents</a></li>
                            <li><a data-toggle="tab" href="#tab_admin" class="text-danger"><i class="fa fa-lock"></i><b>Access & Exit</b></a></li>
                            <li><a data-toggle="tab" href="#tab_logs" class="text-info"><i class="fa fa-history"></i> <b>History & Logs</b></a></li>
                        </ul>

                        <div class="tab-content">
                            <div id="tab_personal" class="tab-pane fade in active">
                                <h4 class="section-header">Identity Details</h4>
                                <div class="row">
                                    <div class="col-md-10 col-sm-9">
                                        <div class="row">
                                            <div class="col-md-3 col-sm-6 form-group">
                                                <label>First Name</label><asp:TextBox ID="txt_fname" runat="server" CssClass="form-control" /></div>
                                            <div class="col-md-3 col-sm-6 form-group">
                                                <label>Middle Name</label><asp:TextBox ID="txt_mname" runat="server" CssClass="form-control" /></div>
                                            <div class="col-md-3 col-sm-6 form-group">
                                                <label>Last Name</label><asp:TextBox ID="txt_lname" runat="server" CssClass="form-control" /></div>
                                            <div class="col-md-3 col-sm-6 form-group">
                                                <label>Full Name (Auto)</label><asp:TextBox ID="txt_fullname" runat="server" CssClass="form-control readonly-text" ReadOnly="true" ClientIDMode="Static" /></div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-3 col-sm-6 form-group">
                                                <label>Father's Name</label><asp:TextBox ID="txt_father" runat="server" CssClass="form-control" /></div>
                                            <div class="col-md-3 col-sm-6 form-group">
                                                <label>Mother's Name</label><asp:TextBox ID="txt_mother" runat="server" CssClass="form-control" /></div>
                                            <div class="col-md-3 col-sm-6 form-group">
                                                <label>Date Of Birth</label><asp:TextBox ID="txt_dob" runat="server" CssClass="form-control" TextMode="Date" /></div>
                                            <div class="col-md-3 col-sm-6 form-group">
                                                <label>Blood Group</label><asp:TextBox ID="txt_blood" runat="server" CssClass="form-control" MaxLength="5" /></div>
                                        </div>
                                    </div>
                                    <div class="col-md-2 col-sm-3 text-center" style="display: flex; flex-direction: column; align-items: center; justify-content: center; border-left: 1px dashed #ddd;">
                                        <asp:Image ID="img_ProfilePic" runat="server" Width="115px" Height="115px" ImageUrl="~/images/placeholder.png"
                                            Style="border-radius: 8px; object-fit: cover; border: 2px solid #1ABB9C; cursor: pointer; transition: transform 0.2s;"
                                            onclick="openImageModal(this.src);" ToolTip="Click to view full size"
                                            onmouseover="this.style.transform='scale(1.05)'" onmouseout="this.style.transform='scale(1)'" />
                                        <span class="text-muted mt-2" style="font-size: 11px; font-weight: 600;"><i class="fa fa-search-plus"></i>Click to Enlarge</span>
                                    </div>
                                </div>
                                <h4 class="section-header mt-2">Contact Details</h4>
                                <div class="row">
                                    <div class="col-md-4 col-sm-6 form-group">
                                        <label>Mobile No</label><asp:TextBox ID="txt_mobile" runat="server" CssClass="form-control" MaxLength="10" /></div>
                                    <div class="col-md-4 col-sm-6 form-group">
                                        <label>Email ID</label><asp:TextBox ID="txt_email" runat="server" CssClass="form-control" /></div>
                                    <div class="col-md-4 col-sm-6 form-group">
                                        <label>Contact Update On</label><asp:TextBox ID="txt_ContactUpdateOn" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                </div>
                            </div>

                            <div id="tab_job" class="tab-pane fade">
                                <h4 class="section-header">Organization Hierarchy (Read-Only)</h4>
                                <div class="row">
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Work Country</label><asp:TextBox ID="txt_Country" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Work State</label><asp:TextBox ID="txt_State" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Work Region</label><asp:TextBox ID="txt_Region" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Company Code</label><asp:TextBox ID="txt_Company" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                </div>
                                <h4 class="section-header mt-2">Professional Mapping</h4>
                                <div class="row">
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Highest Qualification</label><asp:DropDownList ID="DDL_Education" runat="server" CssClass="form-control" /></div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Worksite</label><asp:DropDownList ID="DDL_Worksite" runat="server" CssClass="form-control" /></div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Skill Category</label><asp:DropDownList ID="DDL_SkillCat" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDL_SkillCat_SelectedIndexChanged" /></div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Skill Designation</label><asp:DropDownList ID="DDL_Designation" runat="server" CssClass="form-control" /></div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Role Type</label><asp:DropDownList ID="DDL_UserRole" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDL_UserRole_SelectedIndexChanged" /></div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Permission Level</label><asp:DropDownList ID="DDL_Role" runat="server" CssClass="form-control" /></div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Date of Joining (DOJ)</label><asp:TextBox ID="txt_doj" runat="server" CssClass="form-control" TextMode="Date" /></div>
                                    <div class="col-md-3 col-sm-6 form-group">
                                        <label>Registration Type</label><asp:TextBox ID="txt_RegType" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                </div>
                            </div>

                            <div id="tab_payroll" class="tab-pane fade" runat="server" clientidmode="Static">
                                <h4 class="section-header">Payroll Configurations</h4>
                                <div class="row">
                                    <div class="col-md-2 col-sm-4 form-group">
                                        <label>Work Hours</label><asp:DropDownList ID="DDL_WorkHours" runat="server" CssClass="form-control" /></div>
                                    <div class="col-md-2 col-sm-4 form-group">
                                        <label>OT Factor</label><asp:DropDownList ID="DDL_OTFactor" runat="server" CssClass="form-control" /></div>
                                    <div class="col-md-2 col-sm-4 form-group">
                                        <label>OT Multiplier</label><asp:TextBox ID="txt_OTMult" runat="server" CssClass="form-control" /></div>
                                    <div class="col-md-2 col-sm-4 form-group">
                                        <label>OT Divisibility</label><asp:TextBox ID="txt_OTDiv" runat="server" CssClass="form-control" TextMode="Number" /></div>
                                    <div class="col-md-2 col-sm-4 form-group">
                                        <label>Fixed Salary?</label>
                                        <asp:DropDownList ID="DDL_FixedSal" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="No">No</asp:ListItem>
                                            <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2 col-sm-4 form-group">
                                        <label>Show F16 (Att. Rep)</label>
                                        <asp:DropDownList ID="DDL_F16" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                            <asp:ListItem Value="No">No</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2 col-sm-4 form-group">
                                        <label>Show F17 (Pay Rep)</label>
                                        <asp:DropDownList ID="DDL_F17" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                            <asp:ListItem Value="No">No</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <h4 class="section-header mt-2">Earnings & Allowances</h4>
                                <div class="row">
                                    <div class="col-md-2 col-sm-4 form-group">
                                        <label>Fixed Amount</label><asp:TextBox ID="txt_fixed_amt" runat="server" CssClass="form-control" Text="0.00" /></div>
                                    <div class="col-md-2 col-sm-4 form-group">
                                        <label>DA / VDA</label><asp:TextBox ID="txt_da_vda" runat="server" CssClass="form-control" Text="0.00" /></div>
                                    <div class="col-md-2 col-sm-4 form-group">
                                        <label>HRA</label><asp:TextBox ID="txt_hra" runat="server" CssClass="form-control" Text="0.00" /></div>
                                    <div class="col-md-2 col-sm-4 form-group">
                                        <label>Conv. Allowance</label><asp:TextBox ID="txt_conv" runat="server" CssClass="form-control" Text="0.00" /></div>
                                    <div class="col-md-2 col-sm-4 form-group">
                                        <label>Medical Allow.</label><asp:TextBox ID="txt_medical" runat="server" CssClass="form-control" Text="0.00" /></div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2 col-sm-4 form-group">
                                        <label>Washing Allow.</label><asp:TextBox ID="txt_washing" runat="server" CssClass="form-control" Text="0.00" /></div>
                                    <div class="col-md-2 col-sm-4 form-group">
                                        <label>Att. Bonus</label><asp:TextBox ID="txt_att_bonus" runat="server" CssClass="form-control" Text="0.00" /></div>
                                    <div class="col-md-2 col-sm-4 form-group">
                                        <label>Special Allow.</label><asp:TextBox ID="txt_spcl" runat="server" CssClass="form-control" Text="0.00" /></div>
                                    <div class="col-md-2 col-sm-4 form-group">
                                        <label>Misc Earnings</label><asp:TextBox ID="txt_misc" runat="server" CssClass="form-control" Text="0.00" /></div>
                                </div>

                                <h4 class="section-header mt-2">Deductions & Balances (ReadOnly)</h4>
                                <div class="row">
                                    <div class="col-md-2 form-group">
                                        <label>Advance Bal</label><asp:TextBox ID="txt_RemAdv" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                    <div class="col-md-2 form-group">
                                        <label>Cur. Advance</label><asp:TextBox ID="txt_CurAdv" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                    <div class="col-md-2 form-group">
                                        <label>Fines Bal</label><asp:TextBox ID="txt_RemFines" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                    <div class="col-md-2 form-group">
                                        <label>Cur. Fines</label><asp:TextBox ID="txt_CurFines" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                    <div class="col-md-2 form-group">
                                        <label>Others Bal</label><asp:TextBox ID="txt_RemOthers" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                    <div class="col-md-2 form-group">
                                        <label>Cur. Others</label><asp:TextBox ID="txt_CurOthers" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                </div>
                            </div>

                            <div id="tab_bank" class="tab-pane fade">
                                <div class="row">
                                    <div class="col-md-4">
                                        <h4 class="section-header">Statutory Info</h4>
                                        <div class="form-group">
                                            <label>UAN Number</label><asp:TextBox ID="txt_uan" runat="server" CssClass="form-control" /></div>
                                        <div class="form-group">
                                            <label>ESIC Number</label><asp:TextBox ID="txt_esic" runat="server" CssClass="form-control" /></div>
                                    </div>
                                    <div class="col-md-8" style="border-left: 1px solid #ddd;">
                                        <h4 class="section-header">Bank Details (Salary)</h4>
                                        <div class="row">
                                            <div class="col-md-6 form-group">
                                                <label>Bank Name</label><asp:DropDownList ID="DDL_BankName" runat="server" CssClass="form-control" /></div>
                                            <div class="col-md-6 form-group">
                                                <label>Account Number</label><asp:TextBox ID="txt_acc_no" runat="server" CssClass="form-control" /></div>
                                            <div class="col-md-6 form-group">
                                                <label>IFSC Code</label><asp:TextBox ID="txt_ifsc" runat="server" CssClass="form-control" /></div>
                                            <div class="col-md-6 form-group">
                                                <label>Bank Branch</label><asp:TextBox ID="txt_bankbranch" runat="server" CssClass="form-control" /></div>
                                            <div class="col-md-6 form-group">
                                                <label>Last Bank Update</label><asp:TextBox ID="txt_BankUpd" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                            <div class="col-md-6 form-group">
                                                <label>Bank Updated By</label><asp:TextBox ID="txt_BankUpdBy" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="tab_compliance" class="tab-pane fade">
                                <h4 class="section-header">Gate & Safety Passes</h4>
                                <div class="row">
                                    <div class="col-md-3 form-group"><label>Gatepass No</label><asp:TextBox ID="txt_gp_no" runat="server" CssClass="form-control" /></div>
                                    <div class="col-md-3 form-group"><label>Gatepass Expiry</label><asp:TextBox ID="txt_gp_val" runat="server" CssClass="form-control" TextMode="Date" /></div>
                                    <div class="col-md-3 form-group"><label>Safety Pass No</label><asp:TextBox ID="txt_rfid" runat="server" CssClass="form-control" /></div>
                                    <div class="col-md-3 form-group"><label>Safety Validity</label><asp:TextBox ID="txt_rfid_val" runat="server" CssClass="form-control" TextMode="Date" /></div>
                                    <div class="col-md-3 form-group"><label>Police Verif. (PV) Expiry</label><asp:TextBox ID="txt_pv_val" runat="server" CssClass="form-control" TextMode="Date" /></div>
                                </div>
                                
                                <h4 class="section-header mt-3">Modification Tracking & Approval</h4>
                                <div class="row">
                                    <div class="col-md-3 form-group">
                                        <label>GP Last Modified By</label>
                                        <asp:TextBox ID="txt_GPModBy" runat="server" CssClass="form-control readonly-text" ReadOnly="true" />
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>GP Modified Date</label>
                                        <asp:TextBox ID="txt_GPModDt" runat="server" CssClass="form-control readonly-text" ReadOnly="true" />
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>GP Approval Status</label>
                                        <asp:TextBox ID="txt_GPAppr" runat="server" CssClass="form-control readonly-text" ReadOnly="true" Font-Bold="true" />
                                    </div>
                                    <div class="col-md-3 form-group" style="margin-top: 22px;">
                                        <asp:Button ID="btn_ApproveGP" runat="server" Text="Approve" CssClass="btn btn-success btn-sm" OnClick="btn_ApproveGP_Click" Visible="false" />
                                        <asp:Button ID="btn_RejectGP" runat="server" Text="Reject" CssClass="btn btn-danger btn-sm" OnClick="btn_RejectGP_Click" Visible="false" OnClientClick="return confirm('Are you sure you want to reject these Gate/Safety pass details?');" />
                                    </div>
                                </div>
                            </div>

                            <div id="tab_docs" class="tab-pane fade">
                                <h4 class="section-header">Document Verification</h4>
                                <div class="alert alert-info" style="padding: 10px;">
                                    <i class="fa fa-info-circle"></i><strong>Rejection Note:</strong> Enter a reason below before clicking 'Reject'. The employee will be forced to re-upload.
                                    <asp:TextBox ID="txt_DocAdminNote" runat="server" CssClass="form-control mt-1" placeholder="e.g. Image blurry, please scan original..." />
                                </div>
                                <table class="table table-striped table-hover table-condensed">
                                    <thead>
                                        <tr>
                                            <th>Document Type</th>
                                            <th>Doc ID / Details</th>
                                            <th>Status</th>
                                            <th>Document Date</th>
                                            <th>Actions</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>Aadhaar Card</td>
                                            <td>
                                                <asp:Label ID="lbl_AadhaarDet" runat="server" /></td>
                                            <td>
                                                <asp:Label ID="lbl_AadhaarStat" runat="server" /></td>
                                            <td>
                                                <asp:Label ID="lbl_AadhaarDate" runat="server" /></td>
                                            <td>
                                                <asp:HyperLink ID="lnk_ViewAadhaar" runat="server" Target="_blank" CssClass="btn btn-info btn-sm"><i class="fa fa-eye"></i> View</asp:HyperLink>
                                                <asp:Button ID="btn_ApproveAadhaar" runat="server" Text="Approve" CssClass="btn btn-success btn-sm" OnClick="btn_ApproveDoc_Click" CommandArgument="Aadhaar" />
                                                <asp:Button ID="btn_RejectAadhaar" runat="server" Text="Reject" CssClass="btn btn-danger btn-sm" OnClick="btn_RejectDoc_Click" CommandArgument="Aadhaar" OnClientClick="return confirm('Reject Aadhaar?');" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>PAN Card</td>
                                            <td>
                                                <asp:Label ID="lbl_PanDet" runat="server" /></td>
                                            <td>
                                                <asp:Label ID="lbl_PanStat" runat="server" /></td>
                                            <td>
                                                <asp:Label ID="lbl_PanDate" runat="server" /></td>
                                            <td>
                                                <asp:HyperLink ID="lnk_ViewPan" runat="server" Target="_blank" CssClass="btn btn-info btn-sm"><i class="fa fa-eye"></i> View</asp:HyperLink>
                                                <asp:Button ID="btn_ApprovePan" runat="server" Text="Approve" CssClass="btn btn-success btn-sm" OnClick="btn_ApproveDoc_Click" CommandArgument="Pan" />
                                                <asp:Button ID="btn_RejectPan" runat="server" Text="Reject" CssClass="btn btn-danger btn-sm" OnClick="btn_RejectDoc_Click" CommandArgument="Pan" OnClientClick="return confirm('Reject PAN?');" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Bank Proof</td>
                                            <td>
                                                <asp:Label ID="lbl_BankDet" runat="server" Text="--" /></td>
                                            <td>
                                                <asp:Label ID="lbl_BankStat" runat="server" /></td>
                                            <td>
                                                <asp:Label ID="lbl_BankDate" runat="server" /></td>
                                            <td>
                                                <asp:HyperLink ID="lnk_ViewBank" runat="server" Target="_blank" CssClass="btn btn-info btn-sm"><i class="fa fa-eye"></i> View</asp:HyperLink>
                                                <asp:Button ID="btn_ApproveBank" runat="server" Text="Approve" CssClass="btn btn-success btn-sm" OnClick="btn_ApproveDoc_Click" CommandArgument="Bank" />
                                                <asp:Button ID="btn_RejectBank" runat="server" Text="Reject" CssClass="btn btn-danger btn-sm" OnClick="btn_RejectDoc_Click" CommandArgument="Bank" OnClientClick="return confirm('Reject Bank Doc?');" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>10th / SSC</td>
                                            <td>
                                                <asp:Label ID="lbl_TenDet" runat="server" Text="--" /></td>
                                            <td>
                                                <asp:Label ID="lbl_TenStat" runat="server" /></td>
                                            <td>
                                                <asp:Label ID="lbl_TenDate" runat="server" /></td>
                                            <td>
                                                <asp:HyperLink ID="lnk_ViewTen" runat="server" Target="_blank" CssClass="btn btn-info btn-sm"><i class="fa fa-eye"></i> View</asp:HyperLink>
                                                <asp:Button ID="btn_ApproveTen" runat="server" Text="Approve" CssClass="btn btn-success btn-sm" OnClick="btn_ApproveDoc_Click" CommandArgument="Ten" />
                                                <asp:Button ID="btn_RejectTen" runat="server" Text="Reject" CssClass="btn btn-danger btn-sm" OnClick="btn_RejectDoc_Click" CommandArgument="Ten" OnClientClick="return confirm('Reject 10th Doc?');" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>12th / HSC</td>
                                            <td>
                                                <asp:Label ID="lbl_TwelveDet" runat="server" Text="--" /></td>
                                            <td>
                                                <asp:Label ID="lbl_TwelveStat" runat="server" /></td>
                                            <td>
                                                <asp:Label ID="lbl_TwelveDate" runat="server" /></td>
                                            <td>
                                                <asp:HyperLink ID="lnk_ViewTwelve" runat="server" Target="_blank" CssClass="btn btn-info btn-sm"><i class="fa fa-eye"></i> View</asp:HyperLink>
                                                <asp:Button ID="btn_ApproveTwelve" runat="server" Text="Approve" CssClass="btn btn-success btn-sm" OnClick="btn_ApproveDoc_Click" CommandArgument="Twelve" />
                                                <asp:Button ID="btn_RejectTwelve" runat="server" Text="Reject" CssClass="btn btn-danger btn-sm" OnClick="btn_RejectDoc_Click" CommandArgument="Twelve" OnClientClick="return confirm('Reject 12th Doc?');" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Graduation</td>
                                            <td>
                                                <asp:Label ID="lbl_GradDet" runat="server" Text="--" /></td>
                                            <td>
                                                <asp:Label ID="lbl_GradStat" runat="server" /></td>
                                            <td>
                                                <asp:Label ID="lbl_GradDate" runat="server" /></td>
                                            <td>
                                                <asp:HyperLink ID="lnk_ViewGrad" runat="server" Target="_blank" CssClass="btn btn-info btn-sm"><i class="fa fa-eye"></i> View</asp:HyperLink>
                                                <asp:Button ID="btn_ApproveGrad" runat="server" Text="Approve" CssClass="btn btn-success btn-sm" OnClick="btn_ApproveDoc_Click" CommandArgument="Grad" />
                                                <asp:Button ID="btn_RejectGrad" runat="server" Text="Reject" CssClass="btn btn-danger btn-sm" OnClick="btn_RejectDoc_Click" CommandArgument="Grad" OnClientClick="return confirm('Reject Graduation Doc?');" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>

                                <div class="row mt-2">
                                    <div class="col-md-3 form-group">
                                        <label>Doc Policy Accepted</label><asp:TextBox ID="txt_DocPolicy" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                    <div class="col-md-3 form-group">
                                        <label>Policy Accepted On</label><asp:TextBox ID="txt_DocPolicyDt" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                    <div class="col-md-3 form-group">
                                        <label>Upload Bypassed?</label><asp:TextBox ID="txt_DocBypass" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                    <div class="col-md-3 form-group">
                                        <label>Snoozed Until</label><asp:TextBox ID="txt_DocSkipDt" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                </div>
                            </div>

                            <div id="tab_admin" class="tab-pane fade">
                                <h4 class="section-header"><i class="fa fa-desktop"></i>System Access Log</h4>
                                <div class="row">
                                    <div class="col-md-3 form-group">
                                        <label>Online Status</label>
                                        <asp:Label ID="lbl_LoginStatus" runat="server" CssClass="form-control" Style="border: none; font-weight: bold;"></asp:Label>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Last Login Time</label>
                                        <asp:TextBox ID="txt_LastLogin" runat="server" CssClass="form-control readonly-text" ReadOnly="true" />
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Last Logout Time</label>
                                        <asp:TextBox ID="txt_LastLogout" runat="server" CssClass="form-control readonly-text" ReadOnly="true" />
                                    </div>
                                </div>

                                <h4 class="section-header mt-3"><i class="fa fa-key"></i>Credentials & Security</h4>
                                <div class="row">
                                    <div class="col-md-3">
                                        <label>Login ID (Read-Only)</label><asp:TextBox ID="txt_LoginID" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                    <div class="col-md-3">
                                        <label>Plain Password</label><asp:TextBox ID="txt_plain_pass" runat="server" Text="Not Available" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                    <div class="col-md-3">
                                        <label>Sec. Question 1</label><asp:TextBox ID="txt_sq1" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                    <div class="col-md-3">
                                        <label>Sec. Answer 1</label><asp:TextBox ID="txt_sqans1" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                </div>
                                <div class="row mt-2">
                                    <div class="col-md-3">
                                        <label>Sec. Question 2</label><asp:TextBox ID="txt_sq2" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                    <div class="col-md-3">
                                        <label>Sec. Answer 2</label><asp:TextBox ID="txt_sqans2" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                    <div class="col-md-3 form-group">
                                        <label>Password Expiry</label><asp:TextBox ID="txt_PassExpiry" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                    <div class="col-md-3 form-group">
                                        <label>Pass Last Updated</label><asp:TextBox ID="txt_PassUpdateDt" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                </div>
                                <div class="row mt-2">
                                    <div class="col-md-3 form-group">
                                        <label>Pass Updated By (Name)</label><asp:TextBox ID="txt_PassUpdByName" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                    <div class="col-md-3 form-group">
                                        <label>Pass Updated By (ID)</label><asp:TextBox ID="txt_PassUpdByWrk" runat="server" CssClass="form-control readonly-text" ReadOnly="true" /></div>
                                </div>

                                <h4 class="section-header mt-3"><i class="fa fa-shield"></i>Multi-Factor Authentication</h4>
                                <p class="small text-muted">When MFA is required, the user must enter an email OTP after a successful password. A registered Email on the Personal tab is mandatory.</p>
                                <div class="row">
                                    <div class="col-md-3 form-group">
                                        <label>MFA Required</label>
                                        <asp:DropDownList ID="DDL_MFAEnabled" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="No (password only)" Value="0" />
                                            <asp:ListItem Text="Yes (Email OTP)" Value="1" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>MFA Method</label>
                                        <asp:DropDownList ID="DDL_MFAMethod" runat="server" CssClass="form-control" Enabled="false">
                                            <asp:ListItem Text="Email OTP" Value="EmailOTP" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Enforced On</label>
                                        <asp:TextBox ID="txt_MFAEnforcedOn" runat="server" CssClass="form-control readonly-text" ReadOnly="true" />
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Enforced By</label>
                                        <asp:TextBox ID="txt_MFAEnforcedBy" runat="server" CssClass="form-control readonly-text" ReadOnly="true" />
                                    </div>
                                </div>
                                <div class="row mt-2">
                                    <div class="col-md-3 form-group">
                                        <label>Last MFA Verified</label>
                                        <asp:TextBox ID="txt_MFALastVerified" runat="server" CssClass="form-control readonly-text" ReadOnly="true" />
                                    </div>
                                </div>

                                <div class="admin-alert mt-4">
                                    <h4 class="text-danger section-header mb-2" style="background: transparent;"><i class="fa fa-exclamation-triangle"></i>Account & Exit Management</h4>
                                    <div class="row">
                                        <div class="col-md-3 form-group">
                                            <label>Current Status</label>
                                            <asp:Label ID="lbl_CurrentStatus" runat="server" CssClass="badge badge-info p-2 d-block" Text="Loading..."></asp:Label>
                                        </div>
                                        <div class="col-md-3 form-group">
                                            <label>Login Access Override</label>
                                            <asp:DropDownList ID="DDL_LoginAccess" runat="server" CssClass="form-control" Enabled="false">
                                                <asp:ListItem Text="User Online (1)" Value="1" />
                                                <asp:ListItem Text="User Offline (0)" Value="0" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-3 form-group">
                                            <label>Set WorkStatus</label>
                                            <asp:DropDownList ID="DDL_AdminStatus" runat="server" CssClass="form-control" ClientIDMode="Static" onchange="toggleStatusUI()">
                                                <asp:ListItem Text="Active" Value="Active" />
                                                <asp:ListItem Text="InActive" Value="InActive" />
                                            </asp:DropDownList>
                                        </div>

                                    </div>

                                    <div id="divReasonSelection" style="display: none; padding: 10px; background: #f9f9f9; border: 1px solid #ddd; border-radius: 4px; margin-top: 10px;">
                                        <div class="row">
                                            <div class="col-md-3 form-group">
                                                <label>Change Type <span class="text-danger">*</span></label>
                                                <asp:DropDownList ID="ddlChangeType" runat="server" CssClass="form-control" ClientIDMode="Static" onchange="toggleReasons()">
                                                    <asp:ListItem Text="-- Select Type --" Value="" />
                                                    <asp:ListItem Text="Temporary (Leave/Suspension)" Value="Temporary" />
                                                    <asp:ListItem Text="Permanent (Exit/Resignation)" Value="Permanent" />
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 form-group">
                                                <label>Specific Reason <span class="text-danger">*</span></label>
                                                <select id="ddlSpecificReason" runat="server" class="form-control" clientidmode="Static" onchange="saveReason()">
                                                    <option value="">-- Select Reason --</option>
                                                </select>
                                                <asp:HiddenField ID="hfSavedReason" runat="server" ClientIDMode="Static" />
                                            </div>
                                            <div class="col-md-6 form-group">
                                                <label>Status Remarks (Required for InActive) <span class="text-danger">*</span></label>
                                                <asp:TextBox ID="txt_StatusRemarks" runat="server" CssClass="form-control" placeholder="Provide reason for status change..." />
                                            </div>
                                        </div>

                                        <div id="divPermanentDates" style="display: none; background: #fff; padding: 10px; border: 1px dashed #ccc; margin-top: 10px;">
                                            <div class="row">
                                                <div class="col-md-4 form-group">
                                                    <label>Date of Resignation (DOR)</label><asp:TextBox ID="txt_DOR" runat="server" CssClass="form-control" TextMode="Date" ClientIDMode="Static" /></div>
                                                <div class="col-md-4 form-group">
                                                    <label>Date of Exit (DOE)</label><asp:TextBox ID="txt_DOE" runat="server" CssClass="form-control" TextMode="Date" ClientIDMode="Static" /></div>
                                                <div class="col-md-4 form-group">
                                                    <label>Date of Relief (DO_Relief)</label><asp:TextBox ID="txt_DORelief" runat="server" CssClass="form-control" TextMode="Date" ClientIDMode="Static" /></div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row mt-3">
                                        <div class="col-md-12">
                                            <label>Audit Log Entry <span class="text-danger">*</span></label>
                                            <asp:TextBox ID="txt_AdminReason" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Required reason for updating this Master record..." />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="tab_logs" class="tab-pane fade">
                                <h4 class="section-header"><i class="fa fa-history"></i> Audit & Change History</h4>
                                <div style="background-color: #f8f9fa; border: 1px solid #e9ecef; border-radius: 4px; padding: 15px; max-height: 600px; overflow-y: auto; font-family: Consolas, monospace; font-size: 13px; line-height: 1.6; color: #333;">
                                    <asp:Literal ID="lit_AuditLogs" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </div>

                        <div class="ln_solid"></div>
                        <div class="row">
                            <div class="col-md-6">
                                <asp:Label ID="lbl_msg" runat="server" Font-Bold="true"></asp:Label></div>
                            <div class="col-md-6 text-right">
                                <asp:Button ID="btn_Cancel" runat="server" Text="Back to List" CssClass="btn btn-info btn-sm" PostBackUrl="view_emp_mastertbldata_v2.aspx" CausesValidation="false" />
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
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button></div>
            </div>
        </div>
    </div>

    <div id="ImageModal" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-md" style="display: flex; justify-content: center;">
            <div class="modal-content" style="background: transparent; border: none; box-shadow: none;">
                <div class="modal-header" style="border: none; padding: 0; text-align: right;">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="color: white; opacity: 1; font-size: 30px; text-shadow: 0 2px 4px rgba(0,0,0,0.5);">&times;</button>
                </div>
                <div class="modal-body text-center" style="padding: 0;">
                    <img id="enlargedProfilePic" src="" alt="Profile Picture" style="max-width: 100%; max-height: 80vh; border-radius: 8px; border: 4px solid white; box-shadow: 0 8px 16px rgba(0,0,0,0.5);" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
