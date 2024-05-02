using System.ComponentModel.DataAnnotations;

namespace GeoQuest.DTOs
{
    public class SubjectDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public int StudentsCount { get; set; }

    }
}
