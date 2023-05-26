<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="vw_inchsupmem.aspx.cs" Inherits="WebApplication1.bussiness.production.vw_inchsupmem" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="container">
			<div class="page-title">
				<div class="title_left">
					<h5>Create Manpower Supply Memo's - <asp:Label ID="lbl_month" runat="server"></asp:Label><asp:Label ID="lbl_monthcode" Visible="false" runat="server"></asp:Label>, <asp:Label ID="lbl_year" runat="server"></asp:Label></h5>
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
						<div class="x_content">
							<div class="row">

								<div class="col-12 text-center">
									<div class="btn-group" role="group" aria-label="">
									  <asp:Button ID="btn_prevmonth" runat="server" Text="Prev Month" CssClass="btn btn-success btn-sm" OnClick="btn_prevmonth_Click" />
									  <asp:Button ID="btn_currentdata" runat="server" Text="Current Month" CssClass="btn btn-primary btn-sm" OnClick="btn_currentdata_Click" />
									  <asp:Button ID="btn_nextmonth" runat="server" Text="Next Month" CssClass="btn btn-success btn-sm" OnClick="btn_nextmonth_Click" />
									</div>
								</div>
							</div>
						</div>

					</div>
				</div>
			</div>

			<div class="row">
				<div class="card-box col-md-12 col-sm-12 small" style="width: 100%; height: 450px; overflow: scroll;">
					<asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm table-condensed text-wrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand">
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
									<asp:Label ID="lbl_Creator_Workman" runat="server" Text='<%# Eval("Creator_Workman") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Date" HeaderStyle-Width="5%">
								<ItemTemplate>
									<asp:Label ID="lbl_CreatedDate" runat="server" Text='<%# Eval("CreatedDate","{0:dd-MM-yyyy}") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="JOBID" HeaderStyle-Width="5%">
								<ItemTemplate>
									<asp:Button ID="btn_viewdetails" runat="server" Text='<%# Eval("JOBID") %>' Font-Size="Smaller" CssClass="btn btn-sm btn-info" CommandName="View_Details" CommandArgument="<%# Container.DataItemIndex %>" />
									<asp:Label ID="lbl_JOBID" runat="server" Text='<%# Eval("JOBID") %>' Visible="false" />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Create" HeaderStyle-Width="5%">
								<ItemTemplate>
									<asp:Button ID="btn_createsupmemo" runat="server" Text='SUPPLY MEMO' Font-Size="Smaller" CssClass="btn btn-sm btn-warning" CommandName="CSUPMEM" CommandArgument="<%# Container.DataItemIndex %>" />
									<asp:Label ID="lbl_JOBID_Status" runat="server" Text='<%# Eval("JOBID_Status") %>' Visible="false" />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="JOB Site" HeaderStyle-Width="10%">
								<ItemTemplate>
									<asp:Label ID="lbl_JOB_Site" runat="server" Text='<%# Eval("JOB_Site") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center small" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Approver Name" HeaderStyle-Width="10%">
								<ItemTemplate>
									<asp:Label ID="lbl_JOB_InchargeName" runat="server" Text='<%# Eval("JOB_InchargeName") %>' Font-Bold="true" />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Approval Status" Visible="false">
								<ItemTemplate>
									<asp:Label ID="lbl_Incharge_Approval" runat="server" Text='<%# Eval("Incharge_Approval") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="2%">
								<ItemTemplate>
									<asp:Label ID="lbl_JOB_Shift" runat="server" Text='<%# Eval("JOB_Shift") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="WorkOrder No" HeaderStyle-Width="7%" Visible="false">
								<ItemTemplate>
									<asp:Label ID="lbl_WorkOrderNo" runat="server" Text='<%# Eval("WorkOrderNo") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Permit No" HeaderStyle-Width="5%">
								<ItemTemplate>
									<asp:Label ID="lbl_JOB_PermitNo" runat="server" Font-Bold="true" Text='<%# Eval("JOB_PermitNo") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Approval Status" Visible="false">
								<ItemTemplate>
									<asp:Label ID="lbl_FinalUpldStatus" runat="server" Text='<%# Eval("FinalUpldStatus") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="JOB Title" HeaderStyle-Width="20%">
								<ItemTemplate>
									<asp:Label ID="lbl_JOB_Title" runat="server" Text='<%# Eval("JOB_Title").ToString().Length > 50? (Eval("JOB_Title") as string).Substring(0,50) + " ..." : Eval("JOB_Title")  %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-wrap text-justify" />
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
