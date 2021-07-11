using AVSSalesExplorer.Pages;
using AVSSalesExplorer.Services;
using AVSSalesExplorer.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace AVSSalesExplorer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost host;
        
        public App()
        {
            host = new HostBuilder().ConfigureServices((services) => {
                services.AddSingleton<ImageResizeService>();
                services.AddDbContext<ItemDbContext>();
                services.AddTransient<IItemService, ItemService>();
                services.AddTransient<IItemSaleService, ItemSaleService>();
                services.AddTransient<ISalesListDataService, SalesListDataService>();

                services.AddSingleton<MainWindow>();
                services.AddSingleton<ItemListPageViewModel>();                
                services.AddSingleton<EditItemViewModel>();
                services.AddSingleton<NewItemSizeViewModel>();
                services.AddSingleton<NewSaleViewModel>();
                services.AddSingleton<ItemSalesViewModel>();
                services.AddSingleton<SalesListViewModel>();
            }).Build();

            DependencyResolver.Instance.Init(host);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var dbContext = host.Services.GetRequiredService<ItemDbContext>();
            dbContext.Database.EnsureCreated();

            var mainWindow = host.Services.GetRequiredService<MainWindow>();
            mainWindow.Content = new LandingPage();
            mainWindow.Show();            
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            using (host)
            {
                await host.StopAsync();
            }
        }
    }
}