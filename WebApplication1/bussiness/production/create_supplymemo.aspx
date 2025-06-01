<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="create_supplymemo.aspx.cs" Inherits="WebApplication1.bussiness.production.create_supplymemo" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="container">
            <div class="page-title">
                <div class="title_left">
                    <h5>Create Daily Manpower Supply Memo</h5>
                </div>
            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12  ">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>JOB Basic Details</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">

                            <%--form elements div ---- start--%>
                            <div class="row">

                                <div class="col-md-2 col-sm-12  form-group">
                                    <label>Description of JOB </label>
                                </div>
                                <div class="col-md-10 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_jobtitle" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" TextMode="MultiLine" Rows="1" Font-Bold="true" ForeColor="Black"></asp:TextBox>
                                </div>


                                <div class="col-md-2 col-sm-6  form-group">
                                    <label>JOB ID </label>
                                </div>
                                <div class="col-md-2 col-sm-6  form-group">
                                    <asp:TextBox ID="txt_jobid" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" Font-Bold="true" ForeColor="Blue"></asp:TextBox>
                                </div>

                                <div class="col-md-2 col-sm-12  form-group">
                                    <label>JOB Execution Date</label>
                                </div>
                                <div class="col-md-2 col-sm-12  form-group">
                                    <asp:Label ID="lbl_jobdaydetails" runat="server" Text="Label" Font-Bold="true" ForeColor="Blue"></asp:Label>
                                    <asp:TextBox ID="txt_jobdate" class="form-control form-control-sm rounded" Visible="false" runat="server" ReadOnly="true" Font-Size="smaller" Font-Bold="true" ForeColor="Blue"></asp:TextBox>
                                </div>

                                <div class="col-md-2 col-sm-12 form-group" runat="server" visible="false">
                                    <label>JOB Execution Day</label>
                                </div>
                                <div class="col-md-2 col-sm-12  form-group" runat="server" visible="false">
                                    <asp:TextBox ID="txt_jobday" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" Font-Bold="true" ForeColor="Blue"></asp:TextBox>
                                </div>

                                <div class="col-md-2 col-sm-12  form-group" runat="server" visible="false">
                                    <label>JOB Shift </label>
                                </div>
                                <div class="col-md-2 col-sm-12  form-group" runat="server" visible="false">
                                    <asp:TextBox ID="txt_jobshift" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" Font-Bold="true" ForeColor="Blue"></asp:TextBox>
                                </div>

                                <div class="col-md-2 col-sm-12  form-group">
                                    <label>Work Order No </label>
                                </div>
                                <div class="col-md-2 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_workorderno" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" Font-Bold="true" ForeColor="Brown"></asp:TextBox>
                                </div>

                                <div class="col-md-2 col-sm-12  form-group">
                                    <label>Work Permit No </label>
                                </div>
                                <div class="col-md-2 col-sm-12  form-group">
                                    <%--<asp:TextBox ID="txt_permitno" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" Font-Bold="true" ForeColor="Brown"></asp:TextBox>--%>
                                    <asp:TextBox ID="txt_permitno" runat="server" ReadOnly="true" Font-Bold="true"
                                        ForeColor="Brown" MaxLength="100" class="form-control form-control-sm rounded">
                                    </asp:TextBox>

                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                        ControlToValidate="txt_permitno" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Only numeric values separated by commas" ValidationExpression="^((?i:na|n/a)|\d+(\s*,\s*\d+)*)$">
                                    </asp:RegularExpressionValidator>

                                    <small class="form-text text-muted ml-4">Example: 5896523, 234234, 456456. Max 100 characters.</small>
                                </div>

                                <%--<div class="col-md-2 col-sm-12  form-group">
									<label>JOB ID Status </label>
								</div>
								<div class="col-md-2 col-sm-12  form-group">
									<asp:Label ID="lbl_jobidsstatus" runat="server" Text="N/A" Font-Bold="true" ForeColor="Red"></asp:Label>
								</div>--%>

                                <div class="col-md-2 col-sm-12  form-group">
                                    <label>Work Site Name </label>
                                </div>
                                <div class="col-md-2 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_worksitename" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" Font-Size="Small"></asp:TextBox>
                                    <asp:Label ID="lbl_worksitedbcode" runat="server" Text="Label" Visible="false"></asp:Label>
                                    <asp:Label ID="lbl_jobrgn" runat="server" Text="Label" Visible="false"></asp:Label>
                                    <asp:Label ID="lbl_jobcompay" runat="server" Text="Label" Visible="false"></asp:Label>
                                </div>

                                <div class="col-md-2 col-sm-12  form-group">
                                    <label>JOB Department </label>
                                </div>
                                <div class="col-md-2 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_jobdept" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" Font-Size="Small"></asp:TextBox>
                                </div>

                                <div class="col-md-2 col-sm-12  form-group">
                                    <label>JOB Work Location </label>
                                </div>
                                <div class="col-md-2 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_jobloc" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" Font-Size="Small"></asp:TextBox>
                                </div>

                                <div class="col-md-2 col-sm-12  form-group">
                                    <label>JOB Executor </label>
                                </div>
                                <div class="col-md-2 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_jobsupv" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" Font-Bold="true" ForeColor="DarkBlue"></asp:TextBox>
                                    <asp:Label ID="lbl_creatorwrk" runat="server" Text="N/A" Visible="true" Font-Bold="true" ForeColor="Black"></asp:Label>
                                </div>


                                <div class="col-md-2 col-sm-12  form-group">
                                    <label>Site In-Charge Name </label>
                                </div>
                                <div class="col-md-2 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_inchargename" class="form-control form-control-sm rounded" runat="server" ReadOnly="true" Font-Bold="true" ForeColor="DarkBlue"></asp:TextBox>
                                    <asp:Label ID="lbl_inchargewrk" runat="server" Text="N/A" Visible="true" Font-Bold="true" ForeColor="Black"></asp:Label>
                                </div>

                                <div class="col-md-2 col-sm-12  form-group">
                                    <label>Approver Remarks </label>
                                </div>
                                <div class="col-md-10 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_approverrmrks" class="form-control form-control-sm rounded" runat="server" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <%--form elements div ---- end--%>

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

                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Attached Permit Data</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row">
                                <div class="col-md-12 col-sm-12">
                                    <div class="card-box table-responsive">
                                        <p class="text-muted font-13 m-b-30">
                                            Last attached permit file deleted on :
											<asp:Label ID="lbl_permitdeleteddate" runat="server" Text="__________"></asp:Label>, by
											<asp:Label ID="lbl_permitdeletedby" runat="server" Text="______________"></asp:Label>
                                        </p>
                                        <asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap small" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowDeleting="GridView1_RowDeleting">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="DB ID" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="JOBID" Visible="false" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_JOBID" runat="server" Text='<%# Bind("JOBID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="File Name" HeaderStyle-Width="40%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Name" runat="server" Text='<%# Eval("Name") %>' Font-Size="Small" />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="View" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("Id") %>' OnClick="DownloadFile" ToolTip="Click to Download" CommandName="Download"><span class="glyphicon glyphicon-save" aria-hidden='true'></span></asp:LinkButton>
                                                        <asp:Label ID="lbl_dbid" runat="server" Visible="false" Text='<%# Bind("Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Time" HeaderStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_TimeStamp" runat="server" Text='<%# Eval("TimeStamp") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="5%" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btndelete" runat="server" CommandName="Delete" Height="15px" ImageUrl="~/erp_images/fi-sr-trash.svg" Width="15px" ToolTip="Delete" OnClientClick="return confirm('Do you want to DELETE...?')" ImageAlign="Middle" />
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
                            </div>
                        </div>
                    </div>
                </div>


                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Attached Manpower Data & Details</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                                <%--<li><a class="close-link"><i class="fa fa-close"></i></a></li>--%>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <asp:GridView BorderWidth="0" ID="GridView2" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap small" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found"
                                OnRowCancelingEdit="GridView2_RowCancelingEdit" OnRowDeleting="GridView2_RowDeleting" OnRowEditing="GridView2_RowEditing" OnRowDataBound="GridView2_RowDataBound" OnRowUpdating="GridView2_RowUpdating" OnRowCommand="GridView2_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl" Visible="True" HeaderStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text text-center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="ID" HeaderStyle-Width="0%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Id" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text text-center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="JOBID" Visible="false" HeaderStyle-Width="0%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_JOBID" runat="server" Text='<%# Bind("JOBID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text text-center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="JOB Date" Visible="false" HeaderStyle-Width="0%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_CreatedDate" runat="server" Text='<%# Bind("CreatedDate","{0:dd-MM-yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text text-center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Region" HeaderStyle-Width="0%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_JOB_Region" runat="server" Text='<%# Bind("JOB_Region") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text text-center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Company" HeaderStyle-Width="0%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_JOB_Company" runat="server" Text='<%# Bind("JOB_Company") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text text-center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="{Employee Name} [Workmen] [Work Hours]" HeaderStyle-Width="18%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_EmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                                            [<asp:Label ID="lbl_EmployeeWrk" runat="server" Text='<%# Bind("EmployeeWrk") %>' Font-Bold="true" ForeColor="Black"></asp:Label>]
											[<asp:Label ID="lbl_WourkHours" runat="server" Text='<%# Bind("WourkHours") %>'></asp:Label>]
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text text-left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Pay Category" HeaderStyle-Width="10%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_EmpCategory" runat="server" Text='<%# Bind("EmpCategory") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text text-center small bold" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="PO Skill" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_PO_SkillCategory" runat="server" Text='<%# Bind("PO_SkillCategory") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="DDL_EmpCategory" class="form-control form-control-sm rounded no-padding" runat="server" AutoPostBack="true"></asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemStyle CssClass="text text-center small bold text-primary" Font-Bold="true" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="PO Designation" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_PO_EmpDesignation" runat="server" Text='<%# Bind("PO_EmpDesignation") %>'></asp:Label>
                                        </ItemTemplate>
                                        <%--<EditItemTemplate>
                                            <asp:DropDownList ID="DDL_EmpDesignation" class="form-control form-control-sm rounded no-padding" runat="server" Enabled="true"></asp:DropDownList>
                                        </EditItemTemplate>--%>
                                        <ItemStyle CssClass="text text-center text-info" Font-Bold="true" />
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Site Name" HeaderStyle-Width="0%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Employee_Worksite" runat="server" Text='<%# Bind("Employee_Worksite") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text text-center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Site Code" HeaderStyle-Width="0%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Employee_WorksiteCode" runat="server" Text='<%# Bind("Employee_WorksiteCode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text text-center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="IN Punch Time" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Inpunch_Time" runat="server" Text='<%# Bind("Inpunch_Time","{0:dd-MM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <%--<EditItemTemplate>
                                            <asp:TextBox ID="txt_Inpunch_Time" runat="server" class="form-control form-control-sm rounded no-padding" Text='<%# DataBinder.Eval(Container.DataItem,"Inpunch_Time", "{0:yyyy-MM-dd hh:mm:ss tt}") %> ' Width="100%"></asp:TextBox>
                                        </EditItemTemplate>--%>
                                        <ItemStyle CssClass="text text-center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="OUT Punch Time" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Outpunch_Time" runat="server" Text='<%# Bind("Outpunch_Time","{0:dd-MM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <%--<EditItemTemplate>
                                            <asp:TextBox ID="txt_Outpunch_Time" runat="server" class="form-control form-control-sm rounded no-padding" Text='<%# DataBinder.Eval(Container.DataItem,"Outpunch_Time", "{0:yyyy-MM-dd hh:mm:ss tt}") %> ' Width="100%"></asp:TextBox>
                                        </EditItemTemplate>--%>
                                        <ItemStyle CssClass="text text-center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Work Time" HeaderStyle-Width="0%" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_WorkedHours" runat="server" Text='<%# Bind("WorkedHours") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text text-center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Lunch" HeaderStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_LunchFactor" runat="server" Text='<%# Bind("LunchFactor") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="DDL_LunchYesNo" runat="server" class="form-control form-control-sm rounded no-padding" SelectedValue='<%# Bind("LunchFactor") %>'>
                                                <asp:ListItem Text="Yes" Value="Yes">Yes</asp:ListItem>
                                                <asp:ListItem Text="No" Value="No">No</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemStyle CssClass="text text-center" />
                                    </asp:TemplateField>



                                    <asp:TemplateField HeaderText="Gievn OT" HeaderStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ProvidedOT" runat="server" Text='<%# Bind("ProvidedOT") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txt_ProvidedOT" runat="server" class="form-control form-control-sm rounded no-padding" Text='<%# DataBinder.Eval(Container.DataItem,"ProvidedOT") %> ' Width="100%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemStyle CssClass="text text-center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Atten Status" HeaderStyle-Width="0%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_AttendanceStatus" runat="server" Text='<%# Bind("AttendanceStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="DDL_AttendanceStatus" class="form-control form-control-sm rounded no-padding" runat="server"></asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemStyle CssClass="text text-center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Atten. Code" HeaderStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_AttendanceCode" runat="server" Text='<%# Bind("AttendanceCode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="DDL_AttendanceCode" class="form-control form-control-sm rounded" runat="server"></asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemStyle CssClass="text text-center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Shift" HeaderStyle-Width="0%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ShiftCalc" runat="server" Text='<%# Bind("ShiftCalc") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text text-center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="GP No" HeaderStyle-Width="5%" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_GatePassNo" runat="server" Text='<%# Bind("GatePassNo") %>'></asp:Label>
                                            <%--<asp:Label ID="lbl_GatePassExpiry" runat="server" Text='<%# Bind("GatePassExpiry") %>' DataFormatString="{0:d}" Font-Bold="true" Visible="true" ForeColor="Blue"></asp:Label>--%>
                                        </ItemTemplate>
                                        <%--<EditItemTemplate>
											<asp:TextBox ID="txt_GatePassExpiry" ReadOnly="true" Visible="true" runat="server" class="date-picker form-control rounded" placeholder="yyyy-mm-dd" required="required" onfocus="this.type='date'" Text='<%# DataBinder.Eval(Container.DataItem,"GatePassExpiry", "{0:yyyy-MM-dd}")%>' onmouseover="this.type='date'" onclick="this.type = 'date'" onblur="this.type='text'" onmouseout="timeFunctionLong(this)"></asp:TextBox>
											<script>
												function timeFunctionLong(txt_DOB) {
													setTimeout(function () {
														txt_DOB.type = 'text';
													}, 60000);
												}
											</script>
										</EditItemTemplate>--%>

                                        <ItemStyle CssClass="text text-center small bold" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Safety No" HeaderStyle-Width="5%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_SafetyPassNo" runat="server" Text='<%# Bind("SafetyPassNo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text text-center small bold" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action" HeaderStyle-Width="7%" Visible="true">
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="btnupdate" runat="server" CommandName="Update" Height="15px" ImageUrl="~/erp_images/fi-sr-disk.svg" Width="15px" ToolTip="Save" ImageAlign="Middle" />
                                            <asp:ImageButton ID="Btncancale" runat="server" CommandName="Cancel" Height="15px" ImageUrl="~/erp_images/fi-sr-cross-circle.svg" Width="15px" ToolTip="Cancel Update" ImageAlign="Middle" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnedit" runat="server" Visible="true" CommandName="Edit" Height="15px" ImageUrl="~/erp_images/fi-sr-pencil.svg" Width="15px" ToolTip="Update" ImageAlign="Middle" />
                                            <asp:ImageButton ID="btndelete" runat="server" CommandName="Delete" CommandArgument="<%# Container.DataItemIndex %>" Height="15px" ImageUrl="~/erp_images/fi-sr-trash.svg" Width="15px" ToolTip="Delete" OnClientClick="return confirm('Do you want to DELETE...?')" ImageAlign="Middle" />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text text-center" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="text text-center" BackColor="#0099ff" />
                                <EmptyDataTemplate>
                                    <div class="grid">No Data Found</div>
                                </EmptyDataTemplate>
                            </asp:GridView>

                            <%--form buttons div ---- start--%>
                            <div class="col-md-6 center-margin" runat="server" id="attachmanpowerrow" visible="false">
                                <div class="ln_solid"></div>
                                <div class="item form-group row">
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Button ID="btn_attachmanpower" runat="server" Text="Attach Manpower" CssClass="btn btn-success btn-sm" Enabled="true" OnClick="btn_attachmanpower_Click" />
                                    </div>

                                    <div class="col-md-6 col-sm-12">
                                        <asp:Label ID="Label1" runat="server" Text="">Click to Attach Manpower</asp:Label>
                                    </div>
                                </div>
                            </div>
                            <%--form buttons div ---- end--%>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 col-sm-12" id="Div_MemoTypeSelector" runat="server" visible="true">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Select Memo Creation Type</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>

                        <div class="x_content">
                            <div class="card-box col-md-12 col-sm-12">
                                <div class="col-md-6 col-sm-6">
                                    <span>Select MEMO Creation Type :</span>
                                </div>
                                <div class="col-md-6 col-sm-6">
                                    <asp:DropDownList ID="DDL_MemoType" runat="server" class="form-control form-control-sm rounded no-padding" OnSelectedIndexChanged="DDL_MemoType_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="--- SELECT MEMO TYPE ---" Value="-1" Selected="True">--- SELECT MEMO TYPE ---</asp:ListItem>
                                        <asp:ListItem Text="Without Line Items" Value="0">Without Line Items</asp:ListItem>
                                        <asp:ListItem Text="With Line Items" Value="1">With Line Items</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RFV1" runat="server" ErrorMessage="**" Display="Dynamic" SetFocusOnError="true" InitialValue="-1" ControlToValidate="DDL_MemoType" ValidationGroup="CreateMemo"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>


                <div class="col-md-12 col-sm-12" id="LineItems_SelectorPanel" runat="server" visible="false">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Workorder Line Items for Selection</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>

                        <div class="x_content">
                            <div class="card-box col-md-12 col-sm-12" style="width: 1100px; height: 250px; overflow: scroll;">
                                <asp:GridView BorderWidth="0" ID="GridView3" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap small" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl" Visible="True" HeaderStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SRL ID" HeaderStyle-Width="3%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Id" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="WODB_Code" HeaderStyle-Width="10%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_WODB_Code" runat="server" Text='<%# Bind("WODB_Code") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="WOI_DBCode" HeaderStyle-Width="10%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_WOI_DBCode" runat="server" Text='<%# Bind("WOI_DBCode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item NO" HeaderStyle-Width="4%" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_ItemNO" runat="server" Text='<%# Bind("ItemNO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Line Number" HeaderStyle-Width="5%" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_LineNumber" runat="server" Text='<%# Bind("LineNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service Number" HeaderStyle-Width="10%" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_ServiceNumber" runat="server" Text='<%# Bind("ServiceNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service Description" HeaderStyle-Width="20%" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Service_Description" runat="server" Text='<%# Bind("Service_Description") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order Quantity" HeaderStyle-Width="5%" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Order_Quantity" runat="server" Text='<%# Bind("Order_Quantity") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate" HeaderStyle-Width="0%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Rate" runat="server" Text='<%# Bind("Rate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PerUnit Value" HeaderStyle-Width="5%" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_PerUnit_Value" runat="server" Text='<%# Bind("PerUnit_Value") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Shift Skill" HeaderStyle-Width="10%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Shift_Skill" runat="server" Text='<%# Bind("Shift_Skill") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Select" HeaderStyle-Width="5%" Visible="true">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckRow" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <%--form buttons div ---- start--%>
                            <div class="col-md-12" runat="server" id="Div_AddLineItems" visible="true">
                                <div class="ln_solid"></div>
                                <div class="item form-group row">
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Label ID="Label2" runat="server" Text="">Click to Add Selected Line Items</asp:Label>
                                    </div>
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Button ID="btn_addlineitems" runat="server" Text="Add Line Items" CssClass="btn btn-success btn-sm" Enabled="true" OnClientClick="return validateCheckBoxes();" OnClick="btn_addlineitems_Click" />
                                    </div>
                                </div>
                            </div>
                            <%--form buttons div ---- end--%>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 col-sm-12" id="LineItems_SelectedPanel" runat="server" visible="false">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>View Selected Line Items</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>

                        <div class="x_content">
                            <div class="card-box col-md-12 col-sm-12" style="width: 1100px; height: 250px; overflow: scroll;">
                                <asp:GridView BorderWidth="0" ID="GridView4" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap small" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl" Visible="True" HeaderStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SRL ID" HeaderStyle-Width="3%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Id" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="WODB_Code" HeaderStyle-Width="10%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_WODB_Code" runat="server" Text='<%# Bind("WODB_Code") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="WOI_DBCode" HeaderStyle-Width="10%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_WOI_DBCode" runat="server" Text='<%# Bind("WOI_DBCode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item NO" HeaderStyle-Width="4%" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_ItemNO" runat="server" Text='<%# Bind("ItemNO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Line Number" HeaderStyle-Width="5%" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_LineNumber" runat="server" Text='<%# Bind("LineNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service Number" HeaderStyle-Width="10%" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_ServiceNumber" runat="server" Text='<%# Bind("ServiceNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service Description" HeaderStyle-Width="20%" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Service_Description" runat="server" Text='<%# Bind("Service_Description") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order Quantity" HeaderStyle-Width="5%" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Order_Quantity" runat="server" Text='<%# Bind("Order_Quantity") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate" HeaderStyle-Width="0%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Rate" runat="server" Text='<%# Bind("Rate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PerUnit Value" HeaderStyle-Width="5%" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_PerUnit_Value" runat="server" Text='<%# Bind("PerUnit_Value") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Shift Skill" HeaderStyle-Width="10%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Shift_Skill" runat="server" Text='<%# Bind("Shift_Skill") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Shift Skill" HeaderStyle-Width="10%" Visible="true">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="DDL_EmpCategory" class="form-control form-control-sm rounded no-padding" runat="server">
                                                    <asp:ListItem Text="--SELECT--" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="UNIFIED" Value="5"></asp:ListItem>
                                                    <asp:ListItem Text="HIGHLY-SKILLED" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="SKILLED" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="SEMI-SKILLED" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="UN-SKILLED" Value="4"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RFV1" runat="server" ErrorMessage="Selection Required" Display="Dynamic" SetFocusOnError="true" ControlToValidate="DDL_EmpCategory" InitialValue="0" ValidationGroup="LI_Selector" ForeColor="DarkRed"></asp:RequiredFieldValidator>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text text-center" />
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                            <%--form buttons div ---- start--%>
                            <div class="col-md-6 center-margin" runat="server" id="LineItems_SelectorButtonDIV" visible="true">
                                <div class="ln_solid"></div>
                                <div class="item form-group row">
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Label ID="Label3" runat="server" Text="">Click to Add Finalize Selection</asp:Label>
                                    </div>
                                    <div class="col-md-3 col-sm-6">
                                        <asp:Button ID="btn_proceednxt" runat="server" Text="Proceed Next" CausesValidation="true" ValidationGroup="LI_Selector" CssClass="btn btn-success btn-sm" Enabled="true" OnClick="btn_proceednxt_Click" />
                                    </div>
                                    <div class="col-md-3 col-sm-6">
                                        <asp:Button ID="btn_reset" runat="server" Text="Clear Selection" CssClass="btn btn-warning btn-sm" Enabled="true" OnClick="btn_reset_Click" />
                                    </div>
                                </div>
                            </div>
                            <%--form buttons div ---- end--%>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Shift Details</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                                <%--<li><a class="close-link"><i class="fa fa-close"></i></a></li>--%>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row">
                                <div class="animated flipInY col-lg-3 col-md-3 col-sm-6  ">
                                    <div class="tile-stats">
                                        <div class="icon">
                                            <i class="fa fa-user"></i>
                                        </div>
                                        <div class="count">
                                            <asp:Label ID="lbl_HS_ShiftCount" runat="server" Text="0"></asp:Label>
                                        </div>
                                        <h3>Highly-Skilled</h3>
                                    </div>
                                </div>
                                <div class="animated flipInY col-lg-3 col-md-3 col-sm-6  ">
                                    <div class="tile-stats">
                                        <div class="icon">
                                            <i class="fa fa-user"></i>
                                        </div>
                                        <div class="count">
                                            <asp:Label ID="lbl_S_ShiftCount" runat="server" Text="0"></asp:Label>
                                        </div>
                                        <h3>Skilled</h3>
                                    </div>
                                </div>
                                <div class="animated flipInY col-lg-3 col-md-3 col-sm-6  ">
                                    <div class="tile-stats">
                                        <div class="icon">
                                            <i class="fa fa-user"></i>
                                        </div>
                                        <div class="count">
                                            <asp:Label ID="lbl_SS_ShiftCount" runat="server" Text="0"></asp:Label>
                                        </div>
                                        <h3>Semi-Skilled</h3>
                                    </div>
                                </div>
                                <div class="animated flipInY col-lg-3 col-md-3 col-sm-6  ">
                                    <div class="tile-stats">
                                        <div class="icon">
                                            <i class="fa fa-user"></i>
                                        </div>
                                        <div class="count">
                                            <asp:Label ID="lbl_US_ShiftCount" runat="server" Text="0"></asp:Label>
                                        </div>
                                        <h3>Un-Skilled</h3>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--form buttons div ---- start--%>
                    <div id="btn_panel_" class="col-md-6 center-margin" runat="server" visible="true">
                        <div class="ln_solid"></div>
                        <div class="row center col-md-12">
                            <div class="col-7">
                                <button type="button" class="btn btn-danger btn-sm" id="btnShowPopup" runat="server" data-toggle="modal" data-target="#myModal">
                                    DELETE
                                </button>
                                <asp:Button ID="btn_home" runat="server" Text="Home" CssClass="btn btn-danger btn-sm" Enabled="false" Visible="false" OnClick="btn_home_Click" />
                                <asp:Button ID="btn_backpage" runat="server" Text="Go Back" ToolTip="Click to jump tp previous page" CssClass="btn btn-warning btn-sm" Enabled="true" Visible="true" OnClick="btn_backpage_Click" />
                                <asp:Button ID="btn_crtspm" runat="server" Text="Create Memo" ToolTip="Click to Create Supply Memo" CausesValidation="true" ValidationGroup="CreateMemo" CssClass="btn btn-primary btn-sm" Enabled="true" OnClick="btn_crtspm_Click" />
                            </div>

                            <div class="col-5">
                                <asp:Label ID="lbl_msg" runat="server" Text="Save for Supply Memo" Font-Bold="true" ForeColor="DarkBlue"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <%--form buttons div ---- end--%>
                </div>
            </div>
        </div>

        <div class="modal fade" id="myModal" data-backdrop="static">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Enter Comments for Deletion</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <div class="col-md-6 col-sm-12 form-group">
                                        <label>Delete Remarks<span class="text text-danger"></span></label>
                                    </div>
                                    <div class="col-md-6 col-sm-12 form-group">
                                        <asp:TextBox ID="txt_deletermrks" runat="server" TextMode="MultiLine" Rows="3" Columns="1" class="form-control form-control-sm rounded" ReadOnly="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_txt_deletermrks" runat="server" ValidationGroup="DELETE" Display="Dynamic" ForeColor="Red" ControlToValidate="txt_deletermrks" ErrorMessage="**"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btn_confirmdelete" runat="server" ValidationGroup="DELETE" Enabled="true" CausesValidation="true" Text="Proceed" CssClass="btn btn-info btn-sm" OnClick="btn_confirmdelete_Click" />
                        <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">Close</button>
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

        function validateCheckBoxes() {
            var checkboxes = document.querySelectorAll('[id*=CheckRow]');
            var checked = Array.prototype.slice.call(checkboxes).some(function (checkbox) {
                return checkbox.checked;
            });

            if (!checked) {
                alert("Please select at least one JOBID....!!!");
                return false;
            }

            return true;
        }
    </script>
</asp:Content>
