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
using Yamed.Server;
using Insurance_SPR;
using xNet;
namespace Insurance
{
    /// <summary>
    /// Логика взаимодействия для Window4.xaml
    /// </summary>
    public partial class Poisk_Vladik : Window
    {
        public Poisk_Vladik()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            fam.Focus();
         

        }
        List<string> Type_docZ = new List<string>();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
         
            var req = new HttpRequest();
            var Cookies = new CookieDictionary();

            req.Cookies = Cookies;
            req.UserAgent = Http.ChromeUserAgent();
            req[HttpHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            req[HttpHeader.AcceptLanguage] = "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7";
            var G1 = req.Get("http://192.168.10.13/index.php").ToString();
            var sid = Request.Pars(G1,"sid\" value=\"", "\"");

            req.AllowAutoRedirect = true;
            req.Cookies = Cookies;
            req[HttpHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            req[HttpHeader.AcceptLanguage] = "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7";
            req.UserAgent = Http.ChromeUserAgent();
            var Resp = req.Post("http://192.168.10.13/handler.php", "login=25016&prz=&passw=964687&b1=&hidden=auth_form&sid="+sid, "application/x-www-form-urlencoded").ToString();
            if (Resp.Contains("Выход"))
            {  
                //ПОИСК ПО ФИО И ДР
                if (SerPolis.Text!=""&& NomPolis.Text!= ""&& fam.Text == "" && im.Text == "" && ot.Text == "" && dr.Text == "" && ENP.Text == "" && SNILS.Text == "")
                {

                    req.AllowAutoRedirect = true;
                    req.Cookies = Cookies;
                    req[HttpHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                    req[HttpHeader.AcceptLanguage] = "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7";
                    req.UserAgent = Http.ChromeUserAgent();
                    var ResponsePolis = req.Post("http://192.168.10.13/handler.php", $@"plser={SerPolis.Text}&plnum={NomPolis.Text}&hidden=polis&b1=&sid=" + sid, "application/x-www-form-urlencoded").ToString();
                    var FAM = Request.Pars(ResponsePolis, "FAM1\">", "<");
                    var IM = Request.Pars(ResponsePolis, "IM1\">", "<");
                    var OT = Request.Pars(ResponsePolis, "OT1\">", "<");
                    var DR = Request.Pars(ResponsePolis, "DR1\">", "<");
                    var VP = Request.Pars(ResponsePolis, "VP1\">", "<");
                    var SNILS = Request.Pars(ResponsePolis, "SNILS1\">", "<");
                    var NPOLIS = Request.Pars(ResponsePolis, "NPOLIS1\">", "<");
                    var ENP = Request.Pars(ResponsePolis, "ENP1\">", "<");
                    var MR = Request.Pars(ResponsePolis, "MR1\">", "<");
                    var POL_NAME = Request.Pars(ResponsePolis, "POL_NAME1\">", "<");
                    var SPOSOB = Request.Pars(ResponsePolis, "Способ прикрепления: </span></td>", "</td>");
                    var Data_prikrep = Request.Pars(ResponsePolis, "DAtt1\">", "<");
                    var SMO_NAM = Request.Pars(ResponsePolis, "SMO_NAM1\">", "<");
                    var PRZ_NAM = Request.Pars(ResponsePolis, "PRZ_NAME1\">", "<");
                    var DBEG = Request.Pars(ResponsePolis, "DBEG1\">", "<");
                    var DEND = Request.Pars(ResponsePolis, "DEND1\">", "<");
                    var DSTOP = Request.Pars(ResponsePolis, "DSTOP1\">", "<");
                    var RSTOP = Request.Pars(ResponsePolis, "RSTOP1\">", "<");
                    var DS = Request.Pars(ResponsePolis, "DS1\">", "<");
                    FAM_TFOMS.Text = FAM;
                    IM_TFOMS.Text = IM;
                    OT_TFOMS.Text = OT;
                    DR_TFOMS.Text = DR;
                    VIDPOLIS_TFOMS.Text = VP;
                    SNILS_TFOMS.Text = SNILS;
                    POLIS_TFOMS.Text = NPOLIS;
                    ENP_TFOMS.Text = ENP;
                    MR_TFOMS.Text = MR;
                    POLIKLIN_TFOMS.Text = POL_NAME;
                    DATE_PRIKREP_TFOMS.Text = Data_prikrep;
                    SPOSOB_TFOMS.Text = SPOSOB.Replace("<td>", "").Trim();
                    SMO_TFOMS.Text = SMO_NAM;
                    PRZ_TFOMS.Text = PRZ_NAM;
                    DATE_START_TFOMS.Text = DBEG;

                    if (DEND != "..")
                    {
                        DATE_END_TFOMS.Text = DEND;
                    }
                    else
                    {
                        DATE_END_TFOMS.Text = "";
                    }

                    if (DSTOP != "..")
                    {
                        DATE_NULL_TFOMS.Text = DSTOP;
                    }
                    else
                    {
                        DATE_NULL_TFOMS.Text = "";
                    }

                    if (RSTOP != "..")
                    {
                        PRICIHIA_NULL_TFOMS.Text = RSTOP;
                    }
                    else
                    {
                        PRICIHIA_NULL_TFOMS.Text = "";
                    }

                    if (DS != "..")
                    {
                        DATE_DEAD_TFOMS.Text = DS;
                    }
                    else
                    {
                        DATE_DEAD_TFOMS.Text = "";
                    }

                    if (DS!="" && DS!="..")
                    {
                        DATE_DEAD_TFOMS.Background = Brushes.Red;
                    }

                }
                //ПОИСК ПО СЕРИИ И НОМЕРУ
                else if (SerPolis.Text == "" && NomPolis.Text == "" && fam.Text == "" && im.Text == "" && ot.Text == "" && dr.Text == "" && ENP.Text != "" && SNILS.Text == "")
                {

                    req.AllowAutoRedirect = true;
                    req.Cookies = Cookies;
                    req[HttpHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                    req[HttpHeader.AcceptLanguage] = "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7";
                    req.UserAgent = Http.ChromeUserAgent();
                    var ResponsePolis = req.Post("http://192.168.10.13/handler.php", $@"plnum={ENP_TFOMS.Text}&hidden=enponly&b1=&sid=" + sid, "application/x-www-form-urlencoded").ToString();
                    var FAM = Request.Pars(ResponsePolis, "FAM1\">", "<");
                    var IM = Request.Pars(ResponsePolis, "IM1\">", "<");
                    var OT = Request.Pars(ResponsePolis, "OT1\">", "<");
                    var DR = Request.Pars(ResponsePolis, "DR1\">", "<");
                    var VP = Request.Pars(ResponsePolis, "VP1\">", "<");
                    var SNILS = Request.Pars(ResponsePolis, "SNILS1\">", "<");
                    var NPOLIS = Request.Pars(ResponsePolis, "NPOLIS1\">", "<");
                    var ENP = Request.Pars(ResponsePolis, "ENP1\">", "<");
                    var MR = Request.Pars(ResponsePolis, "MR1\">", "<");
                    var POL_NAME = Request.Pars(ResponsePolis, "POL_NAME1\">", "<");
                    var SPOSOB = Request.Pars(ResponsePolis, "Способ прикрепления: </span></td>", "</td>");
                    var Data_prikrep = Request.Pars(ResponsePolis, "DAtt1\">", "<");
                    var SMO_NAM = Request.Pars(ResponsePolis, "SMO_NAM1\">", "<");
                    var PRZ_NAM = Request.Pars(ResponsePolis, "PRZ_NAME1\">", "<");
                    var DBEG = Request.Pars(ResponsePolis, "DBEG1\">", "<");
                    var DEND = Request.Pars(ResponsePolis, "DEND1\">", "<");
                    var DSTOP = Request.Pars(ResponsePolis, "DSTOP1\">", "<");
                    var RSTOP = Request.Pars(ResponsePolis, "RSTOP1\">", "<");
                    var DS = Request.Pars(ResponsePolis, "DS1\">", "<");
                    FAM_TFOMS.Text = FAM;
                    IM_TFOMS.Text = IM;
                    OT_TFOMS.Text = OT;
                    DR_TFOMS.Text = DR;
                    VIDPOLIS_TFOMS.Text = VP;
                    SNILS_TFOMS.Text = SNILS;
                    POLIS_TFOMS.Text = NPOLIS;
                    ENP_TFOMS.Text = ENP;
                    MR_TFOMS.Text = MR;
                    POLIKLIN_TFOMS.Text = POL_NAME;
                    DATE_PRIKREP_TFOMS.Text = Data_prikrep;
                    SPOSOB_TFOMS.Text = SPOSOB.Replace("<td>", "").Trim();
                    SMO_TFOMS.Text = SMO_NAM;
                    PRZ_TFOMS.Text = PRZ_NAM;
                    DATE_START_TFOMS.Text = DBEG;

                    if (DEND != "..")
                    {
                        DATE_END_TFOMS.Text = DEND;
                    }
                    else
                    {
                        DATE_END_TFOMS.Text = "";
                    }

                    if (DSTOP != "..")
                    {
                        DATE_NULL_TFOMS.Text = DSTOP;
                    }
                    else
                    {
                        DATE_NULL_TFOMS.Text = "";
                    }

                    if (RSTOP != "..")
                    {
                        PRICIHIA_NULL_TFOMS.Text = RSTOP;
                    }
                    else
                    {
                        PRICIHIA_NULL_TFOMS.Text = "";
                    }

                    if (DS != "..")
                    {
                        DATE_DEAD_TFOMS.Text = DS;
                    }
                    else
                    {
                        DATE_DEAD_TFOMS.Text = "";
                    }

                    if (DS != "" && DS != "..")
                    {
                        DATE_DEAD_TFOMS.Background = Brushes.Red;
                    }

                }
                //ПОИСК ПО ENP
                else if (SerPolis.Text == "" && NomPolis.Text == "" && fam.Text == "" && im.Text == ""  && dr.Text == "" && ENP.Text != "" && SNILS.Text == "")
                {

                    req.AllowAutoRedirect = true;
                    req.Cookies = Cookies;
                    req[HttpHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                    req[HttpHeader.AcceptLanguage] = "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7";
                    req.UserAgent = Http.ChromeUserAgent();
                    var ResponsePolis = req.Post("http://192.168.10.13/handler.php", $@"plnum={ENP_TFOMS.Text}&hidden=enponly&b1=&sid=" + sid, "application/x-www-form-urlencoded").ToString();
                    var FAM = Request.Pars(ResponsePolis, "FAM1\">", "<");
                    var IM = Request.Pars(ResponsePolis, "IM1\">", "<");
                    var OT = Request.Pars(ResponsePolis, "OT1\">", "<");
                    var DR = Request.Pars(ResponsePolis, "DR1\">", "<");
                    var VP = Request.Pars(ResponsePolis, "VP1\">", "<");
                    var SNILS = Request.Pars(ResponsePolis, "SNILS1\">", "<");
                    var NPOLIS = Request.Pars(ResponsePolis, "NPOLIS1\">", "<");
                    var ENP = Request.Pars(ResponsePolis, "ENP1\">", "<");
                    var MR = Request.Pars(ResponsePolis, "MR1\">", "<");
                    var POL_NAME = Request.Pars(ResponsePolis, "POL_NAME1\">", "<");
                    var SPOSOB = Request.Pars(ResponsePolis, "Способ прикрепления: </span></td>", "</td>");
                    var Data_prikrep = Request.Pars(ResponsePolis, "DAtt1\">", "<");
                    var SMO_NAM = Request.Pars(ResponsePolis, "SMO_NAM1\">", "<");
                    var PRZ_NAM = Request.Pars(ResponsePolis, "PRZ_NAME1\">", "<");
                    var DBEG = Request.Pars(ResponsePolis, "DBEG1\">", "<");
                    var DEND = Request.Pars(ResponsePolis, "DEND1\">", "<");
                    var DSTOP = Request.Pars(ResponsePolis, "DSTOP1\">", "<");
                    var RSTOP = Request.Pars(ResponsePolis, "RSTOP1\">", "<");
                    var DS = Request.Pars(ResponsePolis, "DS1\">", "<");
                    FAM_TFOMS.Text = FAM;
                    IM_TFOMS.Text = IM;
                    OT_TFOMS.Text = OT;
                    DR_TFOMS.Text = DR;
                    VIDPOLIS_TFOMS.Text = VP;
                    SNILS_TFOMS.Text = SNILS;
                    POLIS_TFOMS.Text = NPOLIS;
                    ENP_TFOMS.Text = ENP;
                    MR_TFOMS.Text = MR;
                    POLIKLIN_TFOMS.Text = POL_NAME;
                    DATE_PRIKREP_TFOMS.Text = Data_prikrep;
                    SPOSOB_TFOMS.Text = SPOSOB.Replace("<td>", "").Trim();
                    SMO_TFOMS.Text = SMO_NAM;
                    PRZ_TFOMS.Text = PRZ_NAM;
                    DATE_START_TFOMS.Text = DBEG;

                    if (DEND != "..")
                    {
                        DATE_END_TFOMS.Text = DEND;
                    }
                    else
                    {
                        DATE_END_TFOMS.Text = "";
                    }

                    if (DSTOP != "..")
                    {
                        DATE_NULL_TFOMS.Text = DSTOP;
                    }
                    else
                    {
                        DATE_NULL_TFOMS.Text = "";
                    }

                    if (RSTOP != "..")
                    {
                        PRICIHIA_NULL_TFOMS.Text = RSTOP;
                    }
                    else
                    {
                        PRICIHIA_NULL_TFOMS.Text = "";
                    }

                    if (DS != "..")
                    {
                        DATE_DEAD_TFOMS.Text = DS;
                    }
                    else
                    {
                        DATE_DEAD_TFOMS.Text = "";
                    }

                    if (DS != "" && DS != "..")
                    {
                        DATE_DEAD_TFOMS.Background = Brushes.Red;
                    }

                }
                //ПОИСК ПО Фамилиии и Документу удостоверяющего личность
                else if (SerPolis.Text == "" && NomPolis.Text == "" && fam.Text == "" && im.Text == "" && ot.Text == "" && dr.Text == "" && ENP.Text == "" && SNILS.Text == "" && Type_doc.Text !="" && SerPolis_Passport.Text !="" && NomPolis_Passport.Text !="")
                {

                    req.AllowAutoRedirect = true;
                    req.Cookies = Cookies;
                    req[HttpHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                    req[HttpHeader.AcceptLanguage] = "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7";
                    req.UserAgent = Http.ChromeUserAgent();
                    var ResponsePolis = req.Post("http://192.168.10.13/handler.php", $@"f1=%D0%9C%D0%A3%D0%A5%D0%90%D0%9C%D0%95%D0%A2%D0%93%D0%90%D0%9B%D0%98%D0%9C%D0%9E%D0%92&im1=%D0%92%D0%90%D0%A1%D0%98%D0%9B%D0%98%D0%99&ot1=%D0%9C%D0%95%D0%9D%D0%90%D0%A4%D0%90%D0%97%D0%95%D0%9B%D0%9E%D0%92%D0%98%D0%A7&dd=08&mm=%D0%9D%D0%BE%D1%8F%D0%B1%D1%80%D1%8C&yy=1977&hidden=personal&b1=&sid=" + sid, "application/x-www-form-urlencoded").ToString();
                    var FAM = Request.Pars(ResponsePolis, "FAM1\">", "<");
                    var IM = Request.Pars(ResponsePolis, "IM1\">", "<");
                    var OT = Request.Pars(ResponsePolis, "OT1\">", "<");
                    var DR = Request.Pars(ResponsePolis, "DR1\">", "<");
                    var VP = Request.Pars(ResponsePolis, "VP1\">", "<");
                    var SNILS = Request.Pars(ResponsePolis, "SNILS1\">", "<");
                    var NPOLIS = Request.Pars(ResponsePolis, "NPOLIS1\">", "<");
                    var ENP = Request.Pars(ResponsePolis, "ENP1\">", "<");
                    var MR = Request.Pars(ResponsePolis, "MR1\">", "<");
                    var POL_NAME = Request.Pars(ResponsePolis, "POL_NAME1\">", "<");
                    var SPOSOB = Request.Pars(ResponsePolis, "Способ прикрепления: </span></td>", "</td>");
                    var Data_prikrep = Request.Pars(ResponsePolis, "DAtt1\">", "<");
                    var SMO_NAM = Request.Pars(ResponsePolis, "SMO_NAM1\">", "<");
                    var PRZ_NAM = Request.Pars(ResponsePolis, "PRZ_NAME1\">", "<");
                    var DBEG = Request.Pars(ResponsePolis, "DBEG1\">", "<");
                    var DEND = Request.Pars(ResponsePolis, "DEND1\">", "<");
                    var DSTOP = Request.Pars(ResponsePolis, "DSTOP1\">", "<");
                    var RSTOP = Request.Pars(ResponsePolis, "RSTOP1\">", "<");
                    var DS = Request.Pars(ResponsePolis, "DS1\">", "<");
                    FAM_TFOMS.Text = FAM;
                    IM_TFOMS.Text = IM;
                    OT_TFOMS.Text = OT;
                    DR_TFOMS.Text = DR;
                    VIDPOLIS_TFOMS.Text = VP;
                    SNILS_TFOMS.Text = SNILS;
                    POLIS_TFOMS.Text = NPOLIS;
                    ENP_TFOMS.Text = ENP;
                    MR_TFOMS.Text = MR;
                    POLIKLIN_TFOMS.Text = POL_NAME;
                    DATE_PRIKREP_TFOMS.Text = Data_prikrep;
                    SPOSOB_TFOMS.Text = SPOSOB.Replace("<td>", "").Trim();
                    SMO_TFOMS.Text = SMO_NAM;
                    PRZ_TFOMS.Text = PRZ_NAM;
                    DATE_START_TFOMS.Text = DBEG;

                    if (DEND != "..")
                    {
                        DATE_END_TFOMS.Text = DEND;
                    }
                    else
                    {
                        DATE_END_TFOMS.Text = "";
                    }

                    if (DSTOP != "..")
                    {
                        DATE_NULL_TFOMS.Text = DSTOP;
                    }
                    else
                    {
                        DATE_NULL_TFOMS.Text = "";
                    }

                    if (RSTOP != "..")
                    {
                        PRICIHIA_NULL_TFOMS.Text = RSTOP;
                    }
                    else
                    {
                        PRICIHIA_NULL_TFOMS.Text = "";
                    }

                    if (DS != "..")
                    {
                        DATE_DEAD_TFOMS.Text = DS;
                    }
                    else
                    {
                        DATE_DEAD_TFOMS.Text = "";
                    }

                    if (DS != "" && DS != "..")
                    {
                        DATE_DEAD_TFOMS.Background = Brushes.Red;
                    }

                }
                // Поиск 
                else if (fam.Text == "")
                {
                    req.AllowAutoRedirect = true;
                    req.Cookies = Cookies;
                    req[HttpHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                    req[HttpHeader.AcceptLanguage] = "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7";
                    req.UserAgent = Http.ChromeUserAgent();
                    var ResponsePolis = req.Post("http://192.168.10.13/handler.php", $@"f1=%D0%9C%D0%A3%D0%A5%D0%90%D0%9C%D0%95%D0%A2%D0%93%D0%90%D0%9B%D0%98%D0%9C%D0%9E%D0%92&im1=%D0%92%D0%90%D0%A1%D0%98%D0%9B%D0%98%D0%99&ot1=%D0%9C%D0%95%D0%9D%D0%90%D0%A4%D0%90%D0%97%D0%95%D0%9B%D0%9E%D0%92%D0%98%D0%A7&dd=08&mm=%D0%9D%D0%BE%D1%8F%D0%B1%D1%80%D1%8C&yy=1977&hidden=personal&b1=&sid=" + sid, "application/x-www-form-urlencoded").ToString();
                    if (ResponsePolis.Contains("html"))
                    {
                        var FAM = Request.Pars(ResponsePolis, "FAM1\">", "<");
                        var IM = Request.Pars(ResponsePolis, "IM1\">", "<");
                        var OT = Request.Pars(ResponsePolis, "OT1\">", "<");
                        var DR = Request.Pars(ResponsePolis, "DR1\">", "<");
                        var VP = Request.Pars(ResponsePolis, "VP1\">", "<");
                        var SNILS = Request.Pars(ResponsePolis, "SNILS1\">", "<");
                        var NPOLIS = Request.Pars(ResponsePolis, "NPOLIS1\">", "<");
                        var ENP = Request.Pars(ResponsePolis, "ENP1\">", "<");
                        var MR = Request.Pars(ResponsePolis, "MR1\">", "<");
                        var POL_NAME = Request.Pars(ResponsePolis, "POL_NAME1\">", "<");
                        var SPOSOB = Request.Pars(ResponsePolis, "Способ прикрепления: </span></td>", "</td>");
                        var Data_prikrep = Request.Pars(ResponsePolis, "DAtt1\">", "<");
                        var SMO_NAM = Request.Pars(ResponsePolis, "SMO_NAM1\">", "<");
                        var PRZ_NAM = Request.Pars(ResponsePolis, "PRZ_NAME1\">", "<");
                        var DBEG = Request.Pars(ResponsePolis, "DBEG1\">", "<");
                        var DEND = Request.Pars(ResponsePolis, "DEND1\">", "<");
                        var DSTOP = Request.Pars(ResponsePolis, "DSTOP1\">", "<");
                        var RSTOP = Request.Pars(ResponsePolis, "RSTOP1\">", "<");
                        var DS = Request.Pars(ResponsePolis, "DS1\">", "<");
                        FAM_TFOMS.Text = FAM;
                        IM_TFOMS.Text = IM;
                        OT_TFOMS.Text = OT;
                        DR_TFOMS.Text = DR;
                        VIDPOLIS_TFOMS.Text = VP;
                        SNILS_TFOMS.Text = SNILS;
                        POLIS_TFOMS.Text = NPOLIS;
                        ENP_TFOMS.Text = ENP;
                        MR_TFOMS.Text = MR;
                        POLIKLIN_TFOMS.Text = POL_NAME;
                        DATE_PRIKREP_TFOMS.Text = Data_prikrep;
                        SPOSOB_TFOMS.Text = SPOSOB.Replace("<td>", "").Trim();
                        SMO_TFOMS.Text = SMO_NAM;
                        PRZ_TFOMS.Text = PRZ_NAM;
                        DATE_START_TFOMS.Text = DBEG;

                        if (DEND != "..")
                        {
                            DATE_END_TFOMS.Text = DEND;
                        }
                        else
                        {
                            DATE_END_TFOMS.Text = "";
                        }

                        if (DSTOP != "..")
                        {
                            DATE_NULL_TFOMS.Text = DSTOP;
                        }
                        else
                        {
                            DATE_NULL_TFOMS.Text = "";
                        }

                        if (RSTOP != "..")
                        {
                            PRICIHIA_NULL_TFOMS.Text = RSTOP;
                        }
                        else
                        {
                            PRICIHIA_NULL_TFOMS.Text = "";
                        }

                        if (DS != "..")
                        {
                            DATE_DEAD_TFOMS.Text = DS;
                        }
                        else
                        {
                            DATE_DEAD_TFOMS.Text = "";
                        }

                        if (DS != "" && DS != "..")
                        {
                            DATE_DEAD_TFOMS.Background = Brushes.Red;
                        }
                    }
                }





            }

           
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            fam.Text = "";
            im.Text = "";
            ot.Text = "";
            dr.EditValue = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Type_docZ.Clear();
            Type_docZ.Add("Паспорт гражданина СССР");
            Type_docZ.Add("Свидетельство о рассмотрении ходатайства о признании иммигранта беженцем на территории Российской Федерации");
            Type_docZ.Add("Вид на жительство");
            Type_docZ.Add("Удостоверение беженца в Российской Федерации");
            Type_docZ.Add("Временное удостоверение личности гражданина Российской Федерации");
            Type_docZ.Add("Паспорт гражданина Российской Федерации");
            Type_docZ.Add("Заграничный паспорт гражданина Российской Федерации");
            Type_docZ.Add("Паспорт моряка");
            Type_docZ.Add("Военный билет офицера запаса");
            Type_docZ.Add("Иные документы");
            Type_docZ.Add("Загранпаспорт гражданина СССР");
            Type_docZ.Add("Документ иностранного гражданина");
            Type_docZ.Add("Документ лица без гражданства");
            Type_docZ.Add("Разрешение на временное проживание");
            Type_docZ.Add("Свидетельство о рождении, выданное не в Российской Федерации");
            Type_docZ.Add("Свидетельство о предоставлении временного убежища на территории Российской Федерации");
            Type_docZ.Add("Удостоверение сотрудника Евразийской экономической комиссии");
            Type_docZ.Add("Копия жалобы о лишении статуса беженца");
            Type_docZ.Add("Иной документ, соответствующий свидетельству о предоставлении убежища на территории Российской Федерации");
            Type_docZ.Add("Трудовой договор");
            Type_docZ.Add("Свидетельство о рождении, выданное в Российской Федерации");
            Type_docZ.Add("Удостоверение личности офицера");
            Type_docZ.Add("Справка об освобождении из места лишения свободы");
            Type_docZ.Add("Паспорт Минморфлота");
            Type_docZ.Add("Военный билет");
            Type_docZ.Add("Дипломатический паспорт гражданина Российской Федерации");
            Type_docZ.Add("Паспорт иностранного гражданина");
            Type_doc.ItemsSource = Type_docZ;
        }
    }
    
}
