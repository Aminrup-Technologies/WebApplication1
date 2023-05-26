<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="manage_profile.aspx.cs" Inherits="WebApplication1.bussiness.production.manage_profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>Profile Management</h3>
                </div>

                <div class="title_right">
                    <div class="col-md-5 col-sm-5 form-group row pull-right top_search"></div>
                </div>
            </div>

            <div class="clearfix"></div>

            <div class="row">

                <div class="col-md-12" id="basicinfo" runat="server" visible="true">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>General Information</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">

                            <!-- Large modal : Personal Information-------START------>
                            <div class="modal fade bs-basic-modal-lg" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
                                <div class="modal-dialog modal-lg">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h4 class="modal-title" id="myModalLabel">Update Basic Information</h4>
                                            <button type="button" class="close" data-dismiss="modal">
                                                <span aria-hidden="true">×</span>
                                            </button>
                                        </div>
                                        <div class="modal-body">
                                            <div class="container-fluid">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <div class="col-md-2 form-group">
                                                            <label>First Name :<span class="text text-danger">*</span></label>
                                                        </div>
                                                        <div class="col-md-2 form-group">
                                                            <asp:TextBox ID="TextBox1" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-2 form-group">
                                                            <label>Middle Name :<span class="text text-danger"></span></label>
                                                        </div>
                                                        <div class="col-md-2 form-group">
                                                            <asp:TextBox ID="TextBox2" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-2 form-group">
                                                            <label>Last Name :<span class="text text-danger">*</span></label>
                                                        </div>
                                                        <div class="col-md-2 form-group">
                                                            <asp:TextBox ID="TextBox3" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-2 form-group">
                                                            <label>Full Name :<span class="text text-danger">*</span></label>
                                                        </div>
                                                        <div class="col-md-10 form-group">
                                                            <asp:TextBox ID="TextBox4" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-2 form-group">
                                                            <label>Father Name :<span class="text text-danger">*</span></label>
                                                        </div>
                                                        <div class="col-md-4 form-group">
                                                            <asp:TextBox ID="txt_empfathername" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-2 form-group">
                                                            <label>Mother Name :<span class="text text-danger">*</span></label>
                                                        </div>
                                                        <div class="col-md-4 form-group">
                                                            <asp:TextBox ID="TextBox5" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-2 form-group">
                                                            <label>DOB :<span class="text text-danger">*</span></label>
                                                        </div>
                                                        <div class="col-md-4 form-group">
                                                            <asp:TextBox ID="txt_DOB" runat="server" class="date-picker form-control form-control-sm rounded" ReadOnly="true" placeholder="yyyy-mm-dd" required="required" onfocus="this.type='date'" onmouseover="this.type='date'" onclick="this.type = 'date'" onblur="this.type='text'" onmouseout="timeFunctionLong(this)"></asp:TextBox>
                                                            <script>
                                                                function timeFunctionLong(txt_DOB) {
                                                                    setTimeout(function () {
                                                                        txt_DOB.type = 'text';
                                                                    }, 60000);
                                                                }
                                                            </script>
                                                        </div>

                                                        <div class="col-md-2 form-group">
                                                            <label>Blood Group :<span class="text text-danger">*</span></label>
                                                        </div>
                                                        <div class="col-md-4 form-group">
                                                            <asp:TextBox ID="TextBox6" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-2 form-group">
                                                            <label>Mobile No. :<span class="text text-danger">*</span></label>
                                                        </div>
                                                        <div class="col-md-4 form-group">
                                                            <asp:TextBox ID="TextBox7" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-2 form-group">
                                                            <label>Alternative Contact :<span class="text text-danger">*</span></label>
                                                        </div>
                                                        <div class="col-md-4 form-group">
                                                            <asp:TextBox ID="TextBox8" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                                            <button type="button" class="btn btn-primary">Save changes</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- Large modal : Personal Information-------END------>


                        </div>
                        <a class="btn btn-app" data-toggle="modal" data-target=".bs-basic-modal-lg"><span class="badge bg-green">Click</span><i class="fa fa-edit"></i>Basic Detail</a>
                        <a class="btn btn-app" data-toggle="modal" data-target=".bs-example-modal-lg"><span class="badge bg-red">N/A</span><i class="fa fa-edit"></i>Work Details</a>
                        <a class="btn btn-app" data-toggle="modal" data-target=".bs-example-modal-lg"><span class="badge bg-red">N/A</span><i class="fa fa-edit"></i>Personal Details</a>
                        <a class="btn btn-app" data-toggle="modal" data-target=".bs-example-modal-lg"><span class="badge bg-red">N/A</span><i class="fa fa-edit"></i>Experience Details</a>
                        <a class="btn btn-app" data-toggle="modal" data-target=".bs-example-modal-lg"><span class="badge bg-red">N/A</span><i class="fa fa-edit"></i>Education Details</a>
                        <a class="btn btn-app" data-toggle="modal" data-target=".bs-example-modal-lg"><span class="badge bg-red">N/A</span><i class="fa fa-edit"></i>Dependents Details</a>
                    </div>
                </div>

                <div class="col-md-12" id="jobinfo" runat="server" visible="true">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>JOB Information</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">

                            <!-- Large modal : Personal Information-------START------>
                            <%--<div class="modal fade bs-basic-modal-lg" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
								<div class="modal-dialog modal-lg">
									<div class="modal-content">
										<div class="modal-header">
											<h4 class="modal-title" id="myModalLabel2A">Update Basic Information</h4>
											<button type="button" class="close" data-dismiss="modal">
												<span aria-hidden="true">×</span>
											</button>
										</div>
										<div class="modal-body">
											<div class="container-fluid">
												<div class="col-md-12">
													<div class="form-group">
														<div class="col-md-2 form-group">
															<label>First Name :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-2 form-group">
															<asp:TextBox ID="TextBox9" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Middle Name :<span class="text text-danger"></span></label>
														</div>
														<div class="col-md-2 form-group">
															<asp:TextBox ID="TextBox10" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Last Name :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-2 form-group">
															<asp:TextBox ID="TextBox11" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Full Name :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-10 form-group">
															<asp:TextBox ID="TextBox12" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Father Name :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox13" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Mother Name :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox14" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>DOB :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox15" runat="server" class="date-picker form-control form-control-sm rounded" ReadOnly="true" placeholder="yyyy-mm-dd" required="required" onfocus="this.type='date'" onmouseover="this.type='date'" onclick="this.type = 'date'" onblur="this.type='text'" onmouseout="timeFunctionLong(this)"></asp:TextBox>
															<script>
																function timeFunctionLong(txt_DOB) {
																	setTimeout(function () {
																		txt_DOB.type = 'text';
																	}, 60000);
																}
															</script>
														</div>

														<div class="col-md-2 form-group">
															<label>Blood Group :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox16" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Mobile No. :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox17" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Alternative Contact :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox18" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
														</div>

													</div>
												</div>
											</div>
										</div>
										<div class="modal-footer">
											<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
											<button type="button" class="btn btn-primary">Save changes</button>
										</div>
									</div>
								</div>
							</div>--%>
                            <!-- Large modal : Personal Information-------END------>


                        </div>
                        <a class="btn btn-app" data-toggle="modal" data-target=".bs-example-modal-lg"><span class="badge bg-red">N/A</span><i class="fa fa-edit"></i>Employee Status</a>
                        <a class="btn btn-app" data-toggle="modal" data-target=".bs-example-modal-lg"><span class="badge bg-red">N/A</span><i class="fa fa-edit"></i>Employement Type</a>
                    </div>
                </div>

                <div class="col-md-12" id="leaveinfo" runat="server" visible="true">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Leave Information</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">

                            <!-- Large modal : Personal Information-------START------>
                            <%--<div class="modal fade bs-basic-modal-lg" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
								<div class="modal-dialog modal-lg">
									<div class="modal-content">
										<div class="modal-header">
											<h4 class="modal-title" id="myModalLabel2A">Update Basic Information</h4>
											<button type="button" class="close" data-dismiss="modal">
												<span aria-hidden="true">×</span>
											</button>
										</div>
										<div class="modal-body">
											<div class="container-fluid">
												<div class="col-md-12">
													<div class="form-group">
														<div class="col-md-2 form-group">
															<label>First Name :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-2 form-group">
															<asp:TextBox ID="TextBox9" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Middle Name :<span class="text text-danger"></span></label>
														</div>
														<div class="col-md-2 form-group">
															<asp:TextBox ID="TextBox10" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Last Name :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-2 form-group">
															<asp:TextBox ID="TextBox11" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Full Name :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-10 form-group">
															<asp:TextBox ID="TextBox12" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Father Name :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox13" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Mother Name :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox14" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>DOB :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox15" runat="server" class="date-picker form-control form-control-sm rounded" ReadOnly="true" placeholder="yyyy-mm-dd" required="required" onfocus="this.type='date'" onmouseover="this.type='date'" onclick="this.type = 'date'" onblur="this.type='text'" onmouseout="timeFunctionLong(this)"></asp:TextBox>
															<script>
																function timeFunctionLong(txt_DOB) {
																	setTimeout(function () {
																		txt_DOB.type = 'text';
																	}, 60000);
																}
															</script>
														</div>

														<div class="col-md-2 form-group">
															<label>Blood Group :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox16" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Mobile No. :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox17" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Alternative Contact :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox18" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
														</div>

													</div>
												</div>
											</div>
										</div>
										<div class="modal-footer">
											<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
											<button type="button" class="btn btn-primary">Save changes</button>
										</div>
									</div>
								</div>
							</div>--%>
                            <!-- Large modal : Personal Information-------END------>


                        </div>
                        <a class="btn btn-app" data-toggle="modal" data-target=".bs-example-modal-lg"><span class="badge bg-red">N/A</span><i class="fa fa-edit"></i>Balances</a>
                        <a class="btn btn-app" data-toggle="modal" data-target=".bs-example-modal-lg"><span class="badge bg-red">N/A</span><i class="fa fa-edit"></i>Leave Apply</a>
                        <a class="btn btn-app" data-toggle="modal" data-target=".bs-example-modal-lg"><span class="badge bg-red">N/A</span><i class="fa fa-edit"></i>Leave History</a>
                    </div>
                </div>

                <div class="col-md-12" id="documents" runat="server" visible="true">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Documents</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">

                            <!-- Large modal : Personal Information-------START------>
                            <%--<div class="modal fade bs-basic-modal-lg" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
								<div class="modal-dialog modal-lg">
									<div class="modal-content">
										<div class="modal-header">
											<h4 class="modal-title" id="myModalLabel2A">Update Basic Information</h4>
											<button type="button" class="close" data-dismiss="modal">
												<span aria-hidden="true">×</span>
											</button>
										</div>
										<div class="modal-body">
											<div class="container-fluid">
												<div class="col-md-12">
													<div class="form-group">
														<div class="col-md-2 form-group">
															<label>First Name :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-2 form-group">
															<asp:TextBox ID="TextBox9" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Middle Name :<span class="text text-danger"></span></label>
														</div>
														<div class="col-md-2 form-group">
															<asp:TextBox ID="TextBox10" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Last Name :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-2 form-group">
															<asp:TextBox ID="TextBox11" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Full Name :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-10 form-group">
															<asp:TextBox ID="TextBox12" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Father Name :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox13" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Mother Name :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox14" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>DOB :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox15" runat="server" class="date-picker form-control form-control-sm rounded" ReadOnly="true" placeholder="yyyy-mm-dd" required="required" onfocus="this.type='date'" onmouseover="this.type='date'" onclick="this.type = 'date'" onblur="this.type='text'" onmouseout="timeFunctionLong(this)"></asp:TextBox>
															<script>
																function timeFunctionLong(txt_DOB) {
																	setTimeout(function () {
																		txt_DOB.type = 'text';
																	}, 60000);
																}
															</script>
														</div>

														<div class="col-md-2 form-group">
															<label>Blood Group :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox16" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Mobile No. :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox17" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Alternative Contact :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox18" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
														</div>

													</div>
												</div>
											</div>
										</div>
										<div class="modal-footer">
											<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
											<button type="button" class="btn btn-primary">Save changes</button>
										</div>
									</div>
								</div>
							</div>--%>
                            <!-- Large modal : Personal Information-------END------>


                        </div>
                        <a class="btn btn-app" data-toggle="modal" data-target=".bs-example-modal-lg"><span class="badge bg-red">N/A</span><i class="fa fa-files-o"></i>PAN</a>
                        <a class="btn btn-app" data-toggle="modal" data-target=".bs-example-modal-lg"><span class="badge bg-red">N/A</span><i class="fa fa-edit"></i>UUID</a>
                        <a class="btn btn-app" data-toggle="modal" data-target=".bs-example-modal-lg"><span class="badge bg-red">N/A</span><i class="fa fa-money"></i>EPFO</a>
                        <a class="btn btn-app" data-toggle="modal" data-target=".bs-example-modal-lg"><span class="badge bg-red">N/A</span><i class="fa fa-medkit"></i>ESIC</a>
                        <a class="btn btn-app" href="emp_profilepic.aspx"><span class="badge bg-green">Ok</span><i class="fa fa-edit"></i>Photograph</a>
                    </div>
                </div>

                <div class="col-md-12" id="payrollbox" runat="server" visible="true">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Payroll</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">

                            <!-- Large modal : Personal Information-------START------>
                            <%--<div class="modal fade bs-basic-modal-lg" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
								<div class="modal-dialog modal-lg">
									<div class="modal-content">
										<div class="modal-header">
											<h4 class="modal-title" id="myModalLabel2A">Update Basic Information</h4>
											<button type="button" class="close" data-dismiss="modal">
												<span aria-hidden="true">×</span>
											</button>
										</div>
										<div class="modal-body">
											<div class="container-fluid">
												<div class="col-md-12">
													<div class="form-group">
														<div class="col-md-2 form-group">
															<label>First Name :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-2 form-group">
															<asp:TextBox ID="TextBox9" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Middle Name :<span class="text text-danger"></span></label>
														</div>
														<div class="col-md-2 form-group">
															<asp:TextBox ID="TextBox10" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Last Name :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-2 form-group">
															<asp:TextBox ID="TextBox11" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Full Name :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-10 form-group">
															<asp:TextBox ID="TextBox12" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Father Name :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox13" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Mother Name :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox14" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>DOB :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox15" runat="server" class="date-picker form-control form-control-sm rounded" ReadOnly="true" placeholder="yyyy-mm-dd" required="required" onfocus="this.type='date'" onmouseover="this.type='date'" onclick="this.type = 'date'" onblur="this.type='text'" onmouseout="timeFunctionLong(this)"></asp:TextBox>
															<script>
																function timeFunctionLong(txt_DOB) {
																	setTimeout(function () {
																		txt_DOB.type = 'text';
																	}, 60000);
																}
															</script>
														</div>

														<div class="col-md-2 form-group">
															<label>Blood Group :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox16" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Mobile No. :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox17" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
														</div>

														<div class="col-md-2 form-group">
															<label>Alternative Contact :<span class="text text-danger">*</span></label>
														</div>
														<div class="col-md-4 form-group">
															<asp:TextBox ID="TextBox18" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
														</div>

													</div>
												</div>
											</div>
										</div>
										<div class="modal-footer">
											<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
											<button type="button" class="btn btn-primary">Save changes</button>
										</div>
									</div>
								</div>
							</div>--%>
                            <!-- Large modal : Personal Information-------END------>


                        </div>
                        <a class="btn btn-app" data-toggle="modal" data-target=".bs-example-modal-lg"><span class="badge bg-red">N/A</span><i class="fa fa-inr"></i>Overview</a>
                        <a class="btn btn-app" data-toggle="modal" data-target=".bs-example-modal-lg"><span class="badge bg-red">N/A</span><i class="fa fa-bank (alias)"></i>Payroll Bank</a>
                        <a class="btn btn-app" data-toggle="modal" data-target=".bs-example-modal-lg"><span class="badge bg-red">N/A</span><i class="fa fa-edit"></i>Basic Pay Info</a>
                        <a class="btn btn-app" data-toggle="modal" data-target=".bs-example-modal-lg"><span class="badge bg-red">N/A</span><i class="fa fa-edit"></i>Allowances Info</a>
                        <a class="btn btn-app" data-toggle="modal" data-target=".bs-example-modal-lg"><span class="badge bg-red">N/A</span><i class="fa fa-edit"></i>Deductions Info</a>
                    </div>
                </div>

                <div class="col-md-12" id="Security" runat="server" visible="true">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Security</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">

                            <!-- Large modal : Password Change-------START------>
                            <div class="modal fade bs-pass-modal-lg" id="password_modal" data-backdrop="static">
                                <div class="modal-dialog modal-lg">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h4 class="modal-title" id="myModalLabel2A">Change Login Credentails</h4>
                                            <button type="button" class="close" data-dismiss="modal">
                                                <span aria-hidden="true">×</span>
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

                                                        <div class="col-md-2 form-group">
                                                            <label>New Password:<span class="text text-danger">*</span></label>
                                                        </div>
                                                        <div class="col-md-4 form-group">
                                                            <asp:TextBox ID="txt_newpass1" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RFV_P1" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txt_newpass1" InitialValue="" ValidationGroup="ChnagePassword" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="REV_P1" runat="server" ErrorMessage="AlphaNumeric Password Policy" ForeColor="Red" ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$" ControlToValidate="txt_newpass1"></asp:RegularExpressionValidator>
                                                        </div>

                                                        <div class="col-md-2 form-group">
                                                            <label>New Password:<span class="text text-danger">*</span></label>
                                                        </div>
                                                        <div class="col-md-4 form-group">
                                                            <asp:TextBox ID="txt_newpass2" runat="server" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" TextMode="Password"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RFV_P2" runat="server" ErrorMessage="*" ForeColor="Red" InitialValue="" ControlToValidate="txt_newpass2" ValidationGroup="ChnagePassword" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="CV_P1" runat="server" ErrorMessage="CompareValidator" ControlToCompare="txt_newpass1" ForeColor="Red" ControlToValidate="txt_newpass2"></asp:CompareValidator>
                                                        </div>

                                                        <div class="col-md-2 form-group">
                                                            <label>Security Q1:<span class="text text-danger">*</span></label>
                                                        </div>
                                                        <div class="col-md-10 form-group">
                                                            <asp:DropDownList ID="DDL_SQ1" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RFV_P3" runat="server" ErrorMessage="Selection Required" ValidationGroup="ChnagePassword" ControlToValidate="DDL_SQ1" ForeColor="Red" InitialValue="Please Select Option" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        </div>

                                                        <div class="col-md-2 form-group">
                                                            <label>Answer to Q1 :<span class="text text-danger">*</span></label>
                                                        </div>
                                                        <div class="col-md-10 form-group">
                                                            <asp:TextBox ID="txt_SQAns1" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RFV_P4" runat="server" ErrorMessage="Input Required" ControlToValidate="txt_SQAns1" ValidationGroup="ChnagePassword" InitialValue="" SetFocusOnError="true" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>

                                                        <div class="col-md-2 form-group">
                                                            <label>Security Q2 :<span class="text text-danger">*</span></label>
                                                        </div>
                                                        <div class="col-md-10 form-group">
                                                            <asp:DropDownList ID="DDL_SQ2" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RFV_P5" runat="server" ErrorMessage="Selection Required" ControlToValidate="DDL_SQ2" ForeColor="Red" Display="Dynamic" InitialValue="Please Select Option" SetFocusOnError="true" ValidationGroup="ChnagePassword"></asp:RequiredFieldValidator>
                                                        </div>

                                                        <div class="col-md-2 form-group">
                                                            <label>Answer to Q2 :<span class="text text-danger">*</span></label>
                                                        </div>
                                                        <div class="col-md-10 form-group">
                                                            <asp:TextBox ID="txt_SQAns2" class="form-control form-control-sm rounded" Text="N/A" ReadOnly="true" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RFV_P6" runat="server" ErrorMessage="Input Required" ControlToValidate="txt_SQAns2" ValidationGroup="ChnagePassword" InitialValue="" SetFocusOnError="true" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>

                                                        </div>

                                                        <div class="col-md-8 form-group">
                                                            <asp:Label ID="lbl_msgpass" runat="server" Text="Complete the above and Click Save Changes" Font-Bold="true" ForeColor="Black"></asp:Label>
                                                        </div>

                                                        <div class="col-md-4 form-group">
                                                            <asp:Button ID="btn_relogin" CausesValidation="false" runat="server" Text="Re-Login" Enabled="false" CssClass="btn btn-primary btn-sm" OnClick="btn_relogin_Click" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">Close</button>
                                            <asp:Button ID="btn_discardsvpass" runat="server" Text="Discard Changes" CausesValidation="false" CssClass="btn btn-warning btn-sm" OnClick="btn_discardsvpass_Click" />
                                            <asp:Button ID="btn_svpass" runat="server" CssClass="btn btn-success btn-sm" Text="Save Changes" Enabled="false" OnClick="btn_svpass_Click" CausesValidation="true" ValidationGroup="ChnagePassword" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- Large modal : Personal Information-------END------>


                        </div>
                        <a class="btn btn-app" data-toggle="modal" data-target=".bs-example-modal-lg"><span class="badge bg-red">N/A</span><i class="fa fa-key"></i>View Password</a>
                        <a class="btn btn-app" data-toggle="modal" data-target="#password_modal"><span class="badge bg-green">Click</span><i class="fa fa-key"></i>Change Password</a>
                        <a class="btn btn-app" data-toggle="modal" data-target=".bs-example-modal-lg"><span class="badge bg-red">N/A</span><i class="fa fa-edit"></i>Permissions</a>
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

        function ShowPopup1() {
            $("#myModal").modal("show");
        }

        function ShowPasswordModal() {
            $("#password_modal").modal("show");
        }
    </script>
</asp:Content>
