namespace MeilisearchDotnet.Types {
    public class UpdateType {
        public string Name { get; set; }
        public int Number { get; set; }
    }
    public class Update {
        public string Status { get; set; }
        public int UpdateId { get; set; }

        public UpdateType Type { get; set; }

        public int Duration { get; set; }

        public string EnqueuedAt { get; set; }

        public string ProcessedAt { get; set; }
    }
}
