using AVSSalesExplorer.Pages;
using AVSSalesExplorer.Services;
using AVSSalesExplorer.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
                
                services.AddSingleton<MainWindowViewModel>();                
                services.AddSingleton<EditItemViewModel>();
                services.AddSingleton<NewItemSizeViewModel>();
                services.AddSingleton<NewSaleViewModel>();
            }).Build();

            DependencyResolver.Instance.Init(host);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var dbContext = host.Services.GetRequiredService<ItemDbContext>();
            dbContext.Database.EnsureCreated();
                        
            var mainWindow = new MainWindow();
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
