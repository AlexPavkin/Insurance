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
using Insurance_SPR;
using System.Collections.ObjectModel;
using System.Collections;

namespace Insurance
{
    /// <summary>
    /// Логика взаимодействия для Agent_insert.xaml
    /// </summary>
    //public class AgentsSmo
    //{
    //    public int ID { get; set; }
    //    public string PRZ_CODE { get; set; }
    //    public string AGENT { get; set; }

    //}
    //public class PrzSmo
    //{
    //    public int ID { get; set; }
    //    public string PRZ_CODE { get; set; }
    //    public string PRZ_NAME { get; set; }
    //    public string NameWithCode { get; set; }

    //}
    public partial class Agent_insert : Window
    {
        private int btn_;
        private int id_;

        public Agent_insert(int btn, int id)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            prz_combo.DataContext = MyReader.MySelect<PrzSmo>(@"select id,PRZ_NAME,prz_code,NameWithCode from pol_prz",
                Properties.Settings.Default.DocExchangeConnectionString);
            
            //prz_combo.SelectedIndex = -1;
            btn_ = btn;
            id_ = id;
            if (btn == 1)
                Title = "Добавить агента";
            else
                Title = "Редактировать агента";
        }
        public ObservableCollection<string> list = new ObservableCollection<string>();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            //object agent_ = null;
           var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            
            Premiss_edt.DataContext = MyReader.MySelect<PremissSmo>($@"select Premissions from Premissionses", connectionString);
            if (btn_==1)
            {

            }
            else if(btn_==2)
            {
                var con1 = new SqlConnection(connectionString);
                var comm1 = new SqlCommand($@"SELECT pa.PRZ_CODE,at.password,at.premissions,pa.agent FROM POL_PRZ_AGENTS pa
left join auth at on pa.id=at.user_id 
where pa.id={id_}", con1);
                con1.Open();
                var reader1 = comm1.ExecuteReader();

                while (reader1.Read()) // построчно считываем данные
                {
                    object PRZ_CODE = reader1["PRZ_CODE"];
                    object Pass = reader1["Password"];
                    object Prem = reader1["Premissions"];
                    object Agnt = reader1["AGENT"];

                    string pr = PRZ_CODE.ToString();
                    prz_combo.EditValue = pr.Split(',');
                    agent_tbx.Text = Agnt.ToString();
                    pass.Text = Pass.ToString();
                    Premiss_edt.Text = Prem.ToString();
                }

                reader1.Close();
                con1.Close();
            }
            //List<string> zap =new List<string>();
            //zap.Clear();
            //try
            //{
            //    var con1 = new SqlConnection(connectionString);
            //    var comm1 = new SqlCommand($@"SELECT * FROM POL_PRZ", con1);
            //    con1.Open();
            //    var reader1 = comm1.ExecuteReader();

            //    while (reader1.Read()) // построчно считываем данные
            //    {
            //        var PRZ_CODE = reader1["PRZ_CODE"];
            //        var NameWithCode1 = reader1["NameWithCode"];
            //      zap.Add(NameWithCode1.ToString());
            //        //if (prem.ToString() == "Admin")
            //        //   {
            //        //       Premiss_edt.Text = "Admin";
            //        //   }
            //        //else if (prem.ToString() == "User")
            //        //   {
            //        //       Premiss_edt.Text = "User";
            //        //   }
            //        //   else 
            //        //   {
            //        //       Premiss_edt.Text = "123";
            //        //   }
            //    }

            //    reader1.Close();
            //    con1.Close();
            //}
            //catch
            //{
            //}

            //prz_combo.ItemsSource = zap;

            //var con = new SqlConnection(connectionString);
            //var comm = new SqlCommand($@"select*from pol_prz_agents  where id={id_}", con);
            //con.Open();
            //var reader = comm.ExecuteReader();

            //while (reader.Read()) // построчно считываем данные
            //{
            //    object prz_ = reader["PRZ_CODE"];
            //    agent_ = reader["AGENT"];
            //    prz_combo.EditValue = prz_.ToString().Split(',');
            //    agent_tbx.Text = agent_.ToString();
            //}

            //reader.Close();
            //con.Close();

            //try
            //{
            //    var con1 = new SqlConnection(connectionString);
            //    var comm1 = new SqlCommand($@"SELECT * FROM [Auth] where Login='{agent_.ToString()}'", con1);
            //    con1.Open();
            //    var reader1 = comm1.ExecuteReader();

            //    while (reader1.Read()) // построчно считываем данные
            //    {
            //        var passw = reader1["Password"];
            //        var prem = reader1["Premissions"];
            //        pass.Text = passw.ToString();
            //        //if (prem.ToString() == "Admin")
            //        //   {
            //        //       Premiss_edt.Text = "Admin";
            //        //   }
            //        //else if (prem.ToString() == "User")
            //        //   {
            //        //       Premiss_edt.Text = "User";
            //        //   }
            //        //   else 
            //        //   {
            //        //       Premiss_edt.Text = "123";
            //        //   }
            //    }

            //    reader1.Close();
            //    con1.Close();
            //}
            //catch
            //{
            //}

            //Premiss_edt.DataContext = MyReader.MySelect<PremissSmo>(@"select * from Premissionses",
            //    Properties.Settings.Default.DocExchangeConnectionString);

            //Premiss_edt.SelectedIndex = 0;
            //prz_combo.SelectedIndex = -1;
        }

