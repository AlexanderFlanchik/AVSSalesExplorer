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
using AVSSalesExplorer.Common;
using AVSSalesExplorer.ViewModels;

namespace AVSSalesExplorer.Pages
{
    /// <summary>
    /// Interaction logic for NewItemSizeDialog.xaml
    /// </summary>
    public partial class NewItemSizeDialog : Window
    {
        private readonly NewItemSizeViewModel vm;
        
        public NewItemSizeDialog()
        {            
            InitializeComponent();

            vm = DependencyResolver.Instance.GetRequiredService<NewItemSizeViewModel>();
            DataContext = vm;
        }

        private void Amount_PreviewTextInput(object sender, TextCompositionEventArgs e)
            => this.GetIntNumberTextBoxValidationHandler().Invoke(sender, e);

        private void OKBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            //Close();
        }
    }
}