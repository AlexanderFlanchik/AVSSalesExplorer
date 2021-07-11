using AVSSalesExplorer.ViewModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AVSSalesExplorer.Pages
{
    /// <summary>
    /// Interaction logic for SalesList.xaml
    /// </summary>
    public partial class SalesList : Page
    {
        private readonly MainWindow _mainWindow = DependencyResolver.Instance.GetRequiredService<MainWindow>();
        private readonly SalesListViewModel vm;

        public SalesList()
        {
            InitializeComponent();
            
            vm = DependencyResolver.Instance.GetRequiredService<SalesListViewModel>();
            DataContext = vm;
            
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var dateToDay = DateTime.DaysInMonth(year, month);

            vm.DateFrom = new DateTime(year, month, 1);
            vm.DateTo = new DateTime(year, month, dateToDay);

            SaleDateFrom.SelectedDateChanged += (s, e) => { 
                if (vm.DateFrom > vm.DateTo)
                {
                    vm.DateTo = vm.DateFrom;
                }
            };

            SaleDateTo.SelectedDateChanged += (s, e) =>
            {
                if (vm.DateTo < vm.DateFrom)
                {
                    vm.DateFrom = vm.DateTo;
                }
            };
        }

        private void GobackBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Content = new LandingPage();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await vm.LoadData();
        }

        private void SalesList_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;
        }

        private async void ApplyDatesFilterBtn_Click(object sender, RoutedEventArgs e)
        {
            await vm.LoadData();
        }

        private async void pageForwardBtn_Click(object sender, RoutedEventArgs e)
        {
            vm.PageNumber++;
            await vm.LoadData();
        }

        private async void pageBackBtn_Click(object sender, RoutedEventArgs e)
        {
            vm.PageNumber--;
            await vm.LoadData();
        }

        private async void PageSizeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (vm.Sales is null || !vm.Sales.Any())
            {
                e.Handled = true;
                
                return;
            }

            vm.PageNumber = 1;
            await vm.LoadData();
        }
    }
}