<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="view_expensedetails.aspx.cs" Inherits="WebApplication1.bussiness.production.view_expensedetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h5>View & Update Expense Details</h5>
                </div>
            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12  ">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Expense Basic Details</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">

                            <%--form elements div ---- start--%>
                            <div class="row">
                                <div class="col-md-3 col-sm-6  form-group">
                                    <label>Expense ID </label>
                                </div>
                                <div class="col-md-3 col-sm-6  form-group">
                                    <asp:Label ID="lbl_expenseid" runat="server" Text="N/A" Font-Bold="true" ForeColor="Black"></asp:Label>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Approval Status </label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:Label ID="lbl_appstatus" runat="server" Text="No Data" Font-Bold="true"></asp:Label>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Creation DATE</label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_expdate" class="form-control form-control-sm rounded" runat="server" ReadOnly="true"></asp:TextBox>
                                    <asp:Label ID="lbl_expday" runat="server" Text=""></asp:Label>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Work Order No </label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_workorderno" class="form-control form-control-sm rounded" runat="server" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Work Site Name </label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_worksitename" class="form-control form-control-sm rounded" runat="server" ReadOnly="true"></asp:TextBox>
                                    <asp:Label ID="lbl_worksitedbcode" runat="server" Text="Label" Visible="true"></asp:Label>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Requester Name </label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_requestername" class="form-control form-control-sm rounded" runat="server" ReadOnly="true"></asp:TextBox>
                                    <asp:Label ID="lbl_requesterwrk" runat="server" Text="Label" Visible="true"></asp:Label>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>JOB Department </label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_jobdept" class="form-control form-control-sm rounded" runat="server" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>JOB Work Location </label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_jobloc" class="form-control form-control-sm rounded" runat="server" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Approver Name</label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:Label ID="lbl_appname" runat="server" Text="" Font-Bold="true" ForeColor="Blue"></asp:Label>
                                    [<asp:Label ID="lbl_aapwrk" runat="server" Text="" Visible="true"></asp:Label>]
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Approval Date</label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:Label ID="lbl_aapdate" runat="server" Text="N/A" Visible="true"></asp:Label>
                                </div>
                            </div>
                            <%--form elements div ---- end--%>

                            <%--form buttons div ---- start--%>
                            <div class="col-md-6 center-margin" runat="server" visible="false">
                                <div class="ln_solid"></div>
                                <div class="item form-group row">
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Button ID="btn_update" runat="server" Text="Update" CssClass="btn btn-success btn-sm" Enabled="false" />
                                        <asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="btn btn-danger btn-sm" Enabled="false" Visible="false" />
                                        <asp:Button ID="btn_back" runat="server" Text="Back" CssClass="btn btn-warning btn-sm" Enabled="true" Visible="true" />
                                    </div>

                                    <div class="col-md-6 col-sm-12">
                                        <asp:Label ID="lbl_msg" runat="server" Text=""></asp:Label>
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
                            <h2>Attached Expense Data & Details</h2>
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
                                        <asp:GridView ID="GridView2" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowCancelingEdit="GridView2_RowCancelingEdit" OnRowDeleting="GridView2_RowDeleting" OnRowEditing="GridView2_RowEditing" OnRowUpdating="GridView2_RowUpdating" OnRowCommand="GridView2_RowCommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl. NO" Visible="True" HeaderStyle-Width="5%">
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

                                                <asp:TemplateField HeaderText="Expense ID" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_ExpenseID" runat="server" Text='<%# Bind("ExpenseID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Expense Head" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_ExpHead" runat="server" Text='<%# Bind("ExpHead") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Expense Sub-Head" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_ExpSubHead" runat="server" Text='<%# Bind("ExpSubHead") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Qnty" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Quantity" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_Quantity" runat="server" class="form-control form-control-sm rounded" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %> ' Width="100%"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Expense Description" HeaderStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Description" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_Description" runat="server" class="form-control form-control-sm rounded" Text='<%# DataBinder.Eval(Container.DataItem,"Description") %> ' Width="100%"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Amount (Rs.)" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_ClaimAmount" runat="server" Text='<%# Bind("ClaimAmount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_ClaimAmount" runat="server" class="form-control form-control-sm rounded" Text='<%# DataBinder.Eval(Container.DataItem,"ClaimAmount") %> ' Width="100%"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="File Name" HeaderStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_FileName" runat="server" Text='<%# Bind("FileName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="View File" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Click to Download" CommandName="DownloadFile"><span class="glyphicon glyphicon-save" aria-hidden='true'></span></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Status" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btn_swapstatus" runat="server" Text='<%# Eval("AppStatus") %>' Font-Size="Smaller" CssClass="btn btn-sm btn-info" CommandName="Swap_Status" CommandArgument="<%# Container.DataItemIndex %>" />
                                                        <asp:Label ID="lbl_AppStatus" runat="server" Text='<%# Eval("AppStatus") %>' Visible="false" />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="3%" Visible="true">
                                                    <EditItemTemplate>
                                                        <asp:ImageButton ID="btnupdate" runat="server" CommandName="Update" Height="15px" CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/erp_images/fi-sr-disk.svg" Width="15px" ToolTip="Save" ImageAlign="Middle" />
                                                        <asp:ImageButton ID="Btncancale" runat="server" CommandName="Cancel" Height="15px" CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/erp_images/fi-sr-cross-circle.svg" Width="15px" ToolTip="Cancel Update" ImageAlign="Middle" />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnedit" runat="server" Visible="true" CommandName="Edit" CommandArgument="<%# Container.DataItemIndex %>" Height="15px" ImageUrl="~/erp_images/fi-sr-pencil.svg" Width="15px" ToolTip="Update" ImageAlign="Middle" />
                                                        <asp:ImageButton ID="btndelete" runat="server" Height="15px" CommandName="Delete" CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/erp_images/fi-sr-trash.svg" Width="15px" ToolTip="Delete" OnClientClick="return confirm('Do you want to DELETE...?')" ImageAlign="Middle" />
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
</asp:Content>
