using System.ComponentModel;
using static System.Formats.Asn1.AsnWriter;

namespace StudentPerformance.Models
{
    public class Student
    {
        public int Id { get; set; }
        [DisplayName("Имя студента")]
        public string Name { get; set; }
        [DisplayName("Табельный номер")]
        public string StudentId { get; set; }
        public List<Score> Scores { get; set; }
    }
}
