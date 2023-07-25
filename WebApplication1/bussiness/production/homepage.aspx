<%@ Page Title="ATS | Home" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="homepage.aspx.cs" Inherits="WebApplication1.bussiness.production.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <!-- /top tiles -->
        <%--<div class="">
			<div class="page-title">
				<div class="title_left">
					<h3>Welcome Back!</h3>
				</div>
			</div>

			<div class="clearfix"></div>
			<br />
		</div>--%>
        <div class="row">
            <%--<div class="x_panel">
				<div class="x_content">--%>

            <div class="col-md-12 col-sm-12 col-lg-12 profile_details">
                <div class="well profile_view col-sm-12 col-lg-12">
                    <div class="col-sm-12">
                        <h4 class="brief"><i>Employee Card</i> :
							<asp:Label ID="lbl_id" runat="server" Text="N/A"></asp:Label>
                        </h4>

                        <div class="right col-md-4 col-sm-4 text-center">
                            <img id="ProfilePic_3" runat="server" src="...." width="220" height="240" alt="ProfilePhoto" class="img-circle img-fluid">
                        </div>

                        <div class="left col-md-8 col-sm-8 text-left">
                            <h2 style="text-align: center;">
                                <asp:Label ID="lbl_workmansl" runat="server" Text="N/A" ForeColor="DarkBlue" Font-Bold="true"></asp:Label>
                                :
								<asp:Label ID="lbl_username" runat="server" Text="N/A" ForeColor="Black" Font-Bold="true"></asp:Label>
                            </h2>
                            <hr />
                            <p>
                                <strong>Designation : </strong>
                                <asp:Label ID="lbl_desg" runat="server" Text=""></asp:Label>
                                [<asp:Label ID="lbl_skillcat" runat="server" Text=""></asp:Label>]
                            </p>

                            <p>
                                <strong>Deputed Company :</strong>
                                <asp:Label ID="lbl_wrkcopmany" runat="server" Text=""></asp:Label>,
								<asp:Label ID="lbl_region" runat="server" Text=""></asp:Label>,
								<asp:Label ID="lbl_state" runat="server" Text="N/A"></asp:Label>.
                            </p>

                            <p>
                                <strong>Deputed Site :</strong>
                                <asp:Label ID="lbl_wrksite" runat="server" Text=""></asp:Label>
                            </p>

                            <ul class="list-unstyled">
                                <li><i class="fa fa-calendar"></i>DOJ :
									<asp:Label ID="lbl_doj" runat="server" Text="N/A"></asp:Label></li>
                                <li><i class="fa fa-clock-o"></i>Work Tenure :
									<asp:Label ID="lbl_workage" runat="server" Text=""></asp:Label></li>
                            </ul>
                        </div>
                    </div>

                    <div class=" profile-bottom text-center">
                        <div class="col-sm-6 emphasis">
                            <%--<p class="ratings">
								<a>4.0</a>
								<a href="#"><span class="fa fa-star"></span></a>
								<a href="#"><span class="fa fa-star"></span></a>
								<a href="#"><span class="fa fa-star"></span></a>
								<a href="#"><span class="fa fa-star"></span></a>
								<a href="#"><span class="fa fa-star-o"></span></a>
							</p>--%>
                        </div>
                        <div class="col-sm-6 emphasis">
                            <asp:Button ID="btn_mngprofile" runat="server" Text="Manage Profile" CausesValidation="false" class="btn btn-success btn-sm" PostBackUrl="~/bussiness/production/manage_profile.aspx" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- Small modal -->
            <asp:Button ID="ShowPopup" runat="server" Text="Button" class="btn btn-primary" Visible="false" data-toggle="modal" data-target=".bs-example-modal-sm" />
            <div id="MyPopup" class="modal fade bs-example-modal-sm" data-backdrop="static" tabindex="-1" role="dialog" aria-hidden="true">
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
            <div class="col-md-6 col-sm-6 profile_details">
                <div class="well profile_view col-sm-12 col-lg-12">
                    <div class="col-sm-12">
                        <h4 class="brief"><i>Attendance :
							<asp:Label ID="lbl_calmonth" runat="server" Text="0"></asp:Label>,
							<asp:Label ID="lbl_calyear" runat="server" Text="0"></asp:Label></i></h4>

                        <div class="right col-md-6 col-sm-6 text-center">
                            <asp:Label ID="lbl_totalpresent" runat="server" Text="0" ForeColor="Green" Font-Size="90px"></asp:Label>
                            /
							<asp:Label ID="lbl_caldays" runat="server" Font-Size="Medium" Font-Bold="true" ForeColor="Black" Text="0"></asp:Label>
                        </div>
                        <div class="left col-md-6 col-sm-6">
                            <h2>Days Worked :
								<asp:Label ID="lbl_dayswrkd" runat="server" Text="0" Font-Bold="true"></asp:Label></h2>
                            <span><strong style="color: green;">P : </strong>
                                <asp:Label ID="lbl_presentdayscount" runat="server" Text="0" Font-Bold="true"></asp:Label>
                            </span>&nbsp;|&nbsp;
							<span><strong style="color: darkorange;">OD : </strong>
                                <asp:Label ID="lbl_oddayscount" runat="server" Text="0" Font-Bold="true"></asp:Label>
                            </span>&nbsp;|&nbsp;
							<span><strong style="color: darkblue;">NH : </strong>
                                <asp:Label ID="lbl_nhcount" runat="server" Text="0" Font-Bold="true"></asp:Label>
                            </span>&nbsp;|&nbsp;
							<span><strong style="color: blue;">FL : </strong>
                                <asp:Label ID="lbl_flcount" runat="server" Text="0" Font-Bold="true"></asp:Label>
                            </span>
                            <hr />
                            <h2>Total OT :
								<asp:Label ID="lbl_totalot" runat="server" Text="32" ForeColor="Brown" Font-Bold="true"></asp:Label>
                                Hours</h2>
                        </div>

                    </div>
                    <div class=" profile-bottom text-center">
                        <div class=" col-sm-6 emphasis">
                            **Only Approved Count
                        </div>
                        <div class=" col-sm-6 emphasis">
                            <asp:Button ID="btn_vwmntlyattn" runat="server" class="btn btn-success btn-sm" CausesValidation="false" Text="View Details" OnClick="btn_vwmntlyattn_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-6 col-sm-6 profile_details">
                <div class="well profile_view col-sm-12 col-lg-12">
                    <div class="col-sm-12">
                        <h4 class="brief"><i>Gatepass : </i>
                            <asp:Label ID="lbl_gpno" runat="server" Text="0" Font-Bold="true"></asp:Label></h4>
                        <div class="right col-md-5 col-sm-6 text-center">
                            <asp:Label ID="lbl_gpexpdays" runat="server" ForeColor="Green" Text="0" Font-Size="80px"></asp:Label>
                            Days Left
                        </div>
                        <div class="left col-md-7 col-sm-6">
                            <span style="text-align: center;"><strong>Gatepass Expiry : </strong>
                                <asp:Label ID="lbl_gpvalidity" runat="server" Text="0" ForeColor="Green" Font-Bold="true"></asp:Label>
                            </span>
                            <hr />
                            <h2>Saftey No :
								<asp:Label ID="lbl_rfidno" runat="server" Text="32" ForeColor="Green" Font-Bold="true"></asp:Label></h2>

                            <span><strong>Safety Expiry : </strong>
                                <asp:Label ID="lbl_rfidvalidity" runat="server" Text="0" ForeColor="Green" Font-Bold="true"></asp:Label>,
								<asp:Label ID="lbl_rfiddays" runat="server" ForeColor="Green" Text="0"></asp:Label>
                                Days Left.
                            </span>
                            <br />
                            <span><strong>PV Expiry : </strong>
                                <asp:Label ID="lbl_pvvalidity" runat="server" Text="0" ForeColor="Green" Font-Bold="true"></asp:Label>,
								<asp:Label ID="lbl_pvdays" runat="server" ForeColor="Green" Text="0"></asp:Label>
                                Days Left.
                            </span>
                        </div>
                    </div>
                    <div class=" profile-bottom text-center">
                        <div class=" col-sm-6 emphasis">
                            <p class="ratings">
                                &nbsp;
                            </p>
                        </div>
                        <div class="right col-sm-6 pull-right">
                            <button type="button" class="btn btn-primary btn-sm" id="btnShowPopup2" data-toggle="modal" data-target="#myModal2">
                                Update
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-6 col-sm-6 profile_details">
                <div class="well profile_view col-sm-12 col-lg-12">
                    <div class="col-sm-12">
                        <h4 class="brief"><i>Salary Bank Account</i></h4>
                        <div class="right col-md-5 col-sm-6 text-center">
                            <img src="../../erp_images/bank.jpg" alt="ProfilePhoto" class="img-circle">
                        </div>
                        <div class="left col-md-7 col-sm-6">
                            <h2 style="text-align: center;">
                                <asp:Label ID="lbl_bankname" runat="server" Text="0" ForeColor="Black" Font-Bold="true"></asp:Label></h2>
                            <hr />

                            <span style="text-align: center;"><strong>Account No :</strong>
                                <asp:Label ID="lbl_accno" runat="server" Text="0" ForeColor="DarkBlue" Font-Bold="true"></asp:Label>
                            </span>
                            <br />

                            <span><strong>IFSC Code : </strong>
                                <asp:Label ID="lbl_ifsc" runat="server" Text="0" ForeColor="DeepSkyBlue" Font-Bold="true"></asp:Label>
                            </span>
                            <br />
                            <span><strong>Bank Branch : </strong>
                                <asp:Label ID="lbl_branch" runat="server" Text="N/A" Font-Bold="true"></asp:Label>
                            </span>
                            <br />
                            <span>
                                <asp:Label ID="lbl_bankupdtinfo" runat="server" Text="N/A" Font-Bold="true"></asp:Label>
                            </span>
                        </div>
                    </div>
                    <div class=" profile-bottom text-center">
                        <div class=" col-sm-6 emphasis">
                            <p class="ratings">
                                &nbsp;
                            </p>
                        </div>
                        <div class="right col-sm-6 pull-right">
                            <button type="button" class="btn btn-primary btn-sm" id="btnShowPopup" data-toggle="modal" data-target="#myModal">
                                Update
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-6 col-sm-6 profile_details">
                <div class="well profile_view col-sm-12 col-lg-12">
                    <div class="col-sm-12">
                        <h4 class="brief"><i>Employee Benefits (Last Month) </i></h4>
                        <div class="right col-md-5 col-sm-6 text-center">
                            <img src="../../erp_images/piggy.png" alt="ProfilePhoto" class="img-circle" width="150" height="150">
                        </div>
                        <div class="left col-md-7 col-sm-6">
                            <h2 style="text-align: center; color: black;">EPFO Contribution</h2>

                            <span style="text-align: center;"><strong>EPFO No. :</strong>
                                <asp:Label ID="lbl_pfno" runat="server" Text="0" ForeColor="DarkBlue" Font-Bold="true"></asp:Label>
                            </span>
                            <br />
                            <span style="text-align: center;"><strong>Last Cont. :</strong>
                                ₹.<asp:Label ID="lbl_lastpfpay" runat="server" Text="0.00" ForeColor="DarkBlue" Font-Bold="true"></asp:Label>
                            </span>
                            <hr />


                            <h2 style="text-align: center; color: brown;">ESIC Contribution</h2>
                            <span style="text-align: center;"><strong>ESIC No. :</strong>
                                <asp:Label ID="lbl_esicno" runat="server" Text="N/A" ForeColor="DarkBlue" Font-Bold="true"></asp:Label>
                            </span>
                            <br />
                            <span><strong>Last Cont. : </strong>
                                ₹.<asp:Label ID="lbl_lastesicpay" runat="server" Text="0.00" Font-Bold="true"></asp:Label>
                            </span>
                        </div>
                    </div>
                    <div class=" profile-bottom text-center">
                        <div class=" col-sm-6 emphasis">
                            <p class="ratings">
                                &nbsp;
                            </p>
                        </div>
                        <div class="right col-sm-6 pull-right">
                            <button type="button" class="btn btn-primary btn-sm">
                                <i class="fa fa-user"></i>View
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <%--</div>
			</div>--%>
            <div class="clearfix"></div>
            <br />
        </div>

        <%--- Up-loader Modal --%>
        <div class="modal fade" id="myModal" data-backdrop="static">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Update Salary Account Details</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <div class="col-md-6 col-sm-12 form-group">
                                        <label>Enter Bank Name :<span class="text text-danger"></span></label>
                                    </div>
                                    <div class="col-md-6 col-sm-12 form-group">
                                        <asp:TextBox ID="txt_bankname" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV1" runat="server" ValidationGroup="BANK" Display="Dynamic" ForeColor="Red" ControlToValidate="txt_bankname" ErrorMessage="**"></asp:RequiredFieldValidator>
                                    </div>

                                    <div class="col-md-6 col-sm-12 form-group">
                                        <label>Enter Account Number :<span class="text text-danger"></span></label>
                                    </div>
                                    <div class="col-md-6 col-sm-12 form-group">
                                        <asp:TextBox ID="txt_accno" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV2" runat="server" ValidationGroup="BANK" Display="Dynamic" ForeColor="Red" ErrorMessage="**" ControlToValidate="txt_accno"></asp:RequiredFieldValidator>
                                    </div>

                                    <div class="col-md-6 col-sm-12 form-group" id="dup_acnorow1" runat="server" visible="false">
                                        <label>Re-Enter Account Number :<span class="text text-danger"></span></label>
                                    </div>
                                    <div class="col-md-6 col-sm-12 form-group" id="dup_acnorow2" runat="server" visible="false">
                                        <asp:TextBox ID="txt_cnfaccno" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV3" runat="server" ValidationGroup="BANK" ErrorMessage="**" ControlToValidate="txt_cnfaccno"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="CV1" runat="server" ValidationGroup="BANK" Display="Dynamic" ForeColor="Red" ErrorMessage="Enter same Account Number" ControlToCompare="txt_accno" ControlToValidate="txt_cnfaccno"></asp:CompareValidator>
                                    </div>

                                    <div class="col-md-6 col-sm-12 form-group">
                                        <label>Enter IFSC Code :<span class="text text-danger"></span></label>
                                    </div>
                                    <div class="col-md-6 col-sm-12 form-group">
                                        <asp:TextBox ID="txt_ifsc" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV4" runat="server" ValidationGroup="BANK" Display="Dynamic" ForeColor="Red" ErrorMessage="**" ControlToValidate="txt_ifsc"></asp:RequiredFieldValidator>
                                    </div>

                                    <div class="col-md-6 col-sm-12 form-group">
                                        <label>Enter Branch Name :<span class="text text-danger"></span></label>
                                    </div>
                                    <div class="col-md-6 col-sm-12 form-group">
                                        <asp:TextBox ID="txt_branchname" runat="server" class="form-control form-control-sm rounded" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btn_bankedit" runat="server" ValidationGroup="BANK" Enabled="false" CausesValidation="true" Text="Make Changes" CssClass="btn btn-info btn-sm" OnClick="btn_bankedit_Click" />
                        <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <%--- Up-loader Modal -------- END --%

		<%--- Up-loader Modal ---------START----%>
        <div class="modal fade" id="myModal2" data-backdrop="static">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Update Gatepass Details</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">

                                    <div class="col-md-6 col-sm-12 form-group">
                                        <label>Gatepass No :<span class="text text-danger"></span></label>
                                    </div>
                                    <div class="col-md-6 col-sm-12 form-group">
                                        <asp:Label ID="lbl_oldgpno" runat="server" Text="N/A" CssClass="form-control form-control-sm rounded" BackColor="#e6e6e6"></asp:Label>
                                    </div>

                                    <div class="col-md-6 col-sm-12 form-group" id="nwgprow1" runat="server" visible="false">
                                        <label style="font-weight: bold; color: darkblue;">Enter New Gatepass No :<span class="text text-danger"></span></label>
                                    </div>
                                    <div class="col-md-6 col-sm-12 form-group" id="nwgprow2" runat="server" visible="false">
                                        <asp:TextBox ID="txt_nwgpno" runat="server" CssClass="form-control form-control-sm rounded" BorderColor="Blue" BorderWidth="2px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="GPDATA" ControlToValidate="txt_nwgpno" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                    </div>

                                    <div class="col-md-6 col-sm-12 form-group">
                                        <label>Gatepass Validity :<span class="text text-danger"></span></label>
                                    </div>
                                    <div class="col-md-6 col-sm-12 form-group">
                                        <asp:Label ID="lbl_oldgpvalidity" runat="server" Text="N/A" CssClass="form-control form-control-sm rounded" BackColor="#e6e6e6"></asp:Label>
                                    </div>

                                    <div class="col-md-6 col-sm-12 form-group" id="nwgpvalrow1" runat="server" visible="false">
                                        <label style="font-weight: bold; color: darkblue;">New Gatepass Validity :<span class="text text-danger"></span></label>
                                    </div>
                                    <div class="col-md-6 col-sm-12 form-group" id="nwgpvalrow2" runat="server" visible="false">
                                        <asp:TextBox ID="txt_nwgpvalidity" runat="server" CssClass="form-control form-control-sm rounded" class='date' type="date" name="date" BorderColor="Blue" BorderWidth="2px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="GPDATA" ControlToValidate="txt_nwgpvalidity" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                    </div>

                                    <div class="col-md-6 col-sm-12 form-group">
                                        <label>Safety No :<span class="text text-danger"></span></label>
                                    </div>
                                    <div class="col-md-6 col-sm-12 form-group">
                                        <asp:Label ID="lbl_oldsftyno" runat="server" Text="N/A" CssClass="form-control form-control-sm rounded" BackColor="#e6e6e6"></asp:Label>
                                    </div>

                                    <div class="col-md-6 col-sm-12 form-group" id="nwsftyrow1" runat="server" visible="false">
                                        <label style="font-weight: bold; color: darkblue;">Enter New Safety No :<span class="text text-danger"></span></label>
                                    </div>
                                    <div class="col-md-6 col-sm-12 form-group" id="nwsftyrow2" runat="server" visible="false">
                                        <asp:TextBox ID="txt_nwsftyno" runat="server" CssClass="form-control form-control-sm rounded" BorderColor="Blue" BorderWidth="2px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="GPDATA" ControlToValidate="txt_nwsftyno" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                    </div>

                                    <div class="col-md-6 col-sm-12 form-group">
                                        <label>Safety Validity :<span class="text text-danger"></span></label>
                                    </div>
                                    <div class="col-md-6 col-sm-12 form-group">
                                        <asp:Label ID="lbl_oldsftyval" runat="server" Text="N/A" CssClass="form-control form-control-sm rounded"></asp:Label>
                                    </div>

                                    <div class="col-md-6 col-sm-12 form-group" id="nwrfidrow1" runat="server" visible="false">
                                        <label style="font-weight: bold; color: darkblue;">New Safety Validity :<span class="text text-danger"></span></label>
                                    </div>
                                    <div class="col-md-6 col-sm-12 form-group" id="nwrfidrow2" runat="server" visible="false">
                                        <asp:TextBox ID="txt_nwsftyvalidity" runat="server" CssClass="form-control form-control-sm rounded" class='date' type="date" name="date" BorderColor="Blue" BorderWidth="2px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="GPDATA" ControlToValidate="txt_nwsftyvalidity" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                    </div>

                                    <div class="col-md-6 col-sm-12 form-group">
                                        <label>PV Validity :<span class="text text-danger"></span></label>
                                    </div>
                                    <div class="col-md-6 col-sm-12 form-group">
                                        <asp:Label ID="lbl_oldpvvalidity" runat="server" Text="N/A" CssClass="form-control form-control-sm rounded"></asp:Label>
                                    </div>

                                    <div class="col-md-6 col-sm-12 form-group" id="nwpvrow1" runat="server" visible="false">
                                        <label style="font-weight: bold; color: darkblue;">New PV Validity :<span class="text text-danger"></span></label>
                                    </div>
                                    <div class="col-md-6 col-sm-12 form-group" id="nwpvrow2" runat="server" visible="false">
                                        <asp:TextBox ID="txt_nwpvvalidity" runat="server" CssClass="form-control form-control-sm rounded" class='date' type="date" name="date" BorderColor="Blue" BorderWidth="2px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="GPDATA" ControlToValidate="txt_nwpvvalidity" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <asp:Label ID="Label1" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btn_gtpsedit" runat="server" CausesValidation="true" ValidationGroup="GPDATA" Text="Make Changes" CssClass="btn btn-info btn-sm" OnClick="btn_gtpsedit_Click" />
                        <asp:Button ID="btn_cancel" runat="server" CausesValidation="false" Text="Cancel" CssClass="btn btn-warning btn-sm" OnClick="btn_cancel_Click" />
                        <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <%--- Up-loader Modal -------- END --%>

        <%--- Contact Details Modal ---------START----%>
        <div class="modal fade" id="myModal4" data-backdrop="static">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Update Contact Details</h4>
                        <%--<button type="button" class="close" data-dismiss="modal">&times;</button>--%>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">

                                    <div class="col-md-6 col-sm-12 form-group">
                                        <label>Mobile No :<span class="text text-danger"></span></label>
                                    </div>
                                    <div class="col-md-6 col-sm-12 form-group">
                                        <asp:Label ID="lbl_oldmobileno" runat="server" Text="N/A" CssClass="form-control form-control-sm rounded" BackColor="#e6e6e6"></asp:Label>
                                    </div>

                                    <div class="col-md-6 col-sm-12 form-group" id="InputMob1" runat="server" visible="false">
                                        <label style="font-weight: bold; color: darkblue;">Confirm Mobile No :<span class="text text-danger"></span></label>
                                    </div>
                                    <div class="col-md-6 col-sm-12 form-group" id="InputMob2" runat="server" visible="false">
                                        <asp:TextBox ID="txt_nwmobileno" runat="server" CssClass="form-control form-control-sm rounded" BorderColor="Blue" BorderWidth="2px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="MobileFieldValidator" runat="server" ValidationGroup="ContData" ErrorMessage="Input Required" ControlToValidate="txt_nwmobileno" InitialValue="" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="MobileValidator" runat="server" ControlToValidate="txt_nwmobileno"
                                            ErrorMessage="Invalid mobile number" ValidationExpression="^[0-9]{10}$"
                                            ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationGroup="ContData"></asp:RegularExpressionValidator>
                                    </div>

                                    <div class="col-md-6 col-sm-12 form-group">
                                        <label>Email Address :<span class="text text-danger"></span></label>
                                    </div>
                                    <div class="col-md-6 col-sm-12 form-group">
                                        <asp:Label ID="lbl_oldemailadd" runat="server" Text="N/A" CssClass="form-control form-control-sm rounded" BackColor="#e6e6e6"></asp:Label>
                                    </div>

                                    <div class="col-md-6 col-sm-12 form-group" id="InputEmail1" runat="server" visible="false">
                                        <label style="font-weight: bold; color: darkblue;">Confirm Email Address :<span class="text text-danger"></span></label>
                                    </div>
                                    <div class="col-md-6 col-sm-12 form-group" id="InputEmail2" runat="server" visible="false">
                                        <asp:TextBox ID="txt_nwemailadd" runat="server" CssClass="form-control form-control-sm rounded" BorderColor="Blue" BorderWidth="2px" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="EmailFieldValidator" runat="server" ControlToValidate="txt_nwemailadd" ErrorMessage="Input Required" InitialValue="" ValidationGroup="ContData" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="EmailValidator" runat="server" ControlToValidate="txt_nwemailadd"
                                            ErrorMessage="Invalid email address" ValidationExpression="\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b"
                                            ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationGroup="ContData"></asp:RegularExpressionValidator>
                                    </div>

                                    <div class="col-md-6 col-sm-12 form-group" id="OTP_1" runat="server" visible="false">
                                        <asp:Button ID="btn_SendOTP" runat="server" Text="Send OTP" CausesValidation="true" ValidationGroup="ContData" CssClass="btn btn-info btn-sm" OnClick="btn_SendOTP_Click" />
                                    </div>
                                    <div class="col-md-6 col-sm-12 form-group" id="OTP_2" runat="server" visible="false">
                                        <asp:TextBox ID="TextBoxEnteredOTP" runat="server" CssClass="form-control form-control-sm rounded" BorderColor="Blue" BorderWidth="2px"></asp:TextBox>
                                    </div>

                                    <div class="col-md-6 col-sm-12 form-group" id="Div1" runat="server" visible="false">
                                        <asp:Label ID="lbl_mailermsg" runat="server" Text="" Style="font-weight: bold; color:darkred;"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btn_sv_contactdata" runat="server" Text="Make Changes" CssClass="btn btn-info btn-sm" OnClick="btn_sv_contactdata_Click" />
                        <asp:Button ID="btn_cancel_contactdata" runat="server" CausesValidation="false" Text="Confirm" CssClass="btn btn-warning btn-sm" OnClick="btn_cancel_contactdata_Click" />
                    </div>
                </div>
            </div>
        </div>
        <%--- Up-loader Modal -------- END --%>

        <!-- Large modal : Password Change Popup-------START------>
        <div class="modal fade bs-pass-modal-lg" id="myModal3" data-backdrop="static">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Change Login Credentails</h4>
                        <button type="button" class="close" data-dismiss="modal" disabled="disabled">
                            &times;
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="container-fluid">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="form-group">
                                    <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                        <label>Login ID:<span class="text text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-4 col-sm-6 col-xs-6 form-group">
                                        <asp:TextBox ID="txt_atsloginid" runat="server" class="form-control form-control-sm rounded" Text="N/A" Font-Bold="true" ForeColor="Blue" ReadOnly="true"></asp:TextBox>
                                    </div>

                                    <div class="col-md-2 form-group">
                                        <label>Workmen Sl:<span class="text text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-4 form-group">
                                        <asp:TextBox ID="txt_atsworkmenno" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" Font-Bold="true" ForeColor="Blue"></asp:TextBox>
                                    </div>

                                    <div class="col-md-2 form-group">
                                        <label>OLD Password:<span class="text text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-10 form-group">
                                        <asp:TextBox ID="txt_oldpass" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="false" OnTextChanged="txt_oldpass_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    </div>

                                    <div class="col-md-2 form-group" id="newpwd_row1" runat="server" visible="false">
                                        <label>New Password:<span class="text text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-4 form-group" id="newpwd_row2" runat="server" visible="false">
                                        <asp:TextBox ID="txt_newpass1" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_P1" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txt_newpass1" InitialValue="" ValidationGroup="ChnagePassword" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="REV_P1" runat="server" ErrorMessage="AlphaNumeric Password Policy" ForeColor="Red" ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$" ControlToValidate="txt_newpass1"></asp:RegularExpressionValidator>
                                    </div>

                                    <div class="col-md-2 form-group" id="newpwd_row3" runat="server" visible="false">
                                        <label>New Password:<span class="text text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-4 form-group" id="newpwd_row4" runat="server" visible="false">
                                        <asp:TextBox ID="txt_newpass2" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_P2" runat="server" ErrorMessage="*" ForeColor="Red" InitialValue="" ControlToValidate="txt_newpass2" ValidationGroup="ChnagePassword" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="CV_P1" runat="server" ErrorMessage="CompareValidator" ControlToCompare="txt_newpass1" ForeColor="Red" ControlToValidate="txt_newpass2"></asp:CompareValidator>
                                    </div>

                                    <div class="col-md-2 form-group" id="newpwd_row5" runat="server" visible="false">
                                        <label>Security Q1:<span class="text text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-10 form-group" id="newpwd_row6" runat="server" visible="false">
                                        <asp:DropDownList ID="DDL_SQ1" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFV_P3" runat="server" ErrorMessage="Selection Required" ValidationGroup="ChnagePassword" ControlToValidate="DDL_SQ1" ForeColor="Red" InitialValue="Please Select Option" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    </div>

                                    <div class="col-md-2 form-group" id="newpwd_row7" runat="server" visible="false">
                                        <label>Answer to Q1 :<span class="text text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-10 form-group" id="newpwd_row8" runat="server" visible="false">
                                        <asp:TextBox ID="txt_SQAns1" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_P4" runat="server" ErrorMessage="Input Required" ControlToValidate="txt_SQAns1" ValidationGroup="ChnagePassword" InitialValue="" SetFocusOnError="true" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>

                                    <div class="col-md-2 form-group" id="newpwd_row9" runat="server" visible="false">
                                        <label>Security Q2 :<span class="text text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-10 form-group" id="newpwd_row10" runat="server" visible="false">
                                        <asp:DropDownList ID="DDL_SQ2" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFV_P5" runat="server" ErrorMessage="Selection Required" ControlToValidate="DDL_SQ2" ForeColor="Red" Display="Dynamic" InitialValue="Please Select Option" SetFocusOnError="true" ValidationGroup="ChnagePassword"></asp:RequiredFieldValidator>
                                    </div>

                                    <div class="col-md-2 form-group" id="newpwd_row11" runat="server" visible="false">
                                        <label>Answer to Q2 :<span class="text text-danger">*</span></label>
                                    </div>
                                    <div class="col-md-10 form-group" id="newpwd_row12" runat="server" visible="false">
                                        <asp:TextBox ID="txt_SQAns2" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_P6" runat="server" ErrorMessage="Input Required" ControlToValidate="txt_SQAns2" ValidationGroup="ChnagePassword" InitialValue="" SetFocusOnError="true" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>

                                    </div>

                                    <div class="col-md-8 form-group">
                                        <asp:Label ID="lbl_msgpass" runat="server" Text="Complete the above and Click Save Changes" Font-Bold="true" ForeColor="Red"></asp:Label>
                                    </div>

                                    <div class="col-md-4 form-group">
                                        <asp:Button ID="btn_relogin" CausesValidation="false" runat="server" Text="Re-Login" Enabled="false" CssClass="btn btn-primary btn-sm" OnClick="btn_relogin_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btn_closecvpass" runat="server" Text="Close" CausesValidation="false" class="btn btn-danger btn-sm" data-dismiss="modal" Enabled="true" />
                        <asp:Button ID="btn_discardsvpass" runat="server" Text="Discard Changes" CausesValidation="false" CssClass="btn btn-warning btn-sm" OnClick="btn_discardsvpass_Click" />
                        <asp:Button ID="btn_svpass" runat="server" CssClass="btn btn-success btn-sm" Text="Save Changes" Enabled="false" OnClick="btn_svpass_Click" CausesValidation="true" ValidationGroup="ChnagePassword" />
                    </div>
                </div>
            </div>
        </div>
        <!-- Large modal : Personal Information-------END------>

    </div>

    <script type="text/javascript">
        function ShowPopup(title, body) {
            $("#MyPopup .modal-title").html(title);
            $("#MyPopup .modal-body").html(body);
            $("#MyPopup").modal("show");
        }

        function ShowPopup1() {
            $("#myModal").modal("show");
        }

        function ShowPopup2() {
            $("#myModal2").modal("show");
        }

        function ShowPasswordModal() {
            $("#myModal3").modal("show");
        }

        function ShowContactModal() {
            $("#myModal4").modal("show");
        }

        function toggleDiv() {
            // Check if the email is valid
            var emailValidator = document.getElementById('<%= EmailValidator.ClientID %>');
            var emailIsValid = emailValidator.isvalid;

            // Toggle div based on validation result
            var divElement = document.getElementById("myDiv");
            if (emailIsValid) {
                if (divElement.style.display === "none") {
                    divElement.style.display = "block";
                } else {
                    divElement.style.display = "none";
                }
            } else {
                // Optionally, you can display an error message or take other actions for an invalid email.
                alert("Please enter a valid email address.");
            }
        }

    </script>
</asp:Content>
