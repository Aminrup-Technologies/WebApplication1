<%@ Page Title="Permit Upload" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="job_permitupload.aspx.cs" Inherits="WebApplication1.bussiness.production.job_permitupload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>Work Permit Upload</h3>
                </div>
            </div>
            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Permit Attachment Status & View</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row">
                                <div class="col-md-4 col-sm-12 form-group">
                                    <asp:Label ID="lbl_jobidddl" runat="server">Select JOBID</asp:Label>
                                </div>
                                <div class="col-md-8 col-sm-12 form-group">
                                    <asp:DropDownList ID="DDL_JOBID" runat="server" class="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_JOBID_SelectedIndexChanged"></asp:DropDownList>
                                </div>


                                <div class="col-md-4 col-sm-12  form-group" id="jobid_details_row1" runat="server" visible="false">
                                    <label>Selected JOBID Details <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-8 col-sm-12  form-group" id="jobid_details_row2" runat="server" visible="false">
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

                                <div class="col-md-12 center-margin">
                                    <div class="ln_solid"></div>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group" id="AppStatus_Row1" runat="server" visible="false">
                                    <asp:Label ID="Label1" runat="server" Text="Approval Status"></asp:Label>
                                </div>
                                <div class="col-md-3 col-sm-12 form-group" id="AppStatus_Row2" runat="server" visible="false">
                                    <asp:Label ID="lbl_approvalstatus" runat="server" Text="N/A" Font-Bold="true"></asp:Label>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group" id="PrmtStatus_Row1" runat="server" visible="false">
                                    <asp:Label ID="Label2" runat="server" Text="Upload Status"></asp:Label>
                                </div>
                                <div class="col-md-3 col-sm-12 form-group" id="PrmtStatus_Row2" runat="server" visible="false">
                                    <asp:Label ID="lbl_permitstatus" runat="server" Text="N/A" Font-Bold="true"></asp:Label>
                                    &nbsp; | &nbsp;
									<asp:Button ID="btn_uploadbtn" runat="server" Text="UPLOAD" CssClass="btn btn-warning btn-sm" OnClick="btn_uploadbtn_Click" />
                                </div>

                                <div class="col-md-3 col-sm-12 form-group" id="FileCount_Row1" runat="server" visible="false">
                                    <asp:Label ID="Label6" runat="server" Text="Total File Count"></asp:Label>
                                </div>
                                <div class="col-md-3 col-sm-12 form-group" id="FileCount_Row2" runat="server" visible="false">
                                    <asp:Label ID="lbl_filecount" runat="server" Text="0" Font-Bold="true"></asp:Label>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group" id="PrmtUpldDate1" runat="server" visible="false">
                                    <asp:Label ID="Label8" runat="server" Text="Upload Date"></asp:Label>
                                </div>
                                <div class="col-md-3 col-sm-12 form-group" id="PrmtUpldDate2" runat="server" visible="false">
                                    <asp:Label ID="lbl_permituploaddate" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group" id="UpldTyp_Row1" runat="server" visible="false">
                                    <asp:Label ID="Label3" runat="server" Text="Upload Type"></asp:Label>
                                </div>
                                <div class="col-md-3 col-sm-12 form-group" id="UpldTyp_Row2" runat="server" visible="false">
                                    <asp:DropDownList ID="DDL_UploadType" runat="server" class="form-control form-control-sm rounded" AutoPostBack="true" OnSelectedIndexChanged="DDL_UploadType_SelectedIndexChanged">
                                        <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                        <asp:ListItem>PDF File</asp:ListItem>
                                        <asp:ListItem>Photograph</asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                                <div class="col-md-3 col-sm-12 form-group" id="cancelbtn_row1" runat="server" visible="false">
                                    <asp:Label ID="Label4" runat="server" Text="Click to Cancel"></asp:Label>
                                </div>
                                <div class="col-md-3 col-sm-12 form-group" id="cancelbtn_row2" runat="server" visible="false">
                                    <asp:Button ID="btn_cnclupld" runat="server" Text="CANCEL UPLOAD" CssClass="btn btn-danger btn-sm" OnClick="btn_cnclupld_Click" />
                                </div>

                                <div class="col-md-3 col-sm-12 form-group" id="pdfuploadbuttonrow1" runat="server" visible="false">
                                    <label>Upload Permit (.pdf / .jpg) <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12 form-group" id="pdfuploadbuttonrow2" runat="server" visible="false">
                                    <button type="button" class="btn btn-primary btn-sm" id="btnShowPopup" data-toggle="modal" data-target="#myModal">
                                        <i class="fa fa-plus-circle"></i>&nbsp;Upload Permit File
                                    </button>
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


                            <%--- Up-loader Modal --%>
                            <div class="modal fade" id="myModal">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h4 class="modal-title">Upload File</h4>
                                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                                        </div>
                                        <div class="modal-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label>Choose file</label>
                                                        <div class="input-group">
                                                            <div class="custom-file col-md-8">
                                                                <asp:FileUpload ID="FileUpload1" CssClass="custom-file-input" runat="server" />
                                                                <label class="custom-file-label"></label>
                                                            </div>
                                                            <asp:Label ID="lbl_fileyesno" runat="server" Text="Label" Visible="false"></asp:Label>
                                                            <div class="input-group-append col-md-4">
                                                                <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-primary btn-sm" CausesValidation="false" Text="Upload" OnClick="ImportPermit" />
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

                            <div class="row" id="permituploaded" runat="server" visible="false">
                                <div class="col-md-12 col-sm-12" style="vertical-align: middle; text-align: center;">
                                    <asp:Image ID="Img_Success" runat="server" ImageUrl="~/erp_images/success_gif.gif" Width="100px" Height="100px" />
                                    <asp:Label ID="lbl_upldscs_msg" runat="server" Text="Permit Uploaded Successfully...!" Font-Bold="true" Font-Size="Large"></asp:Label>
                                </div>
                            </div>

                            <%--button start--%>
                            <div class="col-md-6 center-margin" id="permituploadedbuttons" runat="server" visible="false">
                                <div class="ln_solid"></div>
                                <div class="col-md-12 col-sm-12 center" style="text-align: center;">
                                    <asp:Button ID="btn_inpunch" runat="server" Text="OUT Punch" CssClass="btn btn-info btn-sm" OnClick="btn_inpunch_Click" />
                                </div>
                            </div>
                            <%--button end--%>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 col-sm-12" id="GridTable_Row" runat="server" visible="false">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>View and Manage : Uploaded Data</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                                <%--<li><a class="close-link"><i class="fa fa-close"></i></a></li>--%>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row">

                                <div class="col-md-12 col-sm-12">
                                    <div class="card-box table-responsive">
                                        <p class="text-muted font-13 m-b-30">
                                            Last attached permit file deleted on :
											<asp:Label ID="lbl_permitdeleteddate" runat="server" Text=""></asp:Label>, by
                                            <asp:Label ID="lbl_permitdeletedby" runat="server" Text=""></asp:Label>
                                        </p>
                                        <asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found" OnRowDeleting="GridView1_RowDeleting">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="GridHeaderText-Center" />
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="DB ID" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Id" runat="server" Text='<%# Eval("Id") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="GridHeaderText-Center" />
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="JOB ID" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_JOBID" runat="server" Text='<%# Eval("JOBID") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="GridHeaderText-Center" />
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="File Name" HeaderStyle-Width="30%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Name" runat="server" Text='<%# Eval("Name") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="GridHeaderText-Center" />
                                                    <ItemStyle CssClass="grid" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="TimeStamp" HeaderStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_TimeStamp" runat="server" Text='<%# Eval("TimeStamp") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="GridHeaderText-Center" />
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="View Permit" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("Id") %>' OnClick="DownloadFile" ToolTip="Click to Download" CommandName="Download"><span class="glyphicon glyphicon-save" aria-hidden='true'></span></asp:LinkButton>
                                                        <asp:Label ID="lbl_dbid" runat="server" Visible="false" Text='<%# Bind("Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="text text-center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="5%">
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
    </script>
</asp:Content>
