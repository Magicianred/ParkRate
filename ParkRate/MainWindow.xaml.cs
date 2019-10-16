using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ParkRate.ViewModel;

namespace ParkRate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ValidateContent(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9\.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SelectOnFocus(object sender, RoutedEventArgs e)
        {
            var textBox = ((TextBox)sender);
            textBox.SelectionStart = 0;
            textBox.SelectionLength = textBox.Text.Length;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ArrivalTimeTxt.Focus();
            ArrivalTimeTxt.CaretIndex = ArrivalTimeTxt.Text.Length;

            const string configFilePath = @"config.xml";
            ParkRateViewModel viewModel = (ParkRateViewModel) DataContext;
            viewModel.ConfigFilePath = configFilePath;
            if (File.Exists(configFilePath))
            {
                viewModel.UpdateWithConfig(ParkRateConfig.FromXml(viewModel.ConfigFilePath));
            }
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
