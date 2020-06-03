using System.Text.Json.Serialization;

namespace MeilisearchDotnet.Types
{
    public struct UpdateIndexRequest
    {
        [JsonPropertyName("primaryKey")]
        public string PrimaryKey { get; set; }
    }
}
