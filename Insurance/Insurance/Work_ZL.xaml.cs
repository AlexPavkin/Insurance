using DevExpress.Xpf.Editors.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Yamed.Server;
using System.Threading;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.PivotGrid;
using Yamed.Core;
using System.Data.SqlClient;

namespace Insurance
{
    //public class People
    //{        
    //    public Int32 ID { get; set; }
    //    public  Guid IDGUID { get; set; }
    //    [System.ComponentModel.DisplayName("Комментарий")]
    //    public string COMMENT { get; set; }
    //    [System.ComponentModel.DisplayName("Файл выгрузки")]
    //    public string FILENAME { get; set; }
    //    [System.ComponentModel.DisplayName("Активна (да/нет)")]
    //    public bool ACTIVE { get; set; }
    //    [System.ComponentModel.DisplayName("Причина подачи заявления")]
    //    public string TIP_OP { get; set; }
    //    [System.ComponentModel.DisplayName("ЕНП")]
    //    public string ENP { get; set; }
    //    [System.ComponentModel.DisplayName("СНИЛС")]
    //    public string SS { get; set; }
    //    [System.ComponentModel.DisplayName("Фамилия")]
    //    public string FAM { get; set; }
    //    [System.ComponentModel.DisplayName("Имя")]
    //    public string IM { get; set; }
    //    [System.ComponentModel.DisplayName("Отчество")]
    //    public string OT { get; set; }        
    //    [System.ComponentModel.DisplayName("Пол")]
    //    public Int32 W { get; set; }
    //    [System.ComponentModel.DisplayName("Дата рождения")]
    //    public DateTime? DR { get; set; }
    //    [System.ComponentModel.DisplayName("Вид документа")]
    //    public int DOCTYPE { get; set; }
    //    [System.ComponentModel.DisplayName("Серия документа")]
    //    public string DOCSER { get; set; }
    //    [System.ComponentModel.DisplayName("Номер документа")]
    //    public string DOCNUM { get; set; }
    //    [System.ComponentModel.DisplayName("Телефон")]
    //    public string PHONE { get; set; }
    //    [System.ComponentModel.DisplayName("Вид полиса")]
    //    public int VPOLIS { get; set; }
    //    [System.ComponentModel.DisplayName("Серия полиса")]
    //    public string SPOLIS { get; set; }
    //    [System.ComponentModel.DisplayName("Номер полиса")]
    //    public string NPOLIS { get; set; }
    //    [System.ComponentModel.DisplayName("Дата начала")]
    //    public DateTime? DBEG { get; set; }
    //    [System.ComponentModel.DisplayName("Дата получения")]
    //    public DateTime? DRECEIVED { get; set; }
    //    [System.ComponentModel.DisplayName("Дата окончания")]
    //    public DateTime? DEND { get; set; }
    //    [System.ComponentModel.DisplayName("Дата прекращения")]
    //    public DateTime? DSTOP { get; set; }
    //    [System.ComponentModel.DisplayName("Прикрепление")]
    //    public string NameWithID { get; set; }



