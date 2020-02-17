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
using System.IO;
using System.Data.OleDb;
using FastReport.Export;
using Insurance_SPR;

namespace Insurance
{
    /// <summary>
    /// Логика взаимодействия для ReportsPreview.xaml
    /// </summary>
    public partial class ReportsPreview : Window
    {
        FastReport.Preview.PreviewControl prew = new FastReport.Preview.PreviewControl();
        Report report1 = new Report();
        public New_Report Sform = new New_Report();
        

        public ReportsPreview()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            
            if(Vars.RepName.Contains("Заявление")==true)
            {
                SqlCommand com = new SqlCommand($@" select Template from pol_reports where id={Vars.IdRep}", con);
                con.Open();
                string rep = (string)com.ExecuteScalar();
                con.Close();


                //SqlCommand com1 = new SqlCommand($@"select body from Prepared_Reports where id=5", con);

                //con.Open();
                //byte[] bytes = (byte[])com1.ExecuteScalar();
                //con.Close();

                //report1.ReportResourceString = Convert.ToBase64String(bytes);
                report1.ReportResourceString = rep;
                report1.Dictionary.Connections[1].ConnectionString = Properties.Settings.Default.DocExchangeConnectionString;
                report1.Dictionary.Connections[0].ConnectionString = Properties.Settings.Default.FIASConnectionString;
                report1.SetParameterValue("@id", Vars.IdZl);
                ////report1.SetParameterValue("con", Properties.Settings.Default.DocExchangeConnectionString);
                ////report1.SetParameterValue("con1", Properties.Settings.Default.DocExchangeConnectionString);
                report1.Preview = prew;


                report1.Prepare();
                
                report1.ShowPrepared();
                
                RepPrev.Child = prew;

                //MemoryStream stream = new MemoryStream();
                
                //report1.Export(rt,stream);
                ////report1.Save(stream);
                //byte[] bytes = stream.ToArray();
                ////using (var stream1 = new MemoryStream())
                ////{


                ////        stream1.Write(bytes, 0, (int)stream.Length);
                ////        stream1.Position = 0;


                //SqlCommand com1 = new SqlCommand($@"insert into Prepared_Reports (Body) values(@body)", con);
                //com1.Parameters.AddWithValue("@body", OleDbType.VarBinary).Value = bytes;
                //con.Open();
                //com1.ExecuteNonQuery();
                //con.Close();


                //}


            }
            else if(Vars.RepName.Contains("временного")==true )
            {
                SqlCommand com = new SqlCommand($@" select Template from pol_reports where id={Vars.IdRep}", con);
                con.Open();
                string rep = (string)com.ExecuteScalar();
                con.Close();

                report1.ReportResourceString = rep;
                report1.Dictionary.Connections[0].ConnectionString = Properties.Settings.Default.DocExchangeConnectionString;
                //report1.Dictionary.Connections[0].ConnectionString = Properties.Settings.Default.FIASConnectionString;
                report1.SetParameterValue("@id", Vars.IdZl);
                //report1.SetParameterValue("con", Properties.Settings.Default.DocExchangeConnectionString);
                //report1.SetParameterValue("con1", Properties.Settings.Default.DocExchangeConnectionString);
                report1.Preview = prew;


                report1.Prepare();


                report1.ShowPrepared();

                RepPrev.Child = prew;
            }
            else
            {
                SqlCommand com = new SqlCommand($@"select Template from pol_reports where id={Vars.IdRep}", con);
                con.Open();
                string rep = (string)com.ExecuteScalar();
                con.Close();

                report1.ReportResourceString = rep;
                report1.Dictionary.Connections[0].ConnectionString = Properties.Settings.Default.DocExchangeConnectionString;
                //report1.Dictionary.Connections[0].ConnectionString = Properties.Settings.Default.FIASConnectionString;
                //report1.SetParameterValue("@id", _idpers);
                //report1.SetParameterValue("con", Properties.Settings.Default.DocExchangeConnectionString);
                //report1.SetParameterValue("con1", Properties.Settings.Default.DocExchangeConnectionString);
                report1.Preview = prew;


                report1.Prepare();


                report1.ShowPrepared();

                RepPrev.Child = prew;
            }
            
        }
    }
}
