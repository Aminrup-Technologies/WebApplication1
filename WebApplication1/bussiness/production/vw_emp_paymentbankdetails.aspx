<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="vw_emp_paymentbankdetails.aspx.cs" Inherits="WebApplication1.bussiness.production.vw_emp_paymentbankdetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="">
			<div class="page-title">
				<div class="title_left">
					<h5>View Employee Payment Bank Data | <asp:Button ID="btn_exportdata" runat="server" CssClass="btn btn-primary btn-sm" Text="Export" OnClick="ExportExcel"/></h5> 
				</div>

				<div class="title_right">
					<div class="col-md-5 col-sm-5 form-group pull-right top_search"></div>
				</div>

			</div>

			<div class="clearfix"></div>

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

			<div class="row">

				<div class="col-md-12 col-sm-12" id="upperbox" runat="server" visible="false">
					<div class="x_panel">
						<%--<div class="x_title">
							<h2>View Monthly JOBs Summary :: Search Parameters</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>--%>
						<div class="x_content">
							<div class="row" id="selectionrow" runat="server" visible="false">

								<div class="col-md-3 col-sm-12  form-group">
									<label>Employee Work Status <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-12  form-group">
									<asp:DropDownList ID="DDL_EmpWorkStatus" CssClass="form-control form-control-sm rounded" runat="server">
										<asp:ListItem Selected="True">Please Select Option</asp:ListItem>
										<asp:ListItem>All</asp:ListItem>
										<asp:ListItem>Active</asp:ListItem>
										<asp:ListItem>InActive</asp:ListItem>
									</asp:DropDownList>
								</div>


							</div>

							<%--button   start--%>
							<div class="col-md-6 center-margin" id="buttonrow" runat="server" visible="false">
								<div class="ln_solid"></div>
								<div class="item form-group row">
									<div class="col-md-6 col-sm-12">
										<asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to Filter Data!!"></asp:Label>
									</div>
									<div class="col-md-6 col-sm-12">
										<button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
										<button type="reset" class="btn btn-warning btn-sm">Reset</button>
										<asp:Button ID="btn_submit" runat="server" Text="Submit" CssClass="btn btn-success btn-sm"/>
									</div>
								</div>
							</div>
							<%--button   end--%>
						</div>

					</div>
				</div>

				<div class="col-md-12 col-sm-12 small" style="width: 100%; height: 450px; overflow: scroll">
					<asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowEditing="GridView1_RowEditing" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowUpdating="GridView1_RowUpdating">
						<Columns>
							<asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="2%">
								<ItemTemplate>
									<asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="ID" HeaderStyle-Width="5%" Visible="false">
								<ItemTemplate>
									<asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' />
								</ItemTemplate>
								<HeaderStyle CssClass="GridHeaderText-Center" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Status" HeaderStyle-Width="5%">
								<ItemTemplate>
									<asp:Label ID="lbl_WorkStatus" runat="server" Text='<%# Eval("WorkStatus") %>' Visible="true" />
								</ItemTemplate>
								<HeaderStyle CssClass="GridHeaderText-Center" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Workman" HeaderStyle-Width="5%">
								<ItemTemplate>
									<asp:Label ID="lbl_WorkmanSL" runat="server" Text='<%# Eval("WorkmanSL") %>' Visible="true" />
								</ItemTemplate>
								<HeaderStyle CssClass="GridHeaderText-Center" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="16%">
								<ItemTemplate>
									<asp:Label ID="lbl_FullName" runat="server" Text='<%# Eval("FullName") %>' />
								</ItemTemplate>
								<HeaderStyle CssClass="GridHeaderText-Center" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Father Name" HeaderStyle-Width="15%" Visible="false">
								<ItemTemplate>
									<asp:Label ID="lbl_Fathername" runat="server" Font-Size="Smaller" Text='<%# Eval("Fathername") %>' />
								</ItemTemplate>
								<HeaderStyle CssClass="GridHeaderText-Center" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Bank Name" HeaderStyle-Width="20%">
								<ItemTemplate>
									<asp:Label ID="lbl_Payment_Bank" runat="server"  Font-Bold="true" ForeColor="Blue" Text='<%# Eval("Payment_Bank") %>' />
								</ItemTemplate>
								<EditItemTemplate>
									<asp:TextBox ID="txt_Payment_Bank" runat="server" ValidationGroup="BANKDATA" Text='<%# Bind("Payment_Bank") %>' CssClass="form-control form-control-sm rounded" Width="100%"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RFV1" runat="server" ValidationGroup="BANKDATA"  ForeColor="Red" ErrorMessage="RequiredField" Display="Dynamic" ControlToValidate="txt_Payment_Bank"></asp:RequiredFieldValidator>
								</EditItemTemplate>
								<HeaderStyle CssClass="grid" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Bank A/C No" HeaderStyle-Width="13%">
								<ItemTemplate>
									<asp:Label ID="lbl_Payment_Account" runat="server"  Font-Bold="true" ForeColor="Green" Text='<%# Eval("Payment_Account") %>' />
								</ItemTemplate>
								<EditItemTemplate>
									<asp:TextBox ID="txt_Payment_Account" runat="server" Text='<%# Bind("Payment_Account") %>' CssClass="form-control form-control-sm rounded" Width="100%"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RFV2" runat="server" ValidationGroup="BANKDATA" ErrorMessage="RequiredField" Display="Dynamic" ControlToValidate="txt_Payment_Account"></asp:RequiredFieldValidator>
								</EditItemTemplate>
								<HeaderStyle CssClass="grid" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="CNF Bank A/C No" HeaderStyle-Width="13%">
								<ItemTemplate>
									<asp:Label ID="lbl_CNF_Payment_Account" runat="server" Font-Bold="true" ForeColor="Brown" Text='<%# Eval("Payment_Account") %>' />
								</ItemTemplate>
								<EditItemTemplate>
									<asp:TextBox ID="txt_CNF_Payment_Account" runat="server" Text='<%# Bind("Payment_Account") %>' CssClass="form-control form-control-sm rounded" Width="100%"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RFV3" runat="server" ValidationGroup="BANKDATA" ForeColor="Red" ErrorMessage="RequiredField" Display="Dynamic" ControlToValidate="txt_CNF_Payment_Account"></asp:RequiredFieldValidator>
									<asp:CompareValidator ID="CV1" runat="server" ValidationGroup="BANKDATA" ForeColor="Red" ErrorMessage="Enter Same Acc No" Display="Dynamic" ControlToValidate="txt_CNF_Payment_Account" ControlToCompare="txt_Payment_Account"></asp:CompareValidator>
								</EditItemTemplate>
								<HeaderStyle CssClass="grid" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="IFSC Code" HeaderStyle-Width="13%">
								<ItemTemplate>
									<asp:Label ID="lbl_Payment_IFSC" runat="server" Text='<%# Eval("Payment_IFSC") %>' Font-Bold="true" ForeColor="Brown" />
								</ItemTemplate>
								<EditItemTemplate>
									<asp:TextBox ID="txt_Payment_IFSC" runat="server" ValidationGroup="BANKDATA" Text='<%# Bind("Payment_IFSC") %>' CssClass="form-control form-control-sm rounded" Width="100%"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RFV4" runat="server" InitialValue="" ForeColor="Red" ValidationGroup="BANKDATA" ErrorMessage="RequiredField" Display="Dynamic" ControlToValidate="txt_Payment_IFSC"></asp:RequiredFieldValidator>
								</EditItemTemplate>
								<HeaderStyle CssClass="grid" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Branch Name" HeaderStyle-Width="10%">
								<ItemTemplate>
									<asp:Label ID="lbl_BankBranch" runat="server" Font-Bold="true" ForeColor="Black" Text='<%# Eval("BankBranch") %>' />
								</ItemTemplate>
								<EditItemTemplate>
									<asp:TextBox ID="txt_BankBranch" runat="server" Text='<%# Bind("BankBranch") %>' CssClass="form-control form-control-sm rounded" Width="100%"></asp:TextBox>
									<asp:RequiredFieldValidator ID="RFV5" InitialValue="" runat="server" ForeColor="Red" ValidationGroup="BANKDATA" ErrorMessage="RequiredField" Display="Dynamic" ControlToValidate="txt_BankBranch"></asp:RequiredFieldValidator>
								</EditItemTemplate>
								<HeaderStyle CssClass="grid" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Action" HeaderStyle-Width="5%" Visible="true">
								<EditItemTemplate>
									<asp:ImageButton ID="btnupdate" ValidationGroup="BANKDATA" CausesValidation="true" runat="server" CommandName="Update" Height="15px" ImageUrl="~/erp_images/fi-sr-disk.svg" Width="15px" ToolTip="Save" ImageAlign="Middle" />
									<asp:ImageButton ID="Btncancale" runat="server" CommandName="Cancel" Height="15px" ImageUrl="~/erp_images/fi-sr-cross-circle.svg" Width="15px" ToolTip="Cancel Update" ImageAlign="Middle" />
								</EditItemTemplate>
								<ItemTemplate>
									<asp:ImageButton ID="btnedit" runat="server" Visible="true" CommandName="Edit" Height="15px" ImageUrl="~/erp_images/fi-sr-pencil.svg" Width="15px" ToolTip="Update" ImageAlign="Middle" />
									<asp:ImageButton ID="btndelete" runat="server" Visible="false" CommandName="Delete" CommandArgument="<%# Container.DataItemIndex %>" Height="15px" ImageUrl="~/erp_images/fi-sr-trash.svg" Width="15px" ToolTip="Delete" OnClientClick="return confirm('Do you want to DELETE...?')" ImageAlign="Middle" />
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
	</script>
</asp:Content>
