using System.ComponentModel.DataAnnotations;

namespace Betto.Model.DTO
{
    public class SignUpDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Username { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]
        public string Password { get; set; }
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string MailAddress { get; set; }
    }
}
