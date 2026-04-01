<%@ Page Title="ATS | Cloud ERP | Home" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="homepage_v2.aspx.cs" Inherits="WebApplication1.bussiness.production.homepage_v2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Core Card Styling */
        .modern-card {
            background: #ffffff;
            border-radius: 12px;
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.05);
            border: 1px solid rgba(0,0,0,0.08);
            margin-bottom: 25px;
            transition: all 0.3s ease;
            display: flex;
            flex-direction: column;
            height: 100%; /* Equal height for cards in the same row */
        }

            .modern-card:hover {
                box-shadow: 0 8px 25px rgba(0, 0, 0, 0.1);
                transform: translateY(-2px);
            }

        .modern-card-header {
            background: #fcfcfc;
            border-bottom: 1px solid #f0f0f0;
            padding: 12px 20px;
            border-radius: 12px 12px 0 0;
            font-weight: 700;
            color: #2c3e50;
            font-size: 15px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .modern-card-body {
            padding: 20px;
            flex-grow: 1;
        }

        .modern-card-footer {
            background: #fafafa;
            border-top: 1px solid #f0f0f0;
            padding: 12px 20px;
            border-radius: 0 0 12px 12px;
            text-align: right;
        }

        /* Lists & Typography */
        .text-theme {
            color: #2c3e50;
        }

        .info-list {
            list-style: none;
            padding: 0;
            margin: 0;
        }

            .info-list li {
                display: flex; /* Aligns icons perfectly with text */
                align-items: flex-start;
                padding: 8px 0;
                border-bottom: 1px dashed #eee;
                font-size: 14px;
                color: #444;
                text-align: left !important; /* Force left alignment */
            }

                .info-list li i {
                    margin-top: 3px;
                    color: #3498db;
                    width: 22px;
                    text-align: center;
                    margin-right: 10px;
                    font-size: 15px;
                }

                .info-list li:last-child {
                    border-bottom: none;
                }

        /* ID Card Specifics */
        .profile-avatar {
            border: 4px solid #f8f9fa;
            box-shadow: 0 4px 10px rgba(0,0,0,0.1);
            width: 130px;
            height: 130px;
            object-fit: cover;
            border-radius: 50%;
        }

        .qr-img {
            width: 110px;
            height: 110px;
            border-radius: 8px;
        }

        .data-label {
            font-size: 11px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            color: #7f8c8d;
            margin-bottom: 2px;
        }

        .data-value {
            font-size: 14px;
            font-weight: 600;
            color: #2c3e50;
        }

        .desktop-divider {
            border-left: 1px dashed #e0e0e0;
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
        }

        /* Badges (Attendance) */
        .stat-badge {
            display: inline-block;
            padding: 5px 10px;
            border-radius: 20px;
            font-size: 11px;
            font-weight: bold;
            margin: 3px 2px;
            color: #fff;
        }

        .bg-p {
            background-color: #2ecc71;
        }

        .bg-hd {
            background-color: #f1c40f;
            color: #333;
        }

        .bg-nhp {
            background-color: #3498db;
        }

        .bg-flp {
            background-color: #9b59b6;
        }

        .bg-od {
            background-color: #e67e22;
        }

        .bg-nh {
            background-color: #34495e;
        }

        .bg-fl {
            background-color: #e74c3c;
        }

        .bg-pending {
            background-color: #95a5a6;
        }

        .big-stat {
            font-size: 42px;
            font-weight: 800;
            line-height: 1;
        }

        .big-stat-label {
            font-size: 12px;
            text-transform: uppercase;
            letter-spacing: 1px;
            color: #7f8c8d;
            font-weight: 600;
        }

        /* Mobile Breakpoints */
        @media (max-width: 767px) {
            .modern-card {
                height: auto;
                margin-bottom: 15px;
            }

            .profile-avatar {
                width: 100px;
                height: 100px;
                margin-bottom: 10px;
            }

            /* Swap vertical divider for horizontal on mobile */
            .desktop-divider {
                border-left: none;
                border-top: 1px dashed #e0e0e0;
                padding-top: 15px;
                margin-top: 15px;
                padding-bottom: 10px; /* Prevents text from hitting bottom edge */
            }

            .mobile-center {
                text-align: center;
            }

            .mobile-data-box {
                text-align: left;
            }
        }

        .qr-img {
            width: 110px;
            height: 110px;
            border-radius: 8px;
            cursor: pointer; /* Makes it look clickable */
            transition: transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out;
        }

            .qr-img:hover {
                transform: scale(1.05); /* Slight zoom on hover */
                box-shadow: 0 6px 15px rgba(0,0,0,0.15) !important;
            }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="row" EnableViewState="false">

            <div class="col-md-12 col-sm-12">
                <div class="modern-card">
                    <div class="modern-card-header">
                        <span><i class="fa fa-id-badge text-primary"></i>Digital ID Card :
                            <asp:Label ID="lbl_id" runat="server" Text="N/A" ForeColor="#3498db"></asp:Label></span>
                        <asp:Button ID="btn_mngprofile" runat="server" Text="Manage Profile" CausesValidation="false" class="btn btn-outline-primary btn-sm m-0" PostBackUrl="~/bussiness/production/manage_profile.aspx" />
                    </div>
                    <div class="modern-card-body">
                        <div class="row align-items-stretch">

                            <div class="col-md-3 col-sm-12 text-center mobile-center align-self-center">
                                <img id="ProfilePic_3" runat="server" src="...." alt="ProfilePhoto" class="profile-avatar">
                                <h2 class="text-theme mb-0 mt-2" style="font-weight: 700; font-size: 18px;">
                                    <asp:Label ID="lbl_workmansl" runat="server" Text="N/A" ForeColor="#3498db"></asp:Label>
                                    - 
                                    <asp:Label ID="lbl_username" runat="server" Text="N/A"></asp:Label>
                                </h2>
                                <h5 class="text-muted mb-0" style="font-size: 14px;">
                                    <asp:Label ID="lbl_desg" runat="server" Text=""></asp:Label>
                                    <asp:Label ID="lbl_skillcat" runat="server" Text="" Visible="false"></asp:Label>
                                </h5>
                            </div>

                            <div class="col-md-6 col-sm-12 mt-3 mt-md-0 align-self-center mobile-data-box">
                                <div class="row" style="background: #f8f9fa; border-radius: 8px; padding: 15px 10px; margin: 0; border: 1px solid #f0f0f0;">
                                    <div class="col-6 col-md-6 mb-3">
                                        <div class="data-label"><i class="fa fa-building"></i>Company</div>
                                        <div class="data-value">
                                            <asp:Label ID="lbl_wrkcopmany" runat="server"></asp:Label></div>
                                    </div>
                                    <div class="col-6 col-md-6 mb-3">
                                        <div class="data-label"><i class="fa fa-map-marker"></i>Location</div>
                                        <div class="data-value">
                                            <asp:Label ID="lbl_region" runat="server"></asp:Label>,
                                            <asp:Label ID="lbl_state" runat="server" Text="N/A"></asp:Label></div>
                                    </div>
                                    <div class="col-6 col-md-6 mb-2 mb-md-0">
                                        <div class="data-label"><i class="fa fa-industry"></i>Site</div>
                                        <div class="data-value">
                                            <asp:Label ID="lbl_wrksite" runat="server"></asp:Label></div>
                                    </div>
                                    <div class="col-6 col-md-6 mb-2 mb-md-0">
                                        <div class="data-label"><i class="fa fa-calendar"></i>Date of Joining</div>
                                        <div class="data-value">
                                            <asp:Label ID="lbl_doj" runat="server" Text="N/A"></asp:Label></div>
                                    </div>
                                </div>
                                <div class="mt-2 px-2">
                                    <span class="text-muted small"><i class="fa fa-clock-o"></i>Work Tenure:</span>
                                    <strong class="text-dark" style="font-size: 13px;">
                                        <asp:Label ID="lbl_workage" runat="server"></asp:Label></strong>
                                </div>
                            </div>

                            <div class="col-md-3 col-sm-12 desktop-divider">
                                <img id="img_qrcode" runat="server" src="" alt="QR Code" class="img-thumbnail shadow-sm mb-1 qr-img" onclick="showEnlargedQR(this.src);" title="Click to enlarge" />
                                <div class="small font-weight-bold text-muted" style="font-size: 11px;"><i class="fa fa-qrcode"></i>Scan to Verify</div>
                                <div class="text-primary mt-1" style="font-size: 10px; cursor: pointer;" onclick="showEnlargedQR(document.getElementById('<%= img_qrcode.ClientID %>').src);"><i class="fa fa-search-plus"></i>Click to Enlarge</div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-6 col-sm-12">
                <div class="modern-card">
                    <div class="modern-card-header">
                        <span><i class="fa fa-calendar-check-o text-success"></i>Attendance:
                            <asp:Label ID="lbl_calmonth" runat="server" Text="0"></asp:Label>
                            <asp:Label ID="lbl_calyear" runat="server" Text="0"></asp:Label></span>
                    </div>
                    <div class="modern-card-body d-flex flex-column justify-content-center">
                        <div class="row text-center mb-4">
                            <div class="col-6 border-right">
                                <div class="big-stat text-success">
                                    <asp:Label ID="lbl_totalpresent" runat="server" Text="0"></asp:Label>
                                    <span style="font-size: 20px; color: #bdc3c7;">/
                                        <asp:Label ID="lbl_caldays" runat="server" Text="0"></asp:Label></span>
                                </div>
                                <div class="big-stat-label">Days Approved</div>
                            </div>
                            <div class="col-6">
                                <div class="big-stat text-info">
                                    <asp:Label ID="lbl_dayswrkd" runat="server" Text="0"></asp:Label>
                                </div>
                                <div class="big-stat-label">Actual Days</div>
                            </div>
                        </div>

                        <div class="text-center mb-3">
                            <span class="stat-badge bg-p" title="Present">P:
                                <asp:Label ID="lbl_presentdayscount" runat="server" Text="0"></asp:Label></span>
                            <span class="stat-badge bg-hd" title="Half Day">HD:
                                <asp:Label ID="lbl_halfdaycount" runat="server" Text="0"></asp:Label></span>
                            <span class="stat-badge bg-nhp" title="National Holiday Present">NHP:
                                <asp:Label ID="lbl_nhpcount" runat="server" Text="0"></asp:Label></span>
                            <span class="stat-badge bg-flp" title="Festival Leave Present">FLP:
                                <asp:Label ID="lbl_flpcount" runat="server" Text="0"></asp:Label></span>
                            <span class="stat-badge bg-od" title="On Duty">OD:
                                <asp:Label ID="lbl_oddayscount" runat="server" Text="0"></asp:Label></span>
                            <span class="stat-badge bg-nh" title="National Holiday">NH:
                                <asp:Label ID="lbl_nhcount" runat="server" Text="0"></asp:Label></span>
                            <span class="stat-badge bg-fl" title="Festival Leave">FL:
                                <asp:Label ID="lbl_flcount" runat="server" Text="0"></asp:Label></span>
                            <span class="stat-badge bg-pending" title="Pending Approval">Pending:
                                <asp:Label ID="lbl_pendingcount" runat="server" Text="0"></asp:Label></span>
                        </div>

                        <div class="text-center p-2 rounded" style="background: #fffcf5; border: 1px solid #f8e1b5;">
                            <span class="text-muted"><i class="fa fa-clock-o"></i>Total Overtime:</span>
                            <strong style="color: #e67e22; font-size: 16px;">
                                <asp:Label ID="lbl_totalot" runat="server" Text="32"></asp:Label>
                                Hours</strong>
                        </div>
                    </div>
                    <div class="modern-card-footer d-flex justify-content-between align-items-center">
                        <small class="text-danger"><i>**Only Approved Count</i></small>
                        <asp:Button ID="btn_vwmntlyattn" runat="server" class="btn btn-success btn-sm m-0" CausesValidation="false" Text="View Details" OnClick="btn_vwmntlyattn_Click" />
                    </div>
                </div>
            </div>

            <div class="col-md-6 col-sm-12">
                <div class="modern-card">
                    <div class="modern-card-header">
                        <span><i class="fa fa-ticket text-info"></i>Gatepass & Clearances</span>
                        <span class="badge badge-secondary" style="font-size: 13px;">
                            <asp:Label ID="lbl_gpno" runat="server" Text="0"></asp:Label></span>
                    </div>
                    <div class="modern-card-body d-flex align-items-center">
                        <div class="row w-100 align-items-center">
                            <div class="col-sm-5 text-center mb-3 mb-sm-0">
                                <div class="big-stat text-success">
                                    <asp:Label ID="lbl_gpexpdays" runat="server" Text="0"></asp:Label>
                                </div>
                                <div class="big-stat-label">Days Left</div>
                            </div>
                            <div class="col-sm-7">
                                <ul class="info-list">
                                    <li><i class="fa fa-calendar-times-o"></i>
                                        <div><strong>GP Expiry:</strong>
                                            <asp:Label ID="lbl_gpvalidity" runat="server" Text="0" ForeColor="#2ecc71" Font-Bold="true"></asp:Label></div>
                                    </li>
                                    <li>
                                        <i class="fa fa-shield"></i>
                                        <div>
                                            <strong>Safety No:</strong>
                                            <asp:Label ID="lbl_rfidno" runat="server" Text="32" ForeColor="#2ecc71" Font-Bold="true"></asp:Label><br />
                                            <span class="text-muted" style="font-size: 12px;">Exp:
                                                <asp:Label ID="lbl_rfidvalidity" runat="server" Text="0" Font-Bold="true"></asp:Label>
                                                (<asp:Label ID="lbl_rfiddays" runat="server" Text="0"></asp:Label>
                                                Days)</span>
                                        </div>
                                    </li>
                                    <li>
                                        <i class="fa fa-check-circle"></i>
                                        <div>
                                            <strong>PV Expiry:</strong>
                                            <asp:Label ID="lbl_pvvalidity" runat="server" Text="0" ForeColor="#2ecc71" Font-Bold="true"></asp:Label><br />
                                            <span class="text-muted" style="font-size: 12px;">(<asp:Label ID="lbl_pvdays" runat="server" Text="0"></asp:Label>
                                                Days Left)</span>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="modern-card-footer">
                        <button type="button" class="btn btn-primary btn-sm m-0" id="btnShowPopup2" data-toggle="modal" data-target="#myModal2">
                            <i class="fa fa-refresh"></i>Update Passes
                        </button>
                    </div>
                </div>
            </div>

            <div class="col-md-6 col-sm-12">
                <div class="modern-card">
                    <div class="modern-card-header">
                        <span><i class="fa fa-bank text-secondary"></i>Salary Bank Account</span>
                    </div>
                    <div class="modern-card-body d-flex align-items-center">
                        <div class="row w-100 align-items-center">
                            <div class="col-sm-4 text-center mb-3 mb-sm-0">
                                <img src="../../erp_images/bank.jpg" alt="Bank" class="img-fluid rounded border p-2" style="max-width: 90px; background: #f8f9fa;">
                                <h5 class="mt-2 text-theme mb-0" style="font-weight: bold; font-size: 15px;">
                                    <asp:Label ID="lbl_bankname" runat="server" Text="0"></asp:Label></h5>
                            </div>
                            <div class="col-sm-8">
                                <ul class="info-list">
                                    <li><i class="fa fa-credit-card"></i>
                                        <div><strong>Account No:</strong>
                                            <asp:Label ID="lbl_accno" runat="server" Text="0" ForeColor="#2980b9" Font-Bold="true"></asp:Label></div>
                                    </li>
                                    <li><i class="fa fa-code"></i>
                                        <div><strong>IFSC Code:</strong>
                                            <asp:Label ID="lbl_ifsc" runat="server" Text="0" ForeColor="#3498db" Font-Bold="true"></asp:Label></div>
                                    </li>
                                    <li><i class="fa fa-map-marker"></i>
                                        <div><strong>Branch:</strong>
                                            <asp:Label ID="lbl_branch" runat="server" Text="N/A" Font-Bold="true"></asp:Label></div>
                                    </li>
                                </ul>
                                <div class="mt-3 text-muted" style="font-size: 12px; background: #f9f9f9; padding: 8px; border-radius: 4px; border: 1px solid #eee;">
                                    <i class="fa fa-info-circle"></i>
                                    <asp:Label ID="lbl_bankupdtinfo" runat="server" Text="N/A"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modern-card-footer">
                        <button type="button" class="btn btn-primary btn-sm m-0" id="btnShowPopup" data-toggle="modal" data-target="#myModal">
                            <i class="fa fa-pencil"></i>Update Details
                        </button>
                    </div>
                </div>
            </div>

            <div class="col-md-6 col-sm-12">
                <div class="modern-card">
                    <div class="modern-card-header">
                        <span><i class="fa fa-heartbeat text-danger"></i>Employee Benefits (Last Month)</span>
                    </div>
                    <div class="modern-card-body d-flex align-items-center">
                        <div class="row w-100 align-items-center">
                            <div class="col-sm-4 text-center mb-3 mb-sm-0">
                                <img src="../../erp_images/piggy.png" alt="Benefits" class="img-fluid" style="max-width: 100px; opacity: 0.9;">
                            </div>
                            <div class="col-sm-8">
                                <div class="p-3 mb-3 rounded shadow-sm" style="background: #f1f8ff; border-left: 4px solid #3498db;">
                                    <h6 style="font-weight: 700; color: #2c3e50; margin-top: 0; margin-bottom: 8px;"><i class="fa fa-shield text-primary"></i>EPFO Contribution</h6>
                                    <div style="font-size: 13px;"><strong>EPFO No:</strong>
                                        <asp:Label ID="lbl_pfno" runat="server" Text="0" ForeColor="#3498db" Font-Bold="true"></asp:Label></div>
                                    <div style="font-size: 13px;"><strong>Last Cont.:</strong> ₹
                                        <asp:Label ID="lbl_lastpfpay" runat="server" Text="0.00" ForeColor="#2c3e50" Font-Bold="true"></asp:Label></div>
                                </div>
                                <div class="p-3 rounded shadow-sm" style="background: #fff5f5; border-left: 4px solid #e74c3c;">
                                    <h6 style="font-weight: 700; color: #2c3e50; margin-top: 0; margin-bottom: 8px;"><i class="fa fa-plus-square text-danger"></i>ESIC Contribution</h6>
                                    <div style="font-size: 13px;"><strong>ESIC No:</strong>
                                        <asp:Label ID="lbl_esicno" runat="server" Text="N/A" ForeColor="#e74c3c" Font-Bold="true"></asp:Label></div>
                                    <div style="font-size: 13px;"><strong>Last Cont.:</strong> ₹
                                        <asp:Label ID="lbl_lastesicpay" runat="server" Text="0.00" ForeColor="#2c3e50" Font-Bold="true"></asp:Label></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modern-card-footer">
                        <button type="button" class="btn btn-outline-secondary btn-sm m-0">
                            <i class="fa fa-eye"></i>View History
                        </button>
                    </div>
                </div>
            </div>

        </div>
        <div class="clearfix"></div>
        <br />

        <asp:Button ID="ShowPopup" runat="server" Text="Button" class="btn btn-primary" Visible="false" data-toggle="modal" data-target=".bs-example-modal-sm" />
        <div id="MyPopup" class="modal fade bs-example-modal-sm" data-backdrop="static" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog modal-sm modal-dialog-centered">
                <div class="modal-content border-0">
                    <div class="modal-header bg-info text-white">
                        <h5 class="modal-title m-0" id="myModalLabel2"></h5>
                        <button type="button" class="close text-white" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">×</span>
                        </button>
                    </div>
                    <div class="modal-body text-center p-4">
                    </div>
                    <div class="modal-footer bg-light">
                        <button type="button" class="btn btn-secondary btn-sm" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="myModal" data-backdrop="static">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content border-0">
                    <div class="modal-header bg-primary text-white">
                        <h5 class="modal-title m-0"><i class="fa fa-bank"></i>Update Salary Account Details</h5>
                        <button type="button" class="close text-white" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body p-4">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group row align-items-center mb-2">
                                    <div class="col-sm-5">
                                        <label class="m-0">Enter Bank Name :</label></div>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txt_bankname" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV1" runat="server" ValidationGroup="BANK" Display="Dynamic" ForeColor="Red" ControlToValidate="txt_bankname" ErrorMessage="**"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group row align-items-center mb-2">
                                    <div class="col-sm-5">
                                        <label class="m-0">Enter Account Number :</label></div>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txt_accno" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV2" runat="server" ValidationGroup="BANK" Display="Dynamic" ForeColor="Red" ErrorMessage="**" ControlToValidate="txt_accno"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group row align-items-center mb-2" id="dup_acnorow1" runat="server" visible="false">
                                    <div class="col-sm-5">
                                        <label class="m-0">Re-Enter Account Number :</label></div>
                                    <div class="col-sm-7" id="dup_acnorow2" runat="server">
                                        <asp:TextBox ID="txt_cnfaccno" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV3" runat="server" ValidationGroup="BANK" ErrorMessage="**" ControlToValidate="txt_cnfaccno" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="CV1" runat="server" ValidationGroup="BANK" Display="Dynamic" ForeColor="Red" ErrorMessage="Enter same Account Number" ControlToCompare="txt_accno" ControlToValidate="txt_cnfaccno"></asp:CompareValidator>
                                    </div>
                                </div>
                                <div class="form-group row align-items-center mb-2">
                                    <div class="col-sm-5">
                                        <label class="m-0">Enter IFSC Code :</label></div>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txt_ifsc" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV4" runat="server" ValidationGroup="BANK" Display="Dynamic" ForeColor="Red" ErrorMessage="**" ControlToValidate="txt_ifsc"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group row align-items-center mb-0">
                                    <div class="col-sm-5">
                                        <label class="m-0">Enter Branch Name :</label></div>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txt_branchname" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="text-center mt-2">
                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="modal-footer bg-light">
                        <button type="button" class="btn btn-outline-danger btn-sm" data-dismiss="modal">Close</button>
                        <asp:Button ID="btn_bankedit" runat="server" ValidationGroup="BANK" Enabled="false" CausesValidation="true" Text="Make Changes" CssClass="btn btn-info btn-sm" OnClick="btn_bankedit_Click" />
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="myModal2" data-backdrop="static">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content border-0">
                    <div class="modal-header bg-primary text-white">
                        <h5 class="modal-title m-0"><i class="fa fa-ticket"></i>Update Gatepass Details</h5>
                        <button type="button" class="close text-white" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body p-4">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group row align-items-center mb-2">
                                    <div class="col-sm-5">
                                        <label class="m-0">Gatepass No :</label></div>
                                    <div class="col-sm-7">
                                        <asp:Label ID="lbl_oldgpno" runat="server" Text="N/A" CssClass="form-control form-control-sm rounded bg-light border-0"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group row align-items-center mb-2" id="nwgprow1" runat="server" visible="false">
                                    <div class="col-sm-5">
                                        <label class="m-0 text-primary font-weight-bold">New Gatepass No :</label></div>
                                    <div class="col-sm-7" id="nwgprow2" runat="server">
                                        <asp:TextBox ID="txt_nwgpno" runat="server" CssClass="form-control form-control-sm rounded border-primary"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="GPDATA" ControlToValidate="txt_nwgpno" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="form-group row align-items-center mb-2">
                                    <div class="col-sm-5">
                                        <label class="m-0">Gatepass Validity :</label></div>
                                    <div class="col-sm-7">
                                        <asp:Label ID="lbl_oldgpvalidity" runat="server" Text="N/A" CssClass="form-control form-control-sm rounded bg-light border-0"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group row align-items-center mb-2" id="nwgpvalrow1" runat="server" visible="false">
                                    <div class="col-sm-5">
                                        <label class="m-0 text-primary font-weight-bold">New GP Validity :</label></div>
                                    <div class="col-sm-7" id="nwgpvalrow2" runat="server">
                                        <asp:TextBox ID="txt_nwgpvalidity" runat="server" CssClass="form-control form-control-sm rounded border-primary date" type="date" name="date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="GPDATA" ControlToValidate="txt_nwgpvalidity" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="form-group row align-items-center mb-2">
                                    <div class="col-sm-5">
                                        <label class="m-0">Safety No :</label></div>
                                    <div class="col-sm-7">
                                        <asp:Label ID="lbl_oldsftyno" runat="server" Text="N/A" CssClass="form-control form-control-sm rounded bg-light border-0"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group row align-items-center mb-2" id="nwsftyrow1" runat="server" visible="false">
                                    <div class="col-sm-5">
                                        <label class="m-0 text-primary font-weight-bold">New Safety No :</label></div>
                                    <div class="col-sm-7" id="nwsftyrow2" runat="server">
                                        <asp:TextBox ID="txt_nwsftyno" runat="server" CssClass="form-control form-control-sm rounded border-primary" ReadOnly="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="GPDATA" ControlToValidate="txt_nwsftyno" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="" Enabled="false"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="form-group row align-items-center mb-2">
                                    <div class="col-sm-5">
                                        <label class="m-0">Safety Validity :</label></div>
                                    <div class="col-sm-7">
                                        <asp:Label ID="lbl_oldsftyval" runat="server" Text="N/A" CssClass="form-control form-control-sm rounded bg-light border-0"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group row align-items-center mb-2" id="nwrfidrow1" runat="server" visible="false">
                                    <div class="col-sm-5">
                                        <label class="m-0 text-primary font-weight-bold">New Safety Validity :</label></div>
                                    <div class="col-sm-7" id="nwrfidrow2" runat="server">
                                        <asp:TextBox ID="txt_nwsftyvalidity" runat="server" CssClass="form-control form-control-sm rounded border-primary date" type="date" name="date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="GPDATA" ControlToValidate="txt_nwsftyvalidity" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="form-group row align-items-center mb-2">
                                    <div class="col-sm-5">
                                        <label class="m-0">PV Validity :</label></div>
                                    <div class="col-sm-7">
                                        <asp:Label ID="lbl_oldpvvalidity" runat="server" Text="N/A" CssClass="form-control form-control-sm rounded bg-light border-0"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group row align-items-center mb-0" id="nwpvrow1" runat="server" visible="false">
                                    <div class="col-sm-5">
                                        <label class="m-0 text-primary font-weight-bold">New PV Validity :</label></div>
                                    <div class="col-sm-7" id="nwpvrow2" runat="server">
                                        <asp:TextBox ID="txt_nwpvvalidity" runat="server" CssClass="form-control form-control-sm rounded border-primary date" type="date" name="date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="GPDATA" ControlToValidate="txt_nwpvvalidity" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <asp:Label ID="Label1" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="modal-footer bg-light">
                        <button type="button" class="btn btn-outline-danger btn-sm" data-dismiss="modal">Close</button>
                        <asp:Button ID="btn_cancel" runat="server" CausesValidation="false" Text="Cancel" CssClass="btn btn-warning btn-sm" OnClick="btn_cancel_Click" />
                        <asp:Button ID="btn_gtpsedit" runat="server" CausesValidation="true" ValidationGroup="GPDATA" Text="Make Changes" CssClass="btn btn-info btn-sm" OnClick="btn_gtpsedit_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-pass-modal-lg" id="myModal3" data-backdrop="static">
            <div class="modal-dialog modal-lg modal-dialog-centered">
                <div class="modal-content border-0">
                    <div class="modal-header bg-warning text-dark">
                        <h5 class="modal-title m-0"><i class="fa fa-key"></i>Change Login Credentials</h5>
                        <button type="button" class="close text-dark" data-dismiss="modal" disabled="disabled">&times;</button>
                    </div>
                    <div class="modal-body p-4">
                        <div class="container-fluid p-0">
                            <div class="row">
                                <div class="col-md-6 form-group mb-3">
                                    <label>Login ID:</label>
                                    <asp:TextBox ID="txt_atsloginid" runat="server" class="form-control form-control-sm rounded font-weight-bold text-primary" Text="N/A" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-6 form-group mb-3">
                                    <label>Workmen Sl:</label>
                                    <asp:TextBox ID="txt_atsworkmenno" runat="server" class="form-control form-control-sm rounded font-weight-bold text-primary" Text="N/A" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="col-md-8 form-group mb-3">
                                    <label>OLD Password: <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txt_oldpass" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="false"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="OLDPASS" ForeColor="IndianRed" ErrorMessage="OLD Password is required" InitialValue="" Display="Dynamic" ControlToValidate="txt_oldpass" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4 form-group mb-3 d-flex align-items-end">
                                    <asp:Button ID="btn_validateoldpassword" runat="server" Text="Verify Old Password" ValidationGroup="OLDPASS" CausesValidation="true" CssClass="btn btn-sm btn-success w-100 m-0" OnClick="btn_validateoldpassword_Click" />
                                </div>

                                <div class="col-md-6 form-group mb-3" id="newpwd_row1" runat="server" visible="false">
                                    <label>New Password: <span class="text-danger">*</span></label>
                                    <div id="newpwd_row2" runat="server">
                                        <asp:TextBox ID="txt_newpass1" runat="server" class="form-control form-control-sm rounded" Text="" ReadOnly="true" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_P1" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txt_newpass1" InitialValue="" ValidationGroup="ChnagePassword" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="REV_P1" runat="server" ErrorMessage="AlphaNumeric, Min 8 chars, 1 Uppercase, 1 Special Char" ForeColor="Red" ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$" ControlToValidate="txt_newpass1" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div class="col-md-6 form-group mb-3" id="newpwd_row3" runat="server" visible="false">
                                    <label>Confirm New Password: <span class="text-danger">*</span></label>
                                    <div id="newpwd_row4" runat="server">
                                        <asp:TextBox ID="txt_newpass2" runat="server" class="form-control form-control-sm rounded" Text="" ReadOnly="true" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_P2" runat="server" ErrorMessage="*" ForeColor="Red" InitialValue="" ControlToValidate="txt_newpass2" ValidationGroup="ChnagePassword" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="CV_P1" runat="server" ErrorMessage="Passwords must match" ControlToCompare="txt_newpass1" ForeColor="Red" ControlToValidate="txt_newpass2" Display="Dynamic"></asp:CompareValidator>
                                    </div>
                                </div>

                                <div class="col-md-6 form-group mb-3" id="newpwd_row5" runat="server" visible="false">
                                    <label>Security Question 1: <span class="text-danger">*</span></label>
                                    <div id="newpwd_row6" runat="server">
                                        <asp:DropDownList ID="DDL_SQ1" runat="server" CssClass="form-control form-control-sm rounded mb-2"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFV_P3" runat="server" ErrorMessage="Selection Required" ValidationGroup="ChnagePassword" ControlToValidate="DDL_SQ1" ForeColor="Red" InitialValue="Please Select Option" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="col-md-6 form-group mb-3" id="newpwd_row7" runat="server" visible="false">
                                    <label>Answer to Q1: <span class="text-danger">*</span></label>
                                    <div id="newpwd_row8" runat="server">
                                        <asp:TextBox ID="txt_SQAns1" class="form-control form-control-sm rounded" placeholder="Answer to Q1" ReadOnly="true" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_P4" runat="server" ErrorMessage="Input Required" ControlToValidate="txt_SQAns1" ValidationGroup="ChnagePassword" InitialValue="" SetFocusOnError="true" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="col-md-6 form-group mb-3" id="newpwd_row9" runat="server" visible="false">
                                    <label>Security Question 2: <span class="text-danger">*</span></label>
                                    <div id="newpwd_row10" runat="server">
                                        <asp:DropDownList ID="DDL_SQ2" runat="server" CssClass="form-control form-control-sm rounded mb-2"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFV_P5" runat="server" ErrorMessage="Selection Required" ControlToValidate="DDL_SQ2" ForeColor="Red" Display="Dynamic" InitialValue="Please Select Option" SetFocusOnError="true" ValidationGroup="ChnagePassword"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="col-md-6 form-group mb-3" id="newpwd_row11" runat="server" visible="false">
                                    <label>Answer to Q2: <span class="text-danger">*</span></label>
                                    <div id="newpwd_row12" runat="server">
                                        <asp:TextBox ID="txt_SQAns2" class="form-control form-control-sm rounded" placeholder="Answer to Q2" ReadOnly="true" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_P6" runat="server" ErrorMessage="Input Required" ControlToValidate="txt_SQAns2" ValidationGroup="ChnagePassword" InitialValue="" SetFocusOnError="true" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="col-md-8 text-center mx-auto mt-2">
                                    <asp:Label ID="lbl_msgpass" runat="server" Text="Your login credentials have expired, Please change to continue" Font-Bold="true" ForeColor="#e74c3c"></asp:Label>
                                </div>
                                <div class="col-md-12 text-center mt-3">
                                    <asp:Button ID="btn_relogin" CausesValidation="false" runat="server" Text="Re-Login" Enabled="false" CssClass="btn btn-primary px-4" OnClick="btn_relogin_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer bg-light">
                        <asp:Button ID="btn_closecvpass" runat="server" Text="SKIP NOW" Visible="true" CausesValidation="false" class="btn btn-outline-danger btn-sm" data-dismiss="modal" Enabled="true" />
                        <asp:Button ID="btn_discardsvpass" runat="server" Text="Discard Changes" CausesValidation="false" CssClass="btn btn-warning btn-sm" OnClick="btn_discardsvpass_Click" />
                        <asp:Button ID="btn_svpass" runat="server" CssClass="btn btn-success btn-sm" Text="Save Changes" Enabled="false" OnClick="btn_svpass_Click" CausesValidation="true" ValidationGroup="ChnagePassword" />
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="myModal4" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content border-0">
                    <div class="modal-header bg-info text-white">
                        <h5 class="modal-title m-0"><i class="fa fa-phone-square"></i>Contact Verification Required</h5>
                    </div>
                    <div class="modal-body p-4">
                        <div class="alert alert-warning" style="border-left: 4px solid #e67e22; background: #fffcf5;">
                            <h6 class="alert-heading font-weight-bold m-0 mb-2">
                                <i class="fa fa-exclamation-circle text-warning"></i>Why are you seeing this?
                            </h6>
                            <p class="mb-2 text-dark" style="font-size: 13px;">To ensure workplace safety and seamless communication, our policy requires all employees to re-verify their contact details <strong>every 90 days</strong>.</p>
                            <hr class="my-2" style="border-color: #f8e1b5;">
                            <p class="m-0 text-dark" style="font-size: 13px;">
                                <strong>What to do:</strong><br />
                                1. Review your info below.<br />
                                2. If correct, click <b>Confirm</b>.<br />
                                3. If incorrect, click <b>Make Changes</b>.
                            </p>
                        </div>

                        <div class="text-center mb-3">
                            <p id="p_verify_reason" runat="server" class="text-danger font-weight-bold m-0">Your last verification has expired.</p>
                        </div>

                        <div class="row align-items-center mb-2">
                            <div class="col-sm-5 text-right">
                                <label class="m-0 text-muted">Current Mobile No:</label></div>
                            <div class="col-sm-7">
                                <asp:Label ID="lbl_oldmobileno" runat="server" Text="N/A" CssClass="font-weight-bold text-dark"></asp:Label></div>
                        </div>
                        <div class="row align-items-center mb-3" id="InputMob1" runat="server" visible="false">
                            <div class="col-sm-5 text-right">
                                <label class="m-0 text-primary font-weight-bold">New Mobile No:</label></div>
                            <div class="col-sm-7" id="InputMob2" runat="server">
                                <asp:TextBox ID="txt_nwmobileno" runat="server" CssClass="form-control form-control-sm border-primary" placeholder="10 digit number"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="MobileFieldValidator" runat="server" ValidationGroup="ContData" ErrorMessage="Required" ControlToValidate="txt_nwmobileno" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="MobileValidator" runat="server" ControlToValidate="txt_nwmobileno" ErrorMessage="10 Digits required" ValidationExpression="^[0-9]{10}$" ForeColor="Red" Display="Dynamic" ValidationGroup="ContData"></asp:RegularExpressionValidator>
                            </div>
                        </div>

                        <div class="row align-items-center mb-2">
                            <div class="col-sm-5 text-right">
                                <label class="m-0 text-muted">Current Email:</label></div>
                            <div class="col-sm-7">
                                <asp:Label ID="lbl_oldemailadd" runat="server" Text="N/A" CssClass="font-weight-bold text-dark"></asp:Label></div>
                        </div>
                        <div class="row align-items-center mb-3" id="InputEmail1" runat="server" visible="false">
                            <div class="col-sm-5 text-right">
                                <label class="m-0 text-primary font-weight-bold">New Email:</label></div>
                            <div class="col-sm-7" id="InputEmail2" runat="server">
                                <asp:TextBox ID="txt_nwemailadd" runat="server" CssClass="form-control form-control-sm border-primary" AutoCompleteType="Disabled" placeholder="name@domain.com"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="EmailFieldValidator" runat="server" ControlToValidate="txt_nwemailadd" ErrorMessage="Required" Display="Dynamic" ForeColor="Red" ValidationGroup="ContData"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="EmailValidator" runat="server" ControlToValidate="txt_nwemailadd" ErrorMessage="Invalid Format" ValidationExpression="\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b" ForeColor="Red" Display="Dynamic" ValidationGroup="ContData"></asp:RegularExpressionValidator>
                            </div>
                        </div>

                        <div class="row mb-2" id="OTP_1" runat="server" visible="false">
                            <div class="col-md-12 text-center">
                                <asp:Button ID="btn_SendOTP" runat="server" Text="Send OTP" CausesValidation="true" ValidationGroup="ContData" CssClass="btn btn-info btn-sm px-4" OnClick="btn_SendOTP_Click" />
                            </div>
                        </div>
                        <div class="row mb-2" id="OTP_2" runat="server" visible="false">
                            <div class="col-md-8 mx-auto">
                                <asp:TextBox ID="TextBoxEnteredOTP" runat="server" CssClass="form-control text-center border-primary" placeholder="Enter 6-digit OTP"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row" id="Div1" runat="server" visible="false">
                            <div class="col-md-12 text-center">
                                <asp:Label ID="lbl_mailermsg" runat="server" Text="" CssClass="font-weight-bold text-danger"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer bg-light d-flex justify-content-between">
                        <small class="text-muted"><i class="fa fa-info-circle"></i>Failure to verify restricts access.</small>
                        <div>
                            <asp:Button ID="btn_cancel1" runat="server" CausesValidation="false" Text="Logout" CssClass="btn btn-outline-danger btn-sm" OnClick="btn_cancel1_Click" />
                            <asp:Button ID="btn_sv_contactdata" runat="server" Text="Make Changes" CssClass="btn btn-primary btn-sm" OnClick="btn_sv_contactdata_Click" />
                            <asp:Button ID="btn_cancel_contactdata" runat="server" CausesValidation="false" Text="Confirm Details" CssClass="btn btn-success btn-sm" OnClick="btn_cancel_contactdata_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="myModal5" data-backdrop="static">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content border-0">
                    <div class="modal-header bg-primary text-white">
                        <h5 class="modal-title m-0"><i class="fa fa-file-text-o"></i>Document Upload Required</h5>
                    </div>
                    <div class="modal-body p-4 text-center">
                        <i class="fa fa-cloud-upload text-primary mb-3" style="font-size: 48px;"></i>
                        <div class="alert alert-info border-0 shadow-sm text-left">
                            <asp:Label ID="Label2" runat="server" Text="From your next login, please upload a soft copy of your Aadhaar, Bank Passbook, PAN and Qualification as required by ATS Management for documentation purposes. Thank you for your cooperation." CssClass="d-block mb-3"></asp:Label>
                            <hr style="border-color: rgba(0,0,0,0.1);" />
                            <asp:Label ID="Label3" runat="server" Text="अपने अगले लॉगिन से, कृपया अपने आधार, बैंक पासबुक, पैन और योग्यता की सॉफ्ट कॉपी अपलोड करें, जैसा कि दस्तावेज़ीकरण उद्देश्यों के लिए एटीएस प्रबंधन द्वारा आवश्यक है। आपके सहयोग के लिए धन्यवाद।" CssClass="d-block text-muted" Style="font-size: 13px;"></asp:Label>
                        </div>
                    </div>
                    <div class="modal-footer justify-content-center bg-light">
                        <asp:Button ID="btn_declinedoc" runat="server" CausesValidation="false" Text="Remind me in 3 Days" CssClass="btn btn-outline-secondary btn-sm px-4" OnClick="btn_declinedoc_Click" />
                        <asp:Button ID="btn_acceptdoc" runat="server" CausesValidation="false" Text="I Understand" CssClass="btn btn-success btn-sm px-4" OnClick="btn_acceptdoc_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="qrModal" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-sm">
                <div class="modal-content border-0 shadow-lg">
                    <div class="modal-header bg-white border-0 pb-0">
                        <button type="button" class="close text-dark" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body text-center pt-0 pb-4">
                        <h5 class="font-weight-bold text-theme mb-3">Live Employee QR</h5>
                        <img id="enlarged_qr_img" src="" alt="Enlarged QR Code" class="img-fluid rounded" style="max-width: 220px; width: 100%; border: 1px solid #eee; padding: 10px; background: #fff; box-shadow: 0 4px 10px rgba(0,0,0,0.05);" />
                        <p class="text-muted small mt-3 mb-0">Scan this code to verify identity and live GPS coordinates.</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        // Function to open the QR Modal
        function showEnlargedQR(imgSrc) {
            if (imgSrc) {
                $('#enlarged_qr_img').attr('src', imgSrc);
                $('#qrModal').modal('show');
            }
        }

        // ==========================================
        // FETCH GPS & UPDATE QR CODE
        // ==========================================
        $(document).ready(function () {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function (position) {
                    var lat = position.coords.latitude.toFixed(6);
                    var lon = position.coords.longitude.toFixed(6);

                    $.ajax({
                        type: "POST",
                        url: "homepage_v2.aspx/GetLocationQR",
                        data: JSON.stringify({ latitude: lat, longitude: lon }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            if (response.d) {
                                var newQrSrc = "data:image/png;base64," + response.d;

                                // Update the card's QR code
                                $("[id$=img_qrcode]").attr("src", newQrSrc);
                                $("[id$=img_qrcode]").css("border", "2px solid #2ecc71"); // Green border indicates GPS success

                                // Sync the modal's QR code just in case they click it fast
                                $('#enlarged_qr_img').attr("src", newQrSrc);
                            }
                        }
                    });
                }, function (error) {
                    console.log("GPS Location denied or unavailable. Fallback to basic QR.");
                }, { enableHighAccuracy: true, timeout: 5000, maximumAge: 0 });
            }
        });

        // Existing Modals
        function ShowPopup(title, body) {
            $("#MyPopup .modal-title").html(title);
            $("#MyPopup .modal-body").html(body);
            $("#MyPopup").modal("show");
        }
        function ShowPopup1() { $("#myModal").modal("show"); }
        function ShowPopup2() { $("#myModal2").modal("show"); }
        function ShowPasswordModal() { $("#myModal3").modal("show"); }
        function ShowContactModal() { $("#myModal4").modal("show"); }
        function ShowDocModal() { $("#myModal5").modal("show"); }
    </script>
</asp:Content>
