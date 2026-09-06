using System;

namespace WebApplication1.bussiness.production
{
    /// <summary>
    /// Frozen M3 JOBID URL-safe Base64 encode/decode.
    /// Algorithm is copied verbatim from the former per-page EncodeJobID/DecodeJobID bodies.
    /// Base64 is not authorization.
    /// </summary>
    public static class JobIdCodec
    {
        public static string Encode(string jobId)
        {
            if (string.IsNullOrEmpty(jobId)) return "";
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(jobId);
            return Convert.ToBase64String(plainTextBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }

        public static string Decode(string encodedJobId)
        {
            if (string.IsNullOrEmpty(encodedJobId)) return "";
            string incoming = encodedJobId.Replace("-", "+").Replace("_", "/");
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
