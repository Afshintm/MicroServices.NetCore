using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Http.Utils.Standard
{
    public interface IHttpClientManager:IDisposable {
        Task<T> GetAsync<T>(string path, string parameters = null) where T : class, new();
        Task<T> PostAsync<T>(string path, object body) where T : class;
        string ParamBuilder(string controller, string action = null, Dictionary<string, string> parameters = null, bool keyValuPairFlag = false);
        void ConfigureHttpClient(IDictionary<string, string> headers);
    }
    public class HttpClientManager : IHttpClientManager
    {
        HttpClient _httpClient;
        public HttpClientManager() {
            _httpClient = new HttpClient();
        }
        
        public void ConfigureHttpClient(IDictionary<string, string> headers)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public async Task<T> GetAsync<T>(string path, string parameters = null) where T : class ,new()
        {
            if (string.IsNullOrEmpty(path))
                return null;
            if (parameters != null) {
            }
            var response =  _httpClient.GetAsync(parameters);
            var t = response.Result;
            return await Task.Run(() => {
                return new T();
            });

        }

        public string ParamBuilder(string controller, string action = null, Dictionary<string, string> parameters = null, bool keyValuPairFlag = false)
        {
            throw new NotImplementedException();
        }

        public Task<T> PostAsync<T>(string path, object body) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
