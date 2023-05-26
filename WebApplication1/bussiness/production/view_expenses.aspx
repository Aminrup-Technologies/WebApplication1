<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="view_expenses.aspx.cs" Inherits="WebApplication1.bussiness.production.view_expenses" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="">
			<div class="page-title">
				<div class="title_left">
					<h5>View Claim Expenses - <asp:Label ID="lbl_month" runat="server"></asp:Label><asp:Label ID="lbl_monthcode" Visible="false" runat="server"></asp:Label>, <asp:Label ID="lbl_year" runat="server"></asp:Label></h5>
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
				<div class="col-md-12 col-sm-12  ">
					<div class="x_panel">
						<%--<div class="x_title">
							<h2>Filter Options</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>--%>
						<div class="x_content">
							<div class="row">

								<div class="col-12 text-center">
									<div class="btn-group" role="group" aria-label="">
									  <asp:Button ID="btn_prevmonth" runat="server" Text="Prev Month" CssClass="btn btn-success btn-sm" OnClick="btn_prevmonth_Click" />
									  <asp:Button ID="btn_currentdata" runat="server" Text="Current Month" CssClass="btn btn-primary btn-sm" OnClick="btn_currentdata_Click" />
									  <asp:Button ID="btn_nextmonth" runat="server" Text="Next Month" CssClass="btn btn-success btn-sm" OnClick="btn_nextmonth_Click" />
									</div>
								</div>

								<div class="col-12">
									<hr />
								</div>


								<div class="col-md-3 col-sm-6 col-xs-6 form-group">
									<label>Search By Locations<span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-6 col-xs-6 form-group">
									<asp:DropDownList ID="DDL_Location" CssClass="form-control form-control-sm rounded" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL_Location_SelectedIndexChanged" ></asp:DropDownList>
								</div>

							</div>

							<%--button   start--%>
							<div class="col-md-6 center-margin">
								<div class="ln_solid"></div>
								<div class="item form-group row">
									<div class="col-md-6 col-sm-12">
										<asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to Save Data!!"></asp:Label>
									</div>
									<div class="col-md-6 col-sm-12">
										<button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
										<asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="btn btn-warning btn-sm" OnClick="btn_reset_Click"/>
										<asp:Button ID="btn_submit" runat="server" Text="Submit" CssClass="btn btn-success btn-sm" OnClick="btn_submit_Click"/>
									</div>
								</div>
							</div>
							<%--button   end--%>
						</div>

					</div>
				</div>
			</div>

			<div class="row">
				<div class="card-box col-md-12 col-sm-12 small" style="width: 100%; height: 450px; overflow: scroll;">
					<asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowCommand="GridView1_RowCommand" OnRowDeleting="GridView1_RowDeleting">
						<Columns>
							<asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="2%">
								<ItemTemplate>
									<asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="DBID" Visible="false">
								<ItemTemplate>
									<asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="DBID" Visible="false">
								<ItemTemplate>
									<asp:Label ID="lbl_LoggedByWrk" runat="server" Text='<%# Eval("LoggedByWrk") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Date" HeaderStyle-Width="5%">
								<ItemTemplate>
									<asp:Label ID="lbl_LoggedOn" runat="server" Text='<%# Eval("LoggedOn","{0:dd-MM-yyyy}") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="DB ID" HeaderStyle-Width="5%">
								<ItemTemplate>
									<asp:Button ID="btn_viewdetails" runat="server" Text='<%# Eval("ExpenseID") %>' Font-Size="Smaller" CssClass="btn btn-sm btn-info" CommandName="View_Details" CommandArgument="<%# Container.DataItemIndex %>" />
									<asp:Label ID="lbl_ExpenseID" runat="server" Text='<%# Eval("ExpenseID") %>' Visible="false" />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Work Region" HeaderStyle-Width="3%"  Visible="false">
								<ItemTemplate>
									<asp:Label ID="lbl_RegionCode" runat="server" Text='<%# Eval("RegionCode") %>' Visible="true" />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							 <asp:TemplateField HeaderText="Company" HeaderStyle-Width="5%" Visible="false">
								<ItemTemplate>
									<asp:Label ID="lbl_CompanyCode" runat="server" Text='<%# Eval("CompanyCode") %>' Visible="true" />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Location" HeaderStyle-Width="5%">
								<ItemTemplate>
									<asp:Label ID="lbl_Location" runat="server" Text='<%# Eval("Location") %>' Visible="true" />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Workorder No" HeaderStyle-Width="5%">
								<ItemTemplate>
									<asp:Label ID="lbl_WorkorderNo" runat="server" Text='<%# Eval("WorkorderNo") %>' Visible="true" />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Worksite" HeaderStyle-Width="5%" Visible="false">
								<ItemTemplate>
									<asp:Label ID="lbl_WorksiteName" runat="server" Text='<%# Eval("WorksiteName") %>' Visible="true" />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Status" HeaderStyle-Width="5%">
								<ItemTemplate>
									<asp:Label ID="lbl_AppStatus" runat="server" Text='<%# Eval("AppStatus") %>' Font-Bold="true" ForeColor="Red" Visible="true" />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Approver Name" HeaderStyle-Width="10%">
								<ItemTemplate>
									<asp:Label ID="lbl_AppByName" runat="server" Text='<%# Eval("AppByName") %>' Visible="true" />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Action" HeaderStyle-Width="3%" Visible="true">
								<ItemTemplate>
									<asp:ImageButton ID="btndelete" runat="server" CommandName="Delete" CommandArgument="<%# Container.DataItemIndex %>" Height="15px" ImageUrl="~/erp_images/fi-sr-trash.svg" Width="15px" ToolTip="Delete" OnClientClick="return confirm('Do you want to DELETE...?')" ImageAlign="Middle" />
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
	<script type="text/javascript">
		function ShowPopup(title, body) {
			$("#MyPopup .modal-title").html(title);
			$("#MyPopup .modal-body").html(body);
			$("#MyPopup").modal("show");
		}
	</script>
</asp:Content>
