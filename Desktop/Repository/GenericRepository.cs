using Desktop.Properties;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace Desktop.Repository
{
    public class GenericRepository<T>
    {
        private readonly string _token;

        public GenericRepository(string token)
        {
            _token = token;
        }

        public GenericRepository()
        {
        }

        private WebRequest CreateRequest(string url, string method)
        {
            var request = WebRequest.CreateHttp($"{Settings.Default.BaseUrl}{url}");
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Method = method;
            if (_token != null)
            {
                request.Headers.Add("Authorization", $"Bearer {_token}");
            }

            return request;
        }

        private WebRequest CreateGetRequest(string url)
        {
            return CreateRequest(url, "GET");
        }

        private WebRequest CreatePostRequest(string url)
        {
            return CreateRequest(url, "POST");
        }

        private T CreateResponse(WebRequest request)
        {
            var response = request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()))
            {
                var jsonResponse = reader.ReadToEnd();
                var result = JsonConvert.DeserializeObject<T>(jsonResponse);

                return result;
            }
        }

        public T Post(string url, object body)
        {
            var request = CreatePostRequest(url);

            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                var jsonBody = JsonConvert.SerializeObject(body);
                writer.Write(jsonBody);
            }

            return CreateResponse(request);
        }

        public T Get(string url)
        {
            var request = CreateGetRequest(url);

            return CreateResponse(request);
        }
    }
}
