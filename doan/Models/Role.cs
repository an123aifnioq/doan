using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;


namespace doan.Models
{
    [Table("Role")]
    public partial class Role
    {
        [Key]
        public int RoleID { get; set; }

        public string? RoleName { get; set; }

        public string? Description { get; set; }
        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    }
}