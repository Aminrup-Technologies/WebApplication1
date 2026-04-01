<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="bulk_employeeupdate.aspx.cs" Inherits="WebApplication1.bussiness.production.bulk_employeeupdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function CheckAll(source) {
            var checkboxes = document.querySelectorAll('input[type="checkbox"]');
            for (var i = 0; i < checkboxes.length; i++) {
                checkboxes[i].checked = source.checked;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="col-md-12 col-sm-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Update Existing Employee Data</h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        
                        <div class="row" id="divSetup" runat="server">
                            <div class="col-md-6 col-sm-6">
                                <p class="text-muted font-13 m-b-30">Select columns to map, then upload your data file.</p>
                                <asp:GridView ID="GridViewColumns" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <input type="checkbox" id="chkSelectAll" onclick="CheckAll(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="COLUMN_NAME" HeaderText="Column Name" />
                                    </Columns>
                                </asp:GridView>
                            </div>

                            <div class="col-md-6 center-margin">
                                <div class="ln_solid"></div>
                                <div class="item form-group row">
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Label ID="lbl_msg" runat="server" Text="Upload Excel file to proceed."></asp:Label>
                                    </div>
                                    <div class="col-md-6 col-sm-12">
                                        <asp:FileUpload ID="FileUploadExcel" runat="server" CssClass="form-control" /><br />
                                        <asp:Button ID="btnUploadExcel" runat="server" Visible="false" Text="Upload" OnClick="btnUploadExcel_Click" CssClass="btn btn-sm btn-primary" />
                                        <asp:Label ID="lblUploadMessage" runat="server" CssClass="text-danger"></asp:Label>
                                        <asp:Button ID="btnMapColumnsAndUpload" runat="server" CssClass="btn btn-sm btn-warning" Text="Next: Map Columns" OnClick="btnMapColumnsAndUpload_Click" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-12 col-sm-12">
                                <div class="card-box table-responsive">
                                    <asp:GridView ID="GridViewColumnMapping" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap">
                                        <Columns>
                                            <asp:BoundField DataField="DatabaseColumn" HeaderText="Database Column" />
                                            <asp:TemplateField HeaderText="Excel Column">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlExcelColumns" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>

                            <div class="col-md-6 center-margin">
                                <div class="ln_solid"></div>
                                <div class="item form-group row">
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Label ID="lbl_msg2" runat="server" Text="Click PREVIEW to see changes before updating."></asp:Label>
                                    </div>
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Button ID="btnMapColumns" runat="server" CssClass="btn btn-sm btn-primary" Text="Check Mapping" OnClick="btnMapColumns_Click" />
                                        <asp:Button ID="btnPreviewChanges" runat="server" CssClass="btn btn-sm btn-info" Text="Preview Changes" OnClick="btnPreviewChanges_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row" id="divPreview" runat="server" visible="false">
                            <div class="col-md-12 col-sm-12">
                                <h3>Data Preview & Confirmation</h3>
                                <p class="text-info">Please review the proposed changes below. The database will only be updated once you confirm.</p>
                                
                                <div class="card-box table-responsive" style="max-height: 400px; overflow-y: auto;">
                                    <asp:GridView ID="GridViewPreview" runat="server" AutoGenerateColumns="True" CssClass="table table-striped table-hover table-bordered table-sm dt-responsive nowrap">
                                    </asp:GridView>
                                </div>
                                
                                <div class="ln_solid"></div>
                                <div class="item form-group row">
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Label ID="lbl_msg3" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div class="col-md-6 col-sm-12 text-right">
                                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-sm btn-secondary" Text="Cancel & Go Back" OnClick="btnCancel_Click" />
                                        <asp:Button ID="btnConfirmUpdate" runat="server" CssClass="btn btn-sm btn-danger" Text="Confirm & Execute Update" OnClick="btnConfirmUpdate_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>