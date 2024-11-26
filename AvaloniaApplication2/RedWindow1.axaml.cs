using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaApplication2.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using static AvaloniaApplication2.SavingDate;
using HarfBuzzSharp;
using System.Diagnostics;
using AvaloniaApplication2.Models;
using AvaloniaApplication2;
using System.IO.Pipelines;

namespace AvaloniaApplication2;

public partial class RedWindow1 : Window
{
    private string? _PictureFile = null;//_RedClient != null ? _RedClient.Photo : null; //�����������, ������� ���������� ����� ������ (���� ��� � ���� ����, ����� null)
    private string? _SelectedImage = null; //��������� �����������
    public List<ListDoptov> Doptovs = Helper.defaultDbContext.ListDoptovs.Where(x => x.IdTov == prods.Id).ToList();
    public RedWindow1()
    {
        InitializeComponent();
        Filtr2.ItemsSource = SavingDate.manufactrurs;
        Filtr2.SelectedIndex = 0;
        Listins(Doptovs);
        Podgruzdan();
    }

    private void Podgruzdan()
    {
        if (SavingDate.prods != null)
        {
            Id.Text = Convert.ToString(SavingDate.prods.Id);
            Name.Text = Convert.ToString(SavingDate.prods.Name);
            Description.Text = Convert.ToString(SavingDate.prods.Description);
            Price.Text = Convert.ToString(SavingDate.prods.Price);
            Filtr2.SelectedIndex = (int)SavingDate.prods.Manufactured;
            Sclad.IsChecked = SavingDate.prods.Isactive == 1 ? true : false;
            image_clientPhoto.Source = new Bitmap($"Assets/{SavingDate.prods.Photo}");
            image_clientPhoto.IsVisible = true;
            _SelectedImage = SavingDate.prods.Photo;
        }
        else
        {
            Id.Text = Convert.ToString(SavingDate.products.Count() + 1);
        }
    }

    private void ComboBox_SizeChanged(object? sender, Avalonia.Controls.SizeChangedEventArgs e)//���������
    {
    }



    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//��������� �����
    {
        new MainWindow().Show();
        Close();
    }

    private void Button_Click_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//��������� ������
    {
        if (SavingDate.prods == null)
        {

            Product product = new Product();
            product.Id = SavingDate.products.Count() + 1;
            product.Name = Name.Text;
            product.Description = Description.Text;
            product.Price = Convert.ToInt32(Price.Text);
            product.Photo = _SelectedImage;
            product.Isactive = Sclad.IsChecked == true ? 1 : 2;
            product.Manufactured = Filtr2.SelectedIndex;
            Helper.defaultDbContext.Products.Add(product); //���������� � ��
            Helper.defaultDbContext.SaveChanges(); //���������� ���������


        }
        else
        {
            SavingDate.prods.Name = Name.Text;
            SavingDate.prods.Description = Description.Text;
            SavingDate.prods.Price = Convert.ToInt32(Price.Text);
            SavingDate.prods.Photo = _SelectedImage;
            SavingDate.prods.Manufactured = Filtr2.SelectedIndex;
            SavingDate.prods.Isactive = Sclad.IsChecked == true ? 1 : 2;

            //Helper.user.�������s.Add(newClient); //���������� � ��
            Helper.defaultDbContext.SaveChanges(); //���������� ���������
        }
    }
    private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) //����� � �������� �����������
    {
        var btn = (sender as Button)!;
        switch (btn.Name)
        {
            case "btn_addImage": //����������
                OpenFileDialog dialog = new(); //�������� ����������
                dialog.Filters.Add(fileFilter); //���������� �������
                string[] result = await dialog.ShowAsync(this); //����� �����
                if (result == null || result.Length == 0 || new System.IO.FileInfo(result[0]).Length > 2000000)
                    return;//���� ������� ��������� ��� ������ ����� ����� ��������� 2 ��, �� �������� �� ����� �������

                string imageName = System.IO.Path.GetFileName(result[0]); //��������� ����� �����
                string[] extention = imageName.Split('.'); //�������� ����� ������� �� �������� � ����������
                string temp = extention[0]; //� ���������� ���������� �������� �������� �����. ��� ����� �������� � ��������
                int i = 1; //�������
                           // while (SameName(temp) != null) //���� ����� ��� �������� ������������ ����� ���������� �������� �����
                           //{
                           //      temp = extention[0] + $"{i}"; //����� ��� �����
                           //  i++;
                           // }
                imageName = temp + '.' + extention[1]; //����� ��� ����� � �����������

                System.IO.File.Copy(result[0], $"Assets/{imageName}", true); //����������� ����� � ����� �������

                tblock_clientPhoto.Text = imageName;
                image_clientPhoto.Source = new Bitmap($"Assets/{imageName}");
                tblock_clientPhoto.IsVisible = image_clientPhoto.IsVisible = btn_deleteImage.IsVisible = true;
                if (_SelectedImage != null && _SelectedImage != _PictureFile) //���� �� ��������� ����� �������� ���� ������� ������, � ��� ���� ��������� �������� �� �������� �� ����, �������� ������������ ����������� ������
                    System.IO.File.Delete($"Assets/{_SelectedImage}"); //�������� ����������� ����������� �� �������
                _SelectedImage = imageName;

                break;
            case "btn_deleteImage": //��������
                tblock_clientPhoto.IsVisible = image_clientPhoto.IsVisible = btn_deleteImage.IsVisible = false;

                // if (_SelectedImage != _PictureFile) //�������� ���������� ������ ���� ��������� ����������� �� �������� ��������� �� ����, �������� ������������ ����������� �������
                System.IO.File.Delete($"Assets/{_SelectedImage}"); //�������� ���������� �����������
                                                                   //���������� ���������
                                                                   //  _//SelectedImage = null;//������� ���� � ��������� ������������
                break;
        }
    }
    private readonly FileDialogFilter fileFilter = new() //������ ��� ����������
    {
        Extensions = new List<string>() { "jpg", "png" }, //��������� ����������, ������������ � ����������
        Name = "����� �����������" //���������
    };

    private void Button_Click_3(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (SavingDate.doptov != null)
        {
            new DoptovRed().Show();
            Close();
        }

    }

    private void Button_Click_4(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//�������������� ������ �������������� �������
    {
        new ListDopProduct().Show();
        Close();
    }

    private void Button_Click_5(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//�������������� �������� ��� ������ 
    {

        var btn = (sender as Button)!;
        switch (btn.Name)
        {
            case "Buts":
                string v = ((string)btn!.Tag!); //((int)btn!.Tag!)
                List<Doptov> products = Helper.defaultDbContext.Doptovs.Where(x => x.Name == v).ToList();
                SavingDate.doptov = products[0];//Helper.defaultDbContext.Products.Find((v));
                new DoptovRed().Show();
                Close(); break;
        }


    }
    private void Listins(List<ListDoptov> list)
    {


        DopProd.ItemsSource = list.Select(x => new
        {
            x.Id,
            x.Iddoptow,
            doptovs[(int)x.Iddoptow - 1].Name,
            doptovs[(int)x.Iddoptow - 1].Price,
            doptovs[(int)x.Iddoptow - 1].Photo,
            photos = new Bitmap($"Assets/{doptovs[(int)x.Iddoptow - 1].Photo}"),
            //Color = $"{tag[(int)x.IdTag - 1].Color}",
            //tag[(int)y.IdTag - 1].Color,   
        });
    }
}