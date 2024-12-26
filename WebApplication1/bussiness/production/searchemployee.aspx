<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="searchemployee.aspx.cs" Inherits="WebApplication1.bussiness.production.searchemployee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>Employee Search Page</h3>
                </div>
            </div>
            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_content">
                            <div class="novalidate">
                                <div class="field item form-group">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">Name / Workman SL<span class="required">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:DropDownList ID="DDL_SearchType" runat="server" class="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_SearchType_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="0">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">By Employee Name</asp:ListItem>
                                            <asp:ListItem Value="2">By Workman SL</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="field item form-group" id="Nameinputrow" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">Employee Name<span class="required">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_empname" class="form-control form-control-sm rounded" runat="server" placeholder="Enter Employee Name"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="field item form-group" id="WorkmanInput_Row" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">Employee Workman<span class="required">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_empworkman" class="form-control form-control-sm rounded" runat="server" placeholder="Enter Employee Workman"></asp:TextBox>
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


                            <%--ADD button start--%>
                            <div class="col-md-6 center-margin">
                                <div class="ln_solid"></div>
                                <div class="item form-group row">
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to ADD Data!!"></asp:Label>
                                    </div>
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="btn btn-danger btn-sm" OnClick="btn_cancel_Click" />
                                        <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="btn btn-warning btn-sm" OnClick="btn_reset_Click" />
                                        <asp:Button ID="btn_search" runat="server" Text="SEARCH" CssClass="btn btn-success btn-sm" OnClick="btn_search_Click" />
                                    </div>
                                </div>
                            </div>
                            <%--ADD button end--%>

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

                                            <asp:TemplateField HeaderText="Father Name" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Fathername" runat="server" Text='<%# Eval("Fathername") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Mobile" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_MobileNo" runat="server" Text='<%# Eval("MobileNo") %>' />
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

                            <div class="col-md-6 col-sm-6 profile_details" id="AttViewer" runat="server" visible="false">
                                <div class="well profile_view col-sm-12 col-lg-12">
                                    <div class="col-sm-12">
                                        <h4 class="brief"><i>Gatepass : </i>
                                            <asp:Label ID="lbl_gpno" runat="server" Text="TSL/ERP/00000" Font-Bold="true"></asp:Label></h4>
                                        <div class="right col-md-5 col-sm-6 text-center">
                                            <asp:Label ID="lbl_gpexpdays" runat="server" ForeColor="Green" Text="00" Font-Size="80px"></asp:Label>
                                            Days Left
                                        </div>
                                        <div class="left col-md-7 col-sm-6">
                                            <span style="text-align: center;"><strong>Gatepass Expiry : </strong>
                                                <asp:Label ID="lbl_gpvalidity" runat="server" Text="DD/MM/YYYY" ForeColor="Green" Font-Bold="true"></asp:Label>
                                            </span>
                                            <hr />
                                            <h2>Saftey No :
								<asp:Label ID="lbl_rfidno" runat="server" Text="TSL/ERP/00000" ForeColor="Green" Font-Bold="true"></asp:Label></h2>

                                            <span><strong>Safety Expiry : </strong>
                                                <asp:Label ID="lbl_rfidvalidity" runat="server" Text="DD/MM/YYYY" ForeColor="Green" Font-Bold="true"></asp:Label>,
								<asp:Label ID="lbl_rfiddays" runat="server" ForeColor="Green" Text="00"></asp:Label>
                                                Days Left.
                                            </span>
                                            <br />
                                            <span><strong>PV Expiry : </strong>
                                                <asp:Label ID="lbl_pvvalidity" runat="server" Text="DD/MM/YYYY" ForeColor="Green" Font-Bold="true"></asp:Label>,
								<asp:Label ID="lbl_pvdays" runat="server" ForeColor="Green" Text="00"></asp:Label>
                                                Days Left.
                                            </span>
                                        </div>
                                    </div>
                                    <div class=" profile-bottom text-center">
                                        <div class=" col-sm-6 emphasis">
                                            <p class="ratings">
                                                &nbsp;
                                            </p>
                                        </div>
                                        <div class="right col-sm-6 pull-right">
                                            <button type="button" class="btn btn-primary btn-sm" id="btnShowPopup2" data-toggle="modal" data-target="#myModal2">
                                                Update
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <%--- Up-loader Modal ---------START----%>
                            <div class="modal fade" id="myModal2" data-backdrop="static">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h4 class="modal-title">Update Gatepass Details</h4>
                                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                                        </div>
                                        <div class="modal-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">

                                                        <div class="col-md-6 col-sm-12 form-group">
                                                            <label>Gatepass No :<span class="text text-danger"></span></label>
                                                        </div>
                                                        <div class="col-md-6 col-sm-12 form-group">
                                                            <asp:Label ID="lbl_oldgpno" runat="server" Text="N/A" CssClass="form-control form-control-sm rounded" BackColor="#e6e6e6"></asp:Label>
                                                        </div>

                                                        <div class="col-md-6 col-sm-12 form-group" id="nwgprow1" runat="server" visible="false">
                                                            <label style="font-weight: bold; color: darkblue;">Enter New Gatepass No :<span class="text text-danger"></span></label>
                                                        </div>
                                                        <div class="col-md-6 col-sm-12 form-group" id="nwgprow2" runat="server" visible="false">
                                                            <asp:TextBox ID="txt_nwgpno" runat="server" CssClass="form-control form-control-sm rounded" BorderColor="Blue" BorderWidth="2px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="GPDATA" ControlToValidate="txt_nwgpno" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                                        </div>

                                                        <div class="col-md-6 col-sm-12 form-group">
                                                            <label>Gatepass Validity :<span class="text text-danger"></span></label>
                                                        </div>
                                                        <div class="col-md-6 col-sm-12 form-group">
                                                            <asp:Label ID="lbl_oldgpvalidity" runat="server" Text="N/A" CssClass="form-control form-control-sm rounded" BackColor="#e6e6e6"></asp:Label>
                                                        </div>

                                                        <div class="col-md-6 col-sm-12 form-group" id="nwgpvalrow1" runat="server" visible="false">
                                                            <label style="font-weight: bold; color: darkblue;">New Gatepass Validity :<span class="text text-danger"></span></label>
                                                        </div>
                                                        <div class="col-md-6 col-sm-12 form-group" id="nwgpvalrow2" runat="server" visible="false">
                                                            <asp:TextBox ID="txt_nwgpvalidity" runat="server" CssClass="form-control form-control-sm rounded" class='date' type="date" name="date" BorderColor="Blue" BorderWidth="2px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="GPDATA" ControlToValidate="txt_nwgpvalidity" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                                        </div>

                                                        <div class="col-md-6 col-sm-12 form-group">
                                                            <label>Safety No :<span class="text text-danger"></span></label>
                                                        </div>
                                                        <div class="col-md-6 col-sm-12 form-group">
                                                            <asp:Label ID="lbl_oldsftyno" runat="server" Text="N/A" CssClass="form-control form-control-sm rounded" BackColor="#e6e6e6"></asp:Label>
                                                        </div>

                                                        <div class="col-md-6 col-sm-12 form-group" id="nwsftyrow1" runat="server" visible="false">
                                                            <label style="font-weight: bold; color: darkblue;">Enter New Safety No :<span class="text text-danger"></span></label>
                                                        </div>
                                                        <div class="col-md-6 col-sm-12 form-group" id="nwsftyrow2" runat="server" visible="false">
                                                            <asp:TextBox ID="txt_nwsftyno" runat="server" CssClass="form-control form-control-sm rounded" BorderColor="Blue" BorderWidth="2px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="GPDATA" ControlToValidate="txt_nwsftyno" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                                        </div>

                                                        <div class="col-md-6 col-sm-12 form-group">
                                                            <label>Safety Validity :<span class="text text-danger"></span></label>
                                                        </div>
                                                        <div class="col-md-6 col-sm-12 form-group">
                                                            <asp:Label ID="lbl_oldsftyval" runat="server" Text="N/A" CssClass="form-control form-control-sm rounded"></asp:Label>
                                                        </div>

                                                        <div class="col-md-6 col-sm-12 form-group" id="nwrfidrow1" runat="server" visible="false">
                                                            <label style="font-weight: bold; color: darkblue;">New Safety Validity :<span class="text text-danger"></span></label>
                                                        </div>
                                                        <div class="col-md-6 col-sm-12 form-group" id="nwrfidrow2" runat="server" visible="false">
                                                            <asp:TextBox ID="txt_nwsftyvalidity" runat="server" CssClass="form-control form-control-sm rounded" class='date' type="date" name="date" BorderColor="Blue" BorderWidth="2px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="GPDATA" ControlToValidate="txt_nwsftyvalidity" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                                        </div>

                                                        <div class="col-md-6 col-sm-12 form-group">
                                                            <label>PV Validity :<span class="text text-danger"></span></label>
                                                        </div>
                                                        <div class="col-md-6 col-sm-12 form-group">
                                                            <asp:Label ID="lbl_oldpvvalidity" runat="server" Text="N/A" CssClass="form-control form-control-sm rounded"></asp:Label>
                                                        </div>

                                                        <div class="col-md-6 col-sm-12 form-group" id="nwpvrow1" runat="server" visible="false">
                                                            <label style="font-weight: bold; color: darkblue;">New PV Validity :<span class="text text-danger"></span></label>
                                                        </div>
                                                        <div class="col-md-6 col-sm-12 form-group" id="nwpvrow2" runat="server" visible="false">
                                                            <asp:TextBox ID="txt_nwpvvalidity" runat="server" CssClass="form-control form-control-sm rounded" class='date' type="date" name="date" BorderColor="Blue" BorderWidth="2px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="GPDATA" ControlToValidate="txt_nwpvvalidity" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <asp:Label ID="Label1" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button ID="btn_gtpsedit" runat="server" CausesValidation="true" ValidationGroup="GPDATA" Text="Make Changes" CssClass="btn btn-info btn-sm" OnClick="btn_gtpsedit_Click" />
                                            <asp:Button ID="btn_gtpscncl" runat="server" CausesValidation="false" Text="Cancel" CssClass="btn btn-warning btn-sm" OnClick="btn_gtpscncl_Click" />
                                            <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">Close</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%--- Up-loader Modal -------- END --%>
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

        function ShowPopup2() {
            $("#myModal2").modal("show");
        }
    </script>
</asp:Content>
