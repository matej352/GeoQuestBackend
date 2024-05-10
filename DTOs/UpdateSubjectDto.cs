using System.ComponentModel.DataAnnotations;

namespace GeoQuest.DTOs
{
    public class UpdateSubjectDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}
