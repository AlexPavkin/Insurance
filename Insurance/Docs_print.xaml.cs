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
    /// Логика взаимодействия для Docs_print.xaml
    /// </summary>
    public partial class Docs_print : Window
    {
        public Docs_print()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Vars.IdRep = Convert.ToInt32(docs_prn.GetFocusedRowCellValue("ID"));
            Vars.RepName = docs_prn.GetFocusedRowCellValue("Name").ToString();
            ReportsPreview rep = new ReportsPreview();
            rep.ShowDialog();
        }

        private void Docs_prn_Loaded(object sender, RoutedEventArgs e)
        {
            if (Vars.Forms != 1)
            {
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                var reportsList =
                        MyReader.MySelect<Reports>(
                            $@"
            SELECT ID,Name,RepName 
  FROM [dbo].[POL_REPORTS]
 order by ID", connectionString);
                docs_prn.ItemsSource = reportsList;

            }
            else
            {
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                var reportsList =
                        MyReader.MySelect<Reports>(
                            $@"
            SELECT ID,Name,RepName 
  FROM [dbo].[POL_REPORTS] where RepType={Vars.Forms}
 order by ID", connectionString);
                docs_prn.ItemsSource = reportsList;
                Vars.Forms = 0;
            }
            
            docs_prn.SelectedItem = -1;
            docs_prn.View.FocusedRowHandle = -1;
            docs_prn.Columns[2].Visible = false;
            docs_prn.Columns[1].Width = 500;
        }
    }
}
