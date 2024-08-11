using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class User1
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Client, Manager, Support
    }
}
