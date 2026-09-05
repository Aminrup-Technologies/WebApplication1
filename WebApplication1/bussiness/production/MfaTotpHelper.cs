using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.bussiness.production
{
    /// <summary>
    /// RFC 6238 TOTP (HMAC-SHA1, 6 digits, 30-second step) for authenticator apps.
    /// Secrets are stored as Base32, matching Google Authenticator / Microsoft Authenticator.
    /// </summary>
    public static class MfaTotpHelper
    {
        public const string Issuer = "ATS Cloud ERP";
        public const int StepSeconds = 30;
        public const int WindowSteps = 1;
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        public static string GenerateSecret()
        {
            byte[] bytes = new byte[20];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return ToBase32(bytes);
        }

        public static string BuildOtpAuthUri(string accountName, string base32Secret)
        {
            string account = string.IsNullOrWhiteSpace(accountName) ? "user" : accountName.Trim();
            string label = Issuer + ":" + account;
            return "otpauth://totp/" + Uri.EscapeDataString(label)
                + "?secret=" + (base32Secret ?? "")
                + "&issuer=" + Uri.EscapeDataString(Issuer)
                + "&digits=6&period=" + StepSeconds;
        }

        public static bool ValidateCode(string base32Secret, string pin)
        {
            if (string.IsNullOrWhiteSpace(base32Secret) || string.IsNullOrWhiteSpace(pin)) return false;
            string code = pin.Trim();
            if (code.Length != 6) return false;

            long timestep = UnixTimeSeconds() / StepSeconds;
            for (int i = -WindowSteps; i <= WindowSteps; i++)
            {
                if (MfaAuthHelper.SlowEquals(ComputeCode(base32Secret, timestep + i), code))
                    return true;
            }
            return false;
        }

        public static string ComputeCode(string base32Secret, long timestep)
        {
            byte[] key = FromBase32(base32Secret);
            if (key == null || key.Length == 0) return "000000";

            byte[] counter = new byte[8];
            ulong ts = (ulong)timestep;
            for (int i = 7; i >= 0; i--)
            {
                counter[i] = (byte)(ts & 0xff);
                ts >>= 8;
            }

            byte[] hash;
            using (HMACSHA1 hmac = new HMACSHA1(key))
            {
                hash = hmac.ComputeHash(counter);
            }

            int offset = hash[hash.Length - 1] & 0x0f;
            int binary = ((hash[offset] & 0x7f) << 24)
                | ((hash[offset + 1] & 0xff) << 16)
                | ((hash[offset + 2] & 0xff) << 8)
                | (hash[offset + 3] & 0xff);
            int otp = binary % 1000000;
            return otp.ToString("D6");
        }

        public static string ToBase32(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return "";
            StringBuilder output = new StringBuilder();
            int bitBuffer = 0;
            int bitsInBuffer = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                bitBuffer = (bitBuffer << 8) | bytes[i];
                bitsInBuffer += 8;
                while (bitsInBuffer >= 5)
                {
                    bitsInBuffer -= 5;
                    output.Append(Alphabet[(bitBuffer >> bitsInBuffer) & 31]);
                }
            }
            if (bitsInBuffer > 0)
            {
                output.Append(Alphabet[(bitBuffer << (5 - bitsInBuffer)) & 31]);
            }
            return output.ToString();
        }

        public static byte[] FromBase32(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return new byte[0];
            string s = input.Trim().Replace(" ", "").Replace("=", "").ToUpperInvariant();
            List<byte> output = new List<byte>();
            int bitBuffer = 0;
            int bitsInBuffer = 0;
            for (int i = 0; i < s.Length; i++)
            {
                int val = Alphabet.IndexOf(s[i]);
                if (val < 0) continue;
                bitBuffer = (bitBuffer << 5) | val;
                bitsInBuffer += 5;
                if (bitsInBuffer >= 8)
                {
                    bitsInBuffer -= 8;
                    output.Add((byte)((bitBuffer >> bitsInBuffer) & 0xff));
                }
            }
            return output.ToArray();
        }

        private static long UnixTimeSeconds()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }
    }
}
