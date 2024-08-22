<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="manage_rollsaccess.aspx.cs" Inherits="WebApplication1.bussiness.production.manage_rollsaccess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="container">
            <div class="row">
                <div class="col-md-12 col-sm-12 ">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>
                                <asp:Label ID="lbl_docnumber" runat="server" Text="Roles : Access Permission"></asp:Label></h2>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">

                            <div class="col-md-3">
                                <div class="mb-3">
                                    <asp:Label ID="lbl_DDL_Employee_Type" runat="server" AssociatedControlID="DDL_Employee_Type" Text="Employee Type" ForeColor="Blue" Font-Bold="true" Font-Size="Small"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RFV_DDL_Employee_Type" runat="server" ErrorMessage="*" ValidationGroup="Submit" ForeColor="Red" ControlToValidate="DDL_Employee_Type" Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
                                    [<asp:Label ID="lbl_DDL_EmpType_Value" runat="server" AssociatedControlID="DDL_Employee_Type" Text="N/A" ForeColor="LightBlue" Font-Bold="true" Font-Size="Smaller"></asp:Label>]
                                    <div class="input-group-sm">
                                        <asp:DropDownList ID="DDL_Employee_Type" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" ValidationGroup="Submit" OnSelectedIndexChanged="DDL_Plant_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>


                            <div class="col-md-3">
                                <div class="mb-3">
                                    <asp:Label ID="Lbl_TB_Emp_PermissionValue" runat="server" AssociatedControlID="TB_Emp_PermissionValue" Text="Role Access Type :" ForeColor="Blue" Font-Bold="true" Font-Size="Small"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RFV_TB_Emp_PermissionValue" runat="server" ErrorMessage="*" ControlToValidate="TB_Emp_PermissionValue" ValidationGroup="Submit" InitialValue="" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="REV_TB_Emp_PermissionValue" runat="server" ControlToValidate="TB_Emp_PermissionValue" ForeColor="Red" ValidationGroup="Submit" ErrorMessage="Numeric Only" ValidationExpression="^[0-9]*$" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <div class="input-group-sm">
                                        <asp:TextBox ID="TB_Emp_PermissionValue" runat="server" CssClass="form-control form-control-sm rounded" Placeholder="Role Type (3-20 characters)" MaxLength="20"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="mb-3">
                                    <asp:Label ID="Lbl_TB_Emp_PermissionText" runat="server" AssociatedControlID="TB_Emp_PermissionText" Text="Role Access Value :" ForeColor="Blue" Font-Bold="true" Font-Size="Small"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RFV_TB_Emp_PermissionText" runat="server" ErrorMessage="*" ControlToValidate="TB_Emp_PermissionText" ValidationGroup="Submit" InitialValue="" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="REV_TB_Emp_PermissionText" runat="server" ControlToValidate="TB_Emp_PermissionText" ForeColor="Red" ValidationGroup="Submit" ErrorMessage="Alphanumeric Only" ValidationExpression="^[a-zA-Z0-9\s\-]*$" Display="Dynamic"></asp:RegularExpressionValidator>
                                    <div class="input-group-sm">
                                        <asp:TextBox ID="TB_Emp_PermissionText" runat="server" CssClass="form-control form-control-sm rounded" Placeholder="Role value (3-50 characters)" MaxLength="50"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <%--button start--%>
                    <div class="col-md-6 center-margin">
                        <%--<div class="ln_solid"></div>--%>
                        <div class="item form-group row">
                            <div class="col-md-6 col-sm-12">
                                <asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to view Data!!"></asp:Label>
                            </div>
                            <div class="col-md-6 col-sm-12">
                                <asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="btn btn-danger btn-sm" CausesValidation="false" OnClick="btn_cancel_Click" />
                                <asp:Button ID="btn_insert" runat="server" Text="Reset" CssClass="btn btn-warning btn-sm" CausesValidation="false" />
                                <asp:Button ID="btn_submit" runat="server" Text="Submit" CssClass="btn btn-success btn-sm" CausesValidation="true" ValidationGroup="Submit" OnClick="btn_submit_Click" />
                            </div>
                        </div>
                    </div>
                    <%--button end--%>
                </div>

                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Create, Retrieve, Update & Delete Panel</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row">
                                <div class="card-box col-md-12 col-sm-12" style="width: 100%; height: 450px; overflow: scroll;">
                                    <asp:GridView ID="GridViewPlantLines" runat="server" AllowPaging="True" PageSize="10" Width="100%" CssClass="table table-striped table-hover table-bordered table-responsive table-sm table-condensed"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" DataKeyNames="id"
                                        OnRowEditing="GridViewPlantLines_RowEditing" OnRowCancelingEdit="GridViewPlantLines_RowCancelingEdit"
                                        OnRowUpdating="GridViewPlantLines_RowUpdating" OnRowDeleting="GridViewPlantLines_RowDeleting" OnPageIndexChanging="GridViewPlantLines_PageIndexChanging">
                                        <Columns>
                                            <asp:BoundField DataField="Id" HeaderText="DB ID" ReadOnly="True" />
                                            <asp:BoundField DataField="EmpType_Value" HeaderText="Role type" ReadOnly="True" />

                                            <asp:TemplateField HeaderText="Role Access Value">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmp_PermissionValue" runat="server" Text='<%# Eval("Emp_PermissionValue") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtEmp_PermissionValue" runat="server" Text='<%# Bind("Emp_PermissionValue") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Role Access Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmp_PermissionText" runat="server" Text='<%# Eval("Emp_PermissionText") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtEmp_PermissionText" runat="server" Text='<%# Bind("Emp_PermissionText") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="added_on" HeaderText="Added On" DataFormatString="{0:dd/MM/yyyy}" />

                                            <asp:TemplateField HeaderText="View Status" HeaderStyle-Width="2%" ItemStyle-Width="2%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkViewStatus" runat="server" Checked='<%# Convert.ToBoolean(Eval("view_status")) %>' Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="chkViewStatusEdit" runat="server" Checked='<%# Convert.ToBoolean(Eval("view_status")) %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Delete Status" HeaderStyle-Width="2%" ItemStyle-Width="2%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkDeleteStatus" runat="server" Checked='<%# Convert.ToBoolean(Eval("delete_status")) %>' Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="chkDeleteStatusEdit" runat="server" Checked='<%# Convert.ToBoolean(Eval("delete_status")) %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <%--<asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />--%>

                                            <asp:TemplateField HeaderText="Actions">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Edit" CssClass="btn btn-sm btn-info" />
                                                <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Delete" CssClass="btn btn-sm btn-danger" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" Text="Update" CssClass="btn btn-sm btn-success" />
                                                <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btn btn-sm btn-secondary" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
