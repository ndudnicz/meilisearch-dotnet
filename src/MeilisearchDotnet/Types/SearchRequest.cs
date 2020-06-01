using System.Linq;
using System.Collections.Generic;

namespace MeilisearchDotnet.Types
{
    public class SearchRequest
    {
        public string Q { get; set; }
        public int? Offset { get; set; }
        public int? Limit { get; set; }
        public IEnumerable<string> AttributesToRetrieve { get; set; }
        public IEnumerable<string> AttributesToCrop { get; set; }
        public int? CropLength { get; set; }
        public IEnumerable<string> AttributesToHighlight { get; set; }
        public IEnumerable<string> Filters { get; set; }
        public bool? Matches { get; set; }

        public string ToQueryString()
        {
            List<string> s = new List<string>() {
                Offset.HasValue ? "offset=" + Offset.ToString() : null,
                Limit.HasValue ? "limit=" + Limit.ToString() : null,
                AttributesToRetrieve != null ? "attributesToRetrieve=" + string.Join(",", AttributesToRetrieve.Where(x => !string.IsNullOrEmpty(x))) : null,
                AttributesToCrop != null ? "attributesToCrop=" + string.Join(",", AttributesToCrop.Where(x => !string.IsNullOrEmpty(x))) : null,
                CropLength.HasValue ? "cropLength=" + CropLength.ToString() : null,
                AttributesToHighlight != null ? "attributesToHighlight=" + string.Join(",", AttributesToHighlight.Where(x => !string.IsNullOrEmpty(x))) : null,
                Filters != null ? "filters=" + string.Join(",", Filters.Where(x => !string.IsNullOrEmpty(x))) : null,
                Matches.HasValue ? "matches=" + Limit.ToString() : null
            };
            return string.Join("&", s.Where(x => !string.IsNullOrEmpty(x)));
        }

    }
}
