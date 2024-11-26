﻿using System;
using System.Collections.Generic;

namespace AvaloniaApplication2.Models;

public partial class Doptov
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Price { get; set; }

    public string? Photo { get; set; }

    public int? Isactive { get; set; }

    public int? Manufactured { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<ListDoptov> ListDoptovs { get; set; } = new List<ListDoptov>();
}
