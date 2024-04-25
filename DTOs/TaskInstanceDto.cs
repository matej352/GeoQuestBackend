using GeoQuest.Enums;

namespace GeoQuest.DTOs
{
    public class TaskInstanceDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }  // this is student's answer
        public string? NonMapPoint { get; set; }
        public TaskType Type { get; set; } // Assuming TaskType is an enum for different task types
        public TaskInstanceOptionsDto? Options { get; set; } // Optional
        public int TestInstanceId { get; set; }

    }

    public class TaskInstanceOptionsDto
    {
        public int Id { get; set; }
        public bool SingleSelect { get; set; }
        public List<TaskInstanceOptionAnswerDto> OptionAnswers { get; set; }
    }

    public class TaskInstanceOptionAnswerDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
    }

}
