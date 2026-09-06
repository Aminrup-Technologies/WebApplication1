using System;

namespace WebApplication1.bussiness.production
{
    /// <summary>
    /// Frozen M3 JOBID URL-safe Base64 encode/decode.
    /// Algorithm is identical to the former per-page copies on create / IN / Permit / OUT.
    /// Base64 is not authorization.
    /// </summary>
    public static class JobIdEncoding
    {
        public static string EncodeJobID(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return "";
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }

        public static string DecodeJobID(string maskedData)
        {
            if (string.IsNullOrEmpty(maskedData)) return "";
            string incoming = maskedData.Replace("-", "+").Replace("_", "/");
            switch (incoming.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }
            var base64EncodedBytes = Convert.FromBase64String(incoming);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
