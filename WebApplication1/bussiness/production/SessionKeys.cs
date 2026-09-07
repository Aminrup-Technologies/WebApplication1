using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.bussiness.production
{
    /// <summary>
    /// Centralized repository for all Session string keys to prevent typos and enable IntelliSense.
    /// WHEN: 2026-09-06
    /// WHY: PR #88 adds impersonation snapshot key names without changing login Session writes.
    /// WHAT: IS_IMPERSONATING and ORIGINAL_* constants. No caller sets these yet.
    /// </summary>
    public class SessionKeys
    {
        // Core User Data
        public const string UserID = "USERID";
        public const string WorkmanSL = "WORKMAN";
        public const string UserName = "USERNAME";
        public const string UserFirstName = "USERFNAME";
        public const string UserPhoto = "User_Photo";

        // Roles & Permissions
        public const string RolePermissionDB = "RolePermissionDB";
        public const string UserRoleDB = "UserRoleDB";
        public const string Region = "REGION";
        public const string UserType = "USERTYPE";
        public const string Permission = "PERMISSION";

        // Employee Attributes
        public const string UserState = "STATE";
        public const string CompanyCode = "COMPANY_CODE";
        public const string WorkSite = "U_SITE";
        public const string SiteCode = "U_SITECODE";
        public const string Designation = "U_DESG";
        public const string Skill = "U_SKILL";

        // Temporary/Action States
        public const string GeneratedOTP = "GeneratedOTP";
        public const string RecipientEmail = "RecipientEmail";
        public const string BaseQRData = "BaseQRData";

        // Login MFA pending challenge (set only after password succeeds; USERID is not set yet)
        public const string MfaPendingLoginId = "MFA_PENDING_LOGINID";
        public const string MfaOtpHash = "MFA_OTP_HASH";
        public const string MfaOtpExp = "MFA_OTP_EXP";
        public const string MfaOtpTry = "MFA_OTP_TRY";
        public const string MfaOtpEmail = "MFA_OTP_EMAIL";
        public const string MfaOtpMobile = "MFA_OTP_MOBILE";
        public const string MfaRemember = "MFA_REMEMBER";
        public const string MfaResendAt = "MFA_RESEND_AT";
        public const string MfaMethod = "MFA_METHOD";
        public const string MfaTotpEnroll = "MFA_TOTP_ENROLL";
        public const string MfaTotpSecret = "MFA_TOTP_SECRET";

        // One-shot overlay after a successful login until homepage_v2 has painted
        public const string ShowHomeLoader = "ATS_SHOW_HOME_LOADER";

        // Impersonation snapshot (set only while an admin is viewing as another user)
        public const string IsImpersonating = "IS_IMPERSONATING";
        public const string OriginalUserID = "ORIGINAL_USERID";
        public const string OriginalWorkmanSL = "ORIGINAL_WORKMAN";
        public const string OriginalUserName = "ORIGINAL_USERNAME";
        public const string OriginalUserType = "ORIGINAL_USERTYPE";
        public const string OriginalUserRoleDB = "ORIGINAL_UserRoleDB";
        public const string OriginalRolePermissionDB = "ORIGINAL_RolePermissionDB";
    }
}
