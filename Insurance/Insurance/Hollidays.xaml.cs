using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using Insurance_SPR;

namespace Insurance
{
    /// <summary>
    /// Логика взаимодействия для Hollydays.xaml
    /// </summary>
    
    public partial class Holidays : Window
    {
        
        public Holidays()
        {
            IList<DateTime> holidays_nav=new List<DateTime>();
            IList<DateTime> works_nav = new List<DateTime>();
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            var holidaysList =
                MyReader.MySelect<HOLIDAYS>($@"select ID, H_DATE,[YEAR],COMMENT from SPR_HOLIDAYS order by H_DATE desc", connectionString);
            holidays_grid.ItemsSource = holidaysList;
            holidays_grid.GroupBy(holidays_grid.Columns[2], DevExpress.Data.ColumnSortOrder.Descending);
            //holidays_grid.GroupBy("YEAR");
            holidays_grid.Columns[0].Width = 50;
            holidays_grid.Columns[1].Width = 70;
            holidays_grid.Columns[3].Width = 250;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand($@"select H_DATE from SPR_HOLIDAYS where STATUS='1'", con);
            SqlCommand comm1 = new SqlCommand($@"select H_DATE from SPR_HOLIDAYS where STATUS in('2','3')", con);
            con.Open();            
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                
                DateTime h_date_ = Convert.ToDateTime(reader["H_DATE"]);
                holidays_nav.Add(h_date_);
                
            }
                
            reader.Close();
            SqlDataReader reader1 = comm1.ExecuteReader();
            while (reader1.Read())
            {

                DateTime h_date_ = Convert.ToDateTime(reader1["H_DATE"]);
                works_nav.Add(h_date_);

            }

            reader1.Close();
            con.Close();
            DateNavigator.ExactWorkdays = works_nav;
            DateNavigator.Holidays = holidays_nav;
                              
                
            //}
            //catch (Exception ex)
            //{
                //string m = "Невозможно проверить обновление программы! Нет подключениея к Internet!";
                //string t = "Внимание!";
                //int b = 1;
                //Message me = new Message(m, t, b);
                //me.ShowDialog();
                ////MessageBox.Show("Невозможно проверить обновление программы! Нет подключениея к Internet!", "Ошибка!");

            //}

        }

        private void Insert_holidays_Click(object sender, RoutedEventArgs e)
        {
            IList<DateTime> holidays_nav = new List<DateTime>();
            IList<DateTime> works_nav = new List<DateTime>();
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            for (int i = 0; i < DateNavigator.SelectedDates.Count; i++)
            {
                SqlCommand comm = new SqlCommand($@"insert into SPR_HOLIDAYS values('{DateNavigator.SelectedDates[i]}',{DateNavigator.SelectedDates[i].Year},'1')", con);
                con.Open();
                comm.ExecuteNonQuery();
                con.Close();
            }
            var holidaysList =
                MyReader.MySelect<HOLIDAYS>($@"select ID, H_DATE,[YEAR],COMMENT from SPR_HOLIDAYS order by H_DATE desc", connectionString);
            holidays_grid.ItemsSource = holidaysList;
            holidays_grid.GroupBy("YEAR");
            SqlCommand comm1 = new SqlCommand($@"select H_DATE from SPR_HOLIDAYS where STATUS='1'", con);
            SqlCommand comm2 = new SqlCommand($@"select H_DATE from SPR_HOLIDAYS where STATUS in('2','3')", con);
            con.Open();
            SqlDataReader reader = comm1.ExecuteReader();
            while (reader.Read())
            {

                DateTime h_date_ = Convert.ToDateTime(reader["H_DATE"]);
                holidays_nav.Add(h_date_);

            }
            reader.Close();
            SqlDataReader reader1 = comm2.ExecuteReader();
            while (reader1.Read())
            {

                DateTime h_date_ = Convert.ToDateTime(reader1["H_DATE"]);
                works_nav.Add(h_date_);

            }
            con.Close();
            DateNavigator.ExactWorkdays = works_nav;
            DateNavigator.Holidays = holidays_nav;
        }

