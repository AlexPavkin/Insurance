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

namespace Insurance
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PRZ _pRZ = new PRZ();
            _pRZ.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SettGLOB set = new SettGLOB();
            set.ShowDialog();
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {

            Agents agents = new Agents();
            agents.ShowDialog();
        }

        private void SettingsWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Insurance_SPR.SPR.Premmissions == "User")
            {
                agents.Visibility = Visibility.Hidden;
                prz.Visibility = Visibility.Hidden;
            }
            else
            {
                agents.Visibility = Visibility.Visible;
                prz.Visibility = Visibility.Visible;
            }
        }

        private void Hollydays_btn_Click(object sender, RoutedEventArgs e)
        {
            Holidays h = new Holidays();
            h.ShowDialog();
        }
    }
}
