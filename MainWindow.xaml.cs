using AVSSalesExplorer.DTOs;
using AVSSalesExplorer.Pages;
using AVSSalesExplorer.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AVSSalesExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel vm;
        private readonly EditItemViewModel editVm;
        
        public MainWindow()
        {
            InitializeComponent();
            
            vm = DependencyResolver.Instance.GetRequiredService<MainWindowViewModel>();
            editVm = DependencyResolver.Instance.GetRequiredService<EditItemViewModel>();
            
            DataContext = vm;
            productGrid.Loaded += ProductGrid_Loaded;            
        }

        private async void ProductGrid_Loaded(object sender, RoutedEventArgs e)
        {            
            await vm.LoadData();            
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var rowVm = ((Button)sender).DataContext as ItemViewModel;
            var updateItemRequest = new UpdateItemRequest 
                { 
                    Id = rowVm.Id,
                    Price = rowVm.Price,
                    Category = rowVm.Category,
                    Description = rowVm.Description,
                    Photo = rowVm.Photo,
                    PurchaseDate = rowVm.PurchaseDate,
                    Comment = rowVm.Comment,
                    Sizes = rowVm.Sizes.ToArray()
                };

            editVm.SetItemData(updateItemRequest);

            EditItemDialog editItemDialog = new();
            if (editItemDialog.ShowDialog() == true)
            {
                rowVm.Description = editVm.Description;
                rowVm.Sizes = new ObservableCollection<ItemSizeRequest>(editVm.Sizes);
                rowVm.Price = editVm.Price;
                rowVm.Photo = editVm.Photo;
                rowVm.PurchaseDate = editVm.PurchaseDate;
                rowVm.Comment = editVm.Comment;
                
                ((Button)sender).DataContext = rowVm;
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            // Show confirm & delete row with grid data update
            var rowVm = ((Button)sender).DataContext as ItemViewModel;
            if (MessageBox.Show($"Вы действительно хотите удалить {rowVm.Description}? Данные о размерах и продажах тоже будут удалены.", "Удаление", 
                MessageBoxButton.OKCancel, 
                MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                await vm.DeleteItemById(rowVm.Id);
                await vm.LoadData();
            }
        }

        private async void AddNewItem_Click(object sender, RoutedEventArgs e)
        {
            editVm.NewItem();
            var editItemDialog = new EditItemDialog();

            var result = editItemDialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                await vm.LoadData();
            }
        }
    }
}