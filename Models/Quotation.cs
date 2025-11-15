using System;

namespace CodeBidder.Models
{
    public class Quotation
    {
        public int Id { get; set; }
        public int ProjectId { get; set; } // Foreign key to the project
        public int DeveloperId { get; set; } // Foreign key to the developer
        public decimal EstimatedCost { get; set; }
        public string Timeline { get; set; } // Estimated timeline for completion
        public string AdditionalDetails { get; set; } // Developer's additional remarks
        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        // Navigation properties
        public ProjectDetailsModel Project { get; set; }
        public User Developer { get; set; }
    }
}
