namespace CodeBidder.Models
{
    public class UserProject
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectDetailId { get; set; }
        public User User { get; set; } 
        public ProjectDetailsModel ProjectDetail { get; set; }
        
    }
}
