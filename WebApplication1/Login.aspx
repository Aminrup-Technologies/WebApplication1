<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="WebApplication1.bussiness.production.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <!-- Meta, title, CSS, favicons, etc. -->
    <link rel="shortcut icon" href="../../erp_images/OH4Y_Logo.png" />
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>Cloud ERP | Login</title>
    <!-- Bootstrap -->
    <link href="bussiness/vendors/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Font Awesome -->
    <link href="bussiness/vendors/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <!-- NProgress -->
    <link href="bussiness/vendors/nprogress/nprogress.css" rel="stylesheet" />
    <!-- Animate.css -->
    <link href="bussiness/vendors/animate.css/animate.min.css" rel="stylesheet" />
    <!-- Custom Theme Style -->
    <link href="bussiness/build/css/custom.min.css" rel="stylesheet" />
    <script type="text/javascript">
        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
    </script>
</head>
<body class="login">
    <form id="form1" runat="server">
        <div>
            <a class="hiddenanchor" id="signin"></a>
            <div class="login_wrapper">
                <div class="login_form">
                    <section class="login_content">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/erp_images/OH4Y_Logo.png" Height="100" Width="100" />
                        <h1><asp:Label ID="lbl_companyname" runat="server" Text="Aminrup Technologies"></asp:Label></h1>
                        <div class="form-horizontal">
                            <div class="form-group row">
                                <label class="control-label col-md-4 col-sm-6 ">User ID</label>
                                <div class="col-md-8 col-sm-6 ">
                                    <asp:TextBox ID="txt_loginid" runat="server" class="form-control" placeholder="ATS00__"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txt_loginid" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="form-group row">
                                <label class="control-label col-md-4 col-sm-6 ">Password</label>
                                <div class="col-md-8 col-sm-6 ">
                                    <asp:TextBox ID="txt_password" runat="server" class="form-control" placeholder="Login Password" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txt_password" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div>
                                <div class="clearfix"></div>
                                <br />
                                <asp:Button ID="Button1" runat="server" Text="Login" class="btn btn-success submit" OnClick="Button1_Click1" />
                                <a class="reset_pass text text-danger" href="#">Lost your password?</a>
                            </div>

                            <div class="clearfix"></div>

                            <div class="separator">
                                <br />
                                <div>
                                    <p>© 2021-2024 All Rights Reserved. <span style="font-weight: bold; color: darkred;">
                                        <asp:Label ID="lbl_compfooter" runat="server" Text="Company Name"></asp:Label></span> Powered by <a href="#" target="_blank"><span style="font-weight:bold; color:darkblue">Aminrup Technologies</span></a></p>
                                </div>
                            </div>
                        </div>
                    </section>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
