using System.ComponentModel.DataAnnotations;

namespace GeoQuest.DTOs
{
    public class SubjectDetailsDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<StudentDto> Students { get; set; }

        public List<TestPublishedDto> PublishedTests { get; set; }

    }
}
