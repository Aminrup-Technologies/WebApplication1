<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="pyrl_deductionlist.aspx.cs" Inherits="WebApplication1.bussiness.production.pyrl_deductionlist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
		<div class="">
			<div class="page-title">
				<div class="title_left">
					<h5>View Active Deductions</h5>
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
				<div class="card-box col-md-12 col-sm-12" style="width: 100%; height: 450px; overflow: scroll;">
					<asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found">
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

							<asp:TemplateField HeaderText="DBID" Visible="true" HeaderStyle-Width="10%">
								<ItemTemplate>
									<asp:Label ID="lbl_WorkmanSL" runat="server" Text='<%# Eval("WorkmanSL") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<%--<asp:TemplateField HeaderText="JOBID" HeaderStyle-Width="5%">
								<ItemTemplate>
									<asp:Button ID="btn_viewdetails" runat="server" Text='<%# Eval("JOBID") %>' Font-Size="Smaller" CssClass="btn btn-sm btn-info" CommandName="View_Details" CommandArgument="<%# Container.DataItemIndex %>" />
									<asp:Label ID="lbl_JOBID" runat="server" Text='<%# Eval("JOBID") %>' Visible="false" />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Status" HeaderStyle-Width="5%">
								<ItemTemplate>
									<asp:Button ID="btn_jobidstatus" runat="server" Text='<%# Eval("JOBID_Status") %>' Font-Size="Smaller" CssClass="btn btn-sm btn-info" CommandName="Swap_JOBIDStatus" CommandArgument="<%# Container.DataItemIndex %>" />
									<asp:Label ID="lbl_JOBID_Status" runat="server" Text='<%# Eval("JOBID_Status") %>' Visible="false" />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>--%>

							<asp:TemplateField HeaderText="FullName" HeaderStyle-Width="15%">
								<ItemTemplate>
									<asp:Label ID="lbl_FullName" runat="server" Text='<%# Eval("FullName") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center small" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Advance" HeaderStyle-Width="8%">
								<ItemTemplate>
									<asp:Label ID="lbl_Advance" runat="server" Font-Bold="true" Text='<%# Eval("Advance") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center"  />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Rem_Advance" Visible="true">
								<ItemTemplate>
									<asp:Label ID="lbl_Rem_Advance" runat="server"  Text='<%# Eval("Rem_Advance") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Cur_Advance" HeaderStyle-Width="3%">
								<ItemTemplate>
									<asp:Label ID="lbl_Cur_Advance" runat="server" Text='<%# Eval("Cur_Advance") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" BackColor="LightPink" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Fines" HeaderStyle-Width="7%">
								<ItemTemplate>
									<asp:Label ID="lbl_Fines" runat="server" Text='<%# Eval("Fines") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Rem_Fines" HeaderStyle-Width="5%">
								<ItemTemplate>
									<asp:Label ID="lbl_Rem_Fines" runat="server" Font-Bold="true" Text='<%# Eval("Rem_Fines") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Cur_Fines" Visible="true">
								<ItemTemplate>
									<asp:Label ID="lbl_Cur_Fines" runat="server" Text='<%# Eval("Cur_Fines") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center"  BackColor="LightPink" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Others" HeaderStyle-Width="7%" Visible="true">
								<ItemTemplate>
									<asp:Label ID="lbl_Others" runat="server" Text='<%# Eval("Others") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Rem_Others" HeaderStyle-Width="5%">
								<ItemTemplate>
									<asp:Label ID="lbl_Rem_Others" runat="server" Font-Bold="true" Text='<%# Eval("Rem_Others") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Cur_Others" Visible="true">
								<ItemTemplate>
									<asp:Label ID="lbl_Cur_Others" runat="server" Text='<%# Eval("Cur_Others") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" BackColor="LightPink" />
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
