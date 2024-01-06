using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan.Models
{
    [Table("ProductReview")]


    public partial class ProductReview
    {
        [Key]
        public int ProductReviewID { get; set; }

        public string? Name { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? Detail { get; set; }

        public int? ProductID { get; set; }

        public bool IsActive { get; set; }
        public int? Star { get; set; }
        public int? AccountID { get; set; }
        public virtual Account? Account { get; set; }
        public virtual Product? Product { get; set; }
    }
}