using System.Linq;
using System.Collections.Generic;

namespace MeilisearchDotnet.Types
{
    public class GetDocumentsParams
    {
        public int? Offset { get; set; }
        public int? Limit { get; set; }
        public IEnumerable<string> AttributesToRetrieve { get; set; }

        public string ToQueryString()
        {
            List<string> s = new List<string>() {
                Offset.HasValue ? "offset=" + Offset.ToString() : null,
                Limit.HasValue ? "limit=" + Limit.ToString() : null,
                AttributesToRetrieve != null ? "attributesToRetrieve=" + string.Join(",", AttributesToRetrieve.Where(x => !string.IsNullOrEmpty(x))) : null
            };
            return string.Join("&", s.Where(x => !string.IsNullOrEmpty(x)));
        }
    }
}
