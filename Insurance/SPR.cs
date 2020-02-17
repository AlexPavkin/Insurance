using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Insurance
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

        public class MyReader
        {
            public static string load_pers_grid =$@"SELECT top(5000)  pp.SROKDOVERENOSTI,pp.ID,pp.ACTIVE,op.przcod,pe.UNLOAD,ENP ,FAM , IM  , OT ,W ,DR ,r.NameWithID, pp.COMMENT,pe.DVIZIT, pp.DATEVIDACHI, pp.PRIZNAKVIDACHI,
            SS  ,VPOLIS,SPOLIS ,NPOLIS,DBEG ,DEND ,DSTOP ,BLANK ,DRECEIVED,f.NameWithId,op.filename,pp.phone,p.AGENT, pp.CYCLE
              FROM [dbo].[POL_PERSONS] pp left join 
            pol_events pe on pp.event_guid=pe.idguid
         	LEFT JOIN POL_PRZ_AGENTS p
			on p.ID = pe.AGENT
		    left join pol_polises ps
            on pp.idguid=ps.person_guid
            left join pol_oplist op
            on pp.idguid=op.person_guid
            left join r001 r
            on pe.tip_op=r.kod
            left join f003 f
            on pp.MO=f.mcod
            order by pp.ID DESC";
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


    }
}
