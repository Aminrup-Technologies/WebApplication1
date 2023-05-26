<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="vw_tbt_masterapproval.aspx.cs" Inherits="WebApplication1.bussiness.production.vw_tbt_masterapproval" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="right_col" role="main">
		<div class="">
			<div class="page-title">
				<div class="title_left">
					<h5>View & Approve TBT: Safety Officer <asp:Label ID="lbl_month" runat="server"></asp:Label><asp:Label ID="lbl_monthcode" Visible="false" runat="server"></asp:Label>, <asp:Label ID="lbl_year" runat="server"></asp:Label></h5>
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
			<!-- Small Modal - END---->

			<div class="row">
				<div class="col-md-12 col-sm-12  ">
					<div class="x_panel">
						<div class="x_title">
							<%--<h2>Search View Parameters</h2>--%>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>
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
									<label>By Department <span class="text text-danger">*</span></label>
								</div>
								<div class="col-md-3 col-sm-6 col-xs-6 form-group">
									<asp:DropDownList ID="DDL_TBTDept" CssClass="form-control form-control-sm rounded" runat="server">
									</asp:DropDownList>
								</div>

								<div class="col-md-3 col-sm-6 col-xs-6 form-group">
									<label>By Status</label>
								</div>
								<div class="col-md-3 col-sm-6 col-xs-6 form-group">
									<asp:DropDownList ID="DDL_TBTStatus" runat="server" CssClass="form-control form-control-sm rounded">
										<asp:ListItem Selected="True">--Select--</asp:ListItem>
										<asp:ListItem Value="0">All</asp:ListItem>
										<asp:ListItem Value="1">TBT-Complete</asp:ListItem>
										<asp:ListItem Value="2">TBT-Pending</asp:ListItem>
										<asp:ListItem Value="3">Photo-Pending</asp:ListItem>
										<asp:ListItem Value="4">Photo-Uploaded</asp:ListItem>
										<asp:ListItem Value="5">Approval-Pending</asp:ListItem>
										<asp:ListItem Value="6">Approval-Done</asp:ListItem>
										<asp:ListItem Value="7">Approval-Returned</asp:ListItem>
										<asp:ListItem Value="8">Approval-Rejected</asp:ListItem>
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
					<asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand">
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

							<asp:TemplateField HeaderText="TBT Date" HeaderStyle-Width="7%">
								<ItemTemplate>
									<asp:Label ID="lbl_TBT_Date" runat="server" Text='<%# Eval("TBT_Date","{0:dd-MM-yyyy}") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="TBT ID" HeaderStyle-Width="5%">
								<ItemTemplate>
									<asp:Button ID="btn_viewtbtdetails" runat="server" Text='<%# Eval("TBT_ID") %>' Font-Size="Smaller" CssClass="btn btn-sm btn-info" CommandName="View_TBTDetails" CommandArgument="<%# Container.DataItemIndex %>" />
									<asp:Label ID="lbl_TBT_ID" runat="server" Text='<%# Eval("TBT_ID") %>' Visible="false" />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="JOBID" HeaderStyle-Width="5%">
								<ItemTemplate>
									<asp:Button ID="btn_viewjobdetails" runat="server" Text='<%# Eval("Ref_JOBID") %>' Font-Size="Smaller" CssClass="btn btn-sm btn-primary" CommandName="View_JOBDetails" CommandArgument="<%# Container.DataItemIndex %>" />
									<asp:Label ID="lbl_Ref_JOBID" runat="server" Text='<%# Eval("Ref_JOBID") %>' Visible="false" />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Supervisor / Submitter Name" HeaderStyle-Width="10%">
								<ItemTemplate>
									<asp:Label ID="lbl_TBT_SupvName" runat="server" Text='<%# Eval("TBT_SupvName") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Site-Incharge Name" HeaderStyle-Width="10%">
								<ItemTemplate>
									<asp:Label ID="lbl_AreaInchargeName" runat="server" Text='<%# Eval("AreaInchargeName") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="DBID" Visible="false">
								<ItemTemplate>
									<asp:Label ID="lbl_TBT_SupvWrk" runat="server" Text='<%# Eval("TBT_SupvWrk") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="TBT Department" HeaderStyle-Width="8%">
								<ItemTemplate>
									<asp:Label ID="lbl_TBT_Dept" runat="server" Text='<%# Eval("TBT_Dept") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center small" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="TBT Location" HeaderStyle-Width="8%">
								<ItemTemplate>
									<asp:Label ID="lbl_TBT_Location" runat="server" Text='<%# Eval("TBT_Location") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center small" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Approval Status" Visible="false">
								<ItemTemplate>
									<asp:Label ID="lbl_SafetySupvApprovalStatus" runat="server" Text='<%# Eval("SafetySupvApprovalStatus") %>' />
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Delete" HeaderStyle-Width="3%" Visible="false">
								<EditItemTemplate>
									<asp:ImageButton ID="btnupdate" runat="server" CommandName="Update" Height="15px" ImageUrl="~/erp_images/fi-sr-disk.svg" Width="15px" ToolTip="Save" ImageAlign="Middle" />
									&nbsp;
									<asp:ImageButton ID="Btncancale" runat="server" CommandName="Cancel" Height="15px" ImageUrl="~/erp_images/fi-sr-cross-circle.svg" Width="15px" ToolTip="Cancel Update" ImageAlign="Middle" />
								</EditItemTemplate>
								<ItemTemplate>
									<asp:ImageButton ID="btnedit" runat="server" Visible="false" CommandName="Edit" Height="15px" ImageUrl="~/erp_images/fi-sr-pencil.svg" Width="15px" ToolTip="Update" ImageAlign="Middle" />
									&nbsp;
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
