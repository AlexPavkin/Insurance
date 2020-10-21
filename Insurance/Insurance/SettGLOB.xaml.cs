using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace Insurance
{
    /// <summary>
    /// Логика взаимодействия для SettGLOB.xaml
    /// </summary>
    public partial class SettGLOB : Window
    {
        public SettGLOB()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Wigruzka_tfoms_Checked(object sender, RoutedEventArgs e)
        {
            if (Wigruzka_tfoms.IsChecked == true)
            {

                Insurance_SPR.SPR.PATH_VIGRUZKA = FolderDialog();
                Insurance_SPR.SPR.Avto_vigruzka = true;
                Insurance_SPR.SPR.Avto_vigruzka_priznak = true;
            }
            else
            {
                Insurance_SPR.SPR.Avto_vigruzka = false;
                Insurance_SPR.SPR.Avto_vigruzka_priznak = false;
            }
            
        }

        public string FolderDialog()
        {
            string FilePath="";
            FolderBrowserDialog saveFileDialog = new FolderBrowserDialog();
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
             FilePath = saveFileDialog.SelectedPath;
                
            }
            return FilePath;
        }
    }
}
