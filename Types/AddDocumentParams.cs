namespace MeilisearchDotnet.Types
{
    public class AddDocumentParams
    {
        public string primaryKey { get; set; }

        public string ToQueryString()
        {
            return "primaryKey=" + primaryKey;
        }
    }
}
