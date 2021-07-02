using System.Windows;
using AVSSalesExplorer.ViewModels;

namespace AVSSalesExplorer.Pages
{
    /// <summary>
    /// Interaction logic for ItemSalesDialog.xaml
    /// </summary>
    public partial class ItemSalesDialog : Window
    {
        private readonly ItemSalesViewModel vm;

        public ItemSalesDialog()
        {
            InitializeComponent();
            vm = DependencyResolver.Instance.GetRequiredService<ItemSalesViewModel>();
            DataContext = vm;

            SalesGrid.Loaded += async (sender, e) => {
                await vm.LoadData();
            };
        }

        private void OKButton_Clicked(object sender, RoutedEventArgs e)
        {
           DialogResult = true;
        }
    }
}