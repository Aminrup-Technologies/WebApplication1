<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="WebApplication1.bussiness.production.login" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>ATS | Cloud ERP - Login</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />

    <link href="https://cdn.jsdelivr.net/npm/@pnotify/core@5.2.0/dist/PNotify.css" rel="stylesheet">
    <link href="https://cdn.jsdelivr.net/npm/@pnotify/core@5.2.0/dist/BrightTheme.css" rel="stylesheet">

    <style>
        body {
            background: #f0f2f5;
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            font-family: 'Inter', sans-serif;
            padding: 15px;
        }

        .login-card {
            background: #fff;
            border-radius: 12px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.08);
            width: 100%;
            max-width: 400px;
            padding: 2.5rem;
        }

        .brand-logo {
            display: block;
            margin: 0 auto 1.2rem;
        }

        .btn-success {
            background-color: #198754;
            border: none;
            padding: 0.7rem;
            font-weight: 600;
        }

        #forgot_section {
            display: none;
        }

        .pnotify-custom {
            border-radius: 8px;
            font-weight: 600;
        }

        .footer-text {
            font-size: 0.8rem; /* Small, professional size */
            text-align: center;
            margin-top: 2.5rem; /* Spacing from the buttons */
            color: #6c757d; /* Muted gray text */
            border-top: 1px solid #e9ecef; /* Thin separator line */
            padding-top: 1rem;
        }

            .footer-text a {
                color: #198754; /* Match the success button color */
                transition: color 0.2s;
            }

                .footer-text a:hover {
                    color: #146c43; /* Darker green on hover */
                    text-decoration: underline !important;
                }
    </style>

    <script type="text/javascript">
        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-card">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/erp_images/ats_translogo.png" Height="80" Width="80" CssClass="brand-logo" />

            <div id="login_section">
                <h1 class="h4 text-center fw-bold mb-4">Work-Sure ERP</h1>

                <div class="mb-3">
                    <label class="form-label small fw-bold text-muted">USER ID</label>
                    <asp:TextBox ID="txt_loginid" runat="server" CssClass="form-control" placeholder="ATS00__"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <label class="form-label small fw-bold text-muted">PASSWORD</label>
                    <div class="input-group">
                        <asp:TextBox ID="txt_password" runat="server" CssClass="form-control" TextMode="Password" placeholder="••••••••"></asp:TextBox>
                        <span class="input-group-text bg-white" id="pass-toggle" style="cursor: pointer;">
                            <i class="fa fa-eye" id="eye-icon"></i>
                        </span>
                    </div>
                </div>

                <div class="d-flex justify-content-between mb-4">
                    <div class="form-check">
                        <asp:CheckBox ID="chk_remember" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label small" for="chk_remember">Remember Me</label>
                    </div>
                    <a href="javascript:void(0)" onclick="toggleUI(true)" class="small text-danger text-decoration-none fw-bold">Forgot?</a>
                </div>

                <asp:Button ID="btn_login" runat="server" Text="LOG IN" CssClass="btn btn-success w-100" OnClick="btn_login_Click" OnClientClick="return validateLogin();" />
            </div>

            <div id="forgot_section">
                <h3 class="h5 text-center fw-bold mb-3">Reset Password</h3>
                <asp:TextBox ID="txt_reset_code" runat="server" CssClass="form-control mb-3" placeholder="Employee Code"></asp:TextBox>
                <asp:TextBox ID="txt_reset_email" runat="server" CssClass="form-control mb-3" placeholder="Registered Email"></asp:TextBox>

                <asp:PlaceHolder ID="ph_otp" runat="server" Visible="false">
                    <asp:TextBox ID="txt_otp" runat="server" CssClass="form-control mb-2" placeholder="OTP"></asp:TextBox>
                    <asp:TextBox ID="txt_new_pass" runat="server" CssClass="form-control mb-3" TextMode="Password" placeholder="New Password"></asp:TextBox>
                </asp:PlaceHolder>

                <asp:Button ID="btn_send_otp" runat="server" Text="SEND OTP" CssClass="btn btn-primary w-100 mb-2" OnClick="btn_send_otp_Click" />
                <asp:Button ID="btn_verify_reset" runat="server" Text="RESET PASSWORD" Visible="false" CssClass="btn btn-success w-100 mb-2" OnClick="btn_verify_reset_Click" />

                <button type="button" class="btn btn-link w-100 small text-muted text-decoration-none" onclick="toggleUI(false)">Back to Login</button>
            </div>

            <div class="footer-text">
                <p class="mb-0">
                    © 2021-2026 All Rights Reserved. 
                    <br />
                    <span style="font-weight: bold; color: darkred;">
                        <asp:Label ID="lbl_compfooter" runat="server" Text="ATS,JSR"></asp:Label>
                    </span>
                </p>
                <p class="mt-1">
                    Powered by <a href="https://aminruptechnologies.co.in/" target="_blank" class="text-decoration-none fw-bold">Aminrup Technologies</a>
                </p>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/@pnotify/core@5.2.0/dist/PNotify.js"></script>
    <script>
        function notify(title, text, type) {
            PNotify.alert({
                title: title,
                text: text,
                type: type,
                delay: 3000,
                addClass: 'pnotify-custom'
            });
        }

        function validateLogin() {
            const id = document.getElementById('<%= txt_loginid.ClientID %>').value;
            const pass = document.getElementById('<%= txt_password.ClientID %>').value;
            if (!id || !pass) {
                notify('Wait!', 'Please enter both User ID and Password.', 'notice');
                return false;
            }
            return true;
        }

        function toggleUI(showForgot) {
            document.getElementById('login_section').style.display = showForgot ? 'none' : 'block';
            document.getElementById('forgot_section').style.display = showForgot ? 'block' : 'none';
        }

        document.getElementById('pass-toggle').addEventListener('click', function () {
            const input = document.getElementById('<%= txt_password.ClientID %>');
            const icon = document.getElementById('eye-icon');
            input.type = input.type === "password" ? "text" : "password";
            icon.classList.toggle('fa-eye');
            icon.classList.toggle('fa-eye-slash');
        });
    </script>
</body>
</html>
