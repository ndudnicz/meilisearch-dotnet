using System.Collections.Generic;

namespace MeilisearchDotnet.Types
{
    public class Hits<T>
    {

    }
    public class SearchResponse<T>
    {
        public IEnumerable<T> Hits { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int ProcessingTimeMs { get; set; }
        public string Query { get; set; }

    }
}
