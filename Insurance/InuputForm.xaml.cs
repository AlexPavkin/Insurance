using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для InuputForm.xaml
    /// </summary>
    public partial class InuputForm : Window
    {
        public InuputForm()
        {
            InitializeComponent();
        }
     

        private void Otmena_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Insurance_SPR.SPR.S_PR = inputTEXT.Text;
            this.Close();
     
        }
    }
}
