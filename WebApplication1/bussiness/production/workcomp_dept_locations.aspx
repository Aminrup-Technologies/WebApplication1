<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="workcomp_dept_locations.aspx.cs" Inherits="WebApplication1.bussiness.production.department_locations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>Department Work Locations</h3>
                </div>

                <div class="title_right">
                    <div class="col-md-5 col-sm-5 form-group pull-right top_search">
                        <!--<div class="input-group">
					<input type="text" class="form-control" placeholder="Search for...">
					<span class="input-group-btn">
					  <button class="btn btn-default" type="button">Go!</button>
					</span>
				  </div>-->
                    </div>
                </div>
            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12  ">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Add : Department Locations</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                                <%--<li><a class="close-link"><i class="fa fa-close"></i></a></li>--%>
                            </ul>
                            <div class="clearfix"></div>
                        </div>

                        <div class="x_content">
                            <div class="row">

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Select Country Name <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_WorkCountry" CssClass="form-control form-control-sm rounded" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL_WorkCountry_SelectedIndexChanged"></asp:DropDownList>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Select State Name <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_WorkStates" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_WorkStates_SelectedIndexChanged"></asp:DropDownList>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Select Work Region <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_Region" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Region_SelectedIndexChanged"></asp:DropDownList>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Select Company <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_Company" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Company_SelectedIndexChanged"></asp:DropDownList>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Select Department <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDl_Departments" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDl_Departments_SelectedIndexChanged"></asp:DropDownList>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Location Name <span class="text text-danger">*</span></label>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_locname" class="form-control form-control-sm rounded" placeholder="Enter Location Short Code..." runat="server"></asp:TextBox>
                                    <small class="form-text text-muted ml-4">Example : IN</small>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Location Code<span class="text text-danger">*</span></label>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_loccode" class="form-control form-control-sm rounded" placeholder="Enter Location Short Code..." runat="server"></asp:TextBox>
                                    <small class="form-text text-muted ml-4">Example : IN</small>
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
                                        <asp:Button ID="btn_submit" runat="server" Text="Submit" CssClass="btn btn-success btn-sm" OnClientClick="return ValidateFormField()" OnClick="btn_submit_Click" />
                                    </div>
                                </div>
                            </div>
                            <%--button   end--%>

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

                <div class="col-md-12 col-sm-12  ">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>View and Manage : Department Locations</h2>
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
                                        <asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="3%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="text text-center" />
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="text text-center" />
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Country Code" Visible="false" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Country_Code" runat="server" Text='<%# Eval("Country_Code") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="text text-center" />
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="State Code" Visible="false" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_State_Code" runat="server" Text='<%# Eval("State_Code") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="text text-center" />
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Region Code" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Work_Region_Code" runat="server" Text='<%# Eval("Work_Region_Code") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="text text-center" />
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Company Code" HeaderStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Company_Code" runat="server" Text='<%# Eval("Company_Code") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="text text-center" />
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Department Code" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Dept_DBCode" runat="server" Text='<%# Eval("Dept_DBCode") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="text text-center" />
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Department Name" HeaderStyle-Width="18%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Company_Department" runat="server" Text='<%# Eval("Company_Department") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="text text-center" />
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="DB Code" HeaderStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_DB_Code" runat="server" Text='<%# Eval("DB_Code") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="text text-center" />
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Location Name" HeaderStyle-Width="18%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_CompDept_Location" runat="server" Text='<%# Eval("CompDept_Location") %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_CompDept_Location" class="form-control form-control-sm rounded" Text='<%# Eval("CompDept_Location") %>' Width="100%" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="Dynamic" ErrorMessage="*" ForeColor="Red" ControlToValidate="txt_CompDept_Location" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="text text-center" />
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Location Code" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_CompDept_Location_Code" runat="server" Text='<%# Eval("CompDept_Location_Code") %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_CompDept_Location_Code" class="form-control form-control-sm rounded" Text='<%# Eval("CompDept_Location_Code") %>' Width="100%" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ErrorMessage="*" ForeColor="Red" ControlToValidate="txt_CompDept_Location_Code" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="text text-center" />
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="5%">
                                                    <EditItemTemplate>
                                                        <asp:ImageButton ID="btnupdate" runat="server" CommandName="Update" Height="15px" ImageUrl="~/erp_images/fi-sr-disk.svg" Width="15px" ToolTip="Save" ImageAlign="Middle" />
                                                        &nbsp;
														<asp:ImageButton ID="Btncancale" runat="server" CommandName="Cancel" Height="15px" ImageUrl="~/erp_images/fi-sr-cross-circle.svg" Width="15px" ToolTip="Cancel Update" ImageAlign="Middle" />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnedit" runat="server" CommandName="Edit" Height="15px" ImageUrl="~/erp_images/fi-sr-pencil.svg" Width="15px" ToolTip="Update" ImageAlign="Middle" />
                                                        &nbsp;
														<asp:ImageButton ID="btndelete" runat="server" CommandName="Delete" Height="15px" ImageUrl="~/erp_images/fi-sr-trash.svg" Width="15px" ToolTip="Delete" OnClientClick="return confirm('Do you want to DELETE...?')" ImageAlign="Middle" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="text text-center" />
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                            </Columns>
                                            <HeaderStyle CssClass="text text-center" />
                                            <EditRowStyle CssClass="bg-blue-sky" />
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

        function ValidateFormField() {
            if (document.getElementById('<%=DDL_WorkCountry.ClientID%>').selectedIndex == 0) {
                document.getElementById('<%=DDL_WorkCountry.ClientID%>').focus();
                ShowPopup("Error :", "Work Country selection required...!");
                return false;
            }

            if (document.getElementById('<%=DDL_WorkStates.ClientID%>').selectedIndex == 0) {
                ShowPopup("Error :", "Work State selection required");
                document.getElementById('<%=DDL_WorkStates.ClientID%>').focus();
                return false;
            }

            if (document.getElementById('<%=DDL_Region.ClientID%>').selectedIndex == 0) {
                ShowPopup("Error :", "Work Region selection required");
                document.getElementById('<%=DDL_Region.ClientID%>').focus();
                return false;
            }

            if (document.getElementById('<%=DDL_Company.ClientID%>').selectedIndex == 0) {
                ShowPopup("Error :", "Company Selection required");
                document.getElementById('<%=DDL_Company.ClientID%>').focus();
                return false;
            }

            if (document.getElementById('<%=txt_locname.ClientID%>').value == "") {
                ShowPopup("Error :", "Location Name required");
                document.getElementById('<%=txt_locname.ClientID%>').focus();
                return false;
            }

            if (document.getElementById('<%=txt_loccode.ClientID%>').value == "") {
                ShowPopup("Error :", "Location Short Name / Code required");
                document.getElementById('<%=txt_loccode.ClientID%>').focus();
                return false;
            }
        }
    </script>
</asp:Content>
