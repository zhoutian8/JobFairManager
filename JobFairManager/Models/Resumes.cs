using System.ComponentModel.DataAnnotations;

namespace JobFairManager.Models
{
    public class Resumes
    {
        [Key]
        public int ResumeId { get; set; }
        public int JobId { get; set; }
        public int UserId { get; set; }
        public string? FilePath { get; set; }
        public DateTime SubmissionDate { get; set; }
    }
}
