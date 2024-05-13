namespace GeoQuest.DTOs
{
    public class TestDto
    {
        public int Id { get; set; }

        public int TeacherId { get; set; }

        public TimeSpan Duration { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Subject { get; set; }

        public int SubjectId { get; set; }

        public int SubjectsStudentsCount { get; set; }

        public int TestsTasksCount { get; set; }

    }
}
