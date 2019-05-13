using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AAD_Sample.Web.Extensions;
using AAD_Sample.Web.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Web.InteractionTests.Helpers
{
    public static class HttpClientExtensions
    {
        public static async Task<string> GetTokenAsync(
            this HttpClient client, 
            string username,
            string password)
        {
            var request = new TokenRequest
            {
                Username = username,
                Password = password
            };
            var response = await client.PostAsync("api/token", request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.ReadAsAsync<TokenResponse>();
                return result?.Token;
            }
            return null;
        }

        public static async Task<HttpResponseMessage> GetAsync(this HttpClient client, string pathString,
            Action<HttpRequestMessage> replay = null)
        {
            var path = Normalize(pathString);
            var requestUri = path.ToUriComponent();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            replay?.Invoke(request);

            return await client.SendAsync(request);
        }

        public static async Task<HttpResponseMessage> PostAsync<T>(this HttpClient client, string pathString, T body,
            Action<HttpRequestMessage> replay = null) where T : class
        {
            var path = Normalize(pathString);
            var requestUri = path.ToUriComponent();
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
            };
            replay?.Invoke(request);

            return await client.SendAsync(request);
        }
        
        private static PathString Normalize(string pathString)
        {
            Contract.Assert(pathString != null);
            if (!pathString.StartsWith("/"))
                pathString = $"/{pathString}";
            if (!pathString.EndsWith("/"))
                pathString = $"{pathString}/";
            return pathString;
        }
    }
}
