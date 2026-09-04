using System;

namespace WebApplication1.bussiness.production
{
    /// <summary>
    /// Shared MFA helpers for login challenge and admin configuration.
    /// Methods: Email OTP and Authenticator (TOTP).
    /// </summary>
    public static class MfaAuthHelper
    {
        public const string MethodEmailOtp = "EmailOTP";
        public const string MethodAuthenticator = "Authenticator";
        public const int OtpLifetimeMinutes = 5;
        public const int MaxOtpAttempts = 3;
        public const int ResendCooldownSeconds = 60;

        public static bool IsEnabled(object value)
        {
            if (value == null || value == DBNull.Value) return false;
            if (value is bool) return (bool)value;
            if (value is byte) return ((byte)value) != 0;
            if (value is int) return ((int)value) != 0;
            if (value is short) return ((short)value) != 0;

            string text = value.ToString().Trim();
            if (text.Length == 0) return false;
            if (text == "1") return true;
            return text.Equals("true", StringComparison.OrdinalIgnoreCase)
                || text.Equals("yes", StringComparison.OrdinalIgnoreCase);
        }

        public static bool HasEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            string trimmed = email.Trim();
            int at = trimmed.IndexOf('@');
            return at > 0 && at < trimmed.Length - 1;
        }

        public static string MaskEmail(string email)
        {
            if (!HasEmail(email)) return "(no email)";

            string trimmed = email.Trim();
            int at = trimmed.IndexOf('@');
            string local = trimmed.Substring(0, at);
            string domain = trimmed.Substring(at);

            if (local.Length <= 1) return local + "***" + domain;
            return local.Substring(0, 1) + "***" + domain;
        }

        public static bool SlowEquals(string a, string b)
        {
            if (a == null || b == null || a.Length != b.Length) return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }
            return diff == 0;
        }

        public static bool IsMissingColumnException(Exception ex)
        {
            if (ex == null) return false;
            string msg = ex.Message ?? "";
            return msg.IndexOf("Invalid column name", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static string NormalizeMethod(string method)
        {
            if (string.IsNullOrWhiteSpace(method)) return MethodEmailOtp;
            string value = method.Trim();
            if (value.Equals(MethodAuthenticator, StringComparison.OrdinalIgnoreCase)
                || value.Equals("TOTP", StringComparison.OrdinalIgnoreCase)
                || value.Equals("AuthenticatorApp", StringComparison.OrdinalIgnoreCase))
            {
                return MethodAuthenticator;
            }
            return MethodEmailOtp;
        }

        public static bool IsAuthenticator(string method)
        {
            return NormalizeMethod(method) == MethodAuthenticator;
        }
    }
}
