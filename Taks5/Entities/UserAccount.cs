using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Taks5.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class UserAccount
    {
        public int  ID { get; set; }

        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime? LastLogin { get; set; } 
        public bool IsBlocked { get; set; } = false;
    }
}
