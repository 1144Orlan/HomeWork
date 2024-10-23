using System.ComponentModel;

namespace StudentPerformance.Models
{
    public class Score
    {
        public int Id { get; set; }
        [DisplayName("Предмет")]
        public string Subject { get; set; }
        [DisplayName("Баллы")]
        public int Points { get; set; }        
        public int StudentId { get; set; }        
        public Student Student { get; set; }
    }
}
