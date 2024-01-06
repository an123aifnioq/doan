using doan.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan.Models
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public int AccountID { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? FullName { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public int? RoleID { get; set; }

        public string? LastLogin { get; set; }

        public bool IsActive { get; set; }
        public string? Image { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    }
}
