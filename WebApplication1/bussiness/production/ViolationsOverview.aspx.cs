using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApplication1.bussiness.production
{
    public partial class ViolationsOverview : System.Web.UI.Page
    {
        // ==========================================
        // CONFIGURATION VARIABLES
        // ==========================================
        private static readonly string SupersetBaseUrl = "https://reports.aminruptechnologies.co.in";

        // TODO: Replace with the same Embedded ID you put in the frontend script
        private static readonly string EmbeddedId = "ab1b802a-4bda-4b4d-84e8-792484030cdb";

        // TODO: Provide the Superset username and password of an account that has access to view this dashboard
        private static readonly string ServiceUsername = "magician";
        private static readonly string ServicePassword = "M@g1k_25";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Enforce TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        [WebMethod]
        public static string FetchSupersetGuestToken()
        {
            try
            {
                return Task.Run(() => GetGuestTokenAsync()).Result;
            }
            catch (Exception ex)
            {
                // CRITICAL FIX: Unwrap the AggregateException to get the REAL error message
                string realError = ex.GetBaseException().Message;
                return JsonConvert.SerializeObject(new { error = realError });
            }
        }

        private static async Task<string> GetGuestTokenAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                // STEP 1: Get the standard access token
                var loginPayload = new
                {
                    username = ServiceUsername,
                    password = ServicePassword,
                    provider = "db"
                };

                var loginContent = new StringContent(JsonConvert.SerializeObject(loginPayload), Encoding.UTF8, "application/json");

                var loginResponse = await client.PostAsync($"{SupersetBaseUrl}/api/v1/security/login", loginContent).ConfigureAwait(false);

                if (!loginResponse.IsSuccessStatusCode)
                {
                    var errorBody = await loginResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new Exception($"Superset Login API Failed: {loginResponse.StatusCode} | Details: {errorBody}");
                }

                var loginResult = await loginResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var accessToken = JObject.Parse(loginResult)["access_token"].ToString();

                // STEP 2: Request the Guest Token specific to this dashboard
                var guestTokenPayload = new
                {
                    user = new
                    {
                        username = "erp_embedded_user",
                        first_name = "ERP",
                        last_name = "User"
                    },
                    resources = new[]
                    {
                        new { type = "dashboard", id = EmbeddedId }
                    },
                    rls = new object[] { }
                };

                var guestContent = new StringContent(JsonConvert.SerializeObject(guestTokenPayload), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var guestResponse = await client.PostAsync($"{SupersetBaseUrl}/api/v1/security/guest_token/", guestContent).ConfigureAwait(false);

                if (!guestResponse.IsSuccessStatusCode)
                {
                    var errorBody = await guestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new Exception($"Superset Guest Token API Failed: {guestResponse.StatusCode} | Details: {errorBody}");
                }

                var guestResult = await guestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var guestToken = JObject.Parse(guestResult)["token"].ToString();

                return guestToken;
            }
        }
    }
}