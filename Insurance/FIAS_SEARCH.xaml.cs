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
    /// Логика взаимодействия для FIAS_SEARCH.xaml
    /// </summary>
    public partial class FIAS_SEARCH : Window
    {

        public class Houses
        {
            [System.ComponentModel.DisplayName("Индекс")]
            public string POSTALCODE { get; set; }
            [System.ComponentModel.DisplayName("ОКАТО")]
            public string OKATO { get; set; }
            [System.ComponentModel.DisplayName("ОКТМО")]
            public string OKTMO { get; set; }
            [System.ComponentModel.DisplayName("Номер дома")]
            public string HOUSENUM { get; set; }
        }


        public FIAS_SEARCH()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string mguid="";
            if (fias2.reg_ul.EditValue == null && fias2.DopUlBoxEdit.EditValue==null)
            {
                if (fias2.reg_np.EditValue == null)
                {
                    mguid = fias2.reg_town.EditValue.ToString();
                }
               
              else
              {
                  mguid = fias2.reg_np.EditValue.ToString();
              }
            }
            else if(fias2.reg_ul.EditValue == null && fias2.DopUlBoxEdit.EditValue != null)
            {
                mguid = fias2.DopUlBoxEdit.EditValue.ToString();
            }
            else if (fias2.reg_ul.EditValue != null && fias2.DopUlBoxEdit.EditValue != null)
            {
                mguid = fias2.DopUlBoxEdit.EditValue.ToString();
            }

            else
            {
                mguid = fias2.reg_ul.EditValue.ToString();
            }

            var connectionString = Properties.Settings.Default.FIASConnectionString;
            var housesList =
                MyReader.MySelect<Houses>(
                    $@"
            SELECT
      [POSTALCODE]
      ,[OKATO]
      ,[OKTMO]
      ,[HOUSENUM]
  FROM [FIAS].[dbo].[Houses] where AOGUID = '{mguid}' and ENDDATE>getdate()", connectionString);
            houses_grid.ItemsSource =housesList;
            houses_grid.View.FocusedRowHandle = -1;

        }
    }
}
