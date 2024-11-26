using AvaloniaApplication2.Models;
using AvaloniaApplication2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication2
{
    internal class SavingDate
    {
        public static List<Product> products = Helper.defaultDbContext.Products.ToList();
        public static List<Doptov> doptovs = Helper.defaultDbContext.Doptovs.ToList();
        public static List<ListDoptov> doptovlist = Helper.defaultDbContext.ListDoptovs.ToList();
        public static List<Manufactured> manufactrur = Helper.defaultDbContext.Manufactureds.ToList();
        public static List<string> manufactrurs = new List<string>() { "Все товары" };
        public static Product prods = null;
        public static Doptov doptov = null;

    }
}
