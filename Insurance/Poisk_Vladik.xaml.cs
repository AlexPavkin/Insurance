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
using Insurance.Classes;

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
        Dictionary<string, string> Month = new Dictionary<string, string>();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
        var req = new HttpRequest();
        var Cookies = new CookieDictionary();

            req.Cookies = Cookies;
            req.UserAgent = Http.ChromeUserAgent();
            req[HttpHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            req[HttpHeader.AcceptLanguage] = "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7";
            var G1 = req.Get("http://192.168.10.13/index.php").ToString();
            var sid = Request.Pars(G1, "sid\" value=\"", "\"");

            req.AllowAutoRedirect = true;
            req.Cookies = Cookies;
            req[HttpHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            req[HttpHeader.AcceptLanguage] = "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7";
            req.UserAgent = Http.ChromeUserAgent();
            var Resp = req.Post("http://192.168.10.13/handler.php", "login=25016&prz=&passw=964687&b1=&hidden=auth_form&sid=" + sid, "application/x-www-form-urlencoded").ToString();
            if (Resp.Contains("Выход"))
            {
                //ПОИСК ПО Серии и номеру полиса
                if (SerPolis.Text != "" && NomPolis.Text != "" && fam.Text == "" && im.Text == "" && ot.Text == "" && dr.Text == "" && ENP.Text == "" && SNILSF.Text == "")
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
                    POLIS_TFOMS.Text = NPOLIS.Trim();
                    ENP_TFOMS.Text = ENP;
                    MR_TFOMS.Text = MR;
                    POLIKLIN_TFOMS.Text = POL_NAME;
                    DATE_PRIKREP_TFOMS.Text = Data_prikrep;
                    SPOSOB_TFOMS.Text = SPOSOB.Replace("<td>", "").Trim();
                    SMO_TFOMS.Text = SMO_NAM;
                    PRZ_TFOMS.Text = PRZ_NAM;
                    DATE_START_TFOMS.Text = DBEG;

                    if (DEND != ".. ")
                    {
                        DATE_END_TFOMS.Text = DEND;
                    }
                    else
                    {
                        DATE_END_TFOMS.Text = "";
                    }

                    if (DSTOP != ".. ")
                    {
                        DATE_NULL_TFOMS.Text = DSTOP;
                    }
                    else
                    {
                        DATE_NULL_TFOMS.Text = "";
                    }

                    if (RSTOP != ".. ")
                    {
                        PRICIHIA_NULL_TFOMS.Text = RSTOP;
                    }
                    else
                    {
                        PRICIHIA_NULL_TFOMS.Text = "";
                    }

                    if (DS != ".. ")
                    {
                        DATE_DEAD_TFOMS.Text = DS;
                    }
                    else
                    {
                        DATE_DEAD_TFOMS.Text = "";
                    }

                    if (DS != "" && DS != ".. ")
                    {
                        DATE_DEAD_TFOMS.Background = Brushes.Red;
                    }
                    

                    string load_pers_grid = $@" SELECT TOP(1) pp.SROKDOVERENOSTI,pp.ID,pp.ACTIVE,op.przcod,pe.UNLOAD,ENP ,FAM , IM  , OT ,W ,DR ,MO,oks.CAPTION as C_OKSM,r.NameWithID , pp.COMMENT,pe.DVIZIT, pp.DATEVIDACHI, pp.PRIZNAKVIDACHI,
            SS  ,VPOLIS,SPOLIS ,NPOLIS,DBEG ,DEND ,DSTOP ,BLANK ,DRECEIVED,f.NameWithId as MO_NameWithId,op.filename,pp.phone,p.AGENT, pp.CYCLE, st.namewithkod as STOP_REASON, pe.id as EVENT_ID
              FROM [dbo].[POL_PERSONS] pp left join 
            pol_events pe on pp.event_guid=pe.idguid
         	LEFT JOIN POL_PRZ_AGENTS p
			on p.ID = pe.AGENT
		    left join pol_polises ps
            on pp.EVENT_GUID=ps.EVENT_GUID
            left join pol_oplist op
            on pp.EVENT_GUID=op.EVENT_GUID
            left join r001 r
            on pe.tip_op=r.kod
            left join f003 f
            on pp.MO=f.mcod
            left join SPR_STOP st
            on ps.STOP_REASON=st.kod
			left join SPR_79_OKSM oks
			on pp.C_OKSM=oks.A3
			where pp.FAM = '{FAM}'
			and pp.IM = '{IM}'
			and pp.OT = '{OT}'
			and pp.DR = '{DR}'
			order by pe.ID DESC";
                 var List =  MyReader.MySelect<Events>(load_pers_grid, Properties.Settings.Default.DocExchangeConnectionString);
                    if (List.Count != 0)
                    {
                        FAM_B.Text = List[0].FAM;
                        IM_B.Text = List[0].IM;
                        OT_B.Text = List[0].OT;
                        DR_B.EditValue = List[0].DR;
                        Vars.IdP = List[0].ID.ToString();
                        Vars.Btn = "2";
                        if (List[0].VPOLIS == 1)
                        {
                            VIDPOLISA_B.Text = "Полис ОМС старого образца";
                        }
                        else if (List[0].VPOLIS == 2)
                        {
                            VIDPOLISA_B.Text = "Временное свидетельство, подтверждающее оформление полиса обязательного медицинского страхования";
                        }
                        else if (List[0].VPOLIS == 3)
                        {
                            VIDPOLISA_B.Text = "Полис ОМС единого образца";
                        }
                        SNILS_B.Text = List[0].SS;
                        POLIS_B.Text = List[0].SPOLIS.Trim() + " " + List[0].NPOLIS.Trim();
                        ENP_B.Text = List[0].ENP;
                        MR_B.Text = MR;
                        ENP_B.Text = List[0].ENP;
                        POLIK_B.Text = POLIKLIN_TFOMS.Text;
                        DATE_P_B.Text = DATE_PRIKREP_TFOMS.Text;
                        SPOSOB_B.Text = SPOSOB.Replace("<td>", "").Trim();
                        //SMO_B.Text = List[0].;
                        PRZ_B.Text = List[0].PRZCOD;
                        DATE_BEG_B.EditValue = List[0].DBEG;
                        D_END_B.EditValue = List[0].DEND;
                        D_NULL_B.EditValue = List[0].DSTOP;
                        PRICH_B.Text = List[0].STOP_REASON;
                        
                    }
                    else
                    {
                        string m = "В нашей базе не найден данный ЗЛ!";
                        string t = "Ошибка!";
                        int b = 1;
                        Message me = new Message(m, t, b);
                        me.ShowDialog();

                        return;

                    }

                }
                //ПОИСК ПО ЕНП
                else if (SerPolis.Text == "" && NomPolis.Text == "" && fam.Text == "" && im.Text == "" && ot.Text == "" && dr.Text == "" && this.ENP.Text != "" && SNILSF.Text == "")
                {

                    req.AllowAutoRedirect = true;
                    req.Cookies = Cookies;
                    req[HttpHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                    req[HttpHeader.AcceptLanguage] = "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7";
                    req.UserAgent = Http.ChromeUserAgent();
                    var ResponsePolis = req.Post("http://192.168.10.13/handler.php", $@"plnum={this.ENP.Text}&hidden=enponly&b1=&sid=" + sid, "application/x-www-form-urlencoded").ToString();
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
                    POLIS_TFOMS.Text = NPOLIS.Trim();
                    ENP_TFOMS.Text = ENP;
                    MR_TFOMS.Text = MR;
                    POLIKLIN_TFOMS.Text = POL_NAME;
                    DATE_PRIKREP_TFOMS.Text = Data_prikrep;
                    SPOSOB_TFOMS.Text = SPOSOB.Replace("<td>", "").Trim();
                    SMO_TFOMS.Text = SMO_NAM;
                    PRZ_TFOMS.Text = PRZ_NAM;
                    DATE_START_TFOMS.Text = DBEG;

                    if (DEND != ".. ")
                    {
                        DATE_END_TFOMS.Text = DEND;
                    }
                    else
                    {
                        DATE_END_TFOMS.Text = "";
                    }

                    if (DSTOP != ".. ")
                    {
                        DATE_NULL_TFOMS.Text = DSTOP;
                    }
                    else
                    {
                        DATE_NULL_TFOMS.Text = "";
                    }

                    if (RSTOP != ".. ")
                    {
                        PRICIHIA_NULL_TFOMS.Text = RSTOP;
                    }
                    else
                    {
                        PRICIHIA_NULL_TFOMS.Text = "";
                    }

                    if (DS != ".. ")
                    {
                        DATE_DEAD_TFOMS.Text = DS;
                    }
                    else
                    {
                        DATE_DEAD_TFOMS.Text = "";
                    }

                    if (DS != "" && DS != ".. ")
                    {
                        DATE_DEAD_TFOMS.Background = Brushes.Red;
                    }


                    string load_pers_grid = $@"SELECT TOP(1) pp.SROKDOVERENOSTI,pp.ID,pp.ACTIVE,op.przcod,pe.UNLOAD,ENP ,FAM , IM  , OT ,W ,DR ,MO,oks.CAPTION as C_OKSM,r.NameWithID , pp.COMMENT,pe.DVIZIT, pp.DATEVIDACHI, pp.PRIZNAKVIDACHI,
            SS  ,VPOLIS,SPOLIS ,NPOLIS,DBEG ,DEND ,DSTOP ,BLANK ,DRECEIVED,f.NameWithId as MO_NameWithId,op.filename,pp.phone,p.AGENT, pp.CYCLE, st.namewithkod as STOP_REASON, pe.id as EVENT_ID
              FROM [dbo].[POL_PERSONS] pp left join 
            pol_events pe on pp.event_guid=pe.idguid
         	LEFT JOIN POL_PRZ_AGENTS p
			on p.ID = pe.AGENT
		    left join pol_polises ps
            on pp.EVENT_GUID=ps.EVENT_GUID
            left join pol_oplist op
            on pp.EVENT_GUID=op.EVENT_GUID
            left join r001 r
            on pe.tip_op=r.kod
            left join f003 f
            on pp.MO=f.mcod
            left join SPR_STOP st
            on ps.STOP_REASON=st.kod
			left join SPR_79_OKSM oks
			on pp.C_OKSM=oks.A3
			where pp.FAM = '{FAM}'
			and pp.IM = '{IM}'
			and pp.OT = '{OT}'
			and pp.DR = '{DR}'
			order by pe.ID DESC";
                    var List = MyReader.MySelect<Events>(load_pers_grid, Properties.Settings.Default.DocExchangeConnectionString);
                    if (List.Count != 0)
                    {
                        FAM_B.Text = List[0].FAM;
                        IM_B.Text = List[0].IM;
                        OT_B.Text = List[0].OT;
                        DR_B.EditValue = List[0].DR;
                        Vars.IdP = List[0].ID.ToString();
                        Vars.Btn = "2";
                        if (List[0].VPOLIS == 1)
                        {
                            VIDPOLISA_B.Text = "Полис ОМС старого образца";
                        }
                        else if (List[0].VPOLIS == 2)
                        {
                            VIDPOLISA_B.Text = "Временное свидетельство, подтверждающее оформление полиса обязательного медицинского страхования";
                        }
                        else if (List[0].VPOLIS == 3)
                        {
                            VIDPOLISA_B.Text = "Полис ОМС единого образца";
                        }
                        SNILS_B.Text = List[0].SS;
                        POLIS_B.Text = List[0].SPOLIS.Trim() + " " + List[0].NPOLIS.Trim();
                        ENP_B.Text = List[0].ENP;
                        MR_B.Text = MR;
                        ENP_B.Text = List[0].ENP;
                        POLIK_B.Text = POLIKLIN_TFOMS.Text;
                        DATE_P_B.Text = DATE_PRIKREP_TFOMS.Text;
                        SPOSOB_B.Text = SPOSOB.Replace("<td>", "").Trim();
                        //SMO_B.Text = List[0].;
                        PRZ_B.Text = List[0].PRZCOD;
                        DATE_BEG_B.EditValue = List[0].DBEG;
                        D_END_B.EditValue = List[0].DEND;
                        D_NULL_B.EditValue = List[0].DSTOP;
                        PRICH_B.Text = List[0].STOP_REASON;

                    }
                    else
                    {
                        string m = "В нашей базе не найден данный ЗЛ!";
                        string t = "Ошибка!";
                        int b = 1;
                        Message me = new Message(m, t, b);
                        me.ShowDialog();

                        return;

                    }


                }
                //ПОИСК ПО номеру временного свидетельства
                else if (SerPolis.Text == "" && NomPolis.Text != "" && fam.Text == "" && im.Text == "" && dr.Text == "" && ENP.Text == "" && SNILSF.Text == "")
                {

                    req.AllowAutoRedirect = true;
                    req.Cookies = Cookies;
                    req[HttpHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                    req[HttpHeader.AcceptLanguage] = "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7";
                    req.UserAgent = Http.ChromeUserAgent();
                    var ResponsePolis = req.Post("http://192.168.10.13/handler.php", $@"plnum={ENP_TFOMS.Text}&hidden=svid&b1=&sid=" + sid, "application/x-www-form-urlencoded").ToString();
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
                    POLIS_TFOMS.Text = NPOLIS.Trim();
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

                    if (DSTOP != ".. ")
                    {
                        DATE_NULL_TFOMS.Text = DSTOP;
                    }
                    else
                    {
                        DATE_NULL_TFOMS.Text = "";
                    }

                    if (RSTOP != ".. ")
                    {
                        PRICIHIA_NULL_TFOMS.Text = RSTOP;
                    }
                    else
                    {
                        PRICIHIA_NULL_TFOMS.Text = "";
                    }

                    if (DS != ".. ")
                    {
                        DATE_DEAD_TFOMS.Text = DS;
                    }
                    else
                    {
                        DATE_DEAD_TFOMS.Text = "";
                    }

                    if (DS != "" && DS != ".. ")
                    {
                        DATE_DEAD_TFOMS.Background = Brushes.Red;
                    }
                    string load_pers_grid = $@" SELECT TOP(1) pp.SROKDOVERENOSTI,pp.ID,pp.ACTIVE,op.przcod,pe.UNLOAD,ENP ,FAM , IM  , OT ,W ,DR ,MO,oks.CAPTION as C_OKSM,r.NameWithID , pp.COMMENT,pe.DVIZIT, pp.DATEVIDACHI, pp.PRIZNAKVIDACHI,
            SS  ,VPOLIS,SPOLIS ,NPOLIS,DBEG ,DEND ,DSTOP ,BLANK ,DRECEIVED,f.NameWithId as MO_NameWithId,op.filename,pp.phone,p.AGENT, pp.CYCLE, st.namewithkod as STOP_REASON, pe.id as EVENT_ID
              FROM [dbo].[POL_PERSONS] pp left join 
            pol_events pe on pp.event_guid=pe.idguid
         	LEFT JOIN POL_PRZ_AGENTS p
			on p.ID = pe.AGENT
		    left join pol_polises ps
            on pp.EVENT_GUID=ps.EVENT_GUID
            left join pol_oplist op
            on pp.EVENT_GUID=op.EVENT_GUID
            left join r001 r
            on pe.tip_op=r.kod
            left join f003 f
            on pp.MO=f.mcod
            left join SPR_STOP st
            on ps.STOP_REASON=st.kod
			left join SPR_79_OKSM oks
			on pp.C_OKSM=oks.A3
			where pp.FAM = '{FAM}'
			and pp.IM = '{IM}'
			and pp.OT = '{OT}'
			and pp.DR = '{DR}'
			order by pe.ID DESC";
                    var List = MyReader.MySelect<Events>(load_pers_grid, Properties.Settings.Default.DocExchangeConnectionString);
                    if (List.Count != 0)
                    {
                        FAM_B.Text = List[0].FAM;
                        IM_B.Text = List[0].IM;
                        OT_B.Text = List[0].OT;
                        DR_B.EditValue = List[0].DR;
                        Vars.IdP = List[0].ID.ToString();
                        Vars.Btn = "2";
                        if (List[0].VPOLIS == 1)
                        {
                            VIDPOLISA_B.Text = "Полис ОМС старого образца";
                        }
                        else if (List[0].VPOLIS == 2)
                        {
                            VIDPOLISA_B.Text = "Временное свидетельство, подтверждающее оформление полиса обязательного медицинского страхования";
                        }
                        else if (List[0].VPOLIS == 3)
                        {
                            VIDPOLISA_B.Text = "Полис ОМС единого образца";
                        }
                        SNILS_B.Text = List[0].SS;
                        POLIS_B.Text = List[0].SPOLIS.Trim() + " " + List[0].NPOLIS.Trim();
                        ENP_B.Text = List[0].ENP;
                        MR_B.Text = MR;
                        ENP_B.Text = List[0].ENP;
                        POLIK_B.Text = POLIKLIN_TFOMS.Text;
                        DATE_P_B.Text = DATE_PRIKREP_TFOMS.Text;
                        SPOSOB_B.Text = SPOSOB.Replace("<td>", "").Trim();
                        //SMO_B.Text = List[0].;
                        PRZ_B.Text = List[0].PRZCOD;
                        DATE_BEG_B.EditValue = List[0].DBEG;
                        D_END_B.EditValue = List[0].DEND;
                        D_NULL_B.EditValue = List[0].DSTOP;
                        PRICH_B.Text = List[0].STOP_REASON;
                    }
                    else
                    {
                        string m = "В нашей базе не найден данный ЗЛ!";
                        string t = "Ошибка!";
                        int b = 1;
                        Message me = new Message(m, t, b);
                        me.ShowDialog();

                        return;

                    }


                }
                //ПОИСК ПО Полным персональным данным
                else if (SerPolis.Text == "" && NomPolis.Text == "" && fam.Text != "" && im.Text != "" && dr.Text != "" && ENP.Text == "" && SNILSF.Text == "" && Type_doc.Text == "" && SerPolis_Passport.Text == "" && NomPolis_Passport.Text == "")
                {
                    var drr = dr.DateTime.ToLongDateString().Split(' ');
                    if (drr[0].Length == 1)
                    {
                        drr[0] = "0" + drr[0];
                    }

                    var Per2 = Month[drr[1]];
                    req.AllowAutoRedirect = true;
                    req.Cookies = Cookies;
                    req[HttpHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                    req[HttpHeader.AcceptLanguage] = "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7";
                    req.UserAgent = Http.ChromeUserAgent();
                    var ResponsePolis = req.Post("http://192.168.10.13/handler.php", $@"f1={Uri.EscapeUriString(fam.Text)}&im1={Uri.EscapeUriString(im.Text)}&ot1={Uri.EscapeUriString(ot.Text)}&dd={drr[0]}&mm={Uri.EscapeUriString(Per2)}&yy={drr[2]}&hidden=personal&b1=&sid=" + sid, "application/x-www-form-urlencoded").ToString();
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
                    POLIS_TFOMS.Text = NPOLIS.Trim();
                    ENP_TFOMS.Text = ENP;
                    MR_TFOMS.Text = MR;
                    POLIKLIN_TFOMS.Text = POL_NAME;
                    DATE_PRIKREP_TFOMS.Text = Data_prikrep;
                    SPOSOB_TFOMS.Text = SPOSOB.Replace("<td>", "").Trim();
                    SMO_TFOMS.Text = SMO_NAM;
                    PRZ_TFOMS.Text = PRZ_NAM;
                    DATE_START_TFOMS.Text = DBEG;


                    if (DEND != ".. ")
                    {
                        DATE_END_TFOMS.Text = DEND;
                    }
                    else
                    {
                        DATE_END_TFOMS.Text = "";
                    }

                    if (DSTOP != ".. ")
                    {
                        DATE_NULL_TFOMS.Text = DSTOP;
                    }
                    else
                    {
                        DATE_NULL_TFOMS.Text = "";
                    }

                    if (RSTOP != ".. ")
                    {
                        PRICIHIA_NULL_TFOMS.Text = RSTOP;
                    }
                    else
                    {
                        PRICIHIA_NULL_TFOMS.Text = "";
                    }

                    if (DS != ".. ")
                    {
                        DATE_DEAD_TFOMS.Text = DS;
                    }
                    else
                    {
                        DATE_DEAD_TFOMS.Text = "";
                    }

                    if (DS != "" && DS != ".. ")
                    {
                        DATE_DEAD_TFOMS.Background = Brushes.Red;
                    }
                    string load_pers_grid = $@"SELECT TOP(1) pp.SROKDOVERENOSTI,pp.ID,pp.ACTIVE,op.przcod,pe.UNLOAD,ENP ,FAM , IM  , OT ,W ,DR ,MO,oks.CAPTION as C_OKSM,r.NameWithID , pp.COMMENT,pe.DVIZIT, pp.DATEVIDACHI, pp.PRIZNAKVIDACHI,
            SS  ,VPOLIS,SPOLIS ,NPOLIS,DBEG ,DEND ,DSTOP ,BLANK ,DRECEIVED,f.NameWithId as MO_NameWithId,op.filename,pp.phone,p.AGENT, pp.CYCLE, st.namewithkod as STOP_REASON, pe.id as EVENT_ID
              FROM [dbo].[POL_PERSONS] pp left join 
            pol_events pe on pp.event_guid=pe.idguid
         	LEFT JOIN POL_PRZ_AGENTS p
			on p.ID = pe.AGENT
		    left join pol_polises ps
            on pp.EVENT_GUID=ps.EVENT_GUID
            left join pol_oplist op
            on pp.EVENT_GUID=op.EVENT_GUID
            left join r001 r
            on pe.tip_op=r.kod
            left join f003 f
            on pp.MO=f.mcod
            left join SPR_STOP st
            on ps.STOP_REASON=st.kod
			left join SPR_79_OKSM oks
			on pp.C_OKSM=oks.A3
			where pp.FAM = '{FAM}'
			and pp.IM = '{IM}'
			and pp.OT = '{OT}'
			and pp.DR = '{DR}'
			order by pe.ID DESC";
                    var List = MyReader.MySelect<Events>(load_pers_grid, Properties.Settings.Default.DocExchangeConnectionString);
                    if (List.Count != 0)
                    {
                        FAM_B.Text = List[0].FAM;
                        IM_B.Text = List[0].IM;
                        OT_B.Text = List[0].OT;
                        DR_B.EditValue = List[0].DR;
                        Vars.IdP = List[0].ID.ToString();
                        Vars.Btn = "2";
                        if (List[0].VPOLIS == 1)
                        {
                            VIDPOLISA_B.Text = "Полис ОМС старого образца";
                        }
                        else if (List[0].VPOLIS == 2)
                        {
                            VIDPOLISA_B.Text = "Временное свидетельство, подтверждающее оформление полиса обязательного медицинского страхования";
                        }
                        else if (List[0].VPOLIS == 3)
                        {
                            VIDPOLISA_B.Text = "Полис ОМС единого образца";
                        }
                        SNILS_B.Text = List[0].SS;
                        POLIS_B.Text = List[0].SPOLIS.Trim() + " " + List[0].NPOLIS.Trim();
                        ENP_B.Text = List[0].ENP;
                        MR_B.Text = MR;
                        ENP_B.Text = List[0].ENP;
                        POLIK_B.Text = POLIKLIN_TFOMS.Text;
                        DATE_P_B.Text = DATE_PRIKREP_TFOMS.Text;
                        SPOSOB_B.Text = SPOSOB.Replace("<td>", "").Trim();
                        //SMO_B.Text = List[0].;
                        PRZ_B.Text = List[0].PRZCOD;
                        DATE_BEG_B.EditValue = List[0].DBEG;
                        D_END_B.EditValue = List[0].DEND;
                        D_NULL_B.EditValue = List[0].DSTOP;
                        PRICH_B.Text = List[0].STOP_REASON;

                    }
                    else
                    {
                        string m = "В нашей базе не найден данный ЗЛ!";
                        string t = "Ошибка!";
                        int b = 1;
                        Message me = new Message(m, t, b);
                        me.ShowDialog();

                        return;

                    }


                }
                // Поиск по фамилии и номеру (серии) документа
                else if (SerPolis.Text == "" && NomPolis.Text == "" && fam.Text != "" && im.Text == "" && dr.Text == "" && ENP.Text == "" && SNILSF.Text == "" && Type_doc.Text != "" && SerPolis_Passport.Text != "" && NomPolis_Passport.Text != "")
                {

                    req.AllowAutoRedirect = true;
                    req.Cookies = Cookies;
                    req[HttpHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                    req[HttpHeader.AcceptLanguage] = "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7";
                    req.UserAgent = Http.ChromeUserAgent();
                    var ResponsePolis = req.Post("http://192.168.10.13/handler.php", $@"f1={Uri.EscapeUriString(fam.Text)}&tp1={Uri.EscapeUriString(Type_doc.Text)}&ser={SerPolis_Passport.Text}&num={NomPolis_Passport.Text}&hidden=doc&b1=&sid=" + sid, "application/x-www-form-urlencoded").ToString();
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
                    POLIS_TFOMS.Text = NPOLIS.Trim();
                    ENP_TFOMS.Text = ENP;
                    MR_TFOMS.Text = MR;
                    POLIKLIN_TFOMS.Text = POL_NAME;
                    DATE_PRIKREP_TFOMS.Text = Data_prikrep;
                    SPOSOB_TFOMS.Text = SPOSOB.Replace("<td>", "").Trim();
                    SMO_TFOMS.Text = SMO_NAM;
                    PRZ_TFOMS.Text = PRZ_NAM;
                    DATE_START_TFOMS.Text = DBEG;

                    if (DEND != ".. ")
                    {
                        DATE_END_TFOMS.Text = DEND;
                    }
                    else
                    {
                        DATE_END_TFOMS.Text = "";
                    }

                    if (DSTOP != ".. ")
                    {
                        DATE_NULL_TFOMS.Text = DSTOP;
                    }
                    else
                    {
                        DATE_NULL_TFOMS.Text = "";
                    }

                    if (RSTOP != ".. ")
                    {
                        PRICIHIA_NULL_TFOMS.Text = RSTOP;
                    }
                    else
                    {
                        PRICIHIA_NULL_TFOMS.Text = "";
                    }

                    if (DS != ".. ")
                    {
                        DATE_DEAD_TFOMS.Text = DS;
                    }
                    else
                    {
                        DATE_DEAD_TFOMS.Text = "";
                    }

                    if (DS != "" && DS != ".. ")
                    {
                        DATE_DEAD_TFOMS.Background = Brushes.Red;
                    }
                    string load_pers_grid = $@"SELECT TOP(1) pp.SROKDOVERENOSTI,pp.ID,pp.ACTIVE,op.przcod,pe.UNLOAD,ENP ,FAM , IM  , OT ,W ,DR ,MO,oks.CAPTION as C_OKSM,r.NameWithID , pp.COMMENT,pe.DVIZIT, pp.DATEVIDACHI, pp.PRIZNAKVIDACHI,
            SS  ,VPOLIS,SPOLIS ,NPOLIS,DBEG ,DEND ,DSTOP ,BLANK ,DRECEIVED,f.NameWithId as MO_NameWithId,op.filename,pp.phone,p.AGENT, pp.CYCLE, st.namewithkod as STOP_REASON, pe.id as EVENT_ID
              FROM [dbo].[POL_PERSONS] pp left join 
            pol_events pe on pp.event_guid=pe.idguid
         	LEFT JOIN POL_PRZ_AGENTS p
			on p.ID = pe.AGENT
		    left join pol_polises ps
            on pp.EVENT_GUID=ps.EVENT_GUID
            left join pol_oplist op
            on pp.EVENT_GUID=op.EVENT_GUID
            left join r001 r
            on pe.tip_op=r.kod
            left join f003 f
            on pp.MO=f.mcod
            left join SPR_STOP st
            on ps.STOP_REASON=st.kod
			left join SPR_79_OKSM oks
			on pp.C_OKSM=oks.A3
			where pp.FAM = '{FAM}'
			and pp.IM = '{IM}'
			and pp.OT = '{OT}'
			and pp.DR = '{DR}'
			order by pe.ID DESC";
                    var List = MyReader.MySelect<Events>(load_pers_grid, Properties.Settings.Default.DocExchangeConnectionString);
                    if (List.Count != 0)
                    {
                        FAM_B.Text = List[0].FAM;
                        IM_B.Text = List[0].IM;
                        OT_B.Text = List[0].OT;
                        DR_B.EditValue = List[0].DR;
                        Vars.IdP = List[0].ID.ToString();
                        Vars.Btn = "2";
                        if (List[0].VPOLIS == 1)
                        {
                            VIDPOLISA_B.Text = "Полис ОМС старого образца";
                        }
                        else if (List[0].VPOLIS == 2)
                        {
                            VIDPOLISA_B.Text = "Временное свидетельство, подтверждающее оформление полиса обязательного медицинского страхования";
                        }
                        else if (List[0].VPOLIS == 3)
                        {
                            VIDPOLISA_B.Text = "Полис ОМС единого образца";
                        }
                        SNILS_B.Text = List[0].SS;
                        POLIS_B.Text = List[0].SPOLIS.Trim() + " " + List[0].NPOLIS.Trim();
                        ENP_B.Text = List[0].ENP;
                        MR_B.Text = MR;
                        ENP_B.Text = List[0].ENP;
                        POLIK_B.Text = POLIKLIN_TFOMS.Text;
                        DATE_P_B.Text = DATE_PRIKREP_TFOMS.Text;
                        SPOSOB_B.Text = SPOSOB.Replace("<td>", "").Trim();
                        //SMO_B.Text = List[0].;
                        PRZ_B.Text = List[0].PRZCOD;
                        DATE_BEG_B.EditValue = List[0].DBEG;
                        D_END_B.EditValue = List[0].DEND;
                        D_NULL_B.EditValue = List[0].DSTOP;
                        PRICH_B.Text = List[0].STOP_REASON;

                    }
                    else
                    {
                        string m = "В нашей базе не найден данный ЗЛ!";
                        string t = "Ошибка!";
                        int b = 1;
                        Message me = new Message(m, t, b);
                        me.ShowDialog();

                        return;

                    }

                }
                // Поиск по SNILS
                else if (SerPolis.Text == "" && NomPolis.Text == "" && fam.Text == "" && im.Text == "" && dr.Text == "" && ENP.Text == "" && Type_doc.Text == "" && SerPolis_Passport.Text == "" && NomPolis_Passport.Text == "" && SNILSF.Text != "")
                {

                    var snilsik = SNILSF.Text.Split(' ');


                    req.AllowAutoRedirect = true;
                    req.Cookies = Cookies;
                    req[HttpHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                    req[HttpHeader.AcceptLanguage] = "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7";
                    req.UserAgent = Http.ChromeUserAgent();
                    var ResponsePolis = req.Post("http://192.168.10.13/handler.php", $@"ss21={snilsik[0]}&ss22={snilsik[1]}&ss23={snilsik[2]}&ss24={snilsik[3]}&hidden=ss&b1=&sid=" + sid, "application/x-www-form-urlencoded").ToString();
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
                    POLIS_TFOMS.Text = NPOLIS.Trim();
                    ENP_TFOMS.Text = ENP;
                    MR_TFOMS.Text = MR;
                    POLIKLIN_TFOMS.Text = POL_NAME;
                    DATE_PRIKREP_TFOMS.Text = Data_prikrep;
                    SPOSOB_TFOMS.Text = SPOSOB.Replace("<td>", "").Trim();
                    SMO_TFOMS.Text = SMO_NAM;
                    PRZ_TFOMS.Text = PRZ_NAM;
                    DATE_START_TFOMS.Text = DBEG;

                    if (DEND != ".. ")
                    {
                        DATE_END_TFOMS.Text = DEND;
                    }
                    else
                    {
                        DATE_END_TFOMS.Text = "";
                    }

                    if (DSTOP != ".. ")
                    {
                        DATE_NULL_TFOMS.Text = DSTOP;
                    }
                    else
                    {
                        DATE_NULL_TFOMS.Text = "";
                    }

                    if (RSTOP != ".. ")
                    {
                        PRICIHIA_NULL_TFOMS.Text = RSTOP;
                    }
                    else
                    {
                        PRICIHIA_NULL_TFOMS.Text = "";
                    }

                    if (DS != ".. ")
                    {
                        DATE_DEAD_TFOMS.Text = DS;
                    }
                    else
                    {
                        DATE_DEAD_TFOMS.Text = "";
                    }

                    if (DS != "" && DS != ".. ")
                    {
                        DATE_DEAD_TFOMS.Background = Brushes.Red;
                    }
                    string load_pers_grid = $@"SELECT TOP(1) pp.SROKDOVERENOSTI,pp.ID,pp.ACTIVE,op.przcod,pe.UNLOAD,ENP ,FAM , IM  , OT ,W ,DR ,MO,oks.CAPTION as C_OKSM,r.NameWithID , pp.COMMENT,pe.DVIZIT, pp.DATEVIDACHI, pp.PRIZNAKVIDACHI,
            SS  ,VPOLIS,SPOLIS ,NPOLIS,DBEG ,DEND ,DSTOP ,BLANK ,DRECEIVED,f.NameWithId as MO_NameWithId,op.filename,pp.phone,p.AGENT, pp.CYCLE, st.namewithkod as STOP_REASON, pe.id as EVENT_ID
              FROM [dbo].[POL_PERSONS] pp left join 
            pol_events pe on pp.event_guid=pe.idguid
         	LEFT JOIN POL_PRZ_AGENTS p
			on p.ID = pe.AGENT
		    left join pol_polises ps
            on pp.EVENT_GUID=ps.EVENT_GUID
            left join pol_oplist op
            on pp.EVENT_GUID=op.EVENT_GUID
            left join r001 r
            on pe.tip_op=r.kod
            left join f003 f
            on pp.MO=f.mcod
            left join SPR_STOP st
            on ps.STOP_REASON=st.kod
			left join SPR_79_OKSM oks
			on pp.C_OKSM=oks.A3
			where pp.FAM = '{FAM}'
			and pp.IM = '{IM}'
			and pp.OT = '{OT}'
			and pp.DR = '{DR}'
			order by pe.ID DESC";
                    var List = MyReader.MySelect<Events>(load_pers_grid, Properties.Settings.Default.DocExchangeConnectionString);
                    if (List.Count != 0)
                    {
                        FAM_B.Text = List[0].FAM;
                        IM_B.Text = List[0].IM;
                        OT_B.Text = List[0].OT;
                        DR_B.EditValue = List[0].DR;
                        Vars.IdP = List[0].ID.ToString();
                        Vars.Btn = "2";
                        if (List[0].VPOLIS == 1)
                        {
                            VIDPOLISA_B.Text = "Полис ОМС старого образца";
                        }
                        else if (List[0].VPOLIS == 2)
                        {
                            VIDPOLISA_B.Text = "Временное свидетельство, подтверждающее оформление полиса обязательного медицинского страхования";
                        }
                        else if (List[0].VPOLIS == 3)
                        {
                            VIDPOLISA_B.Text = "Полис ОМС единого образца";
                        }
                        SNILS_B.Text = List[0].SS;
                        POLIS_B.Text = List[0].SPOLIS.Trim() + " " + List[0].NPOLIS.Trim();
                        ENP_B.Text = List[0].ENP;
                        MR_B.Text = MR;
                        ENP_B.Text = List[0].ENP;
                        POLIK_B.Text = POLIKLIN_TFOMS.Text;
                        DATE_P_B.Text = DATE_PRIKREP_TFOMS.Text;
                        SPOSOB_B.Text = SPOSOB.Replace("<td>", "").Trim();
                        //SMO_B.Text = List[0].;
                        PRZ_B.Text = List[0].PRZCOD;
                        DATE_BEG_B.EditValue = List[0].DBEG;
                        D_END_B.EditValue = List[0].DEND;
                        D_NULL_B.EditValue = List[0].DSTOP;
                        PRICH_B.Text = List[0].STOP_REASON;

                    }
                    else
                    {
                        string m = "В нашей базе не найден данный ЗЛ!";
                        string t = "Ошибка!";
                        int b = 1;
                        Message me = new Message(m, t, b);
                        me.ShowDialog();

                        return;

                    }

                }
                // Поиск по Прикрепление и полис на дату лечения
                else if (SerPolis.Text == "" && NomPolis.Text == "" && fam.Text != "" && im.Text != "" && dr.Text != "" && ENP.Text != "" && Type_doc.Text == "" && SerPolis_Passport.Text == "" && NomPolis_Passport.Text == "" && SNILSF.Text == "" && Date_lech.Text != "")
                {
                    var Date_lechen = Date_lech.DateTime.ToLongDateString().Split(' ');
                    var drr = dr.DateTime.ToLongDateString().Split(' ');
                    if (drr[0].Length == 1)
                    {
                        drr[0] = "0" + drr[0];
                    }
                    if (Date_lechen[0].Length == 1)
                    {
                        Date_lechen[0] = "0" + Date_lechen[0];
                    }
                    ///




                    var Per2 = Month[drr[1]];
                    ///
                    req.AllowAutoRedirect = true;
                    req.Cookies = Cookies;
                    req[HttpHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                    req[HttpHeader.AcceptLanguage] = "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7";
                    req.UserAgent = Http.ChromeUserAgent();
                    var ResponsePolis = req.Post("http://192.168.10.13/handler.php", $@"dd2={Date_lechen[0]}&mm2={Uri.EscapeDataString(Per2)}&yy2={Date_lechen[2]}&f1={Uri.EscapeDataString(fam.Text)}&im1={Uri.EscapeDataString(im.Text)}&ot1={Uri.EscapeDataString(ot.Text)}&enp={this.ENP.Text}&dd={drr[0]}&mm={Uri.EscapeDataString(Per2)}&yy={drr[2]}&hidden=attache&b1=&sid=" + sid, "application/x-www-form-urlencoded").ToString();
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
                    POLIS_TFOMS.Text = NPOLIS.Trim();
                    ENP_TFOMS.Text = ENP;
                    MR_TFOMS.Text = MR;
                    POLIKLIN_TFOMS.Text = POL_NAME;
                    DATE_PRIKREP_TFOMS.Text = Data_prikrep;
                    SPOSOB_TFOMS.Text = SPOSOB.Replace("<td>", "").Trim();
                    SMO_TFOMS.Text = SMO_NAM;
                    PRZ_TFOMS.Text = PRZ_NAM;
                    DATE_START_TFOMS.Text = DBEG;

                    if (DEND != ".. ")
                    {
                        DATE_END_TFOMS.Text = DEND;
                    }
                    else
                    {
                        DATE_END_TFOMS.Text = "";
                    }

                    if (DSTOP != ".. ")
                    {
                        DATE_NULL_TFOMS.Text = DSTOP;
                    }
                    else
                    {
                        DATE_NULL_TFOMS.Text = "";
                    }

                    if (RSTOP != ".. ")
                    {
                        PRICIHIA_NULL_TFOMS.Text = RSTOP;
                    }
                    else
                    {
                        PRICIHIA_NULL_TFOMS.Text = "";
                    }

                    if (DS != ".. ")
                    {
                        DATE_DEAD_TFOMS.Text = DS;
                    }
                    else
                    {
                        DATE_DEAD_TFOMS.Text = "";
                    }

                    if (DS != "" && DS != ".. ")
                    {
                        DATE_DEAD_TFOMS.Background = Brushes.Red;
                    }
                    string load_pers_grid = $@"SELECT TOP(1) pp.SROKDOVERENOSTI,pp.ID,pp.ACTIVE,op.przcod,pe.UNLOAD,ENP ,FAM , IM  , OT ,W ,DR ,MO,oks.CAPTION as C_OKSM,r.NameWithID , pp.COMMENT,pe.DVIZIT, pp.DATEVIDACHI, pp.PRIZNAKVIDACHI,
            SS  ,VPOLIS,SPOLIS ,NPOLIS,DBEG ,DEND ,DSTOP ,BLANK ,DRECEIVED,f.NameWithId as MO_NameWithId,op.filename,pp.phone,p.AGENT, pp.CYCLE, st.namewithkod as STOP_REASON, pe.id as EVENT_ID
              FROM [dbo].[POL_PERSONS] pp left join 
            pol_events pe on pp.event_guid=pe.idguid
         	LEFT JOIN POL_PRZ_AGENTS p
			on p.ID = pe.AGENT
		    left join pol_polises ps
            on pp.EVENT_GUID=ps.EVENT_GUID
            left join pol_oplist op
            on pp.EVENT_GUID=op.EVENT_GUID
            left join r001 r
            on pe.tip_op=r.kod
            left join f003 f
            on pp.MO=f.mcod
            left join SPR_STOP st
            on ps.STOP_REASON=st.kod
			left join SPR_79_OKSM oks
			on pp.C_OKSM=oks.A3
			where pp.FAM = '{FAM}'
			and pp.IM = '{IM}'
			and pp.OT = '{OT}'
			and pp.DR = '{DR}'
			order by pe.ID DESC";
                    var List = MyReader.MySelect<Events>(load_pers_grid, Properties.Settings.Default.DocExchangeConnectionString);
                    if (List.Count != 0)
                    {
                        FAM_B.Text = List[0].FAM;
                        IM_B.Text = List[0].IM;
                        OT_B.Text = List[0].OT;
                        DR_B.EditValue = List[0].DR;
                        Vars.IdP = List[0].ID.ToString();
                        Vars.Btn = "2";
                        if (List[0].VPOLIS == 1)
                        {
                            VIDPOLISA_B.Text = "Полис ОМС старого образца";
                        }
                        else if (List[0].VPOLIS == 2)
                        {
                            VIDPOLISA_B.Text = "Временное свидетельство, подтверждающее оформление полиса обязательного медицинского страхования";
                        }
                        else if (List[0].VPOLIS == 3)
                        {
                            VIDPOLISA_B.Text = "Полис ОМС единого образца";
                        }
                        SNILS_B.Text = List[0].SS;
                        POLIS_B.Text = List[0].SPOLIS.Trim() + " " + List[0].NPOLIS.Trim();
                        ENP_B.Text = List[0].ENP;
                        MR_B.Text = MR;
                        ENP_B.Text = List[0].ENP;
                        POLIK_B.Text = POLIKLIN_TFOMS.Text;
                        DATE_P_B.Text = DATE_PRIKREP_TFOMS.Text;
                        SPOSOB_B.Text = SPOSOB.Replace("<td>", "").Trim();
                        //SMO_B.Text = List[0].;
                        PRZ_B.Text = List[0].PRZCOD;
                        DATE_BEG_B.EditValue = List[0].DBEG;
                        D_END_B.EditValue = List[0].DEND;
                        D_NULL_B.EditValue = List[0].DSTOP;
                        PRICH_B.Text = List[0].STOP_REASON;
                  
                    }
                    else
                    {
                        string m = "В нашей базе не найден данный ЗЛ!";
                        string t = "Ошибка!";
                        int b = 1;
                        Message me = new Message(m, t, b);
                        me.ShowDialog();

                        return;

                    }
                }


            }


            POISKVLADIK.FAM_TFOMS = FAM_TFOMS.Text.Trim().ToUpper();
            POISKVLADIK.IM_TFOMS = IM_TFOMS.Text.Trim().ToUpper();
            POISKVLADIK.OT_TFOMS = OT_TFOMS.Text.Trim().ToUpper();
            POISKVLADIK.DR_TFOMS = DR_TFOMS.Text;
            POISKVLADIK.VIDPOLIS_TFOMS = VIDPOLIS_TFOMS.Text;
            POISKVLADIK.SNILS_TFOMS = SNILS_TFOMS.Text;
            POISKVLADIK.POLIS_TFOMS = POLIS_TFOMS.Text;
            POISKVLADIK.ENP_TFOMS = ENP_TFOMS.Text;
            POISKVLADIK.MR_TFOMS = MR_TFOMS.Text;
            POISKVLADIK.POLIKLIN_TFOMS = POLIKLIN_TFOMS.Text;
            POISKVLADIK.DATE_PRIKREP_TFOMS = DATE_PRIKREP_TFOMS.Text;
            POISKVLADIK.SPOSOB_TFOMS = SPOSOB_TFOMS.Text;
            POISKVLADIK.SMO_TFOMS = SMO_TFOMS.Text;
            POISKVLADIK.PRZ_TFOMS = PRZ_TFOMS.Text;
            POISKVLADIK.DATE_START_TFOMS = DATE_START_TFOMS.Text;

        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            fam.Text = "";
            im.Text = "";
            ot.Text = "";
            dr.EditValue = null;
            SerPolis.Text = "";
            NomPolis.Text = "";
            ENP.Text = "";
            SNILSF.Text = "";
            SerPolis_Passport.Text = "";
            NomPolis_Passport.Text = "";
            Date_lech.EditValue = null;
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

            Month.Clear();
            Month.Add("Сентября", "Сентябрь");
            Month.Add("сентября", "Сентябрь");
            Month.Add("Октября", "Октябрь");
            Month.Add("октября", "Октябрь");
            Month.Add("Ноября", "Ноябрь");
            Month.Add("ноября", "Ноябрь");
            Month.Add("Декабря", "Декабрь");
            Month.Add("декабря", "Декабрь");
            Month.Add("Января", "Январь");
            Month.Add("января", "Январь");
            Month.Add("Февраля", "Февраль");
            Month.Add("февраля", "Февраль");
            Month.Add("Марта", "Март");
            Month.Add("марта", "Март");
            Month.Add("Апреля", "Апрель");
            Month.Add("апреля", "Апрель");
            Month.Add("Мая", "Май");
            Month.Add("мая", "Май");
            Month.Add("Июня", "Июнь");
            Month.Add("июня", "Июнь");
            Month.Add("Июля", "Июль");
            Month.Add("июля", "Июль");
            Month.Add("Августа", "Август");
            Month.Add("августа", "Август");
        }

        private void Poisk1_Copy_Click(object sender, RoutedEventArgs e)
        {
            POISKVLADIK.FAM_TFOMS = FAM_TFOMS.Text.Trim().ToUpper();
            POISKVLADIK.IM_TFOMS = IM_TFOMS.Text.Trim().ToUpper();
            POISKVLADIK.OT_TFOMS = OT_TFOMS.Text.Trim().ToUpper();
            POISKVLADIK.DR_TFOMS = DR_TFOMS.Text.Trim();
            POISKVLADIK.VIDPOLIS_TFOMS = VIDPOLIS_TFOMS.Text;
            POISKVLADIK.SNILS_TFOMS = SNILS_TFOMS.Text;
            POISKVLADIK.POLIS_TFOMS = POLIS_TFOMS.Text;
            POISKVLADIK.ENP_TFOMS = ENP_TFOMS.Text.Trim();
            POISKVLADIK.MR_TFOMS = MR_TFOMS.Text;
            POISKVLADIK.POLIKLIN_TFOMS = POLIKLIN_TFOMS.Text;
            POISKVLADIK.DATE_PRIKREP_TFOMS = DATE_PRIKREP_TFOMS.Text;
            POISKVLADIK.SPOSOB_TFOMS = SPOSOB_TFOMS.Text;
            POISKVLADIK.SMO_TFOMS = SMO_TFOMS.Text;
            POISKVLADIK.PRZ_TFOMS = PRZ_TFOMS.Text;
            POISKVLADIK.DATE_START_TFOMS = DATE_START_TFOMS.Text;
            POISKVLADIK.Load_ZL = true;
            this.Close();

        }
    }
    
}
