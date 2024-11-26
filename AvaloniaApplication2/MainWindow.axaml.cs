using Avalonia.Controls;
using Avalonia.Media.Imaging;
using AvaloniaApplication2.Models;
using AvaloniaApplication2;
using AvaloniaApplication2.Models;
using System.Collections.Generic;

using System.Linq;
using System.Security.Cryptography;

namespace AvaloniaApplication2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Listins(SavingDate.products);
            Filtr1.SelectedIndex = 0;
            Listinss();
            Filtr2.ItemsSource = SavingDate.manufactrurs;
            Filtr2.SelectedIndex = 0;

        }

        private void Listins(List<Product> list)
        {

            lbox_books.ItemsSource = list.Select(x => new
            {
                x.Name,
                x.Price,
                photos = new Bitmap($"Assets/{x.Photo}"),
                x.Manufactured,
                Color = x.Isactive == 1 ? "White" : "Gray"

            });
            ColvoKont.Text = $"Выведено записей {SavingDate.products.Count} из {SavingDate.products.Count}";
        }
        private void Listinss()
        {
            for (int i = 0; i < SavingDate.manufactrur.Count - 1; i++)
            {
                SavingDate.manufactrurs.Add(SavingDate.manufactrur[i].Name);
            }


        }

        private void ComboBox_SizeChanged(object? sender, Avalonia.Controls.SizeChangedEventArgs e)
        {
            Filtrs();
        }

        private void ComboBox_SizeChanged_1(object? sender, Avalonia.Controls.SizeChangedEventArgs e)
        {
            Filtrs();
        }

        private void Filtrs()
        {
            List<Product> filtr = SavingDate.products;
            int d = Filtr1.SelectedIndex;
            switch (d)
            {
                case 0: Listins(SavingDate.products); break;
                case 1:
                    {
                        for (int i = 0; i < filtr.Count; i++)
                            for (int j = 0; j < filtr.Count - i - 1; j++)
                            {
                                if (filtr[j].Price < filtr[j + 1].Price)
                                {
                                    Product temp = filtr[j];
                                    filtr[j] = filtr[j + 1];
                                    filtr[j + 1] = temp;
                                }
                            }
                        Listins(filtr);
                    }
                    break;
                case 2:
                    {
                        for (int i = 0; i < filtr.Count; i++)
                            for (int j = 0; j < filtr.Count - i - 1; j++)
                            {
                                if (filtr[j].Price > filtr[j + 1].Price)
                                {
                                    Product temp = filtr[j];
                                    filtr[j] = filtr[j + 1];
                                    filtr[j + 1] = temp;
                                }
                            }
                        Listins(filtr);
                    }
                    break;
            }
            int v = Filtr2.SelectedIndex;
            if (v == 0)
            {
                Listins(SavingDate.products);
            }
            else
            {
                Listins(filtr.Where(x => x.Manufactured == v).ToList());
                ColvoKont.Text = $"Выведено записей {filtr.Where(x => x.Manufactured == v).ToList().Count()} из {SavingDate.products.Count}";
            }
        }


        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var btn = (sender as Button)!;
            switch (btn.Name)
            {
                case "Buts":
                    string v = ((string)btn!.Tag!); //((int)btn!.Tag!)
                    List<Product> products = Helper.defaultDbContext.Products.Where(x => x.Name == v).ToList();
                    SavingDate.prods = products[0];//Helper.defaultDbContext.Products.Find((v));
                    new RedWindow1().Show();
                    Close(); break;
            }
        }

        private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            new RedWindow1().Show();
            Close();
        }
    }
}