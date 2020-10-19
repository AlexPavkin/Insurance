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
using FastReport.Design;
using System.Data.SqlClient;
using FastReport.Utils;
using System.IO;

namespace Insurance
{
    /// <summary>
    /// Логика взаимодействия для Window9.xaml
    /// </summary>
    public partial class Doc_Edit : Window
    {        
        FastReport.Design.StandardDesigner.DesignerControl designer = new FastReport.Design.StandardDesigner.DesignerControl();
        Report report1 = new Report();
        public New_Report Sform = new New_Report();
        
        string _btn_clk;
        string _r_id;
       
        public Doc_Edit(string btn_clk,string r_id)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WindowState = WindowState.Maximized;
            designer.Report = report1;
            designer.RefreshLayout();
            wfhDesign.Child = designer;
            _btn_clk = btn_clk;
            _r_id = r_id;
            designer.Load += DesignerSettings_DesignerLoaded;

        }
        void cmdSave_CustomAction(object sender, EventArgs e)

        {

            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            if (_btn_clk == "0")
            {
                if (Sform.ShowDialog() == true)

                {
                    Stream mStream = new MemoryStream();

                    report1.Save(mStream);
                    mStream.Seek(0, SeekOrigin.Begin);
                    StreamReader reader = new StreamReader(mStream);
                    SqlCommand com = new SqlCommand($@" insert into pol_reports (name,repname,template,reptype) values('{Sform.name.Text}','{Sform.repname.Text}',@temp,1)", con);
                    com.Parameters.AddWithValue("@temp", reader.ReadToEnd());
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();

                }
            }
            else if(_btn_clk=="1")
            {

                Stream mStream = new MemoryStream();
                

                report1.Save(mStream);
                
                mStream.Seek(0, SeekOrigin.Begin);
                StreamReader reader = new StreamReader(mStream);
                SqlCommand com = new SqlCommand($@" update pol_reports set template=@temp where id={_r_id}", con);
                com.Parameters.AddWithValue("@temp", reader.ReadToEnd());
                //com.Parameters.AddWithValue("@script", report1.ScriptText);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();

                mStream.Close();
                reader.Close();
                //SqlCommand com = new SqlCommand($@" update pol_reports set template='{report1.ReportResourceString}' where id={_r_id}", con);

                //con.Open();
                //com.ExecuteNonQuery();
                //con.Close();
            }
        }
        void cmdEdit_CustomAction(object sender, EventArgs e)
        {
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
           
            SqlCommand com = new SqlCommand($@" select Template from pol_reports where id={_r_id}", con);
            //SqlCommand com1 = new SqlCommand($@" select RepScript from pol_reports where id={_r_id}", con);
            con.Open();
            string _template=(string)com.ExecuteScalar();
            // _script = (string)com1.ExecuteScalar();
            con.Close();
            report1.ReportResourceString = _template;
            //report1.ScriptText = _script;
            //report1.Design();
           Designer designer = sender as Designer;
            designer.Report = report1;
           designer.SetModified(this,"LoadData");
            

        }
        void cmdClose_CustomAction(object sender, EventArgs e)
        {
            
        }
        public void DesignerSettings_DesignerLoaded(object sender, EventArgs e)

        {
            if(_btn_clk=="1")
            {
                cmdEdit_CustomAction(sender,e);
            }

            (sender as Designer).cmdSave.CustomAction += new EventHandler(cmdSave_CustomAction);
            //(sender as Designer).cmdSaveAs.CustomAction += new EventHandler(cmdSave_CustomAction);
            //(sender as Designer).cmdClose.CustomAction += new EventHandler(cmdClose_CustomAction);
            //(sender as Designer).cmdEdit.CustomAction += new EventHandler(cmdEdit_CustomAction);

            //(sender as Designer).cmdOpen.CustomAction += new EventHandler(cmdOpen_CustomAction);

        }
       
        private void Designer_Load(object sender, RoutedEventArgs e)
        {
            (sender as Designer).Load += DesignerSettings_DesignerLoaded;

            //report1.Design();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            designer.Dispose();
        }
    }
}
