using Newtonsoft.Json;
using SpiceApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace RestApiDenemeleri
{
    public static class Request<T> where T : class
    {
        private static readonly HttpClient client = new HttpClient();
        public static  Response<T> PostAsync(string uri, StringContent body)
        {
            using (HttpResponseMessage response = client.PostAsync(uri, body).Result)
            {
                return JsonConvert.DeserializeObject<Response<T>>(response.Content.ReadAsStringAsync().Result);
            }            
        }
        public static Response<T> GetAsync(string path)
        {
            using(HttpResponseMessage response = client.GetAsync(path).Result)
            {
                return JsonConvert.DeserializeObject<Response<T>>(response.Content.ReadAsStringAsync().Result);
            }
        }
        public static Response<T> PutAsync(string uri, StringContent content)
        {
            using(HttpResponseMessage response =  client.PutAsync(uri, content).Result)
            {
                return JsonConvert.DeserializeObject<Response<T>>(response.Content.ReadAsStringAsync().Result);
            }
        }
        public static  Response<T> DeleteAsync(string uri)
        {
            using(HttpResponseMessage response =  client.DeleteAsync(uri).Result)
            {
                return JsonConvert.DeserializeObject<Response<T>>( response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
