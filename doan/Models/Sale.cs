using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan.Models
{
    [Table("Sale")]
    public class Sale
    {
        [Key]
        public int SaleID { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? ProductID { get; set; }
        public int Percentage { get; set; }
        public int? CategoryProductID { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public virtual Product? Product { get; set; }
        public virtual ProductCategory? ProductCategories { get; set; }

    }
}
