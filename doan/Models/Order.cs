using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace doan.Models
{
    [Table("Order")]
    public partial class Order
    {
        [Key]
        public int OrderID { get; set; }

        public string? Code { get; set; }

        public string? CustomerName { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public int? TotalAmount { get; set; }

        public int? Quantity { get; set; }

        public int? OrderStatusID { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }
        public int? AccountID { get; set; }
        public int? PaymentStatusID { get; set; }

        public virtual Account? Account { get; set; }
        public virtual OrderStatus? OrderStatus { get; set; }
        public virtual PaymentStatus? PaymentStatus { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}