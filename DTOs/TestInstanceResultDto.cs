namespace GeoQuest.DTOs
{
    public class TestInstanceResultDto
    {
        public int TestInstanceId { get; set; }
        public int TotalPoints { get; set; }
        public double PercentageResolved { get; set; }
        public List<TestTaskResultDto> TestTasks { get; set; }
    }

    public class TestTaskResultDto
    {
        public int TestTaskId { get; set; }
        public string Question { get; set; }
        public string? CorrectAnswer { get; set; }

        public OptionsDto? Options { get; set; } // Optional
        public string StudentAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
