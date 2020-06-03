using System.Text.Json.Serialization;

namespace MeilisearchDotnet.Types
{
    public struct IndexRequest
    {
        [JsonPropertyName("uid")]
        public string Uid { get; set; }
        [JsonPropertyName("primaryKey")]
        public string PrimaryKey { get; set; }
    }
}
