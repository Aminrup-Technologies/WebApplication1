<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="attach_manpower.aspx.cs" Inherits="WebApplication1.bussiness.production.attach_manpower" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>Attach Manpower Data Aganist JOBID</h3>
                </div>
            </div>
            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Attendnace Add Page<small>(Manpower Attendance)</small></h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="novalidate" id="InpunchPanel_Row" runat="server" visible="true">
                                <div class="field item form-group" id="JOBIDDetails_Row" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">Selected JOBID Details</label>
                                    <div class="col-md-6 col-sm-6">
                                        JOB_ID :<asp:Label ID="lbl_jobid" runat="server" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>;
									JOB Date :<asp:Label ID="lbl_jobiddate" runat="server" Text="Label"></asp:Label>;
									JOB Supervisor :<asp:Label ID="lbl_jobcreatorname" runat="server" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lbl_creatorwrk" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>;
									JOB Site :
									<asp:Label ID="lbl_jobsite" runat="server" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lbl_jobsitecode" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_creatorregion" runat="server" Visible="false" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lbl_creatorcompany" runat="server" Visible="false" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lbl_crtrsitename" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_crtrsitecode" runat="server" Visible="false" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>;
									JOB Work Order :
									<asp:Label ID="lbl_wrkordr" runat="server" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>;
									JOB Permit No :
									<asp:Label ID="lbl_permitno" runat="server" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>;
													<asp:Label ID="lbl_jobrgn" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_jobcompay" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
                                        In-charge Name :
									<asp:Label ID="lbl_inchargename" runat="server" Text="Label" ForeColor="blue" Font-Bold="true"></asp:Label>;
													<asp:Label ID="lbl_inchargewrk" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
                                        JOB Location :
									<asp:Label ID="lbl_jobloc" runat="server" Text="Label" ForeColor="Black" Font-Bold="true"></asp:Label>;
									JOB Shift :
									<asp:Label ID="lbl_jobshift" runat="server" Text="Label" ForeColor="Green" Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lbl_dept" runat="server" Text="Label" ForeColor="Black" Font-Bold="true" Visible="false"></asp:Label>
                                    </div>
                                </div>

                                <div class="field item form-group" id="AttachedAttendanceRow" runat="server" visible="false">
                                    <div class="card-box table-responsive small">
                                        <asp:GridView ID="GridView2" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl. NO" Visible="True" HeaderStyle-Width="2%">
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

                                                <asp:TemplateField HeaderText="Attendnace Date" HeaderStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_CreatedDate" runat="server" Text='<%# Bind("CreatedDate","{0:dd-MM-yyyy}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Submitter Name" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_SubmitterName" runat="server" Text='<%# Bind("SubmitterName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Workman" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_EmployeeWrk" runat="server" Text='<%# Bind("EmployeeWrk") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_EmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="IN Punch Time" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Inpunch_Time" runat="server" Text='<%# Bind("Inpunch_Time") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="OUT Time" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Outpunch_Time" runat="server" Text='<%# Bind("Outpunch_Time") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Lunch Factor" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_LunchFactor" runat="server" Text='<%# Bind("LunchFactor") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Worked Hours" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_WorkedHours" runat="server" Text='<%# Bind("WorkedHours") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="OT" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_ProvidedOT" runat="server" Text='<%# Bind("ProvidedOT") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Atten Status" HeaderStyle-Width="3%" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_AttendanceStatus" runat="server" Text='<%# Bind("AttendanceStatus") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Atten Code" HeaderStyle-Width="3%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_AttendanceCode" runat="server" Text='<%# Bind("AttendanceCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="5%" Visible="false">
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

                                <div class="field item form-group" id="WorkmanInput_Row" runat="server" visible="true">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">Employee Workman<span class="required">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_empworkman" class="form-control form-control-sm rounded" runat="server" AutoPostBack="true" placeholder="Enter Employee Workman" OnTextChanged="txt_empworkman_TextChanged"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="ADDTOLIST" ControlToValidate="txt_empworkman" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="field item form-group" id="PendingOUTMsg" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3  label-align">Error Message <span class="required">:</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:Label ID="lbl_pendingmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>

                                <div class="field item form-group" id="EmployeeName_Row" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3  label-align">Employee Name <span class="required">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_empname" runat="server" ReadOnly="true" class="form-control form-control-sm rounded"></asp:TextBox>
                                        <asp:Label ID="lbl_workhours" runat="server" Text="0" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_category" runat="server" Text="" Visible="false"></asp:Label><asp:Label ID="lbl_categorycode" runat="server" Text="" Visible="true"></asp:Label>-
										<asp:Label ID="lbl_designation" runat="server" Text="" Visible="false">
                                            <asp:Label ID="lbl_designationcode" runat="server" Text="" Visible="true"></asp:Label>
                                        </asp:Label><asp:Label ID="lbl_pocategorycode" runat="server" Text="" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_gpno" runat="server" Text="" Visible="false"></asp:Label><asp:Label ID="lbl_pocategoryname" runat="server" Text="" Visible="false"></asp:Label>
                                    </div>
                                </div>

                                <div class="field item form-group" id="EmployeeWorksite_Row" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">Employee Worksite <span class="required">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_worksite" runat="server" ReadOnly="true" class="form-control form-control-sm rounded"></asp:TextBox>
                                        <asp:Label ID="lbl_worksitecode" runat="server" Text="" Visible="false"></asp:Label>
                                    </div>
                                </div>


                                <div class="field item form-group" id="InPunchDate_Row" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">In-Punch Date<span class="required">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_indate" runat="server" CssClass="form-control form-control-sm rounded" class='date' type="date" name="date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="ADDTOLIST" ControlToValidate="txt_indate" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="field item form-group" id="InPunchTime_Row" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3  label-align">In-Punch Time<span class="required">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_intime" runat="server" CssClass="form-control form-control-sm rounded" class='time' type="time" name="time"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="ADDTOLIST" ControlToValidate="txt_intime" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="field item form-group" id="OUTPunchDate_Row" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3 label-align">OUT Punch Date<span class="required">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_outdate" runat="server" CssClass="form-control form-control-sm rounded" class='date' type="date" name="date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="ADDTOLIST" ControlToValidate="txt_outdate" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="field item form-group" id="OUTPunchTime_Row" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3  label-align">OUT Punch Time<span class="required">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_outtime" runat="server" CssClass="form-control form-control-sm rounded" class='time' type="time" name="time"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ValidationGroup="ADDTOLIST" ControlToValidate="txt_outtime" runat="server" ErrorMessage="Required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue=""></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="field item form-group" id="LunchFactorRow" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3  label-align">Lunch<span class="required">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:RadioButtonList ID="RBTN_LunchFactor" runat="server" CssClass="rounded" CellPadding="2" CellSpacing="5" RepeatDirection="Horizontal">
                                            <asp:ListItem>Yes</asp:ListItem>
                                            <asp:ListItem>No</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="ADDTOLIST" runat="server" ErrorMessage="Reqired" ControlToValidate="RBTN_LunchFactor" Display="Dynamic" CssClass="text text-warning" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="field item form-group" id="OTRow" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3  label-align">Over Time (Hours)<span class="required">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="txt_ot" runat="server" CssClass="form-control form-control-sm rounded" MaxLength="2" TextMode="Number"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ValidationGroup="ADDTOLIST" runat="server" ErrorMessage="Required" ControlToValidate="txt_ot" Display="Dynamic" CssClass="text text-warning" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="ADDTOLIST" ControlToValidate="txt_ot" ErrorMessage="Value must be less than or equal to 16" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ClientValidationFunction="validateInput"></asp:CustomValidator>
                                    </div>
                                </div>

                                <div class="field item form-group" id="AttenCode" runat="server" visible="false">
                                    <label class="col-form-label col-md-3 col-sm-3  label-align">Attendance Code<span class="required">*</span></label>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:DropDownList ID="DDL_AttenCode" runat="server" CssClass="form-control form-control-sm rounded"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFV3" ValidationGroup="ADDTOLIST" runat="server" CssClass="text text-warning" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="DDL_AttenCode" SetFocusOnError="true" InitialValue="Please Select Option" ToolTip="Kindly select value"></asp:RequiredFieldValidator>
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
                            <div class="col-md-6 center-margin" id="Addto_buttons" runat="server" visible="false">
                                <div class="ln_solid"></div>
                                <div class="item form-group row">
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to ADD Data!!"></asp:Label>
                                    </div>
                                    <div class="col-md-6 col-sm-12">
                                        <button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
                                        <button type="reset" class="btn btn-warning btn-sm">Reset</button>
                                        <asp:Button ID="btn_submit" runat="server" Text="ADD TO LIST" ValidationGroup="ADDTOLIST" CausesValidation="true" CssClass="btn btn-success btn-sm" OnClick="btn_submit_Click" />
                                    </div>
                                </div>
                            </div>
                            <%--ADD button end--%>


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

                                            <asp:TemplateField HeaderText="Workman" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_wrk" runat="server" Text='<%# Bind("wrk") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid" />
                                                <ItemStyle CssClass="grid" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Name" HeaderStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_name" runat="server" Text='<%# Bind("name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Hours" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_wrkhrs" runat="server" Text='<%# Bind("wrkhrs") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Hours" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_category" runat="server" Text='<%# Bind("category") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Hours" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_categorycode" runat="server" Text='<%# Bind("categorycode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Hours" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_po_category" runat="server" Text='<%# Bind("po_category") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Hours" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_po_categorycode" runat="server" Text='<%# Bind("po_categorycode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Hours" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_designation" runat="server" Text='<%# Bind("designation") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Hours" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_designationcode" runat="server" Text='<%# Bind("designationcode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Hours" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_gpno" runat="server" Text='<%# Bind("gpno") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="SiteName" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_wrksitename" runat="server" Text='<%# Bind("wrksitename") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="SiteCode" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_wrksitecode" runat="server" Text='<%# Bind("wrksitecode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="IN" HeaderStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_in" runat="server" Text='<%# Bind("in") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid" />
                                                <ItemStyle CssClass="grid" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="OUT" HeaderStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_out" runat="server" Text='<%# Bind("out") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid" />
                                                <ItemStyle CssClass="grid" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Lunch" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_lunch" runat="server" Text='<%# Bind("lunch") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid" />
                                                <ItemStyle CssClass="grid" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Mins" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_wrkmin" runat="server" Text='<%# Bind("wrkmin") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid" />
                                                <ItemStyle CssClass="grid" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Hours" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_wrkdhrs" runat="server" Text='<%# Bind("wrkdhrs") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid" />
                                                <ItemStyle CssClass="grid" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="C OT" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_cot" runat="server" Text='<%# Bind("cot") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid" />
                                                <ItemStyle CssClass="grid" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="OT" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_pot" runat="server" Text='<%# Bind("pot") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid" />
                                                <ItemStyle CssClass="grid" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Status" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_presentstatus" runat="server" Text='<%# Bind("presentstatus") %>'></asp:Label>
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

                            <div class="row" id="AttendanceSentMesg" runat="server" visible="false">
                                <div class="col-md-12 col-sm-12" style="vertical-align: middle; text-align: center;">
                                    <asp:Image ID="Img_Success" runat="server" ImageUrl="~/erp_images/success_gif.gif" Width="100px" Height="100px" />
                                    <asp:Label ID="Label1" runat="server" Text="Manpower Attendance Sent..!" Font-Bold="true" Font-Size="Large"></asp:Label>
                                </div>

                                <div class="col-md-12 col-sm-12" style="vertical-align: middle; text-align: center;">
                                    <asp:Button ID="btn_back" runat="server" Text="Go Back" CssClass="btn btn-success btn-sm" OnClick="btn_back_Click" />
                                </div>
                            </div>


                            <%--Final SAVE button start--%>
                            <div class="col-md-6 center-margin" id="SendAttendance_Buttons" runat="server" visible="false">
                                <div class="ln_solid"></div>
                                <div class="item form-group row">
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Label ID="lbl_finalmsg" runat="server" Text="Click SUBMIT to SEND Data!!"></asp:Label>
                                    </div>
                                    <div class="col-md-6 col-sm-12">
                                        <button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
                                        <asp:Button ID="btn_finalsubmit" runat="server" Text="SAVE" CssClass="btn btn-success btn-sm" OnClick="btn_finalsubmit_Click" />
                                    </div>
                                </div>
                            </div>
                            <%--Final SAVE button end--%>
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

        function ShowPopup2() {
            $("#myModal2").modal("show");
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
