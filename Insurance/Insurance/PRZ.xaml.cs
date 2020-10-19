using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Yamed.Server;
using System.Data.SqlClient;
using Insurance_SPR;

namespace Insurance
{
    /// <summary>
    /// Логика взаимодействия для Window6.xaml
    /// </summary>
    //public class Prz
    //{
    //    public int ID { get; set; }
    //    public string SMO_CODE { get; set; }
    //    public string PRZ_CODE { get; set; }
    //    public string PRZ_NAME { get; set; }
    //    public string NameWithCode { get; set; }
    //}

    public partial class PRZ : Window
    {
        public PRZ()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            
        }

        private void settings_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void add_btn_Click(object sender, RoutedEventArgs e)
        {
            int btn = 1;
            int id = 0;
            Prz_Insert w7 = new Prz_Insert(btn,id);
            w7.ShowDialog();
        }

        private void prz_Loaded(object sender, RoutedEventArgs e)
        {
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            var peopleList =
                    MyReader.MySelect<Prz>(
                        $@"
            SELECT  ID,SMO_CODE,PRZ_CODE,PRZ_NAME  ,NameWithCode
  FROM POL_PRZ 
 order by ID", connectionString);
            prz.ItemsSource = peopleList;
            prz.View.FocusedRowHandle = -1;
        }

        private void del_btn_Click(object sender, RoutedEventArgs e)
        {
            string sg_rows = " ";
            int[] rt = prz.GetSelectedRowHandles();
            for (int i = 0; i < rt.Count(); i++)

            {
                var ddd = prz.GetCellValue(rt[i], "ID");
                var sgr = sg_rows.Insert(sg_rows.Length, ddd.ToString()) + ",";
                sg_rows = sgr;
            }

            sg_rows = sg_rows.Substring(0, sg_rows.Length - 1);
            var result = System.Windows.MessageBox.Show("Вы действительно хотите удалить записи", "Внимание!", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            else
            {

                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand($@"delete from pol_prz where id in({sg_rows})", con);
                con.Open();
                comm.ExecuteNonQuery();
                con.Close();
                int kol_zap = rt.Count();
                int lastnumber = kol_zap % 10;
                if (lastnumber > 1 && lastnumber < 5)
                {
                    System.Windows.MessageBox.Show(" Успешно удалено  " + rt.Count() + " записи!");
                }
                else if (lastnumber == 1)
                {
                    System.Windows.MessageBox.Show(" Успешно удалена  " + rt.Count() + " запись!");
                }
                else
                {
                    System.Windows.MessageBox.Show(" Успешно удалено  " + rt.Count() + " записей!");
                }

                prz_Loaded(sender, e);
            }
        }

        private void Settings_Activated(object sender, EventArgs e)
        {
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            var peopleList =
                    MyReader.MySelect<Prz>(
                        $@"
            SELECT  ID,SMO_CODE,PRZ_CODE,PRZ_NAME  ,NameWithCode
  FROM POL_PRZ 
 order by ID", connectionString);
            prz.ItemsSource = peopleList;
            prz.View.FocusedRowHandle = -1;
        }

        private void Edit_btn_Click(object sender, RoutedEventArgs e)
        {
            int btn = 2;
            int id = Convert.ToInt32(prz.GetFocusedRowCellValue("ID"));
            Prz_Insert w7 = new Prz_Insert(btn,id);
            w7.ShowDialog();
        }
    }
    
    
}
