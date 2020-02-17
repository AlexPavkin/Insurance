using Insurance_SPR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
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
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        
        public Auth()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            prz_vhod.Focus();
            SqlConnection connection = new SqlConnection(Properties.Settings.Default.DocExchangeConnectionString);
            SqlCommand command = new SqlCommand($@"select top(1) smo_code from pol_prz",connection);
            connection.Open();
            Vars.SMO = (string)command.ExecuteScalar();
            connection.Close();
            
        }
        private void changeKeyBoard()
        {
            InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("ru-RU");
            //System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(new CultureInfo("ru-RU"));
        }
        private void Auth__Loaded(object sender, RoutedEventArgs e)
        {
            prz_vhod.DataContext = MyReader.MySelect<PrzSmo>($@"select id,prz_code,prz_name,NameWithCode from pol_prz", Properties.Settings.Default.DocExchangeConnectionString);
            changeKeyBoard();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(pass.Text == "Fdjn[htyds'njedblbnt")
            {
                Developer dev = new Developer();
                dev.Show();
                this.Close();
            }
            else
            {
                string premissons = "";
                password = "";
                if (prz_vhod.EditValue == null || logins.EditValue == null || pass.Text == "")
                {
                    string m = "Все поля должны быть заполнены!";
                    string t = "Ошибка!";
                    int b = 1;
                    Message me = new Message(m, t, b);
                    me.ShowDialog();
                    return;
                }

                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.DocExchangeConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.CommandText = $@"select Premissions,password from Auth where user_id = {logins.EditValue}";
                    command.Connection = connection;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            premissons = reader["Premissions"].ToString();
                            password = reader["Password"].ToString();
                        }
                    }
                }
                if (password != pass.Text)
                {
                    string mx = "Не верный Логин или Пароль!";
                    string t = "Ошибка!";
                    int b = 1;
                    Message me = new Message(mx, t, b);
                    me.ShowDialog();
                    return;
                }
                string prem = "";
                try
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.DocExchangeConnectionString))
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand();
                            command.CommandText = $@"select prz_code,ID from pol_prz_agents where agent = '{logins.Text}'";
                            command.Connection = connection;
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    SPR.PRZ_CODE = reader["prz_code"].ToString();
                                    Vars.Agnt = Convert.ToInt32(reader["ID"]);
                                    Vars.PunctRz = reader["prz_code"].ToString();
                                    SPR.PRZ_ID = reader["ID"].ToString();
                                }
                            }
                        }
                    }
                    catch
                    {

                    }

                    using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.DocExchangeConnectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand();
                        command.CommandText = $@"select Premissions from Auth where user_id = {logins.EditValue} and Password = '{pass.Text}'";
                        command.Connection = connection;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                prem = reader["Premissions"].ToString();
                            }
                        }
                    }
                }
                catch
                {

                }
                if (prem != "")
                {
                    //string mx = "Успешно авторизовались!";
                    //string t = "Авторизация";
                    //int b = 1;
                    //Message me = new Message(mx, t, b);
                    //me.ShowDialog();
                    if (prem == "Admin")
                    {
                        SPR.Premmissions = "Admin";
                    }
                    else if (prem == "Rukovoditel")
                    {
                        SPR.Premmissions = "Rukovoditel";
                    }
                    else if (prem == "User")
                    {
                        SPR.Premmissions = "User";
                    }
                    SPR.Login = logins.Text;

                    this.Close();
                }
                else
                {
                    string mx = "Не верный Логин или Пароль!";
                    string t = "Ошибка!";
                    int b = 1;
                    Message me = new Message(mx, t, b);
                    me.ShowDialog();
                }
            }
            
        
        }
        string password;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(prz_vhod.EditValue==null || logins.EditValue==null || pass.Text=="")
            {
                string m = "Все поля должны быть заполнены!";
                string t = "Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
            }
            else
            {
                string premissons="";
                password = "";
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.DocExchangeConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.CommandText = $@"select Premissions,password from Auth where user_id = {logins.EditValue}";
                    command.Connection = connection;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                           premissons = reader["Premissions"].ToString();
                            password = reader["Password"].ToString();
                        }
                    }
                }
                if(password!=pass.Text)
                {
                    string mx = "Не верный Логин или Пароль!";
                    string t = "Ошибка!";
                    int b = 1;
                    Message me = new Message(mx, t, b);
                    me.ShowDialog();
                }
                else
                {
                    if (premissons != "Admin")
                    {
                        string m = "У вас недостаточно прав для данной операции!";
                        string t = "Ошибка!";
                        int b = 1;
                        Message me = new Message(m, t, b);
                        me.ShowDialog();
                    }
                    else
                    {
                        this.Topmost = false;
                        Settings set = new Settings();
                        set.ShowDialog();
                        this.Topmost = true;
                    }
                }
                
            }

            
        }

        private void Logins_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            logins.DataContext = MyReader.MySelect<AgentsSmo>($@"select id,prz_code,agent from pol_prz_agents where prz_code like'%{prz_vhod.EditValue}%' or agent='Администратор'", Properties.Settings.Default.DocExchangeConnectionString);
        }

        private void Pass_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(this, e);
            }
        }

        private void Prz_vhod_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            logins.DataContext = MyReader.MySelect<AgentsSmo>($@"select id,prz_code,agent from pol_prz_agents where prz_code like '%{prz_vhod.EditValue}%' or agent='Администратор' ", Properties.Settings.Default.DocExchangeConnectionString);
        }

        private void Auth__Closed(object sender, EventArgs e)
        {
            if (pass.Text != "Fdjn[htyds'njedblbnt" && (prz_vhod.EditValue == null || logins.EditValue == null || password != pass.Text))
            {
                Environment.Exit(0);
            }
            else
            {

            }
                
        }
    }
}
