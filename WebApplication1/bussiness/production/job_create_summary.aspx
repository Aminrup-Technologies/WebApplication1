<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="job_create_summary.aspx.cs" Inherits="WebApplication1.bussiness.production.job_create_summary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="container">
            <%--<div class="page-title">
                <div class="title_left">
                    <h5>Create Memo Summary</h5>
                </div>
            </div>

            <div class="clearfix"></div>--%>

            <div class="row">
                <div class="col-md-12 col-sm-12  ">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Search Filters for Daily Manpower Supply / Line Items JOBS</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">

                            <div class="col-md-3">
                                <div class="mb-3">
                                    <asp:Label ID="Label5" runat="server" AssociatedControlID="DDL_ViewType" Text="View Type" ForeColor="Blue" Font-Bold="true" Font-Size="Small"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RFV_DDL_ViewType" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="DDL_ViewType" Display="Dynamic" InitialValue="0" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    <div class="input-group-sm">
                                        <asp:DropDownList ID="DDL_ViewType" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_ViewType_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Selected="True">--select--</asp:ListItem>
                                            <asp:ListItem Value="1">Supervisor</asp:ListItem>
                                            <asp:ListItem Value="2">Approver</asp:ListItem>
                                            <asp:ListItem Value="3">Billing Staff</asp:ListItem>
                                        </asp:DropDownList>

                                    </div>

                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="mb-3">
                                    <asp:Label ID="Label1" runat="server" AssociatedControlID="DDL_BillingType" Text="Billing Nature" ForeColor="Blue" Font-Bold="true" Font-Size="Small"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RFV_DDL_BillingType" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="DDL_BillingType" Display="Dynamic" InitialValue="0" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    <div class="input-group-sm">
                                        <asp:DropDownList ID="DDL_BillingType" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" ValidationGroup="Submit" OnSelectedIndexChanged="DDL_BillingType_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Selected="True">--select--</asp:ListItem>
                                            <asp:ListItem Value="MS">Manpower Supply</asp:ListItem>
                                            <asp:ListItem Value="LI">Line Item</asp:ListItem>
                                        </asp:DropDownList>

                                    </div>

                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="mb-3">
                                    <asp:Label ID="lbl_DDL_Workorder" runat="server" AssociatedControlID="DDL_Workorder" Text="P.O. - Worksite" ForeColor="Blue" Font-Bold="true" Font-Size="Small"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RFV_DDL_Workorder" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="DDL_Workorder" Display="Dynamic" InitialValue="0" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    <div class="input-group-sm">
                                        <asp:DropDownList ID="DDL_Workorder" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="false" ValidationGroup="Submit"></asp:DropDownList>

                                    </div>

                                </div>
                            </div>

                            <div class="col-md-3" id="Sup_Selector_Div" runat="server" visible="false">
                                <div class="mb-3">
                                    <asp:Label ID="lbl_DDL_Supervisor" runat="server" AssociatedControlID="DDL_Supervisor" Text="JOB Supervisor" ForeColor="Blue" Font-Bold="true" Font-Size="Small"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RFV_DDL_Supervisor" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="DDL_Supervisor" Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
                                    <div class="input-group-sm">
                                        <asp:DropDownList ID="DDL_Supervisor" runat="server" CssClass="form-control form-control-sm rounded" AutoPostBack="true" ValidationGroup="Submit">
                                            <asp:ListItem Enabled="False" Selected="True" Value="0">--select--</asp:ListItem>
                                        </asp:DropDownList>

                                    </div>

                                </div>
                            </div>


                            <div class="col-md-3">
                                <div class="mb-3">
                                    <asp:Label ID="lbl_txt_date1" runat="server" AssociatedControlID="txt_date1" Text="Date From Selection" ForeColor="Blue" Font-Bold="true" Font-Size="Small"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RFV_txt_date1" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txt_date1" Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
                                    <div class="input-group-sm">
                                        <asp:TextBox ID="txt_date1" runat="server" CssClass="form-control form-control-sm rounded" class='date' type="date" name="date" BorderColor="Blue" BorderWidth="2px"></asp:TextBox>
                                    </div>

                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="mb-3">
                                    <asp:Label ID="lbl_txt_date2" runat="server" AssociatedControlID="txt_date2" Text="Date To Selection" ForeColor="Blue" Font-Bold="true" Font-Size="Small"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RFV_txt_date2" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txt_date2" Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
                                    <div class="input-group-sm">
                                        <asp:TextBox ID="txt_date2" runat="server" CssClass="form-control form-control-sm rounded" class='date' type="date" name="date" BorderColor="Blue" BorderWidth="2px"></asp:TextBox>
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
                        </div>
                    </div>

                    <%--button start--%>
                    <div class="col-md-6 center-margin">
                        <%--<div class="ln_solid"></div>--%>
                        <div class="item form-group row">
                            <div class="col-md-6 col-sm-12">
                                <asp:Label ID="lbl_msg" runat="server" AssociatedControlID="btn_submit" Text="Click SUBMIT to view Data!!"></asp:Label>
                            </div>
                            <div class="col-md-6 col-sm-12">
                                <button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
                                <button type="reset" class="btn btn-warning btn-sm">Reset</button>
                                <asp:Button ID="btn_submit" runat="server" Text="Submit" CssClass="btn btn-success btn-sm" CausesValidation="true" ValidationGroup="Submit" OnClick="btn_submit_Click" />
                            </div>
                        </div>
                    </div>
                    <%--button end--%>
                </div>


                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>View and Select JOBID(s) for Memo Summary Creation</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row">
                                <div class="card-box col-md-12 col-sm-12 small" style="width: 100%; height: 450px; overflow: scroll;">
                                    <asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-hover table-bordered table-responsive table-sm table-condensed text-wrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="3%">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="checkAll" Text="Check All" runat="server" onclick="checkAll(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkone" runat="server" onclick="Check_Click(this)" />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center" />
                                                <HeaderStyle CssClass="text text-center"/>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="2%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="DBID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="DBID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Creator_Workman" runat="server" Text='<%# Eval("Creator_Workman") %>' /><br />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="JOB Date & JOB Shift" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_CreatedDate" runat="server" Text='<%# Eval("CreatedDate","{0:dd-MM-yyyy}") %>' /><br />
                                                    <asp:Label ID="lbl_JOB_Shift" runat="server" Text='<%# Eval("JOB_Shift") %>' /> Shift
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="JOBID & Supply Memo ID" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <%--<asp:Button ID="btn_viewdetails" runat="server" Text='<%# Eval("JOBID") %>' Font-Size="Smaller" CssClass="btn btn-sm btn-info" CommandName="View_Details" CommandArgument="<%# Container.DataItemIndex %>" />--%>
                                                    <asp:Label ID="lbl_JOBID" runat="server" Text='<%# Eval("JOBID") %>' Visible="true" /><br />
                                                    <asp:Label ID="lbl_JOBID_Status" runat="server" Text='<%# Eval("JOBID_Status") %>' Visible="true" Font-Bold="true" ForeColor="#0033cc" />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="JOB WorkSite & Worksite In-charge" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_JOB_Site" runat="server" Text='<%# Eval("JOB_Site") %>' /><br />
                                                    <asp:Label ID="lbl_JOB_InchargeName" runat="server" Text='<%# Eval("JOB_InchargeName") %>' Font-Bold="true" />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center small" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="WorkOrder No & Permit No" HeaderStyle-Width="7%" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_WorkOrderNo" runat="server" Text='<%# Eval("WorkOrderNo") %>' /><br />
                                                    <asp:Label ID="lbl_JOB_PermitNo" runat="server" Font-Bold="true" Text='<%# Eval("JOB_PermitNo") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="JOB Title" HeaderStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_JOB_Title" runat="server" Text='<%# Eval("JOB_Title").ToString().Length > 50? (Eval("JOB_Title") as string).Substring(0,50) + " ..." : Eval("JOB_Title")  %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-wrap text-justify" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Work Shift Details" HeaderStyle-Width="10%" Visible="true">
                                                <ItemTemplate>
                                                    HS : <asp:Label ID="lbl_Total_HSShiftCount" runat="server" Font-Bold="true" Text='<%# Eval("Total_HSShiftCount") %>' />
                                                    || S : <asp:Label ID="lbl_Total_SShiftCount" runat="server" Font-Bold="true" Text='<%# Eval("Total_SShiftCount") %>' /><br />
                                                    SS : <asp:Label ID="lbl_Total_SSShiftCount" runat="server" Font-Bold="true" Text='<%# Eval("Total_SSShiftCount") %>' />
                                                    || US : <asp:Label ID="lbl_Total_USShiftCount" runat="server" Font-Bold="true" Text='<%# Eval("Total_USShiftCount") %>' /><br />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Total Work Shift" HeaderStyle-Width="5%" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Total_ShiftCount" runat="server" Text='<%# Eval("Total_ShiftCount") %>' /><br />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="text text-center bg-light"/>
                                        <EmptyDataTemplate>
                                            <div class="grid">No Data Found</div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>

                    <%--button start--%>
                    <div class="col-md-6 center-margin" id="SummaryCreate_Div" runat="server" visible="false">
                        <%--<div class="ln_solid"></div>--%>
                        <div class="item form-group row">
                            <div class="col-md-6 col-sm-12">
                                <asp:Label ID="lbl_btn_summary" runat="server" AssociatedControlID="btn_summary" Text="Click CREATE to generate summary Data!!"></asp:Label>
                            </div>
                            <div class="col-md-6 col-sm-12">
                                <button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
                                <button type="reset" class="btn btn-warning btn-sm">Reset</button>
                                <asp:Button ID="btn_summary" runat="server" Text="Create" CssClass="btn btn-success btn-sm" CausesValidation="true" ValidationGroup="Submit" OnClientClick="return validateCheckBoxes();" OnClick="btn_summary_Click"/>
                            </div>
                        </div>
                    </div>
                    <%--button end--%>
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

        function Check_Click(objRef) {
            var row = objRef.parentNode.parentNode;
            if (objRef.checked) {
                row.style.backgroundColor = "#66FF90";
            }
            else {
                if (row.rowIndex % 2 == 0) {
                    row.style.backgroundColor = "#E5E8E8";
                }
                else {
                    row.style.backgroundColor = "white";
                }
            }
            var GridView = row.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var headerCheckBox = inputList[0];
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;
        }

        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        row.style.backgroundColor = "#66FF90";
                        inputList[i].checked = true;
                    }
                    else {
                        if (row.rowIndex % 2 == 0) {
                            row.style.backgroundColor = "#E5E8E8";
                        }
                        else {
                            row.style.backgroundColor = "white";
                        }
                        inputList[i].checked = false;
                    }
                }
            }
        }

        function validateCheckBoxes() {
            var checkboxes = document.querySelectorAll('[id*=chkone]');
            var checked = Array.prototype.slice.call(checkboxes).some(function (checkbox) {
                return checkbox.checked;
            });

            if (!checked) {
                alert("Please select at least one line item.");
                return false;
            }

            return true;
        }
    </script>
</asp:Content>
