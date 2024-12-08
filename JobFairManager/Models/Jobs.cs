using System.ComponentModel.DataAnnotations;

namespace JobFairManager.Models
{
    /// <summary>
    /// 岗位表
    /// </summary>
    public class Jobs
    {
        [Key]
        public int JobId { get; set; }
        /// <summary>
        /// 岗位标题
        /// </summary>
        public string? JobTitle { get; set; }
        /// <summary>
        /// 岗位描述
        /// </summary>
        public string? JobDescription { get; set; }
        /// <summary>
        /// 招聘要求
        /// </summary>
        public string? Requirements { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
