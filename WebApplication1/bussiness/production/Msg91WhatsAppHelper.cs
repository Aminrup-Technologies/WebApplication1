using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace WebApplication1.bussiness.production
{
    /// <summary>
    /// Sends the existing login MFA OTP over MSG91 WhatsApp using the same
    /// auth key, integrated number, and WABA namespace as job alerts.
    /// Does not generate a second OTP.
    /// </summary>
    public static class Msg91WhatsAppHelper
    {
        public const string BulkUrl = "https://api.msg91.com/api/v5/whatsapp/whatsapp-outbound-message/bulk/";
        public const string DefaultNamespace = "af05507b_02e4_4d95_8f8c_164ce03fc2df";

        public static string BuildOtpJson(string integratedNumber, string templateName, string language, string ns, string mobile, string otp)
        {
            var payload = new
            {
                integrated_number = integratedNumber,
                content_type = "template",
                payload = new
                {
                    messaging_product = "whatsapp",
                    type = "template",
                    template = new
                    {
                        name = templateName,
                        language = new { code = language, policy = "deterministic" },
                        @namespace = ns,
                        to_and_components = new[]
                        {
                            new
                            {
                                to = new[] { mobile },
                                components = new Dictionary<string, object>
                                {
                                    { "body_1", new { type = "text", value = otp } }
                                }
                            }
                        }
                    }
                }
            };
            return new JavaScriptSerializer().Serialize(payload).Replace("\"@namespace\"", "\"namespace\"");
        }

        public static bool SendOtp(string mobile, string otp, out string error)
        {
            error = "";
            string authKey = (ConfigurationManager.AppSettings["Msg91AuthKey"] ?? "").Trim();
            string integratedNumber = (ConfigurationManager.AppSettings["Msg91IntegratedNumber"] ?? "").Trim();
            string templateName = (ConfigurationManager.AppSettings["Msg91MfaTemplateName"] ?? "").Trim();
            string ns = (ConfigurationManager.AppSettings["Msg91WhatsAppNamespace"] ?? "").Trim();
            string language = (ConfigurationManager.AppSettings["Msg91WhatsAppLanguage"] ?? "").Trim();

            if (string.IsNullOrEmpty(ns)) ns = DefaultNamespace;
            if (string.IsNullOrEmpty(language)) language = "en";

            if (string.IsNullOrEmpty(authKey) || string.IsNullOrEmpty(integratedNumber))
            {
                error = "MSG91 AuthKey or IntegratedNumber is not configured.";
                return false;
            }
            if (string.IsNullOrEmpty(templateName))
            {
                error = "MSG91 MFA WhatsApp template is not configured (Msg91MfaTemplateName).";
                return false;
            }
            if (string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(otp))
            {
                error = "Mobile number or OTP was missing.";
                return false;
            }

            string json = BuildOtpJson(integratedNumber, templateName, language, ns, mobile, otp);

            try
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BulkUrl);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers["authkey"] = authKey;
                request.Timeout = 20000;
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    reader.ReadToEnd();
                    int status = (int)response.StatusCode;
                    if (status >= 200 && status < 300) return true;
                    error = "MSG91 rejected the WhatsApp OTP request.";
                    return false;
                }
            }
            catch (WebException ex)
            {
                error = "MSG91 WhatsApp request failed.";
                try
                {
                    if (ex.Response != null)
                    {
                        using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                        {
                            string body = reader.ReadToEnd();
                            if (!string.IsNullOrWhiteSpace(body)) error = error + " " + body;
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(ex.Message))
                    {
                        error = error + " " + ex.Message;
                    }
                }
                catch
                {
                    if (!string.IsNullOrWhiteSpace(ex.Message)) error = error + " " + ex.Message;
                }
                return false;
            }
            catch (Exception ex)
            {
                error = string.IsNullOrWhiteSpace(ex.Message) ? "Unexpected WhatsApp send error." : ex.Message;
                return false;
            }
        }
    }
}
