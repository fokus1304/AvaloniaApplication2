using System;
using System.Collections.Generic;

namespace AvaloniaApplication2.Models;

public partial class Manufactured
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
