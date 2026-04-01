using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.bussiness.production
{
    /// <summary>
    /// Centralized repository for all Session string keys to prevent typos and enable IntelliSense.
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
    }
}