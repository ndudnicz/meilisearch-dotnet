using System.Collections.Generic;

namespace MeilisearchDotnet.Types
{
    public class IndexStats
    {
        public int NumberOfDocuments { get; set; }
        public bool IsIndexing { get; set; }
        public Dictionary<string, int> FieldFrequency { get; set; }
    }
}
