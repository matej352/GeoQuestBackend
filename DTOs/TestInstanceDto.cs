namespace GeoQuest.DTOs
{
    public class TestInstanceDto
    {

        public int Id { get; set; }

        public string TeacherName { get; set; }

        public TimeSpan Duration { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Subject { get; set; }

        public string Score { get; set; }


    }


    public class FinishTestInstanceDto
    {
        public int Id { get; set; }

        public TimeSpan ElapsedTime { get; set; }
    }

    public class UpdateElapsedTimeDto
    {
        public int Id { get; set; }

        public TimeSpan ElapsedTime { get; set; }
    }
}
