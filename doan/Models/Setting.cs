using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan.Models
{
    [Table("Setting")]
    public partial class Setting
    {
        [Key]
        public int SettingID { get; set; }

        public string? SettingKey { get; set; }

        public string? SettingValue { get; set; }

        public string? SettingDescription { get; set; }
    }
}
