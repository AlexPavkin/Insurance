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
        public delegate void MyDelegateErr(Exception ex);
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


                IEnumerable<SharpCompress.Archives.Zip.ZipArchiveEntry> entrs=null;
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
                    var archive = SharpCompress.Archives.Zip.ZipArchive.Open(opf.FileName);
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
                                //try
                                //{
                                //    connSQL1.Open();
                                //    using (var command = new SqlCommand($@" DELETE FROM Houses where AOGUID in (select AOGUID from AddressObjects where REGIONCODE={Region[i]}) ; 
                                //                                            DELETE FROM AddressObjects where REGIONCODE={Region[i]};", connSQL1)) //DBCC CHECKIDENT (ADDROB, RESEED, 0); DBCC CHECKIDENT(AddressObjects, RESEED, 0);
                                //    {
                                //        command.CommandTimeout = 0;
                                //        command.ExecuteNonQuery();
                                //    }
                                //    connSQL1.Close();

                                //}
                                //catch
                                //{

                                //}

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
                               
                                    


                                    try
                                    {
                                        string command0 = $@" update AddressObjects set [AOGUID]=CAST(dt.AOGUID as UNIQUEIDENTIFIER),[FORMALNAME]=dt.FORMALNAME,[REGIONCODE]=dt.REGIONCODE,[AUTOCODE]=dt.AUTOCODE,[AREACODE]=dt.AREACODE,
                                        [CITYCODE]=dt.CITYCODE,[CTARCODE]=dt.CTARCODE,[PLACECODE]=dt.PLACECODE,[STREETCODE]=dt.STREETCODE,[EXTRCODE]=dt.EXTRCODE,[SEXTCODE]=dt.SEXTCODE,[OFFNAME]=dt.OFFNAME,
                                        [POSTALCODE]=dt.POSTALCODE,[IFNSFL]=dt.IFNSFL,[TERRIFNSFL]=dt.TERRIFNSFL,[IFNSUL]=dt.IFNSUL,[TERRIFNSUL]=dt.TERRIFNSUL,[OKATO]=dt.OKATO,[OKTMO]=dt.OKTMO,[UPDATEDATE]=dt.UPDATEDATE,
                                        [SHORTNAME]=dt.SHORTNAME,[AOLEVEL]=dt.AOLEVEL,[PARENTGUID]=CAST(dt.PARENTGUID as uniqueidentifier),[AOID]=CAST(dt.AOID as uniqueidentifier),[PREVID]=cast(dt.PREVID as UNIQUEIDENTIFIER),
                                        [NEXTID]=CAST(dt.NEXTID as uniqueidentifier),[CODE]=dt.CODE,[PLAINCODE]=dt.PLAINCODE,[ACTSTATUS]=dt.ACTSTATUS,[CENTSTATUS]=dt.CENTSTATUS,[OPERSTATUS]=dt.OPERSTATUS,[CURRSTATUS]=isnull(dt.CURRSTATUS,0),
                                        [STARTDATE]=dt.STARTDATE,[ENDDATE]=dt.ENDDATE, [NORMDOC]=cast(dt.NORMDOC as uniqueidentifier),[LIVESTATUS]=dt.LIVESTATUS
                                        from @dt dt where AddressObjects.aoid=CAST(dt.AOID as uniqueidentifier)";

                                        string command = $@" insert into AddressObjects ([AOGUID],[FORMALNAME],[REGIONCODE],[AUTOCODE],[AREACODE],[CITYCODE],[CTARCODE],[PLACECODE],
                                        [STREETCODE],[EXTRCODE],[SEXTCODE],[OFFNAME],[POSTALCODE],[IFNSFL],[TERRIFNSFL],[IFNSUL],[TERRIFNSUL],[OKATO],[OKTMO],[UPDATEDATE],[SHORTNAME],[AOLEVEL],[PARENTGUID],[AOID],[PREVID],
                                        [NEXTID],[CODE],[PLAINCODE],[ACTSTATUS],[CENTSTATUS],[OPERSTATUS],[CURRSTATUS],[STARTDATE],[ENDDATE],[NORMDOC],[LIVESTATUS])  
                                        select CAST(dt.AOGUID as UNIQUEIDENTIFIER),dt.FORMALNAME,dt.REGIONCODE,dt.AUTOCODE,dt.AREACODE,dt.CITYCODE,dt.CTARCODE,dt.PLACECODE,dt.STREETCODE,dt.EXTRCODE,dt.SEXTCODE,dt.OFFNAME,dt.POSTALCODE,
                                        dt.IFNSFL,dt.TERRIFNSFL,dt.IFNSUL,dt.TERRIFNSUL,dt.OKATO,dt.OKTMO,dt.UPDATEDATE,dt.SHORTNAME,dt.AOLEVEL,CAST(dt.PARENTGUID as uniqueidentifier),CAST(dt.AOID as uniqueidentifier),
                                        cast(dt.PREVID as UNIQUEIDENTIFIER),CAST(dt.NEXTID as uniqueidentifier),dt.CODE,dt.PLAINCODE,dt.ACTSTATUS,dt.CENTSTATUS,dt.OPERSTATUS,isnull(dt.CURRSTATUS,0),dt.STARTDATE,dt.ENDDATE,
                                        cast(dt.NORMDOC as uniqueidentifier),dt.LIVESTATUS from @dt dt
                                        left join AddressObjects ao on CAST(dt.AOID as uniqueidentifier) =ao.aoid where ao.aoguid is null";
                                        MyReader.UpdateFromTable<DataTable>(command0, ConnectionString1, dt);
                                        MyReader.UpdateFromTable<DataTable>(command, ConnectionString1, dt);
                                    }
                                    catch(Exception ex)
                                    {
                                        string m = ex.ToString();
                                        string t = "Ошибка!";
                                        int b = 1;
                                        Message me = new Message(m, t, b);
                                        me.ShowDialog();
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
                    //DataTable dt = new DataTable();
                    // string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + f2 + ";Extended Properties=dBASE IV;";
                    //string ConnectionString = @"Provider=VFPOLEDB.1;Data Source=" + f2 + ";";
                    //string ConnectionString1 = Properties.Settings.Default.fias;
                    string ConnectionString1 = Properties.Settings.Default.FIASConnectionString;
                    //OleDbConnection conn = new OleDbConnection(ConnectionString);
                    SqlConnection connSQL = new SqlConnection(ConnectionString1);
                    var archive = SharpCompress.Archives.Zip.ZipArchive.Open(opf.FileName);
                    entrs = archive.Entries.Where(x => x.Key.StartsWith("ADDROB") == true || x.Key.StartsWith("HOUSE") == true);
                    foreach (var entry in entrs)
                    {
                        j = j + (50.00 / entrs.Count());
                        Thread.Sleep(50);
                        //Progressing pr = new Progressing(Progress);

                        Dispatcher.BeginInvoke(new MyProgress(ProgressCount), Math.Round(j, 0, MidpointRounding.AwayFromZero));
                        

                            if (entry.Key.Contains("ADDROB")==true)
                            {
                                DataTable dt = new DataTable();
                                entry.WriteToDirectory(opf.FileName.Replace("\\" + opf.SafeFileName, ""), new ExtractionOptions());
                                //try
                                //{
                                //    connSQL1.Open();
                                //    using (var command = new SqlCommand($@" DELETE FROM Houses ; 
                                //                                            DELETE FROM AddressObjects", connSQL1)) //DBCC CHECKIDENT (ADDROB, RESEED, 0); DBCC CHECKIDENT(AddressObjects, RESEED, 0);
                                //    {
                                //        command.CommandTimeout = 0;
                                //        command.ExecuteNonQuery();
                                //    }
                                //    connSQL1.Close();

                                //}
                                //catch
                                //{

                                //}

                                //DataTable dt = new DataTable();
                                string dbffile = opf.FileName.Replace(opf.SafeFileName, entry.Key);
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
                                            for (int i = 0; i < rtt.Count(); i++)
                                            {
                                                if (rtt[i].ToString() == "")
                                                {
                                                    rtt[i] = null;
                                                }
                                            }
                                            dt.LoadDataRow(rtt, true);

                                        }

                                    }
                                string command0 = $@" update AddressObjects set [AOGUID]=CAST(dt.AOGUID as UNIQUEIDENTIFIER),[FORMALNAME]=dt.FORMALNAME,[REGIONCODE]=dt.REGIONCODE,[AUTOCODE]=dt.AUTOCODE,[AREACODE]=dt.AREACODE,
                                        [CITYCODE]=dt.CITYCODE,[CTARCODE]=dt.CTARCODE,[PLACECODE]=dt.PLACECODE,[STREETCODE]=dt.STREETCODE,[EXTRCODE]=dt.EXTRCODE,[SEXTCODE]=dt.SEXTCODE,[OFFNAME]=dt.OFFNAME,
                                        [POSTALCODE]=dt.POSTALCODE,[IFNSFL]=dt.IFNSFL,[TERRIFNSFL]=dt.TERRIFNSFL,[IFNSUL]=dt.IFNSUL,[TERRIFNSUL]=dt.TERRIFNSUL,[OKATO]=dt.OKATO,[OKTMO]=dt.OKTMO,[UPDATEDATE]=dt.UPDATEDATE,
                                        [SHORTNAME]=dt.SHORTNAME,[AOLEVEL]=dt.AOLEVEL,[PARENTGUID]=CAST(dt.PARENTGUID as uniqueidentifier),[AOID]=CAST(dt.AOID as uniqueidentifier),[PREVID]=cast(dt.PREVID as UNIQUEIDENTIFIER),
                                        [NEXTID]=CAST(dt.NEXTID as uniqueidentifier),[CODE]=dt.CODE,[PLAINCODE]=dt.PLAINCODE,[ACTSTATUS]=dt.ACTSTATUS,[CENTSTATUS]=dt.CENTSTATUS,[OPERSTATUS]=dt.OPERSTATUS,[CURRSTATUS]=isnull(dt.CURRSTATUS,0),
                                        [STARTDATE]=dt.STARTDATE,[ENDDATE]=dt.ENDDATE, [NORMDOC]=cast(dt.NORMDOC as uniqueidentifier),[LIVESTATUS]=dt.LIVESTATUS
                                        from @dt dt where AddressObjects.aoid=CAST(dt.AOID as uniqueidentifier)";

                                string command = $@" insert into AddressObjects ([AOGUID],[FORMALNAME],[REGIONCODE],[AUTOCODE],[AREACODE],[CITYCODE],[CTARCODE],[PLACECODE],
                                        [STREETCODE],[EXTRCODE],[SEXTCODE],[OFFNAME],[POSTALCODE],[IFNSFL],[TERRIFNSFL],[IFNSUL],[TERRIFNSUL],[OKATO],[OKTMO],[UPDATEDATE],[SHORTNAME],[AOLEVEL],[PARENTGUID],[AOID],[PREVID],
                                        [NEXTID],[CODE],[PLAINCODE],[ACTSTATUS],[CENTSTATUS],[OPERSTATUS],[CURRSTATUS],[STARTDATE],[ENDDATE],[NORMDOC],[LIVESTATUS])  
                                        select CAST(dt.AOGUID as UNIQUEIDENTIFIER),dt.FORMALNAME,dt.REGIONCODE,dt.AUTOCODE,dt.AREACODE,dt.CITYCODE,dt.CTARCODE,dt.PLACECODE,dt.STREETCODE,dt.EXTRCODE,dt.SEXTCODE,dt.OFFNAME,dt.POSTALCODE,
                                        dt.IFNSFL,dt.TERRIFNSFL,dt.IFNSUL,dt.TERRIFNSUL,dt.OKATO,dt.OKTMO,dt.UPDATEDATE,dt.SHORTNAME,dt.AOLEVEL,CAST(dt.PARENTGUID as uniqueidentifier),CAST(dt.AOID as uniqueidentifier),
                                        cast(dt.PREVID as UNIQUEIDENTIFIER),CAST(dt.NEXTID as uniqueidentifier),dt.CODE,dt.PLAINCODE,dt.ACTSTATUS,dt.CENTSTATUS,dt.OPERSTATUS,isnull(dt.CURRSTATUS,0),dt.STARTDATE,dt.ENDDATE,
                                        cast(dt.NORMDOC as uniqueidentifier),dt.LIVESTATUS from @dt dt
                                        left join AddressObjects ao on CAST(dt.AOID as uniqueidentifier) =ao.aoid where ao.aoguid is null";
                                
                                try
                                        {
                                            MyReader.UpdateFromTable<DataTable>(command0, ConnectionString1, dt);
                                            MyReader.UpdateFromTable<DataTable>(command, ConnectionString1, dt);
                                        }
                                        catch (Exception ex)
                                        {
                                            Dispatcher.BeginInvoke(new MyDelegateErr(MessErr), ex);
                                            Thread.Sleep(100);
                                            while (j >= 0)
                                            {
                                                Dispatcher.BeginInvoke(new MyProgress(ProgressCount), j);
                                                j = j - 1;
                                        
                                                Thread.Sleep(50);
                                                if (j > 0 && j < 1)
                                                {
                                                    j = 0;
                                                }
                                            }
                                        
                                            return;
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
                                            dt1.LoadDataRow(rtt, true);

                                        }

                                    
                                }
                            
                                    string command0 = $@"update Houses set [POSTALCODE]=dt.POSTALCODE,[IFNSFL]=dt.IFNSFL,[TERRIFNSFL]=dt.TERRIFNSFL,[IFNSUL]=dt.IFNSUL,[TERRIFNSUL]=dt.TERRIFNSUL,[OKATO]=dt.OKATO,
