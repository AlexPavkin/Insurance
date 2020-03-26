using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Bytescout.Spreadsheet;
using Insurance_SPR;
using DevExpress.Xpf.Core;
using Ionic.Zip;
using System.Windows.Threading;
using WFR = System.Windows.Forms;
using System.Threading;

namespace Insurance
{
    /// <summary>
    /// Логика взаимодействия для Developer.xaml
    /// </summary>
    public partial class Developer : Window
    {
        //static byte[] ConvertTobyteArray(List<object> obj)
        //{

        //    Encoding encode = Encoding.ASCII;

        //    List<byte> listByte = new List<byte>();
        //    string[] ResultCollectionArray = obj.Select(i => i.ToString()).ToArray<string>();

        //    foreach (var item in ResultCollectionArray)
        //    {
        //        foreach (byte b in encode.GetBytes(item))
        //            listByte.Add(b);
        //    }

        //    return listByte.ToArray();

        //}
        public class ATTACHED_MO
        {
            public string GUID { get; set; }
            [System.ComponentModel.DisplayName("ОКАТО")]
            public string OKATO { get; set; }
            [System.ComponentModel.DisplayName("СМО")]
            public string SMO { get; set; }
            [System.ComponentModel.DisplayName("ДПФС")]
            public string DPFS { get; set; }
            [System.ComponentModel.DisplayName("Серия")]
            public string SER { get; set; }
            [System.ComponentModel.DisplayName("Номер")]
            public string NUM { get; set; }
            [System.ComponentModel.DisplayName("ЕНП")]
            public string ENP { get; set; }
            [System.ComponentModel.DisplayName("МО")]
            public string MO { get; set; }


        }
        public class PEOPLES
        {
            public int ID { get; set; }
            public string FAM { get; set; }
            
            public string IM { get; set; }
            
            public string OT { get; set; }
           
            public int W { get; set; }
           
            public DateTime? DR { get; set; }
            
            public string ENP { get; set; }
            public string C_OKSM { get; set; }
            public Guid EVENT_GUID { get; set; }
            public string MO { get; set; }


        }
        public class EVENT_S
        {
            public int ID { get; set; }
            public Guid IDGUID { get; set; }

            public string TIP_OP { get; set; }           

            public int FPOLIS { get; set; }

            public DateTime? DVIZIT { get; set; }
            
        }


        public ObservableCollection<ATTACHED_MO> Attache_mo = new ObservableCollection<ATTACHED_MO>();
        public Developer()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ////DispatcherTimer timer = new DispatcherTimer();  // если надо, то в скобках указываем приоритет, например DispatcherPriority.Render
            ////timer.Interval = TimeSpan.FromSeconds(15);
            ////timer.Tick += new EventHandler(Reload_tick);
            ////timer.Start();


        }
        private int rows_count;
        private DataTable tb;
        private DataTable tb1;
        private string[] polis_up_file;
        private void From_excel_btn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OF = new OpenFileDialog();

