using System;
using System.Text;
using System.Net.Http;
using MeilisearchDotnet.Types;
using System.Threading.Tasks;

namespace MeilisearchDotnet {
    public class Meilisearch: MeiliHttpClientWrapper {
        public Meilisearch(string host, string apiKey): base(host, apiKey) { }

        public Index getIndex(string indexUid) {
            return null;
        }

        public async Task<Keys> GetKeys() {
            string url = "/keys";

            return await Get<Keys>(url);
        }

        public async Task<bool> IsHealthy() {
            string url = "/health";

            try {
                await Get<bool>(url);
                return true;
            } catch(Exception e) {
                return false;
            }
        }

        public async Task<object> ChangeHealthTo(bool health) {
            string url = "/health";
            StringContent payload = new StringContent("{\"health\":" + health.ToString().ToLower() + "}", Encoding.UTF8, "application/x-www-form-urlencoded");
            return await Put<string>(url, payload);
        }
    }
}
