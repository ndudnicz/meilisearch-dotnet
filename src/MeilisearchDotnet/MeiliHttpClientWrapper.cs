using System;
using System.Net.Http;
using System.Threading.Tasks;
using MeilisearchDotnet.Exceptions;

namespace MeilisearchDotnet
{
    public class MeiliHttpClientWrapper
    {
        protected HttpClient HttpClient { get; set; }
        protected MeiliHttpClientWrapper(string host, string apiKey)
        {
            HttpClient = new HttpClient();
            HttpClient.BaseAddress = new Uri(host);
            if (apiKey != null)
            {
                HttpClient.DefaultRequestHeaders.Add("X-Meili-API-Key", apiKey);
            }
        }

        protected MeiliHttpClientWrapper(HttpClient httpclient)
        {
            HttpClient = httpclient;
        }

        protected async Task<T> Get<T>(string url)
        {
            try
            {
                HttpResponseMessage res = await HttpClient.GetAsync(url);
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsAsync<T>();
                }
                else
                {
                    if (res.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new MeilisearchDotnet.Exceptions.NotFoundException();
                    }
                    else if (res.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new MeilisearchDotnet.Exceptions.BadRequestException();
                    }
                    else
                    {
                        string content = await res.Content.ReadAsStringAsync();
                        throw new MeilisearchApiException(content != null && content.Length > 0 ? content : res.StatusCode.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected async Task<T> Post<T>(string url, StringContent payload)
        {
            try
            {
                HttpResponseMessage res = await HttpClient.PostAsync(url, payload);
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsAsync<T>();
                }
                else
                {
                    if (res.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new MeilisearchDotnet.Exceptions.NotFoundException();
                    }
                    else if (res.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new MeilisearchDotnet.Exceptions.BadRequestException();
                    }
                    else
                    {
                        string content = await res.Content.ReadAsStringAsync();
                        throw new MeilisearchApiException(content != null && content.Length > 0 ? content : res.StatusCode.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected async Task<T> Put<T>(string url, StringContent payload)
        {
            try
            {
                HttpResponseMessage res = await HttpClient.PutAsync(url, payload);
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsAsync<T>();
                }
                else
                {
                    if (res.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new MeilisearchDotnet.Exceptions.NotFoundException();
                    }
                    else if (res.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new MeilisearchDotnet.Exceptions.BadRequestException();
                    }
                    else
                    {
                        string content = await res.Content.ReadAsStringAsync();
                        throw new MeilisearchApiException(content != null && content.Length > 0 ? content : res.StatusCode.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected async Task<T> Delete<T>(string url)
        {
            try
            {
                HttpResponseMessage res = await HttpClient.DeleteAsync(url);
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsAsync<T>();
                }
                else
                {
                    if (res.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new MeilisearchDotnet.Exceptions.NotFoundException();
                    }
                    else if (res.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new MeilisearchDotnet.Exceptions.BadRequestException();
                    }
                    else
                    {
                        string content = await res.Content.ReadAsStringAsync();
                        throw new MeilisearchApiException(content != null && content.Length > 0 ? content : res.StatusCode.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected async Task<T> Send<T>(HttpRequestMessage req)
        {
            try
            {
                HttpResponseMessage res = await HttpClient.SendAsync(req);
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsAsync<T>();
                }
                else
                {
                    if (res.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new MeilisearchDotnet.Exceptions.NotFoundException();
                    }
                    else if (res.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new MeilisearchDotnet.Exceptions.BadRequestException();
                    }
                    else
                    {
                        string content = await res.Content.ReadAsStringAsync();
                        throw new MeilisearchApiException(content != null && content.Length > 0 ? content : res.StatusCode.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
