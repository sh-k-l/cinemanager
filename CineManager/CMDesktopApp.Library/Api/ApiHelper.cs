using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CMDesktopApp.Library.Api
{
    public class ApiHelper : IApiHelper
    {
        private HttpClient _apiClient;
        public ApiHelper()
        {
            Initialise();
        }

        public HttpClient ApiClient
        {
            get
            {
                return _apiClient;   
            }
        }

        public void AddAuthToken(string token)
        {
            _apiClient.DefaultRequestHeaders.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiClient.DefaultRequestHeaders.Add("Authorization", $"bearer {token}");

        }

        public void ClearDefaultHeaders()
        {
            _apiClient.DefaultRequestHeaders.Clear();
        }

        private void Initialise()
        {
            string apiAddress = ConfigurationManager.AppSettings["api"];

            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri(apiAddress);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
