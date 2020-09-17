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
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Globalization;
using System.Data.SqlClient;
using Yamed.Server;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using Insurance_SPR;
using ExcelDataReader;
using Bytescout.Spreadsheet;
using WFR = System.Windows.Forms;
using Bytescout.Spreadsheet.Constants;
using Microsoft.Win32;

namespace Insurance
{
    /// <summary>
    /// Логика взаимодействия для Window8.xaml
    /// </summary>
    public class PeopleSrz
    {
        public string Q { get; set; }
        public string ENP { get; set; }
        public string SS { get; set; }
        public string FAM { get; set; }
        public string IM { get; set; }
        public string OT { get; set; }
        public Int32 W { get; set; }
        public DateTime DR { get; set; }
        public string PHONE { get; set; }
        public string NPOL { get; set; }
        public string MR { get; set; }
        public string DOCTP { get; set; }
        public string DOCS { get; set; }
        public string DOCN { get; set; }
        public DateTime DOCDT { get; set; }
        public string DOCORG { get; set; }
    }

    public partial class Polis_Up : Window
    {
        public string[] ex_path;
        public string[] polis_up_file_;
        private string call_;
        RoutedEventArgs eva;
        public Polis_Up(string[] path_ex, string[] Polis_upp_file, string call)
        {


            ////DevExpress.SpreadsheetSource.ISpreadsheetSourceOptions=sou
            ////DevExpress.SpreadsheetSource.Xls.XlsSpreadsheetSource exs = new DevExpress.SpreadsheetSource.Xls.XlsSpreadsheetSource(ex_path[0], DevExpress.SpreadsheetSource.ISpreadsheetSourceOptions);
            ////DevExpress.SpreadsheetSource.Xls.XlsSourceDataReader edr = new DevExpress.SpreadsheetSource.Xls.XlsSourceDataReader();
            ////pol_zagr.ItemsSource=DevExpress.SpreadsheetSource.Xls.XlsSourceDataReader
            //Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
            //    new Action(delegate ()
            //    {
            //                //string sh;
            //                //if (call_ == "attache")
            //                //{
            //                //    sh = "Лист1";
            //                //}
            //                //else
            //                //{
            //                //    sh = "Sheet";
            //                //}
            //                dateP.DateTime = DateTime.Now;

            //        DataTable tb = new DataTable();
            //        Spreadsheet excel = new Spreadsheet();

            //        excel.LoadFromFile(ex_path[0]);
            //        NumberFormatType formatType = excel.Worksheet(0).Cell(7, 2).ValueDataTypeByNumberFormatString;
            //                //var adr = excel.Worksheet(0).Columns[2]..Address();
            //                //excel.Worksheet(0).Range(7, 2, excel.Worksheet(0).NotEmptyRowMax, 3).CopyInto(7, 4);
            //                excel.Worksheet(0).Range("$C$7:$D$18").NumberFormatString = "дд.мм.гггг";
            //                //excel.Worksheet(0).Range(7, 2, excel.Worksheet(0).NotEmptyRowMax, 3).Formula = $"=ТЕКСТ(H5;{(char)34}дд.мм.гггг{(char)34})";
            //                //excel.Worksheets[0].Range("C1:D100000").NumberFormatString = "MM.DD.YYYY";
            //                //excel.Worksheet(0).Columns[2].NumberFormat= "m/d/yyyy";
            //                //for (int i = 1; i < excel.Worksheets[0].NotEmptyRowMax; i++)
            //                //{
            //                //    // Установить номер


            //                //    // Установить текущую ячейку
            //                //    Cell currentCell = excel.Worksheets[0].Cell(i, 3);

            //                //    // Установить дату. Это дни с 01.01.1900
            //                //    // Вы также можете конвертировать число в дату, используя функцию: DateTime.FromOADate (double d)
            //                //    ////currentCell.Value = 30000 + i * 1000;

            //                //    // Установить формат даты
            //                //    currentCell.NumberFormatString = "dd.mm.yyyy";
            //                //}
            //                //excel.Worksheet(0).Columns[3].CellFormat = ExtendedFormat//"dd:mm:yy";
            //                //excel.Worksheet(0).Range("C1:D100000").NumberFormatString= "dd.mm.yyyy hh:mm:ss";
            //                //excel.Worksheet(0).Cell(0, 0).Value = "???";
            //                //for (int y = 5; y < excel.Worksheet(0).Rows.LastFormatedRow; y++)
            //                //{
            //                //    excel.Worksheet(0).Cell(y, 3).ValueDataTypeByNumberFormatString =Bytescout.Spreadsheet.Constants.NumberFormatType.Text;

            //                //}
            //                //excel.SaveAs(ex_path[0].Replace(".xls","_1.xls"));

            //                tb = excel.ExportToDataTable(0, true);

            //                //string filename = ex_path[0];
            //                //string ConStr = String.Format("Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0}; Extended Properties=Excel 12.0;", filename);
            //                //System.Data.DataSet ds = new System.Data.DataSet("EXCEL");
            //                //OleDbConnection cn = new OleDbConnection(ConStr);
            //                //cn.Open();
            //                //DataTable schemaTable = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            //                //string sheet1 = (string)schemaTable.Rows[0].ItemArray[2];
            //                //string select = String.Format("SELECT * FROM [{0}]", sheet1);
            //                //OleDbDataAdapter ad = new OleDbDataAdapter(select, cn);
            //                //Cursor = Cursors.Wait;
            //                //ad.Fill(ds);
            //                //Cursor = Cursors.Arrow;
            //                ////Cursor = Cursors.Arrow;
            //                //tb = ds.Tables[0];
            //                rows_count = tb.Rows.Count;
            //                //cn.Close();

            //                pol_zagr.ItemsSource = tb;
            //        pol_zagr.GetRowsAsync(0, 10);
            //                ////if (call_=="attache")
            //                ////{
            //                ////    pol_zagr.SelectAll();
            //                ////}

            //            }));
            InitializeComponent();
            
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //Cursor = Cursors.Wait;
            ex_path = path_ex;
            nam_smo.Focus();
            polis_up_file_ = Polis_upp_file;
            call_ = call;
            if(call_=="attache" )
            {
                this.Title = "Загрузка прикрепления";
                nam_smo.Visibility = Visibility.Hidden;
                ns_lbl.Visibility = Visibility.Hidden;
                P060_krome.Visibility = Visibility.Hidden;
                dateP.Visibility = Visibility.Hidden;
                date_lbl.Visibility = Visibility.Hidden;
                btn_load_txt.Text = "Загрузить";
                                
            }
           
            if (call_ == "attache_tf")
            {
                this.Title = "Проставление прикрепления";
                nam_smo.Visibility = Visibility.Hidden;
                ns_lbl.Visibility = Visibility.Hidden;
                P060_krome.Visibility = Visibility.Hidden;
                dateP.Visibility = Visibility.Hidden;
                date_lbl.Visibility = Visibility.Hidden;
                upload_polises.Visibility = Visibility.Collapsed;

            }
            else
            {
                if(Vars.SMO.Substring(0,2)=="22")
                {
                    One_folder.Visibility = Visibility.Visible;
                }
                //One_folder.Visibility = Visibility.Visible;

            }
            polises_in_Loaded(this, eva);
        }
        private int rows_count;
        private DataTable tb;
        private void polises_in_Loaded_1(object sender, RoutedEventArgs e)
        {
            
            //dateP.DateTime = DateTime.Now;

            //tb = new DataTable();
            //string filename = ex_path;
            //string ConStr = String.Format("Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0}; Extended Properties=Excel 12.0;", filename);
            //System.Data.DataSet ds = new System.Data.DataSet("EXCEL");
            //OleDbConnection cn = new OleDbConnection(ConStr);
            //cn.Open();
            //DataTable schemaTable = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            //string sheet1 = (string)schemaTable.Rows[0].ItemArray[2];
            //string select = String.Format("SELECT * FROM [{0}]", sheet1);
            //OleDbDataAdapter ad = new OleDbDataAdapter(select, cn);
            //Cursor = Cursors.Wait;
            //ad.Fill(ds);
            //Cursor = Cursors.Arrow;
            ////Cursor = Cursors.Arrow;
            //tb = ds.Tables[0];
            //rows_count = tb.Rows.Count;
            //cn.Close();

            //pol_zagr.ItemsSource = tb;
            ////if (call_=="attache")
            ////{
            ////    pol_zagr.SelectAll();
            ////}
        }
        DataTable dataTable_ex;
        private void polises_in_Loaded(object sender, RoutedEventArgs e)
        {


            //StreamReader f = new StreamReader(ex_path[0]);
            //string[] a = f.ReadToEnd().Split(',');
            var file = new FileInfo(ex_path[0]);

            using (var stream = File.Open(ex_path[0], FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader reader;

                if (file.Extension.Equals(".xls"))
                    reader = ExcelDataReader.ExcelReaderFactory.CreateBinaryReader(stream);
                else if (file.Extension.Equals(".xlsx"))
                    reader = ExcelDataReader.ExcelReaderFactory.CreateOpenXmlReader(stream);
                else if (file.Extension.Equals(".csv"))
                    goto csv;
                else
                    throw new Exception("Invalid FileName");

                //// reader.IsFirstRowAsColumnNames
                var conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true

                    }
                };

                var dataSet = reader.AsDataSet(conf);
                dataTable_ex = dataSet.Tables[0];
                rows_count = dataTable_ex.Rows.Count;
                pol_zagr.ItemsSource = dataTable_ex;
                //pol_zagr1.ItemsSource = dataTable;
                //pol_zagr.ItemsSource = a;
            }
            csv:
            if (file.Extension.Equals(".csv"))
            {
                tb = new DataTable();
                string filename = file.FullName;
                string[] attache = File.ReadAllLines(filename, Encoding.GetEncoding("Windows-1251"));

                //var cls0 = attache[0].Split('|');
                var cls0 = attache[0].Split(';');
                cls0 = cls0.Where(x => x != "").ToArray();
                for (int i = 0; i < cls0.Count(); i++)
                {
                    tb.Columns.Add(cls0[i].ToString().Replace($"{(char)34}", ""), typeof(string));
                }
                //tb.Columns.AddRange();
                for (int i = 1; i < attache.Count(); i++)
                {
                    // получаем все ячейки строки
                    var row1 = attache[i].Substring(0, attache[i].Length - 1);
                    row1 = row1.Replace($"{(char)34}", "");
                    var cls = row1.Split(';');
                    //cls = cls.Where(x => x != "").ToArray();
                    tb.LoadDataRow(cls, LoadOption.Upsert);
                    //Attache_mo.Add(new ATTACHED_MO { GUID = cls[0], OKATO = cls[1], SMO = cls[2], DPFS = cls[3], SER = cls[4], NUM = cls[5], ENP = cls[6], MO = cls[7] });
                    pol_zagr.ItemsSource = tb;
                }
            }
        }
        private static string Arr_toStr(object[] a)
        {
            string sg_rows = "{";
            
            for(int i=0;i<a.Length;i++)
            {
                sg_rows = sg_rows+a[i]+",";
            }
            sg_rows = sg_rows.Substring(0, sg_rows.Length - 1)+"}";
            
            return sg_rows;
        }
        private ObservableCollection<ENP_BLANKS> ENP_s = new ObservableCollection<ENP_BLANKS>();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var yyy = pol_zagr.GetCellValue(0, pol_zagr.Columns[0]).ToString();
            if (pol_zagr.GetCellValue(0, pol_zagr.Columns[0]).ToString() == "Курская область" || pol_zagr.GetCellValue(0, pol_zagr.Columns[0]).ToString() == "Алтайский край")

