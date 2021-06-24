using AVSSalesExplorer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
    }
}
