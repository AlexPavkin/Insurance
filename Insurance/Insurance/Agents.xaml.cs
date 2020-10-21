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

namespace Insurance
{
    /// <summary>
    /// Логика взаимодействия для Agents.xaml
    /// </summary>
    public partial class Agents : Window
    {
        public Agents()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Insert_agent_Click(object sender, RoutedEventArgs e)
        {
            int btn = 1;
            int id = 0;
            Agent_insert agi = new Agent_insert(btn,id);
            agi.ShowDialog();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            var peopleList =
                    MyReader.MySelect<AgentsSmo>(
                        $@"
            select * from pol_prz_agents
 order by ID", connectionString);
            agents_grid.ItemsSource = peopleList;
            agents_grid.View.FocusedRowHandle = -1;
            agents_grid.SelectedItem = -1;
        }

        private void Del_agent_Click(object sender, RoutedEventArgs e)
        {
            string sg_rows = " ";
            int[] rt = agents_grid.GetSelectedRowHandles();
            for (int i = 0; i < rt.Count(); i++)

            {
                var ddd = agents_grid.GetCellValue(rt[i], "ID");
                var sgr = sg_rows.Insert(sg_rows.Length, ddd.ToString()) + ",";
                sg_rows = sgr;
            }

            sg_rows = sg_rows.Substring(0, sg_rows.Length - 1);
            
            if (agents_grid.SelectedItems.Count==0)
            {
                MessageBox.Show("Выберите агента для удаления!");
            }
            else
            {
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand comm = new SqlCommand($@"delete from pol_prz_agents where id in ({sg_rows})
 delete from auth where user_id in ({sg_rows})", con);
                con.Open();
                comm.ExecuteNonQuery();
                con.Close();
            }
            Window_Activated(this,e);
        }

        private void Edit_btn_Click(object sender, RoutedEventArgs e)
        {
            int btn = 2;
            int id = Convert.ToInt32(agents_grid.GetFocusedRowCellValue("ID"));
            Agent_insert agi = new Agent_insert(btn, id);
            agi.ShowDialog();
        }
    }
}
