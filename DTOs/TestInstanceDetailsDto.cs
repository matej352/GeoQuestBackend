namespace GeoQuest.DTOs
{
    public class TestInstanceDetailsDto
    {
        public int Id { get; set; }

        public TimeSpan Duration { get; set; }

        public bool Started { get; set; }

        public bool Finished { get; set; }

        public TimeSpan? ElapsedTime { get; set; }

    }
}
