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
using System.Data.SqlClient;

namespace Insurance
{
    /// <summary>
    /// Логика взаимодействия для Window11.xaml
    /// </summary>
    public class Reports
    {

        public Int32 ID { get; set; }
        [System.ComponentModel.DisplayName("Наименование отчета")]
        public string Name { get; set; }
        [System.ComponentModel.DisplayName("Имя в базе")]
        public string RepName { get; set; }


    }
    public partial class Docs : Window
    {
        
        public Docs()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            reports_grid.View.FocusedRowHandle = -1;
            
        }

        private void reports_Loaded(object sender, RoutedEventArgs e)
        {
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            var reportsList =
                    MyReader.MySelect<Reports>(
                        $@"
            SELECT ID,Name,RepName 
  FROM [dbo].[POL_REPORTS]
 order by ID", connectionString);
            reports_grid.ItemsSource = reportsList;
            reports_grid.SelectedItem = -1;
            reports_grid.View.FocusedRowHandle = -1;
            reports_grid.Columns[1].Width = 300;
        }
        

        private void add_btn_Click(object sender, RoutedEventArgs e)
        {
            string r_id = "0";
            string btn_clk = "0";
            Doc_Edit w9 = new Doc_Edit(btn_clk, r_id);
            w9.Show();
        }

        private void edit_btn_Click(object sender, RoutedEventArgs e)
        {
            
            if (reports_grid.View.FocusedRowHandle >-1)
            {
                string r_id = reports_grid.GetFocusedRowCellValue("ID").ToString();
                string btn_clk = "1";
                Doc_Edit w9 = new Doc_Edit(btn_clk, r_id);
                w9.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите отчет для редактирования");
                return;
            }
        }

        private void Del_btn_Click(object sender, RoutedEventArgs e)
        {
            string sg_rows = " ";
            int[] rt = reports_grid.GetSelectedRowHandles();
            for (int i = 0; i < rt.Count(); i++)

            {
                var ddd = reports_grid.GetCellValue(rt[i], "ID");
                var sgr = sg_rows.Insert(sg_rows.Length, ddd.ToString()) + ",";
                sg_rows = sgr;
            }

            sg_rows = sg_rows.Substring(0, sg_rows.Length - 1);
            var result = MessageBox.Show("Вы действительно хотите удалить записи", "Внимание!", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            else
            {
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand comm = new SqlCommand($@"delete from pol_reports where id in({sg_rows})", con);
                con.Open();
                comm.ExecuteNonQuery();
                con.Close();

                reports_Loaded(sender, e);
            }
        }

        
    }
}
