using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace Insurance_SPR
{
    public static class SPR
    {
        public static string PATH_VIGRUZKA { get; set; }
        public static string Premmissions { get; set; }
        public static string Login { get; set; }
        public static string PRZ_CODE { get; set; }
        public static string PRZ_ID { get; set; }
        public static bool Avto_vigruzka { get; set; }
        public static bool Avto_vigruzka_priznak { get; set; }
        public static string Row_filter { get; set; }
        public class MyReader
        {
            public static string load_pers_grid =$@"SELECT  pp.SROKDOVERENOSTI,pp.ID,pp.ACTIVE,op.przcod,pe.UNLOAD,ENP ,FAM , IM  , OT ,W ,DR ,MO,oks.CAPTION as C_OKSM,r.NameWithID , pp.COMMENT,pe.DVIZIT, pp.DATEVIDACHI, pp.PRIZNAKVIDACHI,
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
			on pp.C_OKSM=oks.A3  ";

            //            public static string load_pers_grid_user = $@"SELECT top(5000)  pp.SROKDOVERENOSTI,pp.ID,pp.ACTIVE,op.przcod,pe.UNLOAD,ENP ,FAM , IM  , OT ,W ,DR ,r.NameWithID, pp.COMMENT,pe.DVIZIT, pp.DATEVIDACHI, pp.PRIZNAKVIDACHI,
            //            SS  ,VPOLIS,SPOLIS ,NPOLIS,DBEG ,DEND ,DSTOP ,BLANK ,DRECEIVED,f.NameWithId,op.filename,pp.phone,p.AGENT, pp.CYCLE,(select pr.namewithid  from POL_PERSONS_INFORM pin
            //left join PRICHINA_INFORMIROVANIYA pr
            //on pin.PRICHINA_INFORM=pr.ID
            //where PERSON_ID=pp.ID and pin.id=(select max(id) from POL_PERSONS_INFORM where PERSON_ID=pp.ID)) as inform
            //              FROM [dbo].[POL_PERSONS] pp left join 
            //            pol_events pe on pp.event_guid=pe.idguid
            //         	LEFT JOIN POL_PRZ_AGENTS p
            //			on p.ID = pe.AGENT
            //		    left join pol_polises ps
            //            on pp.idguid=ps.person_guid
            //            left join pol_oplist op
            //            on pp.idguid=op.person_guid
            //            left join r001 r
            //            on pe.tip_op=r.kod
            //            left join f003 f
            //            on pp.MO=f.mcod ";



            //            public static string grid_filter = $@"SELECT top(5000)  pp.SROKDOVERENOSTI,pp.ID,pp.ACTIVE,op.przcod,pe.UNLOAD,ENP ,FAM , IM  , OT ,W ,DR ,r.NameWithID, pp.COMMENT,pe.DVIZIT, pp.DATEVIDACHI, pp.PRIZNAKVIDACHI,
            //            SS  ,VPOLIS,SPOLIS ,NPOLIS,DBEG ,DEND ,DSTOP ,BLANK ,DRECEIVED,f.NameWithId,op.filename,pp.phone,p.AGENT, pp.CYCLE,(select pr.namewithid  from POL_PERSONS_INFORM pin
            //left join PRICHINA_INFORMIROVANIYA pr
            //on pin.PRICHINA_INFORM=pr.ID
            //where PERSON_ID=pp.ID and pin.id=(select max(id) from POL_PERSONS_INFORM where PERSON_ID=pp.ID)) as inform
            //              FROM [dbo].[POL_PERSONS] pp left join 
            //            pol_events pe on pp.event_guid=pe.idguid
            //         	LEFT JOIN POL_PRZ_AGENTS p
            //			on p.ID = pe.AGENT
            //		    left join pol_polises ps
            //            on pp.idguid=ps.person_guid
            //            left join pol_oplist op
            //            on pp.idguid=op.person_guid
            //            left join r001 r
            //            on pe.tip_op=r.kod
            //            left join f003 f
            //            on pp.MO=f.mcod ";

            public static string load_pers_grid2 = $@" SELECT pp.ID,pp.IDGUID,pp.active,pe.TIP_OP,pp.SS ,pp.ENP ,pp.FAM , pp.IM  , pp.OT ,pp.W ,pp.DR , pp.PHONE ,
pp.COMMENT ,f.NameWithID ,op.filename,d.DOCTYPE,d.DOCSER,d.DOCNUM,VPOLIS,SPOLIS ,NPOLIS,DBEG ,DRECEIVED,DEND ,DSTOP
  FROM [dbo].[POL_PERSONS] pp
left join f003 f on pp.mo=f.mcod 
left join pol_events pe
on pp.event_guid=pe.idguid
left join pol_polises ps
on pp.EVENT_GUID=ps.EVENT_GUID
left join pol_oplist op
on pp.EVENT_GUID=op.EVENT_GUID
left join pol_documents d
on pp.IDGUID=d.PERSON_GUID and d.MAIN=1 and d.ACTIVE=1  
 ";


            public static string SELECTVAIN(string selectcmd, string connstring)
            {
                object UNLOAD = "";
                using (SqlConnection connection = new SqlConnection(connstring))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(selectcmd, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            UNLOAD = reader.GetValue(0);

                        }
                    }

                    reader.Close();
                }

                return UNLOAD.ToString();
            }
            public static DataTable Query(string query, string connstring)
            {
                SqlConnection connect = new SqlConnection(connstring);
                DataTable result = new DataTable();
                try
                {
                    connect.Open();
                    SqlCommand command = new SqlCommand (query, connect);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(result);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (connect.State != ConnectionState.Closed)
                        connect.Close();
                }
                return result;
            }


            public static DataTable LoadFromCSV(string ex_path, char column_razd =';', char rows_razd = ';')
            {
                string filename = ex_path;
                string[] attache = File.ReadAllLines(filename, Encoding.GetEncoding(1251));
                DataTable tb = new DataTable();
                //var cls0 = attache[0].Split('|');
                var cls0 = attache[0].Split(column_razd);
                cls0 = cls0.Where(x => x != "").ToArray();
                for (int i = 0; i < cls0.Count(); i++)
                {
                    tb.Columns.Add(/*"Column" + */cls0[i].ToString().Replace("\"", ""), typeof(string));
                }
                //tb.Columns.AddRange();
                //foreach (string row in attache)
                for (int i = 1; i < attache.Count(); i++)
                {
                    // получаем все ячейки строки
                    var row1 = attache[i].Substring(0, attache[i].Length - 1).Replace("\"", "");
                    var cls = row1.Split(rows_razd);
                    //var row1 = row.Substring(0, row.Length - 1);
                    //var cls = row1.Split(';');
                    //cls = cls.Where(x => x != "").ToArray();
                    tb.LoadDataRow(cls, LoadOption.Upsert);
                    //Attache_mo.Add(new ATTACHED_MO { GUID = cls[0], OKATO = cls[1], SMO = cls[2], DPFS = cls[3], SER = cls[4], NUM = cls[5], ENP = cls[6], MO = cls[7] });
                }
                return tb;
            }
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
        }

        //public class FIO
        //{

        //    public string FAM { get; set; }
        //    public string IM { get; set; }
        //    public string OT { get; set; }


        //}

        //public class NAME_VP
        //{

        //    public int ID { get; set; }
        //    public string NAME { get; set; }

        //}
        public static string Translit(string str)
        {
            string[] rus_up = { "Й", "Ц", "У", "К", "Е", "Н", "Г", "Ш", "Щ", "З", "Х", "Ъ", "Ф", "Ы", "В", "А", "П", "Р", "О", "Л", "Д", "Ж", "Э", "Я", "Ч", "С", "М", "И", "Т", "Ь", "Б", "Ю", ",", "." };
            string[] eng_up = { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "{", "}", "A", "S", "D", "F", "G", "H", "J", "K", "L", ":", "\"", "Z", "X", "C", "V", "B", "N", "M", "<", ">", "?", "/" };
            string[] eng_low = { "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "[", "]", "a", "s", "d", "f", "g", "h", "j", "k", "l", ";", "'", "z", "x", "c", "v", "b", "n", "m", ",", ".", ".", "/" };
            for (int i = 0; i <= 33; i++)
            {
                str = str.Replace(eng_up[i], rus_up[i]);
                str = str.Replace(eng_low[i], rus_up[i]);
            }
            return str;
        }
        public static string insert_polises = "insert into pol_polises (vpolis,spolis,npolis,dbeg,dend,dstop,blank,dreceived,person_guid,event_guid,dout) values (@vpolis,@spolis,@npolis,@dbeg,@dend,@dstop,@blank,@dreceived, " +
                "(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),@dout ) ";

        public static string insert_polises_3 = "insert into pol_polises (vpolis,spolis,npolis,dbeg,dend,dstop,blank,dreceived,person_guid,event_guid,dout) values (@vpolis,@spolis,@npolis,@dbeg,@dend,@dstop,@blank,@dreceived, " +
                "(select idguid from pol_persons where id=@id_p),(select event_guid from pol_persons where id=@id_p),@dout ) ";

        public static string update_polises = "update pol_polises set dbeg=@dbeg,dend=@dend,dstop=@dstop,blank=0,dreceived=@dreceived,person_guid= " +
                                "(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),event_guid=(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY())" +
                                "where spolis=@spolis and npolis=@npolis ";
        public static string update_polises_3 = "update pol_polises set dbeg=@dbeg,dend=@dend,dstop=@dstop,blank=0,dreceived=@dreceived,person_guid= " +
                                "(select idguid from pol_persons where id=@id_p),event_guid=(select event_guid from pol_persons where id=@id_p)" +
                                "where spolis=@spolis and npolis=@npolis ";

    }
}
