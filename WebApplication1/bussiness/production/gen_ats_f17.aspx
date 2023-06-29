<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="gen_ats_f17.aspx.cs" Inherits="WebApplication1.bussiness.production.gen_ats_f17" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h5>Generate Form 17 (Trail / NO Deductions) </h5>
                </div>

                <div class="title_right">
                    <div class="col-md-5 col-sm-5 form-group pull-right top_search"></div>
                </div>
            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12  ">
                    <div class="x_panel">
                        <%--<div class="x_title">
							<h2>Report View Parameters</h2>
							<ul class="nav navbar-right panel_toolbox">
								<li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
							</ul>
							<div class="clearfix"></div>
						</div>--%>
                        <div class="x_content">
                            <div class="row">

                                <div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <label>Work Region <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <asp:DropDownList ID="DDL_Region" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Region_SelectedIndexChanged"></asp:DropDownList>
                                </div>

                                <div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <label>Select Company <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <asp:DropDownList ID="DDL_Company" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                </div>

                                <div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <label>Select Year <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <asp:DropDownList ID="DDL_Year" CssClass="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
                                </div>


                                <div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <label>Select Month <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <asp:DropDownList ID="DDL_Month" CssClass="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
                                </div>

                                <div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <label>Select START Day <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <asp:DropDownList ID="DDL_Day" CssClass="form-control form-control-sm rounded" runat="server" Visible="true"></asp:DropDownList>
                                </div>

                                <div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <label>Select END Day <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <asp:DropDownList ID="DDL_Y2" CssClass="form-control form-control-sm rounded" runat="server" Visible="false"></asp:DropDownList>
                                    <asp:DropDownList ID="DDL_M2" CssClass="form-control form-control-sm rounded" runat="server" Visible="false"></asp:DropDownList>
                                    <asp:DropDownList ID="DDL_D2" CssClass="form-control form-control-sm rounded" runat="server" Visible="true"></asp:DropDownList>
                                </div>

                                <div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <label>Select Employee Status <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <asp:DropDownList ID="DDL_EmpWorkStatus" CssClass="form-control form-control-sm rounded" runat="server">
                                        <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                        <asp:ListItem>All</asp:ListItem>
                                        <asp:ListItem>Active</asp:ListItem>
                                        <asp:ListItem>InActive</asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                                <div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <label>Calender Working Days <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-6 col-lg-2 form-group">
                                    <asp:DropDownList ID="DDL_Days" CssClass="form-control form-control-sm rounded" runat="server" Visible="true">
                                        <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                        <asp:ListItem>24</asp:ListItem>
                                        <asp:ListItem>25</asp:ListItem>
                                        <asp:ListItem>26</asp:ListItem>
                                        <asp:ListItem>27</asp:ListItem>
                                        <asp:ListItem>28</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <%--button   start--%>
                            <div class="col-md-6 center-margin">
                                <div class="ln_solid"></div>
                                <div class="item form-group row">
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Label ID="lbl_msg" runat="server" Text="It may Take some time...!!"></asp:Label>
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
            </div>

            <div class="col-md-12 col-sm-12  ">
                <div>&nbsp;</div>
            </div>

            <div class="row" id="f17grid" runat="server" visible="false">
                <div class="card-box col-md-12 col-sm-12 small" style="width: 1500px; height: 450px; overflow: scroll;">
                    <asp:GridView ID="GridView" runat="server" Width="100%" CssClass="table table-striped table-hover table-bordered table-sm" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnDataBound="GridView_DataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="W SL" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_WorkmanSL" runat="server" Text='<%# Bind("WorkmanSL") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Region" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_WorkRegion" runat="server" Text='<%# Bind("WorkRegion") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Employee Name" Visible="true" HeaderStyle-Width="25%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_FullName" runat="server" Text='<%# Bind("FullName") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="SKill Category" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SkillCategory" runat="server" Text='<%# Bind("SkillCategory") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Wage Rate" HeaderStyle-Width="2%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_payrate" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Designation" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SkillDesignation" runat="server" Text='<%# Bind("SkillDesignation") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Fixed Yes / No" HeaderStyle-Width="2%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_FixedSalary_YesNo" runat="server" Text='<%# Bind("FixedSalary_YesNo") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Fixed Salary" HeaderStyle-Width="2%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_FixedAmount" runat="server" Text='<%# Bind("FixedAmount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Fixed Rate" HeaderStyle-Width="2%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_fixedwagerate" runat="server"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="WH" HeaderStyle-Width="2%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_WorkHours" runat="server" Text='<%# Bind("WorkHours") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="OT F" HeaderStyle-Width="2%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_OTFactor" runat="server" Text='<%# Bind("OTFactor") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="OT M" HeaderStyle-Width="2%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_OTMultiplier" runat="server" Text='<%# Bind("OTMultiplier") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="OT Div" HeaderStyle-Width="2%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_OT_Divisibility" runat="server" Text='<%# Bind("OT_Divisibility") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Advance" HeaderStyle-Width="2%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Cur_Advance" runat="server" Text='<%# Bind("Cur_Advance") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Fines" HeaderStyle-Width="2%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Cur_Fines" runat="server" Text='<%# Bind("Cur_Fines") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Others" HeaderStyle-Width="2%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Cur_Others" runat="server" Text='<%# Bind("Cur_Others") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="DA / VDA" HeaderStyle-Width="2%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_DA_VDA" runat="server" Text='<%# Bind("DA_VDA") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="HRA" HeaderStyle-Width="2%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_HRA" runat="server" Text='<%# Bind("HRA") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Conv." HeaderStyle-Width="2%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Conv_Allowance" runat="server" Text='<%# Bind("Conv_Allowance") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Medical" HeaderStyle-Width="2%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Medical_Allowance" runat="server" Text='<%# Bind("Medical_Allowance") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Washing" HeaderStyle-Width="2%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Washing_Allowance" runat="server" Text='<%# Bind("Washing_Allowance") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="ATT" HeaderStyle-Width="2%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_ATT_Allowance" runat="server" Text='<%# Bind("ATT_Allowance") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="SPCL" HeaderStyle-Width="2%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SPCL_Allowance" runat="server" Text='<%# Bind("SPCL_Allowance") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="text text-center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Misc" HeaderStyle-Width="2%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Misc_Earnings" runat="server" Text='<%# Bind("Misc_Earnings") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="P" HeaderStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_presents" runat="server" Font-Bold="true" ForeColor="#009933"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="OT" HeaderStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_ttlot" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Basic Wages" HeaderStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_basicsalary" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Wages of Fix Rate" HeaderStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_fixratesal" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="OT Pay" HeaderStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_otwages" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="DA/ VDA Pay" HeaderStyle-Width="2%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_DaVdaPay" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="HRA Pay" HeaderStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_HRAPay" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Conv Pay" HeaderStyle-Width="2%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_ConvPay" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Med Pay" HeaderStyle-Width="2%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_MedPay" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Wash Pay" HeaderStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_WashPay" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="ATT Pay" HeaderStyle-Width="2%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_AttPay" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="SPCL Pay" HeaderStyle-Width="2%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SPCLPay" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Misc Pay" HeaderStyle-Width="2%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_MiscPay" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="G-BW Pay" HeaderStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_otherspay" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Actual Gross" HeaderStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_actualgross" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="ESIC Gross" HeaderStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_grossamount" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="PF Pay" HeaderStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_PFPay" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="ESIC Pay" HeaderStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_esicpay" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="NET Pay - 1" HeaderStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_netpay1" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Total Ded" HeaderStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_ttlded" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Final Net Pay-1" HeaderStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_netpayfnl" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="NET Pay - 2" HeaderStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_netpay2" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid" />
                                <ItemStyle CssClass="grid" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <div class="col-md-12 col-sm-12 center">
                <div class="col-md-12 col-sm-12 text-center">The Below buttons will be ACTIVE only after successfull loading for Trail F17 Data</div>
            </div>

            <div class="col-md-12 col-sm-12  ">
                <div>&nbsp;</div>
            </div>

            <div class="col-md-12 col-sm-12" style="text-align: center;">
                <div class="col-md-6 col-sm-6" style="text-align: center;">
                    <div class="col-md-3 col-sm-3" style="text-align: center;">
                        <asp:Button ID="btnExport" runat="server" Enabled="false" Text="Export To Excel" ToolTip="Click to Download Data in Excel Format" CssClass="btn btn-success btn-sm" OnClick="btn_excelexport_Click" />
                    </div>
                    <div class="col-md-3 col-sm-3" style="text-align: center;">
                        <asp:Button ID="btnInsertDB" runat="server" Enabled="false" Text="Export To Server" ToolTip="Click to SAVE Data in SERVER" CssClass="btn btn-warning btn-sm" OnClick="btnInsertDB_Click" />
                    </div>
                </div>

                <div class="col-md-6 col-sm-6" style="text-align: center;">
                    <div class="col-md-3 col-sm-3" style="text-align: center;">
                        <asp:Button ID="btn_f17print" runat="server" Enabled="false" Visible="true" Text="Print Form 17" ToolTip="Click to Print F17" CssClass="btn btn-primary btn-sm" OnClick="btn_f17print_Click" />
                    </div>
                    <div class="col-md-3 col-sm-3" style="text-align: center;">
                        <asp:Button ID="btn_f29print" runat="server" Enabled="false" Visible="true" Text="Print Form 29" ToolTip="Click to Print F29" CssClass="btn btn-primary btn-sm" OnClick="btn_f29print_Click" />
                    </div>
                </div>
            </div>

            <div class="col-md-12 col-sm-12  ">
                <div>&nbsp;</div>
            </div>

            <div class="col-md-12 col-sm-12  ">
                <div>&nbsp;</div>
            </div>

            <div class="col-md-12 col-sm-12  ">
                <div>&nbsp;</div>
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
            if (document.getElementById('<%=DDL_Region.ClientID%>').selectedIndex == 0) {
                document.getElementById('<%=DDL_Region.ClientID%>').focus();
                ShowPopup("Error :", "Work Region selection required...!");
                return false;
            }

            if (document.getElementById('<%=DDL_Company.ClientID%>').selectedIndex == 0) {
                document.getElementById('<%=DDL_Company.ClientID%>').focus();
                ShowPopup("Error :", "Work Company selection required...!");
                return false;
            }

            if (document.getElementById('<%=DDL_EmpWorkStatus.ClientID%>').selectedIndex == 0) {
                ShowPopup("Error :", "Employee Status selection required");
                document.getElementById('<%=DDL_EmpWorkStatus.ClientID%>').focus();
                return false;
            }
        }
    </script>
</asp:Content>
