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
    /// Логика взаимодействия для Window7.xaml
    /// </summary>
    public partial class Prz_Insert : Window
    {
        private int btn_;
        private int id_;
        public Prz_Insert(int btn,int id)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            btn_ = btn;
            id_ = id;
            if(btn==1)
            {
                Title = "Добавление пункта выдачи полисов";
            }
            else
            {
                Title = "Редактирование пункта выдачи полисов";
            }
        }

        private void zap_btn_Click(object sender, RoutedEventArgs e)
        {
            if (btn_==1)
            {
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand comm = new SqlCommand($@"Insert into pol_prz (smo_code,prz_code,prz_name) values('{codsmo.Text}','{codprz.Text}','{nameprz.Text}')", con);
                con.Open();
                comm.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand comm = new SqlCommand($@"update pol_prz set smo_code='{codsmo.Text}',prz_code='{codprz.Text}',prz_name='{nameprz.Text}' where id={id_}", con);
                con.Open();
                comm.ExecuteNonQuery();
                con.Close();
            }
            
            this.Close();
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand($@"select*from pol_prz  where id={id_}", con);
            con.Open();
            SqlDataReader reader = comm.ExecuteReader();

            while (reader.Read()) // построчно считываем данные
            {
                object smo_ = reader["SMO_CODE"];
                object prz_ = reader["PRZ_CODE"];
                object name_ = reader["PRZ_NAME"];

                codsmo.Text = smo_.ToString();
                codprz.Text = prz_.ToString();
                nameprz.Text = name_.ToString();
            }
            reader.Close();
            con.Close();
        }
    }
}
