﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace doan.Models
{
    [Table("OrderStatus")]
    public partial class OrderStatus
    {
        [Key]
        public int OrderStatusID { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}