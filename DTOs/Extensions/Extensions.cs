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
                Description = s.Description,
                StudentsCount = s.Student.Count,
            };
        }

        public static CreateTestDto CreateAsTestDto(this Test t)
        {
            return new CreateTestDto
            {
                Id = t.Id,
                Name = t.Name,
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
                Name = t.Name,
                Description = t.Description,
                Duration = t.Duration,
                Subject = t.Subject.Name,
            };
        }

        public static TestInstanceDto AsTestInstanceDto(this TestInstance t)
        {
            return new TestInstanceDto
            {

                Id = t.Id,
                TeacherName = t.TestInstanceBase.Test.Teacher.FirstName + " " + t.TestInstanceBase.Test.Teacher.LastName,
                Name = t.TestInstanceBase.Test.Name,
                Description = t.TestInstanceBase.Test.Description,
                Duration = t.TestInstanceBase.Test.Duration,
                Subject = t.TestInstanceBase.Test.Subject.Name,
            };
        }

        public static TestInstanceDetailsDto AsTestInstanceDetailsDto(this TestInstance t)
        {
            return new TestInstanceDetailsDto
            {
                Id = t.Id,
                Duration = t.TestInstanceBase.Test.Duration,
                Started = t.Started,
                Finished = t.Finished,
                ElapsedTime = t.ElapsedTime
            };
        }




        public static TaskDto AsTaskDto(this TestTask task)
        {
            return new TaskDto
            {
                Id = task.Id,
                TestId = (int)task.TestId,
                MapCenter = task.MapCenter,
                MapType = (MapType?)task.MapType,   //vjv ne treba ? i u bazi mogu ovi podaci za Map biti not null, trenutno su nullable
                MapZoomLevel = task.MapZoomLevel,
                Question = task.Question,
                Answer = task.Answer,
                NonMapPoint = task.NonMapPoint,
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


        public static StudentDto AsStudentDto(this Account a)
        {
            return new StudentDto
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Email = a.Email
            };
        }


    }
}