    //}
    //public class R001
    //{
    //    public string Kod { get; set; }
    //    public string NameWithID { get; set; }
    //}
    //public class R003
    //{
    //    public int ID { get; set; }
    //    public string NameWithID { get; set; }
    //}
    //public class V013
    //{
    //    public int IDKAT { get; set; }
    //    public string NameWithID { get; set; }
    //}
    //public class F008
    //{
    //    public int ID { get; set; }
    //    public string NameWithID { get; set; }
    //}
    //public class F011
    //{
    //    public int ID { get; set; }
    //    public string NameWithID { get; set; }
    //}
    //public class V005
    //{
    //    public int IDPOL { get; set; }
    //    public string NameWithID { get; set; }
    //}
    //public class OKSM
    //{
    //    public string ID { get; set; }
    //    public string CAPTION { get; set; }
    //}
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Work_ZL : Window
    {


//        public Work_ZL()
//        {
//            InitializeComponent();
//            WindowState = WindowState.Maximized;
//            d_obr.DateTime = DateTime.Now;
//            cel_vizita.DataContext = MyReader.MySelect<R001>(@"select Kod,NameWithID from R001", Properties.Settings.Default.DocExchangeConnectionString);
//            sp_pod_z.DataContext = MyReader.MySelect<R003>(@"select ID,NameWithID from R003", Properties.Settings.Default.DocExchangeConnectionString);

//        }


//        string cel_viz;
//        private void dalee_Click(object sender, RoutedEventArgs e)
//        {
            
//            string petit;
//            if (petition.IsChecked == true)
//            {
//                petit = "1";
//            }
//            else
//            {
//                petit = "0";
//            }
//            if (pers_grid_1.SelectedItems.Count != 0)
//            {
//                string iid;
//                string btn = "3";

//                iid = pers_grid_1.GetFocusedRowCellValue("ID").ToString();


//                string ppol = pers_grid_1.GetFocusedRowCellValue("W").ToString();


//                if (cel_vizita.SelectedIndex == -1)
//                {
//                    MessageBox.Show("Выберите причину внесения изменений в РС ЕРЗ!");
                    
//                    return;
//                }
//                else
//                {
//                    Vars.CelVisit = cel_vizita.EditValue.ToString();
//                    cel_viz = cel_vizita.EditValue.ToString();
//                }

//                string sppz;
//                if (sp_pod_z.EditValue == null)
//                {
//                    MessageBox.Show("Выберите способ подачи заявления!");
//                    return;
//                }
//                else
//                {
//                    Vars.Sposob = sp_pod_z.EditValue.ToString();
//                    sppz = sp_pod_z.EditValue.ToString();
//                }

//                string dobr = d_obr.ToString();
//                Person_Data w2 = new Person_Data(iid, btn, ppol, cel_viz, sppz, dobr, petit);
//                w2.Owner = this;
//                //this.Visibility = Visibility.Collapsed;
//                w2.Show();
//               // this.Visibility = Visibility.Visible;
//                //this.Close();

//            }
//            else
//            {
//                MessageBox.Show("Выберите клиента");
//            }
//            Vars.DateVisit = d_obr.DateTime;
//        }

//        private void pers_grid_Loaded(object sender, RoutedEventArgs e)
//        {




//        }

//        private void pers_grid_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
//        {


//        }

//        private void cel_vizita_SelectedIndexChanged(object sender, RoutedEventArgs e)
//        {


//        }




//        private void izm_klient_Click(object sender, RoutedEventArgs e)
//        {
//            if(cel_vizita.SelectedIndex==-1 && pers_grid_1.SelectedItems.Count==0)
//            {
//                MessageBox.Show("Выберите клиента!");
//                return;
//            }
//            else if (cel_vizita.SelectedIndex == -1 && pers_grid_1.SelectedItems.Count != 0)
//            {
//                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
//                SqlConnection con = new SqlConnection(connectionString);
//                SqlCommand comm = new SqlCommand(@"select tip_op from pol_events where person_guid=(select idguid from pol_persons where id=@id)", con);
//            comm.Parameters.AddWithValue("@id", pers_grid_1.GetFocusedRowCellValue("ID").ToString());
//            con.Open();
//            SqlDataReader reader = comm.ExecuteReader();

//            while (reader.Read()) // построчно считываем данные
//            {
//                object tip_op = reader["TIP_OP"];

//                    Vars.CelVisit = tip_op.ToString();
//                    Vars.NewCelViz = 0;
//            }

//            reader.Close();

//            con.Close();
//                //MessageBox.Show("Выберите причину подачи заявления!");
//                //return;

//            }
//            else
//            {
//                Vars.CelVisit = cel_vizita.EditValue.ToString();
//                Vars.NewCelViz = 1;
//            }
//            Events Create = new Events();
//            Create.DORDER = DateTime.Now;
//            string petit;
//            if (petition.IsChecked == true)
//            {
//                petit = "1";
//            }
//            else
//            {
//                petit = "0";
//            }
//            if (pers_grid_1.SelectedItems.Count != 0)
//            {
//                string btn = "2";

//                string iid = pers_grid_1.GetFocusedRowCellValue("ID").ToString();
//                string ppol = pers_grid_1.GetFocusedRowCellValue("W").ToString();
//                string cel_viz;
//                if (cel_vizita.SelectedIndex == -1)
//                {
//                    cel_viz = "";
//                }
//                else
//                {
//                    cel_viz = cel_vizita.EditValue.ToString();
//                }

//                string sppz;
//                if (sp_pod_z.EditValue == null)
//                {
//                    sppz = "0";
//                }
//                else
//                {
//                    sppz = sp_pod_z.EditValue.ToString();
//                }
//                string dobr = d_obr.ToString();
//                Person_Data w2 = new Person_Data(iid, btn, ppol, cel_viz, sppz, dobr, petit);
//                w2.Owner = this;
//                //this.Visibility = Visibility.Collapsed;
//                w2.Show();
//                //this.Visibility = Visibility.Visible;
//                //this.Close();

//            }
//            else
//            {
//                MessageBox.Show("Выберите клиента");
//                return;
//            }
//        }

//        private void new_klient_Click(object sender, RoutedEventArgs e)
//        {
            
            
//            string petit;
//            if (petition.IsChecked == true)
//            {
//                petit = "1";
//            }
//            else
//            {
//                petit = "0";
//            }
//            if (pers_grid_1.SelectedItems.Count != 0)
//            {
//                //string btn = "1";

//                //string iid = "";
//                //string ppol = "";
//                //if (cel_vizita.EditValue == null)
//                //{
//                //    MessageBox.Show("Выберите причину внесения изменений в РС ЕРЗ!!!");
//                //    return;
//                //}
//                //else
//                //{
//                //    Vars.CelVisit = cel_vizita.EditValue.ToString();
//                //    string cel_viz = cel_vizita.EditValue.ToString();
//                //}


//                //if (sp_pod_z.EditValue == null)
//                //{
//                //    MessageBox.Show("Выберите способ подачи заявления!");
//                //    return;
//                //}
//                //else
//                //{
//                //    string sppz = sp_pod_z.EditValue.ToString();
//                //    Vars.Sposob = sp_pod_z.EditValue.ToString();
//                //    string dobr = d_obr.ToString();
//                //    Person_Data w2 = new Person_Data(iid, btn, ppol, cel_viz, sppz, dobr, petit);
//                //    w2.Owner = this;
//                //    //this.Visibility = Visibility.Collapsed;
//                //    w2.Show();
//                //   // this.Visibility = Visibility.Visible;


//                //}
//                MessageBox.Show("Вы вводите данные на новое ЗЛ. Снимите в таблице выделение с уже существующего ЗЛ!!!","Внимание!!!");
//                return;
//            }
//            else
//            {
//                string btn = "1";

//                string iid = "";
//                string ppol = "";


//                if (cel_vizita.EditValue == null)
//                {
//                    MessageBox.Show("Выберите причину внесения изменений в РС ЕРЗ!!!");
//                    return;
//                }
//                else
//                {
//                    Vars.CelVisit = cel_vizita.EditValue.ToString();
//                    string cel_viz = cel_vizita.EditValue.ToString();
//                }

//                if (sp_pod_z.SelectedIndex < 0)
//                {
//                    MessageBox.Show("Выберите способ подачи заявления!!!");
//                    return;
//                }
//                else
//                {
//                    string sppz = sp_pod_z.EditValue.ToString();
//                    Vars.Sposob = sp_pod_z.EditValue.ToString();
//                    string dobr = d_obr.ToString();
//                    Person_Data w2 = new Person_Data(iid, btn, ppol, cel_viz, sppz, dobr, petit);
//                    w2.Owner = this;
//                    //this.Visibility = Visibility.Collapsed;
//                    w2.Show();
//                    //this.Visibility = Visibility.Visible;
//                    //this.Close();


//                }
//            }
//            Vars.DateVisit = d_obr.DateTime;
//        }

//        private void pers_grid_1_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
//        {

//        }

//        private void w_1_Loaded(object sender, RoutedEventArgs e)
//        {
//            pers_grid_1.SelectedItem = -1;
//            cel_vizita.SelectedIndex = -1;
//        }
//        private int rows_count;
//        private void pers_grid_1_Loaded(object sender, RoutedEventArgs e)
//        {
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
//        }



//        private void pers_grid_1_TextInput(object sender, TextCompositionEventArgs e)
//        {


//        }

//        private void pers_grid_1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
//        {


//        }
//        public ObservableCollection<string> list = new ObservableCollection<string>();



//        private void pers_grid_1_PreviewKeyDown(object sender, KeyEventArgs e)
//        {
//            if (e.Key == Key.Enter)
//            {
//                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
//                if (pers_grid_1.ActiveFilterInfo==null)
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
//        }

//        private void tableView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
//        {


//        }

//        private void tableView_ShowGridMenu(object sender, DevExpress.Xpf.Grid.GridMenuEventArgs e)
//        {

//            //BarSubItem mainMenu = new BarSubItem();
//            //mainMenu.Name = "mainMenu";
//            //mainMenu.Content = "Удалить";

//            ////BarButtonItemLink barButtonItemLink = new BarButtonItemLink();
//            ////barButtonItemLink.BarItemName = "subMenu";
//            ////mainMenu.ItemLinks.Add(barButtonItemLink);
//            ////e.Customizations.Add(subMenu);
//            //e.Customizations.Add(mainMenu);

//        }

//        private void poisk_Click(object sender, RoutedEventArgs e)
//        {
//            Poisk w4 = new Poisk();
//            w4.ShowDialog();
//        }

//        private void del_btn_Click(object sender, RoutedEventArgs e)
//        {
//            string sg_rows = " ";
//            int[] rt = pers_grid_1.GetSelectedRowHandles();
//            for (int i = 0; i < rt.Count(); i++)

//            {
//                var ddd = pers_grid_1.GetCellValue(rt[i], "ID");
//                var sgr = sg_rows.Insert(sg_rows.Length, ddd.ToString()) + ",";
//                sg_rows = sgr;
//            }

//            sg_rows = sg_rows.Substring(0, sg_rows.Length - 1);
//            var result = MessageBox.Show("Вы действительно хотите удалить записи", "Внимание!", MessageBoxButton.YesNo);
//            if (result != MessageBoxResult.Yes)
//            {
//                return;
//            }
//            else
//            {
//                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
//                SqlConnection con = new SqlConnection(connectionString);
//                SqlCommand comm = new SqlCommand($@"
//delete from POL_POLISES where PERSON_GUID in(select idguid from POL_PERSONS where id in({sg_rows}))
//delete from POL_RELATION_DOC_PERS where PERSON_GUID in(select idguid from POL_PERSONS where id in({sg_rows}))
//delete from POL_DOCUMENTS where PERSON_GUID in(select idguid from POL_PERSONS where id in({sg_rows}))
//delete from POL_RELATION_ADDR_PERS where PERSON_GUID in(select idguid from POL_PERSONS where id in({sg_rows}))
//delete from POL_ADDRESSES where EVENT_GUID in (select EVENT_GUID from POL_PERSONS where id in({sg_rows}))
//delete from POL_PERSONSB where PERSON_GUID in(select idguid from POL_PERSONS where id in({sg_rows}))
//delete from POL_OPLIST where PERSON_GUID in(select idguid from POL_PERSONS where id in({sg_rows}))
//delete from POL_EVENTS where PERSON_GUID in(select idguid from POL_PERSONS where id in({sg_rows}))
//delete from POL_PERSONS where ID in({sg_rows})", con);
//                con.Open();
//                comm.ExecuteNonQuery();
//                con.Close();
//                int kol_zap = rt.Count();
//                int lastnumber = kol_zap % 10;
//                if (lastnumber > 1 && lastnumber < 5)
//                {
//                    MessageBox.Show(" Успешно удалено  " + rt.Count() + " записи!");
//                }
//                else if (lastnumber == 1)
//                {
//                    MessageBox.Show(" Успешно удалена  " + rt.Count() + " запись!");
//                }
//                else
//                {
//                    MessageBox.Show(" Успешно удалено  " + rt.Count() + " записей!");
//                }

//                pers_grid_1_Loaded(sender, e);
//            }
//        }

//        private void new_klient_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
//        {

//        }
//        private void GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
//        {

//        }

//        private void print_Click(object sender, RoutedEventArgs e)
//        {
//            Vars.IdZl = Convert.ToInt32(pers_grid_1.GetFocusedRowCellValue("ID"));
//            Vars.Forms = 1;
//            Docs_print docs = new Docs_print();
//            docs.ShowDialog();
//            //int idpers = Convert.ToInt32(pers_grid_1.GetFocusedRowCellValue("ID"));
//            //Zayavlenie shreport = new Zayavlenie(idpers);
//            //shreport.Show();
//        }

//        private void W_1_Activated(object sender, EventArgs e)
//        {


//            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
//            var peopleList =
//                    MyReader.MySelect<People>(
//                        $@"
//             SELECT top(1000) p.ID,p.IDGUID,p.active,pe.TIP_OP,p.SS ,p.ENP ,p.FAM , p.IM  , p.OT ,p.W ,p.DR , p.PHONE , 
//p.COMMENT ,f.NameWithID, op.filename
//  FROM [dbo].[POL_PERSONS] p
//left join f003 f on p.mo=f.mcod 
//left join pol_events pe
//on p.event_guid=pe.idguid
//left join pol_oplist op
//on p.idguid=op.person_guid
//--where p.active=1 
// order by ID desc", connectionString);
//            pers_grid_1.ItemsSource = peopleList;
//            pers_grid_1.View.FocusedRowHandle = -1;
//            pers_grid_1.Columns[11].Width = 400;
//        }

//        private void W_1_Closed(object sender, EventArgs e)
//        {
//            //this.Owner.Visibility = Visibility.Visible;
//            this.Owner.Focus();
//            Vars.F_ids = null;
//        }
    }

    
}
