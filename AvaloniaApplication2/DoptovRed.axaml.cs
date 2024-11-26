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
    private string? _PictureFile = null;//_RedClient != null ? _RedClient.Photo : null; //�����������, ������� ���������� ����� ������ (���� ��� � ���� ����, ����� null)
    private string? _SelectedImage = null; //��������� �����������
    public DoptovRed()
    {
        InitializeComponent();
        Filtr2.ItemsSource = SavingDate.manufactrurs;
        Filtr2.SelectedIndex = 0;
        Podgruzdan();
    }
    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//��������� �����
    {
        new RedWindow1().Show();
        Close();
    }

    private void Button_Click_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//��������� ������
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
            Helper.defaultDbContext.Doptovs.Add(product); //���������� � ��
            Helper.defaultDbContext.ListDoptovs.Add(doptov);
            Helper.defaultDbContext.SaveChanges(); //���������� ���������


        }
        else
        {
            SavingDate.doptov.Name = Name.Text;
            SavingDate.doptov.Description = Description.Text;
            SavingDate.doptov.Price = Convert.ToInt32(Price.Text);
            SavingDate.doptov.Photo = _SelectedImage;
            SavingDate.doptov.Manufactured = Filtr2.SelectedIndex;
            SavingDate.doptov.Isactive = Sclad.IsChecked == true ? 1 : 2;

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
    private void ComboBox_SizeChanged(object? sender, Avalonia.Controls.SizeChangedEventArgs e)//���������
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