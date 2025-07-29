using System.ComponentModel.DataAnnotations;

namespace Taks5.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please Enter Valid Email.")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
