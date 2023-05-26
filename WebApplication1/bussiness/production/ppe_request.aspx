<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="ppe_request.aspx.cs" Inherits="WebApplication1.bussiness.production.ppe_request" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .col1 {
            width: 10%;
        }

        .col2 {
            width: 30%;
        }

        .col3 {
            width: 30%;
        }

        .col4 {
            width: 30%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h5>Manual PPE Audit & Request</h5>
                </div>
            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>DOC#ATS/CSM/PPEChecklist-01</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>

                        <div class="x_content">
                            <%--Form Row start--%>
                            <div class="form-horizontal" id="id_creation" runat="server" visible="true">

                                <div class="col-md-3 col-sm-12 form-group" id="input_row1" runat="server" visible="true">
                                    <label>Workmem Sl<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12 form-group" id="input_row2" runat="server" visible="true">
                                    <asp:TextBox ID="txt_empworkman" class="form-control form-control-sm rounded" runat="server" placeholder="Enter Employee Workman" AutoPostBack="true" OnTextChanged="txt_empworkman_TextChanged"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="CreateID" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_empworkman" SetFocusOnError="true" InitialValue="" ToolTip="Kindly enter value"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-3 col-sm-6 col-xs-6 form-group" id="dept_row1" runat="server" visible="true">
                                    <label>Department<span class="text text-danger"></span></label>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-6 form-group" id="dept_row2" runat="server" visible="true">
                                    <asp:DropDownList ID="DDL_Dept" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Dept_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="CreateID" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_Dept" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group" id="loc_row1" runat="server" visible="false">
                                    <label>Location<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12 form-group" id="loc_row2" runat="server" visible="false">
                                    <asp:DropDownList ID="DDL_Location" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="CreateID" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_Location" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group" id="sftyofcrrow1" runat="server" visible="false">
                                    <label>Safety Officer<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12 form-group" id="sftyofcrrow2" runat="server" visible="false">
                                    <asp:DropDownList ID="DDL_SftyOfcr" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator34" ValidationGroup="CreateID" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_SftyOfcr" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group" id="inchargerow1" runat="server" visible="false">
                                    <label>Site In-Charge<span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12 form-group" id="inchargerow2" runat="server" visible="false">
                                    <asp:DropDownList ID="DDL_Approver" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator32" ValidationGroup="CreateID" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_Approver" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                                </div>

                            </div>

                            <%--Form Row END--%>

                            <div class="col-md-12 col-sm-12">
                                <div class="card-box table-responsive">
                                    <asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl. NO" Visible="True" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Workman" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_WorkmanSL" runat="server" Text='<%# Eval("WorkmanSL") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_FullName" runat="server" Text='<%# Eval("FullName") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Mobile" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_MobileNo" runat="server" Text='<%# Eval("MobileNo") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="WorkSite" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_WorkSite" runat="server" Text='<%# Eval("WorkSite") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Designation" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_SkillDesignation" runat="server" Text='<%# Eval("SkillDesignation") %>' />
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

                            <%--ADD button-1 start--%>
                            <div class="col-md-12" id="CreateIDRow" runat="server" visible="false">
                                <div class="ln_solid"></div>
                                <div class="row center">
                                    <div class="col-md-6 center">
                                        <asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to ADD Data!!"></asp:Label>
                                    </div>
                                    <div class="col-md-6 center">
                                        <asp:Button ID="btn_home" runat="server" Text="Home" CssClass="btn btn-danger btn-sm" PostBackUrl="~/bussiness/production/homepage.aspx" />
                                        <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="btn btn-warning btn-sm" PostBackUrl="~/bussiness/production/ppe_request.aspx" />
                                        <asp:Button ID="btn_submit" runat="server" Text="Generate ID" ValidationGroup="CreateID" Enabled="true" CausesValidation="true" CssClass="btn btn-success btn-sm" OnClick="btn_submit_Click" />
                                    </div>
                                </div>
                            </div>
                            <%--ADD button-1 end--%>

                            <div class="row" id="ID_CreatedMsg" runat="server" visible="false">
                                <div class="col-md-12 col-sm-12" style="vertical-align: middle; text-align: center;">
                                    <asp:Image ID="Img_Success1" runat="server" ImageUrl="~/erp_images/success_gif.gif" Width="100px" Height="100px" />
                                    <asp:Label ID="lbl_idcreatedmsg" runat="server" Text="ID Created..!" Font-Bold="true" Font-Size="Large"></asp:Label>
                                    <asp:Label ID="lbl_PPCID" runat="server" Text="" Visible="false"></asp:Label>
                                </div>
                            </div>

                        </div>
                    </div>


                    <div class="x_panel" runat="server" visible="false" id="TableBox">
                        <div class="x_title">
                            <h2>Employee PPE Checklist</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>

                        <div class="x_content">
                            <table class="table table-striped table-bordered table-responsive table-sm dt-responsive nowrap">
                                <tr>
                                    <td class="col1 text text-center"><span>Sl. No.</span></td>
                                    <td class="col2 text text-center"><span>PPE ITEMS</span></td>
                                    <td class="col3 text text-center"><span>Status</span></td>
                                    <td class="col4 text text-center"><span>Request ID</span></td>
                                </tr>

                                <tr>
                                    <td class="col1 text text-center"><span>1.</span></td>
                                    <td class="col2 text text-left"><span>Helmet</span></td>
                                    <td class="col3 text text-left">
                                        <asp:RadioButtonList ID="RBTN_Helmet" CssClass="" runat="server" CellPadding="5" Height="15px" CellSpacing="5" RepeatColumns="2" AutoPostBack="true" RepeatDirection="Horizontal" Width="100%" OnSelectedIndexChanged="RBTN_Helmet_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="0" Text="Ok">Ok</asp:ListItem>
                                            <asp:ListItem Value="1" Text="Not Ok">Not Ok</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                    <td class="col4 text text-center">
                                        <asp:Label ID="lbl_helmetrqid" runat="server" Text="N/A"></asp:Label></td>
                                </tr>
                                <tr id="HelmetNC" runat="server" visible="false">
                                    <td class="col1 text text-center">&nbsp;</td>
                                    <td class="col2 text text-center">
                                        <label>Helemt Request Type<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_HelmetWhy" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">PPE Validity Expired</asp:ListItem>
                                            <asp:ListItem Value="2">Damaged</asp:ListItem>
                                            <asp:ListItem Value="3">Non-Standard</asp:ListItem>
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Required" ControlToValidate="DDL_HelmetWhy" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="HelmetRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col3 text text-center">
                                        <label>Helmet Size<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_HelmetSize" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Free Size</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Required" ControlToValidate="DDL_HelmetSize" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="HelmetRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col4 text text-center">
                                        <asp:FileUpload ID="FU_Helemt" runat="server" CssClass="form-control form-control-sm rounded" />
                                        <asp:RequiredFieldValidator runat="server" Display="Dynamic" ErrorMessage="Required" ForeColor="Red" ControlToValidate="FU_Helemt" ValidationGroup="HelmetRequest" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:Button ID="btn_HelmetRequest" runat="server" ValidationGroup="HelmetRequest" CausesValidation="true" Text="Request" CssClass="btn btn-primary btn-sm" OnClick="btn_HelmetRequest_Click" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="col1 text text-center"><span>2.</span></td>
                                    <td class="col2 text text-left"><span>Safety Shoes</span></td>
                                    <td class="col3 text text-left">
                                        <asp:RadioButtonList ID="RBTN_SftyShoes" CssClass="" runat="server" CellPadding="5" Height="15px" CellSpacing="5" RepeatColumns="2" AutoPostBack="true" RepeatDirection="Horizontal" Width="100%" OnSelectedIndexChanged="RBTN_SftyShoes_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="0" Text="Ok">Ok</asp:ListItem>
                                            <asp:ListItem Value="1" Text="Not Ok">Not Ok</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                    <td class="col4 text text-center">
                                        <asp:Label ID="lbl_SftyShoes_rqid" runat="server" Text="N/A"></asp:Label></td>
                                </tr>
                                <tr id="SafetyShoes_NC" runat="server" visible="false">
                                    <td class="col1 text text-center">&nbsp;</td>
                                    <td class="col2 text text-center">
                                        <label>PPE Request Type<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_SftyShoesWhy" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">PPE Validity Expired</asp:ListItem>
                                            <asp:ListItem Value="2">Damaged</asp:ListItem>
                                            <asp:ListItem Value="3">Non-Standard</asp:ListItem>
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Required" ControlToValidate="DDL_SftyShoesWhy" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="SafetyShoesRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col3 text text-center">
                                        <label>Safety Shoes Size<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_SftyShoesSize" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1" Text="5">5</asp:ListItem>
                                            <asp:ListItem Value="2" Text="6">6</asp:ListItem>
                                            <asp:ListItem Value="3" Text="7">7</asp:ListItem>
                                            <asp:ListItem Value="4" Text="8">8</asp:ListItem>
                                            <asp:ListItem Value="5" Text="9">9</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Required" ControlToValidate="DDL_SftyShoesSize" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="SafetyShoesRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col4 text text-center">
                                        <asp:FileUpload ID="FU_SftyShoes" runat="server" CssClass="form-control form-control-sm rounded" />
                                        <asp:RequiredFieldValidator runat="server" Display="Dynamic" ErrorMessage="Required" ForeColor="Red" ControlToValidate="FU_SftyShoes" ValidationGroup="HelmetRequest" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:Button ID="btn_sftyshoesrequest" runat="server" ValidationGroup="SafetyShoesRequest" CausesValidation="true" Text="Request" CssClass="btn btn-primary btn-sm" OnClick="btn_sftyshoesrequest_Click" />
                                    </td>
                                </tr>


                                <tr>
                                    <td class="col1 text text-center"><span>3.</span></td>
                                    <td class="col2 text text-left"><span>Duty Shirt</span></td>
                                    <td class="col3 text text-left">
                                        <asp:RadioButtonList ID="RBTN_DutyShirt" CssClass="" runat="server" CellPadding="5" Height="15px" CellSpacing="5" RepeatColumns="2" AutoPostBack="true" RepeatDirection="Horizontal" Width="100%" OnSelectedIndexChanged="RBTN_DutyShirt_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="0" Text="Ok">Ok</asp:ListItem>
                                            <asp:ListItem Value="1" Text="Not Ok">Not Ok</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                    <td class="col4 text text-center">
                                        <asp:Label ID="lbl_DutyShirt_rqid" runat="server" Text="N/A"></asp:Label></td>
                                </tr>
                                <tr id="DutyShirt_NC" runat="server" visible="false">
                                    <td class="col1 text text-center">&nbsp;</td>
                                    <td class="col2 text text-center">
                                        <label>Duty Shirt Request Type<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_DutyShirtWhy" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">PPE Validity Expired</asp:ListItem>
                                            <asp:ListItem Value="2">Damaged</asp:ListItem>
                                            <asp:ListItem Value="3">Non-Standard</asp:ListItem>
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Required" ControlToValidate="DDL_DutyShirtWhy" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="DutyShirtRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col3 text text-center">
                                        <label>Duty Shirt Size<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_DutyShirtSize" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1" Text="S">S</asp:ListItem>
                                            <asp:ListItem Value="2" Text="M">M</asp:ListItem>
                                            <asp:ListItem Value="3" Text="L">L</asp:ListItem>
                                            <asp:ListItem Value="4" Text="XL">XL</asp:ListItem>
                                            <asp:ListItem Value="5" Text="XXL">XXL</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Required" ControlToValidate="DDL_DutyShirtSize" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="DutyShirtRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col4 text text-center">
                                        <asp:FileUpload ID="FU_DutyShirt" runat="server" CssClass="form-control form-control-sm rounded" />
                                        <asp:RequiredFieldValidator runat="server" Display="Dynamic" ErrorMessage="Required" ForeColor="Red" ControlToValidate="FU_DutyShirt" ValidationGroup="DutyShirtRequest" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:Button ID="btn_DutyShirtRequest" runat="server" ValidationGroup="DutyShirtRequest" CausesValidation="true" Text="Request" CssClass="btn btn-primary btn-sm" OnClick="btn_DutyShirtRequest_Click" />
                                    </td>
                                </tr>


                                <tr>
                                    <td class="col1 text text-center"><span>4.</span></td>
                                    <td class="col2 text text-left"><span>Duty Pant</span></td>
                                    <td class="col3 text text-left">
                                        <asp:RadioButtonList ID="RBTN_DutyPant" CssClass="" runat="server" CellPadding="5" Height="15px" CellSpacing="5" RepeatColumns="2" AutoPostBack="true" RepeatDirection="Horizontal" Width="100%" OnSelectedIndexChanged="RBTN_DutyPant_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="0" Text="Ok">Ok</asp:ListItem>
                                            <asp:ListItem Value="1" Text="Not Ok">Not Ok</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                    <td class="col4 text text-center">
                                        <asp:Label ID="lbl_DutyPant_rqid" runat="server" Text="N/A"></asp:Label></td>
                                </tr>
                                <tr id="DutyPant_NC" runat="server" visible="false">
                                    <td class="col1 text text-center">&nbsp;</td>
                                    <td class="col2 text text-center">
                                        <label>Duty Pant Request Type<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_DutyPantWhy" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">PPE Validity Expired</asp:ListItem>
                                            <asp:ListItem Value="2">Damaged</asp:ListItem>
                                            <asp:ListItem Value="3">Non-Standard</asp:ListItem>
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Required" ControlToValidate="DDL_DutyPantWhy" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="DutyPantRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col3 text text-center">
                                        <label>Duty Pant Size<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_DutyPantSize" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1" Text="28">28</asp:ListItem>
                                            <asp:ListItem Value="2" Text="30">30</asp:ListItem>
                                            <asp:ListItem Value="3" Text="32">32</asp:ListItem>
                                            <asp:ListItem Value="4" Text="34">34</asp:ListItem>
                                            <asp:ListItem Value="5" Text="36">36</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Required" ControlToValidate="DDL_DutyPantSize" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="DutyPantRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col4 text text-center">
                                        <asp:FileUpload ID="FU_DutyPant" runat="server" CssClass="form-control form-control-sm rounded" />
                                        <asp:RequiredFieldValidator runat="server" Display="Dynamic" ErrorMessage="Required" ForeColor="Red" ControlToValidate="FU_DutyPant" ValidationGroup="DutyPantRequest" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:Button ID="btn_DutyPantRequest" runat="server" ValidationGroup="DutyPantRequest" CausesValidation="true" Text="Request" CssClass="btn btn-primary btn-sm" OnClick="btn_DutyPantRequest_Click" />
                                    </td>
                                </tr>


                                <tr>
                                    <td class="col1 text text-center"><span>5.</span></td>
                                    <td class="col2 text text-left"><span>Safety Googles</span></td>
                                    <td class="col3 text text-left">
                                        <asp:RadioButtonList ID="RBTN_SafetyGoogles" CssClass="" runat="server" CellPadding="5" Height="15px" CellSpacing="5" RepeatColumns="2" AutoPostBack="true" RepeatDirection="Horizontal" Width="100%" OnSelectedIndexChanged="RBTN_SafetyGoogles_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="0" Text="Ok">Ok</asp:ListItem>
                                            <asp:ListItem Value="1" Text="Not Ok">Not Ok</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                    <td class="col4 text text-center">
                                        <asp:Label ID="lbl_SafetyGoogles_rqid" runat="server" Text="N/A"></asp:Label></td>
                                </tr>
                                <tr id="SafetyGoogles_NC" runat="server" visible="false">
                                    <td class="col1 text text-center">&nbsp;</td>
                                    <td class="col2 text text-center">
                                        <label>Safety Googles Request Type<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_SafetyGooglesWhy" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">PPE Validity Expired</asp:ListItem>
                                            <asp:ListItem Value="2">Damaged</asp:ListItem>
                                            <asp:ListItem Value="3">Non-Standard</asp:ListItem>
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Required" ControlToValidate="DDL_SafetyGooglesWhy" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="SafetyGooglesRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col3 text text-center">
                                        <label>Safety Googles Size<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_SafetyGooglesSize" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Free Size</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="Required" ControlToValidate="DDL_SafetyGooglesSize" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="SafetyGooglesRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col4 text text-center">
                                        <asp:FileUpload ID="FU_SafetyGoogles" runat="server" CssClass="form-control form-control-sm rounded" />
                                        <asp:RequiredFieldValidator runat="server" Display="Dynamic" ErrorMessage="Required" ForeColor="Red" ControlToValidate="FU_SafetyGoogles" ValidationGroup="SafetyGooglesRequest" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:Button ID="btn_SafetyGooglesRequest" runat="server" ValidationGroup="SafetyGooglesRequest" CausesValidation="true" Text="Request" CssClass="btn btn-primary btn-sm" OnClick="btn_SafetyGooglesRequest_Click" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="col1 text text-center"><span>6.</span></td>
                                    <td class="col2 text text-left"><span>Nose Mask</span></td>
                                    <td class="col3 text text-left">
                                        <asp:RadioButtonList ID="RBTN_NoseMask" CssClass="" runat="server" CellPadding="5" Height="15px" CellSpacing="5" RepeatColumns="2" AutoPostBack="true" RepeatDirection="Horizontal" Width="100%" OnSelectedIndexChanged="RBTN_NoseMask_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="0" Text="Ok">Ok</asp:ListItem>
                                            <asp:ListItem Value="1" Text="Not Ok">Not Ok</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                    <td class="col4 text text-center">
                                        <asp:Label ID="lbl_NoseMask_rqid" runat="server" Text="N/A"></asp:Label></td>
                                </tr>
                                <tr id="NoseMask_NC" runat="server" visible="false">
                                    <td class="col1 text text-center">&nbsp;</td>
                                    <td class="col2 text text-center">
                                        <label>Nose Mask Request Type<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_NoseMaskWhy" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">PPE Validity Expired</asp:ListItem>
                                            <asp:ListItem Value="2">Damaged</asp:ListItem>
                                            <asp:ListItem Value="3">Non-Standard</asp:ListItem>
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="Required" ControlToValidate="DDL_NoseMaskWhy" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="NoseMaskRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col3 text text-center">
                                        <label>Nose Mask Size<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_NoseMaskSize" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Free Size</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="Required" ControlToValidate="DDL_NoseMaskSize" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="NoseMaskRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col4 text text-center">
                                        <asp:FileUpload ID="FU_NoseMask" runat="server" CssClass="form-control form-control-sm rounded" />
                                        <asp:RequiredFieldValidator runat="server" Display="Dynamic" ErrorMessage="Required" ForeColor="Red" ControlToValidate="FU_NoseMask" ValidationGroup="NoseMaskRequest" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:Button ID="btn_NoseMaskRequest" runat="server" ValidationGroup="NoseMaskRequest" CausesValidation="true" Text="Request" CssClass="btn btn-primary btn-sm" OnClick="btn_NoseMaskRequest_Click" />
                                    </td>
                                </tr>


                                <tr>
                                    <td class="col1 text text-center"><span>7.</span></td>
                                    <td class="col2 text text-left"><span>COTTON RUBBER HAND GLOVES</span></td>
                                    <td class="col3 text text-left">
                                        <asp:RadioButtonList ID="RBTN_CottonGloves" CssClass="" runat="server" CellPadding="5" Height="15px" CellSpacing="5" RepeatColumns="2" AutoPostBack="true" RepeatDirection="Horizontal" Width="100%" OnSelectedIndexChanged="RBTN_CottonGloves_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="0" Text="Ok">Ok</asp:ListItem>
                                            <asp:ListItem Value="1" Text="Not Ok">Not Ok</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                    <td class="col4 text text-center">
                                        <asp:Label ID="lbl_CottonGloves_rqid" runat="server" Text="N/A"></asp:Label></td>
                                </tr>

                                <tr id="CottonGloves_NC" runat="server" visible="false">
                                    <td class="col1 text text-center">&nbsp;</td>
                                    <td class="col2 text text-center">
                                        <label>PPE Request Type<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_CottonGlovesWhy" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">PPE Validity Expired</asp:ListItem>
                                            <asp:ListItem Value="2">Damaged</asp:ListItem>
                                            <asp:ListItem Value="3">Non-Standard</asp:ListItem>
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="Required" ControlToValidate="DDL_CottonGlovesWhy" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="CottonGlovesRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col3 text text-center">
                                        <label>Safety Shoes Size<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_CottonGlovesSize" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Free Size</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="Required" ControlToValidate="DDL_CottonGlovesSize" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="CottonGlovesRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col4 text text-center">
                                        <asp:FileUpload ID="FU_CottonGlovesSize" runat="server" CssClass="form-control form-control-sm rounded" />
                                        <asp:RequiredFieldValidator runat="server" Display="Dynamic" ErrorMessage="Required" ForeColor="Red" ControlToValidate="FU_CottonGlovesSize" ValidationGroup="CottonGlovesRequest" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:Button ID="btn_CottonGlovesRqst" runat="server" ValidationGroup="CottonGlovesRequest" CausesValidation="true" Text="Request" CssClass="btn btn-primary btn-sm" OnClick="btn_CottonGlovesRqst_Click" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="col1 text text-center"><span>8.</span></td>
                                    <td class="col2 text text-left"><span>BLACK GOGGLES</span></td>
                                    <td class="col3 text text-left">
                                        <asp:RadioButtonList ID="RBTN_BlackGoogles" CssClass="" runat="server" CellPadding="5" Height="15px" CellSpacing="5" RepeatColumns="2" AutoPostBack="true" RepeatDirection="Horizontal" Width="100%" OnSelectedIndexChanged="RBTN_BlackGoogles_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="0" Text="Ok">Ok</asp:ListItem>
                                            <asp:ListItem Value="1" Text="Not Ok">Not Ok</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                    <td class="col4 text text-center">
                                        <asp:Label ID="lbl_BlackGoogles_rqid" runat="server" Text="N/A"></asp:Label></td>
                                </tr>

                                <tr id="BlackGoogles_NC" runat="server" visible="false">
                                    <td class="col1 text text-center">&nbsp;</td>
                                    <td class="col2 text text-center">
                                        <label>PPE Request Type<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_BlackGooglesWhy" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">PPE Validity Expired</asp:ListItem>
                                            <asp:ListItem Value="2">Damaged</asp:ListItem>
                                            <asp:ListItem Value="3">Non-Standard</asp:ListItem>
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="Required" ControlToValidate="DDL_SftyShoesWhy" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="SafetyShoesRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col3 text text-center">
                                        <label>Safety Shoes Size<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_BlackGooglesSize" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Free Size</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="Required" ControlToValidate="DDL_SftyShoesSize" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="SafetyShoesRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col4 text text-center">
                                        <asp:FileUpload ID="FU_BlackGoogles" runat="server" CssClass="form-control form-control-sm rounded" />
                                        <asp:RequiredFieldValidator runat="server" Display="Dynamic" ErrorMessage="Required" ForeColor="Red" ControlToValidate="FU_BlackGoogles" ValidationGroup="HelmetRequest" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:Button ID="btn_BlackGoogles" runat="server" ValidationGroup="SafetyShoesRequest" CausesValidation="true" Text="Request" CssClass="btn btn-primary btn-sm" OnClick="btn_BlackGoogles_Click" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="col1 text text-center"><span>9.</span></td>
                                    <td class="col2 text text-left"><span>PVC GLOVES</span></td>
                                    <td class="col3 text text-left">
                                        <asp:RadioButtonList ID="RBTN_PVCGloves" CssClass="" runat="server" CellPadding="5" Height="15px" CellSpacing="5" RepeatColumns="2" AutoPostBack="true" RepeatDirection="Horizontal" Width="100%" OnSelectedIndexChanged="RBTN_PVCGloves_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="0" Text="Ok">Ok</asp:ListItem>
                                            <asp:ListItem Value="1" Text="Not Ok">Not Ok</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                    <td class="col4 text text-center">
                                        <asp:Label ID="lbl_PVCGloves_rqid" runat="server" Text="N/A"></asp:Label></td>
                                </tr>

                                <tr id="PVCGloves_NC" runat="server" visible="false">
                                    <td class="col1 text text-center">&nbsp;</td>
                                    <td class="col2 text text-center">
                                        <label>PPE Request Type<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_PVCGlovesWhy" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">PPE Validity Expired</asp:ListItem>
                                            <asp:ListItem Value="2">Damaged</asp:ListItem>
                                            <asp:ListItem Value="3">Non-Standard</asp:ListItem>
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ErrorMessage="Required" ControlToValidate="DDL_PVCGlovesWhy" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="PVCGlovesRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col3 text text-center">
                                        <label>Safety Shoes Size<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_PVCGlovesSize" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Free Size</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="Required" ControlToValidate="DDL_PVCGlovesSize" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="PVCGlovesRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col4 text text-center">
                                        <asp:FileUpload ID="FU_PVCGloves" runat="server" CssClass="form-control form-control-sm rounded" />
                                        <asp:RequiredFieldValidator runat="server" Display="Dynamic" ErrorMessage="Required" ForeColor="Red" ControlToValidate="FU_PVCGloves" ValidationGroup="PVCGlovesRequest" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:Button ID="btn_PVCGlovesRequest" runat="server" ValidationGroup="PVCGlovesRequest" CausesValidation="true" Text="Request" CssClass="btn btn-primary btn-sm" OnClick="btn_PVCGlovesRequest_Click" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="col1 text text-center"><span>10.</span></td>
                                    <td class="col2 text text-left"><span>LEATHER HAND GLOVES</span></td>
                                    <td class="col3 text text-left">
                                        <asp:RadioButtonList ID="RBTN_LthrGloves" CssClass="" runat="server" CellPadding="5" Height="15px" CellSpacing="5" RepeatColumns="2" AutoPostBack="true" RepeatDirection="Horizontal" Width="100%" OnSelectedIndexChanged="RBTN_LthrGloves_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="0" Text="Ok">Ok</asp:ListItem>
                                            <asp:ListItem Value="1" Text="Not Ok">Not Ok</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                    <td class="col4 text text-center">
                                        <asp:Label ID="lbl_LthrGloves_rqid" runat="server" Text="N/A"></asp:Label></td>
                                </tr>

                                <tr id="LthrGloves_NC" runat="server" visible="false">
                                    <td class="col1 text text-center">&nbsp;</td>
                                    <td class="col2 text text-center">
                                        <label>PPE Request Type<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_LthrGlovesWhy" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">PPE Validity Expired</asp:ListItem>
                                            <asp:ListItem Value="2">Damaged</asp:ListItem>
                                            <asp:ListItem Value="3">Non-Standard</asp:ListItem>
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ErrorMessage="Required" ControlToValidate="DDL_LthrGlovesWhy" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="LthrGlovesRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col3 text text-center">
                                        <label>Safety Shoes Size<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_LthrGlovesSize" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Free Size</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ErrorMessage="Required" ControlToValidate="DDL_LthrGlovesSize" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="LthrGlovesRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col4 text text-center">
                                        <asp:FileUpload ID="FU_LthrGloves" runat="server" CssClass="form-control form-control-sm rounded" />
                                        <asp:RequiredFieldValidator runat="server" Display="Dynamic" ErrorMessage="Required" ForeColor="Red" ControlToValidate="FU_LthrGloves" ValidationGroup="LthrGlovesRequest" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:Button ID="btn_LthrGlovesRequest" runat="server" ValidationGroup="LthrGlovesRequest" CausesValidation="true" Text="Request" CssClass="btn btn-primary btn-sm" OnClick="btn_LthrGlovesRequest_Click" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="col1 text text-center"><span>11.</span></td>
                                    <td class="col2 text text-left"><span>LEG GUARD</span></td>
                                    <td class="col3 text text-left">
                                        <asp:RadioButtonList ID="RBTN_LegGard" CssClass="" runat="server" CellPadding="5" Height="15px" CellSpacing="5" RepeatColumns="2" AutoPostBack="true" RepeatDirection="Horizontal" Width="100%" OnSelectedIndexChanged="RBTN_LegGard_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="0" Text="Ok">Ok</asp:ListItem>
                                            <asp:ListItem Value="1" Text="Not Ok">Not Ok</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                    <td class="col4 text text-center">
                                        <asp:Label ID="lbl_LegGard_rqid" runat="server" Text="N/A"></asp:Label></td>
                                </tr>

                                <tr id="LegGard_NC" runat="server" visible="false">
                                    <td class="col1 text text-center">&nbsp;</td>
                                    <td class="col2 text text-center">
                                        <label>PPE Request Type<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_LegGardWhy" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">PPE Validity Expired</asp:ListItem>
                                            <asp:ListItem Value="2">Damaged</asp:ListItem>
                                            <asp:ListItem Value="3">Non-Standard</asp:ListItem>
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="Required" ControlToValidate="DDL_LegGardWhy" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="LegGardRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col3 text text-center">
                                        <label>Safety Shoes Size<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_LegGardSize" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Free Size</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ErrorMessage="Required" ControlToValidate="DDL_LegGardSize" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="LegGardRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col4 text text-center">
                                        <asp:FileUpload ID="FU_LegGard" runat="server" CssClass="form-control form-control-sm rounded" />
                                        <asp:RequiredFieldValidator runat="server" Display="Dynamic" ErrorMessage="Required" ForeColor="Red" ControlToValidate="FU_LegGard" ValidationGroup="LegGardRequest" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:Button ID="btn_LegGardRequest" runat="server" ValidationGroup="LegGardRequest" CausesValidation="true" Text="Request" CssClass="btn btn-primary btn-sm" OnClick="btn_LegGardRequest_Click" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="col1 text text-center"><span>12.</span></td>
                                    <td class="col2 text text-left"><span>HAND SLEEVE</span></td>
                                    <td class="col3 text text-left">
                                        <asp:RadioButtonList ID="RBTN_HandSleves" CssClass="" runat="server" CellPadding="5" Height="15px" CellSpacing="5" RepeatColumns="2" AutoPostBack="true" RepeatDirection="Horizontal" Width="100%" OnSelectedIndexChanged="RBTN_HandSleves_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="0" Text="Ok">Ok</asp:ListItem>
                                            <asp:ListItem Value="1" Text="Not Ok">Not Ok</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                    <td class="col4 text text-center">
                                        <asp:Label ID="lbl_HandSleves_rqid" runat="server" Text="N/A"></asp:Label></td>
                                </tr>

                                <tr id="HandSleves_NC" runat="server" visible="false">
                                    <td class="col1 text text-center">&nbsp;</td>
                                    <td class="col2 text text-center">
                                        <label>PPE Request Type<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_HandSlevesWhy" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">PPE Validity Expired</asp:ListItem>
                                            <asp:ListItem Value="2">Damaged</asp:ListItem>
                                            <asp:ListItem Value="3">Non-Standard</asp:ListItem>
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ErrorMessage="Required" ControlToValidate="DDL_HandSlevesWhy" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="HandSlevesRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col3 text text-center">
                                        <label>Safety Shoes Size<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_HandSlevesSize" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Free Size</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ErrorMessage="Required" ControlToValidate="DDL_HandSlevesSize" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="HandSlevesRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col4 text text-center">
                                        <asp:FileUpload ID="FU_HandSleves" runat="server" CssClass="form-control form-control-sm rounded" />
                                        <asp:RequiredFieldValidator runat="server" Display="Dynamic" ErrorMessage="Required" ForeColor="Red" ControlToValidate="FU_HandSleves" ValidationGroup="HandSlevesRequest" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:Button ID="btn_HandSlevesRequest" runat="server" ValidationGroup="HandSlevesRequest" CausesValidation="true" Text="Request" CssClass="btn btn-primary btn-sm" OnClick="btn_HandSlevesRequest_Click" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="col1 text text-center"><span>13.</span></td>
                                    <td class="col2 text text-left"><span>FR JACKET</span></td>
                                    <td class="col3 text text-left">
                                        <asp:RadioButtonList ID="RBTN_FRJacket" CssClass="" runat="server" CellPadding="5" Height="15px" CellSpacing="5" RepeatColumns="2" AutoPostBack="true" RepeatDirection="Horizontal" Width="100%" OnSelectedIndexChanged="RBTN_FRJacket_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="0" Text="Ok">Ok</asp:ListItem>
                                            <asp:ListItem Value="1" Text="Not Ok">Not Ok</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                    <td class="col4 text text-center">
                                        <asp:Label ID="lbl_FRJacket_rqid" runat="server" Text="N/A"></asp:Label></td>
                                </tr>

                                <tr id="FRJacket_NC" runat="server" visible="false">
                                    <td class="col1 text text-center">&nbsp;</td>
                                    <td class="col2 text text-center">
                                        <label>PPE Request Type<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_FRJacketWhy" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">PPE Validity Expired</asp:ListItem>
                                            <asp:ListItem Value="2">Damaged</asp:ListItem>
                                            <asp:ListItem Value="3">Non-Standard</asp:ListItem>
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ErrorMessage="Required" ControlToValidate="DDL_FRJacketWhy" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="FRJacketRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col3 text text-center">
                                        <label>Safety Shoes Size<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_FRJacketSize" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Free Size</asp:ListItem>
                                            <asp:ListItem Value="2" Text="S">S</asp:ListItem>
                                            <asp:ListItem Value="3" Text="M">M</asp:ListItem>
                                            <asp:ListItem Value="4" Text="L">L</asp:ListItem>
                                            <asp:ListItem Value="5" Text="xL">Xl</asp:ListItem>
                                            <asp:ListItem Value="6" Text="XXL">XXl</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ErrorMessage="Required" ControlToValidate="DDL_FRJacketSize" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="FRJacketRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col4 text text-center">
                                        <asp:FileUpload ID="FU_FRJacket" runat="server" CssClass="form-control form-control-sm rounded" />
                                        <asp:RequiredFieldValidator runat="server" Display="Dynamic" ErrorMessage="Required" ForeColor="Red" ControlToValidate="FU_SftyShoes" ValidationGroup="FRJacketRequest" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:Button ID="btn_FRJacketRequest" runat="server" ValidationGroup="FRJacketRequest" CausesValidation="true" Text="Request" CssClass="btn btn-primary btn-sm" OnClick="btn_FRJacketRequest_Click" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="col1 text text-center"><span>14.</span></td>
                                    <td class="col2 text text-left"><span>APRON</span></td>
                                    <td class="col3 text text-left">
                                        <asp:RadioButtonList ID="RBTN_Apron" CssClass="" runat="server" CellPadding="5" Height="15px" CellSpacing="5" RepeatColumns="2" AutoPostBack="true" RepeatDirection="Horizontal" Width="100%" OnSelectedIndexChanged="RBTN_Apron_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="0" Text="Ok">Ok</asp:ListItem>
                                            <asp:ListItem Value="1" Text="Not Ok">Not Ok</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                    <td class="col4 text text-center">
                                        <asp:Label ID="lbl_Apron_rqid" runat="server" Text="N/A"></asp:Label></td>
                                </tr>

                                <tr id="Apron_NC" runat="server" visible="false">
                                    <td class="col1 text text-center">&nbsp;</td>
                                    <td class="col2 text text-center">
                                        <label>PPE Request Type<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_ApronWhy" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">PPE Validity Expired</asp:ListItem>
                                            <asp:ListItem Value="2">Damaged</asp:ListItem>
                                            <asp:ListItem Value="3">Non-Standard</asp:ListItem>
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ErrorMessage="Required" ControlToValidate="DDL_ApronWhy" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="ApronRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col3 text text-center">
                                        <label>Safety Shoes Size<span class="text text-danger">*</span></label>
                                        <asp:DropDownList ID="DDL_ApronSize" runat="server" CssClass="form-control form-control-sm rounded">
                                            <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Free Size</asp:ListItem>
                                            <asp:ListItem Value="2" Text="S">S</asp:ListItem>
                                            <asp:ListItem Value="3" Text="M">M</asp:ListItem>
                                            <asp:ListItem Value="4" Text="L">L</asp:ListItem>
                                            <asp:ListItem Value="5" Text="xL">Xl</asp:ListItem>
                                            <asp:ListItem Value="6" Text="XXL">XXl</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ErrorMessage="Required" ControlToValidate="DDL_ApronSize" InitialValue="--Select--" Display="Dynamic" SetFocusOnError="true" ValidationGroup="ApronRequest" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="col4 text text-center">
                                        <asp:FileUpload ID="FU_Apron" runat="server" CssClass="form-control form-control-sm rounded" />
                                        <asp:RequiredFieldValidator runat="server" Display="Dynamic" ErrorMessage="Required" ForeColor="Red" ControlToValidate="FU_Apron" ValidationGroup="ApronRequest" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:Button ID="btn_ApronRequest" runat="server" ValidationGroup="ApronRequest" CausesValidation="true" Text="Request" CssClass="btn btn-primary btn-sm" OnClick="btn_ApronRequest_Click" />
                                    </td>
                                </tr>

                            </table>


                            <%--ADD button-1 start--%>
                            <div class="col-md-12" id="Panel2Buttons" runat="server" visible="false">
                                <div class="ln_solid"></div>
                                <div class="row center">
                                    <div class="col-md-6 center">
                                        <asp:Label ID="Label1" runat="server" Text="Click SUBMIT to ADD Data!!"></asp:Label>
                                    </div>
                                    <div class="col-md-6 center">
                                        <asp:Button ID="Button1" runat="server" Text="Home" CssClass="btn btn-danger btn-sm" PostBackUrl="~/bussiness/production/homepage.aspx" />
                                        <asp:Button ID="btn_draft" runat="server" Text="Draft" CssClass="btn btn-warning btn-sm" OnClick="btn_draft_Click" />
                                        <asp:Button ID="btn_finalsbmt" runat="server" Text="Submit" ValidationGroup="CreateSOPID" Enabled="true" CausesValidation="true" CssClass="btn btn-success btn-sm" OnClick="btn_finalsbmt_Click" />
                                    </div>
                                </div>
                            </div>
                            <%--ADD button-1 end--%>

                            <div class="row" id="Panel2Message" runat="server" visible="false">
                                <div class="col-md-12 col-sm-12" style="vertical-align: middle; text-align: center;">
                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/erp_images/success_gif.gif" Width="100px" Height="100px" />
                                    <asp:Label ID="lbl_panel2msg" runat="server" Text="ID Created..!" Font-Bold="true" Font-Size="Large"></asp:Label>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <%--Body Row END--%>
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
