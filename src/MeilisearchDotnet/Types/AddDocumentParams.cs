using System.Net;
using System.Text.Json.Serialization;

namespace MeilisearchDotnet.Types
{
    public class AddDocumentParams
    {
        [JsonPropertyName("primaryKey")]
        public string PrimaryKey { get; set; }

        public string ToQueryString()
        {
            return "primaryKey=" + WebUtility.UrlEncode(PrimaryKey);
        }
    }
}
