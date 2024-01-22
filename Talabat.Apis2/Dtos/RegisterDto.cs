using System.ComponentModel.DataAnnotations;

namespace Talabat.Apis2.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string  DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string  Email { get; set; }
        [Required]
        [Phone]
        public string  PhoneNumber { get; set; }
        public string Password { get; set; }



    }
}
