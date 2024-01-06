using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan.Models
{
    [Table("BlogComment")]
    public partial class BlogComment
    {
        [Key]
        public int CommentID { get; set; }

        public string? Name { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? Detail { get; set; }

        public int? BlogID { get; set; }

        public bool IsActive { get; set; }
        public int? AccountID { get; set; }
        public virtual Account? Account { get; set; }
        public virtual Blog? Blog { get; set; }
    }
}