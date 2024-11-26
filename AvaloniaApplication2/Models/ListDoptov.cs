using System;
using System.Collections.Generic;

namespace AvaloniaApplication2.Models;

public partial class ListDoptov
{
    public int Id { get; set; }

    public int? IdTov { get; set; }

    public int? Iddoptow { get; set; }

    public virtual Product? IdTovNavigation { get; set; }

    public virtual Doptov? IddoptowNavigation { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
