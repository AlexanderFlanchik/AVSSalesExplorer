using AVSSalesExplorer.Pages;
using AVSSalesExplorer.ViewModels;
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
            // TODO: Show edit row dialog
            var rowVm = ((Button)sender).DataContext as ItemViewModel;
            MessageBox.Show($"Id = {rowVm.Id}, Описание: {rowVm.Description}");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // Show confirm & delete row with grid data update
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