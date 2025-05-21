using System.ComponentModel.DataAnnotations;

namespace FitnessClubAutomation.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Cost")]
        [DataType(DataType.Currency)]
        public decimal Cost { get; set; }

        [Required]
        [Display(Name = "Duration (minutes)")]
        public int DurationMinutes { get; set; }

        [Required]
        [Display(Name = "Type")]
        public TrainingType Type { get; set; }

        // foreign key for Staff (Coach)
        public int StaffId { get; set; }
               
        public Staff Coach { get; set; }
                
        public List<ClientService> ClientServices { get; set; }

        // for group training schedule
        public List<TrainingSession> TrainingSessions { get; set; }
    }

    public enum TrainingType
    {
        Group,
        Individual
    }
}