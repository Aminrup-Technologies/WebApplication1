using System;
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
    public partial class LiveJOB_pnl : System.Web.UI.Page
    {
        private static readonly string SupersetBaseUrl = "https://reports.aminruptechnologies.co.in";
        private static readonly string EmbeddedId = "a867c534-fde8-4dbc-a0c1-a8be874baa0d";
        private static readonly string ServiceUsername = "magician";
        private static readonly string ServicePassword = "M@g1k_25";

        protected void Page_Load(object sender, EventArgs e)
        {
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
                string realError = ex.GetBaseException().Message;
                return JsonConvert.SerializeObject(new { error = realError });
            }
        }

        private static async Task<string> GetGuestTokenAsync()
        {
            using (HttpClient client = new HttpClient())
            {
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