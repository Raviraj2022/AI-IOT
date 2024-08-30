using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AJ_KITCHEN_SENS_SVC.Common
{
    public class HttpClientHelper<T>
    {
        public T GetRequestWithoutAuth(string apiUrl)
        {
            string endpoint = apiUrl;
            var result1 = default(T);
            try
            {


                using (HttpClient client = new HttpClient())
                {
                    // client.DefaultRequestHeaders.Add("Authorization", BasicAuth);
                    using (var Response = client.GetAsync(endpoint))
                    {
                        Response.Wait();
                        var result = Response.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var readTask = result.Content.ReadAsStringAsync();
                            readTask.Wait();
                            result1 = JsonConvert.DeserializeObject<T>(readTask.Result);

                        }


                    }
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
            return result1;
        }
        public T GetSingleItemRequest(string apiUrl, string BasicAuth)
        {
            string endpoint = apiUrl;
            var result1 = default(T);
            try
            {


                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", BasicAuth);
                    using (var Response = client.GetAsync(endpoint))
                    {
                        Response.Wait();
                        var result = Response.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var readTask = result.Content.ReadAsStringAsync();
                            readTask.Wait();
                            result1 = JsonConvert.DeserializeObject<T>(readTask.Result);

                        }


                    }
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
            return result1;
        }
        public T PostRequest(string apiUrl, string bodyparam, string BasicAuth)
        {
            string endpoint = apiUrl;
            var result1 = default(T);
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", BasicAuth);
                    StringContent content = new StringContent(bodyparam, Encoding.UTF8, "application/json");
                    using (var Response = client.PostAsync(endpoint, content))
                    {
                        Response.Wait();
                        var result = Response.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var readTask = result.Content.ReadAsStringAsync();
                            readTask.Wait();
                            result1 = JsonConvert.DeserializeObject<T>(readTask.Result);

                        }

                    }
                }
            }
            catch (Exception ex)
            {

                return default(T);
            }
            return result1;
        }

        public string PostRequestString(string apiUrl, string bodyparam, string BasicAuth)
        {
            string endpoint = apiUrl;
            string result1 = string.Empty;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", BasicAuth);
                    StringContent content = new StringContent(bodyparam, Encoding.UTF8, "application/json");
                    using (var Response = client.PostAsync(endpoint, content))
                    {
                        Response.Wait();
                        var result = Response.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var readTask = result.Content.ReadAsStringAsync();
                            readTask.Wait();
                            result1 = readTask.Result;

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
            return result1;
        }
        public string GetRequest(string apiUrl, string BasicAuth)
        {
            string endpoint = apiUrl;
            string result1 = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", BasicAuth);
                using (var Response = client.GetAsync(endpoint))
                {
                    Response.Wait();
                    var result = Response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();
                        result1 = readTask.Result;

                    }


                }
            }
            return result1;
        }
    }
}