        private void Cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save_btn_Click(object sender, RoutedEventArgs e)
        {
            if (btn_ == 1)
            {
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                var con = new SqlConnection(connectionString);
                var comm =
                    new SqlCommand($@"insert into pol_prz_agents (prz_code,agent) values('{p}','{agent_tbx.Text}')
                                        select id from pol_prz_agents where id=SCOPE_IDENTITY()",
                        con);
                con.Open();
                int user= (int)comm.ExecuteScalar();
                con.Close();


                var con1 = new SqlConnection(connectionString);
                var comm1 =
                    new SqlCommand(
                        $@"insert into Auth (Login,Password,Premissions,user_id) values('{agent_tbx.Text}','{pass.Text}','{Premiss_edt.Text}',{user})",
                        con1);
                con1.Open();
                comm1.ExecuteNonQuery();
                con1.Close();
            }
            else
            {
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                var con = new SqlConnection(connectionString);
                var comm =
                    new SqlCommand(
                        $@"update pol_prz_agents set prz_code='{p}',agent='{agent_tbx.Text}' where id={id_}",
                        con);
                con.Open();
                comm.ExecuteNonQuery();
                con.Close();


                var con1 = new SqlConnection(connectionString);
                var comm1 =
                    new SqlCommand(
                        $@" 
if exists(select user_id from auth where user_id={id_})
update Auth set Login='{agent_tbx.Text}',Password='{pass.Text}',Premissions='{Premiss_edt.Text}' where user_id={id_}
 else
insert into auth(login,password,premissions,user_id) values('{agent_tbx.Text}','{pass.Text}','{Premiss_edt.Text}',{id_})",
                        con1);
                con1.Open();
                comm1.ExecuteNonQuery();
                con1.Close();
                p = "";
            }

            Close();
        }

        private string p;


        private void Prz_combo_SelectedIndexChanged_1(object sender, RoutedEventArgs e)
        {
            //p = "";
            //var ppp = prz_combo.EditValue;
            //var dst = prz_combo.Text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            //var i = dst.Count();

            //for (i = 0; i < dst.Count(); i++) p += dst[i].Substring(0, 3) + ",";

            //if (p.Length == 4)
            //    p = p.Replace(",", "");
            //else
            //    try
            //    {
            //        p = p.Substring(0, p.Length - 1);
            //    }
            //    catch
            //    {
            //    }
        }

        private void Prz_combo_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            string p1 = "";
            if (prz_combo.EditValue == null)
            {

            }
            else
            {
                var d1 = (ICollection)prz_combo.EditValue;
                string[] dd1 = new string[d1.Count];
                d1.CopyTo(dd1, 0);
                for (int i = 0; i < dd1.Count(); i++)
                {
                    p1 += (dd1[i] + ";");
                    p = p1.Substring(0, p1.Length - 1);
                }
            }
        }
    }
}