namespace MeilisearchDotnet.Types
{
    public class SearchRequest
    {
        public string Q { get; set; }
        public int? Offset { get; set; }
        public int? Limit { get; set; }
        public string AttributesToRetrieve { get; set; }
        public string AttributesToCrop { get; set; }
        public int? CropLength { get; set; }
        public string AttributesToHighlight { get; set; }
        public string Filters { get; set; }
        public bool? Matches { get; set; }
    }
}
