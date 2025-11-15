using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeBidder.Models
{
    public class ProjectDetailsModel
    {
        // Unique ID for each project
        [Key]
        public int Id { get; set; }

        // Project Details
        [Required(ErrorMessage = "Project Name is required.")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "Project Description is required.")]
        public string ProjectDescription { get; set; }

        // Intended Audience
        public string IntendedAudience { get; set; }

        // Competitors
        public string Competitors { get; set; }

        // Project Scope
        public string ProjectScope { get; set; }

        // Platform Preferences
        [Required(ErrorMessage = "Target Platform is required.")]
        public string TargetPlatform { get; set; }

        public string OtherPlatform { get; set; } // For "Other" platform option

        // Features
        [Required(ErrorMessage = "Core Features are required.")]
        public string CoreFeatures { get; set; }

        public string AdditionalFeatures { get; set; }

        // Design Preferences
        public string UiDesign { get; set; }
        public string ColorScheme { get; set; }

        // Timeline & Budget
        [Required(ErrorMessage = "Expected Timeline is required.")]
        public string ExpectedTimeline { get; set; }

        [Required(ErrorMessage = "Budget Range is required.")]
        public string BudgetRange { get; set; }

        // Technical Requirements
        public string TechStack { get; set; }
        public string DbRequirements { get; set; }

        // Maintenance & Support
        public bool OngoingMaintenance { get; set; }
        public bool PostLaunchSupport { get; set; }

        // Miscellaneous
        public string Miscellaneous { get; set; }

        // Navigation property for user enrollment (if applicable)
        public ICollection<UserProject> Enrollment { get; set; }
    }
}