            {
                int contr = 0;
                for (int i = 0; i < rows_count; i++)
                {

                    pol_zagr.View.FocusedRowHandle = i;


                    if (pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[0]).Contains(nam_smo.Text) == true)
                    {
                        contr = 1;

                    }
                    else if (contr == 1 && pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[0]).Contains("СТРАХ") == true)
                    {
                        contr = i;
                        //pol_zagr.View.FocusedRowHandle=i;
                        break;
                    }
                    else if (contr == 1 && pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[0]).Length == 16 && pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[0]).Any(c => char.IsDigit(c)))
                    {

                        pol_zagr.SelectItem(i);
                        ENP_s.Add(new ENP_BLANKS {SBLANK = pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[1]).Substring(0, 2) + " " +
                        pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[1]).Substring(2, 2),
                        NUMBLANK = pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[1]).Substring(4, 7),
                        ENP = pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[0])
                        });
                        //pol_zagr.View.MoveNextRow();
                    }
                    else if (i == rows_count - 1)
                    {
                        contr = rows_count - 1;
                    }
                    //else
                    //{
                    //    pol_zagr.GetListIndexByRowHandle(i);
                    //}
                    //else if (contr == 1 && pol_zagr.GetFocusedRowCellDisplayText("Готовые полисы").Length != 16)
                    //{
                    //    pol_zagr.UnselectItem(i);
                    //}

                }
                pol_zagr.View.FocusedRowHandle = contr;
                //pol_zagr.View.MoveLastRow();

                string sg_rows = " ";
                int[] rt = pol_zagr.GetSelectedRowHandles();
                
                List<string> sss = new List<string>();
                for (int i = 0; i < rt.Count(); i++)
                {
                    var rrr = ((DataRowView)pol_zagr.GetRow(rt[i])).Row.ItemArray;
                    sss.Add(Arr_toStr(rrr));
                }

                //{
                //    var ddd = pol_zagr.GetCellValue(rt[i], "Готовые полисы");
                //    var sgr = sg_rows.Insert(sg_rows.Length, ddd.ToString()) + ",";
                //    sg_rows = sgr;
                //}
                //sg_rows = sg_rows.Substring(0, sg_rows.Length - 1);
                //pol_zagr.SelectItem(3);
                //if (pol_zagr.GetFocusedRowCellDisplayText("Готовые полисы").Contains("СПАССКИЕ")==true)
                //{
                //    int[] spass = pol_zagr.GetSelectedRowHandles();
                //}
            }
            else //if(pol_zagr.Columns[0].HeaderCaption.ToString().Contains("Упаковка"))
            {
                for (int i = 0; i < rows_count; i++)
                {
                    pol_zagr.View.FocusedRowHandle = i;
                    //if (pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[0]) != "")
                    //{
                        if (pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[0]).Length == 16 && pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[0]).Any(c => char.IsDigit(c)))
                        {                           
                            pol_zagr.SelectItem(i);
                            ENP_s.Add(new ENP_BLANKS {SBLANK= pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[3]).Substring(0,2)+" "+
                            pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[3]).Substring(2, 2),
                            NUMBLANK= pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[3]).Substring(4, 7),
                            ENP= pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[0])
                            });
                            if (pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[4]).Length == 16)
                            {
                                ENP_s.Add(new ENP_BLANKS {SBLANK = pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[8]).Substring(0, 2) + " " +
                                pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[8]).Substring(2, 2),
                                NUMBLANK = pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[8]).Substring(4, 7),
                                ENP = pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[4])
                                });
                            }
                            if (pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[10]).Length == 16)
                            {
                                ENP_s.Add(new ENP_BLANKS {SBLANK = pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[13]).Substring(0, 2) + " " +
                                pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[13]).Substring(2, 2),
                                NUMBLANK = pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[13]).Substring(4, 7),
                                ENP = pol_zagr.GetFocusedRowCellDisplayText(pol_zagr.Columns[10])
                                });
                            }
                        }
                            
                    //}
                }
            }

        }

        private void Zagr_1()
        {

                    Cursor = Cursors.Wait;
                    int i = 0;
                    //int[] checked_items = pol_zagr.GetSelectedRowHandles();

                    for (i = 0; i < ENP_s.Count(); i++)
                    {
                         //var ttt = pol_zagr.GetCellValue(checked_items[i], pol_zagr.Columns[3]).ToString();
                         

                         var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                        SqlConnection con = new SqlConnection(connectionString);
                        //if (P060_krome.IsChecked == true)
                        //{
                            SqlCommand com = new SqlCommand($@" update POL_POLISES
set VPOLIS=3, SPOLIS='{ENP_s[i].SBLANK}', 
NPOLIS='{ENP_s[i].NUMBLANK}' , DRECEIVED='{dateP.DateTime}', DEND=(case
        when (select C_OKSM from POL_PERSONS where id=(select max(id) from POL_PERSONS where enp='{ENP_s[i].ENP}')) not in ('RUS') then DSTOP else null end) ,DSTOP=null
where EVENT_GUID=(select EVENT_GUID from POL_PERSONS
 where id=(select max(id) from POL_PERSONS where enp='{ENP_s[i].ENP}')) 

 update pol_events set tip_op='П060', unload=0 where IDGUID=(select EVENT_GUID from POL_PERSONS
 where id=(select max(id) from POL_PERSONS where enp='{ENP_s[i].ENP}')) AND tip_op<>'П060'
 update pol_persons set comment='Загружены серия и номер бланка! Файл: {polis_up_file_[0]}' where id=(select max(id) from POL_PERSONS 
where   enp='{ENP_s[i].ENP}') 
update pol_unload_history set korob='Загружены серия и номер бланка! Файл: {polis_up_file_[0]}' 
where event_guid=(select event_guid from POL_PERSONS where id=(select max(id) from POL_PERSONS 
where enp='{ENP_s[i].ENP}')) and comment like '%включен в заявку%'", con);
                            con.Open();
                            com.ExecuteNonQuery();
                            con.Close();
//                        }
//                        else
//                        {
//                            //var rtr = pol_zagr.GetCellValue(checked_items[i], "Готовые полисы").ToString();
//                            SqlCommand com = new SqlCommand($@" update POL_POLISES
//set VPOLIS=3, SPOLIS='{ENP_s[i].SBLANK}', 
//NPOLIS='{ENP_s[i].NUMBLANK}' , DRECEIVED='{dateP.DateTime}', DEND=(case
//        when (select C_OKSM from POL_PERSONS where id=(select max(id) from POL_PERSONS where enp='{ENP_s[i].ENP}')) not in ('RUS') then DSTOP else null end) ,DSTOP=null
//where PERSON_GUID=(select IDGUID from POL_PERSONS
// where id=(select max(id) from POL_PERSONS where enp='{ENP_s[i].ENP}')) 
// update pol_events set tip_op='П060', unload=0 where PERSON_GUID=(select IDGUID from POL_PERSONS
// where id=(select max(id) from POL_PERSONS where enp='{ENP_s[i].ENP}'))
// update pol_persons set comment='Загружены серия и номер бланка! Файл: {polis_up_file_[0]}' where id=(select max(id) from POL_PERSONS 
//where enp='{ENP_s[i].ENP}') 
//update pol_unload_history set korob='Загружены серия и номер бланка! Файл: {polis_up_file_[0]}' 
//where person_guid=(select idguid from POL_PERSONS where id=(select max(id) from POL_PERSONS 
//where enp='{ENP_s[i].ENP}')) and comment like '%включен в заявку%'", con);
//                            con.Open();
//                            com.ExecuteNonQuery();
//                            con.Close();
//                        }

                        //MessageBox.Show(pol_zagr.GetCellValue(checked_items[i], "Готовые полисы").ToString());


                    }
                    

                    if (polis_up_file_.Count() == 1)
                    {
                        Cursor = Cursors.Arrow;
                        string m1 = "Серии и номера бланков успешно загружены!";
                        string t1 = "Сообщение";
                        int b1 = 1;
                        Message me1 = new Message(m1, t1, b1);
                        me1.ShowDialog();
                        return;
                    }
                    else
                    {
                        
                        int y = 1;
                        for (y = 1; y <= polis_up_file_.Count() - 1; y++)
                        {

                            ENP_s.Clear();
                            Load_file(y);
                            nam_smo.Text = "СПАСС";
                            
                            Button_Click(this, new RoutedEventArgs());
                            Cursor = Cursors.Wait;
                            //checked_items = pol_zagr.GetSelectedRowHandles();
                            for (i = 0; i < ENP_s.Count(); i++)
                            {
                               //var sss = pol_zagr.GetCellValue(checked_items[i], "Готовые полисы").ToString();
                               var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                               SqlConnection con = new SqlConnection(connectionString);
                               //if (P060_krome.IsChecked == true)
                               //{
                            SqlCommand com = new SqlCommand($@" update POL_POLISES
set VPOLIS=3, SPOLIS='{ENP_s[i].SBLANK}', 
NPOLIS='{ENP_s[i].NUMBLANK}' , DRECEIVED='{dateP.DateTime}', DEND=(case
        when (select C_OKSM from POL_PERSONS where id=(select max(id) from POL_PERSONS where enp='{ENP_s[i].ENP}')) not in ('RUS') then DSTOP else null end) ,DSTOP=null
where EVENT_GUID=(select EVENT_GUID from POL_PERSONS
 where id=(select max(id) from POL_PERSONS where enp='{ENP_s[i].ENP}')) 

 update pol_events set tip_op='П060', unload=0 where IDGUID=(select EVENT_GUID from POL_PERSONS
 where id=(select max(id) from POL_PERSONS where enp='{ENP_s[i].ENP}')) AND tip_op<>'П060'
 update pol_persons set comment='Загружены серия и номер бланка! Файл: {polis_up_file_[0]}' where id=(select max(id) from POL_PERSONS 
where   enp='{ENP_s[i].ENP}') 
update pol_unload_history set korob='Загружены серия и номер бланка! Файл: {polis_up_file_[0]}' 
where event_guid=(select event_guid from POL_PERSONS where id=(select max(id) from POL_PERSONS 
where enp='{ENP_s[i].ENP}')) and comment like '%включен в заявку%'", con);
                                   con.Open();
                                   com.ExecuteNonQuery();
                                   con.Close();
//                               }
//                               else
//                               {
//                                   //var rtr = pol_zagr.GetCellValue(checked_items[i], "Готовые полисы").ToString();
//                                   SqlCommand com = new SqlCommand($@" update POL_POLISES
//set VPOLIS=3, SPOLIS='{ENP_s[i].SBLANK}', 
//NPOLIS='{ENP_s[i].NUMBLANK}' , DRECEIVED='{dateP.DateTime}', DEND=(case
//        when (select C_OKSM from POL_PERSONS where id=(select max(id) from POL_PERSONS where enp='{ENP_s[i].ENP}')) not in ('RUS') then DSTOP else null end) ,DSTOP=null
//where PERSON_GUID=(select IDGUID from POL_PERSONS
// where id=(select max(id) from POL_PERSONS where enp='{ENP_s[i].ENP}')) 
// update pol_events set tip_op='П060', unload=0 where PERSON_GUID=(select IDGUID from POL_PERSONS
// where id=(select max(id) from POL_PERSONS where enp='{ENP_s[i].ENP}'))
// update pol_persons set comment='Загружены серия и номер бланка! Файл: {polis_up_file_[y]}' where id=(select max(id) from POL_PERSONS 
//where enp='{ENP_s[i].ENP}') 
//update pol_unload_history set korob='Загружены серия и номер бланка! Файл: {polis_up_file_[y]}' 
//where person_guid=(select idguid from POL_PERSONS where id=(select max(id) from POL_PERSONS 
//where enp='{ENP_s[i].ENP}')) and comment like '%включен в заявку%'", con);
//                                   con.Open();
//                                   com.ExecuteNonQuery();
//                                   con.Close();
//                               }
                            } 
                        //MessageBox.Show(pol_zagr.GetCellValue(checked_items[i], "Готовые полисы").ToString());

                        }
                        
                    }



            var ccr = Cursor;
            Cursor = Cursors.Arrow;
            string m = "Серии и номера бланков успешно загружены!";
            string t = "Сообщение";
            int b = 1;
            Message me = new Message(m, t, b);
            me.ShowDialog();
            
        }
        
        private void nam_smo_GotFocus(object sender, RoutedEventArgs e)
        {
            InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("ru-RU");
        }

        private void nam_smo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(this, e);
            }
        }

        private void p033_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            int[] checked_items = pol_zagr.GetSelectedRowHandles();

            for (i = 0; i < checked_items.Count() - 1; i++)
            {
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand com = new SqlCommand($@" insert into pol_persons(enp,fam,im,w,dr) values('{pol_zagr.GetCellValue(checked_items[i], "Column").ToString()}',
'f','i',0,'01-01-1900 00:00:00.000' ) 
insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid) 
VALUES (newid(),getdate(),1,0,'П033',(select idguid from pol_persons where enp='{pol_zagr.GetCellValue(checked_items[i], "Column").ToString()}'))
 update pol_persons set event_guid=(select idguid from pol_events where person_guid=pol_persons.idguid),parentguid=idguid,rperson_guid='00000000-0000-0000-0000-000000000000'
 insert into pol_polises (vpolis,spolis,npolis, event_guid,dbeg,dend,DRECEIVED) values(3,'{i}','{i}',
 (select event_guid from pol_persons where enp='{pol_zagr.GetCellValue(checked_items[i], "Column").ToString()}'),getdate(),'31-12-2080 00:00:00.000',getdate())
 update pol_polises set person_guid=idguid from pol_persons where pol_polises.event_guid=pol_persons.event_guid 
 insert into pol_addresses (idguid,event_guid,fias_l1,fias_l3,fias_l4,fias_l6,fias_l7) values(newid(),
(select event_guid from pol_persons where enp='{pol_zagr.GetCellValue(checked_items[i], "Column").ToString()}'),'{Guid.Empty}','{Guid.Empty}','{Guid.Empty}','{Guid.Empty}','{Guid.Empty}')
 insert into POL_RELATION_ADDR_PERS (PERSON_GUID,ADDR_GUID,BOMG,ADDRES_G,DREG,ADDRES_P,DT1,EVENT_GUID) 
