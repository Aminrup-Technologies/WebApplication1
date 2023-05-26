<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="add_wo_skillcategory.aspx.cs" Inherits="WebApplication1.bussiness.production.add_wo_skillcategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="container">
            <%--<div class="page-title">
                <div class="title_left">
                    <h3>Work Order Data Management</h3>
                </div>
            </div>

            <div class="clearfix"></div>--%>

            <div class="row">
                <div class="col-md-12 col-sm-12  ">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Add : Work Order <--> Skill Category</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row">

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Select Work Region <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_WorkRegion" CssClass="form-control form-control-sm rounded" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL_WorkRegion_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_WorkRegion" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Select Company Name<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_Company" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Company_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_Company" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Select DepartmentName <span class="text text-danger">*</span></label>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_Departments" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Departments_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_Departments" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Select Work Order<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12 form-group">
                                    <asp:DropDownList ID="DDL_Workorder" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Workorder_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_Workorder" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Select Skill Designation <span class="text text-danger">*</span></label>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_SkillDesignation" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_SkillDesignation_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_SkillDesignation" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group">
                                    <label>Selct Skill Category<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12 form-group">
                                    <asp:DropDownList ID="DDL_SkillCategory" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="false"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_SkillCategory" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
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
                                        <button type="reset" class="btn btn-warning btn-sm">Reset</button>
                                        <asp:Button ID="btn_submit" runat="server" Text="Submit" CssClass="btn btn-success btn-sm" OnClick="btn_submit_Click" />
                                    </div>
                                </div>
                            </div>
                            <%--button   end--%>
                        </div>

                    </div>
                </div>

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

                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>View and Manage : Work Order - Skill Category</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                                <%--<li><a class="close-link"><i class="fa fa-close"></i></a></li>--%>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row">

                                <div class="col-md-12 col-sm-12">
                                    <div class="card-box table-responsive small">
                                        <asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="GridHeaderText-Center" />
                                                    <ItemStyle CssClass="grid" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Region Code" HeaderStyle-Width="7%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Work_Region_Code" runat="server" Text='<%# Eval("Work_Region_Code") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="GridHeaderText-Center hidden-small" />
                                                    <ItemStyle CssClass="grid" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Company Code" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Company_Code" runat="server" Text='<%# Eval("Company_Code") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="GridHeaderText-Center" />
                                                    <ItemStyle CssClass="grid" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Department Name" Visible="true" HeaderStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Department_Name" runat="server" Text='<%# Eval("Department_Name") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="GridHeaderText-Center" />
                                                    <ItemStyle CssClass="grid" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Work Order Number" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_WO_Number" runat="server" Text='<%# Eval("WO_Number") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="GridHeaderText-Center" />
                                                    <ItemStyle CssClass="grid" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Skill Designation" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Designation_Type" runat="server" Text='<%# Eval("Designation_Type") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="GridHeaderText-Center" />
                                                    <ItemStyle CssClass="grid" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Skill Category" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Category_Type" runat="server" Text='<%# Eval("Category_Type") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="GridHeaderText-Center" />
                                                    <ItemStyle CssClass="grid" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="WO / PO Skill Category" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_WO_Category_Type" runat="server" Text='<%# Eval("WO_Category_Type") %>' />
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
