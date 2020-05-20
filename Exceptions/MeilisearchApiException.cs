using System;

namespace MeilisearchDotnet.Exceptions {
    [System.Serializable]
    public class MeilisearchApiException : Exception
    {
        public MeilisearchApiException() { }
        public MeilisearchApiException(string message) : base(message) { }
        public MeilisearchApiException(string message, System.Exception inner) : base(message, inner) { }
        protected MeilisearchApiException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}