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
using xNet;
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
                        MyReader.LoadFromTable<DataTable>(Properties.Settings.Default.DocExchangeConnectionString, tb, "ONETAB");

                    }
                    else
                    {
                        string filename = ex_path;
                        string[] attache = File.ReadAllLines(filename,Encoding.GetEncoding("Windows-1251"));

                        //var cls0 = attache[0].Split('|');
                        var cls0 = attache[0].Split(';');
                        cls0 = cls0.Where(x => x != "").ToArray();
                        for (int i = 0; i < cls0.Count(); i++)
                        {
                            tb.Columns.Add("Column" + i.ToString(), typeof(string));
                        }
                        //tb.Columns.AddRange();
                        for (int i=1; i<attache.Count();i++)
                        {
                            // получаем все ячейки строки
                            var row1 = attache[i].Substring(0, attache[i].Length - 1);
                            row1 = row1.Replace($"{(char)34}", "");
                            var cls = row1.Split(';');
                            //cls = cls.Where(x => x != "").ToArray();
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
            //OF.Filter = "Файлы DBF (.dbf)|*.dbf";
            OF.Multiselect = true;

            bool res = OF.ShowDialog().Value;

            if (res == true)
            {

                string[] files = OF.FileNames;
                string[] names = OF.SafeFileNames;

                for (int y = 0; y < files.Count(); y++)
                {
                    //var dir=Directory.CreateDirectory(files[y].Replace(".arj", "\\").Replace(".ARJ", "\\"));
                    new ZipFile(files[y]).ExtractAll(files[y].Replace(".arj", "").Replace(".ARJ", ""), ExtractExistingFileAction.OverwriteSilently);
                    DataTable dt = new DataTable();
                    string dbffile = files[0];//OF.FileName;
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
                        for (int i = 0; i < fields.Count(); i++)
                        {
                            dt.Columns.Add(fields[i].Name, fields[i].Type);
                        }

                        //var result = (from s in fields select s.Name).ToArray();
                        //dt.Load(dbf.NextRecord());
                        for (int i = 0; i < dbf.RecordCount; i++)
                        {
                            var rtt = dbf.NextRecord();
                            if (rtt != null)
                            {
                                dt.LoadDataRow(rtt, false);
                            }

                        }

                        //dbf.SetSelectFields("F1", "F3");

                        pol_zagr.ItemsSource = dt;

                    }
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

                foreach (var d in dirs)
                {
                    int i = 0;
                    var fls = d.GetFiles();
                    foreach (var f in fls)
                    {
                        if (!Directory.Exists(System.IO.Path.Combine(ex_path, DateTime.Today.ToShortDateString())))
                            Directory.CreateDirectory(System.IO.Path.Combine(ex_path, DateTime.Today.ToShortDateString()));


                        i = i + 1;
                        f.CopyTo(System.IO.Path.Combine(ex_path, DateTime.Today.ToShortDateString(), i.ToString() + d + f));
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

        private void To_dbf_Click(object sender, RoutedEventArgs e)
        {
            //var cs = "Data Source = 192.168.0.112; Initial Catalog = DocExchange_smolensk; User ID = sa; Password = Gbljh:100";
            SaveFileDialog SF = new SaveFileDialog();
            SF.DefaultExt = ".dbf";
            SF.Filter = "Файлы DBF (.dbf)|*.dbf";
            bool res = SF.ShowDialog().Value;

            if (res == true)
            {
                var inf = MyReader.MySelect<P4_INFORM>($@"select fam as SURNAME,im as NAME,ot as SECNAME,
pp.dr as DR, w as POL, pp.SS as SNILS,1 as SCOMP,ENP as SN_POL, l.kod as KOD_POL, l.kod1 as KOD_POL1, mkb_p4 as KMKB,
DATEPART(yy,Date_P4) as DYEAR, Month_1_P4 as PM1, Month_2_P4 as PM2, Month_3_P4 as PM3, Month_4_P4 as PM4,
PERSON_ID as ID_TFOMS,'39001' as SMO, pol.VPOLIS as DPFS, soglasie_p4 as sogl, Theme_P4 as tema,Date_P4 as Date_uv,
SPOSOB_P4 as sposob,RESULT_P4 as result,PRIMECH as prim

 from POL_PERSONS_INFORM p
 join POL_PERSONS pp on p.PERSONGUID=pp.IDGUID
 left join lpu_39 l on pp.MO=l.MCOD
 left join POL_POLISES pol on pp.EVENT_GUID=pol.EVENT_GUID
 
 where p.id=8", Properties.Settings.Default.DocExchangeConnectionString);
                //DataTable dt = new DataTable();
                string dbffile = SF.FileName;
                using (Stream fos = File.Open(dbffile, FileMode.Create, FileAccess.ReadWrite))
                {

                    var writer = new DotNetDBF.DBFWriter(fos);
                    writer.CharEncoding = Encoding.GetEncoding(866);
                    writer.Signature = DotNetDBF.DBFSignature.DBase3;
                    writer.LanguageDriver = 0x26; // кодировка 866
                    writer.Fields = new[]{
                        new DotNetDBF.DBFField("SURNAME", DotNetDBF.NativeDbType.Char, 40),
                        new DotNetDBF.DBFField("NAME", DotNetDBF.NativeDbType.Char, 40),
                        new DotNetDBF.DBFField("SECNAME", DotNetDBF.NativeDbType.Char, 40),
                        new DotNetDBF.DBFField("DR", DotNetDBF.NativeDbType.Date),
                        new DotNetDBF.DBFField("POL", DotNetDBF.NativeDbType.Numeric, 1,0),
                        new DotNetDBF.DBFField("SNILS", DotNetDBF.NativeDbType.Char, 20),
                        new DotNetDBF.DBFField("SCOMP", DotNetDBF.NativeDbType.Numeric, 2,0),
                        new DotNetDBF.DBFField("SN_POL", DotNetDBF.NativeDbType.Char, 16),
                        new DotNetDBF.DBFField("KOD_POL", DotNetDBF.NativeDbType.Numeric, 5,0),
                        new DotNetDBF.DBFField("KOD_POL1", DotNetDBF.NativeDbType.Numeric, 2,0),
                        new DotNetDBF.DBFField("KMKB", DotNetDBF.NativeDbType.Char, 7),
                        new DotNetDBF.DBFField("DYEAR", DotNetDBF.NativeDbType.Numeric, 4,0),
                        new DotNetDBF.DBFField("PM1", DotNetDBF.NativeDbType.Numeric, 2,0),
                        new DotNetDBF.DBFField("PM2", DotNetDBF.NativeDbType.Numeric, 2,0),
                        new DotNetDBF.DBFField("PM3", DotNetDBF.NativeDbType.Numeric, 2,0),
                        new DotNetDBF.DBFField("PM4", DotNetDBF.NativeDbType.Numeric, 2,0),
                        new DotNetDBF.DBFField("ID_TFOMS", DotNetDBF.NativeDbType.Numeric, 5, 0),
                        new DotNetDBF.DBFField("SMO", DotNetDBF.NativeDbType.Char, 5),
                        new DotNetDBF.DBFField("DPFS", DotNetDBF.NativeDbType.Numeric, 1, 0),
                        new DotNetDBF.DBFField("sogl", DotNetDBF.NativeDbType.Numeric, 1, 0),
                        new DotNetDBF.DBFField("tema", DotNetDBF.NativeDbType.Numeric, 1, 0),
                        new DotNetDBF.DBFField("Date_uv", DotNetDBF.NativeDbType.Date),
                        new DotNetDBF.DBFField("sposob", DotNetDBF.NativeDbType.Numeric, 2, 0),
                        new DotNetDBF.DBFField("result", DotNetDBF.NativeDbType.Numeric, 1, 0),
                        new DotNetDBF.DBFField("prim", DotNetDBF.NativeDbType.Char, 250),
                        //var field9 = new DotNetDBF.DBFField("DMONTH", DotNetDBF.NativeDbType.Char, 20);
                        //var field10 = new DotNetDBF.DBFField("DYEAR", DotNetDBF.NativeDbType.Char, 20);
                        //var field11 = new DotNetDBF.DBFField("TEMA", DotNetDBF.NativeDbType.Char, 20);
                        //new DotNetDBF.DBFField("ID_TFOMS", DotNetDBF.NativeDbType.Numeric, 5, 0),
                        //new DotNetDBF.DBFField("DATE_UV", DotNetDBF.NativeDbType.Date)
                    };

                    //writer.Fields = new DotNetDBF.DBFWriter(fos).Fields;

                    for (int i = 0; i < inf.Count; i++)
                    {
                        writer.WriteRecord(inf[i].SURNAME, inf[i].NAME, inf[i].SECNAME, inf[i].DR,
                             inf[i].POL, inf[i].SNILS, inf[i].SCOMP, inf[i].SN_POL, inf[i].KOD_POL, inf[i].KOD_POL1, inf[i].KMKB,
                             inf[i].DYEAR, inf[i].PM1, inf[i].PM2, inf[i].PM3, inf[i].PM4, inf[i].ID_TFOMS, inf[i].SMO, inf[i].DPFS,
                             inf[i].sogl, inf[i].tema, inf[i].Date_uv, inf[i].sposob, inf[i].result, inf[i].prim
                           // добавляем поля в набор
                           );

                    }

                    //writer.Write(fos);

                    //var dbf = new DotNetDBF.DBFReader(fos);

                    //var cnt = dbf.RecordCount;
                }
            }
        }

        private void To_dbf_3_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog SF = new SaveFileDialog();
            SF.DefaultExt = ".dbf";
            SF.Filter = "Файлы DBF (.dbf)|*.dbf";
            bool res = SF.ShowDialog().Value;

            if (res == true)
            {
                var inf = MyReader.MySelect<P3_INFORM>($@"select pp.id as ID_TFOMS,fam as SURNAME,im as NAME,ot as SECNAME,
pp.dr as DR, w as POL, pp.SS as SNILS,1 as SCOMP,pol.VPOLIS as DPFS,ENP as SN_POL,VID_P3 as VIDPROF, 
DATEPART(yy,Date_P3) as DYEAR, DATEPART(MM,Date_P3) as DMONTH, Theme_P3 as Tema,Date_P3 as DATE_UV,
SPOSOB_P3 as sposob,RESULT_P3 as result,PRIMECH as prim, l.kod as KOD_POL, l.kod1 as KOD_POL1,'39001' as SMO
 from POL_PERSONS_INFORM p
 join POL_PERSONS pp on p.PERSONGUID=pp.IDGUID
 left join lpu_39 l on pp.MO=l.MCOD
 left join POL_POLISES pol on pp.EVENT_GUID=pol.EVENT_GUID
  where p.id=5", Properties.Settings.Default.DocExchangeConnectionString);
                //DataTable dt = new DataTable();
                string dbffile = SF.FileName;
                using (Stream fos = File.Open(dbffile, FileMode.Create, FileAccess.ReadWrite))
                {

                    var writer = new DotNetDBF.DBFWriter(fos);
                    writer.CharEncoding = Encoding.GetEncoding(866);
                    writer.Signature = DotNetDBF.DBFSignature.DBase3;
                    writer.LanguageDriver = 0x26; // кодировка 866
                    writer.Fields = new[]{
                        new DotNetDBF.DBFField("ID_TFOMS", DotNetDBF.NativeDbType.Numeric, 5, 0),
                        new DotNetDBF.DBFField("SURNAME", DotNetDBF.NativeDbType.Char, 40),
                        new DotNetDBF.DBFField("NAME", DotNetDBF.NativeDbType.Char, 40),
                        new DotNetDBF.DBFField("SECNAME", DotNetDBF.NativeDbType.Char, 40),
                        new DotNetDBF.DBFField("DR", DotNetDBF.NativeDbType.Date),
                        new DotNetDBF.DBFField("POL", DotNetDBF.NativeDbType.Numeric, 1,0),
                        new DotNetDBF.DBFField("SNILS", DotNetDBF.NativeDbType.Char, 20),
                        new DotNetDBF.DBFField("SCOMP", DotNetDBF.NativeDbType.Numeric, 2,0),
                        new DotNetDBF.DBFField("DPFS", DotNetDBF.NativeDbType.Numeric, 1, 0),
                        new DotNetDBF.DBFField("SN_POL", DotNetDBF.NativeDbType.Char, 16),
                        new DotNetDBF.DBFField("VIDPROF", DotNetDBF.NativeDbType.Numeric, 1,0),
                        new DotNetDBF.DBFField("DYEAR", DotNetDBF.NativeDbType.Numeric, 4,0),
                        new DotNetDBF.DBFField("DMONTH", DotNetDBF.NativeDbType.Numeric, 2,0),
                        new DotNetDBF.DBFField("Tema", DotNetDBF.NativeDbType.Numeric, 1, 0),
                        new DotNetDBF.DBFField("DATE_UV", DotNetDBF.NativeDbType.Date),
                        new DotNetDBF.DBFField("SPOSOB", DotNetDBF.NativeDbType.Numeric, 2, 0),
                        new DotNetDBF.DBFField("RESULT", DotNetDBF.NativeDbType.Numeric, 1, 0),
                        new DotNetDBF.DBFField("prim", DotNetDBF.NativeDbType.Char, 250),
                        new DotNetDBF.DBFField("KOD_POL", DotNetDBF.NativeDbType.Numeric, 5,0),
                        new DotNetDBF.DBFField("KOD_POL1", DotNetDBF.NativeDbType.Numeric, 2,0),
                        new DotNetDBF.DBFField("SMO", DotNetDBF.NativeDbType.Char, 5),

                    };

                    for (int i = 0; i < inf.Count; i++)
                    {
                        writer.WriteRecord(inf[i].ID_TFOMS, inf[i].SURNAME, inf[i].NAME, inf[i].SECNAME, inf[i].DR,
                             inf[i].POL, inf[i].SNILS, inf[i].SCOMP, inf[i].DPFS, inf[i].SN_POL, inf[i].VIDPROF,
                             inf[i].DYEAR, inf[i].DMONTH, inf[i].Tema, inf[i].DATE_UV, inf[i].SPOSOB, inf[i].RESULT,
                             inf[i].prim, inf[i].KOD_POL, inf[i].KOD_POL1, inf[i].SMO
                           // добавляем поля в набор
                           );

                    }


                }
            }
        }

        private void K_file_Click(object sender, RoutedEventArgs e)
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
                    string rfile = shfiles[y];

                    XmlElement xRoot = xDoc.DocumentElement;
                    //var x = xDoc.GetElementsByTagName("INSURANCE");
                    foreach (XmlNode n in xDoc.GetElementsByTagName("INSURANCE"))
                    {
                        if (n.ChildNodes.Count > 1)
                        {

                            for (int i = 0; i <= n.ChildNodes.Count - 1; i++)
                            {
                                var c = n.ChildNodes[i];
                                n.RemoveChild(n.FirstChild);
                            }

                        }


                    }

                    //var xx = xDoc.ChildNodes[1].ChildNodes[0].Attributes.GetNamedItem("N_REC").Value.ToString();
                    //XmlNode xxx = xDoc.GetElementsByTagName("RECLIST");
                    //var xxxx = xxx.ch;
                    var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                    SqlConnection con = new SqlConnection(connectionString);
                    SqlCommand com = new SqlCommand($@"exec [Load_flk] @xml = '{xDoc.LastChild.OuterXml.Replace((char)39, (char)32)}', @fname='{rfile}'", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();


                    y = y + 1;
                }
                string m = "Изменения данных от ТФОМС успешно загружены!";
                string t = "Сообщение";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();


            }
        }

        private void Load_zah_data_Click(object sender, RoutedEventArgs e)
        {
            string[] file_names = {"dict_doc.dbf","lnsi21.dbf","lnsi22.dbf","lnsi23.dbf","lpu.dbf","statpol.dbf",
             "polis.dbf","polisext.dbf","polisprd.dbf","polissvd.dbf"};
            string ConnectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
            WFR.FolderBrowserDialog OF = new WFR.FolderBrowserDialog();
            //OF.Filter= "Файлы DBF (.dbf)|*.dbf";
            OF.Description = "Выберите папку с программой захарова";
            if (OF.ShowDialog() == WFR.DialogResult.OK)
            {

                DirectoryInfo dir = new DirectoryInfo(OF.SelectedPath);
                foreach (var item in dir.GetDirectories())
                {
                    if (item.Name == "DBF" || item.Name == "DEFAULT")
                    {
                        var spr = item.GetFiles("*.dbf");
                        foreach (var f in spr)
                        {
                            if (file_names.Contains(f.Name))
                            {
                                //string ConnectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                                DataTable dt = new DataTable();
                                using (Stream fos = File.Open(f.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                {
                                    var dbf = new DotNetDBF.DBFReader(fos);
                                    dbf.CharEncoding = Encoding.GetEncoding(866);
                                    var cnt = dbf.RecordCount;
                                    var fields = dbf.Fields;
                                    for (int ii = 0; ii < fields.Count(); ii++)
                                    {
                                        DataColumn workCol = dt.Columns.Add(fields[ii].Name, fields[ii].Type);
                                        if (workCol.DataType == typeof(string)) workCol.MaxLength = fields[ii].FieldLength;
                                        workCol.AllowDBNull = true;
                                        workCol.DefaultValue = DBNull.Value;
                                    }

                                    for (int ii = 0; ii < dbf.RecordCount; ii++)
                                    {
                                        var rtt = dbf.NextRecord();

                                        if (rtt != null)
                                        {
                                            for (int i = 0; i < rtt.Count(); i++)
                                            {
                                                if (rtt[i] == null)
                                                {
                                                    rtt[i] = null;
                                                }
                                                else
                                                if (rtt[i].ToString() == "")
                                                {
                                                    rtt[i] = null;
                                                }
                                            }
                                            dt.LoadDataRow(rtt, true);

                                        }

                                    }

                                }

                                string sqltable = f.Name.Replace(".dbf", "");
                                MyReader.LoadFromTable<DataTable>(ConnectionString1, dt, sqltable);
                            }
                        }
                    }


                }
                foreach (var item in dir.GetDirectories())
                {
                    if (item.Name == "DEFAULT")
                    {
                        DirectoryInfo dir1 = new DirectoryInfo(item.FullName);
                        foreach (var item1 in dir1.GetDirectories())
                        {
                            if (item1.Name == "Photo" || item1.Name == "Sign")
                            {
                                DataTable dt1 = new DataTable();
                                dt1.Columns.Add("N_REG", typeof(decimal));
                                dt1.Columns.Add("Photo", typeof(string));


                                foreach (var item2 in item1.GetFiles("*.jpg"))
                                {
                                    byte[] buf = File.ReadAllBytes(item2.FullName);
                                    object[] r = { item2.Name.Replace(".jpg", ""), Convert.ToBase64String(buf) };
                                    dt1.LoadDataRow(r, LoadOption.OverwriteChanges);

                                }
                                MyReader.LoadFromTable<DataTable>(ConnectionString1, dt1, item1.Name);

                            }
                            SqlConnection con = new SqlConnection(ConnectionString1);
                            SqlCommand com = new SqlCommand($@"if not exists (select * from INFORMATION_SCHEMA.COLUMNS where COLUMN_NAME='Podp' and TABLE_NAME='Photo')
                                                              alter table photo add Podp nvarchar(max)
                                                              IF exists (select * from sys.tables where name='sign')
                                                              Update photo set photo.podp=s.photo from [sign] s
                                                              where photo.n_reg=s.n_reg
                                                              IF exists (select * from sys.tables where name='sign')
                                                              drop table [sign]", con);
                            com.CommandTimeout = 0;
                            con.Open();
                            com.ExecuteNonQuery();
                            con.Close();
                        }

                    }
                }

            }


        }

        private void From_excel_column_tostr_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OF = new OpenFileDialog();

            OF.Multiselect = false;
            bool res = OF.ShowDialog().Value;
            string ex_path = OF.FileName;
            string cell = "";
            tb = new DataTable();
            if (ex_path.Contains(".xls") || ex_path.Contains(".xlsx"))
            {
                Spreadsheet excel = new Spreadsheet();
                excel.LoadFromFile(ex_path);
                var r = excel.Worksheet(0).NotEmptyRowMax;
                for (int i = 0; i <= excel.Worksheet(0).NotEmptyRowMax; i++)
                {
                    var cc = excel.Worksheet(0).Cell(i, 0).Value.ToString();
                    cell = cell + (char)34 + cc + (char)34 + ",";
                }
                text_b.Text = cell.Substring(0, cell.Length - 1);
                tb = excel.ExportToDataTable();

            }
            else
            {
                string filename = ex_path;
                string[] attache = File.ReadAllLines(filename);
                var cls0 = attache[0].Split(';');
                for (int i = 0; i < cls0.Count(); i++)
                {
                    tb.Columns.Add("Column" + i.ToString(), typeof(string));
                }

                foreach (string row in attache)
                {
                    // получаем все ячейки строки

                    var cls = row.Split('|');

                    tb.LoadDataRow(cls, LoadOption.Upsert);
                    cell = (char)34 + cell + (char)34 + ",";
                }
                cell = cell.Substring(0, cell.Length - 1);
                text_b.Text = cell;
            }
            pol_zagr.ItemsSource = tb;

        }
        private string[,] adr1;
        private void Move_zah_data__Click(object sender, RoutedEventArgs e)
        {
            var ad = (fias_d.adres.Text ?? "").Split(',');

            fias_d.reg.EditValue = null; fias_d.reg_rn.EditValue = null; fias_d.reg_town.EditValue = null; fias_d.reg_np.EditValue = null;
            fias_d.reg_ul.EditValue = null; fias_d.reg_dom.EditValue = null;
            Guid reg = Guid.Empty; Guid rn = Guid.Empty; Guid town = Guid.Empty; Guid np = Guid.Empty; Guid ul = Guid.Empty; Guid dom = Guid.Empty;

            SqlConnection con_f = new SqlConnection(Properties.Settings.Default.FIASConnectionString);
            SqlCommand com_f1 = new SqlCommand($@"select aoguid from AddressObjects where AOLEVEL=1 and dbo.Translit('{ad[1]}') like char(37)+dbo.Translit(FORMALNAME)+char(37) and ACTSTATUS=1", con_f);
            using (FIASContext fdb = new FIASContext())
            {
                fdb.Database.Log = Console.Write;
                //string ad1 = ad[1].Replace("ё", "е").Replace(" ", "").Trim(' ');
                //string ad2 = ad[2].Replace("ё", "е").Replace(" ", "").Trim(' ');
                //string ad3 = ad[3].Replace("ё", "е").Replace(" ", "").Trim(' ');
                //string ad4 = ad[4].Replace("ё", "е").Replace(" ", "").Trim(' ');
                //string ad5 = ad[5].Replace("ё", "е").Replace(" ", "").Trim(' ');
                //string ad6 = ad[6].Replace(" ", "").Trim(' ');
                //string ad7 = ad[7].Replace(" ", "").Trim(' ');
                string ad1 = ad[1] == "" ? "" : ad[1].Replace("ё", "е").Substring(0, ad[1].Length - new string(ad[1].ToCharArray().Reverse().ToArray()).IndexOf(' ')).Trim(' ');
                string ad2 = ad[2] == "" ? "" : ad[2].Replace("ё", "е").Substring(0, ad[2].Length - new string(ad[2].ToCharArray().Reverse().ToArray()).IndexOf(' ')).Trim(' ');
                string ad3 = ad[3] == "" ? "" : ad[3].Replace("ё", "е").Substring(0, ad[3].Length - new string(ad[3].ToCharArray().Reverse().ToArray()).IndexOf(' ')).Trim(' ');
                string ad4 = ad[4] == "" ? "" : ad[4].Replace("ё", "е").Substring(0, ad[4].Length - new string(ad[4].ToCharArray().Reverse().ToArray()).IndexOf(' ')).Trim(' ');
                string ad5 = ad[5] == "" ? "" : ad[5].Replace("ё", "е").Substring(0, ad[5].Length - new string(ad[5].ToCharArray().Reverse().ToArray()).IndexOf(' ')).Trim(' ');
                string ad6 = ad[6].Replace(" ", "").Trim(' ');
                string ad7 = ad[7].Replace(" ", "").Trim(' ');
                reg = fdb.AddressObjects.Where(x => x.AOLEVEL == 1 && x.FORMALNAME.Replace("ё", "е") == ad1 && x.ACTSTATUS == 1).Select(x => x.AOGUID).FirstOrDefault();

                rn = fdb.AddressObjects.Where(x => x.AOLEVEL == 3 && x.FORMALNAME.Replace("ё", "е") == ad2 && x.PARENTGUID == reg && x.ACTSTATUS == 1).Select(x => x.AOGUID).FirstOrDefault();

                town = fdb.AddressObjects.Where(x => x.AOLEVEL == 4 && x.FORMALNAME.Replace("ё", "е") == ad3 && x.PARENTGUID == rn && x.ACTSTATUS == 1).Select(x => x.AOGUID).FirstOrDefault();

                if (town == Guid.Empty)
                {
                    town = fdb.AddressObjects.Where(x => x.AOLEVEL == 4 && x.FORMALNAME.Replace("ё", "е") == ad3 && x.PARENTGUID == reg && x.ACTSTATUS == 1).Select(x => x.AOGUID).FirstOrDefault();
                    if (town == Guid.Empty && "Москва,Санкт-Петербург,Севастополь,Байконур".Contains(ad1))
                    {
                        town = reg;
                    }
                }

                np = fdb.AddressObjects.Where(x => x.AOLEVEL == 6 && x.FORMALNAME.Replace("ё", "е") == ad4 && x.PARENTGUID == rn && x.ACTSTATUS == 1).Select(x => x.AOGUID).FirstOrDefault();

                if (np == Guid.Empty)
                {
                    np = fdb.AddressObjects.Where(x => x.AOLEVEL == 6 && x.FORMALNAME.Replace("ё", "е") == ad4 && x.PARENTGUID == town && x.ACTSTATUS == 1).Select(x => x.AOGUID).FirstOrDefault();
                }

                ul = fdb.AddressObjects.Where(x => x.AOLEVEL == 7 && x.FORMALNAME.Replace("ё", "е") == ad5 && x.PARENTGUID == town && x.ACTSTATUS == 1).Select(x => x.AOGUID).FirstOrDefault();

                if (ul == Guid.Empty)
                {
                    ul = fdb.AddressObjects.Where(x => x.AOLEVEL == 7 && x.FORMALNAME.Replace("ё", "е") == ad5 && x.PARENTGUID == np && x.ACTSTATUS == 1).Select(x => x.AOGUID).FirstOrDefault();
                }

                dom = fdb.Houses.Where(x => x.AOGUID == ul && ((x.HOUSENUM == ad6 && x.BUILDNUM == ad7) || x.HOUSENUM + x.BUILDNUM == ad6 || x.HOUSENUM == ad6) && x.ENDDATE > DateTime.Today).Select(x => x.HOUSEGUID).FirstOrDefault();

                if (dom == Guid.Empty)
                {
                    dom = fdb.Houses.Where(x => x.AOGUID == np && (x.HOUSENUM == ad6 || x.HOUSENUM + x.BUILDNUM == ad6 /*|| x.HOUSENUM.StartsWith(ad6.Substring(0, (ad6.Length < 2 ? ad6.Length : ad6.Length - 1))))*/) && x.ENDDATE > DateTime.Today).Select(x => x.HOUSEGUID).FirstOrDefault();
                    if (dom == Guid.Empty)
                    {
                        dom = fdb.Houses.Where(x => x.AOGUID == town && (x.HOUSENUM == ad6 || x.HOUSENUM + x.BUILDNUM == ad6 /*|| x.HOUSENUM.StartsWith(ad6.Substring(0, (ad6.Length < 2 ? ad6.Length : ad6.Length - 1))))*/) && x.ENDDATE > DateTime.Today).Select(x => x.HOUSEGUID).FirstOrDefault();

                    }
                }
                else
                {

                }

            }
            //                com_f1.CommandTimeout = 0;
            //            con_f.Open();
            //            if (ad[1] == "") return;
            //            reg = (Guid)com_f1.ExecuteScalar();
            //            if (ad[2] == "")
            //            {
            //                SqlCommand com_f3 = new SqlCommand($@"select aoguid from AddressObjects where dbo.Translit('{ad[3]}') like char(37)+dbo.Translit(FORMALNAME)+char(37) and ACTSTATUS=1
            //and PARENTGUID ='{reg}'and  AOLEVEL in(3,4)", con_f);
            //                town = (Guid)com_f3.ExecuteScalar();
            //            }
            //            else
            //            {
            //                SqlCommand com_f3 = new SqlCommand($@"select aoguid from AddressObjects where dbo.Translit('{ad[2]}') like char(37)+dbo.Translit(FORMALNAME)+char(37) and ACTSTATUS=1
            //and PARENTGUID ='{reg}'and  AOLEVEL in(3)", con_f);
            //                rn = (Guid)com_f3.ExecuteScalar();                
            //            }
            //            if (ad[3] == "")
            //            {
            //                SqlCommand com_f4 = new SqlCommand($@"select aoguid from AddressObjects where dbo.Translit('{ad[4]}') like char(37)+dbo.Translit(FORMALNAME)+char(37) and ACTSTATUS=1
            //and PARENTGUID ='{rn}'and  AOLEVEL in(4,6)", con_f);
            //                np = (Guid)com_f4.ExecuteScalar();
            //            }
            //            else
            //            {
            //                SqlCommand com_f4 = new SqlCommand($@"select aoguid from AddressObjects where dbo.Translit('{ad[3]}') like char(37)+dbo.Translit(FORMALNAME)+char(37) and ACTSTATUS=1
            //and PARENTGUID ='{(rn==Guid.Empty?reg:rn)}'and  AOLEVEL in(3,4)", con_f);
            //                town = (Guid)com_f4.ExecuteScalar();
            //            }
            //            if (ad[4] == "")
            //            {
            //                SqlCommand com_f5 = new SqlCommand($@"select aoguid from AddressObjects where dbo.Translit('{ad[5]}') like char(37)+dbo.Translit(FORMALNAME)+char(37) and ACTSTATUS=1
            //and PARENTGUID ='{town}'and  AOLEVEL in(7,90,91)", con_f);
            //                ul = (Guid)com_f5.ExecuteScalar();
            //            }
            //            else
            //            {
            //                SqlCommand com_f5 = new SqlCommand($@"select aoguid from AddressObjects where dbo.Translit('{ad[4]}') like char(37)+dbo.Translit(FORMALNAME)+char(37) and ACTSTATUS=1
            //and PARENTGUID ='{rn}'and  AOLEVEL in(6)", con_f);
            //                np = (Guid)com_f5.ExecuteScalar();
            //            }
            //            if (ad[5] == "")
            //            {
            ////                SqlCommand com_f5 = new SqlCommand($@"select aoguid from AddressObjects where '{ad[5]}' like char(37)+FORMALNAME+char(37) and ACTSTATUS=1
            ////and PARENTGUID ='{town}'and  AOLEVEL in(7,90,91)", con_f);
            ////                ul = (Guid)com_f5.ExecuteScalar();
            //            }
            //            else
            //            {
            //                SqlCommand com_f5 = new SqlCommand($@"select aoguid from AddressObjects where dbo.Translit('{ad[5]}') like char(37)+dbo.Translit(FORMALNAME)+char(37) and ACTSTATUS=1
            //and PARENTGUID ='{(town.ToString()== "00000000-0000-0000-0000-000000000000" ? np:town)}'and  AOLEVEL in(7)", con_f);
            //                ul = (Guid)com_f5.ExecuteScalar();
            //            }
            //            if (ad[6] == "")
            //            {
            //                SqlCommand com_f6 = new SqlCommand($@"select houseguid from houses 
            //                                                   where ('{ad[7]}' = strucnum ) and enddate>'{DateTime.Today}'
            //and AOGUID ='{(ul.ToString() == "00000000-0000-0000-0000-000000000000" ? np : ul)}'", con_f);
            //                dom = (Guid)com_f6.ExecuteScalar();
            //            }
            //            else
            //            {
            //                SqlCommand com_f6 = new SqlCommand($@"select top(1) houseguid from houses 
            //                                                   where (('{ad[6]}' = housenum and '{ad[7]}'=buildnum) or '{ad[6]}'+'{ad[7]}' = housenum or housenum like'{ad[6].Substring(0,ad[6].Length-1)}%') and enddate>'{DateTime.Today}'
            //and AOGUID ='{(ul.ToString() == "00000000-0000-0000-0000-000000000000" ? np : ul)}'", con_f);
            //                dom = (Guid)com_f6.ExecuteScalar();
            //            }


            con_f.Close();
            fias_d.reg.EditValue = reg;
            if (rn != Guid.Empty) fias_d.reg_rn.EditValue = rn;
            if (town != Guid.Empty) fias_d.reg_town.EditValue = town;
            if (np != Guid.Empty) fias_d.reg_np.EditValue = np;
            if (ul != Guid.Empty) fias_d.reg_ul.EditValue = ul;
            if (dom != Guid.Empty) fias_d.reg_dom.EditValue = dom;
            fias_d.reg_korp.Text = ad[7];
            fias_d.reg_kv.Text = ad[8];


        }

        private void Pol_zagr_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            fias_d.adres.Text = pol_zagr.GetFocusedRowCellValue(pol_zagr.Columns[1]).ToString();
        }

        private void Load_zah_adres_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> adr = new Dictionary<string, string>();
            SqlConnection con = new SqlConnection(Properties.Settings.Default.DocExchangeConnectionString);
            SqlCommand com = new SqlCommand($@"select n_reg,adres from polis", con);
            com.CommandTimeout = 0;
            con.Open();
            SqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                int i = 0;
                adr.Add(dr["n_reg"].ToString(), dr["adres"].ToString());

            }

            //com.ExecuteNonQuery();
            dr.Close();
            con.Close();
            pol_zagr.ItemsSource = adr;
        }
        private void Probnik()
        {
            //Spreadsheet excel = new Spreadsheet();
            //excel.ImportFromDataTable();

        }

        private void FIAS_ONLINE_Click(object sender, RoutedEventArgs e)
        {
            using (var request = new HttpRequest
            {
                IgnoreProtocolErrors = true,
                AllowAutoRedirect = true,
                KeepAlive = true

            })
            {
                List<string> Rayon = new List<string>();

            }
        }

        private void FIAS_ONLINE_Click_1(object sender, RoutedEventArgs e)
        {
            List<string> RegionZ = new List<string>();
            using (var request = new HttpRequest
            {
                IgnoreProtocolErrors = true,
                AllowAutoRedirect = true,
                KeepAlive = true

            })
            {
                request.Cookies = new CookieDictionary();
                request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
     
                request.AddHeader("accept-language", "ru,en;q=0.9");
                request.AddHeader("cache-control", "max-age=0");
                request.AddHeader("sec-fetch-dest", "document");
                request.AddHeader("sec-fetch-mode", "navigate");
                request.AddHeader("sec-fetch-site", "none");
                request.AddHeader("sec-fetch-user", "?1");
                request.AddHeader("upgrade-insecure-requests", "1");
                request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 YaBrowser/20.6.2.195 Yowser/2.5 Safari/537.36");

                var Region = request.Get("https://фиас.онлайн/").ToString();
                RegionZ = Request.RegexParses(Region, "<a href=\"", "<");

            }
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
