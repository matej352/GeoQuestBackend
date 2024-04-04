using System.ComponentModel.DataAnnotations;

namespace GeoQuest.DTOs
{
    public class TestDto
    {

        public int Id { get; set; }

        public int TeacherId { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int SubjectId { get; set; }
    }
}
