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
                        <div class="row">

                            <div class="col-md-6 col-sm-6">
                                <p class="text-muted font-13 m-b-30">Click on the above SUBMIT button to save the uploaded data</p>
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
                                        <asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to Save Data!!"></asp:Label>
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
                                        <asp:Label ID="lbl_msg2" runat="server" Text="Click SUBMIT to Save Data!!"></asp:Label>
                                    </div>
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Button ID="btnMapColumns" runat="server" CssClass="btn btn-sm btn-primary" Text="Check Mapping" OnClick="btnMapColumns_Click" />
                                        <asp:Button ID="btnUpdateDatabase" runat="server" CssClass="btn btn-sm btn-danger" Text="Update DB" OnClick="btnUpdateDatabase_Click" />
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
        function ShowPopup() {
            $("#btnShowPopup").click();
        }
        function openModal() {
            $('#myModal').modal('show');
        }
    </script>
</asp:Content>
