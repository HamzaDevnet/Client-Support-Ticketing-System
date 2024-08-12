using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApplication1.DTO
{

    public class RegisterDto
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

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
        [JsonIgnore]
        public byte[]? UserImage { get; set; } = null;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }


        //public string UserType { get; internal set; }

        [Required]
        public string Address { get; set; }
    }

}
