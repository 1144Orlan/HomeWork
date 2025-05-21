using System.ComponentModel.DataAnnotations;

namespace FitnessClubAutomation.Models
{
    public class TrainingSession
    {
        public int Id { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; }

        [Required]
        [Display(Name = "Date and Time")]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; }

        [Display(Name = "Maximum Participants")]
        public int? MaxParticipants { get; set; }

        [Display(Name = "Current Participants")]
        public int CurrentParticipants { get; set; }
    }
}