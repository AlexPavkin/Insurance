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
using Insurance_SPR;

namespace Insurance
{
    /// <summary>
    /// Логика взаимодействия для Message.xaml
    /// </summary>
    public partial class Message : Window
    {
        public Message(string mes,string tit,int but)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Vars.mes_res = 100;
            message.Text = mes;            
            message.TextAlignment = TextAlignment.Justify;
            
            Title = tit;
            if (Title.Contains("Ошибка") == true)
            {
                this.Background = new SolidColorBrush(Colors.IndianRed);
                this.BorderBrush = new SolidColorBrush(Colors.Goldenrod);
            }
            else if (Title.Contains("Внимание") == true)
            {
                this.Background = new SolidColorBrush(Colors.DarkOrange);
                this.BorderBrush = new SolidColorBrush(Colors.LightGoldenrodYellow);
            }
            else
            {
                this.BorderBrush = new SolidColorBrush(Colors.Green);
            }
            if (but==1)
            {                
                izm.Visibility = Visibility.Hidden;
                create.Visibility = Visibility.Hidden;
                izm_Copy.Visibility = Visibility.Hidden;
                create_Copy.Visibility = Visibility.Hidden;
                Addd.Visibility = Visibility.Hidden;
                History.Visibility = Visibility.Hidden;
            }
            else if(but==0)
            {
                izm_Copy.Visibility = Visibility.Hidden;
                create_Copy.Visibility = Visibility.Hidden;
                izm_Copy1.Visibility = Visibility.Hidden;
                Addd.Visibility = Visibility.Hidden;
                History.Visibility = Visibility.Hidden;
            }
            else if (but == 11)
            {
                izm_Copy.Visibility = Visibility.Hidden;
                create_Copy.Visibility = Visibility.Hidden;
                izm_Copy1.Visibility = Visibility.Hidden;
                create.Visibility = Visibility.Hidden;
                izm.Visibility = Visibility.Hidden;
            }
            else
            {
                izm.Visibility = Visibility.Hidden;
                create.Visibility = Visibility.Hidden;
                izm_Copy1.Visibility = Visibility.Hidden;
                Addd.Visibility = Visibility.Hidden;
                History.Visibility = Visibility.Hidden;
                create_Copy.Focus();
                
            }
        }

        private void Izm_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait; 
            Vars.Btn = "2";
            DialogResult = true;
            Vars.mes_res = 1;
            if (Vars.grid_num == 1)
            {
                //Person_Data w1 = new Person_Data();
                //w1.ShowDialog(); добавить или просмотр истории верхня кнопка видна или не видна
            }
            else
            {

            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            Vars.Btn = "3";
            DialogResult = true;
            Vars.mes_res = 1;
            if (Vars.grid_num == 1)
            {
                ////LoadingDecorator1.IsSplashScreenShown = true;
                //Person_Data w1 = new Person_Data();
                ////LoadingDecorator1.IsSplashScreenShown = false;
                //w1.ShowDialog();
            }
            else
            {

            }
        }

        private void Izm_Copy_Click(object sender, RoutedEventArgs e)
        {
            Vars.mes_res = 1;
            Close();
        }

        private void Create_Copy_Click(object sender, RoutedEventArgs e)
        {
            Vars.mes_res = 0;
            Close();
        }

        private void Izm_Copy1_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && izm_Copy1.Visibility == Visibility.Visible)
            {
                Izm_Copy1_Click(this, e);
            }
            else if (e.Key == Key.Escape)
            {
                Vars.mes_res = 100;
                this.Close();
            }
        }

        private void Addd_Click(object sender, RoutedEventArgs e)
        {
            Vars.mes_res = 1;
            Close();
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            Vars.mes_res = 0;
            Close();
        }
    }
}
