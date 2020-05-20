namespace MeilisearchDotnet {
    [System.Serializable]
    public class MeilisearchApiException : System.Exception
    {
        public MeilisearchApiException() { }
        public MeilisearchApiException(string message) : base(message) { }
        public MeilisearchApiException(string message, System.Exception inner) : base(message, inner) { }
        protected MeilisearchApiException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}