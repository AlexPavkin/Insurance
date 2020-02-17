using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Win32;
using SharpCompress.Archives;
using SharpCompress.Common;
using Insurance_SPR;



namespace Insurance
{
    class WorkDBF
    {
        private OdbcConnection Conn = null;
        public DataTable Execute(string Command)
        {
            DataTable dt = null;
            if (Conn != null)
            {
                try
                {
                    Conn.Open();
                    dt = new DataTable();
                    System.Data.Odbc.OdbcCommand oCmd = Conn.CreateCommand();
                    oCmd.CommandText = Command;
                    dt.Load(oCmd.ExecuteReader());
                    Conn.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            return dt;
        }
        public DataTable GetAll(string DB_path)
        {
            return Execute("SELECT * FROM " + DB_path);
        }

        public WorkDBF()
        {
            this.Conn = new System.Data.Odbc.OdbcConnection();
            Conn.ConnectionString = @"Driver={Microsoft dBase  Driver (*.dbf)};" +
                   "SourceType=DBF;Exclusive=No;" +
                   "Collate=Machine;NULL=NO;DELETED=NO;" +
                   "BACKGROUNDFETCH=NO;";
        }
    }
    /// <summary>
    /// Логика взаимодействия для FIAS_UPD.xaml
    /// </summary>
    public partial class FIAS_UPD : Window
    {
        public delegate void ThreadStart(string[] reg);
        public delegate void MyDelegate();
        public delegate void MyProgress(double i);
        public FIAS_UPD()
        {

            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        private void Progress()
        {
            PBar.Visibility = Visibility.Visible;
        }
        private void ProgressEnd()
        {
            PBar.Visibility = Visibility.Hidden;
        }
        private void ProgressCount(double i)
        {
            PBar.Value = i;
        }

        public string regions;
        private void Fias_upd_Click(object sender, RoutedEventArgs e)
        {
            DevExpress.Xpf.Editors.ProgressBarEdit pb = PBar;
            regions = Region.Text;
            if (Region_check.IsChecked == false)
            {
                bool ch = false;
                var r = Region.Text.Replace(" ", "").Split(',');
                string m = "Вы действительно хотите обновить справочник по всем регионам?";
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

                    //LoadingDecorator1.IsSplashScreenShown = true;
                    CopyToSQL_ADDROB(r, ch);
                    //LoadingDecorator1.IsSplashScreenShown = false;

                }
            }
            else
            {
                bool ch = true;
                var r = Region.Text.Replace(" ", "").Split(',');
                string m = $@"Вы действительно хотите обновить справочник по регионам: {Region.Text} ?";
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
                    Thread t1 = new Thread(delegate () { CopyToSQL_ADDROB(r, ch); });
                    t1.Start();

                    //CopyToSQL_ADDROB(r);


                    //Close();
                }
            }


            void CopyToSQL_ADDROB(string[] Region, bool chk)
            {
                OpenFileDialog opf = new OpenFileDialog();

                opf.InitialDirectory = "c:\\";
                opf.ShowDialog();

                string f1 = opf.FileName;
                string f = opf.SafeFileName;
                string f2 = f1.Replace(f, "");

                Dispatcher.BeginInvoke(new MyDelegate(Progress));
                //LoadingDecorator1.IsSplashScreenShown = true;
                string ConnectionString2 = Properties.Settings.Default.FIASConnectionString;
                var fff = ConnectionString2.Split(';');
                //var fias_cat = fff[1].Replace("Initial Catalog=","");
                
                SqlConnection connSQL1 = new SqlConnection(ConnectionString2);
                var fias_cat = connSQL1.Database;


                IEnumerable<SharpCompress.Archives.Rar.RarArchiveEntry> entrs=null;
                //Directory.GetFiles(f2);
                // List<string> files = Directory.GetFiles(f2, "*.DBF").ToList<string>();
                double j = 0;
                if (chk == true)
                {
                    //string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + f2 + ";Extended Properties=dBASE IV;";
                    //string ConnectionString = @"Provider=VFPOLEDB.1;Data Source=" + f2 + ";";
                    string ConnectionString1 = Properties.Settings.Default.FIASConnectionString;

                    SqlConnection connSQL = new SqlConnection(ConnectionString1);
                    //files = Directory.GetFiles(f2, "ADDROB" + Region + "*.DBF");
                    var archive = SharpCompress.Archives.Rar.RarArchive.Open(opf.FileName);
                    entrs = archive.Entries.Where(x=>x.Key.StartsWith("ADDROB")==true||x.Key.StartsWith("HOUSE")==true);
                    foreach (var entry in entrs)
                    {
                        j=j+(50.00/entrs.Count());
                        Thread.Sleep(50);
                        //Progressing pr = new Progressing(Progress);
                        
                        Dispatcher.BeginInvoke(new MyProgress(ProgressCount),Math.Round(j,0,MidpointRounding.AwayFromZero));
                        for (int i = 0; i < Region.Count(); i++)
                        {

                            if (entry.Key == "ADDROB" + Region[i] + ".DBF")
                            {

                                entry.WriteToDirectory(opf.FileName.Replace("\\" + opf.SafeFileName, ""), new ExtractionOptions());
                                try
                                {
                                    connSQL1.Open();
                                    using (var command = new SqlCommand($@" DELETE FROM Houses where AOGUID in (select AOGUID from AddressObjects where REGIONCODE={Region[i]}) ; 
                                                                            DELETE FROM AddressObjects where REGIONCODE={Region[i]};", connSQL1)) //DBCC CHECKIDENT (ADDROB, RESEED, 0); DBCC CHECKIDENT(AddressObjects, RESEED, 0);
                                    {
                                        command.CommandTimeout = 0;
                                        command.ExecuteNonQuery();
                                    }
                                    connSQL1.Close();

                                }
                                catch
                                {

                                }

                                DataTable dt = new DataTable();
                                string dbffile = opf.FileName.Replace(opf.SafeFileName, "ADDROB" + Region[i] + ".DBF");
                                using (Stream fos = File.Open(dbffile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                {
                                    var dbf = new DotNetDBF.DBFReader(fos);
                                    dbf.CharEncoding = Encoding.GetEncoding(866);

                                    var cnt = dbf.RecordCount;



                                    var fields = dbf.Fields;
                                    for (int ii = 0; ii < fields.Count(); ii++)
                                    {
                                        DataColumn workCol = dt.Columns.Add(fields[ii].Name, fields[ii].Type);
                                        workCol.AllowDBNull = true;
                                        workCol.DefaultValue = DBNull.Value;
                                    }

                                    //var result = (from s in fields select s.Name).ToArray();
                                    //dt.Load(dbf.NextRecord());
                                    for (int ii = 0; ii < dbf.RecordCount; ii++)
                                    {
                                        var rtt = dbf.NextRecord();
                                        
                                        if (rtt != null)
                                        {
                                            for (i = 0; i < rtt.Count(); i++)
                                            {
                                                if (rtt[i].ToString() == "")
                                                {
                                                    rtt[i] = null;
                                                }
                                            }
                                            dt.LoadDataRow(rtt, true);
                                            
                                        }

                                    }
                                    


                                    try
                                    {
                                        //                                        string command = $@" insert into AddressObjects ([AOGUID],[FORMALNAME],[REGIONCODE],[AUTOCODE],[AREACODE],[CITYCODE],[CTARCODE],[PLACECODE],
                                        //[STREETCODE],[EXTRCODE],[SEXTCODE],[OFFNAME],[POSTALCODE],[IFNSFL],[TERRIFNSFL],[IFNSUL],[TERRIFNSUL],[OKATO],[OKTMO],[UPDATEDATE],[SHORTNAME],[AOLEVEL],[PARENTGUID],[AOID],[PREVID],
                                        //[NEXTID],[CODE],[PLAINCODE],[ACTSTATUS],[CENTSTATUS],[OPERSTATUS],[CURRSTATUS],[STARTDATE],[ENDDATE],[NORMDOC],[LIVESTATUS])  
                                        //select CAST([AOGUID] as UNIQUEIDENTIFIER),[FORMALNAME],[REGIONCODE],[AUTOCODE],[AREACODE],[CITYCODE],[CTARCODE],[PLACECODE],[STREETCODE],[EXTRCODE],[SEXTCODE],[OFFNAME],[POSTALCODE],
                                        //[IFNSFL],[TERRIFNSFL],[IFNSUL],[TERRIFNSUL],[OKATO],[OKTMO],[UPDATEDATE],[SHORTNAME],[AOLEVEL],CAST([PARENTGUID] as uniqueidentifier),CAST([AOID] as uniqueidentifier),
                                        //cast([PREVID] as UNIQUEIDENTIFIER),CAST([NEXTID] as uniqueidentifier),[CODE],[PLAINCODE],[ACTSTATUS],[CENTSTATUS],[OPERSTATUS],[CURRSTATUS],[STARTDATE],[ENDDATE],
                                        //cast([NORMDOC] as uniqueidentifier),[LIVESTATUS]  from @t ";
                                        
                                        string command = $@" insert into AddressObjects ([AOGUID],[FORMALNAME],[REGIONCODE],[AUTOCODE],[AREACODE],[CITYCODE],[CTARCODE],[PLACECODE],
                                        [STREETCODE],[EXTRCODE],[SEXTCODE],[OFFNAME],[POSTALCODE],[IFNSFL],[TERRIFNSFL],[IFNSUL],[TERRIFNSUL],[OKATO],[OKTMO],[UPDATEDATE],[SHORTNAME],[AOLEVEL],[PARENTGUID],[AOID],[PREVID],
                                        [NEXTID],[CODE],[PLAINCODE],[ACTSTATUS],[CENTSTATUS],[OPERSTATUS],[CURRSTATUS],[STARTDATE],[ENDDATE],[NORMDOC],[LIVESTATUS])  
                                        select CAST([AOGUID] as UNIQUEIDENTIFIER),[FORMALNAME],[REGIONCODE],[AUTOCODE],[AREACODE],[CITYCODE],[CTARCODE],[PLACECODE],[STREETCODE],[EXTRCODE],[SEXTCODE],[OFFNAME],[POSTALCODE],
                                        [IFNSFL],[TERRIFNSFL],[IFNSUL],[TERRIFNSUL],[OKATO],[OKTMO],[UPDATEDATE],[SHORTNAME],[AOLEVEL],CAST([PARENTGUID] as uniqueidentifier),CAST([AOID] as uniqueidentifier),
                                        cast([PREVID] as UNIQUEIDENTIFIER),CAST([NEXTID] as uniqueidentifier),[CODE],[PLAINCODE],[ACTSTATUS],[CENTSTATUS],[OPERSTATUS],[CURRSTATUS],[STARTDATE],[ENDDATE],
                                        cast([NORMDOC] as uniqueidentifier),[LIVESTATUS] from @dt ";
                                        MyReader.UpdateFromTable<DataTable>(command, ConnectionString1, dt);
                                    }
                                    catch
                                    {
                                    }

                                }
                                try
                                {

                                    File.Delete(opf.FileName.Replace(opf.SafeFileName, "") + entry.Key);

                                }
                                catch
                                {
                                }
                                //if (i== Region.Count() - 1 && entry.Key == "ADDROB" + Region[Region.Count() - 1] + ".DBF")
                                if (j >= 50)
                                {
                                    goto forward;
                                }
                            }

                            connSQL.Close();


                        }
                    }
                    
                }

                else
                {
                    //files = Directory.GetFiles(f2, "ADDROB*.DBF"); //Directory.GetFiles(f2);
                    DataTable dt = new DataTable();
                    // string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + f2 + ";Extended Properties=dBASE IV;";
                    //string ConnectionString = @"Provider=VFPOLEDB.1;Data Source=" + f2 + ";";
                    //string ConnectionString1 = Properties.Settings.Default.fias;
                    string ConnectionString1 = Properties.Settings.Default.FIASConnectionString;
                    //OleDbConnection conn = new OleDbConnection(ConnectionString);
                    SqlConnection connSQL = new SqlConnection(ConnectionString1);
                    var archive = SharpCompress.Archives.Rar.RarArchive.Open(opf.FileName);
                    foreach (var entry in archive.Entries)
                    {
                        if (entry.Key.Contains("ADDROB") == true)
                        {
                            entry.WriteToDirectory(opf.FileName.Replace("\\" + opf.SafeFileName, ""), new ExtractionOptions());
                            string ConnectionString = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0}; 
Extended Properties=dBASE IV;", opf.FileName.Replace("\\" + opf.SafeFileName, ""));

                            OleDbConnection conn = new OleDbConnection(ConnectionString);
                            DataTable dt1 = new DataTable();
                            connSQL.Close();

                            try
                            {
                                OleDbDataAdapter oledbAdapter;
                                DataSet ds;

                                ds = new DataSet();
                                ds.Clear();
                                int columns = 0;

                                conn.Open();
                                connSQL.Open();

                                ds = new DataSet();
                                ds.Clear();


                                string Sql = "select * from " + entry.Key;
                                oledbAdapter = new OleDbDataAdapter(Sql, conn);
                                try
                                {
                                    oledbAdapter.Fill(ds);
                                }
                                catch
                                {
                                    MessageBox.Show("Ошибка ядра! Переименуйте файл и попробуйте снова!");
                                }

                                columns = ds.Tables[0].Columns.Count;

                                conn.Close();
                                connSQL.Close();


                                var TB = $@"  CREATE TABLE [dbo].[AddressObjects](
    [ACTSTATUS][float] NULL,

    [AOGUID] [varchar] (36) NULL,
	[AOID] [varchar] (36) NULL,
	[AOLEVEL] [float] NULL,
	[AREACODE] [varchar] (3) NULL,
	[AUTOCODE] [varchar] (1) NULL,
	[CENTSTATUS] [float] NULL,
	[CITYCODE] [varchar] (3) NULL,
	[CODE] [varchar] (17) NULL,
	[CURRSTATUS] [float] NULL,
	[ENDDATE] [date] NULL,
	[FORMALNAME] [varchar] (120) NULL,
	[IFNSFL] [varchar] (4) NULL,
	[IFNSUL] [varchar] (4) NULL,
	[NEXTID] [varchar] (36) NULL,
	[OFFNAME] [varchar] (120) NULL,
	[OKATO] [varchar] (11) NULL,
	[OKTMO] [varchar] (11) NULL,
	[OPERSTATUS] [float] NULL,
	[PARENTGUID] [varchar] (36) NULL,
	[PLACECODE] [varchar] (3) NULL,
	[PLAINCODE] [varchar] (15) NULL,
	[POSTALCODE] [varchar] (6) NULL,
	[PREVID] [varchar] (36) NULL,
	[REGIONCODE] [varchar] (2) NULL,
	[SHORTNAME] [varchar] (10) NULL,
	[STARTDATE] [date] NULL,
	[STREETCODE] [varchar] (4) NULL,
	[TERRIFNSFL] [varchar] (4) NULL,
	[TERRIFNSUL] [varchar] (4) NULL,
	[UPDATEDATE] [date] NULL,
	[CTARCODE] [varchar] (3) NULL,
	[EXTRCODE] [varchar] (4) NULL,
	[SEXTCODE] [varchar] (3) NULL,
	[LIVESTATUS] [float] NULL,
	[NORMDOC] [varchar] (36) NULL,
	[PLANCODE] [varchar] (4) NULL,
	[CADNUM] [varchar] (100) NULL,
	[DIVTYPE] [float] NULL
) ON[PRIMARY]

  UPDATE AddressObjects SET NEXTID=NULL WHERE NEXTID=''
  UPDATE AddressObjects SET PARENTGUID = NULL WHERE PARENTGUID = ''
  UPDATE AddressObjects SET PREVID = NULL WHERE PREVID = ''
  UPDATE AddressObjects SET NORMDOC = NULL WHERE NORMDOC = ''";
                                TB = TB.Replace("AddressObjects", entry.Key.Replace(".DBF", ""));
                                try
                                {
                                    connSQL.Open();
                                    using (var command = new SqlCommand(TB, connSQL))
                                    {

                                        command.ExecuteNonQuery();
                                    }

                                    connSQL.Close();
                                }
                                catch
                                {
                                    MessageBox.Show("Ошибка создания таблицы -" + f.Replace(".DBF", ""));
                                }


                                conn.Open();
                                connSQL.Open();
                                SqlBulkCopy bulk = new SqlBulkCopy(connSQL);
                                bulk.DestinationTableName = "dbo." + entry.Key.Replace(".DBF", "");
                                bulk.BulkCopyTimeout = 0;
                                bulk.BatchSize = 100000;

                                OleDbCommand comm = conn.CreateCommand();
                                comm.CommandText = @"SELECT *  FROM " + entry.Key;
                                dt.Load(comm.ExecuteReader());
                                bulk.WriteToServer(dt);
                                conn.Close();
                                connSQL.Close();
                            }

                            catch (Exception e1)
                            {
                                MessageBox.Show("ошибка копирования" + e1.ToString());
                            }
                            
                        }

                        try
                        {
                            connSQL.Open();
                            using (var command = new SqlCommand($@"insert into AddressObjects ([AOGUID],[FORMALNAME],[REGIONCODE],[AUTOCODE],[AREACODE],[CITYCODE],[CTARCODE],[PLACECODE],
[STREETCODE],[EXTRCODE],[SEXTCODE],[OFFNAME],[POSTALCODE],[IFNSFL],[TERRIFNSFL],[IFNSUL],[TERRIFNSUL],[OKATO],[OKTMO],[UPDATEDATE],[SHORTNAME],[AOLEVEL],[PARENTGUID],[AOID],[PREVID],
[NEXTID],[CODE],[PLAINCODE],[ACTSTATUS],[CENTSTATUS],[OPERSTATUS],[CURRSTATUS],[STARTDATE],[ENDDATE],[NORMDOC],[LIVESTATUS])  
select CAST([AOGUID] as UNIQUEIDENTIFIER),[FORMALNAME],[REGIONCODE],[AUTOCODE],[AREACODE],[CITYCODE],[CTARCODE],[PLACECODE],[STREETCODE],[EXTRCODE],[SEXTCODE],[OFFNAME],[POSTALCODE],
[IFNSFL],[TERRIFNSFL],[IFNSUL],[TERRIFNSUL],[OKATO],[OKTMO],[UPDATEDATE],[SHORTNAME],[AOLEVEL],CAST([PARENTGUID] as uniqueidentifier),CAST([AOID] as uniqueidentifier),
cast([PREVID] as UNIQUEIDENTIFIER),CONVERT(uniqueidentifier,[NEXTID]),[CODE],[PLAINCODE],[ACTSTATUS],[CENTSTATUS],[OPERSTATUS],[CURRSTATUS],[STARTDATE],[ENDDATE],
cast([NORMDOC] as uniqueidentifier),[LIVESTATUS]  from {entry.Key.Replace(".DBF", "")} ", connSQL))
                            {

                                command.ExecuteNonQuery();
                            }
                            connSQL.Close();
                        }
                        catch
                        {
                            connSQL.Close();
                        }
                        try
                        {
                            connSQL.Open();
                            using (var command = new SqlCommand("DROP TABLE " + entry.Key.Replace(".DBF", ""), connSQL))
                            {
                                command.ExecuteNonQuery();
                            }
                            connSQL.Close();
                            File.Delete(opf.FileName.Replace(opf.SafeFileName, "") + entry.Key);
                        }
                        catch
                        {
                        }
                        if (entry.Key.Contains("NDOCTYPE") == true)
                        {
                            goto forward;
                        }
                    }

                }

            forward:
                //Dispatcher.BeginInvoke(new MyProgress(ProgressCount), 50);
                //////////////////////////////////////////////////////////////////////////////////////HOUSES///////////////////////////////////////////////////////////////////////////////////////////////
                string[] fileshouses;
                if (chk == true)
                {
                    //int j = 50;
                    //var archive = SharpCompress.Archives.Rar.RarArchive.Open(opf.FileName);
                    foreach (var entry in entrs)
                    {
                        j=j+(50.00/entrs.Count());
                        Thread.Sleep(50);
                        //Progressing pr = new Progressing(Progress);
                        
                        Dispatcher.BeginInvoke(new MyProgress(ProgressCount), Math.Round(j, 0, MidpointRounding.AwayFromZero));
                        for (int i = 0; i < Region.Count(); i++)
                        {
                            if (entry.Key == "HOUSE" + Region[i] + ".DBF")
                            {
                                entry.WriteToDirectory(opf.FileName.Replace("\\" + opf.SafeFileName, ""), new ExtractionOptions());
                                DataTable dt1 = new DataTable();

                                string ConnectionString11 = Properties.Settings.Default.FIASConnectionString;
                                // conn1 = new OleDbConnection(ConnectionStringZ);
                                SqlConnection connSQ1L = new SqlConnection(ConnectionString11);
                                string dbffile = opf.FileName.Replace(opf.SafeFileName, "HOUSE" + Region[i] + ".DBF");
                                using (Stream fos = File.Open(dbffile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                {
                                    var dbf = new DotNetDBF.DBFReader(fos);
                                    dbf.CharEncoding = Encoding.GetEncoding(866);

                                    var cnt = dbf.RecordCount;



                                    var fields = dbf.Fields;
                                    for (int ii = 0; ii < fields.Count(); ii++)
                                    {
                                        DataColumn workCol = dt1.Columns.Add(fields[ii].Name, fields[ii].Type);
                                        workCol.AllowDBNull = true;
                                        workCol.DefaultValue = DBNull.Value;
                                    }

                                    //var result = (from s in fields select s.Name).ToArray();
                                    //dt.Load(dbf.NextRecord());
                                    for (int ii = 0; ii < dbf.RecordCount; ii++)
                                    {
                                        var rtt = dbf.NextRecord();

                                        if (rtt != null)
                                        {
                                            for (i = 0; i < rtt.Count(); i++)
                                            {
                                                if (rtt[i].ToString() == "")
                                                {
                                                    rtt[i] = null;
                                                }
                                            }
                                            dt1.LoadDataRow(rtt, true);

                                        }

                                    }


                                    string command=$@"insert into Houses ([POSTALCODE],[IFNSFL],[TERRIFNSFL],[IFNSUL],[TERRIFNSUL],[OKATO],[OKTMO],[UPDATEDATE],[HOUSENUM],
[ESTSTATUS],[BUILDNUM],[STRUCNUM],[STRSTATUS],[HOUSEID],[HOUSEGUID],[AOGUID],[STARTDATE],[ENDDATE],[STATSTATUS],[NORMDOC],[COUNTER])  
select [POSTALCODE],[IFNSFL],[TERRIFNSFL],[IFNSUL],[TERRIFNSUL],[OKATO],[OKTMO],[UPDATEDATE],[HOUSENUM],[ESTSTATUS],[BUILDNUM],[STRUCNUM],[STRSTATUS],[HOUSEID],[HOUSEGUID],[AOGUID],
[STARTDATE],[ENDDATE],[STATSTATUS],[NORMDOC],[COUNTER]  from @dt";
                                    MyReader.UpdateFromTable<DataTable>(command, ConnectionString11, dt1);
                                    
                                }
                                try
                                {

                                    File.Delete(opf.FileName.Replace(opf.SafeFileName, "") + entry.Key);

                                }
                                catch
                                {
                                }
                                //if (i== Region.Count() - 1 && entry.Key == "HOUSE" + Region[Region.Count() - 1] + ".DBF")
                                if (j == 100)
                                {
                                    goto endproc1;
                                }
                            }

                        }
                    }
                    endproc1:
                    
                    //Dispatcher.BeginInvoke(new MyProgress(ProgressCount),100);
                    Dispatcher.BeginInvoke(new MyDelegate(ProgressMessR));
                    //string m = $@"Для вступления в силу изменений программа будет перезапущена!";
                    //string t = "Сообщение!";
                    //int b = 1;
                    //Message me = new Message(m, t, b);
                    //me.ShowDialog();
                    //System.Diagnostics.Process.Start(Environment.CurrentDirectory + @"\InsRun.vbs");
                    //this.Close();
                    //Application.Current.Shutdown();
                }

                else
                {
                    //fileshouses = Directory.GetFiles(f2, "HOUSE*.DBF");
                    //for (int p = 0; p < fileshouses.Length; p++)
                    //{
                    //    fileshouses[p] = fileshouses[p].Replace(f2, "");
                    //}
                    //for (int xz = 0; xz < fileshouses.Length; xz++)
                    //{
                    DataTable dt1 = new DataTable();
                    //string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + f2 + ";Extended Properties=dBASE IV;";
                    //string ConnectionStringZ = @"Provider=VFPOLEDB.1;Data Source=" + f2 + ";";
                    string ConnectionStringZ = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0}; 
Extended Properties=dBASE IV;", opf.FileName.Replace("\\" + opf.SafeFileName, ""));
                    //string ConnectionString1 = Properties.Settings.Default.fias;
                    string ConnectionString11 = Properties.Settings.Default.FIASConnectionString;
                    OleDbConnection conn1 = new OleDbConnection(ConnectionStringZ);
                    SqlConnection connSQ1L = new SqlConnection(ConnectionString11);
                    var archive = SharpCompress.Archives.Rar.RarArchive.Open(opf.FileName);

                    foreach (var entry in archive.Entries)
                    {
                        if (entry.Key.Contains("HOUSE") == true)
                        {
                            try
                            {
                                OleDbDataAdapter oledbAdapter1;
                                DataSet ds1;

                                int columns = 0;

                                conn1.Open();
                                connSQL1.Open();

                                ds1 = new DataSet();
                                ds1.Clear();

                                string Sql = "select * from " + entry.Key;
                                oledbAdapter1 = new OleDbDataAdapter(Sql, conn1);
                                try
                                {
                                    oledbAdapter1.Fill(ds1);
                                }
                                catch
                                {
                                    MessageBox.Show("Ошибка ядра! Переименуйте файл и попробуйте снова!");
                                }

                                columns = ds1.Tables[0].Columns.Count;

                                conn1.Close();
                                connSQL1.Close();

                                //List<string> SQL = new List<string>();
                                //SQL.Add("CREATE TABLE [" + entry.Key.Replace(".DBF", "") + "] (");
                                //SQL.Add(Environment.NewLine);
                                //for (int x = 0; x < columns; x++)
                                //{

                                //    if (x == columns - 1)
                                //    {
                                //        SQL.Add("[" + ds1.Tables[0].Columns[x] + "] VARCHAR(250) );");
                                //    }
                                //    else
                                //    {
                                //        SQL.Add("[" + ds1.Tables[0].Columns[x] + "] VARCHAR(250)," + Environment.NewLine);
                                //    }
                                //}

                                //string Table = string.Join(Environment.NewLine, SQL.ToArray());
                                //try
                                //{
                                //    connSQL1.Open();
                                //    using (var command = new SqlCommand(Table, connSQL1))
                                //    {

                                //        command.ExecuteNonQuery();
                                //    }



                                //    connSQL1.Close();
                                //}
                                //catch
                                //{
                                //    MessageBox.Show("Ошибка создания таблицы -" + f.Replace(".DBF", ""));
                                //}
                                var TBHOUSES = $@"CREATE TABLE [dbo].[AddressObjects](
 [AOGUID] [varchar](36) NULL,
  [BUILDNUM] [varchar](10) NULL,
  [ENDDATE] [datetime] NULL,
  [ESTSTATUS] [float] NULL,
  [HOUSEGUID] [varchar](36) NULL,
  [HOUSEID] [varchar](36) NULL,
  [HOUSENUM] [varchar](20) NULL,
  [STATSTATUS] [float] NULL,
  [IFNSFL] [varchar](4) NULL,
  [IFNSUL] [varchar](4) NULL,
  [OKATO] [varchar](11) NULL,
  [OKTMO] [varchar](11) NULL,
  [POSTALCODE] [varchar](6) NULL,
  [STARTDATE] [date] NULL,
  [STRUCNUM] [varchar](10) NULL,
  [STRSTATUS] [float] NULL,
  [TERRIFNSFL] [varchar](4) NULL,
  [TERRIFNSUL] [varchar](4) NULL,
  [UPDATEDATE] [date] NULL,
  [NORMDOC] [varchar](36) NULL,
  [COUNTER] [float] NULL,
  [CADNUM] [varchar](100) NULL,
  [DIVTYPE] [float] NULL
) ON [PRIMARY]

  UPDATE AddressObjects SET AOGUID=NULL WHERE AOGUID=''
  UPDATE AddressObjects SET HOUSEGUID = NULL WHERE HOUSEGUID = ''
  UPDATE AddressObjects SET HOUSEID = NULL WHERE HOUSEID = ''
  UPDATE AddressObjects SET NORMDOC = NULL WHERE NORMDOC = ''";
                                TBHOUSES = TBHOUSES.Replace("AddressObjects", entry.Key.Replace(".DBF", ""));
                                try
                                {
                                    connSQL1.Open();
                                    using (var command = new SqlCommand(TBHOUSES, connSQL1))
                                    {

                                        command.ExecuteNonQuery();
                                    }

                                    connSQL1.Close();
                                }
                                catch
                                {
                                    MessageBox.Show("Ошибка создания таблицы -" + f.Replace(".DBF", ""));
                                }

                                conn1.Open();
                                connSQL1.Open();
                                SqlBulkCopy bulk = new SqlBulkCopy(connSQL1);
                                bulk.DestinationTableName = "dbo." + entry.Key.Replace(".DBF", "");
                                bulk.BatchSize = 100000;
                                bulk.BulkCopyTimeout = 0;

                                OleDbCommand comm1 = conn1.CreateCommand();
                                comm1.CommandText = @"SELECT *  FROM " + entry.Key;
                                dt1.Load(comm1.ExecuteReader());

                                bulk.WriteToServer(dt1);
                                //OleDbDataReader read = comm.ExecuteReader();
                                //bulk.WriteToServer(read);

                                conn1.Close();
                                connSQL1.Close();
                            }

                            catch (Exception e3)
                            {
                                MessageBox.Show("ошибка копирования" + e3.ToString());
                            }

                            //try
                            //{
                            //    connSQL1.Open();
                            //    using (var command = new SqlCommand("insert into HOUSEZ([AOGUID],[BUILDNUM],[ENDDATE],[ESTSTATUS],[HOUSEGUID],[HOUSEID],[HOUSENUM],[STATSTATUS],[IFNSFL],[IFNSUL],[OKATO],[OKTMO],[POSTALCODE],[STARTDATE],[STRUCNUM] ,[STRSTATUS] ,[TERRIFNSFL],[TERRIFNSUL],[UPDATEDATE] ,[NORMDOC],[COUNTER],[CADNUM] ,[DIVTYPE]) 	select [AOGUID],[BUILDNUM],[ENDDATE],[ESTSTATUS],[HOUSEGUID],[HOUSEID],[HOUSENUM],[STATSTATUS],[IFNSFL],[IFNSUL],[OKATO],[OKTMO],[POSTALCODE],[STARTDATE],[STRUCNUM] ,[STRSTATUS] ,[TERRIFNSFL],[TERRIFNSUL],[UPDATEDATE] ,[NORMDOC],[COUNTER],[CADNUM] ,[DIVTYPE] from " + entry.Key.Replace(".DBF", ""), connSQL1))
                            //    {

                            //        command.ExecuteNonQuery();
                            //    }
                            //    connSQL1.Close();
                            //}
                            //catch
                            //{
                            //}


                        }


                        try
                        {
                            connSQL1.Open();
                            using (var command = new SqlCommand($@"insert into Houses ([POSTALCODE],[IFNSFL],[TERRIFNSFL],[IFNSUL],[TERRIFNSUL],[OKATO],[OKTMO],[UPDATEDATE],[HOUSENUM],
[ESTSTATUS],[BUILDNUM],[STRUCNUM],[STRSTATUS],[HOUSEID],[HOUSEGUID],[AOGUID],[STARTDATE],[ENDDATE],[STATSTATUS],[NORMDOC],[COUNTER])  
select [POSTALCODE],[IFNSFL],[TERRIFNSFL],[IFNSUL],[TERRIFNSUL],[OKATO],[OKTMO],[UPDATEDATE],[HOUSENUM],[ESTSTATUS],[BUILDNUM],[STRUCNUM],[STRSTATUS],[HOUSEID],[HOUSEGUID],[AOGUID],
[STARTDATE],[ENDDATE],[STATSTATUS],[NORMDOC],[COUNTER]  from {entry.Key.Replace(".DBF", "")}", connSQL1))
                            {

                                command.ExecuteNonQuery();
                            }
                            connSQL1.Close();
                        }
                        catch
                        {
                        }
                        try
                        {
                            connSQL1.Open();
                            using (var command = new SqlCommand("DROP TABLE " + entry.Key.Replace(".DBF", ""), connSQL1))
                            {

                                command.ExecuteNonQuery();
                            }
                            File.Delete(opf.FileName.Replace(opf.SafeFileName, "") + entry.Key);
                            connSQL1.Close();
                        }
                        catch
                        {
                        }
                        if (entry.Key.Contains("NDOCTYPE") == true)
                        {
                            goto endproc;
                        }
                    }
                endproc:
                    
                    Dispatcher.BeginInvoke(new MyDelegate(ProgressMessAll));
                }
                try
                {
                    connSQL1.Open();
                    using (var command = new SqlCommand($@" declare @n nvarchar(50)
                    set @n=(SELECT name FROM sysfiles WHERE filename LIKE '%LDF%')
                    ALTER DATABASE [{fias_cat}] SET RECOVERY SIMPLE WITH NO_WAIT
                    DBCC SHRINKFILE (@n , 0)
                    ALTER DATABASE [{fias_cat}] SET RECOVERY FULL"
, connSQL1)) //DBCC CHECKIDENT (ADDROB, RESEED, 0); DBCC CHECKIDENT(AddressObjects, RESEED, 0);
             //                    using (var command = new SqlCommand($@" declare @n nvarchar(50)
             //set @n=(SELECT name FROM sysfiles WHERE filename LIKE '%LDF%')
             //ALTER DATABASE [{fias_cat}] SET RECOVERY SIMPLE WITH NO_WAIT
             //DBCC SHRINKFILE (@n , 0)" , connSQL1)) //DBCC CHECKIDENT (ADDROB, RESEED, 0); DBCC CHECKIDENT(AddressObjects, RESEED, 0);
                    {
                        command.CommandTimeout = 0;
                        command.ExecuteNonQuery();
                    }
                    connSQL1.Close();

                }
                catch
                {
                    connSQL1.Close();
                }
                //LoadingDecorator1.IsSplashScreenShown = false;
            }

        }
        private void ProgressMessR()
        {
            string m = $@"Справочник ФИАС по регионам: {regions} успешно обновлен!";
            string t = "Сообщение!";
            int b = 1;
            Message me = new Message(m, t, b);
            me.ShowDialog();

            string m1 = $@"Для вступления в силу изменений программа будет перезапущена!";
            string t1 = "Сообщение!";
            int b1 = 1;
            Message me1 = new Message(m1, t1, b1);
            me1.ShowDialog();
            System.Diagnostics.Process.Start(Environment.CurrentDirectory + @"\InsRun.vbs");
            this.Close();
            Application.Current.Shutdown();
        }
        private void ProgressMessAll()
        {
            string m = $@"Справочник ФИАС по всем регионам успешно обновлен!";
            string t = "Сообщение!";
            int b = 1;
            Message me = new Message(m, t, b);
            me.ShowDialog();
            string m1 = $@"Для вступления в силу изменений программа будет перезапущена!";
            string t1 = "Сообщение!";
            int b1 = 1;
            Message me1 = new Message(m1, t1, b1);
            me1.ShowDialog();
            System.Diagnostics.Process.Start(Environment.CurrentDirectory + @"\InsRun.vbs");
            this.Close();
            Application.Current.Shutdown();
        }




    }
}
