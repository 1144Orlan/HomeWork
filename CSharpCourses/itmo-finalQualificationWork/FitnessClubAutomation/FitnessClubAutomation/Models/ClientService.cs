using System.ComponentModel.DataAnnotations;

namespace FitnessClubAutomation.Models
{
    public class ClientService
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;

        public int? TrainingSessionId { get; set; }
        public TrainingSession? TrainingSession { get; set; }

        [Required]
        [Display(Name = "Registration Date")]
        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}