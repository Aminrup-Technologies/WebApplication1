<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="viewupdate_emppayrolldata.aspx.cs" Inherits="WebApplication1.bussiness.production.viewupdate_emppayrolldata" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<style>
		.fixed-header {
			position: fixed;
			top: 0;
			display: none;
			width: 100%;
			z-index: 1;
			background-color: #f2f2f2;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="">
			<div class="page-title">
				<div class="title_left">
					<h5>View & Manage Employee Payroll Inputs ||
						<asp:Button ID="Button1" runat="server" Text="Export" CssClass="btn btn-primary btn-sm" OnClick="ExportExcel" /></h5>
				</div>

				<div class="title_right">
					<div class="col-md-5 col-sm-5 form-group pull-right top_search"></div>
				</div>

			</div>
			<div class="clearfix"></div>

			<div class="row">

				<div class="col-md-12 col-sm-12  ">
					<div class="x_panel">
						<%--<div class="x_title">
							<h2>View Monthly JOBs Summary :: Search Parameters</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>--%>
						<div class="x_content">
							<div class="row">

								<div class="col-md-2 col-sm-12  form-group">
									<label>Employee Work Status <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-2 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_EmpWorkStatus" CssClass="form-control form-control-sm rounded" runat="server">
										<asp:ListItem Selected="True">Please Select Option</asp:ListItem>
										<asp:ListItem>All</asp:ListItem>
										<asp:ListItem>Active</asp:ListItem>
										<asp:ListItem>InActive</asp:ListItem>
									</asp:DropDownList>
								</div>

								<div class="col-md-2 col-sm-12  form-group">
									<label>Skill Category <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-2 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_SkillCategory" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
								</div>

								<div class="col-md-2 col-sm-12  form-group">
									<label>F17 / F29 Dsiplay<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-2 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_Form17YesNo" CssClass="form-control form-control-sm rounded" runat="server">
										<asp:ListItem Selected="True">Please Select Option</asp:ListItem>
										<asp:ListItem>All</asp:ListItem>
										<asp:ListItem>Yes</asp:ListItem>
										<asp:ListItem>No</asp:ListItem>
									</asp:DropDownList>
								</div>


								<div class="col-md-2 col-sm-12  form-group">
									<label>Fixed Salary <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-2 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_FixedYesNo" CssClass="form-control form-control-sm rounded" runat="server">
										<asp:ListItem Selected="True">Please Select Option</asp:ListItem>
										<asp:ListItem>All</asp:ListItem>
										<asp:ListItem>Yes</asp:ListItem>
										<asp:ListItem>No</asp:ListItem>
									</asp:DropDownList>
								</div>


							</div>

							<%--button   start--%>
							<div class="col-md-6 center-margin">
								<div class="ln_solid"></div>
								<div class="item form-group row">
									<div class="col-md-6 col-sm-12">
										<asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to Filter Data!!"></asp:Label>
									</div>
									<div class="col-md-6 col-sm-12">
										<button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
										<button type="reset" class="btn btn-warning btn-sm">Reset</button>
										<asp:Button ID="btn_submit" runat="server" Text="Submit" CssClass="btn btn-success btn-sm" OnClientClick="return ValidateFormField()" OnClick="btn_submit_Click" />
									</div>
								</div>
							</div>
							<%--button   end--%>

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

				<div class="col-md-12 col-sm-12 small" style="width: 100%; height: 450px; overflow: scroll; overflow-x: auto;">
					<asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowEditing="GridView1_RowEditing" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowUpdating="GridView1_RowUpdating">
						<Columns>
							<asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="2%">
								<ItemTemplate>
									<table>
										<tr>
											<td>
												<asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label></td>
										</tr>
									</table>
								</ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="ID" HeaderStyle-Width="5%" Visible="false">
								<ItemTemplate>
									<asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' />
								</ItemTemplate>
								<HeaderStyle CssClass="GridHeaderText-Center" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="15%">
								<ItemTemplate>
									<asp:Label ID="lbl_FullName" runat="server" Text='<%# Eval("FullName") %>' Font-Bold="true" />
									[<asp:Label ID="lbl_WorkmanSL" runat="server" Text='<%# Eval("WorkmanSL") %>' Visible="true" Font-Bold="true" ForeColor="Blue" />]<br />
									[<asp:Label ID="lbl_SkillCategory" runat="server" Text='<%# Eval("SkillCategory") %>' Font-Bold="true" ForeColor="Brown" />]
									[<asp:Label ID="lbl_SkillDesignation" runat="server" Text='<%# Eval("SkillDesignation") %>' Font-Bold="true" ForeColor="DarkBlue" />]<br />
									DOJ:[<asp:Label ID="lbl_DOJ" runat="server" Text='<%# Eval("DOJ", "{0:dd-MM-yyyy}") %>' Font-Bold="true" ForeColor="DarkGreen" />]<br />
                                    Safety No : <asp:Label ID="lbl_SafetyPassNo" runat="server" Text='<%# Eval("SafetyPassNo") %>' Font-Bold="true" ForeColor="DarkGreen" />
								</ItemTemplate>
								<HeaderStyle CssClass="GridHeaderText-Center" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="F16" HeaderStyle-Width="7%">
								<ItemTemplate>
									F16:<asp:Label ID="lbl_F16_YesNo" runat="server" Text='<%# Eval("F16_YesNo") %>' /><br />
                                    F17:<asp:Label ID="lbl_F17_YesNo" runat="server" Text='<%# Eval("F17_YesNo") %>' />
								</ItemTemplate>
								<EditItemTemplate>
									F16:<asp:DropDownList ID="DDL_F16_YesNo" CssClass="form-control form-control-sm rounded" Width="100%" runat="server" SelectedValue='<%# Bind("F16_YesNo") %>'>
										<asp:ListItem Selected="True">Please Select Option</asp:ListItem>
										<asp:ListItem>All</asp:ListItem>
										<asp:ListItem>Yes</asp:ListItem>
										<asp:ListItem>No</asp:ListItem>
									</asp:DropDownList>
                                    <br />
                                    F17<asp:DropDownList ID="DDL_F17_YesNo" CssClass="form-control form-control-sm rounded" Width="100%" runat="server" SelectedValue='<%# Bind("F17_YesNo") %>'>
										<asp:ListItem Selected="True">Please Select Option</asp:ListItem>
										<asp:ListItem>All</asp:ListItem>
										<asp:ListItem>Yes</asp:ListItem>
										<asp:ListItem>No</asp:ListItem>
									</asp:DropDownList>

								</EditItemTemplate>

								<HeaderStyle CssClass="GridHeaderText-Center" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Fixed Y/N" HeaderStyle-Width="6%" Visible="true">
								<ItemTemplate>
									Fixed Y/N:<asp:Label ID="lbl_FixedSalary_YesNo" runat="server" Text='<%# Bind("FixedSalary_YesNo") %>'></asp:Label><br />
                                    Amnt:<asp:Label ID="lbl_FixedAmount" runat="server" Text='<%# Bind("FixedAmount") %>'></asp:Label>
								</ItemTemplate>
								<EditItemTemplate>
									Fixed Y/N:<asp:DropDownList ID="DDL_FixedSalary_YesNo" CssClass="form-control form-control-sm rounded" runat="server" SelectedValue='<%# Bind("FixedSalary_YesNo") %>'>
										<asp:ListItem Selected="True">Please Select Option</asp:ListItem>
										<asp:ListItem>All</asp:ListItem>
										<asp:ListItem>Yes</asp:ListItem>
										<asp:ListItem>No</asp:ListItem>
									</asp:DropDownList><br />
                                    Amnt:<asp:TextBox ID="txt_FixedAmount" runat="server" Text='<%# Bind("FixedAmount") %>' TextMode="MultiLine" Rows="1" Columns="8" CssClass="form-control form-control-sm rounded" Width="100%" MaxLength="5"></asp:TextBox>
									<asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txt_FixedAmount"
										Type="Integer" MinimumValue="0" MaximumValue="99999" ErrorMessage="Value must be between 1 and 99,999." ValidationGroup="Update" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" />
								</EditItemTemplate>
								<HeaderStyle CssClass="grid" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="WH" HeaderStyle-Width="6%" Visible="true">
								<ItemTemplate>
									Hours:<asp:Label ID="lbl_WorkHours" runat="server" Text='<%# Bind("WorkHours") %>'></asp:Label><br />
                                    OT_Div:<asp:Label ID="lbl_OT_Divisibility" runat="server" Text='<%# Bind("OT_Divisibility") %>'></asp:Label>
								</ItemTemplate>
								<EditItemTemplate>
									Hours:<asp:DropDownList ID="DDL_WorkHours" runat="server" CssClass="form-control form-control-sm rounded" SelectedValue='<%# Bind("WorkHours") %>'>
										<asp:ListItem Selected="True">Please Select Option</asp:ListItem>
										<asp:ListItem>All</asp:ListItem>
										<asp:ListItem></asp:ListItem>
										<asp:ListItem Value="8">Eight Hours</asp:ListItem>
										<asp:ListItem Value="12">Twelve Hours</asp:ListItem>
									</asp:DropDownList><br />
                                    OT_Div:<asp:DropDownList ID="DDL_OT_Divisibility" runat="server" CssClass="form-control form-control-sm rounded" SelectedValue='<%# Bind("OT_Divisibility") %>'>
										<asp:ListItem Selected="True">Please Select Option</asp:ListItem>
										<asp:ListItem></asp:ListItem>
										<asp:ListItem Value="">All</asp:ListItem>
										<asp:ListItem Value="8">8 Hours</asp:ListItem>
										<asp:ListItem Value="12">12 Hours</asp:ListItem>
										<asp:ListItem Value="24">24 Hours</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<HeaderStyle CssClass="grid" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="OT F" HeaderStyle-Width="6%" Visible="true">
								<ItemTemplate>
									OT_Factor:<asp:Label ID="lbl_OTFactor" runat="server" Text='<%# Bind("OTFactor") %>'></asp:Label><br />
                                    OT_Mult:<asp:Label ID="lbl_OTMultiplier" runat="server" Text='<%# Bind("OTMultiplier") %>'></asp:Label>
								</ItemTemplate>
								<EditItemTemplate>
									OT_Factor:<asp:DropDownList ID="DDL_OTFactor" runat="server" CssClass="form-control form-control-sm rounded" SelectedValue='<%# Bind("OTFactor") %>'>
										<asp:ListItem Selected="True">Please Select Option</asp:ListItem>
										<asp:ListItem></asp:ListItem>
										<asp:ListItem>All</asp:ListItem>
										<asp:ListItem Value="1">Single</asp:ListItem>
										<asp:ListItem Value="2">Double</asp:ListItem>
										<asp:ListItem Value="0">Zero</asp:ListItem>
									</asp:DropDownList><br />
                                    OT_Mult:<asp:DropDownList ID="DDL_OTMultiplier" runat="server" CssClass="form-control form-control-sm rounded" SelectedValue='<%# Bind("OTMultiplier") %>'>
										<asp:ListItem Selected="True">Please Select Option</asp:ListItem>
										<asp:ListItem></asp:ListItem>
										<asp:ListItem>All</asp:ListItem>
										<asp:ListItem>Basic</asp:ListItem>
										<asp:ListItem>Gross</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<HeaderStyle CssClass="grid" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="DA / VDA" HeaderStyle-Width="2%" Visible="true">
								<ItemTemplate>
									DA/VDA:<asp:Label ID="lbl_DA_VDA" runat="server" Text='<%# Bind("DA_VDA") %>'></asp:Label><br />
                                    Wash_Allow:<asp:Label ID="lbl_Washing_Allowance" runat="server" Text='<%# Bind("Washing_Allowance") %>'></asp:Label>
								</ItemTemplate>
								<EditItemTemplate>
									DA/VDA:<asp:TextBox ID="txt_DA_VDA" runat="server" Text='<%# Bind("DA_VDA") %>' CssClass="form-control form-control-sm rounded" Width="100%"></asp:TextBox><br />
                                    Wash_Allow:<asp:TextBox ID="txt_Washing_Allowance" runat="server" Text='<%# Bind("Washing_Allowance") %>' CssClass="form-control form-control-sm rounded" Width="100%"></asp:TextBox>
								</EditItemTemplate>
								<HeaderStyle CssClass="grid" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="HRA & Conv." HeaderStyle-Width="6%" Visible="true">
								<ItemTemplate>
									HRA:<asp:Label ID="lbl_HRA" runat="server" Text='<%# Bind("HRA") %>'></asp:Label><br />
                                    Conv.<asp:Label ID="lbl_Conv_Allowance" runat="server" Text='<%# Bind("Conv_Allowance") %>'></asp:Label>
								</ItemTemplate>
								<EditItemTemplate>
									HRA:<asp:TextBox ID="txt_HRA" runat="server" Text='<%# Bind("HRA") %>' CssClass="form-control form-control-sm rounded" Width="100%"></asp:TextBox><br />
                                    Conv.<asp:TextBox ID="txt_Conv_Allowance" runat="server" Text='<%# Bind("Conv_Allowance") %>' CssClass="form-control form-control-sm rounded" Width="100%"></asp:TextBox>
								</EditItemTemplate>
								<HeaderStyle CssClass="grid" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Med & Att" HeaderStyle-Width="6%" Visible="true">
								<ItemTemplate>
									Med_Allow:<asp:Label ID="lbl_Medical_Allowance" runat="server" Text='<%# Bind("Medical_Allowance") %>'></asp:Label><br />
                                    Att_Allow:<asp:Label ID="lbl_ATT_Allowance" runat="server" Text='<%# Bind("ATT_Allowance") %>'></asp:Label>
								</ItemTemplate>
								<EditItemTemplate>
									Med_Allow:<asp:TextBox ID="txt_Medical_Allowance" runat="server" Text='<%# Bind("Medical_Allowance") %>' CssClass="form-control form-control-sm rounded" Width="100%"></asp:TextBox><br />
                                    Att_Allow:<asp:TextBox ID="txt_ATT_Allowance" runat="server" Text='<%# Bind("ATT_Allowance") %>' CssClass="form-control form-control-sm rounded" Width="100%"></asp:TextBox>
								</EditItemTemplate>
								<HeaderStyle CssClass="grid" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="SPCL" HeaderStyle-Width="4%" Visible="true">
								<ItemTemplate>
									SPCL_All:<asp:Label ID="lbl_SPCL_Allowance" runat="server" Text='<%# Bind("SPCL_Allowance") %>'></asp:Label><br />
                                    Misc_All:<asp:Label ID="lbl_Misc_Earnings" runat="server" Text='<%# Bind("Misc_Earnings") %>'></asp:Label>
								</ItemTemplate>
								<EditItemTemplate>
									SPCL_All:<asp:TextBox ID="txt_SPCL_Allowance" runat="server" Text='<%# Bind("SPCL_Allowance") %>' CssClass="form-control form-control-sm rounded" Width="100%"></asp:TextBox><br />
                                    Misc_All:<asp:TextBox ID="txt_Misc_Earnings" runat="server" Text='<%# Bind("Misc_Earnings") %>' CssClass="form-control form-control-sm rounded" Width="100%"></asp:TextBox>
								</EditItemTemplate>
								<HeaderStyle CssClass="grid" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Action" HeaderStyle-Width="3%" Visible="true">
								<EditItemTemplate>
									<asp:ImageButton ID="btnupdate" runat="server" CommandName="Update" Height="15px" ImageUrl="~/erp_images/fi-sr-disk.svg" Width="15px" ToolTip="Save" ImageAlign="Middle" />
									<br /><asp:ImageButton ID="Btncancale" runat="server" CommandName="Cancel" Height="15px" ImageUrl="~/erp_images/fi-sr-cross-circle.svg" Width="15px" ToolTip="Cancel Update" ImageAlign="Middle" />
								</EditItemTemplate>
								<ItemTemplate>
									<asp:ImageButton ID="btnedit" runat="server" Visible="true" CommandName="Edit" Height="15px" ImageUrl="~/erp_images/fi-sr-pencil.svg" ValidationGroup="Update" Width="15px" ToolTip="Update" ImageAlign="Middle" />
									<br /><asp:ImageButton ID="btndelete" runat="server" Visible="false" CommandName="Delete" CommandArgument="<%# Container.DataItemIndex %>" Height="15px" ImageUrl="~/erp_images/fi-sr-trash.svg" Width="15px" ToolTip="Delete" OnClientClick="return confirm('Do you want to DELETE...?')" ImageAlign="Middle" />
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

	<script type="text/javascript">
		function ShowPopup(title, body) {
			$("#MyPopup .modal-title").html(title);
			$("#MyPopup .modal-body").html(body);
			$("#MyPopup").modal("show");
		}

		function ShowPopup1() {
			$("#myModal").modal("show");
		}
	</script>

	<script>
		window.onscroll = function () { fixHeader() };

		var header = document.getElementById("GridView1").getElementsByTagName("thead")[0];
		var sticky = header.offsetTop;

		function fixHeader() {
			if (window.pageYOffset > sticky) {
				header.classList.add("fixed-header");
			} else {
				header.classList.remove("fixed-header");
			}
		}
	</script>

</asp:Content>
