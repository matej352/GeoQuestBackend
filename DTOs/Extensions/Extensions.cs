using GeoQuest.Enums;
using GeoQuest.Models;

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


    }
}
