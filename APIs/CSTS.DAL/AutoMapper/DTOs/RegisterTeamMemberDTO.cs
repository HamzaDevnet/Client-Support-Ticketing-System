using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO
{
    public class RegisterTeamMemberDTO
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "UserName")]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string MobileNumber { get; set; }

        [Display(Name = "User Image")]
        public byte[]? UserImage { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
