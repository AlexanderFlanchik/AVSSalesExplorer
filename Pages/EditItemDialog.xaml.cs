using AVSSalesExplorer.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AVSSalesExplorer.Pages
{
    /// <summary>
    /// Interaction logic for EditItemDialog.xaml
    /// </summary>
    public partial class EditItemDialog : Window
    {
        private readonly EditItemViewModel vm;

        public EditItemDialog(EditItemViewModel viewModel)
        {
            InitializeComponent();

            vm = viewModel;
            DataContext = vm;
            Loaded += (s, e) => Title = vm.Id > 0 ? "Редактирование товара" : "Новый товар";            
        }

        private void RemoveSize_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            
            Close();
        }

        private void NewSize_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void LoadPhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            var fileOpenDialog = new OpenFileDialog
            {
                Title = "Выберите изображение",
                Filter = "Изображения(*.bmp;*.jpg;*.gif;*.png)|*.BMP;*.JPG;*.GIF;*.PNG"
            };

            try
            {
                var showDialog = fileOpenDialog.ShowDialog();
                if (showDialog.HasValue && showDialog.Value)
                {
                    var fi = new FileInfo(fileOpenDialog.FileName);
                    if (!fi.Exists)
                    {
                        throw new InvalidOperationException();
                    }

                    using var stream = fi.OpenRead();
                    const int maxSize = 150;

                    // This loads an image and updates view data context
                    await vm.LoadAndResizePhoto(stream, fi.Extension[1..], maxSize);
                }
            }
            catch
            {
                MessageBox.Show("Невозможно загрузить изображение. Выберите пожалуйста другой файл.",
                         "Загрузка изображения",
                         MessageBoxButton.OK,
                         MessageBoxImage.Warning
                  );
            }
        }

        private void Price_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var inpt = (sender as TextBox).Text;
            
            Regex regex;
            if (inpt.Contains(".") || inpt.Contains(","))
            {
                regex = new Regex(@"[^0-9]+");
            } 
            else 
            {
                regex = new Regex(@"[^0-9.|,]+");
            }
            
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}