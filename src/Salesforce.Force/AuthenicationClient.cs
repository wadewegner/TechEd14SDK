using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Salesforce.Force
{
    public class AuthenticationClient
    {
        private HttpClient _httpClient;
        private string _tokenRequestEndpointUrl = "https://login.salesforce.com/services/oauth2/token";

        public string InstanceUrl { get; set; }
        public string AccessToken { get; set; }
        public string ApiVersion { get; set; }

        public AuthenticationClient()
            : this(new HttpClient(), "https://login.salesforce.com/services/oauth2/token")
        {
        }

        public AuthenticationClient(HttpClient httpClient)
            : this(httpClient, "https://login.salesforce.com/services/oauth2/token")
        {
        }

        public AuthenticationClient(string tokenRequestEndpointUrl)
            : this(new HttpClient(), tokenRequestEndpointUrl)
        {
        }

        public AuthenticationClient(HttpClient httpClient, string tokenRequestEndpointUrl)
        {
            _httpClient = httpClient;
            _tokenRequestEndpointUrl = tokenRequestEndpointUrl;

            ApiVersion = "v29.0";
        }



        public async Task UsernamePasswordAsync(string clientId, string clientSecret, string username, string password)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_tokenRequestEndpointUrl),
                Content = content
            };

            var responseMessage = await _httpClient.SendAsync(request);
            var response = await responseMessage.Content.ReadAsStringAsync();

            if (responseMessage.IsSuccessStatusCode)
            {
                var authToken = JsonConvert.DeserializeObject<AuthToken>(response);

                AccessToken = authToken.access_token;
                InstanceUrl = authToken.instance_url;
            }
            else
            {
                var errorResponse = JsonConvert.DeserializeObject<AuthErrorResponse>(response);
                throw new Exception(errorResponse.error_description);
            }



        }
    }

    public class AuthErrorResponse
    {
        public string error_description;
        public string error;
    }


    public class AuthToken
    {
        public string id;
        public string issued_at;
        public string instance_url;
        public string signature;
        public string access_token;
        public string refresh_token;
    }

}
