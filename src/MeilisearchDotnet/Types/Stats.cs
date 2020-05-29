using System.Collections.Generic;

namespace MeilisearchDotnet.Types
{

    public class Stats
    {
        public int DatabaseSize { get; set; }
        public string LastUpdate { get; set; }
        public Dictionary<string, IndexStats> Indexes { get; set; }
    }
}
