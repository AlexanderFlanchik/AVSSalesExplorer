using System.Windows;
using System.Windows.Controls;

namespace AVSSalesExplorer.Pages
{
    /// <summary>
    /// Interaction logic for LandingPage.xaml
    /// </summary>
    public partial class LandingPage : Page
    {
        private readonly Window _mainWindow = DependencyResolver.Instance.GetRequiredService<MainWindow>();

        public LandingPage()
        {
            InitializeComponent();
        }

        private void ItemListLink_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Content = new ItemListPage();
        }

        private void SaleListLink_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Content = new SalesList();
        }
    }
}