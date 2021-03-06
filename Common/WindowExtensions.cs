using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AVSSalesExplorer.Common
{
    internal static class WindowExtensions
    {
        /// <summary>
        /// Gets an input validation handler for TextBox control that allows enter only valid decimal numbers.
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        internal static TextCompositionEventHandler GetDecimalNumberTextBoxValidationHandler(this Window window)
        {
            return (sender, e) => {
                if (sender is not TextBox textBox)
                {
                    return;
                }

                var inpt = textBox.Text;
                var decimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                Regex regex;
                if (inpt.Contains(decimalSeparator))
                {
                    regex = new Regex(@"[^0-9]+");
                }
                else
                {
                    regex = new Regex($@"[^0-9.|{decimalSeparator}]+");
                }

                e.Handled = regex.IsMatch(e.Text);
            };
        }

        /// <summary>
        /// Gets an input validation handler for TextBox control that allows enter only valid integer numbers.
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        internal static TextCompositionEventHandler GetIntNumberTextBoxValidationHandler(this Window window)
        {
            return (sender, e) => {
                if (sender is not TextBox textBox)
                {
                    return;
                }

                var inpt = textBox.Text;
                Regex regex = new Regex(@"[^0-9]+");
                

                e.Handled = regex.IsMatch(e.Text);
            };
        }
    }
}