<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="WebApplication1.bussiness.production.login" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>ATS | Cloud ERP - Login</title>

    <link rel="preconnect" href="https://cdn.jsdelivr.net" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="bussiness/vendors/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/@pnotify/core@5.2.0/dist/PNotify.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/@pnotify/core@5.2.0/dist/BrightTheme.css" rel="stylesheet" />

    <style type="text/css">
        body { background: #f0f2f5; min-height: 100vh; display: flex; align-items: center; justify-content: center; font-family: 'Inter', sans-serif; padding: 15px; }
        .login-card { background: #fff; border-radius: 12px; box-shadow: 0 10px 30px rgba(0,0,0,0.08); width: 100%; max-width: 420px; padding: 2.5rem; }
        .brand-logo { display: block; margin: 0 auto 1.2rem; }
        .btn-success { background-color: #198754; border: none; padding: 0.7rem; font-weight: 600; }
        .nav-pills .nav-link { border-radius: 8px; font-weight: 600; color: #6c757d; }
        .nav-pills .nav-link.active { background-color: #198754; color: #fff; }
        .pnotify-custom { border-radius: 8px; font-weight: 600; }
        .footer-text { font-size: 0.8rem; text-align: center; margin-top: 2.5rem; color: #6c757d; border-top: 1px solid #e9ecef; padding-top: 1rem; }
        .footer-text a { color: #198754; transition: color 0.2s; }
        .footer-text a:hover { color: #146c43; text-decoration: underline !important; }
        .mfa-qr { display: flex; justify-content: center; margin: 0 auto 0.75rem; }
        .mfa-manual { font-family: ui-monospace, Consolas, monospace; letter-spacing: 0.08em; word-break: break-all; }
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
            <h1 class="h5 text-center fw-bold mb-4">ATS Cloud ERP</h1>

            <ul runat="server" class="nav nav-pills nav-justified mb-4" id="loginTabs" role="tablist">
                <li class="nav-item" role="presentation">
                    <button runat="server" id="tab_login_btn" class="nav-link active" data-bs-toggle="pill" data-bs-target="#pane_login" type="button" role="tab" aria-selected="true">Login</button>
                </li>
                <li class="nav-item" role="presentation">
                    <button runat="server" id="tab_forgot_btn" class="nav-link" data-bs-toggle="pill" data-bs-target="#pane_forgot" type="button" role="tab" aria-selected="false">Reset Password</button>
                </li>
            </ul>

            <div class="tab-content" id="loginTabsContent">
                
                <div runat="server" id="pane_login" class="tab-pane fade show active" role="tabpanel" ClientIDMode="Static">
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

                    <div class="form-check mb-4">
                        <asp:CheckBox ID="chk_remember" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label small" for="chk_remember">Remember Me</label>
                    </div>

                    <asp:Button ID="btn_login" runat="server" Text="LOG IN" CssClass="btn btn-success w-100" OnClick="btn_login_Click" OnClientClick="return validateLogin();" />
                </div>

                <div runat="server" id="pane_forgot" class="tab-pane fade" role="tabpanel" ClientIDMode="Static">
                    <label class="form-label small fw-bold text-muted">Employee Code</label>
                    <asp:TextBox ID="txt_reset_code" runat="server" CssClass="form-control mb-3" placeholder="Enter Login ID"></asp:TextBox>
                    
                    <asp:Button ID="btn_fetch_email" runat="server" Text="FETCH DETAILS" CssClass="btn btn-secondary w-100 mb-3" OnClick="btn_fetch_email_Click" />

                    <asp:PlaceHolder ID="ph_email_section" runat="server" Visible="false">
                        <label class="form-label small fw-bold text-muted">Registered Email (Edit to update)</label>
                        <asp:TextBox ID="txt_reset_email" runat="server" CssClass="form-control mb-3" placeholder="Email Address"></asp:TextBox>
                        <asp:Button ID="btn_send_otp" runat="server" Text="SEND OTP" CssClass="btn btn-primary w-100 mb-3" OnClick="btn_send_otp_Click" />
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="ph_otp" runat="server" Visible="false">
                        <label class="form-label small fw-bold text-muted">Enter OTP & New Password</label>
                        <asp:TextBox ID="txt_otp" runat="server" CssClass="form-control mb-2" placeholder="6-Digit OTP"></asp:TextBox>
                        <asp:TextBox ID="txt_new_pass" runat="server" CssClass="form-control mb-3" TextMode="Password" placeholder="New Password"></asp:TextBox>
                        <asp:Button ID="btn_verify_reset" runat="server" Text="VERIFY & RESET" CssClass="btn btn-success w-100 mb-2" OnClick="btn_verify_reset_Click" />
                    </asp:PlaceHolder>
                </div>

                <div runat="server" id="pane_mfa" class="tab-pane fade" role="tabpanel" visible="false" ClientIDMode="Static">
                    <p class="text-center fw-bold mb-2">Two-step verification</p>
                    <p class="small text-muted text-center mb-3">
                        <asp:Label ID="lbl_mfa_hint" runat="server" Text="Enter the 6-digit verification code."></asp:Label>
                    </p>
                    <asp:PlaceHolder ID="ph_mfa_enroll" runat="server" Visible="false">
                        <div id="mfaQrBox" class="mfa-qr"></div>
                        <asp:HiddenField ID="hf_mfa_otpauth" runat="server" />
                        <p class="small text-muted text-center mb-1">Can't scan? Enter this key in your authenticator app:</p>
                        <p class="small text-center fw-bold mfa-manual mb-3">
                            <asp:Label ID="lbl_mfa_manual" runat="server"></asp:Label>
                        </p>
                        <script src="https://cdnjs.cloudflare.com/ajax/libs/qrcodejs/1.0.0/qrcode.min.js"></script>
                    </asp:PlaceHolder>
                    <div class="mb-3">
                        <label class="form-label small fw-bold text-muted">VERIFICATION CODE</label>
                        <asp:TextBox ID="txt_mfa_otp" runat="server" CssClass="form-control" MaxLength="6" placeholder="6-Digit code" autocomplete="one-time-code"></asp:TextBox>
                    </div>
                    <asp:Button ID="btn_mfa_verify" runat="server" Text="VERIFY & CONTINUE" CssClass="btn btn-success w-100 mb-2" OnClick="btn_mfa_verify_Click" />
                    <asp:Button ID="btn_mfa_resend" runat="server" Text="RESEND CODE" CssClass="btn btn-outline-secondary w-100 mb-2" OnClick="btn_mfa_resend_Click" CausesValidation="false" />
                    <asp:Button ID="btn_mfa_back" runat="server" Text="BACK TO LOGIN" CssClass="btn btn-link w-100 text-muted" OnClick="btn_mfa_back_Click" CausesValidation="false" />
                </div>
            </div>

            <div class="footer-text">
                <p class="mb-0">
                    © 2021-2026 All Rights Reserved. <br />
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

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@pnotify/core@5.2.0/dist/PNotify.js"></script>
    <script>
        window.notify = function (title, text, type) {
            if (typeof PNotify === "undefined") {
                alert(title + ": " + text); return;
            }
            PNotify.alert({ title: title, text: text, type: type, delay: 3000, addClass: 'pnotify-custom' });
        };

        function validateLogin() {
            const id = document.getElementById('<%= txt_loginid.ClientID %>').value;
            const pass = document.getElementById('<%= txt_password.ClientID %>').value;
            if (!id || !pass) {
                notify('Wait!', 'Please enter both User ID and Password.', 'notice');
                return false;
            }
            return true;
        }

        document.addEventListener("DOMContentLoaded", function () {
            const toggle = document.getElementById('pass-toggle');
            if (toggle) {
                toggle.addEventListener('click', function () {
                    const input = document.getElementById('<%= txt_password.ClientID %>');
                    const icon = document.getElementById('eye-icon');
                    input.type = input.type === "password" ? "text" : "password";
                    icon.classList.toggle('fa-eye');
                    icon.classList.toggle('fa-eye-slash');
                });
            }
            renderMfaQr();
        });

        function renderMfaQr() {
            const box = document.getElementById('mfaQrBox');
            const uriField = document.getElementById('<%= hf_mfa_otpauth.ClientID %>');
            if (!box || !uriField || !uriField.value || typeof QRCode === 'undefined') return;
            box.innerHTML = '';
            new QRCode(box, { text: uriField.value, width: 176, height: 176, correctLevel: QRCode.CorrectLevel.M });
        }
    </script>
</body>
</html>