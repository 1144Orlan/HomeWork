using System.ComponentModel.DataAnnotations;

namespace FitnessClubAutomation.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Client Code")]
        public string ClientCode { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Status")]
        public ClientStatus Status { get; set; }
                
        public List<ClientService> ClientServices { get; set; }
    }

    public enum ClientStatus
    {
        Permanent,
        OneTime
    }
}