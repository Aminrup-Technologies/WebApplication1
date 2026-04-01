<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="manage_jobid_v2.aspx.cs" Inherits="WebApplication1.bussiness.production.manage_jobid_v2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .top-label {
            font-weight: 600;
            color: #333;
        }

        .grid-action-btn {
            padding: 2px 8px;
            font-size: 12px;
            margin: 2px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <div class="page-title">
                        <div class="title_left">
                            <h5><i class="fa fa-list-alt text-primary"></i>View Created JOB ID -
                               
                                <asp:Label ID="lbl_month" runat="server" CssClass="text-primary"></asp:Label>
                                <asp:Label ID="lbl_monthcode" Visible="false" runat="server"></asp:Label>,
                               
                                <asp:Label ID="lbl_year" runat="server" CssClass="text-primary"></asp:Label>
                            </h5>
                        </div>
                    </div>
                    <div class="clearfix"></div>

                    <div class="row">
                        <div class="col-md-12 col-sm-12">
                            <div class="x_panel" style="border-radius: 8px; box-shadow: 0 2px 5px rgba(0,0,0,0.05); border: 1px solid #e0e6ed;">

                                <div class="x_title border-bottom-0 pb-0">
                                    <h2 class="text-primary font-weight-bold"><i class="fa fa-filter"></i>Search & Filter Records</h2>
                                    <ul class="nav navbar-right panel_toolbox">
                                        <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                                    </ul>
                                    <div class="clearfix"></div>
                                </div>

                                <div class="x_content">
                                    <div class="row mb-4">
                                        <div class="col-12 d-flex justify-content-center">
                                            <div class="btn-group shadow-sm" role="group" aria-label="Month Navigation">
                                                <asp:LinkButton ID="btn_prevmonth" runat="server" CssClass="btn btn-outline-secondary btn-sm" OnClick="btn_prevmonth_Click">
                                <i class="fa fa-chevron-left mr-1"></i> Prev Month
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btn_currentdata" runat="server" CssClass="btn btn-primary btn-sm font-weight-bold px-3" OnClick="btn_currentdata_Click">
                                <i class="fa fa-calendar mr-1"></i> Current Month
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btn_nextmonth" runat="server" CssClass="btn btn-outline-secondary btn-sm" OnClick="btn_nextmonth_Click">
                                Next Month <i class="fa fa-chevron-right ml-1"></i>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row align-items-end" style="background-color: #f8f9fa; padding: 15px; border-radius: 6px; border: 1px solid #e9ecef;">

                                        <div class="col-md-3 col-sm-6 form-group mb-md-0 mb-3">
                                            <label class="text-muted small font-weight-bold text-uppercase mb-1"><i class="fa fa-info-circle"></i>JOB Status</label>
                                            <asp:DropDownList ID="DDL_JobStatus" CssClass="form-control form-control-sm rounded custom-select" runat="server">
                                                <asp:ListItem Value="0" Selected="True">-- All Statuses --</asp:ListItem>
                                                <asp:ListItem Value="1">JOBID - Active</asp:ListItem>
                                                <asp:ListItem Value="2">JOBID - Blocked</asp:ListItem>
                                                <asp:ListItem Value="3">Permit - Pending</asp:ListItem>
                                                <asp:ListItem Value="4">Permit - Uploaded</asp:ListItem>
                                                <asp:ListItem Value="5">Approval - Pending</asp:ListItem>
                                                <asp:ListItem Value="6">Approval - Done</asp:ListItem>
                                                <asp:ListItem Value="7">Approval - Returned</asp:ListItem>
                                                <asp:ListItem Value="8">Approval - Rejected</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>

                                        <div class="col-md-3 col-sm-6 form-group mb-md-0 mb-3">
                                            <label class="text-muted small font-weight-bold text-uppercase mb-1"><i class="fa fa-tags"></i>JOB Type</label>
                                            <asp:DropDownList ID="DDL_BillingType" runat="server" CssClass="form-control form-control-sm rounded custom-select"></asp:DropDownList>
                                        </div>

                                        <div class="col-md-3 col-sm-12 form-group mb-md-0 mb-3">
                                            <label class="text-muted small font-weight-bold text-uppercase mb-1"><i class="fa fa-search"></i>Quick Search</label>
                                            <asp:TextBox ID="txt_quicksearch" runat="server" CssClass="form-control form-control-sm rounded" placeholder="Type to filter grid..." onkeyup="filterGrid()"></asp:TextBox>
                                        </div>

                                        <div class="col-md-3 col-sm-12 text-md-right text-center mt-2 mt-md-0">
                                            <asp:LinkButton ID="btn_reset" runat="server" CssClass="btn btn-outline-warning btn-sm px-3" OnClick="btn_reset_Click">
            <i class="fa fa-refresh mr-1"></i> Reset
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btn_submit" runat="server" CssClass="btn btn-success btn-sm px-3 shadow-sm" OnClick="btn_submit_Click">
            <i class="fa fa-filter mr-1"></i> Fetch
                                            </asp:LinkButton>
                                        </div>

                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12 col-sm-12">
                            <div class="card-box small" style="width: 100%; height: 450px; overflow: auto; border: 1px solid #e5e5e5; background: #fff;">
                                <asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand">
                                    <HeaderStyle CssClass="bg-dark text-white text-center align-middle" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_slno" runat="server" Font-Bold="true" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center align-middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="DBID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' />
                                                <asp:Label ID="lbl_Creator_Workman" runat="server" Text='<%# Eval("Creator_Workman") %>' />
                                                <asp:Label ID="lbl_JOBID" runat="server" Text='<%# Eval("JOBID") %>' />
                                                <asp:Label ID="lbl_JOBID_Status" runat="server" Text='<%# Eval("JOBID_Status") %>' />
                                                <asp:Label ID="lbl_FinalUpldStatus" runat="server" Text='<%# Eval("FinalUpldStatus") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Date" HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_CreatedDate" runat="server" Text='<%# Eval("CreatedDate","{0:dd-MMM-yyyy}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center align-middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="JOBID" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Button ID="btn_viewdetails" runat="server" Text='<%# Eval("JOBID") %>' CssClass="btn btn-sm btn-info grid-action-btn font-weight-bold" CommandName="View_Details" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="View Details" />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center align-middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Status" HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:Button ID="btn_jobidstatus" runat="server" Text='<%# Eval("JOBID_Status") %>' CssClass="btn grid-action-btn" CommandName="Swap_JOBIDStatus" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Toggle Status" />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center align-middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Site / Location" HeaderStyle-Width="12%">
                                            <ItemTemplate>
                                                <strong>
                                                    <asp:Label ID="lbl_JOB_Site" runat="server" Text='<%# Eval("JOB_Site") %>' /></strong><br />
                                                <span class="text-muted"><%# Eval("JOB_Location") %></span>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text-center align-middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Approver" HeaderStyle-Width="12%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_JOB_InchargeName" runat="server" Font-Bold="true" Text='<%# Eval("JOB_InchargeName") %>' /><br />
                                                <asp:Label ID="lbl_Incharge_Approval" runat="server" CssClass="small font-weight-bold" Text='<%# Eval("Incharge_Approval") %>' />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text-center align-middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Shift" HeaderStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_JOB_Shift" runat="server" CssClass="badge" Text='<%# Eval("JOB_Shift") %>' />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center align-middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Permit No" HeaderStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_JOB_PermitNo" runat="server" Font-Bold="true" Text='<%# Eval("JOB_PermitNo") %>' />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center align-middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="MP" HeaderStyle-Width="3%">
                                            <ItemTemplate>
                                                <span class="badge" style="font-size: 12px;"><%# Eval("ManpowerCount") %></span>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center align-middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="JOB Title" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <span title='<%# Eval("JOB_Title") %>'>
                                                    <%# Eval("JOB_Title").ToString().Length > 50 ? Eval("JOB_Title").ToString().Substring(0,50) + "..." : Eval("JOB_Title") %>
                                                </span>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text-wrap align-middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Action" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btndelete" runat="server" CommandName="DeleteRec" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn btn-outline-danger grid-action-btn" ToolTip="Delete JOB" OnClientClick="return confirm('WARNING: Do you want to permanently DELETE this JOB and all data?');">
                                                    <i class="fa fa-trash"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text-center align-middle" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div class="alert alert-info text-center mt-3"><i class="fa fa-info-circle"></i>No Data Found for this selection.</div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <script type="text/javascript">
        function showPNotify(title, text, type) {
            new PNotify({
                title: title, text: text, type: type, styling: 'bootstrap3', delay: 4000
            });
        }

        // =========================================================
        // CLIENT-SIDE QUICK SEARCH FILTER
        // =========================================================
        function filterGrid() {
            // 1. Get the search input and the GridView table
            var input = document.getElementById('<%= txt_quicksearch.ClientID %>');
            var filter = input.value.toUpperCase();
            var table = document.getElementById('<%= GridView1.ClientID %>');

            // Safety check: if grid is empty or hasn't loaded, do nothing
            if (!table) return;

            var tr = table.getElementsByTagName("tr");

            // 2. Loop through all table rows (start at index 1 to skip the header row!)
            for (var i = 1; i < tr.length; i++) {
                var rowContainsFilter = false;
                var td = tr[i].getElementsByTagName("td");

                // 3. Loop through every column in the current row
                for (var j = 0; j < td.length; j++) {
                    if (td[j]) {
                        var txtValue = td[j].textContent || td[j].innerText;
                        if (txtValue.toUpperCase().indexOf(filter) > -1) {
                            rowContainsFilter = true;
                            break; // Stop checking columns if we found a match in this row
                        }
                    }
                }

                // 4. Toggle visibility
                if (rowContainsFilter) {
                    tr[i].style.display = ""; // Show
                } else {
                    tr[i].style.display = "none"; // Hide
                }
            }
        }

        // Re-apply the filter automatically if the UpdatePanel refreshes the grid
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            filterGrid();
        });
    </script>
</asp:Content>
