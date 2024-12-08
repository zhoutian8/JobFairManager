using System.ComponentModel.DataAnnotations;

namespace JobFairManager.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string Salt { get; set; }
    }
}
