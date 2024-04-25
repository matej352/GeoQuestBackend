using GeoQuest.Enums;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace GeoQuest.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string? Answer { get; set; }
        public string? NonMapPoint { get; set; }
        public TaskType Type { get; set; } // Assuming TaskType is an enum for different task types
        public OptionsDto? Options { get; set; } // Optional
        public int TestId { get; set; }

        // If you're using Entity Framework or similar ORM, you might need navigation properties here
    }

    public class OptionsDto
    {
        public int Id { get; set; }
        public bool SingleSelect { get; set; }
        public List<OptionAnswerDto> OptionAnswers { get; set; }
    }

    public class OptionAnswerDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool Correct { get; set; }
    }
}
