using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace doan.Models
{
    [Table("OrderDetail")]
    public partial class OrderDetail
    {
        public int OrderDetailID { get; set; }

        public int? OrderID { get; set; }

        public int? ProductID { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}