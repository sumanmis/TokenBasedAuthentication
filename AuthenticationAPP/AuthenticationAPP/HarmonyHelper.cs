using AuthenticationAPP.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace AuthenticationAPP
{
    public class HarmonyHelper
    {

        private static HttpClient _httpClient = new HttpClient();
        HarmonySettings settings = new HarmonySettings();
        
        public void CreateContentBlock(string content)
        {

            content = "<HTML><BODY><br><B>Test Content</B></BODY></HTML>";
            settings.content.content = content;
        }

        public void SetHarmonyApiHeaders(HarmonySettings settings)
        {
            var authResponse = GetHarmonyApiAuthResponse(settings);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MIME_TYPE_APPLICATION_JSON));
            _httpClient.DefaultRequestHeaders.Add("Authorization", string.Concat(authResponse.TokenType, " ", authResponse.AccessToken));
            _httpClient.DefaultRequestHeaders.Add("X-OUID", settings.OUID);
        }

        public HarmonyAuthResponse GetHarmonyApiAuthResponse(HarmonySettings settings, bool refreshToken = false)
        {
            var authResponse = (HarmonyAuthResponse)HttpRuntime.Cache[Constants.CACHE_GET_HARMONY_API_AUTH_RESPONSE];
            settings.AuthUrl = "https://api.harmony.epsilon.com/v1/contentBlocks";
            if (authResponse == null
                || refreshToken)
            {
              
                // get auth token
                var byteArray = new UTF8Encoding().GetBytes(string.Concat(settings.ClientId, ":", settings.ClientSecret));

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                var formData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("scope", "cn mail sn givenname uid employeeNumber"),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", settings.ApiUsername),
                    new KeyValuePair<string, string>("password", settings.ApiPassword),
                    new KeyValuePair<string, string>("characterSet", settings.content.characterSet),
                    new KeyValuePair<string, string>("content", settings.content.content),
                    new KeyValuePair<string, string>("contentType", settings.content.contentType),
                    new KeyValuePair<string, string>("description", settings.description),
                    new KeyValuePair<string, string>("name", settings.name),
                    new KeyValuePair<string, string>("parentId", settings.parentId),
                    new KeyValuePair<string, string>("type", settings.type),
                    new KeyValuePair<string, string>("subType", settings.subType),

                };

                var payload = new FormUrlEncodedContent(formData);

                var response = _httpClient.PostAsync(settings.AuthUrl, payload).Result;
                var data = response.Content.ReadAsStringAsync().Result;
                response.EnsureSuccessStatusCode();

                authResponse = JObject.Parse(data).ToObject<HarmonyAuthResponse>();
                HttpRuntime.Cache.Insert(Constants.CACHE_GET_HARMONY_API_AUTH_RESPONSE, authResponse, null, DateTime.Now.AddHours(2), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            

            return authResponse;
        
    }
}
}