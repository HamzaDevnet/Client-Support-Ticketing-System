using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class LoginRequest
    {
        [Key]
        public int Id { get; set; }
        public string EmailOrUserName { get; set; }
        public string Password { get; set; }
    }
}
