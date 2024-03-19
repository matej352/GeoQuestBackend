using GeoQuest.Enums;

namespace GeoQuest.DTOs
{
    public class AccountDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public UserRole Role { get; set; }
    }
}
