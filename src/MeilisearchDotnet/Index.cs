using System;
using System.Net;
using System.Linq;
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
            HttpClient httpClient,
            string IndexUid
        ) : base(httpClient)
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

                if (res.Status != "enqueued")
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

        /// <summary>
        /// Search for documents into an index
        /// <summary>
        public async Task<MeilisearchDotnet.Types.SearchResponse<T>> Search<T>(
            string query,
            MeilisearchDotnet.Types.SearchRequest options = null
        )
        {
            // TODO
            string url = null;

            if (options != null)
            {
                url = "/indexes/" + Uid + "/search?q=" + query + "&" + WebUtility.UrlEncode(options.ToQueryString());
            }
            else
            {
                url = "/indexes/" + Uid + "/search?q=" + query;
            }


            return await Get<MeilisearchDotnet.Types.SearchResponse<T>>(url);
        }

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
        public async Task<MeilisearchDotnet.Types.EnqueuedUpdate> AddDocuments<T>(
            IEnumerable<T> documents,
            MeilisearchDotnet.Types.AddDocumentParams options = null
        )
        {
            string url = null;

            if (options != null)
            {
                url = "/indexes/" + Uid + "/documents?" + WebUtility.UrlEncode(options.ToQueryString());
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
        public async Task<MeilisearchDotnet.Types.EnqueuedUpdate> UpdateDocuments<T>(
            IEnumerable<T> documents,
            MeilisearchDotnet.Types.AddDocumentParams options = null
        )
        {
            string url = null;

            if (options != null)
            {
                url = "/indexes/" + Uid + "/documents?" + WebUtility.UrlEncode(options.ToQueryString());
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
            url = "/indexes/" + Uid + "/documents?" + WebUtility.UrlEncode(options.ToQueryString());
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

        public async Task<T> GetDocument<T>(int documentId)
        {
            return await GetDocument<T>(documentId.ToString());
        }

        /// <summary>
        /// Delete one document
        /// </summary>
        public async Task<MeilisearchDotnet.Types.EnqueuedUpdate> DeleteDocument(string documentId)
        {
            string url = "/indexes/" + Uid + "/documents/" + documentId;

            return await Delete<MeilisearchDotnet.Types.EnqueuedUpdate>(url);
        }

        public async Task<MeilisearchDotnet.Types.EnqueuedUpdate> DeleteDocument(int documentId)
        {
            return await DeleteDocument(documentId.ToString());
        }

        /// <summary>
        /// Delete multiples documents of an index
        /// </summary>
        public async Task<MeilisearchDotnet.Types.EnqueuedUpdate> DeleteDocuments(IEnumerable<string> documentsIds)
        {
            string url = "/indexes/" + Uid + "/documents/delete-batch";
            string dataString = JsonSerializer.Serialize(documentsIds);
            StringContent payload = new StringContent(dataString, Encoding.UTF8, "application/x-www-form-urlencoded");

            return await Post<MeilisearchDotnet.Types.EnqueuedUpdate>(url, payload);
        }

        public async Task<MeilisearchDotnet.Types.EnqueuedUpdate> DeleteDocuments(IEnumerable<int> documentsIds)
        {
            return await DeleteDocuments(documentsIds.Select(x => x.ToString()));
        }

        /// <summary>
        /// Delete all documents of an index
        /// </summary>
        public async Task<MeilisearchDotnet.Types.EnqueuedUpdate> DeleteAllDocuments()
        {
            string url = "/indexes/" + Uid + "/documents";

            return await Delete<MeilisearchDotnet.Types.EnqueuedUpdate>(url);
        }

        ///
        /// SETTINGS
        ///

        /// <summary>
        /// Retrieve all settings
        /// </summary>
        public async Task<MeilisearchDotnet.Types.Settings> GetSettings()
        {
            string url = "/indexes/" + Uid + "/settings";

            return await Delete<MeilisearchDotnet.Types.Settings>(url);
        }

        /// <summary>
        /// Update all settings
        /// </summary>
        public async Task<MeilisearchDotnet.Types.EnqueuedUpdate> UpdateSettings(MeilisearchDotnet.Types.Settings settings)
        {
            string url = "/indexes/" + Uid + "/settings";
            string dataString = JsonSerializer.Serialize(settings);
            StringContent payload = new StringContent(dataString, Encoding.UTF8, "application/x-www-form-urlencoded");

            return await Post<MeilisearchDotnet.Types.EnqueuedUpdate>(url, payload);
        }

        /// <summary>
        /// Reset settings
        /// </summary>
        public async Task<MeilisearchDotnet.Types.EnqueuedUpdate> ResetSettings()
        {
            string url = "/indexes/" + Uid + "/settings";

            return await Delete<MeilisearchDotnet.Types.EnqueuedUpdate>(url);
        }

        ///
        /// SYNONYMS
        ///

        /// <summary>
        /// Get the list of all synonyms
        /// </summary>
        public async Task<Dictionary<string, IEnumerable<string>>> GetSynonyms()
        {
            string url = "/indexes/" + Uid + "/settings/synonyms";

            return await Get<Dictionary<string, IEnumerable<string>>>(url);
        }

        /// <summary>
        /// Update the list of synonyms. Overwrite the old list
        /// </summary>
        public async Task<MeilisearchDotnet.Types.EnqueuedUpdate> UpdateSynonyms(Dictionary<string, IEnumerable<string>> synonyms)
        {
            string url = "/indexes/" + Uid + "/settings/synonyms";
            string dataString = JsonSerializer.Serialize(synonyms);
            StringContent payload = new StringContent(dataString, Encoding.UTF8, "application/x-www-form-urlencoded");

            return await Post<MeilisearchDotnet.Types.EnqueuedUpdate>(url, payload);
        }

        /// <summary>
        /// Empty the synonyme list
        /// </summary>
        public async Task<MeilisearchDotnet.Types.EnqueuedUpdate> ResetSynonyms()
        {
            string url = "/indexes/" + Uid + "/settings/synonyms";

            return await Delete<MeilisearchDotnet.Types.EnqueuedUpdate>(url);
        }

        ///
        /// STOP WORDS
        ///

        /// <summary>
        /// Get the list of all stop-words
        /// </summary>
        public async Task<IEnumerable<string>> GetStopWords()
        {
            string url = "/indexes/" + Uid + "/settings/stop-words";

            return await Get<IEnumerable<string>>(url);
        }

        /// <summary>
        /// Update the list of stop-words. Overwrite the old list
        /// </summary>
        public async Task<MeilisearchDotnet.Types.EnqueuedUpdate> UpdateStopWords(IEnumerable<string> stopWords)
        {
            string url = "/indexes/" + Uid + "/settings/stop-words";
            string dataString = JsonSerializer.Serialize(stopWords);
            StringContent payload = new StringContent(dataString, Encoding.UTF8, "application/x-www-form-urlencoded");

            return await Post<MeilisearchDotnet.Types.EnqueuedUpdate>(url, payload);
        }

        /// <summary>
        /// Empty the stop-words list
        /// </summary>
        public async Task<MeilisearchDotnet.Types.EnqueuedUpdate> ResetStopWords()
        {
            string url = "/indexes/" + Uid + "/settings/stop-words";

            return await Delete<MeilisearchDotnet.Types.EnqueuedUpdate>(url);
        }

        ///
        /// RANKING RULES
        ///

        /// <summary>
        /// Get the list of all ranking-rules
        /// </summary>
        public async Task<IEnumerable<string>> GetRankingRules()
        {
            string url = "/indexes/" + Uid + "/settings/ranking-rules";

            return await Get<IEnumerable<string>>(url);
        }

        /// <summary>
        /// Update the list of ranking-rules. Overwrite the old list
        /// </summary>
        public async Task<MeilisearchDotnet.Types.EnqueuedUpdate> UpdateRankingRules(IEnumerable<string> rankingRules)
        {
            string url = "/indexes/" + Uid + "/settings/ranking-rules";
            string dataString = JsonSerializer.Serialize(rankingRules);
            StringContent payload = new StringContent(dataString, Encoding.UTF8, "application/x-www-form-urlencoded");

            return await Post<MeilisearchDotnet.Types.EnqueuedUpdate>(url, payload);
        }

        /// <summary>
        /// Empty the ranking-rules list
        /// </summary>
        public async Task<MeilisearchDotnet.Types.EnqueuedUpdate> ResetRankingRules()
        {
            string url = "/indexes/" + Uid + "/settings/ranking-rules";

            return await Delete<MeilisearchDotnet.Types.EnqueuedUpdate>(url);
        }

        ///
        /// DISTINCT ATTRIBUTE
        ///

        /// <summary>
        /// Get the distinct-attributes
        /// </summary>
        public async Task<string> GetDistinctAttribute()
        {
            string url = "/indexes/" + Uid + "/settings/distinct-attribute";

            return await Get<string>(url);
        }

        /// <summary>
        /// Update the distinct-attribute
        /// </summary>
        public async Task<MeilisearchDotnet.Types.EnqueuedUpdate> updateDistinctAttribute(string distinctAttribute)
        {
            string url = "/indexes/" + Uid + "/settings/distinct-attribute";
            string dataString = JsonSerializer.Serialize(distinctAttribute);
            StringContent payload = new StringContent(dataString, Encoding.UTF8, "application/x-www-form-urlencoded");

            return await Post<MeilisearchDotnet.Types.EnqueuedUpdate>(url, payload);
        }

        /// <summary>
        /// Reset the distinct-attribute
        /// </summary>
        public async Task<MeilisearchDotnet.Types.EnqueuedUpdate> ResetDistinctAttribute()
        {
            string url = "/indexes/" + Uid + "/settings/distinct-attribute";

            return await Delete<MeilisearchDotnet.Types.EnqueuedUpdate>(url);
        }
    }
}
