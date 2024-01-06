using doan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan.Models
{
    [Table("Product")]
    public partial class Product
    {
        [Key]
        public int ProductID { get; set; }

        public string? Title { get; set; }

        public string? Alias { get; set; }

        public int? CategoryProductID { get; set; }

        public string? Description { get; set; }

        public string? Detail { get; set; }

        public string? Image { get; set; }

        public int? Price { get; set; }

        public int? PriceSale { get; set; }

        public int? Quantity { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public bool IsNew { get; set; }

        public bool IsBestSeller { get; set; }

        public int? UnitInStock { get; set; }

        public bool IsActive { get; set; }
        public int Rating { get; set; }
        public virtual ProductCategory? CategoryProduct { get; set; }

        public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    }
}