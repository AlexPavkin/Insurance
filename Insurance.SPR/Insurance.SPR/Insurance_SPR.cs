using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Insurance_SPR
{
    //public class DOCUMENTS
    //{
    //    public DateTime? DOCDATE { get; set; }
    //    public string DOCNUM { get; set; }
    //    public string DOCSER { get; set; }
    //    public int DOCTYPE { get; set; }
    //}
    
    public class Events
    {
        public Int32 ID { get; set; }

        [System.ComponentModel.DisplayName("Выгружен (да/нет)")]
        public bool UNLOAD { get; set; }
        [System.ComponentModel.DisplayName("Файл выгрузки")]
        public string FILENAME { get; set; }
        [System.ComponentModel.DisplayName("Комментарий")]
        public string COMMENT { get; set; }
        [System.ComponentModel.DisplayName("Причина подачи заявления")]
        public string NameWithID { get; set; }
        [System.ComponentModel.DisplayName("ЕНП")]
        public string ENP { get; set; }
        [System.ComponentModel.DisplayName("Активен (да/нет)")]
        public bool ACTIVE { get; set; }
        [System.ComponentModel.DisplayName("Фамилия")]
        public string FAM { get; set; }
        [System.ComponentModel.DisplayName("Имя")]
        public string IM { get; set; }
        [System.ComponentModel.DisplayName("Отчество")]
        public string OT { get; set; }
        [System.ComponentModel.DisplayName("Пол")]
        public Int32 W { get; set; }
        [System.ComponentModel.DisplayName("Дата рождения")]
        public DateTime? DR { get; set; }
        [System.ComponentModel.DisplayName("Гражданство")]
        public string C_OKSM { get; set; }
        [System.ComponentModel.DisplayName("Телефон")]
        public string PHONE { get; set; }
        [System.ComponentModel.DisplayName("СНИЛС")]
        public string SS { get; set; }
        [System.ComponentModel.DisplayName("Дата заявки")]
        public DateTime? DVIZIT { get; set; }
        [System.ComponentModel.DisplayName("Вид полиса")]
        public int VPOLIS { get; set; }
        [System.ComponentModel.DisplayName("Серия полиса")]
        public string SPOLIS { get; set; }
        [System.ComponentModel.DisplayName("Номер полиса")]
        public string NPOLIS { get; set; }
        [System.ComponentModel.DisplayName("Дата начала")]
        public DateTime? DBEG { get; set; }
        [System.ComponentModel.DisplayName("Дата получения")]
        public DateTime? DRECEIVED { get; set; }
        [System.ComponentModel.DisplayName("Дата окончания")]
        public DateTime? DEND { get; set; }
        [System.ComponentModel.DisplayName("Дата прекращения")]
        public DateTime? DSTOP { get; set; }
        [System.ComponentModel.DisplayName("Причина снятия с учета")]
        public string STOP_REASON { get; set; }
        [System.ComponentModel.DisplayName("Прикреплен к МО")]
        public string MO_NameWithId { get; set; }
        [System.ComponentModel.DisplayName("Пункт выдачи")]
        public string PRZCOD { get; set; } //Пункт выдачи
        [System.ComponentModel.DisplayName("Агент")]
        public string AGENT { get; set; } //Пункт выдачи
        [System.ComponentModel.DisplayName("Дата выдачи")]
        public DateTime? DATEVIDACHI { get; set; } //Пункт выдачи
        [System.ComponentModel.DisplayName("Признак выдачи")]
        public bool PRIZNAKVIDACHI { get; set; } //Пункт выдачи
        [System.ComponentModel.DisplayName("Срок доверености")]
        public DateTime? SROKDOVERENOSTI { get; set; }
        //[System.ComponentModel.DisplayName("Информирование")]
        //public string INFORM { get; set; } //Пункт выдачи
        [System.ComponentModel.DisplayName("Обработка закончена")]
        public bool CYCLE { get; set; } //Пункт выдачи
        public Int32 EVENT_ID { get; set; }


    }
    public class INFORMED
    {
        [System.ComponentModel.DisplayName("Фамилия")]
        public string FAM { get; set; }
        [System.ComponentModel.DisplayName("Имя")]
        public string IM { get; set; }
        [System.ComponentModel.DisplayName("Отчество")]
        public string OT { get; set; }
        [System.ComponentModel.DisplayName("Пол")]
        public Int32 W { get; set; }
        [System.ComponentModel.DisplayName("Дата рождения")]
        public DateTime? DR { get; set; }
        [System.ComponentModel.DisplayName("Телефон")]
        public string PHONE { get; set; }
        [System.ComponentModel.DisplayName("ЕНП")]
        public string ENP { get; set; }
        [System.ComponentModel.DisplayName("Дата информирования")]
        public DateTime? DATE_INFORM { get; set; }
        [System.ComponentModel.DisplayName("Причина информирования")]
        public string PRICHINA_INFORM { get; set; }
    }
    public class INFORM_ALL
    {
        public int ID { get; set; }
        public string SURNAME { get; set; }
        public string NAME { get; set; }
        public string SECNAME { get; set; }
        public DateTime? DR { get; set; }
        public int POL { get; set; }
        public string SNILS { get; set; }        
        public int DPFS_3 { get; set; }
        public string SN_POL_3 { get; set; }
        public string VIDPROF_3 { get; set; }        
        public string Tema_3 { get; set; }
        public DateTime DATE_UV_3 { get; set; }
        public string SPOSOB_3 { get; set; }
        public string RESULT_3 { get; set; }
        public string prim_3 { get; set; }
        //---------------------------------------------        
        public string KMKB { get; set; }        
        public int PM1 { get; set; }
        public int PM2 { get; set; }
        public int PM3 { get; set; }
        public int PM4 { get; set; }         
        public int DPFS_4 { get; set; }
        public string SN_POL_4 { get; set; }
        public bool sogl_4 { get; set; }
        public string tema_4 { get; set; }
        public DateTime Date_uv_4 { get; set; }
        public string sposob_4 { get; set; }
        public string result_4 { get; set; }
        public string prim_4 { get; set; }

    }
    public class P4_INFORM
    {               
        public string SURNAME { get; set; }        
        public string NAME { get; set; }        
        public string SECNAME { get; set; }         
        public DateTime? DR { get; set; }        
        public int POL { get; set; }        
        public string SNILS { get; set; }        
        public int SCOMP { get; set; }                        
        public string SN_POL { get; set; }
        public int KOD_POL { get; set; }
        public int KOD_POL1 { get; set; }
        public string KMKB { get; set; }
        public int DYEAR { get; set; }
        public int PM1 { get; set; }
        public int PM2 { get; set; }
        public int PM3 { get; set; }
        public int PM4 { get; set; }
        public int ID_TFOMS { get; set; }
        public string SMO { get; set; }
        public int DPFS { get; set; }
        public bool sogl { get; set; }
        public int tema { get; set; }
        public DateTime Date_uv { get; set; }
        public int sposob { get; set; }
        public int result { get; set; }
        public string prim { get; set; }

    }

    public class P3_INFORM
    {
        public int ID_TFOMS { get; set; }
        public string SURNAME { get; set; }
        public string NAME { get; set; }
        public string SECNAME { get; set; }
        public DateTime? DR { get; set; }
        public int POL { get; set; }
        public string SNILS { get; set; }
        public int SCOMP { get; set; }
        public int DPFS { get; set; }
        public string SN_POL { get; set; }
        public int VIDPROF { get; set; }
        public int DYEAR { get; set; }
        public int DMONTH { get; set; }
        public int Tema { get; set; }
        public DateTime DATE_UV { get; set; }
        public int SPOSOB { get; set; }
        public int RESULT { get; set; }
        public string prim { get; set; }
        public int KOD_POL { get; set; }
        public int KOD_POL1 { get; set; }
        public string SMO { get; set; }
        
    }
    public class NASELEN_KALININGRAD
    {
        public string surname { get; set; }
        public string name { get; set; }
        public string secname { get; set; }
        public DateTime? dr { get; set; }
        public int pol { get; set; }
        public string s_polis { get; set; }
        public string n_polis { get; set; }
        public string mr { get; set; }
        public DateTime? datp { get; set; }
        public DateTime? date_e { get; set; }
        public string tdok { get; set; }
        public string s_pasp { get; set; }
        public string n_pasp { get; set; }
        public string tip_op { get; set; }

        //public int scomp { get; set; }
        //public int dogovor { get; set; }
        //public int tdok { get; set; }
        //public int tdok_i { get; set; }
        //public string s_pasp { get; set; }
        //public string s_pasp_i { get; set; }
        //public int n_pasp { get; set; }
        //public int n_pasp_i { get; set; }
        //public DateTime? docdt { get; set; }
        //public DateTime? docdt_i { get; set; }
        //public DateTime? docdt_e { get; set; }
        //public DateTime? docdt_e_i { get; set; }
        //public string docorg { get; set; }
        //public string cn { get; set; }
        //public string surname { get; set; }
        //public string name { get; set; }
        //public string secname { get; set; }
        //public int pol { get; set; }
        //public int true_dr { get; set; }
        //public DateTime? dr { get; set; }
        //public string MR { get; set; }
        //public string okato { get; set; }
        //public string okato_f { get; set; }
        //public string index { get; set; }
        //public int rn { get; set; }
        //public string city { get; set; }
        //public string nasp { get; set; }
        //public string lstreet { get; set; }
        //public string street { get; set; }
        //public int house { get; set; }
        //public string lhouse { get; set; }
        //public string corpus { get; set; }
        //public int flat { get; set; }
        //public string lflat { get; set; }
        //public string comn { get; set; }
        //public DateTime? dat_reg { get; set; }
        //public string phone { get; set; }
        //public string phone1 { get; set; }
        //public string email { get; set; }
        //public string index_f { get; set; }
        //public int rn_f { get; set; }
        //public string city_f { get; set; }
        //public string nasp_f { get; set; }
        //public string lstreet_f { get; set; }
        //public string street_f { get; set; }
        //public int house_f { get; set; }
        //public string lhouse_f { get; set; }

    }

    public class RFILESMOLENSK
    {
        public string FAM { get; set; }
        public string IM { get; set; }
        public string OT { get; set; }
        public DateTime? DR { get; set; }
        public int W { get; set; }
        public int VPOLIS { get; set; }
        public string S_POL { get; set; }
        public string N_POL { get; set; }
        public string Q { get; set; }
        public DateTime? DP { get; set; }
        public DateTime? DENDP { get; set; }
        public string DOCTYPE { get; set; }
        public string SN_PASP { get; set; }
        public string SNILS { get; set; }
        public string OKATO { get; set; }
        public string RNNAME { get; set; }
        public string NPNAME { get; set; }
        public string UL { get; set; }
        public string ULCODE { get; set; }
        public string DOM { get; set; }
        public string KOR { get; set; }
        public string STR { get; set; }
        public string KV { get; set; }
        public string TEL { get; set; }
        public string MCOD { get; set; }
        public DateTime? D_PR { get; set; }
        public DateTime? D_OT { get; set; }
        public int S_PR { get; set; }
        public string SNILS_VR { get; set; }
        

    }
    
    public class F003
    {
        public string mcod { get; set; }
        public string NameWithID { get; set; }
    }
    public class ENP_BLANKS
    {
        public string SBLANK { get; set; }
        public string NUMBLANK { get; set; }
        public string ENP { get; set; }

    }

    public class INFORMIROVAN
    {
        public int ID { get; set; }
        public string PRICHINA_INFORM { get; set; }
    }

    public class NAMEVP
    {
        public string NAME_VP { get; set; }

    }
    public static class Vars
    {
        public static  List<string> IDZZ { get; set; }
        public static int []IDS { get; set; }
        public static  string IDSZ { get; set; }
        public static int Agnt { get; set; }
        public static string PunctRz { get; set; }
        public static string SMO { get; set; }
        public static DateTime DateVisit { get; set; }
        public static string CelVisit { get; set; }
        public static string Sposob { get; set; }
        public static int IdZl { get; set; }
        public static int IdRep { get; set; }
        public static string RepName { get; set; }
        public static int Forms { get; set; }
        public static int NewCelViz { get; set; }
        public static string F_page_ids { get; set; }
        public static string[] F_ids { get; set; }
        public static string Btn { get; set; }
        public static string petit { get; set; }
        public static string W { get; set; }
        public static bool ttab { get; set; }
        public static string IdP { get; set; }
        public static int PredID { get; set; }
        public static int grid_num { get; set; }
        public static int mes_res { get; set; }
        public static Guid RPGUID { get; set; }
        public static string MainTitle { get; set; }
    }
    public class People1
    {
        public string NPOL { get; set; }
        public string ENP { get; set; }

        public string FAM { get; set; }
        public string IM { get; set; }
        public string OT { get; set; }
        public Int32 W { get; set; }
        public DateTime DR { get; set; }
        public string RNNAME { get; set; }
        public string CITY { get; set; }
        public string NP { get; set; }
        public string UL { get; set; }
        public string DOM { get; set; }
        public string KOR { get; set; }
        public string KV { get; set; }
        public string Q { get; set; }

    }
    public class People
    {
        public Int32 ID { get; set; }
        public Guid IDGUID { get; set; }
        [System.ComponentModel.DisplayName("Комментарий")]
        public string COMMENT { get; set; }
        [System.ComponentModel.DisplayName("Файл выгрузки")]
        public string FILENAME { get; set; }
        [System.ComponentModel.DisplayName("Активна (да/нет)")]
        public bool ACTIVE { get; set; }
        [System.ComponentModel.DisplayName("Причина подачи заявления")]
        public string TIP_OP { get; set; }
        [System.ComponentModel.DisplayName("ЕНП")]
        public string ENP { get; set; }
        [System.ComponentModel.DisplayName("СНИЛС")]
        public string SS { get; set; }
        [System.ComponentModel.DisplayName("Фамилия")]
        public string FAM { get; set; }
        [System.ComponentModel.DisplayName("Имя")]
        public string IM { get; set; }
        [System.ComponentModel.DisplayName("Отчество")]
        public string OT { get; set; }
        [System.ComponentModel.DisplayName("Пол")]
        public Int32 W { get; set; }
        [System.ComponentModel.DisplayName("Дата рождения")]
        public DateTime? DR { get; set; }
        [System.ComponentModel.DisplayName("Вид документа")]
        public int DOCTYPE { get; set; }
        [System.ComponentModel.DisplayName("Серия документа")]
        public string DOCSER { get; set; }
        [System.ComponentModel.DisplayName("Номер документа")]
        public string DOCNUM { get; set; }
        [System.ComponentModel.DisplayName("Телефон")]
        public string PHONE { get; set; }
        [System.ComponentModel.DisplayName("Вид полиса")]
        public int VPOLIS { get; set; }
        [System.ComponentModel.DisplayName("Серия полиса")]
        public string SPOLIS { get; set; }
        [System.ComponentModel.DisplayName("Номер полиса")]
        public string NPOLIS { get; set; }
        [System.ComponentModel.DisplayName("Дата начала")]
        public DateTime? DBEG { get; set; }
        [System.ComponentModel.DisplayName("Дата получения")]
        public DateTime? DRECEIVED { get; set; }
        [System.ComponentModel.DisplayName("Дата окончания")]
        public DateTime? DEND { get; set; }
        [System.ComponentModel.DisplayName("Дата прекращения")]
        public DateTime? DSTOP { get; set; }
        [System.ComponentModel.DisplayName("Прикрепление")]
        public string NameWithID { get; set; }



    }
    public class R001
    {
        public string Kod { get; set; }
        public string NameWithID { get; set; }
    }
    public class R002
    {

    }
    public class R003
    {
        public int ID { get; set; }
        public string NameWithID { get; set; }
    }
    public class V013
    {
        public int IDKAT { get; set; }
        public string NameWithID { get; set; }
    }
    public class F008
    {
        public int ID { get; set; }
        public string NameWithID { get; set; }
    }
    public class F011
    {
        public int ID { get; set; }
        public string NameWithID { get; set; }
    }
    public class V005
    {
        public int IDPOL { get; set; }
        public string NameWithID { get; set; }
    }
    public class OKSM
    {
        public string ID { get; set; }
        public string CAPTION { get; set; }
    }
    public class FIO
    {

        public string FAM { get; set; }
        public string IM { get; set; }
        public string OT { get; set; }


    }

    public class MKB
    {
        public string IDDS { get; set; }
        public string DSNAME { get; set; }
        public string NameWithID { get; set; }
    }

    public class RESULT_INFORM_P3
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class RESULT_INFORM_P4
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class SPOSOB_INFORM
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class THEME_INFORM_P3
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string NameWithID { get; set; }
    }
    public class THEME_INFORM_P4
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class VID_MEROPR_INFORM_P3
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class NAME_VP
    {

        public int ID { get; set; }
        public string NAME { get; set; }

    }
    public class AgentsSmo
    {
        public int ID { get; set; }
        public string PRZ_CODE { get; set; }
        public string AGENT { get; set; }
        

    }
    public class PrzSmo
    {
        public int ID { get; set; }
        public string PRZ_CODE { get; set; }
        public string PRZ_NAME { get; set; }
        public string NameWithCode { get; set; }

    }
    
    public class PremissSmo
    {
        public string Premissions { get; set; }

    }
    public class Prz
    {
        public int ID { get; set; }
        public string SMO_CODE { get; set; }
        public string PRZ_CODE { get; set; }
        public string PRZ_NAME { get; set; }
        public string NameWithCode { get; set; }
    }
    public class Dost
    {
        public string ID { get; set; }
        public string NameWithID { get; set; }
    }
    public class STOP_LIST
    {
        public string VPOLIS { get; set; }
        public string SPOLIS { get; set; }
        public string NPOLIS { get; set; }
        public string DEND { get; set; }
        public string DSTOP { get; set; }
        public string STOP_R { get; set; }
        public string ENP { get; set; }
    }
    public class UNLOAD_HISTORY
    {
        public int ID { get; set; }
        [System.ComponentModel.DisplayName("Фамилия")]
        public string FAM { get; set; }
        [System.ComponentModel.DisplayName("Имя")]
        public string IM { get; set; }
        [System.ComponentModel.DisplayName("Отчество")]
        public string OT { get; set; }
        [System.ComponentModel.DisplayName("Дата рождения")]
        public DateTime DR { get; set; }
        [System.ComponentModel.DisplayName("Событие")]
        public string TIP_OP { get; set; }
        [System.ComponentModel.DisplayName("Файл выгрузки")]        
        public string FNAME { get; set; }
        [System.ComponentModel.DisplayName("Комментарий")]
        public string COMMENT { get; set; }
        [System.ComponentModel.DisplayName("Короб")]
        public string KOROB { get; set; }
        [System.ComponentModel.DisplayName("Дата файла")]
        public DateTime FDATE { get; set; }
    }
    public class UNLOAD_FILES_S
    {
        public string FXML { get; set; }
        public DateTime? FDATE { get; set; }
    }
        public class UNLOAD_FILES
    {
        public int ID { get; set; }        
        [System.ComponentModel.DisplayName("Файл выгрузки")]
        public string FILENAME { get; set; }        
        [System.ComponentModel.DisplayName("Дата файла")]
        public DateTime? FDATE { get; set; }
        [System.ComponentModel.DisplayName("Событие")]
        public string TIP_OP { get; set; }
        [System.ComponentModel.DisplayName("Застрахованное лицо")]
        public string FIO { get; set; }
        [System.ComponentModel.DisplayName("Дата рождения")]
        public DateTime? DR { get; set; }
        [System.ComponentModel.DisplayName("ЕНП")]
        public string ENP { get; set; }
        [System.ComponentModel.DisplayName("Тип полиса")]
        public int VPOLIS { get; set; }
        [System.ComponentModel.DisplayName("Серия полиса")]
        public string SPOLIS { get; set; }
        [System.ComponentModel.DisplayName("Номер полиса")]
        public string NPOLIS { get; set; }
        [System.ComponentModel.DisplayName("Дата начала")]
        public DateTime? DBEG { get; set; }
        [System.ComponentModel.DisplayName("Дата окончания")]
        public DateTime? DEND { get; set; }
        [System.ComponentModel.DisplayName("Дата прекращения")]
        public DateTime? DSTOP { get; set; }
        [System.ComponentModel.DisplayName("Предыдущие ФИО")]
        public string PREV_FIO { get; set; }
        [System.ComponentModel.DisplayName("Предыдущая ДР")]
        public DateTime? PREV_DR { get; set; }
        [System.ComponentModel.DisplayName("Вид документа")]
        public int DOCTYPE { get; set; }
        [System.ComponentModel.DisplayName("Серия документа")]
        public string DOCSER { get; set; }
        [System.ComponentModel.DisplayName("Номер документа")]
        public string DOCNUM { get; set; }
        [System.ComponentModel.DisplayName("Дата документа")]
        public DateTime? DOCDATE { get; set; }
        [System.ComponentModel.DisplayName("Вид д.документа")]
        public int? DDOCTYPE { get; set; }
        [System.ComponentModel.DisplayName("Серия д.документа")]
        public string DDOCSER { get; set; }
        [System.ComponentModel.DisplayName("Номер д.документа")]
        public string DDOCNUM { get; set; }
        [System.ComponentModel.DisplayName("Дата д.документа")]
        public DateTime? DDOCDATE { get; set; }
        [System.ComponentModel.DisplayName("Вид документа (ПД)")]
        public int? PREV_DOCTYPE { get; set; }
        [System.ComponentModel.DisplayName("Серия документа (ПД)")]
        public string PREV_DOCSER { get; set; }
        [System.ComponentModel.DisplayName("Номер документа (ПД)")]
        public string PREV_DOCNUM { get; set; }
        [System.ComponentModel.DisplayName("Дата документа (ПД)")]
        public DateTime? PREV_DOCDATE { get; set; }

    }
    public class People_history
    {
        public Int32 ID { get; set; }
        
        [System.ComponentModel.DisplayName("Комментарий")]
        public string COMMENT { get; set; }
        [System.ComponentModel.DisplayName("Файл выгрузки")]
        public string FILENAME { get; set; }
        [System.ComponentModel.DisplayName("Активна (да/нет)")]
        public bool ACTIVE { get; set; }
        [System.ComponentModel.DisplayName("Дата заявки")]
        public DateTime? DVIZIT { get; set; }
        [System.ComponentModel.DisplayName("Причина подачи заявления")]
        public string TIP_OP { get; set; }
        [System.ComponentModel.DisplayName("ЕНП")]
        public string ENP { get; set; }
        [System.ComponentModel.DisplayName("СНИЛС")]
        public string SS { get; set; }
        [System.ComponentModel.DisplayName("Фамилия")]
        public string FAM { get; set; }
        [System.ComponentModel.DisplayName("Имя")]
        public string IM { get; set; }
        [System.ComponentModel.DisplayName("Отчество")]
        public string OT { get; set; }
        [System.ComponentModel.DisplayName("Пол")]
        public Int32 W { get; set; }
        [System.ComponentModel.DisplayName("Дата рождения")]
        public DateTime? DR { get; set; }
        [System.ComponentModel.DisplayName("Фамилия(ПД)")]
        public string FAM_OLD { get; set; }
        [System.ComponentModel.DisplayName("Имя(ПД)")]
        public string IM_OLD { get; set; }
        [System.ComponentModel.DisplayName("Отчество(ПД)")]
        public string OT_OLD { get; set; }
        [System.ComponentModel.DisplayName("Пол(ПД)")]
        public Int32 W_OLD { get; set; }
        [System.ComponentModel.DisplayName("Дата рождения(ПД)")]
        public DateTime? DR_OLD { get; set; }
        [System.ComponentModel.DisplayName("Вид документа")]
        public int DOCTYPE { get; set; }
        [System.ComponentModel.DisplayName("Серия документа")]
        public string DOCSER { get; set; }
        [System.ComponentModel.DisplayName("Номер документа")]
        public string DOCNUM { get; set; }
        [System.ComponentModel.DisplayName("Телефон")]
        public string PHONE { get; set; }
        [System.ComponentModel.DisplayName("Вид полиса")]
        public int VPOLIS { get; set; }
        [System.ComponentModel.DisplayName("Серия полиса")]
        public string SPOLIS { get; set; }
        [System.ComponentModel.DisplayName("Номер полиса")]
        public string NPOLIS { get; set; }
        [System.ComponentModel.DisplayName("Дата начала")]
        public DateTime? DBEG { get; set; }
        [System.ComponentModel.DisplayName("Дата получения")]
        public DateTime? DRECEIVED { get; set; }
        [System.ComponentModel.DisplayName("Дата окончания")]
        public DateTime? DEND { get; set; }
        [System.ComponentModel.DisplayName("Дата прекращения")]
        public DateTime? DSTOP { get; set; }
        [System.ComponentModel.DisplayName("Прикрепление")]
        public string NameWithID { get; set; }
        [System.ComponentModel.DisplayName("Вид документа(ПД)")]
        public int DOCTYPE_OLD { get; set; }
        [System.ComponentModel.DisplayName("Серия документа(ПД)")]
        public string DOCSER_OLD { get; set; }
        [System.ComponentModel.DisplayName("Номер документа(ПД)")]
        public string DOCNUM_OLD { get; set; }
        [System.ComponentModel.DisplayName("Дата документа(ПД)")]
        public DateTime? DOCDATE_OLD { get; set; }
    }
    public class HOLIDAYS
    {
        public static string doc_ex_con;
        public int ID { get; set; }
        [System.ComponentModel.DisplayName("Праздничная дата")]
        public DateTime H_DATE { get; set; }
        [System.ComponentModel.DisplayName("Год")]
        public int YEAR { get; set; }
        [System.ComponentModel.DisplayName("Комментарий")]
        public string COMMENT { get; set; }
        
        public static List<DateTime> H_List
        {
            get
            {
                List<DateTime> list_h=new List<DateTime>();
                SqlConnection con = new SqlConnection(doc_ex_con);
                
                SqlCommand cmd = new SqlCommand($@"select H_DATE from SPR_HOLIDAYS where year between {DateTime.Today.Year} and {DateTime.Today.Year+1} and status=1", con);
                con.Open();
                cmd.CommandTimeout = 0;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    object h_date = dr["H_DATE"];
                    list_h.Add((DateTime)h_date);
                }
                
                dr.Close();
                con.Close();         
                
                return list_h;
            }

        }
        public static List<DateTime> W_List
        {
            get
            {
                List<DateTime> list_w = new List<DateTime>();
                SqlConnection con = new SqlConnection(doc_ex_con);

                SqlCommand cmd = new SqlCommand($@"select H_DATE from SPR_HOLIDAYS where year between {DateTime.Today.Year} and {DateTime.Today.Year + 1} and status in(2,3)", con);
                con.Open();
                cmd.CommandTimeout = 0;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    object h_date = dr["H_DATE"];
                    list_w.Add((DateTime)h_date);
                }

                dr.Close();
                con.Close();

                return list_w;
            }

        }
        public static DateTime AddWorkDays(DateTime h_start)
        {
            List<DateTime> h = HOLIDAYS.H_List;
            List<DateTime> w = HOLIDAYS.W_List;
            for (int i = 1; i <45; i++)
            {
                h_start = h_start.AddDays(1);
                if (h.Exists(x => x == h_start) == true || (h_start.DayOfWeek == DayOfWeek.Saturday || h_start.DayOfWeek == DayOfWeek.Sunday && w.Exists(x => x == h_start) == false))
                {                    
                    i = i - 1;
                }
            }            

            return h_start;
        }
    }
    public class XML_HOLIDAYS
    {
        public string id { get; set; }
        public string title { get; set; }
        public DateTime d { get; set; }
        public string t { get; set; }
        public string h { get; set; }
    }
    
    public class G_layuot
    {
        public static string lrt;
        public static string lrt1;
        public static string lrt2;
        public static void restore_Layout(string connectionString,GridControl grid1,GridControl grid2)
        {
            var connectionString1 = connectionString;
            SqlConnection con1 = new SqlConnection(connectionString1);
            SqlCommand comm1 = new SqlCommand($@"select isnull(LayRTable,'')  from auth where user_id={Convert.ToInt32(Insurance_SPR.SPR.PRZ_ID)}", con1);
            con1.Open();
            lrt = (string)comm1.ExecuteScalar();
            con1.Close();
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(lrt);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            if (stream.Length > 0)
                grid1.RestoreLayoutFromStream(stream);

            stream.Close();
            writer.Close();
            SqlConnection con2 = new SqlConnection(connectionString1);
            SqlCommand comm2 = new SqlCommand($@"select isnull(LayRTable1,'')  from auth where user_id={Convert.ToInt32(Insurance_SPR.SPR.PRZ_ID)}", con2);
            con2.Open();
            lrt1 = (string)comm2.ExecuteScalar();
            con2.Close();
            MemoryStream stream1 = new MemoryStream();
            StreamWriter writer1 = new StreamWriter(stream1);
            writer1.Write(lrt1);
            writer1.Flush();
            stream1.Seek(0, SeekOrigin.Begin);

            if (stream1.Length > 0)
                grid2.RestoreLayoutFromStream(stream1);
            stream1.Close();
            writer1.Close();
            grid1.FilterString = "";
            grid2.FilterString = "";
            //Layout_InUse = stream;
            //Layout_InUse1 = stream1;
            ////layout_InUse();
        }
        public static void restore_Layout(string connectionString, GridControl grid1, string num_lr)
        {
            var connectionString1 = connectionString;
            SqlConnection con1 = new SqlConnection(connectionString1);
            SqlCommand comm1 = new SqlCommand($@"select isnull(LayRTable"+num_lr+$@",'')  from auth where user_id={Convert.ToInt32(Insurance_SPR.SPR.PRZ_ID)}", con1);
            con1.Open();
            lrt2 = (string)comm1.ExecuteScalar();
            con1.Close();
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(lrt);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            if (stream.Length > 0)
                grid1.RestoreLayoutFromStream(stream);

            stream.Close();
            writer.Close();
            //SqlConnection con2 = new SqlConnection(connectionString1);
            //SqlCommand comm2 = new SqlCommand($@"select isnull(LayRTable1,'')  from auth where user_id={Convert.ToInt32(Insurance_SPR.SPR.PRZ_ID)}", con2);
            //con2.Open();
            //lrt1 = (string)comm2.ExecuteScalar();
            //con2.Close();
            //MemoryStream stream1 = new MemoryStream();
            //StreamWriter writer1 = new StreamWriter(stream1);
            //writer1.Write(lrt1);
            //writer1.Flush();
            //stream1.Seek(0, SeekOrigin.Begin);

            //if (stream1.Length > 0)
            //    grid2.RestoreLayoutFromStream(stream1);
            //stream1.Close();
            //writer1.Close();
            grid1.FilterString = "";
            //grid2.FilterString = "";
            //Layout_InUse = stream;
            //Layout_InUse1 = stream1;
            ////layout_InUse();
        }
        public static void save_Layout(string connectionString2, GridControl grid1, GridControl grid2)
        {
            Stream mStream = new MemoryStream();
            grid1.SaveLayoutToStream(mStream);
            mStream.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(mStream);
            string first = reader.ReadToEnd();



            var connectionString = connectionString2;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand($@"UPDATE auth set LayRTable=@lrt where user_id={Convert.ToInt32(Insurance_SPR.SPR.PRZ_ID)}", con);
            comm.Parameters.AddWithValue("@lrt", first);
            con.Open();
            comm.ExecuteNonQuery();
            con.Close();
            mStream.Close();
            reader.Close();

            Stream mStream1 = new MemoryStream();
            grid2.SaveLayoutToStream(mStream1);
            mStream1.Seek(0, SeekOrigin.Begin);
            StreamReader reader1 = new StreamReader(mStream1);
            string first1 = reader1.ReadToEnd();


            var connectionString1 = connectionString2;
            SqlConnection con1 = new SqlConnection(connectionString1);
            SqlCommand comm1 = new SqlCommand($@"UPDATE auth set LayRTable1=@lrt1 where user_id={Convert.ToInt32(Insurance_SPR.SPR.PRZ_ID)}", con1);
            comm1.Parameters.AddWithValue("@lrt1", first1);
            con1.Open();
            comm1.ExecuteNonQuery();
            con1.Close();
            mStream1.Close();
            reader1.Close();
            lrt = first;
            lrt1 = first1;
        }
        public static void layout_InUse(GridControl grid1, GridControl grid2)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(lrt);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            if (stream.Length > 0)
                grid1.RestoreLayoutFromStream(stream);
            stream.Close();
            writer.Close();
            MemoryStream stream1 = new MemoryStream();
            StreamWriter writer1 = new StreamWriter(stream1);
            writer1.Write(lrt1);
            writer1.Flush();
            stream1.Seek(0, SeekOrigin.Begin);
            if (stream1.Length > 0)
                grid2.RestoreLayoutFromStream(stream1);
            stream1.Close();
            writer1.Close();
        }
        public static void layout_InUse(GridControl grid1)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            
            writer.Write(lrt2);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            if (stream.Length > 0)
                grid1.RestoreLayoutFromStream(stream);
            stream.Close();
            writer.Close();
            //MemoryStream stream1 = new MemoryStream();
            //StreamWriter writer1 = new StreamWriter(stream1);
            //writer1.Write(lrt1);
            //writer1.Flush();
            //stream1.Seek(0, SeekOrigin.Begin);
            //if (stream1.Length > 0)
            //    grid2.RestoreLayoutFromStream(stream1);
            //stream1.Close();
            //writer1.Close();
        }
    }



}
