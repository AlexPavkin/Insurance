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
using System.Data.SqlClient;

namespace Insurance
{
    /// <summary>
    /// Логика взаимодействия для Window3.xaml
    /// </summary>
    public partial class Blanks : Window
    {
        public Blanks()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void zagr_Click(object sender, RoutedEventArgs e)
        {
            if (ser.Text == "" || nnum.Text == "" || knum .Text== "")
            {
                MessageBox.Show("Ведите серию, начальный и конечный номер бланка!");
            }
            else {
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand comm = new SqlCommand(@"declare @i int set @i = @nach
while @i <= @kon
begin
  insert into pol_polises(vpolis,spolis,npolis,blank) values(2,@sp,@i,1)
 set @i = @i + 1
end", con);
                comm.Parameters.AddWithValue("@sp", ser.Text);
                comm.Parameters.AddWithValue("@nach", nnum.Text);
                comm.Parameters.AddWithValue("@kon", knum.Text);
                con.Open();
                comm.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Бланки успешно загружены!");
                this.Close();
            }
        }
    }
}
