using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.XtraGrid.Columns;
using Insurance_SPR;
using DevExpress;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Data;
using Microsoft.Win32;

namespace Insurance
{
    /// <summary>
    /// Interaction logic for DXWindow1.xaml
    /// </summary>
    public partial class Inform : Window
    {
        private string call_;
        public List<FIO> fio_col;
        public Inform(string call)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WindowState = WindowState.Maximized;
            call_ = call;
            if (call_ == "unload_history" || call_ == "unload_files")
            {
                WindowState = WindowState.Maximized;
                infom_ctrl.Visibility = Visibility.Collapsed;
                inform_grid1.Visibility = Visibility.Collapsed;
                //inform_grid.VerticalAlignment = VerticalAlignment.Stretch;
                del_file_panel.Visibility = Visibility.Visible;
                inform_file_panel.Visibility = Visibility.Collapsed;

                if (call_ == "unload_history")
                {
                    ViewFilesItem.IsEnabled = true;
                }
                
            }
            else if (call_ == "person_history")
            {
                infom_ctrl.Visibility = Visibility.Collapsed;
                inform_grid1.Visibility = Visibility.Collapsed;
                //inform_grid.VerticalAlignment = VerticalAlignment.Stretch;
                del_file_panel.Visibility = Visibility.Collapsed;
                inform_file_panel.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (Vars.mes_res != 0)
                {
                    //this.Height = 100;
                    G_layuot.restore_Layout(Properties.Settings.Default.DocExchangeConnectionString, inform_grid, "1");
                }
                inform_grid.Visibility = Visibility.Visible;
                del_file_panel.Visibility = Visibility.Collapsed;
            }
            
