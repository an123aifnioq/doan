using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan.Models
{
    [Table("Category")]
    public partial class Category
    {
        [Key]
        public int CategoryID { get; set; }

        public string? Title { get; set; }

        public string? Alias { get; set; }

        public string? Description { get; set; }

        public int? Position { get; set; }

        public string? SeoTitle { get; set; }

        public string? SeoDescription { get; set; }

        public string? SeoKeywords { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }
        public string? Image { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

        public virtual ICollection<News> News { get; set; } = new List<News>();

    }
}