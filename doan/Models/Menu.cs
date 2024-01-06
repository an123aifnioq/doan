using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan.Models
{
    [Table("Menu")]
    public class Menu
    {
        [Key]
        public int MenuID { get; set; }

        public string? Title { get; set; }

        public string? Alias { get; set; }

        public string? Description { get; set; }

        public int? Levels { get; set; }

        public int? ParentID { get; set; }

        public int? Position { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public bool? IsActive { get; set; }
        public int? MenuOrder { get; set; }
    }
}
