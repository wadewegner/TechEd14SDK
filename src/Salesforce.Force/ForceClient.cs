using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Salesforce.Force
{
    public class ForceClient
    {
        private readonly string _instanceUrl;
        private readonly string _apiVersion;
        private readonly string _accessToken;
        private HttpClient _httpClient;

        public ForceClient(string instanceUrl, string accessToken, string apiVersion)
            : this(instanceUrl, accessToken, apiVersion, new HttpClient())
        {
        }

        public ForceClient(string instanceUrl, string accessToken, string apiVersion, HttpClient httpClient)
        {
            _instanceUrl = instanceUrl;
            _accessToken = accessToken;
            _apiVersion = apiVersion;
            _httpClient = httpClient;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<QueryResult<T>> QueryAsync<T>(string query)
        {
            var urlSuffix = string.Format("query?q={0}", query);

            var url = string.Format("{0}/services/data/{1}/{2}", _instanceUrl, _apiVersion, urlSuffix);

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get
            };

            var responseMessage = await _httpClient.SendAsync(request);
            var response = await responseMessage.Content.ReadAsStringAsync();


            if (responseMessage.IsSuccessStatusCode)
            {
                var jObject = JObject.Parse(response);

                var r = JsonConvert.DeserializeObject<QueryResult<T>>(jObject.ToString());
                return r;
            }

            var errorResponse = JsonConvert.DeserializeObject<ErrorResponses>(response);
            throw new Exception(errorResponse[0].message);
        }
                
    }

    public class QueryResult<T>
    {
        public string nextRecordsUrl { get; set; }
        public int totalSize { get; set; }
        public string done { get; set; }
        public List<T> records { get; set; }
    }

    public class ErrorResponses : List<ErrorResponse> { }

    public class ErrorResponse
    {
        public string message;
        public string errorCode;
    }
                
}
