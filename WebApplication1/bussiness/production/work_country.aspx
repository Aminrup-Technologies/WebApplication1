<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="work_country.aspx.cs" Inherits="WebApplication1.bussiness.production.work_country" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>Work Country</h3>
                </div>
            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12 " id="add_panel" runat="server" visible="false">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Add : Your work country</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">

                            <%--form elements div ---- start--%>
                            <div class="row">
                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Work Country Name <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_countryname" class="form-control form-control-sm" placeholder="Enter Country Name..." runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_countryname" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4">Example : India</small>
                                </div>


                                <div class="col-md-3 col-sm-12  form-group">
                                    <label>Work Country Code <span class="text text-danger">*</span></label>
                                </div>
                                <div class="col-md-3 col-sm-12  form-group">
                                    <asp:TextBox ID="txt_countrycode" class="form-control form-control-sm" placeholder="Enter Country Short Code..." runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ErrorMessage="This field is required" ForeColor="Red" ControlToValidate="txt_countrycode" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <small class="form-text text-muted ml-4">Example : IN</small>
                                </div>
                            </div>
                            <%--form elements div ---- end--%>

                            <%--form buttons div ---- start--%>
                            <div class="col-md-6 center-margin">
                                <div class="ln_solid"></div>
                                <div class="item form-group row">
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to Save Data!!"></asp:Label>
                                    </div>
                                    <div class="col-md-6 col-sm-12">
                                        <button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
                                        <button type="reset" class="btn btn-warning btn-sm">Reset</button>
                                        <asp:Button ID="btn_submit" runat="server" Text="Submit" CssClass="btn btn-success btn-sm" Enabled="false" />
                                    </div>
                                </div>
                            </div>
                            <%--form buttons div ---- end--%>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 col-sm-12  ">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>View and Manage : Work Country</h2>
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
                                        <asp:GridView ID="GridView1" runat="server" Width="100%" class="table table-striped table-hover table-bordered table-responsive table-sm dt-responsive nowrap" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL" Visible="True" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="GridHeaderText-Center" />
                                                    <ItemStyle CssClass="grid" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Country Name" HeaderStyle-Width="50%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Country_Name" runat="server" Text='<%# Eval("Country_Name") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="GridHeaderText-Center" />
                                                    <ItemStyle CssClass="grid" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Country Code" HeaderStyle-Width="40%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Country_Code" runat="server" Text='<%# Eval("Country_Code") %>' />
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
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
