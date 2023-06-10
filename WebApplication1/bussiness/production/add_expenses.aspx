<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="add_expenses.aspx.cs" Inherits="WebApplication1.bussiness.production.add_expenses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="container">
            <div class="page-title">
                <div class="title_left">
                    <h3>Add Expenses</h3>
                </div>
            </div>
            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-6 col-sm-6 profile_details">
                    <div class="well profile_view col-sm-12 col-lg-12">
                        <div class="col-sm-12">
                            <h4 class="brief"><i>Budget Account</i></h4>
                            <div class="right col-md-4 col-sm-6 text-center">
                                <img src="../../erp_images/bank.jpg" alt="ProfilePhoto" class="img-circle" width="100" height="100">
                            </div>
                            <div class="left col-md-8 col-sm-6">
                                <h2 style="text-align: center;">₹
                                    <asp:Label ID="lbl_budgetamnt" runat="server" Text="0" ForeColor="Black" Font-Bold="true"></asp:Label></h2>
                                <hr />

                                <span style="text-align: center;"><strong>Claimed :</strong>
                                    ₹
                                    <asp:Label ID="lbl_usedamnt" runat="server" Text="0" ForeColor="Black" Font-Bold="true"></asp:Label>
                                </span>
                                <span style="text-align: center;">&nbsp;||&nbsp;</span>
                                <span style="text-align: center;"><strong>Outstanding :</strong>
                                    ₹
                                    <asp:Label ID="lbl_leftamnt" runat="server" Text="0" ForeColor="DarkRed" Font-Bold="true"></asp:Label>
                                </span>
                                <br />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6 col-sm-6 profile_details">
                    <div class="well profile_view col-sm-12 col-lg-12">
                        <div class="col-sm-12">
                            <h4 class="brief"><i>Budget Logs</i></h4>
                            <div class="col-sm-12">N/A</div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Claim Expenses Against Bill / Receipt</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row" id="addexpenserow" runat="server" visible="true">
                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <label>Select Work Region<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6 form-group">
                                    <asp:DropDownList ID="DDL_Region" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Region_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RFV_1" ValidationGroup="Submit" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_Region" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group" id="comp_row1" runat="server" visible="false">
                                    <label>Select Work Company<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6 form-group" id="comp_row2" runat="server" visible="false">
                                    <asp:DropDownList ID="DDL_Company" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Company_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RFV_2" ValidationGroup="Submit" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_Company" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group" id="dept_row1" runat="server" visible="false">
                                    <label>Select Compnay Dept<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6 form-group" id="dept_row2" runat="server" visible="false">
                                    <asp:DropDownList ID="DDL_CompDept" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_CompDept_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RFV_3" ValidationGroup="Submit" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_CompDept" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group" id="wrkordr_row1" runat="server" visible="false">
                                    <label>Select Workorder<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6 form-group" id="wrkordr_row2" runat="server" visible="false">
                                    <asp:DropDownList ID="DDL_Workorder" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RFV_4" ValidationGroup="Submit" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_Workorder" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group" id="loc_row1" runat="server" visible="false">
                                    <label>Select Location<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6 form-group" id="loc_row2" runat="server" visible="false">
                                    <asp:DropDownList ID="DDL_Location" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Location_SelectedIndexChanged" Enabled="true"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RFV_5" ValidationGroup="Submit" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_Location" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-2 col-sm-6 col-xs-6 form-group" id="worksite_row1" runat="server" visible="false">
                                    <label>Select Worksite<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-2 col-sm-6 col-xs-6 form-group" id="worksite_row2" runat="server" visible="false">
                                    <asp:DropDownList ID="DDL_Worksite" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RFV_9" ValidationGroup="Submit" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_Worksite" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>

                        <div class="row" id="expense_created" runat="server" visible="false">
                            <div class="col-md-6 col-sm-12" style="vertical-align: middle; text-align: center;">
                                <asp:Image ID="Img_Success" runat="server" ImageUrl="~/erp_images/success_gif.gif" Width="100px" Height="100px" />
                                <asp:Label ID="Label2" runat="server" Text="Expense ID Created...!" Font-Bold="true" Font-Size="Large"></asp:Label>
                            </div>
                            <div class="col-md-6 col-sm-12" style="vertical-align: middle; text-align: center;" id="Addexpbtnrow" runat="server" visible="true">
                                <asp:Button ID="btn_addexp" runat="server" Text="ADD Expenses" CssClass="btn btn-success btn-sm" OnClick="btn_addexp_Click" />
                                <asp:Label ID="lbl_expid" runat="server" Text="" Visible="false"></asp:Label>
                            </div>
                        </div>


                        <div class="row" id="expense_add" runat="server" visible="false">
                            <div class="col-md-2 col-sm-6 col-xs-6 form-group" id="heads_row1" runat="server" visible="false">
                                <label>Select Expense Category<span class="text text-danger">*</span></label>
                            </div>
                            <div class="col-md-2 col-sm-6 col-xs-6 form-group" id="heads_row2" runat="server" visible="false">
                                <asp:DropDownList ID="DDL_ExpHeads" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_ExpHeads_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFV_6" ValidationGroup="addtolist" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_ExpHeads" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-2 col-sm-6 col-xs-6 form-group" id="subhead_row1" runat="server" visible="false">
                                <label>Select Expense Title<span class="text text-danger">*</span></label>
                            </div>
                            <div class="col-md-2 col-sm-6 col-xs-6 form-group" id="subhead_row2" runat="server" visible="false">
                                <asp:DropDownList ID="DDL_SubHeads" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_SubHeads_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFV_7" ValidationGroup="addtolist" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_SubHeads" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-2 col-sm-12 form-group" id="quantity_row1" runat="server" visible="false">
                                <label>Quantity<span class="text text-danger">*</span></label>
                            </div>
                            <div class="col-md-2 col-sm-12 form-group" id="quantity_row2" runat="server" visible="false">
                                <asp:TextBox ID="txt_quantity" runat="server" class="form-control form-control-sm rounded" Text="1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="addtolist" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_quantity" SetFocusOnError="true" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txt_quantity" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Only Numeric" ID="RegularExpressionValidator2" ValidationExpression="^[0-9.]+$"></asp:RegularExpressionValidator>
                            </div>

                            <div class="col-md-2 col-sm-12 form-group" id="expamnt_row1" runat="server" visible="false">
                                <label>Expense Amount<span class="text text-danger">*</span></label>
                            </div>
                            <div class="col-md-2 col-sm-12 form-group" id="expamnt_row2" runat="server" visible="false">
                                <asp:TextBox ID="txt_amount" runat="server" class="form-control form-control-sm rounded"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="addtolist" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_amount" SetFocusOnError="true" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txt_amount" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Only Numeric" ID="RegularExpressionValidator1" ValidationExpression="^[0-9.]+$"></asp:RegularExpressionValidator>
                            </div>

                            <div class="col-md-2 col-sm-6 col-xs-6 form-group" id="expdescp_row1" runat="server" visible="false">
                                <label>Expense Description</label>
                            </div>
                            <div class="col-md-2 col-sm-6 col-xs-6 form-group" id="expdescp_row2" runat="server" visible="false">
                                <asp:TextBox ID="txt_expdescp" runat="server" class="form-control form-control-sm rounded" TextMode="MultiLine" Rows="3" Text="N/A"></asp:TextBox>
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txt_expdescp" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ErrorMessage="NO special characters" ID="rfvname" ValidationExpression="^[a-zA-Z0-9 ,]+$"></asp:RegularExpressionValidator>
                            </div>


                            <div class="col-md-3 col-sm-12 form-group" id="pdfuploadbuttonrow1" runat="server" visible="false">
                                <label>Upload File (.pdf / .jpg) <span class="text text-danger">*</span></label>
                            </div>
                            <div class="col-md-3 col-sm-12 form-group" id="pdfuploadbuttonrow2" runat="server" visible="false">
                                <button type="button" class="btn btn-primary btn-sm" id="btnShowPopup" runat="server" data-toggle="modal" data-target="#myModal">
                                    <i class="fa fa-plus-circle"></i>&nbsp;Upload Bill / Receipt
                                </button>
                            </div>

                            <div class="col-md-3 col-sm-12 form-group" id="Div3" runat="server" visible="false">
                                <label>Upload Message<span class="text text-danger">*</span></label>
                            </div>
                            <div class="col-md-3 col-sm-12 form-group" id="Div4" runat="server" visible="false">
                                <asp:Label ID="Label1" runat="server" Text="Label" Font-Bold="true" ForeColor="DarkGreen">Uploaded Successfully</asp:Label>
                                <asp:Label ID="lbl_filename" runat="server" Text=""></asp:Label>
                                <asp:Label ID="lbl_filetype" runat="server" Text="" Visible="false"></asp:Label>
                                <asp:Label ID="lbl_ext" runat="server" Text="" Visible="false"></asp:Label>
                                <asp:Label ID="lbl_data" runat="server" Text="" Visible="false"></asp:Label>
                            </div>
                        </div>

                        <%--button start 2 --%>
                        <div class="col-md-6 center-margin" id="AddExpensesButtons" runat="server" visible="false">
                            <div class="ln_solid"></div>
                            <div class="item form-group row">
                                <div class="col-md-6 col-sm-12 pb-2" style="text-align: center;">
                                    <asp:Label ID="Label4" runat="server" Text="Click ADD to Save Data!!"></asp:Label>
                                </div>
                                <div class="col-md-6 col-sm-12" style="text-align: center;">
                                    <asp:Button ID="btn_addbtns" runat="server" Text="ADD TO LIST" ValidationGroup="addtolist" CssClass="btn btn-primary btn-sm" OnClick="btn_addbtns_Click" />
                                </div>
                            </div>
                        </div>
                        <%--button end 2 --%>

                        <div class="col-md-12 col-sm-12" id="ViewState_TableRow" runat="server" visible="false">
                            <div class="card-box table-responsive">
                                <asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowDeleting="GridView1_RowDeleting">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl. NO" Visible="True" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="grid" />
                                            <ItemStyle CssClass="grid" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Head" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_ExpHead" runat="server" Text='<%# Bind("ExpHead") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="grid" />
                                            <ItemStyle CssClass="grid" />
                                        </asp:TemplateField>

                                       <%-- <asp:TemplateField HeaderText="Sub-Head" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_ExpSubHead" runat="server" Text='<%# Bind("ExpSubHead") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="grid" />
                                        </asp:TemplateField>--%>

                                        <asp:TemplateField HeaderText="Qnty" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Quantity" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="grid" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Description" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Description" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="grid" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Amount" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_ClaimAmount" runat="server" Text='<%# Bind("ClaimAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="grid" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="File" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_FileName" runat="server" Text='<%# Bind("FileName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="grid" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="SiteName" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_FileType" runat="server" Text='<%# Bind("FileType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="grid" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="SiteCode" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Extension" runat="server" Text='<%# Bind("Extension") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="grid" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="IN" HeaderStyle-Width="20%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Data" runat="server" Text='<%# Bind("Data") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="grid" />
                                            <ItemStyle CssClass="grid" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Action" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btndelete" runat="server" CommandName="Delete" Height="15px" ImageUrl="~/erp_images/fi-sr-trash.svg" Width="15px" ToolTip="Delete" OnClientClick="return confirm('Do you want to DELETE...?')" ImageAlign="Middle" />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>

                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div class="grid">No Data Found</div>
                                    </EmptyDataTemplate>
                                </asp:GridView>

                            </div>
                        </div>

                        <%--button start 2 --%>
                        <div class="col-md-6 center-margin" id="finaldisptach" runat="server" visible="false">
                            <div class="ln_solid"></div>
                            <div class="item form-group row">
                                <div class="col-md-6 col-sm-12 pb-2" style="text-align: center;">
                                    <asp:Label ID="lbl_fnlmsg" runat="server" Text="Click ADD to Save Data!!"></asp:Label>
                                </div>
                                <div class="col-md-6 col-sm-12" style="text-align: center;">
                                    <asp:Button ID="btn_finish" runat="server" Text="FINISH" CausesValidation="false" CssClass="btn btn-success btn-sm" OnClick="btn_finish_Click" />
                                </div>
                            </div>
                        </div>
                        <%--button end 2 --%>

                        <div class="row" id="ExpensesAdded" runat="server" visible="false">
                            <div class="col-md-12 col-sm-12" style="vertical-align: middle; text-align: center;">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/erp_images/success_gif.gif" Width="100px" Height="100px" />
                                <asp:Label ID="Label5" runat="server" Text="Expenses sent for Approval" Font-Bold="true" Font-Size="Large"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>

                <%--button start 1 --%>
                <div class="col-md-6 center-margin" id="submitbtns" runat="server" visible="false">
                    <div class="ln_solid"></div>
                    <div class="item form-group row">
                        <div class="col-md-6 col-sm-12 pb-2" style="text-align: center;">
                            <asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to Save Data!!"></asp:Label>
                        </div>
                        <div class="col-md-6 col-sm-12" style="text-align: center;">
                            <button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
                            <asp:Button ID="btn_reset" runat="server" Text="Reset" OnClick="btn_reset_Click" CssClass="btn btn-warning btn-sm" />
                            <asp:Button ID="btn_submit" runat="server" Text="Proceed Next" ValidationGroup="Submit" CssClass="btn btn-success btn-sm" OnClick="btn_submit_Click" />
                        </div>
                    </div>
                </div>
                <%--button end 1 --%>
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

            <%--- Up-loader Modal --%>
            <div class="modal fade" id="myModal">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">Upload Bill / Receipt</h4>
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label>Choose file for upload</label>
                                        <div class="input-group">

                                            <div class="col-md-6 col-sm-6 form-group" id="UpldTyp_Row1" runat="server" visible="true">
                                                <asp:Label ID="Label3" runat="server" Text="Upload Type"></asp:Label>
                                            </div>
                                            <div class="col-md-6 col-sm-6 form-group" id="UpldTyp_Row2" runat="server" visible="true">
                                                <asp:DropDownList ID="DDL_UploadType" runat="server" class="form-control form-control-sm rounded">
                                                    <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                                    <asp:ListItem>PDF File</asp:ListItem>
                                                    <asp:ListItem>Photograph</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Select Upload Type" ControlToValidate="DDL_UploadType" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="UPLOAD"></asp:RequiredFieldValidator>
                                            </div>


                                            <div class="custom-file col-md-8">
                                                <asp:FileUpload ID="FileUpload1" CssClass="custom-file-input" runat="server" />
                                                <label class="custom-file-label"></label>
                                            </div>
                                            <asp:Label ID="lbl_fileyesno" runat="server" Text="Label" Visible="false"></asp:Label>
                                            <div class="input-group-append col-md-4">
                                                <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-primary btn-sm" CausesValidation="true" ValidationGroup="UPLOAD" Text="Upload" OnClick="ImportPermit" />
                                            </div>
                                        </div>
                                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
            <%--- Up-loader Modal -------- END --%>
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
