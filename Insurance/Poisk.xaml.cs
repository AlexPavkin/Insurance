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
using Yamed.Server;
using Insurance_SPR;

namespace Insurance
{
    /// <summary>
    /// Логика взаимодействия для Window4.xaml
    /// </summary>
    public partial class Poisk : Window
    {
        public Poisk()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            fam.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var connectionString = Properties.Settings.Default.srz_miniConnectionString;
            var peopleList =
                MyReader.MySelect<People1>(
                    $@"
            SELECT NPOL,ENP,FAM , IM  , OT ,W ,DR, RNNAME,CITY, NP, UL, DOM, KOR, KV, Q  
  FROM [dbo].[PEOPLE]
where FAM LIKE '{
                     fam.Text}%' and  IM LIKE '{
                     im.Text}%' and  OT LIKE '{
                     ot.Text}%' and  convert(nvarchar,dr,104) like left('{
                     dr.EditValue}%',10) order by ID", connectionString);
            poisk_grid.ItemsSource = peopleList;
            poisk_grid.View.FocusedRowHandle = -1;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            fam.Text = "";
            im.Text = "";
            ot.Text = "";
            dr.EditValue = null;
        }
    }
    
}
