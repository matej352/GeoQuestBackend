using System.ComponentModel.DataAnnotations;

namespace GeoQuest.DTOs
{
    public class SubjectDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

    }
}
