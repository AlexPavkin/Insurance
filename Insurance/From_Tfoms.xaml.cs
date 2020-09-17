using Microsoft.Win32;
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
using System.Xml;
using System.Xml.Linq;
using Insurance_SPR;
using System.Collections.ObjectModel;
using System.IO;

namespace Insurance
{
    /// <summary>
    /// Логика взаимодействия для Window5.xaml
    /// </summary>
    public partial class From_Tfoms : Window
    {
        public From_Tfoms()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if(Vars.SMO!="46004" && Vars.SMO != "39001" )
            {
                attache.IsEnabled =false;
                attache_to_foms.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog OPF = new OpenFileDialog();

            OPF.Multiselect = true;
            bool res = OPF.ShowDialog().Value;
            string[] files = OPF.FileNames;
            string[] shfiles = OPF.SafeFileNames;
            
            int y = 0;
            if (res == true)
            {
                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(file);
                    string rfile = shfiles[y].Replace("P","i");
                    if(rfile.Length>22)
                    {
                        rfile = rfile.Substring(0, 18) + ".xml";
                    }
                    

                    //var test = xDoc.Descendants("Companies").Elements("Company").Select(r => r.Value).ToArray();
                    // получим корневой элемент
                    XmlElement xRoot = xDoc.DocumentElement;
                    if (xRoot.Attributes.GetNamedItem("COMMENT") != null)
                    {
                        var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                        SqlConnection con = new SqlConnection(connectionString);
                        SqlCommand com = new SqlCommand($@"update pol_persons 
 set comment='{xRoot.Attributes.GetNamedItem("COMMENT").Value.Replace((char)39, (char)32)}' 
 where event_guid in (select event_guid from pol_oplist where filename='{rfile}') 
 update pol_events set unload=0 where 
 idguid in (select event_guid from pol_oplist where filename='{rfile}')
 update pol_unload_history set comment ='{xRoot.Attributes.GetNamedItem("COMMENT").Value.Replace((char)39, (char)32)}'
 where  fname='{rfile}'", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                    }
                    else
                    {
                        var xx = xDoc.ChildNodes[1].ChildNodes[0].Attributes.GetNamedItem("N_REC").Value.ToString();
                        if (xx.Length < 36)
                        {
                            goto forward;
                        }
                        var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                            SqlConnection con = new SqlConnection(connectionString);
                            SqlCommand com = new SqlCommand( $@"exec [Load_flk] @xml = '{xDoc.LastChild.OuterXml.Replace((char)39, (char)32)}', @fname='{rfile}'", con);
                                con.Open();
                                com.ExecuteNonQuery();
                                con.Close();                                
                            
                    }
                    forward:
                    y = y + 1;
                }
                string m = "ЕНП и ошибки ФЛК успешно загружены";
                string t = "Сообщение";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                
                
            }


            //            System.IO.File.WriteAllText("c:\\elmedicine\\bin2018\\2.vbs", "Option Explicit \n"+
            //                "Dim FSO, Folder\n"+
            //                "Set FSO = CreateObject(" +(char)34+"Scripting.FileSystemObject"+(char)34+")\n"+
            //                "Folder = FSO.GetAbsolutePathName("+ (char)34 + (char)46 + (char)92 + "Yamed.exe"+ (char)34 +")\n"+

            //"dim WSHShell\n"+
            //"Set WSHShell = WScript.CreateObject("+ (char)34 + "WScript.Shell"+ (char)34 + ")\n"+
            //"WSHShell.Exec Folder\n"+
            //"WScript.Sleep 5000\n"+

            //"Dim objWShell\n"+
            //"Set objWShell = CreateObject("+ (char)34 + "WScript.Shell"+(char)34+")\n"+
            //"objWShell.AppActivate "+ (char)34 + "Авторизация" + (char)34 +", True"+"\n"+
            //"objWShell.SendKeys "+ (char)34 + (char)123 + "TAB"+ (char)125 + (char)34 + ", True" +"\n"+
            //"objWShell.SendKeys " + (char)34 + (char)123 + "DOWN 2"+ (char)125+ (char)34 + ", True" +"\n"+

            //"objWShell.SendKeys " + (char)34 + (char)123 + "TAB" + (char)125 + (char)34 + ", True" +"\n"+
            //"objWShell.SendKeys "+ (char)34 + "1"+ (char)34 +", True"+"\n"+
            //"objWShell.SendKeys " + (char)34 + (char)123 + "ENTER" + (char)125 + (char)34 + ", True" +"\n"+
            //"Set objWShell = Nothing\n"+
            //"WScript.Sleep 5000\n"+
            //"Set objWShell = CreateObject(" + (char)34 + "WScript.Shell"+ (char)34 +")\n"+
            //"objWShell.AppActivate " + (char)34 + "ЯМед.Электронная Медицина" + (char)34 +", True"+"\n"+
            //"objWShell.SendKeys " + (char)34 + (char)123 + "TAB" + (char)125 + (char)34 + ", True" +"\n"+
            //"objWShell.SendKeys " + (char)34 + (char)123 + "DOWN" + (char)125 + (char)34 + ", True" +"\n"+
            //"objWShell.SendKeys " + (char)34 + (char)123 + "RIGHT" + (char)125 + (char)34 + ", True");
        }

        private void flk_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OPF = new OpenFileDialog();

            OPF.Multiselect = false;
            bool res = OPF.ShowDialog().Value;
            string file = OPF.FileName;
            List<string> xml_comment = new List<string>();
            List<string> xml_enp = new List<string>();
            List<string> xml_spol = new List<string>();
            List<string> xml_npol = new List<string>();
            List<string> xml_nrec = new List<string>();

            if (res == true)
            {
               
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(file);
                    //var test = xDoc.Descendants("Companies").Elements("Company").Select(r => r.Value).ToArray();
                    // получим корневой элемент
                    XmlElement xRoot = xDoc.DocumentElement;
                    // обход всех узлов в корневом элементе
                    foreach (XmlNode xnode in xRoot)
                    {
                        if (xnode.Name == "REP")
                        {
                            xml_comment.Add(xnode.Attributes.GetNamedItem("COMMENT").Value.Replace((char)39, ' '));
                            xml_nrec.Add(xnode.Attributes.GetNamedItem("N_REC").Value);

                        }
                        // обходим все дочерние узлы элемента user
                        foreach (XmlNode childnode in xnode.ChildNodes)
                        {
                            //for (i = 0; i < xRoot.ChildNodes.Count-1; i++)
                            //{

                            if (childnode.Name == "INSURANCE")
                            {
                                if (childnode.Attributes.GetNamedItem("ENP").Value == "")
                                {
                                    xml_enp.Add("");
                                }
                                else
                                {
                                    xml_enp.Add(childnode.Attributes.GetNamedItem("ENP").Value);
                                }

                                //MessageBox.Show(xml_enp.ToString());
                                if (childnode.FirstChild.Name == "POLIS")
                                {
                                    if (childnode.FirstChild.Attributes.GetNamedItem("NPOLIS").Value == "")
                                    {
                                        xml_spol.Add("");
                                        xml_npol.Add("");
                                    }

                                    else
                                    {
                                        xml_spol.Add(childnode.FirstChild.Attributes.GetNamedItem("NPOLIS").Value.Substring(0, 3));
                                        xml_npol.Add(childnode.FirstChild.Attributes.GetNamedItem("NPOLIS").Value.Substring(3, 6));
                                    }

                                }


                            }
                            // если узел age
                            //if (childnode.Name == "age")
                            //{
                            //    Console.WriteLine("Возраст: {0}", childnode.InnerText);
                            //}
                            //}
                        }
                        //Console.WriteLine();

                    }
                    //Console.Read();




                    int i = 0;
                    for (i = 0; i <= xml_enp.Count - 1; i++)
                    {
                        var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                        SqlConnection con = new SqlConnection(connectionString);
                        SqlCommand com = new SqlCommand($@" update pol_persons set comment='{xml_comment[i]}' 
where --=(select person_guid from pol_polises where spolis='{xml_spol[i]}' and npolis='{xml_npol[i]}')
 event_guid='{xml_nrec[i]}'", con);
                        con.Open();

                        com.ExecuteNonQuery();


                        con.Close();
                    }
                string m = "Комментарии ФЛК успешно загружены!";
                string t = "Сообщение";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                return;
                
            }


            //            System.IO.File.WriteAllText("c:\\elmedicine\\bin2018\\2.vbs", "Option Explicit \n"+
            //                "Dim FSO, Folder\n"+
            //                "Set FSO = CreateObject(" +(char)34+"Scripting.FileSystemObject"+(char)34+")\n"+
            //                "Folder = FSO.GetAbsolutePathName("+ (char)34 + (char)46 + (char)92 + "Yamed.exe"+ (char)34 +")\n"+

            //"dim WSHShell\n"+
            //"Set WSHShell = WScript.CreateObject("+ (char)34 + "WScript.Shell"+ (char)34 + ")\n"+
            //"WSHShell.Exec Folder\n"+
            //"WScript.Sleep 5000\n"+

            //"Dim objWShell\n"+
            //"Set objWShell = CreateObject("+ (char)34 + "WScript.Shell"+(char)34+")\n"+
            //"objWShell.AppActivate "+ (char)34 + "Авторизация" + (char)34 +", True"+"\n"+
            //"objWShell.SendKeys "+ (char)34 + (char)123 + "TAB"+ (char)125 + (char)34 + ", True" +"\n"+
            //"objWShell.SendKeys " + (char)34 + (char)123 + "DOWN 2"+ (char)125+ (char)34 + ", True" +"\n"+

            //"objWShell.SendKeys " + (char)34 + (char)123 + "TAB" + (char)125 + (char)34 + ", True" +"\n"+
            //"objWShell.SendKeys "+ (char)34 + "1"+ (char)34 +", True"+"\n"+
            //"objWShell.SendKeys " + (char)34 + (char)123 + "ENTER" + (char)125 + (char)34 + ", True" +"\n"+
            //"Set objWShell = Nothing\n"+
            //"WScript.Sleep 5000\n"+
            //"Set objWShell = CreateObject(" + (char)34 + "WScript.Shell"+ (char)34 +")\n"+
            //"objWShell.AppActivate " + (char)34 + "ЯМед.Электронная Медицина" + (char)34 +", True"+"\n"+
            //"objWShell.SendKeys " + (char)34 + (char)123 + "TAB" + (char)125 + (char)34 + ", True" +"\n"+
            //"objWShell.SendKeys " + (char)34 + (char)123 + "DOWN" + (char)125 + (char)34 + ", True" +"\n"+
            //"objWShell.SendKeys " + (char)34 + (char)123 + "RIGHT" + (char)125 + (char)34 + ", True");
        }
        private ObservableCollection<STOP_LIST> stop_list = new ObservableCollection<STOP_LIST>();
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OPF = new OpenFileDialog();

            OPF.Multiselect = true;
            bool res = OPF.ShowDialog().Value;
            string[] files = OPF.FileNames;
            //List<string> VPOLIC = new List<string>();
            //List<string> NPOLIS = new List<string>();
            //List<string> DEND = new List<string>();
            //List<string> DSTOP = new List<string>();
            
            if (res == true)
            {
                Cursor = Cursors.Wait;
                foreach (string file in files)
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(file);                    

                    var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                    SqlConnection con = new SqlConnection(connectionString);
                    SqlCommand com = new SqlCommand($@"exec [Load_Stoplist] @xml = '{xDoc.LastChild.OuterXml}'",  con);
                    con.Open();
                    com.CommandTimeout = 0;
                    com.ExecuteNonQuery();
                    con.Close();

                }
                
                Cursor = Cursors.Arrow;
                string m = "Данные Стоп Листов успешно загружены!";
                string t = "Сообщение";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                return;
                              

            }
        }
        public string[] Attache_file;
        private void Attache_Click(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog OF = new OpenFileDialog();

            bool res = OF.ShowDialog().Value;
            string[] path_ex = OF.FileNames;
            Attache_file = OF.SafeFileNames;
            string call = "attache";
            if (res == true)
            {
                
                Polis_Up w8_polises_in = new Polis_Up(path_ex, Attache_file,call);
                w8_polises_in.ShowDialog();
            }
            else
            {
                return;
            }
        }

        private void Attache_to_foms_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OF = new OpenFileDialog();

            bool res = OF.ShowDialog().Value;
            string[] path_ex = OF.FileNames;
            Attache_file = OF.SafeFileNames;
            string call = "attache_tf";
            if (res == true)
            {

                Polis_Up w8_polises_in = new Polis_Up(path_ex, Attache_file, call);
                w8_polises_in.ShowDialog();
            }
            else
            {
                return;
            }
        }
    }
    
}
