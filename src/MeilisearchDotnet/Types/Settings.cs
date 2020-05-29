using System.Collections.Generic;

namespace MeilisearchDotnet.Types
{
    public class Settings
    {
        public string DistinctAttribute { get; set; }
        public IEnumerable<string> SearchableAttributes { get; set; }
        public IEnumerable<string> DisplayedAttributes { get; set; }
        public IEnumerable<string> RankingRules { get; set; }
        public IEnumerable<string> StopWords { get; set; }
        public Dictionary<string, IEnumerable<string>> Synonyms { get; set; }
        public bool? IndexNewFields  { get; set; }
    }
}
