using System;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeilisearchDotnet.Types;

namespace MeilisearchDotnet
{
    public class Meilisearch : MeiliHttpClientWrapper
    {
        public string Host;
        public string ApiKey;

        // Avoid useless api calls by storing indexes we already know
        public Dictionary<string, MeilisearchDotnet.Index> Indexes { get; set; }

        public Meilisearch(string host, string apiKey) : base(host, apiKey)
        {
            Host = host;
            ApiKey = apiKey;
            Indexes = new Dictionary<string, MeilisearchDotnet.Index>();
        }

        ~Meilisearch()
        {
            Indexes = null;
            HttpClient.Dispose();
        }

        /// <summary>
        /// Return an Index instance
        /// </summary>
        public async Task<MeilisearchDotnet.Index> GetIndex(string indexUid)
        {
            if (Indexes.TryGetValue(indexUid, out MeilisearchDotnet.Index index))
            {
                return index;
            }
            else
            {
                string url = "/indexes/" + indexUid;
                MeilisearchDotnet.Types.IndexResponse indexResponses = await Get<MeilisearchDotnet.Types.IndexResponse>(url);

                index = new Index(HttpClient, indexUid);
                Indexes.Add(indexUid, index);
                return index;
            }
        }

        /// <summary>
        /// List all indexes in the database and refresh Indexes Dictionary
        /// </summary>
        public async Task<IEnumerable<MeilisearchDotnet.Types.IndexResponse>> ListIndexes()
        {
            string url = "/indexes";
            IEnumerable<MeilisearchDotnet.Types.IndexResponse> indexResponses = await Get<IEnumerable<MeilisearchDotnet.Types.IndexResponse>>(url);
            Dictionary<string, MeilisearchDotnet.Index> newIndexes = new Dictionary<string, MeilisearchDotnet.Index>();

            foreach (MeilisearchDotnet.Types.IndexResponse indexResponse in indexResponses)
            {
                if (Indexes.TryGetValue(indexResponse.Uid, out MeilisearchDotnet.Index index))
                {
                    newIndexes.Add(indexResponse.Uid, index);
                }
                else
                {
                    index = new Index(HttpClient, indexResponse.Uid);
                    newIndexes.Add(indexResponse.Uid, index);
                }
                Indexes = newIndexes;
            }
            return indexResponses;
        }

        /// <summary>
        /// Create a new index
        /// </summary>
        public async Task<Index> CreateIndex(MeilisearchDotnet.Types.IndexRequest data)
        {
            string url = "/indexes";
            string dataString = JsonSerializer.Serialize(data);
            StringContent payload = new StringContent(dataString, Encoding.UTF8, "application/json");
            MeilisearchDotnet.Types.IndexResponse indexResponse = await Post<MeilisearchDotnet.Types.IndexResponse>(url, payload);
            MeilisearchDotnet.Index index = new Index(HttpClient, indexResponse.Uid);

            Indexes.Add(index.Uid, index);
            return index;
        }

        /// <summary>
        /// Update an index
        /// </summary>
        public async Task<MeilisearchDotnet.Types.IndexResponse> UpdateIndex(string indexUid, MeilisearchDotnet.Types.UpdateIndexRequest data)
        {
            string url = "/indexes/" + indexUid;
            string dataString = JsonSerializer.Serialize(data);
            StringContent payload = new StringContent(dataString, Encoding.UTF8, "application/x-www-form-urlencoded");

            return await Put<MeilisearchDotnet.Types.IndexResponse>(url, payload);
        }

        /// <summary>
        /// Delete an index.
        /// </summary>
        public async Task<string> DeleteIndex(string indexUid)
        {
            string url = "/indexes/" + indexUid;

            Indexes.Remove(indexUid);
            return await Delete<string>(url);
        }

        ///
        /// KEYS
        ///

        /// <summary>
        /// Get private and public key
        /// </summary>
        public async Task<MeilisearchDotnet.Types.Keys> GetKeys()
        {
            string url = "/keys";

            return await Get<MeilisearchDotnet.Types.Keys>(url);
        }

        ///
        /// HEALTH
        ///

        /// <summary>
        /// Check if the server is healhty
        /// </summary>
        public async Task<bool> IsHealthy()
        {
            string url = "/health";

            try
            {
                await Get<bool>(url);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Set the healthyness to `health` value
        /// </summary>
        public async Task<string> ChangeHealthTo(bool health)
        {
            string url = "/health";
            string dataString = JsonSerializer.Serialize(new { health });
            StringContent payload = new StringContent(dataString, Encoding.UTF8, "application/x-www-form-urlencoded");
            return await Put<string>(url, payload);
        }

        /// <summary>
        /// Change the healthyness to healthy
        /// </summary>
        public Task<string> SetHealthy()
        {
            return ChangeHealthTo(true);
        }

        /// <summary>
        /// Change the healthyness to unhealthy
        /// </summary>
        public Task<string> SetUnhealthy(bool health)
        {
            return ChangeHealthTo(false);
        }

        ///
        /// STATS
        ///

        /// <summary>
        /// Get the stats of all the database
        /// </summary>
        public async Task<MeilisearchDotnet.Types.Stats> Stats()
        {
            string url = "/stats";

            return await Get<Stats>(url);
        }

        /// <summary>
        /// Get the version of MeiliSearch
        /// </summary>
        public async Task<MeilisearchDotnet.Types.Version> Version()
        {
            string url = "/version";

            return await Get<MeilisearchDotnet.Types.Version>(url);
        }

        /// <summary>
        /// Get the server consuption, RAM / CPU / Network
        /// </summary>
        public async Task<MeilisearchDotnet.Types.SysInfo> SysInfo()
        {
            string url = "/sys-info";

            return await Get<MeilisearchDotnet.Types.SysInfo>(url);
        }

        /// <summary>
        /// Get the server consuption, RAM / CPU / Network. All information as human readable
        /// </summary>
        public async Task<MeilisearchDotnet.Types.SysInfoPretty> SysInfoPretty()
        {
            string url = "/sys-info/pretty";

            return await Get<MeilisearchDotnet.Types.SysInfoPretty>(url);
        }
    }
}
