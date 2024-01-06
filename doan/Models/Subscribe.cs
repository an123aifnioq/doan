using System;
using System.Collections.Generic;

namespace doan.Models;

public partial class Subscribe
{
    public int SubscribeID { get; set; }

    public string? Email { get; set; }

    public DateTime? CreateDate { get; set; }
}
