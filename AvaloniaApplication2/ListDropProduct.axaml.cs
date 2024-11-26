using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using HarfBuzzSharp;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using static AvaloniaApplication2.SavingDate;
using AvaloniaApplication2.Models;
using System.Linq;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using AvaloniaApplication2.Models;
using AvaloniaApplication2;

namespace AvaloniaApplication2;

public partial class ListDopProduct : Window
{
    public List<ListDoptov> Doptovs = Helper.defaultDbContext.ListDoptovs.Where(x => x.IdTov == prods.Id).ToList();
    public List<Doptov> _Alldoptov = Helper.defaultDbContext.Doptovs.ToList();
    public ListDopProduct()
    {
        InitializeComponent();
        //Name_Pers.Text = _RedClient.Name;
        Listins(Doptovs); //Вывод тего конкретного человека
        Listins2(_Alldoptov);//Выводвсех тегов
    }
    private void Listins(List<ListDoptov> list)
    {


        TagList.ItemsSource = list.Select(x => new
        {
            x.Id,
            x.Iddoptow,
            doptovs[(int)x.Iddoptow - 1].Name,
            doptovs[(int)x.Iddoptow - 1].Price,
            doptovs[(int)x.Iddoptow - 1].Photo,
            photos = new Bitmap($"Assets/{doptovs[(int)x.Iddoptow - 1].Photo}"),
        });
    }
    private void Listins2(List<Doptov> list)
    {


        TagList2.ItemsSource = list.Select(x => new
        {
            doptovs[(int)x.Id - 1].Name,
            doptovs[(int)x.Id - 1].Price,
            doptovs[(int)x.Id - 1].Photo,
            photos = new Bitmap($"Assets/{doptovs[(int)x.Id - 1].Photo}"),
        });
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//Вернуться назад
    {
        new RedWindow1().Show();
        Close();
    }



    private void Button_Click_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//Добавление тега клиенту
    {

        if (NumberCheck(TagNomer.Text) == false)
        {
            int v = Convert.ToInt32(TagNomer.Text);
            if (v > 0 && v <= SavingDate.doptovs.Count())
            {
                ListDoptov taglist = new ListDoptov();
                taglist.Id = SavingDate.doptovlist.Count() + 1;
                taglist.IdTov = SavingDate.prods.Id;
                taglist.Iddoptow = v;
                Helper.defaultDbContext.ListDoptovs.Add(taglist);
                Helper.defaultDbContext.SaveChanges();
                new ListDopProduct().Show();
                Close();

            }


        }
    }

    private void Button_Click_3(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//удалить тег у клиента 
    {
        if (NumberCheck(TagNomer1.Text) == false)
        {
            int v = Convert.ToInt32(TagNomer1.Text);
            if (v > 0 && v <= SavingDate.doptovs.Count())
            {
                List<ListDoptov> Tag = Helper.defaultDbContext.ListDoptovs.Where(x => x.IdTov == prods.Id && x.Iddoptow == v).ToList();
                // int j= 

                Helper.defaultDbContext.ListDoptovs.Remove(Helper.defaultDbContext.ListDoptovs.Find(Tag.Last().Id)); //В БД находит объект и удаляет
                Helper.defaultDbContext.SaveChanges(); //сохранение изменений
                new ListDopProduct().Show();
                Close();

            }


        }
    }
    private bool NumberCheck(string name)// проверка на корректность выбора тегов
    {
        return new string[]
        { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P",
               "A", "S", "D", "F", "G", "H", "J", "K", "L", "Z",
                "X", "C", "V", "B", "N", "M", "Й", "Ц", "У", "К",
                 "Е", "Н", "Г", "Ш", "Щ", "З", "Х", "Ъ", "Ф", "Ы",
                  "В", "А", "П", "Р", "О", "Л", "Д", "Ж", "Э", "Я",
                   "Ч", "С", "М", "И", "Т", "Ь", "Б", "Ю",
                "`" , "~", "!", "@", "\"", "#", "№", "$", ";", "%",
                "^", ":" , "&", "?", "*", "_", "=",
                "'", "|", "/", "<", ">"
        }.Any(name.Contains);
    }
}
