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
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Collections.Specialized;
using System.Threading;
using System.Diagnostics;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;
using Yamed.Server;
using System.Windows.Media.Effects;
using Yamed.Control.Editors;
using System.Globalization;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Insurance_SPR;


namespace Insurance
{
    
    //public static class BusinessDays
    //{
    //    public static System.DateTime AddBusinessDays(this System.DateTime source, int businessDays)
    //    {
    //        var dayOfWeek = businessDays < 0
    //                            ? ((int)source.DayOfWeek - 12) % 7
    //                            : ((int)source.DayOfWeek + 6) % 7;

    //        switch (dayOfWeek)
    //        {
    //            case 6:
    //                businessDays--;
    //                break;
    //            case -6:
    //                businessDays++;
    //                break;
    //        }

    //        return source.AddDays(businessDays + ((businessDays + dayOfWeek) / 5) * 2);
    //    }
    //}
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

    public partial class Person_Data : Window
    {
        bool tttab;
        //string _iid;
        //string petition;
        string dorder;
        string btn_;
        RoutedEventArgs e;
        //public delegate void Progressing_load(object o,RoutedEventArgs e);
        public Person_Data()
        {
            //LoadingDecorator1.IsSplashScreenShown = true;

            InitializeComponent();

            //Progressing_load pl = new Progressing_load(Window_Loaded);
            //IAsyncResult ar = pl.BeginInvoke(this,e, null, null);
            //while (!ar.IsCompleted)
            //{
            //    pers_grid_1_Loaded(this,e);
            //}
            //Thread t0 = new Thread(delegate ()
            //{
                //Window_Loaded1(this, e);
            //});
            //t0.Start();
            //LoadingDecorator1.IsSplashScreenShown = true;
            WindowState = WindowState.Maximized;
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

            if (Vars.Btn == "2")
            {
                this.Title = "Исправление ошибочных данных застрахованного лица";
            }
            else if (Vars.Btn == "1")
            {
                this.Title = "Новый клиент";
            }
            else
            {
                this.Title = "Создание нового события существующему ЗЛ";
            }
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
            
        }




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
            