            OF.Multiselect = false;
            bool res = OF.ShowDialog().Value;
            string ex_path = OF.FileName;
            string call = "polis";
            polis_up_file = OF.SafeFileNames;
            ZipFile zip = new ZipFile(ex_path);
            zip.ExtractAll(ex_path.Replace(OF.SafeFileName, ""), ExtractExistingFileAction.OverwriteSilently);
            ex_path = ex_path.Replace(OF.SafeFileName, zip.EntryFileNames.First());

            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                new Action(delegate ()
                {
                    tb = new DataTable();
                    if (ex_path.Contains(".xls") || ex_path.Contains(".xlsx"))
                    {
                        Spreadsheet excel = new Spreadsheet();
                        excel.LoadFromFile(ex_path);
                        tb = excel.ExportToDataTable();
                    }
                    else
                    {
                        string filename = ex_path;
                        string[] attache = File.ReadAllLines(filename);

                        //var cls0 = attache[0].Split('|');
                        var cls0 = attache[0].Split(';');
                        for (int i = 0; i < cls0.Count(); i++)
                        {
                            tb.Columns.Add("Column" + i.ToString(), typeof(string));
                        }
                        //tb.Columns.AddRange();
                        foreach (string row in attache)
                        {
                            // получаем все ячейки строки

                            var cls = row.Split('|');

                            tb.LoadDataRow(cls, LoadOption.Upsert);
                            //Attache_mo.Add(new ATTACHED_MO { GUID = cls[0], OKATO = cls[1], SMO = cls[2], DPFS = cls[3], SER = cls[4], NUM = cls[5], ENP = cls[6], MO = cls[7] });
                        }
                    }



                    //string filename = ex_path[0].Replace(".xls", ".csv");
                    //string[] attache = File.ReadAllLines(filename);



                    //tb.Columns.Add("GUID", typeof(string)); tb.Columns.Add("ОКАТО", typeof(string)); tb.Columns.Add("СМО", typeof(string)); tb.Columns.Add("ДПФС", typeof(string));
                    //tb.Columns.Add("Серия", typeof(string)); tb.Columns.Add("Номер", typeof(string)); tb.Columns.Add("ЕНП", typeof(string)); tb.Columns.Add("МО", typeof(string));

                    //foreach (string row in attache)
                    //{
                    //    // получаем все ячейки строки

                    //    var cls = row.Split('|');

                    //    tb.LoadDataRow(cls, LoadOption.Upsert);
                    //    Attache_mo.Add(new ATTACHED_MO { GUID = cls[0], OKATO = cls[1], SMO = cls[2], DPFS = cls[3], SER = cls[4], NUM = cls[5], ENP = cls[6], MO = cls[7] });
                    //}

                    pol_zagr.ItemsSource = tb;
                    //pol_zagr.ItemsSource = Attache_mo;



                }));

        }

        private void From_excel_btn_Copy_Click(object sender, RoutedEventArgs e)
        {

            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand comm = new SqlCommand($@"update pol_persons set mo=amo.MO from @AttachedMO amo where pol_persons.enp=amo.ENP", con);

            SqlParameter Att_Mo = comm.Parameters.AddWithValue("@AttachedMO", tb);
            Att_Mo.SqlDbType = SqlDbType.Structured;
            Att_Mo.TypeName = "dbo.AttacheTableType";
            con.Open();
            comm.CommandTimeout = 0;
            comm.ExecuteNonQuery();

            con.Close();

        }

        private void From_excel_btn_Copy1_Click(object sender, RoutedEventArgs e)
        {
            var peopleList =
                           MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + "order by pe.id desc", Properties.Settings.Default.DocExchangeConnectionString);

            pol_zagr.ItemsSource = peopleList;

            SaveFileDialog SF = new SaveFileDialog();
            SF.DefaultExt = ".csv";
            SF.Filter = "Файлы CSV (.csv)|*.csv";
            bool res = SF.ShowDialog().Value;
            string fname = SF.FileName;


            if (res == true)
            {
                DevExpress.XtraPrinting.CsvExportOptions options = new DevExpress.XtraPrinting.CsvExportOptions();
                options.Encoding = Encoding.GetEncoding(1251);
                options.Separator = "|";
                pol_zagr.View.ShowColumnHeaders = false;
                pol_zagr.View.ExportToCsv(fname, options);
                pol_zagr.View.ShowColumnHeaders = true;

            }


        }

        private void From_excel_btn_Copy2_Click(object sender, RoutedEventArgs e)
        {
            string fname1 = $@"IS{Vars.SMO}T{Vars.SMO.Substring(0, 2)}_{DateTime.Today.Year.ToString().Substring(0, 2) + (DateTime.Today.Month.ToString().Length < 2 ? "0" + DateTime.Today.Month.ToString() : DateTime.Today.Month.ToString())}1.csv";
            SaveFileDialog SF = new SaveFileDialog();
            SF.FileName = fname1;
            SF.DefaultExt = ".csv";
            SF.Filter = "Файлы CSV (.csv)|*.csv";
            bool res = SF.ShowDialog().Value;
            string fname = SF.FileName;


            if (res == true)
            {
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                SqlConnection con = new SqlConnection(connectionString);

                SqlCommand comm = new SqlCommand($@"declare @r char='|';
            SELECT convert(nvarchar(36), p.IDGUID) + @r + isnull(fam, '') + @r + isnull(im, '') + @r + isnull(ot, '') + @r + convert(nvarchar, isnull(cast(dr as date), '')) + @r
            + convert(nvarchar, isnull(w, '')) + @r + convert(nvarchar, isnull(DOCTYPE, '')) + @r + isnull(DOCSER, '') + @r + isnull(DOCNUM, '') + @r + isnull(ss, '') + @r
            + '1027739449913' + @r + '01000' + @r + enp + @r + convert(nvarchar, isnull(vpolis, '')) + @r + isnull(SPOLIS, '') + @r + isnull(NPOLIS, '') + @r
            + convert(nvarchar, getdate(), 23) + @r + convert(nvarchar, getdate(), 23) + @r + '1022200897840' + @r + '0'
from POL_PERSONS p
left
join POL_DOCUMENTS d on p.EVENT_GUID = d.EVENT_GUID
left
join POL_POLISES pp on p.EVENT_GUID = pp.EVENT_GUID", con);
                con.Open();
                SqlDataReader dr = comm.ExecuteReader();
                StreamWriter sr = new StreamWriter(fname, false, Encoding.GetEncoding(1251));

                while (dr.Read())
                {

                    sr.Write("{0}", dr[0]);
                    sr.WriteLine();

                }

                sr.Close();
                con.Close();
                DXMessageBox.Show("Выгрузка завершена");
            }
        }

        private void From_excel_btn_Copy3_Click(object sender, RoutedEventArgs e)
        {
            text_b.Text = MyReader.MyExecuteWithResult(@"select*from pol_persons", Properties.Settings.Default.DocExchangeConnectionString);
            if (text_b.Text == "Done")
            {
                OpenFileDialog OF = new OpenFileDialog();
                OF.Multiselect = false;
                bool res = OF.ShowDialog().Value;
            }
            //OpenFileDialog OF = new OpenFileDialog();

            //OF.Multiselect = false;
            //bool res = OF.ShowDialog().Value;
            //string ex_path = OF.FileName;
            //text_b.Text = ""; // очищаем текстовое поле
            //string _url = @"https://ips.rosminzdrav.ru/5358bf30e7897"; // URL SOAP API-сервиса
            //string _method = @"GetMos​"; // Метод, который вызывается на API сервисе
            //string _soapEnvelope = File.ReadAllText(ex_path); // SOAP-конверт (запрос), который будет отправлен к API
            //string _response = GetResponseSoap(_url, _method, _soapEnvelope); // получаем ответ SOAP сервиса в виде XML
            //text_b.Text = _response; // выводим данные в текстовое поле
        }
        private string GetResponseSoap(string _url, string _method, string _soapEnvelope)
        {
            _url = _url.Trim('/').Trim('\\'); // в конце адреса удалить слэш, если он имеется
            WebRequest _request = HttpWebRequest.Create(_url);
            //все эти настройки можно взять со страницы описания веб-сервиса
            _request.Method = "POST";
            _request.ContentType = "xml; charset=utf-8";
            _request.ContentLength = _soapEnvelope.Length;
            //_request.Headers.Add(_method);
            // пишем тело
            StreamWriter _streamWriter = new StreamWriter(_request.GetRequestStream());
            _streamWriter.Write(_soapEnvelope);
            _streamWriter.Close();
            // читаем тело
            WebResponse _response = _request.GetResponse();
            StreamReader _streamReader = new StreamReader(_response.GetResponseStream());
            string _result = _streamReader.ReadToEnd(); // переменная в которую пишется результат (ответ) сервиса
            return _result;
        }
        public static List<PEOPLES> erp;
        public static List<PEOPLES> erp1;
        public static List<Events> ev;
        public static List<Events> ev1;
        private void From_dbf_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OF = new OpenFileDialog();
            OF.DefaultExt = ".dbf";
            OF.Filter = "Файлы DBF (.dbf)|*.dbf";
            bool res = OF.ShowDialog().Value;
           
            if (res == true)
            {
                DataTable dt = new DataTable();
                string dbffile = OF.FileName;
                using (Stream fos = File.Open(dbffile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    var dbf = new DotNetDBF.DBFReader(fos);
                    dbf.CharEncoding = Encoding.GetEncoding(866);
                    
                    var cnt = dbf.RecordCount;
                    //var writer = new DotNetDBF.DBFWriter(fos);
                    //    writer.CharEncoding = Encoding.GetEncoding(866);
                    //    writer.Signature = DotNetDBF.DBFSignature.DBase3;
                    //writer.LanguageDriver = 0x26; // кодировка 866
                    //var field1 = new DotNetDBF.DBFField("DOCDATE", DotNetDBF.NativeDbType.Date);
                    //var field2 = new DotNetDBF.DBFField("DOCNUMBER", DotNetDBF.NativeDbType.Char, 10);
                    //var field3 = new DotNetDBF.DBFField("DOCSER", DotNetDBF.NativeDbType.Char, 8);
                    //var field4 = new DotNetDBF.DBFField("DOCTYPE", DotNetDBF.NativeDbType.Numeric, 2, 0);
                    //var field5 = new DotNetDBF.DBFField("RETPRICE", DotNetDBF.NativeDbType.Numeric, 10, 2);
                    //var field6 = new DotNetDBF.DBFField("QUANTITY", DotNetDBF.NativeDbType.Numeric, 3, 2);
                    //var field7 = new DotNetDBF.DBFField("APCODE", DotNetDBF.NativeDbType.Numeric, 10, 0);
                    //var field8 = new DotNetDBF.DBFField("CLNTNAME", DotNetDBF.NativeDbType.Char, 255);
                    //var field9 = new DotNetDBF.DBFField("CLNTPHONE", DotNetDBF.NativeDbType.Char, 20);

                    //writer.Fields = new DotNetDBF.DBFReader(fos).Fields;

                    //for (int i = 0; i < docList.Count; i++)
                    //{
                    //    writer.AddRecord(docList[i].DOCDATE, docList[i].DOCNUM, docList[i].DOCSER, docList[i].DOCTYPE
                    //       // добавляем поля в набор
                    //       );
                    //}



                    //writer.Write(fos);




                    //var dbf = new DotNetDBF.DBFReader(fos);
                    
                    //var cnt = dbf.RecordCount;
                    

                    var fields = dbf.Fields;
                    for(int i=0;i<fields.Count();i++)
                    {
                        dt.Columns.Add(fields[i].Name,fields[i].Type);
                    }
                    
                    //var result = (from s in fields select s.Name).ToArray();
                    //dt.Load(dbf.NextRecord());
                    for(int i=0;i<dbf.RecordCount;i++)
                    {
                        var rtt = dbf.NextRecord();
                        if(rtt!=null)
                        {
                            dt.LoadDataRow(rtt, false);
                        }
                        
                    }

                    //dbf.SetSelectFields("F1", "F3");

                    pol_zagr.ItemsSource = dt;

                }
//                //var peopleList =
//                //MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + " order by pe.ID DESC", Properties.Settings.Default.DocExchangeConnectionString);
//                ev = MyReader.MySelect<Events>($@"SELECT top(20000) pp.SROKDOVERENOSTI, pp.ID, pp.ACTIVE, op.przcod, pe.UNLOAD, ENP, FAM, IM, OT, W, DR, MO, oks.CAPTION as C_OKSM, r.NameWithID, pp.COMMENT, pe.DVIZIT, pp.DATEVIDACHI, pp.PRIZNAKVIDACHI,
//            SS, VPOLIS, SPOLIS, NPOLIS, DBEG, DEND, DSTOP, BLANK, DRECEIVED, f.NameWithId as MO_NameWithId, op.filename, pp.phone, p.AGENT, pp.CYCLE, (select pr.namewithid  from POL_PERSONS_INFORM pin
//      left
//                                                                                                                                                            join PRICHINA_INFORMIROVANIYA pr
//                                                                                                                                                       on pin.PRICHINA_INFORM = pr.ID
//where PERSON_ID = pp.ID and pin.id = (select max(id) from POL_PERSONS_INFORM where PERSON_ID = pp.ID)) as inform, st.namewithkod as STOP_REASON, pe.id as EVENT_ID
//              FROM[dbo].[POL_PERSONS] pp left join
//           pol_events pe on pp.event_guid = pe.idguid

//             LEFT JOIN POL_PRZ_AGENTS p

//            on p.ID = pe.AGENT

//            left join pol_polises ps
//            on pp.EVENT_GUID = ps.EVENT_GUID
//            left join pol_oplist op
//            on pp.EVENT_GUID = op.EVENT_GUID
//            left join r001 r
//            on pe.tip_op = r.kod
//            left join f003 f
//            on pp.MO = f.mcod
//            left join SPR_STOP st
//            on ps.STOP_REASON = st.kod

//            left join SPR_79_OKSM oks

//            on pp.C_OKSM = oks.A3", Properties.Settings.Default.DocExchangeConnectionString);
//                //erp = MyReader.MySelect<PEOPLES>("select top 100 * from pol_persons", Properties.Settings.Default.DocExchangeConnectionString);
//                //var ere = MyReader.MySelect<EVENT_S>("select*from pol_events", Properties.Settings.Default.DocExchangeConnectionString);
//                //var err = MyReader.MySelect<R001>("select*from R001", Properties.Settings.Default.DocExchangeConnectionString);
//                //var result = (from p in erp
//                //              join pe in ere
//                //              on p.EVENT_GUID equals pe.IDGUID
//                //              from pr in err
//                //              .Where(pr => pr.Kod == pe.TIP_OP)
//                //              .DefaultIfEmpty()

//                //              select new { pe.ID,p.FAM, p.IM, p.OT, p.DR, p.W, NameWithID = (pr == null ? String.Empty : pr.NameWithID), pe.DVIZIT, pe.FPOLIS }).OrderByDescending(a=>a.ID);

//                //List<object> lst = new List<object>();
//                //using (SqlConnection con = new SqlConnection(Properties.Settings.Default.DocExchangeConnectionString))
//                //{

//                //    using (SqlCommand cmd = new SqlCommand("select*from pol_persons", con))
//                //    {
//                //        con.Open();
//                //        cmd.CommandTimeout = 0;
//                //        SqlDataReader dr = cmd.ExecuteReader();
//                //        while (dr.Read())
//                //        {
//                //            lst.Add(dr);
//                //        }
//                //        dr.Close();
//                //    }

//                //    con.Close();
//                //}
//                //var sss = lst[25685];
//                pol_zagr.ItemsSource = ev.OrderByDescending(a=>a.ID);
                
                
            }
            

        }

        private void Add_btn_Click(object sender, RoutedEventArgs e)
        {
            //pol_zagr.RefreshData();
            //Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
            //    new Action(delegate ()
            //    {
                    //var pl = new PEOPLES {ID=400002, FAM="КУКУЕВА",IM="ИНГА",OT="ПЕТРОВНА",W=2,DR=DateTime.Today.AddYears(-15),ENP="1234567891234567"};
                    //erp.Add(pl);
                    //pol_zagr.ItemsSource = erp.OrderByDescending(a => a.ID);
                    //pol_zagr.RefreshData();
                    //var r1 = erp.Select(x => x.ID);

                    //Type type = erp.GetType();

                    //var props = type.GetProperties();
                    //foreach (var propertyInfo in props)
                    //{

                    //}
                    string com = $@"SELECT top(20000) sss.id , pp.SROKDOVERENOSTI, pp.ID, pp.ACTIVE, op.przcod, pe.UNLOAD, pp.ENP, pp.FAM, pp.IM, pp.OT, pp.W,pp. DR, MO, oks.CAPTION as C_OKSM, r.NameWithID , pp.COMMENT, pe.DVIZIT, pp.DATEVIDACHI, pp.PRIZNAKVIDACHI,
            pp.SS, ps.VPOLIS, ps.SPOLIS, ps.NPOLIS, ps.DBEG, ps.DEND, ps.DSTOP, BLANK, ps.DRECEIVED, f.NameWithId as MO_NameWithId , op.filename, pp.phone, p.AGENT, pp.CYCLE, (select pr.namewithid  from POL_PERSONS_INFORM pin
      left join PRICHINA_INFORMIROVANIYA pr
      on pin.PRICHINA_INFORM = pr.ID
where PERSON_ID = pp.ID and pin.id = (select max(id) from POL_PERSONS_INFORM where PERSON_ID = pp.ID)) as inform, st.namewithkod as STOP_REASON, pe.id as EVENT_ID
              FROM POL_PERSONS pp left join
           pol_events pe on pp.event_guid = pe.idguid

             LEFT JOIN POL_PRZ_AGENTS p

            on p.ID = pe.AGENT

            left join pol_polises ps
            on pp.EVENT_GUID = ps.EVENT_GUID
            left join pol_oplist op
            on pp.EVENT_GUID = op.EVENT_GUID
            left join r001 r
            on pe.tip_op = r.kod
            left join f003 f
            on pp.MO = f.mcod
            left join SPR_STOP st
            on ps.STOP_REASON = st.kod

            left join SPR_79_OKSM oks

            on pp.C_OKSM = oks.A3
			left join @t sss
			on sss.ID=pp.ID
			where sss.id is null";
                    //string com = $@"select * from  @t order by id desc";

                    ev1 = MyReader.MySelect<Events>(com, Properties.Settings.Default.DocExchangeConnectionString, ev);
                    ev.AddRange(ev1);
                    //foreach (var item in erp1)
                    //{
                    //    if (erp.Where(c => c.ID == item.ID).Count() == 0)
                    //    {
                    //        erp.Add(item);
                    //    }
                    //}
                    pol_zagr.RefreshData();
                //}));
            
        }
        private RoutedEventArgs re;
        private void Reload_tick(object sender, EventArgs e)
        {

            ////Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
            ////    new Action(delegate ()
            ////    {
            ////        erp1 = MyReader.MySelect<PEOPLES>($@"select top(20)* from pol_persons order by id desc", Properties.Settings.Default.DocExchangeConnectionString);
            ////        foreach (var item in erp1)
            ////        {
            ////            if (erp.Where(c => c.ID == item.ID).Count() == 0)
            ////            {
            ////                erp.Add(item);
            ////            }
            ////        }
            ////        //pol_zagr.RefreshData();
            ////Add_btn_Click(this,re);
            ////}));

            //Thread thread = new Thread(spisok_up);

            //thread.Start();

            ////pol_zagr.RefreshData();
        }

        private void File_Copy_Click(object sender, RoutedEventArgs e)
        {
            WFR.FolderBrowserDialog OF = new WFR.FolderBrowserDialog();
            if (OF.ShowDialog() == WFR.DialogResult.OK)
            {
                string ex_path = OF.SelectedPath;
                DirectoryInfo Dinfo = new DirectoryInfo(ex_path);
                var dirs = Dinfo.GetDirectories();
            
                foreach(var d in dirs)
                {
                    int i = 0;
                    var fls = d.GetFiles();
                    foreach(var f in fls)
                    {
                        if (!Directory.Exists(System.IO.Path.Combine(ex_path, DateTime.Today.ToShortDateString())))
                        Directory.CreateDirectory(System.IO.Path.Combine(ex_path, DateTime.Today.ToShortDateString()));
                        
                           
                        i = i + 1;
                        f.CopyTo(System.IO.Path.Combine(ex_path,DateTime.Today.ToShortDateString(),i.ToString()+d+f));
                    }
                }
            }
        }
        private void spisok_up()
        {
            string com = $@"SELECT top(20000) sss.id , pp.SROKDOVERENOSTI, pp.ID, pp.ACTIVE, op.przcod, pe.UNLOAD, pp.ENP, pp.FAM, pp.IM, pp.OT, pp.W,pp. DR, MO, oks.CAPTION as C_OKSM, r.NameWithID , pp.COMMENT, pe.DVIZIT, pp.DATEVIDACHI, pp.PRIZNAKVIDACHI,
            pp.SS, ps.VPOLIS, ps.SPOLIS, ps.NPOLIS, ps.DBEG, ps.DEND, ps.DSTOP, BLANK, ps.DRECEIVED, f.NameWithId as MO_NameWithId , op.filename, pp.phone, p.AGENT, pp.CYCLE, (select pr.namewithid  from POL_PERSONS_INFORM pin
      left join PRICHINA_INFORMIROVANIYA pr
      on pin.PRICHINA_INFORM = pr.ID
where PERSON_ID = pp.ID and pin.id = (select max(id) from POL_PERSONS_INFORM where PERSON_ID = pp.ID)) as inform, st.namewithkod as STOP_REASON, pe.id as EVENT_ID
              FROM POL_PERSONS pp left join
           pol_events pe on pp.event_guid = pe.idguid

             LEFT JOIN POL_PRZ_AGENTS p

            on p.ID = pe.AGENT

            left join pol_polises ps
            on pp.EVENT_GUID = ps.EVENT_GUID
            left join pol_oplist op
            on pp.EVENT_GUID = op.EVENT_GUID
            left join r001 r
            on pe.tip_op = r.kod
            left join f003 f
            on pp.MO = f.mcod
            left join SPR_STOP st
            on ps.STOP_REASON = st.kod

            left join SPR_79_OKSM oks

            on pp.C_OKSM = oks.A3
			left join @t sss
			on sss.ID=pp.ID
			where sss.id is null";
            //string com = $@"select * from  @t order by id desc";
            
            try
            {
                ev1 = MyReader.MySelect<Events>(com, Properties.Settings.Default.DocExchangeConnectionString, ev);
            }
            catch (Exception e)
            {    
                
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {
                    string t0 = "Ошибка!";
                    int b0 = 1;
                    Message me0 = new Message(e.Message, t0, b0);
                    me0.ShowDialog();
                }
                );
                return;
            }
            
            ev.AddRange(ev1);
            //foreach (var item in erp1)
            //{
            //    if (erp.Where(c => c.ID == item.ID).Count() == 0)
            //    {
            //        erp.Add(item);
            //    }
            //}
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {
                    
                    pol_zagr.RefreshData();
                }
            );
            
        }

        private void Text_b_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataTable dt = new DataTable();
            
            int[] rrr;
            var r = pol_zagr.SelectedItems;
            
            dt = null;

        }
    }
    public class Class_params : IComparable<Class_params>
    {
        public  string NAME { get; set; }
        public  Type TYPE { get; set; }
        public int CompareTo(Class_params p)
        {
            return this.NAME.CompareTo(p.NAME);
        }
    }
}
