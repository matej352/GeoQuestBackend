namespace GeoQuest.DTOs
{
    public class TestPublishedDto
    {
        public int Id { get; set; }

        public int TeacherId { get; set; }

        public TimeSpan Duration { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Subject { get; set; }

        public int FinishedInstanceCount { get; set; }

        public int InstanceCount { get; set; }

        public bool Active { get; set; }

    }
}
