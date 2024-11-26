using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaApplication2.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using AvaloniaApplication2.Models;
using AvaloniaApplication2;

namespace AvaloniaApplication2;

public partial class DoptovRed : Window
{
    private string? _PictureFile = null;//_RedClient != null ? _RedClient.Photo : null; //изображение, которое изначально имеет объект (если оно у него есть, иначе null)
    private string? _SelectedImage = null; //выбранное изображение
    public DoptovRed()
    {
        InitializeComponent();
        Filtr2.ItemsSource = SavingDate.manufactrurs;
        Filtr2.SelectedIndex = 0;
        Podgruzdan();
    }
    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//Вернуться назад
    {
        new RedWindow1().Show();
        Close();
    }

    private void Button_Click_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//Сохранить данные
    {
        if (SavingDate.doptov == null)
        {

            Doptov product = new Doptov();
            product.Id = SavingDate.doptovs.Count() + 1;
            product.Name = Name.Text;
            product.Description = Description.Text;
            product.Price = Convert.ToInt32(Price.Text);
            product.Photo = _SelectedImage;
            product.Isactive = Sclad.IsChecked == true ? 1 : 2;
            product.Manufactured = Filtr2.SelectedIndex;
            ListDoptov doptov = new ListDoptov();
            doptov.Id = SavingDate.doptovs.Count() + 1;
            doptov.IdTov = SavingDate.prods.Id;
            doptov.Iddoptow = SavingDate.doptovs.Count() + 1;
            Helper.defaultDbContext.Doptovs.Add(product); //добавление в БД
            Helper.defaultDbContext.ListDoptovs.Add(doptov);
            Helper.defaultDbContext.SaveChanges(); //сохранение изменений


        }
        else
        {
            SavingDate.doptov.Name = Name.Text;
            SavingDate.doptov.Description = Description.Text;
            SavingDate.doptov.Price = Convert.ToInt32(Price.Text);
            SavingDate.doptov.Photo = _SelectedImage;
            SavingDate.doptov.Manufactured = Filtr2.SelectedIndex;
            SavingDate.doptov.Isactive = Sclad.IsChecked == true ? 1 : 2;

            //Helper.user.Клиентыs.Add(newClient); //добавление в БД
            Helper.defaultDbContext.SaveChanges(); //сохранение изменений
        }
    }
    private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) //Выбор и удаление изображения
    {
        var btn = (sender as Button)!;
        switch (btn.Name)
        {
            case "btn_addImage": //добавление
                OpenFileDialog dialog = new(); //Открытие проводника
                dialog.Filters.Add(fileFilter); //Применение фильтра
                string[] result = await dialog.ShowAsync(this); //Выбор файла
                if (result == null || result.Length == 0 || new System.IO.FileInfo(result[0]).Length > 2000000)
                    return;//Если закрыть проводник или размер файла будет превышать 2 МБ, то картинка не будет выбрана

                string imageName = System.IO.Path.GetFileName(result[0]); //получение имени файла
                string[] extention = imageName.Split('.'); //Название файла делится на название и расширение
                string temp = extention[0]; //В изменяемой переменной хранится название файла. Оно будет меняться в процессе
                int i = 1; //Счетчик
                           // while (SameName(temp) != null) //Пока метод для проверки уникальности файла возвращает название файла
                           //{
                           //      temp = extention[0] + $"{i}"; //Новое имя файла
                           //  i++;
                           // }
                imageName = temp + '.' + extention[1]; //Новое имя файла с расширением

                System.IO.File.Copy(result[0], $"Assets/{imageName}", true); //Копирование файла в папку ассетов

                tblock_clientPhoto.Text = imageName;
                image_clientPhoto.Source = new Bitmap($"Assets/{imageName}");
                tblock_clientPhoto.IsVisible = image_clientPhoto.IsVisible = btn_deleteImage.IsVisible = true;
                if (_SelectedImage != null && _SelectedImage != _PictureFile) //Если до установки новой картинки была выбрана другая, и при этом выбранная картинка не значение из поля, хранящее изначальноне изображение товара
                    System.IO.File.Delete($"Assets/{_SelectedImage}"); //Удаление предыдущего изображения из ассетов
                _SelectedImage = imageName;

                break;
            case "btn_deleteImage": //удаление
                tblock_clientPhoto.IsVisible = image_clientPhoto.IsVisible = btn_deleteImage.IsVisible = false;

                // if (_SelectedImage != _PictureFile) //Удаление произойдет только если удаляемое изображение не является значением из поля, хранящее изначальноне изображение объекта
                System.IO.File.Delete($"Assets/{_SelectedImage}"); //Удаление выбранного изображения
                                                                   //сохранение изменений
                                                                   //  _//SelectedImage = null;//очистка поля с выбранным изображением
                break;
        }
    }
    private readonly FileDialogFilter fileFilter = new() //Фильтр для проводника
    {
        Extensions = new List<string>() { "jpg", "png" }, //доступные расширения, отображаемые в проводнике
        Name = "Файлы изображений" //пояснение
    };
    private void ComboBox_SizeChanged(object? sender, Avalonia.Controls.SizeChangedEventArgs e)//комбобокс
    {
    }
    private void Podgruzdan()
    {
        if (SavingDate.doptov != null)
        {
            Id.Text = Convert.ToString(SavingDate.doptov.Id);
            Name.Text = Convert.ToString(SavingDate.doptov.Name);
            Description.Text = Convert.ToString(SavingDate.doptov.Description);
            Price.Text = Convert.ToString(SavingDate.doptov.Price);
            Filtr2.SelectedIndex = (int)SavingDate.doptov.Manufactured;
            Sclad.IsChecked = SavingDate.doptov.Isactive == 1 ? true : false;
            image_clientPhoto.Source = new Bitmap($"Assets/{SavingDate.doptov.Photo}");
            image_clientPhoto.IsVisible = true;
            _SelectedImage = SavingDate.doptov.Photo;
        }
        else
        {
            Id.Text = Convert.ToString(SavingDate.doptovs.Count() + 1);
        }
    }
}