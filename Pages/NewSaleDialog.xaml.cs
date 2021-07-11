using AVSSalesExplorer.Common;
using AVSSalesExplorer.DTOs;
using AVSSalesExplorer.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using ModelValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace AVSSalesExplorer.Pages
{
    /// <summary>
    /// Interaction logic for NewSaleDialog.xaml
    /// </summary>
    public partial class NewSaleDialog : Window
    {
        private readonly NewSaleViewModel vm;

        public NewSaleDialog()
        {
            InitializeComponent();
            vm = DependencyResolver.Instance.GetRequiredService<NewSaleViewModel>();
            DataContext = vm;
        }

        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            var validationContext = new ValidationContext(vm);
            var validationResults = new List<ModelValidationResult>();
            
            if (Validator.TryValidateObject(vm, validationContext, validationResults))
            {
                var newSaleRequest = new NewItemSaleRequest() 
                { 
                    ItemId = vm.ItemId,
                    Price = vm.Price,
                    Address = vm.Address,
                    Phone = vm.Phone,
                    Customer = vm.Customer,
                    SaleDate = DateTime.Now
                };

                if (vm.Category != ItemCategory.Bags)
                {
                    newSaleRequest.Size = vm.Size;
                }

                vm.SaleId = await vm.CreateNewSale(newSaleRequest);

                DialogResult = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            vm.ClearValidationResults();
            DialogResult = false;
        }

        private void Price_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var mainWindow = DependencyResolver.Instance.GetRequiredService<MainWindow>();
            mainWindow.GetDecimalNumberTextBoxValidationHandler()
                    .Invoke(sender, e);
        }
    }
}