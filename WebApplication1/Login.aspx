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
        .login-busy {
            display: none;
            position: fixed;
            inset: 0;
            z-index: 100000;
            background: rgba(255,255,255,0.92);
            align-items: center;
            justify-content: center;
            flex-direction: column;
        }
        .login-busy.is-on { display: flex; }
        .login-busy-spin {
            width: 42px;
            height: 42px;
            border: 4px solid #e9ecef;
            border-top-color: #198754;
            border-radius: 50%;
            animation: login-busy-spin 0.8s linear infinite;
        }
        .login-busy p { margin-top: 14px; font-weight: 600; color: #2a3f54; }
        .login-busy-timer {
            margin-top: 8px;
            font-size: 1.4rem;
            font-weight: 700;
            font-variant-numeric: tabular-nums;
            font-family: ui-monospace, Consolas, monospace;
            color: #198754;
        }
        .login-busy-phases {
            margin-top: 6px;
            font-size: 0.8rem;
            font-weight: 600;
            color: #6c757d;
        }
        @keyframes login-busy-spin { to { transform: rotate(360deg); } }
    </style>

    <script type="text/javascript">
        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
    </script>
</head>
<body>
    <div id="login-busy" class="login-busy" aria-live="polite" aria-busy="true">
        <div class="login-busy-spin"></div>
        <p id="login-busy-msg">Signing you in...</p>
        <div id="login-busy-timer" class="login-busy-timer">0.0s</div>
        <div id="login-busy-phases" class="login-busy-phases"></div>
    </div>
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
                    <asp:Button ID="btn_mfa_verify" runat="server" Text="VERIFY & CONTINUE" CssClass="btn btn-success w-100 mb-2" OnClick="btn_mfa_verify_Click" OnClientClick="return showLoginBusy('Verifying...');" />
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

        var ATS_TIMER_KEY = 'atsLoginTimer';
        var atsTimerTick = null;

        function atsReadTimer() {
            try {
                var raw = sessionStorage.getItem(ATS_TIMER_KEY);
                return raw ? JSON.parse(raw) : null;
            } catch (e) { return null; }
        }

        function atsWriteTimer(t) {
            try { sessionStorage.setItem(ATS_TIMER_KEY, JSON.stringify(t)); } catch (e) { }
        }

        function atsClearTimer() {
            try {
                sessionStorage.removeItem(ATS_TIMER_KEY);
                sessionStorage.removeItem('atsLoginNav');
            } catch (e) { }
        }

        function atsFormatMs(ms) {
            if (!isFinite(ms) || ms < 0) ms = 0;
            var s = ms / 1000;
            if (s < 60) return s.toFixed(1) + 's';
            var m = Math.floor(s / 60);
            var rem = s - (m * 60);
            return m + ':' + (rem < 10 ? '0' : '') + rem.toFixed(1);
        }

        function atsPhaseText(t, liveMs) {
            var parts = [];
            if (t.loginMs != null) parts.push('Login ' + atsFormatMs(t.loginMs));
            else if (t.phase === 'login') parts.push('Login ' + atsFormatMs(liveMs));
            if (t.verifyMs != null) parts.push('Verify ' + atsFormatMs(t.verifyMs));
            else if (t.phase === 'verify') parts.push('Verify ' + atsFormatMs(liveMs));
            return parts.join('  ·  ');
        }

        function atsTimerTotals(t) {
            var done = (t.loginMs || 0) + (t.verifyMs || 0) + (t.homeMs || 0);
            if (t.busyAt) return done + (Date.now() - t.busyAt);
            return done;
        }

        function atsPaintLoginTimer() {
            var t = atsReadTimer();
            var timerEl = document.getElementById('login-busy-timer');
            var phaseEl = document.getElementById('login-busy-phases');
            if (!t || !timerEl) return;
            var live = t.busyAt ? (Date.now() - t.busyAt) : 0;
            timerEl.textContent = atsFormatMs(atsTimerTotals(t));
            if (phaseEl) phaseEl.textContent = atsPhaseText(t, live);
        }

        function atsStartLoginTick() {
            atsPaintLoginTimer();
            if (atsTimerTick) return;
            atsTimerTick = setInterval(atsPaintLoginTimer, 100);
        }

        function atsCloseOpenPhase(t) {
            if (!t || !t.busyAt || !t.phase) return t;
            var elapsed = Date.now() - t.busyAt;
            if (t.phase === 'login') t.loginMs = (t.loginMs || 0) + elapsed;
            else if (t.phase === 'verify') t.verifyMs = (t.verifyMs || 0) + elapsed;
            t.busyAt = null;
            return t;
        }

        function showLoginBusy(msg) {
            var el = document.getElementById('login-busy');
            var text = document.getElementById('login-busy-msg');
            if (text && msg) text.textContent = msg;
            if (el) el.classList.add('is-on');
            try { sessionStorage.setItem('atsLoginNav', '1'); } catch (e) { }

            var t = atsReadTimer() || {};
            var isVerify = msg && msg.toLowerCase().indexOf('verif') >= 0;
            if (isVerify) {
                if (t.phase === 'login') t = atsCloseOpenPhase(t);
                if (t.phase !== 'verify' || !t.busyAt) {
                    t.phase = 'verify';
                    t.busyAt = Date.now();
                }
            } else {
                t = { loginMs: null, verifyMs: null, homeMs: null, phase: 'login', busyAt: Date.now() };
            }
            if (!t.busyAt) t.busyAt = Date.now();
            atsWriteTimer(t);
            atsStartLoginTick();
            return true;
        }

        function validateLogin() {
            const id = document.getElementById('<%= txt_loginid.ClientID %>').value;
            const pass = document.getElementById('<%= txt_password.ClientID %>').value;
            if (!id || !pass) {
                notify('Wait!', 'Please enter both User ID and Password.', 'notice');
                return false;
            }
            return showLoginBusy('Signing you in...');
        }

        document.addEventListener("DOMContentLoaded", function () {
            try {
                if (!document.getElementById('pane_mfa')) {
                    atsClearTimer();
                } else {
                    var t = atsReadTimer();
                    if (t && t.busyAt) {
                        t = atsCloseOpenPhase(t);
                        t.phase = 'mfaWait';
                        atsWriteTimer(t);
                    }
                }
            } catch (e) { }

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
            bindMfaAutoSubmit();
        });

        function bindMfaAutoSubmit() {
            var input = document.getElementById('<%= txt_mfa_otp.ClientID %>');
            var btn = document.getElementById('<%= btn_mfa_verify.ClientID %>');
            if (!input || !btn || input.getAttribute('data-autosubmit') === '1') return;
            input.setAttribute('data-autosubmit', '1');
            input.setAttribute('inputmode', 'numeric');
            var submitted = false;
            input.addEventListener('input', function () {
                var digits = (input.value || '').replace(/\D/g, '');
                if (digits.length > 6) digits = digits.substring(0, 6);
                if (input.value !== digits) input.value = digits;
                if (digits.length === 6 && !submitted && !btn.disabled) {
                    submitted = true;
                    showLoginBusy('Verifying...');
                    btn.click();
                    return;
                }
                if (digits.length < 6) submitted = false;
            });
        }

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