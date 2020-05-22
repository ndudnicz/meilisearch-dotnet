using System;
using System.Net.Http;
using System.Threading.Tasks;
using MeilisearchDotnet.Exceptions;

namespace MeilisearchDotnet
{
    public class MeiliHttpClientWrapper {
        protected HttpClient httpClient { get; set; }
        protected MeiliHttpClientWrapper(string host, string apiKey) {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(host);
            if (apiKey != null)
            {
                httpClient.DefaultRequestHeaders.Add("X-Meili-API-Key", apiKey);
            }
        }

        public async Task<T> Get<T>(string url) {
            try
            {
                HttpResponseMessage res = await httpClient.GetAsync(url);
                if (res.IsSuccessStatusCode) {
                    return await res.Content.ReadAsAsync<T>();
                } else {
                    throw new MeilisearchApiException(await res.Content.ReadAsStringAsync());
                }
            }
            catch (Exception e)
            {
                throw new MeilisearchApiException(e.Message, e);
            }
        }

        public async Task<T> Post<T>(string url, StringContent payload) {
            try
            {
                HttpResponseMessage res = await httpClient.PostAsync(url, payload);
                if (res.IsSuccessStatusCode) {
                    return await res.Content.ReadAsAsync<T>();
                } else {
                    throw new MeilisearchApiException(await res.Content.ReadAsStringAsync());
                }
            }
            catch (Exception e)
            {
                throw new MeilisearchApiException(e.Message, e);
            }
        }

        // public async Task<T> PostJson<T, O>(string url, O payload) {
        //     try
        //     {
        //         HttpResponseMessage res = await httpClient.PostAsync(url, payload.ToString());
        //         return await res.Content.ReadAsAsync<T>();
        //     }
        //     catch (Exception e)
        //     {
        //         throw new MeilisearchApiException(e.Message, e);
        //     }
        // }

        public async Task<T> Put<T>(string url, StringContent payload) {
            try
            {
                HttpResponseMessage res = await httpClient.PutAsync(url, payload);
                 if (res.IsSuccessStatusCode) {
                    return await res.Content.ReadAsAsync<T>();
                } else {
                    throw new MeilisearchApiException(await res.Content.ReadAsStringAsync());
                }
            }
            catch (Exception e)
            {
                throw new MeilisearchApiException(e.Message, e);
            }
        }

        public async void Delete() {
        }

        public async Task<T> Send<T>(HttpRequestMessage req) {
            try
            {
                HttpResponseMessage res = await httpClient.SendAsync(req);
                if (res.IsSuccessStatusCode) {
                    return await res.Content.ReadAsAsync<T>();
                } else {
                    throw new MeilisearchApiException(await res.Content.ReadAsStringAsync());
                }
            }
            catch (Exception e)
            {
                throw new MeilisearchApiException(e.Message, e);
            }
        }
    }
}
