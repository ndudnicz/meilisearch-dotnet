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

        public Meilisearch(string host, string apiKey) : base(host, apiKey)
        {
            Host = host;
            ApiKey = apiKey;
        }

        /// <summary>
        /// Return an Index instance
        /// </summary>
        public Index GetIndex(string indexUid)
        {
            return new Index(Host, ApiKey, indexUid);
        }

        /// <summary>
        /// List all indexes in the database
        /// </summary>
        public async Task<IEnumerable<MeilisearchDotnet.Types.IndexResponse>> ListIndexes()
        {
            string url = "/indexes";

            return await Get<IEnumerable<MeilisearchDotnet.Types.IndexResponse>>(url);
        }

        /// <summary>
        /// Create a new index
        /// </summary>
        public async Task<Index> CreateIndex(MeilisearchDotnet.Types.IndexRequest data)
        {
            string url = "/indexes";
            string dataString = JsonSerializer.Serialize(data);
            StringContent payload = new StringContent(dataString, Encoding.UTF8, "application/json");

            MeilisearchDotnet.Types.IndexResponse index = await Post<MeilisearchDotnet.Types.IndexResponse>(url, payload);

            return GetIndex(index.Uid);
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
