<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="add_expense_subheads.aspx.cs" Inherits="WebApplication1.bussiness.production.add_expense_subheads" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="">
			<div class="page-title">
				<div class="title_left">
					<h3>Expense : Heads & Sub-Heads</h3>
				</div>

				<div class="title_right">
					<div class="col-md-5 col-sm-5 form-group pull-right top_search"></div>
				</div>

			</div>

			<div class="clearfix"></div>

			<div class="row">
				<div class="col-md-12 col-sm-12 " id="add_panel" runat="server" visible="false">
					<div class="x_panel">
						<div class="x_title">
							<h2>Add : Expenses Sub-Head</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">
							<div class="row">

								<div class="col-md-2 col-sm-12  form-group">
									<label>Select Expense Head Name <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-2 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_ExpenseHead" CssClass="form-control form-control-sm rounded" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL_ExpenseHead_SelectedIndexChanged"></asp:DropDownList>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_ExpenseHead" SetFocusOnError="true" ValidationGroup="SAVE" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
									<asp:Label ID="lbl_hdslno" runat="server" Text="" Visible="false"></asp:Label>
								</div>


								<div class="col-md-2 col-sm-12  form-group">
									<label>Enter Sub-Head Name <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-2 col-sm-12  form-group">
									<asp:TextBox ID="txt_subhead" CssClass="form-control form-control-sm rounded" placeholder="Enter Sub-Head Name..." runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="SAVE" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_subhead" SetFocusOnError="true"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4">Example : Say Electricity</small>
								</div>



								<div class="col-md-2 col-sm-12  form-group">
									<label>Enter Sub-Head Code <span class="text text-danger">*</span></label>
								</div>

								<div class="col-md-2 col-sm-12  form-group">
									<asp:TextBox ID="txt_subheadcode" CssClass="form-control form-control-sm rounded" placeholder="Enter Sub-Head Short Code..." runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_subheadcode" SetFocusOnError="true"  ValidationGroup="SAVE"></asp:RequiredFieldValidator>
									<small class="form-text text-muted ml-4">Example : Say ELE</small>
								</div>
							</div>

							<div class="col-md-6 center-margin">
								<div class="ln_solid"></div>
								<div class="item form-group row">
									<div class="col-md-6 col-sm-12">
										<asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to Save Data!!"></asp:Label>
									</div>
									<div class="col-md-6 col-sm-12">
										<button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
										<button type="reset" class="btn btn-warning btn-sm">Reset</button>
										<asp:Button ID="btn_submit" runat="server" Text="Submit" CausesValidation="true" ValidationGroup="SAVE" CssClass="btn btn-success btn-sm" Enabled="false" OnClick="btn_submit_Click" />
									</div>
								</div>
							</div>

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

				<div class="col-md-12 col-sm-12  ">
					<div class="x_panel">
						<div class="x_title">
							<h2>View and Manage : Expense Sub - Heads</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
								<%--<li><a class="close-link"><i class="fa fa-close"></i></a></li>--%>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">
							<div class="row">

								<div class="col-md-12 col-sm-12">
									<div class="card-box table-responsive">
										<asp:GridView ID="GridView1" runat="server" CssClass="table table-striped table-hover table-bordered table-responsive table-condensed table-sm" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
											<Columns>
												<asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="sl" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="SlNo" HeaderStyle-Width="5%">
													<ItemTemplate>
														<asp:Label ID="lbl_SlNo" runat="server" Text='<%# Eval("SlNo") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="DBID" Visible="false">
													<ItemTemplate>
														<asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Head" HeaderStyle-Width="15%">
													<ItemTemplate>
														<asp:Label ID="lbl_Head" runat="server" Text='<%# Eval("Head") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="HeadCode" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_HeadCode" runat="server" Text='<%# Eval("HeadCode") %>' />
													</ItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="SubHead" HeaderStyle-Width="30%">
													<ItemTemplate>
														<asp:Label ID="lbl_SubHead" runat="server" Text='<%# Eval("SubHead") %>' />
													</ItemTemplate>
													<EditItemTemplate>
														<asp:TextBox ID="txt_SubHead" class="form-control form-control-sm rounded" Text='<%# Eval("SubHead") %>' Width="100%" runat="server"></asp:TextBox>
														<asp:RequiredFieldValidator ID="RFV_1" runat="server" Display="Dynamic" ErrorMessage="*" ForeColor="Red" ControlToValidate="txt_SubHead" SetFocusOnError="true"></asp:RequiredFieldValidator>
													</EditItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="SubHeadCode" HeaderStyle-Width="10%">
													<ItemTemplate>
														<asp:Label ID="lbl_SubHeadCode" runat="server" Text='<%# Eval("SubHeadCode") %>' />
													</ItemTemplate>
													<EditItemTemplate>
														<asp:TextBox ID="txt_SubHeadCode" class="form-control form-control-sm rounded" Text='<%# Eval("SubHeadCode") %>' Width="100%" runat="server"></asp:TextBox>
														<asp:RequiredFieldValidator ID="RFV_2" runat="server" Display="Dynamic" ErrorMessage="*" ForeColor="Red" ControlToValidate="txt_SubHeadCode" SetFocusOnError="true"></asp:RequiredFieldValidator>
													</EditItemTemplate>
													<HeaderStyle CssClass="GridHeaderText-Center" />
													<ItemStyle CssClass="grid" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Action" HeaderStyle-Width="10%">
													<EditItemTemplate>
														<asp:ImageButton ID="btnupdate" runat="server" CommandName="Update" Height="15px" ImageUrl="~/erp_images/fi-sr-disk.svg" Width="15px" ToolTip="Save" ImageAlign="Middle" />
														&nbsp;
														<asp:ImageButton ID="Btncancale" runat="server" CommandName="Cancel" Height="15px" ImageUrl="~/erp_images/fi-sr-cross-circle.svg" Width="15px" ToolTip="Cancel Update" ImageAlign="Middle" />
													</EditItemTemplate>
													<ItemTemplate>
														<asp:ImageButton ID="btnedit" runat="server" CommandName="Edit" Height="15px" ImageUrl="~/erp_images/fi-sr-pencil.svg" Width="15px" ToolTip="Update" ImageAlign="Middle" />
														&nbsp;
														<asp:ImageButton ID="btndelete" runat="server" CommandName="Delete" Height="15px" ImageUrl="~/erp_images/fi-sr-trash.svg" Width="15px" ToolTip="Delete" OnClientClick="return confirm('Do you want to DELETE...?')" ImageAlign="Middle" />
													</ItemTemplate>
													<ItemStyle CssClass="text text-center" />
												</asp:TemplateField>
											</Columns>
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
			</div>
		</div>
	</div>
	<script type="text/javascript">
		function ShowPopup(title, body) {
			$("#MyPopup .modal-title").html(title);
			$("#MyPopup .modal-body").html(body);
			$("#MyPopup").modal("show");
		}
	</script>
</asp:Content>
