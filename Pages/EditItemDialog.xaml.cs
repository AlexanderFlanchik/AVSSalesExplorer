using AVSSalesExplorer.ViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AVSSalesExplorer.Common;
using System.Linq;
using AVSSalesExplorer.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ModelValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace AVSSalesExplorer.Pages
{
    /// <summary>
    /// Interaction logic for EditItemDialog.xaml
    /// </summary>
    public partial class EditItemDialog : Window
    {
        private readonly EditItemViewModel vm;
        private readonly NewItemSizeViewModel newItemSizeVm;

        public EditItemDialog()
        {
            InitializeComponent();
            vm = DependencyResolver.Instance.GetRequiredService<EditItemViewModel>();
            newItemSizeVm = DependencyResolver.Instance.GetRequiredService<NewItemSizeViewModel>();

            DataContext = vm;
            Loaded += (s, e) => Title = vm.Id > 0 ? "Редактирование товара" : "Новый товар";            
        }

        private void RemoveSize_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button delBtn)
            {
                return;
            }

            var dc = (ItemSizeRequest)delBtn.DataContext;
            var sizes = vm.Sizes.ToList();
            
            vm.Sizes = sizes.Except(new[] { dc }).OrderBy(s => s.Size).ToArray();
        }

        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {            
            var validationResults = new List<ModelValidationResult>();
            var validationContext = new ValidationContext(vm);
            validationErrorsMessage.Text = string.Empty;
                       
            if (Validator.TryValidateObject(vm, validationContext, validationResults))
            {
                // all OK
                if (vm.IsNewItem) // Create
                {
                    var newAddnewItemRequest = new AddNewItemRequest();
                    FillItemRequestData(newAddnewItemRequest);

                    await vm.AddNewItem(newAddnewItemRequest);
                }
                else // Update
                {
                    var updateItemRequest = new UpdateItemRequest { Id = vm.Id };
                    FillItemRequestData(updateItemRequest);

                    await vm.UpdateItem(updateItemRequest);
                }

                DialogResult = true;
            }
            else
            {                
                // Errors
                validationErrorsMessage.Text = string.Join(". ", validationResults.Select(c => c.ErrorMessage).ToArray());                    
            }

            void FillItemRequestData(ItemRequest request)
            {
                request.Description = vm.Description;
                request.Photo = vm.Photo;
                request.Price = vm.Price;
                request.PurchaseDate = vm.PurchaseDate;
                request.Comment = vm.Comment;
                request.Sizes = vm.Sizes;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {            
            DialogResult = false;
        }

        private void NewSize_Click(object sender, RoutedEventArgs e)
        {                
            var alreadyUsedSizes = vm.Sizes?.Select(s => s.Size).ToArray();
            if (alreadyUsedSizes != null && alreadyUsedSizes.Any())
            {
                newItemSizeVm.AlreadyAddedSizes = alreadyUsedSizes;
                if (!newItemSizeVm.AvailableSizes.Any())                                
                {
                    MessageBox.Show("Вы уже добавили все доступные размеры.", "Новый размер", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            if (newItemSizeVm.AvailableSizes.Any())
            {
                newItemSizeVm.Size = newItemSizeVm.AvailableSizes.FirstOrDefault();
            }
            
            newItemSizeVm.Amount = 1;

            var newItemSizeDialog = new NewItemSizeDialog();
            if (newItemSizeDialog.ShowDialog() == true)
            {
                var newItemSizeRequest = new ItemSizeRequest 
                    { 
                        InStock = true, 
                        Size = newItemSizeVm.Size, 
                        Amount = newItemSizeVm.Amount 
                    };

                var sizes = vm.Sizes.ToList();
                sizes.Add(newItemSizeRequest);

                vm.Sizes = sizes.ToArray();
            }
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
                if (fileOpenDialog.ShowDialog() == true)
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
            => this.GetDecimalNumberTextBoxValidationHandler().Invoke(sender, e);
    }
}