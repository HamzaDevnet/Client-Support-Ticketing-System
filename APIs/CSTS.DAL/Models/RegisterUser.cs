using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class RegisterUser
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string MobileNumber { get; set; }
        public string? UserImage { get; set; } = null;
        public DateTime DateOfBirth { get; set; }
        //public string UserType { get; internal set; }
        
        //public string Address { get; set; }
    }
}
