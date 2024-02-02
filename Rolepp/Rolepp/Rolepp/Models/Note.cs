using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rolepp.Models
{
    public class Note
    {
        [Key]
        public int NoteId { get; set; }

        [Required]
        public string NoteCode { get; set; }

        [Required]
        public string CreateName { get; set; }

        [Required]
        public string Customer { get; set; }

        [Required]
        public string AddressCustomer { get; set; }

        [Required]
        public string Reason { get; set; }

        [Required]
        [Range(1, 4, ErrorMessage = "Status must be between 1 and 4")]
        public int Status { get; set; } = 1; // Default value is 1

        public void UpdateStatus(int newStatus)
        {
            if (newStatus >= 1 && newStatus <= 4)
            {
                Status = newStatus;
            }
        }
    }
}
