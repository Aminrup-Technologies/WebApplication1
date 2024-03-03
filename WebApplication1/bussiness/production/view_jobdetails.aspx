<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="view_jobdetails.aspx.cs" Inherits="WebApplication1.bussiness.production.view_jobdetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript">
		function ShowPopup(title, body) {
			$("#MyPopup .modal-title").html(title);
			$("#MyPopup .modal-body").html(body);
			$("#MyPopup").modal("show");
		}

		function openWhatsApp() {
			// collet the user input
			//var value = $("input[name=message]").val();
			//var value = document.getElementById('<%=txt_permitno.ClientID %>').outerText;
			// JavaScript function to open URL in new window

			var whatsappMessage = $('#' + '<%=HF_Msg.ClientID%>').val();
			//alert(whatsappMessage);

			//var whatsappMessage = "My title" + "\r\n\r\n" + "My description and link"
			whatsappMessage = window.encodeURIComponent(whatsappMessage)
			window.open("whatsapp://send?text=" + whatsappMessage, '_blank');

			//var value = "Hi, How are you..?"+"\r\n"+"Your Permit Number is" + document.getElementById('<%=txt_permitno.ClientID %>').outerText + "";
			//window.open("whatsapp://send?text=" + value, '_blank');
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="container">
			<div class="page-title">
				<div class="title_left">
					<h5>View JOBID Details</h5>
				</div>
			</div>

			<div class="clearfix"></div>

			<div class="row">
				<div class="col-md-12 col-sm-12  ">
					<div class="x_panel">
						<div class="x_title">
							<h2>JOB Basic Details</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">

							<%--form elements div ---- start--%>
							<div class="row">

								<asp:HiddenField ID="HF_Msg" runat="server" />

								<div class="col-md-2 col-sm-12  form-group">
									<label>JOB Title </label>
								</div>
								<div class="col-md-10 col-sm-12  form-group">
									<asp:TextBox ID="txt_jobtitle" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" TextMode="MultiLine" Rows="1" Font-Bold="true" ForeColor="Black"></asp:TextBox>
								</div>


								<div class="col-md-2 col-sm-6  form-group">
									<label>JOB ID </label>
								</div>
								<div class="col-md-2 col-sm-6  form-group">
									<asp:TextBox ID="txt_jobid" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" Font-Bold="true" ForeColor="Blue"></asp:TextBox>
								</div>

								<div class="col-md-2 col-sm-12  form-group">
									<label>JOB Execution Date</label>
								</div>
								<div class="col-md-2 col-sm-12  form-group">
									<asp:TextBox ID="txt_jobdate" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" Font-Bold="true" ForeColor="Blue"></asp:TextBox>
									<asp:Label ID="lbl_jobday" runat="server" Text="" Visible="false"></asp:Label>
								</div>

								<div class="col-md-2 col-sm-12  form-group">
									<label>JOB Execution Day</label>
								</div>
								<div class="col-md-2 col-sm-12  form-group">
									<asp:TextBox ID="txt_jobday" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" Font-Bold="true" ForeColor="Blue"></asp:TextBox>
								</div>

								<div class="col-md-2 col-sm-12  form-group">
									<label>JOB Shift </label>
								</div>
								<div class="col-md-2 col-sm-12  form-group">
									<asp:TextBox ID="txt_jobshift" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" Font-Bold="true" ForeColor="Blue"></asp:TextBox>
								</div>

								<div class="col-md-2 col-sm-12  form-group">
									<label>Work Order No </label>
								</div>
								<div class="col-md-2 col-sm-12  form-group">
									<asp:TextBox ID="txt_workorderno" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" Font-Bold="true" ForeColor="Brown"></asp:TextBox>
								</div>

								<div class="col-md-2 col-sm-12  form-group">
									<label>Work Permit No </label>
								</div>
								<div class="col-md-2 col-sm-12  form-group">
									<asp:TextBox ID="txt_permitno" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" Font-Bold="true" ForeColor="Brown"></asp:TextBox>
								</div>

								<%--<div class="col-md-2 col-sm-12  form-group">
									<label>JOB ID Status </label>
								</div>
								<div class="col-md-2 col-sm-12  form-group">
									<asp:Label ID="lbl_jobidsstatus" runat="server" Text="N/A" Font-Bold="true" ForeColor="Red"></asp:Label>
								</div>--%>

								<div class="col-md-2 col-sm-12  form-group">
									<label>Work Site Name </label>
								</div>
								<div class="col-md-2 col-sm-12  form-group">
									<asp:TextBox ID="txt_worksitename" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" Font-Size="Small"></asp:TextBox>
									<asp:Label ID="lbl_worksitedbcode" runat="server" Text="Label" Visible="false"></asp:Label>
								</div>

								<div class="col-md-2 col-sm-12  form-group">
									<label>JOB Department </label>
								</div>
								<div class="col-md-2 col-sm-12  form-group">
									<asp:TextBox ID="txt_jobdept" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" Font-Size="Small"></asp:TextBox>
								</div>

								<div class="col-md-2 col-sm-12  form-group">
									<label>JOB Work Location </label>
								</div>
								<div class="col-md-2 col-sm-12  form-group">
									<asp:TextBox ID="txt_jobloc" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" Font-Size="Small"></asp:TextBox>
								</div>

								<div class="col-md-2 col-sm-12  form-group">
									<label>JOB Executor </label>
								</div>
								<div class="col-md-2 col-sm-12  form-group">
									<asp:TextBox ID="txt_jobsupv" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" Font-Bold="true" ForeColor="DarkBlue"></asp:TextBox>
									<asp:Label ID="lbl_creatorwrk" runat="server" Text="N/A" Visible="true" Font-Bold="true"></asp:Label>
								</div>


								<div class="col-md-2 col-sm-12  form-group">
									<label>Site In-Charge Name </label>
								</div>
								<div class="col-md-2 col-sm-12  form-group">
									<asp:TextBox ID="txt_inchargename" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" Font-Bold="true" ForeColor="DarkBlue"></asp:TextBox>
									<asp:Label ID="lbl_inchargewrk" runat="server" Text="N/A" Visible="true" Font-Bold="true"></asp:Label>
								</div>

								<%--<div class="col-md-2 col-sm-12  form-group">
									<label>Upload Status </label>
								</div>
								<div class="col-md-2 col-sm-12  form-group">
									<asp:Label ID="lbl_prmtupldstatus" runat="server" Text="" Font-Bold="true"></asp:Label>
								</div>--%>

								<%--<div class="col-md-2 col-sm-12 form-group" id="FileCount_Row1" runat="server" visible="true">
									<asp:Label ID="Label6" runat="server" Text="Total File Count"></asp:Label>
								</div>
								<div class="col-md-2 col-sm-12 form-group" id="FileCount_Row2" runat="server" visible="true">
									<asp:Label ID="lbl_filecount" runat="server" Text="0" Font-Bold="true"></asp:Label>
								</div>

								<div class="col-md-2 col-sm-12 form-group" id="PrmtUpldDate1" runat="server" visible="true">
									<asp:Label ID="Label8" runat="server" Text="Upload Date"></asp:Label>
								</div>
								<div class="col-md-2 col-sm-12 form-group" id="PrmtUpldDate2" runat="server" visible="true">
									<asp:Label ID="lbl_permituploaddate" runat="server" Text="" Font-Bold="true"></asp:Label>
								</div>--%>

								<%--<div class="col-md-2 col-sm-6  form-group">
									<label>Approval Status </label>
								</div>
								<div class="col-md-2 col-sm-6  form-group">
									<asp:Label ID="lbl_approvalstatus" runat="server" Text="" Font-Bold="true"></asp:Label>
								</div>--%>

								<div class="col-md-2 col-sm-12  form-group">
									<label>Approver Remarks </label>
								</div>
								<div class="col-md-2 col-sm-12  form-group">
									<asp:TextBox ID="txt_approverrmrks" class="form-control form-control-sm rounded" runat="server" ReadOnly="true"></asp:TextBox>
								</div>
							</div>
							<%--form elements div ---- end--%>

							<%--form buttons div ---- start--%>
							<div class="col-md-6 center-margin" runat="server" visible="true">
								<div class="ln_solid"></div>
								<div class="row center">
									<div class="col-6">
										<asp:Button ID="btn_update" runat="server" Text="Update" CssClass="btn btn-success btn-sm" Enabled="false" OnClick="btn_update_Click" />
										<asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="btn btn-danger btn-sm" Enabled="false" Visible="false" OnClick="btn_cancel_Click" />
										<asp:Button ID="btn_back" runat="server" Text="Back" CssClass="btn btn-warning btn-sm" Enabled="true" Visible="true" OnClick="btn_back_Click" />
									</div>

									<div class="col-6">
										<asp:Label ID="lbl_msg" runat="server" Text=""></asp:Label>
										<%--<asp:Button ID="btn_delete" runat="server" Text="Delete Request" CssClass="btn btn-danger btn-sm" Enabled="false" Visible="true" OnClick="btn_delete_Click" />--%>
										<!-- create an image icon to open the WhatsApp onclick -->
										<%--<asp:Button runat="server" Text="Export to PDF" ID="btnExport" CssClass="btn btn-primary btn-sm" OnClick="btnExport_Click" />--%>
										<%--<asp:TextBox ID="txtbox1" runat="server" type="text" name="message" visible="false"></asp:TextBox>--%>
										<img src="https://image.freepik.com/free-vector/whatsapp-icon-design_23-2147900927.jpg" height="30" size="30" onclick="openWhatsApp()">
									</div>
								</div>
							</div>
							<%--form buttons div ---- end--%>

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
							<h2>Attached Permit Data</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">
							<div class="row">

								<div class="col-md-12 col-sm-12">
									<div class="card-box table-responsive">
										<p class="text-muted font-13 m-b-30">
											Last attached permit file deleted on :
											<asp:Label ID="lbl_permitdeleteddate" runat="server" Text="__________"></asp:Label>, by
											<asp:Label ID="lbl_permitdeletedby" runat="server" Text="______________"></asp:Label>
										</p>
										<asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowDeleting="GridView1_RowDeleting">
											<Columns>
												<asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="DB ID" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="JOBID" Visible="false" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_JOBID" runat="server" Text='<%# Bind("JOBID") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="File Name" HeaderStyle-Width="40%">
													<ItemTemplate>
														<asp:Label ID="lbl_Name" runat="server" Text='<%# Eval("Name") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="View" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("Id") %>' OnClick="DownloadFile" ToolTip="Click to Download" CommandName="Download"><span class="glyphicon glyphicon-save" aria-hidden='true'></span></asp:LinkButton>
														<asp:Label ID="lbl_dbid" runat="server" Visible="false" Text='<%# Bind("Id") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Time" HeaderStyle-Width="20%">
													<ItemTemplate>
														<asp:Label ID="lbl_TimeStamp" runat="server" Text='<%# Eval("TimeStamp") %>' />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Delete" HeaderStyle-Width="5%" Visible="false">
													<ItemTemplate>
														<asp:ImageButton ID="btndelete" runat="server" CommandName="Delete" Height="15px" ImageUrl="~/erp_images/fi-sr-trash.svg" Width="15px" ToolTip="Delete" OnClientClick="return confirm('Do you want to DELETE...?')" ImageAlign="Middle" />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

											</Columns>
											<HeaderStyle CssClass="text text-center" />
											<EmptyDataTemplate>
												<div class="grid">No Data Found</div>
											</EmptyDataTemplate>
										</asp:GridView>

									</div>
								</div>
							</div>
						</div>
					</div>
				</div>


				<div class="col-md-12 col-sm-12">
					<div class="x_panel">
						<div class="x_title">
							<h2>Attached Manpower Data & Details</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
								<%--<li><a class="close-link"><i class="fa fa-close"></i></a></li>--%>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">
							<div class="row">

								<div class="col-md-12 col-sm-12 small">
									<div class="card-box table-responsive">
										<asp:GridView ID="GridView2" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowCancelingEdit="GridView2_RowCancelingEdit" OnRowDeleting="GridView2_RowDeleting" OnRowEditing="GridView2_RowEditing" OnRowDataBound="GridView2_RowDataBound" OnRowUpdating="GridView2_RowUpdating">
											<Columns>
												<asp:TemplateField HeaderText="Sl" Visible="True" HeaderStyle-Width="2%">
													<ItemTemplate>
														<asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="ID" HeaderStyle-Width="10%" Visible="false">
													<ItemTemplate>
														<asp:Label ID="lbl_Id" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="JOBID" Visible="true" HeaderStyle-Width="8%">
													<ItemTemplate>
														<asp:Label ID="lbl_JOBID" runat="server" Text='<%# Bind("JOBID") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="JOB Date" Visible="true" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_CreatedDate" runat="server" Text='<%# Bind("CreatedDate","{0:dd-MM-yyyy}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="WRK" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_EmployeeWrk" runat="server" Text='<%# Bind("EmployeeWrk") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="20%">
													<ItemTemplate>
														<asp:Label ID="lbl_EmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-left" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="H" HeaderStyle-Width="1%" Visible="false">
													<ItemTemplate>
														<asp:Label ID="lbl_WourkHours" runat="server" Text='<%# Bind("WourkHours") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="IN Punch Time" HeaderStyle-Width="20%">
													<ItemTemplate>
														<asp:Label ID="lbl_Inpunch_Time" runat="server" Text='<%# Bind("Inpunch_Time","{0:dd-MM-yyyy hh:mm:ss tt}") %>'></asp:Label>
													</ItemTemplate>
													<EditItemTemplate>
														<asp:TextBox ID="txt_Inpunch_Time" runat="server" class="form-control form-control-sm rounded small" Text='<%# DataBinder.Eval(Container.DataItem,"Inpunch_Time", "{0:yyyy-MM-dd hh:mm:ss tt}") %> ' Width="100%"></asp:TextBox>
													</EditItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="OUT Punch Time" HeaderStyle-Width="20%">
													<ItemTemplate>
														<asp:Label ID="lbl_Outpunch_Time" runat="server" Text='<%# Bind("Outpunch_Time","{0:dd-MM-yyyy hh:mm:ss tt}") %>'></asp:Label>
													</ItemTemplate>
													<EditItemTemplate>
														<asp:TextBox ID="txt_Outpunch_Time" runat="server" class="form-control form-control-sm rounded small" Text='<%# DataBinder.Eval(Container.DataItem,"Outpunch_Time", "{0:yyyy-MM-dd hh:mm:ss tt}") %> ' Width="100%"></asp:TextBox>
													</EditItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Lunch" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_LunchFactor" runat="server" Text='<%# Bind("LunchFactor") %>'></asp:Label>
													</ItemTemplate>
													<EditItemTemplate>
														<asp:DropDownList ID="DDL_LunchYesNo" runat="server" class="form-control form-control-sm rounded" SelectedValue='<%# Bind("LunchFactor") %>'>
															<asp:ListItem Text="Yes" Value="Yes">Yes</asp:ListItem>
															<asp:ListItem Text="No" Value="No">No</asp:ListItem>
														</asp:DropDownList>
													</EditItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="POT" HeaderStyle-Width="6%">
													<ItemTemplate>
														<asp:Label ID="lbl_ProvidedOT" runat="server" Text='<%# Bind("ProvidedOT") %>'></asp:Label>
													</ItemTemplate>
													<EditItemTemplate>
														<asp:TextBox ID="txt_ProvidedOT" runat="server" class="form-control form-control-sm rounded" Text='<%# DataBinder.Eval(Container.DataItem,"ProvidedOT") %>' Width="100%"></asp:TextBox>
														<asp:CustomValidator ID="CustomValidator2" runat="server" ValidationGroup="Update" ControlToValidate="txt_ProvidedOT" Display="Dynamic" ErrorMessage="Value must be less than or equal to 16" ForeColor="IndianRed" ClientValidationFunction="validateProvidedOT"></asp:CustomValidator>
													</EditItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>


												<asp:TemplateField HeaderText="Atten Status" HeaderStyle-Width="3%" Visible="false">
													<ItemTemplate>
														<asp:Label ID="lbl_AttendanceStatus" runat="server" Text='<%# Bind("AttendanceStatus") %>'></asp:Label>
													</ItemTemplate>
													<EditItemTemplate>
														<asp:DropDownList ID="DDL_AttendanceStatus" class="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
													</EditItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Code" HeaderStyle-Width="3%">
													<ItemTemplate>
														<asp:Label ID="lbl_AttendanceCode" runat="server" Text='<%# Bind("AttendanceCode") %>'></asp:Label>
													</ItemTemplate>
													<EditItemTemplate>
														<asp:DropDownList ID="DDL_AttendanceCode" class="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
													</EditItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Action" HeaderStyle-Width="12%" Visible="false">
													<EditItemTemplate>
														<asp:ImageButton ID="btnupdate" runat="server" CommandName="Update" Height="15px" ImageUrl="~/erp_images/fi-sr-disk.svg" Width="15px" ToolTip="Save" ImageAlign="Middle" CausesValidation="true" ValidationGroup="Update" />
														<asp:ImageButton ID="Btncancale" runat="server" CommandName="Cancel" Height="15px" ImageUrl="~/erp_images/fi-sr-cross-circle.svg" Width="15px" ToolTip="Cancel Update" CausesValidation="false" ImageAlign="Middle" />
													</EditItemTemplate>
													<ItemTemplate>
														<asp:ImageButton ID="btnedit" runat="server" Visible="true" CommandName="Edit" Height="15px" ImageUrl="~/erp_images/fi-sr-pencil.svg" Width="15px" ToolTip="Update" ImageAlign="Middle" />
														<asp:ImageButton ID="btndelete" runat="server" Enabled="false" CommandName="Delete" CommandArgument="<%# Container.DataItemIndex %>" Height="15px" ImageUrl="~/erp_images/fi-sr-trash.svg" Width="15px" ToolTip="Delete" OnClientClick="return confirm('Do you want to DELETE...?')" ImageAlign="Middle" />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>
											</Columns>
											<HeaderStyle CssClass="text text-center" />
											<EmptyDataTemplate>
												<div class="grid">No Data Found</div>
											</EmptyDataTemplate>
										</asp:GridView>

									</div>
								</div>

								<%--form buttons div ---- start--%>
								<div class="col-md-6 center-margin" runat="server" id="attachmanpowerrow" visible="false">
									<div class="ln_solid"></div>
									<div class="item form-group row">
										<div class="col-md-6 col-sm-12">
											<asp:Button ID="btn_attachmanpower" runat="server" Text="Attach Manpower" CssClass="btn btn-success btn-sm" Enabled="true" OnClick="btn_attachmanpower_Click" />
										</div>

										<div class="col-md-6 col-sm-12">
											<asp:Label ID="Label1" runat="server" Text="">Click to Attach Manpower</asp:Label>
										</div>
									</div>
								</div>
								<%--form buttons div ---- end--%>
							</div>
						</div>
					</div>
				</div>

			</div>
		</div>
	</div>

	<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

	<script type="text/javascript">
		function ShowPopup(title, body) {
			$("#MyPopup .modal-title").html(title);
			$("#MyPopup .modal-body").html(body);
			$("#MyPopup").modal("show");
		}
	</script>

	<script>
		function validateProvidedOT(sender, args) {
			var textBox = $("#" + sender.controltovalidate);
			var inputValue = textBox.val();
			if (inputValue.trim() !== '') {
				var numericValue = parseInt(inputValue);
				if (isNaN(numericValue) || numericValue > 16) {
					args.IsValid = false;
				} else {
					args.IsValid = true;
				}
			} else {
				args.IsValid = false;
			}
		}
	</script>
</asp:Content>