            //Adder.Visibility = Visibility.Collapsed;
            //Premiss_edt.Visibility = Visibility.Collapsed;
            //dateP.Visibility = Visibility.Collapsed;
            
            
            //Del_file_btn.Visibility = Visibility.Hidden;
            //del_btn_hist.Visibility = Visibility.Collapsed;
            //all_files.Visibility = Visibility.Hidden;
            //Premiss_edt.Visibility = Visibility.Collapsed;
            //dateP.Visibility = Visibility.Collapsed;

        }
        private void Informed_Loaded(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
            new Action(delegate ()
            {
                if (call_ == "attache_history")
                {
                    fio_col = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
                    fam_p3.DataContext = fio_col;
                    im_p3.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
                    ot_p3.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
                    fam_p4.DataContext = fio_col;
                    im_p4.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
                    ot_p4.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
                    MKB.DataContext = MyReader.MySelect<MKB>(@"SELECT IDDS,DSNAME,NameWithID  FROM M001_KSG", Properties.Settings.Default.DocExchangeConnectionString);
                    Theme_p3.DataContext = MyReader.MySelect<THEME_INFORM_P3>(@"SELECT ID ,Name,NameWithID  FROM THEME_INFORM_P3", Properties.Settings.Default.DocExchangeConnectionString);
                    Sposob_p3.DataContext = MyReader.MySelect<SPOSOB_INFORM>(@"SELECT ID ,Name  FROM SPOSOB_INFORM", Properties.Settings.Default.DocExchangeConnectionString);
                    Result_p3.DataContext = MyReader.MySelect<RESULT_INFORM_P3>(@"SELECT ID ,Name  FROM RESULT_INFORM_P3", Properties.Settings.Default.DocExchangeConnectionString);
                    Vid_meropr_p3.DataContext = MyReader.MySelect<VID_MEROPR_INFORM_P3>(@"SELECT ID ,Name  FROM VID_MEROPR_INFORM_P3", Properties.Settings.Default.DocExchangeConnectionString);
                    Tema_yved_p4.DataContext = MyReader.MySelect<THEME_INFORM_P4>(@"SELECT ID ,Name  FROM THEME_INFORM_P4", Properties.Settings.Default.DocExchangeConnectionString);
                    Sposob_p4.DataContext = MyReader.MySelect<SPOSOB_INFORM>(@"SELECT ID ,Name  FROM SPOSOB_INFORM", Properties.Settings.Default.DocExchangeConnectionString);
                    Result_p4.DataContext = MyReader.MySelect<RESULT_INFORM_P4>(@"SELECT TOP(4) ID ,Name  FROM RESULT_INFORM_P3", Properties.Settings.Default.DocExchangeConnectionString);


                    //Adder.Visibility = Visibility.Visible;
                    // Premiss_edt.Visibility = Visibility.Visible;
                    //dateP.Visibility = Visibility.Visible;
                    //Premiss_edt.DataContext = MyReader.MySelect<INFORMIROVAN>(@"SELECT * FROM PRICHINA_INFORMIROVANIYA",
                    //    Properties.Settings.Default.DocExchangeConnectionString);

                    //Premiss_edt.SelectedIndex = 0;
                    //dateP.DateTime = DateTime.Now;

                    var peopleList = MyReader.MySelect<Events>(SPR.MyReader.load_pers_grid + "order by pe.ID DESC", Properties.Settings.Default.DocExchangeConnectionString);
                    inform_grid.ItemsSource = peopleList;
                    inform_grid.View.FocusedRowHandle = -1;


                    inform_grid.Visibility = Visibility.Visible;
                    //Del_file_btn.Visibility = Visibility.Hidden;
                    del_file_panel.Visibility = Visibility.Collapsed;
                    //del_btn_hist.Visibility = Visibility.Hidden;

                }
                else if (call_ == "unload_history")
                {
                    Title = "История обмена данными с ТФОМС";
                    //Adder.Visibility = Visibility.Hidden;
                    //Premiss_edt.Visibility = Visibility.Hidden;
                    //dateP.Visibility = Visibility.Hidden;
                    //Del_file_btn.Visibility = Visibility.Hidden;
                    //del_btn_hist.Visibility = Visibility.Hidden;
                    inform_grid.Visibility = Visibility.Visible;
                    var peopleList =
                    MyReader.MySelect<UNLOAD_HISTORY>(
                        $@"  SELECT uh.id,uh.tip_op,uh.fname,uh.comment,uh.korob,uh.fdate,fam,im,ot,dr
  FROM POL_UNLOAD_HISTORY uh
  left join POL_PERSONS p 
  on uh.person_guid=p.idguid
			WHERE p.ID IN ({Vars.IDSZ})
   ORDER BY uh.ID", Properties.Settings.Default.DocExchangeConnectionString);
                    inform_grid.ItemsSource = peopleList;
                    inform_grid.GroupBy("FAM");
                    inform_grid.GroupBy("IM", true);
                    inform_grid.GroupBy("OT", true);


                }
                else if (call_ == "person_history")
                {
                    Title = "История событий ЗЛ";
                    //inform_file_panel.Visibility = Visibility.Collapsed;
                    //Premiss_edt.Visibility = Visibility.Hidden;
                    //dateP.Visibility = Visibility.Hidden;
                    //Del_file_btn.Visibility = Visibility.Hidden;
                    del_file_panel.Visibility = Visibility.Collapsed;
                    inform_grid.Visibility = Visibility.Visible;
                    var peopleList =
                    MyReader.MySelect<People_history>($@"  SELECT  pe.ID,pe.DVIZIT,pp.active,pe.TIP_OP,pp.SS ,pp.ENP ,pp.FAM , pp.IM  , pp.OT ,pp.W ,pp.DR , po.FAM as FAM_OLD ,
  po.IM as IM_OLD  , po.OT as OT_OLD ,po.W as W_OLD,po.DR as DR_OLD ,pp.PHONE ,
pp.COMMENT, f.NameWithID, op.filename, d.DOCTYPE, d.DOCSER, d.DOCNUM, VPOLIS, SPOLIS, NPOLIS, DBEG, DRECEIVED, DEND, 
DSTOP,do.DOCTYPE as DOCTYPE_OLD, do.DOCSER as DOCSER_OLD, do.DOCNUM as DOCNUM_OLD,do.DOCDATE as DOCDATE_OLD
  FROM[dbo].[POL_PERSONS] pp
  
left join f003 f on pp.mo = f.mcod
left join pol_events pe
on pp.idguid = pe.PERSON_GUID
left join POL_PERSONS_OLD po
  on pe.IDGUID=po.EVENT_GUID
left join pol_documents d
on pe.idguid = d.EVENT_GUID and d.MAIN = 1
left join pol_documents_old do
on pe.idguid = do.EVENT_GUID and d.MAIN = 1
 left join pol_polises ps
on pe.IDGUID = ps.EVENT_GUID
 left join pol_oplist op
on pe.idguid = op.EVENT_GUID
 WHERE pp.ID IN({ Vars.IDSZ})", Properties.Settings.Default.DocExchangeConnectionString);
                    inform_grid.ItemsSource = peopleList;
                    inform_grid.GroupBy("FAM");
                    inform_grid.GroupBy("IM", true);
                    inform_grid.GroupBy("OT", true);


                }
                else
                {
                    Title = "Сформированные файлы выгрузки в ТФОМС (последние 5 файлов)";
                    //Adder.Visibility = Visibility.Hidden;
                    //Premiss_edt.Visibility = Visibility.Hidden;
                    //dateP.Visibility = Visibility.Hidden;
                    //del_btn_hist.Visibility = Visibility.Hidden;
                    //Del_file_btn.Visibility=Visibility.Visible;
                    inform_grid.Visibility = Visibility.Visible;
                    inform_grid.VerticalAlignment = VerticalAlignment.Stretch;
                    //all_files.Visibility = Visibility.Visible;
                    del_file_panel.Visibility = Visibility.Visible;
                    var peopleList =
                    MyReader.MySelect<UNLOAD_FILES>(
                        $@"  DECLARE @t table(rn int,id int,fname nvarchar(50),fxml XML)
insert into @t (rn,id,fname,fxml) select top(5) ROW_NUMBER() OVER(ORDER BY id) as rn, id,filename,fxml from POL_FILES order by id desc
DECLARE @tt table(fname nvarchar(50),tip_op nvarchar(50),fio nvarchar(150), dr datetime,fio_old nvarchar(150),dr_old datetime,enp nvarchar(16),
vpolis int,spolis nvarchar(11), npolis nvarchar(11),dbeg datetime,dend datetime,dstop datetime,
doctype int,docser nvarchar(20),docnum nvarchar(20),docdate datetime,ddoctype int,ddocser nvarchar(20),ddocnum nvarchar(20),ddocdate datetime,
old_doctype int,old_docser nvarchar(20),old_docnum nvarchar(20),old_docdate datetime)

declare @xml xml
declare @i int=0
while @i<=(select max(rn) from @t)
begin
set @xml=(select fxml from @t where rn=@i)
insert into @tt
select *from(select
 b.p.value('@FILENAME [1]', 'nvarchar(254)') as fname,
 t.x.value('../TIP_OP [1]', 'nvarchar(254)') as tip_op,
 isnull(t.x.value('./FAM [1]', 'nvarchar(254)'),'') +' '+
 isnull(t.x.value('./IM [1]', 'nvarchar(254)'),'') +' '+
 isnull(t.x.value('./OT [1]', 'nvarchar(254)'),'') as fio,
 t.x.value('./DR [1]', 'datetime') as dr,
 isnull(t.x.query('../OLD_PERSON/FAM').value('. [1]', 'nvarchar(254)'),'')+' '+
 isnull(t.x.query('../OLD_PERSON/IM').value('. [1]', 'nvarchar(254)'),'') +' '+
 isnull(t.x.query('../OLD_PERSON/OT').value('. [1]', 'nvarchar(254)'),'') as fio_old,
 (case when t.x.query('../OLD_PERSON/DR').value('. [1]', 'datetime')='' then null 
 else t.x.query('../OLD_PERSON/DR').value('. [1]', 'datetime') end)  as dr_old,
 t.x.query('../INSURANCE/ENP').value('. [1]', 'nvarchar(254)')  as enp,
 t.x.query('../INSURANCE/POLIS/VPOLIS').value('. [1]', 'int') as vpolis,
 t.x.query('../INSURANCE/POLIS/SPOLIS').value('. [1]', 'nvarchar(254)') as spolis,
 t.x.query('../INSURANCE/POLIS/NPOLIS').value('. [1]', 'nvarchar(254)') as npolis,
 t.x.query('../INSURANCE/POLIS/DBEG').value('. [1]', 'datetime') as dbeg,
 (case when t.x.query('../INSURANCE/POLIS/DEND').value('. [1]', 'datetime')='' then null
 else t.x.query('../INSURANCE/POLIS/DEND').value('. [1]', 'datetime') end) as dend,
 (case when t.x.query('../INSURANCE/POLIS/DSTOP').value('. [1]', 'datetime')='' then null
 else t.x.query('../INSURANCE/POLIS/DSTOP').value('. [1]', 'datetime') end) as dstop,
 t.x.query('../DOC_LIST/DOC[1]/DOCTYPE').value('. [1]', 'int') as doctype,
 t.x.query('../DOC_LIST/DOC[1]/DOCSER').value('. [1]', 'nvarchar(254)') as docser,
 t.x.query('../DOC_LIST/DOC[1]/DOCNUM').value('. [1]', 'nvarchar(254)') as docnum,
 (case when t.x.query('../DOC_LIST/DOC[1]/DOCDATE').value('. [1]', 'datetime')='' then null 
 else t.x.query('../DOC_LIST/DOC[1]/DOCDATE').value('. [1]', 'datetime') end) as docdate,
 (case when t.x.query('../DOC_LIST/DOC[2]/DOCTYPE').value('. [1]', 'int')='' then null
 else t.x.query('../DOC_LIST/DOC[2]/DOCTYPE').value('. [1]', 'int') end) as ddoctype,
 t.x.query('../DOC_LIST/DOC[2]/DOCSER').value('. [1]', 'nvarchar(254)') as ddocser,
 t.x.query('../DOC_LIST/DOC[2]/DOCNUM').value('. [1]', 'nvarchar(254)') as ddocnum,
 (case when t.x.query('../DOC_LIST/DOC[2]/DOCDATE').value('. [1]', 'datetime')='' then null 
 else t.x.query('../DOC_LIST/DOC[2]/DOCDATE').value('. [1]', 'datetime')end) as ddocdate,
 (case when t.x.query('../OLDDOC_LIST/OLD_DOC/DOCTYPE').value('. [1]', 'int')='' then null
 else t.x.query('../OLDDOC_LIST/OLD_DOC/DOCTYPE').value('. [1]', 'int') end) as old_doctype,
 t.x.query('../OLDDOC_LIST/OLD_DOC/DOCSER').value('. [1]', 'nvarchar(254)') as old_docser,
 t.x.query('../OLDDOC_LIST/OLD_DOC/DOCNUM').value('. [1]', 'nvarchar(254)') as old_docnum,
 (case when t.x.query('../OLDDOC_LIST/OLD_DOC/DOCDATE').value('. [1]', 'datetime')='' then null
 else t.x.query('../OLDDOC_LIST/OLD_DOC/DOCDATE').value('. [1]', 'datetime') end) as old_docdate
 from  @xml.nodes('/OPLIST/OP/PERSON') t(x)
 --cross apply @xml.nodes('/OPLIST/OP') a(w)
 cross apply @xml.nodes('/OPLIST') b(p))T0
 group by fname,tip_op,fio,dr,fio_old,dr_old,enp,vpolis,spolis,npolis,dbeg,dend,dstop,doctype,docser,docnum,docdate,ddoctype,ddocser,ddocnum,ddocdate,
 old_doctype,old_docser,old_docnum,old_docdate
 set @i=@i+1
 end 
 --select*from @tt
 select f.id as ID,f.FILENAME,f.FDATE,t.tip_op as TIP_OP,t.fio as FIO,t.dr as DR,t.fio_old as PREV_FIO,t.dr_old as PREV_DR,t.enp as ENP,
 t.vpolis as VPOLIS,t.spolis as SPOLIS,t.npolis as NPOLIS,t.dbeg as DBEG,t.dend as DEND,t.dstop as DSTOP,
 t.doctype as DOCTYPE,t.docser as DOCSER,t.docnum as DOCNUM,t.docdate as DOCDATE,t.ddoctype as DDOCTYPE,t.ddocser as DDOCSER,t.ddocnum as DDOCNUM,t.ddocdate as DDOCDATE,
 t.old_doctype as PREV_DOCTYPE,t.old_docser as PREV_DOCSER,t.old_docnum as PREV_DOCNUM,t.old_docdate as PREV_DOCDATE
 from (select top (5) *from POL_FILES order by id desc) f
 left join @tt t
 on replace(f.FILENAME,right(filename,4),'')=t.fname", Properties.Settings.Default.DocExchangeConnectionString);
                    inform_grid.ItemsSource = peopleList;
                    //pers_grid.GroupBy(pers_grid.Columns[0],DevExpress.Data.ColumnSortOrder.Descending);
                    //pers_grid.GroupBy(pers_grid.Columns[1], true);
                    //pers_grid.GroupBy(pers_grid.Columns[2], true);
                    inform_grid.GroupBy(inform_grid.Columns[0], DevExpress.Data.ColumnSortOrder.Descending);
                    inform_grid.GroupBy("FILENAME", true);
                    inform_grid.GroupBy("FDATE", true);
                    inform_grid.Columns["TIP_OP"].Width = 60;
                    inform_grid.Columns["FIO"].Width = 350;
                    inform_grid.Columns["DR"].Width = 100;
                }
                Cursor = Cursors.Arrow;
            }));

        }

        private void Informed_Closed(object sender, EventArgs e)
        {
           this.Close();
        }

        private void Adder_Click_3(object sender, RoutedEventArgs e)
        {

            //Vars.IDSZ = Funcs.MyIds(inform_grid1.GetSelectedRowHandles(), inform_grid1);
            //inform_grid1.GetSelectedRowHandles();
            var id = (int)inform_grid.GetFocusedRowCellValue("ID");
            
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                var con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand($@"INSERT INTO POL_PERSONS_INFORM (PERSON_ID,PERSONGUID,Month_P3,Year_P3,Theme_P3,Date_P3,SPOSOB_P3,RESULT_P3,VID_P3,PRIMECH) 
VALUES ({id},(select idguid from pol_persons where id={id}),{month_p3.EditValue??"null"},{Year_p3.EditValue},'{Theme_p3.EditValue ?? "null"}',
'{Date_evd_p3.EditValue ?? "null"}','{Sposob_p3.EditValue ?? "null"}','{Result_p3.EditValue ?? "null"}','{Vid_meropr_p3.EditValue ?? "null"}','{Primech_p3.Text}')", con);

            con.Open();
                com.ExecuteNonQuery();
                con.Close();
                string m = "ЗЛ успешно проинформированы! Приложение 3";
                string t = "Сообщение!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
            
            
        }
        private void Adder_Click_4(object sender, RoutedEventArgs e)
        {
            //Vars.IDSZ = Funcs.MyIds(inform_grid.GetSelectedRowHandles(), inform_grid);
            var id = (int)inform_grid.GetFocusedRowCellValue("ID");
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            var con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand($@"INSERT INTO POL_PERSONS_INFORM (PERSON_ID,PERSONGUID,Year_P4,Month_1_P4,Month_2_P4,Month_3_P4,Month_4_P4,Theme_P4,Date_P4,SPOSOB_P4,RESULT_P4,SOGLASIE_P4,MKB_P4,PRIMECH) 
VALUES ({id},(select idguid from pol_persons where id={id}),{Year_p4.EditValue},{Month1.EditValue??"null"},{Month2.EditValue??"null"},
{Month3.EditValue??"null"},{Month4.EditValue??"null"},{Tema_yved_p4.EditValue??"null"},'{dateyved_p4.EditValue??"null"}',{Sposob_p4.EditValue??"null"},
'{Result_p4.EditValue??"null"}',{Convert.ToInt32(Soglasie.EditValue)},'{MKB.EditValue??"null"}','{Primech_p4.Text}')", con);
            //var ll = MKB_combo.EditValue.ToString();
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
            string m = "ЗЛ успешно проинформированы! Приложение 4";
            string t = "Сообщение!";
            int b = 1;
            Message me = new Message(m, t, b);
            me.ShowDialog();

        }
        private void Del_file_btn_Click(object sender, RoutedEventArgs e)
        {            
            List<int> files_id = new List<int>();
            List<string> f_names = new List<string>();

            string sg_rows = " ";
            int[] zzz = inform_grid.GetSelectedRowHandles();
            
            for(int z=0;z<zzz.Count();z++)
            {
                if(inform_grid.IsGroupRowHandle(zzz[z]))
                {
                    files_id.Add((int)inform_grid.GetGroupRowValue(zzz[z], inform_grid.Columns[0]));
                    f_names.Add(inform_grid.GetGroupRowValue(zzz[z],inform_grid.Columns[1]).ToString());
                }
            }
            
            //int[] rt = files_id.ToArray();
            
            //for (int i = 0; i < rt.Count(); i++)
            //{
            //    var ddd = inform_grid.GetCellValue(rt[i], "ID");
            //    var sgr = sg_rows.Insert(sg_rows.Length, ddd.ToString()) + ",";
            //    sg_rows = sgr;
            //}

            //sg_rows = sg_rows.Substring(0, sg_rows.Length - 1);
            string m = "Вы действительно хотите удалить файл(ы)?";
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
                for (int i = 0; i < files_id.Count; i++)
                {
                    var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                    SqlConnection con = new SqlConnection(connectionString);
                    SqlCommand comm = new SqlCommand($@" update POL_OPLIST set FILENAME='Некорретный файл удален' where FILENAME ='{f_names[i]}' 
delete from POL_FILES where ID ={files_id[i]} ", con);
                    con.Open();
                    comm.ExecuteNonQuery();
                    con.Close();
                }
                //int kol_zap = rt.Count();
                int kol_zap = files_id.Count();
                int lastnumber = kol_zap % 10;
                if (lastnumber > 1 && lastnumber < 5)
                {
                    string m1 = " Успешно удалено  " + files_id.Count() + " файла!";
                    string t1 = "Сообщение";
                    int b1 = 1;
                    Message me1 = new Message(m1, t1, b1);
                    me1.ShowDialog();
                    //MessageBox.Show(" Успешно удалено  " + rt.Count() + " записи!");
                }
                else if (lastnumber == 1)
                {
                    string m1 = " Успешно удалено  " + files_id.Count() + " файл!";
                    string t1 = "Сообщение";
                    int b1 = 1;
                    Message me1 = new Message(m1, t1, b1);
                    me1.ShowDialog();
                    //MessageBox.Show(" Успешно удалена  " + rt.Count() + " запись!");
                }
                else
                {
                    string m1 = " Успешно удалено  " + files_id.Count() + " файлов!";
                    string t1 = "Сообщение";
                    int b1 = 1;
                    Message me1 = new Message(m1, t1, b1);
                    me1.ShowDialog();
                    //MessageBox.Show(" Успешно удалено  " + rt.Count() + " записей!");
                }

                Informed_Loaded(this,e);
            }
        }

        private void All_files_Checked(object sender, RoutedEventArgs e)
        {
            
            if(start_d.EditValue==null || end_d.EditValue==null)
            {
                string m0 = "Вы не выбрали период! Будут загружены последние 5 файлов. Продолжить?";
                string t0 = "Внимание!";
                int b0 = 2;
                Message me0 = new Message(m0, t0, b0);
                me0.ShowDialog();
                if (Vars.mes_res == 1)
                {

                    All_files_Unchecked(this, e);
                    return;
                }
                else
                {
                    Vars.mes_res = 1;
                    return;
                }
            }
            else
            {
                Cursor = Cursors.Wait;
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                new Action(delegate ()
                {
                    this.Title = "Сформированные файлы выгрузки в ТФОМС (за период)";
                    //Adder.Visibility = Visibility.Hidden;
                    //Premiss_edt.Visibility = Visibility.Hidden;
                    //dateP.Visibility = Visibility.Hidden;
                    //del_btn_hist.Visibility = Visibility.Hidden;
                    Del_file_btn.Visibility = Visibility.Visible;
                    inform_grid.Visibility = Visibility.Visible;
                    var peopleList =
                    MyReader.MySelect<UNLOAD_FILES>(
                        $@"  DECLARE @t table(rn int,id int,fname nvarchar(50),fxml XML)
insert into @t (rn,id,fname,fxml) select  ROW_NUMBER() OVER(ORDER BY id) as rn, id,filename,fxml from POL_FILES 
where cast(FDATE as date) between '{start_d.EditValue}' and '{end_d.EditValue}' order by id desc
DECLARE @tt table(fname nvarchar(50),tip_op nvarchar(50),fio nvarchar(150), dr datetime,fio_old nvarchar(150),dr_old datetime,enp nvarchar(16),
vpolis int,spolis nvarchar(11), npolis nvarchar(20),dbeg datetime,dend datetime,dstop datetime,
doctype int,docser nvarchar(20),docnum nvarchar(20),docdate datetime,ddoctype int,ddocser nvarchar(20),ddocnum nvarchar(20),ddocdate datetime,
old_doctype int,old_docser nvarchar(20),old_docnum nvarchar(20),old_docdate datetime)

declare @xml xml
declare @i int=0
while @i<=(select max(rn) from @t)
begin
set @xml=(select fxml from @t where rn=@i)
insert into @tt
select *from(select
 b.p.value('@FILENAME [1]', 'nvarchar(254)') as fname,
 t.x.value('../TIP_OP [1]', 'nvarchar(254)') as tip_op,
 isnull(t.x.value('./FAM [1]', 'nvarchar(254)'),'') +' '+
 isnull(t.x.value('./IM [1]', 'nvarchar(254)'),'') +' '+
 isnull(t.x.value('./OT [1]', 'nvarchar(254)'),'') as fio,
 t.x.value('./DR [1]', 'datetime') as dr,
 isnull(t.x.query('../OLD_PERSON/FAM').value('. [1]', 'nvarchar(254)'),'')+' '+
 isnull(t.x.query('../OLD_PERSON/IM').value('. [1]', 'nvarchar(254)'),'') +' '+
 isnull(t.x.query('../OLD_PERSON/OT').value('. [1]', 'nvarchar(254)'),'') as fio_old,
 (case when t.x.query('../OLD_PERSON/DR').value('. [1]', 'datetime')='' then null 
 else t.x.query('../OLD_PERSON/DR').value('. [1]', 'datetime') end)  as dr_old,
 t.x.query('../INSURANCE/ENP').value('. [1]', 'nvarchar(254)')  as enp,
 t.x.query('../INSURANCE/POLIS/VPOLIS').value('. [1]', 'int') as vpolis,
 t.x.query('../INSURANCE/POLIS/SPOLIS').value('. [1]', 'nvarchar(254)') as spolis,
 t.x.query('../INSURANCE/POLIS/NPOLIS').value('. [1]', 'nvarchar(254)') as npolis,
 t.x.query('../INSURANCE/POLIS/DBEG').value('. [1]', 'datetime') as dbeg,
 (case when t.x.query('../INSURANCE/POLIS/DEND').value('. [1]', 'datetime')='' then null
 else t.x.query('../INSURANCE/POLIS/DEND').value('. [1]', 'datetime') end) as dend,
 (case when t.x.query('../INSURANCE/POLIS/DSTOP').value('. [1]', 'datetime')='' then null
 else t.x.query('../INSURANCE/POLIS/DSTOP').value('. [1]', 'datetime') end) as dstop,
 t.x.query('../DOC_LIST/DOC[1]/DOCTYPE').value('. [1]', 'int') as doctype,
 t.x.query('../DOC_LIST/DOC[1]/DOCSER').value('. [1]', 'nvarchar(254)') as docser,
 t.x.query('../DOC_LIST/DOC[1]/DOCNUM').value('. [1]', 'nvarchar(254)') as docnum,
 (case when t.x.query('../DOC_LIST/DOC[1]/DOCDATE').value('. [1]', 'datetime')='' then null 
 else t.x.query('../DOC_LIST/DOC[1]/DOCDATE').value('. [1]', 'datetime') end) as docdate,
 (case when t.x.query('../DOC_LIST/DOC[2]/DOCTYPE').value('. [1]', 'int')='' then null
 else t.x.query('../DOC_LIST/DOC[2]/DOCTYPE').value('. [1]', 'int') end) as ddoctype,
 t.x.query('../DOC_LIST/DOC[2]/DOCSER').value('. [1]', 'nvarchar(254)') as ddocser,
 t.x.query('../DOC_LIST/DOC[2]/DOCNUM').value('. [1]', 'nvarchar(254)') as ddocnum,
 (case when t.x.query('../DOC_LIST/DOC[2]/DOCDATE').value('. [1]', 'datetime')='' then null 
 else t.x.query('../DOC_LIST/DOC[2]/DOCDATE').value('. [1]', 'datetime')end) as ddocdate,
 (case when t.x.query('../OLDDOC_LIST/OLD_DOC/DOCTYPE').value('. [1]', 'int')='' then null
 else t.x.query('../OLDDOC_LIST/OLD_DOC/DOCTYPE').value('. [1]', 'int') end) as old_doctype,
 t.x.query('../OLDDOC_LIST/OLD_DOC/DOCSER').value('. [1]', 'nvarchar(254)') as old_docser,
 t.x.query('../OLDDOC_LIST/OLD_DOC/DOCNUM').value('. [1]', 'nvarchar(254)') as old_docnum,
 (case when t.x.query('../OLDDOC_LIST/OLD_DOC/DOCDATE').value('. [1]', 'datetime')='' then null
 else t.x.query('../OLDDOC_LIST/OLD_DOC/DOCDATE').value('. [1]', 'datetime') end) as old_docdate
 from  @xml.nodes('/OPLIST/OP/PERSON') t(x)
 --cross apply @xml.nodes('/OPLIST/OP') a(w)
 cross apply @xml.nodes('/OPLIST') b(p))T0
 group by fname,tip_op,fio,dr,fio_old,dr_old,enp,vpolis,spolis,npolis,dbeg,dend,dstop,doctype,docser,docnum,docdate,ddoctype,ddocser,ddocnum,ddocdate,
 old_doctype,old_docser,old_docnum,old_docdate
 set @i=@i+1
 end 
 --select*from @tt
 select f.id as ID,f.FILENAME,f.FDATE,t.tip_op as TIP_OP,t.fio as FIO,t.dr as DR,t.fio_old as PREV_FIO,t.dr_old as PREV_DR,t.enp as ENP,
 t.vpolis as VPOLIS,t.spolis as SPOLIS,t.npolis as NPOLIS,t.dbeg as DBEG,t.dend as DEND,t.dstop as DSTOP,
 t.doctype as DOCTYPE,t.docser as DOCSER,t.docnum as DOCNUM,t.docdate as DOCDATE,t.ddoctype as DDOCTYPE,t.ddocser as DDOCSER,t.ddocnum as DDOCNUM,t.ddocdate as DDOCDATE,
 t.old_doctype as PREV_DOCTYPE,t.old_docser as PREV_DOCSER,t.old_docnum as PREV_DOCNUM,t.old_docdate as PREV_DOCDATE
 from (select  *from POL_FILES where cast(FDATE as date) between '{start_d.EditValue}' and '{end_d.EditValue}' ) f
 left join @tt t
 on replace(f.FILENAME,right(filename,4),'')=t.fname", Properties.Settings.Default.DocExchangeConnectionString);
                    inform_grid.ItemsSource = peopleList;
                    //inform_grid.GroupBy(inform_grid.Columns[0],DevExpress.Data.ColumnSortOrder.Descending);
                    //inform_grid.GroupBy(inform_grid.Columns[1], true);
                    //inform_grid.GroupBy(inform_grid.Columns[2], true);
                    inform_grid.GroupBy(inform_grid.Columns[0], DevExpress.Data.ColumnSortOrder.Descending);
                    inform_grid.GroupBy("FILENAME", true);
                    inform_grid.GroupBy("FDATE", true);
                    inform_grid.Columns["TIP_OP"].Width = 60;
                    inform_grid.Columns["FIO"].Width = 350;
                    inform_grid.Columns["DR"].Width = 100;
                    Cursor = Cursors.Arrow;
                }));
            }

            
                
            
        }

        private void All_files_Unchecked(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
            new Action(delegate ()
            {
                Title = "Сформированные файлы выгрузки в ТФОМС (последние 5 файлов)";
                //Adder.Visibility = Visibility.Hidden;
                //Premiss_edt.Visibility = Visibility.Hidden;
                //dateP.Visibility = Visibility.Hidden;
                //del_btn_hist.Visibility = Visibility.Hidden;
                Del_file_btn.Visibility = Visibility.Visible;
                inform_grid.Visibility = Visibility.Visible;
                all_files.Visibility = Visibility.Visible;
                var peopleList =
                MyReader.MySelect<UNLOAD_FILES>(
                    $@"  DECLARE @t table(rn int,id int,fname nvarchar(50),fxml XML)
insert into @t (rn,id,fname,fxml) select top(5) ROW_NUMBER() OVER(ORDER BY id) as rn, id,filename,fxml from POL_FILES order by id desc
DECLARE @tt table(fname nvarchar(50),tip_op nvarchar(50),fio nvarchar(150), dr datetime,fio_old nvarchar(150),dr_old datetime,enp nvarchar(16),
vpolis int,spolis nvarchar(11), npolis nvarchar(11),dbeg datetime,dend datetime,dstop datetime,
doctype int,docser nvarchar(20),docnum nvarchar(20),docdate datetime,ddoctype int,ddocser nvarchar(20),ddocnum nvarchar(20),ddocdate datetime,
old_doctype int,old_docser nvarchar(20),old_docnum nvarchar(20),old_docdate datetime)

declare @xml xml
declare @i int=0
while @i<=(select max(rn) from @t)
begin
set @xml=(select fxml from @t where rn=@i)
insert into @tt
select *from(select
 b.p.value('@FILENAME [1]', 'nvarchar(254)') as fname,
 t.x.value('../TIP_OP [1]', 'nvarchar(254)') as tip_op,
 isnull(t.x.value('./FAM [1]', 'nvarchar(254)'),'') +' '+
 isnull(t.x.value('./IM [1]', 'nvarchar(254)'),'') +' '+
 isnull(t.x.value('./OT [1]', 'nvarchar(254)'),'') as fio,
 t.x.value('./DR [1]', 'datetime') as dr,
 isnull(t.x.query('../OLD_PERSON/FAM').value('. [1]', 'nvarchar(254)'),'')+' '+
 isnull(t.x.query('../OLD_PERSON/IM').value('. [1]', 'nvarchar(254)'),'') +' '+
 isnull(t.x.query('../OLD_PERSON/OT').value('. [1]', 'nvarchar(254)'),'') as fio_old,
 (case when t.x.query('../OLD_PERSON/DR').value('. [1]', 'datetime')='' then null 
 else t.x.query('../OLD_PERSON/DR').value('. [1]', 'datetime') end)  as dr_old,
 t.x.query('../INSURANCE/ENP').value('. [1]', 'nvarchar(254)')  as enp,
 t.x.query('../INSURANCE/POLIS/VPOLIS').value('. [1]', 'int') as vpolis,
 t.x.query('../INSURANCE/POLIS/SPOLIS').value('. [1]', 'nvarchar(254)') as spolis,
 t.x.query('../INSURANCE/POLIS/NPOLIS').value('. [1]', 'nvarchar(254)') as npolis,
 t.x.query('../INSURANCE/POLIS/DBEG').value('. [1]', 'datetime') as dbeg,
 (case when t.x.query('../INSURANCE/POLIS/DEND').value('. [1]', 'datetime')='' then null
 else t.x.query('../INSURANCE/POLIS/DEND').value('. [1]', 'datetime') end) as dend,
 (case when t.x.query('../INSURANCE/POLIS/DSTOP').value('. [1]', 'datetime')='' then null
 else t.x.query('../INSURANCE/POLIS/DSTOP').value('. [1]', 'datetime') end) as dstop,
 t.x.query('../DOC_LIST/DOC[1]/DOCTYPE').value('. [1]', 'int') as doctype,
 t.x.query('../DOC_LIST/DOC[1]/DOCSER').value('. [1]', 'nvarchar(254)') as docser,
 t.x.query('../DOC_LIST/DOC[1]/DOCNUM').value('. [1]', 'nvarchar(254)') as docnum,
 (case when t.x.query('../DOC_LIST/DOC[1]/DOCDATE').value('. [1]', 'datetime')='' then null 
 else t.x.query('../DOC_LIST/DOC[1]/DOCDATE').value('. [1]', 'datetime') end) as docdate,
 (case when t.x.query('../DOC_LIST/DOC[2]/DOCTYPE').value('. [1]', 'int')='' then null
 else t.x.query('../DOC_LIST/DOC[2]/DOCTYPE').value('. [1]', 'int') end) as ddoctype,
 t.x.query('../DOC_LIST/DOC[2]/DOCSER').value('. [1]', 'nvarchar(254)') as ddocser,
 t.x.query('../DOC_LIST/DOC[2]/DOCNUM').value('. [1]', 'nvarchar(254)') as ddocnum,
 (case when t.x.query('../DOC_LIST/DOC[2]/DOCDATE').value('. [1]', 'datetime')='' then null 
 else t.x.query('../DOC_LIST/DOC[2]/DOCDATE').value('. [1]', 'datetime')end) as ddocdate,
 (case when t.x.query('../OLDDOC_LIST/OLD_DOC/DOCTYPE').value('. [1]', 'int')='' then null
 else t.x.query('../OLDDOC_LIST/OLD_DOC/DOCTYPE').value('. [1]', 'int') end) as old_doctype,
 t.x.query('../OLDDOC_LIST/OLD_DOC/DOCSER').value('. [1]', 'nvarchar(254)') as old_docser,
 t.x.query('../OLDDOC_LIST/OLD_DOC/DOCNUM').value('. [1]', 'nvarchar(254)') as old_docnum,
 (case when t.x.query('../OLDDOC_LIST/OLD_DOC/DOCDATE').value('. [1]', 'datetime')='' then null
 else t.x.query('../OLDDOC_LIST/OLD_DOC/DOCDATE').value('. [1]', 'datetime') end) as old_docdate
 from  @xml.nodes('/OPLIST/OP/PERSON') t(x)
 --cross apply @xml.nodes('/OPLIST/OP') a(w)
 cross apply @xml.nodes('/OPLIST') b(p))T0
 group by fname,tip_op,fio,dr,fio_old,dr_old,enp,vpolis,spolis,npolis,dbeg,dend,dstop,doctype,docser,docnum,docdate,ddoctype,ddocser,ddocnum,ddocdate,
 old_doctype,old_docser,old_docnum,old_docdate
 set @i=@i+1
 end 
 --select*from @tt
 select f.id as ID,f.FILENAME,f.FDATE,t.tip_op as TIP_OP,t.fio as FIO,t.dr as DR,t.fio_old as PREV_FIO,t.dr_old as PREV_DR,t.enp as ENP,
 t.vpolis as VPOLIS,t.spolis as SPOLIS,t.npolis as NPOLIS,t.dbeg as DBEG,t.dend as DEND,t.dstop as DSTOP,
 t.doctype as DOCTYPE,t.docser as DOCSER,t.docnum as DOCNUM,t.docdate as DOCDATE,t.ddoctype as DDOCTYPE,t.ddocser as DDOCSER,t.ddocnum as DDOCNUM,t.ddocdate as DDOCDATE,
 t.old_doctype as PREV_DOCTYPE,t.old_docser as PREV_DOCSER,t.old_docnum as PREV_DOCNUM,t.old_docdate as PREV_DOCDATE
 from (select top (5) *from POL_FILES order by id desc) f
 left join @tt t
 on replace(f.FILENAME,right(filename,4),'')=t.fname", Properties.Settings.Default.DocExchangeConnectionString);
                inform_grid.ItemsSource = peopleList;
                //inform_grid.GroupBy(inform_grid.Columns[0],DevExpress.Data.ColumnSortOrder.Descending);
                //inform_grid.GroupBy(inform_grid.Columns[1], true);
                //inform_grid.GroupBy(inform_grid.Columns[2], true);
                inform_grid.GroupBy(inform_grid.Columns[0], DevExpress.Data.ColumnSortOrder.Descending);
                inform_grid.GroupBy("FILENAME", true);
                inform_grid.GroupBy("FDATE", true);
                inform_grid.Columns["TIP_OP"].Width = 60;
                inform_grid.Columns["FIO"].Width = 350;
                inform_grid.Columns["DR"].Width = 100;
                Cursor = Cursors.Arrow;
            }));
        }

        private void inform_grid_AutoGeneratedColumns(object sender, RoutedEventArgs e)
        {
            G_layuot.layout_InUse(inform_grid);
            inform_grid.FilterString = "";
        }
        private void Files_ItemClick(object sender, RoutedEventArgs e)
        {
            //            List<UNLOAD_FILES> UFS = new List<UNLOAD_FILES>();
            
            var names = Funcs.MyFieldValues(inform_grid.GetSelectedRowHandles(), inform_grid, "FNAME");
            var files = MyReader.MySelect<UNLOAD_FILES_S>($@"select fxml ,fdate from POL_FILES where 
            filename in(select*from [dbo].[GetNamesFromString]('{names}',','))", Properties.Settings.Default.DocExchangeConnectionString);
            this.Title = "Сформированные файлы выгрузки в ТФОМС (за период)";

            //            foreach (var file in files)
            //            {
            //                XmlReader nodeReader;
            //                XmlDocument xDoc = new XmlDocument();
            //                using (nodeReader = XmlReader.Create(new StringReader(file.FXML)))
            //                {
            //                    // the reader must be in the Interactive state in order to  
            //                    // Create a LINQ to XML tree from it.  
            //                    nodeReader.MoveToContent();
            //                    xDoc.Load(nodeReader);

            //                }
            //                XmlDocument doc = new XmlDocument();
            //                doc.Load(new StringReader(file.FXML));
            //                DataTable Dt = new DataTable(Name);
            //                try
            //                {

            //                    XmlNode NodoEstructura = doc.FirstChild.FirstChild;
            //                    //  Table structure (columns definition) 
            //                    foreach (XmlNode columna in NodoEstructura.ChildNodes)
            //                    {
            //                        if(columna.HasChildNodes==true && columna.FirstChild.FirstChild!=null)
            //                        {
            //                            foreach(XmlNode ccol in columna.ChildNodes)
            //                            {
            //                                if(Dt.Columns.Contains(ccol.Name))
            //                                {
            //                                    Dt.Columns.Add(ccol.Name+"1", typeof(String));
            //                                }
            //                                else
            //                                {
            //                                    Dt.Columns.Add(ccol.Name, typeof(String));
            //                                }

            //                            }

            //                        }
            //                        else
            //                        {
            //                            Dt.Columns.Add(columna.Name, typeof(String));
            //                        }

            //                    }

            //                    XmlNode Filas = doc.FirstChild;
            //                    //  Data Rows 
            //                    foreach (XmlNode Fila in Filas.ChildNodes)
            //                    {
            //                        List<string> Valores = new List<string>();
            //                        foreach (XmlNode Columna in Fila.ChildNodes)
            //                        {
            //                            Valores.Add(Columna.InnerText);
            //                        }
            //                        Dt.Rows.Add(Valores.ToArray());
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    MessageBox.Show(ex.Message);
            //                }





            //                XmlElement xRoot = xDoc.DocumentElement;
            //                XmlNode op = xRoot.SelectSingleNode("OP");
            //                XmlNode pers = op.SelectSingleNode("PERSON");
            //                XmlNode ins = op.SelectSingleNode("INSURANCE");
            //                XmlNode ins_polis = ins.SelectSingleNode("POLIS");
            //                XmlNode doc_list = op.SelectSingleNode("DOC_LIST");
            //                XmlNode old_pers = op.SelectSingleNode("OLD_PERSON");


            //                foreach (XmlNode xnode in xRoot)
            //                {
            //                    //if (xnode.Name == "OP")
            //                    //{
            //                    int doc_cnt = doc_list.ChildNodes.Count;
            //                    var rrr = doc_cnt == 1 ? Convert.ToInt32(null) : Convert.ToInt32(doc_list.LastChild.SelectSingleNode("./DOCTYPE").InnerText);
            //                    var rrr1 =  doc_cnt == 1 ? null : doc_list.LastChild.SelectSingleNode("./DOCSER").InnerText;
            //                    var rrr2 = doc_cnt == 1 ? null : doc_list.LastChild.SelectSingleNode("./DOCNUM").InnerText;
            //                    var rrr3 = old_pers == null ? "" : old_pers.SelectSingleNode("./FAM").InnerText + " " + old_pers.SelectSingleNode("./IM").InnerText + " " + old_pers.SelectSingleNode("./OT").InnerText;


            //                    UFS.Add(new UNLOAD_FILES {FILENAME=xRoot.Attributes.GetNamedItem("FILENAME").Value+".xml",TIP_OP= op.SelectSingleNode("./TIP_OP").InnerText,
            //                        FIO =pers.SelectSingleNode("./FAM").InnerText+" "+ pers.SelectSingleNode("./IM").InnerText + " "+ pers.SelectSingleNode("./OT").InnerText,
            //                        DR = Convert.ToDateTime(pers.SelectSingleNode("./DR").InnerText),

            //                           PREV_FIO = old_pers==null ? "" : old_pers.SelectSingleNode("./FAM").InnerText + " " + old_pers == null ? "" : old_pers.SelectSingleNode("./IM").InnerText + " " + old_pers == null ? "" : old_pers.SelectSingleNode("./OT").InnerText,
            //                           PREV_DR = Convert.ToDateTime(old_pers == null ? null : old_pers.SelectSingleNode("./DR").InnerText)

            //                       , ENP=ins.InnerXml.Contains("ENP") == false ? null : ins_polis.SelectSingleNode("./ENP").InnerText,
            //                        VPOLIS = Convert.ToInt32(ins_polis.SelectSingleNode("./VPOLIS").InnerText),NPOLIS= ins_polis.SelectSingleNode("./NPOLIS").InnerText,
            //                        DBEG = Convert.ToDateTime(ins_polis.InnerXml.Contains("DBEG") == false ? null : ins_polis.SelectSingleNode("./DBEG").InnerText),
            //                        DEND = Convert.ToDateTime(ins_polis.InnerXml.Contains("DEND") == false ? null : ins_polis.SelectSingleNode("./DEND").InnerText),
            //                        DSTOP = Convert.ToDateTime(ins_polis.InnerXml.Contains("DSTOP") == false ? null : ins_polis.SelectSingleNode("./DSTOP").InnerText),
            //                        DOCTYPE = Convert.ToInt32(doc_list.FirstChild.SelectSingleNode("./DOCTYPE").InnerText),
            //                        DOCSER = doc_list.FirstChild.SelectSingleNode("./DOCSER").InnerText,
            //                        DOCNUM = doc_list.FirstChild.SelectSingleNode("./DOCNUM").InnerText,
            //                        DOCDATE = Convert.ToDateTime(doc_list.FirstChild.SelectSingleNode("./DOCDATE").InnerText),
            //                        //DDOCTYPE= doc_cnt==1 ? Convert.ToInt32(null) : Convert.ToInt32(doc_list.LastChild.SelectSingleNode("./DOCTYPE").InnerText),
            //                        //DDOCSER = doc_cnt == 1 ? null : doc_list.LastChild.SelectSingleNode("./DOCSER").InnerText,
            //                        //DDOCNUM = doc_cnt == 1 ? null : doc_list.LastChild.SelectSingleNode("./DOCNUM").InnerText,
            //                        //DDOCDATE = doc_cnt == 1 ? Convert.ToDateTime(null) : Convert.ToDateTime(doc_list.LastChild.SelectSingleNode("./DOCDATE"))

            //                    });

            //                    //}
            //                    //// обходим все дочерние узлы элемента user
            //                    //foreach (XmlNode childnode in xnode.ChildNodes)
            //                    //{

            //                    //}
            //                }
            //            }
            //XmlElement xRoot = xDoc.DocumentElement;
            inform_grid.ItemsSource =
            MyReader.MySelect<UNLOAD_FILES>(
                $@"  DECLARE @t table(rn int,id int,fname nvarchar(50),fxml XML)
            insert into @t (rn,id,fname,fxml) select  ROW_NUMBER() OVER(ORDER BY id) as rn, id,filename,fxml from POL_FILES 
            where filename in(select*from [dbo].[GetNamesFromString]('{names}',',')) order by id desc
            DECLARE @tt table(fname nvarchar(50),tip_op nvarchar(50),fio nvarchar(150), dr datetime,fio_old nvarchar(150),dr_old datetime,enp nvarchar(16),
            vpolis int,spolis nvarchar(11), npolis nvarchar(20),dbeg datetime,dend datetime,dstop datetime,
            doctype int,docser nvarchar(20),docnum nvarchar(20),docdate datetime,ddoctype int,ddocser nvarchar(20),ddocnum nvarchar(20),ddocdate datetime,
            old_doctype int,old_docser nvarchar(20),old_docnum nvarchar(20),old_docdate datetime)

            declare @xml xml
            declare @i int=0
            while @i<=(select max(rn) from @t)
            begin
            set @xml=(select fxml from @t where rn=@i)
            insert into @tt
            select *from(select
             b.p.value('@FILENAME [1]', 'nvarchar(254)') as fname,
             t.x.value('../TIP_OP [1]', 'nvarchar(254)') as tip_op,
             isnull(t.x.value('./FAM [1]', 'nvarchar(254)'),'') +' '+
             isnull(t.x.value('./IM [1]', 'nvarchar(254)'),'') +' '+
             isnull(t.x.value('./OT [1]', 'nvarchar(254)'),'') as fio,
             t.x.value('./DR [1]', 'datetime') as dr,
             isnull(t.x.query('../OLD_PERSON/FAM').value('. [1]', 'nvarchar(254)'),'')+' '+
             isnull(t.x.query('../OLD_PERSON/IM').value('. [1]', 'nvarchar(254)'),'') +' '+
             isnull(t.x.query('../OLD_PERSON/OT').value('. [1]', 'nvarchar(254)'),'') as fio_old,
             (case when t.x.query('../OLD_PERSON/DR').value('. [1]', 'datetime')='' then null 
             else t.x.query('../OLD_PERSON/DR').value('. [1]', 'datetime') end)  as dr_old,
             t.x.query('../INSURANCE/ENP').value('. [1]', 'nvarchar(254)')  as enp,
             t.x.query('../INSURANCE/POLIS/VPOLIS').value('. [1]', 'int') as vpolis,
             t.x.query('../INSURANCE/POLIS/SPOLIS').value('. [1]', 'nvarchar(254)') as spolis,
             t.x.query('../INSURANCE/POLIS/NPOLIS').value('. [1]', 'nvarchar(254)') as npolis,
             t.x.query('../INSURANCE/POLIS/DBEG').value('. [1]', 'datetime') as dbeg,
             (case when t.x.query('../INSURANCE/POLIS/DEND').value('. [1]', 'datetime')='' then null
             else t.x.query('../INSURANCE/POLIS/DEND').value('. [1]', 'datetime') end) as dend,
             (case when t.x.query('../INSURANCE/POLIS/DSTOP').value('. [1]', 'datetime')='' then null
             else t.x.query('../INSURANCE/POLIS/DSTOP').value('. [1]', 'datetime') end) as dstop,
             t.x.query('../DOC_LIST/DOC[1]/DOCTYPE').value('. [1]', 'int') as doctype,
             t.x.query('../DOC_LIST/DOC[1]/DOCSER').value('. [1]', 'nvarchar(254)') as docser,
             t.x.query('../DOC_LIST/DOC[1]/DOCNUM').value('. [1]', 'nvarchar(254)') as docnum,
             (case when t.x.query('../DOC_LIST/DOC[1]/DOCDATE').value('. [1]', 'datetime')='' then null 
             else t.x.query('../DOC_LIST/DOC[1]/DOCDATE').value('. [1]', 'datetime') end) as docdate,
             (case when t.x.query('../DOC_LIST/DOC[2]/DOCTYPE').value('. [1]', 'int')='' then null
             else t.x.query('../DOC_LIST/DOC[2]/DOCTYPE').value('. [1]', 'int') end) as ddoctype,
             t.x.query('../DOC_LIST/DOC[2]/DOCSER').value('. [1]', 'nvarchar(254)') as ddocser,
             t.x.query('../DOC_LIST/DOC[2]/DOCNUM').value('. [1]', 'nvarchar(254)') as ddocnum,
             (case when t.x.query('../DOC_LIST/DOC[2]/DOCDATE').value('. [1]', 'datetime')='' then null 
             else t.x.query('../DOC_LIST/DOC[2]/DOCDATE').value('. [1]', 'datetime')end) as ddocdate,
             (case when t.x.query('../OLDDOC_LIST/OLD_DOC/DOCTYPE').value('. [1]', 'int')='' then null
             else t.x.query('../OLDDOC_LIST/OLD_DOC/DOCTYPE').value('. [1]', 'int') end) as old_doctype,
             t.x.query('../OLDDOC_LIST/OLD_DOC/DOCSER').value('. [1]', 'nvarchar(254)') as old_docser,
             t.x.query('../OLDDOC_LIST/OLD_DOC/DOCNUM').value('. [1]', 'nvarchar(254)') as old_docnum,
             (case when t.x.query('../OLDDOC_LIST/OLD_DOC/DOCDATE').value('. [1]', 'datetime')='' then null
             else t.x.query('../OLDDOC_LIST/OLD_DOC/DOCDATE').value('. [1]', 'datetime') end) as old_docdate
             from  @xml.nodes('/OPLIST/OP/PERSON') t(x)
             --cross apply @xml.nodes('/OPLIST/OP') a(w)
             cross apply @xml.nodes('/OPLIST') b(p))T0
             group by fname,tip_op,fio,dr,fio_old,dr_old,enp,vpolis,spolis,npolis,dbeg,dend,dstop,doctype,docser,docnum,docdate,ddoctype,ddocser,ddocnum,ddocdate,
             old_doctype,old_docser,old_docnum,old_docdate
             set @i=@i+1
             end 
             --select*from @tt
             select f.id as ID,f.FILENAME,f.FDATE,t.tip_op as TIP_OP,t.fio as FIO,t.dr as DR,t.fio_old as PREV_FIO,t.dr_old as PREV_DR,t.enp as ENP,
             t.vpolis as VPOLIS,t.spolis as SPOLIS,t.npolis as NPOLIS,t.dbeg as DBEG,t.dend as DEND,t.dstop as DSTOP,
             t.doctype as DOCTYPE,t.docser as DOCSER,t.docnum as DOCNUM,t.docdate as DOCDATE,t.ddoctype as DDOCTYPE,t.ddocser as DDOCSER,t.ddocnum as DDOCNUM,t.ddocdate as DDOCDATE,
             t.old_doctype as PREV_DOCTYPE,t.old_docser as PREV_DOCSER,t.old_docnum as PREV_DOCNUM,t.old_docdate as PREV_DOCDATE
             from (select  *from POL_FILES where filename in(select*from [dbo].[GetNamesFromString]('{names}',',')) ) f
             left join @tt t
             on replace(f.FILENAME,right(filename,4),'')=t.fname", Properties.Settings.Default.DocExchangeConnectionString);

            inform_grid.GroupBy(inform_grid.Columns[0], DevExpress.Data.ColumnSortOrder.Descending);
            inform_grid.GroupBy(inform_grid.Columns[1], true);
            inform_grid.GroupBy(inform_grid.Columns[2], true);
            inform_grid.GroupBy(inform_grid.Columns[0], DevExpress.Data.ColumnSortOrder.Descending);
            inform_grid.GroupBy("FILENAME", true);
            inform_grid.GroupBy("FDATE", true);
            inform_grid.Columns["TIP_OP"].Width = 60;
            inform_grid.Columns["FIO"].Width = 350;
            inform_grid.Columns["DR"].Width = 100;
        }

        private void Adder_Copy1_Click(object sender, RoutedEventArgs e)
        {
            Vars.IDSZ = Funcs.MyIds(inform_grid.GetSelectedRowHandles(), inform_grid);
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            var con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand($@"delete from POL_PERSONS_INFORM where id = {Vars.IDSZ}", con);
            //var ll = MKB_combo.EditValue.ToString();
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
            string m = "Информирование успешно удалено!";
            string t = "Сообщение!";
            int b = 1;
            Message me = new Message(m, t, b);
            me.ShowDialog();
        }

        private void TableView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            fam_p3.Text= inform_grid.GetFocusedRowCellValue("FAM").ToString();
            fam_p4.Text = inform_grid.GetFocusedRowCellValue("FAM").ToString();
            im_p3.Text = inform_grid.GetFocusedRowCellValue("IM").ToString();
            im_p4.Text = inform_grid.GetFocusedRowCellValue("IM").ToString();
            ot_p3.Text = inform_grid.GetFocusedRowCellValue("OT").ToString();
            ot_p4.Text = inform_grid.GetFocusedRowCellValue("OT").ToString();

            var id = inform_grid.GetFocusedRowCellValue("ID");
            var peopleList = MyReader.MySelect<INFORM_ALL>($@"  select t0.ID_TFOMS,SURNAME,NAME,SECNAME,DR,POL,SNILS,DPFS_3,SN_POL_3,VIDPROF_3,Tema_3,DATE_UV_3,sposob_3,result_3,prim_3,
KMKB, PM1,  PM2,  PM3,  PM4, DPFS_4,SN_POL_4,  sogl_4,  tema_4,Date_uv_4, sposob_4, result_4, prim_4 
 from (select p.id, pp.id as ID_TFOMS,fam as SURNAME,im as NAME,ot as SECNAME,
pp.dr as DR, w as POL, pp.SS as SNILS,pol.VPOLIS as DPFS_3,
(case when pol.VPOLIS=3 then ENP else pol.SPOLIS+' '+pol.NPOLIS end) as SN_POL_3,vp3.Name as VIDPROF_3, 
tp3.Name as Tema_3,Date_P3 as DATE_UV_3,
sps.Name as sposob_3,rp3.Name as result_3,PRIMECH as prim_3
 from POL_PERSONS_INFORM p
 join POL_PERSONS pp on p.PERSONGUID=pp.IDGUID
 left join lpu_39 l on pp.MO=l.MCOD
 left join POL_POLISES pol on pp.EVENT_GUID=pol.EVENT_GUID
 left join THEME_INFORM_P3 tp3 on p.Theme_P3=tp3.ID
 left join RESULT_INFORM_P3 rp3 on p.RESULT_P3=rp3.ID
 left join SPOSOB_INFORM sps on p.SPOSOB_P3=sps.ID
 left join VID_MEROPR_INFORM_P3 vp3 on p.VID_P3=vp3.ID
 where  p.PERSON_ID={id})
  T0
 left join
 (select p.id, pp.id as ID_TFOMS, mkb_p4 as KMKB,
Month_1_P4 as PM1, Month_2_P4 as PM2, Month_3_P4 as PM3, Month_4_P4 as PM4,
pol.VPOLIS as DPFS_4,(case when pol.VPOLIS=3 then ENP else pol.SPOLIS+' '+pol.NPOLIS end)as SN_POL_4, 
soglasie_p4 as sogl_4, tp4.Name as tema_4,Date_P4 as Date_uv_4,
sps.Name as sposob_4,rp4.Name as result_4,PRIMECH as prim_4
 from POL_PERSONS_INFORM p
 join POL_PERSONS pp on p.PERSONGUID=pp.IDGUID
 left join lpu_39 l on pp.MO=l.MCOD
 left join POL_POLISES pol on pp.EVENT_GUID=pol.EVENT_GUID 
 left join THEME_INFORM_P4 tp4 on p.Theme_P4=tp4.ID
 left join RESULT_INFORM_P3 rp4 on p.RESULT_P4=rp4.ID
 left join SPOSOB_INFORM sps on p.SPOSOB_P3=sps.ID
 where p.PERSON_ID={id})T1 on t0.id=T1.id ", Properties.Settings.Default.DocExchangeConnectionString);
            inform_grid1.ItemsSource = peopleList;
            inform_grid1.View.FocusedRowHandle = -1;
        }

        private void Unload_3_Click(object sender, RoutedEventArgs e)
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

        private void Unload_4_Click(object sender, RoutedEventArgs e)
        {
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
    }
    
    
}
