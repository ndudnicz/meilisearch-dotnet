namespace MeilisearchDotnet.Types
{
    public struct UpdateType
    {
        public string Name { get; set; }
        public int Number { get; set; }
    }
    public struct Update
    {
        public string Status { get; set; }
        public int UpdateId { get; set; }
        public UpdateType Type { get; set; }
        public double Duration { get; set; }
        public string EnqueuedAt { get; set; }
        public string ProcessedAt { get; set; }
    }
}
