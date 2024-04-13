using GeoQuest.Enums;
using GeoQuest.Models;
using System.ComponentModel.DataAnnotations;

namespace GeoQuest.DTOs.Extensions
{
    public static class Extensions
    {


        public static AccountDto AsAccountDto(this Account a)
        {
            return new AccountDto
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Email = a.Email,
                Role = (UserRole)a.Role,
            };
        }

        public static SubjectDto AsSubjectDto(this Subject s)
        {
            return new SubjectDto
            {
                Id = s.Id,
                Name = s.Name,
            };
        }

        public static CreateTestDto CreateAsTestDto(this Test t)
        {
            return new CreateTestDto
            {

                Id = t.Id,
                TeacherId = t.TeacherId,
                Description = t.Description,
                Duration = t.Duration,
                SubjectId = t.SubjectId,
            };
        }

        public static TestDto AsTestDto(this Test t)
        {
            return new TestDto
            {

                Id = t.Id,
                TeacherId = t.TeacherId,
                Description = t.Description,
                Duration = t.Duration,
                Subject = t.Subject.Name,
            };
        }


        public static TaskDto AsTaskDto(this Models.Task task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Question = task.Question,
                Answer = task.Answer,
                Type = (TaskType)task.Type, // Ensure that the task.Type conversion to TaskType enum is safe
                Options = task.OptionsId != null ? new OptionsDto
                {
                    Id = task.Options.Id,
                    SingleSelect = task.Options.SingleSelect,
                    OptionAnswers = task.Options.OptionAnswer?.Select(oa => new OptionAnswerDto
                    {
                        Id = oa.Id,
                        Content = oa.Content,
                        Correct = oa.Correct
                    }).ToList() ?? new List<OptionAnswerDto>() // Use null-conditional operator and null-coalescing operator
                } : null,
            };
        }


    }
}
