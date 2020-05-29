using System;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using MeilisearchDotnet.Exceptions;

namespace MeilisearchDotnet
{
    public class Index : MeiliHttpClientWrapper
    {

        public string Uid { get; set; }

        public Index(
            string host,
            string apiKey,
            string IndexUid
        ) : base(host, apiKey)
        {
            Uid = IndexUid;
        }

        /// <summary>
        /// Get the informations about an update status.
        /// </summary>
        public async Task<MeilisearchDotnet.Types.Update> GetUpdateStatus(int updateId)
        {
            string url = "/indexes/" + Uid + "/updates/" + updateId.ToString();

            return await Get<MeilisearchDotnet.Types.Update>(url);
        }

        public async Task<MeilisearchDotnet.Types.Update> WaitForPendingUpdate(
            int updateId,
            double timeoutMs = 5000.0,
            int intervalMs = 50
        )
        {
            DateTime endingTime = DateTime.Now.AddMilliseconds(timeoutMs);

            while (DateTime.Now < endingTime)
            {
                MeilisearchDotnet.Types.Update res = await GetUpdateStatus(updateId);

                if (res.Status == "enqueued")
                {
                    return res;
                }
                await Task.Delay(intervalMs);
            }
            throw new MeilisearchApiException(
                "timeout of " + timeoutMs.ToString() + " ms has been exceeded on process " + updateId.ToString() + " when waiting for pending update to resolve."
            );
        }

        /// <summary>
        /// Get the list of all updates
        /// </summary>
        public async Task<IEnumerable<MeilisearchDotnet.Types.Update>> GetAllUpdateStatus()
        {
            string url = "/indexes/" + Uid + "/updates";

            return await Get<IEnumerable<MeilisearchDotnet.Types.Update>>(url);
        }

        ///
        /// SEARCH
        ///

        ///
        /// INDEX
        ///
        /// <summary>
        /// Show index information
        /// </summary>
        public async Task<MeilisearchDotnet.Types.IndexResponse> Show()
        {
            string url = "/indexes/" + Uid;

            return await Get<MeilisearchDotnet.Types.IndexResponse>(url);
        }

        /// <summary>
        /// Update an index
        /// </summary>
        public async Task<MeilisearchDotnet.Types.IndexResponse> UpdateIndex(MeilisearchDotnet.Types.UpdateIndexRequest data)
        {
            string url = "/indexes/" + Uid;
            string dataString = JsonSerializer.Serialize(data);
            StringContent payload = new StringContent(dataString, Encoding.UTF8, "application/x-www-form-urlencoded");

            return await Put<MeilisearchDotnet.Types.IndexResponse>(url, payload);
        }

        /// <summary>
        /// Delete an index.
        /// </summary>
        public async Task<string> DeleteIndex()
        {
            string url = "/indexes/" + Uid;

            return await Delete<string>(url);
        }

        ///
        /// STATS
        ///

        /// <summary>
        /// get stats of an index
        /// </summary>
        public async Task<MeilisearchDotnet.Types.IndexStats> GetStats()
        {
            string url = "/indexes/" + Uid + "/stats";

            return await Get<MeilisearchDotnet.Types.IndexStats>(url);
        }

        ///
        /// DOCUMENTS
        ///

        /// <summary>
        /// Add or replace multiples documents to an index
        /// </summary>
        /* Example :
        Meilisearch ms = new Meilisearch("http://localhost:7700", "keykeykey");
        MeilisearchDotnet.Index index = await ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest {
            uid = "zz",
            primaryKey = "toto"
        });

        MeilisearchDotnet.Types.EnqueuedUpdate ret = await index.AddDocuments<Doc>(new List<Doc>() {
            new Doc { Key1 = 222 },
            new Doc { Key1 = 222 }
        });

        ret = await index.AddDocuments<Doc>(new List<Doc>() {
            new Doc { Key1 = 222 },
            new Doc { Key1 = 222 }
        }, new MeilisearchDotnet.Types.AddDocumentParams {
            primaryKey = "toto"
        });
         */
        public async Task<MeilisearchDotnet.Types.EnqueuedUpdate> AddDocuments<T>(
            IEnumerable<T> documents,
            MeilisearchDotnet.Types.AddDocumentParams options = null
        )
        {
            string url = null;

            if (options != null)
            {
                url = "/indexes/" + Uid + "/documents?" + options.ToQueryString();
            }
            else
            {
                url = "/indexes/" + Uid + "/documents";
            }
            string dataString = JsonSerializer.Serialize(documents);
            StringContent payload = new StringContent(dataString, Encoding.UTF8, "application/x-www-form-urlencoded");

            return await Post<MeilisearchDotnet.Types.EnqueuedUpdate>(url, payload);
        }

        /// <summary>
        /// Add or update multiples documents to an index
        /// </summary>
        /* Example :
        await index.UpdateDocuments(new List<Doc>() {
            new Doc { id = 222, value = 1 },
            new Doc { id = 333, value = 1 }
        });
        */
        public async Task<MeilisearchDotnet.Types.EnqueuedUpdate> UpdateDocuments<T>(
            IEnumerable<T> documents,
            MeilisearchDotnet.Types.AddDocumentParams options = null
        )
        {
            string url = null;

            if (options != null)
            {
                url = "/indexes/" + Uid + "/documents?" + options.ToQueryString();
            }
            else
            {
                url = "/indexes/" + Uid + "/documents";
            }
            string dataString = JsonSerializer.Serialize(documents);
            StringContent payload = new StringContent(dataString, Encoding.UTF8, "application/x-www-form-urlencoded");

            return await Put<MeilisearchDotnet.Types.EnqueuedUpdate>(url, payload);
        }


        /// <summary>
        /// Get documents of an index
        /// </summary>
        public async Task<T> GetDocuments<T>(
            MeilisearchDotnet.Types.GetDocumentsParams options = null
        )
        {
            string url = null;

            if (options != null)
            {
                url = "/indexes/" + Uid + "/documents?" + options.ToQueryString();
            }
            else
            {
                url = "/indexes/" + Uid + "/documents";
            }
            return await Get<T>(url);
        }

        /// <summary>
        /// Get one document
        /// </summary>
        public async Task<T> GetDocument<T>(string documentId)
        {
            string url = "/indexes/" + Uid + "/documents/" + documentId;

            return await Get<T>(url);
        }

        /// <summary>
        /// Delete one document
        /// </summary>
        public async Task<MeilisearchDotnet.Types.EnqueuedUpdate> DeleteDocument(string documentId)
        {
            string url = "/indexes/" + Uid + "/documents/" + documentId;

            return await Delete<MeilisearchDotnet.Types.EnqueuedUpdate>(url);
        }
    }
}
