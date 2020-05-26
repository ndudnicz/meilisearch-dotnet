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
        public Meilisearch(string host, string apiKey) : base(host, apiKey) {
            Host = host;
            ApiKey = apiKey;
        }

        /**
         * Return an Index instance
         */
        public Index GetIndex(string indexUid) {
            return new Index(Host, ApiKey, indexUid);
        }

        /**
         * List all indexes in the database
         */
        public async Task<IEnumerable<MeilisearchDotnet.Types.IndexResponse>> ListIndexes() {
            string url = "/indexes";

            return await Get<IEnumerable<MeilisearchDotnet.Types.IndexResponse>>(url);
        }

        /**
         * Create a new index
         */
        public async Task<Index> CreateIndex(MeilisearchDotnet.Types.IndexRequest data) {
            string url = "/indexes";
            string dataString = JsonSerializer.Serialize(data);
            StringContent payload = new StringContent(dataString, Encoding.UTF8, "application/json");

            MeilisearchDotnet.Types.IndexResponse index = await Post<MeilisearchDotnet.Types.IndexResponse>(url, payload);

            return GetIndex(index.Uid);
        }

        ///
        /// KEYS
        ///

        /**
         * Get private and public key
         */
        public async Task<MeilisearchDotnet.Types.Keys> GetKeys() {
            string url = "/keys";

            return await Get<MeilisearchDotnet.Types.Keys>(url);
        }

        ///
        /// HEALTH
        ///

        /**
         * Check if the server is healhty
         */
        public async Task<bool> IsHealthy() {
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

        /**
         * Set the healthyness to `health` value
         */
        public async Task<string> ChangeHealthTo(bool health) {
            string url = "/health";
            string dataString = JsonSerializer.Serialize(new {health});
            StringContent payload = new StringContent(dataString, Encoding.UTF8, "application/x-www-form-urlencoded");
            return await Put<string>(url, payload);
        }

        /**
         * Change the healthyness to healthy
         */
        public Task<string> SetHealthy() {
            return ChangeHealthTo(true);
        }

        /**
         * Change the healthyness to unhealthy
         */
        public Task<string> SetUnhealthy(bool health) {
            return ChangeHealthTo(false);
        }

        ///
        /// STATS
        ///

        /**
         * Get the stats of all the database
         */
        public async Task<MeilisearchDotnet.Types.Stats> Stats() {
            string url = "/stats";

            return await Get<Stats>(url);
        }

        /**
         * Get the version of MeiliSearch
         */
        public async Task<MeilisearchDotnet.Types.Version> Version() {
            string url = "/version";

            return await Get<MeilisearchDotnet.Types.Version>(url);
        }

        /**
         * Get the server consuption, RAM / CPU / Network
         */
        public async Task<MeilisearchDotnet.Types.SysInfo> SysInfo() {
            string url = "/sys-info";

            return await Get<MeilisearchDotnet.Types.SysInfo>(url);
        }

        /**
         * Get the server consuption, RAM / CPU / Network. All information as human readable
         */
        public async Task<MeilisearchDotnet.Types.SysInfoPretty> SysInfoPretty() {
            string url = "/sys-info/pretty";

            return await Get<MeilisearchDotnet.Types.SysInfoPretty>(url);
        }
    }
}