values((select idguid from pol_persons where enp='{pol_zagr.GetCellValue(checked_items[i], "Column").ToString()}'),
(select idguid from pol_addresses where event_guid=(select event_guid from pol_persons where enp='{pol_zagr.GetCellValue(checked_items[i], "Column").ToString()}')),0,1,
'01-01-1900 00:00:00.000',1,getdate(),(select event_guid from pol_persons where enp='{pol_zagr.GetCellValue(checked_items[i], "Column").ToString()}')) 
 insert into pol_personsb(type,photo,person_guid) values(2,'',(select idguid from pol_persons where enp='{pol_zagr.GetCellValue(checked_items[i], "Column").ToString()}')),
 (3,'',(select idguid from pol_persons where enp='{pol_zagr.GetCellValue(checked_items[i], "Column").ToString()}'))
 insert into pol_documents(idguid,person_guid,oksm,doctype,docnum,docdate,event_guid) values(newid(),(select idguid from pol_persons where enp='{pol_zagr.GetCellValue(checked_items[i], "Column").ToString()}'),
'RUS',14,'000000','01-01-1900 00:00:00.000',(select event_guid from pol_persons where enp='{pol_zagr.GetCellValue(checked_items[i], "Column").ToString()}'))
 insert into POL_RELATION_DOC_PERS (person_guid,doc_guid,dt,event_guid) values((select idguid from pol_persons where enp='{pol_zagr.GetCellValue(checked_items[i], "Column").ToString()}'),
(select idguid from pol_documents where person_guid=(select idguid from pol_persons where enp='{pol_zagr.GetCellValue(checked_items[i], "Column").ToString()}')),getdate(),
(select event_guid from pol_persons where enp='{pol_zagr.GetCellValue(checked_items[i], "Column").ToString()}'))", con);

                con.Open();


                com.ExecuteNonQuery();
                //MessageBox.Show(pol_zagr.GetCellValue(checked_items[i], "Column1").ToString());

                con.Close();
            }
            MessageBox.Show("Серии и номера бланков успешно загружены!");

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            string sg_rows = " ";
            int[] rt = pol_zagr.GetSelectedRowHandles();
            for (int i = 0; i < rt.Count(); i++)

            {
                var ddd = pol_zagr.GetCellValue(rt[i], "Column");
                var sgr = sg_rows.Insert(sg_rows.Length, "'" + ddd.ToString()).Trim() + "'" + ",";
                sg_rows = sgr;
            }
            //MessageBox.Show(sg_rows.Substring(0, sg_rows.Length - 1));
            var connectionString = Properties.Settings.Default.srzConnectionString;
            {
                var peopleSrzList =
                Reader2List.CustomSelect<PeopleSrz>(
                    $@"
            SELECT Q,ENP,FAM , IM  , OT ,W ,DR , PHONE ,NPOL,MR,DOCTP,DOCS,DOCN,DOCDT,DOCORG,SS 
  FROM [dbo].[PEOPLE]
where enp in({sg_rows.Substring(0, sg_rows.Length - 1)}) and q='46001'", connectionString);

                pol_zagr1.ItemsSource = peopleSrzList;
                pol_zagr1.RefreshData();
                //MessageBox.Show(sg_rows.Substring(0, sg_rows.Length - 1));
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string ss;
            string phone;
            string fam;
            string im;
            string ot;
            string w;
            string dr;
            string mr;
            string doctp;
            string docs;
            string docn;
            string docdt;
            string docorg;
            int i = 0;
            int[] checked_items1 = pol_zagr1.GetSelectedRowHandles();


            //MessageBox.Show(pol_zagr1.GetCellValue(checked_items1[0], "SS").ToString() + ", " + pol_zagr1.GetCellValue(checked_items1[0], "PHONE").ToString() + ", " + pol_zagr1.GetCellValue(checked_items1[0], "NPOL").ToString().Substring(0, 2) + " " + pol_zagr1.GetCellValue(checked_items1[0], "NPOL").ToString().Substring(2, 2));
            for (i = 0; i < checked_items1.Count() - 1; i++)
            {
                if (pol_zagr1.GetCellValue(checked_items1[i], "DOCORG") == null)
                {
                    docorg = "";
                }
                else
                {
                    docorg = pol_zagr1.GetCellValue(checked_items1[i], "DOCORG").ToString();
                }
                if (pol_zagr1.GetCellValue(checked_items1[i], "DOCDT") == null)
                {
                    docdt = "";
                }
                else
                {
                    docdt = pol_zagr1.GetCellValue(checked_items1[i], "DOCDT").ToString();
                }
                if (pol_zagr1.GetCellValue(checked_items1[i], "DOCN") == null)
                {
                    docn = "";
                }
                else
                {
                    docn = pol_zagr1.GetCellValue(checked_items1[i], "DOCN").ToString();
                }
                if (pol_zagr1.GetCellValue(checked_items1[i], "DOCS") == null)
                {
                    docs = "";
                }
                else
                {
                    docs = pol_zagr1.GetCellValue(checked_items1[i], "DOCS").ToString();
                }
                if (pol_zagr1.GetCellValue(checked_items1[i], "DOCTP") == null)
                {
                    doctp = "";
                }
                else
                {
                    doctp = pol_zagr1.GetCellValue(checked_items1[i], "DOCTP").ToString();
                }
                if (pol_zagr1.GetCellValue(checked_items1[i], "MR") == null)
                {
                    mr = "";
                }
                else
                {
                    mr = pol_zagr1.GetCellValue(checked_items1[i], "MR").ToString();
                }
                if (pol_zagr1.GetCellValue(checked_items1[i], "DR") == null)
                {
                    dr = "";
                }
                else
                {
                    dr = pol_zagr1.GetCellValue(checked_items1[i], "DR").ToString();
                }
                if (pol_zagr1.GetCellValue(checked_items1[i], "W") == null)
                {
                    w = "";
                }
                else
                {
                    w = pol_zagr1.GetCellValue(checked_items1[i], "W").ToString();
                }
                if (pol_zagr1.GetCellValue(checked_items1[i], "OT") == null)
                {
                    ot = "";
                }
                else
                {
                    ot = pol_zagr1.GetCellValue(checked_items1[i], "OT").ToString();
                }
                if (pol_zagr1.GetCellValue(checked_items1[i], "IM") == null)
                {
                    im = "";
                }
                else
                {
                    im = pol_zagr1.GetCellValue(checked_items1[i], "IM").ToString();
                }
                if (pol_zagr1.GetCellValue(checked_items1[i], "FAM") == null)
                {
                    fam = "";
                }
                else
                {
                    fam = pol_zagr1.GetCellValue(checked_items1[i], "FAM").ToString();
                }
                if (pol_zagr1.GetCellValue(checked_items1[i], "SS") == null)
                {
                    ss = "";
                }
                else
                {
                    ss = pol_zagr1.GetCellValue(checked_items1[i], "SS").ToString();
                }
                if (pol_zagr1.GetCellValue(checked_items1[i], "PHONE") == null)
                {
                    phone = "";
                }
                else
                {
                    phone = pol_zagr1.GetCellValue(checked_items1[i], "PHONE").ToString();
                }
                var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                SqlConnection con = new SqlConnection(connectionString1);
                SqlCommand com = new SqlCommand($@"update pol_persons set ss='{ss}',
fam='{fam}',im='{im}',ot='{ot}',w={Convert.ToInt32(w)},mr='{mr}',dr='{Convert.ToDateTime(dr)}',phone='{phone}' where enp='{pol_zagr1.GetCellValue(checked_items1[i], "ENP").ToString()}'
update pol_documents set doctype={Convert.ToInt32(doctp)},docser='{docs}',docnum='{docn}',docdate='{Convert.ToDateTime(docdt)}',docmr='{mr}',name_vp='{docorg}' 
where person_guid=(select idguid from pol_persons where enp='{pol_zagr1.GetCellValue(checked_items1[i], "ENP").ToString()}')
 update pol_polises set spolis='{pol_zagr1.GetCellValue(checked_items1[i], "NPOL").ToString().Substring(0, 2)}'+' '+'{pol_zagr1.GetCellValue(checked_items1[i], "NPOL").ToString().Substring(2, 2)}'
, npolis='{pol_zagr1.GetCellValue(checked_items1[i], "NPOL").ToString().Substring(4, 7)}' 
where person_guid=(select idguid from pol_persons where enp='{pol_zagr1.GetCellValue(checked_items1[i], "ENP").ToString()}')
 
", con);
                //com.CommandTimeout = 180;
                con.Open();

                com.ExecuteNonQuery();
                //MessageBox.Show(pol_zagr.GetCellValue(checked_items[i], "Готовые полисы").ToString());

                con.Close();
            }
            var connectionString2 = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con1 = new SqlConnection(connectionString2);

            SqlCommand com1 = new SqlCommand($@"
            delete from pol_polises where person_guid in(select idguid from pol_persons where fam = 'f')
delete from POL_RELATION_DOC_PERS where person_guid in(select idguid from pol_persons where fam = 'f')
delete from POL_DOCUMENTS where person_guid in(select idguid from pol_persons where fam = 'f')
delete from POL_PERSONSB where person_guid in(select idguid from pol_persons where fam = 'f')
delete from POL_RELATION_ADDR_PERS where person_guid in(select idguid from pol_persons where fam = 'f')
delete from POL_ADDRESSES where event_guid in(select event_guid from pol_persons where fam = 'f')
delete from POL_PERSONS where fam = 'f'
 delete from POL_EVENTS where idguid not in(select event_guid from pol_persons) ", con1);
            //com1.CommandTimeout = 180;

            con1.Open();

            com1.ExecuteNonQuery();
            //MessageBox.Show(pol_zagr.GetCellValue(checked_items[i], "Готовые полисы").ToString());

            con1.Close();
            MessageBox.Show("Данные успешно загружены!");
        }
        private void Zagr_2()
        {
            if (call_ == "attache" && Vars.SMO.StartsWith("22")==false && Vars.SMO.StartsWith("39") == false)
            {
                int i = 0;
                int[] checked_items = pol_zagr.GetSelectedRowHandles();
                for (i = 0; i < checked_items.Count(); i++)
                {
                    var uu = pol_zagr.GetCellValue(checked_items[i], "mo").ToString();
                    var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                    SqlConnection con = new SqlConnection(connectionString);
                    SqlCommand com = new SqlCommand($@" update POL_PERSONS
                set mo='{pol_zagr.GetCellValue(checked_items[i], "mo").ToString()}', dstart=getdate() 
                where enp='{pol_zagr.GetCellValue(checked_items[i], "mo").ToString()}' ", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    com.ExecuteNonQuery();
                    con.Close();
                }
            }
            else if (call_ == "attache" && Vars.SMO.StartsWith("39") == true)
            {
                int i = 0;
                int[] checked_items = pol_zagr.GetSelectedRowHandles();
                for (i = 0; i < checked_items.Count(); i++)
                {
                    var uu = pol_zagr.GetCellValue(checked_items[i], "lpu").ToString();
                    var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                    SqlConnection con = new SqlConnection(connectionString);
                    SqlCommand com = new SqlCommand($@" update POL_PERSONS
                set mo='{pol_zagr.GetCellValue(checked_items[i], "lpu").ToString()}', dstart=getdate() 
                where enp='{pol_zagr.GetCellValue(checked_items[i], "enp").ToString()}' ", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    com.ExecuteNonQuery();
                    con.Close();
                }
            }
            else if (call_ == "attache" && Vars.SMO.StartsWith("22") == true)
            {
                var con_str = Properties.Settings.Default.DocExchangeConnectionString;
                SqlConnection con = new SqlConnection(con_str);
                SqlCommand com = new SqlCommand($@"create table [dbo].[Attache](
	[ENP] [nvarchar](16) NULL,
	[MO] [nvarchar](6) NULL) ON [PRIMARY]", con);
                SqlCommand com1 = new SqlCommand($@"update POL_PERSONS  set mo=a.mo,DSTART=GETDATE() 
  from Attache a
  where POL_PERSONS.enp=a.enp 
drop table [dbo].[Attache]", con);
                con.Open();
                com.ExecuteNonQuery();
                SqlBulkCopy bulk = new SqlBulkCopy(con);
                bulk.DestinationTableName = "dbo.Attache";
                bulk.BulkCopyTimeout = 0;
                bulk.BatchSize = 100000;
                bulk.WriteToServer(tb);
                com1.ExecuteNonQuery();
                con.Close();
                
            }

            string m = "Прикрепление успешно загружено!";
            string t = "Сообщение";
            int b = 1;
            Message me = new Message(m, t, b);
            me.ShowDialog();
            return;
        }
        private void upload_polises_Click(object sender, RoutedEventArgs e)
        {
            if (call_=="attache")
            {
                Zagr_2();
            }            
            else
            {
                Zagr_1();
            }
        }
        private void Load_file(int y)
        {
            dateP.DateTime = DateTime.Now;
            tb = new DataTable();
            Spreadsheet excel = new Spreadsheet();
            excel.LoadFromFile(ex_path[y]);
            tb = excel.ExportToDataTable("Sheet", true);
            //tb = new DataTable();
            //string filename = ex_path[y];
            //string ConStr = String.Format("Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0}; Extended Properties=Excel 12.0;", filename);
            //System.Data.DataSet ds = new System.Data.DataSet("EXCEL");
            //OleDbConnection cn = new OleDbConnection(ConStr);
            //cn.Open();
            //DataTable schemaTable = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            //string sheet1 = (string)schemaTable.Rows[0].ItemArray[2];
            //string select = String.Format("SELECT * FROM [{0}]", sheet1);
            //OleDbDataAdapter ad = new OleDbDataAdapter(select, cn);
            //Cursor = Cursors.Wait;
            //ad.Fill(ds);
            //Cursor = Cursors.Arrow;
            ////Cursor = Cursors.Arrow;
            //tb = ds.Tables[0];
            rows_count = tb.Rows.Count;
            //cn.Close();

            pol_zagr.ItemsSource = tb;
            
            //if (call_=="attache")
            //{
            //    pol_zagr.SelectAll();
            //}
        }

        private void Pol_zagr_Loaded(object sender, RoutedEventArgs e)
        {

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

        private void Upd_attache_tfoms_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> att = new Dictionary<string, string>();
            DataTable dt = new DataTable();
            int ii = 1;
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("enp", typeof(string));
            dt.Columns.Add("mo_s", typeof(string));
            var con_str = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(con_str);
            SqlCommand com = new SqlCommand($@"select enp,mo from pol_persons where enp is not null and enp!=''",con);
            
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while(reader.Read())
            {
                
                string _enp = reader["enp"].ToString();
                string _mo = reader["mo"].ToString();
               
                dt.LoadDataRow(new object[] { ii, _enp, _mo },true);
                ii = ii + 1;
            }

            var result = from exDataRow in dataTable_ex.AsEnumerable()
                         join sqlDataRow in dt.AsEnumerable()
                         on exDataRow.Field<string>("ЕНП") equals sqlDataRow.Field<string>("enp")
                         into gres
                         from subpet in gres.DefaultIfEmpty()
                         select new
                         {
                             ЕНП = exDataRow.Field<string>("ЕНП"),
                             /* other DataTable row values here */

                             mo =subpet?.Field<string>("mo_s")??string.Empty
                         };
            pol_zagr.ItemsSource = result;
            pol_zagr.RefreshData();

            SaveFileDialog SF = new SaveFileDialog();
            SF.DefaultExt = ".xls";
            SF.Filter = "Файлы Excel (.xls; .xlsx)|*.xls;*.xlsx";
            bool res = SF.ShowDialog().Value;
            if(res)
            {
                string fname = SF.FileName;
                pol_zagr.View.ExportToXls(fname);

                //pers_grid.View.ShowPrintPreview(this);
                string m = $@"Файл успешно сохранен и находится в папке:
                {SF.FileName.Replace(SF.SafeFileName, "")}";
                string t = "Сообщение";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
            }

        }
    }
}