        private void Del_holidays_Click(object sender, RoutedEventArgs e)
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

            var sg_rows_h = Funcs.MyIds(holidays_grid.GetSelectedRowHandles(), holidays_grid);

            string m = "Вы действительно хотите удалить записи?";
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
                SqlCommand comm = new SqlCommand($@"delete from SPR_HOLIDAYS where id in({sg_rows_h})", con);
                con.Open();
                comm.ExecuteNonQuery();
                con.Close();
                var holidaysList =
                    MyReader.MySelect<HOLIDAYS>($@"select ID, H_DATE,[YEAR],COMMENT from SPR_HOLIDAYS order by H_DATE desc", connectionString);
                holidays_grid.ItemsSource = holidaysList;
                holidays_grid.GroupBy("YEAR");
            }
        }

        private void Load_hollidays_Click(object sender, RoutedEventArgs e)
        {
            string year = year_l.Text;
            try
            {
                // Create a request for the URL.        
                HttpWebRequest requestt = (HttpWebRequest)HttpWebRequest.Create($@"http://xmlcalendar.ru/data/ru/{year}/calendar.xml");
                requestt.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
                
                
                HttpWebResponse response = (HttpWebResponse)requestt.GetResponse();
                Stream ReceiveStream1 = response.GetResponseStream();
                //StreamReader sr = new StreamReader(ReceiveStream1, true);
                //string responseFromServer = sr.ReadToEnd();
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(ReceiveStream1);
                
                response.Close();
                
                List<XML_HOLIDAYS> xml_hdays = new List<XML_HOLIDAYS>();
                //List<string> xml_hdays = new List<string>();
                
                XmlElement xRoot = xDoc.DocumentElement;
                XmlNodeList xHol = xRoot.ChildNodes[0].ChildNodes;
                XmlNodeList xDays = xRoot.ChildNodes[1].ChildNodes;
                // обход всех узлов в корневом элементе
                string id1 = "";
                for (int i = 0; i < xHol.Count; i++)
                {
                    string id = xHol[i].Attributes.GetNamedItem("id").Value;
                    //string title = xHol[i].Attributes.GetNamedItem("title").Value;
                    for (int y = 0; y < xDays.Count; y++)
                    {
                        DayOfWeek dw = Convert.ToDateTime($"{year}/" + xDays[y].Attributes.GetNamedItem("d").Value.Replace(".", "/")).DayOfWeek;
                
                        if (xDays[y].Attributes.Count > 2 && dw != DayOfWeek.Sunday && dw != DayOfWeek.Saturday)
                        {
                
                            if (xDays[y].Attributes.GetNamedItem("h").Value == id)
                            {
                                id1 = xDays[y].Attributes.GetNamedItem("h").Value;
                                xml_hdays.Add(
                                new XML_HOLIDAYS
                                {
                                    id = xHol[i].Attributes.GetNamedItem("id").Value,
                                    title = xHol[i].Attributes.GetNamedItem("title").Value,
                                    d = Convert.ToDateTime($"{year}/" + xDays[y].Attributes.GetNamedItem("d").Value.Replace(".", "/")),
                                    t = xDays[y].Attributes.GetNamedItem("t").Value,
                                    h = xDays[y].Attributes.GetNamedItem("h").Value
                                });
                
                            }
                            else
                            {
                                id1 = xDays[y].Attributes.GetNamedItem("h").Value;
                            }
                        }
                        else if (xDays[y].Attributes.Count > 2 && (dw == DayOfWeek.Sunday || dw == DayOfWeek.Saturday))
                        {
                            id1 = xDays[y].Attributes.GetNamedItem("h").Value;
                        }
                        else if (xDays[y].Attributes.Count > 2 && xDays[y].Attributes.GetNamedItem("h").Value != id)
                        {
                            id1 = xDays[y].Attributes.GetNamedItem("h").Value;
                        }
                        else if (xDays[y].Attributes.Count < 3 && dw != DayOfWeek.Sunday && dw != DayOfWeek.Saturday && xDays[y].Attributes.GetNamedItem("t").Value == "1")
                        {
                
                            if (id == id1)
                            {
                                xml_hdays.Add(
                                new XML_HOLIDAYS
                                {
                                    id = xHol[i].Attributes.GetNamedItem("id").Value,
                                    title = "Перенесенный праздничный день",
                                    d = Convert.ToDateTime($"{year}/" + xDays[y].Attributes.GetNamedItem("d").Value.Replace(".", "/")),
                                    t = xDays[y].Attributes.GetNamedItem("t").Value,
                                    h = xHol[i].Attributes.GetNamedItem("id").Value,
                                });
                            }
                        }
                
                
                    }
                }
                for (int y = 0; y < xDays.Count; y++)
                {
                    DayOfWeek dw = Convert.ToDateTime($"{year}/" + xDays[y].Attributes.GetNamedItem("d").Value.Replace(".", "/")).DayOfWeek;
                    if (xDays[y].Attributes.Count < 3 && (dw == DayOfWeek.Sunday || dw == DayOfWeek.Saturday) && (xDays[y].Attributes.GetNamedItem("t").Value == "2" || xDays[y].Attributes.GetNamedItem("t").Value == "2"))
                    {
                        xml_hdays.Add(
                        new XML_HOLIDAYS
                        {
                            id = "0",
                            title = "Рабочий день",
                            d = Convert.ToDateTime($"{year}/" + xDays[y].Attributes.GetNamedItem("d").Value.Replace(".", "/")),
                            t = xDays[y].Attributes.GetNamedItem("t").Value,
                            h = "0",
                        });
                    }
                }
                xml_hdays.Sort((a, b) => a.d.CompareTo(b.d));
                IList<DateTime> holidays_nav = new List<DateTime>();
                IList<DateTime> works_nav = new List<DateTime>();
                var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
                SqlConnection con = new SqlConnection(connectionString);
                for (int i = 0; i < xml_hdays.Count; i++)
                {
                    SqlCommand comm = new SqlCommand($@"insert into SPR_HOLIDAYS values('{xml_hdays[i].d}',{Convert.ToInt32(year_l.Text)},
'{xml_hdays[i].t}','{xml_hdays[i].title}')", con);
                    con.Open();
                    comm.ExecuteNonQuery();
                    con.Close();
                }
                var holidaysList =
                    MyReader.MySelect<HOLIDAYS>($@"select ID, H_DATE,[YEAR],COMMENT from SPR_HOLIDAYS order by H_DATE desc", connectionString);
                holidays_grid.ItemsSource = holidaysList;
                holidays_grid.GroupBy(holidays_grid.Columns[2], DevExpress.Data.ColumnSortOrder.Descending);
                //holidays_grid.GroupBy("YEAR");
                holidays_grid.Columns[0].Width = 50;
                holidays_grid.Columns[1].Width = 70;
                holidays_grid.Columns[3].Width = 250;
                SqlCommand comm1 = new SqlCommand($@"select H_DATE from SPR_HOLIDAYS where STATUS='1'", con);
                SqlCommand comm2 = new SqlCommand($@"select H_DATE from SPR_HOLIDAYS where STATUS in('2','3')", con);
                con.Open();
                SqlDataReader reader = comm1.ExecuteReader();
                while (reader.Read())
                {

                    DateTime h_date_ = Convert.ToDateTime(reader["H_DATE"]);
                    holidays_nav.Add(h_date_);

                }
                reader.Close();
                SqlDataReader reader1 = comm2.ExecuteReader();
                while (reader1.Read())
                {

                    DateTime h_date_ = Convert.ToDateTime(reader1["H_DATE"]);
                    works_nav.Add(h_date_);

                }
                con.Close();
                DateNavigator.ExactWorkdays = works_nav;
                DateNavigator.Holidays = holidays_nav;

            }
            catch (Exception)
            {
                string m = "Невозможно загрузить праздничные дни! Нет подключениея к Internet!";
                string t = "Внимание!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                MessageBox.Show("Невозможно проверить обновление программы! Нет подключениея к Internet!", "Ошибка!");

            }

        }
    }
}
