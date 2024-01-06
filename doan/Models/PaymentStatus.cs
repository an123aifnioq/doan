using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace doan.Models
{
    [Table("PaymentStatus")]
    public partial class PaymentStatus
    {
        [Key]
        public int PaymentStatusID { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}