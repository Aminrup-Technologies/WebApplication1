<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="generate_banksheets.aspx.cs" Inherits="WebApplication1.bussiness.production.generate_banksheets" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h5>Generate Bank Sheets || <asp:Button ID="Button1" runat="server" Text="Export" Enabled="false" CssClass="btn btn-primary btn-sm" OnClick="ExportExcel"/></h5>
                </div>

                <div class="title_right">
                    <div class="col-md-5 col-sm-5 form-group pull-right top_search"></div>
                </div>
            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12  ">
                    <div class="x_panel">
                        <div class="x_content">
                            <div class="row">

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Work Region <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_Region" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_Region_SelectedIndexChanged"></asp:DropDownList>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Select Company <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_Company" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Select Year <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_Year" CssClass="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
                                </div>


                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Select Month <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_Month" CssClass="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Select START Day <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_Day" CssClass="form-control form-control-sm rounded" runat="server" Visible="true"></asp:DropDownList>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Select END Day <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_Y2" CssClass="form-control form-control-sm rounded" runat="server" Visible="false"></asp:DropDownList>
                                    <asp:DropDownList ID="DDL_M2" CssClass="form-control form-control-sm rounded" runat="server" Visible="false"></asp:DropDownList>
                                    <asp:DropDownList ID="DDL_D2" CssClass="form-control form-control-sm rounded" runat="server" Visible="true"></asp:DropDownList>
                                </div>

                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Select Report Type <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:DropDownList ID="DDL_ReportType" CssClass="form-control form-control-sm rounded" runat="server">
                                        <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                        <asp:ListItem Value="1">NET Pay 1</asp:ListItem>
                                        <asp:ListItem Value="2">NET Pay 2</asp:ListItem>
                                    </asp:DropDownList>
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
                                        <asp:Button ID="btn_submit" runat="server" Text="Submit" CssClass="btn btn-success btn-sm" OnClientClick="return ValidateFormField()" OnClick="btn_submit_Click"/>
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
                    <div id="Grid" class="card-box table-responsive">
                        <asp:Label ID="lblTotalData" runat="server"></asp:Label>
                    </div>
                    <hr />
                    <asp:HiddenField ID="hfGridHtml" runat="server" />
                    <asp:Button ID="btnExport" runat="server" Enabled="false" Text="Export To Excel" CssClass="btn btn-success btn-sm" OnClick="btn_excelexport_Click" />
                    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
                    <script type="text/javascript">
                        $(function () {
                            $("[id*=btnExport]").click(function () {
                                $("[id*=hfGridHtml]").val($("#Grid").html());
                            });
                        });
                    </script>
                </div>

                <div class="col-md-12 col-sm-12" id="GridHolder" runat="server" visible="false">
                    <asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found">
                        <Columns>
							<asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="2%">
								<ItemTemplate>
									<asp:Label ID="lbl_SrNo" runat="server" Text='<%# Eval("SrNo") %>'></asp:Label>
								</ItemTemplate>
								<ItemStyle CssClass="text text-center" />
							</asp:TemplateField>

                            <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="10%">
								<ItemTemplate>
									<asp:Label ID="lbl_WorkmanSL" runat="server" Text='<%# Eval("WorkmanSL") %>' />
								</ItemTemplate>
								<HeaderStyle CssClass="GridHeaderText-Center" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

                            <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="10%">
								<ItemTemplate>
									<asp:Label ID="lbl_acc" runat="server" Text='<%# Eval("acc") %>' />
								</ItemTemplate>
								<HeaderStyle CssClass="GridHeaderText-Center" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

                            <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="10%">
								<ItemTemplate>
									<asp:Label ID="lbl_FullName" runat="server" Text='<%# Eval("FullName") %>' />
								</ItemTemplate>
								<HeaderStyle CssClass="GridHeaderText-Center" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

                            <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="10%">
								<ItemTemplate>
									<asp:Label ID="lbl_Payment_Account" runat="server" Text='<%# Eval("Payment_Account") %>' />
								</ItemTemplate>
								<HeaderStyle CssClass="GridHeaderText-Center" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

                            <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="10%">
								<ItemTemplate>
									<asp:Label ID="lbl_Payment_Bank" runat="server" Text='<%# Eval("Payment_Bank") %>' />
								</ItemTemplate>
								<HeaderStyle CssClass="GridHeaderText-Center" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

                            <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="10%">
								<ItemTemplate>
									<asp:Label ID="lbl_Payment_IFSC" runat="server" Text='<%# Eval("Payment_IFSC") %>' />
								</ItemTemplate>
								<HeaderStyle CssClass="GridHeaderText-Center" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

                            <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="10%">
								<ItemTemplate>
									<asp:Label ID="lbl_BankBranch" runat="server" Text='<%# Eval("BankBranch") %>' />
								</ItemTemplate>
								<HeaderStyle CssClass="GridHeaderText-Center" />
								<ItemStyle CssClass="grid" />
							</asp:TemplateField>

                            <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="10%">
								<ItemTemplate>
									<asp:Label ID="lbl_NetPay" runat="server" Text='<%# Eval("NetPay") %>' />
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

                <div class="col-md-12 col-sm-12  ">
                    <div>&nbsp;</div>
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

            if (document.getElementById('<%=DDL_Year.ClientID%>').selectedIndex == 0) {
                document.getElementById('<%=DDL_Year.ClientID%>').focus();
                ShowPopup("Error :", "Calender Year selection required...!");
                return false;
            }
        }
    </script>
</asp:Content>
