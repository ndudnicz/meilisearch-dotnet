using MeilisearchDotnet;

namespace MeilisearchDotnet
{
    public class Index: MeiliHttpClientWrapper {

        public string Uid { get; set; }

        public Index(string host, string apiKey, string IndexUid): base(host, apiKey) {
            Uid = IndexUid;
        }
    }
}