[OKTMO]=dt.OKTMO,[UPDATEDATE]=dt.UPDATEDATE,[HOUSENUM]=dt.HOUSENUM,[ESTSTATUS]=dt.ESTSTATUS,[BUILDNUM]=dt.BUILDNUM,[STRUCNUM]=dt.STRUCNUM,[STRSTATUS]=dt.STRSTATUS,[HOUSEID]=dt.HOUSEID,[HOUSEGUID]=dt.HOUSEGUID,
[AOGUID]=dt.AOGUID,[STARTDATE]=dt.STARTDATE,[ENDDATE]=dt.ENDDATE,[STATSTATUS]=dt.STATSTATUS,[NORMDOC]=dt.NORMDOC,[COUNTER]=dt.COUNTER  
from @dt dt where Houses.HOUSEID=dt.HOUSEID";

                                    string command=$@"insert into Houses ([POSTALCODE],[IFNSFL],[TERRIFNSFL],[IFNSUL],[TERRIFNSUL],[OKATO],[OKTMO],[UPDATEDATE],[HOUSENUM],
[ESTSTATUS],[BUILDNUM],[STRUCNUM],[STRSTATUS],[HOUSEID],[HOUSEGUID],[AOGUID],[STARTDATE],[ENDDATE],[STATSTATUS],[NORMDOC],[COUNTER])  
select dt.POSTALCODE,dt.IFNSFL,dt.TERRIFNSFL,dt.IFNSUL,dt.TERRIFNSUL,dt.OKATO,dt.OKTMO,dt.UPDATEDATE,dt.HOUSENUM,dt.ESTSTATUS,dt.BUILDNUM,dt.STRUCNUM,dt.STRSTATUS,dt.HOUSEID,dt.HOUSEGUID,dt.AOGUID,
dt.STARTDATE,dt.ENDDATE,dt.STATSTATUS,dt.NORMDOC,dt.COUNTER  from @dt dt
left join houses h on dt.HOUSEID=h.HOUSEID where h.id is null";
                                    try
                                    {
                                        MyReader.UpdateFromTable<DataTable>(command0, ConnectionString11, dt1);
                                        MyReader.UpdateFromTable<DataTable>(command, ConnectionString11, dt1);
                                    }
                                    catch (Exception ex)
                                    {
                                        Dispatcher.BeginInvoke(new MyDelegateErr(MessErr),ex);
                                        Thread.Sleep(100);
                                        while(j>=0)
                                        {
                                            Dispatcher.BeginInvoke(new MyProgress(ProgressCount), j);
                                            j = j - 1;
                                            
                                            Thread.Sleep(50);
                                            if(j>0 && j<1)
                                            {
                                                j = 0;
                                            }
                                        }
                                        
                                        return;
                                    }

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
                    //DataTable dt1 = new DataTable();
                    //string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + f2 + ";Extended Properties=dBASE IV;";
                    //string ConnectionStringZ = @"Provider=VFPOLEDB.1;Data Source=" + f2 + ";";
                    string ConnectionStringZ = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0}; 
