using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using MeilisearchDotnet.Types;

namespace MeilisearchDotnet
{
    public class Meilisearch : MeiliHttpClientWrapper
    {
        public Meilisearch(string host, string apiKey) : base(host, apiKey) { }

        public Index getIndex(string indexUid)
        {
            // TODO
            return null;
        }

        ///
        /// KEYS
        ///

        /**
         * Get private and public key
         */
        public async Task<MeilisearchDotnet.Types.Keys> GetKeys()
        {
            string url = "/keys";

            return await Get<MeilisearchDotnet.Types.Keys>(url);
        }

        ///
        /// HEALTH
        ///

        /**
         * Check if the server is healhty
         */
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

        /**
         * Set the healthyness to `health` value
         */
        public async Task<string> ChangeHealthTo(bool health)
        {
            string url = "/health";
            StringContent payload = new StringContent("{\"health\":" + health.ToString().ToLower() + "}", Encoding.UTF8, "application/x-www-form-urlencoded");
            return await Put<string>(url, payload);
        }

        /**
         * Change the healthyness to healthy
         */
        public Task<string> SetHealthy()
        {
            return ChangeHealthTo(true);
        }

        /**
         * Change the healthyness to unhealthy
         */
        public Task<string> SetUnhealthy(bool health)
        {
            return ChangeHealthTo(false);
        }

        ///
        /// STATS
        ///

        /**
         * Get the stats of all the database
         */
        public async Task<MeilisearchDotnet.Types.Stats> Stats()
        {
            string url = "/stats";

            return await Get<Stats>(url);
        }

        /**
         * Get the version of MeiliSearch
         */
        public async Task<MeilisearchDotnet.Types.Version> Version()
        {
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
