<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="job_outpunch.aspx.cs" Inherits="WebApplication1.bussiness.production.job_outpunch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>JOB Manpower Exit</h3>
                </div>
            </div>
            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Out-Punch Page <small>(Attendance)</small></h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="novalidate" id="OUTpunchPanel_Row" runat="server" visible="true">
                                <span class="section">Manpower Exit</span>

                                <div class="field item form-group">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">JOB ID<span class="required">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:DropDownList ID="DDL_JOBID" runat="server" class="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_JOBID_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="DDL_JOBID" runat="server" ErrorMessage="Required" Display="Dynamic" SetFocusOnError="true" InitialValue="--Select--"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="field item form-group" id="JOBIDDetails_Row" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">JOBID Details</label>
                                    <div class="col-form-label col-md-6 col-sm-6">
                                        <asp:Label ID="lbl_jobid" runat="server" Text="Label"></asp:Label>;<asp:Label ID="lbl_jobiddate" runat="server" Text="Label"></asp:Label>;
										<asp:Label ID="lbl_jobsite" runat="server" Text="Label"></asp:Label>;<asp:Label ID="lbl_jobsitecode" runat="server" Text="Label" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_jobcreatorname" runat="server" Text="Label"></asp:Label>;<asp:Label ID="lbl_creatorwrk" runat="server" Text="Label" Visible="false"></asp:Label>;
										<asp:Label ID="lbl_creatorregion" runat="server" Visible="false" Text="Label"></asp:Label>;<asp:Label ID="lbl_creatorcompany" runat="server" Visible="false" Text="Label"></asp:Label>;
										<asp:Label ID="lbl_crtrsitename" runat="server" Text="Label" Visible="false"></asp:Label>;<asp:Label ID="lbl_crtrsitecode" runat="server" Visible="false" Text="Label"></asp:Label>;
										<asp:Label ID="lbl_wrkordr" runat="server" Text="Label"></asp:Label>;<asp:Label ID="lbl_permitno" runat="server" Text="Label"></asp:Label>;
										<asp:Label ID="lbl_jobrgn" runat="server" Text="Label" Visible="false"></asp:Label>;
										<asp:Label ID="lbl_jobcompay" runat="server" Text="Label" Visible="false"></asp:Label>;<asp:Label ID="lbl_inchargename" runat="server" Text="Label"></asp:Label>;
										<asp:Label ID="lbl_inchargewrk" runat="server" Text="Label" Visible="false"></asp:Label>;<asp:Label ID="lbl_jobloc" runat="server" Text="Label"></asp:Label>;
										<asp:Label ID="lbl_jobshift" runat="server" Text="Label"></asp:Label>;<asp:Label ID="lbl_dept" runat="server" Text="Label" Visible="false"></asp:Label>
                                    </div>
                                </div>

                                <div class="x_panel" runat="server" id="CSMRow" visible="false">
                                    <div class="x_title">
                                        <h2>Daily Communications</h2>
                                        <ul class="nav navbar-right panel_toolbox">
                                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                            </li>
                                        </ul>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="x_content">
                                        <a class="btn btn-app" href="csm_toolboxtalk.aspx">
                                            <span class="badge bg-green">
                                                <asp:Label ID="lbl_tbtcount" runat="server" Text="0"></asp:Label></span>
                                            <i class="fa fa-edit"></i>TBT Talk

                                        </a>
                                        <a class="btn btn-app" href="csm_soptraining.aspx">
                                            <span class="badge bg-green">
                                                <asp:Label ID="lbl_sopcount" runat="server" Text="0"></asp:Label></span>
                                            <i class="fa fa-users"></i>SOP Training

                                        </a>
                                    </div>
                                </div>

                                <div class="row" id="IncompleteCSM" runat="server" visible="false">
                                    <div class="col-md-12 col-sm-12" style="vertical-align: middle; text-align: center;">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/erp_images/crossgif.gif" Width="100px" Height="100px" />
                                        <asp:Label ID="Label2" runat="server" Text="Incomplete CSM Documents!" Font-Bold="true" Font-Size="Large"></asp:Label>
                                    </div>
                                </div>

                                <div class="field item form-group" id="EmployeeName_Row" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3  label-align">Employee Name</label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_empname" runat="server" ReadOnly="true" class="form-control form-control-sm rounded"></asp:TextBox>
                                        <asp:Label ID="lbl_Id" runat="server" Text="0" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_empworkman" runat="server" Text="0" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_workhours" runat="server" Text="0" Visible="false"></asp:Label>
                                    </div>
                                </div>

                                <div class="field item form-group" id="InPunchDate_Row" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">IN Time</label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_inpunchtime" runat="server" CssClass="form-control form-control-sm rounded"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="field item form-group" id="OUTPunchDate_Row" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">OUT Date<span class="required">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_date" runat="server" CssClass="form-control form-control-sm rounded" class='date' type="date" name="date"></asp:TextBox>
                                        <small class="form-text text-muted ml-4">Example : 29-04-2021</small>
                                    </div>
                                </div>

                                <div class="field item form-group" id="OUTPunchTime_Row" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3  label-align">OUT Time<span class="required">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_time" runat="server" CssClass="form-control form-control-sm rounded" class='time' type="time" name="time"></asp:TextBox>
                                        <small class="form-text text-muted ml-4">Example : 05:00 PM</small>
                                    </div>
                                </div>

                                <div class="field item form-group" id="LunchFactorRow" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3  label-align">Lunch<span class="required">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:RadioButtonList ID="RBTN_LunchFactor" runat="server" CssClass="rounded" CellPadding="2" CellSpacing="5" RepeatDirection="Horizontal">
                                            <asp:ListItem>Yes</asp:ListItem>
                                            <asp:ListItem>No</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="PunchOUT_Button" runat="server" ErrorMessage="Reqired" ControlToValidate="RBTN_LunchFactor" Display="Dynamic" CssClass="text text-warning" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="field item form-group" id="AttenCode" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3  label-align">LunchAttendance Code (Applicable to Individuals)<span class="required">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:DropDownList ID="DDL_AttenCode" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFV3" ValidationGroup="PunchOUT_Button" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_AttenCode" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="field item form-group" id="OTRow" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3  label-align">Over Time (Hours)<span class="required">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_ot" runat="server" CssClass="form-control form-control-sm rounded" MaxLength="2" TextMode="Number"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="PunchOUT_Button" runat="server" ErrorMessage="Required" ControlToValidate="txt_ot" Display="Dynamic" CssClass="text text-warning" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="PunchOUT_Button" ControlToValidate="txt_ot" ErrorMessage="Value must be less than or equal to 16" Display="Dynamic" ClientValidationFunction="validateInput"></asp:CustomValidator>

                                    </div>
                                </div>
                            </div>

                            <%--ADD button start--%>
                            <div class="col-md-6 center-margin" id="PunchOUT_buttons" runat="server" visible="false">
                                <div class="ln_solid"></div>
                                <div class="item form-group row">
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Label ID="lbl_msg" runat="server" Text="Click SAVE to Punch OUT..!!"></asp:Label>
                                    </div>
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Button ID="btn_cancel" runat="server" Text="Cancel" CssClass="btn btn-danger btn-sm" CausesValidation="false" OnClick="btn_cancel_Click" />
                                        <button type="reset" class="btn btn-warning btn-sm">Reset</button>
                                        <asp:Button ID="btn_punchout" runat="server" Text="Punch OUT" CausesValidation="true" ValidationGroup="PunchOUT_Button" CssClass="btn btn-success btn-sm" OnClick="btn_punchout_Click" />
                                    </div>
                                </div>
                            </div>
                            <%--ADD button end--%>

                            <div class="row" id="NoPenidngPunch" runat="server" visible="false">
                                <div class="col-md-12 col-sm-12" style="vertical-align: middle; text-align: center;">
                                    <asp:Image ID="Img_Success" runat="server" ImageUrl="~/erp_images/success_gif.gif" Width="100px" Height="100px" />
                                    <asp:Label ID="Label1" runat="server" Text="No Pending OUT Punch..!" Font-Bold="true" Font-Size="Large"></asp:Label>
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

                            <div class="col-md-12 col-sm-12" id="ViewState_TableRow" runat="server" visible="false">
                                <div class="card-box table-responsive small">
                                    <asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowDeleting="GridView1_RowDeleting">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl. NO" Visible="True" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="ID" HeaderStyle-Width="10%" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Id" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Workman" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_EmployeeWrk" runat="server" Text='<%# Bind("EmployeeWrk") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Name" HeaderStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_EmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="IN Time" HeaderStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Inpunch_Time" runat="server" Text='<%# Bind("Inpunch_Time") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Punch OUT" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Button ID="PunchOUT" runat="server" CssClass="btn btn-sm btn-primary" Text="Punch OUT" Enabled="true" CommandArgument='<% #Eval("Id") %>' OnClick="PunchOUT_Click" />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Action" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btndelete" runat="server" CommandName="Delete" Height="15px" ImageUrl="~/erp_images/fi-sr-trash.svg" Width="15px" ToolTip="Delete" OnClientClick="return confirm('Do you want to DELETE...?')" ImageAlign="Middle" />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text text-center" />
                                            </asp:TemplateField>

                                        </Columns>
                                        <HeaderStyle CssClass="text text-center" />
                                        <EditRowStyle CssClass="bg-blue-sky" />
                                        <SelectedRowStyle CssClass="bg-blue-sky" />
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

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script type="text/javascript">
        function ShowPopup(title, body) {
            $("#MyPopup .modal-title").html(title);
            $("#MyPopup .modal-body").html(body);
            $("#MyPopup").modal("show");
        }
    </script>

    <script>
    function validateInput(sender, args) {
        var inputValue = $("#<%= txt_ot.ClientID %>").val();
        if (inputValue.trim() !== '') {
            var numericValue = parseInt(inputValue);
            if (isNaN(numericValue) || numericValue > 16) {
                args.IsValid = false;
            } else {
                args.IsValid = true;
            }
        } else {
            args.IsValid = false;
        }
    }
</script>


</asp:Content>
