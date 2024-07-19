<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="view_emp_mastertbldata.aspx.cs" Inherits="WebApplication1.bussiness.production.view_emp_mastertbldata" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .thumbnail {
            position: relative;
            overflow: hidden;
            width: 100px; /* Set the initial width of the thumbnail */
            height: 100px; /* Set the initial height of the thumbnail */
            transition: width 0.3s, height 0.3s; /* Add smooth transition effect */
        }

            .thumbnail:hover {
                width: 150px; /* Set the enlarged width on hover */
                height: 150px; /* Set the enlarged height on hover */
            }

        .thumbnail-image {
            width: 100%;
            height: 100%;
            object-fit: cover; /* Ensure the image covers the entire container */
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h5>View Employee Master Data ||
                        <asp:Button ID="Button1" runat="server" Text="Export" OnClick="ExportExcel" CssClass="btn btn-primary btn-sm" /></h5>
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
                <div class="card-box col-md-12 col-sm-12 small" style="width: 100%; height: 450px; overflow: scroll">
                    <asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand">
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

                            <asp:TemplateField HeaderText="Region Code" HeaderStyle-Width="5%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_WorkRegion" runat="server" Text='<%# Eval("WorkRegion") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Company Code" HeaderStyle-Width="5%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_WorkCompany" runat="server" Text='<%# Eval("WorkCompany") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Login_Details" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    Login:<asp:Label ID="lbl_LoginID" runat="server" Text='<%# Eval("LoginID") %>' />
                                    <br />
                                    PWD:<asp:Label ID="lbl_LoginPassword" runat="server" Text='<%# Eval("LoginPassword") %>' />
                                    <br />
                                    ActiveOn:<asp:Label ID="lbl_LastLogin" runat="server" Text='<%# Eval("LastLogin") %>' />

                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Work_Status" HeaderStyle-Width="3%">
                                <ItemTemplate>
                                    <asp:Button ID="btn_viewdetails" runat="server" Text='<%# Eval("WorkmanSL") %>' Font-Size="Smaller" CssClass="btn btn-sm btn-info" CommandName="View_Details" CommandArgument="<%# Container.DataItemIndex %>" />
                                    <asp:Label ID="lbl_WorkmanSL" runat="server" Text='<%# Eval("WorkmanSL") %>' Visible="false" />
                                    <br />
                                    <asp:Button ID="btn_workstatus" runat="server" Text='<%# Eval("WorkStatus") %>' Font-Size="Smaller" CssClass="btn btn-sm btn-info" CommandName="Swap_WorkStatus" CommandArgument="<%# Container.DataItemIndex %>" />
                                    <asp:Label ID="lbl_WorkStatus" runat="server" Text='<%# Eval("WorkStatus") %>' Visible="false" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Photo" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <!-- Replace "YourImagePathField" with the actual field name containing the image path in your data source -->
                                    <%--<asp:Image ID="imgPhoto" runat="server" ImageUrl='<%# Eval("PrfPicPath") %>' Height="70" Width="70" />--%>
                                    <div class="thumbnail">
                                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("PrfPicPath") %>' CssClass="thumbnail-image" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="20%">
                                <ItemTemplate>
                                    Name:<asp:Label ID="lbl_FullName" runat="server" Text='<%# Eval("FullName") %>' Font-Bold="true" /><br />
                                    Grade:<asp:Label ID="lbl_SkillCategory" runat="server" Text='<%# Eval("SkillCategory") %>' /><br />
                                    Desg:<asp:Label ID="lbl_SkillDesignation" runat="server" Text='<%# Eval("SkillDesignation") %>' /><br />
                                    Father_Name:<asp:Label ID="lbl_Fathername" runat="server" Text='<%# Eval("Fathername") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Imp Dates" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    DOR:<asp:Label ID="lbl_DOR" runat="server" Text='<%# Eval("DOR", "{0:dd-MM-yyyy}") %>' /><br />
                                    DOJ:<asp:Label ID="lbl_DOJ" runat="server" Text='<%# Eval("DOJ", "{0:dd-MM-yyyy}") %>' /><br />
                                    DOE:<asp:Label ID="lbl_DOE" runat="server" Text="N/A" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Contact" HeaderStyle-Width="7%">
                                <ItemTemplate>
                                    Mob:<asp:Label ID="lbl_MobileNo" runat="server" Text='<%# Eval("MobileNo") %>' /><br />
                                    Email:<asp:Label ID="lbl_Email" runat="server" Text='<%# Eval("Email") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Working Details" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    Site:<asp:Label ID="lbl_WorkSite" runat="server" Text='<%# Eval("WorkSite") %>' Font-Bold="true" /><br />
                                    SP:<asp:Label ID="lbl_SafetyPassNo" runat="server" Text='<%# Eval("SafetyPassNo") %>' Font-Bold="true" />
                                    <br />
                                    GP:<asp:Label ID="lbl_GatePassNo" runat="server" Text='<%# Eval("GatePassNo") %>' Font-Bold="true" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="GridHeaderText-Center" />
                                <ItemStyle CssClass="grid" />
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