Extended Properties=dBASE IV;", opf.FileName.Replace("\\" + opf.SafeFileName, ""));
                    //string ConnectionString1 = Properties.Settings.Default.fias;
                    string ConnectionString11 = Properties.Settings.Default.FIASConnectionString;
                    OleDbConnection conn1 = new OleDbConnection(ConnectionStringZ);
                    SqlConnection connSQ1L = new SqlConnection(ConnectionString11);
                    var archive = SharpCompress.Archives.Zip.ZipArchive.Open(opf.FileName);
                    entrs = archive.Entries.Where(x => x.Key.StartsWith("ADDROB") == true || x.Key.StartsWith("HOUSE") == true);
                    foreach (var entry in entrs)
                    {
                        j = j + (50.00 / entrs.Count());
                        Thread.Sleep(50);
                        //Progressing pr = new Progressing(Progress);

                        Dispatcher.BeginInvoke(new MyProgress(ProgressCount), Math.Round(j, 0, MidpointRounding.AwayFromZero));
                        
                            if (entry.Key.Contains("HOUSE"))
                            {
                                entry.WriteToDirectory(opf.FileName.Replace("\\" + opf.SafeFileName, ""), new ExtractionOptions());
                                //DataTable dt1 = new DataTable();
                                DataTable dt1 = new DataTable();
                                //string ConnectionString11 = Properties.Settings.Default.FIASConnectionString;
                                // conn1 = new OleDbConnection(ConnectionStringZ);
                                //SqlConnection connSQ1L = new SqlConnection(ConnectionString11);
                                string dbffile = opf.FileName.Replace(opf.SafeFileName, entry.Key);
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
                                            for (int i = 0; i < rtt.Count(); i++)
                                            {
                                                if (rtt[i].ToString() == "")
                                                {
                                                    rtt[i] = null;
                                                }
                                            }
                                            dt1.LoadDataRow(rtt, true);

                                        }

                                    }
                                string command0 = $@"update Houses set [POSTALCODE]=dt.POSTALCODE,[IFNSFL]=dt.IFNSFL,[TERRIFNSFL]=dt.TERRIFNSFL,[IFNSUL]=dt.IFNSUL,[TERRIFNSUL]=dt.TERRIFNSUL,[OKATO]=dt.OKATO,