            this.Close();
            
        }

        
        private void dalee_Click(object sender, RoutedEventArgs e)
        {
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
            prev_doc_stringSql();
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
            if (interval.Days / 365.25 < 14 &&  Vars.Sposob=="1")
            {
                string m = "Для ребенка до 14 лет выбран способ подачи заявления "+(char)34+ "Лично"+(char)34+"!!! Измените способ подачи на " + (char)34 + "Представитель" + (char)34 + "!";
                string t = "Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();

                return;
                             
            }
            
            //-------------------------------------------------------------------------------------------------
            // если нет дополнительного документа
            //if (ddtype.EditValue == null)
            //{
            //    if (Vars.Btn == "1")
            //    {
            //        //----------------------------------------------------------------------------
            //        //Не БОМЖ и адрес регистрации и адрес проживания не совпадают;

            //        if (fias.bomj.IsChecked == false && fias1.sovp_addr.IsChecked == false)
            //        {

            //            //Если тип полиса - временное свидетельство и поле серия не пусто;

            //            if (type_policy.EditValue.ToString() == "2" && spolis_ != null)
            //            {
                            
            //                InsMethods.Save_bt1_b0_s0_p2_sp1(this);
                                                        
            //                return;

            //            }
            //            //-----------------------------------------------------------------------
            //            //Если тип полиса - временное свидетельство и поле серия пусто;
            //            else if (type_policy.EditValue.ToString() == "2" && spolis_ == null)
            //            {

                            
            //                InsMethods.Save_bt1_b0_s0_p2_sp0(this);
                            
            //                return;

            //            }

            //            //------------------------------------------------------------
            //            //Если тип полиса - не временное свидетельство;

            //            else
            //            {

                            
            //                InsMethods.Save_bt1_b0_s0_p13(this);
                             
                            
            //                return;
            //            }
            //        }

            //        //----------------------------------------------------------------------------
            //        //Если БОМЖ и адреса не совпадают

            //        else if (fias.bomj.IsChecked == true && fias1.sovp_addr.IsChecked == false)
            //        {
                                                
            //            if (type_policy.EditValue.ToString() == "2" && spolis_ == null)
            //            {

                            
            //                InsMethods.Save_bt1_b1_s0_p2_sp0(this);
                            
                            
            //                return;
            //            }
            //            else if (type_policy.EditValue.ToString() == "2" && spolis_ != null)
            //            {

                            
            //                InsMethods.Save_bt1_b1_s0_p2_sp1(this);
                             
                            
            //                return;
            //            }
            //            else
            //            {

                            
            //                InsMethods.Save_bt1_b1_s0_p13(this);
                             
                            
            //                return;
            //            }
            //        }
            //        //---------------------------------------------------------------------------------------------
            //        // Иначе;      
            //        else
            //        {
            //            dstrkor = fias.reg_dom.Text.Split(',');
            //            domsplit = dstrkor[0].Replace("д.", "");
            //            dstrkor1 = fias1.reg_dom1.Text.Split(',');
            //            domsplit1 = dstrkor1[0].Replace("д.", "");

            //            if (type_policy.EditValue.ToString() == "2" && spolis_ != null)
            //            {

                            
            //                InsMethods.Save_bt1_b0_s1_p2_sp1(this);
                             
                            
            //                return;
            //            }
            //            else if (type_policy.EditValue.ToString() == "2" && spolis_ == null)
            //            {

                            
            //                InsMethods.Save_bt1_b0_s1_p2_sp0(this);
                            
                            
            //                return;

            //            }
            //            else
            //            {

                            
            //                InsMethods.Save_bt1_b0_s1_p13(this);
                             
                            
            //                return;
            //            }
            //        }
                                       
            //    }
            //    //------------------------------------------------------------------------------
            //    // При нажатии кнопки "Далее" если в предидущем окне была нажата кнопка "Далее".
            //    else if (Vars.Btn == "3")
            //    {
            //        dstrkor = fias.reg_dom.Text.Split(',');
            //        domsplit = dstrkor[0].Replace("д.", "");
            //        dstrkor1 = fias1.reg_dom1.Text.Split(',');
            //        domsplit1 = dstrkor1[0].Replace("д.", "");

            //        if (fias.bomj.IsChecked == false && fias1.sovp_addr.IsChecked == false)
            //        {

                        
            //            InsMethods.Save_bt3_b0_s0_p13(this);
                            
                        
            //            return;
            //        }
            //        else if (fias.bomj.IsChecked == true && fias1.sovp_addr.IsChecked == false)
            //        {

                        
            //            InsMethods.Save_bt3_b1_s0_p13(this);
                            
                        
            //            return;
            //        }
            //        else
            //        {

                        
            //            InsMethods.Save_bt3_b0_s1_p13(this);
                             
                        
            //            return;
            //        }



            //    }

            //    //--------------------------------------------------------------------------------------------------
            //    // При нажатии кнопки "Далее" если в предидущем окне была нажата кнопка "Изменить данные клиента".
            //    else
            //    {
            //        dstrkor = fias.reg_dom.Text.Split(',');
            //        domsplit = dstrkor[0].Replace("д.", "");
            //        dstrkor1 = fias1.reg_dom1.Text.Split(',');
            //        domsplit1 = dstrkor1[0].Replace("д.", "");
            //        if (Vars.NewCelViz==0)
            //        {
            //            string m1 = "Цель визита останется прежней. Вы согласны?";
            //            string t1 = "Внимание!";
            //            int b1 = 2;
            //            Message me1 = new Message(m1, t1, b1);
            //            me1.ShowDialog();

            //            if (Vars.mes_res == 1)
            //            {

            //            }
            //            else
            //            {
            //                return;
            //            }
            //        }
            //        else
            //        {
            //            string m1 = "Цель визита будет заменена на " + (char)34 + Vars.CelVisit + (char)34 + ". Вы согласны?";
            //            string t1 = "Внимание!";
            //            int b1 = 2;
            //            Message me1 = new Message(m1, t1, b1);
            //            me1.ShowDialog();

            //            if (Vars.mes_res == 1)
            //            {

            //            }
            //            else
            //            {
            //                return;
            //            }
            //        }

            //        var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            //        SqlConnection con = new SqlConnection(connectionString);
            //        SqlCommand comm = new SqlCommand("select PERSON_GUID as prf from pol_personsb where person_guid=(select idguid from pol_persons where id=@id) and type=2", con);
            //        comm.Parameters.AddWithValue("@id", Vars.IdP);
            //        con.Open();
            //        Guid prf1 = (Guid)comm.ExecuteScalar();
            //        con.Close();

            //        SqlConnection con1 = new SqlConnection(connectionString);
            //        SqlCommand comm1 = new SqlCommand("select PERSON_GUID as prp from pol_personsb where person_guid=(select idguid from pol_persons where id=@id) and type=3", con1);
            //        comm1.Parameters.AddWithValue("@id", Vars.IdP);
            //        con1.Open();
            //        Guid prp1 = (Guid)comm1.ExecuteScalar();
            //        con1.Close();


            //        if (prf1 == null)
            //        {

            //            if (zl_photo.EditValue != null) // если изображение в pictureBox1 имеется
            //            {

            //            }


            //            PutImageBinaryInDb();

            //        }
            //        else
            //        {

            //            //Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
            //            //     new Action(delegate ()
            //            //     {
                        
            //            InsMethods.Save_bt2_prf1(this);
            //                 //}));
            //            return;
            //        }


            //        if (prp1 == null)
            //        {
            //            PutImageBinaryInDb1();
            //        }
            //        else
            //        {

                        
            //            InsMethods.Save_bt2_prp1(this);
                         
            //        return;
            //        }


                    

            //    }

            //}
            ////------------------------------------------------------------------------------------------
            ////если есть дополнительный документ
            //else
            //{
            //    if (Vars.Btn == "1")
            //    {
            //        //----------------------------------------------------------------------------
            //        //Не БОМЖ и адрес регистрации и адрес проживания не совпадают;

            //        if (fias.bomj.IsChecked == false && fias1.sovp_addr.IsChecked == false)
            //        {


            //            //Если тип полиса - временное свидетельство и поле серия не пусто;

            //            if (type_policy.EditValue.ToString() == "2" && spolis_ != null)
            //            {

                            
            //                InsMethods.SaveDD_bt1_b0_s0_p2_sp1(this);
                             
                            
            //                return;

            //            }
            //            //-----------------------------------------------------------------------
            //            //Если тип полиса - временное свидетельство и поле серия пусто;
            //            else if (type_policy.EditValue.ToString() == "2" && spolis_ == null)
            //            {

                            
            //                InsMethods.SaveDD_bt1_b0_s0_p2_sp0(this);
                             
                            
            //                return;

            //            }

            //            //------------------------------------------------------------
            //            //Если тип полиса - не временное свидетельство;

            //            else
            //            {

                            
            //                InsMethods.SaveDD_bt1_b0_s0_p13(this);
                             
                            
            //                return;
            //            }
            //        }

            //        //----------------------------------------------------------------------------
            //        //Если БОМЖ и адреса не совпадают

            //        else if (fias.bomj.IsChecked == true && fias1.sovp_addr.IsChecked == false)
            //        {

            //            if (type_policy.EditValue.ToString() == "2" && spolis_ == null)
            //            {

                            
            //                InsMethods.SaveDD_bt1_b1_s0_p2_sp0(this);
                             
                            
            //                return;
            //            }
            //            else if (type_policy.EditValue.ToString() == "2" && spolis_ != null)
            //            {

                            
            //                InsMethods.SaveDD_bt1_b1_s0_p2_sp1(this);
                             
                            
            //                return;
            //            }
            //            else
            //            {

                            
            //                InsMethods.SaveDD_bt1_b1_s0_p13(this);
                             
            //                return;
            //            }
            //        }
            //        //---------------------------------------------------------------------------------------------
            //        // Иначе;      
            //        else
            //        {
                        
            //            if (type_policy.EditValue.ToString() == "2" && spolis_ != null)
            //            {

                            
            //                InsMethods.SaveDD_bt1_b0_s1_p2_sp1(this);
                             
                            
            //                return;
            //            }
            //            else if (type_policy.EditValue.ToString() == "2" && spolis_ == null)
            //            {

                            
            //                InsMethods.SaveDD_bt1_b0_s1_p2_sp0(this);
                             
                            
            //                return;

            //            }
            //            else
            //            {
                            
            //                InsMethods.SaveDD_bt1_b0_s1_p13(this);
                             
                            
            //                return;
            //            }
            //        }


            //    }
            //    //------------------------------------------------------------------------------
            //    // При нажатии кнопки "Далее" если в предидущем окне была нажата кнопка "Далее".
            //    else if (Vars.Btn == "3")
            //    {
            //        dstrkor = fias.reg_dom.Text.Split(',');
            //        domsplit = dstrkor[0].Replace("д.", "");
            //        dstrkor1 = fias1.reg_dom1.Text.Split(',');
            //        domsplit1 = dstrkor1[0].Replace("д.", "");

            //        if (fias.bomj.IsChecked == false && fias1.sovp_addr.IsChecked == false)
            //        {

                        
            //            InsMethods.SaveDD_bt3_b0_s0_p13(this);
                             
                        
            //            return;
            //        }
            //        else if (fias.bomj.IsChecked == true && fias1.sovp_addr.IsChecked == false)
            //        {

                        
            //            InsMethods.SaveDD_bt3_b1_s0_p13(this);
                             
            //            return;
            //        }
            //        else
            //        {

                        
            //            InsMethods.SaveDD_bt3_b0_s1_p13(this);
                             
                        
            //            return;
            //        }


            //    }

            //    //--------------------------------------------------------------------------------------------------
            //    // При нажатии кнопки "Далее" если в предидущем окне была нажата кнопка "Изменить данные клиента".
            //    else
            //    {
            //        dstrkor = fias.reg_dom.Text.Split(',');
            //        domsplit = dstrkor[0].Replace("д.", "");
            //        dstrkor1 = fias1.reg_dom1.Text.Split(',');
            //        domsplit1 = dstrkor1[0].Replace("д.", "");
            //        if (Vars.NewCelViz == 0)
            //        {
            //            string m1 = "Цель визита останется прежней. Вы согласны?";
            //            string t1 = "Внимание!";
            //            int b1 = 2;
            //            Message me1 = new Message(m1, t1, b1);
            //            me1.ShowDialog();
                        
            //            if (Vars.mes_res==1)
            //            {

            //            }
            //            else
            //            {
            //                return;
            //            }
            //        }
            //        else
            //        {
            //            string m1 = "Цель визита будет заменена на " + (char)34 + Vars.CelVisit + (char)34 + ". Вы согласны?";
            //            string t1 = "Внимание!";
            //            int b1 = 2;
            //            Message me1 = new Message(m1, t1, b1);
            //            me1.ShowDialog();

            //            if (Vars.mes_res == 1)
            //            {

            //            }
            //            else
            //            {
            //                return;
            //            }
                        
            //        }
            //        var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            //        SqlConnection con = new SqlConnection(connectionString);
            //        SqlCommand comm = new SqlCommand("select PERSON_GUID as prf from pol_personsb where person_guid=(select idguid from pol_persons where id=@id) and type=2", con);
            //        comm.Parameters.AddWithValue("@id", Vars.IdP);
            //        con.Open();
            //        Guid prf1 = (Guid)comm.ExecuteScalar();
            //        con.Close();

            //        SqlConnection con1 = new SqlConnection(connectionString);
            //        SqlCommand comm1 = new SqlCommand("select PERSON_GUID as prp from pol_personsb where person_guid=(select idguid from pol_persons where id=@id) and type=3", con1);
            //        comm1.Parameters.AddWithValue("@id", Vars.IdP);
            //        con1.Open();
            //        Guid prp1 = (Guid)comm1.ExecuteScalar();
            //        con1.Close();


            //        if (prf1 == null)
            //        {

            //            if (zl_photo.EditValue != null) // если изображение в pictureBox1 имеется
            //            {

            //            }


            //            PutImageBinaryInDb();

            //        }
            //        else
            //        {

                        
            //            InsMethods.SaveDD_bt2_prf1(this);
                             
                        
            //            return;
            //        }


            //        if (prp1 == null)
            //        {
            //            PutImageBinaryInDb1();
            //        }
            //        else
            //        {

                        
            //            InsMethods.SaveDD_bt2_prp1(this);
                             
                        
            //            return;
            //        }


                    

            //    }

            //}
        }
        public ObservableCollection<string> list = new ObservableCollection<string>();
        public ObservableCollection<string> list1 = new ObservableCollection<string>();
        public ObservableCollection<string> list2 = new ObservableCollection<string>();
        public ObservableCollection<string> list3 = new ObservableCollection<string>();
        public ObservableCollection<string> list4 = new ObservableCollection<string>();
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
        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //}
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //LoadingDecorator1.IsSplashScreenShown = true;
            Cursor = Cursors.Wait;
         Thread t0 = new Thread(delegate ()
         {
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
           new Action(delegate ()
           {
               //Vars.DateVisit = Convert.ToDateTime(d_obr.EditValue);
               var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
               var peopleList =
                       MyReader.MySelect<People>(
                           $@"
            SELECT top(5000) p.ID,p.IDGUID,p.active,pe.TIP_OP,p.SS ,p.ENP ,p.FAM , p.IM  , p.OT ,p.W ,p.DR , p.PHONE ,
p.COMMENT ,f.NameWithID,op.filename,d.DOCTYPE,d.DOCSER,d.DOCNUM,VPOLIS,SPOLIS ,NPOLIS,DBEG ,DRECEIVED,DEND ,DSTOP
  FROM [dbo].[POL_PERSONS] p
left join f003 f on p.mo=f.mcod 
left join pol_events pe
on p.event_guid=pe.idguid
left join pol_polises ps
on p.idguid=ps.person_guid
left join pol_oplist op
on p.idguid=op.person_guid
left join pol_documents d
on p.idguid=d.person_guid
 order by ID desc", connectionString);
               pers_grid_2.ItemsSource = peopleList;
               pers_grid_2.View.FocusedRowHandle = -1;
           }));
         });
            t0.Start();
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
            if (Vars.Btn != "1")
            {
              //Thread t2 = new Thread(delegate ()
              //{
                      Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
               new Action(delegate ()
               {
                   

                   var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                   SqlConnection con = new SqlConnection(connectionString1);
                   SqlCommand comm = new SqlCommand("select photo as prf from pol_personsb where person_guid=(select idguid from pol_persons where id=@id) and type=2", con);
                   comm.Parameters.AddWithValue("@id", Vars.IdP);
                   con.Open();
                   string prf1 = (string)comm.ExecuteScalar();
                   con.Close();
                   SqlCommand comm1 = new SqlCommand("select photo as prp from pol_personsb where person_guid=(select idguid from pol_persons where id=@id) and type=3", con);
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
                   if (prp == null || prp=="")
                   {
                       zl_podp.EditValue = "";
                   }
                   else
                   {
                       zl_podp.EditValue = Convert.FromBase64String(prp);
                   }

                   SqlCommand comm3 = new SqlCommand(@"select t0.*,t1.FAM as fam1,t1.im as im1, t1.ot as ot1,
t1.dr as dr1,t1.phone as phone1,t2.PRELATION,t1.idguid as idguid1,t1.w as w1,t0.mo as MO,t0.dstart as DSTART,
t3.fam as fam2,t3.im as im2,t3.ot as ot2,t3.w as w2,t3.dr as dr2,t3.mr as mr2,t2.tip_op as tip_op,
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
(select * from pol_persons )T3
on t0.idguid = t3.PARENTGUID", con);
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
                    snils.Text = ss.ToString();
                    phone.Text = phone_.ToString();
                    email.Text = email_.ToString();
                    kat_zl.EditValue = kateg;
                    dost1.EditValue = dost_1.Split(',');
                    cel_vizita.EditValue = tip_op_.ToString();
                    if(sppz_.ToString()=="")
                    {

                    }
                    else
                    {
                        sp_pod_z.EditValue = Convert.ToInt32(sppz_);
                    }
                    if(dvisit_.ToString()=="")
                    {

                    }
                    else
                    {
                        d_obr.EditValue = Convert.ToDateTime(dvisit_);
                    }
                    if(petition_.ToString()=="")
                    {

                    }
                    else
                    {
                        petition.EditValue = Convert.ToBoolean(petition_);
                    }
                    
                    pr_pod_z_polis.SelectedIndex = Convert.ToInt32(rpolis_.ToString()=="" ? 0: rpolis_) -1;
                    form_polis.SelectedIndex = Convert.ToInt32(fpolis_.ToString() == "" ? -1 : fpolis_);
                    pr_pod_z_smo.SelectedIndex = Convert.ToInt32(rsmo_.ToString() == "" ? 0 : rsmo_) -1;

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
                    if (rpersonguid.ToString() == "00000000-0000-0000-0000-000000000000" || rpersonguid.ToString()=="")
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
                            status_p2.SelectedIndex = Convert.ToInt32(prelation) - 1;
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
               SqlCommand comm2 = new SqlCommand(@"select * from pol_documents where person_guid=(select idguid from pol_persons where id=@id) and main=1 and active=1", con);
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
                person_guid=(select idguid from pol_persons where id=@id) and main=0 and active=0", con);
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
where person_guid=(select idguid from pol_persons where id=@id) and main=0 and active=1", con);
                   comm4.Parameters.AddWithValue("@id", Vars.IdP);
                   con.Open();
                   SqlDataReader reader2 = comm4.ExecuteReader();

                   while (reader2.Read()) // построчно считываем данные
                   {
                       object doctype = reader2["DOCTYPE"];
                       object docser = reader2["DOCSER"];
                       object docnum = reader2["DOCNUM"];
                       object docdate = reader2["DOCDATE"];
                       object name_vp = reader2["NAME_VP"];



                       ddtype.EditValue = doctype;
                       ddser.Text = docser.ToString();
                       ddnum.Text = docnum.ToString();
                       dddate.DateTime = Convert.ToDateTime(docdate);
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
where pr.PERSON_GUID=(select IDGUID from pol_persons where id=@id) and pr.addres_g=1 ", con);
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
                       dstrkor = fias.reg_dom.Text.Split(',');
                       domsplit = dstrkor[0].Replace("д.", "");
                       fias.reg_korp.Text = korp_.ToString();
                       fias.reg_str.Text = str_.ToString();
                       fias.reg_kv.Text = kv_.ToString();
                       fias.bomj.IsChecked = Convert.ToBoolean(bomg);

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
where pr.PERSON_GUID=(select IDGUID from pol_persons where id=@id) and pr.addres_p=1 ", con);
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
                             "where pr.PERSON_GUID=(select IDGUID from pol_persons where id=@id) and pr.addres_p=1", con);
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
                           dstrkor1 = fias1.reg_dom1.Text.Split(',');
                           domsplit1 = dstrkor1[0].Replace("д.", "");
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
                   if (Vars.CelVisit == "П010" || Vars.CelVisit == "П034" || Vars.CelVisit == "П035" || Vars.CelVisit == "П036" || Vars.CelVisit == "П061")
                   {
                       SqlCommand comm7;

                       if (Vars.Btn == "2")
                       {
                           comm7 = new SqlCommand("select * from pol_polises where person_guid=(select idguid from POL_persons where id=@id)", con);
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
                       SqlCommand comm7 = new SqlCommand("select * from pol_polises  where PERSON_GUID=(select IDGUID from pol_persons where id=@id)", con);
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


                   list.Add("1 Родитель");
                   list.Add("2 Опекун");
                   list.Add("3 Представитель");
                   status_p2.ItemsSource = list;

                   
                   //form_polis.SelectedIndex = 1;

                   list4.Add("1 Отсутствует отчество");
                   list4.Add("2 Отсутствует фамилия");
                   list4.Add("3 Отсутствует имя");
                   list4.Add("4 Известен только месяц и год даты рождения");
                   list4.Add("5 Известен только год даты рождения");
                   list4.Add("6 Дата рождения не соответствует календарю");
                   this.dost1.ItemsSource = list4;


                   pers_grid_2.View.FocusedRowHandle = -1;
                   pers_grid_2.SelectedItem = -1;




                   var molist =
                           MyReader.MySelect<F003>(
                               $@"
            SELECT mcod,namewithid from f003
 order by mcod", Properties.Settings.Default.DocExchangeConnectionString);
                   mo_cmb.DataContext = molist;

                   pol.DataContext = MyReader.MySelect<V005>(@"select IDPOL,NameWithID from SPR_79_V005", Properties.Settings.Default.DocExchangeConnectionString);
                   pol_pr.DataContext = MyReader.MySelect<V005>(@"select IDPOL,NameWithID from SPR_79_V005", Properties.Settings.Default.DocExchangeConnectionString);
                   doc_type.DataContext = MyReader.MySelect<F011>(@"select ID,NameWithID from SPR_79_F011", Properties.Settings.Default.DocExchangeConnectionString);
                   doctype1.DataContext = MyReader.MySelect<F011>(@"select ID,NameWithID from SPR_79_F011", Properties.Settings.Default.DocExchangeConnectionString);
                   doc_type1.DataContext = MyReader.MySelect<F011>(@"select ID,NameWithID from SPR_79_F011", Properties.Settings.Default.DocExchangeConnectionString);
                   ddtype.DataContext = MyReader.MySelect<F011>(@"select ID,NameWithID from SPR_79_F011", Properties.Settings.Default.DocExchangeConnectionString);
                   str_vid.DataContext = MyReader.MySelect<OKSM>(@"select ID,CAPTION from SPR_79_OKSM", Properties.Settings.Default.DocExchangeConnectionString);
                   str_vid1.DataContext = MyReader.MySelect<OKSM>(@"select ID,CAPTION from SPR_79_OKSM", Properties.Settings.Default.DocExchangeConnectionString);
                   str_r.DataContext = MyReader.MySelect<OKSM>(@"select ID,CAPTION from SPR_79_OKSM", Properties.Settings.Default.DocExchangeConnectionString);
                   gr.DataContext = MyReader.MySelect<OKSM>(@"select ID,CAPTION from SPR_79_OKSM", Properties.Settings.Default.DocExchangeConnectionString);
                   kat_zl.DataContext = MyReader.MySelect<V013>(@"select IDKAT,NameWithID from V013", Properties.Settings.Default.DocExchangeConnectionString);
                   type_policy.DataContext = MyReader.MySelect<F008>(@"select ID,NameWithID from SPR_79_F008", Properties.Settings.Default.DocExchangeConnectionString);
                   prev_pol.DataContext = MyReader.MySelect<V005>(@"select IDPOL,NameWithID from SPR_79_V005", Properties.Settings.Default.DocExchangeConnectionString);
                   //kem_vid.DataContext = MyReader.MySelect<NAMEVP>(@"select distinct name_vp from pol_documents order by name_vp",connectionString);
                   // LoadingDecorator1.IsSplashScreenShown = false;
                   fam.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_fam", Properties.Settings.Default.DocExchangeConnectionString);
                   im.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_im", Properties.Settings.Default.DocExchangeConnectionString);
                   ot.DataContext = MyReader.MySelect<FIO>(@"select distinct fam,im,ot from spr_ot", Properties.Settings.Default.DocExchangeConnectionString);
                   kem_vid.DataContext = MyReader.MySelect<NAME_VP>(@"select id,name from spr_namevp", Properties.Settings.Default.DocExchangeConnectionString);
                   Cursor = Cursors.Arrow;
               }));
             //Thread t1 = new Thread(delegate ()
             //{

         
        }


        public string sblank;
        public int rper_load=0;
        public Guid rper;
        private void pers_grid_2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            //LoadingDecorator1.IsSplashScreenShown = true;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            if (tabs.SelectedIndex == 6)
            {
                

                SqlCommand comm11 = new SqlCommand("select * from pol_documents where person_guid=(select IDGUID from pol_persons where id=@id) and main=1", con);
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
                Cursor = Cursors.Wait;
                Vars.IdP = pers_grid_2.GetFocusedRowCellValue("ID").ToString();
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
                   var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                   var peopleList =
                       MyReader.MySelect<People>(
                           $@"
            SELECT top(1000) p.ID,p.IDGUID,p.active,pe.TIP_OP,p.SS ,p.ENP ,p.FAM , p.IM  , p.OT ,p.W ,p.DR , p.PHONE ,
p.COMMENT ,f.NameWithID,op.filename,d.DOCTYPE,d.DOCSER,d.DOCNUM,VPOLIS,SPOLIS ,NPOLIS,DBEG ,DRECEIVED,DEND ,DSTOP
  FROM [dbo].[POL_PERSONS] p
left join f003 f on p.mo=f.mcod 
left join pol_events pe
on p.event_guid=pe.idguid
left join pol_polises ps
on p.idguid=ps.person_guid
left join pol_oplist op
on p.idguid=op.person_guid
left join pol_documents d
on p.idguid=d.person_guid
 order by ID desc", connectionString);
                   pers_grid_2.ItemsSource = peopleList;
                   //pers_grid_2.Columns[1].Visible = false;
                   //pers_grid_2.Columns[2].Visible = false;
                   //pers_grid_2.Columns[3].Visible = false;
                   //pers_grid_2.Columns[4].Visible = false;
                   pers_grid_2.View.FocusedRowHandle = -1;
                   pers_grid_2.SelectedItem = -1;
               }));
        }
        private void reg_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        public string s;


        private void dost1_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

            string[] dst = dost1.Text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            int i = dst.Count();
            for (i = 0; i < dst.Count(); i++)
            {
                _7.Text += (dst[i].Substring(0, 1) + ",");

            }
            s = _7.Text.Substring(0, _7.Text.Length - 1);
            _7.Text = s;
        }





