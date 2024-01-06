using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace doan.Models
{
    [Table("Blog")]
    public partial class Blog
    {
        [Key]
        public int BlogID { get; set; }

        public string? Title { get; set; }

        public string? Alias { get; set; }

        public int? CategoryID { get; set; }

        public string? Description { get; set; }

        public string? Detail { get; set; }

        public string? Image { get; set; }

        public string? SeoTitle { get; set; }

        public string? SeoDescription { get; set; }

        public string? SeoKeywords { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public int? AccountID { get; set; }

        public bool IsActive { get; set; }
        public virtual Account? Account { get; set; }
        public virtual Category? Category { get; set; }

        public virtual ICollection<BlogComment> BlogComments { get; set; } = new List<BlogComment>();
    }
}