using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Data.SqlClient;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
//using Ionic.Zip;
using System.Data;
using System.Reflection;
using DevExpress.Xpf.Grid;
using DevExpress.Data.Filtering;
using System.Collections.ObjectModel;
using System.Net;
using System.Xml.Linq;
using Insurance_SPR;
using DevExpress.Xpf.Editors;
using System.Threading;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Globalization;
using System.Windows.Threading;
using System.Collections;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using DotNetDBF;
using Bytescout.Spreadsheet;
using Ionic.Zip;
using System.Xml;
using Insurance.Classes;

namespace Insurance
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    
    public static class BusinessDays
    {
        public static System.DateTime AddBusinessDays(this System.DateTime source, int businessDays)
        {
            var dayOfWeek = businessDays < 0
                                ? ((int)source.DayOfWeek - 12) % 7
                                : ((int)source.DayOfWeek + 6) % 7;

            switch (dayOfWeek)
            {
                case 6:
                    businessDays--;
                    break;
                case -6:
                    businessDays++;
                    break;
            }

            return source.AddDays(businessDays + ((businessDays + dayOfWeek) / 5) * 2);
        }
    }
    
    public class MyReader
    {        
        public static List<T> MySelect<T>(string selectCmd, string connectionString)
        {
                List<T> list;
            
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(selectCmd, con))
                    {
                        con.Open();
                        cmd.CommandTimeout = 0;
                        SqlDataReader dr = cmd.ExecuteReader();
                        list = DataReaderMapToList<T>(dr);
                        dr.Close();
                    }

                    con.Close();
                }
            
            return list;
        }
        public static List<T> MySelect<T>(string com, string connectionString, List<T> ids)
        {
            List<T> list;
            var dt = new DataTable();
            var dtc = Get_Fields_Dt<T>();
            //dtc.Sort();
            string sqltype = "";
            dt = ToDataTable<T>(ids);
            foreach(var d in dtc)
            {
                //dt.Columns.Add(d.NAME,d.TYPE);
                string s;
                switch(d.TYPE.Name)
                {
                    case "Int32":
                         s = "int";
                         break;
                    case "String":
                        s = "nvarchar(500)";
                        break;
                    case "Guid":
                        s = "uniqueidentifier";
                        break;
                    case "Boolean":
                        s = "bit";
                        break;
                    case "DateTime":
                        s = "DateTime2";
                        break;
                    default:
                        s = d.TYPE.Name;
                        break;
                }
                    
                sqltype = sqltype + d.NAME + " " + s + ",";
                
            }
            sqltype = sqltype.Substring(0, sqltype.Length - 1);
            //foreach (var item in ids)
            //{
                
            //    dt.Rows.Add(item);
                
            //}
            
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd0 = new SqlCommand($@" 
IF exists (select * from sys.table_types where name='ForSelect')  
DROP TYPE dbo.ForSelect   
CREATE TYPE ForSelect AS TABLE ({sqltype})", con))
                {
                    con.Open();
                    cmd0.ExecuteNonQuery();
                    con.Close();
                }
                using (SqlCommand cmd = new SqlCommand(com, con))
                {
                    SqlParameter t = cmd.Parameters.AddWithValue("@t", dt);
                    t.SqlDbType = SqlDbType.Structured;
                    t.TypeName = "dbo.ForSelect";
                    con.Open();
                    cmd.CommandTimeout = 0;
                    SqlDataReader dr = cmd.ExecuteReader();
                    list = DataReaderMapToList<T>(dr);
                    dr.Close();
                }

                con.Close();
            }

            return list;
        }
        public static void UpdateFromTable<T>(string com, string connectionString, DataTable dt)
        {
            string sqltype = "";
           
            
            foreach (DataColumn dc in dt.Columns)
            {
                //dt.Columns.Add(d.NAME,d.TYPE);
                string s;
                switch (dc.DataType.Name.ToString())
                {
                    case "Int32":
                        s = "int";
                        break;
                    case "String":
                        s = "nvarchar(500)";
                        break;
                    case "Guid":
                        s = "uniqueidentifier";
                        break;
                    case "Boolean":
                        s = "bit";
                        break;
                    case "DateTime":
                        s = "DateTime2";
                        break;
                    case "Decimal":
                        s = "numeric(10,2)";
                        break;
                    default:
                        s = dc.DataType.Name.ToString();
                        break;
                }

                sqltype = sqltype + dc.ColumnName + " " + s + ",";

            }
            sqltype = sqltype.Substring(0, sqltype.Length - 1);
            //foreach (var item in ids)
            //{

            //    dt.Rows.Add(item);

            //}
        
            SqlConnection con = new SqlConnection(connectionString);
            
              SqlCommand cmd0 = new SqlCommand($@" 
IF exists (select * from sys.table_types where name='ForUpdate')  
DROP TYPE dbo.ForUpdate   
CREATE TYPE ForUpdate AS TABLE ({sqltype})", con);     
              
            SqlCommand cmd = new SqlCommand(com, con);
                
                    var t = new SqlParameter("@dt", SqlDbType.Structured);
                    t.TypeName = "dbo.ForUpdate";
                    t.Value = dt;
                    cmd.Parameters.Add(t);
                    //SqlParameter t = cmd.Parameters.AddWithValue("@t", dt);
                    //t.SqlDbType = SqlDbType.Structured;
                    //t.TypeName = "dbo.ForUpdate";
                    
                        cmd.CommandTimeout = 0;
                        con.Open();
                        cmd0.ExecuteNonQuery();
                        int str = cmd.ExecuteNonQuery();
                        int isrt = str;
                        con.Close();    
            

           
        }
        public static DataTable ToDataTable<T>(List<T> items)

        {

            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)

            {

                //Setting column names as Property names

                dataTable.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments().Count() > 0 ? prop.PropertyType.GetGenericArguments()[0] : prop.PropertyType);

            }

            foreach (T item in items)

            {

                var values = new object[Props.Length];

                for (int i = 0; i < Props.Length; i++)

                {

                    //inserting property values to datatable rows

                    values[i] = Props[i].GetValue(item, null);

                }

                dataTable.Rows.Add(values);

            }

            //put a breakpoint here and check datatable

            return dataTable;

        }
        
        public static List<Class_params> Get_Fields_Dt<T>()
        {
            List<Class_params> list= new List<Class_params>();
            T obj = default(T);
            obj = Activator.CreateInstance<T>();
            Type type = obj.GetType();
            var props = type.GetProperties();
            foreach (var propertyInfo in props)
            {
                list.Add(new Class_params {NAME= propertyInfo.Name,TYPE=propertyInfo.PropertyType.GetGenericArguments().Count()>0 ? propertyInfo.PropertyType.GetGenericArguments()[0] : propertyInfo.PropertyType });
            }
            return list;
        }
        
        public static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);

            obj = Activator.CreateInstance<T>();
            var propList = obj.GetType().GetProperties().Where(IsAcceptableDbType).ToList();

            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in propList)
                {
                    if (!dr.IsDBNull(dr.GetOrdinal(prop.Name)))
                    {
                        prop.SetValue(obj, dr.GetValue(dr.GetOrdinal(prop.Name)), null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }
        public static bool IsAcceptableDbType(PropertyInfo pi)
        {
            return (pi.PropertyType.BaseType ==
                       Type.GetType("System.ValueType") ||
                       pi.PropertyType ==
                       Type.GetType("System.String") ||
                       pi.PropertyType ==
                       Type.GetType("System.Byte[]")
                       );
        }
        
        static public string MyExecuteWithResult(string command, string connectionString)
        {
            string res = "Done";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(command, con))
                {
                    con.Open();
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return res;
        }

    }


    public class Funcs
    {
        public static string MyIds(int[] ids, DevExpress.Xpf.Grid.GridControl grid)
        {

            string sg_rows = "";
            int[] rt = ids;
            for (int i = 0; i < rt.Count(); i++)

            {
                var ddd = grid.GetCellValue(rt[i], "ID");
                var sgr = sg_rows.Insert(sg_rows.Length, ddd.ToString()) + ",";
                sg_rows = sgr;
            }

            sg_rows = sg_rows.Substring(0, sg_rows.Length - 1);
            return sg_rows;


        }
        public static string MyIds(int[] ids)
        {

            string sg_rows = "";
            int[] rt = ids;
            for (int i = 0; i < rt.Count(); i++)

            {
                var ddd = rt[i];
                var sgr = sg_rows.Insert(sg_rows.Length, ddd.ToString()) + ",";
                sg_rows = sgr;
            }

            sg_rows = sg_rows.Substring(0, sg_rows.Length - 1);
            return sg_rows;
        }

        public static string MyFields(string[] fields)
        {

            string sg_rows = "";
            string[] rt = fields;
            for (int i = 0; i < rt.Count(); i++)

            {
                var ddd = rt[i];
                var sgr = sg_rows.Insert(sg_rows.Length, ddd.ToString()) + ",";
                sg_rows = sgr;
            }

            sg_rows = sg_rows.Substring(0, sg_rows.Length - 1);
            return sg_rows;


        }
        public static string MyFieldValues(int[] ids, DevExpress.Xpf.Grid.GridControl grid,string field)
        {

            string sg_rows = "";
            int[] rt = ids.Where(x=>x>=0).ToArray();
            for (int i = 0; i < rt.Count(); i++)

            {
                var ddd = grid.GetCellValue(rt[i], field);
                var sgr = sg_rows.Insert(sg_rows.Length, ddd.ToString()) + ",";
                sg_rows = sgr;
            }

            sg_rows = sg_rows.Substring(0, sg_rows.Length - 1);
            return sg_rows;


        }
        public static string[] MyFieldValuesArray(int[] ids, DevExpress.Xpf.Grid.GridControl grid, string field)
        {
            int[] rt = ids.Distinct().Where(x => x >= 0).ToArray();
            string[] myArr = new string[rt.Count()];            
            for (int i = 0; i < rt.Count(); i++)
            {
                myArr[i] = grid.GetCellValue(rt[i], field).ToString();                
            }
            
            return myArr;


        }

    }

    public partial class MainWindow : Window
    {
        RoutedCommand newCmd = new RoutedCommand();
        public static ObservableCollection<Events> events_g1;
        bool tttab;
        //string _iid;
        //string petition;
        string dorder;
        string btn_;
        RoutedEventArgs e;
        public string strf_adm = "order by pe.ID DESC";
        public string strf_usr = $@"WHERE pe.AGENT =" + Vars.Agnt + " order by pe.ID DESC";

        public string doc_ex_con = Properties.Settings.Default.DocExchangeConnectionString;

        //List<DateTime> h = HOLIDAYS.H_List;
        private static int Get_holidays(DateTime h_start, DateTime h_end)
        {
            HOLIDAYS.doc_ex_con = Properties.Settings.Default.DocExchangeConnectionString;
            List<DateTime> h = HOLIDAYS.H_List;
            List<DateTime> w = HOLIDAYS.W_List;
            for (int i = 0; i < h.Count; i++)
            {
                if (h_end == h[i] || (h_end.DayOfWeek == DayOfWeek.Saturday || h_end.DayOfWeek == DayOfWeek.Sunday && w.Exists(x => x == h_end) == false))
                {
                    h_end = h_end.AddDays(1);
                    i = i - 1;
                }
            }

            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand($@"select count(H_DATE) from SPR_HOLIDAYS where H_DATE > '{h_start}' and H_DATE < '{h_end}' and status='1'", con);
            SqlCommand comm1 = new SqlCommand($@"select count(H_DATE) from SPR_HOLIDAYS where H_DATE between '{h_start}' and '{h_end}' and status in('2','3')", con);
            con.Open();
            int h_kol = (int)comm.ExecuteScalar();
            int w_kol = (int)comm1.ExecuteScalar();
            con.Close();
            return h_kol - w_kol;
        }


        private void Obnovit_ItemClick(object sender, System.EventArgs e)
        {
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            var peopleList =
                MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf_adm, connectionString);
            pers_grid.ItemsSource = peopleList;
            G_layuot.save_Layout(Properties.Settings.Default.DocExchangeConnectionString, pers_grid, pers_grid_2);
            ////restore_Layout();
            ////layout_InUse();
         List<F003> molist = MyReader.MySelect<F003>($@"SELECT mcod,namewithid from f003 order by mcod", Properties.Settings.Default.DocExchangeConnectionString);
         List<V005> pol_DataContext = MyReader.MySelect<V005>(@"select IDPOL,NameWithID from SPR_79_V005", Properties.Settings.Default.DocExchangeConnectionString);
         List<F011> doc_type_DataContext = MyReader.MySelect<F011>(@"select ID,NameWithID from SPR_79_F011", Properties.Settings.Default.DocExchangeConnectionString);
         List<OKSM> str_vid_DataContext = MyReader.MySelect<OKSM>(@"select ID,CAPTION from SPR_79_OKSM", Properties.Settings.Default.DocExchangeConnectionString);
         List<V013> kat_zl_DataContext = MyReader.MySelect<V013>(@"select IDKAT,NameWithID from V013", Properties.Settings.Default.DocExchangeConnectionString);
         List<F008> type_policy_DataContext = MyReader.MySelect<F008>(@"select ID,NameWithID from SPR_79_F008", Properties.Settings.Default.DocExchangeConnectionString);
         List<FIO> fio_col = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
         List<FIO> im_DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
         List<FIO> ot_DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
         List<NAME_VP> kem_vid_DataContext = MyReader.MySelect<NAME_VP>(@"select id,name from spr_namevp", Properties.Settings.Default.DocExchangeConnectionString);
    }

        public List<F003> molist = MyReader.MySelect<F003>($@"SELECT mcod,namewithid from f003 order by mcod", Properties.Settings.Default.DocExchangeConnectionString);
        public List<V005> pol_DataContext = MyReader.MySelect<V005>(@"select IDPOL,NameWithID from SPR_79_V005", Properties.Settings.Default.DocExchangeConnectionString);
        //public List<V005> pol_pr_DataContext = MyReader.MySelect<V005>(@"select IDPOL,NameWithID from SPR_79_V005", Properties.Settings.Default.DocExchangeConnectionString);
        public List<F011> doc_type_DataContext = MyReader.MySelect<F011>(@"select ID,NameWithID from SPR_79_F011", Properties.Settings.Default.DocExchangeConnectionString);
        //public List<F011> doctype1_DataContext = MyReader.MySelect<F011>(@"select ID,NameWithID from SPR_79_F011", Properties.Settings.Default.DocExchangeConnectionString);
        //public List<F011> doc_type1_DataContext = MyReader.MySelect<F011>(@"select ID,NameWithID from SPR_79_F011", Properties.Settings.Default.DocExchangeConnectionString);
        //public List<F011> ddtype_DataContext = MyReader.MySelect<F011>(@"select ID,NameWithID from SPR_79_F011", Properties.Settings.Default.DocExchangeConnectionString);
        public List<OKSM> str_vid_DataContext = MyReader.MySelect<OKSM>(@"select ID,CAPTION from SPR_79_OKSM", Properties.Settings.Default.DocExchangeConnectionString);
        //public List<OKSM> str_vid1_DataContext = MyReader.MySelect<OKSM>(@"select ID,CAPTION from SPR_79_OKSM", Properties.Settings.Default.DocExchangeConnectionString);
        //public List<OKSM> str_r_DataContext = MyReader.MySelect<OKSM>(@"select ID,CAPTION from SPR_79_OKSM", Properties.Settings.Default.DocExchangeConnectionString);
        //public List<OKSM> gr_DataContext = MyReader.MySelect<OKSM>(@"select ID,CAPTION from SPR_79_OKSM", Properties.Settings.Default.DocExchangeConnectionString);
        public List<V013> kat_zl_DataContext = MyReader.MySelect<V013>(@"select IDKAT,NameWithID from V013", Properties.Settings.Default.DocExchangeConnectionString);
        public List<F008> type_policy_DataContext = MyReader.MySelect<F008>(@"select ID,NameWithID from SPR_79_F008", Properties.Settings.Default.DocExchangeConnectionString);
        //public List<V005> prev_pol_DataContext = MyReader.MySelect<V005>(@"select IDPOL,NameWithID from SPR_79_V005", Properties.Settings.Default.DocExchangeConnectionString);
        //kem_vid.DataContext = MyReader.MySelect<NAMEVP>(@"select distinct name_vp from pol_documents order by name_vp",connectionString);
        // LoadingDecorator1.IsSplashScreenShown = false;
        public List<FIO> fio_col = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);        
        public List<FIO> im_DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
        public List<FIO> ot_DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
        public List<NAME_VP> kem_vid_DataContext = MyReader.MySelect<NAME_VP>(@"select id,name from spr_namevp", Properties.Settings.Default.DocExchangeConnectionString);
        //public List<FIO> fam1_DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
        //public List<FIO> im1_DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
        //public List<FIO> ot1_DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
        //public List<FIO> prev_fam_DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
        //public List<FIO> prev_im_DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
        //public List<FIO> prev_ot_DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
        public MainWindow()
        {
 //           Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
 //               new Action(delegate ()
 //               {
 //                   var molist =
 //                           MyReader.MySelect<F003>(
 //                               $@"
 //           SELECT mcod,namewithid from f003
 //order by mcod", Properties.Settings.Default.DocExchangeConnectionString);
                    

 //                   pol.DataContext = MyReader.MySelect<V005>(@"select IDPOL,NameWithID from SPR_79_V005", Properties.Settings.Default.DocExchangeConnectionString);
 //                   pol_pr.DataContext = MyReader.MySelect<V005>(@"select IDPOL,NameWithID from SPR_79_V005", Properties.Settings.Default.DocExchangeConnectionString);
 //                   doc_type.DataContext = MyReader.MySelect<F011>(@"select ID,NameWithID from SPR_79_F011", Properties.Settings.Default.DocExchangeConnectionString);
 //                   doctype1.DataContext = MyReader.MySelect<F011>(@"select ID,NameWithID from SPR_79_F011", Properties.Settings.Default.DocExchangeConnectionString);
 //                   doc_type1.DataContext = MyReader.MySelect<F011>(@"select ID,NameWithID from SPR_79_F011", Properties.Settings.Default.DocExchangeConnectionString);
 //                   ddtype.DataContext = MyReader.MySelect<F011>(@"select ID,NameWithID from SPR_79_F011", Properties.Settings.Default.DocExchangeConnectionString);
 //                   str_vid.DataContext = MyReader.MySelect<OKSM>(@"select ID,CAPTION from SPR_79_OKSM", Properties.Settings.Default.DocExchangeConnectionString);
 //                   str_vid1.DataContext = MyReader.MySelect<OKSM>(@"select ID,CAPTION from SPR_79_OKSM", Properties.Settings.Default.DocExchangeConnectionString);
 //                   str_r.DataContext = MyReader.MySelect<OKSM>(@"select ID,CAPTION from SPR_79_OKSM", Properties.Settings.Default.DocExchangeConnectionString);
 //                   gr.DataContext = MyReader.MySelect<OKSM>(@"select ID,CAPTION from SPR_79_OKSM", Properties.Settings.Default.DocExchangeConnectionString);
 //                   kat_zl.DataContext = MyReader.MySelect<V013>(@"select IDKAT,NameWithID from V013", Properties.Settings.Default.DocExchangeConnectionString);
 //                   type_policy.DataContext = MyReader.MySelect<F008>(@"select ID,NameWithID from SPR_79_F008", Properties.Settings.Default.DocExchangeConnectionString);
 //                   prev_pol.DataContext = MyReader.MySelect<V005>(@"select IDPOL,NameWithID from SPR_79_V005", Properties.Settings.Default.DocExchangeConnectionString);
 //                   //kem_vid.DataContext = MyReader.MySelect<NAMEVP>(@"select distinct name_vp from pol_documents order by name_vp",connectionString);
 //                   // LoadingDecorator1.IsSplashScreenShown = false;
 //                   fio_col = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
 //                   fam.DataContext = fio_col;
 //                   im.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
 //                   ot.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
 //                   kem_vid.DataContext = MyReader.MySelect<NAME_VP>(@"select id,name from spr_namevp", Properties.Settings.Default.DocExchangeConnectionString);
 //                   fam1.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
 //                   im1.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
 //                   ot1.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
 //                   prev_fam.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
 //                   prev_im.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
 //                   prev_ot.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);

 //               }));

            newCmd.InputGestures.Add(new KeyGesture(Key.L, ModifierKeys.Alt));
            CommandBindings.Add(new CommandBinding(newCmd, Develop_show));
            HOLIDAYS.doc_ex_con = Properties.Settings.Default.DocExchangeConnectionString;
            Auth at = new Auth();
            at.ShowInTaskbar = false;
            at.ShowDialog();
            SplashScreen splash = new SplashScreen("Insurance_icons\\polis.jpg");
            splash.Show(false, true);
            splash.Close(TimeSpan.FromSeconds(5));
            
            

            Thread t0 = new Thread(delegate ()
            {
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                new Action(delegate ()
                {
                    //Vars.DateVisit = Convert.ToDateTime(d_obr.EditValue);
                    var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                    var peopleList =
                        MyReader.MySelect<People>(SPR.MyReader.load_pers_grid2 + strf_adm, connectionString);
                    pers_grid_2.ItemsSource = peopleList;
                    pers_grid_2.View.FocusedRowHandle = -1;
                }));
            });
            t0.Start();
            
            //Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
            //    new Action(delegate ()
            //    {
                    Thread t5 = new Thread(delegate ()
                    {
                        Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                new Action(delegate ()
                {
                    strf_adm = "order by pe.ID DESC";
                    strf_usr = $@"WHERE pe.AGENT =" + Vars.Agnt + " order by pe.ID DESC";
                    //if (stream.Length > 0)
                    var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                    if (SPR.Premmissions != "User")
                    {
                        var peopleList =
                            MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf_adm, connectionString);
                        //////ev =
                        //////     MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf_adm, connectionString);

                        pers_grid.ItemsSource = peopleList;
                        //////pers_grid.ItemsSource = ev;

                    }
                    else
                    {
                        var peopleList =
                            MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf_usr, connectionString);
                        //foreach (var item in peopleList) events_g1.Add(item);
                        //pers_grid.ItemsSource = events_g1;
                        pers_grid.ItemsSource = peopleList;
                        del_zl.IsEnabled = false;

                    }


                    //writer.Write(peopleList);
                    ////pers_grid.ItemsSource = peopleList;


                    //stream.Close();
                    //writer.Close();


                }));
                    });
                    t5.Start();

                //}));

            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                new Action(delegate ()
                {

                    //Insurance.DocExchangeDataSet docExchangeDataSet = ((Insurance.DocExchangeDataSet)(this.FindResource("docExchangeDataSet")));
                    //    // Загрузить данные в таблицу POL_PERSONS. Можно изменить этот код как требуется.
                    //    Insurance.DocExchangeDataSetTableAdapters.POL_PERSONSTableAdapter docExchangeDataSetPOL_PERSONSTableAdapter = new Insurance.DocExchangeDataSetTableAdapters.POL_PERSONSTableAdapter();
                    //    docExchangeDataSetPOL_PERSONSTableAdapter.Fill(docExchangeDataSet.POL_PERSONS);
                    //    System.Windows.Data.CollectionViewSource pOL_PERSONSViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("pOL_PERSONSViewSource")));
                    //    pOL_PERSONSViewSource.View.MoveCurrentToFirst();
                    //    pers_grid_2.Columns[1].Visible = false;
                    //    pers_grid_2.Columns[2].Visible = false;
                    //    pers_grid_2.Columns[3].Visible = false;
                    //    pers_grid_2.Columns[4].Visible = false;
                    //    pers_grid_2.View.FocusedRowHandle = -1;





                    pers_grid_2.View.FocusedRowHandle = -1;
                    pers_grid_2.SelectedItem = -1;




                    

                }));

            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();  // если надо, то в скобках указываем приоритет, например DispatcherPriority.Render
            //timer.Interval = TimeSpan.FromSeconds(15);
            //timer.Tick += new EventHandler(Reload_tick);
            //timer.Start();

            mo_cmb.DataContext = molist;
            pol.DataContext = pol_DataContext;
            pol_pr.DataContext = pol_DataContext;
            doc_type.DataContext = doc_type_DataContext;
            doctype1.DataContext = doc_type_DataContext;
            doc_type1.DataContext = doc_type_DataContext;
            ddtype.DataContext = doc_type_DataContext;
            str_vid.DataContext = str_vid_DataContext;
            str_vid1.DataContext = str_vid_DataContext;
            str_r.DataContext = str_vid_DataContext;
            gr.DataContext = str_vid_DataContext;
            kat_zl.DataContext = kat_zl_DataContext;
            type_policy.DataContext = type_policy_DataContext;
            prev_pol.DataContext = pol_DataContext;
            //kem_vid.DataContext = MyReader.MySelect<NAMEVP>(@"select distinct name_vp from pol_documents order by name_vp",connectionString);
            // LoadingDecorator1.IsSplashScreenShown = false;            
            fam.DataContext = fio_col;
            im.DataContext = im_DataContext;
            ot.DataContext = ot_DataContext;
            kem_vid.DataContext = kem_vid_DataContext;
            fam1.DataContext = fio_col;
            im1.DataContext = im_DataContext;
            ot1.DataContext = ot_DataContext;
            prev_fam.DataContext = fio_col;
            prev_im.DataContext = im_DataContext;
            prev_ot.DataContext = ot_DataContext;

            list1.Add("1 Первичный выбор СМО");
            list1.Add("2 Замена СМО в соответствии с правом замены");
            list1.Add("3 Замена СМО в связи со сменой места жительства");
            list1.Add("4 Замена СМО в связи с прекращением действия договора");
            this.pr_pod_z_smo.ItemsSource = list1;
            //pr_pod_z_smo.SelectedIndex = 0;

            list2.Add("1 Изменение реквизитов");
            list2.Add("2 Установление ошибочности сведений");
            list2.Add("3 Ветхость и непригодность полиса");
            list2.Add("4 Утрата ранее выданного полиса");
            list2.Add("5 Окончание срока действия полиса");
            this.pr_pod_z_polis.ItemsSource = list2;

            list3.Add("0 Не требует изготовления полиса");
            list3.Add("1 Бумажный бланк");
            list3.Add("2 Пластиковая карта");
            list3.Add("3 В составе УЭК");
            list3.Add("4 Отказ от полиса");
            this.form_polis.ItemsSource = list3;

            list0.Add("1 Родитель");
            list0.Add("2 Опекун");
            list0.Add("3 Представитель");
            status_p2.ItemsSource = list0;


            //form_polis.SelectedIndex = 1;

            list4.Add(new Dost { ID = "1", NameWithID = "1 Отсутствует отчество" });
            list4.Add(new Dost { ID = "2", NameWithID = "2 Отсутствует фамилия" });
            list4.Add(new Dost { ID = "3", NameWithID = "3 Отсутствует имя" });
            list4.Add(new Dost { ID = "4", NameWithID = "4 Известен только месяц и год даты рождения" });
            list4.Add(new Dost { ID = "5", NameWithID = "5 Известен только год даты рождения" });
            list4.Add(new Dost { ID = "6", NameWithID = "6 Дата рождения не соответствует календарю" });

            this.dost1.ItemsSource = list4;

            G_layuot.restore_Layout(Properties.Settings.Default.DocExchangeConnectionString, pers_grid, pers_grid_2);
            //LoadingDecorator1.IsSplashScreenShown = false;
            WindowState = WindowState.Maximized;
            Vars.MainTitle = "Insurance(полисная часть) v1.018";
            Title = Vars.MainTitle;
            prz.SelectedIndex = -1;
            //if (SPR.Premmissions == "User")
            //{
            //    upload.IsEnabled = false;
            //    download.IsEnabled = false;
            //}
            //else
            //{
            //    upload.IsEnabled = true;
            //    download.IsEnabled = true;
            //}
            //d_obr.DateTime = DateTime.Now;
            prz.Focus();
            //var dt = new DateTime(2020, 1, 1);
            //if (DateTime.Today >= dt)
            //{
            //    MessageBox.Show("Для использования данного функционала Вам необходимо заключить договор с ООО Компания ЯМЕД!!!");
            //    Application.Current.Shutdown();

            //}


            //prz_Agent.DataContext = MyReader.MySelect<AgentsSmo>(@"select id,prz_code,agent from pol_prz_agents", Properties.Settings.Default.DocExchangeConnectionString);
            if (SPR.Premmissions == "User")
            {
                prz.DataContext = MyReader.MySelect<PrzSmo>($@"select id,prz_code,prz_name,NameWithCode from pol_prz where prz_code in ({SPR.PRZ_CODE})", Properties.Settings.Default.DocExchangeConnectionString);
                prz_Agent.DataContext = MyReader.MySelect<AgentsSmo>($@"select id,prz_code,agent from pol_prz_agents where id={Vars.Agnt}", Properties.Settings.Default.DocExchangeConnectionString);
            }
            else
            {
                prz.DataContext = MyReader.MySelect<PrzSmo>($@"select id,prz_code,prz_name,NameWithCode from pol_prz", Properties.Settings.Default.DocExchangeConnectionString);
                prz_Agent.DataContext = MyReader.MySelect<AgentsSmo>($@"select id,prz_code,agent from pol_prz_agents", Properties.Settings.Default.DocExchangeConnectionString);
            }

            prz.SelectedIndex = 0;
            prz_Agent.EditValue = Vars.Agnt;
            //cel_vizita.DataContext = MyReader.MySelect<R001>(@"select Kod,NameWithID from R001", Properties.Settings.Default.DocExchangeConnectionString);
            //sp_pod_z.DataContext = MyReader.MySelect<R003>(@"select ID,NameWithID from R003", Properties.Settings.Default.DocExchangeConnectionString);
            Version_Check();

            //splash.Close(TimeSpan.FromSeconds(10));
            d_obr.EditValue = DateTime.Now;
            fias1.btn_ = Vars.Btn;
            fias.btn_1 = Vars.Btn;
            //_iid = Vars.IdZl;
            //petition = petit;
            //Vars.IdP = iid;
            //_2.Text = Vars.Btn;
            btn_ = Vars.Btn;
            //dorder = dobr;
            if (Vars.Btn != "1")
            {
                pol.SelectedIndex = Convert.ToInt32(Vars.W);
            }
            else
            {
                pol.SelectedIndex = -1;
            }
            //_4.Text = cel_viz;
            //Vars.Sposob = sppz;
            //_6.Text = dobr;


            pers_grid_2.View.FocusedRowHandle = -1;
            //MessageBox.Show(Vars.CelVisit);
            //FiasControl fiasControl = new FiasControl();

            //fiasControl.Method(ttab) += tab_forward(); //подписываем обработчик к событию
            tttab = fias.ttab;
            //doc_type.SelectedIndex = 13;
            date_end.NullValue = DateTime.MinValue;
            fakt_prekr.NullValue = DateTime.MinValue;
            ddeath.NullValue = DateTime.MinValue;
            dr1.NullValue = DateTime.MinValue;
            dout.NullValue = DateTime.MinValue;
            cel_vizita.DataContext = MyReader.MySelect<R001>(@"select Kod,NameWithID from R001", Properties.Settings.Default.DocExchangeConnectionString);
            sp_pod_z.DataContext = MyReader.MySelect<R003>(@"select ID,NameWithID from R003", Properties.Settings.Default.DocExchangeConnectionString);
            //Events ev = new Events();
            //events_list = ev.GetType().GetProperties().Select(x => x.Name).ToArray();
            //string ev_filds = Funcs.MyFields(events_list);
            if (Vars.SMO.StartsWith("46") == true || Vars.SMO.StartsWith("04") == true)
            {
                Del_file_btn.Visibility = Visibility.Visible;
            }
            else
            {
                Del_file_btn.Visibility = Visibility.Collapsed;
            }
            if (Vars.SMO.StartsWith("22") == true)
            {
                Load_att_btn.Visibility = Visibility.Visible;
                Unload_att_btn.Visibility = Visibility.Visible;
            }
            else
            {
                Load_att_btn.Visibility = Visibility.Collapsed;
                Unload_att_btn.Visibility = Visibility.Collapsed;
            }

        }
        //string cel_viz;

        //public string[] events_list;
        private void new_obr_Click(object sender, RoutedEventArgs e)
        {
            fio_col = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
            im_DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
            ot_DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
            kem_vid_DataContext = MyReader.MySelect<NAME_VP>(@"select id,name from spr_namevp", Properties.Settings.Default.DocExchangeConnectionString);
            fam.DataContext = fio_col;
            im.DataContext = im_DataContext;
            ot.DataContext = ot_DataContext;
            kem_vid.DataContext = kem_vid_DataContext;
            fam1.DataContext = fio_col;
            im1.DataContext = im_DataContext;
            ot1.DataContext = ot_DataContext;
            prev_fam.DataContext = fio_col;
            prev_im.DataContext = im_DataContext;
            prev_ot.DataContext = ot_DataContext;
            //layout_InUse();

            //object UNLOAD="";
            //int AGENT = 0;
            if (SPR.Premmissions == "User")
            {
                //try
                //{

                //     string sqlExpression = $@"SELECT pe.UNLOAD,pe.agent FROM[dbo].[POL_PERSONS] pp left join pol_events pe on pp.event_guid=pe.idguid where pp.id ={pers_grid.GetFocusedRowCellValue("ID")}";
                //     using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.DocExchangeConnectionString))
                //     {
                //     connection.Open();
                //     SqlCommand command = new SqlCommand(sqlExpression, connection);
                //     SqlDataReader reader = command.ExecuteReader();

                //         if (reader.HasRows) // если есть данные
                //         {
                //             while (reader.Read()) // построчно считываем данные
                //             {
                //                  UNLOAD = reader.GetValue(0);
                //                  AGENT = Convert.ToInt32(reader.GetValue(1));

                //             }
                //         }

                //     reader.Close();
                //     }

                //}
                //catch
                //{

                //}



            }


            //LoadingDecorator1.IsSplashScreenShown = true;

            if (prz.SelectedIndex < 0 || prz_Agent.SelectedIndex < 0)
            {
                string m = "Выберите пункт выдачи полисов и агента!";
                string t = "Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();

                return;

            }
            else
            {
                //LoadingDecorator1.IsSplashScreenShown = true;


                Vars.PunctRz = prz.EditValue.ToString();
                Vars.Agnt = Convert.ToInt32(prz_Agent.EditValue);

                if (pers_grid.SelectedItems.Count != 0)
                {
                    if (pers_grid.SelectedItems.Count > 1)
                    {
                        string m1 = "Вы выбрали больше 1 клиента!";
                        string t1 = "Ошибка!";
                        int b1 = 1;
                        Message me1 = new Message(m1, t1, b1);
                        me1.ShowDialog();

                        return;
                    }
                    //if (pers_grid.GetFocusedRowCellValue("UNLOAD").ToString() == "True" && SPR.Premmissions=="User")
                    //{
                    //    string m2 = "Редактирование заявлений отправленных в ТФОМС запрещено!";
                    //    string t2 = "Ошибка!";
                    //    int b2 = 1;
                    //    Message me2 = new Message(m2, t2, b2);
                    //    me2.ShowDialog();
                    //    return;

                    //}
                    Vars.grid_num = 1;
                    Vars.IdP = pers_grid.GetCellValue(pers_grid.GetSelectedRowHandles()[0], "ID").ToString();
                    string m = "Вы хотите исправить ошибки в данных или создать новое событие?";
                    string t = "Внимание!";
                    int b = 0;
                    Message me = new Message(m, t, b);

                    me.ShowDialog();
                    if (Vars.mes_res == 100)
                    {
                        return;
                    }
                    else if (Vars.Btn == "2")
                    {
                        if (SPR.Premmissions == "User")
                        {


                            if (prz_Agent.Text != pers_grid.GetFocusedRowCellValue("AGENT").ToString() && pers_grid.GetFocusedRowCellValue("AGENT").ToString() != "")
                            {

                                string mz = "Редактирование чужих записей запрещено!";
                                string tz = "Ошибка!";
                                int bz = 1;
                                Message mez = new Message(mz, tz, bz);
                                mez.ShowDialog();
                                MainTab.SelectedIndex = 0;
                                Tab_ZL.Visibility = Visibility.Hidden;
                                return;
                            }
                            else if (pers_grid.GetFocusedRowCellValue("UNLOAD").ToString() == "True")
                            {
                                string m2 = "Редактирование заявлений отправленных в ТФОМС запрещено!";
                                string t2 = "Ошибка!";
                                int b2 = 1;
                                Message me2 = new Message(m2, t2, b2);
                                me2.ShowDialog();
                                return;
                            }

                        }
                        this.Title = "Исправление ошибочных данных застрахованного лица";

                    }
                    //else
                    //{
                    MainTab.SelectedIndex = 1;
                    Tab_ZL.Visibility = Visibility.Visible;
                    //pers_grid_Loaded(this, e);
                    Window_Loaded(this, e);
                    //}
                   
                }
                else
                {
                    //LoadingDecorator1.IsSplashScreenShown = true;
                    Cursor = Cursors.Wait;
                    Vars.Btn = "1";
                    //Person_Data w1 = new Person_Data();
                    //LoadingDecorator1.IsSplashScreenShown = false;
                    MainTab.SelectedIndex = 1;
                    Tab_ZL.Visibility = Visibility.Visible;
                    //pers_grid_Loaded(this, e);
                    //Window_Loaded(this, e);
                    //Cursor = Cursors.Arrow;
                }
                if (Vars.Btn == "2")
                {

                    this.Title = "Исправление ошибочных данных застрахованного лица";
                }
                else if (Vars.Btn == "1")
                {
                    this.Title = "Новый клиент";
                }
                else if (Vars.Btn == "3")
                {
                    this.Title = "Создание нового события существующему ЗЛ";
                }
                else
                {
                    this.Title = Vars.MainTitle;
                }
                //LoadingDecorator1.IsSplashScreenShown = false;

                //Vars.CelVisit = cel_vizita.EditValue.ToString();

                // this.Visibility = Visibility.Visible;
            }
            //LoadingDecorator1.IsSplashScreenShown = true;
            Cursor = Cursors.Arrow;
            //restore_Layout();
            G_layuot.save_Layout(Properties.Settings.Default.DocExchangeConnectionString, pers_grid, pers_grid_2);
        }

        private void pers_grid_Loaded(object sender, RoutedEventArgs e)
        {
            strf_adm = "order by pe.ID DESC";
            strf_usr = $@"WHERE pe.AGENT =" + Vars.Agnt + " order by pe.ID DESC";
            //layout_InUse();

            //            //pers_grid.Columns[1].Visible = false;
            //            //pers_grid.Columns[2].Visible = false;
            //            //pers_grid.Columns[3].Visible = false;
            //            //pers_grid.Columns[4].Visible = false;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            if (SPR.Premmissions != "User")
            {
                var peopleList =
                    MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf_adm, connectionString);
                pers_grid.ItemsSource = peopleList;
            }
            else
            {
                var peopleList =
                    MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf_usr, connectionString);
                pers_grid.ItemsSource = peopleList;

            }

            pers_grid.View.FocusedRowHandle = -1;
            pers_grid.SelectedItem = -1;
            //           // pers_grid.Columns["DSTOP"].EditSettings = new TextEditSettings() { ShowNullText = true };
            ////restore_Layout();

        }

        public void Vladik()
        {
            
            if (POISKVLADIK.Load_ZL == true)
            {
               

                if (Vars.IdP != null)
                {
                   
                    Tab_ZL.Visibility = Visibility.Visible;
                    MainTab.SelectedIndex = 1;
                    Window_Loaded(this, e);

                }
                else
                {
                
                    Tab_ZL.Visibility = Visibility.Visible;
                    MainTab.SelectedIndex = 1;
                    
                    try
                    {
                        fam.Text = POISKVLADIK.FAM_TFOMS;
                      
                
                    }
                    catch
                    {

                    }
                    try
                    {
                        im.Text = POISKVLADIK.IM_TFOMS;
                    }
                    catch
                    {

                    }
                    try
                    {
                        ot.Text = POISKVLADIK.OT_TFOMS;
                    }
                    catch
                    {

                    }
                    try
                    {
                        dr.Text = POISKVLADIK.DR_TFOMS;
                    }
                    catch
                    {

                    }
                    try
                    {
                        if (POISKVLADIK.VIDPOLIS_TFOMS == "Полис до 2011г.")
                        {
                            type_policy.Text = "2    Временное свидетельство, подтверждающее оформление полиса обязательного медицинского страхования";
                        }
                        else
                        {
                            type_policy.Text = "2    Временное свидетельство, подтверждающее оформление полиса обязательного медицинского страхования";
                        }
                    }
                    catch
                    {

                    }
                    try
                    {
                        snils.Text = POISKVLADIK.SNILS_TFOMS;
                    }
                    catch
                    {

                    }

                    try
                    {
                        //num_blank.Text = POISKVLADIK.POLIS_TFOMS;
                        enp.Text = POISKVLADIK.ENP_TFOMS;
                        mr2.Text = POISKVLADIK.MR_TFOMS;
                        mo_cmb.Text = POISKVLADIK.POLIKLIN_TFOMS;
                        date_mo.Text = POISKVLADIK.DATE_PRIKREP_TFOMS;
                        date_vid1.Text = POISKVLADIK.DATE_START_TFOMS;
                        dost1.Text = POISKVLADIK.SPOSOB_TFOMS;
                        date_end.Text = POISKVLADIK.DATE_END_TFOMS;
                        fakt_prekr.Text = POISKVLADIK.SMO_TFOMS;
                        ddeath.Text = POISKVLADIK.DATE_DEAD_TFOMS;
                    }
                    catch
                    {

                    }
                }




                POISKVLADIK.Load_ZL = false;
               
                return;
            }
        }

        private void w_main_Activated(object sender, EventArgs e)
        {
         
            DispatcherTimer timer = new DispatcherTimer();  // если надо, то в скобках указываем приоритет, например DispatcherPriority.Render
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(Ot_tick);
            if (SPR.Avto_vigruzka == true)
            {
                timer.Start();
            }
            else
            {
                timer.Stop();
            }
            //            if(pers_grid.SelectedItems.Count>0)
            //            {

            //            }
            //            else
            //            {
            //                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            //                var peopleList =
            //                        MyReader.MySelect<Events>(
            //                            $@"
            //            SELECT top(1000)  pp.ID,pp.ACTIVE,op.przcod,pe.UNLOAD,ENP ,FAM , IM  , OT ,W ,DR ,r.NameWithID, pp.COMMENT,DORDER,
            //SS  ,VPOLIS,SPOLIS ,NPOLIS,DBEG ,DEND ,DSTOP ,BLANK ,DRECEIVED,f.NameWithId,op.filename,pp.phone
            //  FROM [dbo].[POL_PERSONS] pp left join 
            //pol_events pe on pp.event_guid=pe.idguid
            //left join pol_polises ps
            //on pp.idguid=ps.person_guid
            //left join pol_oplist op
            //on pp.idguid=op.person_guid
            //left join r001 r
            //on pe.tip_op=r.kod
            //left join f003 f
            //on pp.MO=f.mcod
            //--where  pp.active=1
            // order by pp.ID desc", connectionString);

            //                pers_grid.ItemsSource = peopleList;

            //                pers_grid.View.FocusedRowHandle = -1;
            //                pers_grid.SelectedItem = -1;
            //            }
            if (SPR.Premmissions == "User")
            {
                prz_Agent.DataContext = MyReader.MySelect<AgentsSmo>(
                    $@"select id,prz_code,agent from pol_prz_agents where agent='{SPR.Login}'",
                    Properties.Settings.Default.DocExchangeConnectionString);
                prz.DataContext = MyReader.MySelect<PrzSmo>(
                    $@"select id,prz_code,prz_name,NameWithCode from pol_prz where prz_code in( {SPR.PRZ_CODE})",
                    Properties.Settings.Default.DocExchangeConnectionString);
            }
            else
            {
                prz.DataContext = MyReader.MySelect<PrzSmo>(
                    $@"select id,prz_code,prz_name,NameWithCode from pol_prz",
                    Properties.Settings.Default.DocExchangeConnectionString);
                prz_Agent.DataContext = MyReader.MySelect<AgentsSmo>(
                    $@"select id,prz_code,agent from pol_prz_agents where prz_code like'%{prz.EditValue}%'",
                    Properties.Settings.Default.DocExchangeConnectionString);

            }
            //G_layuot.save_Layout(Properties.Settings.Default.DocExchangeConnectionString,pers_grid,pers_grid_2);
            ////restore_Layout();
            ////layout_InUse();
        }

        private void download_Click(object sender, RoutedEventArgs e)
        {
            Blanks w3 = new Blanks();
            w3.ShowDialog();

        }

        private void download_Click_1(object sender, RoutedEventArgs e)
        {
            From_Tfoms w5 = new From_Tfoms();
            w5.ShowDialog();
            pers_grid_Loaded(this, e);

        }
        private string strfilter = "";

        private void pers_grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                if (pers_grid.ActiveFilterInfo == null)
                {
                    if (SPR.Premmissions != "User")
                    {
                        var peopleList =
                            MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf_adm, connectionString);
                        pers_grid.ItemsSource = peopleList;
                    }
                    else
                    {
                        var peopleList =
                            MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf_usr, connectionString);
                        pers_grid.ItemsSource = peopleList;

                    }
                    pers_grid.View.FocusedRowHandle = -1;
                }
                else
                {
                    strfilter = pers_grid.FilterString;
                    string strf = pers_grid.ActiveFilterInfo.FilterString.Substring(pers_grid.ActiveFilterInfo.FilterString.IndexOf("'", 0));
                    string strf1 = strf.Replace("'", "").Replace(")", "").Replace(".", "");
                    string strf2 = $@"where FAM LIKE '{SPR.Translit(strf1)}%' OR  IM LIKE '{SPR.Translit(strf1)}%' OR  OT LIKE '{SPR.Translit(strf1)}%' order by ID desc";
                    var peopleList = MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf2, connectionString);
                    pers_grid.ItemsSource = peopleList;
                    pers_grid.View.FocusedRowHandle = -1;
                    //pers_grid.FilterString = "";

                }



            }
            //else
            //{
            //    string strf = pers_grid.ActiveFilterInfo.FilterString.Substring(pers_grid.ActiveFilterInfo.FilterString.IndexOf("'", 0));
            //    string strf1 = strf.Replace("'", "").Replace(")", "").Replace(".", "");
            //    pers_grid.SetFocusedRowCellValue("FAM", SPR.Translit(strf1));
            //}



        }
        private void Ot_tick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(DateTime.Now.Hour) == 20 && SPR.Avto_vigruzka_priznak == true)
            {
                var sg_rows = "";
                DateTime now;
                now = DateTime.Now;
                pers_grid.FilterString = $@"[DVIZIT] >= #{now.ToString("yyyy-MM-dd")}# And [DVIZIT] < #{now.AddDays(1).ToString("yyyy-MM-dd")}#";
                List<int> id_filter = new List<int>();
                id_filter = pers_grid.DataController.GetAllFilteredAndSortedRows().OfType<Events>().Select(x => x.ID).ToList();


                if (id_filter.Count == 0)
                {
                    string m1 = "Вы не выбрали ЗЛ для выгрузки!";
                    string t1 = "Ошибка!";
                    int b1 = 1;
                    Message me1 = new Message(m1, t1, b1);
                    me1.ShowDialog();
                    return;
                }

                for (int i = 0; i < id_filter.Count; i++)
                {
                    if (i == id_filter.Count - 1)
                    {
                        sg_rows += id_filter[i];
                    }
                    else
                    {
                        sg_rows += id_filter[i] + ",";
                    }

                }

                int rtc = id_filter.Count();
                if (prz.EditValue == null)
                {
                    string m = "Выберите пункт выдачи!";
                    string t = "Ошибка!";
                    int b = 1;
                    Message me = new Message(m, t, b);
                    me.ShowDialog();

                    return;
                }
                else
                {

                }

                string fname1;
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand comm4 = new SqlCommand($@"select top (1) smo_code from pol_prz", con);
                con.Open();
                comm4.CommandTimeout = 0;
                string smocod = (string)comm4.ExecuteScalar();
                con.Close();

                SqlCommand comm2 = new SqlCommand($@"declare @num nvarchar(5);
set @num=(select
(case
when (select count(*) from POL_FILES where 
FNAME_MM=datepart(mm,GETDATE()) 
and FNAME_GG=right(datepart(yy,GETDATE()),2) and id=(select max(id) from POL_FILES where FNAME_MM=datepart(mm,GETDATE()) 
and FNAME_GG=right(datepart(yy,GETDATE()),2)))=0 then 1
else (select FNAME_Z+1 from POL_FILES where 
FNAME_MM=datepart(mm,GETDATE()) 
and FNAME_GG=right(datepart(yy,GETDATE()),2) and id=(select max(id) from POL_FILES where FNAME_MM=datepart(mm,GETDATE()) 
and FNAME_GG=right(datepart(yy,GETDATE()),2))) end)) ;
select 'i'+(select top(1) SMO_CODE from pol_prz)+'_'+'{prz.EditValue.ToString()}'+'_'+
(case when len(convert(nvarchar,datepart(mm,GETDATE())))=1 then '0'+convert(nvarchar,datepart(mm,GETDATE()))
else convert(nvarchar,datepart(mm,GETDATE())) end)+
convert(nvarchar,right(datepart(yy,GETDATE()),2))+@num+'.xml'", con);
                con.Open();
                comm2.CommandTimeout = 0;
                string fname_xml = (string)comm2.ExecuteScalar();
                con.Close();
                SqlCommand comm5 = new SqlCommand($@"declare @num nvarchar(5);
set @num=(select
(case
when (select count(*) from POL_FILES where 
FNAME_MM=datepart(mm,GETDATE()) 
and FNAME_GG=right(datepart(yy,GETDATE()),2) and id=(select max(id) from POL_FILES where FNAME_MM=datepart(mm,GETDATE()) 
and FNAME_GG=right(datepart(yy,GETDATE()),2)))=0 then 1
else (select FNAME_Z+1 from POL_FILES where 
FNAME_MM=datepart(mm,GETDATE()) 
and FNAME_GG=right(datepart(yy,GETDATE()),2) and id=(select max(id) from POL_FILES where FNAME_MM=datepart(mm,GETDATE()) 
and FNAME_GG=right(datepart(yy,GETDATE()),2))) end)) ;
select @num", con);
                con.Open();
                comm5.CommandTimeout = 0;
                string num = (string)comm5.ExecuteScalar();
                con.Close();
                string fname3;

                fname3 = "i" + smocod + "_" + prz.EditValue.ToString() + "_" + DateTime.Today.ToString("MM") + DateTime.Today.ToString("yy") + ".xml";

                string sff = SPR.PATH_VIGRUZKA + fname3;

                if (smocod.Substring(0, 2) == "57")
                {

                    SqlCommand comm = new SqlCommand($@"exec [Unload_ToFoms] @num = '{num}', @FName = '{fname3}', @col = {rtc}, @prz = '{prz.EditValue.ToString()}', @ids = '{sg_rows}' ", con);
                    con.Open();
                    comm.CommandTimeout = 0;
                    fname1 = (string)comm.ExecuteScalar();
                    con.Close();
                }
                else if (smocod.Substring(0, 2) == "22" || smocod.Substring(0, 2) == "25")
                {
                    SqlCommand comm = new SqlCommand($@"exec [Unload_ToFoms] @num = '{num}', @FName = '{fname3}', @col = {rtc}, @prz = '{prz.EditValue.ToString()}', @ids = '{sg_rows}' e", con);

                    con.Open();
                    comm.CommandTimeout = 0;
                    fname1 = (string)comm.ExecuteScalar();
                    con.Close();
                }
                else
                {
                    SqlCommand comm = new SqlCommand($@"exec [Unload_ToFoms] @num = '{num}', @FName = '{fname3}', @col = {rtc}, @prz = '{prz.EditValue.ToString()}', @ids = '{sg_rows}' ", con);

                    con.Open();
                    comm.CommandTimeout = 0;
                    fname1 = (string)comm.ExecuteScalar();
                    con.Close();
                }
                SqlCommand comm1 = new SqlCommand($@"Select FXML from pol_files where filename='{fname1}'", con);
                con.Open();
                comm1.CommandTimeout = 0;
                string fxml = (string)comm1.ExecuteScalar();
                con.Close();

                System.IO.File.WriteAllText(sff, "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "windows-1251" + (char)34 + "?>" + fxml, Encoding.GetEncoding("windows-1251"));
                XDocument xDoc = XDocument.Load(sff);
                xDoc.Save(sff);
                string fnamezip = sff.Replace(".xml", "");
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddFile(sff, "");

                    zip.Save(fnamezip + ".zip");

                }




                System.IO.FileInfo file_zip = new System.IO.FileInfo(fnamezip + ".zip");
                long size = file_zip.Length;
                byte[] array;
                using (System.IO.FileStream fs = new System.IO.FileStream(fnamezip + ".zip", FileMode.Open))
                {
                    // преобразуем строку в байты
                    array = new byte[fs.Length];
                    // считываем данные
                    fs.Read(array, 0, array.Length);
                    fs.Close();
                    // декодируем байты в строку
                    //string textFromFile = System.Text.Encoding.Default.GetString(array);

                }
                SqlCommand comm3 = new SqlCommand($@"UPDATE pol_files set fzip=convert(varbinary(max),'{array}'),fsize={size} where filename='{fname1}'", con);
                con.Open();
                comm3.CommandTimeout = 0;
                comm3.ExecuteScalar();
                con.Close();
                //System.Diagnostics.Process.Start(fnamezip + ".zip");

            }
            else
            {
                return;
            }

            SPR.Avto_vigruzka_priznak = false;
            SPR.Avto_vigruzka = false;
        }

        private void w_main_Loaded(object sender, RoutedEventArgs e)
        {
      
            //layout_InUse();
            pers_grid.ClearSorting();


            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand($@"select top (1) smo_code from pol_prz", con);
            con.Open();
            Vars.SMO = (string)comm.ExecuteScalar();
            con.Close();



        }

        private void upload_Click(object sender, RoutedEventArgs e)
        {
            if (SPR.Premmissions == "User")
            {
                string m = "У вас недостаточно прав для данной операции!";
                string t = "Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
            }
            else
            {

                if (pers_grid.SelectedItems.Count == 0)
                {
                    string m1 = "Вы не выбрали ЗЛ для выгрузки!";
                    string t1 = "Ошибка!";
                    int b1 = 1;
                    Message me1 = new Message(m1, t1, b1);
                    me1.ShowDialog();
                    return;
                }

                var sg_rows = Funcs.MyIds(pers_grid.GetSelectedRowHandles(), pers_grid);

                //string sg_rows = " ";
                //int[] rt = pers_grid.GetSelectedRowHandles();
                int rtc = pers_grid.GetSelectedRowHandles().Count();
                
                //for (int i = 0; i < rt.Count(); i++)

                //{
                //    var ddd = pers_grid.GetCellValue(rt[i], "ID");
                //    var sgr = sg_rows.Insert(sg_rows.Length, ddd.ToString()) + ",";
                //    sg_rows = sgr;
                //}

                //sg_rows = sg_rows.Substring(0, sg_rows.Length - 1);
                if (prz.EditValue == null)
                {
                    string m = "Выберите пункт выдачи!";
                    string t = "Ошибка!";
                    int b = 1;
                    Message me = new Message(m, t, b);
                    me.ShowDialog();

                    return;
                }
                else
                {

                }


                string fname1;
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand comm4 = new SqlCommand($@"select top (1) smo_code from pol_prz", con);
                con.Open();
                comm4.CommandTimeout = 0;
                string smocod = (string)comm4.ExecuteScalar();
                con.Close();

                SqlCommand comm2 = new SqlCommand($@"declare @num nvarchar(5);
set @num=(select
(case
when (select count(*) from POL_FILES where 
FNAME_MM=datepart(mm,GETDATE()) 
and FNAME_GG=right(datepart(yy,GETDATE()),2) and id=(select max(id) from POL_FILES where FNAME_MM=datepart(mm,GETDATE()) 
and FNAME_GG=right(datepart(yy,GETDATE()),2)))=0 then 0 
else (select FNAME_Z+1 from POL_FILES where 
FNAME_MM=datepart(mm,GETDATE()) 
and FNAME_GG=right(datepart(yy,GETDATE()),2) and id=(select max(id) from POL_FILES where FNAME_MM=datepart(mm,GETDATE()) 
and FNAME_GG=right(datepart(yy,GETDATE()),2))) end));
set @num =(case 
when len(@num)=1 and @num=0 then @num+'01'
when len(@num)=1 and @num!=0 then '00'+@num
when len(@num)=2 then '0'+@num else @num end)
select 'i'+(select top(1) SMO_CODE from pol_prz)+'_'+'{prz.EditValue.ToString()}'+'_'+
(case when len(convert(nvarchar,datepart(mm,GETDATE())))=1 then '0'+convert(nvarchar,datepart(mm,GETDATE()))
else convert(nvarchar,datepart(mm,GETDATE())) end)+
convert(nvarchar,right(datepart(yy,GETDATE()),2))+@num+'.xml'", con);
                con.Open();
                comm2.CommandTimeout = 0;
                string fname = (string)comm2.ExecuteScalar();
                con.Close();
                SqlCommand comm5 = new SqlCommand($@"declare @num nvarchar(5);
set @num=(select
(case
when (select count(*) from POL_FILES where 
FNAME_MM=datepart(mm,GETDATE()) 
and FNAME_GG=right(datepart(yy,GETDATE()),2) and id=(select max(id) from POL_FILES where FNAME_MM=datepart(mm,GETDATE()) 
and FNAME_GG=right(datepart(yy,GETDATE()),2)))=0 then 0 
else (select FNAME_Z+1 from POL_FILES where 
FNAME_MM=datepart(mm,GETDATE()) 
and FNAME_GG=right(datepart(yy,GETDATE()),2) and id=(select max(id) from POL_FILES where FNAME_MM=datepart(mm,GETDATE()) 
and FNAME_GG=right(datepart(yy,GETDATE()),2))) end));
set @num =(case 
when len(@num)=1 and @num=0 then @num+'01'
when len(@num)=1 and @num!=0 then '00'+@num
when len(@num)=2 then '0'+@num else @num end)
select @num", con);
                con.Open();
                comm5.CommandTimeout = 0;
                string num = (string)comm5.ExecuteScalar();
                con.Close();

                SaveFileDialog SF = new SaveFileDialog();
                SF.InitialDirectory = "c:\\";

                SF.FileName = fname;

                string fname3;
                bool res = SF.ShowDialog().Value;
                if (SF.SafeFileName.ToString() == fname)
                {
                    fname3 = fname;
                }
                else
                {
                    fname3 = "i" + smocod + "_" + prz.EditValue.ToString() + "_" +
                             DateTime.Today.ToString("MM") + DateTime.Today.ToString("yy") +
                             SF.SafeFileName.ToString() + ".xml";
                    num = SF.SafeFileName.ToString();
                }

                string sff = SF.FileName.Replace(SF.SafeFileName, fname3);
                if (res == true)
                {
                    if (smocod.Substring(0, 2) == "57")
                    {

                        SqlCommand comm = new SqlCommand(
                            $@"exec [Unload_ToFoms] @num = '{num}', @FName = '{fname3}', @col = {rtc}, @prz = '{prz.EditValue.ToString()}', @ids = '{sg_rows}' ", con);
                        con.Open();
                        comm.CommandTimeout = 0;
                        fname1 = (string)comm.ExecuteScalar();
                        con.Close();
                    }
                    else if (smocod.Substring(0, 2) == "22" || smocod.Substring(0, 2) == "25")
                    {
                        SqlCommand comm = new SqlCommand(
                           $@"exec [Unload_ToFoms] @num = '{num}', @FName = '{fname3}', @col = {rtc}, @prz = '{prz.EditValue.ToString()}', @ids = '{sg_rows}' ", con);

                        con.Open();
                        comm.CommandTimeout = 0;
                        fname1 = (string)comm.ExecuteScalar();
                        con.Close();
                    }
                    else
                    {
                        SqlCommand comm = new SqlCommand(
                            $@"exec [Unload_ToFoms] @num = '{num}', @FName = '{fname3}', @col = {rtc}, @prz = '{prz.EditValue.ToString()}', @ids = '{sg_rows}' ", con);

                        con.Open();
                        comm.CommandTimeout = 0;
                        fname1 = (string)comm.ExecuteScalar();
                        con.Close();
                    }

                    SqlCommand comm1 = new SqlCommand($@"Select FXML from pol_files where filename='{fname1}'",
                        con);
                    con.Open();
                    comm1.CommandTimeout = 0;
                    string fxml = (string)comm1.ExecuteScalar();
                    con.Close();

                    System.IO.File.WriteAllText(sff,
                        "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 +
                        "windows-1251" + (char)34 + "?>" + fxml, Encoding.GetEncoding("windows-1251"));
                    XDocument xDoc = XDocument.Load(sff);
                    xDoc.Save(sff);
                    string fnamezip = sff.Replace(".xml", "");
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AddFile(sff, "");

                        zip.Save(fnamezip + ".zip");

                    }

                    pers_grid.UnselectAll();
                    string m = "Файл " + fname3 + " успешно сохранен!";
                    string t = "Сообщение!";
                    int b = 1;
                    Message me = new Message(m, t, b);
                    me.ShowDialog();
                    //MessageBox.Show("Файл " + fname3 + " успешно сохранен!");

                    //File.Delete(sff);// + ".xml");



                    System.IO.FileInfo file_zip = new System.IO.FileInfo(fnamezip + ".zip");
                    long size = file_zip.Length;
                    byte[] array;
                    using (System.IO.FileStream fs = new System.IO.FileStream(fnamezip + ".zip", FileMode.Open))
                    {
                        // преобразуем строку в байты
                        array = new byte[fs.Length];
                        // считываем данные
                        fs.Read(array, 0, array.Length);
                        // декодируем байты в строку
                        //string textFromFile = System.Text.Encoding.Default.GetString(array);

                    }

                    SqlCommand comm3 =
                        new SqlCommand(
                            $@"UPDATE pol_files set fzip=convert(varbinary(max),'{array}'),fsize={size} where filename='{fname1}'",
                            con);
                    con.Open();
                    comm3.CommandTimeout = 0;
                    comm3.ExecuteScalar();
                    con.Close();
                    //System.Diagnostics.Process.Start(fnamezip + ".zip");
                    pers_grid_Loaded(this, e);
                }
                else
                {
                    return;
                }


                //System.IO.File.AppendAllText(SF.FileName, fxml);
                //OpenFileDialog OPF = new OpenFileDialog();
                //OPF.ShowDialog();
                //OPF.Multiselect = false;


                //string file = OPF.FileName;
                //ProcessStartInfo infoStartProcess = new ProcessStartInfo();

                //infoStartProcess.WorkingDirectory = @"c:\elmedicine\bin2018";
                //infoStartProcess.FileName = @"2.vbs";

                //Process.Start(infoStartProcess);
                //System.Threading.Thread.Sleep(10000);
                //    System.IO.File.Delete(infoStartProcess.WorkingDirectory+"\\"+ infoStartProcess.FileName);
                //this.Close();


                ////Process.Start(@"c:\elmedicine\bin2018\1.vbs");
            }

        }

        private void settingss_Click(object sender, RoutedEventArgs e)
        {
            if (SPR.Premmissions != "Admin")
            {
                string m = "У вас недостаточно прав для данной операции!";
                string t = "Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
            }
            else
            {

                Settings w6 = new Settings();
                w6.ShowDialog();
            }
        }

        public string[] Polis_upp_file;
        private void polises_Click(object sender, RoutedEventArgs e)
        {
            //int[] rt = pers_grid.GetSelectedRowHandles();
            //ObservableCollection<Events> ers = new ObservableCollection<Events>();
            //for(int i =0;i<rt.Count();i++)
            //{
            //    ers.Add((Insurance_SPR.Events)pers_grid.GetRow(i));
            //}
            //var ttt = ers.Select(x=>x.EVENT_ID);
            OpenFileDialog OF = new OpenFileDialog();

            OF.Multiselect = true;
            bool res = OF.ShowDialog().Value;
            string[] path_ex = OF.FileNames;
            string call = "polis";
            Polis_upp_file = OF.SafeFileNames;
            if (res == true)
            {
                Polis_Up w8_polises_in = new Polis_Up(path_ex, Polis_upp_file, call);
                w8_polises_in.ShowDialog();
            }
            else
            {
                return;
            }


        }
        private bool upd_click = false;
        private void p060_Click(object sender, RoutedEventArgs e)
        {
            upd_click = true;
            WebClient webcl = new WebClient();
            webcl.DownloadFile(@"http://elmedweb.ru/INSURANCE/Updater Inshurance.exe", @"Updater Inshurance.exe");
            Version_Check();
            //FIAS_SEARCH fs = new FIAS_SEARCH();
            //fs.Show();

            //System.IO.File.AppendAllText(SF.FileName, fxml);
            //OpenFileDialog OPF = new OpenFileDialog();
            //OPF.ShowDialog();
            //OPF.Multiselect = false;


            //string file = OPF.FileName;
            //ProcessStartInfo infoStartProcess = new ProcessStartInfo();

            //infoStartProcess.WorkingDirectory = @"c:\elmedicine\bin2018";
            //infoStartProcess.FileName = @"2.vbs";

            //Process.Start(infoStartProcess);
            //System.Threading.Thread.Sleep(10000);
            //    System.IO.File.Delete(infoStartProcess.WorkingDirectory+"\\"+ infoStartProcess.FileName);
            //this.Close();


            ////Process.Start(@"c:\elmedicine\bin2018\1.vbs");

        }

        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            string r_id = "0";
            string btn_clk = "0";
            Doc_Edit w9 = new Doc_Edit(btn_clk, r_id);
            w9.Show();
        }

        private void Reports_Spisok_Click(object sender, RoutedEventArgs e)
        {
            if (SPR.Premmissions != "Admin")
            {
                string m = "У вас недостаточно прав для данной операции!";
                string t = "Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
            }
            else
            {

                Docs w11 = new Docs();
                w11.ShowDialog();
            }
        }

        private void Sbros_Click(object sender, RoutedEventArgs e)
        {
            string sg_rows = " ";
            int[] rt = pers_grid.GetSelectedRowHandles();
            for (int i = 0; i < rt.Count(); i++)

            {
                var ddd = pers_grid.GetCellValue(rt[i], "ID");
                var sgr = sg_rows.Insert(sg_rows.Length, ddd.ToString()) + ",";
                sg_rows = sgr;
            }
            sg_rows = sg_rows.Substring(0, sg_rows.Length - 1);
            string m = "Вы действительно хотите снять признак выгрузки?";
            string t = "Внимание!";
            int b = 2;
            Message me = new Message(m, t, b);
            me.ShowDialog();


            if (Vars.mes_res != 1)
            {
                return;
            }
            else

            {
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand comm = new SqlCommand($@"update pol_events set unload=0 where idguid in (select event_guid from pol_persons where id in({sg_rows}))", con);
                con.Open();
                comm.ExecuteNonQuery();
                con.Close();
                pers_grid_Loaded(this, e);
            }

        }

        private void Prz_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (prz.SelectedIndex == -1)
            //{

            //}
            //else
            //{
            //    prz_Agent.DataContext = MyReader.MySelect<AgentsSmo>(@"select id,prz_code,agent from pol_prz_agents", Properties.Settings.Default.DocExchangeConnectionString);
            //    prz.DataContext = MyReader.MySelect<PrzSmo>(@"select id,prz_code,prz_name,NameWithCode from pol_prz", Properties.Settings.Default.DocExchangeConnectionString);
            //    Vars.PunctRz = prz.EditValue.ToString();

            //}

        }

        private void Prz_Agent_LostFocus(object sender, RoutedEventArgs e)
        {
            if (prz_Agent.SelectedIndex == -1)
            {

            }
            else
            {

                Vars.Agnt = Convert.ToInt32(prz_Agent.EditValue);
            }

        }

        private void PrintForms_Click(object sender, RoutedEventArgs e)
        {
            Vars.IdZl = Convert.ToInt32(pers_grid.GetFocusedRowCellValue("ID"));
            Docs_print docs = new Docs_print();
            docs.ShowDialog();
            pers_grid.UnselectAll();
            //layoutStream.Seek(0, SeekOrigin.Begin);
            //pers_grid.RestoreLayoutFromStream(layoutStream);
        }

        private void Pers_grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Prz_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

            if (prz.SelectedIndex == -1)
            {
                return;
            }
            else
            {
                if (SPR.Premmissions == "User")
                {
                    prz_Agent.DataContext = MyReader.MySelect<AgentsSmo>($@"select id,prz_code,agent 
from pol_prz_agents where prz_code like'%{prz.EditValue.ToString()}%' and agent = '{SPR.Login}'", Properties.Settings.Default.DocExchangeConnectionString);
                    prz_Agent.SelectedIndex = 0;
                }
                else
                {
                    prz_Agent.DataContext = MyReader.MySelect<AgentsSmo>($@"select id,prz_code,agent 
from pol_prz_agents where prz_code like'%{prz.EditValue.ToString()}%'", Properties.Settings.Default.DocExchangeConnectionString);
                    prz_Agent.SelectedIndex = 0;
                }
            }

        }

        private void Prz_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {

        }
        //---------------------------------------------------------------W2----------------------------------------------------------------

        private void dalee_Click(object sender, RoutedEventArgs e)
        {
            if (pers_grid.SelectedItems.Count > 1)
            {
                string m1 = "Вы выбрали больше 1 клиента!";
                string t1 = "Ошибка!";
                int b1 = 1;
                Message me1 = new Message(m1, t1, b1);
                me1.ShowDialog();

                return;
            }
            if (prz.SelectedIndex < 0 || prz_Agent.SelectedIndex < 0)
            {
                string m = "Выберите пункт выдачи полисов и агента!";
                string t = "Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                return;

            }
            //string petit;
            //if (petition.IsChecked == true)
            //{
            //    petit = "1";
            //}
            //else
            //{
            //    petit = "0";
            //}
            if (pers_grid.SelectedItems.Count != 0)
            {
                string iid;
                string btn = "3";

                Vars.IdZl = Convert.ToInt32(pers_grid.GetFocusedRowCellValue("ID"));


                Vars.W = pers_grid.GetFocusedRowCellValue("W").ToString();


                //if (cel_vizita.SelectedIndex == -1)
                //{
                //    MessageBox.Show("Выберите причину внесения изменений в РС ЕРЗ!");

                //    return;
                //}
                //else
                //{
                //    Vars.CelVisit = cel_vizita.EditValue.ToString();
                //    cel_viz = cel_vizita.EditValue.ToString();
                //}

                //string sppz;
                //if (sp_pod_z.EditValue == null)
                //{
                //    MessageBox.Show("Выберите способ подачи заявления!");
                //    return;
                //}
                //else
                //{
                //    Vars.Sposob = sp_pod_z.EditValue.ToString();
                //    sppz = sp_pod_z.EditValue.ToString();
                //}

                //string dobr = d_obr.ToString();
                Person_Data w2 = new Person_Data();

                //this.Visibility = Visibility.Collapsed;
                w2.Show();
                // this.Visibility = Visibility.Visible;
                //this.Close();

            }
            else
            {
                string m1 = "Выберите клиента";
                string t1 = "Ошибка!";
                int b1 = 1;
                Message me1 = new Message(m1, t1, b1);
                me1.ShowDialog();
                return;
            }
            //Vars.DateVisit = d_obr.DateTime;
        }



        private void pers_grid_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {


        }

        private void cel_vizita_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {


        }




        private void izm_klient_Click(object sender, RoutedEventArgs e)
        {
            if (pers_grid.SelectedItems.Count > 1)
            {
                string m1 = "Вы выбрали больше 1 клиента!";
                string t1 = "Ошибка!";
                int b1 = 1;
                Message me1 = new Message(m1, t1, b1);
                me1.ShowDialog();

                return;

            }
            if (prz.SelectedIndex < 0 || prz_Agent.SelectedIndex < 0)
            {
                string m = "Выберите пункт выдачи полисов и агента!";
                string t = "Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                return;
            }
            else
            {
                //Vars.PunctRz = prz.EditValue.ToString();
                //Vars.Agnt = Convert.ToInt32(prz_Agent.EditValue);
                //Work_ZL w1 = new Work_ZL();
                //w1.Owner = this;
                ////this.Visibility = Visibility.Collapsed;
                //w1.Show();
                //// this.Visibility = Visibility.Visible;
            }
            if (pers_grid.SelectedItems.Count == 0)
            {
                string m = "Выберите клиента!";
                string t = "Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                return;

            }
            else if (pers_grid.SelectedItems.Count != 0)
            {
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand comm = new SqlCommand(@"select tip_op from pol_events where person_guid=(select idguid from pol_persons where id=@id)", con);
                comm.Parameters.AddWithValue("@id", pers_grid.GetFocusedRowCellValue("ID").ToString());
                con.Open();
                SqlDataReader reader = comm.ExecuteReader();

                while (reader.Read()) // построчно считываем данные
                {
                    object tip_op = reader["TIP_OP"];

                    Vars.CelVisit = tip_op.ToString();
                    Vars.NewCelViz = 0;
                }

                reader.Close();

                con.Close();
                //MessageBox.Show("Выберите причину подачи заявления!");
                //return;

            }
            else
            {
                //Vars.CelVisit = cel_vizita.EditValue.ToString();
                //Vars.NewCelViz = 1;
            }
            Events Create = new Events();
            Create.DVIZIT = DateTime.Now;
            //string petit;
            //if (petition.IsChecked == true)
            //{
            //    petit = "1";
            //}
            //else
            //{
            //    petit = "0";
            //}
            if (pers_grid.SelectedItems.Count != 0)
            {
                string btn = "2";

                Vars.IdZl = Convert.ToInt32(pers_grid.GetFocusedRowCellValue("ID"));
                Vars.W = pers_grid.GetFocusedRowCellValue("W").ToString();
                string cel_viz;
                //if (cel_vizita.SelectedIndex == -1)
                //{
                //    cel_viz = "";
                //}
                //else
                //{
                //    cel_viz = cel_vizita.EditValue.ToString();
                //}

                //string sppz;
                //if (sp_pod_z.EditValue == null)
                //{
                //    sppz = "0";
                //}
                //else
                //{
                //    sppz = sp_pod_z.EditValue.ToString();
                //}
                //string dobr = d_obr.ToString();
                Person_Data w2 = new Person_Data();
                w2.Owner = this;
                //this.Visibility = Visibility.Collapsed;
                w2.Show();
                //this.Visibility = Visibility.Visible;
                //this.Close();

            }
            else
            {
                string m = "Выберите клиента!";
                string t = "Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                return;
            }
        }

        private void new_klient_Click(object sender, RoutedEventArgs e)
        {
            if (prz.SelectedIndex < 0 || prz_Agent.SelectedIndex < 0)
            {
                string m = "Выберите пункт выдачи полисов и агента!";
                string t = "Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                //MessageBox.Show("Выберите пункт выдачи полисов и агента!");
                return;
            }

            //string petit;
            //if (petition.IsChecked == true)
            //{
            //    petit = "1";
            //}
            //else
            //{
            //    petit = "0";
            //}
            if (pers_grid.SelectedItems.Count != 0)
            {
                //string btn = "1";

                //string iid = "";
                //string ppol = "";
                //if (cel_vizita.EditValue == null)
                //{
                //    MessageBox.Show("Выберите причину внесения изменений в РС ЕРЗ!!!");
                //    return;
                //}
                //else
                //{
                //    Vars.CelVisit = cel_vizita.EditValue.ToString();
                //    string cel_viz = cel_vizita.EditValue.ToString();
                //}


                //if (sp_pod_z.EditValue == null)
                //{
                //    MessageBox.Show("Выберите способ подачи заявления!");
                //    return;
                //}
                //else
                //{
                //    string sppz = sp_pod_z.EditValue.ToString();
                //    Vars.Sposob = sp_pod_z.EditValue.ToString();
                //    string dobr = d_obr.ToString();
                //    Person_Data w2 = new Person_Data(iid, btn, ppol, cel_viz, sppz, dobr, petit);
                //    w2.Owner = this;
                //    //this.Visibility = Visibility.Collapsed;
                //    w2.Show();
                //   // this.Visibility = Visibility.Visible;


                //}
                string m = "Выберите пункт выдачи полисов и агента!";
                string t = "Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                MessageBox.Show("Вы вводите данные на новое ЗЛ. Снимите в таблице выделение с уже существующего ЗЛ!!!", "Внимание!!!");
                return;
            }
            else
            {
                //string btn = "1";

                //string iid = "";
                //string ppol = "";
                //Vars.CelVisit = cel_vizita.EditValue.ToString();

                //if (cel_vizita.EditValue == null)
                //{
                //    MessageBox.Show("Выберите причину внесения изменений в РС ЕРЗ!!!");
                //    return;
                //}
                //else
                //{
                //    Vars.CelVisit = cel_vizita.EditValue.ToString();
                //    string cel_viz = cel_vizita.EditValue.ToString();
                //}

                //if (sp_pod_z.SelectedIndex < 0)
                //{
                //    MessageBox.Show("Выберите способ подачи заявления!!!");
                //    return;
                //}
                //else
                //{
                //    string sppz = sp_pod_z.EditValue.ToString();
                //    Vars.Sposob = sp_pod_z.EditValue.ToString();
                //    string dobr = d_obr.ToString();
                //    Person_Data w2 = new Person_Data(iid, btn, ppol, cel_viz, sppz, dobr, petit);
                //    w2.Owner = this;
                //    //this.Visibility = Visibility.Collapsed;
                //    w2.Show();
                //    //this.Visibility = Visibility.Visible;
                //    //this.Close();


                //}
            }
            //Vars.DateVisit = d_obr.DateTime;
        }

        private void pers_grid_1_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {

        }

        private void w_1_Loaded(object sender, RoutedEventArgs e)
        {
            //pers_grid_1.SelectedItem = -1;
            //cel_vizita.SelectedIndex = -1;
        }
        private int rows_count;
        private void pers_grid_1_Loaded_0(object sender, RoutedEventArgs e)
        {

            //            pers_grid_1.ShowLoadingPanel = true;
            //            //Insurance.DocExchangeDataSet docExchangeDataSet = ((Insurance.DocExchangeDataSet)(this.FindResource("docExchangeDataSet")));
            //            //// Загрузить данные в таблицу POL_PERSONS. Можно изменить этот код как требуется.
            //            //Insurance.DocExchangeDataSetTableAdapters.POL_PERSONSTableAdapter docExchangeDataSetPOL_PERSONSTableAdapter = new Insurance.DocExchangeDataSetTableAdapters.POL_PERSONSTableAdapter();
            //            //docExchangeDataSetPOL_PERSONSTableAdapter.Fill(docExchangeDataSet.POL_PERSONS);
            //            //System.Windows.Data.CollectionViewSource pOL_PERSONSViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("pOL_PERSONSViewSource")));
            //            //pOL_PERSONSViewSource.View.MoveCurrentToFirst();
            //            //pers_grid_1.Columns[1].Visible = false;
            //            //pers_grid_1.Columns[2].Visible = false;
            //            //pers_grid_1.Columns[3].Visible = false;
            //            //pers_grid_1.Columns[4].Visible = false;
            //            //pers_grid_1.View.FocusedRowHandle = -1;
            //            pers_grid_1.View.FocusedRowHandle = -1;
            //            if (Vars.F_ids != null)
            //            {
            //                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            //                var peopleList =
            //                        MyReader.MySelect<People>(
            //                            $@"
            //             SELECT top(1000)  p.ID,p.IDGUID,p.active,pe.TIP_OP,p.SS ,p.ENP ,p.FAM , p.IM  , p.OT ,p.W ,p.DR , p.PHONE ,
            //p.COMMENT ,f.NameWithID,op.filename
            //  FROM [dbo].[POL_PERSONS] p
            //left join f003 f on p.mo=f.mcod 
            //left join pol_events pe
            //on p.event_guid=pe.idguid
            //left join pol_oplist op
            //on p.idguid=op.person_guid
            //--where p.active=1 
            // order by ID desc", connectionString);
            //                pers_grid_1.ItemsSource = peopleList;
            //            }
            //            else
            //            {

            //            }
            //            pers_grid_1.View.FocusedRowHandle = -1;
            //            pers_grid_1.Columns[11].Width = 400;
            //            //if (Vars.F_ids != null)
            //            //{

            //            //    int i = 0;
            //            //    int j = 0;
            //            //    rows_count = pers_grid_1.VisibleRowCount;
            //            //    for (i = 0; i <= rows_count; i++)
            //            //    {
            //            //        pers_grid_1.View.FocusedRowHandle = i;
            //            //        for (j = 0; j < Vars.F_ids.Count(); j++)
            //            //        {
            //            //            if (pers_grid_1.GetFocusedRowCellDisplayText("ID").Contains(Vars.F_ids[j].ToString()))
            //            //            {

            //            //                pers_grid_1.SelectItem(i);
            //            //                //pol_zagr.View.MoveNextRow();
            //            //            }
            //            //        }
            //            //    }
            //            //}

            //            pers_grid_1.ShowLoadingPanel = false;
        }



        private void pers_grid_1_TextInput(object sender, TextCompositionEventArgs e)
        {


        }

        private void pers_grid_1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {


        }
        public ObservableCollection<string> list = new ObservableCollection<string>();



        private void pers_grid_1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //            if (e.Key == Key.Enter)
            //            {
            //                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            //                if (pers_grid_1.ActiveFilterInfo == null)
            //                {
            //                    var peopleList =
            //                    MyReader.MySelect<People>(
            //                        $@"
            //             SELECT top(1000) p.ID,p.IDGUID,p.active,pe.TIP_OP,p.SS ,p.ENP ,p.FAM , p.IM  , p.OT ,p.W ,p.DR , p.PHONE ,
            //p.COMMENT ,f.NameWithID,op.filename
            //  FROM [dbo].[POL_PERSONS] p
            //left join f003 f on p.mo=f.mcod 
            //left join pol_events pe
            //on p.event_guid=pe.idguid
            //left join pol_oplist op
            //on p.idguid=op.person_guid
            // order by p.ID", connectionString);
            //                    pers_grid_1.ItemsSource = peopleList;
            //                    pers_grid_1.View.FocusedRowHandle = -1;
            //                }
            //                else
            //                {
            //                    string strf = pers_grid_1.ActiveFilterInfo.FilterString.Substring(pers_grid_1.ActiveFilterInfo.FilterString.IndexOf("'", 0));
            //                    string strf1 = strf.Replace("'", "").Replace(")", "").Replace(".", "");
            //                    var peopleList =
            //                    MyReader.MySelect<People>(
            //                        $@"
            //             SELECT top(1000) p.ID,p.IDGUID,p.active,pe.TIP_OP,p.SS ,p.ENP ,p.FAM , p.IM  , p.OT ,p.W ,p.DR , p.PHONE ,
            //p.COMMENT ,f.NameWithID,op.filename
            //  FROM [dbo].[POL_PERSONS] p
            //left join f003 f on p.mo=f.mcod 
            //left join pol_events pe
            //on p.event_guid=pe.idguid
            //left join pol_oplist op
            //on p.idguid=op.person_guid
            //where p.FAM LIKE '{
            //                         strf1}%' OR  p.IM LIKE '{
            //                         strf1}%' OR  p.OT LIKE '{
            //                         strf1}%' order by p.ID", connectionString);
            //                    pers_grid_1.ItemsSource = peopleList;
            //                    pers_grid_1.View.FocusedRowHandle = -1;
            //                }




            //            }
        }

        private void tableView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {


        }

        private void tableView_ShowGridMenu(object sender, DevExpress.Xpf.Grid.GridMenuEventArgs e)
        {

            //BarSubItem mainMenu = new BarSubItem();
            //mainMenu.Name = "mainMenu";
            //mainMenu.Content = "Удалить";

            ////BarButtonItemLink barButtonItemLink = new BarButtonItemLink();
            ////barButtonItemLink.BarItemName = "subMenu";
            ////mainMenu.ItemLinks.Add(barButtonItemLink);
            ////e.Customizations.Add(subMenu);
            //e.Customizations.Add(mainMenu);

        }

        private void poisk_Click(object sender, RoutedEventArgs e)
        {
            if(Vars.SMO.Substring(0, 2) == "25")
            {
                Poisk_Vladik w4 = new Poisk_Vladik();
                w4.ShowDialog();
                Vladik();
            }
            else if(Vars.SMO.Substring(0, 2) == "46")
            {
                Poisk w4 = new Poisk();
                w4.ShowDialog();
            }
            
        }

        private void del_btn_Click(object sender, RoutedEventArgs e)
        {
            if (SPR.Premmissions == "User")
            {
                string m0 = "У вас недостаточно прав доступа для данной операции!";
                string t0 = "Ошибка!";
                int b0 = 1;
                Message me0 = new Message(m0, t0, b0);
                me0.ShowDialog();
                return;
            }

            string sg_rows_e = " ";
            string sg_rows_zl = " ";
            int[] rt = pers_grid.GetSelectedRowHandles();
            for (int i = 0; i < rt.Count(); i++)

            {
                var ddd_e = pers_grid.GetCellValue(rt[i], "EVENT_ID");
                var sgr_e = sg_rows_e.Insert(sg_rows_e.Length, ddd_e.ToString()) + ",";
                sg_rows_e = sgr_e;
                var ddd_zl = pers_grid.GetCellValue(rt[i], "ID");
                var sgr_zl = sg_rows_zl.Insert(sg_rows_zl.Length, ddd_zl.ToString()) + ",";
                sg_rows_zl = sgr_zl;
            }

            sg_rows_e = sg_rows_e.Substring(0, sg_rows_e.Length - 1);
            sg_rows_zl = sg_rows_zl.Substring(0, sg_rows_zl.Length - 1);

            string m = "Вы действительно хотите удалить последние события для ЗЛ?";
            string t = "Внимание!";
            int b = 2;
            Message me = new Message(m, t, b);
            me.ShowDialog();

            //var result = MessageBox.Show("Вы действительно хотите удалить записи", "Внимание!", MessageBoxButton.YesNo);
            if (Vars.mes_res != 1)
            {
                return;
            }
            else
            {
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand comm = new SqlCommand($@"
delete from POL_PERSONS_OLD where EVENT_GUID in(select IDGUID from POL_EVENTS where id in({sg_rows_e}))
delete from POL_DOCUMENTS_OLD where EVENT_GUID in(select IDGUID from POL_EVENTS where id in({sg_rows_e}))
delete from POL_POLISES where EVENT_GUID in(select IDGUID from POL_EVENTS where id in({sg_rows_e}))
delete from POL_RELATION_DOC_PERS where EVENT_GUID in(select IDGUID from POL_EVENTS where id in({sg_rows_e}))
delete from POL_DOCUMENTS where EVENT_GUID in(select IDGUID from POL_EVENTS where id in({sg_rows_e}))
delete from POL_RELATION_ADDR_PERS where EVENT_GUID in(select IDGUID from POL_EVENTS where id in({sg_rows_e}))
delete from POL_ADDRESSES where EVENT_GUID in (select IDGUID from POL_EVENTS where id in({sg_rows_e}))
delete from POL_PERSONSB where EVENT_GUID in(select IDGUID from POL_EVENTS where id in({sg_rows_e}))
delete from POL_OPLIST where EVENT_GUID in(select IDGUID from POL_EVENTS where id in({sg_rows_e}))
delete from POL_EVENTS where IDGUID in(select IDGUID from POL_EVENTS where id in({sg_rows_e}))
 
DECLARE @eventId int,@evguid uniqueidentifier, @perguid uniqueidentifier

DECLARE MY_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
FOR 
SELECT max(id),idguid,PERSON_GUID 
FROM POL_EVENTS where PERSON_GUID in(select idguid from POL_PERSONS where id in({sg_rows_zl})) group by IDGUID,PERSON_GUID

OPEN MY_CURSOR
FETCH NEXT FROM MY_CURSOR INTO @eventId,@evguid, @perguid
WHILE @@FETCH_STATUS = 0
BEGIN 
    --Do something with Id here
	update POL_PERSONS set EVENT_GUID=@evguid
    from POL_EVENTS e where POL_PERSONS.IDGUID=@perguid
    update pol_documents set active=1 where event_guid=@evguid

    PRINT @evguid
    FETCH NEXT FROM MY_CURSOR INTO @eventId,@evguid, @perguid
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR
", con);
                con.Open();
                comm.ExecuteNonQuery();
                con.Close();
                int kol_zap = rt.Count();
                int lastnumber = kol_zap % 10;
                if (lastnumber > 1 && lastnumber < 5)
                {
                    string m1 = " Успешно удалено  " + rt.Count() + " записи!";
                    string t1 = "Сообщение";
                    int b1 = 1;
                    Message me1 = new Message(m1, t1, b1);
                    me1.ShowDialog();
                    //MessageBox.Show(" Успешно удалено  " + rt.Count() + " записи!");
                }
                else if (lastnumber == 1)
                {
                    string m1 = " Успешно удалено  " + rt.Count() + " запись!";
                    string t1 = "Сообщение";
                    int b1 = 1;
                    Message me1 = new Message(m1, t1, b1);
                    me1.ShowDialog();
                    //MessageBox.Show(" Успешно удалена  " + rt.Count() + " запись!");
                }
                else
                {
                    string m1 = " Успешно удалено  " + rt.Count() + " записей!";
                    string t1 = "Сообщение";
                    int b1 = 1;
                    Message me1 = new Message(m1, t1, b1);
                    me1.ShowDialog();
                    //MessageBox.Show(" Успешно удалено  " + rt.Count() + " записей!");
                }

                pers_grid_Loaded(sender, e);
            }
        }

        private void new_klient_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {

        }
        private void GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {

        }

        private void print_Click(object sender, RoutedEventArgs e)
        {
            Vars.IdZl = Convert.ToInt32(pers_grid.GetFocusedRowCellValue("ID"));
            Vars.Forms = 1;
            Docs_print docs = new Docs_print();
            docs.ShowDialog();
            //int idpers = Convert.ToInt32(pers_grid_1.GetFocusedRowCellValue("ID"));
            //Zayavlenie shreport = new Zayavlenie(idpers);
            //shreport.Show();

        }

        private void W_1_Activated(object sender, EventArgs e)
        {



        }

        private void W_1_Closed(object sender, EventArgs e)
        {
            ////this.Owner.Visibility = Visibility.Visible;
            //this.Owner.Focus();
            //Vars.F_ids = null;
        }

        private void TableView_EditFormShowing(object sender, EditFormShowingEventArgs e)
        {

        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            print_Click(this, e);
        }
        private void Version_Check()
        {
            try
            {
                // Create a request for the URL.        
                HttpWebRequest requestt = (HttpWebRequest)HttpWebRequest.Create("http://elmedicine.ru");
                requestt.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";


                HttpWebResponse response = (HttpWebResponse)requestt.GetResponse();
                Stream ReceiveStream1 = response.GetResponseStream();
                StreamReader sr = new StreamReader(ReceiveStream1, true);
                string responseFromServer = sr.ReadToEnd();

                response.Close();

            }
            catch (Exception ex)
            {
                string m = "Невозможно проверить обновление программы! Нет подключениея к Internet!";
                string t = "Внимание!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                //MessageBox.Show("Невозможно проверить обновление программы! Нет подключениея к Internet!", "Ошибка!");

            }
            try
            {

                string html;
                string version = Vars.MainTitle.Substring(Vars.MainTitle.Length - 6, 6);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://elmedicine.ru/INSURANCE/version.txt");
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }
                if (html.Contains("stop"))
                {

                    Environment.Exit(0);
                }
                else
                if (Convert.ToInt32(html.Substring(html.Length - 3, 3)) > Convert.ToInt32(version.Substring(version.Length - 3, 3)))
                {
                    string m = "Доступно обновление программы! Хотите установить сейчас?";
                    string t = "Внимание!";
                    int b = 2;
                    Message me = new Message(m, t, b);
                    me.ShowDialog();
                    if (Vars.mes_res == 1)
                    {

                        System.Diagnostics.Process.Start("Updater Inshurance.exe");
                    }
                    else
                    {
                        return;
                    }
                }
                else if (Convert.ToInt32(html.Substring(html.Length - 3, 3)) <= Convert.ToInt32(version.Substring(version.Length - 3, 3)) && upd_click == true && upd_click == true)
                {
                    string m = "Вы ипользуете последнюю версию программы!";
                    string t = "Сообщение";
                    int b = 1;
                    Message me = new Message(m, t, b);
                    me.ShowDialog();
                    return;

                }
            }
            catch
            {

            }
            //if (File.Exists("VersionSoftware.txt") == false)
            //{
            //    MessageBox.Show("Невозможно определить версию программы! Отсутствуют необходимые файлы!","Ошибка!");
            //    return;
            //}
            //else
            //{


            //    try
            //    {
            //        string a1 = File.ReadAllText("VersionSoftware.txt");

            //        string a2 = Title.Substring(Title.Length - 6, 6);


            //        if (a1 != a2)
            //        {
            //            var result = MessageBox.Show("Доступно обновление программы! Хотите установить сейчас?", "Внимание!", MessageBoxButton.YesNo);
            //            if (result == MessageBoxResult.Yes)
            //            {
            //                System.Diagnostics.Process.Start("Updater_Inshurance.exe");
            //            }
            //            else
            //            {
            //                return;
            //            }
            //        }
            //        else 
            //        {
            //            MessageBox.Show("Вы ипользуете последнюю версию программы");

            //            return;

            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        System.Windows.MessageBox.Show("Ошибка!" + ex.Message);

            //    }






            //}

        }
        //Stream layoutStream;
        private void Help_btn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Environment.CurrentDirectory + @"\Instruction_new.doc");
            //MemoryStream stream = new MemoryStream();
            //pers_grid.SaveLayoutToStream(stream); // works alright
            //this.layoutStream = stream;

        }

        private void Fias_upd_Click(object sender, RoutedEventArgs e)
        {
            if (SPR.Premmissions != "Admin")
            {
                string m = "У вас недостаточно прав для данной операции!";
                string t = "Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
            }
            else
            {

                string m = "Обновление справочника ФИАС может занять продолжительное время. Вы хотите обновить справочник сейчас?";
                string t = "Внимание!";
                int b = 2;
                Message me = new Message(m, t, b);
                me.ShowDialog();

                //var result = MessageBox.Show("Вы действительно хотите удалить записи", "Внимание!", MessageBoxButton.YesNo);
                if (Vars.mes_res != 1)
                {
                    return;
                }
                else
                {
                    FIAS_UPD f = new FIAS_UPD();
                    f.ShowDialog();
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------------------


        //public class People1
        //{
        //    public string NPOL { get; set; }
        //    public string ENP { get; set; }

        //    public string FAM { get; set; }
        //    public string IM { get; set; }
        //    public string OT { get; set; }
        //    public Int32 W { get; set; }
        //    public DateTime DR { get; set; }
        //    public string RNNAME { get; set; }
        //    public string CITY { get; set; }
        //    public string NP { get; set; }
        //    public string UL { get; set; }
        //    public string DOM { get; set; }
        //    public string KOR { get; set; }
        //    public string KV { get; set; }
        //    public string Q { get; set; }

        //}

        /// <summary>
        /// Логика взаимодействия для Window2.xaml
        /// </summary>






        private void PutImageBinaryInDb()
        {

            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);


            SqlCommand command = new SqlCommand("insert into pol_personsb (photo,person_guid,type) values(@screen,(select idguid from pol_persons where id=@id ),2)", con);
            if (zl_photo.EditValue == null || zl_photo.EditValue.ToString() == "")
            {
                command.Parameters.AddWithValue("@screen", "");
            }
            else
            {
                command.Parameters.AddWithValue("@screen", Convert.ToBase64String((byte[])zl_photo.EditValue)); // записываем само изображение
            }

            command.Parameters.AddWithValue("@id", Convert.ToInt32(Vars.IdP));




            //command.Parameters.AddWithValue("@guid", iImageExtension); // записываем расширение изображения
            con.Open();
            command.ExecuteNonQuery();
            con.Close();

        }

        private void PutImageBinaryInDb1()
        {

            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);


            SqlCommand command = new SqlCommand("insert into pol_personsb (photo,person_guid,type) values(@screen1,(select idguid from pol_persons where id=@id ),3)", con);


            command.Parameters.AddWithValue("@id", Convert.ToInt32(Vars.IdP));

            if (zl_podp.EditValue == null)
            {
                command.Parameters.AddWithValue("@screen1", "");
            }
            else
            {
                command.Parameters.AddWithValue("@screen1", Convert.ToBase64String((byte[])zl_podp.EditValue));
            }



            //command.Parameters.AddWithValue("@guid", iImageExtension); // записываем расширение изображения
            con.Open();
            command.ExecuteNonQuery();
            con.Close();

        }



        private void nazad_Click(object sender, RoutedEventArgs e)
        {

            G_layuot.save_Layout(Properties.Settings.Default.DocExchangeConnectionString, pers_grid, pers_grid_2);
            //restore_Layout();

            //list0.Clear();
            //list1.Clear();
            //list2.Clear();
            //list3.Clear();
            //list4.Clear();
            MainTab.SelectedIndex = 0;
            Tab_ZL.Visibility = Visibility.Hidden;
            InsMethods.PersData_Default(this);
            this.Title = Vars.MainTitle;


            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            if (pers_grid.ActiveFilterInfo == null)
            {
                if (SPR.Premmissions == "User")
                {

                    var peopleList = MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf_usr, connectionString);
                    pers_grid.ItemsSource = peopleList;
                    pers_grid.View.FocusedRowHandle = -1;
                    //////ev = MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf_usr, connectionString);
                    //////pers_grid.ItemsSource = ev;
                    //////pers_grid.View.FocusedRowHandle = -1;
                }
                else
                {

                    var peopleList = MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf_adm, connectionString);
                    pers_grid.ItemsSource = peopleList;
                    pers_grid.View.FocusedRowHandle = -1;
                    //////ev = MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf_adm, connectionString);
                    //////pers_grid.ItemsSource = ev;
                    //////pers_grid.View.FocusedRowHandle = -1;
                }
            }
            else
            {
                var fcrt = pers_grid.ActiveFilterInfo.FilterOperator;
                if (SPR.Premmissions == "User")
                {

                    var peopleList = MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf_usr, connectionString);
                    pers_grid.ItemsSource = peopleList;
                    pers_grid.View.FocusedRowHandle = -1;
                    //////ev = MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf_usr, connectionString);
                    //////pers_grid.ItemsSource = ev;
                    //////pers_grid.View.FocusedRowHandle = -1;
                }
                else
                {

                    var peopleList = MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf_adm, connectionString);
                    pers_grid.ItemsSource = peopleList;
                    pers_grid.View.FocusedRowHandle = -1;
                    //////ev = MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf_adm, connectionString);
                    //////pers_grid.ItemsSource = ev;
                    //////pers_grid.View.FocusedRowHandle = -1;
                }

                pers_grid.FilterCriteria = fcrt;
                
                
            }

            //InsMethods.PersData_Default(this);
            //layout_InUse();

        }


        private void dalee_Click_1(object sender, RoutedEventArgs e)
        {
            if(ddnum.Text!="" && (int)type_policy.EditValue==2 && fakt_prekr.EditValue==null)
            {
                string m = "Дата прекращения ВС не может быть пустой у иностранца!";
                string t = "Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                
                return;
            }
            else if(ddnum.Text != "" && (int)type_policy.EditValue == 2 )
            {
                if(docexp1.EditValue==null && (int)ddtype.EditValue != 11)
                {
                    string m = "Дата окончания ДД может быть пустой только у бессрочного вида на жительство!";
                    string t = "Ошибка!";
                    int b = 1;
                    Message me = new Message(m, t, b);
                    me.ShowDialog();

                    return;
                }
                else if(docexp1.EditValue != null && (DateTime)(docexp1.EditValue ?? new DateTime(1900, 1, 1)) < (DateTime)(fakt_prekr.EditValue))
                {
                    string m = "Дата прекращения ВС не может быть больше даты окончания действия ДД!";
                    string t = "Ошибка!";
                    int b = 1;
                    Message me = new Message(m, t, b);
                    me.ShowDialog();

                    return;
                }
                
                
            }
            if((im.Text=="" || ot.Text=="") && dost1.EditValue==null)
            {
                string m = "Проверьте ФИО и надежность идентификации!";
                string t = "Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();

                return;
            }
            if((cel_vizita.EditValue.ToString().Contains("П010") && pr_pod_z_smo.SelectedIndex!=0) || (cel_vizita.DisplayText.Contains("желани") && pr_pod_z_smo.SelectedIndex != 1)
                || (cel_vizita.DisplayText.Contains("житель") && pr_pod_z_smo.SelectedIndex != 2) || (cel_vizita.DisplayText.Contains("расторж") && pr_pod_z_smo.SelectedIndex != 3))
            {
                string m = "Несовпадение причины внесения изменений в РС ЕРЗ и причины подачи заявления о выборе (замене) СМО";
                string t = "Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();

                return;
            }
            Vars.PunctRz = prz.EditValue.ToString();
            Vars.mes_res = 0;
            if (kat_zl.SelectedIndex == -1)
            {
                string m = "Вы не указали категорию ЗЛ!";
                string t = "Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();

                return;
            }

            //MessageBox.Show(fakt_prekr.DisplayText.ToString());
            Events Create = new Events();
            Create.DVIZIT = DateTime.Now;
            prev_doc_stringSql(); //Предидущий документ
            //----------------------------------------------------------------------------------------
            // При нажатии кнопки "Далее" если в предидущем окне была нажата кнопка "Новый клиент".
            DateTime firstDate = dr.DateTime;
            DateTime secondDate = DateTime.Now;
            TimeSpan interval = secondDate.Subtract(firstDate);
            if (interval.Days / 365.25 < 14 && fam1.Text == "")
            {
                string m = "У ребенка до 14 лет обязательно должен быть представитель!";
                string t = "Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                return;
            }

            if (interval.Days / 365.25 < 14 && Vars.Sposob == "1")
            {
                string m = "Для ребенка до 14 лет выбран способ подачи заявления " + (char)34 + "Лично" + (char)34 +
                           "!!! Измените способ подачи на " + (char)34 + "Представитель" + (char)34 + "!";
                string t = "Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                return;
            }

            //-------------------------------------------------------------------------------------------------
            // если нет дополнительного документа
            if (ddtype.EditValue == null)
            {
                if (Vars.Btn == "1")
                {
                    //----------------------------------------------------------------------------
                    if (fias.reg_dom.EditValue != null)
                    {
                        dstrkor = fias.reg_dom.Text.Replace(' ', ',').Split(',');
                        domsplit = dstrkor[0].Replace("д.", "");
                    }
                    else
                    {
                        domsplit = "";
                    }
                    if (fias1.reg_dom1.EditValue != null)
                    {
                        dstrkor1 = fias1.reg_dom1.Text.Replace(' ', ',').Split(',');
                        domsplit1 = dstrkor1[0].Replace("д.", "");
                    }
                    else
                    {
                        domsplit1 = "";
                    }
                    //Не БОМЖ и адрес регистрации и адрес проживания не совпадают;

                    if (fias.bomj.IsChecked == false && fias1.sovp_addr.IsChecked == false)
                    {

                        //Если тип полиса - временное свидетельство и поле серия не пусто;

                        if (type_policy.EditValue.ToString() == "2" && spolis_ != null)
                        {

                            InsMethods.Save_bt1_b0_s0_p2_sp1(this);
                            return;

                        }
                        //-----------------------------------------------------------------------
                        //Если тип полиса - временное свидетельство и поле серия пусто;
                        else if (type_policy.EditValue.ToString() == "2" && spolis_ == null)
                        {


                            InsMethods.Save_bt1_b0_s0_p2_sp0(this);
                            return;

                        }

                        //------------------------------------------------------------
                        //Если тип полиса - не временное свидетельство;

                        else
                        {


                            InsMethods.Save_bt1_b0_s0_p13(this);
                            return;
                        }
                    }

                    //----------------------------------------------------------------------------
                    //Если БОМЖ и адреса не совпадают

                    else if (fias.bomj.IsChecked == true && fias1.sovp_addr.IsChecked == false)
                    {

                        if (type_policy.EditValue.ToString() == "2" && spolis_ == null)
                        {


                            InsMethods.Save_bt1_b1_s0_p2_sp0(this);
                            return;
                        }
                        else if (type_policy.EditValue.ToString() == "2" && spolis_ != null)
                        {


                            InsMethods.Save_bt1_b1_s0_p2_sp1(this);
                            return;
                        }
                        else
                        {


                            InsMethods.Save_bt1_b1_s0_p13(this);
                            return;
                        }
                    }
                    //---------------------------------------------------------------------------------------------
                    // Иначе;      
                    else
                    {
                        if (fias.reg_dom.EditValue != null)
                        {
                            dstrkor = fias.reg_dom.Text.Replace(' ', ',').Split(',');
                            domsplit = dstrkor[0].Replace("д.", "");
                        }
                        else
                        {
                            domsplit = "";
                        }
                        if (fias1.reg_dom1.EditValue != null)
                        {
                            dstrkor1 = fias1.reg_dom1.Text.Replace(' ', ',').Split(',');
                            domsplit1 = dstrkor1[0].Replace("д.", "");
                        }
                        else
                        {
                            domsplit1 = "";
                        }


                        if (type_policy.EditValue.ToString() == "2" && spolis_ != null)
                        {


                            InsMethods.Save_bt1_b0_s1_p2_sp1(this);
                            return;
                        }
                        else if (type_policy.EditValue.ToString() == "2" && spolis_ == null)
                        {


                            InsMethods.Save_bt1_b0_s1_p2_sp0(this);
                            return;

                        }
                        else
                        {


                            InsMethods.Save_bt1_b0_s1_p13(this);
                            return;
                        }
                    }

                }
                //------------------------------------------------------------------------------
                // При нажатии кнопки "Далее" если в предидущем окне была нажата кнопка "Далее".
                else if (Vars.Btn == "3")
                {
                    if (fias.reg_dom.EditValue != null)
                    {
                        dstrkor = fias.reg_dom.Text.Replace(' ', ',').Split(',');
                        domsplit = dstrkor[0].Replace("д.", "");
                    }
                    else
                    {
                        domsplit = "";
                    }
                    if (fias1.reg_dom1.EditValue != null)
                    {
                        dstrkor1 = fias1.reg_dom1.Text.Replace(' ', ',').Split(',');
                        domsplit1 = dstrkor1[0].Replace("д.", "");
                    }
                    else
                    {
                        domsplit1 = "";
                    }

                    if (fias.bomj.IsChecked == false && fias1.sovp_addr.IsChecked == false)
                    {


                        InsMethods.Save_bt3_b0_s0_p13(this);
                        return;
                    }
                    else if (fias.bomj.IsChecked == true && fias1.sovp_addr.IsChecked == false)
                    {


                        InsMethods.Save_bt3_b1_s0_p13(this);
                        return;
                    }
                    else
                    {


                        InsMethods.Save_bt3_b0_s1_p13(this);
                        return;
                    }



                }

                //--------------------------------------------------------------------------------------------------
                // При нажатии кнопки "Далее" если в предидущем окне была нажата кнопка "Изменить данные клиента".
                else
                {
                    if (fias.reg_dom.EditValue != null)
                    {
                        dstrkor = fias.reg_dom.Text.Replace(' ', ',').Split(',');
                        domsplit = dstrkor[0].Replace("д.", "");
                    }
                    else
                    {
                        domsplit = "";
                    }
                    if (fias1.reg_dom1.EditValue != null)
                    {
                        dstrkor1 = fias1.reg_dom1.Text.Replace(' ', ',').Split(',');
                        domsplit1 = dstrkor1[0].Replace("д.", "");
                    }
                    else
                    {
                        domsplit1 = "";
                    }
                    if (Vars.NewCelViz == 0)
                    {
                        string m1 = "Цель визита останется прежней. Вы согласны?";
                        string t1 = "Внимание!";
                        int b1 = 2;
                        Message me1 = new Message(m1, t1, b1);
                        me1.ShowDialog();

                        if (Vars.mes_res == 1)
                        {

                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        string m1 = "Цель визита будет заменена на " + (char)34 + Vars.CelVisit + (char)34 +
                                    ". Вы согласны?";
                        string t1 = "Внимание!";
                        int b1 = 2;
                        Message me1 = new Message(m1, t1, b1);
                        me1.ShowDialog();

                        if (Vars.mes_res == 1)
                        {

                        }
                        else
                        {
                            return;
                        }
                    }

                    var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                    SqlConnection con = new SqlConnection(connectionString);
                    SqlCommand comm =
                        new SqlCommand(
                            "select PERSON_GUID as prf from pol_personsb where person_guid=(select idguid from pol_persons where id=@id) and type=2",
                            con);
                    comm.Parameters.AddWithValue("@id", Vars.IdP);
                    con.Open();
                    Guid prf1 = (Guid)comm.ExecuteScalar();
                    con.Close();

                    SqlConnection con1 = new SqlConnection(connectionString);
                    SqlCommand comm1 =
                        new SqlCommand(
                            "select PERSON_GUID as prp from pol_personsb where person_guid=(select idguid from pol_persons where id=@id) and type=3",
                            con1);
                    comm1.Parameters.AddWithValue("@id", Vars.IdP);
                    con1.Open();
                    Guid prp1 = (Guid)comm1.ExecuteScalar();
                    con1.Close();


                    if (prf1 == null && prp1 == null)
                    {
                        PutImageBinaryInDb();
                        PutImageBinaryInDb1();
                        InsMethods.Save_bt2_prf1(this);
                        return;
                    }
                    else if (prf1 != null && prp1 == null)
                    {
                        PutImageBinaryInDb1();
                        InsMethods.Save_bt2_prf1(this);
                        return;
                    }
                    else if (prf1 == null && prp1 != null)
                    {
                        PutImageBinaryInDb();
                        InsMethods.Save_bt2_prf1(this);
                        return;
                    }
                    else
                    {
                        InsMethods.Save_bt2_prf1(this);
                        return;
                    }
                    //{

                    //    //Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                    //    //     new Action(delegate ()
                    //    //     {

                    //    InsMethods.Save_bt2_prf1(this);
                    //    //}));
                    //    return;
                    //}


                    //if (prp1 == null)
                    //{
                    //    PutImageBinaryInDb1();
                    //}
                    //else
                    //{


                    //    InsMethods.Save_bt2_prp1(this);

                    //    return;
                    //}




                }

            }
            //------------------------------------------------------------------------------------------
            //если есть дополнительный документ
            else
            {
                if (Vars.Btn == "1")
                {
                    //----------------------------------------------------------------------------
                    //Не БОМЖ и адрес регистрации и адрес проживания не совпадают;

                    if (fias.bomj.IsChecked == false && fias1.sovp_addr.IsChecked == false)
                    {


                        //Если тип полиса - временное свидетельство и поле серия не пусто;

                        if (type_policy.EditValue.ToString() == "2" && spolis_ != null)
                        {


                            InsMethods.SaveDD_bt1_b0_s0_p2_sp1(this);
                            return;

                        }
                        //-----------------------------------------------------------------------
                        //Если тип полиса - временное свидетельство и поле серия пусто;
                        else if (type_policy.EditValue.ToString() == "2" && spolis_ == null)
                        {


                            InsMethods.SaveDD_bt1_b0_s0_p2_sp0(this);
                            return;

                        }

                        //------------------------------------------------------------
                        //Если тип полиса - не временное свидетельство;

                        else
                        {


                            InsMethods.SaveDD_bt1_b0_s0_p13(this);
                            return;
                        }
                    }

                    //----------------------------------------------------------------------------
                    //Если БОМЖ и адреса не совпадают

                    else if (fias.bomj.IsChecked == true && fias1.sovp_addr.IsChecked == false)
                    {

                        if (type_policy.EditValue.ToString() == "2" && spolis_ == null)
                        {


                            InsMethods.SaveDD_bt1_b1_s0_p2_sp0(this);
                            return;
                        }
                        else if (type_policy.EditValue.ToString() == "2" && spolis_ != null)
                        {


                            InsMethods.SaveDD_bt1_b1_s0_p2_sp1(this);
                            return;
                        }
                        else
                        {


                            InsMethods.SaveDD_bt1_b1_s0_p13(this);
                            return;
                        }
                    }
                    //---------------------------------------------------------------------------------------------
                    // Иначе;      
                    else
                    {

                        if (type_policy.EditValue.ToString() == "2" && spolis_ != null)
                        {

                            if (fias.reg_dom.EditValue != null)
                            {
                                dstrkor = fias.reg_dom.Text.Replace(' ', ',').Split(',');
                                domsplit = dstrkor[0].Replace("д.", "");
                            }
                            else
                            {
                                domsplit = "";
                            }
                            if (fias1.reg_dom1.EditValue != null)
                            {
                                dstrkor1 = fias1.reg_dom1.Text.Replace(' ', ',').Split(',');
                                domsplit1 = dstrkor1[0].Replace("д.", "");
                            }
                            else
                            {
                                domsplit1 = "";
                            }
                            InsMethods.SaveDD_bt1_b0_s1_p2_sp1(this);
                            return;
                        }
                        else if (type_policy.EditValue.ToString() == "2" && spolis_ == null)
                        {
                            if (fias.reg_dom.EditValue != null)
                            {
                                dstrkor = fias.reg_dom.Text.Replace(' ', ',').Split(',');
                                domsplit = dstrkor[0].Replace("д.", "");
                            }
                            else
                            {
                                domsplit = "";
                            }
                            if (fias1.reg_dom1.EditValue != null)
                            {
                                dstrkor1 = fias1.reg_dom1.Text.Replace(' ', ',').Split(',');
                                domsplit1 = dstrkor1[0].Replace("д.", "");
                            }
                            else
                            {
                                domsplit1 = "";
                            }

                            InsMethods.SaveDD_bt1_b0_s1_p2_sp0(this);
                            return;

                        }
                        else
                        {

                            InsMethods.SaveDD_bt1_b0_s1_p13(this);
                            return;
                        }
                    }


                }
                //------------------------------------------------------------------------------
                // При нажатии кнопки "Далее" если в предидущем окне была нажата кнопка "Далее".
                else if (Vars.Btn == "3")
                {
                    if (fias.reg_dom.EditValue != null)
                    {
                        dstrkor = fias.reg_dom.Text.Replace(' ', ',').Split(',');
                        domsplit = dstrkor[0].Replace("д.", "");
                    }
                    else
                    {
                        domsplit = "";
                    }
                    if (fias1.reg_dom1.EditValue != null)
                    {
                        dstrkor1 = fias1.reg_dom1.Text.Replace(' ', ',').Split(',');
                        domsplit1 = dstrkor1[0].Replace("д.", "");
                    }
                    else
                    {
                        domsplit1 = "";
                    }

                    if (fias.bomj.IsChecked == false && fias1.sovp_addr.IsChecked == false)
                    {


                        InsMethods.SaveDD_bt3_b0_s0_p13(this);
                        return;
                    }
                    else if (fias.bomj.IsChecked == true && fias1.sovp_addr.IsChecked == false)
                    {


                        InsMethods.SaveDD_bt3_b1_s0_p13(this);
                        return;
                    }
                    else
                    {


                        InsMethods.SaveDD_bt3_b0_s1_p13(this);
                        return;
                    }


                }

                //--------------------------------------------------------------------------------------------------
                // При нажатии кнопки "Далее" если в предидущем окне была нажата кнопка "Изменить данные клиента".
                else
                {
                    if (fias.reg_dom.EditValue != null)
                    {
                        dstrkor = fias.reg_dom.Text.Replace(' ', ',').Split(',');
                        domsplit = dstrkor[0].Replace("д.", "");
                    }
                    else
                    {
                        domsplit = "";
                    }
                    if (fias1.reg_dom1.EditValue != null)
                    {
                        dstrkor1 = fias1.reg_dom1.Text.Replace(' ', ',').Split(',');
                        domsplit1 = dstrkor1[0].Replace("д.", "");
                    }
                    else
                    {
                        domsplit1 = "";
                    }
                    if (Vars.NewCelViz == 0)
                    {
                        string m1 = "Цель визита останется прежней. Вы согласны?";
                        string t1 = "Внимание!";
                        int b1 = 2;
                        Message me1 = new Message(m1, t1, b1);
                        me1.ShowDialog();

                        if (Vars.mes_res == 1)
                        {

                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        string m1 = "Цель визита будет заменена на " + (char)34 + Vars.CelVisit + (char)34 +
                                    ". Вы согласны?";
                        string t1 = "Внимание!";
                        int b1 = 2;
                        Message me1 = new Message(m1, t1, b1);
                        me1.ShowDialog();

                        if (Vars.mes_res == 1)
                        {

                        }
                        else
                        {
                            return;
                        }

                    }

                    var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                    SqlConnection con = new SqlConnection(connectionString);
                    SqlCommand comm =
                        new SqlCommand(
                            "select PERSON_GUID as prf from pol_personsb where person_guid=(select idguid from pol_persons where id=@id) and type=2",
                            con);
                    comm.Parameters.AddWithValue("@id", Vars.IdP);
                    con.Open();
                    Guid prf1 = (Guid)comm.ExecuteScalar();
                    con.Close();

                    SqlConnection con1 = new SqlConnection(connectionString);
                    SqlCommand comm1 =
                        new SqlCommand(
                            "select PERSON_GUID as prp from pol_personsb where person_guid=(select idguid from pol_persons where id=@id) and type=3",
                            con1);
                    comm1.Parameters.AddWithValue("@id", Vars.IdP);
                    con1.Open();
                    Guid prp1 = (Guid)comm1.ExecuteScalar();
                    con1.Close();


                    if (prf1 == null && prp1 == null)
                    {
                        PutImageBinaryInDb();
                        PutImageBinaryInDb1();
                        InsMethods.SaveDD_bt2_prf1(this);
                        return;
                    }
                    else if (prf1 != null && prp1 == null)
                    {
                        PutImageBinaryInDb1();
                        InsMethods.SaveDD_bt2_prf1(this);
                        return;
                    }
                    else if (prf1 == null && prp1 != null)
                    {
                        PutImageBinaryInDb();
                        InsMethods.SaveDD_bt2_prf1(this);
                        return;
                    }
                    else
                    {
                        InsMethods.SaveDD_bt2_prf1(this);
                        return;
                    }




                }

            }

            var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
            if (SPR.Premmissions == "User")
            {
                var peopleList = MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf_usr, connectionString1);
                pers_grid.ItemsSource = peopleList;
                pers_grid.View.FocusedRowHandle = -1;

            }
            else
            {
                var peopleList = MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf_adm, connectionString1);
                pers_grid.ItemsSource = peopleList;
                pers_grid.View.FocusedRowHandle = -1;


            }
        }

        public ObservableCollection<string> list0 = new ObservableCollection<string>();
        public ObservableCollection<string> list1 = new ObservableCollection<string>();
        public ObservableCollection<string> list2 = new ObservableCollection<string>();
        public ObservableCollection<string> list3 = new ObservableCollection<string>();
        public ObservableCollection<Dost> list4 = new ObservableCollection<Dost>();
        public ObservableCollection<Table> list5 = new ObservableCollection<Table>();



        public string L1;
        public string L3;
        public string L4;
        public string L6;
        public string L7;

        public string L1_1;
        public string L3_1;
        public string L4_1;
        public string L6_1;
        public string L7_1;
        public string spolis_;
        public string idguid;

        public string domsplit;
        public string domsplit1;
        public string[] dstrkor;
        public string[] dstrkor1;
        public int old_doc;
        public string old_doc_guid;
        public int dop_doc;
        public string dop_doc_guid;
        public int predst;
        public Guid prev_persguid = Guid.Empty;

        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //}
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            //this.pr_pod_z_smo.ItemsSource = null;
            //this.pr_pod_z_polis.ItemsSource = null;
            //this.form_polis.ItemsSource = null;
            //status_p2.ItemsSource = null;
            //status_p2.ItemsSource = null;
            //list1.Add("1 Первичный выбор СМО");
            //list1.Add("2 Замена СМО в соответствии с правом замены");
            //list1.Add("3 Замена СМО в связи со сменой места жительства");
            //list1.Add("4 Замена СМО в связи с прекращением действия договора");
            //this.pr_pod_z_smo.ItemsSource = list1;
            ////pr_pod_z_smo.SelectedIndex = 0;

            //list2.Add("1 Изменение реквизитов");
            //list2.Add("2 Установление ошибочности сведений");
            //list2.Add("3 Ветхость и непригодность полиса");
            //list2.Add("4 Утрата ранее выданного полиса");
            //list2.Add("5 Окончание срока действия полиса");
            //this.pr_pod_z_polis.ItemsSource = list2;

            //list3.Add("0 Не требует изготовления полиса");
            //list3.Add("1 Бумажный бланк");
            //list3.Add("2 Пластиковая карта");
            //list3.Add("3 В составе УЭК");
            //list3.Add("4 Отказ от полиса");
            //this.form_polis.ItemsSource = list3;

            //list0.Add("1 Родитель");
            //list0.Add("2 Опекун");
            //list0.Add("3 Представитель");
            //status_p2.ItemsSource = list0;


            ////form_polis.SelectedIndex = 1;

            //list4.Add(new Dost { ID = "1", NameWithID = "1 Отсутствует отчество" });
            //list4.Add(new Dost { ID = "2", NameWithID = "2 Отсутствует фамилия" });
            //list4.Add(new Dost { ID = "3", NameWithID = "3 Отсутствует имя" });
            //list4.Add(new Dost { ID = "4", NameWithID = "4 Известен только месяц и год даты рождения" });
            //list4.Add(new Dost { ID = "5", NameWithID = "5 Известен только год даты рождения" });
            //list4.Add(new Dost { ID = "6", NameWithID = "6 Дата рождения не соответствует календарю" });

            //this.dost1.ItemsSource = list4;


            //layout_InUse();
            pers_grid_2.View.FocusedRowHandle = -1;


            //if (SPR.Premmissions == "USER")
            //{
            //    upload.IsEnabled = false;
            //    download.IsEnabled = false;
            //}
            //else
            //{
            //    upload.IsEnabled = true;
            //    download.IsEnabled = true;
            //}

            //LoadingDecorator1.IsSplashScreenShown = true;
            Cursor = Cursors.Wait;
            //Thread t0 = new Thread(delegate ()
            //{
            //    Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
            //    new Action(delegate ()
            //    {
            //       //Vars.DateVisit = Convert.ToDateTime(d_obr.EditValue);
            //       var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            //       var peopleList =
            //           MyReader.MySelect<People>(SPR.MyReader.load_pers_grid2 + strf_adm, connectionString);
            //       pers_grid_2.ItemsSource = peopleList;
            //       pers_grid_2.View.FocusedRowHandle = -1;
            //    }));
            //});
            //t0.Start();
            //list1.Add("1 Первичный выбор СМО");
            //list1.Add("2 Замена СМО в соответствии с правом замены");
            //list1.Add("3 Замена СМО в связи со сменой места жительства");
            //list1.Add("4 Замена СМО в связи с прекращением действия договора");
            //this.pr_pod_z_smo.ItemsSource = list1;
            ////pr_pod_z_smo.SelectedIndex = 0;

            //list2.Add("1 Изменение реквизитов");
            //list2.Add("2 Установление ошибочности сведений");
            //list2.Add("3 Ветхость и непригодность полиса");
            //list2.Add("4 Утрата ранее выданного полиса");
            //list2.Add("5 Окончание срока действия полиса");
            //this.pr_pod_z_polis.ItemsSource = list2;

            //list3.Add("0 Не требует изготовления полиса");
            //list3.Add("1 Бумажный бланк");
            //list3.Add("2 Пластиковая карта");
            //list3.Add("3 В составе УЭК");
            //list3.Add("4 Отказ от полиса");
            //this.form_polis.ItemsSource = list3;

            if (Vars.Btn != "1")
            {
                //Thread t2 = new Thread(delegate ()
                //{
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                new Action(delegate ()
                {


                    var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                    SqlConnection con = new SqlConnection(connectionString1);
                    SqlCommand comm = new SqlCommand("select photo as prf from pol_personsb where event_guid=(select event_guid from pol_persons where id=@id) and type=2", con);
                    comm.Parameters.AddWithValue("@id", Vars.IdP);
                    con.Open();
                    string prf1 = (string)comm.ExecuteScalar();
                    con.Close();
                    SqlCommand comm1 = new SqlCommand("select photo as prp from pol_personsb where event_guid=(select event_guid from pol_persons where id=@id) and type=3", con);
                    comm1.Parameters.AddWithValue("@id", Vars.IdP);
                    con.Open();
                    string prp = (string)comm1.ExecuteScalar();
                    con.Close();

                    if (prf1 == null || prf1 == "")
                    {
                        zl_photo.EditValue = "";
                    }
                    else
                    {
                        zl_photo.EditValue = Convert.FromBase64String(prf1);
                    }
                    if (prp == null || prp == "")
                    {
                        zl_podp.EditValue = "";
                    }
                    else
                    {
                        zl_podp.EditValue = Convert.FromBase64String(prp);
                    }

                    SqlCommand comm3 = new SqlCommand(@"select t0.*,t1.FAM as fam1,t1.im as im1, t1.ot as ot1, t0.DATEVIDACHI as datevidachi, t0.PRIZNAKVIDACHI as priznak_vidachi,
t1.dr as dr1,t1.phone as phone1,t2.PRELATION,t1.idguid as idguid1,t1.w as w1,t0.mo as MO,t0.dstart as DSTART,
t3.idguid as prev_persguid,t3.fam as fam2,t3.im as im2,t3.ot as ot2,t3.w as w2,t3.dr as dr2,t3.mr as mr2,t2.tip_op as tip_op,
t2.method as sppz,t2.rsmo as rsmo,t2.rpolis as rpolis,t2.fpolis as fpolis,t2.petition as petition,t2.dvizit 

from
(select * from pol_persons where id = @id)T0
LEFT join
(select * from pol_persons )T1
on t0.RPERSON_GUID = t1.IDGUID
LEFT join
(select * from pol_events )T2
on t0.EVENT_GUID=t2.IDGUID
LEFT join
(select * from pol_persons_old )T3
on t0.idguid = t3.person_guid", con);
                    comm3.Parameters.AddWithValue("@id", Vars.IdP);
                    con.Open();
                    SqlDataReader reader1 = comm3.ExecuteReader();

                    while (reader1.Read()) // построчно считываем данные
                    {
                        object fam_ = reader1["FAM"];
                        object im_ = reader1["IM"];

                        object ot_ = reader1["OT"];
                        object dr_ = reader1["DR"];
                        object datevidachi_ = reader1["datevidachi"];
                        object priznak_vidachi_ = reader1["PRIZNAKVIDACHI"];
                        object w = reader1["W"];
                        object mr_ = reader1["MR"];
                        object birthoksm = reader1["BIRTH_OKSM"];
                        object coksm = reader1["C_OKSM"];
                        object ss = reader1["SS"];
                        object enp_ = reader1["ENP"];
                        object dost = reader1["DOST"];
                        object phone_ = reader1["PHONE"];
                        object email_ = reader1["EMAIL"];
                        object rpersonguid = reader1["RPERSON_GUID"];
                        object kateg = reader1["kateg"];
                        object dost_ = reader1["DOST"];
                        object ddeath_ = reader1["DDEATH"];
                        object fam_1 = reader1["fam1"];
                        object im_1 = reader1["im1"];
                        object ot_1 = reader1["ot1"];
                        object dr_1 = reader1["dr1"];
                        object phone_1 = reader1["phone1"];
                        object prelation = reader1["PRELATION"];
                        object idguid_ = reader1["idguid1"];
                        object w1_ = reader1["w1"];
                        object _mo = reader1["MO"];
                        object _dstart = reader1["DSTART"];
                        object fam2_ = reader1["fam2"];
                        object im2_ = reader1["im2"];
                        object ot2_ = reader1["ot2"];
                        object dr2_ = reader1["dr2"];
                        object w2 = reader1["w2"];
                        object mr2_ = reader1["mr2"];
                        object tip_op_ = reader1["tip_op"];
                        object sppz_ = reader1["sppz"];
                        object rsmo_ = reader1["rsmo"];
                        object rpolis_ = reader1["rpolis"];
                        object fpolis_ = reader1["fpolis"];
                        object petition_ = reader1["petition"];
                        object dvisit_ = reader1["dvizit"];
                        object srok_doverenosti_ = reader1["SROKDOVERENOSTI"];
                        object prev_persguid_ = reader1["prev_persguid"];

                        prev_persguid = prev_persguid_.ToString() == "" ? Guid.Empty : (Guid)prev_persguid_;
                        rper = idguid_.ToString() == "" ? Guid.Empty : (Guid)idguid_;
                        if (ddeath_.ToString() == "")
                        {

                        }
                        else
                        {
                            ddeath.DateTime = Convert.ToDateTime(ddeath_);
                        }


                        string dost_1 = dost_.ToString();
                        fam.Text = fam_.ToString();
                        im.Text = im_.ToString();
                        ot.Text = ot_.ToString();
                        dr.DateTime = Convert.ToDateTime(dr_);
                        pol.SelectedIndex = Convert.ToInt32(w);
                        mr2.Text = mr_.ToString();
                        str_r.EditValue = birthoksm.ToString();
                        gr.EditValue = coksm.ToString();
                        enp.Text = enp_.ToString();

                        srok_doverenosti.EditValue = srok_doverenosti_;

                        if (datevidachi_.ToString() == "")
                        {

                        }
                        else
                        {
                            //date_vidachi.DateTime = Convert.ToDateTime(datevidachi_);
                            date_vidachi.EditValue = datevidachi_;
                        }


                        if (priznak_vidachi_.ToString() == "True")
                        {
                            priznak_vidachi.IsChecked = true;
                        }
                        else
                        {
                            priznak_vidachi.IsChecked = false;
                        }

                        snils.Text = ss.ToString();
                        phone.Text = phone_.ToString();
                        email.Text = email_.ToString();
                        kat_zl.EditValue = kateg;

                        dost1.EditValue = dost_1.Split(';');
                        //dost1.SelectedItem= dost_1.Split(',').ToList();
                        cel_vizita.EditValue = tip_op_.ToString();

                        if (sppz_.ToString() == "")
                        {

                        }
                        else
                        {
                            sp_pod_z.EditValue = Convert.ToInt32(sppz_);
                        }

                        if (dvisit_.ToString() == "")
                        {

                        }
                        else if (dvisit_.ToString() == "01.01.2018 0:00:00")
                        {
                            d_obr.EditValue = DateTime.Now;
                        }
                        else
                        {
                            d_obr.EditValue = Convert.ToDateTime(dvisit_);
                        }

                        if (petition_.ToString() == "")
                        {

                        }
                        else
                        {
                            petition.EditValue = Convert.ToBoolean(petition_);
                        }

                        pr_pod_z_polis.SelectedIndex = Convert.ToInt32(rpolis_.ToString() == "" ? 0 : rpolis_) - 1;
                        form_polis.SelectedIndex = Convert.ToInt32(fpolis_.ToString() == "" ? -1 : fpolis_);
                        pr_pod_z_smo.SelectedIndex = Convert.ToInt32(rsmo_.ToString() == "" ? 0 : rsmo_) - 1;

                        if (_mo.ToString() == "")
                        {

                        }
                        else
                        {
                            mo_cmb.EditValue = _mo.ToString();
                        }

                        if (_dstart.ToString() == "")
                        {

                        }
                        else
                        {
                            date_mo.EditValue = Convert.ToDateTime(_dstart);
                        }

                        prev_fam.Text = fam2_.ToString();
                        prev_im.Text = im2_.ToString();
                        prev_ot.Text = ot2_.ToString();
                        if (w2.ToString() == "")
                        {

                        }
                        else
                        {
                            prev_pol.SelectedIndex = Convert.ToInt32(w2);
                        }
                        if (dr2_.ToString() == "")
                        {

                        }
                        else
                        {
                            prev_dr.DateTime = Convert.ToDateTime(dr2_);
                        }

                        prev_mr.Text = mr2_.ToString();
                        if (rpersonguid.ToString() == "00000000-0000-0000-0000-000000000000" || rpersonguid.ToString() == "")
                        {

                        }
                        else
                        {
                            fam1.Text = fam_1.ToString();
                            im1.Text = im_1.ToString();
                            ot1.Text = ot_1.ToString();
                            pol_pr.SelectedIndex = Convert.ToInt32(w1_ == DBNull.Value ? -1 : w1_);

                            idguid = idguid_.ToString();
                            if (dr_1.ToString() == "")
                            {
                                dr1.EditValue = "";
                            }
                            else
                            {
                                dr1.DateTime = Convert.ToDateTime(dr_1);
                            }

                            phone_p1.Text = phone_1.ToString();
                            if (prelation.ToString() == "")
                            {
                                status_p2.SelectedIndex = -1;
                            }
                            else
                            {
                                status_p2.SelectedIndex = Convert.ToInt32(prelation);
                            }

                        }

                    }
                    reader1.Close();
                    con.Close();
                }));
                //});
                //  t2.Start();
                //  Thread t3 = new Thread(delegate ()
                //  {
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                new Action(delegate ()
                {
                    var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                    SqlConnection con = new SqlConnection(connectionString1);
                    SqlCommand comm2 = new SqlCommand(@"select * from pol_documents where isnull(event_guid,PERSON_GUID)=(select isnull(event_guid,IDGUID) from pol_persons where id=@id) and main=1 and active=1", con);
                    comm2.Parameters.AddWithValue("@id", Vars.IdP);
                    con.Open();
                    SqlDataReader reader = comm2.ExecuteReader();

                    while (reader.Read()) // построчно считываем данные
                    {
                        object doctype = reader["DOCTYPE"];
                        object docser = reader["DOCSER"];
                        object docnum = reader["DOCNUM"];
                        object docdate = reader["DOCDATE"];
                        object name_vp = reader["NAME_VP"];
                        object name_vp_code = reader["NAME_VP_CODE"];
                        object docmr = reader["DOCMR"];
                        object str_vid_ = reader["OKSM"];

                        doc_type.EditValue = doctype;
                        doc_ser.Text = docser.ToString();
                        doc_num.Text = docnum.ToString();
                        date_vid.DateTime = Convert.ToDateTime(docdate);
                        kem_vid.Text = name_vp.ToString();
                        kod_podr.Text = name_vp_code.ToString();
                        //mr2.Text = docmr.ToString();
                        str_vid.EditValue = str_vid_;



                    }

                    reader.Close();

                    con.Close();

                    SqlCommand comm16 = new SqlCommand(@"select * from pol_documents_old where 
                event_guid=(select event_guid from pol_persons where id=@id)", con);
                    comm16.Parameters.AddWithValue("@id", Vars.IdP);
                    con.Open();
                    SqlDataReader reader16 = comm16.ExecuteReader();
                    if (reader16.HasRows == false)
                    {
                        doc_type1.EditValue = 14;
                    }

                    while (reader16.Read()) // построчно считываем данные
                    {
                        object doctype_1 = reader16["DOCTYPE"];
                        object docser_1 = reader16["DOCSER"];
                        object docnum_1 = reader16["DOCNUM"];
                        object docdate_1 = reader16["DOCDATE"];
                        object name_vp_1 = reader16["NAME_VP"];
                        object name_vp_code_1 = reader16["NAME_VP_CODE"];
                        object docmr_1 = reader16["DOCMR"];
                        object str_vid_1 = reader16["OKSM"];
                        object idguid_ = reader16["IDGUID"];


                        doc_type1.EditValue = doctype_1;
                        old_doc = 1;
                        old_doc_guid = idguid_.ToString();


                        doc_ser1.Text = docser_1.ToString();
                        doc_num1.Text = docnum_1.ToString();

                        date_vid2.DateTime = Convert.ToDateTime(docdate_1);


                        kem_vid1.Text = name_vp_1.ToString();
                        kod_podr1.Text = name_vp_code_1.ToString();
                        prev_mr.Text = docmr_1.ToString();
                        str_vid1.EditValue = str_vid_1;



                    }

                    reader16.Close();

                    con.Close();


                    SqlCommand comm4 = new SqlCommand(@"select * from pol_documents 
where event_guid=(select event_guid from pol_persons where id=@id) and main=0 and active=1", con);
                    comm4.Parameters.AddWithValue("@id", Vars.IdP);
                    con.Open();
                    SqlDataReader reader2 = comm4.ExecuteReader();

                    while (reader2.Read()) // построчно считываем данные
                    {
                        object doctype = reader2["DOCTYPE"];
                        object docser = reader2["DOCSER"];
                        object docnum = reader2["DOCNUM"];
                        object docdate = reader2["DOCDATE"];
                        object docexp = reader2["DOCEXP"];
                        object name_vp = reader2["NAME_VP"];
                        object idguid_ = reader2["IDGUID"];


                        ddtype.EditValue = doctype;
                        ddser.Text = docser.ToString();
                        ddnum.Text = docnum.ToString();
                        dddate.DateTime = Convert.ToDateTime(docdate);
                        if (docexp == null)
                        {
                            docexp1.EditValue = null;
                        }
                        else
                        {
                            docexp1.EditValue = Convert.ToDateTime(docexp);
                        }

                        ddkemv.Text = name_vp.ToString();

                        dop_doc = 1;
                        dop_doc_guid = idguid_.ToString();

                    }

                    reader2.Close();

                    con.Close();
                    SqlCommand comm10 = new SqlCommand(@"select * from pol_documents where PERSON_GUID=(select RPERSON_GUID 
from POL_EVENTS where IDGUID=(select event_guid from pol_persons where id=@id) and main=1)", con);
                    comm10.Parameters.AddWithValue("@id", Vars.IdP);
                    con.Open();
                    SqlDataReader reader10 = comm10.ExecuteReader();

                    while (reader10.Read()) // построчно считываем данные 
                    {
                        object doctype = reader10["DOCTYPE"];
                        object docser = reader10["DOCSER"];
                        object docnum = reader10["DOCNUM"];
                        object docdate = reader10["DOCDATE"];
                        object name_vp = reader10["NAME_VP"];



                        doctype1.EditValue = doctype;
                        docser1.Text = docser.ToString();
                        docnum1.Text = docnum.ToString();
                        docdate1.DateTime = Convert.ToDateTime(docdate);




                    }

                    reader10.Close();

                    con.Close();
                }));
                //});
                // t3.Start();

                // Thread t4 = new Thread(delegate ()
                // {
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                new Action(delegate ()
                {
                    var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                    SqlConnection con = new SqlConnection(connectionString1);

                    SqlCommand comm5 = new SqlCommand(@"select *from pol_addresses pa left join POL_RELATION_ADDR_PERS pr on pa.IDGUID=pr.ADDR_GUID 
where pr.event_guid=(select event_guid from pol_persons where id=@id) and pr.addres_g=1 ", con);
                    comm5.Parameters.AddWithValue("@id", Vars.IdP);
                    con.Open();
                    SqlDataReader reader3 = comm5.ExecuteReader();

                    while (reader3.Read()) // построчно считываем данные
                    {
                        object obl = reader3["FIAS_L1"];
                        object rn = reader3["FIAS_L3"];
                        object town = reader3["FIAS_L4"];
                        object np = reader3["FIAS_L6"];
                        object street = reader3["FIAS_L7"];
                        object dom_ = reader3["HOUSE_GUID"];
                        object korp_ = reader3["KORP"];
                        object str_ = reader3["EXT"];
                        object kv_ = reader3["KV"];
                        object d_reg = reader3["DREG"];
                        object bomg = reader3["BOMG"];
                        object addr_g_ = reader3["ADDRES_G"];
                        object addr_p_ = reader3["ADDRES_P"];
                        // object str = reader3["addrstr"];



                        //fias.addrstr.Text = str.ToString();
                        L1 = obl.ToString();
                        L3 = rn.ToString();
                        L4 = town.ToString();
                        L6 = np.ToString();
                        L7 = street.ToString();
                        fias.reg.EditValue = obl;
                        if (rn.ToString() == "00000000-0000-0000-0000-000000000000")
                        {
                            fias.reg_rn.EditValue = null;
                        }
                        else
                        {
                            fias.reg_rn.EditValue = rn;
                        }
                        if (town.ToString() == "00000000-0000-0000-0000-000000000000")
                        {
                            fias.reg_town.EditValue = null;
                        }
                        else
                        {
                            fias.reg_town.EditValue = town;
                        }
                        //fias.reg_rn.EditValue = rn;
                        //fias.reg_town.EditValue = town;
                        if (np.ToString() == "00000000-0000-0000-0000-000000000000")
                        {
                            fias.reg_np.EditValue = null;

                        }
                        else
                        {
                            fias.reg_np.EditValue = np;
                        }
                        if (street.ToString() == "00000000-0000-0000-0000-000000000000")
                        {
                            fias.reg_ul.EditValue = null;

                        }
                        else
                        {
                            fias.reg_ul.EditValue = street;
                        }

                        fias.reg_dom.EditValue = dom_;
                        if (fias.reg_dom.EditValue != null)
                        {
                            dstrkor = fias.reg_dom.Text.Replace(' ', ',').Split(',');
                            domsplit = dstrkor[0].Replace("д.", "");
                        }
                        else
                        {
                            domsplit = "";
                        }

                        fias.reg_korp.Text = korp_.ToString();
                        fias.reg_str.Text = str_.ToString();
                        fias.reg_kv.Text = kv_.ToString();
                        try
                        {
                            fias.bomj.IsChecked = Convert.ToBoolean(bomg);
                        }
                        catch
                        {
                            fias.bomj.IsChecked = Convert.ToBoolean(0);
                            string mx = "Не удалось получить значение из базы!";
                            string t = "Внимание!";
                            int b = 1;
                            Message me = new Message(mx, t, b);
                            me.ShowDialog();
                        }
                        if (d_reg.ToString() == "")
                        {

                        }
                        else
                        {
                            fias.reg_dr.EditValue = Convert.ToDateTime(d_reg);
                        }
                        if (Convert.ToBoolean(addr_g_) == true && Convert.ToBoolean(addr_p_) == true)
                        {
                            fias1.sovp_addr.IsChecked = true;
                        }
                        else
                        {
                            fias1.sovp_addr.IsChecked = false;
                        }



                    }
                    reader3.Close();
                    con.Close();
                    if (fias.bomj.IsChecked == false)
                    {
                        SqlCommand comm6 = new SqlCommand(@"select *from pol_addresses pa left join POL_RELATION_ADDR_PERS pr on pa.IDGUID=pr.ADDR_GUID 
where pr.event_guid=(select event_guid from pol_persons where id=@id) and pr.addres_p=1 ", con);
                        comm6.Parameters.AddWithValue("@id", Vars.IdP);
                        con.Open();
                        SqlDataReader reader4 = comm6.ExecuteReader();

                        while (reader4.Read()) // построчно считываем данные
                        {
                            object obl = reader4["FIAS_L1"];
                            object rn = reader4["FIAS_L3"];
                            object town = reader4["FIAS_L4"];
                            object np = reader4["FIAS_L6"];
                            object street = reader4["FIAS_L7"];
                            object dom_ = reader4["HOUSE_GUID"];
                            object korp_ = reader4["KORP"];
                            object str_ = reader4["EXT"];
                            object kv_ = reader4["KV"];
                            object d_reg = reader4["DREG"];
                            object bomg = reader4["BOMG"];
                            //object str = reader4["addrstr"];

                            //fias1.addrstr1.Text = str.ToString();
                            L1_1 = obl.ToString();
                            L3_1 = rn.ToString();
                            L4_1 = town.ToString();
                            L6_1 = np.ToString();
                            L7_1 = street.ToString();
                            fias1.reg1.EditValue = obl;
                            if (rn.ToString() == "00000000-0000-0000-0000-000000000000")
                            {
                                fias1.reg_rn1.EditValue = null;
                            }
                            else
                            {
                                fias1.reg_rn1.EditValue = rn;
                            }
                            if (town.ToString() == "00000000-0000-0000-0000-000000000000")
                            {
                                fias1.reg_town1.EditValue = null;
                            }
                            else
                            {
                                fias1.reg_town1.EditValue = town;
                            }
                            //fias.reg_rn.EditValue = rn;
                            //fias.reg_town.EditValue = town;
                            if (np.ToString() == "00000000-0000-0000-0000-000000000000")
                            {
                                fias1.reg_np1.EditValue = null;

                            }
                            else
                            {
                                fias1.reg_np1.EditValue = np;
                            }
                            if (street.ToString() == "00000000-0000-0000-0000-000000000000")
                            {
                                fias1.reg_ul1.EditValue = null;

                            }
                            else
                            {
                                fias1.reg_ul1.EditValue = street;
                            }
                            fias1.reg_dom1.EditValue = dom_;

                            if (fias1.reg_dom1.EditValue != null)
                            {
                                dstrkor1 = fias1.reg_dom1.Text.Replace(' ', ',').Split(',');
                                domsplit1 = dstrkor1[0].Replace("д.", "");
                            }
                            else
                            {
                                domsplit1 = "";
                            }
                            fias1.reg_korp1.Text = korp_.ToString();
                            fias1.reg_str1.Text = str_.ToString();
                            fias1.reg_kv1.Text = kv_.ToString();
                            if (d_reg.ToString() == "")
                            {

                            }
                            else
                            {
                                fias1.reg_dr1.EditValue = Convert.ToDateTime(d_reg);
                            }



                        }
                        reader4.Close();
                        con.Close();

                        SqlCommand comm26 = new SqlCommand("select * from pol_addresses pa " +
                          "left join POL_RELATION_ADDR_PERS pr on pa.IDGUID=pr.ADDR_GUID  " +
                          "where pr.event_guid=(select event_guid from pol_persons where id=@id) and pr.addres_p=1", con);
                        comm26.Parameters.AddWithValue("@id", Vars.IdP);
                        con.Open();
                        SqlDataReader reader14 = comm26.ExecuteReader();

                        while (reader14.Read()) // построчно считываем данные
                        {
                            object obl = reader14["FIAS_L1"];
                            object rn = reader14["FIAS_L3"];
                            object town = reader14["FIAS_L4"];
                            object np = reader14["FIAS_L6"];
                            object street = reader14["FIAS_L7"];
                            object dom_ = reader14["HOUSE_GUID"];
                            object korp_ = reader14["KORP"];
                            object str_ = reader14["EXT"];
                            object kv_ = reader14["KV"];
                            object d_reg = reader14["DREG"];
                            object bomg = reader14["BOMG"];


                            L1_1 = obl.ToString();
                            L3_1 = rn.ToString();
                            L4_1 = town.ToString();
                            L6_1 = np.ToString();
                            L7_1 = street.ToString();
                            fias1.reg1.EditValue = obl;
                            if (rn.ToString() == "00000000-0000-0000-0000-000000000000")
                            {
                                fias1.reg_rn1.EditValue = null;
                            }
                            else
                            {
                                fias1.reg_rn1.EditValue = rn;
                            }
                            if (town.ToString() == "00000000-0000-0000-0000-000000000000")
                            {
                                fias1.reg_town1.EditValue = null;
                            }
                            else
                            {
                                fias1.reg_town1.EditValue = town;
                            }
                            //fias.reg_rn.EditValue = rn;
                            //fias.reg_town.EditValue = town;
                            if (np.ToString() == "00000000-0000-0000-0000-000000000000")
                            {
                                fias1.reg_np1.EditValue = null;

                            }
                            else
                            {
                                fias1.reg_np1.EditValue = np;
                            }
                            if (street.ToString() == "00000000-0000-0000-0000-000000000000")
                            {
                                fias1.reg_ul1.EditValue = null;

                            }
                            else
                            {
                                fias1.reg_ul1.EditValue = street;
                            }
                            fias1.reg_dom1.EditValue = dom_;

                            if (fias1.reg_dom1.EditValue != null)
                            {
                                dstrkor1 = fias1.reg_dom1.Text.Replace(' ', ',').Split(',');
                                domsplit1 = dstrkor1[0].Replace("д.", "");
                            }
                            else
                            {
                                domsplit1 = "";
                            }
                            fias1.reg_korp1.Text = korp_.ToString();
                            fias1.reg_str1.Text = str_.ToString();
                            fias1.reg_kv1.Text = kv_.ToString();
                            if (d_reg.ToString() == "")
                            {

                            }
                            else
                            {
                                fias1.reg_dr1.EditValue = Convert.ToDateTime(d_reg);
                            }



                        }
                        reader14.Close();
                        con.Close();
                        if (Vars.Btn == "2")
                        {
                            this.Title = "Исправление ошибочных данных застрахованного лица";
                        }
                        else
                        {
                            this.Title = "Создание нового события существующему ЗЛ";
                        }
                    }
                    if (Vars.CelVisit == "П010" || Vars.CelVisit == "П034" || Vars.CelVisit == "П035" || Vars.CelVisit == "П036" || Vars.CelVisit == "П061" || Vars.CelVisit == "П062" || Vars.CelVisit == "П063")
                    {
                        SqlCommand comm7;
                        type_policy.EditValue = 2;
                        if (Vars.Btn == "2")
                        {
                            comm7 = new SqlCommand("select * from pol_polises where event_guid=(select event_guid from POL_persons where id=@id)", con);
                            comm7.Parameters.AddWithValue("@id", Vars.IdP);
                        }
                        else
                        {
                            comm7 = new SqlCommand("select * from pol_polises where id=(select min(id) from POL_POLISES where vpolis=2 and blank=1 and DBEG is null)", con);
                            comm7.Parameters.AddWithValue("@id", Vars.IdP);

                        }

                        con.Open();
                        SqlDataReader reader5 = comm7.ExecuteReader();

                        while (reader5.Read()) // построчно считываем данные
                        {
                            object vpolis = reader5["VPOLIS"];
                            object spolis = reader5["SPOLIS"];
                            object npolis = reader5["NPOLIS"];
                            object dbeg = reader5["DBEG"];
                            object dend = reader5["DEND"];
                            object dstop = reader5["DSTOP"];
                            object dout_ = reader5["DOUT"];
                            object drecieved = reader5["DRECEIVED"];
                            object blank = reader5["BLANK"];


                            try
                            {
                                ser_blank.Text = spolis.ToString();
                                num_blank.Text = npolis.ToString();
                                sblank = spolis.ToString();
                                spolis_ = spolis.ToString();
                            }
                            catch
                            {
                                MessageBox.Show("Ошибка чтения серии или номера бланка! Попробуйте открыть ЗЛ снова!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }

                            if (Vars.Btn == "2")
                            {
                                type_policy.EditValue = Convert.ToInt32(vpolis);
                                date_vid1.EditValue = Convert.ToDateTime(dbeg);
                                date_poluch.EditValue = Convert.ToDateTime(dbeg);
                                if (dstop == DBNull.Value)
                                {
                                    fakt_prekr.EditValue = null;
                                }
                                else
                                {
                                    fakt_prekr.EditValue = Convert.ToDateTime(dstop);
                                }

                            }
                            else
                            {
                                type_policy.EditValue = 2;
                                date_vid1.EditValue = DateTime.Today;
                                date_poluch.EditValue = date_vid1.DateTime;
                            }



                            if (Convert.ToBoolean(blank) == true)
                            {
                                pustoy.IsChecked = true;
                            }
                            else
                            {
                                pustoy.IsChecked = false;
                            }



                        }
                        reader5.Close();
                        con.Close();

                    }
                    else
                    {
                        type_policy.EditValue = 3;
                        date_vid1.EditValue = null;
                        date_poluch.EditValue = null;
                        SqlCommand comm7 = new SqlCommand("select * from pol_polises  where event_guid=(select event_guid from pol_persons where id=@id)", con);
                        comm7.Parameters.AddWithValue("@id", Vars.IdP);
                        con.Open();
                        SqlDataReader reader5 = comm7.ExecuteReader();

                        while (reader5.Read()) // построчно считываем данные
                        {
                            object vpolis = reader5["VPOLIS"];
                            object spolis = reader5["SPOLIS"];
                            object npolis = reader5["NPOLIS"];
                            object dbeg = reader5["DBEG"];
                            object dend = reader5["DEND"];
                            object dstop = reader5["DSTOP"];
                            object dout_ = reader5["DOUT"];
                            object blank = reader5["BLANK"];
                            object dreceived = reader5["DRECEIVED"];




                            type_policy.EditValue = Convert.ToInt32(vpolis);
                            ser_blank.Text = spolis.ToString();
                            num_blank.Text = npolis.ToString();

                            date_vid1.EditValue = Convert.ToDateTime(dbeg);
                            if (dend == DBNull.Value)
                            {
                                date_end.EditValue = null;
                            }
                            else
                            {
                                date_end.EditValue = Convert.ToDateTime(dend);
                            }

                            if (dstop == DBNull.Value)
                            {
                                fakt_prekr.EditValue = null;
                            }
                            else
                            {
                                fakt_prekr.EditValue = Convert.ToDateTime(dstop);
                            }

                            if (dreceived == DBNull.Value)
                            {
                                date_vid.EditValue = null;
                            }
                            else
                            {
                                date_poluch.EditValue = Convert.ToDateTime(dreceived);
                            }
                            if (dout_ == DBNull.Value)
                            {
                                dout.EditValue = null;
                            }
                            else
                            {
                                dout.EditValue = Convert.ToDateTime(dout_);
                            }


                            if (Convert.ToBoolean(blank) == true)
                            {
                                pustoy.IsChecked = true;
                            }
                            else
                            {
                                pustoy.IsChecked = false;
                            }



                        }
                        reader5.Close();
                        con.Close();

                    }
                    SqlCommand comm8 = new SqlCommand(@"select *from pol_addresses old_g where 
event_guid=(select event_guid from pol_persons where id=@id)", con);
                    comm8.Parameters.AddWithValue("@id", Vars.IdP);
                    con.Open();
                    SqlDataReader reader8 = comm8.ExecuteReader();

                    while (reader8.Read()) // построчно считываем данные
                    {
                        object adres_ = reader8["Old_G"];

                        fias.adres.Text = adres_.ToString();
                    }
                    reader8.Close();
                    con.Close();
                    //Binding bind = new Binding();
                    //bind.Source = towntxt;
                    //bind.Path = new PropertyPath("Text");
                    //bind.Mode = BindingMode.TwoWay;
                    //fias.reg_town.SetBinding(ComboBoxEdit.DataContextProperty, bind);
                    //if (kat_zl.EditValue != "")
                    //{
                    //    kat_zl.SelectedIndex = Convert.ToInt32(kat_zl.EditValue) - 1;
                    //}
                    //else
                    //{
                    //    kat_zl.SelectedIndex = -1;
                    //}
                    Vars.NewCelViz = 0;
                }));


                //});
                // t4.Start();
            }
            else
            {
                InsMethods.PersData_Default(this);
            }

            //           Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
            //           new Action(delegate ()
            //           {

            //                   //Insurance.DocExchangeDataSet docExchangeDataSet = ((Insurance.DocExchangeDataSet)(this.FindResource("docExchangeDataSet")));
            //                   //    // Загрузить данные в таблицу POL_PERSONS. Можно изменить этот код как требуется.
            //                   //    Insurance.DocExchangeDataSetTableAdapters.POL_PERSONSTableAdapter docExchangeDataSetPOL_PERSONSTableAdapter = new Insurance.DocExchangeDataSetTableAdapters.POL_PERSONSTableAdapter();
            //                   //    docExchangeDataSetPOL_PERSONSTableAdapter.Fill(docExchangeDataSet.POL_PERSONS);
            //                   //    System.Windows.Data.CollectionViewSource pOL_PERSONSViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("pOL_PERSONSViewSource")));
            //                   //    pOL_PERSONSViewSource.View.MoveCurrentToFirst();
            //                   //    pers_grid_2.Columns[1].Visible = false;
            //                   //    pers_grid_2.Columns[2].Visible = false;
            //                   //    pers_grid_2.Columns[3].Visible = false;
            //                   //    pers_grid_2.Columns[4].Visible = false;
            //                   //    pers_grid_2.View.FocusedRowHandle = -1;


            //               list0.Add("1 Родитель");
            //               list0.Add("2 Опекун");
            //               list0.Add("3 Представитель");
            //               status_p2.ItemsSource = list0;


            //               //form_polis.SelectedIndex = 1;

            //               list4.Add(new Dost { ID = "1", NameWithID = "1 Отсутствует отчество" });
            //               list4.Add(new Dost { ID = "2", NameWithID = "2 Отсутствует фамилия" });
            //               list4.Add(new Dost { ID = "3", NameWithID = "3 Отсутствует имя" });
            //               list4.Add(new Dost { ID = "4", NameWithID = "4 Известен только месяц и год даты рождения" });
            //               list4.Add(new Dost { ID = "5", NameWithID = "5 Известен только год даты рождения" });
            //               list4.Add(new Dost { ID = "6", NameWithID = "6 Дата рождения не соответствует календарю" });

            //               this.dost1.ItemsSource = list4;


            //               pers_grid_2.View.FocusedRowHandle = -1;
            //               pers_grid_2.SelectedItem = -1;




            //               var molist =
            //                       MyReader.MySelect<F003>(
            //                           $@"
            //           SELECT mcod,namewithid from f003
            //order by mcod", Properties.Settings.Default.DocExchangeConnectionString);
            //               mo_cmb.DataContext = molist;

            //               pol.DataContext = MyReader.MySelect<V005>(@"select IDPOL,NameWithID from SPR_79_V005", Properties.Settings.Default.DocExchangeConnectionString);
            //               pol_pr.DataContext = MyReader.MySelect<V005>(@"select IDPOL,NameWithID from SPR_79_V005", Properties.Settings.Default.DocExchangeConnectionString);
            //               doc_type.DataContext = MyReader.MySelect<F011>(@"select ID,NameWithID from SPR_79_F011", Properties.Settings.Default.DocExchangeConnectionString);
            //               doctype1.DataContext = MyReader.MySelect<F011>(@"select ID,NameWithID from SPR_79_F011", Properties.Settings.Default.DocExchangeConnectionString);
            //               doc_type1.DataContext = MyReader.MySelect<F011>(@"select ID,NameWithID from SPR_79_F011", Properties.Settings.Default.DocExchangeConnectionString);
            //               ddtype.DataContext = MyReader.MySelect<F011>(@"select ID,NameWithID from SPR_79_F011", Properties.Settings.Default.DocExchangeConnectionString);
            //               str_vid.DataContext = MyReader.MySelect<OKSM>(@"select ID,CAPTION from SPR_79_OKSM", Properties.Settings.Default.DocExchangeConnectionString);
            //               str_vid1.DataContext = MyReader.MySelect<OKSM>(@"select ID,CAPTION from SPR_79_OKSM", Properties.Settings.Default.DocExchangeConnectionString);
            //               str_r.DataContext = MyReader.MySelect<OKSM>(@"select ID,CAPTION from SPR_79_OKSM", Properties.Settings.Default.DocExchangeConnectionString);
            //               gr.DataContext = MyReader.MySelect<OKSM>(@"select ID,CAPTION from SPR_79_OKSM", Properties.Settings.Default.DocExchangeConnectionString);
            //               kat_zl.DataContext = MyReader.MySelect<V013>(@"select IDKAT,NameWithID from V013", Properties.Settings.Default.DocExchangeConnectionString);
            //               type_policy.DataContext = MyReader.MySelect<F008>(@"select ID,NameWithID from SPR_79_F008", Properties.Settings.Default.DocExchangeConnectionString);
            //               prev_pol.DataContext = MyReader.MySelect<V005>(@"select IDPOL,NameWithID from SPR_79_V005", Properties.Settings.Default.DocExchangeConnectionString);
            //                   //kem_vid.DataContext = MyReader.MySelect<NAMEVP>(@"select distinct name_vp from pol_documents order by name_vp",connectionString);
            //                   // LoadingDecorator1.IsSplashScreenShown = false;
            //               fam.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
            //               im.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
            //               ot.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
            //               kem_vid.DataContext = MyReader.MySelect<NAME_VP>(@"select id,name from spr_namevp", Properties.Settings.Default.DocExchangeConnectionString);
            //               fam1.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
            //               im1.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
            //               ot1.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
            //               prev_fam.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
            //               prev_im.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
            //               prev_ot.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);

            //               Cursor = Cursors.Arrow;
            //           }));
            //Thread t1 = new Thread(delegate ()
            //{


            //restore_Layout();
            //MessageBox.Show(spolis_+" "+(Vars.IdP ?? "111").ToString());
        }


        public string sblank;
        public int rper_load = 0;
        public Guid rper;
        private void pers_grid_2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            //LoadingDecorator1.IsSplashScreenShown = true;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            if (tabs.SelectedIndex == 6)
            {


                SqlCommand comm11 = new SqlCommand("select * from pol_documents where event_guid=(select event_guid from pol_persons where id=@id) and main=1", con);
                comm11.Parameters.AddWithValue("@id", Convert.ToInt32(pers_grid_2.GetFocusedRowCellValue("ID")));
                con.Open();
                SqlDataReader reader11 = comm11.ExecuteReader();

                while (reader11.Read()) // построчно считываем данные
                {
                    object doctype = reader11["DOCTYPE"];
                    object docser = reader11["DOCSER"];
                    object docnum = reader11["DOCNUM"];
                    object docdate = reader11["DOCDATE"];




                    doctype1.EditValue = doctype;
                    docser1.Text = docser.ToString();
                    docnum1.Text = docnum.ToString();
                    docdate1.EditValue = Convert.ToDateTime(docdate);




                }

                reader11.Close();

                con.Close();


                fam1.Text = pers_grid_2.GetFocusedRowCellValue("FAM").ToString();
                im1.Text = pers_grid_2.GetFocusedRowCellValue("IM").ToString();
                ot1.Text = pers_grid_2.GetFocusedRowCellValue("OT").ToString();
                pol_pr.EditValue = Convert.ToInt32(pers_grid_2.GetFocusedRowCellValue("W"));
                dr1.DateTime = Convert.ToDateTime(pers_grid_2.GetFocusedRowCellValue("DR"));
                if (pers_grid_2.GetFocusedRowCellValue("PHONE") == null)
                {

                }
                else
                {
                    phone_p1.Text = pers_grid_2.GetFocusedRowCellValue("PHONE").ToString();
                }
                SqlCommand comm12 = new SqlCommand(@"select idguid from pol_persons where id=@id_p", con);
                comm12.Parameters.AddWithValue("@id_p", pers_grid_2.GetFocusedRowCellValue("ID").ToString());
                con.Open();
                rper = (Guid)comm12.ExecuteScalar();
                con.Close();
                rper_load = 1;

            }
            else
            {
                InsMethods.PersData_Default(this);
                Cursor = Cursors.Wait;
                //try
                //{
                Vars.IdP = pers_grid_2.GetFocusedRowCellValue("ID").ToString();
                //}
                //catch
                //{
                //string mz = "Вы не выбрали представителя! Повторите попытку снова!";
                //string tz = "Внимание!";
                //int bz = 0;
                //Message mez = new Message(mz, tz, bz);
                //mez.ShowDialog();
                //}

                SqlCommand comm12 = new SqlCommand(@"select rperson_guid from pol_persons where id=@id_p", con);
                comm12.Parameters.AddWithValue("@id_p", Vars.IdP);
                con.Open();
                rper = (Guid)comm12.ExecuteScalar();
                con.Close();
                SqlCommand comm11 = new SqlCommand($@"
select * from pol_persons p
left join pol_documents pd
on p.idguid=pd.person_guid
left join pol_events e
on p.idguid=e.person_guid
where e.person_guid='{rper}' and main=1", con);
                comm11.Parameters.AddWithValue("@id", Convert.ToInt32(pers_grid_2.GetFocusedRowCellValue("ID")));
                con.Open();
                SqlDataReader reader11 = comm11.ExecuteReader();

                while (reader11.Read()) // построчно считываем данные
                {
                    object fam_ = reader11["FAM"];
                    object im_ = reader11["IM"];
                    object ot_ = reader11["OT"];
                    object dr_ = reader11["DR"];
                    object w_ = reader11["W"];
                    object phone_ = reader11["PHONE"];
                    object doctype = reader11["DOCTYPE"];
                    object docser = reader11["DOCSER"];
                    object docnum = reader11["DOCNUM"];
                    object docdate = reader11["DOCDATE"];
                    object relation = reader11["PRELATION"];


                    doctype1.EditValue = doctype;
                    docser1.Text = docser.ToString();
                    docnum1.Text = docnum.ToString();
                    docdate1.EditValue = Convert.ToDateTime(docdate);
                    fam1.Text = fam_.ToString();
                    im1.Text = im_.ToString();
                    ot1.Text = ot_.ToString();
                    pol_pr.EditValue = Convert.ToInt32(w_);
                    dr1.DateTime = Convert.ToDateTime(dr_);
                    phone_p1.Text = phone_.ToString();


                }

                reader11.Close();

                con.Close();

                Vars.grid_num = 2;
                string m = "Вы хотите исправить ошибки в данных или создать новое событие?";
                string t = "Внимание!";
                int b = 0;
                Message me = new Message(m, t, b);
                me.ShowDialog();

                Potok();
                Cursor = Cursors.Arrow;
            }
            //LoadingDecorator1.IsSplashScreenShown = false;
            if (Vars.Btn == "2")
            {
                this.Title = "Исправление ошибочных данных застрахованного лица";
            }
            else if (Vars.Btn == "1")
            {
                this.Title = "Новый клиент";
            }
            else if (Vars.Btn == "3")
            {
                this.Title = "Создание нового события существующему ЗЛ";
            }
            else
            {
                this.Title = Vars.MainTitle;
            }

        }

        private void doc_type_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (doc_type.SelectedIndex == 13)
            {
                doc_ser.MaskType = MaskType.Regular;
                doc_ser.Mask = @"\d\d \d\d";
            }
            else if (doc_type.SelectedIndex == 2)
            {

                doc_ser.MaskType = MaskType.RegEx;
                doc_ser.Mask = @"[A-Z]{1,4}-[А-Я]{2}";
                //changeKeyBoard_1();
            }
            else
            {
                doc_ser.MaskType = MaskType.None;
            }


        }

        private void sovp_addr_Checked(object sender, RoutedEventArgs e)
        {
            //fias1.reg1.EditValue = fias.reg.EditValue;
            //fias1.reg_rn1.EditValue = fias.reg_rn.EditValue;
            //fias1.reg_town1.EditValue = fias.reg_town.EditValue;
            //fias1.reg_np1.EditValue = fias.reg_np.EditValue;
            //fias1.reg_str1.EditValue = fias.reg_str.EditValue;
            //fias1.reg_dom1.EditValue = fias.reg_dom.EditValue;
            //fias1.reg_korp1.EditValue = fias.reg_korp.EditValue;
            //fias1.reg_ul1.EditValue = fias.reg_ul.EditValue;
            //fias1.reg_kv1.EditValue = fias.reg_kv.EditValue;
            //fias1.reg_dr1.EditValue = fias.reg_dr.EditValue;
        }

        public void pers_grid_1_Loaded(object sender, RoutedEventArgs e)
        {

            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
               new Action(delegate ()
               {
                   //layout_InUse();
                   ////restore_Layout();
                   var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                   var peopleList =
                       MyReader.MySelect<People>(SPR.MyReader.load_pers_grid2 + strf_adm, connectionString);
                   pers_grid_2.ItemsSource = peopleList;
                   //pers_grid_2.Columns[1].Visible = false;
                   //pers_grid_2.Columns[2].Visible = false;
                   //pers_grid_2.Columns[3].Visible = false;
                   //pers_grid_2.Columns[4].Visible = false;
                   pers_grid_2.View.FocusedRowHandle = -1;
                   pers_grid_2.SelectedItem = -1;
                   ////restore_Layout();
               }));
        }
        private void reg_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        public string s;


        private void dost1_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //string s1 = "";
            //if(dost1.EditValue==null)
            //{

            //}
            //else
            //{
            //    var d1= (ICollection)dost1.EditValue;
            //    string[] dd1 = new string[d1.Count];
            //    d1.CopyTo(dd1,0);
            //    for (int i = 0; i < dd1.Count(); i++)
            //    {
            //        s1 += (dd1[i] + ";");
            //        s = s1.Substring(0, s1.Length - 1);
            //    }
            //}

            //List<object>  ind = dost1.SelectedItems.ToList();
            //dost1.SelectedItem = ind;
            /*string[]*/
            //string dst = dost1.Text;//.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            //var ch1 = dost1.EditValue.ToString();
            //int i = dst.Count();
            //for (i = 0; i < dst.Count(); i++)
            //{
            //    //_7.Text += (dst[i].Substring(0, 1) + ",");
            //    _7.Text += (dst[i] + ",");
            //}
            //s = _7.Text.Substring(0, _7.Text.Length - 1);
            //_7.Text = s;
        }





        private void date_vid1_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            DateTime ddd = HOLIDAYS.AddWorkDays(date_vid1.DateTime);

            //List<DateTime> h = HOLIDAYS.H_List;
            //int h_add = Get_holidays(date_vid1.DateTime, date_vid1.DateTime.AddBusinessDays(44));
            //h_add = Get_holidays(date_vid1.DateTime, date_vid1.DateTime.AddBusinessDays(44+h_add));
            DateTime dte = new DateTime(DateTime.Today.Year, 12, 31);
            if (type_policy.EditValue.ToString() != "2")
            {

            }
            else
            {
                pustoy.IsChecked = false;
                if (date_end.EditValue == null && docexp1.EditValue == null)
                {
                    //date_end.DateTime = date_vid1.DateTime.AddBusinessDays(44+h_add);
                    //for (int i = 0; i < h.Count; i++)
                    //{
                    //    if (date_end.DateTime == h[i] || date_end.DateTime.DayOfWeek == DayOfWeek.Saturday || date_end.DateTime.DayOfWeek == DayOfWeek.Sunday)
                    //    {
                    //        date_end.DateTime = date_end.DateTime.AddDays(1);
                    //        i = i - 1;
                    //    }
                    //}
                    date_end.DateTime = ddd;
                    date_poluch.DateTime = date_vid1.DateTime;
                }
                else if (date_end.EditValue != null && type_policy.EditValue.ToString() == "2")
                {
                    //date_end.DateTime = date_vid1.DateTime.AddBusinessDays(44+h_add);
                    //for (int i = 0; i < h.Count; i++)
                    //{
                    //    if (date_end.DateTime == h[i] || date_end.DateTime.DayOfWeek == DayOfWeek.Saturday || date_end.DateTime.DayOfWeek == DayOfWeek.Sunday)
                    //    {
                    //        date_end.DateTime = date_end.DateTime.AddDays(1);
                    //        i = i - 1;
                    //    }
                    //}
                    date_end.DateTime = ddd;
                    date_poluch.DateTime = date_vid1.DateTime;
                }
                else if (date_end.EditValue == null && Convert.ToDateTime(docexp1.EditValue) > dte)
                {
                    date_end.EditValue = dte;
                    date_poluch.DateTime = date_vid1.DateTime;
                }
                else if (date_end.EditValue == null && Convert.ToDateTime(docexp1.EditValue) < dte)
                {
                    date_end.EditValue = docexp1.EditValue;
                    date_poluch.DateTime = date_vid1.DateTime;
                }

            }



        }

        private void pers_grid_2_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                if (pers_grid_2.ActiveFilterInfo == null)
                {
                    var peopleList =
                        MyReader.MySelect<People>(SPR.MyReader.load_pers_grid2 + strf_adm, connectionString);
                    pers_grid_2.ItemsSource = peopleList;
                }
                else
                {
                    string strf = pers_grid_2.ActiveFilterInfo.FilterString.Substring(pers_grid_2.ActiveFilterInfo.FilterString.IndexOf("'", 0));
                    string strf1 = strf.Replace("'", "").Replace(")", "").Replace(".", "");
                    string strf2 = $@"where FAM LIKE '{strf1}%' OR  IM LIKE '{strf1}%' OR  OT LIKE '{strf1}%' order by ID desc";

                    var peopleList =
                        MyReader.MySelect<People>(SPR.MyReader.load_pers_grid2 + strf2, connectionString);
                    pers_grid_2.ItemsSource = peopleList;
                }

            }
            //else if((Keyboard.Modifiers == ModifierKeys.Shift) && e.Key==Key.E)
            //{
            //    Message m = new Message();
            //    m.Show();
            //}



        }

        private void pol_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {

            if (ot.Text == "")
            {

            }
            else
            {
                if (ot.Text.Substring(ot.Text.Length - 2) == "на" && pol.SelectedIndex == 1)
                {

                    ot.Background = new SolidColorBrush(Colors.Red);
                }
                else if (ot.Text.Substring(ot.Text.Length - 2) == "ич" && pol.SelectedIndex == 2)
                {

                    ot.Background = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    ot.Background = new SolidColorBrush(Colors.White);
                }
            }

        }

        private void doc_type_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {

            DateTime firstDate = Convert.ToDateTime(dr.EditValue);
            DateTime secondDate = DateTime.Now;


            if (firstDate != null)
            {
                TimeSpan interval = secondDate.Subtract(firstDate);
                if (doc_type.EditValue == null)
                {
                    return;
                }
                else
                {
                    if (interval.Days / 365.25 > 14 && doc_type.EditValue.ToString() == "3")
                    {
                        doc_type.Background = new SolidColorBrush(Colors.Red);
                    }
                    else if (interval.Days / 365.25 < 14 && doc_type.EditValue.ToString() == "14")
                    {
                        doc_type.Background = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        doc_type.Background = new SolidColorBrush(Colors.White);
                    }
                }

            }

        }
        private int ssn;
        private int psn;
        private int kontr;
        private string ssn1;
        private int psn1;


        private void snils_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                if (snils.Text == "")
                {
                    snils.Background = new SolidColorBrush(Colors.White);
                    return;
                }
                else
                {
                    string[] dst = snils.Text.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);


                    ssn = Convert.ToInt32(dst[0].Substring(0, 1)) * 9 + Convert.ToInt32(dst[0].Substring(1, 1)) * 8 + Convert.ToInt32(dst[0].Substring(2, 1)) * 7 + Convert.ToInt32(dst[1].Substring(0, 1)) * 6 +
                    Convert.ToInt32(dst[1].Substring(1, 1)) * 5 + Convert.ToInt32(dst[1].Substring(2, 1)) * 4 + Convert.ToInt32(dst[2].Substring(0, 1)) * 3 + Convert.ToInt32(dst[2].Substring(1, 1)) * 2 +
                    Convert.ToInt32(dst[2].Substring(2, 1)) * 1;
                    kontr = Convert.ToInt32(dst[2].Substring(dst[2].Length - 2));
                    if (ssn > 101 && ssn <= 200)
                    {
                        psn = ssn - 101;
                        if (psn != kontr)
                        {
                            snils.Background = new SolidColorBrush(Colors.Red);
                        }
                        else
                        {
                            snils.Background = new SolidColorBrush(Colors.White);
                        }

                    }
                    else if (ssn > 200 && ssn <= 300)
                    {
                        psn = ssn - 101 - 101;
                        if (psn != kontr)
                        {
                            snils.Background = new SolidColorBrush(Colors.Red);
                        }
                        else
                        {
                            snils.Background = new SolidColorBrush(Colors.White);
                        }
                    }
                    else if (ssn == 101 || ssn == 100)
                    {
                        psn = 0;
                        if (psn != kontr)
                        {
                            snils.Background = new SolidColorBrush(Colors.Red);
                        }
                        else
                        {
                            snils.Background = new SolidColorBrush(Colors.White);
                        }
                    }
                    else if (ssn < 100)
                    {
                        psn = ssn;
                        if (psn != kontr)
                        {
                            snils.Background = new SolidColorBrush(Colors.Red);
                        }
                        else
                        {
                            snils.Background = new SolidColorBrush(Colors.White);
                        }
                    }
                    else if (ssn > 300)
                    {
                        psn = ssn / 101;
                        if (psn != kontr)
                        {
                            snils.Background = new SolidColorBrush(Colors.Red);
                        }
                        else
                        {
                            snils.Background = new SolidColorBrush(Colors.White);
                        }
                    }
                }
            }
            catch
            {

            }
        }

        private void date_vid_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            { 
            //if(btn_=="2")
            //{
            DateTime firstDate = dr.DateTime;
            DateTime secondDate = DateTime.Now;
            DateTime docDate = date_vid.DateTime;
            if (firstDate != null)
            {
                TimeSpan interval = secondDate.Subtract(firstDate);
                if (interval.Days / 365.25 >= 20 && interval.Days / 365.25 < 45 && doc_type.EditValue.ToString() == "14")
                {
                    TimeSpan interval1 = docDate.Subtract(firstDate);
                    if (interval1.Days / 365.25 < 20)
                    {
                        date_vid.Background = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        date_vid.Background = new SolidColorBrush(Colors.White);
                    }

                }
                else if (interval.Days / 365.25 >= 45 && doc_type.EditValue.ToString() == "14")
                {
                    TimeSpan interval1 = docDate.Subtract(firstDate);
                    if (interval1.Days / 365.25 < 45)
                    {
                        date_vid.Background = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        date_vid.Background = new SolidColorBrush(Colors.White);
                    }
                }
                else
                {
                    date_vid.Background = new SolidColorBrush(Colors.White);
                }

            }
            else
            {
                return;
            }
            // 
             }
            catch
            {

            }
        }



        private void docexp1_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            DateTime dde = new DateTime(DateTime.Today.Year, 12, 31);
            if (Convert.ToDateTime(docexp1.EditValue) > dde)
            {
                date_end.EditValue = dde;
            }
            else
            {
                date_end.EditValue = docexp1.EditValue;
            }

        }

        private void ser_blank_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            DateTime ddd = HOLIDAYS.AddWorkDays(date_vid1.DateTime);

            //List<DateTime> h = HOLIDAYS.H_List;
            //int h_add = Get_holidays(date_vid1.DateTime, date_vid1.DateTime.AddBusinessDays(44));
            //h_add = Get_holidays(date_vid1.DateTime, date_vid1.DateTime.AddBusinessDays(44+h_add));
            DateTime dte = new DateTime(DateTime.Today.Year, 12, 31);
            if (type_policy.EditValue.ToString() != "2")
            {

            }
            else
            {
                pustoy.IsChecked = false;
                if (date_end.EditValue == null && docexp1.EditValue == null)
                {
                    //date_end.DateTime = date_vid1.DateTime.AddBusinessDays(44+h_add);
                    //for (int i = 0; i < h.Count; i++)
                    //{
                    //    if (date_end.DateTime == h[i] || date_end.DateTime.DayOfWeek == DayOfWeek.Saturday || date_end.DateTime.DayOfWeek == DayOfWeek.Sunday)
                    //    {
                    //        date_end.DateTime = date_end.DateTime.AddDays(1);
                    //        i = i - 1;
                    //    }
                    //}
                    date_end.DateTime = ddd;
                    date_poluch.DateTime = date_vid1.DateTime;
                }
                else if (date_end.EditValue != null && type_policy.EditValue.ToString() == "2")
                {
                    //date_end.DateTime = date_vid1.DateTime.AddBusinessDays(44+h_add);
                    //for (int i = 0; i < h.Count; i++)
                    //{
                    //    if (date_end.DateTime == h[i] || date_end.DateTime.DayOfWeek == DayOfWeek.Saturday || date_end.DateTime.DayOfWeek == DayOfWeek.Sunday)
                    //    {
                    //        date_end.DateTime = date_end.DateTime.AddDays(1);
                    //        i = i - 1;
                    //    }
                    //}
                    date_end.DateTime = ddd;
                    date_poluch.DateTime = date_vid1.DateTime;
                }
                else if (date_end.EditValue == null && Convert.ToDateTime(docexp1.EditValue) > dte)
                {
                    date_end.EditValue = dte;
                    date_poluch.DateTime = date_vid1.DateTime;
                }
                else if (date_end.EditValue == null && Convert.ToDateTime(docexp1.EditValue) < dte)
                {
                    date_end.EditValue = docexp1.EditValue;
                    date_poluch.DateTime = date_vid1.DateTime;
                }

            }

            //---------------------------------------
            //int h_add = Get_holidays(date_vid1.DateTime, date_vid1.DateTime.AddBusinessDays(44));
            ////try
            ////{

            //if (date_end.EditValue == null && type_policy.EditValue.ToString() == "2")
            //{
            //    date_end.DateTime = date_vid1.DateTime.AddBusinessDays(44 + h_add);
            //    date_poluch.DateTime = date_vid1.DateTime;
            //}
            //else if (date_end.EditValue != null && type_policy.EditValue.ToString() == "2")
            //{
            //    date_end.DateTime = date_vid1.DateTime.AddBusinessDays(44 + h_add);
            //    date_poluch.DateTime = date_vid1.DateTime;
            //}
            //else if (date_end.EditValue == null && type_policy.EditValue.ToString() != "2")
            //{
            //    date_vid1.EditValue = DateTime.Today;
            //    date_poluch.EditValue = date_vid1.EditValue;
            //}
            //else
            //{
            //    date_vid1.EditValue = DateTime.Today;
            //    date_poluch.EditValue = date_vid1.EditValue;
            //}
            ////}
            ////catch
            ////{
            ////    Window_Loaded(this, e);
            ////}
        }

        private void enp_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            if (enp.EditValue == "")
            {

            }
            else
            {

                string[] dst1 = enp.Text.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                string adkontr = "";


                ssn1 = dst1[0].Substring(14, 1) + dst1[0].Substring(12, 1) + dst1[0].Substring(10, 1) + dst1[0].Substring(8, 1) +
                dst1[0].Substring(6, 1) + dst1[0].Substring(4, 1) + dst1[0].Substring(2, 1) + dst1[0].Substring(0, 1);
                ssn2 = dst1[0].Substring(13, 1) + dst1[0].Substring(11, 1) + dst1[0].Substring(9, 1) + dst1[0].Substring(7, 1) +
                dst1[0].Substring(5, 1) + dst1[0].Substring(3, 1) + dst1[0].Substring(1, 1);
                psn1 = Convert.ToInt32(ssn1) * 2;

                ssn3 = ssn2 + psn1.ToString();
                for (int i = 0; i < ssn3.Length; i++)
                {
                    skontr = Convert.ToInt32(ssn3.Substring(ssn3.Length - ssn3.Length + i, 1));
                    var adk = adkontr.Insert(adkontr.Length, skontr.ToString());
                    adkontr = adk;

                }
                skontr1 = 0;
                for (int i = 0; i < adkontr.ToString().Length; i++)
                {
                    skontr1 += Convert.ToInt32(adkontr.Substring(i, 1));
                }

                kontr1 = Convert.ToInt32(dst1[0].Substring(dst1[0].Length - 1));
                if (skontr1 % 10 == 0)
                {
                    skontr2 = skontr1;
                }
                else
                {
                    skontr2 = (10 - skontr1 % 10) + skontr1;
                }
                skontr3 = skontr2 - skontr1;
                if (skontr3 != kontr1)
                {

                    enp.Background = new SolidColorBrush(Colors.Red);




                }
                else
                {
                    enp.Background = new SolidColorBrush(Colors.White);
                }
            }
        }
        private string ssn2;
        private string ssn3;
        private int kontr1;
        private int skontr;
        private int skontr1;
        private int skontr2;
        private int skontr3;

        private int prev_doc_clk;
        private void prev_doc_btn_Click(object sender, RoutedEventArgs e)
        {
            if (prev_persguid == Guid.Empty)
            {
                prev_im.Text = im.Text;
                prev_ot.Text = ot.Text;
                prev_dr.EditValue = dr.EditValue;
                prev_pol.EditValue = pol.EditValue;
                prev_mr.Text = mr2.Text;

                prev_doc.Visibility = Visibility.Visible;
                tabs.SelectedIndex = 7;
                prev_doc_clk = 0;
            }
            else
            {
                prev_doc.Visibility = Visibility.Visible;
                tabs.SelectedIndex = 7;
                prev_doc_clk = 1;
            }



        }

        private void kat_zl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
            //    type_policy.Focus();
            //}
            if (e.Key == Key.Enter)
            {
                tabs.SelectedIndex = 2;
            }


        }

        private void pustoy_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                tabs.SelectedIndex = 2;
            }
        }

        private void tab_forward(bool tttab)
        {
            if (tttab == true)
            {
                tabs.SelectedIndex = 4;
            }


        }

        public void w2_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                if (fias.bomj.AllowUpdateTextBlockWhenPrinting == true && tabs.SelectedIndex == 3)
                {
                    tabs.SelectedIndex = 4;
                    fias1.sovp_addr.AllowUpdateTextBlockWhenPrinting = true;
                    fias.bomj.AllowUpdateTextBlockWhenPrinting = false;
                }
                else if (fias1.sovp_addr.AllowUpdateTextBlockWhenPrinting == true && tabs.SelectedIndex == 4)
                {
                    tabs.SelectedIndex = 5;
                    fias1.sovp_addr.AllowUpdateTextBlockWhenPrinting = false;
                }
                //else if (fam1.AllowUpdateTextBlockWhenPrinting == true && tabs.SelectedIndex == 4)
                //{
                //    pr_pod_z_polis.Focus();
                //    fam1.AllowUpdateTextBlockWhenPrinting = false;
                //}



            }
            else if (e.Key == Key.F2 && pers_grid_2.SelectedItems.Count != 0)
            {

            }
        }


        private void zl_podp_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                tabs.SelectedIndex = 6;
                fam1.AllowUpdateTextBlockWhenPrinting = true;
            }
            else if (e.Key == Key.Space)
            {
                zl_podp.Load();

                if (zl_podp.EditValue.ToString() != "System.Byte[]")
                {
                    MemoryStream str = new MemoryStream((byte[])zl_podp.EditValue);
                    //BitmapImage bmp = new BitmapImage();
                    //bmp.StreamSource = str;
                    Bitmap bmp = new Bitmap(str);
                    float x = 0.22f;
                    float x1 = 4.6f;
                    var newWidth = bmp.Height * x;
                    var nw = bmp.Width * x;

                    if (newWidth > bmp.Width)
                    {
                        var newWidth1 = bmp.Width;
                        var newHeight = bmp.Width * x1;
                        System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(0, 0, bmp.Width, Convert.ToInt32(newHeight));
                        Bitmap bmp2 = bmp.Clone(cropRect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                        Bitmap bmp1 = new Bitmap(ResizeImage(bmp2, 160, 736));
                        using (var stream = new MemoryStream())
                        {
                            MakeGrayscale(bmp1).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            zl_podp.EditValue = stream.ToArray();
                        }
                    }
                    else if (newWidth >= nw)
                    {
                        var newWidth1 = bmp.Width;
                        var newHeight = bmp.Width * x1;
                        System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(0, 0, bmp.Width, Convert.ToInt32(newHeight));
                        Bitmap bmp2 = bmp.Clone(cropRect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                        Bitmap bmp1 = new Bitmap(ResizeImage(bmp2, 160, 736));
                        using (var stream = new MemoryStream())
                        {
                            MakeGrayscale(bmp1).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            zl_podp.EditValue = stream.ToArray();
                        }

                    }
                    else
                    {
                        System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(Convert.ToInt32(newWidth) / 10, 0, Convert.ToInt32(newWidth), bmp.Height);
                        Bitmap bmp2 = bmp.Clone(cropRect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                        Bitmap bmp1 = new Bitmap(ResizeImage(bmp2, 160, 736));
                        using (var stream = new MemoryStream())
                        {
                            MakeGrayscale(bmp1).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            zl_podp.EditValue = stream.ToArray();
                        }
                    }

                }
            }
            else if (e.Key == Key.Down)
            {

            }

        }

        private void zl_photo_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Space)
            {
                zl_photo.Load();


                if (zl_photo.EditValue != null)
                {
                    MemoryStream str = new MemoryStream((byte[])zl_photo.EditValue);
                    //BitmapImage bmp = new BitmapImage();
                    //bmp.StreamSource = str;
                    Bitmap bmp = new Bitmap(str);

                    if (bmp.HorizontalResolution < 200 && bmp.Width > 1000)
                    {
                        bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    }
                    else
                    {

                    }

                    float x = 0.8f;
                    float x1 = 1.25f;
                    var newWidth = bmp.Height * x;
                    var nw = bmp.Width * x;

                    if (newWidth > bmp.Width)
                    {
                        var newWidth1 = bmp.Width;
                        var newHeight = bmp.Width * x1;
                        System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(0, 0, bmp.Width, Convert.ToInt32(newHeight));
                        Bitmap bmp2 = bmp.Clone(cropRect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                        Bitmap bmp1 = new Bitmap(ResizeImage(bmp2, 320, 400));
                        using (var stream = new MemoryStream())
                        {
                            MakeGrayscale(bmp1).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            zl_photo.EditValue = stream.ToArray();
                        }

                    }
                    else if (newWidth >= nw)
                    {
                        var newWidth1 = bmp.Width;
                        var newHeight = bmp.Width * x1;
                        System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(0, 0, bmp.Width, Convert.ToInt32(newHeight));

                        Bitmap bmp2 = bmp.Clone(cropRect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                        Bitmap bmp1 = new Bitmap(ResizeImage(bmp, 320, 400));
                        //bmp1.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        using (var stream = new MemoryStream())
                        {
                            MakeGrayscale(bmp1).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            zl_photo.EditValue = stream.ToArray();

                        }

                    }
                    else
                    {
                        System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle((bmp.Width - Convert.ToInt32(newWidth)) / 2, 0, Convert.ToInt32(newWidth), bmp.Height);
                        Bitmap bmp2 = bmp.Clone(cropRect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                        Bitmap bmp1 = new Bitmap(ResizeImage(bmp2, 320, 400));
                        using (var stream = new MemoryStream())
                        {
                            MakeGrayscale(bmp1).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            zl_photo.EditValue = stream.ToArray();
                        }
                    }

                }
                else
                {

                }
            }
            else if (e.Key == Key.Down)
            {
                MemoryStream str = new MemoryStream((byte[])zl_photo.EditValue);
                //BitmapImage bmp = new BitmapImage();
                //bmp.StreamSource = str;
                Bitmap bmp = new Bitmap(str);
                float x = 0.8f;
                //float x1 = 1.25f;
                var newWidth = (bmp.Height - bmp.Height / 20) * x;
                //var nw = bmp.Width * x;

                //bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle((bmp.Width - Convert.ToInt32(newWidth)) / 2, bmp.Height / 20, Convert.ToInt32(newWidth), bmp.Height - bmp.Height / 20);
                Bitmap bmp2 = bmp.Clone(cropRect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                Bitmap bmp1 = new Bitmap(ResizeImage(bmp2, 320, 400));
                using (var stream = new MemoryStream())
                {
                    bmp1.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    zl_photo.EditValue = stream.ToArray();
                }
                //zl_photo.Focus();
            }
            else if (e.Key == Key.Up)
            {
                MemoryStream str = new MemoryStream((byte[])zl_photo.EditValue);
                //BitmapImage bmp = new BitmapImage();
                //bmp.StreamSource = str;
                Bitmap bmp = new Bitmap(str);
                float x = 0.8f;
                //float x1 = 1.25f;
                var newWidth = (bmp.Height - bmp.Height / 20) * x;
                //var nw = bmp.Width * x;

                //bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle((bmp.Width - Convert.ToInt32(newWidth)) / 2, 0, Convert.ToInt32(newWidth), bmp.Height - bmp.Height / 20);
                Bitmap bmp2 = bmp.Clone(cropRect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                Bitmap bmp1 = new Bitmap(ResizeImage(bmp2, 320, 400));
                using (var stream = new MemoryStream())
                {
                    bmp1.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    zl_photo.EditValue = stream.ToArray();
                }
            }

        }

        private void addr_reg_zl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
            //    if (fias.bomj.AllowUpdateTextBlockWhenPrinting == true)
            //    {
            //        tabs.SelectedIndex = 2;
            //        fias1.sovp_addr.AllowUpdateTextBlockWhenPrinting = true;
            //        fias.bomj.AllowUpdateTextBlockWhenPrinting = false;
            //    }
            //}
        }

        private void addr_mg_zl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
            //    if (fias1.sovp_addr.AllowUpdateTextBlockWhenPrinting == true)
            //    {
            //        tabs.SelectedIndex = 3;
            //        fias1.sovp_addr.AllowUpdateTextBlockWhenPrinting = false;
            //    }
            //}
        }

        private void fam1_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter && fam1.Text == "")
            //{
            //   form_polis.Focus();

            //}
            //else
            //{
            //    fam1.AllowUpdateTextBlockWhenPrinting = false;
            //}
        }

        private void status_p2_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                pr_pod_z_polis.Focus();
            }
        }



        private void doc_ser_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {


        }

        private void doc_ser_EditorActivated(object sender, RoutedEventArgs e)
        {

        }

        private void doc_ser_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {

            changeKeyBoard_1();
            //InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-EN");
            //System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(new CultureInfo("en-EN"));
        }

        private void doc_ser_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int c = doc_ser.CaretIndex;
            string cc = "";

            if (doc_ser.CaretIndex == 0)
            {

            }
            else
            {
                cc = doc_ser.DisplayText.Substring(doc_ser.CaretIndex - 1, 1);
            }
            if (cc == "-")
            {
                changeKeyBoard();

            }


        }
        private void changeKeyBoard()
        {
            InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("ru-RU");
            //System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(new CultureInfo("ru-RU"));
        }
        private void changeKeyBoard_1()
        {
            InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
            //System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(new CultureInfo("ru-RU"));
        }
        private void fio_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Right)
            //{
            //    changeKeyBoard();

            //}
        }

        private void str_vid_GotFocus(object sender, RoutedEventArgs e)
        {
            changeKeyBoard();
        }

        private void fam_GotFocus(object sender, RoutedEventArgs e)
        {
            changeKeyBoard();

        }

        private void doc_ser_GotFocus(object sender, RoutedEventArgs e)
        {
            // changeKeyBoard_1();
        }

        private void doc_type_LostFocus(object sender, RoutedEventArgs e)
        {
            if (doc_type.EditValue.ToString() == "14" || doc_type.EditValue.ToString() == "3")
            {
                str_r.EditValue = "RUS";
                str_vid.EditValue = "RUS";
                gr.EditValue = "RUS";
            }

        }

        private void Zl_photo_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            


            if (zl_photo.EditValue != null && zl_photo.EditValue.ToString() != "")
            {
                MemoryStream str = new MemoryStream((byte[])zl_photo.EditValue);
                //BitmapImage bmp = new BitmapImage();
                //bmp.StreamSource = str;
                Bitmap bmp = new Bitmap(str);
                if (bmp.Width == 320 && bmp.Height == 400)
                {

                }
                else
                {
                    if (bmp.HorizontalResolution < 200 && bmp.Width > 1000)
                    {
                        bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    }
                    else
                    {

                    }

                    float x = 0.8f;
                    float x1 = 1.25f;
                    var newWidth = bmp.Height * x;
                    var nw = bmp.Width * x;

                    if (newWidth > bmp.Width)
                    {
                        var newWidth1 = bmp.Width;
                        var newHeight = bmp.Width * x1;
                        System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(0, 0, bmp.Width, Convert.ToInt32(newHeight));
                        Bitmap bmp2 = bmp.Clone(cropRect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                        Bitmap bmp1 = new Bitmap(ResizeImage(bmp2, 320, 400));
                        using (var stream = new MemoryStream())
                        {
                            MakeGrayscale(bmp1).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            zl_photo.EditValue = stream.ToArray();
                        }

                    }
                    else if (newWidth >= nw)
                    {
                        var newWidth1 = bmp.Width;
                        var newHeight = bmp.Width * x1;
                        System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(0, 0, bmp.Width, Convert.ToInt32(newHeight));

                        Bitmap bmp2 = bmp.Clone(cropRect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                        Bitmap bmp1 = new Bitmap(ResizeImage(bmp, 320, 400));
                        //bmp1.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        using (var stream = new MemoryStream())
                        {
                            MakeGrayscale(bmp1).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            zl_photo.EditValue = stream.ToArray();

                        }

                    }
                    else
                    {
                        System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle((bmp.Width - Convert.ToInt32(newWidth)) / 2, 0, Convert.ToInt32(newWidth), bmp.Height);
                        Bitmap bmp2 = bmp.Clone(cropRect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                        Bitmap bmp1 = new Bitmap(ResizeImage(bmp2, 320, 400));
                        using (var stream = new MemoryStream())
                        {
                            MakeGrayscale(bmp1).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            zl_photo.EditValue = stream.ToArray();
                        }
                    }
                }
            }
            else
            {

            }
        }




        private void Zl_photo_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (zl_photo.EditValue != null)
            //{
            //    MemoryStream str = new MemoryStream((byte[])zl_photo.EditValue);
            ////BitmapImage bmp = new BitmapImage();
            ////bmp.StreamSource = str;
            //Bitmap bmp = new Bitmap(str);

            //    //if (bmp.Height < bmp.Width && rotate_btn == false)
            //    //{
            //    //    bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);

            //        float x = 0.8f;
            //        float x1 = 1.25f;
            //        var newWidth = bmp.Height * x;
            //        var nw = bmp.Width * x;

            //        if (newWidth > bmp.PhysicalDimension.Width)
            //        {
            //            var newWidth1 = bmp.Width;
            //            var newHeight = bmp.Width * x1;
            //            System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(0, 0, bmp.Width, Convert.ToInt32(newHeight));
            //            Bitmap bmp2 = bmp.Clone(cropRect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            //            Bitmap bmp1 = new Bitmap(ResizeImage(bmp2, 320, 400));
            //            using (var stream = new MemoryStream())
            //            {
            //                MakeGrayscale(bmp1).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //                zl_photo.EditValue = stream.ToArray();
            //            }

            //        }
            //        else if (newWidth >= nw)
            //        {
            //            var newWidth1 = bmp.Width;
            //            var newHeight = bmp.Width * x1;
            //            System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(0, 0, bmp.Width, Convert.ToInt32(newHeight));

            //            Bitmap bmp2 = bmp.Clone(cropRect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            //            Bitmap bmp1 = new Bitmap(ResizeImage(bmp, 320, 400));
            //            //bmp1.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //            using (var stream = new MemoryStream())
            //            {
            //                MakeGrayscale(bmp1).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //                zl_photo.EditValue = stream.ToArray();

            //            }

            //        }
            //        else
            //        {
            //            System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle((bmp.Width - Convert.ToInt32(newWidth)) / 2, 0, Convert.ToInt32(newWidth), bmp.Height);
            //            Bitmap bmp2 = bmp.Clone(cropRect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            //            Bitmap bmp1 = new Bitmap(ResizeImage(bmp2, 320, 400));
            //            using (var stream = new MemoryStream())
            //            {
            //                MakeGrayscale(bmp1).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //                zl_photo.EditValue = stream.ToArray();
            //            }
            //        }
            //    //}
            //    //else
            //    //{



            //    //    float x = 0.8f;
            //    //    float x1 = 1.25f;
            //    //    var newWidth = bmp.Height * x;
            //    //    var nw = bmp.Width * x;

            //    //    if (newWidth > bmp.Width)
            //    //    {
            //    //        var newWidth1 = bmp.Width;
            //    //        var newHeight = bmp.Width * x1;
            //    //        System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(0, 0, bmp.Width, Convert.ToInt32(newHeight));
            //    //        Bitmap bmp2 = bmp.Clone(cropRect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            //    //        Bitmap bmp1 = new Bitmap(ResizeImage(bmp2, 320, 400));
            //    //        using (var stream = new MemoryStream())
            //    //        {
            //    //            MakeGrayscale(bmp1).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //    //            zl_photo.EditValue = stream.ToArray();
            //    //        }

            //    //    }
            //    //    else if (newWidth >= nw)
            //    //    {
            //    //        var newWidth1 = bmp.Width;
            //    //        var newHeight = bmp.Width * x1;
            //    //        System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(0, 0, bmp.Width, Convert.ToInt32(newHeight));

            //    //        Bitmap bmp2 = bmp.Clone(cropRect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            //    //        Bitmap bmp1 = new Bitmap(ResizeImage(bmp, 320, 400));
            //    //        //bmp1.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //    //        using (var stream = new MemoryStream())
            //    //        {
            //    //            MakeGrayscale(bmp1).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //    //            zl_photo.EditValue = stream.ToArray();

            //    //        }

            //    //    }
            //    //    else
            //    //    {
            //    //        System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle((bmp.Width - Convert.ToInt32(newWidth)) / 2, 0, Convert.ToInt32(newWidth), bmp.Height);
            //    //        Bitmap bmp2 = bmp.Clone(cropRect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            //    //        Bitmap bmp1 = new Bitmap(ResizeImage(bmp2, 320, 400));
            //    //        using (var stream = new MemoryStream())
            //    //        {
            //    //            MakeGrayscale(bmp1).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //    //            zl_photo.EditValue = stream.ToArray();
            //    //        }
            //    //    }
            //    //}

            //}
            //else
            //{

            //}
            ////rotate_btn = false;
        }


        //private void fam1_EditValueChanged(object sender, EditValueChangedEventArgs e)
        //{
        //    //fam1.AllowUpdateTextBlockWhenPrinting = false;
        //}

        //private void fam1_TextInput(object sender, TextCompositionEventArgs e)
        //{
        //    //fam1.AllowUpdateTextBlockWhenPrinting = true;
        //}

        //private void tabs_PreviewKeyDown(object sender, KeyEventArgs e)
        //{

        //}







        //private void grayButton_Click(object sender, EventArgs e)
        //{
        //    if (zl_photo.Image != null) // если изображение в pictureBox1 имеется
        //    {
        //        // создаём Bitmap из изображения, находящегося в pictureBox1
        //        Bitmap input = new Bitmap(zl_photo.Image);
        //        // создаём Bitmap для черно-белого изображения
        //        Bitmap output = new Bitmap(input.Width, input.Height);
        //        // перебираем в циклах все пиксели исходного изображения
        //        for (int j = 0; j < input.Height; j++)
        //            for (int i = 0; i < input.Width; i++)
        //            {
        //                // получаем (i, j) пиксель
        //                UInt32 pixel = (UInt32)(input.GetPixel(i, j).ToArgb());
        //                // получаем компоненты цветов пикселя
        //                float R = (float)((pixel & 0x00FF0000) >> 16); // красный
        //                float G = (float)((pixel & 0x0000FF00) >> 8); // зеленый
        //                float B = (float)(pixel & 0x000000FF); // синий
        //                                                       // делаем цвет черно-белым (оттенки серого) - находим среднее арифметическое
        //                R = G = B = (R + G + B) / 3.0f;
        //                // собираем новый пиксель по частям (по каналам)
        //                UInt32 newPixel = 0xFF000000 | ((UInt32)R << 16) | ((UInt32)G << 8) | ((UInt32)B);
        //                // добавляем его в Bitmap нового изображения
        //                output.SetPixel(i, j, Color.FromArgb((int)newPixel));
        //            }
        //        // выводим черно-белый Bitmap в pictureBox2
        //        zl_photo.Image = output;
        //    }
        //}
        public static Bitmap MakeGrayscale(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
         new float[] {.3f, .3f, .3f, 0, 0},
         new float[] {.59f, .59f, .59f, 0, 0},
         new float[] {.11f, .11f, .11f, 0, 0},
         new float[] {0, 0, 0, 1, 0},
         new float[] {0, 0, 0, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);
           
            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new System.Drawing.Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;

        }
        public static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            var destRect = new System.Drawing.Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void Zl_podp_LostFocus(object sender, RoutedEventArgs e)
        {

            if (zl_podp.EditValue != null)
            {
                string s = zl_podp.EditValue.ToString();
                MemoryStream str = new MemoryStream((byte[])zl_podp.EditValue);
                //BitmapImage bmp = new BitmapImage();
                //bmp.StreamSource = str;
                Bitmap bmp = new Bitmap(str);
                if (bmp.Width == 760 && bmp.Height == 160)
                {

                }
                else
                {
                    float x1 = 0.22f;
                    var newHeight = bmp.Width * x1;
                    var newHeight_p = bmp.Width * x1 / 2;



                    System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(0, bmp.Height / 2 - Convert.ToInt32(newHeight_p), bmp.Width, Convert.ToInt32(newHeight));
                    Bitmap bmp2 = bmp.Clone(cropRect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                    Bitmap bmp1 = new Bitmap(ResizeImage(bmp2, 736, 160));
                    using (var stream = new MemoryStream())
                    {
                        MakeGrayscale(bmp1).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        zl_podp.EditValue = stream.ToArray();
                    }

                }
            }
            else
            {

            }
        }

        private void Right_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //MemoryStream str = new MemoryStream((byte[])zl_photo.EditValue);
            ////BitmapImage bmp = new BitmapImage();
            ////bmp.StreamSource = str;
            //Bitmap bmp = new Bitmap(str);

            //bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }

        private void Zl_photo_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private string prevdocSql;
        private void prev_doc_stringSql()
        {
            if (prev_doc_clk == 1)
            {
                prevdocSql = $@"insert into POL_DOCUMENTS(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid,active)
                                values(newid(),(select person_guid from pol_oplist where id=SCOPE_IDENTITY()),'{str_vid1.EditValue}',{doc_type1.EditValue},
'{doc_ser1.Text}','{doc_num1.Text}','{date_vid2.DateTime}','{kem_vid1.Text}','{kod_podr1.Text}',
                                (select event_guid from pol_oplist where id=SCOPE_IDENTITY()),0)
update pol_documents set PREVDOCGUID=(select idguid from pol_documents where id=SCOPE_IDENTITY() and  event_guid=(select event_guid from pol_documents where id=SCOPE_IDENTITY()) and active=1 and main=1 ";

            }
        }
        private void Zl_photo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Zl_photo_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

            //MemoryStream str = new MemoryStream((byte[])zl_photo.EditValue);
            ////BitmapImage bmp = new BitmapImage();
            ////bmp.StreamSource = str;
            //Bitmap bmp = new Bitmap(str);

            ////bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
            //System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(0, 0, 320, 320);
            //Bitmap bmp2 = bmp.Clone(cropRect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            //Bitmap bmp1 = new Bitmap(ResizeImage(bmp2, 320, 400));
            //using (var stream = new MemoryStream())
            //{
            //    bmp1.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //    zl_photo.EditValue = stream.ToArray();
            //}
        }

        private void fam1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && fam1.Text == "")
            {
                form_polis.Focus();

            }

        }

        private void docexp2_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }



        private void Ot_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ot.Text == "")
            {

            }
            else
            {
                if (ot.Text.Substring(ot.Text.Length - 2) == "НА")
                {

                    pol.EditValue = 2;
                }
                else if (ot.Text.Substring(ot.Text.Length - 2) == "ИЧ")
                {

                    pol.EditValue = 1;
                }
                else if (ot.Text.Substring(ot.Text.Length - 2) == "ЗЫ")
                {
                    pol.EditValue = 2;
                }
                else if (ot.Text.Substring(ot.Text.Length - 2) == "ЛЫ")
                {
                    pol.EditValue = 1;
                }
                else
                {

                }
            }


        }




        private void Dr_LostFocus(object sender, RoutedEventArgs e)
        {
            DateTime firstDate = Convert.ToDateTime(dr.EditValue);
            DateTime secondDate = DateTime.Now;


            if (firstDate != null)
            {
                TimeSpan interval = secondDate.Subtract(firstDate);

                if (interval.Days / 365.25 > 14)
                {
                    doc_type.EditValue = 14;
                }
                else if (interval.Days / 365.25 < 14)
                {
                    doc_type.EditValue = 3;
                }
                else
                {

                }


            }
            //pers_grid_2.FilterString = "";
            //pers_grid_2.FilterString = $@"[IM] Like '{im.Text}' And [OT] Like '{ot.Text}' And [DR]='{dr.EditValue}'";


        }

        private void Prev_doc_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void LayoutGroup_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Doc_ser1_GotFocus(object sender, RoutedEventArgs e)
        {
            changeKeyBoard_1();
        }

        private void Doc_type1_Loaded(object sender, RoutedEventArgs e)
        {

            doc_type1.SelectedIndex = 13;

        }

        private void Doc_type1_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (doc_type1.SelectedIndex == 13)
            {
                doc_ser1.MaskType = MaskType.Regular;
                doc_ser1.Mask = @"\d\d \d\d";
            }
            else if (doc_type1.SelectedIndex == 2)
            {

                doc_ser1.MaskType = MaskType.RegEx;
                doc_ser1.Mask = @"[A-Z]{1,4}-[А-Я]{2}";
                //changeKeyBoard_1();
            }
            else
            {
                doc_ser1.MaskType = MaskType.None;
            }
        }

        private void Doc_ser1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int c = doc_ser1.CaretIndex;
            string cc = "";

            if (doc_ser1.CaretIndex == 0)
            {

            }
            else
            {
                cc = doc_ser1.DisplayText.Substring(doc_ser1.CaretIndex - 1, 1);
            }
            if (cc == "-")
            {
                changeKeyBoard();

            }
        }

        private void Doc_num1_LostFocus(object sender, RoutedEventArgs e)
        {
            changeKeyBoard();
        }

        private void Prev_pol_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Prev_ot_LostFocus(object sender, RoutedEventArgs e)
        {
            if (prev_ot.Text == "")
            {

            }
            else
            {
                if (prev_ot.Text.Substring(prev_ot.Text.Length - 2) == "НА")
                {

                    prev_pol.EditValue = 2;
                }
                else if (prev_ot.Text.Substring(prev_ot.Text.Length - 2) == "ИЧ")
                {

                    prev_pol.EditValue = 1;
                }
                else if (prev_ot.Text.Substring(prev_ot.Text.Length - 2) == "ЗЫ")
                {
                    prev_pol.EditValue = 2;
                }
                else if (prev_ot.Text.Substring(prev_ot.Text.Length - 2) == "ЛЫ")
                {
                    prev_pol.EditValue = 1;
                }
                else
                {

                }
            }
        }

        private void W2_Closed(object sender, EventArgs e)
        {
            Vars.CelVisit = null;
            Vars.Sposob = null;
            Vars.Btn = null;
            //this.Owner.Visibility = Visibility.Visible;
            //this.Owner.Focus();

        }

        private void Ot1_LostFocus(object sender, RoutedEventArgs e)
        {

            if (ot1.Text == "")
            {

            }
            else
            {
                if (ot1.Text.Substring(ot1.Text.Length - 2) == "НА")
                {

                    pol_pr.EditValue = 2;
                }
                else if (ot1.Text.Substring(ot1.Text.Length - 2) == "ИЧ")
                {

                    pol_pr.EditValue = 1;
                }
                else if (ot1.Text.Substring(ot1.Text.Length - 2) == "ЗЫ")
                {
                    pol_pr.EditValue = 2;
                }
                else if (ot1.Text.Substring(ot1.Text.Length - 2) == "ЛЫ")
                {
                    pol_pr.EditValue = 1;
                }
                else
                {

                }
            }
            if (pers_grid_2.SelectedItems.Count == 0)
            {
                doctype1.EditValue = 14;
            }
            else
            {

            }
        }

        private void Doctype1_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (doctype1.SelectedIndex == 13)
            {
                docser1.MaskType = MaskType.Regular;
                docser1.Mask = @"\d\d \d\d";

                docnum1.MaskType = MaskType.Regular;
                docnum1.Mask = @"\d\d\d\d\d\d";
            }
        }

        private void Doctype1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (doctype1.SelectedIndex == 13 && pers_grid_2.SelectedItems.Count == 0)
            {
                docser1.MaskType = MaskType.Regular;
                docser1.Mask = @"\d\d \d\d";

            }
            else
            {
                docser1.MaskType = MaskType.None;
            }
        }

        private void Pr_pod_z_smo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
            //    pr_pod_z_smo.Focus();
            //    To_pers();
            //}
            //else
            //{
            //    pr_pod_z_smo.Focus();
            //    return;
            //}

        }
        private void Potok()
        {
            fio_col = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
            im_DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
            ot_DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
            kem_vid_DataContext = MyReader.MySelect<NAME_VP>(@"select id,name from spr_namevp", Properties.Settings.Default.DocExchangeConnectionString);
            fam.DataContext = fio_col;
            im.DataContext = im_DataContext;
            ot.DataContext = ot_DataContext;
            kem_vid.DataContext = kem_vid_DataContext;
            fam1.DataContext = fio_col;
            im1.DataContext = im_DataContext;
            ot1.DataContext = ot_DataContext;
            prev_fam.DataContext = fio_col;
            prev_im.DataContext = im_DataContext;
            prev_ot.DataContext = ot_DataContext;
            //d_obr.EditValue = null;
            Cursor = Cursors.Wait;
            if (Vars.Btn != "1")
            {
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
               new Action(delegate ()
               {
                   var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                   SqlConnection con = new SqlConnection(connectionString1);
                   SqlCommand comm = new SqlCommand("select photo as prf from pol_personsb where event_guid=(select event_guid from pol_persons where id=@id) and type=2", con);
                   comm.Parameters.AddWithValue("@id", Vars.IdP);
                   con.Open();
                   string prf1 = (string)comm.ExecuteScalar();
                   con.Close();
                   SqlCommand comm1 = new SqlCommand("select photo as prp from pol_personsb where event_guid=(select event_guid from pol_persons where id=@id) and type=3", con);
                   comm1.Parameters.AddWithValue("@id", Vars.IdP);
                   con.Open();
                   string prp = (string)comm1.ExecuteScalar();
                   con.Close();

                   if (prf1 == null || prf1 == "")
                   {
                       zl_photo.EditValue = "";
                   }
                   else
                   {
                       zl_photo.EditValue = Convert.FromBase64String(prf1);
                   }
                   if (prp == null || prp == "")
                   {
                       zl_podp.EditValue = "";
                   }
                   else
                   {
                       zl_podp.EditValue = Convert.FromBase64String(prp);
                   }

                   SqlCommand comm3 = new SqlCommand(@"select t0.*,t1.FAM as fam1,t1.im as im1, t1.ot as ot1, t0.DATEVIDACHI as datevidachi, t0.PRIZNAKVIDACHI as priznak_vidachi,
t1.dr as dr1,t1.phone as phone1,t2.PRELATION,t1.idguid as idguid1,t1.w as w1,t0.mo as MO,t0.dstart as DSTART,
t3.idguid as prev_persguid,t3.fam as fam2,t3.im as im2,t3.ot as ot2,t3.w as w2,t3.dr as dr2,t3.mr as mr2,t2.tip_op as tip_op,
t2.method as sppz,t2.rsmo as rsmo,t2.rpolis as rpolis,t2.fpolis as fpolis,t2.petition as petition,t2.dvizit 

from
(select * from pol_persons where id = @id)T0
LEFT join
(select * from pol_persons )T1
on t0.RPERSON_GUID = t1.IDGUID
LEFT join
(select * from pol_events )T2
on t0.EVENT_GUID=t2.IDGUID
LEFT join
(select * from pol_persons_old )T3
on t0.idguid = t3.person_guid", con);
                   comm3.Parameters.AddWithValue("@id", Vars.IdP);
                   con.Open();
                   SqlDataReader reader1 = comm3.ExecuteReader();

                   while (reader1.Read()) // построчно считываем данные
                   {
                       object fam_ = reader1["FAM"];
                       object im_ = reader1["IM"];
                       object ot_ = reader1["OT"];
                       object dr_ = reader1["DR"];
                       object w = reader1["W"];
                       object mr_ = reader1["MR"];
                       object birthoksm = reader1["BIRTH_OKSM"];
                       object coksm = reader1["C_OKSM"];
                       object ss = reader1["SS"];
                       object enp_ = reader1["ENP"];
                       object dost = reader1["DOST"];
                       object phone_ = reader1["PHONE"];
                       object email_ = reader1["EMAIL"];
                       object rpersonguid = reader1["RPERSON_GUID"];
                       object kateg = reader1["kateg"];
                       object dost_ = reader1["DOST"];
                       object ddeath_ = reader1["DDEATH"];
                       object fam_1 = reader1["fam1"];
                       object im_1 = reader1["im1"];
                       object ot_1 = reader1["ot1"];
                       object dr_1 = reader1["dr1"];
                       object phone_1 = reader1["phone1"];
                       object prelation = reader1["PRELATION"];
                       object idguid_ = reader1["idguid1"];
                       object w1_ = reader1["w1"];
                       object _mo = reader1["MO"];
                       object _dstart = reader1["DSTART"];
                       object fam2_ = reader1["fam2"];
                       object im2_ = reader1["im2"];
                       object ot2_ = reader1["ot2"];
                       object dr2_ = reader1["dr2"];
                       object w2 = reader1["w2"];
                       object mr2_ = reader1["mr2"];
                       object tip_op_ = reader1["tip_op"];
                       object sppz_ = reader1["sppz"];
                       object rsmo_ = reader1["rsmo"];
                       object rpolis_ = reader1["rpolis"];
                       object fpolis_ = reader1["fpolis"];
                       object petition_ = reader1["petition"];
                       object dvisit_ = reader1["dvizit"];
                       object srok_doverenosti_ = reader1["SROKDOVERENOSTI"];
                       object prev_persguid_ = reader1["prev_persguid"];

                       prev_persguid = prev_persguid_.ToString() == "" ? Guid.Empty : (Guid)prev_persguid_;


                       if (ddeath_.ToString() == "")
                       {
                           ddeath.EditValue = null;
                       }
                       else
                       {
                           ddeath.DateTime = Convert.ToDateTime(ddeath_);
                       }


                       string dost_1 = dost_.ToString();
                       fam.Text = fam_.ToString();
                       im.Text = im_.ToString();
                       ot.Text = ot_.ToString();
                       dr.DateTime = Convert.ToDateTime(dr_);
                       pol.SelectedIndex = Convert.ToInt32(w);
                       mr2.Text = mr_.ToString();
                       str_r.EditValue = birthoksm.ToString();
                       gr.EditValue = coksm.ToString();
                       enp.Text = enp_.ToString();
                       snils.Text = ss.ToString();
                       phone.Text = phone_.ToString();
                       email.Text = email_.ToString();
                       kat_zl.EditValue = Convert.ToInt32(kateg.ToString() == "" ? 0 : kateg);
                       dost1.EditValue = dost_1.Split(';');
                       cel_vizita.EditValue = tip_op_.ToString();
                       sp_pod_z.EditValue = Convert.ToInt32(sppz_.ToString() == "" ? 0 : sppz_);
                       if(Vars.Btn=="3")
                       {
                           d_obr.EditValue = DateTime.Today;
                       }
                       else if(Vars.Btn == "2")
                       {
                           d_obr.EditValue = Convert.ToDateTime(dvisit_.ToString() == "" ? DateTime.Today : dvisit_);
                       }
                       
                       
                       petition.EditValue = Convert.ToBoolean(petition_.ToString() == "" ? 0 : petition_);
                       pr_pod_z_polis.SelectedIndex = Convert.ToInt32(rpolis_.ToString() == "" ? 0 : rpolis_) - 1;
                       form_polis.SelectedIndex = Convert.ToInt32(fpolis_.ToString() == "" ? 0 : fpolis_);
                       pr_pod_z_smo.SelectedIndex = Convert.ToInt32(rsmo_.ToString() == "" ? 0 : rsmo_) - 1;
                       try
                       {
                           srok_doverenosti.DateTime = Convert.ToDateTime(srok_doverenosti_);
                       }
                       catch
                       {
                           //srok_doverenosti.EditValue = DateTime.Today;
                       }

                       if (_mo.ToString() == "")
                       {
                           mo_cmb.SelectedIndex = -1;
                       }
                       else
                       {
                           mo_cmb.EditValue = _mo.ToString();
                       }

                       if (_dstart.ToString() == "")
                       {
                           date_mo.EditValue = null;
                       }
                       else
                       {
                           date_mo.EditValue = Convert.ToDateTime(_dstart);
                       }

                       prev_fam.Text = fam2_.ToString();
                       prev_im.Text = im2_.ToString();
                       prev_ot.Text = ot2_.ToString();
                       if (w2.ToString() == "")
                       {
                           prev_pol.SelectedIndex = -1;
                       }
                       else
                       {
                           prev_pol.SelectedIndex = Convert.ToInt32(w2);
                       }
                       if (dr2_.ToString() == "")
                       {
                           prev_dr.EditValue = null;
                       }
                       else
                       {
                           prev_dr.DateTime = Convert.ToDateTime(dr2_);
                       }

                       prev_mr.Text = mr2_.ToString();
                       if (rpersonguid.ToString() == "00000000-0000-0000-0000-000000000000")
                       {

                       }
                       else
                       {
                           fam1.Text = fam_1.ToString();
                           im1.Text = im_1.ToString();
                           ot1.Text = ot_1.ToString();
                           pol_pr.SelectedIndex = Convert.ToInt32(w1_);
                           idguid = idguid_.ToString();
                           if (dr_1.ToString() == "")
                           {
                               dr1.EditValue = "";
                           }
                           else
                           {
                               dr1.DateTime = Convert.ToDateTime(dr_1);
                           }

                           phone_p1.Text = phone_1.ToString();
                           if (prelation.ToString() == "")
                           {
                               status_p2.SelectedIndex = -1;
                           }
                           else
                           {
                               status_p2.SelectedIndex = Convert.ToInt32(prelation);
                           }

                       }

                   }
                   reader1.Close();
                   con.Close();
               }));
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                new Action(delegate ()
                {
                    var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                    SqlConnection con = new SqlConnection(connectionString1);
                    SqlCommand comm2 = new SqlCommand(@"select * from pol_documents where event_guid=(select event_guid from pol_persons where id=@id) and main=1 and active=1", con);
                    comm2.Parameters.AddWithValue("@id", Vars.IdP);
                    con.Open();
                    SqlDataReader reader = comm2.ExecuteReader();

                    while (reader.Read()) // построчно считываем данные
                    {
                        object doctype = reader["DOCTYPE"];
                        object docser = reader["DOCSER"];
                        object docnum = reader["DOCNUM"];
                        object docdate = reader["DOCDATE"];
                        object name_vp = reader["NAME_VP"];
                        object name_vp_code = reader["NAME_VP_CODE"];
                        object docmr = reader["DOCMR"];
                        object str_vid_ = reader["OKSM"];

                        doc_type.EditValue = doctype;
                        doc_ser.Text = docser.ToString();
                        doc_num.Text = docnum.ToString();
                        date_vid.DateTime = Convert.ToDateTime(docdate);
                        kem_vid.Text = name_vp.ToString();
                        kod_podr.Text = name_vp_code.ToString();
                        mr2.Text = docmr.ToString();
                        str_vid.EditValue = str_vid_;



                    }

                    reader.Close();

                    con.Close();

                    SqlCommand comm16 = new SqlCommand(@"select * from pol_documents where 
                    event_guid=(select event_guid from pol_persons where id=@id) and main=0 and active=0", con);
                    comm16.Parameters.AddWithValue("@id", Vars.IdP);
                    con.Open();
                    SqlDataReader reader16 = comm16.ExecuteReader();

                    while (reader16.Read()) // построчно считываем данные
                    {
                        object doctype_1 = reader16["DOCTYPE"];
                        object docser_1 = reader16["DOCSER"];
                        object docnum_1 = reader16["DOCNUM"];
                        object docdate_1 = reader16["DOCDATE"];
                        object name_vp_1 = reader16["NAME_VP"];
                        object name_vp_code_1 = reader16["NAME_VP_CODE"];
                        object docmr_1 = reader16["DOCMR"];
                        object str_vid_1 = reader16["OKSM"];

                        if (doctype_1.ToString() == "")
                        {

                        }
                        else
                        {
                            doc_type1.EditValue = doctype_1;
                        }

                        doc_ser1.Text = docser_1.ToString();
                        doc_num1.Text = docnum_1.ToString();
                        if (docdate_1.ToString() == "")
                        {

                        }
                        else
                        {
                            date_vid2.DateTime = Convert.ToDateTime(docdate_1);
                        }

                        kem_vid1.Text = name_vp_1.ToString();
                        kod_podr1.Text = name_vp_code_1.ToString();
                        prev_mr.Text = docmr_1.ToString();
                        str_vid1.EditValue = str_vid_1;



                    }

                    reader16.Close();

                    con.Close();


                    SqlCommand comm4 = new SqlCommand(@"select * from pol_documents 
where event_guid=(select event_guid from pol_persons where id=@id) and main=0 and active=1", con);
                    comm4.Parameters.AddWithValue("@id", Vars.IdP);
                    con.Open();
                    SqlDataReader reader2 = comm4.ExecuteReader();

                    while (reader2.Read()) // построчно считываем данные
                    {
                        object doctype = reader2["DOCTYPE"];
                        object docser = reader2["DOCSER"];
                        object docnum = reader2["DOCNUM"];
                        object docdate = reader2["DOCDATE"];
                        object docexp = reader2["DOCEXP"];
                        object name_vp = reader2["NAME_VP"];



                        ddtype.EditValue = doctype;
                        ddser.Text = docser.ToString();
                        ddnum.Text = docnum.ToString();
                        dddate.EditValue = Convert.ToDateTime(docdate);
                        docexp1.EditValue = Convert.ToDateTime(docexp);
                        ddkemv.Text = name_vp.ToString();



                    }

                    reader2.Close();

                    con.Close();
                    SqlCommand comm10 = new SqlCommand(@"select * from pol_documents where " +
                        "person_guid=(select RPERSON_GUID from pol_persons where id=@id) and main=1", con);
                    comm10.Parameters.AddWithValue("@id", Vars.IdP);
                    con.Open();
                    SqlDataReader reader10 = comm10.ExecuteReader();

                    while (reader10.Read()) // построчно считываем данные
                    {
                        object doctype = reader10["DOCTYPE"];
                        object docser = reader10["DOCSER"];
                        object docnum = reader10["DOCNUM"];
                        object docdate = reader10["DOCDATE"];
                        object name_vp = reader10["NAME_VP"];



                        doctype1.EditValue = doctype;
                        docser1.Text = docser.ToString();
                        docnum1.Text = docnum.ToString();
                        docdate1.DateTime = Convert.ToDateTime(docdate);




                    }

                    reader10.Close();

                    con.Close();
                }));

                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                new Action(delegate ()
                {
                    var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                    SqlConnection con = new SqlConnection(connectionString1);

                    SqlCommand comm5 = new SqlCommand(@"select *from pol_addresses pa left join POL_RELATION_ADDR_PERS pr on pa.IDGUID=pr.ADDR_GUID 
where pr.event_guid=(select event_guid from pol_persons where id=@id) and pr.addres_g=1 ", con);
                    comm5.Parameters.AddWithValue("@id", Vars.IdP);
                    con.Open();
                    SqlDataReader reader3 = comm5.ExecuteReader();

                    while (reader3.Read()) // построчно считываем данные
                    {
                        object obl = reader3["FIAS_L1"];
                        object rn = reader3["FIAS_L3"];
                        object town = reader3["FIAS_L4"];
                        object np = reader3["FIAS_L6"];
                        object street = reader3["FIAS_L7"];
                        object dom_ = reader3["HOUSE_GUID"];
                        object korp_ = reader3["KORP"];
                        object str_ = reader3["EXT"];
                        object kv_ = reader3["KV"];
                        object d_reg = reader3["DREG"];
                        object bomg = reader3["BOMG"];
                        object addr_g_ = reader3["ADDRES_G"];
                        object addr_p_ = reader3["ADDRES_P"];
                        // object str = reader3["addrstr"];



                        //fias.addrstr.Text = str.ToString();
                        L1 = obl.ToString();
                        L3 = rn.ToString();
                        L4 = town.ToString();
                        L6 = np.ToString();
                        L7 = street.ToString();
                        fias.reg.EditValue = obl;
                        if (rn.ToString() == "00000000-0000-0000-0000-000000000000")
                        {
                            fias.reg_rn.EditValue = null;
                        }
                        else
                        {
                            fias.reg_rn.EditValue = rn;
                        }
                        if (town.ToString() == "00000000-0000-0000-0000-000000000000")
                        {
                            fias.reg_town.EditValue = null;
                        }
                        else
                        {
                            fias.reg_town.EditValue = town;
                        }
                        //fias.reg_rn.EditValue = rn;
                        //fias.reg_town.EditValue = town;
                        if (np.ToString() == "00000000-0000-0000-0000-000000000000")
                        {
                            fias.reg_np.EditValue = null;

                        }
                        else
                        {
                            fias.reg_np.EditValue = np;
                        }
                        if (street.ToString() == "00000000-0000-0000-0000-000000000000")
                        {
                            fias.reg_ul.EditValue = null;

                        }
                        else
                        {
                            fias.reg_ul.EditValue = street;
                        }

                        fias.reg_dom.EditValue = dom_;
                        if (fias.reg_dom.EditValue != null)
                        {
                            dstrkor = fias.reg_dom.Text.Replace(' ', ',').Split(',');
                            domsplit = dstrkor[0].Replace("д.", "");
                        }
                        else
                        {
                            domsplit = "";
                        }

                        fias.reg_korp.Text = korp_.ToString();
                        fias.reg_str.Text = str_.ToString();
                        fias.reg_kv.Text = kv_.ToString();
                        fias.bomj.IsChecked = Convert.ToBoolean(bomg);
                        if (d_reg.ToString() == "")
                        {

                        }
                        else
                        {
                            fias.reg_dr.DateTime = Convert.ToDateTime(d_reg);
                        }

                        if (Convert.ToBoolean(addr_g_) == true && Convert.ToBoolean(addr_p_) == true)
                        {
                            fias1.sovp_addr.IsChecked = true;
                        }
                        else
                        {
                            fias1.sovp_addr.IsChecked = false;
                        }



                    }
                    reader3.Close();
                    con.Close();
                    if (fias.bomj.IsChecked == false)
                    {
                        SqlCommand comm6 = new SqlCommand(@"select *from pol_addresses pa left join POL_RELATION_ADDR_PERS pr on pa.IDGUID=pr.ADDR_GUID 
where pr.event_guid=(select event_guid from pol_persons where id=@id) and pr.addres_p=1 ", con);
                        comm6.Parameters.AddWithValue("@id", Vars.IdP);
                        con.Open();
                        SqlDataReader reader4 = comm6.ExecuteReader();

                        while (reader4.Read()) // построчно считываем данные
                        {
                            object obl = reader4["FIAS_L1"];
                            object rn = reader4["FIAS_L3"];
                            object town = reader4["FIAS_L4"];
                            object np = reader4["FIAS_L6"];
                            object street = reader4["FIAS_L7"];
                            object dom_ = reader4["HOUSE_GUID"];
                            object korp_ = reader4["KORP"];
                            object str_ = reader4["EXT"];
                            object kv_ = reader4["KV"];
                            object d_reg = reader4["DREG"];
                            object bomg = reader4["BOMG"];
                            //object str = reader4["addrstr"];

                            //fias1.addrstr1.Text = str.ToString();
                            L1_1 = obl.ToString();
                            L3_1 = rn.ToString();
                            L4_1 = town.ToString();
                            L6_1 = np.ToString();
                            L7_1 = street.ToString();
                            fias1.reg1.EditValue = obl;
                            if (rn.ToString() == "00000000-0000-0000-0000-000000000000")
                            {
                                fias1.reg_rn1.EditValue = null;
                            }
                            else
                            {
                                fias1.reg_rn1.EditValue = rn;
                            }
                            if (town.ToString() == "00000000-0000-0000-0000-000000000000")
                            {
                                fias1.reg_town1.EditValue = null;
                            }
                            else
                            {
                                fias1.reg_town1.EditValue = town;
                            }
                            //fias.reg_rn.EditValue = rn;
                            //fias.reg_town.EditValue = town;
                            if (np.ToString() == "00000000-0000-0000-0000-000000000000")
                            {
                                fias1.reg_np1.EditValue = null;

                            }
                            else
                            {
                                fias1.reg_np1.EditValue = np;
                            }
                            if (street.ToString() == "00000000-0000-0000-0000-000000000000")
                            {
                                fias1.reg_ul1.EditValue = null;

                            }
                            else
                            {
                                fias1.reg_ul1.EditValue = street;
                            }
                            fias1.reg_dom1.EditValue = dom_;

                            if (fias1.reg_dom1.EditValue != null)
                            {
                                dstrkor1 = fias1.reg_dom1.Text.Replace(' ', ',').Split(',');
                                domsplit1 = dstrkor1[0].Replace("д.", "");
                            }
                            else
                            {
                                domsplit1 = "";
                            }
                            fias1.reg_korp1.Text = korp_.ToString();
                            fias1.reg_str1.Text = str_.ToString();
                            fias1.reg_kv1.Text = kv_.ToString();
                            if (d_reg.ToString() == "")
                            {

                            }
                            else
                            {
                                fias1.reg_dr1.DateTime = Convert.ToDateTime(d_reg);
                            }




                        }
                        reader4.Close();
                        con.Close();

                        SqlCommand comm26 = new SqlCommand("select * from pol_addresses pa " +
                     "left join POL_RELATION_ADDR_PERS pr on pa.IDGUID=pr.ADDR_GUID  " +
                     "where pr.event_guid=(select event_guid from pol_persons where id=@id) and pr.addres_p=1", con);
                        comm26.Parameters.AddWithValue("@id", Vars.IdP);
                        con.Open();
                        SqlDataReader reader14 = comm26.ExecuteReader();

                        while (reader14.Read()) // построчно считываем данные
                        {
                            object obl = reader14["FIAS_L1"];
                            object rn = reader14["FIAS_L3"];
                            object town = reader14["FIAS_L4"];
                            object np = reader14["FIAS_L6"];
                            object street = reader14["FIAS_L7"];
                            object dom_ = reader14["HOUSE_GUID"];
                            object korp_ = reader14["KORP"];
                            object str_ = reader14["EXT"];
                            object kv_ = reader14["KV"];
                            object d_reg = reader14["DREG"];
                            object bomg = reader14["BOMG"];


                            L1_1 = obl.ToString();
                            L3_1 = rn.ToString();
                            L4_1 = town.ToString();
                            L6_1 = np.ToString();
                            L7_1 = street.ToString();
                            fias1.reg1.EditValue = obl;
                            if (rn.ToString() == "00000000-0000-0000-0000-000000000000")
                            {
                                fias1.reg_rn1.EditValue = null;
                            }
                            else
                            {
                                fias1.reg_rn1.EditValue = rn;
                            }
                            if (town.ToString() == "00000000-0000-0000-0000-000000000000")
                            {
                                fias1.reg_town1.EditValue = null;
                            }
                            else
                            {
                                fias1.reg_town1.EditValue = town;
                            }
                            //fias.reg_rn.EditValue = rn;
                            //fias.reg_town.EditValue = town;
                            if (np.ToString() == "00000000-0000-0000-0000-000000000000")
                            {
                                fias1.reg_np1.EditValue = null;

                            }
                            else
                            {
                                fias1.reg_np1.EditValue = np;
                            }
                            if (street.ToString() == "00000000-0000-0000-0000-000000000000")
                            {
                                fias1.reg_ul1.EditValue = null;

                            }
                            else
                            {
                                fias1.reg_ul1.EditValue = street;
                            }
                            fias1.reg_dom1.EditValue = dom_;

                            if (fias1.reg_dom1.EditValue != null)
                            {
                                dstrkor1 = fias1.reg_dom1.Text.Replace(' ', ',').Split(',');
                                domsplit1 = dstrkor1[0].Replace("д.", "");
                            }
                            else
                            {
                                domsplit1 = "";
                            }
                            fias1.reg_korp1.Text = korp_.ToString();
                            fias1.reg_str1.Text = str_.ToString();
                            fias1.reg_kv1.Text = kv_.ToString();
                            if (d_reg.ToString() == "")
                            {

                            }
                            else
                            {
                                fias1.reg_dr1.DateTime = Convert.ToDateTime(d_reg);
                            }



                        }
                        reader14.Close();
                        con.Close();

                        if (Vars.Btn == "2")
                        {
                            this.Title = "Исправление ошибочных данных застрахованного лица";
                        }
                        else
                        {
                            this.Title = "Создание нового события существующему ЗЛ";
                            d_obr.EditValue = DateTime.Today;
                        }
                    }
                }));
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                new Action(delegate ()
                {
                    var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                    SqlConnection con = new SqlConnection(connectionString1);
                    if (Vars.CelVisit == "П010" || Vars.CelVisit == "П034" || Vars.CelVisit == "П035" || Vars.CelVisit == "П036" || Vars.CelVisit == "П061" || Vars.CelVisit == "П062" || Vars.CelVisit == "П063")
                    {
                        SqlCommand comm7;

                        if (Vars.Btn == "2")
                        {
                            comm7 = new SqlCommand("select * from pol_polises where event_guid=(select event_guid from pol_persons where id=@id)", con);
                            comm7.Parameters.AddWithValue("@id", Vars.IdP);
                        }
                        else
                        {
                            comm7 = new SqlCommand("select * from pol_polises where id=(select min(id) from POL_POLISES where vpolis=2 and blank=1 and DBEG is null)", con);
                            comm7.Parameters.AddWithValue("@id", Vars.IdP);

                        }

                        con.Open();
                        SqlDataReader reader5 = comm7.ExecuteReader();

                        while (reader5.Read()) // построчно считываем данные
                        {
                            object vpolis = reader5["VPOLIS"];
                            object spolis = reader5["SPOLIS"];
                            object npolis = reader5["NPOLIS"];
                            object dbeg = reader5["DBEG"];
                            object dend = reader5["DEND"];
                            object dstop = reader5["DSTOP"];
                            object dout_ = reader5["DOUT"];
                            object drecieved = reader5["DRECEIVED"];
                            object blank = reader5["BLANK"];



                            ser_blank.Text = spolis.ToString();
                            num_blank.Text = npolis.ToString();
                            sblank = spolis.ToString();
                            spolis_ = spolis.ToString();

                            if (Vars.Btn == "2")
                            {
                                type_policy.EditValue = Convert.ToInt32(vpolis);
                                date_vid1.EditValue = Convert.ToDateTime(dbeg);
                                date_poluch.EditValue = Convert.ToDateTime(dbeg);
                                if (dstop == DBNull.Value)
                                {
                                    fakt_prekr.EditValue = null;
                                }
                                else
                                {
                                    fakt_prekr.EditValue = Convert.ToDateTime(dstop);
                                }

                            }
                            else
                            {
                                type_policy.EditValue = 2;
                                date_vid1.EditValue = DateTime.Today;
                                date_poluch.EditValue = date_vid1.DateTime;
                            }



                            if (Convert.ToBoolean(blank) == true)
                            {
                                pustoy.IsChecked = true;
                            }
                            else
                            {
                                pustoy.IsChecked = false;
                            }



                        }
                        reader5.Close();
                        con.Close();

                    }
                    else
                    {
                        type_policy.EditValue = 3;
                        date_vid1.EditValue = null;
                        date_poluch.EditValue = null;
                        SqlCommand comm7 = new SqlCommand("select * from pol_polises  where event_guid=(select event_guid from pol_persons where id=@id)", con);
                        comm7.Parameters.AddWithValue("@id", Vars.IdP);
                        con.Open();
                        SqlDataReader reader5 = comm7.ExecuteReader();

                        while (reader5.Read()) // построчно считываем данные
                        {
                            object vpolis = reader5["VPOLIS"];
                            object spolis = reader5["SPOLIS"];
                            object npolis = reader5["NPOLIS"];
                            object dbeg = reader5["DBEG"];
                            object dend = reader5["DEND"];
                            object dstop = reader5["DSTOP"];
                            object dout_ = reader5["DOUT"];
                            object blank = reader5["BLANK"];
                            object dreceived = reader5["DRECEIVED"];




                            type_policy.EditValue = Convert.ToInt32(vpolis);
                            ser_blank.Text = spolis.ToString();
                            num_blank.Text = npolis.ToString();

                            date_vid1.EditValue = Convert.ToDateTime(dbeg);
                            if (dend == DBNull.Value)
                            {
                                date_end.EditValue = null;
                            }
                            else
                            {
                                date_end.EditValue = Convert.ToDateTime(dend);
                            }

                            if (dstop == DBNull.Value)
                            {
                                fakt_prekr.EditValue = null;
                            }
                            else
                            {
                                fakt_prekr.EditValue = Convert.ToDateTime(dstop);
                            }

                            if (dreceived == DBNull.Value)
                            {
                                date_vid.EditValue = null;
                            }
                            else
                            {
                                date_poluch.EditValue = Convert.ToDateTime(dreceived);
                            }
                            if (dout_ == DBNull.Value)
                            {
                                dout.EditValue = null;
                            }
                            else
                            {
                                dout.EditValue = Convert.ToDateTime(dout_);
                            }


                            if (Convert.ToBoolean(blank) == true)
                            {
                                pustoy.IsChecked = true;
                            }
                            else
                            {
                                pustoy.IsChecked = false;
                            }



                        }
                        reader5.Close();
                        con.Close();

                    }
                    SqlCommand comm8 = new SqlCommand(@"select *from pol_addresses old_g where 
event_guid=(select event_guid from pol_persons where id=@id)", con);
                    comm8.Parameters.AddWithValue("@id", Vars.IdP);
                    con.Open();
                    SqlDataReader reader8 = comm8.ExecuteReader();

                    while (reader8.Read()) // построчно считываем данные
                    {
                        object adres_ = reader8["Old_G"];

                        fias.adres.Text = adres_.ToString();
                    }
                    reader8.Close();
                    con.Close();
                    //Binding bind = new Binding();
                    //bind.Source = towntxt;
                    //bind.Path = new PropertyPath("Text");
                    //bind.Mode = BindingMode.TwoWay;
                    //fias.reg_town.SetBinding(ComboBoxEdit.DataContextProperty, bind);
                    //if (kat_zl.EditValue != "")
                    // {
                    //     kat_zl.SelectedIndex = Convert.ToInt32(kat_zl.EditValue) - 1;
                    // }
                    // else
                    // {
                    //     kat_zl.SelectedIndex = -1;
                    // }
                    Vars.NewCelViz = 0;
                }));
            }
            Cursor = Cursors.Arrow;
        }
        public bool blank_polis;
        private void Cel_vizita_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            blank_polis = false;
            var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString1);
            if (cel_vizita.EditValue == null)
            {

            }
            else
            {
                Vars.CelVisit = cel_vizita.EditValue.ToString();
                Vars.NewCelViz = 1;
            }
            if (Vars.CelVisit == "П010" || Vars.CelVisit == "П034" || Vars.CelVisit == "П035" || Vars.CelVisit == "П036" || Vars.CelVisit == "П061" || Vars.CelVisit == "П062" || Vars.CelVisit == "П062" || Vars.CelVisit == "П063")
            {
                if (Vars.Btn != "2")
                {
                    type_policy.EditValue = 2;
                    date_vid1.EditValue = DateTime.Today;
                    date_poluch.EditValue = date_vid1.DateTime;
                    SqlCommand comm7 = new SqlCommand(@"select * from pol_polises where id=(select min(id) from POL_POLISES 
where vpolis=2 and blank=1 and DBEG is null)", con);
                    con.Open();
                    SqlDataReader reader8 = comm7.ExecuteReader();

                    while (reader8.Read()) // построчно считываем данные
                    {
                        object s_polis_ = reader8["SPOLIS"];
                        object n_polis_ = reader8["NPOLIS"];
                        object blank_ = reader8["BLANK"];

                        ser_blank.Text = s_polis_.ToString();
                        num_blank.Text = n_polis_.ToString();
                        pustoy.EditValue = blank_;
                        if ((bool)blank_ == true)
                        {
                            blank_polis = true;
                            pustoy_lbl.Label = "Загружен пустой бланк из базы!";
                            pustoy_lbl.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            ser_blank.Text = "";
                            num_blank.Text = "";
                        }
                    }
                    reader8.Close();
                    con.Close();
                }

            }
            else
            {
                if (ddnum.Text != "")
                {
                    type_policy.EditValue = 3;
                }
                else
                {
                    type_policy.EditValue = 3;
                    ser_blank.Text = "";
                    num_blank.Text = "";
                    date_vid1.EditValue = null;
                    date_poluch.EditValue = null;
                    date_end.EditValue = null;

                }
            }
        }

        private void Sp_pod_z_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            if (sp_pod_z.EditValue == null)
            {

            }
            else
            {
                Vars.Sposob = sp_pod_z.EditValue.ToString();

            }
        }


        private void D_obr_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            Vars.DateVisit = Convert.ToDateTime(d_obr.EditValue);
        }
        private void print_Click_1(object sender, RoutedEventArgs e)
        {
            if (pers_grid_2.SelectedItems.Count > 1)
            {
                string m1 = "Вы выбрали больше 1 клиента!";
                string t1 = "Ошибка!";
                int b1 = 1;
                Message me1 = new Message(m1, t1, b1);
                me1.ShowDialog();

                return;
            }
            else
            {
                Vars.IdZl = Convert.ToInt32(pers_grid_2.GetFocusedRowCellValue("ID"));
                Vars.Forms = 1;
                Docs_print docs = new Docs_print();
                docs.ShowDialog();
            }

            //int idpers = Convert.ToInt32(pers_grid_1.GetFocusedRowCellValue("ID"));
            //Zayavlenie shreport = new Zayavlenie(idpers);
            //shreport.Show();
        }

        private void W2_Activated(object sender, EventArgs e)
        {
            if (pers_grid_2.SelectedItems.Count > 0 && Vars.mes_res == 10)
            {

            }
            else if (pers_grid_2.SelectedItems.Count == 0 && Vars.mes_res == 10)
            {

            }
            else
            {
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                var peopleList =
                    MyReader.MySelect<People>(SPR.MyReader.load_pers_grid2 + strf_adm, connectionString);
                pers_grid_2.ItemsSource = peopleList;
                //pers_grid_2.Columns[1].Visible = false;
                //pers_grid_2.Columns[2].Visible = false;
                //pers_grid_2.Columns[3].Visible = false;
                //pers_grid_2.Columns[4].Visible = false;
                pers_grid_2.View.FocusedRowHandle = -1;
                pers_grid_2.SelectedItem = -1;
            }
        }

        private void Type_policy_LostFocus(object sender, RoutedEventArgs e)
        {
            if (type_policy.SelectedIndex == 1)
            {
                ser_blank.MaskType = MaskType.Regular;
                ser_blank.Mask = @"\d\d\d";
                num_blank.MaskType = MaskType.Regular;
                num_blank.Mask = @"\d\d\d\d\d\d";
            }
            else if (type_policy.SelectedIndex == 2)
            {
                ser_blank.MaskType = MaskType.Regular;
                ser_blank.Mask = @"\d\d \d\d";
                num_blank.MaskType = MaskType.Regular;
                num_blank.Mask = @"\d\d\d\d\d\d\d";
            }
            else
            {
                docser1.MaskType = MaskType.None;
            }
        }

        private void Ddtype_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            //date_end.DateTime = new DateTime(DateTime.Today.Year, 12, 31);
            //date_poluch.DateTime = date_vid1.DateTime;
        }
        private bool rotate_btn = false;
        private bool rotate_btn_l = false;
        private void Rotate_Click(object sender, RoutedEventArgs e)
        {
            rotate_btn = true;
            MemoryStream str = new MemoryStream((byte[])zl_photo.EditValue);
            //BitmapImage bmp = new BitmapImage();
            //bmp.StreamSource = str;
            Bitmap bmp = new Bitmap(str);
            bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            using (var stream = new MemoryStream())
            {
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                zl_photo.EditValue = stream.ToArray();
            }
        }

        private void Zl_photo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MemoryStream str = new MemoryStream((byte[])zl_photo.EditValue);
            //BitmapImage bmp = new BitmapImage();
            //bmp.StreamSource = str;
            Bitmap bmp = new Bitmap(str);
            float x = 0.8f;
            //float x1 = 1.25f;
            var newWidth = (bmp.Height - bmp.Height / 20) * x;
            //var nw = bmp.Width * x;

            //bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
            System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle((bmp.Width - Convert.ToInt32(newWidth)) / 2, bmp.Height / 20, Convert.ToInt32(newWidth), bmp.Height - bmp.Height / 20);
            Bitmap bmp2 = bmp.Clone(cropRect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            Bitmap bmp1 = new Bitmap(ResizeImage(bmp2, 320, 400));
            using (var stream = new MemoryStream())
            {
                bmp1.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                zl_photo.EditValue = stream.ToArray();
            }
            zl_photo.Focus();
        }

        private void Zl_photo_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MemoryStream str = new MemoryStream((byte[])zl_photo.EditValue);
            //BitmapImage bmp = new BitmapImage();
            //bmp.StreamSource = str;
            Bitmap bmp = new Bitmap(str);
            float x = 0.8f;
            //float x1 = 1.25f;
            var newWidth = (bmp.Height - bmp.Height / 20) * x;
            //var nw = bmp.Width * x;

            //bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
            System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle((bmp.Width - Convert.ToInt32(newWidth)) / 2, 0, Convert.ToInt32(newWidth), bmp.Height - bmp.Height / 20);
            Bitmap bmp2 = bmp.Clone(cropRect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            Bitmap bmp1 = new Bitmap(ResizeImage(bmp2, 320, 400));
            using (var stream = new MemoryStream())
            {
                bmp1.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                zl_photo.EditValue = stream.ToArray();
            }
        }

        private void Zl_photo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Zl_photo_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Polis_zl_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Pers_d_zl_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Rotate_l_Click(object sender, RoutedEventArgs e)
        {
            rotate_btn = true;
            MemoryStream str = new MemoryStream((byte[])zl_photo.EditValue);
            //BitmapImage bmp = new BitmapImage();
            //bmp.StreamSource = str;
            Bitmap bmp = new Bitmap(str);
            bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
            using (var stream = new MemoryStream())
            {
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                zl_photo.EditValue = stream.ToArray();
            }
        }



        private void Pol_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Dr_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Vars.mes_res = 10;
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                string strf2 = $@"where IM LIKE '{
                         im.Text}%' and  OT LIKE '{
                         ot.Text}%' and  convert(nvarchar,dr,104) like left('{
                         dr.EditValue}%',10) order by ID desc";
                var peopleList =
                    MyReader.MySelect<People>(SPR.MyReader.load_pers_grid2 + strf2, connectionString);

                pers_grid_2.ItemsSource = peopleList;
                if (pers_grid_2.VisibleRowCount != 0)
                {
                    string m = "Застрахованное лицо с похожими ПД уже есть в базе данных!";
                    string t = "Сообщение";
                    int b = 1;
                    Message me = new Message(m, t, b);
                    me.ShowDialog();

                }
            }
        }

        private void Doc_num_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Vars.mes_res = 10;
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                string strf2 = $@"where d.docser LIKE '{
                         doc_ser.Text}%' and  d.docnum LIKE '{
                         doc_num.Text}%'  order by ID desc";
                var peopleList =
                    MyReader.MySelect<People>(SPR.MyReader.load_pers_grid2 + strf2, connectionString);

                pers_grid_2.ItemsSource = peopleList;
                if (pers_grid_2.VisibleRowCount != 0 && Vars.Btn=="1")
                {
                    string m = "Застрахованное лицо с такими серией и номером документа уже есть в базе данных!";
                    string t = "Сообщение";
                    int b = 1;
                    Message me = new Message(m, t, b);
                    me.ShowDialog();

                }
            }
        }

        private void Fam_ProcessNewValue(DependencyObject sender, ProcessNewValueEventArgs e)
        {
            string _fam = fam.DisplayText;
            string _fam1 = fam1.DisplayText;
            string _prev_fam = prev_fam.DisplayText;

            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            var con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand($@" insert into spr_fam (fam) values('{fam.DisplayText}')", con);
            string m1 = "Добавить новое значение в справочник?";
            string t1 = "Внимание!";
            int b1 = 2;
            Message me1 = new Message(m1, t1, b1);
            me1.ShowDialog();

            if (Vars.mes_res == 1)
            {
                con.Open();
                com.ExecuteNonQuery();
                con.Close();

                fam.DataContext = MyReader.MySelect<FIO>(@"select fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
                fam1.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
                prev_fam.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
                fam.Text = _fam;
                fam1.Text = _fam1;
                prev_fam.Text = _prev_fam;
            }
            else
            {
                Yamed.Control.EnterKeyTraversal.SetIsEnabled(w_main, true);
                //Yamed.Control.EnterKeyTraversal.SetIsEnabled(im, false);
                //return;
            }

        }

        private void Im_ProcessNewValue(DependencyObject sender, ProcessNewValueEventArgs e)
        {
            string _im = im.DisplayText;
            string _im1 = im1.DisplayText;
            string _prev_im = prev_im.DisplayText;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            var con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand($@" insert into spr_im (im) values('{im.DisplayText}')", con);
            string m1 = "Добавить новое значение в справочник?";
            string t1 = "Внимание!";
            int b1 = 2;
            Message me1 = new Message(m1, t1, b1);
            me1.ShowDialog();

            if (Vars.mes_res == 1)
            {
                con.Open();
                com.ExecuteNonQuery();
                con.Close();

                im.DataContext = MyReader.MySelect<FIO>(@"select fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
                im1.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
                prev_im.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
                im.Text = _im;
                im1.Text = _im1;
                prev_im.Text = _prev_im;
            }
            else
            {

            }
        }

        private void Ot_ProcessNewValue(DependencyObject sender, ProcessNewValueEventArgs e)
        {
            string _ot = ot.DisplayText;
            string _ot1 = ot1.DisplayText;
            string _prev_ot = prev_ot.DisplayText;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            var con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand($@" insert into spr_ot (ot) values('{ot.DisplayText}')", con);
            string m1 = "Добавить новое значение в справочник?";
            string t1 = "Внимание!";
            int b1 = 2;
            Message me1 = new Message(m1, t1, b1);
            me1.ShowDialog();

            if (Vars.mes_res == 1)
            {
                con.Open();
                com.ExecuteNonQuery();
                con.Close();

                ot.DataContext = MyReader.MySelect<FIO>(@"select fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
                ot1.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
                prev_ot.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
                ot.Text = _ot;
                ot1.Text = _ot1;
                prev_ot.Text = _prev_ot;
            }
            else
            {

            }

        }

        private void Fam_ProcessNewValue1(DependencyObject sender, ProcessNewValueEventArgs e)
        {
            string _fam = fam.DisplayText;
            string _fam1 = fam1.DisplayText;
            string _prev_fam = prev_fam.DisplayText;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            var con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand($@" insert into spr_fam (fam) values('{fam1.DisplayText}')", con);
            string m1 = "Добавить новое значение в справочник?";
            string t1 = "Внимание!";
            int b1 = 2;
            Message me1 = new Message(m1, t1, b1);
            me1.ShowDialog();

            if (Vars.mes_res == 1)
            {
                con.Open();
                com.ExecuteNonQuery();
                con.Close();

                fam.DataContext = MyReader.MySelect<FIO>(@"select fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
                fam1.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
                prev_fam.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
                fam.Text = _fam;
                fam1.Text = _fam1;
                prev_fam.Text = _prev_fam;
            }
            else
            {

            }
        }

        private void Im_ProcessNewValue1(DependencyObject sender, ProcessNewValueEventArgs e)
        {
            string _im = im.DisplayText;
            string _im1 = im1.DisplayText;
            string _prev_im = prev_im.DisplayText;

            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            var con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand($@" insert into spr_im (im) values('{im1.DisplayText}')", con);
            string m1 = "Добавить новое значение в справочник?";
            string t1 = "Внимание!";
            int b1 = 2;
            Message me1 = new Message(m1, t1, b1);
            me1.ShowDialog();

            if (Vars.mes_res == 1)
            {
                con.Open();
                com.ExecuteNonQuery();
                con.Close();

                im.DataContext = MyReader.MySelect<FIO>(@"select fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
                im1.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
                prev_im.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
                im.Text = _im;
                im1.Text = _im1;
                prev_im.Text = _prev_im;

            }
            else
            {

            }
        }

        private void Ot_ProcessNewValue1(DependencyObject sender, ProcessNewValueEventArgs e)
        {
            string _ot = ot.DisplayText;
            string _ot1 = ot1.DisplayText;
            string _prev_ot = prev_ot.DisplayText;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            var con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand($@" insert into spr_ot (ot) values('{ot1.DisplayText}')", con);
            string m1 = "Добавить новое значение в справочник?";
            string t1 = "Внимание!";
            int b1 = 2;
            Message me1 = new Message(m1, t1, b1);
            me1.ShowDialog();

            if (Vars.mes_res == 1)
            {
                con.Open();
                com.ExecuteNonQuery();
                con.Close();

                ot.DataContext = MyReader.MySelect<FIO>(@"select fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
                ot1.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
                prev_ot.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
                ot.Text = _ot;
                ot1.Text = _ot1;
                prev_ot.Text = _prev_ot;
            }
            else
            {

            }
        }

        private void Kem_vid_ProcessNewValue(DependencyObject sender, ProcessNewValueEventArgs e)
        {
            string _kem_vid = kem_vid.DisplayText;
            string _kem_vid1 = kem_vid1.DisplayText;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            var con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand($@" insert into spr_namevp (name) values('{kem_vid.DisplayText}')", con);
            string m1 = "Добавить новое значение в справочник?";
            string t1 = "Внимание!";
            int b1 = 2;
            Message me1 = new Message(m1, t1, b1);
            me1.ShowDialog();

            if (Vars.mes_res == 1)
            {
                con.Open();
                com.ExecuteNonQuery();
                con.Close();

                kem_vid.DataContext = MyReader.MySelect<NAME_VP>(@"select id,name from spr_namevp", Properties.Settings.Default.DocExchangeConnectionString);
                kem_vid1.DataContext = MyReader.MySelect<NAME_VP>(@"select id,name from spr_namevp", Properties.Settings.Default.DocExchangeConnectionString);
                kem_vid.Text = _kem_vid;
                kem_vid1.Text = _kem_vid1;
            }
            else
            {

            }
        }

        private void Num_blank_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Vars.mes_res = 10;
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                string strf2 = $@"where ps.spolis = '{
                         ser_blank.Text}' and  npolis LIKE '{
                         num_blank.Text}' order by ID desc";
                var peopleList =
                    MyReader.MySelect<People>(SPR.MyReader.load_pers_grid2 + strf2, connectionString);

                pers_grid_2.ItemsSource = peopleList;
                if (pers_grid_2.VisibleRowCount != 0)
                {
                    string m = "Бланк полиса с такими серией и номером уже есть в базе данных!";
                    string t = "Сообщение";
                    int b = 1;
                    Message me = new Message(m, t, b);
                    me.ShowDialog();

                }
            }
        }

        private void DXTabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            //Window_Loaded(this,e);
        }

        private void Ser_blank_GotFocus(object sender, RoutedEventArgs e)
        {
            Type_policy_LostFocus(this, e);
        }

        private void Agent_btn_Izmenit_Click(object sender, RoutedEventArgs e)
        {
            var ids = Funcs.MyIds(pers_grid.GetSelectedRowHandles(), pers_grid);
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);



            SqlCommand comm1 = new SqlCommand($@"UPDATE POL_EVENTS set AGENT={prz_Agent.EditValue} where IDGUID in(select idguid from pol_events e left join
                 POL_PRZ_AGENTS pa on e.agent = pa.id  where person_guid in(select idguid from pol_persons where id in ({ids})))
                UPDATE POL_OPLIST set przcod='{prz.EditValue.ToString()}' where EVENT_GUID in(select idguid from pol_events e left join POL_PRZ_AGENTS pa on e.agent = pa.id  where person_guid in (select idguid from pol_persons where id in({ids}) ))"// +
                                                                                                                                                                                                                                                         // $"insert into POL_OPLIST(PRZCOD,SMOCOD,EVENT_GUID,PERSON_GUID,FILENAME) VALUES ('{prz.EditValue.ToString()}',{smo},(select event_guid FROM POL_PERSONS WHERE ID=@id),(select IDGUID FROM POL_PERSONS WHERE ID=@id),null) "
                , con);

            //comm1.Parameters.AddWithValue("@id", ddd.ToString());
            ////  comm.Parameters.AddWithValue("@agent", prz_Agent.SelectedIndex.ToString());
            //comm1.Parameters.AddWithValue("@prz", prz.Text.Split(' ')[0]);
            con.Open();
            comm1.ExecuteNonQuery();
            con.Close();
            string m = "Пункт выдачи и агент успешно изменен!";
            string t = "Сообщение";
            int b = 1;
            Message me = new Message(m, t, b);
            me.ShowDialog();

            if (SPR.Premmissions == "User")
            {

                var peopleList = MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf_usr, connectionString);
                pers_grid.ItemsSource = peopleList;
                pers_grid.View.FocusedRowHandle = -1;

            }
            else
            {
                var peopleList = MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf_adm, connectionString);
                pers_grid.ItemsSource = peopleList;
                pers_grid.View.FocusedRowHandle = -1;
            }

        }

        private void MainTab_SelectionChanged(object sender, DevExpress.Xpf.Core.TabControlSelectionChangedEventArgs e)
        {

        }

        private void Ddeath_PreviewKeyUp(object sender, KeyEventArgs e)
        {

        }

        private void Date_vidachi_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                tabs.SelectedIndex = 3;
            }
        }

        private void W_main_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Inform_btn_Click(object sender, RoutedEventArgs e)
        {
            //string m = "Добавить информирование ЗЛ?";
            //string t = "Внимание!";
            //int b = 11;
            //Message me = new Message(m, t, b);
            //me.ShowDialog();
            //try
            //{
            //    Vars.IDSZ = Funcs.MyIds(pers_grid.GetSelectedRowHandles(), pers_grid);

            //}
            //catch
            //{

            //    string m1 = "Вы не выбрали ЗЛ для Информирования!";
            //    string t1 = "Внимание!";
            //    int b1 = 1;
            //    Message me1 = new Message(m1, t1, b1);
            //    me1.ShowDialog();
            //    return;
            //}

            //if (Vars.IDSZ == "")
            //{
            //    string m1 = "Вы не выбрали ЗЛ для Информирования!";
            //    string t1 = "Внимание!";
            //    int b1 = 1;
            //    Message me1 = new Message(m1, t1, b1);
            //    me1.ShowDialog();
            //    return;
            //}
            string call = "attache_history";
            Inform inf = new Inform(call);
            inf.ShowDialog();

        }

        private void Docser1_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {

        }

        private void W_main_Unloaded(object sender, RoutedEventArgs e)
        {
            //Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
            //        new Action(delegate ()
            //        {

            //                pers_grid.Columns.ForEach(x =>
            //                {
            //                    if (x.Tag != null)
            //                        x.Width = (GridColumnWidth)x.Tag;
            //                });


            //            Stream mStream = new MemoryStream();
            //            pers_grid.SaveLayoutToStream(mStream);
            //            mStream.Seek(0, SeekOrigin.Begin);
            //            StreamReader reader = new StreamReader(mStream);
            //            var first =  reader.ReadToEnd();                            

            //            mStream.Close();
            //            reader.Close();
            //            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            //            SqlConnection con = new SqlConnection(connectionString);
            //            SqlCommand comm = new SqlCommand($@"UPDATE auth set LayRTable={first}", con);
            //        }));

            //var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            //SqlConnection con = new SqlConnection(connectionString);
            //SqlCommand comm = new SqlCommand($@"UPDATE auth set LayRTable={first}", con);


        }

        private void TableView_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void Pers_grid_Unloaded(object sender, RoutedEventArgs e)
        {

        }



        private void W_main_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            pers_grid.FilterString = "";
            pers_grid_2.FilterString = "";
            G_layuot.save_Layout(Properties.Settings.Default.DocExchangeConnectionString, pers_grid, pers_grid_2);
        }
        //Stream Layout_InUse;
        //Stream Layout_InUse1;
        //string lrt;
        //string lrt1;



        private void W_main_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (fias.bomj.AllowUpdateTextBlockWhenPrinting == true && tabs.SelectedIndex == 3)
                {
                    tabs.SelectedIndex = 4;
                    fias1.sovp_addr.AllowUpdateTextBlockWhenPrinting = true;
                    fias.bomj.AllowUpdateTextBlockWhenPrinting = false;
                }
                else if (fias1.sovp_addr.AllowUpdateTextBlockWhenPrinting == true && tabs.SelectedIndex == 4)
                {
                    tabs.SelectedIndex = 5;
                    fias1.sovp_addr.AllowUpdateTextBlockWhenPrinting = false;
                }
                //else if (fam1.AllowUpdateTextBlockWhenPrinting == true && tabs.SelectedIndex == 4)
                //{
                //    pr_pod_z_polis.Focus();
                //    fam1.AllowUpdateTextBlockWhenPrinting = false;
                //}



            }
        }

        private void Pers_grid_AutoGeneratedColumns(object sender, RoutedEventArgs e)
        {
            G_layuot.layout_InUse(pers_grid, pers_grid_2);
            pers_grid.FilterString = "";
        }

        private void Pers_grid_2_AutoGeneratedColumns(object sender, RoutedEventArgs e)
        {
            G_layuot.layout_InUse(pers_grid, pers_grid_2);
            pers_grid_2.FilterString = "";
        }

        private void Dost1_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            string s1 = "";
            if (dost1.EditValue == null)
            {

            }
            else
            {
                var d1 = (ICollection)dost1.EditValue;
                string[] dd1 = new string[d1.Count];
                d1.CopyTo(dd1, 0);
                for (int i = 0; i < dd1.Count(); i++)
                {
                    s1 += (dd1[i] + ";");
                    s = s1.Substring(0, s1.Length - 1);
                }
            }
        }

        private void Fam_ProcessNewValue2(DependencyObject sender, ProcessNewValueEventArgs e)
        {
            string _fam = fam.DisplayText;
            string _fam1 = fam1.DisplayText;
            string _prev_fam = prev_fam.DisplayText;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            var con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand($@" insert into spr_fam (fam) values('{prev_fam.DisplayText}')", con);
            string m1 = "Добавить новое значение в справочник?";
            string t1 = "Внимание!";
            int b1 = 2;
            Message me1 = new Message(m1, t1, b1);
            me1.ShowDialog();

            if (Vars.mes_res == 1)
            {
                con.Open();
                com.ExecuteNonQuery();
                con.Close();

                fam.DataContext = MyReader.MySelect<FIO>(@"select fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
                fam1.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
                prev_fam.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
                fam.Text = _fam;
                fam1.Text = _fam1;
                prev_fam.Text = _prev_fam;
            }
            else
            {

            }
        }

        private void Im_ProcessNewValue2(DependencyObject sender, ProcessNewValueEventArgs e)
        {
            string _im = im.DisplayText;
            string _im1 = im1.DisplayText;
            string _prev_im = prev_im.DisplayText;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            var con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand($@" insert into spr_im (im) values('{prev_im.DisplayText}')", con);
            string m1 = "Добавить новое значение в справочник?";
            string t1 = "Внимание!";
            int b1 = 2;
            Message me1 = new Message(m1, t1, b1);
            me1.ShowDialog();

            if (Vars.mes_res == 1)
            {
                con.Open();
                com.ExecuteNonQuery();
                con.Close();

                im.DataContext = MyReader.MySelect<FIO>(@"select fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
                im1.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
                prev_im.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
                im.Text = _im;
                im1.Text = _im1;
                prev_im.Text = _prev_im;
            }
            else
            {

            }
        }

        private void Ot_ProcessNewValue2(DependencyObject sender, ProcessNewValueEventArgs e)
        {
            string _ot = ot.DisplayText;
            string _ot1 = ot1.DisplayText;
            string _prev_ot = prev_ot.DisplayText;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            var con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand($@" insert into spr_ot (ot) values('{prev_ot.DisplayText}')", con);
            string m1 = "Добавить новое значение в справочник?";
            string t1 = "Внимание!";
            int b1 = 2;
            Message me1 = new Message(m1, t1, b1);
            me1.ShowDialog();

            if (Vars.mes_res == 1)
            {
                con.Open();
                com.ExecuteNonQuery();
                con.Close();

                ot.DataContext = MyReader.MySelect<FIO>(@"select fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
                ot1.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
                prev_ot.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
                ot.Text = _ot;
                ot1.Text = _ot1;
                prev_ot.Text = _prev_ot;
            }
            else
            {

            }
        }

        private void TableView_CellValueChanging(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {

        }

        private void TableView_PreviewKeyUp(object sender, KeyEventArgs e)
        {

        }

        private void Pers_grid_KeyDown(object sender, KeyEventArgs e)
        {
            //var kkk = e.Key;
            //if (e.Key != Key.Back && e.Key != Key.Space && e.Key != Key.Delete && !e.Key.ToString().Contains("NumPad") &&  e.Key != Key.LeftCtrl && e.Key != Key.RightCtrl &&
            //    e.Key != Key.LeftAlt && e.Key != Key.RightAlt && e.Key != Key.LeftShift && e.Key != Key.RightShift && e.Key != Key.Tab && e.Key != Key.System)
            //{
            //    if(e.Key.ToString().StartsWith("D") && e.Key.ToString().Length == 2)
            //    {
            //        e.Handled = true;
            //        (sender as GridControl).CurrentColumn.AutoFilterValue += SPR.Translit(Convert.ToString(e.Key.ToString().Last()));
            //    }
            //    else
            //    {
            //        e.Handled = true;
            //        (sender as GridControl).CurrentColumn.AutoFilterValue += SPR.Translit(e.Key.ToString());
            //    }

            //}
            //var codeS = Convert.ToInt32(Convert.ToChar(e.Key.ToString().First()));
            //if (codeS>43 &&codeS<97 && !char.IsDigit(e.Key.ToString().First()))
            //{
            //    e.Handled = true;
            //    (sender as GridControl).CurrentColumn.AutoFilterValue += SPR.Translit(e.Key.ToString());
            //}

            //else
            //{
            //    e.Handled = false;
            //}

        }

        private void Pers_grid_GotFocus(object sender, RoutedEventArgs e)
        {
            changeKeyBoard();
        }

        private void Doc_ser_LostFocus(object sender, RoutedEventArgs e)
        {
            changeKeyBoard();
        }

        private void Pers_grid_2_GotFocus(object sender, RoutedEventArgs e)
        {
            changeKeyBoard();
        }

        private void Unload_history_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Vars.IDSZ = Funcs.MyIds(pers_grid.GetSelectedRowHandles(), pers_grid);
            string call = "unload_history";
            Inform inf = new Inform(call);
            inf.ShowDialog();
        }

        private void Del_file_btn_Click(object sender, RoutedEventArgs e)
        {
            string call = "unload_files";
            Inform inf = new Inform(call);
            inf.ShowDialog();
        }

        private void Person_history_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Vars.IDSZ = Funcs.MyIds(pers_grid.GetSelectedRowHandles(), pers_grid);
            string call = "person_history";
            Inform inf = new Inform(call);
            inf.ShowDialog();
        }

        private void Only_stop_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            var peopleList =
                MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + " where st.namewithkod is not null", connectionString);

            pers_grid.ItemsSource = peopleList;
        }

        private void Excel_file_btn_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog SF = new SaveFileDialog();
            SF.DefaultExt = ".xls";
            SF.Filter = "Файлы Excel (.xls; .xlsx)|*.xls;*.xlsx";
            bool res = SF.ShowDialog().Value;

            //OpenFileDialog OF = new OpenFileDialog();
            //OF.DefaultExt = ".dbf";
            //OF.Filter = "Файлы DBF (.dbf)|*.dbf";
            //bool res = OF.ShowDialog().Value;
            if (res == true)
            {
                //string dbffile = OF.FileName;
                //using (Stream fos = File.Open(dbffile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                //{
                //    //using (var writer = new DotNetDBF.DBFWriter())
                //    //{
                //    //    writer.CharEncoding = Encoding.GetEncoding(866);
                //    //    writer.Signature = DotNetDBF.DBFSignature.DBase3;
                //    //    writer.LanguageDriver = 0x26; // кодировка 866
                //    //    var field1 = new DotNetDBF.DBFField("DOCDATE", DotNetDBF.NativeDbType.Date);
                //    //    var field2 = new DotNetDBF.DBFField("DOCNUMBER", DotNetDBF.NativeDbType.Char, 10);
                //    //    var field3 = new DotNetDBF.DBFField("DOCSER", DotNetDBF.NativeDbType.Char, 8);
                //    //    var field4 = new DotNetDBF.DBFField("DOCTYPE", DotNetDBF.NativeDbType.Numeric, 2, 0);
                //    //    //var field5 = new DotNetDBF.DBFField("RETPRICE", DotNetDBF.NativeDbType.Numeric, 10, 2);
                //    //    //var field6 = new DotNetDBF.DBFField("QUANTITY", DotNetDBF.NativeDbType.Numeric, 3, 2);
                //    //    //var field7 = new DotNetDBF.DBFField("APCODE", DotNetDBF.NativeDbType.Numeric, 10, 0);
                //    //    //var field8 = new DotNetDBF.DBFField("CLNTNAME", DotNetDBF.NativeDbType.Char, 255);
                //    //    //var field9 = new DotNetDBF.DBFField("CLNTPHONE", DotNetDBF.NativeDbType.Char, 20);

                //    //    writer.Fields = new[] { field1, field2, field3, field4 };//, field5, field6, field7, field8, field9 };
                //    //    var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                //    //    var docList =
                //    //        MyReader.MySelect<DOCUMENTS>($@"select top(100) docdate,docnum,docser,doctype from pol_documents", connectionString);
                //    //    for (int i = 0; i < docList.Count; i++)
                //    //    {
                //    //        writer.AddRecord(docList[i].DOCDATE, docList[i].DOCNUM, docList[i].DOCSER, docList[i].DOCTYPE
                //    //           // добавляем поля в набор
                //    //           );
                //    //    }


                //    //    writer.Write(fos);
                //    //}


                //    var dbf = new DotNetDBF.DBFReader(fos);
                //    DataTable dt = new DataTable();

                //    var fields = dbf.Fields;
                //    var result = (from s in fields select s.Name).ToArray();
                //    dt.Load(dbf.NextRecord());
                //    var rtt = dbf.NextRecord();
                //    dt.LoadDataRow(rtt,false);
                //    dbf.SetSelectFields("F1", "F3");



                //}


                string fname = SF.FileName;
                pers_grid.View.ExportToXls(fname);

                //pers_grid.View.ShowPrintPreview(this);
                string m = $@"Файл успешно сохранен и находится в папке:
                {SF.FileName.Replace(SF.SafeFileName, "")}";
                string t = "Сообщение";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();


            }
        }

        private void Del_zl_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SPR.Premmissions == "User")
            {
                string m0 = "У вас недостаточно прав доступа для данной операции!";
                string t0 = "Ошибка!";
                int b0 = 1;
                Message me0 = new Message(m0, t0, b0);
                me0.ShowDialog();
                return;
            }

            string sg_rows_e = " ";
            string sg_rows_zl = " ";
            int[] rt = pers_grid.GetSelectedRowHandles();
            for (int i = 0; i < rt.Count(); i++)

            {
                var ddd_e = pers_grid.GetCellValue(rt[i], "EVENT_ID");
                var sgr_e = sg_rows_e.Insert(sg_rows_e.Length, ddd_e.ToString()) + ",";
                sg_rows_e = sgr_e;
                var ddd_zl = pers_grid.GetCellValue(rt[i], "ID");
                var sgr_zl = sg_rows_zl.Insert(sg_rows_zl.Length, ddd_zl.ToString()) + ",";
                sg_rows_zl = sgr_zl;
            }

            sg_rows_e = sg_rows_e.Substring(0, sg_rows_e.Length - 1);
            sg_rows_zl = sg_rows_zl.Substring(0, sg_rows_zl.Length - 1);

            string m = "Вы действительно полностью удалить записи по ЗЛ?";
            string t = "Внимание!";
            int b = 2;
            Message me = new Message(m, t, b);
            me.ShowDialog();

            //var result = MessageBox.Show("Вы действительно хотите удалить записи", "Внимание!", MessageBoxButton.YesNo);
            if (Vars.mes_res != 1)
            {
                return;
            }
            else
            {
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand comm = new SqlCommand($@"
delete from POL_PERSONS_OLD where PERSON_GUID in(select IDGUID from POL_PERSONS where id in({sg_rows_zl}))
delete from POL_DOCUMENTS_OLD where PERSON_GUID in(select IDGUID from POL_PERSONS where id in({sg_rows_zl}))
delete from POL_POLISES where PERSON_GUID in(select IDGUID from POL_PERSONS where id in({sg_rows_zl}))
delete from POL_RELATION_DOC_PERS where PERSON_GUID in(select IDGUID from POL_PERSONS where id in({sg_rows_zl}))
delete from POL_DOCUMENTS where PERSON_GUID in(select IDGUID from POL_PERSONS where id in({sg_rows_zl}))
delete from POL_RELATION_ADDR_PERS where PERSON_GUID in(select IDGUID from POL_PERSONS where id in({sg_rows_zl}))
delete from POL_ADDRESSES where EVENT_GUID in (select IDGUID from POL_EVENTS where PERSON_GUID in(select IDGUID from POL_PERSONS where id in({sg_rows_zl})))
delete from POL_PERSONSB where PERSON_GUID in(select IDGUID from POL_PERSONS where id in({sg_rows_zl}))
delete from POL_OPLIST where PERSON_GUID in(select IDGUID from POL_PERSONS where id in({sg_rows_zl}))
delete from POL_EVENTS where PERSON_GUID in(select IDGUID from POL_PERSONS where id in({sg_rows_zl}))
delete from POL_PERSONS where id in({sg_rows_zl})", con);
                con.Open();
                comm.ExecuteNonQuery();
                con.Close();
                int kol_zap = rt.Count();
                int lastnumber = kol_zap % 10;
                if (lastnumber > 1 && lastnumber < 5)
                {
                    string m1 = " Успешно удалено  " + rt.Count() + " записи!";
                    string t1 = "Сообщение";
                    int b1 = 1;
                    Message me1 = new Message(m1, t1, b1);
                    me1.ShowDialog();
                    //MessageBox.Show(" Успешно удалено  " + rt.Count() + " записи!");
                }
                else if (lastnumber == 1)
                {
                    string m1 = " Успешно удалено  " + rt.Count() + " запись!";
                    string t1 = "Сообщение";
                    int b1 = 1;
                    Message me1 = new Message(m1, t1, b1);
                    me1.ShowDialog();
                    //MessageBox.Show(" Успешно удалена  " + rt.Count() + " запись!");
                }
                else
                {
                    string m1 = " Успешно удалено  " + rt.Count() + " записей!";
                    string t1 = "Сообщение";
                    int b1 = 1;
                    Message me1 = new Message(m1, t1, b1);
                    me1.ShowDialog();
                    //MessageBox.Show(" Успешно удалено  " + rt.Count() + " записей!");
                }

                pers_grid_Loaded(sender, e);
            }
        }

        private void Pd_p_zl_GotFocus(object sender, RoutedEventArgs e)
        {

            //fam1.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
            //im1.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
            //ot1.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);

        }

        private void Prev_doc_GotFocus(object sender, RoutedEventArgs e)
        {
            //prev_fam.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
            //prev_im.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
            //prev_ot.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {

            string m = "Вы действительно хотите очистить все поля?";
            string t = "Внимание!";
            int b = 2;
            Message me = new Message(m, t, b);
            me.ShowDialog();

            //var result = MessageBox.Show("Вы действительно хотите удалить записи", "Внимание!", MessageBoxButton.YesNo);
            if (Vars.mes_res != 1)
            {
                return;
            }
            else
            {
                InsMethods.PersData_Default(this);
            }
        }

        private void Fam1_GotFocus(object sender, RoutedEventArgs e)
        {
            //fam1.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
            //im1.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
            //ot1.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
        }

        private void Fakt_prekr_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                tabs.SelectedIndex = 3;
            }
        }
        ProcessNewValueEventArgs env;
        private void Fam_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //var vvv = fam.DataContext;
            //int ecount = 0;

            //if (ecount == 0)
            //{
            //    if (e.Key==Key.Enter)
            //    {

            //        if (fio_col.Exists(x => x.FAM == fam.Text) == false)
            //        {
            //            Yamed.Control.EnterKeyTraversal.SetIsEnabled(w_main, false);
            //            Fam_ProcessNewValue(this, env);                        
            //            ecount = 1;
            //        }
            //        else
            //        {
            //            Yamed.Control.EnterKeyTraversal.SetIsEnabled(w_main, true);
            //            ecount = 0;
            //        }

            //    }


            //}

            //Tab_ZL.Tag = "IgnoreEnterKeyTraversal";
        }

        private void Develop_show(object sender, RoutedEventArgs e)
        {
            Developer dv = new Developer();
            dv.Show();
        }
        private void TestBarnaul(object sender, RoutedEventArgs e)
        {
            string fname1 = $@"IS{Vars.SMO}T{Vars.SMO.Substring(0, 2)}_{DateTime.Today.Year.ToString().Substring(0, 2) + (DateTime.Today.Month.ToString().Length < 2 ? "0" + DateTime.Today.Month.ToString() : DateTime.Today.Month.ToString())}1.csv";
            SaveFileDialog SF = new SaveFileDialog();
            SF.FileName = fname1;
            SF.DefaultExt = ".zip";
            SF.Filter = "Файлы zip (.zip)|*.zip";
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
                string fnamezip = fname.Replace(".csv", "");
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddFile(fname, "");

                    zip.Save(fnamezip + ".zip");

                }
                
                File.Delete(fname);
                string m = "Файл " + fname + " успешно сохранен!";
                string t = "Сообщение!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();

            }
        }
        private DataTable tb;
        private void TestBarnaul1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OF = new OpenFileDialog();

            OF.Multiselect = false;
            bool res = OF.ShowDialog().Value;
            string ex_path = OF.FileName;
            if (res == true)
            {
                ZipFile zip = new ZipFile(ex_path);
                zip.ExtractAll(ex_path.Replace(OF.SafeFileName, ""), ExtractExistingFileAction.OverwriteSilently);
                ex_path = ex_path.Replace(OF.SafeFileName, zip.EntryFileNames.First());

                
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

                            var cls0 = attache[0].Split('|');
                            for (int i = 0; i < cls0.Count(); i++)
                            {
                                tb.Columns.Add("Column" + i.ToString(), typeof(string));
                            }

                            foreach (string row in attache)
                            {
                            // получаем все ячейки строки

                            var cls = row.Split('|');

                                tb.LoadDataRow(cls, LoadOption.Upsert);

                            }
                        }

                    

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
                File.Delete(ex_path);
                string m = "Прикрепление успешно загружено!";
                string t = "Сообщение!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();

            }
        }

        private void To_pers()
        {
            tabs.SelectedIndex = 1;
        }

        private void Pr_pod_z_smo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //To_pers();
        }

        private void Pr_pod_z_smo_PopupClosed(object sender, ClosePopupEventArgs e)
        {
            To_pers();
        }

        private void Del_rper_btn_Click(object sender, RoutedEventArgs e)
        {
            string m1 = "Вы действительно хотите удалить сведения о представителе ЗЛ?";
            string t1 = "Внимание!";
            int b1 = 2;
            Message me1 = new Message(m1, t1, b1);
            me1.ShowDialog();

            if (Vars.mes_res == 1)
            {
                fam1.Text = "";
                im1.Text = "";
                ot1.Text = "";
                pol_pr.EditValue = null;
                dr1.EditValue = null;
                phone_p1.Text = "";
                doctype1.EditValue = null;
                docser1.Text = "";
                docnum1.Text = "";
                docdate1.EditValue = null;
                status_p2.SelectedIndex = -1;
                srok_doverenosti.EditValue = null;
                rper_load = 0;
                rper = Guid.Empty;
            }
            else
            {
                return;
            }
            
        }

        private void Load_flk_zl_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            OpenFileDialog OPF = new OpenFileDialog();

            OPF.Multiselect = true;
            bool res = OPF.ShowDialog().Value;
            string[] files = OPF.FileNames;
            string[] shfiles = OPF.SafeFileNames;
            int ev_id = (int)pers_grid.GetCellValue(pers_grid.GetSelectedRowHandles()[0],"EVENT_ID");
            int y = 0;
            if (res == true)
            {
                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);

                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(file);
                    string rfile = shfiles[y].Replace("P", "i");
                    if (rfile.Length > 22)
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
 where event_guid in (select event_guid from pol_events where id='{ev_id}') 
 update pol_events set unload=0  where id='{ev_id}')
 update pol_unload_history set comment ='{xRoot.Attributes.GetNamedItem("COMMENT").Value.Replace((char)39, (char)32)}'
 where  fname='{rfile}' and event_guid in(select event_guid from pol_events where id='{ev_id}')", con);
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
                        SqlCommand com = new SqlCommand($@"exec [Load_flk_zl] @xml = '{xDoc.LastChild.OuterXml.Replace((char)39, (char)32)}', @fname='{rfile}', @ev_id={ev_id}", con);
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
        }

        private void Load_OK_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            
            string sg_rows_zl = " ";
            int[] rt = pers_grid.GetSelectedRowHandles();
            for (int i = 0; i < rt.Count(); i++)
            {                
                var ddd_zl = pers_grid.GetCellValue(rt[i], "ID");
                var sgr_zl = sg_rows_zl.Insert(sg_rows_zl.Length, ddd_zl.ToString()) + ",";
                sg_rows_zl = sgr_zl;
            }
            sg_rows_zl = sg_rows_zl.Substring(0, sg_rows_zl.Length - 1);
            string m1 = "Вы действительно хотите проставить комментарий 'ОК!' выбранным ЗЛ?";
            string t1 = "Внимание!";
            int b1 = 2;
            Message me1 = new Message(m1, t1, b1);
            me1.ShowDialog();

            if (Vars.mes_res == 1)
            {
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand comm = new SqlCommand($@"update pol_persons set comment='ОК!' where id in({sg_rows_zl})
                update pol_events set unload=1 where idguid in(select event_guid from pol_persons where id in({sg_rows_zl}))", con);
                con.Open();
                comm.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                return;
            }
        }

        private void Obnovit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            G_layuot.save_Layout(Properties.Settings.Default.DocExchangeConnectionString, pers_grid, pers_grid_2);
            G_layuot.restore_Layout(Properties.Settings.Default.DocExchangeConnectionString, pers_grid, pers_grid_2);
            if (SPR.Premmissions == "User")
            {

                var peopleList = MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf_usr, Properties.Settings.Default.DocExchangeConnectionString);
                pers_grid.ItemsSource = peopleList;
                pers_grid.View.FocusedRowHandle = -1;

            }
            else
            {
                var peopleList = MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + strf_adm, Properties.Settings.Default.DocExchangeConnectionString);
                pers_grid.ItemsSource = peopleList;
                pers_grid.View.FocusedRowHandle = -1;
            }

        }

        private void Otmena_pogash_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            string sg_rows_zl = " ";
            int[] rt = pers_grid.GetSelectedRowHandles();
            for (int i = 0; i < rt.Count(); i++)
            {
                var ddd_zl = pers_grid.GetCellValue(rt[i], "ID");
                var sgr_zl = sg_rows_zl.Insert(sg_rows_zl.Length, ddd_zl.ToString()) + ",";
                sg_rows_zl = sgr_zl;
            }
            sg_rows_zl = sg_rows_zl.Substring(0, sg_rows_zl.Length - 1);
            string m1 = "Вы действительно хотите отменить погашение выбранным ЗЛ?";
            string t1 = "Внимание!";
            int b1 = 2;
            Message me1 = new Message(m1, t1, b1);
            me1.ShowDialog();

            if (Vars.mes_res == 1)
            {
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand comm = new SqlCommand($@"update pol_persons set comment=null, active=1  where id in({sg_rows_zl})
                update POL_POLISES  set STOP_REASON=null,DEND=null,DSTOP=null  where PERSON_GUID in(select idguid from pol_persons where id in({sg_rows_zl}))", con);
                con.Open();
                comm.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                return;
            }
        }


        //        private void Reload_tick(object sender, EventArgs e)
        //        {

        //            //Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
        //            //    new Action(delegate ()
        //            //    {
        //            //        erp1 = MyReader.MySelect<PEOPLES>($@"select top(20)* from pol_persons order by id desc", Properties.Settings.Default.DocExchangeConnectionString);
        //            //        foreach (var item in erp1)
        //            //        {
        //            //            if (erp.Where(c => c.ID == item.ID).Count() == 0)
        //            //            {
        //            //                erp.Add(item);
        //            //            }
        //            //        }
        //            //        //pol_zagr.RefreshData();
        //            //Add_btn_Click(this,re);
        //            //}));

        //            Thread thread = new Thread(spisok_up);

        //            thread.Start();

        //            //pol_zagr.RefreshData();
        //        }
        //        public static List<Events> ev;
        //        public static List<Events> ev1;
        //        private void spisok_up()
        //        {
        //            string com = $@"SELECT top(1000) sss.id , pp.SROKDOVERENOSTI, pp.ID, pp.ACTIVE, op.przcod, pe.UNLOAD, pp.ENP, pp.FAM, pp.IM, pp.OT, pp.W,pp. DR, MO, oks.CAPTION as C_OKSM, r.NameWithID , pp.COMMENT, pe.DVIZIT, pp.DATEVIDACHI, pp.PRIZNAKVIDACHI,
        //            pp.SS, ps.VPOLIS, ps.SPOLIS, ps.NPOLIS, ps.DBEG, ps.DEND, ps.DSTOP, BLANK, ps.DRECEIVED, f.NameWithId as MO_NameWithId , op.filename, pp.phone, p.AGENT, pp.CYCLE, (select pr.namewithid  from POL_PERSONS_INFORM pin
        //      left join PRICHINA_INFORMIROVANIYA pr
        //      on pin.PRICHINA_INFORM = pr.ID
        //where PERSON_ID = pp.ID and pin.id = (select max(id) from POL_PERSONS_INFORM where PERSON_ID = pp.ID)) as inform, st.namewithkod as STOP_REASON, pe.id as EVENT_ID
        //              FROM POL_PERSONS pp left join
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

        //            on pp.C_OKSM = oks.A3
        //			left join @t sss
        //			on sss.ID=pp.ID
        //			where sss.id is null";
        //            //string com = $@"select * from  @t order by id desc";

        //            try
        //            {
        //                ev1 = MyReader.MySelect<Events>(com, Properties.Settings.Default.DocExchangeConnectionString, ev);
        //            }
        //            catch (Exception e)
        //            {

        //                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
        //                (ThreadStart)delegate ()
        //                {
        //                    string t0 = "Ошибка!";
        //                    int b0 = 1;
        //                    Message me0 = new Message(e.Message, t0, b0);
        //                    me0.ShowDialog();
        //                }
        //                );
        //                return;
        //            }

        //            ev.AddRange(ev1);
        //            //foreach (var item in erp1)
        //            //{
        //            //    if (erp.Where(c => c.ID == item.ID).Count() == 0)
        //            //    {
        //            //        erp.Add(item);
        //            //    }
        //            //}
        //            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
        //                (ThreadStart)delegate ()
        //                {

        //                    pers_grid.RefreshData();
        //                }
        //            );

        //        }

    }
}

