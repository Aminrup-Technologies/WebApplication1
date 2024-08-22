<%@ Page Title="" Language="C#" MasterPageFile="~/atsSite.Master" AutoEventWireup="true" CodeBehind="apply.aspx.cs" Inherits="atsweb.apply" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="page-title">
        <div class="container">
            <div class="row">
                <div class="col col-xs-12">
                    <h3>&nbsp;</h3>
                    <ol class="breadcrumb">
                        <li><a href="career.aspx">Career</a></li>
                        <li>Applying</li>
                    </ol>
                </div>
            </div>
        </div>
    </section>


    <!-- start contact-pg-section-s2 -->
    <section class="contact-pg-section-s2 section-padding">
        <div class="container">
            <div class="row">
                <div class="col col-xs-12">
                    <div class="section-title-s2">
                        <div class="align-self-end">
                            <asp:Label runat="server" ID="lblMsg" Visible="False" Text=""></asp:Label>
                        </div>
                        <span>Career</span>
                        <h2>Applying</h2>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 col-lg-6 col-sm-12">
                    <div class="form-container">
                        <div>
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="Name is required" ControlToValidate="txtName" ForeColor="Red" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revName" runat="server" ErrorMessage="Name must be in characters only" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationExpression="^[a-zA-Z\s]+$" ControlToValidate="txtName"></asp:RegularExpressionValidator>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Enter Full Name" ToolTip="Full Name"></asp:TextBox>
                        </div>
                        <br />
                        <div>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email is required" ControlToValidate="txtEmail" ForeColor="Red" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Enter Email" ToolTip="Email" TextMode="Email"></asp:TextBox>
                        </div>
                        <br />
                        <div>
                            <asp:RequiredFieldValidator ID="rfvMobile" runat="server" ErrorMessage="Mobile No. is required" ControlToValidate="txtName" ForeColor="Red" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revMobile" runat="server" ErrorMessage="Mobile No. must have in 10 digits" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationExpression="^[0-9]{10}$" ControlToValidate="txtMobile"> </asp:RegularExpressionValidator>
                            <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" placeholder="Enter Mobile Number" ToolTip="Mobile Number"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="col-md-6 col-lg-6 col-sm-12">
                    <div class="form-container">
                        <div>
                            <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ErrorMessage="Address is required" ControlToValidate="txtAddress"  ForeColor="Red" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Enter Address" ToolTip="Address" TextMode="MultiLine"></asp:TextBox>
                        </div>
                        <br />

                        <div>

                            <asp:RequiredFieldValidator ID="rfvPostCode" runat="server" ErrorMessage="Post/Zip Code is required" ControlToValidate="txtPostCode" ForeColor="Red" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revPostCode" runat="server" ErrorMessage="Post/Zip Code must be of 6 digits" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationExpression="^[0-9]{6}$"  ControlToValidate="txtPostCode"></asp:RegularExpressionValidator>
                            <asp:TextBox ID="txtPostCode" runat="server" CssClass="form-control" placeholder="Enter Post/Zip Code"
                                ToolTip="Post/Zip Code" TextMode="Number"></asp:TextBox>
                        </div>
                        <br />

                        <div>
                            <asp:FileUpload ID="fuUserImage" runat="server" CssClass="form-control"/>
                        </div>
                        <br />

                    </div>

                </div>

            </div>
            <div class="btn_box">
                <asp:Button ID="Button1" runat="server" Text="Apply" CssClass="btn btn-warning rounded-pill pl-4 pr-4 text-white" OnClick="Button1_Click" CausesValidation="true" />
                <asp:Button ID="ButtonBack" runat="server" Text="Back"  CssClass="btn btn-danger rounded-pill pl-4 pr-4 text-white" PostBackUrl="~/career.aspx" CausesValidation="false"/>
            </div>

        </div>
        <!-- end container -->
    </section>
    <!-- end contact-pg-section-s2 -->
</asp:Content>
