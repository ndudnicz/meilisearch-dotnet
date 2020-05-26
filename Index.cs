using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using MeilisearchDotnet.Exceptions;

namespace MeilisearchDotnet
{
    public class Index: MeiliHttpClientWrapper {

        public string Uid { get; set; }

        public Index(string host, string apiKey, string IndexUid): base(host, apiKey) {
            Uid = IndexUid;
        }

        /**
         * Get the informations about an update status
         */
        public async Task<MeilisearchDotnet.Types.Update> GetUpdateStatus(int updateId) {
            string url = "/indexes/"+ Uid + "/updates/" + updateId.ToString();

            return await Get<MeilisearchDotnet.Types.Update>(url);
        }

        public async Task<MeilisearchDotnet.Types.Update> WaitForPendingUpdate(int updateId, double timeoutMs = 5000.0, int intervalMs = 50) {
            DateTime endingTime = DateTime.Now.AddMilliseconds(timeoutMs);

            while (DateTime.Now < endingTime) {
                MeilisearchDotnet.Types.Update res = await GetUpdateStatus(updateId);

                if (res.Status == "enqueued") {
                    return res;
                }
                await Task.Delay(intervalMs);
            }
            throw new MeilisearchApiException(
                "timeout of "+ timeoutMs.ToString() + " ms has been exceeded on process " + updateId.ToString() + " when waiting for pending update to resolve."
            );
        }

        /**
         * Get the list of all updates
         */
        public async Task<IEnumerable<MeilisearchDotnet.Types.Update>> GetAllUpdateStatus() {
            string url = "/indexes/"+ Uid + "/updates";

            return await Get<IEnumerable<MeilisearchDotnet.Types.Update>>(url);
        }
    }
}
