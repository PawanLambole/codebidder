using System.ComponentModel.DataAnnotations;

namespace CodeBidder.Models
{
    public class ProjectDetailsModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [Required]
        public string ProjectDescription { get; set; }

        [Required]
        public string TargetPlatform { get; set; }

        public string? OtherPlatform { get; set; }

        public string? CoreFeatures { get; set; }

        public string? AdditionalFeatures { get; set; }

        public string? UiDesign { get; set; }

        public string? ColorScheme { get; set; }

        [Required]
        public string ExpectedTimeline { get; set; }

        [Required]
        public string BudgetRange { get; set; }

        public string? TechStack { get; set; }

        public string? DbRequirements { get; set; }

        public bool OngoingMaintenance { get; set; }

        public bool PostLaunchSupport { get; set; }

        public string? Miscellaneous { get; set; }
    }
}