        private void date_vid1_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            DateTime dte = new DateTime(DateTime.Today.Year,12,31);
            if (type_policy.EditValue.ToString() != "2")
            {
                
            }
            else
            {
                pustoy.IsChecked = false;
                if (date_end.EditValue ==null && docexp1.EditValue==null)
                {
                    date_end.DateTime = date_vid1.DateTime.AddBusinessDays(44);
                    date_poluch.DateTime = date_vid1.DateTime;
                }
                else if (date_end.EditValue != null && type_policy.EditValue.ToString() == "2")
                {
                    date_end.DateTime = date_vid1.DateTime.AddBusinessDays(44);
                    date_poluch.DateTime = date_vid1.DateTime;
                }
                else if(date_end.EditValue==null && Convert.ToDateTime(docexp1.EditValue) > dte)
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
                        MyReader.MySelect<People>(
                            $@"
            SELECT top(1000) p.ID,p.IDGUID,p.active,pe.TIP_OP,p.SS ,p.ENP ,p.FAM , p.IM  , p.OT ,p.W ,p.DR , p.PHONE ,
p.COMMENT ,f.NameWithID,op.filename,d.DOCTYPE,d.DOCSER,d.DOCNUM,VPOLIS,SPOLIS ,NPOLIS,DBEG ,DRECEIVED,DEND ,DSTOP
  FROM [dbo].[POL_PERSONS] p
left join f003 f on p.mo=f.mcod 
left join pol_events pe
on p.event_guid=pe.idguid
left join pol_polises ps
on p.idguid=ps.person_guid
left join pol_oplist op
on p.idguid=op.person_guid
left join pol_documents d
on p.idguid=d.person_guid
 order by ID desc", connectionString);
                    pers_grid_2.ItemsSource = peopleList;
                }
                else
                {
                    string strf = pers_grid_2.ActiveFilterInfo.FilterString.Substring(pers_grid_2.ActiveFilterInfo.FilterString.IndexOf("'", 0));
                    string strf1 = strf.Replace("'", "").Replace(")", "").Replace(".", "");


                    var peopleList =
                        MyReader.MySelect<People>(
                            $@"
            SELECT top(1000) p.ID,p.IDGUID,p.active,pe.TIP_OP,p.SS ,p.ENP ,p.FAM , p.IM  , p.OT ,p.W ,p.DR , p.PHONE ,
p.COMMENT ,f.NameWithID,op.filename,d.DOCTYPE,d.DOCSER,d.DOCNUM,VPOLIS,SPOLIS ,NPOLIS,DBEG ,DRECEIVED,DEND ,DSTOP
  FROM [dbo].[POL_PERSONS] p
left join f003 f on p.mo=f.mcod 
left join pol_events pe
on p.event_guid=pe.idguid
left join pol_polises ps
on p.idguid=ps.person_guid
left join pol_oplist op
on p.idguid=op.person_guid
left join pol_documents d
on p.idguid=d.person_guid
 
 

where FAM LIKE '{
                             strf1}%' OR  IM LIKE '{
                             strf1}%' OR  OT LIKE '{
                             strf1}%' order by ID desc", connectionString);
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
                    else if (interval.Days / 365.25 < 14 && doc_type.EditValue.ToString() != "3")
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

        private void date_vid_EditValueChanged(object sender, EditValueChangedEventArgs e)
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



        private void docexp1_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            DateTime dde = new DateTime(DateTime.Today.Year,12,31);
            if(Convert.ToDateTime(docexp1.EditValue)>dde)
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
            if (date_end.EditValue == null)
            {
                date_end.DateTime = date_vid1.DateTime.AddBusinessDays(44);
                date_poluch.DateTime = date_vid1.DateTime;
            }
            else if (date_end.EditValue != null && type_policy.EditValue.ToString() == "2")
            {
                date_end.DateTime = date_vid1.DateTime.AddBusinessDays(44);
                date_poluch.DateTime = date_vid1.DateTime;
            }
            else
            {

                date_poluch.DateTime = date_vid1.DateTime;
            }
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
            prev_im.Text = im.Text;
            prev_ot.Text = ot.Text;
            prev_dr.EditValue = dr.EditValue;
            prev_pol.EditValue = pol.EditValue;
            prev_mr.Text = mr2.Text;

            prev_doc.Visibility = Visibility.Visible;
            tabs.SelectedIndex = 7;
            prev_doc_clk = 1;
            

        }

        private void kat_zl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                tabs.SelectedIndex = 2;
            }

        }

        private void pustoy_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                tabs.SelectedIndex = 3;
            }
        }

        private void tab_forward(bool tttab)
        {
            if (tttab == true)
            {
                tabs.SelectedIndex = 5;
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
            else if(e.Key == Key.F2 && pers_grid_2.SelectedItems.Count!=0)
            {

            }




        }

        private void zl_podp_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                tabs.SelectedIndex = 4;
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
            else if(e.Key==Key.Down)
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
            else if(e.Key==Key.Down)
            {
                MemoryStream str = new MemoryStream((byte[])zl_photo.EditValue);
                //BitmapImage bmp = new BitmapImage();
                //bmp.StreamSource = str;
                Bitmap bmp = new Bitmap(str);
                float x = 0.8f;
                //float x1 = 1.25f;
                var newWidth = (bmp.Height- bmp.Height/20) * x;
                //var nw = bmp.Width * x;

                //bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle((bmp.Width- Convert.ToInt32(newWidth))/2, bmp.Height/20,Convert.ToInt32(newWidth), bmp.Height- bmp.Height / 20);
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
                System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle((bmp.Width - Convert.ToInt32(newWidth)) / 2, 0,  Convert.ToInt32(newWidth), bmp.Height - bmp.Height / 20);
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
            if(doc_type.EditValue.ToString()=="14" || doc_type.EditValue.ToString() == "3")
            {
                str_r.EditValue = "RUS";
                str_vid.EditValue = "RUS";
                gr.EditValue = "RUS";
            }

        }

        private void Zl_photo_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            


            if (zl_photo.EditValue!=null && zl_photo.EditValue.ToString()!="") 
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
                
                float x1 = 0.22f;
                var newHeight = bmp.Width*x1;
                var newHeight_p = bmp.Width * x1 / 2;



                    System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(0,bmp.Height/2 - Convert.ToInt32(newHeight_p), bmp.Width, Convert.ToInt32(newHeight));
                    Bitmap bmp2 = bmp.Clone(cropRect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                    Bitmap bmp1 = new Bitmap(ResizeImage(bmp2, 736, 160));
                    using (var stream = new MemoryStream())
                    {
                        MakeGrayscale(bmp1).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        zl_podp.EditValue = stream.ToArray();
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
            if (prev_doc_clk==1)
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

                    pol.EditValue=2;
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
                        doc_type.EditValue=14;
                    }
                    else if (interval.Days / 365.25 < 14 )
                    {
                        doc_type.EditValue=3;
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
            doc_type1.EditValue = 14;

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
            if (e.Key == Key.Enter)
            {
                tabs.SelectedIndex = 1;
            }
        }
        private void Potok()
        {
            Cursor = Cursors.Wait;
            if (Vars.Btn != "1")
            {
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
               new Action(delegate ()
               {
                   var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                   SqlConnection con = new SqlConnection(connectionString1);
                   SqlCommand comm = new SqlCommand("select photo as prf from pol_personsb where person_guid=(select idguid from pol_persons where id=@id) and type=2", con);
                   comm.Parameters.AddWithValue("@id", Vars.IdP);
                   con.Open();
                   string prf1 = (string)comm.ExecuteScalar();
                   con.Close();
                   SqlCommand comm1 = new SqlCommand("select photo as prp from pol_personsb where person_guid=(select idguid from pol_persons where id=@id) and type=3", con);
                   comm1.Parameters.AddWithValue("@id", Vars.IdP);
                   con.Open();
                   string prp = (string)comm1.ExecuteScalar();
                   con.Close();

                   if (prf1 == null || prf1=="")
                   {
                       zl_photo.EditValue = "";
                   }
                   else
                   {
                       zl_photo.EditValue = Convert.FromBase64String(prf1);
                   }
                   if (prp == null || prp=="")
                   {
                       zl_podp.EditValue = "";
                   }
                   else
                   {
                       zl_podp.EditValue = Convert.FromBase64String(prp);
                   }

                   SqlCommand comm3 = new SqlCommand(@"select t0.*,t1.FAM as fam1,t1.im as im1, t1.ot as ot1,
t1.dr as dr1,t1.phone as phone1,t2.PRELATION,t1.idguid as idguid1,t1.w as w1,t0.mo as MO,t0.dstart as DSTART,
t3.fam as fam2,t3.im as im2,t3.ot as ot2,t3.w as w2,t3.dr as dr2,t3.mr as mr2,t2.tip_op as tip_op,
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
(select * from pol_persons )T3
on t0.idguid = t3.PARENTGUID", con);
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
                       kat_zl.EditValue = Convert.ToInt32(kateg.ToString()=="" ? 0 : kateg); 
                       dost1.EditValue = dost_1.Split(',');
                       cel_vizita.EditValue = tip_op_.ToString();
                       sp_pod_z.EditValue = Convert.ToInt32(sppz_.ToString() == "" ? 0 : sppz_);
                       d_obr.EditValue = Convert.ToDateTime(dvisit_.ToString() == "" ? DateTime.Today :dvisit_);
                       petition.EditValue = Convert.ToBoolean(petition_.ToString() == "" ? 0 : petition_);
                       pr_pod_z_polis.SelectedIndex = Convert.ToInt32(rpolis_.ToString() == "" ? 0 : rpolis_) - 1;
                       form_polis.SelectedIndex = Convert.ToInt32(fpolis_.ToString() == "" ? 0 : fpolis_);
                       pr_pod_z_smo.SelectedIndex = Convert.ToInt32(rsmo_.ToString() == "" ? 0 : rsmo_) - 1;

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
                    SqlCommand comm2 = new SqlCommand(@"select * from pol_documents where person_guid=(select idguid from pol_persons where id=@id) and main=1 and active=1", con);
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
                person_guid=(select idguid from pol_persons where id=@id) and main=0 and active=0", con);
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
where person_guid=(select idguid from pol_persons where id=@id) and main=0 and active=1", con);
                    comm4.Parameters.AddWithValue("@id", Vars.IdP);
                    con.Open();
                    SqlDataReader reader2 = comm4.ExecuteReader();

                    while (reader2.Read()) // построчно считываем данные
                    {
                        object doctype = reader2["DOCTYPE"];
                        object docser = reader2["DOCSER"];
                        object docnum = reader2["DOCNUM"];
                        object docdate = reader2["DOCDATE"];
                        object name_vp = reader2["NAME_VP"];



                        ddtype.EditValue = doctype;
                        ddser.Text = docser.ToString();
                        ddnum.Text = docnum.ToString();
                        dddate.DateTime = Convert.ToDateTime(docdate);
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
where pr.PERSON_GUID=(select IDGUID from pol_persons where id=@id) and pr.addres_g=1 ", con);
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
                   dstrkor = fias.reg_dom.Text.Split(',');
                   domsplit = dstrkor[0].Replace("д.", "");
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
where pr.PERSON_GUID=(select IDGUID from pol_persons where id=@id) and pr.addres_p=1 ", con);
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
                       "where pr.PERSON_GUID=(select IDGUID from pol_persons where id=@id) and pr.addres_p=1", con);
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
                       dstrkor1 = fias1.reg_dom1.Text.Split(',');
                       domsplit1 = dstrkor1[0].Replace("д.", "");
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
                    }
                }
             }));
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                new Action(delegate ()
                {
                    var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                    SqlConnection con = new SqlConnection(connectionString1);
                    if (Vars.CelVisit == "П010" || Vars.CelVisit == "П034" || Vars.CelVisit == "П035" || Vars.CelVisit == "П036" || Vars.CelVisit == "П061")
                    {
                        SqlCommand comm7;

                        if (Vars.Btn == "2")
                        {
                            comm7 = new SqlCommand("select * from pol_polises where person_guid=(select idguid from POL_persons where id=@id)", con);
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
                        SqlCommand comm7 = new SqlCommand("select * from pol_polises  where PERSON_GUID=(select IDGUID from pol_persons where id=@id)", con);
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

        private void Cel_vizita_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString1);
            if (cel_vizita.EditValue==null)
            {

            }
            else
            {
                Vars.CelVisit = cel_vizita.EditValue.ToString();
                Vars.NewCelViz = 1;
            }
            if (Vars.CelVisit == "П010" || Vars.CelVisit == "П034" || Vars.CelVisit == "П035" || Vars.CelVisit == "П036" || Vars.CelVisit == "П061" || Vars.CelVisit == "П062")
            {
                if(Vars.Btn!="2")
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
                        object n_polis_ = reader8["SPOLIS"];

                        ser_blank.Text = s_polis_.ToString();
                        num_blank.Text = n_polis_.ToString();
                    }
                    reader8.Close();
                    con.Close();
                }
                
            }
            else
            {
                type_policy.EditValue = 3;
                date_vid1.EditValue = null;
                date_poluch.EditValue = null;
                date_end.EditValue = null;
            }
        }

        private void Sp_pod_z_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            if(sp_pod_z.EditValue==null)
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
        private void print_Click(object sender, RoutedEventArgs e)
        {
            if(pers_grid_2.SelectedItems.Count>1)
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
            if (pers_grid_2.SelectedItems.Count > 0 && Vars.mes_res==10)
            {

            }
            else if(pers_grid_2.SelectedItems.Count == 0 && Vars.mes_res==10)
            {

            }
            else
            {
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                var peopleList =
                    MyReader.MySelect<People>(
                        $@"
            SELECT top(1000) p.ID,p.IDGUID,p.active,pe.TIP_OP,p.SS ,p.ENP ,p.FAM , p.IM  , p.OT ,p.W ,p.DR , p.PHONE ,
p.COMMENT ,f.NameWithID,op.filename,d.DOCTYPE,d.DOCSER,d.DOCNUM,VPOLIS,SPOLIS ,NPOLIS,DBEG ,DRECEIVED,DEND ,DSTOP
  FROM [dbo].[POL_PERSONS] p
left join f003 f on p.mo=f.mcod 
left join pol_events pe
on p.event_guid=pe.idguid
left join pol_polises ps
on p.idguid=ps.person_guid
left join pol_oplist op
on p.idguid=op.person_guid
left join pol_documents d
on p.idguid=d.person_guid", connectionString);
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
                num_blank.Mask= @"\d\d\d\d\d\d";
            }
            else if(type_policy.SelectedIndex==2)
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
                var peopleList =
                    MyReader.MySelect<People>(
                        $@"
            SELECT top(1000) p.ID,p.IDGUID,p.active,pe.TIP_OP,p.SS ,p.ENP ,p.FAM , p.IM  , p.OT ,p.W ,p.DR , p.PHONE ,
p.COMMENT ,f.NameWithID,op.filename,d.DOCTYPE,d.DOCSER,d.DOCNUM,VPOLIS,SPOLIS ,NPOLIS,DBEG ,DRECEIVED,DEND ,DSTOP
  FROM [dbo].[POL_PERSONS] p
left join f003 f on p.mo=f.mcod 
left join pol_events pe
on p.event_guid=pe.idguid
left join pol_polises ps
on p.idguid=ps.person_guid
left join pol_oplist op
on p.idguid=op.person_guid
left join pol_documents d
on p.idguid=d.person_guid
 
 

where IM LIKE '{
                         im.Text}%' and  OT LIKE '{
                         ot.Text}%' and  convert(nvarchar,dr,104) like left('{
                         dr.EditValue}%',10) order by ID desc", connectionString);
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
                var peopleList =
                    MyReader.MySelect<People>(
                        $@"
            SELECT top(1000) p.ID,p.IDGUID,p.active,pe.TIP_OP,p.SS ,p.ENP ,p.FAM , p.IM  , p.OT ,p.W ,p.DR , p.PHONE ,
p.COMMENT ,f.NameWithID,op.filename,d.DOCTYPE,d.DOCSER,d.DOCNUM,VPOLIS,SPOLIS ,NPOLIS,DBEG ,DRECEIVED,DEND ,DSTOP
  FROM [dbo].[POL_PERSONS] p
left join f003 f on p.mo=f.mcod 
left join pol_events pe
on p.event_guid=pe.idguid
left join pol_polises ps
on p.idguid=ps.person_guid
left join pol_oplist op
on p.idguid=op.person_guid
left join pol_documents d
on p.idguid=d.person_guid
 
 

where d.docser LIKE '{
                         doc_ser.Text}%' and  d.docnum LIKE '{
                         doc_num.Text}%'  order by ID desc", connectionString);
                pers_grid_2.ItemsSource = peopleList;
                if (pers_grid_2.VisibleRowCount != 0)
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
            }
            else
            {
               
            }
        }

        private void Im_ProcessNewValue(DependencyObject sender, ProcessNewValueEventArgs e)
        {
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
            }
            else
            {
                
            }
        }

        private void Ot_ProcessNewValue(DependencyObject sender, ProcessNewValueEventArgs e)
        {
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
            }
            else
            {
                
            }
        }

        private void Kem_vid_ProcessNewValue(DependencyObject sender, ProcessNewValueEventArgs e)
        {
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
                var peopleList =
                    MyReader.MySelect<People>(
                        $@"
            SELECT top(1000) p.ID,p.IDGUID,p.active,pe.TIP_OP,p.SS ,p.ENP ,p.FAM , p.IM  , p.OT ,p.W ,p.DR , p.PHONE ,
p.COMMENT ,f.NameWithID,op.filename,d.DOCTYPE,d.DOCSER,d.DOCNUM,VPOLIS,SPOLIS ,NPOLIS,DBEG ,DRECEIVED,DEND ,DSTOP
  FROM [dbo].[POL_PERSONS] p
left join f003 f on p.mo=f.mcod 
left join pol_events pe
on p.event_guid=pe.idguid
left join pol_polises ps
on p.idguid=ps.person_guid
left join pol_oplist op
on p.idguid=op.person_guid
left join pol_documents d
on p.idguid=d.person_guid
 
 

where ps.spolis = '{
                         ser_blank.Text}' and  npolis LIKE '{
                         num_blank.Text}' order by ID desc", connectionString);
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
    }
  
}















