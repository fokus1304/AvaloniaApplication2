using AvaloniaApplication2.Context;
using AvaloniaApplication2.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication2
{
    internal class Helper
    {
        public static readonly DefaultDbContext defaultDbContext = new DefaultDbContext();
    }
}