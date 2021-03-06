using AVSSalesExplorer.Common;
using AVSSalesExplorer.DTOs;
using AVSSalesExplorer.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AVSSalesExplorer.Pages
{
    /// <summary>
    /// Interaction logic for ItemListPage.xaml
    /// </summary>
    public partial class ItemListPage : Page
    {
        private readonly ItemListPageViewModel vm;
        private readonly EditItemViewModel editVm;
        private readonly NewSaleViewModel newSaleVm;
        private readonly ItemSalesViewModel salesDialogVm;
        private readonly MainWindow mainWindow = DependencyResolver.Instance.GetRequiredService<MainWindow>();

        public ItemListPage()
        {
            InitializeComponent();

            vm = DependencyResolver.Instance.GetRequiredService<ItemListPageViewModel>();
            editVm = DependencyResolver.Instance.GetRequiredService<EditItemViewModel>();
            newSaleVm = DependencyResolver.Instance.GetRequiredService<NewSaleViewModel>();
            salesDialogVm = DependencyResolver.Instance.GetRequiredService<ItemSalesViewModel>();

            DataContext = vm;
            productGrid.Loaded += ProductGrid_Loaded;
        }

        private async void ProductGrid_Loaded(object sender, RoutedEventArgs e)
        {
            await vm.LoadData();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var rowVm = GetRowModel(sender);
            var updateItemRequest = new UpdateItemRequest
            {
                Id = rowVm.Id,
                Price = rowVm.Price,
                Category = rowVm.Category,
                Description = rowVm.Description,
                Photo = rowVm.Photo,
                PurchaseDate = rowVm.PurchaseDate,
                Comment = rowVm.Comment,
                Sizes = rowVm.Sizes.Where(s => s.Amount > 0).ToArray(),
                InStock = rowVm.InStock
            };

            editVm.SetItemData(updateItemRequest);

            EditItemDialog editItemDialog = new();
            editItemDialog.Owner = mainWindow;

            if (editItemDialog.ShowDialog() == true)
            {
                rowVm.Description = editVm.Description;
                rowVm.Sizes = new ObservableCollection<ItemSizeRequest>(editVm.Sizes);
                rowVm.Price = editVm.Price;
                rowVm.Photo = editVm.Photo;
                rowVm.PurchaseDate = editVm.PurchaseDate;
                rowVm.Comment = editVm.Comment;
                rowVm.InStock = editVm.InStock;

                ((Button)sender).DataContext = rowVm;
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            // Show confirm & delete row with grid data update
            var rowVm = GetRowModel(sender);
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
            editItemDialog.Owner = mainWindow;

            var result = editItemDialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                await vm.LoadData();
            }
        }

        private async void NewSale_Click(object sender, RoutedEventArgs e)
        {
            newSaleVm.ClearForm();

            var rowVm = GetRowModel(sender);
            newSaleVm.ItemId = rowVm.Id;
            newSaleVm.Photo = rowVm.Photo;
            newSaleVm.Description = rowVm.Description;
            newSaleVm.Category = rowVm.Category;

            if (newSaleVm.Category != ItemCategory.Bags)
            {
                if (!rowVm.Sizes.Any())
                {
                    MessageBox.Show("Все размеры данного товара проданы.", "Новый товар", MessageBoxButton.OK, MessageBoxImage.Warning);

                    return;
                }

                newSaleVm.Sizes = rowVm.Sizes.Where(s => s.Amount > 0).Select(s => s.Size).ToArray();
                newSaleVm.Size = newSaleVm.Sizes.FirstOrDefault();
            }

            var newSaleDlg = new NewSaleDialog();
            newSaleDlg.Owner = mainWindow;

            if (newSaleDlg.ShowDialog() == true)
            {
                // Update sales value and size amount              
                rowVm.Sales++;

                if (newSaleVm.Category == ItemCategory.Bags)
                {
                    rowVm.InStock = false;
                    await vm.UpdateItemInStock(rowVm.Id, false);
                    return;
                }

                var currenSizes = rowVm.Sizes.ToList();
                var sz = currenSizes.Where(s => s.Size == newSaleVm.Size).FirstOrDefault();
                if (sz is null)
                {
                    return;
                }

                sz.Amount--;
                rowVm.Sizes = new ObservableCollection<ItemSizeRequest>(currenSizes);
                rowVm.InStock = currenSizes.Where(s => s.Amount > 0).Any();
            }
        }

        private void SalesDialogOpenBtn_Click(object sender, RoutedEventArgs e)
        {
            var rowVm = GetRowModel(sender);
            salesDialogVm.ItemId = rowVm.Id;

            var salesDialog = new ItemSalesDialog();
            salesDialog.Owner = mainWindow;

            salesDialog.ShowDialog();
        }

        private static ItemViewModel GetRowModel(object sender)
            => ((FrameworkElement)sender).DataContext as ItemViewModel;

        private void productGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;
        }

        private async void goBackBtn_Click(object sender, RoutedEventArgs e)
        {
            vm.PageNumber--;
            await vm.LoadData();
        }

        private async void goForwardBtn_Click(object sender, RoutedEventArgs e)
        {
            vm.PageNumber++;
            await vm.LoadData();
        }

        private async void PageSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (vm.Items == null || !vm.Items.Any())
            {
                e.Handled = true;
                return;
            }

            vm.PageNumber = 1;
            await vm.LoadData();
        }

        private void GobackBtn_Click(object sender, RoutedEventArgs e)
        {            
            mainWindow.Content = new LandingPage();
        }

        private async void ApplyFiltersBtn_Click(object sender, RoutedEventArgs e)
        {
            await vm.LoadData();
        }

        private async void ClearFiltersBtn_Click(object sender, RoutedEventArgs e)
        {
            vm.ResetFilters();
            await vm.LoadData();
        }

        private void Prices_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            mainWindow.GetDecimalNumberTextBoxValidationHandler().Invoke(sender, e);
        }        
    }
}