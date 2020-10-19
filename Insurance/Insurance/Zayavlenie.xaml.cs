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
using FastReport;
using System.Data.SqlClient;

namespace Insurance
{
    /// <summary>
    /// Логика взаимодействия для Window12.xaml
    /// </summary>
    public partial class Zayavlenie : Window
    {
        FastReport.Preview.PreviewControl prew = new FastReport.Preview.PreviewControl();
        Report report1 = new Report();
        public New_Report Sform = new New_Report();
        string _btn_clk;
        string _r_id;
        int _idpers;
        public Zayavlenie(int idpers)
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            //report.Load(@"J:\Program Files (x86)\FastReports\FastReport.Net\Demos\Reports\Text.frx");
            _idpers = idpers;
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);


            SqlCommand com = new SqlCommand($@" select Template from pol_reports where id=1", con);
            con.Open();
            string rep = (string)com.ExecuteScalar();
            con.Close();
            
            report1.ReportResourceString = rep;
            report1.Dictionary.Connections[1].ConnectionString = Properties.Settings.Default.DocExchangeConnectionString;
            report1.Dictionary.Connections[0].ConnectionString = Properties.Settings.Default.FIASConnectionString;
            report1.SetParameterValue("@id", _idpers);
            //report1.SetParameterValue("con", Properties.Settings.Default.DocExchangeConnectionString);
            //report1.SetParameterValue("con1", Properties.Settings.Default.DocExchangeConnectionString);
            report1.Preview = prew;

            
            report1.Prepare();
            

            report1.ShowPrepared();
            
            wfhShow.Child = prew;
        }
    }
}