[OKTMO]=dt.OKTMO,[UPDATEDATE]=dt.UPDATEDATE,[HOUSENUM]=dt.HOUSENUM,[ESTSTATUS]=dt.ESTSTATUS,[BUILDNUM]=dt.BUILDNUM,[STRUCNUM]=dt.STRUCNUM,[STRSTATUS]=dt.STRSTATUS,[HOUSEID]=dt.HOUSEID,[HOUSEGUID]=dt.HOUSEGUID,
[AOGUID]=dt.AOGUID,[STARTDATE]=dt.STARTDATE,[ENDDATE]=dt.ENDDATE,[STATSTATUS]=dt.STATSTATUS,[NORMDOC]=dt.NORMDOC,[COUNTER]=dt.COUNTER  
from @dt dt where Houses.HOUSEID=dt.HOUSEID";

                                string command = $@"insert into Houses ([POSTALCODE],[IFNSFL],[TERRIFNSFL],[IFNSUL],[TERRIFNSUL],[OKATO],[OKTMO],[UPDATEDATE],[HOUSENUM],
[ESTSTATUS],[BUILDNUM],[STRUCNUM],[STRSTATUS],[HOUSEID],[HOUSEGUID],[AOGUID],[STARTDATE],[ENDDATE],[STATSTATUS],[NORMDOC],[COUNTER])  
select dt.POSTALCODE,dt.IFNSFL,dt.TERRIFNSFL,dt.IFNSUL,dt.TERRIFNSUL,dt.OKATO,dt.OKTMO,dt.UPDATEDATE,dt.HOUSENUM,dt.ESTSTATUS,dt.BUILDNUM,dt.STRUCNUM,dt.STRSTATUS,dt.HOUSEID,dt.HOUSEGUID,dt.AOGUID,
dt.STARTDATE,dt.ENDDATE,dt.STATSTATUS,dt.NORMDOC,dt.COUNTER  from @dt dt
left join houses h on dt.HOUSEID=h.HOUSEID where h.id is null";
                                try
                                {
                                    MyReader.UpdateFromTable<DataTable>(command0, ConnectionString11, dt1);
                                    MyReader.UpdateFromTable<DataTable>(command, ConnectionString11, dt1);
                                }
                                catch (Exception ex)
                                {
                                    Dispatcher.BeginInvoke(new MyDelegateErr(MessErr), ex);
                                    Thread.Sleep(100);
                                    while (j >= 0)
                                    {
                                        Dispatcher.BeginInvoke(new MyProgress(ProgressCount), j);
                                        j = j - 1;

                                        Thread.Sleep(50);
                                        if (j > 0 && j < 1)
                                        {
                                            j = 0;
                                        }
                                    }

                                    return;
                                }

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
                                    goto endproc;
                                }
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
            string m = $@"Справочник ФИАС по всем регионам успешно обновлен! Закончено в: {DateTime.Now.ToString()}";
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

        private void MessErr(Exception ex)
        {
            string m = $@"Справочник ФИАС по всем регионам успешно обновлен!";
            string t = "Ошибка!";
            int b = 1;
            Message me = new Message(ex.Message, t, b);
            me.ShowDialog();
            
        }




    }
}
