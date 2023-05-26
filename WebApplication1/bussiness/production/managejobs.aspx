<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="managejobs.aspx.cs" Inherits="WebApplication1.bussiness.production.managejobs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DbConn %>" SelectCommand="SELECT [CreatedDate], [Id], [JOBID], [JOBID_Status], [JOB_Site], [JOB_InchargeName], [JOB_Shift], [JOB_Title], [Incharge_Approval] FROM [tbl_jobs] ORDER BY [CreatedDate] DESC"></asp:SqlDataSource>
	<div class="right_col" role="main">
		<div class="">
			<div class="page-title">
				<div class="title_left">
					<h3>View Created JOB ID</h3>
				</div>

				<div class="title_right">
					<div class="col-md-5 col-sm-5 form-group pull-right top_search"></div>
				</div>

			</div>

			<div class="clearfix"></div>

			<div class="row">
				<div class="col-md-12 col-sm-12">
					<div class="x_panel">
						<div class="x_title">
							<h2>View and Manage : JOB's</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>
						<div class="x_content">
							<div class="row">

								<div class="col-md-12 col-sm-12">
									<div class="card-box table-responsive small">
										<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataSourceID="SqlDataSource1">
											<Columns>
												<asp:TemplateField HeaderText="Sl No"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate></asp:TemplateField>
												<asp:BoundField DataField="Id" HeaderText="DB ID" ReadOnly="True" SortExpression="Id" Visible="False" />
												<asp:BoundField DataField="CreatedDate" DataFormatString="{0:d}" HeaderText="JOB Date" SortExpression="CreatedDate" />
												<asp:BoundField DataField="JOBID" HeaderText="JOB ID" SortExpression="JOBID" />
												<asp:BoundField DataField="JOBID_Status" HeaderText="JOBID Status" SortExpression="JOBID_Status" />
												<asp:BoundField DataField="JOB_Site" HeaderText="JOB Site" SortExpression="JOB_Site" />
												<asp:BoundField DataField="JOB_InchargeName" HeaderText="Incharge Name" SortExpression="JOB_InchargeName" />
												<asp:BoundField DataField="JOB_Shift" HeaderText="JOB_Shift" SortExpression="JOB_Shift" />
												<asp:BoundField DataField="JOB_Title" HeaderText="JOB_Title" SortExpression="JOB_Title" />
												<asp:BoundField DataField="Incharge_Approval" HeaderText="Incharge_Approval" SortExpression="Incharge_Approval" />
											</Columns>
											<FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
											<HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
											<PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
											<RowStyle BackColor="White" ForeColor="#003399" />
											<SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
											<SortedAscendingCellStyle BackColor="#EDF6F6" />
											<SortedAscendingHeaderStyle BackColor="#0D4AC4" />
											<SortedDescendingCellStyle BackColor="#D6DFDF" />
											<SortedDescendingHeaderStyle BackColor="#002876" />
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
