using System;
using System.ComponentModel.DataAnnotations;

namespace CodeBidder.Models
{
    public class Quotation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int DeveloperId { get; set; }

        [Required]
        public string QuotationDetails { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Timeline { get; set; }

        public string? AdditionalNotes { get; set; }

        public DateTime SubmittedDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pending";
    }
}
