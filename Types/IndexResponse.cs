using System;

namespace MeilisearchDotnet.Types {
    public class IndexResponse {
        public string Uid { get; set; }
        public string Name { get; set; }
        public string PrimaryKey { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
