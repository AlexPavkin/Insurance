using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Data.SqlClient;
using Insurance_SPR;



namespace Insurance
{
    public static class InsMethods
    {

        
        public static void Save_bt1_b0_s0_p2_sp1(MainWindow PD)
        {
            string module = "Save_bt1_b0_s0_p2_sp1";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand("insert into pol_persons (IDGUID,ENP,FAM,IM,OT,W,DR,mr,BIRTH_OKSM,C_OKSM,ss,phone,email,kateg,dost,rperson_guid)" +
                                " VALUES (newid(),@enp,@fam,@im,@ot,@w,@dr,@mr,@boksm,@coksm,@ss,@phone,@email,@kateg,@dost,@rpguid)" +
                                
                                
                                "update pol_persons set parentguid='00000000-0000-0000-0000-000000000000' where id=SCOPE_IDENTITY()" +
                                
                                
                                "insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,dorder,agent)" +
                                " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=SCOPE_IDENTITY())," +
                                "(select rperson_guid from pol_persons where id=SCOPE_IDENTITY()),@prelation,@rsmo,@rpolis,@fpolis,@dorder,@agent)" +
                                 
                                
                                 "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where idguid=(select person_guid from pol_events where id=SCOPE_IDENTITY())" +
                                 
                                
                                "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                                "values(newid(),(select person_guid from pol_events where id=SCOPE_IDENTITY()), @oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                                "(select idguid from pol_events where id=SCOPE_IDENTITY()))" +
                                
                                
                                " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                                "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1)," +
                                "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1 and livestatus=1),@FIAS_L1,@FIAS_L3,@FIAS_L4,@FIAS_L6,@FIAS_L90,@FIAS_L91,@FIAS_L7,@DOM,@KORP,@EXT,@KV, " +
                                "(select event_guid from pol_documents where id=SCOPE_IDENTITY()),@HOUSE_GUID)" +
                                
                                
                                "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                                " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                                ", @bomg,1,@dreg,0,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +
                                
                                
                                " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                                "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7_1 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7_1 and actstatus=1)," +
                                "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1_1 and livestatus=1),@FIAS_L1_1,@FIAS_L3_1,@FIAS_L4_1,@FIAS_L6_1,@FIAS_L90_1,@FIAS_L91_1,@FIAS_L7_1,@DOM_1,@KORP_1,@EXT_1,@KV_1, " +
                                "(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()),@HOUSE_GUID_1)" +
                                
                                
                                "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                                " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                                ", @bomg,0,@dreg1,1,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +


                                "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select person_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY() ),2,(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()))" +


                                "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select person_guid from pol_personsb where id=SCOPE_IDENTITY() ),3,(select event_guid from pol_personsb where id=SCOPE_IDENTITY() ))" +
                                
                                
                                "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_personsb where id=SCOPE_IDENTITY() )," +
                                " (select idguid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1)," +
                                "(select event_guid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1))" +


                                "update pol_polises set dbeg=@dbeg,dend=@dend,dstop=@dstop,blank=0,dreceived=@dreceived,person_guid= " +
                                "(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),event_guid=(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY())" +
                                "where spolis=@spolis and npolis=@npolis " +


                                "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                                "(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()))" +                                
                                

                                "select person_guid from pol_oplist where id=SCOPE_IDENTITY()" , con);





            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);

            if (PD.snils.Text == "")
            {
                comm.Parameters.AddWithValue("@ss", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            }

            if (PD.phone.Text == "")
            {
                comm.Parameters.AddWithValue("@phone", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            }
            if (PD.email.Text == "")
            {
                comm.Parameters.AddWithValue("@email", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@email", PD.email.Text);
            }

            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            comm.Parameters.AddWithValue("@dend", PD.date_end.DateTime);
            comm.Parameters.AddWithValue("@dorder", Vars.DateVisit);
            if (PD.fakt_prekr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dstop", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue ?? DBNull.Value);
            }


            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));

            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }
            if (PD.kat_zl.EditValue != null)
            {
                comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            }
            else
            {
                MessageBox.Show("Выберите категогрию ЗЛ!");
            }
            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", PD.fias.reg_dom.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV", PD.fias.reg_kv.Text);
            if (PD.fias.bomj.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@bomg", 1);
                comm.Parameters.AddWithValue("@addr_g", 0);
            }
            else
            {
                comm.Parameters.AddWithValue("@bomg", 0);
                comm.Parameters.AddWithValue("@addr_g", 1);
            }

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }
            comm.Parameters.AddWithValue("@dreg1", PD.fias1.reg_dr1.DateTime);

            //if (PD.fias1.sovp_addr.IsChecked == true)
            //{
            //    comm.Parameters.AddWithValue("@addr_p", 1);
            //}
            //else
            //{
            comm.Parameters.AddWithValue("@addr_p", 1);
            //}

            comm.Parameters.AddWithValue("@FIAS_L1_1", PD.fias1.reg1.EditValue);
            if (PD.fias1.reg_rn1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias1.reg_rn1.EditValue);
            }
            if (PD.fias1.reg_town1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", PD.fias1.reg_town1.EditValue);
            }

            if (PD.fias1.reg_np1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias1.reg_np1.EditValue);
            }
            if (PD.fias1.reg_ul1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", PD.fias1.reg_ul1.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90_1", PD.fias1.reg_ul1.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91_1", PD.fias1.reg_ul1.EditValue);
            }
            if (PD.fias1.reg_dom1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias1.reg_dom1.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM_1", PD.domsplit1);
            comm.Parameters.AddWithValue("@KORP_1", PD.fias1.reg_korp1.Text);
            comm.Parameters.AddWithValue("@EXT_1", PD.fias1.reg_str1.Text);
            comm.Parameters.AddWithValue("@KV_1", PD.fias1.reg_kv1.Text);
            
            comm.Parameters.AddWithValue("@screen", PD.zl_photo.EditValue ?? "");
            
            comm.Parameters.AddWithValue("@screen1", PD.zl_podp.EditValue ?? "");

            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {                
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //---------------------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1 );
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' ", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where idguid='{perguid}')", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),'{perguid}','{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where idguid='{perguid}'))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),'{perguid}',(select event_guid from pol_persons where idguid='{perguid}'),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);

        }

        public static void Save_bt1_b0_s0_p2_sp0(MainWindow PD)
        {
            string module = "Save_bt1_b0_s0_p2_sp0";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand(
                                 "insert into pol_persons (IDGUID,ENP,FAM,IM,OT,W,DR,mr,BIRTH_OKSM,C_OKSM,ss,phone,email,kateg,dost,rperson_guid)" +
                                 " VALUES (newid(),@enp,@fam,@im,@ot,@w,@dr,@mr,@boksm,@coksm,@ss,@phone,@email,@kateg,@dost,@rpguid)" +
                                           
                                 
                                 "update pol_persons set parentguid='00000000-0000-0000-0000-000000000000' where id=SCOPE_IDENTITY()" +
                                     
                                 
                                 "insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,agent)" +
                                 " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=SCOPE_IDENTITY())," +
                                 "(select rperson_guid from pol_persons where id=SCOPE_IDENTITY()),@prelation,@rsmo,@rpolis,@fpolis,@agent)" +
                                       
                                 
                                 "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where idguid=(select person_guid from pol_events where id=SCOPE_IDENTITY())" +
                                       
                                 
                                 "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                                 "values(newid(),(select person_guid from pol_events where id=SCOPE_IDENTITY()), @oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                                 "(select idguid from pol_events where id=SCOPE_IDENTITY()))" +
                                  

                                 " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                                "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1)," +
                                "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1 and livestatus=1),@FIAS_L1,@FIAS_L3,@FIAS_L4,@FIAS_L6,@FIAS_L90,@FIAS_L91,@FIAS_L7,@DOM,@KORP,@EXT,@KV, " +
                                "(select event_guid from pol_documents where id=SCOPE_IDENTITY()),@HOUSE_GUID)" +
                                
                                
                                 "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                                 " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                                 ", @bomg,1,@dreg,0,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +
                                 
                                 
                                 " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                                "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7_1 and actstatus=1 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7_1 and actstatus=1 and actstatus=1)," +
                                "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1_1 and livestatus=1),@FIAS_L1_1,@FIAS_L3_1,@FIAS_L4_1,@FIAS_L6_1,@FIAS_L90_1,@FIAS_L91_1,@FIAS_L7_1,@DOM_1,@KORP_1,@EXT_1,@KV_1, " +
                                "(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()),@HOUSE_GUID_1)" +

                                
                                 "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                                 " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) )," +
                                 "(select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                                 ", @bomg,0,@dreg1,1,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +


                                 "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select person_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY() ),2,(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()))" +


                                "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select person_guid from pol_personsb where id=SCOPE_IDENTITY() ),3,(select event_guid from pol_personsb where id=SCOPE_IDENTITY() ))" +


                                 "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_personsb where id=SCOPE_IDENTITY() )," +
                                 " (select idguid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1)," +
                                 "(select event_guid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1))" +
                                 
                                 
                                 "insert into pol_polises (vpolis,spolis,npolis,dbeg,dend,blank,dreceived,person_guid,event_guid) values (@vpolis,@spolis,@npolis,@dbeg,@dend,@blank,@dreceived, " +
                                 "(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()) ) " +
                                 
                                 
                                 "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                                "(select event_guid from pol_polises where id=SCOPE_IDENTITY()),(select person_guid from pol_polises where id=SCOPE_IDENTITY()))" +                                
                                
                                
                                "select person_guid from pol_oplist where id=SCOPE_IDENTITY()" , con);







            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);
            if (PD.snils.Text == "")
            {
                comm.Parameters.AddWithValue("@ss", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            }

            if (PD.phone.Text == "")
            {
                comm.Parameters.AddWithValue("@phone", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            }
            if (PD.email.Text == "")
            {
                comm.Parameters.AddWithValue("@email", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@email", PD.email.Text);
            }
            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            comm.Parameters.AddWithValue("@dend", PD.date_end.DateTime);
            if (PD.fakt_prekr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dstop", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue ?? DBNull.Value);
            }
            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));

            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }
            if (PD.kat_zl.EditValue != null)
            {
                comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            }
            else
            {
                MessageBox.Show("Выберите категогрию ЗЛ!");
            }
            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", PD.fias.reg_dom.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV", PD.fias.reg_kv.Text);
            if (PD.fias.bomj.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@bomg", 1);
                comm.Parameters.AddWithValue("@addr_g", 0);
            }
            else
            {
                comm.Parameters.AddWithValue("@bomg", 0);
                comm.Parameters.AddWithValue("@addr_g", 1);
            }

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }
            comm.Parameters.AddWithValue("@dreg1", PD.fias1.reg_dr1.DateTime);


            comm.Parameters.AddWithValue("@addr_p", 1);
            comm.Parameters.AddWithValue("@FIAS_L1_1", PD.fias1.reg1.EditValue);
            if (PD.fias1.reg_rn1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias1.reg_rn1.EditValue);
            }
            if (PD.fias1.reg_town1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", PD.fias1.reg_town1.EditValue);
            }

            if (PD.fias1.reg_np1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias1.reg_np1.EditValue);
            }
            if (PD.fias1.reg_ul1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", PD.fias1.reg_ul1.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90_1", PD.fias1.reg_ul1.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91_1", PD.fias1.reg_ul1.EditValue);
            }
            if (PD.fias1.reg_dom1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias1.reg_dom1.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM_1", PD.domsplit1);
            comm.Parameters.AddWithValue("@KORP_1", PD.fias1.reg_korp1.Text);
            comm.Parameters.AddWithValue("@EXT_1", PD.fias1.reg_str1.Text);
            comm.Parameters.AddWithValue("@KV_1", PD.fias1.reg_kv1.Text);
            //}
            if (PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "")
            {
                comm.Parameters.AddWithValue("@screen", "");
            }
            else
            {
                comm.Parameters.AddWithValue("@screen", Convert.ToBase64String((byte[])PD.zl_photo.EditValue)); // записываем само изображение

            }
            if (PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "")
            {
                comm.Parameters.AddWithValue("@screen1", "");
            }
            else
            {
                comm.Parameters.AddWithValue("@screen1", Convert.ToBase64String((byte[])PD.zl_podp.EditValue));

            }
            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {                
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }
             
            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' ", con);
                        
                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where idguid='{perguid}')", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();
                    
                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }


            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),'{perguid}','{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where idguid='{perguid}'))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),'{perguid}',(select event_guid from pol_persons where idguid='{perguid}'),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void Save_bt1_b0_s0_p13(MainWindow PD)
        {
            string module = "Save_bt1_b0_s0_p13";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand("insert into pol_persons (IDGUID,ENP,FAM,IM,OT,W,DR,mr,BIRTH_OKSM,C_OKSM,ss,phone,email,kateg,dost,rperson_guid)" +
                                 " VALUES (newid(),@enp,@fam,@im,@ot,@w,@dr,@mr,@boksm,@coksm,@ss,@phone,@email,@kateg,@dost,@rpguid)" +

                                 "update pol_persons set parentguid='00000000-0000-0000-0000-000000000000' where id=SCOPE_IDENTITY()" +



                                 "insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,agent)" +
                                 " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=SCOPE_IDENTITY())," +
                                 "(select rperson_guid from pol_persons where id=SCOPE_IDENTITY()),@prelation,@rsmo,@rpolis,@fpolis,@agent)" +
                                  "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where idguid=(select person_guid from pol_events where id=SCOPE_IDENTITY())" +

                                 "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                                 "values(newid(),(select person_guid from pol_events where id=SCOPE_IDENTITY()), @oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                                 "(select idguid from pol_events where id=SCOPE_IDENTITY()))" +

                                 " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                                "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1)," +
                                "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1 and livestatus=1),@FIAS_L1,@FIAS_L3,@FIAS_L4,@FIAS_L6,@FIAS_L90,@FIAS_L91,@FIAS_L7,@DOM,@KORP,@EXT,@KV, " +
                                "(select event_guid from pol_documents where id=SCOPE_IDENTITY()),@HOUSE_GUID)" +

                                 "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                                 " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                                 ", @bomg,1,@dreg,0,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                                 " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                                "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7_1 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7_1 and actstatus=1)," +
                                "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1_1 and livestatus=1),@FIAS_L1_1,@FIAS_L3_1,@FIAS_L4_1,@FIAS_L6_1,@FIAS_L90_1,@FIAS_L91_1,@FIAS_L7_1,@DOM_1,@KORP_1,@EXT_1,@KV_1, " +
                                "(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()),@HOUSE_GUID_1)" +

                                 "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                                 " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                                 ", @bomg,0,@dreg1,1,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                                 "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select person_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY() ),2,(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()))" +


                                "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select person_guid from pol_personsb where id=SCOPE_IDENTITY() ),3,(select event_guid from pol_personsb where id=SCOPE_IDENTITY() ))" +

                                 "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_personsb where id=SCOPE_IDENTITY() )," +
                                 " (select idguid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1)," +
                                 "(select event_guid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1))" +

                                 "insert into pol_polises (vpolis,spolis,npolis,dbeg,dend,blank,dreceived,person_guid,event_guid) values (@vpolis,@spolis,@npolis,@dbeg,@dend,@blank,@dreceived, " +
                                 "(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()) ) " +

                                 "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                                "(select event_guid from pol_polises where id=SCOPE_IDENTITY()),(select person_guid from pol_polises where id=SCOPE_IDENTITY()))" +
                                "select person_guid from pol_oplist where id=SCOPE_IDENTITY()", con);




            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm.Parameters.AddWithValue("@email", PD.email.Text);
            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            comm.Parameters.AddWithValue("@dend", PD.date_end.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);
     

            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }
            if (PD.kat_zl.EditValue != null)
            {
                comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            }
            else
            {
                MessageBox.Show("Выберите категогрию ЗЛ!");
            }
            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);

            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", PD.fias.reg_dom.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV", PD.fias.reg_kv.Text);
            if (PD.fias.bomj.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@bomg", 1);
                comm.Parameters.AddWithValue("@addr_g", 0);
            }
            else
            {
                comm.Parameters.AddWithValue("@bomg", 0);
                comm.Parameters.AddWithValue("@addr_g", 1);
            }

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }
            comm.Parameters.AddWithValue("@dreg1", PD.fias1.reg_dr1.DateTime);


            comm.Parameters.AddWithValue("@addr_p", 1);
            comm.Parameters.AddWithValue("@FIAS_L1_1", PD.fias1.reg1.EditValue);
            if (PD.fias1.reg_rn1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias1.reg_rn1.EditValue);
            }
            if (PD.fias1.reg_town1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", PD.fias1.reg_town1.EditValue);
            }

            if (PD.fias1.reg_np1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias1.reg_np1.EditValue);
            }
            if (PD.fias1.reg_ul1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", PD.fias1.reg_ul1.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90_1", PD.fias1.reg_ul1.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91_1", PD.fias1.reg_ul1.EditValue);
            }
            if (PD.fias1.reg_dom1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias1.reg_dom1.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM_1", PD.domsplit1);
            comm.Parameters.AddWithValue("@KORP_1", PD.fias1.reg_korp1.Text);
            comm.Parameters.AddWithValue("@EXT_1", PD.fias1.reg_str1.Text);
            comm.Parameters.AddWithValue("@KV_1", PD.fias1.reg_kv1.Text);
            //}
            if (PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "")
            {
                comm.Parameters.AddWithValue("@screen", "");
            }
            else
            {
                comm.Parameters.AddWithValue("@screen", Convert.ToBase64String((byte[])PD.zl_photo.EditValue)); // записываем само изображение

            }
            if (PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "")
            {
                comm.Parameters.AddWithValue("@screen1", "");
            }
            else
            {
                comm.Parameters.AddWithValue("@screen1", Convert.ToBase64String((byte[])PD.zl_podp.EditValue));

            }

            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {                
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' ", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where idguid='{perguid}')", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),'{perguid}','{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where idguid='{perguid}'))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),'{perguid}',(select event_guid from pol_persons where idguid='{perguid}'),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void Save_bt1_b1_s0_p2_sp0(MainWindow PD)
        {
            string module = "Save_bt1_b1_s0_p2_sp0";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand("insert into pol_persons (IDGUID,ENP,FAM,IM,OT,W,DR,mr,BIRTH_OKSM,C_OKSM,ss,phone,email,kateg,dost,rperson_guid)" +
                            " VALUES (newid(),@enp,@fam,@im,@ot,@w,@dr,@mr,@boksm,@coksm,@ss,@phone,@email,@kateg,@dost,@rpguid)" +

                            "update pol_persons set parentguid='00000000-0000-0000-0000-000000000000' where id=SCOPE_IDENTITY()" +



                            "insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,agent)" +
                            " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=SCOPE_IDENTITY())," +
                            "(select rperson_guid from pol_persons where id=SCOPE_IDENTITY()),@prelation,@rsmo,@rpolis,@fpolis,@agent)" +
                             "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where idguid=(select person_guid from pol_events where id=SCOPE_IDENTITY())" +

                            "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                            "values(newid(),(select person_guid from pol_events where id=SCOPE_IDENTITY()), @oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                            "(select idguid from pol_events where id=SCOPE_IDENTITY()))" +

                            " insert into pol_addresses (IDGUID,EVENT_GUID,fias_l1,FIAS_L3) " +
                            "values(newid(),(select event_guid from pol_documents where id=SCOPE_IDENTITY()),@FIAS_L1,@FIAS_L3)" +

                            "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,addres_p,dt1,dt2,event_guid)" +
                            " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                            ", 1,0,0,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                            "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select person_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY() ),2,(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()))" +


                            "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select person_guid from pol_personsb where id=SCOPE_IDENTITY() ),3,(select event_guid from pol_personsb where id=SCOPE_IDENTITY() ))" +

                            "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_personsb where id=SCOPE_IDENTITY() )," +
                            " (select idguid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1)," +
                            "(select event_guid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1))" +

                            "insert into pol_polises (vpolis,spolis,npolis,dbeg,dend,blank,dreceived,person_guid,event_guid) values (@vpolis,@spolis,@npolis,@dbeg,@dend,@blank,@dreceived, " +
                            "(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()) ) " +

                            "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                                "(select event_guid from pol_polises where id=SCOPE_IDENTITY()),(select person_guid from pol_polises where id=SCOPE_IDENTITY()))" +
                                "select person_guid from pol_oplist where id=SCOPE_IDENTITY()", con);






            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm.Parameters.AddWithValue("@email", PD.email.Text);
            comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            comm.Parameters.AddWithValue("@dend", PD.date_end.DateTime);
            comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);
            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }
            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }

            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }

            //if (PD.fias.bomj.IsChecked == true)
            //{
            //    comm.Parameters.AddWithValue("@bomg", 1);
            //    comm.Parameters.AddWithValue("@addr_g", 0);
            //}
            //else
            //{
            //    comm.Parameters.AddWithValue("@bomg", 0);
            //    comm.Parameters.AddWithValue("@addr_g", 1);
            //}
            comm.Parameters.AddWithValue("@bomg", 1);
            comm.Parameters.AddWithValue("@addr_g", 1);
            comm.Parameters.AddWithValue("@addr_p", 1);

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }

            //if (PD.fias1.sovp_addr.IsChecked == true)
            //{
            //    comm.Parameters.AddWithValue("@addr_p", 1);
            //}
            //else
            //{
            //comm.Parameters.AddWithValue("@addr_p", 0);
            //}


            if (PD.fias1.reg_rn1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias1.reg_rn1.EditValue);
            }

            if (PD.fias1.reg_np1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias1.reg_np1.EditValue);
            }

            if (PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "")
            {
                comm.Parameters.AddWithValue("@screen", "");
            }
            else
            {
                comm.Parameters.AddWithValue("@screen", Convert.ToBase64String((byte[])PD.zl_photo.EditValue)); // записываем само изображение

            }
            if (PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "")
            {
                comm.Parameters.AddWithValue("@screen1", "");
            }
            else
            {
                comm.Parameters.AddWithValue("@screen1", Convert.ToBase64String((byte[])PD.zl_podp.EditValue));

            }

            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {                
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' ", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where idguid='{perguid}')", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),'{perguid}','{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where idguid='{perguid}'))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),'{perguid}',(select event_guid from pol_persons where idguid='{perguid}'),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void Save_bt1_b1_s0_p2_sp1(MainWindow PD)
        {
            string module = "Save_bt1_b1_s0_p2_sp1";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand comm = new SqlCommand("insert into pol_persons (IDGUID,ENP,FAM,IM,OT,W,DR,mr,BIRTH_OKSM,C_OKSM,ss,phone,email,kateg,dost,rperson_guid)" +
                            " VALUES (newid(),@enp,@fam,@im,@ot,@w,@dr,@mr,@boksm,@coksm,@ss,@phone,@email,@kateg,@dost,@rpguid)" +

                            "update pol_persons set parentguid='00000000-0000-0000-0000-000000000000' where id=SCOPE_IDENTITY()" +



                            "insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,agent)" +
                            " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=SCOPE_IDENTITY())," +
                            "(select rperson_guid from pol_persons where id=SCOPE_IDENTITY()),@prelation,@rsmo,@rpolis,@fpolis,@agent)" +
                             "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where idguid=(select person_guid from pol_events where id=SCOPE_IDENTITY())" +

                            "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                            "values(newid(),(select person_guid from pol_events where id=SCOPE_IDENTITY()), @oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                            "(select idguid from pol_events where id=SCOPE_IDENTITY()))" +

                            " insert into pol_addresses (IDGUID,EVENT_GUID,fias_l1,FIAS_L3) " +
                            "values(newid(),(select event_guid from pol_documents where id=SCOPE_IDENTITY()),@FIAS_L1,@FIAS_L3)" +

                            "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,addres_p,dt1,dt2,event_guid)" +
                            " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                            ", 1,0,0,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                            "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select person_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY() ),2,(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()))" +


                            "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select person_guid from pol_personsb where id=SCOPE_IDENTITY() ),3,(select event_guid from pol_personsb where id=SCOPE_IDENTITY() ))" +

                            "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_personsb where id=SCOPE_IDENTITY() )," +
                            " (select idguid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1)," +
                            "(select event_guid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1))" +

                           "update pol_polises set dbeg=@dbeg,dend=@dend,dstop=@dstop,blank=0,dreceived=@dreceived,person_guid= " +
                                "(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),event_guid=(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY())" +
                                "where spolis=@spolis and npolis=@npolis " +

                                "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                                "(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()))" +
                                "select person_guid from pol_oplist where id=SCOPE_IDENTITY()", con);






            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm.Parameters.AddWithValue("@email", PD.email.Text);
            comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            comm.Parameters.AddWithValue("@dend", PD.date_end.DateTime);
            comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);
            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }
            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }

            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }

            //if (PD.fias.bomj.IsChecked == true)
            //{
            //    comm.Parameters.AddWithValue("@bomg", 1);
            //    comm.Parameters.AddWithValue("@addr_g", 0);
            //}
            //else
            //{
            //    comm.Parameters.AddWithValue("@bomg", 0);
            //    comm.Parameters.AddWithValue("@addr_g", 1);
            //}
            comm.Parameters.AddWithValue("@bomg", 1);
            comm.Parameters.AddWithValue("@addr_g", 1);
            comm.Parameters.AddWithValue("@addr_p", 1);
            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }

            //if (PD.fias1.sovp_addr.IsChecked == true)
            //{
            //    comm.Parameters.AddWithValue("@addr_p", 1);
            //}
            //else
            //{
            //comm.Parameters.AddWithValue("@addr_p", 0);
            //}


            if (PD.fias1.reg_rn1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias1.reg_rn1.EditValue);
            }

            if (PD.fias1.reg_np1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias1.reg_np1.EditValue);
            }

            if (PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "")
            {
                comm.Parameters.AddWithValue("@screen", "");
            }
            else
            {
                comm.Parameters.AddWithValue("@screen", Convert.ToBase64String((byte[])PD.zl_photo.EditValue)); // записываем само изображение

            }
            if (PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "")
            {
                comm.Parameters.AddWithValue("@screen1", "");
            }
            else
            {
                comm.Parameters.AddWithValue("@screen1", Convert.ToBase64String((byte[])PD.zl_podp.EditValue));

            }


            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {                
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' ", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where idguid='{perguid}')", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),'{perguid}','{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where idguid='{perguid}'))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),'{perguid}',(select event_guid from pol_persons where idguid='{perguid}'),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void Save_bt1_b1_s0_p13(MainWindow PD)
        {
            string module = "Save_bt1_b1_s0_p13";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand("insert into pol_persons (IDGUID,ENP,FAM,IM,OT,W,DR,mr,BIRTH_OKSM,C_OKSM,ss,phone,email,kateg,dost,rperson_guid)" +
                            " VALUES (newid(),@enp,@fam,@im,@ot,@w,@dr,@mr,@boksm,@coksm,@ss,@phone,@email,@kateg,@dost,@rpguid)" +

                            "update pol_persons set parentguid='00000000-0000-0000-0000-000000000000' where id=SCOPE_IDENTITY()" +



                            "insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,agent)" +
                            " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=SCOPE_IDENTITY())," +
                            "(select rperson_guid from pol_persons where id=SCOPE_IDENTITY()),@prelation,@rsmo,@rpolis,@fpolis,@agent)" +
                             "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where idguid=(select person_guid from pol_events where id=SCOPE_IDENTITY())" +

                            "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                            "values(newid(),(select person_guid from pol_events where id=SCOPE_IDENTITY()), @oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                            "(select idguid from pol_events where id=SCOPE_IDENTITY()))" +

                            " insert into pol_addresses (IDGUID,EVENT_GUID,fias_l1,FIAS_L3) " +
                            "values(newid(),(select event_guid from pol_documents where id=SCOPE_IDENTITY()),@FIAS_L1,@FIAS_L3)" +

                            "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,addres_p,dt1,dt2,event_guid)" +
                            " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                            ", 1,0,0,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                            "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select person_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY() ),2,(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()))" +


                            "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select person_guid from pol_personsb where id=SCOPE_IDENTITY() ),3,(select event_guid from pol_personsb where id=SCOPE_IDENTITY() ))" +

                            "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_personsb where id=SCOPE_IDENTITY() )," +
                            " (select idguid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1)," +
                            "(select event_guid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1))" +

                            "insert into pol_polises (vpolis,spolis,npolis,dbeg,dend,blank,dreceived,person_guid,event_guid) values (@vpolis,@spolis,@npolis,@dbeg,@dend,@blank,@dreceived, " +
                            "(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()) ) " +

                            "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                                "(select event_guid from pol_polises where id=SCOPE_IDENTITY()),(select person_guid from pol_polises where id=SCOPE_IDENTITY()))" +
                                "select person_guid from pol_oplist where id=SCOPE_IDENTITY()", con);





            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm.Parameters.AddWithValue("@email", PD.email.Text);
            comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            comm.Parameters.AddWithValue("@dend", PD.date_end.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);
            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }
            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }

            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }

            //if (PD.fias.bomj.IsChecked == true)
            //{
            //    comm.Parameters.AddWithValue("@bomg", 1);
            //    comm.Parameters.AddWithValue("@addr_g", 0);
            //}
            //else
            //{
            //    comm.Parameters.AddWithValue("@bomg", 0);
            //    comm.Parameters.AddWithValue("@addr_g", 1);
            //}
                comm.Parameters.AddWithValue("@bomg", 1);
                comm.Parameters.AddWithValue("@addr_g", 1);
                comm.Parameters.AddWithValue("@addr_p", 1);
            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }

            //if (PD.fias1.sovp_addr.IsChecked == true)
            //{
            //    comm.Parameters.AddWithValue("@addr_p", 1);
            //}
            //else
            //{
            //comm.Parameters.AddWithValue("@addr_p", 0);
            //}


            if (PD.fias1.reg_rn1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias1.reg_rn1.EditValue);
            }

            if (PD.fias1.reg_np1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias1.reg_np1.EditValue);
            }

            if (PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "")
            {
                comm.Parameters.AddWithValue("@screen", "");
            }
            else
            {
                comm.Parameters.AddWithValue("@screen", Convert.ToBase64String((byte[])PD.zl_photo.EditValue)); // записываем само изображение

            }
            if (PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "")
            {
                comm.Parameters.AddWithValue("@screen1", "");
            }
            else
            {
                comm.Parameters.AddWithValue("@screen1", Convert.ToBase64String((byte[])PD.zl_podp.EditValue));

            }


            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {                
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' ", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where idguid='{perguid}')", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),'{perguid}','{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where idguid='{perguid}'))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),'{perguid}',(select event_guid from pol_persons where idguid='{perguid}'),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void Save_bt1_b0_s1_p2_sp1(MainWindow PD)
        {
            string module = "Save_bt1_b0_s1_p2_sp1";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand("insert into pol_persons (IDGUID,ENP,FAM,IM,OT,W,DR,mr,BIRTH_OKSM,C_OKSM,ss,phone,email,kateg,dost,rperson_guid)" +
                            " VALUES (newid(),@enp,@fam,@im,@ot,@w,@dr,@mr,@boksm,@coksm,@ss,@phone,@email,@kateg,@dost,@rpguid)" +

                            "update pol_persons set parentguid='00000000-0000-0000-0000-000000000000' where id=SCOPE_IDENTITY()" +



                            "insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,agent)" +
                            " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=SCOPE_IDENTITY())," +
                            "(select rperson_guid from pol_persons where id=SCOPE_IDENTITY()),@prelation,@rsmo,@rpolis,@fpolis,@agent)" +
                             "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where idguid=(select person_guid from pol_events where id=SCOPE_IDENTITY())" +

                            "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                            "values(newid(),(select person_guid from pol_events where id=SCOPE_IDENTITY()), @oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                            "(select idguid from pol_events where id=SCOPE_IDENTITY()))" +

                            " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                                "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1)," +
                                "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1 and livestatus=1),@FIAS_L1,@FIAS_L3,@FIAS_L4,@FIAS_L6,@FIAS_L90,@FIAS_L91,@FIAS_L7,@DOM,@KORP,@EXT,@KV, " +
                                "(select event_guid from pol_documents where id=SCOPE_IDENTITY()),@HOUSE_GUID)" +

                            "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                            " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                            ", @bomg,1,@dreg,1,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                            "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select person_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY() ),2,(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()))" +


                            "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select person_guid from pol_personsb where id=SCOPE_IDENTITY() ),3,(select event_guid from pol_personsb where id=SCOPE_IDENTITY() ))" +

                            "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_personsb where id=SCOPE_IDENTITY() )," +
                            " (select idguid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1)," +
                            "(select event_guid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1))" +

                            "update pol_polises set dbeg=@dbeg,dend=@dend,dstop=@dstop,blank=0,dreceived=@dreceived,person_guid= " +
                            "(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),event_guid=(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY())" +
                            "where spolis=@spolis and npolis=@npolis " +

                            "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                            "(select event_guid from pol_polises where id=SCOPE_IDENTITY()),(select person_guid from pol_polises where id=SCOPE_IDENTITY()))" +
                            "select person_guid from pol_oplist where id=SCOPE_IDENTITY()", con);




            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm.Parameters.AddWithValue("@email", PD.email.Text);
            comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            comm.Parameters.AddWithValue("@dend", PD.date_end.DateTime);
            comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);
            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }
            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", PD.fias.reg_dom.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV", PD.fias.reg_kv.Text);
            if (PD.fias.bomj.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@bomg", 1);
                comm.Parameters.AddWithValue("@addr_g", 0);
            }
            else
            {
                comm.Parameters.AddWithValue("@bomg", 0);
                comm.Parameters.AddWithValue("@addr_g", 1);
            }

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }


            comm.Parameters.AddWithValue("@addr_p", 1);
            comm.Parameters.AddWithValue("@FIAS_L1_1", PD.fias.reg.EditValue);
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90_1", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91_1", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias.reg_dom.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM_1", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP_1", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT_1", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV_1", PD.fias.reg_kv.Text);
            //}
            comm.Parameters.AddWithValue("@screen", PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString()=="" ? "" : Convert.ToBase64String((byte[])PD.zl_photo.EditValue));
            
            comm.Parameters.AddWithValue("@screen1", PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() =="" ? "" : Convert.ToBase64String((byte[])PD.zl_podp.EditValue));

            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {                
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' ", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where idguid='{perguid}')", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),'{perguid}','{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where idguid='{perguid}'))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),'{perguid}',(select event_guid from pol_persons where idguid='{perguid}'),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                //}
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }



        public static void Save_bt1_b0_s1_p2_sp0(MainWindow PD)
        {
            string module = "Save_bt1_b0_s1_p2_sp0";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand("insert into pol_persons (IDGUID,ENP,FAM,IM,OT,W,DR,mr,BIRTH_OKSM,C_OKSM,ss,phone,email,kateg,dost,rperson_guid)" +
                            " VALUES (newid(),@enp,@fam,@im,@ot,@w,@dr,@mr,@boksm,@coksm,@ss,@phone,@email,@kateg,@dost,@rpguid)" +

                            "update pol_persons set parentguid='00000000-0000-0000-0000-000000000000' where id=SCOPE_IDENTITY()" +



                            "insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,agent)" +
                            " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=SCOPE_IDENTITY())," +
                            "(select rperson_guid from pol_persons where id=SCOPE_IDENTITY()),@prelation,@rsmo,@rpolis,@fpolis,@agent)" +
                             "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where idguid=(select person_guid from pol_events where id=SCOPE_IDENTITY())" +

                            "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                            "values(newid(),(select person_guid from pol_events where id=SCOPE_IDENTITY()),@oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                            "(select idguid from pol_events where id=SCOPE_IDENTITY()))" +

                            " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                                "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1)," +
                                "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1 and livestatus=1),@FIAS_L1,@FIAS_L3,@FIAS_L4,@FIAS_L6,@FIAS_L90,@FIAS_L91,@FIAS_L7,@DOM,@KORP,@EXT,@KV, " +
                                "(select event_guid from pol_documents where id=SCOPE_IDENTITY()),@HOUSE_GUID)" +

                            "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                            " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                            ", @bomg,1,@dreg,1,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                            "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select person_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY() ),2,(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()))" +


                            "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select person_guid from pol_personsb where id=SCOPE_IDENTITY() ),3,(select event_guid from pol_personsb where id=SCOPE_IDENTITY() ))" +

                            "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_personsb where id=SCOPE_IDENTITY() )," +
                            " (select idguid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1)," +
                            "(select event_guid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1))" +

                            "insert into pol_polises (vpolis,spolis,npolis,dbeg,dend,blank,dreceived,person_guid,event_guid) values (@vpolis,@spolis,@npolis,@dbeg,@dend,@blank,@dreceived, " +
                            "(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()) ) " +

                            "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                                "(select event_guid from pol_polises where id=SCOPE_IDENTITY()),(select person_guid from pol_polises where id=SCOPE_IDENTITY()))" +
                                "select person_guid from pol_oplist where id=SCOPE_IDENTITY()", con);



            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm.Parameters.AddWithValue("@email", PD.email.Text);
            comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.EditValue);
            if (Convert.ToDateTime(PD.date_end.EditValue) == DateTime.MinValue)
            {
                comm.Parameters.AddWithValue("@dend", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dend", PD.date_end.EditValue);
            }
            if (Convert.ToDateTime(PD.fakt_prekr.EditValue) == DateTime.MinValue)
            {
                comm.Parameters.AddWithValue("@dstop", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue);
            }
            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);
            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }
            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", PD.fias.reg_dom.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV", PD.fias.reg_kv.Text);
            if (PD.fias.bomj.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@bomg", 1);
                comm.Parameters.AddWithValue("@addr_g", 0);
            }
            else
            {
                comm.Parameters.AddWithValue("@bomg", 0);
                comm.Parameters.AddWithValue("@addr_g", 1);
            }

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }

            //if (PD.fias1.sovp_addr.IsChecked == true)
            //{
            comm.Parameters.AddWithValue("@FIAS_L1_1", PD.fias.reg.EditValue);
            comm.Parameters.AddWithValue("@addr_p", 1);
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90_1", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91_1", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias.reg_dom.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM_1", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP_1", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT_1", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV_1", PD.fias.reg_kv.Text);
            comm.Parameters.AddWithValue("@screen", PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_photo.EditValue));

            comm.Parameters.AddWithValue("@screen1", PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_podp.EditValue));


            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {                
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}'", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where idguid='{perguid}')", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),'{perguid}','{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where idguid='{perguid}'))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),'{perguid}',(select event_guid from pol_persons where idguid='{perguid}'),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void Save_bt1_b0_s1_p13(MainWindow PD)
        {
            string module = "Save_bt1_b0_s1_p13";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand("insert into pol_persons (IDGUID,ENP,FAM,IM,OT,W,DR,mr,BIRTH_OKSM,C_OKSM,ss,phone,email,kateg,dost,rperson_guid)" +
                            " VALUES (newid(),@enp,@fam,@im,@ot,@w,@dr,@mr,@boksm,@coksm,@ss,@phone,@email,@kateg,@dost,@rpguid)" +

                            "update pol_persons set parentguid='00000000-0000-0000-0000-000000000000' where id=SCOPE_IDENTITY()" +

                            "insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,agent)" +
                            " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=SCOPE_IDENTITY())," +
                            "(select rperson_guid from pol_persons where id=SCOPE_IDENTITY()),@prelation,@rsmo,@rpolis,@fpolis,@agent)" +

                             "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where idguid=(select person_guid from pol_events where id=SCOPE_IDENTITY())" +
                                            

                            "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                            "values(newid(),(select person_guid from pol_events where id=SCOPE_IDENTITY()), @oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                            "(select idguid from pol_events where id=SCOPE_IDENTITY()))" +

                            " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                                "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1)," +
                                "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1 and livestatus=1),@FIAS_L1,@FIAS_L3,@FIAS_L4,@FIAS_L6,@FIAS_L90,@FIAS_L91,@FIAS_L7,@DOM,@KORP,@EXT,@KV, " +
                                "(select event_guid from pol_documents where id=SCOPE_IDENTITY()),@HOUSE_GUID)" +

                            "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                            " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                            ", @bomg,1,@dreg,1,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                            "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select person_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY() ),2,(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()))" +


                            "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select person_guid from pol_personsb where id=SCOPE_IDENTITY() ),3,(select event_guid from pol_personsb where id=SCOPE_IDENTITY() ))" +

                            "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_personsb where id=SCOPE_IDENTITY() )," +
                            " (select idguid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1)," +
                            "(select event_guid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1))" +

                            "insert into pol_polises (vpolis,spolis,npolis,dbeg,dend,blank,dreceived,person_guid,event_guid) values (@vpolis,@spolis,@npolis,@dbeg,@dend,@blank,@dreceived, " +
                            "(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()) ) " +

                             "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                                "(select event_guid from pol_polises where id=SCOPE_IDENTITY()),(select person_guid from pol_polises where id=SCOPE_IDENTITY()))" +
                                "select person_guid from pol_oplist where id=SCOPE_IDENTITY()", con);



            
            
            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm.Parameters.AddWithValue("@email", PD.email.Text);
            comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            if (Convert.ToDateTime(PD.date_end.EditValue) == DateTime.MinValue)
            {
                comm.Parameters.AddWithValue("@dend", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dend", PD.date_end.EditValue);
            }
            if (Convert.ToDateTime(PD.fakt_prekr.EditValue) == DateTime.MinValue)
            {
                comm.Parameters.AddWithValue("@dstop", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue);
            }
            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);
            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }
            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", PD.fias.reg_dom.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV", PD.fias.reg_kv.Text);
            if (PD.fias.bomj.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@bomg", 1);
                comm.Parameters.AddWithValue("@addr_g", 0);
            }
            else
            {
                comm.Parameters.AddWithValue("@bomg", 0);
                comm.Parameters.AddWithValue("@addr_g", 1);
            }

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }

            //if (PD.fias1.sovp_addr.IsChecked == true)
            //{
            comm.Parameters.AddWithValue("@FIAS_L1_1", PD.fias.reg.EditValue);
            comm.Parameters.AddWithValue("@addr_p", 1);
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90_1", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91_1", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias.reg_dom.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM_1", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP_1", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT_1", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV_1", PD.fias.reg_kv.Text);

            comm.Parameters.AddWithValue("@screen", PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_photo.EditValue));

            comm.Parameters.AddWithValue("@screen1", PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_podp.EditValue));


            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");

            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {                
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' ", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where idguid='{perguid}')", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),'{perguid}','{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where idguid='{perguid}'))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),'{perguid}',(select event_guid from pol_persons where idguid='{perguid}'),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void Save_bt3_b0_s0_p13(MainWindow PD)
        {
            string zap_polis;
            if (PD.blank_polis != false)
            {
                zap_polis = SPR.update_polises;
            }
            else
            {
                if (!PD.no_new_polis)
                {
                    zap_polis = SPR.insert_polises;
                }
                else
                {
                    zap_polis = SPR.update_polises;
                }

            }

            string module = "Save_bt3_b0_s0_p13";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);

            // не забыть исправить insert на update в pol_persons

            SqlCommand comm = new SqlCommand($"update pol_documents set main=1,active=0 where event_guid=(select event_guid from pol_persons where id=@id_p) and main=1 and active=1" +
                "update pol_relation_addr_pers set active=0 where event_guid= (select event_guid from pol_persons where id=@id_p) " +
                
                
                "update pol_persons set parentguid='00000000-0000-0000-0000-000000000000', ENP=@enp,FAM=@fam,IM=@im,OT=@ot,W=@w,DR=@dr,ss=@ss,mr=@docmr,birth_oksm=@boksm," +
                 "c_oksm=@coksm,phone=@phone,email=@email,kateg=@kateg,dost=@dost,rperson_guid=@rpguid, active=1, comment='' where id=@id_p " +

                " insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,agent) " +
                " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=@id_p)," +
                "(select rperson_guid from pol_persons where id=@id_p),@prelation,@rsmo,@rpolis,@fpolis,@agent) " +

                 "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where id=@id_p " +
                                  

                "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                "values(newid(),(select idguid from pol_persons where id=@id_p), @oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                "(select event_guid from pol_persons where id=@id_p)) " +

                " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                    "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1)," +
                    "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1 and livestatus=1),@FIAS_L1,@FIAS_L3,@FIAS_L4,@FIAS_L6,@FIAS_L90,@FIAS_L91,@FIAS_L7,@DOM,@KORP,@EXT,@KV, " +
                    "(select event_guid from pol_persons where id=@id_p),@HOUSE_GUID) " +
                                    

                "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                " values((select idguid from pol_persons where id=@id_p), (select idguid from pol_addresses where id=SCOPE_IDENTITY())," +
                " @bomg,1,@dreg,0,sysdatetime(),null,(select event_guid from pol_persons where id=@id_p)) " +

                " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                    "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7_1 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7_1 and actstatus=1)," +
                    "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1_1 and livestatus=1),@FIAS_L1_1,@FIAS_L3_1,@FIAS_L4_1,@FIAS_L6_1,@FIAS_L90_1,@FIAS_L91_1,@FIAS_L7_1,@DOM_1,@KORP_1,@EXT_1,@KV_1, " +
                    "(select event_guid from pol_persons where id=@id_p),@HOUSE_GUID_1) " +

                "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                " values((select idguid from pol_persons where id=@id_p ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                ", @bomg,0,@dreg1,1,sysdatetime(),null,(select event_guid from pol_persons where id=@id_p)) " +


                "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY() )," +
                " (select idguid from pol_documents where event_guid= (select event_guid from pol_persons where id=@id_p ) and main=1)," +
                "(select event_guid from pol_persons where id=@id_p)) " +

                zap_polis +

                "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                    "(select event_guid from pol_persons where id=@id_p),(select idguid from pol_persons where id=@id_p)) " +

                 "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select idguid from pol_persons where id=@id_p ),2,(select event_guid from pol_persons where id=@id_p )) " +
                 "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select idguid from pol_persons where id=@id_p ),3,(select event_guid from pol_persons where id=@id_p )) " +
                                                 
                
                "select idguid from pol_persons where id=@id_p", con);





            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm.Parameters.AddWithValue("@email", PD.email.Text);
            comm.Parameters.AddWithValue("@id_p", Convert.ToInt32(Vars.IdP));
            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            if (Convert.ToDateTime(PD.date_end.EditValue) == DateTime.MinValue || PD.date_end.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dend", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dend", PD.date_end.EditValue);
            }
            if (Convert.ToDateTime(PD.fakt_prekr.EditValue) == DateTime.MinValue || PD.fakt_prekr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dstop", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue);
            }
            if (Convert.ToDateTime(PD.dout.EditValue) == DateTime.MinValue || PD.dout.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dout", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dout", PD.dout.EditValue);
            }
            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);
            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }

            if (PD.kat_zl.EditValue != null)
            {
                comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            }
            else
            {
                MessageBox.Show("Выберите категогрию ЗЛ!");
            }

            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            if (PD.fias.reg.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L1", PD.L1);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            }

            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4", PD.fias.reg_town.EditValue);
            }
            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", PD.fias.reg_dom.EditValue);
            }

            comm.Parameters.AddWithValue("@DOM", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV", PD.fias.reg_kv.Text);
            if (PD.fias.bomj.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@bomg", 1);
                comm.Parameters.AddWithValue("@addr_g", 0);
            }
            else
            {
                comm.Parameters.AddWithValue("@bomg", 0);
                comm.Parameters.AddWithValue("@addr_g", 1);
            }

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }
            comm.Parameters.AddWithValue("@dreg1", PD.fias1.reg_dr1.DateTime);

            if (PD.fias1.sovp_addr.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@addr_p", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@addr_p", 0);
            }
            if (PD.fias1.reg1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L1_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L1_1", PD.fias1.reg1.EditValue);
            }


            if (PD.fias1.reg_rn1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias1.reg_rn1.EditValue);
            }
            if (PD.fias1.reg_town1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", PD.fias1.reg_town1.EditValue);
            }

            if (PD.fias1.reg_np1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias1.reg_np1.EditValue);
            }
            if (PD.fias1.reg_ul1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", PD.fias1.reg_ul1.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90_1", PD.fias1.reg_ul1.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91_1", PD.fias1.reg_ul1.EditValue);
            }
            if (PD.fias1.reg_dom1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias1.reg_dom1.EditValue);
            }

            comm.Parameters.AddWithValue("@DOM_1", PD.domsplit1);
            comm.Parameters.AddWithValue("@KORP_1", PD.fias1.reg_korp1.Text);
            comm.Parameters.AddWithValue("@EXT_1", PD.fias1.reg_str1.Text);
            comm.Parameters.AddWithValue("@KV_1", PD.fias1.reg_kv1.Text);

            comm.Parameters.AddWithValue("@screen", PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_photo.EditValue));

            comm.Parameters.AddWithValue("@screen1", PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_podp.EditValue));


            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where id={Vars.IdP})", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where id={Vars.IdP})", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),(select idguid from pol_persons where id={Vars.IdP}),'{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where id={Vars.IdP}))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),(select idguid from pol_persons where id={Vars.IdP}),(select event_guid from pol_persons where id={Vars.IdP}),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void Save_bt3_b1_s0_p13(MainWindow PD)
        {
            string zap_polis;
            if (PD.blank_polis != false)
            {
                zap_polis = SPR.update_polises;
            }
            else
            {
                if (!PD.no_new_polis)
                {
                    zap_polis = SPR.insert_polises;
                }
                else
                {
                    zap_polis = SPR.update_polises;
                }

            }
            string module = "Save_bt3_b1_s0_p13";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand("update pol_persons set parentguid='00000000-0000-0000-0000-000000000000', ENP=@enp,FAM=@fam,IM=@im,OT=@ot,W=@w,DR=@dr,ss=@ss,mr=@docmr,birth_oksm=@boksm," +
                 "c_oksm=@coksm,phone=@phone,email=@email,kateg=@kateg,dost=@dost,rperson_guid=@rpguid, active=1 where id=@id_p " +

                "insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,agent)" +
                " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=@id_p)," +
                "(select rperson_guid from pol_persons where id=@id_p),@prelation,@rsmo,@rpolis,@fpolis,@agent)" +
                 "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where idguid=(select person_guid from pol_events where id=SCOPE_IDENTITY())" +

                "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                "values(newid(),(select person_guid from pol_events where id=SCOPE_IDENTITY()), @oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                "(select idguid from pol_events where id=SCOPE_IDENTITY()))" +

                " insert into pol_addresses (IDGUID,EVENT_GUID,fias_l1,FIAS_L3) " +
                "values(newid(),(select event_guid from pol_documents where id=SCOPE_IDENTITY()),@FIAS_L1,@FIAS_L3)" +

                "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,addres_p,dt1,dt2,event_guid)" +
                " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                ", 1,0,0,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select idguid from pol_persons where id=@id_p ),2,(select event_guid from pol_persons where id=@id_p )) " +
                 "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select idguid from pol_persons where id=@id_p ),3,(select event_guid from pol_persons where id=@id_p )) " +

                "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_personsb where id=SCOPE_IDENTITY() )," +
                " (select idguid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1)," +
                "(select event_guid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1))" +

                zap_polis +


                "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                    "(select event_guid from pol_polises where id=SCOPE_IDENTITY()),(select person_guid from pol_polises where id=SCOPE_IDENTITY()))" +
                    "select person_guid from pol_oplist where id=SCOPE_IDENTITY()", con);






            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm.Parameters.AddWithValue("@email", PD.email.Text);
            comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            comm.Parameters.AddWithValue("@id_p", Convert.ToInt32(Vars.IdP));
            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            if (Convert.ToDateTime(PD.date_end.EditValue) == DateTime.MinValue || PD.date_end.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dend", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dend", PD.date_end.EditValue);
            }
            if (Convert.ToDateTime(PD.fakt_prekr.EditValue) == DateTime.MinValue || PD.fakt_prekr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dstop", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue);
            }
            if (Convert.ToDateTime(PD.dout.EditValue) == DateTime.MinValue || PD.dout.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dout", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dout", PD.dout.EditValue);
            }
            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);
            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }
            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }

            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }

            if (PD.fias.bomj.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@bomg", 1);
                comm.Parameters.AddWithValue("@addr_g", 0);
            }
            else
            {
                comm.Parameters.AddWithValue("@bomg", 0);
                comm.Parameters.AddWithValue("@addr_g", 1);
            }

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }

            if (PD.fias1.sovp_addr.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@addr_p", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@addr_p", 0);
            }


            if (PD.fias1.reg_rn1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias1.reg_rn1.EditValue);
            }

            if (PD.fias1.reg_np1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias1.reg_np1.EditValue);
            }

            comm.Parameters.AddWithValue("@screen", PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_photo.EditValue));

            comm.Parameters.AddWithValue("@screen1", PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_podp.EditValue));

            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where id={Vars.IdP})", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where id={Vars.IdP})", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),(select idguid from pol_persons where id={Vars.IdP}),'{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where id={Vars.IdP}))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),(select idguid from pol_persons where id={Vars.IdP}),(select event_guid from pol_persons where id={Vars.IdP}),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void Save_bt3_b0_s1_p13(MainWindow PD)
        {
            string zap_polis;

            if (PD.blank_polis != false)
            {
                zap_polis = SPR.update_polises;
            }
            else
            {
                if(!PD.no_new_polis)
                {
                    zap_polis = SPR.insert_polises;
                }
                else
                {
                    zap_polis = SPR.update_polises;
                }
                
            }
            string module = "Save_bt1_b0_s1_p2_sp1";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand("update pol_documents set main=1,active=0 where event_guid=(select event_guid from pol_persons where id=@id_p) and main=1 and active=1" +
                "update pol_relation_addr_pers set active=0 where event_guid= (select event_guid from pol_persons where id=@id_p) " +


                "update pol_persons set parentguid='00000000-0000-0000-0000-000000000000', ENP=@enp,FAM=@fam,IM=@im,OT=@ot,W=@w,DR=@dr,ss=@ss,mr=@docmr,birth_oksm=@boksm," +
                 "c_oksm=@coksm,phone=@phone,email=@email,kateg=@kateg,dost=@dost,rperson_guid=@rpguid, active=1, comment='' where id=@id_p " +

                " insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,agent) " +
                " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=@id_p)," +
                "(select rperson_guid from pol_persons where id=@id_p),@prelation,@rsmo,@rpolis,@fpolis,@agent) " +

                 "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where id=@id_p " +


                "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                "values(newid(),(select idguid from pol_persons where id=@id_p), @oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                "(select event_guid from pol_persons where id=@id_p)) " +

                " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                    "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1)," +
                    "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1 and livestatus=1),@FIAS_L1,@FIAS_L3,@FIAS_L4,@FIAS_L6,@FIAS_L90,@FIAS_L91,@FIAS_L7,@DOM,@KORP,@EXT,@KV, " +
                    "(select event_guid from pol_persons where id=@id_p),@HOUSE_GUID) " +


                "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                " values((select idguid from pol_persons where id=@id_p), (select idguid from pol_addresses where id=SCOPE_IDENTITY())," +
                " @bomg,1,@dreg,1,sysdatetime(),null,(select event_guid from pol_persons where id=@id_p)) " +
                                

                "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY() )," +
                " (select idguid from pol_documents where event_guid= (select event_guid from pol_persons where id=@id_p ) and main=1)," +
                "(select event_guid from pol_persons where id=@id_p)) " +

                zap_polis +

                "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                    "(select event_guid from pol_persons where id=@id_p),(select idguid from pol_persons where id=@id_p)) " +

                 "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select idguid from pol_persons where id=@id_p ),2,(select event_guid from pol_persons where id=@id_p )) " +
                 "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select idguid from pol_persons where id=@id_p ),3,(select event_guid from pol_persons where id=@id_p )) " +


                "select idguid from pol_persons where id=@id_p", con);







            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm.Parameters.AddWithValue("@email", PD.email.Text);
            comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            comm.Parameters.AddWithValue("@id_p", Vars.IdP);
            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            if (Convert.ToDateTime(PD.date_end.EditValue) == DateTime.MinValue || PD.date_end.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dend", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dend", PD.date_end.EditValue);
            }
            if (Convert.ToDateTime(PD.fakt_prekr.EditValue) == DateTime.MinValue || PD.fakt_prekr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dstop", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue);
            }
            if (Convert.ToDateTime(PD.dout.EditValue) == DateTime.MinValue || PD.dout.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dout", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dout", PD.dout.EditValue);
            }
            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);
            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }
            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }

            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", PD.fias.reg_dom.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV", PD.fias.reg_kv.Text);
            if (PD.fias.bomj.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@bomg", 1);
                comm.Parameters.AddWithValue("@addr_g", 0);
            }
            else
            {
                comm.Parameters.AddWithValue("@bomg", 0);
                comm.Parameters.AddWithValue("@addr_g", 1);
            }

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }

            //if (PD.fias1.sovp_addr.IsChecked == true)
            //{
            comm.Parameters.AddWithValue("@addr_p", 1);
            //}
            //else
            //{
            //    comm.Parameters.AddWithValue("@addr_p", 0);
            //}

            comm.Parameters.AddWithValue("@FIAS_L1_1", PD.fias.reg.EditValue);
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90_1", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91_1", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias.reg_dom.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM_1", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP_1", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT_1", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV_1", PD.fias.reg_kv.Text);

            comm.Parameters.AddWithValue("@screen", PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_photo.EditValue));

            comm.Parameters.AddWithValue("@screen1", PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_podp.EditValue));

            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where id={Vars.IdP})", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where id={Vars.IdP})", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),(select idguid from pol_persons where id={Vars.IdP}),'{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where id={Vars.IdP}))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),(select idguid from pol_persons where id={Vars.IdP}),(select event_guid from pol_persons where id={Vars.IdP}),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void Save_bt2_prf1(MainWindow PD)
        {
            //var insert1 = "";
            //var insert2 = "";
            //var update1 = "";
            //var update2 = "";
            //var addr_mest_zhit = SPR.MyReader.SELECTVAIN($"select IDGUID from pol_addresses pa left join POL_RELATION_ADDR_PERS pr on pa.IDGUID = pr.ADDR_GUID where pr.event_guid = (select event_guid from pol_persons where id = {Vars.IdP}) and pr.addres_p = 1", Properties.Settings.Default.DocExchangeConnectionString);
            //if (string.IsNullOrEmpty(addr_mest_zhit))
            //{
            //    insert1 = SPR.insert_address+" "+SPR.insert_pol_relation_address;
            //}
            //else
            //{
            //    update1 = SPR.update_addresses + " "+SPR.update__pol_relation;
            //}
            //var addr_reg = SPR.MyReader.SELECTVAIN($"select IDGUID from pol_addresses pa left join POL_RELATION_ADDR_PERS pr on pa.IDGUID = pr.ADDR_GUID where pr.event_guid = (select event_guid from pol_persons where id = {Vars.IdP}) and pr.addres_g = 1", Properties.Settings.Default.DocExchangeConnectionString);
            //if (string.IsNullOrEmpty(addr_reg))
            //{
            //    insert2 = SPR.insert_address_2+" "+SPR.insert_pol_relation_address_2;
            //}
            //else
            //{
            //    update2 = SPR.update_addresses_2 + " " + SPR.update__pol_relation_2;
            //}
            string module = "Save_bt2_prf1";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con2 = new SqlConnection(connectionString);
            SqlCommand comm2 = new SqlCommand("update pol_persons set DOP_COMMENT=@DOP_COMMENT,COMMENT=@COMMENT, SROKDOVERENOSTI=@SROK,PRIZNAKVIDACHI=@PRIZNAKVIDACHI,DATEVIDACHI=@DATEVIDACHI,ENP=@enp,FAM=@fam,IM=@im,OT=@ot,W=@w,DR=@dr,ss=@ss,mr=@mr,birth_oksm=@boksm,"
                                              + "c_oksm=@coksm,phone=@phone,email=@email,kateg=@kateg,dost=@dost,ddeath=@ddeath,rperson_guid=@rpguid, mo=@mo, dstart=@date_mo where id=@id_p " +

                                             // insert1+" "+insert2 + " " + update1 + " " + update2 +
                "update pol_addresses set fias_l1=@FIAS_L1,fias_l3=@FIAS_L3,fias_l4=@FIAS_L4,fias_l6=@FIAS_L6,fias_l7=@FIAS_L7,fias_l90=@FIAS_L90," +
                "fias_l91=@FIAS_L91, dom=@DOM,korp=@KORP,ext=@EXT,kv=@KV, house_guid=@HOUSE_GUID where idguid=(select ADDR_GUID from pol_relation_addr_pers where addres_g=1 and event_guid=(select event_guid from pol_persons where id=@id_p)) and " +
                "event_guid=(select event_guid from pol_persons where id =@id_p) " +

                "update pol_addresses set fias_l1=@FIAS_L1_1,fias_l3=@FIAS_L3_1,fias_l4=@FIAS_L4_1,fias_l6=@FIAS_L6_1,fias_l7=@FIAS_L7_1,fias_l90=@FIAS_L90_1," +
                "fias_l91=@FIAS_L91_1, dom=@DOM_1,korp=@KORP_1,ext=@EXT_1,kv=@KV_1,house_guid=@HOUSE_GUID_1 where idguid=(select ADDR_GUID from pol_relation_addr_pers where addres_p=1 and event_guid=(select event_guid from pol_persons where id=@id_p)) and " +
                "event_guid=(select event_guid from pol_persons where id =@id_p) " +

                "update pol_relation_addr_pers SET bomg=@bomg,addres_g=@addr_g,addres_p=@addr_p,dreg=@dreg where addres_g=1 and event_guid=(select event_guid from pol_persons where id=@id_p) " +

                "update pol_relation_addr_pers SET bomg=@bomg,addres_g=@addr_g1,addres_p=@addr_p1,dreg=@dreg where addres_p=1  and event_guid=(select event_guid from pol_persons where id=@id_p) " +



                "update pol_documents set oksm=@oksm,doctype=@doctype,docser=@docser,docnum=@docnam,docdate=@docdate,docexp=@docexp,name_vp=@name_vp,name_vp_code=@vp_code, " +
                "docmr=@docmr where event_guid=(select event_guid from pol_persons where id=@id_p)" +

                "update pol_polises set vpolis=@vpolis,spolis=@spolis,npolis=@npolis,dbeg=@dbeg,dend=@dend,dstop=@dstop,blank=@blank,dreceived=@dreceived " +
                "where event_guid=(select event_guid from pol_persons where id=@id_p)" +

               "update pol_personsb set photo=@screen where event_guid=(select event_guid from pol_persons where id =@id_p) and type=2 " +
               "update pol_personsb set photo=@screen1 where event_guid=(select event_guid from pol_persons where id =@id_p) and type=3 " +

               "update pol_events set unload=0,tip_op=@tip_op,rsmo=@rsmo,rpolis=@rpolis,fpolis=@fpolis,method=@method,petition=@pet,dvizit=@dvizit " +
               "where idguid=(select event_guid from pol_persons where id=@id_p)" +
               "select idguid from pol_persons where id=@id_p", con2);



            comm2.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm2.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm2.Parameters.AddWithValue("@im", PD.im.Text);
            comm2.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm2.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm2.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm2.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm2.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm2.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm2.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm2.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm2.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm2.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm2.Parameters.AddWithValue("@method", Vars.Sposob);
            comm2.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));
            comm2.Parameters.AddWithValue("@SROK", PD.srok_doverenosti.EditValue ?? DBNull.Value);
            comm2.Parameters.AddWithValue("@COMMENT", PD.comments.Text);
            comm2.Parameters.AddWithValue("@DOP_COMMENT", PD.Dop_comments.Text);
            if (PD.priznak_vidachi.IsChecked == true)
            {
                comm2.Parameters.AddWithValue("@PRIZNAKVIDACHI", "1");
            }
            else
            {
                comm2.Parameters.AddWithValue("@PRIZNAKVIDACHI", "0");
            }

            comm2.Parameters.AddWithValue("@DATEVIDACHI", PD.date_vidachi.DateTime);

            comm2.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                comm2.Parameters.AddWithValue("@mo", PD.mo_cmb.EditValue.ToString());
            }
            else
            {
                comm2.Parameters.AddWithValue("@mo", "");
            }
            //if (Convert.ToDateTime(date_mo.EditValue) == DateTime.MinValue || date_mo.EditValue == null)
            //{
            //    comm2.Parameters.AddWithValue("@date_mo", DBNull.Value);
            //}
            //else
            //{
            //    comm2.Parameters.AddWithValue("@date_mo", date_mo.EditValue);
            //}
            comm2.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);

            if (Convert.ToDateTime(PD.ddeath.EditValue) == DateTime.MinValue)
            {
                comm2.Parameters.AddWithValue("@ddeath", DBNull.Value);
            }
            else
            {
                comm2.Parameters.AddWithValue("@ddeath", PD.date_end.EditValue);
            }

            comm2.Parameters.AddWithValue("@oksm", PD.str_vid.EditValue.ToString());
            comm2.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm2.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm2.Parameters.AddWithValue("@email", PD.email.Text);
            comm2.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            comm2.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm2.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm2.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm2.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            if (Convert.ToDateTime(PD.date_end.EditValue) == DateTime.MinValue)
            {
                comm2.Parameters.AddWithValue("@dend", DBNull.Value);
            }
            else
            {
                comm2.Parameters.AddWithValue("@dend", PD.date_end.EditValue);
            }
            if (Convert.ToDateTime(PD.fakt_prekr.EditValue) == DateTime.MinValue)
            {
                comm2.Parameters.AddWithValue("@dstop", DBNull.Value);
            }
            else
            {
                comm2.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue);
            }

            comm2.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue??DBNull.Value);
            if (PD.pustoy.IsChecked == true)
            {
                comm2.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm2.Parameters.AddWithValue("@blank", 0);
            }


            comm2.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm2.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm2.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm2.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm2.Parameters.AddWithValue("@docexp", PD.docexp.EditValue ?? DBNull.Value);
            comm2.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm2.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm2.Parameters.AddWithValue("@docmr", PD.mr2.Text);

            comm2.Parameters.AddWithValue("@id_p", Vars.IdP);
            if (PD.s == "" || PD.s == null || PD.dost1.EditValue==null)
            {
                comm2.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm2.Parameters.AddWithValue("@dost", PD.s);
            }

            comm2.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm2.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm2.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm2.Parameters.AddWithValue("@FIAS_L4", Guid.Empty);
            }
            else
            {
                comm2.Parameters.AddWithValue("@FIAS_L4", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm2.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm2.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm2.Parameters.AddWithValue("@FIAS_L7", Guid.Empty);
                comm2.Parameters.AddWithValue("@FIAS_L90", Guid.Empty);
                comm2.Parameters.AddWithValue("@FIAS_L91", Guid.Empty);
                
            }
            else
            {
                comm2.Parameters.AddWithValue("@FIAS_L7", PD.fias.reg_ul.EditValue);
                comm2.Parameters.AddWithValue("@FIAS_L90", PD.fias.reg_ul.EditValue);
                comm2.Parameters.AddWithValue("@FIAS_L91", PD.fias.reg_ul.EditValue);
               
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm2.Parameters.AddWithValue("@HOUSE_GUID", Guid.Empty);
            }
            else
            {
                comm2.Parameters.AddWithValue("@HOUSE_GUID", PD.fias.reg_dom.EditValue);
            }
            comm2.Parameters.AddWithValue("@DOM", PD.domsplit);
            comm2.Parameters.AddWithValue("@KORP", PD.fias.reg_korp.Text);
            comm2.Parameters.AddWithValue("@EXT", PD.fias.reg_str.Text);
            comm2.Parameters.AddWithValue("@KV", PD.fias.reg_kv.Text);
            if (PD.fias.bomj.IsChecked == true)
            {
                comm2.Parameters.AddWithValue("@bomg", 1);
                comm2.Parameters.AddWithValue("@addr_g", 1);
                comm2.Parameters.AddWithValue("@addr_p", 1);
                comm2.Parameters.AddWithValue("@addr_p1", 1);
                comm2.Parameters.AddWithValue("@addr_g1", 1);

            }
            else 
            {
                comm2.Parameters.AddWithValue("@bomg", 0);
                comm2.Parameters.AddWithValue("@addr_g", 1);
                if (PD.fias1.sovp_addr.IsChecked == true)
                {
                    comm2.Parameters.AddWithValue("@addr_p", 1);
                    comm2.Parameters.AddWithValue("@addr_p1", 1);
                    comm2.Parameters.AddWithValue("@addr_g1", 1);
                }
                else
                {
                    comm2.Parameters.AddWithValue("@addr_p", 0);
                    comm2.Parameters.AddWithValue("@addr_p1", 1);
                    comm2.Parameters.AddWithValue("@addr_g1", 0);
                }

            }



            if (PD.fias.reg_dr.EditValue == null)
            {
                comm2.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm2.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }

            if (PD.fias1.sovp_addr.IsChecked == true)
            {
                comm2.Parameters.AddWithValue("@FIAS_L1_1", PD.fias.reg.EditValue);
                if (PD.fias.reg_rn.EditValue == null)
                {
                    comm2.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
                }
                else
                {
                    comm2.Parameters.AddWithValue("@FIAS_L3_1", PD.fias.reg_rn.EditValue);
                }
                if (PD.fias.reg_town.EditValue == null)
                {
                    comm2.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
                }
                else
                {
                    comm2.Parameters.AddWithValue("@FIAS_L4_1", PD.fias.reg_town.EditValue);
                }
                if (PD.fias.reg_np.EditValue == null)
                {
                    comm2.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
                }
                else
                {
                    comm2.Parameters.AddWithValue("@FIAS_L6_1", PD.fias.reg_np.EditValue);
                }
                if (PD.fias.reg_ul.EditValue == null)
                {
                    comm2.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                    comm2.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                    comm2.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
                }
                else
                {
                    comm2.Parameters.AddWithValue("@FIAS_L7_1", PD.fias.reg_ul.EditValue);
                    comm2.Parameters.AddWithValue("@FIAS_L90_1", PD.fias.reg_ul.EditValue);
                    comm2.Parameters.AddWithValue("@FIAS_L91_1", PD.fias.reg_ul.EditValue);
                }
                if (PD.fias.reg_dom.EditValue == null)
                {
                    comm2.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
                }
                else
                {
                    comm2.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias.reg_dom.EditValue);
                }

                comm2.Parameters.AddWithValue("@DOM_1", PD.domsplit);
                comm2.Parameters.AddWithValue("@KORP_1", PD.fias.reg_korp.Text);
                comm2.Parameters.AddWithValue("@EXT_1", PD.fias.reg_str.Text);
                comm2.Parameters.AddWithValue("@KV_1", PD.fias.reg_kv.Text);
            }
            else
            {
                comm2.Parameters.AddWithValue("@FIAS_L1_1", PD.fias1.reg1.EditValue);
                if (PD.fias1.reg_rn1.EditValue == null)
                {
                    comm2.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
                }
                else
                {
                    comm2.Parameters.AddWithValue("@FIAS_L3_1", PD.fias1.reg_rn1.EditValue);
                }
                if (PD.fias1.reg_town1.EditValue == null)
                {
                    comm2.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
                }
                else
                {
                    comm2.Parameters.AddWithValue("@FIAS_L4_1", PD.fias1.reg_town1.EditValue);
                }
                if (PD.fias1.reg_np1.EditValue == null)
                {
                    comm2.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
                }
                else
                {
                    comm2.Parameters.AddWithValue("@FIAS_L6_1", PD.fias1.reg_np1.EditValue);
                }
                if (PD.fias1.reg_ul1.EditValue == null)
                {
                    comm2.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                    comm2.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                    comm2.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
                }
                else
                {
                    comm2.Parameters.AddWithValue("@FIAS_L7_1", PD.fias1.reg_ul1.EditValue);
                    comm2.Parameters.AddWithValue("@FIAS_L90_1", PD.fias1.reg_ul1.EditValue);
                    comm2.Parameters.AddWithValue("@FIAS_L91_1", PD.fias1.reg_ul1.EditValue);
                }
                if (PD.fias.reg_dom.EditValue == null)
                {
                    comm2.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
                }
                else
                {
                    comm2.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias1.reg_dom1.EditValue);
                }
                comm2.Parameters.AddWithValue("@DOM_1", PD.domsplit1);
                comm2.Parameters.AddWithValue("@KORP_1", PD.fias1.reg_korp1.Text);
                comm2.Parameters.AddWithValue("@EXT_1", PD.fias1.reg_str1.Text);
                comm2.Parameters.AddWithValue("@KV_1", PD.fias1.reg_kv1.Text);
            }

            comm2.Parameters.AddWithValue("@screen", PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_photo.EditValue));

            comm2.Parameters.AddWithValue("@screen1", PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_podp.EditValue));


            comm2.Parameters.AddWithValue("@rpguid", PD.rper);

            con2.Open();
            tr = con2.BeginTransaction();
            comm2.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm2.ExecuteScalar();
                tr.Commit();
                con2.Close();
            }
            catch (Exception e)
            {
                tr.Rollback();
                con2.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            if (PD.rper == Guid.Empty)
            {
                if(PD.fam1.Text == "")
                {
                    comm2.Parameters.AddWithValue("@rpguid", Guid.Empty);
                }
                else
                {
                    var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                    SqlConnection con = new SqlConnection(connectionString1);
                    SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                         " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                         "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                         " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                         "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                         "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                         " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                    comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                    comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                    comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                    comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                    comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                    comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                    comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                    comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                    comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                    if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                    {
                        comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                    }
                    else
                    {
                        comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                    }
                    comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                    con.Open();
                    Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                    con.Close();
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where id={Vars.IdP})", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();
                }
                
            }
            else
            {
                var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                SqlConnection con = new SqlConnection(connectionString1);
                SqlCommand comm31 = new SqlCommand($@"update pol_persons set fam='{PD.fam1.Text}', im='{PD.im1.Text}', ot='{PD.ot1.Text}',
 dr=@dr, w={PD.pol_pr.SelectedIndex}, SROKDOVERENOSTI=@srok_d where idguid='{PD.rper}'
  update pol_documents set doctype={PD.doctype1.EditValue ?? 14}, docser='{PD.docser1.Text}', docnum='{PD.docnum1.Text}', docdate='{PD.docdate1.DateTime}'
  where person_guid='{PD.rper}' 
update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where id={Vars.IdP})", con);
                
                comm31.Parameters.AddWithValue("@srok_d", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                {
                    comm31.Parameters.AddWithValue("@dr", "01-01-1900 00:00:00.000");
                }
                else
                {
                    comm31.Parameters.AddWithValue("@dr", PD.dr1.DateTime);
                }
                con.Open();
                comm31.ExecuteNonQuery();
                con.Close();
                //comm2.Parameters.AddWithValue("@rpguid", PD.rper);
            }           
            
                //con2.Open();
                //comm2.ExecuteNonQuery();
                //con2.Close();


            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid,docexp)
                                values(newid(),(select idguid from pol_persons where id={Vars.IdP}),'{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where id={Vars.IdP}),'{PD.docexp2.EditValue}')", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0 && PD.doc_type1.SelectedIndex != -1)
            {
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', DOCEXP='{PD.docexp2.EditValue}',
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.old_doc != 0 && PD.doc_type1.SelectedIndex == -1)
            {
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmddoc = new SqlCommand($@"delete from POL_DOCUMENTS_OLD  where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }


            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {                
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),(select idguid from pol_persons where id={Vars.IdP}),(select event_guid from pol_persons where id={Vars.IdP}),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if(PD.prev_persguid != Guid.Empty && PD.prev_fam.Text == "" && PD.prev_im.Text == "")
            {
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmdpers = new SqlCommand($@"delete from POL_PERSONS_OLD where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            if (PD.docexp1.EditValue == null)
            {
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmdpers =
                    new SqlCommand(
                        $@"update pol_documents set docexp = null where event_guid=(select event_guid from pol_persons where id={Vars.IdP}) and main=0",
                        con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }

            string denttst = SPR.MyReader.SELECTVAIN($@"select dend from pol_polises where event_guid=(select event_guid from pol_persons where id = {Vars.IdP})", Properties.Settings.Default.DocExchangeConnectionString);
            string id = SPR.MyReader.SELECTVAIN($@"select id from pol_polises where event_guid=(select event_guid from pol_persons where id = {Vars.IdP})", Properties.Settings.Default.DocExchangeConnectionString);
            if (string.IsNullOrEmpty(id))
            {
                SqlConnection conz = new SqlConnection(connectionString);
                SqlCommand cmdpers2 =
                    new SqlCommand(
                        $@"insert into pol_polises(vpolis, spolis, npolis, dbeg, dend, dstop, blank, dreceived, person_guid, event_guid) values(@vpolis, @spolis, @npolis, @dbeg, @dend, @dstop, @blank, @dreceived, " +
                $"(select idguid from pol_persons where id={Vars.IdP}),(select event_guid from pol_persons where id={Vars.IdP}))",
                        conz);
                cmdpers2.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
                cmdpers2.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
                cmdpers2.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
                cmdpers2.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
                if (Convert.ToDateTime(PD.date_end.EditValue) == DateTime.MinValue || PD.date_end.EditValue == null)
                {
                    cmdpers2.Parameters.AddWithValue("@dend", DBNull.Value);
                }
                else
                {
                    cmdpers2.Parameters.AddWithValue("@dend", PD.date_end.EditValue);
                }
                if (Convert.ToDateTime(PD.fakt_prekr.EditValue) == DateTime.MinValue || PD.fakt_prekr.EditValue == null)
                {
                    cmdpers2.Parameters.AddWithValue("@dstop", DBNull.Value);
                }
                else
                {
                    cmdpers2.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue);
                }
                cmdpers2.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
                if (PD.pustoy.IsChecked == true)
                {
                    cmdpers2.Parameters.AddWithValue("@blank", 1);
                }
                else
                {
                    cmdpers2.Parameters.AddWithValue("@blank", 0);
                }

                conz.Open();
                cmdpers2.ExecuteNonQuery();
                conz.Close();
            }
            else
                if (string.IsNullOrEmpty(denttst))
            {
                SqlConnection con1 = new SqlConnection(connectionString);
                SqlCommand cmdpers1 =
                    new SqlCommand(
                        $@"update pol_polises set dend={PD.date_end.EditValue ?? DBNull.Value} where event_guid=(select event_guid from pol_persons where id={Vars.IdP})",
                        con1);
                con1.Open();
                cmdpers1.ExecuteNonQuery();
                con1.Close();
            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void Save_bt2_prp1(MainWindow PD)
        {
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con3 = new SqlConnection(connectionString);
            SqlCommand comm3 = new SqlCommand("update pol_persons set DOP_COMMENT=@DOP_COMMENT,COMMENT=@COMMENT, SROKDOVERENOSTI=@srok_doverenosti,PRIZNAKVIDACHI=@PRIZNAKVIDACHI,DATEVIDACHI=@DATEVIDACHI,ENP=@enp,FAM=@fam,IM=@im,OT=@ot,W=@w,DR=@dr,ss=@ss,mr=@mr,birth_oksm=@boksm,"
                + "c_oksm=@coksm,phone=@phone,email=@email,kateg=@kateg,dost=@dost,ddeath=@ddeath,rperson_guid=@rpguid, mo=@mo, dstart=@date_mo where id=@id_p " +

                "update pol_addresses set fias_l1=@FIAS_L1,fias_l3=@FIAS_L3,fias_l4=@FIAS_L4,fias_l6=@FIAS_L6,fias_l7=@FIAS_L7,fias_l90=@FIAS_L90," +
                "fias_l91=@FIAS_L91, dom=@DOM,korp=@KORP,ext=@EXT,kv=@KV,house_guid=@HOUSE_GUID where idguid=(select ADDR_GUID from pol_relation_addr_pers where addres_g=1 and person_guid=(select idguid from pol_persons where id=@id_p)) and " +
                "event_guid=(select event_guid from pol_persons where id =@id_p) " +

                "update pol_addresses set fias_l1=@FIAS_L1_1,fias_l3=@FIAS_L3_1,fias_l4=@FIAS_L4_1,fias_l6=@FIAS_L6_1,fias_l7=@FIAS_L7_1,fias_l90=@FIAS_L90_1," +
                "fias_l91=@FIAS_L91_1,  dom=@DOM_1,korp=@KORP_1,ext=@EXT_1,kv=@KV_1,house_guid=@HOUSE_GUID_1 where idguid=(select ADDR_GUID from pol_relation_addr_pers where addres_p=1 and person_guid=(select idguid from pol_persons where id=@id_p)) and " +
                "event_guid=(select event_guid from pol_persons where id =@id_p) " +

                "update pol_relation_addr_pers SET bomg=@bomg,addres_g=@addr_g,addres_p=@addr_p,dreg=@dreg where addres_g=1 and person_guid=(select idguid from pol_persons where id=@id_p) " +

                "update pol_relation_addr_pers SET bomg=@bomg,addres_g=@addr_g1,addres_p=@addr_p1,dreg=@dreg where addres_p=1 and person_guid=(select idguid from pol_persons where id=@id_p) " +

                "update pol_documents set oksm=@oksm,doctype=@doctype,docser=@docser,docnum=@docnam,docdate=@docdate,docexp=@docexp,name_vp=@name_vp,name_vp_code=@vp_code, " +
                "docmr=@docmr where person_guid=(select idguid from pol_persons where id=@id_p)" +

                "update pol_polises set vpolis=@vpolis,spolis=@spolis,npolis=@npolis,dbeg=@dbeg,dend=@dend,dstop=@dstop,blank=@blank,dreceived=@dreceived " +
                "where person_guid=(select idguid from pol_persons where id=@id_p)" +

               "update pol_personsb set photo=@photo1 where person_guid=(select idguid from pol_persons where id =@id_p) and type=3" +

               "update pol_events set unload=0,tip_op=@tip_op where person_guid=(select idguid from pol_persons where id=@id_p)", con3);

            comm3.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm3.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm3.Parameters.AddWithValue("@im", PD.im.Text);
            comm3.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm3.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm3.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm3.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm3.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm3.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm3.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm3.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
            comm3.Parameters.AddWithValue("@COMMENT", PD.comments.Text);
            comm3.Parameters.AddWithValue("@DOP_COMMENT", PD.Dop_comments.Text);
            if (PD.priznak_vidachi.IsChecked == true)
            {
                comm3.Parameters.AddWithValue("@PRIZNAKVIDACHI", "1");
            }
            else
            {
                comm3.Parameters.AddWithValue("@PRIZNAKVIDACHI", "0");
            }
            comm3.Parameters.AddWithValue("@DATEVIDACHI", PD.date_vidachi.DateTime);
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                comm3.Parameters.AddWithValue("@mo", PD.mo_cmb.EditValue.ToString());
            }
            else
            {
                comm3.Parameters.AddWithValue("@mo", "");
            }
            //if (Convert.ToDateTime(date_mo.EditValue) == DateTime.MinValue || date_mo.EditValue == null)
            //{
            //    comm3.Parameters.AddWithValue("@date_mo", DBNull.Value);
            //}
            //else
            //{
            //    comm3.Parameters.AddWithValue("@date_mo", date_mo.EditValue);
            //}
            comm3.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
            if (Convert.ToDateTime(PD.ddeath.EditValue) == DateTime.MinValue || PD.ddeath.EditValue == null)
            {
                comm3.Parameters.AddWithValue("@ddeath", DBNull.Value);
            }
            else
            {
                comm3.Parameters.AddWithValue("@ddeath", PD.ddeath.DateTime);
            }

            comm3.Parameters.AddWithValue("@oksm", PD.str_vid.EditValue.ToString());
            comm3.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm3.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm3.Parameters.AddWithValue("@email", PD.email.Text);
            comm3.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            comm3.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm3.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm3.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm3.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            if (Convert.ToDateTime(PD.date_end.EditValue) == DateTime.MinValue || PD.date_end.EditValue == null)
            {
                comm3.Parameters.AddWithValue("@dend", DBNull.Value);
            }
            else
            {
                comm3.Parameters.AddWithValue("@dend", PD.date_end.EditValue);
            }
            if (Convert.ToDateTime(PD.fakt_prekr.EditValue) == DateTime.MinValue || PD.fakt_prekr.EditValue == null)
            {
                comm3.Parameters.AddWithValue("@dstop", DBNull.Value);
            }
            else
            {
                comm3.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue);
            }
            comm3.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            if (PD.pustoy.IsChecked == true)
            {
                comm3.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm3.Parameters.AddWithValue("@blank", 0);
            }


            comm3.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm3.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm3.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm3.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm3.Parameters.AddWithValue("@docexp", PD.docexp.EditValue ?? DBNull.Value);
            comm3.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm3.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm3.Parameters.AddWithValue("@docmr", PD.mr2.Text);

            comm3.Parameters.AddWithValue("@id_p", Vars.IdP);
            if (PD.s == "" || PD.s == null)
            {
                comm3.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm3.Parameters.AddWithValue("@dost", PD.s);
            }

            comm3.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm3.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm3.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm3.Parameters.AddWithValue("@FIAS_L4", Guid.Empty);
            }
            else
            {
                comm3.Parameters.AddWithValue("@FIAS_L4", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm3.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm3.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm3.Parameters.AddWithValue("@FIAS_L7", Guid.Empty);
                comm3.Parameters.AddWithValue("@FIAS_L90", Guid.Empty);
                comm3.Parameters.AddWithValue("@FIAS_L91", Guid.Empty);
            }
            else
            {
                comm3.Parameters.AddWithValue("@FIAS_L7", PD.fias.reg_ul.EditValue);
                comm3.Parameters.AddWithValue("@FIAS_L90", PD.fias.reg_ul.EditValue);
                comm3.Parameters.AddWithValue("@FIAS_L91", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm3.Parameters.AddWithValue("@HOUSE_GUID", Guid.Empty);
            }
            else
            {
                comm3.Parameters.AddWithValue("@HOUSE_GUID", PD.fias.reg_dom.EditValue);
            }
            comm3.Parameters.AddWithValue("@DOM", PD.domsplit);
            comm3.Parameters.AddWithValue("@KORP", PD.fias.reg_korp.Text);
            comm3.Parameters.AddWithValue("@EXT", PD.fias.reg_str.Text);
            comm3.Parameters.AddWithValue("@KV", PD.fias.reg_kv.Text);
            if (PD.fias.bomj.IsChecked == true)
            {
                comm3.Parameters.AddWithValue("@bomg", 1);
                comm3.Parameters.AddWithValue("@addr_g", 0);

            }
            else
            {
                comm3.Parameters.AddWithValue("@bomg", 0);
                comm3.Parameters.AddWithValue("@addr_g", 1);
                if (PD.fias1.sovp_addr.IsChecked == true)
                {
                    comm3.Parameters.AddWithValue("@addr_p", 1);
                    comm3.Parameters.AddWithValue("@addr_p1", 1);
                    comm3.Parameters.AddWithValue("@addr_g1", 1);
                }
                else
                {
                    comm3.Parameters.AddWithValue("@addr_p", 0);
                    comm3.Parameters.AddWithValue("@addr_p1", 1);
                    comm3.Parameters.AddWithValue("@addr_g1", 0);
                }

            }
            if (PD.fias.reg_dr.EditValue == null)
            {
                comm3.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm3.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }
            if (PD.fias1.sovp_addr.IsChecked == true)
            {
                comm3.Parameters.AddWithValue("@FIAS_L1_1", PD.fias.reg.EditValue);
                if (PD.fias.reg_rn.EditValue == null)
                {
                    comm3.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
                }
                else
                {
                    comm3.Parameters.AddWithValue("@FIAS_L3_1", PD.fias.reg_rn.EditValue);
                }
                if (PD.fias.reg_town.EditValue == null)
                {
                    comm3.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
                }
                else
                {
                    comm3.Parameters.AddWithValue("@FIAS_L4_1", PD.fias.reg_town.EditValue);
                }
                if (PD.fias.reg_np.EditValue == null)
                {
                    comm3.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
                }
                else
                {
                    comm3.Parameters.AddWithValue("@FIAS_L6_1", PD.fias.reg_np.EditValue);
                }
                if (PD.fias.reg_ul.EditValue == null)
                {
                    comm3.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                    comm3.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                    comm3.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
                }
                else
                {
                    comm3.Parameters.AddWithValue("@FIAS_L7_1", PD.fias.reg_ul.EditValue);
                    comm3.Parameters.AddWithValue("@FIAS_L90_1", PD.fias.reg_ul.EditValue);
                    comm3.Parameters.AddWithValue("@FIAS_L91_1", PD.fias.reg_ul.EditValue);
                }
                if (PD.fias.reg_dom.EditValue == null)
                {
                    comm3.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
                }
                else
                {
                    comm3.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias.reg_dom.EditValue);
                }
                comm3.Parameters.AddWithValue("@DOM_1", PD.domsplit);
                comm3.Parameters.AddWithValue("@KORP_1", PD.fias.reg_korp.Text);
                comm3.Parameters.AddWithValue("@EXT_1", PD.fias.reg_str.Text);
                comm3.Parameters.AddWithValue("@KV_1", PD.fias.reg_kv.Text);
            }
            else
            {
                comm3.Parameters.AddWithValue("@FIAS_L1_1", PD.fias1.reg1.EditValue);
                if (PD.fias1.reg_rn1.EditValue == null)
                {
                    comm3.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
                }
                else
                {
                    comm3.Parameters.AddWithValue("@FIAS_L3_1", PD.fias1.reg_rn1.EditValue);
                }
                if (PD.fias1.reg_town1.EditValue == null)
                {
                    comm3.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
                }
                else
                {
                    comm3.Parameters.AddWithValue("@FIAS_L4_1", PD.fias1.reg_town1.EditValue);
                }
                if (PD.fias1.reg_np1.EditValue == null)
                {
                    comm3.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
                }
                else
                {
                    comm3.Parameters.AddWithValue("@FIAS_L6_1", PD.fias1.reg_np1.EditValue);
                }
                if (PD.fias1.reg_ul1.EditValue == null)
                {
                    comm3.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                    comm3.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                    comm3.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
                }
                else
                {
                    comm3.Parameters.AddWithValue("@FIAS_L7_1", PD.fias1.reg_ul1.EditValue);
                    comm3.Parameters.AddWithValue("@FIAS_L90_1", PD.fias1.reg_ul1.EditValue);
                    comm3.Parameters.AddWithValue("@FIAS_L91_1", PD.fias1.reg_ul1.EditValue);
                }
                if (PD.fias1.reg_dom1.EditValue == null)
                {
                    comm3.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
                }
                else
                {
                    comm3.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias1.reg_dom1.EditValue);
                }
                comm3.Parameters.AddWithValue("@DOM_1", PD.domsplit1);
                comm3.Parameters.AddWithValue("@KORP_1", PD.fias1.reg_korp1.Text);
                comm3.Parameters.AddWithValue("@EXT_1", PD.fias1.reg_str1.Text);
                comm3.Parameters.AddWithValue("@KV_1", PD.fias1.reg_kv1.Text);
            }




            if (PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "")
            {
                comm3.Parameters.AddWithValue("@photo1", "");
            }
            else
            {
                comm3.Parameters.AddWithValue("@photo1", Convert.ToBase64String((byte[])PD.zl_podp.EditValue));
            }
            if (PD.pers_grid_2.SelectedItems.Count == 0 && PD.fam1.Text == "")
            {
                comm3.Parameters.AddWithValue("@rpguid", Guid.Empty);
            }
            else if (PD.pers_grid_2.SelectedItems.Count == 0 && PD.fam1.Text != "")
            {
                SqlCommand comm13 = new SqlCommand(@"select rperson_guid from pol_persons where id=@id_p", con3);
                comm13.Parameters.AddWithValue("@id_p", Vars.IdP);
                con3.Open();
                Guid rpguid1 = (Guid)comm13.ExecuteScalar();
                con3.Close();

                if (rpguid1 == Guid.Empty)
                {
                    var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                    SqlConnection con = new SqlConnection(connectionString1);
                    SqlCommand comm32 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                         " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                         "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                         " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                         "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                         "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                         " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                    comm32.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                    comm32.Parameters.AddWithValue("@im1", PD.im1.Text);
                    comm32.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                    comm32.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                    comm32.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                    comm32.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                    comm32.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                    comm32.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                    comm32.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                    if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                    {
                        comm32.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                    }
                    else
                    {
                        comm32.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                    }
                    comm32.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                    con3.Open();
                    Guid rpguid2 = (Guid)comm32.ExecuteScalar();

                    con3.Close();
                    rpguid1 = rpguid2;
                }
                else
                {

                }
                comm3.Parameters.AddWithValue("@rpguid", rpguid1);
            }
            else if (PD.pers_grid_2.SelectedItems.Count != 0 && PD.fam1.Text != "")
            {
                comm3.Parameters.AddWithValue("@rpguid", PD.pers_grid_2.GetFocusedRowCellValue("IDGUID").ToString());
            }


            con3.Open();
            comm3.ExecuteNonQuery();
            con3.Close();

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid,active,main)
                                values(newid(),(select idguid from pol_persons where id={Vars.IdP}),'{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_oplist where person_guid=(select idguid from pol_persons where id={Vars.IdP})),0,0)
 update pol_documents set prevdocguid=(select idguid from pol_documents where id=SCOPE_IDENTITY()) where person_guid=(select idguid from pol_persons where id={Vars.IdP}) and active=1 and main=1", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}', active=0,main=0 where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            Item_Saved();
            PersData_Default(PD);
        }

        public static void SaveDD_bt1_b0_s0_p2_sp1(MainWindow PD)
        {
            string module = "SaveDD_bt1_b0_s0_p2_sp1";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand("insert into pol_persons (IDGUID,ENP,FAM,IM,OT,W,DR,mr,BIRTH_OKSM,C_OKSM,ss,phone,email,kateg,dost,rperson_guid)" +
                                " VALUES (newid(),@enp,@fam,@im,@ot,@w,@dr,@mr,@boksm,@coksm,@ss,@phone,@email,@kateg,@dost,@rpguid)" +

                                "update pol_persons set parentguid='00000000-0000-0000-0000-000000000000' where id=SCOPE_IDENTITY()" +



                                "insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,dorder,agent)" +
                                " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=SCOPE_IDENTITY())," +
                                "(select rperson_guid from pol_persons where id=SCOPE_IDENTITY()),@prelation,@rsmo,@rpolis,@fpolis,@dorder,@agent)" +
                                 "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where idguid=(select person_guid from pol_events where id=SCOPE_IDENTITY())" +

                                "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                                "values(newid(),(select person_guid from pol_events where id=SCOPE_IDENTITY()), @oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                                "(select idguid from pol_events where id=SCOPE_IDENTITY()))" +

                                "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,DOCEXP,name_vp,main,event_guid)" +
                                "values(newid(),(select person_guid from POL_DOCUMENTS where id=SCOPE_IDENTITY()),@oksm,@doctype1,@docser1,@docnam1,@docdate1,@docexp1,@name_vp1,0," +
                                "(select event_guid from pol_documents where id=SCOPE_IDENTITY()))" +

                                " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                                "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1)," +
                                "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1 and livestatus=1),@FIAS_L1,@FIAS_L3,@FIAS_L4,@FIAS_L6,@FIAS_L90,@FIAS_L91,@FIAS_L7,@DOM,@KORP,@EXT,@KV, " +
                                "(select event_guid from pol_documents where id=SCOPE_IDENTITY()),@HOUSE_GUID)" +

                                "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                                " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                                ", @bomg,1,@dreg,0,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                                " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                                "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7_1 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7_1 and actstatus=1)," +
                                "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1_1 and livestatus=1),@FIAS_L1_1,@FIAS_L3_1,@FIAS_L4_1,@FIAS_L6_1,@FIAS_L90_1,@FIAS_L91_1,@FIAS_L7_1,@DOM_1,@KORP_1,@EXT_1,@KV_1, " +
                                "(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()),@HOUSE_GUID_1)" +

                                "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                                " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                                ", @bomg,0,@dreg1,1,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                                "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select person_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY() ),2,(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()))" +


                                "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select person_guid from pol_personsb where id=SCOPE_IDENTITY() ),3,(select event_guid from pol_personsb where id=SCOPE_IDENTITY() ))" +

                                "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_personsb where id=SCOPE_IDENTITY() )," +
                                " (select idguid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1)," +
                                "(select event_guid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1))" +

                                "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() )," +
                                " (select idguid from pol_documents where person_guid= (select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() ) and main=0)," +
                                "(select event_guid from pol_documents where person_guid= (select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() ) and main=0))" +

                              "update pol_polises set dbeg=@dbeg,dend=@dend,dstop=@dstop,blank=0,dreceived=@dreceived,person_guid= " +
                                "(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),event_guid=(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY())" +
                                "where spolis=@spolis and npolis=@npolis " +

                                "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                                "(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()))" +
                                "select person_guid from pol_oplist where id=SCOPE_IDENTITY()", con);





            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);

            if (PD.snils.Text == "")
            {
                comm.Parameters.AddWithValue("@ss", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            }

            if (PD.phone.Text == "")
            {
                comm.Parameters.AddWithValue("@phone", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            }
            if (PD.email.Text == "")
            {
                comm.Parameters.AddWithValue("@email", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@email", PD.email.Text);
            }

            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            comm.Parameters.AddWithValue("@dend", PD.date_end.DateTime);
            comm.Parameters.AddWithValue("@dorder", Vars.DateVisit);
            comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue ?? DBNull.Value);

            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));

            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }
            if (PD.kat_zl.EditValue != null)
            {
                comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            }
            else
            {
                MessageBox.Show("Выберите категогрию ЗЛ!");
            }
            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@doctype1", PD.ddtype.EditValue);
            comm.Parameters.AddWithValue("@docser1", PD.ddser.Text);
            comm.Parameters.AddWithValue("@docnam1", PD.ddnum.Text);
            comm.Parameters.AddWithValue("@docdate1", PD.dddate.DateTime);
            comm.Parameters.AddWithValue("@name_vp1", PD.ddkemv.Text);
            comm.Parameters.AddWithValue("@docexp1", PD.docexp1.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", PD.fias.reg_dom.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV", PD.fias.reg_kv.Text);
            if (PD.fias.bomj.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@bomg", 1);
                comm.Parameters.AddWithValue("@addr_g", 1);
                comm.Parameters.AddWithValue("@addr_p", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@bomg", 0);
                comm.Parameters.AddWithValue("@addr_g", 1);
            }

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }
            comm.Parameters.AddWithValue("@dreg1", PD.fias1.reg_dr1.DateTime);

            if (PD.fias1.sovp_addr.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@addr_p", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@addr_p", 0);
            }

            comm.Parameters.AddWithValue("@FIAS_L1_1", PD.fias1.reg1.EditValue);
            if (PD.fias1.reg_rn1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias1.reg_rn1.EditValue);
            }
            if (PD.fias1.reg_town1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", PD.fias1.reg_town1.EditValue);
            }

            if (PD.fias1.reg_np1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias1.reg_np1.EditValue);
            }
            if (PD.fias1.reg_ul1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", PD.fias1.reg_ul1.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90_1", PD.fias1.reg_ul1.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91_1", PD.fias1.reg_ul1.EditValue);
            }
            if (PD.fias1.reg_dom1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias1.reg_dom1.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM_1", PD.domsplit1);
            comm.Parameters.AddWithValue("@KORP_1", PD.fias1.reg_korp1.Text);
            comm.Parameters.AddWithValue("@EXT_1", PD.fias1.reg_str1.Text);
            comm.Parameters.AddWithValue("@KV_1", PD.fias1.reg_kv1.Text);

            comm.Parameters.AddWithValue("@screen", PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_photo.EditValue));

            comm.Parameters.AddWithValue("@screen1", PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_podp.EditValue));

            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where id={Vars.IdP})", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where idguid='{perguid}')", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),'{perguid}','{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where idguid='{perguid}'))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),'{perguid}',(select event_guid from pol_persons where idguid='{perguid}'),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void SaveDD_bt1_b0_s0_p2_sp0(MainWindow PD)
        {
            string module = "SaveDD_bt1_b0_s0_p2_sp0";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand("insert into pol_persons (IDGUID,ENP,FAM,IM,OT,W,DR,mr,BIRTH_OKSM,C_OKSM,ss,phone,email,kateg,dost,rperson_guid)" +
                                 " VALUES (newid(),@enp,@fam,@im,@ot,@w,@dr,@mr,@boksm,@coksm,@ss,@phone,@email,@kateg,@dost,@rpguid)" +

                                 "update pol_persons set parentguid='00000000-0000-0000-0000-000000000000' where id=SCOPE_IDENTITY()" +



                                 "insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,agent)" +
                                 " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=SCOPE_IDENTITY())," +
                                 "(select rperson_guid from pol_persons where id=SCOPE_IDENTITY()),@prelation,@rsmo,@rpolis,@fpolis,@agent)" +
                                  "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where idguid=(select person_guid from pol_events where id=SCOPE_IDENTITY())" +

                                 "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                                 "values(newid(),(select person_guid from pol_events where id=SCOPE_IDENTITY()), @oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                                 "(select idguid from pol_events where id=SCOPE_IDENTITY()))" +

                                 "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,DOCEXP,name_vp,main,event_guid)" +
                                "values(newid(),(select person_guid from POL_DOCUMENTS where id=SCOPE_IDENTITY()),@oksm,@doctype1,@docser1,@docnam1,@docdate1,@docexp1,@name_vp1,0," +
                                "(select event_guid from pol_documents where id=SCOPE_IDENTITY()))" +

                                 " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                                "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1)," +
                                "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1 and livestatus=1),@FIAS_L1,@FIAS_L3,@FIAS_L4,@FIAS_L6,@FIAS_L90,@FIAS_L91,@FIAS_L7,@DOM,@KORP,@EXT,@KV, " +
                                "(select event_guid from pol_documents where id=SCOPE_IDENTITY()),@HOUSE_GUID)" +

                                 "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                                 " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                                 ", @bomg,1,@dreg,0,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                                 " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                                "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7_1 and actstatus=1 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7_1 and actstatus=1 and actstatus=1)," +
                                "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1_1 and livestatus=1),@FIAS_L1_1,@FIAS_L3_1,@FIAS_L4_1,@FIAS_L6_1,@FIAS_L90_1,@FIAS_L91_1,@FIAS_L7_1,@DOM_1,@KORP_1,@EXT_1,@KV_1, " +
                                "(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()),@HOUSE_GUID_1)" +

                                 "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                                 " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) )," +
                                 "(select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                                 ", @bomg,0,@dreg1,1,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                                 "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select person_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY() ),2,(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()))" +


                                "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select person_guid from pol_personsb where id=SCOPE_IDENTITY() ),3,(select event_guid from pol_personsb where id=SCOPE_IDENTITY() ))" +

                                 "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_personsb where id=SCOPE_IDENTITY() )," +
                                 " (select idguid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1)," +
                                 "(select event_guid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1))" +

                                 "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() )," +
                                 " (select idguid from pol_documents where person_guid= (select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() ) and main=0)," +
                                 "(select event_guid from pol_documents where person_guid= (select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() ) and main=0))" +

                                 "insert into pol_polises (vpolis,spolis,npolis,dbeg,dend,dstop,blank,dreceived,person_guid,event_guid) values (@vpolis,@spolis,@npolis,@dbeg,@dend,@dstop,@blank,@dreceived, " +
                                 "(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()) ) " +

                                 "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                                "(select event_guid from pol_polises where id=SCOPE_IDENTITY()),(select person_guid from pol_polises where id=SCOPE_IDENTITY()))" +
                                "select person_guid from pol_oplist where id=SCOPE_IDENTITY()", con);







            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);
            if (PD.snils.Text == "")
            {
                comm.Parameters.AddWithValue("@ss", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            }

            if (PD.phone.Text == "")
            {
                comm.Parameters.AddWithValue("@phone", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            }
            if (PD.email.Text == "")
            {
                comm.Parameters.AddWithValue("@email", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@email", PD.email.Text);
            }
            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            if (Convert.ToDateTime(PD.date_end.EditValue) == DateTime.MinValue)
            {
                comm.Parameters.AddWithValue("@dend", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dend", PD.date_end.EditValue);
            }
            if (Convert.ToDateTime(PD.fakt_prekr.EditValue) == DateTime.MinValue)
            {
                comm.Parameters.AddWithValue("@dstop", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue);
            }
            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));

            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }
            if (PD.kat_zl.EditValue != null)
            {
                comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            }
            else
            {
                MessageBox.Show("Выберите категогрию ЗЛ!");
            }
            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@doctype1", PD.ddtype.EditValue);
            comm.Parameters.AddWithValue("@docser1", PD.ddser.Text);
            comm.Parameters.AddWithValue("@docnam1", PD.ddnum.Text);
            comm.Parameters.AddWithValue("@docdate1", PD.dddate.DateTime);
            comm.Parameters.AddWithValue("@name_vp1", PD.ddkemv.Text);
            comm.Parameters.AddWithValue("@docexp1", PD.docexp1.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", PD.fias.reg_dom.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV", PD.fias.reg_kv.Text);
            if (PD.fias.bomj.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@bomg", 1);
                comm.Parameters.AddWithValue("@addr_g", 0);
            }
            else
            {
                comm.Parameters.AddWithValue("@bomg", 0);
                comm.Parameters.AddWithValue("@addr_g", 1);
            }

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }
            comm.Parameters.AddWithValue("@dreg1", PD.fias1.reg_dr1.DateTime);


            comm.Parameters.AddWithValue("@addr_p", 0);
            comm.Parameters.AddWithValue("@FIAS_L1_1", PD.fias1.reg1.EditValue);
            if (PD.fias1.reg_rn1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias1.reg_rn1.EditValue);
            }
            if (PD.fias1.reg_town1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", PD.fias1.reg_town1.EditValue);
            }

            if (PD.fias1.reg_np1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias1.reg_np1.EditValue);
            }
            if (PD.fias1.reg_ul1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", PD.fias1.reg_ul1.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90_1", PD.fias1.reg_ul1.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91_1", PD.fias1.reg_ul1.EditValue);
            }
            if (PD.fias1.reg_dom1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias1.reg_dom1.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM_1", PD.domsplit1);
            comm.Parameters.AddWithValue("@KORP_1", PD.fias1.reg_korp1.Text);
            comm.Parameters.AddWithValue("@EXT_1", PD.fias1.reg_str1.Text);
            comm.Parameters.AddWithValue("@KV_1", PD.fias1.reg_kv1.Text);
            //}
            comm.Parameters.AddWithValue("@screen", PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_photo.EditValue));

            comm.Parameters.AddWithValue("@screen1", PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_podp.EditValue));

            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where id={Vars.IdP})", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where idguid='{perguid}')", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),'{perguid}','{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where idguid='{perguid}'))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),'{perguid}',(select event_guid from pol_persons where idguid='{perguid}'),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void SaveDD_bt1_b0_s0_p13(MainWindow PD)
        {
            string module = "SaveDD_bt1_b0_s0_p13";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand("insert into pol_persons (IDGUID,ENP,FAM,IM,OT,W,DR,mr,BIRTH_OKSM,C_OKSM,ss,phone,email,kateg,dost,rperson_guid)" +
                                 " VALUES (newid(),@enp,@fam,@im,@ot,@w,@dr,@mr,@boksm,@coksm,@ss,@phone,@email,@kateg,@dost,@rpguid)" +

                                 "update pol_persons set parentguid='00000000-0000-0000-0000-000000000000' where id=SCOPE_IDENTITY()" +



                                 "insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,agent)" +
                                 " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=SCOPE_IDENTITY())," +
                                 "(select rperson_guid from pol_persons where id=SCOPE_IDENTITY()),@prelation,@rsmo,@rpolis,@fpolis,@agent)" +
                                  "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where idguid=(select person_guid from pol_events where id=SCOPE_IDENTITY())" +

                                 "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                                 "values(newid(),(select person_guid from pol_events where id=SCOPE_IDENTITY()), @oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                                 "(select idguid from pol_events where id=SCOPE_IDENTITY()))" +

                                 "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,DOCEXP,name_vp,main,event_guid)" +
                                "values(newid(),(select person_guid from POL_DOCUMENTS where id=SCOPE_IDENTITY()),@oksm,@doctype1,@docser1,@docnam1,@docdate1,@docexp1,@name_vp1,0," +
                                "(select event_guid from pol_documents where id=SCOPE_IDENTITY()))" +

                                 " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                                "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1)," +
                                "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1 and livestatus=1),@FIAS_L1,@FIAS_L3,@FIAS_L4,@FIAS_L6,@FIAS_L90,@FIAS_L91,@FIAS_L7,@DOM,@KORP,@EXT,@KV, " +
                                "(select event_guid from pol_documents where id=SCOPE_IDENTITY()),@HOUSE_GUID)" +

                                 "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                                 " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                                 ", @bomg,1,@dreg,0,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                                 " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                                "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7_1 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7_1 and actstatus=1)," +
                                "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1_1 and livestatus=1),@SUBJ,@FIAS_L1_1,@FIAS_L3_1,@FIAS_L4_1,@FIAS_L6_1,@FIAS_L90_1,@FIAS_L91_1,@FIAS_L7_1,@DOM_1,@KORP_1,@EXT_1,@KV_1, " +
                                "(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()),@HOUSE_GUID_1)" +

                                 "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                                 " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                                 ", @bomg,0,@dreg1,1,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                                 "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select person_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY() ),2,(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()))" +


                                "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select person_guid from pol_personsb where id=SCOPE_IDENTITY() ),3,(select event_guid from pol_personsb where id=SCOPE_IDENTITY() ))" +

                                 "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_personsb where id=SCOPE_IDENTITY() )," +
                                 " (select idguid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1)," +
                                 "(select event_guid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1))" +
                                 "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() )," +
                                 " (select idguid from pol_documents where person_guid= (select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() ) and main=0)," +
                                 "(select event_guid from pol_documents where person_guid= (select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() ) and main=0))" +

                                 "insert into pol_polises (vpolis,spolis,npolis,dbeg,dend,blank,dreceived,person_guid,event_guid) values (@vpolis,@spolis,@npolis,@dbeg,@dend,@blank,@dreceived, " +
                                 "(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()) ) " +

                                 "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                                "(select event_guid from pol_polises where id=SCOPE_IDENTITY()),(select person_guid from pol_polises where id=SCOPE_IDENTITY()))" +
                                "select person_guid from pol_oplist where id=SCOPE_IDENTITY()", con);




            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm.Parameters.AddWithValue("@email", PD.email.Text);
            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            comm.Parameters.AddWithValue("@dend", PD.date_end.DateTime);
            comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);

            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }
            if (PD.kat_zl.EditValue != null)
            {
                comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            }
            else
            {
                MessageBox.Show("Выберите категогрию ЗЛ!");
            }
            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@doctype1", PD.ddtype.EditValue);
            comm.Parameters.AddWithValue("@docser1", PD.ddser.Text);
            comm.Parameters.AddWithValue("@docnam1", PD.ddnum.Text);
            comm.Parameters.AddWithValue("@docdate1", PD.dddate.DateTime);
            comm.Parameters.AddWithValue("@name_vp1", PD.ddkemv.Text);
            comm.Parameters.AddWithValue("@docexp1", PD.docexp1.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", PD.fias.reg_dom.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV", PD.fias.reg_kv.Text);
            if (PD.fias.bomj.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@bomg", 1);
                comm.Parameters.AddWithValue("@addr_g", 0);
            }
            else
            {
                comm.Parameters.AddWithValue("@bomg", 0);
                comm.Parameters.AddWithValue("@addr_g", 1);
            }

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }
            comm.Parameters.AddWithValue("@dreg1", PD.fias1.reg_dr1.DateTime);


            comm.Parameters.AddWithValue("@addr_p", 0);
            comm.Parameters.AddWithValue("@FIAS_L1_1", PD.fias1.reg1.EditValue);
            if (PD.fias1.reg_rn1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias1.reg_rn1.EditValue);
            }
            if (PD.fias1.reg_town1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", PD.fias1.reg_town1.EditValue);
            }

            if (PD.fias1.reg_np1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias1.reg_np1.EditValue);
            }
            if (PD.fias1.reg_ul1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", PD.fias1.reg_ul1.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90_1", PD.fias1.reg_ul1.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91_1", PD.fias1.reg_ul1.EditValue);
            }
            if (PD.fias1.reg_dom1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias1.reg_dom1.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM_1", PD.domsplit1);
            comm.Parameters.AddWithValue("@KORP_1", PD.fias1.reg_korp1.Text);
            comm.Parameters.AddWithValue("@EXT_1", PD.fias1.reg_str1.Text);
            comm.Parameters.AddWithValue("@KV_1", PD.fias1.reg_kv1.Text);
            //}
            comm.Parameters.AddWithValue("@screen", PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_photo.EditValue));

            comm.Parameters.AddWithValue("@screen1", PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_podp.EditValue));

            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' ", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where idguid='{perguid}')", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),'{perguid}','{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where idguid='{perguid}'))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),'{perguid}',(select event_guid from pol_persons where idguid='{perguid}'),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void SaveDD_bt1_b1_s0_p2_sp0(MainWindow PD)
        {
            string module = "SaveDD_bt1_b1_s0_p2_sp0";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand("insert into pol_persons (IDGUID,ENP,FAM,IM,OT,W,DR,mr,BIRTH_OKSM,C_OKSM,ss,phone,email,kateg,dost,rperson_guid)" +
                            " VALUES (newid(),@enp,@fam,@im,@ot,@w,@dr,@mr,@boksm,@coksm,@ss,@phone,@email,@kateg,@dost,@rpguid)" +

                            "update pol_persons set parentguid='00000000-0000-0000-0000-000000000000' where id=SCOPE_IDENTITY()" +



                            "insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,agent)" +
                            " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=SCOPE_IDENTITY())," +
                            "(select rperson_guid from pol_persons where id=SCOPE_IDENTITY()),@prelation,@rsmo,@rpolis,@fpolis,@agent)" +
                             "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where idguid=(select person_guid from pol_events where id=SCOPE_IDENTITY())" +

                            "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                            "values(newid(),(select person_guid from pol_events where id=SCOPE_IDENTITY()), @oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                            "(select idguid from pol_events where id=SCOPE_IDENTITY()))" +

                            "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,DOCEXP,name_vp,main,event_guid)" +
                                "values(newid(),(select person_guid from POL_DOCUMENTS where id=SCOPE_IDENTITY()),@oksm,@doctype1,@docser1,@docnam1,@docdate1,@docexp1,@name_vp1,0," +
                                "(select event_guid from pol_documents where id=SCOPE_IDENTITY()))" +

                            " insert into pol_addresses (IDGUID,EVENT_GUID,fias_l1,FIAS_L3) " +
                            "values(newid(),(select event_guid from pol_documents where id=SCOPE_IDENTITY()),@FIAS_L1,@FIAS_L3)" +

                            "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,addres_p,dt1,dt2,event_guid)" +
                            " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                            ", 1,0,0,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                            "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select person_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY() ),2,(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()))" +


                            "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select person_guid from pol_personsb where id=SCOPE_IDENTITY() ),3,(select event_guid from pol_personsb where id=SCOPE_IDENTITY() ))" +

                            "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_personsb where id=SCOPE_IDENTITY() )," +
                            " (select idguid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1)," +
                            "(select event_guid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1))" +

                            "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() )," +
                            " (select idguid from pol_documents where person_guid= (select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() ) and main=0)," +
                            "(select event_guid from pol_documents where person_guid= (select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() ) and main=0))" +

                            "insert into pol_polises (vpolis,spolis,npolis,dbeg,dend,blank,dreceived,person_guid,event_guid) values (@vpolis,@spolis,@npolis,@dbeg,@dend,@blank,@dreceived, " +
                            "(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()) ) " +

                            "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                                "(select event_guid from pol_polises where id=SCOPE_IDENTITY()),(select person_guid from pol_polises where id=SCOPE_IDENTITY()))" +
                                "select person_guid from pol_oplist where id=SCOPE_IDENTITY()", con);






            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm.Parameters.AddWithValue("@email", PD.email.Text);
            comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@doctype1", PD.ddtype.EditValue);
            comm.Parameters.AddWithValue("@docser1", PD.ddser.Text);
            comm.Parameters.AddWithValue("@docnam1", PD.ddnum.Text);
            comm.Parameters.AddWithValue("@docdate1", PD.dddate.DateTime);
            comm.Parameters.AddWithValue("@name_vp1", PD.ddkemv.Text);
            comm.Parameters.AddWithValue("@docexp1", PD.docexp1.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            comm.Parameters.AddWithValue("@dend", PD.date_end.DateTime);
            comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);
            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }
            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }

            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }

            if (PD.fias.bomj.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@bomg", 1);
                comm.Parameters.AddWithValue("@addr_g", 0);
            }
            else
            {
                comm.Parameters.AddWithValue("@bomg", 0);
                comm.Parameters.AddWithValue("@addr_g", 1);
            }

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }

            if (PD.fias1.sovp_addr.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@addr_p", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@addr_p", 0);
            }


            if (PD.fias1.reg_rn1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias1.reg_rn1.EditValue);
            }

            if (PD.fias1.reg_np1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias1.reg_np1.EditValue);
            }

            comm.Parameters.AddWithValue("@screen", PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_photo.EditValue));

            comm.Parameters.AddWithValue("@screen1", PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_podp.EditValue));

            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' ", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where idguid='{perguid}')", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),'{perguid}','{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where idguid='{perguid}'))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),'{perguid}',(select event_guid from pol_persons where idguid='{perguid}'),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void SaveDD_bt1_b1_s0_p2_sp1(MainWindow PD)
        {
            string module = "SaveDD_bt1_b1_s0_p2_sp1";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand("insert into pol_persons (IDGUID,ENP,FAM,IM,OT,W,DR,mr,BIRTH_OKSM,C_OKSM,ss,phone,email,kateg,dost,rperson_guid)" +
                            " VALUES (newid(),@enp,@fam,@im,@ot,@w,@dr,@mr,@boksm,@coksm,@ss,@phone,@email,@kateg,@dost,@rpguid)" +

                            "update pol_persons set parentguid='00000000-0000-0000-0000-000000000000' where id=SCOPE_IDENTITY()" +



                            "insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,agent)" +
                            " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=SCOPE_IDENTITY())," +
                            "(select rperson_guid from pol_persons where id=SCOPE_IDENTITY()),@prelation,@rsmo,@rpolis,@fpolis,@agent)" +
                             "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where idguid=(select person_guid from pol_events where id=SCOPE_IDENTITY())" +

                            "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                            "values(newid(),(select person_guid from pol_events where id=SCOPE_IDENTITY()), @oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                            "(select idguid from pol_events where id=SCOPE_IDENTITY()))" +

                            "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,DOCEXP,name_vp,main,event_guid)" +
                                "values(newid(),(select person_guid from POL_DOCUMENTS where id=SCOPE_IDENTITY()),@oksm,@doctype1,@docser1,@docnam1,@docdate1,@docexp1,@name_vp1,0," +
                                "(select event_guid from pol_documents where id=SCOPE_IDENTITY()))" +

                            " insert into pol_addresses (IDGUID,EVENT_GUID,fias_l1,FIAS_L3) " +
                            "values(newid(),(select event_guid from pol_documents where id=SCOPE_IDENTITY()),@FIAS_L1,@FIAS_L3)" +

                            "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,addres_p,dt1,dt2,event_guid)" +
                            " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                            ", 1,0,0,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                            "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select person_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY() ),2,(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()))" +


                            "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select person_guid from pol_personsb where id=SCOPE_IDENTITY() ),3,(select event_guid from pol_personsb where id=SCOPE_IDENTITY() ))" +

                            "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_personsb where id=SCOPE_IDENTITY() )," +
                            " (select idguid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1)," +
                            "(select event_guid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1))" +

                            "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() )," +
                            " (select idguid from pol_documents where person_guid= (select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() ) and main=0)," +
                            "(select event_guid from pol_documents where person_guid= (select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() ) and main=0))" +

                            "insert into pol_polises (vpolis,spolis,npolis,dbeg,dend,blank,dreceived,person_guid,event_guid,dout) values (@vpolis,@spolis,@npolis,@dbeg,@dend,@blank,@dreceived, " +
                "(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),@dout ) " +

                                "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                                "(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()))" +
                                "select person_guid from pol_oplist where id=SCOPE_IDENTITY()", con);






            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm.Parameters.AddWithValue("@email", PD.email.Text);
            comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@doctype1", PD.ddtype.EditValue);
            comm.Parameters.AddWithValue("@docser1", PD.ddser.Text);
            comm.Parameters.AddWithValue("@docnam1", PD.ddnum.Text);
            comm.Parameters.AddWithValue("@docdate1", PD.dddate.DateTime);
            comm.Parameters.AddWithValue("@name_vp1", PD.ddkemv.Text);
            comm.Parameters.AddWithValue("@docexp1", PD.docexp1.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            comm.Parameters.AddWithValue("@dend", PD.date_end.DateTime);
            comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);
            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }
            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }

            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }

            if (PD.fias.bomj.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@bomg", 1);
                comm.Parameters.AddWithValue("@addr_g", 0);
            }
            else
            {
                comm.Parameters.AddWithValue("@bomg", 0);
                comm.Parameters.AddWithValue("@addr_g", 1);
            }

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }

            if (PD.fias1.sovp_addr.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@addr_p", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@addr_p", 0);
            }


            if (PD.fias1.reg_rn1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias1.reg_rn1.EditValue);
            }

            if (PD.fias1.reg_np1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias1.reg_np1.EditValue);
            }

            comm.Parameters.AddWithValue("@screen", PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_photo.EditValue));

            comm.Parameters.AddWithValue("@screen1", PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_podp.EditValue));

            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' ", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where idguid='{perguid}')", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),'{perguid}','{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where idguid='{perguid}'))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),'{perguid}',(select event_guid from pol_persons where idguid='{perguid}'),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void SaveDD_bt1_b1_s0_p13(MainWindow PD)
        {
            string module = "SaveDD_bt1_b1_s0_p13";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand comm = new SqlCommand("insert into pol_persons (IDGUID,ENP,FAM,IM,OT,W,DR,mr,BIRTH_OKSM,C_OKSM,ss,phone,email,kateg,dost,rperson_guid)" +
                            " VALUES (newid(),@enp,@fam,@im,@ot,@w,@dr,@mr,@boksm,@coksm,@ss,@phone,@email,@kateg,@dost,@rpguid)" +

                            "update pol_persons set parentguid='00000000-0000-0000-0000-000000000000' where id=SCOPE_IDENTITY()" +



                            "insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,agent)" +
                            " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=SCOPE_IDENTITY())," +
                            "(select rperson_guid from pol_persons where id=SCOPE_IDENTITY()),@prelation,@rsmo,@rpolis,@fpolis,@agent)" +
                             "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where idguid=(select person_guid from pol_events where id=SCOPE_IDENTITY())" +

                            "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                            "values(newid(),(select person_guid from pol_events where id=SCOPE_IDENTITY()), @oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                            "(select idguid from pol_events where id=SCOPE_IDENTITY()))" +

                            "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,DOCEXP,name_vp,main,event_guid)" +
                                "values(newid(),(select person_guid from POL_DOCUMENTS where id=SCOPE_IDENTITY()),@oksm,@doctype1,@docser1,@docnam1,@docdate1,@docexp1,@name_vp1,0," +
                                "(select event_guid from pol_documents where id=SCOPE_IDENTITY()))" +

                            " insert into pol_addresses (IDGUID,EVENT_GUID,fias_l1,FIAS_L3) " +
                            "values(newid(),(select event_guid from pol_documents where id=SCOPE_IDENTITY()),@FIAS_L1,@FIAS_L3)" +

                            "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,addres_p,dt1,dt2,event_guid)" +
                            " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                            ", 1,0,0,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                            "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select person_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY() ),2,(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()))" +


                            "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select person_guid from pol_personsb where id=SCOPE_IDENTITY() ),3,(select event_guid from pol_personsb where id=SCOPE_IDENTITY() ))" +

                            "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_personsb where id=SCOPE_IDENTITY() )," +
                            " (select idguid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1)," +
                            "(select event_guid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1))" +

                            "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() )," +
                            " (select idguid from pol_documents where person_guid= (select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() ) and main=0)," +
                            "(select event_guid from pol_documents where person_guid= (select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() ) and main=0))" +

                            "insert into pol_polises (vpolis,spolis,npolis,dbeg,dend,blank,dreceived,person_guid,event_guid) values (@vpolis,@spolis,@npolis,@dbeg,@dend,@blank,@dreceived, " +
                            "(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()) ) " +

                            "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                                "(select event_guid from pol_polises where id=SCOPE_IDENTITY()),(select person_guid from pol_polises where id=SCOPE_IDENTITY()))" +
                                "select person_guid from pol_oplist where id=SCOPE_IDENTITY()", con);





            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm.Parameters.AddWithValue("@email", PD.email.Text);
            comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@doctype1", PD.ddtype.EditValue);
            comm.Parameters.AddWithValue("@docser1", PD.ddser.Text);
            comm.Parameters.AddWithValue("@docnam1", PD.ddnum.Text);
            comm.Parameters.AddWithValue("@docdate1", PD.dddate.DateTime);
            comm.Parameters.AddWithValue("@name_vp1", PD.ddkemv.Text);
            comm.Parameters.AddWithValue("@docexp1", PD.docexp1.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            if (Convert.ToDateTime(PD.date_end.EditValue) == DateTime.MinValue)
            {
                comm.Parameters.AddWithValue("@dend", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dend", PD.date_end.EditValue);
            }
            if (Convert.ToDateTime(PD.fakt_prekr.EditValue) == DateTime.MinValue)
            {
                comm.Parameters.AddWithValue("@dstop", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue);
            }
            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);
            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }
            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }

            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }

            if (PD.fias.bomj.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@bomg", 1);
                comm.Parameters.AddWithValue("@addr_g", 0);
            }
            else
            {
                comm.Parameters.AddWithValue("@bomg", 0);
                comm.Parameters.AddWithValue("@addr_g", 1);
            }

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }

            if (PD.fias1.sovp_addr.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@addr_p", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@addr_p", 0);
            }


            if (PD.fias1.reg_rn1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias1.reg_rn1.EditValue);
            }

            if (PD.fias1.reg_np1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias1.reg_np1.EditValue);
            }


            comm.Parameters.AddWithValue("@screen", PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_photo.EditValue));

            comm.Parameters.AddWithValue("@screen1", PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_podp.EditValue));

            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' ", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where idguid='{perguid}')", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),'{perguid}','{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where idguid='{perguid}'))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),'{perguid}',(select event_guid from pol_persons where idguid='{perguid}'),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void SaveDD_bt1_b0_s1_p2_sp1(MainWindow PD)
        {
            string module = "SaveDD_bt1_b0_s1_p2_sp1";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand comm = new SqlCommand("insert into pol_persons (IDGUID,ENP,FAM,IM,OT,W,DR,mr,BIRTH_OKSM,C_OKSM,ss,phone,email,kateg,dost,rperson_guid)" +
                                " VALUES (newid(),@enp,@fam,@im,@ot,@w,@dr,@mr,@boksm,@coksm,@ss,@phone,@email,@kateg,@dost,@rpguid)" +

                                "update pol_persons set parentguid='00000000-0000-0000-0000-000000000000' where id=SCOPE_IDENTITY()" +



                                "insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,agent)" +
                                " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=SCOPE_IDENTITY())," +
                                "(select rperson_guid from pol_persons where id=SCOPE_IDENTITY()),@prelation,@rsmo,@rpolis,@fpolis,@agent)" +
                                 "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where idguid=(select person_guid from pol_events where id=SCOPE_IDENTITY())" +

                                "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                                "values(newid(),(select person_guid from pol_events where id=SCOPE_IDENTITY()), @oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                                "(select idguid from pol_events where id=SCOPE_IDENTITY()))" +

                                "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,DOCEXP,name_vp,main,event_guid)" +
                                    "values(newid(),(select person_guid from POL_DOCUMENTS where id=SCOPE_IDENTITY()),@oksm,@doctype1,@docser1,@docnam1,@docdate1,@docexp1,@name_vp1,0," +
                                    "(select event_guid from pol_documents where id=SCOPE_IDENTITY()))" +

                                " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                                    "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1)," +
                                    "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1 and livestatus=1),@FIAS_L1,@FIAS_L3,@FIAS_L4,@FIAS_L6,@FIAS_L90,@FIAS_L91,@FIAS_L7,@DOM,@KORP,@EXT,@KV, " +
                                    "(select event_guid from pol_documents where id=SCOPE_IDENTITY()),@HOUSE_GUID)" +

                                "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                                " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                                ", @bomg,1,@dreg,1,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                                "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select person_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY() ),2,(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()))" +


                                "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select person_guid from pol_personsb where id=SCOPE_IDENTITY() ),3,(select event_guid from pol_personsb where id=SCOPE_IDENTITY() ))" +

                                "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_personsb where id=SCOPE_IDENTITY() )," +
                                " (select idguid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1)," +
                                "(select event_guid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1))" +

                                "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() )," +
                                " (select idguid from pol_documents where person_guid= (select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() ) and main=0)," +
                                "(select event_guid from pol_documents where person_guid= (select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() ) and main=0))" +


                                 "update pol_polises set dbeg=@dbeg,dend=@dend,dstop=@dstop,blank=0,dreceived=@dreceived,person_guid= " +
                                "(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),event_guid=(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY())" +
                                "where spolis=@spolis and npolis=@npolis " +


                                    "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                                    "(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()))" +
                                    "select person_guid from pol_oplist where id=SCOPE_IDENTITY()", con);





            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm.Parameters.AddWithValue("@email", PD.email.Text);
            comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            if (Convert.ToDateTime(PD.date_end.EditValue) == DateTime.MinValue)
            {
                comm.Parameters.AddWithValue("@dend", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dend", PD.date_end.EditValue);
            }
            if (Convert.ToDateTime(PD.fakt_prekr.EditValue) == DateTime.MinValue)
            {
                comm.Parameters.AddWithValue("@dstop", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue);
            }
            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);

            comm.Parameters.AddWithValue("@doctype1", PD.ddtype.EditValue);
            comm.Parameters.AddWithValue("@docser1", PD.ddser.Text);
            comm.Parameters.AddWithValue("@docnam1", PD.ddnum.Text);
            comm.Parameters.AddWithValue("@docdate1", PD.dddate.DateTime);
            comm.Parameters.AddWithValue("@name_vp1", PD.ddkemv.Text);
            comm.Parameters.AddWithValue("@docexp1", PD.docexp1.EditValue ?? DBNull.Value);

            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }
            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", PD.fias.reg_dom.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV", PD.fias.reg_kv.Text);
            if (PD.fias.bomj.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@bomg", 1);
                comm.Parameters.AddWithValue("@addr_g", 0);
            }
            else
            {
                comm.Parameters.AddWithValue("@bomg", 0);
                comm.Parameters.AddWithValue("@addr_g", 1);
            }

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }

            //if (PD.fias1.sovp_addr.IsChecked == true)
            //{
            comm.Parameters.AddWithValue("@FIAS_L1_1", PD.fias.reg.EditValue);
            comm.Parameters.AddWithValue("@addr_p", 1);
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90_1", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91_1", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias.reg_dom.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM_1", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP_1", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT_1", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV_1", PD.fias.reg_kv.Text);

            comm.Parameters.AddWithValue("@screen", PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_photo.EditValue));

            comm.Parameters.AddWithValue("@screen1", PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_podp.EditValue));

            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' ", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where idguid='{perguid}')", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),'{perguid}','{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where idguid='{perguid}'))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),'{perguid}',(select event_guid from pol_persons where idguid='{perguid}'),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void SaveDD_bt1_b0_s1_p2_sp0(MainWindow PD)
        {
            string module = "Save_bt1_b0_s1_p2_sp0";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand("insert into pol_persons (IDGUID,ENP,FAM,IM,OT,W,DR,mr,BIRTH_OKSM,C_OKSM,ss,phone,email,kateg,dost,rperson_guid)" +
                            " VALUES (newid(),@enp,@fam,@im,@ot,@w,@dr,@mr,@boksm,@coksm,@ss,@phone,@email,@kateg,@dost,@rpguid)" +

                            "update pol_persons set parentguid='00000000-0000-0000-0000-000000000000' where id=SCOPE_IDENTITY()" +



                            "insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,agent)" +
                            " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=SCOPE_IDENTITY())," +
                            "(select rperson_guid from pol_persons where id=SCOPE_IDENTITY()),@prelation,@rsmo,@rpolis,@fpolis,@agent)" +
                             "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where idguid=(select person_guid from pol_events where id=SCOPE_IDENTITY())" +

                            "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                            "values(newid(),(select person_guid from pol_events where id=SCOPE_IDENTITY()),@oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                            "(select idguid from pol_events where id=SCOPE_IDENTITY()))" +

                            "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,DOCEXP,name_vp,main,event_guid)" +
                                "values(newid(),(select person_guid from POL_DOCUMENTS where id=SCOPE_IDENTITY()),@oksm,@doctype1,@docser1,@docnam1,@docdate1,@docexp1,@name_vp1,0," +
                                "(select event_guid from pol_documents where id=SCOPE_IDENTITY()))" +

                            " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                                "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1)," +
                                "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1 and livestatus=1),@FIAS_L1,@FIAS_L3,@FIAS_L4,@FIAS_L6,@FIAS_L90,@FIAS_L91,@FIAS_L7,@DOM,@KORP,@EXT,@KV, " +
                                "(select event_guid from pol_documents where id=SCOPE_IDENTITY()),@HOUSE_GUID)" +

                            "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                            " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                            ", @bomg,1,@dreg,1,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                            "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select person_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY() ),2,(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()))" +


                            "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select person_guid from pol_personsb where id=SCOPE_IDENTITY() ),3,(select event_guid from pol_personsb where id=SCOPE_IDENTITY() ))" +

                            "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_personsb where id=SCOPE_IDENTITY() )," +
                            " (select idguid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1)," +
                            "(select event_guid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1))" +

                            "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() )," +
                            " (select idguid from pol_documents where person_guid= (select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() ) and main=0)," +
                            "(select event_guid from pol_documents where person_guid= (select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() ) and main=0))" +

                            "insert into pol_polises (vpolis,spolis,npolis,dbeg,dend,dstop,blank,dreceived,person_guid,event_guid) values (@vpolis,@spolis,@npolis,@dbeg,@dend,@dstop,@blank,@dreceived, " +
                            "(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()) ) " +

                            "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                                "(select event_guid from pol_polises where id=SCOPE_IDENTITY()),(select person_guid from pol_polises where id=SCOPE_IDENTITY()))" +
                                "select person_guid from pol_oplist where id=SCOPE_IDENTITY()", con);






            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm.Parameters.AddWithValue("@email", PD.email.Text);
            comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);

            comm.Parameters.AddWithValue("@doctype1", PD.ddtype.EditValue);
            comm.Parameters.AddWithValue("@docser1", PD.ddser.Text);
            comm.Parameters.AddWithValue("@docnam1", PD.ddnum.Text);
            comm.Parameters.AddWithValue("@docdate1", PD.dddate.DateTime);
            comm.Parameters.AddWithValue("@name_vp1", PD.ddkemv.Text);
            comm.Parameters.AddWithValue("@docexp1", PD.docexp1.EditValue ?? DBNull.Value);

            if (Convert.ToDateTime(PD.date_end.EditValue) == DateTime.MinValue)
            {
                comm.Parameters.AddWithValue("@dend", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dend", PD.date_end.EditValue);
            }
            if (Convert.ToDateTime(PD.fakt_prekr.EditValue) == DateTime.MinValue)
            {
                comm.Parameters.AddWithValue("@dstop", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue);
            }
            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);
            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }
            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", PD.fias.reg_dom.EditValue);
            }

            comm.Parameters.AddWithValue("@DOM", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV", PD.fias.reg_kv.Text);
            if (PD.fias.bomj.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@bomg", 1);
                comm.Parameters.AddWithValue("@addr_g", 0);
            }
            else
            {
                comm.Parameters.AddWithValue("@bomg", 0);
                comm.Parameters.AddWithValue("@addr_g", 1);
            }

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }

            //if (PD.fias1.sovp_addr.IsChecked == true)
            //{
            comm.Parameters.AddWithValue("@FIAS_L1_1", PD.fias.reg.EditValue);
            comm.Parameters.AddWithValue("@addr_p", 1);
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", PD.fias.reg_town.EditValue);
            }

            if (PD.fias1.reg_np1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias.reg_np.EditValue);
            }
            if (PD.fias1.reg_ul1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", PD.fias1.reg_ul1.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90_1", PD.fias1.reg_ul1.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91_1", PD.fias1.reg_ul1.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias.reg_dom.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM_1", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP_1", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT_1", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV_1", PD.fias.reg_kv.Text);

            comm.Parameters.AddWithValue("@screen", PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_photo.EditValue));

            comm.Parameters.AddWithValue("@screen1", PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_podp.EditValue));

            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' ", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where idguid='{perguid}')", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),'{perguid}','{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where idguid='{perguid}'))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),'{perguid}',(select event_guid from pol_persons where idguid='{perguid}'),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void SaveDD_bt1_b0_s1_p13(MainWindow PD)
        {
            string module = "SaveDD_bt1_b0_s1_p13";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand("insert into pol_persons (IDGUID,ENP,FAM,IM,OT,W,DR,mr,BIRTH_OKSM,C_OKSM,ss,phone,email,kateg,dost,rperson_guid)" +
                            " VALUES (newid(),@enp,@fam,@im,@ot,@w,@dr,@mr,@boksm,@coksm,@ss,@phone,@email,@kateg,@dost,@rpguid)" +

                            "update pol_persons set parentguid='00000000-0000-0000-0000-000000000000' where id=SCOPE_IDENTITY()" +



                            "insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,agent)" +
                            " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=SCOPE_IDENTITY())," +
                            "(select rperson_guid from pol_persons where id=SCOPE_IDENTITY()),@prelation,@rsmo,@rpolis,@fpolis,@agent)" +
                             "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where idguid=(select person_guid from pol_events where id=SCOPE_IDENTITY())" +

                            "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                            "values(newid(),(select person_guid from pol_events where id=SCOPE_IDENTITY()), @oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                            "(select idguid from pol_events where id=SCOPE_IDENTITY()))" +

                            "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,DOCEXP,name_vp,main,event_guid)" +
                                "values(newid(),(select person_guid from POL_DOCUMENTS where id=SCOPE_IDENTITY()),@oksm,@doctype1,@docser1,@docnam1,@docdate1,@docexp1,@name_vp1,0," +
                                "(select event_guid from pol_documents where id=SCOPE_IDENTITY()))" +

                            " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                                "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1)," +
                                "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1 and livestatus=1),@FIAS_L1,@FIAS_L3,@FIAS_L4,@FIAS_L6,@FIAS_L90,@FIAS_L91,@FIAS_L7,@DOM,@KORP,@EXT,@KV, " +
                                "(select event_guid from pol_documents where id=SCOPE_IDENTITY()),@HOUSE_GUID)" +

                            "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                            " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                            ", @bomg,1,@dreg,1,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                            "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select person_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY() ),2,(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()))" +


                            "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select person_guid from pol_personsb where id=SCOPE_IDENTITY() ),3,(select event_guid from pol_personsb where id=SCOPE_IDENTITY() ))" +

                            "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_personsb where id=SCOPE_IDENTITY() )," +
                            " (select idguid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1)," +
                            "(select event_guid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1))" +

                            "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() )," +
                            " (select idguid from pol_documents where person_guid= (select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() ) and main=0)," +
                            "(select event_guid from pol_documents where person_guid= (select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() ) and main=0))" +

                            "insert into pol_polises (vpolis,spolis,npolis,dbeg,dend,dstop,blank,dreceived,person_guid,event_guid) values (@vpolis,@spolis,@npolis,@dbeg,@dend,@dstop,@blank,@dreceived, " +
                            "(select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()),(select event_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY()) ) " +

                             "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                                "(select event_guid from pol_polises where id=SCOPE_IDENTITY()),(select person_guid from pol_polises where id=SCOPE_IDENTITY()))" +
                                "select person_guid from pol_oplist where id=SCOPE_IDENTITY()", con);





            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm.Parameters.AddWithValue("@email", PD.email.Text);
            comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);

            comm.Parameters.AddWithValue("@doctype1", PD.ddtype.EditValue);
            comm.Parameters.AddWithValue("@docser1", PD.ddser.Text);
            comm.Parameters.AddWithValue("@docnam1", PD.ddnum.Text);
            comm.Parameters.AddWithValue("@docdate1", PD.dddate.DateTime);
            comm.Parameters.AddWithValue("@name_vp1", PD.ddkemv.Text);
            comm.Parameters.AddWithValue("@docexp1", PD.docexp1.EditValue ?? DBNull.Value);

            if (Convert.ToDateTime(PD.date_end.EditValue) == DateTime.MinValue)
            {
                comm.Parameters.AddWithValue("@dend", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dend", PD.date_end.EditValue);
            }
            if (Convert.ToDateTime(PD.fakt_prekr.EditValue) == DateTime.MinValue)
            {
                comm.Parameters.AddWithValue("@dstop", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue);
            }
            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);
            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }
            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", PD.fias.reg_dom.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV", PD.fias.reg_kv.Text);
            if (PD.fias.bomj.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@bomg", 1);
                comm.Parameters.AddWithValue("@addr_g", 0);
            }
            else
            {
                comm.Parameters.AddWithValue("@bomg", 0);
                comm.Parameters.AddWithValue("@addr_g", 1);
            }

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }

            //if (PD.fias1.sovp_addr.IsChecked == true)
            //{
            comm.Parameters.AddWithValue("@FIAS_L1_1", PD.fias.reg.EditValue);
            comm.Parameters.AddWithValue("@addr_p", 1);
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", PD.fias.reg_town.EditValue);
            }

            if (PD.fias1.reg_np1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias.reg_np.EditValue);
            }
            if (PD.fias1.reg_ul1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", PD.fias1.reg_ul1.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90_1", PD.fias1.reg_ul1.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91_1", PD.fias1.reg_ul1.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias.reg_dom.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM_1", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP_1", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT_1", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV_1", PD.fias.reg_kv.Text);

            comm.Parameters.AddWithValue("@screen", PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_photo.EditValue));

            comm.Parameters.AddWithValue("@screen1", PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_podp.EditValue));

            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' ", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where idguid='{perguid}')", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),'{perguid}','{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where idguid='{perguid}'))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),'{perguid}',(select event_guid from pol_persons where idguid='{perguid}'),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }

            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void SaveDD_bt3_b0_s0_p13(MainWindow PD)
        {
            string zap_polis;
            if (PD.blank_polis != false)
            {
                zap_polis = SPR.update_polises;
            }
            else
            {
                if (!PD.no_new_polis)
                {
                    zap_polis = SPR.insert_polises;
                }
                else
                {
                    zap_polis = SPR.update_polises;
                }

            }
            string module = "SaveDD_bt3_b0_s0_p13";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);

            // не забыть исправить insert на update в pol_persons

            SqlCommand comm = new SqlCommand("update pol_persons set parentguid='00000000-0000-0000-0000-000000000000', ENP=@enp,FAM=@fam,IM=@im,OT=@ot,W=@w,DR=@dr,ss=@ss,mr=@docmr,birth_oksm=@boksm," +
                 "c_oksm=@coksm,phone=@phone,email=@email,kateg=@kateg,dost=@dost,rperson_guid=@rpguid, active=1 where id=@id_p " +

                "insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,agent)" +
                " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=@id_p)," +
                "(select rperson_guid from pol_persons where id=@id_p),@prelation,@rsmo,@rpolis,@fpolis,@agent)" +
                 "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where idguid=(select person_guid from pol_events where id=SCOPE_IDENTITY())" +

                "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                "values(newid(),(select person_guid from pol_events where id=SCOPE_IDENTITY()), @oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                "(select idguid from pol_events where id=SCOPE_IDENTITY()))" +

                "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,DOCEXP,name_vp,main,event_guid)" +
                    "values(newid(),(select person_guid from POL_DOCUMENTS where id=SCOPE_IDENTITY()),@oksm,@doctype1,@docser1,@docnam1,@docdate1,@docexp1,@name_vp1,0," +
                    "(select event_guid from pol_documents where id=SCOPE_IDENTITY()))" +

                " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                    "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1)," +
                    "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1 and livestatus=1),@FIAS_L1,@FIAS_L3,@FIAS_L4,@FIAS_L6,@FIAS_L90,@FIAS_L91,@FIAS_L7,@DOM,@KORP,@EXT,@KV, " +
                    "(select event_guid from pol_documents where id=SCOPE_IDENTITY()),@HOUSE_GUID)" +

                "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                ", @bomg,1,@dreg,0,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                    "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7_1 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7_1 and actstatus=1)," +
                    "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1_1 and livestatus=1),@FIAS_L1_1,@FIAS_L3_1,@FIAS_L4_1,@FIAS_L6_1,@FIAS_L90_1,@FIAS_L91_1,@FIAS_L7_1,@DOM_1,@KORP_1,@EXT_1,@KV_1, " +
                    "(select event_guid from pol_relation_addr_pers where id=SCOPE_IDENTITY()),@HOUSE_GUID_1)" +

                "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                ", @bomg,0,@dreg1,1,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select idguid from pol_persons where id=@id_p ),2,(select event_guid from pol_persons where id=@id_p )) " +
                 "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select idguid from pol_persons where id=@id_p ),3,(select event_guid from pol_persons where id=@id_p )) " +

                "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_personsb where id=SCOPE_IDENTITY() )," +
                " (select idguid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1)," +
                "(select event_guid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1))" +

                "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() )," +
                " (select idguid from pol_documents where person_guid= (select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() ) and main=0)," +
                "(select event_guid from pol_documents where person_guid= (select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() ) and main=0))" +

                zap_polis +

                "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                    "(select event_guid from pol_polises where id=SCOPE_IDENTITY()),(select person_guid from pol_polises where id=SCOPE_IDENTITY()))" +
                    "select person_guid from pol_oplist where id=SCOPE_IDENTITY()", con);





            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm.Parameters.AddWithValue("@email", PD.email.Text);
            comm.Parameters.AddWithValue("@id_p", Convert.ToInt32(Vars.IdP));
            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            if (Convert.ToDateTime(PD.date_end.EditValue) == DateTime.MinValue || PD.date_end.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dend", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dend", PD.date_end.EditValue);
            }
            if (Convert.ToDateTime(PD.fakt_prekr.EditValue) == DateTime.MinValue || PD.fakt_prekr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dstop", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue);
            }
            if (Convert.ToDateTime(PD.dout.EditValue) == DateTime.MinValue || PD.dout.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dout", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dout", PD.dout.EditValue);
            }
            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);

            comm.Parameters.AddWithValue("@doctype1", PD.ddtype.EditValue);
            comm.Parameters.AddWithValue("@docser1", PD.ddser.Text);
            comm.Parameters.AddWithValue("@docnam1", PD.ddnum.Text);
            comm.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
            comm.Parameters.AddWithValue("@name_vp1", PD.ddkemv.Text);
            comm.Parameters.AddWithValue("@docexp1", PD.docexp1.EditValue ?? DBNull.Value);

            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }

            if (PD.kat_zl.EditValue != null)
            {
                comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            }
            else
            {
                MessageBox.Show("Выберите категогрию ЗЛ!");
            }

            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            if (PD.fias.reg.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L1", PD.L1);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            }

            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4", PD.L4);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4", PD.fias.reg_town.EditValue);
            }
            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7", PD.L7);
                comm.Parameters.AddWithValue("@FIAS_L90", PD.L7);
                comm.Parameters.AddWithValue("@FIAS_L91", PD.L7);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", PD.fias.reg_dom.EditValue);
            }

            comm.Parameters.AddWithValue("@DOM", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV", PD.fias.reg_kv.Text);
            if (PD.fias.bomj.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@bomg", 1);
                comm.Parameters.AddWithValue("@addr_g", 0);
            }
            else
            {
                comm.Parameters.AddWithValue("@bomg", 0);
                comm.Parameters.AddWithValue("@addr_g", 1);
            }

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }
            comm.Parameters.AddWithValue("@dreg1", PD.fias1.reg_dr1.DateTime);

            if (PD.fias1.sovp_addr.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@addr_p", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@addr_p", 0);
            }
            if (PD.fias1.reg1.EditValue == null)
            {
                string m = "Заполните регион проживания!";
                string t = "Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                return;
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L1_1", PD.fias1.reg1.EditValue);
            }


            if (PD.fias1.reg_rn1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias1.reg_rn1.EditValue);
            }
            if (PD.fias1.reg_town1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", PD.L4_1);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", PD.fias1.reg_town1.EditValue);
            }

            if (PD.fias1.reg_np1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias1.reg_np1.EditValue);
            }
            if (PD.fias1.reg_ul1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", PD.L7_1);
                comm.Parameters.AddWithValue("@FIAS_L90_1", PD.L7_1);
                comm.Parameters.AddWithValue("@FIAS_L91_1", PD.L7_1);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", PD.fias1.reg_ul1.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90_1", PD.fias1.reg_ul1.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91_1", PD.fias1.reg_ul1.EditValue);
            }
            if (PD.fias1.reg_dom1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias1.reg_dom1.EditValue);
            }

            comm.Parameters.AddWithValue("@DOM_1", PD.domsplit1);
            comm.Parameters.AddWithValue("@KORP_1", PD.fias1.reg_korp1.Text);
            comm.Parameters.AddWithValue("@EXT_1", PD.fias1.reg_str1.Text);
            comm.Parameters.AddWithValue("@KV_1", PD.fias1.reg_kv1.Text);

            comm.Parameters.AddWithValue("@screen", PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_photo.EditValue));

            comm.Parameters.AddWithValue("@screen1", PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_podp.EditValue));

            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where id={Vars.IdP})", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where id={Vars.IdP})", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),(select idguid from pol_persons where id={Vars.IdP}),'{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where id={Vars.IdP}))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),(select idguid from pol_persons where id={Vars.IdP}),(select event_guid from pol_persons where id={Vars.IdP}),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void SaveDD_bt3_b1_s0_p13(MainWindow PD)
        {
            string zap_polis;
            if (PD.blank_polis != false)
            {
                zap_polis = SPR.update_polises;
            }
            else
            {
                if (!PD.no_new_polis)
                {
                    zap_polis = SPR.insert_polises;
                }
                else
                {
                    zap_polis = SPR.update_polises;
                }

            }
            string module = "SaveDD_bt3_b1_s0_p13";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand("update pol_persons set parentguid='00000000-0000-0000-0000-000000000000', ENP=@enp,FAM=@fam,IM=@im,OT=@ot,W=@w,DR=@dr,ss=@ss,mr=@docmr,birth_oksm=@boksm," +
                 "c_oksm=@coksm,phone=@phone,email=@email,kateg=@kateg,dost=@dost,rperson_guid=@rpguid, active=1 where id=@id_p " +

                "insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,agent)" +
                " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=@id_p)," +
                "(select rperson_guid from pol_persons where id=@id_p),@prelation,@rsmo,@rpolis,@fpolis,@agent)" +
                 "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where idguid=(select person_guid from pol_events where id=SCOPE_IDENTITY())" +

                "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                "values(newid(),(select person_guid from pol_events where id=SCOPE_IDENTITY()), @oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                "(select idguid from pol_events where id=SCOPE_IDENTITY()))" +

                "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,DOCEXP,name_vp,main,event_guid)" +
                    "values(newid(),(select person_guid from POL_DOCUMENTS where id=SCOPE_IDENTITY()),@oksm,@doctype1,@docser1,@docnam1,@docdate1,@docexp1,@name_vp1,0," +
                    "(select event_guid from pol_documents where id=SCOPE_IDENTITY()))" +

                " insert into pol_addresses (IDGUID,EVENT_GUID,fias_l1,FIAS_L3) " +
                "values(newid(),(select event_guid from pol_documents where id=SCOPE_IDENTITY()),@FIAS_L1,@FIAS_L3)" +

                "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,addres_p,dt1,dt2,event_guid)" +
                " values((select idguid from pol_persons where event_guid=(select event_guid from pol_addresses where id=SCOPE_IDENTITY()) ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                ", 1,0,0,sysdatetime(),null,(select event_guid from pol_addresses where id=SCOPE_IDENTITY()))" +

                "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select idguid from pol_persons where id=@id_p ),2,(select event_guid from pol_persons where id=@id_p )) " +
                 "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select idguid from pol_persons where id=@id_p ),3,(select event_guid from pol_persons where id=@id_p )) " +

                "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_personsb where id=SCOPE_IDENTITY() )," +
                " (select idguid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1)," +
                "(select event_guid from pol_documents where person_guid= (select person_guid from pol_personsb where id=SCOPE_IDENTITY() ) and main=1))" +

                "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() )," +
                " (select idguid from pol_documents where person_guid= (select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() ) and main=0)," +
                "(select event_guid from pol_documents where person_guid= (select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() ) and main=0))" +

                zap_polis +


                "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                    "(select event_guid from pol_polises where id=SCOPE_IDENTITY()),(select person_guid from pol_polises where id=SCOPE_IDENTITY()))" +
                    "select person_guid from pol_oplist where id=SCOPE_IDENTITY()", con);






            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm.Parameters.AddWithValue("@email", PD.email.Text);
            comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            comm.Parameters.AddWithValue("@id_p", Convert.ToInt32(Vars.IdP));
            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            if (Convert.ToDateTime(PD.date_end.EditValue) == DateTime.MinValue || PD.date_end.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dend", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dend", PD.date_end.EditValue);
            }
            if (Convert.ToDateTime(PD.fakt_prekr.EditValue) == DateTime.MinValue || PD.fakt_prekr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dstop", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue);
            }
            if (Convert.ToDateTime(PD.dout.EditValue) == DateTime.MinValue || PD.dout.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dout", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dout", PD.dout.EditValue);
            }
            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);

            comm.Parameters.AddWithValue("@doctype1", PD.ddtype.EditValue);
            comm.Parameters.AddWithValue("@docser1", PD.ddser.Text);
            comm.Parameters.AddWithValue("@docnam1", PD.ddnum.Text);
            comm.Parameters.AddWithValue("@docdate1", PD.dddate.DateTime);
            comm.Parameters.AddWithValue("@name_vp1", PD.ddkemv.Text);
            comm.Parameters.AddWithValue("@docexp1", PD.docexp1.EditValue ?? DBNull.Value);

            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }
            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }

            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }

            if (PD.fias.bomj.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@bomg", 1);
                comm.Parameters.AddWithValue("@addr_g", 0);
            }
            else
            {
                comm.Parameters.AddWithValue("@bomg", 0);
                comm.Parameters.AddWithValue("@addr_g", 1);
            }

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }

            if (PD.fias1.sovp_addr.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@addr_p", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@addr_p", 0);
            }


            if (PD.fias1.reg_rn1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias1.reg_rn1.EditValue);
            }

            if (PD.fias1.reg_np1.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias1.reg_np1.EditValue);
            }

            comm.Parameters.AddWithValue("@screen", PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_photo.EditValue));

            comm.Parameters.AddWithValue("@screen1", PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_podp.EditValue));

            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where id={Vars.IdP})", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where id={Vars.IdP})", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),(select idguid from pol_persons where id={Vars.IdP}),'{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where id={Vars.IdP}))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),(select idguid from pol_persons where id={Vars.IdP}),(select event_guid from pol_persons where id={Vars.IdP}),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void SaveDD_bt3_b0_s1_p13(MainWindow PD)
        {
            string zap_polis;
           
                if (PD.blank_polis != false)
                {
                    zap_polis = SPR.update_polises;
                }
                else
                {
                    if (!PD.no_new_polis)
                    {
                        zap_polis = SPR.insert_polises;
                    }
                    else
                    {
                        zap_polis = SPR.update_polises;
                    }

                }
            string module = "SaveDD_bt3_b0_s1_p13";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand("update pol_documents set main=1,active=0 where event_guid=(select event_guid from pol_persons where id=@id_p) and main=1 and active=1" +
                "update pol_documents set main=0,active=0 where event_guid=(select event_guid from pol_persons where id=@id_p) and main=0 and active=1" +
                
                "update pol_relation_addr_pers set active=0 where event_guid= (select event_guid from pol_persons where id=@id_p) " + 

                "update pol_persons set parentguid='00000000-0000-0000-0000-000000000000', ENP=@enp,FAM=@fam,IM=@im,OT=@ot,W=@w,DR=@dr,ss=@ss,mr=@docmr,birth_oksm=@boksm,"+
                 "c_oksm=@coksm,phone=@phone,email=@email,kateg=@kateg,dost=@dost,rperson_guid=@rpguid, active=1,comment='' where id=@id_p " +

                "insert into pol_events (IDGUID,dvizit,method,petition,tip_op,person_guid,rperson_guid,prelation,rsmo,rpolis,fpolis,agent)" +
                " VALUES (newid(),@dvizit,@method,@pet,@tip_op,(select idguid from pol_persons where id=@id_p)," +
                "(select rperson_guid from pol_persons where id=@id_p),@prelation,@rsmo,@rpolis,@fpolis,@agent)" +
                 "update pol_persons set event_guid=(select idguid from pol_events where id=SCOPE_IDENTITY()) where id=@id_p " +

                "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,NAME_VP,NAME_VP_CODE,DOCMR,event_guid)" +
                "values(newid(),(select idguid from pol_persons where id=@id_p), @oksm,@doctype,@docser,@docnam,@docdate,@name_vp,@vp_code,@docmr," +
                "(select event_guid from pol_persons where id=@id_p))" +

                "insert into POL_DOCUMENTS(IDGUID,PERSON_GUID,OKSM,DOCTYPE,DOCSER,DOCNUM,DOCDATE,DOCEXP,name_vp,main,event_guid)" +
                    "values(newid(),(select idguid from pol_persons where id=@id_p),@oksm,@doctype1,@docser1,@docnam1,@docdate1,@docexp1,@name_vp1,0," +
                    "(select event_guid from pol_persons where id=@id_p))" +

                " insert into pol_addresses (IDGUID,INDX,OKATO,SUBJ,FIAS_L1,FIAS_L3,FIAS_L4,FIAS_L6,FIAS_L90,FIAS_L91,FIAS_L7,DOM,KORP,EXT,KV,EVENT_GUID,HOUSE_GUID) " +
                    "values(newid(),(select POSTALCODE from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1),(select OKATO from fias.dbo.AddressObjects where aoguid=@FIAS_L7 and actstatus=1)," +
                    "(select left(OKATO,5) from fias.dbo.AddressObjects where aoguid=@FIAS_L1 and livestatus=1),@FIAS_L1,@FIAS_L3,@FIAS_L4,@FIAS_L6,@FIAS_L90,@FIAS_L91,@FIAS_L7,@DOM,@KORP,@EXT,@KV, " +
                    "(select event_guid from pol_persons where id=@id_p),@HOUSE_GUID)" +

                "insert into pol_relation_addr_pers (person_guid,addr_guid,bomg,addres_g,dreg,addres_p,dt1,dt2,event_guid)" +
                " values((select idguid from pol_persons where id=@id_p ), (select idguid from pol_addresses where id=SCOPE_IDENTITY())" +
                ", @bomg,1,@dreg,1,sysdatetime(),null,(select event_guid from pol_persons where id=@id_p))" +

                "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen,(select idguid from pol_persons where id=@id_p ),2,(select event_guid from pol_persons where id=@id_p )) " +
                 "insert into pol_personsb (photo,person_guid,type,event_guid) values(@screen1,(select idguid from pol_persons where id=@id_p ),3,(select event_guid from pol_persons where id=@id_p )) " +

                "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_personsb where id=SCOPE_IDENTITY() )," +
                " (select idguid from pol_documents where event_guid= (select event_guid from pol_persons where id=@id_p) and main=1)," +
                "(select event_guid from pol_persons where id=@id_p ))" +

                "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,EVENT_GUID) values((select person_guid from pol_relation_doc_pers where id=SCOPE_IDENTITY() )," +
                " (select idguid from pol_documents where event_guid= (select event_guid from pol_persons where id=@id_p) and main=0)," +
                "(select event_guid from pol_persons where id=@id_p ))" +

                zap_polis +

                "insert into pol_oplist (smocod,przcod,event_guid,person_guid) values((select top(1) SMO_CODE from pol_prz),@prz," +
                    "(select event_guid from pol_persons where id=@id_p),(select idguid from pol_persons where id=@id_p))" +
                    "select idguid from pol_persons where id=@id_p", con);






            comm.Parameters.AddWithValue("@agent", Vars.Agnt);
            comm.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm.Parameters.AddWithValue("@im", PD.im.Text);
            comm.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm.Parameters.AddWithValue("@method", Vars.Sposob);
            comm.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm.Parameters.AddWithValue("@prelation", PD.status_p2.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm.Parameters.AddWithValue("@email", PD.email.Text);
            comm.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            comm.Parameters.AddWithValue("@oksm", PD.gr.EditValue.ToString());
            comm.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm.Parameters.AddWithValue("@docmr", PD.mr2.Text);
            comm.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            comm.Parameters.AddWithValue("@id_p", Vars.IdP);
            comm.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            if (Convert.ToDateTime(PD.date_end.EditValue) == DateTime.MinValue || PD.date_end.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dend", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dend", PD.date_end.EditValue);
            }
            if (Convert.ToDateTime(PD.fakt_prekr.EditValue) == DateTime.MinValue || PD.fakt_prekr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dstop", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue);
            }
            if (Convert.ToDateTime(PD.dout.EditValue) == DateTime.MinValue || PD.dout.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dout", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dout", PD.dout.EditValue);
            }
            comm.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            comm.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));
            comm.Parameters.AddWithValue("@prz", Vars.PunctRz);

            comm.Parameters.AddWithValue("@doctype1", PD.ddtype.EditValue);
            comm.Parameters.AddWithValue("@docser1", PD.ddser.Text);
            comm.Parameters.AddWithValue("@docnam1", PD.ddnum.Text);
            comm.Parameters.AddWithValue("@docdate1", PD.dddate.DateTime);
            comm.Parameters.AddWithValue("@name_vp1", PD.ddkemv.Text);
            comm.Parameters.AddWithValue("@docexp1", PD.docexp1.EditValue ?? DBNull.Value);

            if (PD.pustoy.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm.Parameters.AddWithValue("@blank", 0);
            }
            if (PD.s == "" || PD.s == null)
            {
                comm.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dost", PD.s);
            }

            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID", PD.fias.reg_dom.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM", PD.domsplit);
            comm.Parameters.AddWithValue("@KORP", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV", PD.fias.reg_kv.Text);
            if (PD.fias.bomj.IsChecked == true)
            {
                comm.Parameters.AddWithValue("@bomg", 1);
                comm.Parameters.AddWithValue("@addr_g", 0);
            }
            else
            {
                comm.Parameters.AddWithValue("@bomg", 0);
                comm.Parameters.AddWithValue("@addr_g", 1);
            }

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }

            //if (PD.fias1.sovp_addr.IsChecked == true)
            //{
            comm.Parameters.AddWithValue("@addr_p", 1);
            //}
            //else
            //{
            //    comm.Parameters.AddWithValue("@addr_p", 0);
            //}

            comm.Parameters.AddWithValue("@FIAS_L1_1", PD.fias.reg.EditValue);
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L3_1", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L4_1", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L6_1", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                comm.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@FIAS_L7_1", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L90_1", PD.fias.reg_ul.EditValue);
                comm.Parameters.AddWithValue("@FIAS_L91_1", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
            }
            else
            {
                comm.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias.reg_dom.EditValue);
            }
            comm.Parameters.AddWithValue("@DOM_1", PD.domsplit1);
            comm.Parameters.AddWithValue("@KORP_1", PD.fias.reg_korp.Text);
            comm.Parameters.AddWithValue("@EXT_1", PD.fias.reg_str.Text);
            comm.Parameters.AddWithValue("@KV_1", PD.fias.reg_kv.Text);


            comm.Parameters.AddWithValue("@screen", PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_photo.EditValue));

            comm.Parameters.AddWithValue("@screen1", PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_podp.EditValue));

            comm.Parameters.AddWithValue("@rpguid", "00000000-0000-0000-0000-000000000000");
            con.Open();
            tr = con.BeginTransaction();
            comm.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm.ExecuteScalar();
                tr.Commit();
                con.Close();
            }
            catch (Exception e)
            {
                tr.Rollback();
                con.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            //-----------------------------------------------------------------------------
            // Если в предидущем окне был выбран способ подачи заявления "Через представителя"
            if (perguid != null)
            {
                if (PD.rper == Guid.Empty)
                {
                    if (PD.fam1.Text == "")
                    {
                        //comm.Parameters.AddWithValue("@rpguid", Guid.Empty);
                    }
                    else
                    {
                        var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                        //SqlConnection con = new SqlConnection(connectionString1);
                        SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                             " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                             "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                             " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                             "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                             "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                             " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                        comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                        comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                        comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                        comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                        comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                        comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                        comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                        comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                        comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                        if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                        {
                            comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                        }
                        else
                        {
                            comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                        }
                        comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                        con.Open();
                        Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                        con.Close();
                        SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where id={Vars.IdP})", con);

                        con.Open();
                        comm311.ExecuteNonQuery();
                        con.Close();
                    }

                }
                else
                {
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{PD.rper}' where idguid='{perguid}' 
                    update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where id={Vars.IdP})", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();

                }
            }
            else
            {
                //Item_Not_Saved();
                //return;
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {

                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS_OLD(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid)
                                values(newid(),(select idguid from pol_persons where id={Vars.IdP}),'{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_persons where id={Vars.IdP}))", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {

                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS_OLD set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}' where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),(select idguid from pol_persons where id={Vars.IdP}),(select event_guid from pol_persons where id={Vars.IdP}),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if (PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {

                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS_OLD set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}'
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                SqlCommand cmdmo = new SqlCommand($@"update POL_PERSONS set mo='{PD.mo_cmb.EditValue}',
dstart=@date_mo where idguid='{perguid}'", con);
                cmdmo.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
                //if (date_mo.EditValue == null)
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", DBNull.Value);
                //}
                //else
                //{
                //    cmdmo.Parameters.AddWithValue("@date_mo", Convert.ToDateTime(date_mo.EditValue));
                //}
                con.Open();
                cmdmo.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void SaveDD_bt2_prf1(MainWindow PD)
        {
            string module = "SaveDD_bt2_prf1";
            SqlTransaction tr = null;
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con2 = new SqlConnection(connectionString);
            SqlCommand comm2 = new SqlCommand("update pol_persons set SROKDOVERENOSTI=@srok_doverenosti,PRIZNAKVIDACHI=@PRIZNAKVIDACHI,DATEVIDACHI=@DATEVIDACHI,ENP=@enp,FAM=@fam,IM=@im,OT=@ot,W=@w,DR=@dr,ss=@ss,mr=@mr,birth_oksm=@boksm,"
                + "c_oksm=@coksm,phone=@phone,email=@email,kateg=@kateg,dost=@dost,ddeath=@ddeath,rperson_guid=@rpguid, mo=@mo, dstart=@date_mo, DOP_COMMENT=@DOP_COMMENT, COMMENT=@COMMENT where id=@id_p " +

                "update pol_addresses set fias_l1=@FIAS_L1,fias_l3=@FIAS_L3,fias_l4=@FIAS_L4,fias_l6=@FIAS_L6,fias_l7=@FIAS_L7,fias_l90=@FIAS_L90," +
                "fias_l91=@FIAS_L91, dom=@DOM,korp=@KORP,ext=@EXT,kv=@KV, house_guid=@HOUSE_GUID where idguid=(select ADDR_GUID from pol_relation_addr_pers where addres_g=1 and event_guid=(select event_guid from pol_persons where id=@id_p)) and " +
                "event_guid=(select event_guid from pol_persons where id =@id_p) " +

                "update pol_addresses set fias_l1=@FIAS_L1_1,fias_l3=@FIAS_L3_1,fias_l4=@FIAS_L4_1,fias_l6=@FIAS_L6_1,fias_l7=@FIAS_L7_1,fias_l90=@FIAS_L90_1," +
                "fias_l91=@FIAS_L91_1, dom=@DOM_1,korp=@KORP_1,ext=@EXT_1,kv=@KV_1,house_guid=@HOUSE_GUID_1 where idguid=(select ADDR_GUID from pol_relation_addr_pers where addres_p=1 and event_guid=(select event_guid from pol_persons where id=@id_p)) and " +
                "event_guid=(select event_guid from pol_persons where id =@id_p) " +

                "update pol_relation_addr_pers SET bomg=@bomg,addres_g=@addr_g,addres_p=@addr_p,dreg=@dreg where addres_g=1 and event_guid=(select event_guid from pol_persons where id=@id_p) " +

                "update pol_relation_addr_pers SET bomg=@bomg,addres_g=@addr_g1,addres_p=@addr_p1,dreg=@dreg where addres_p=1  and event_guid=(select event_guid from pol_persons where id=@id_p) " +



                "update pol_documents set oksm=@oksm,doctype=@doctype,docser=@docser,docnum=@docnam,docdate=@docdate,docexp=@docexp,name_vp=@name_vp,name_vp_code=@vp_code, " +
                "docmr=@docmr where event_guid=(select event_guid from pol_persons where id=@id_p) and main=1" +

                "update pol_documents set doctype=@doctype1,docser=@docser1,docnum=@docnam1,docdate=@docdate1,docexp=@docexp1,name_vp=@name_vp1 " +
                 "where event_guid=(select event_guid from pol_persons where id=@id_p) and main=0" +

                "update pol_polises set vpolis=@vpolis,spolis=@spolis,npolis=@npolis,dbeg=@dbeg,dend=@dend,dstop=@dstop,blank=@blank,dreceived=@dreceived " +
                "where event_guid=(select event_guid from pol_persons where id=@id_p)" +

               "update pol_personsb set photo=@screen where event_guid=(select event_guid from pol_persons where id=@id_p) and type=2 " +
               "update pol_personsb set photo=@screen1 where event_guid=(select event_guid from pol_persons where id=@id_p) and type=3 " +

               "update pol_events set unload=0,tip_op=@tip_op,rsmo=@rsmo,rpolis=@rpolis,fpolis=@fpolis,method=@method,petition=@pet,dvizit=@dvizit " +
               "where idguid=(select event_guid from pol_persons where id=@id_p)" +
               "select idguid from pol_persons where id=@id_p", con2);

            comm2.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm2.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm2.Parameters.AddWithValue("@im", PD.im.Text);
            comm2.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm2.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm2.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm2.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm2.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm2.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm2.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm2.Parameters.AddWithValue("@rsmo", PD.pr_pod_z_smo.SelectedIndex + 1);
            comm2.Parameters.AddWithValue("@rpolis", PD.pr_pod_z_polis.SelectedIndex + 1);
            comm2.Parameters.AddWithValue("@fpolis", PD.form_polis.SelectedIndex);
            comm2.Parameters.AddWithValue("@method", Vars.Sposob);
            comm2.Parameters.AddWithValue("@pet", Convert.ToInt32(PD.petition.EditValue));
            comm2.Parameters.AddWithValue("@dvizit", PD.d_obr.EditValue ?? DBNull.Value);
            comm2.Parameters.AddWithValue("@doctype1", PD.ddtype.EditValue);
            comm2.Parameters.AddWithValue("@docser1", PD.ddser.Text);
            comm2.Parameters.AddWithValue("@docnam1", PD.ddnum.Text);
            comm2.Parameters.AddWithValue("@docdate1", PD.dddate.EditValue ?? DBNull.Value);
            comm2.Parameters.AddWithValue("@name_vp1", PD.ddkemv.Text);
            comm2.Parameters.AddWithValue("@docexp1", PD.docexp1.EditValue ?? DBNull.Value);
            comm2.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
            comm2.Parameters.AddWithValue("@COMMENT", PD.comments.Text);
            comm2.Parameters.AddWithValue("@DOP_COMMENT", PD.Dop_comments.Text);

            if (PD.priznak_vidachi.IsChecked == true)
            {
                comm2.Parameters.AddWithValue("@PRIZNAKVIDACHI", "1");
            }
            else
            {
                comm2.Parameters.AddWithValue("@PRIZNAKVIDACHI", "0");
            }
            comm2.Parameters.AddWithValue("@DATEVIDACHI", PD.date_vidachi.DateTime);

            if (PD.mo_cmb.SelectedIndex != -1)
            {
                comm2.Parameters.AddWithValue("@mo", PD.mo_cmb.EditValue.ToString());
            }
            else
            {
                comm2.Parameters.AddWithValue("@mo", "");
            }
            //if (Convert.ToDateTime(date_mo.EditValue) == DateTime.MinValue || date_mo.EditValue == null)
            //{
            //    comm2.Parameters.AddWithValue("@date_mo", DBNull.Value);
            //}
            //else
            //{
            //    comm2.Parameters.AddWithValue("@date_mo", date_mo.EditValue);
            //}
            comm2.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
            if (Convert.ToDateTime(PD.ddeath.EditValue) == DateTime.MinValue)
            {
                comm2.Parameters.AddWithValue("@ddeath", DBNull.Value);
            }
            else
            {
                comm2.Parameters.AddWithValue("@ddeath", PD.date_end.EditValue);
            }

            comm2.Parameters.AddWithValue("@oksm", PD.str_vid.EditValue.ToString());
            comm2.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm2.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm2.Parameters.AddWithValue("@email", PD.email.Text);
            comm2.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            try
            {

                comm2.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());

            }
            catch
            {
                string m = "Выберите тип полиса!";
                string t = "Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                return;
            }
            comm2.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm2.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm2.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            if (Convert.ToDateTime(PD.date_end.EditValue) == DateTime.MinValue)
            {
                comm2.Parameters.AddWithValue("@dend", DBNull.Value);
            }
            else
            {
                comm2.Parameters.AddWithValue("@dend", PD.date_end.EditValue);
            }
            if (Convert.ToDateTime(PD.fakt_prekr.EditValue) == DateTime.MinValue)
            {
                comm2.Parameters.AddWithValue("@dstop", DBNull.Value);
            }
            else
            {
                comm2.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue);
            }

            comm2.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue??DBNull.Value);
            if (PD.pustoy.IsChecked == true)
            {
                comm2.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm2.Parameters.AddWithValue("@blank", 0);
            }


            comm2.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm2.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm2.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm2.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm2.Parameters.AddWithValue("@docexp", PD.docexp.EditValue ?? DBNull.Value);
            comm2.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm2.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm2.Parameters.AddWithValue("@docmr", PD.mr2.Text);

            comm2.Parameters.AddWithValue("@id_p", Vars.IdP);
            if (PD.s == "" || PD.s == null)
            {
                comm2.Parameters.AddWithValue("@dost", DBNull.Value);
            }
            else
            {
                comm2.Parameters.AddWithValue("@dost", PD.s);
            }

            comm2.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm2.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm2.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm2.Parameters.AddWithValue("@FIAS_L4", Guid.Empty);
            }
            else
            {
                comm2.Parameters.AddWithValue("@FIAS_L4", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm2.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm2.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm2.Parameters.AddWithValue("@FIAS_L7", Guid.Empty);
                comm2.Parameters.AddWithValue("@FIAS_L90", Guid.Empty);
                comm2.Parameters.AddWithValue("@FIAS_L91", Guid.Empty);
                
            }
            else
            {
                comm2.Parameters.AddWithValue("@FIAS_L7", PD.fias.reg_ul.EditValue);
                comm2.Parameters.AddWithValue("@FIAS_L90", PD.fias.reg_ul.EditValue);
                comm2.Parameters.AddWithValue("@FIAS_L91", PD.fias.reg_ul.EditValue);
               
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm2.Parameters.AddWithValue("@HOUSE_GUID", Guid.Empty);
            }
            else
            {
                comm2.Parameters.AddWithValue("@HOUSE_GUID", PD.fias.reg_dom.EditValue);
            }
            comm2.Parameters.AddWithValue("@DOM", PD.domsplit);
            comm2.Parameters.AddWithValue("@KORP", PD.fias.reg_korp.Text);
            comm2.Parameters.AddWithValue("@EXT", PD.fias.reg_str.Text);
            comm2.Parameters.AddWithValue("@KV", PD.fias.reg_kv.Text);
            if (PD.fias.bomj.IsChecked == true)
            {
                comm2.Parameters.AddWithValue("@bomg", 1);
                comm2.Parameters.AddWithValue("@addr_g", 1);
                comm2.Parameters.AddWithValue("@addr_p", 1);
                comm2.Parameters.AddWithValue("@addr_p1", 1);
                comm2.Parameters.AddWithValue("@addr_g1", 1);

            }
            else
            {
                comm2.Parameters.AddWithValue("@bomg", 0);
                comm2.Parameters.AddWithValue("@addr_g", 1);
                if (PD.fias1.sovp_addr.IsChecked == true)
                {
                    comm2.Parameters.AddWithValue("@addr_p", 1);
                    comm2.Parameters.AddWithValue("@addr_p1", 1);
                    comm2.Parameters.AddWithValue("@addr_g1", 1);
                }
                else
                {
                    comm2.Parameters.AddWithValue("@addr_p", 0);
                    comm2.Parameters.AddWithValue("@addr_p1", 1);
                    comm2.Parameters.AddWithValue("@addr_g1", 0);
                }

            }

            if (PD.fias.reg_dr.EditValue == null)
            {
                comm2.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm2.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }

            if (PD.fias1.sovp_addr.IsChecked == true)
            {
                comm2.Parameters.AddWithValue("@FIAS_L1_1", PD.fias.reg.EditValue);
                if (PD.fias.reg_rn.EditValue == null)
                {
                    comm2.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
                }
                else
                {
                    comm2.Parameters.AddWithValue("@FIAS_L3_1", PD.fias.reg_rn.EditValue);
                }
                if (PD.fias.reg_town.EditValue == null)
                {
                    comm2.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
                }
                else
                {
                    comm2.Parameters.AddWithValue("@FIAS_L4_1", PD.fias.reg_town.EditValue);
                }
                if (PD.fias.reg_np.EditValue == null)
                {
                    comm2.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
                }
                else
                {
                    comm2.Parameters.AddWithValue("@FIAS_L6_1", PD.fias.reg_np.EditValue);
                }
                if (PD.fias.reg_ul.EditValue == null)
                {
                    comm2.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                    comm2.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                    comm2.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
                }
                else
                {
                    comm2.Parameters.AddWithValue("@FIAS_L7_1", PD.fias.reg_ul.EditValue);
                    comm2.Parameters.AddWithValue("@FIAS_L90_1", PD.fias.reg_ul.EditValue);
                    comm2.Parameters.AddWithValue("@FIAS_L91_1", PD.fias.reg_ul.EditValue);
                }
                if (PD.fias.reg_dom.EditValue == null)
                {
                    comm2.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
                }
                else
                {
                    comm2.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias.reg_dom.EditValue);
                }
                comm2.Parameters.AddWithValue("@DOM_1", PD.domsplit);
                comm2.Parameters.AddWithValue("@KORP_1", PD.fias.reg_korp.Text);
                comm2.Parameters.AddWithValue("@EXT_1", PD.fias.reg_str.Text);
                comm2.Parameters.AddWithValue("@KV_1", PD.fias.reg_kv.Text);
            }
            else
            {
                comm2.Parameters.AddWithValue("@FIAS_L1_1", PD.fias1.reg1.EditValue);
                if (PD.fias1.reg_rn1.EditValue == null)
                {
                    comm2.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
                }
                else
                {
                    comm2.Parameters.AddWithValue("@FIAS_L3_1", PD.fias1.reg_rn1.EditValue);
                }
                if (PD.fias1.reg_town1.EditValue == null)
                {
                    comm2.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
                }
                else
                {
                    comm2.Parameters.AddWithValue("@FIAS_L4_1", PD.fias1.reg_town1.EditValue);
                }
                if (PD.fias1.reg_np1.EditValue == null)
                {
                    comm2.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
                }
                else
                {
                    comm2.Parameters.AddWithValue("@FIAS_L6_1", PD.fias1.reg_np1.EditValue);
                }
                if (PD.fias1.reg_ul1.EditValue == null)
                {
                    comm2.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                    comm2.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                    comm2.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
                }
                else
                {
                    comm2.Parameters.AddWithValue("@FIAS_L7_1", PD.fias1.reg_ul1.EditValue);
                    comm2.Parameters.AddWithValue("@FIAS_L90_1", PD.fias1.reg_ul1.EditValue);
                    comm2.Parameters.AddWithValue("@FIAS_L91_1", PD.fias1.reg_ul1.EditValue);
                }
                if (PD.fias1.reg_dom1.EditValue == null)
                {
                    comm2.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
                }
                else
                {
                    comm2.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias1.reg_dom1.EditValue);
                }
                comm2.Parameters.AddWithValue("@DOM_1", PD.domsplit1);
                comm2.Parameters.AddWithValue("@KORP_1", PD.fias1.reg_korp1.Text);
                comm2.Parameters.AddWithValue("@EXT_1", PD.fias1.reg_str1.Text);
                comm2.Parameters.AddWithValue("@KV_1", PD.fias1.reg_kv1.Text);
            }


            comm2.Parameters.AddWithValue("@screen", PD.zl_photo.EditValue == null || PD.zl_photo.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_photo.EditValue));

            comm2.Parameters.AddWithValue("@screen1", PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "" ? "" : Convert.ToBase64String((byte[])PD.zl_podp.EditValue));


            comm2.Parameters.AddWithValue("@rpguid", PD.rper);

            con2.Open();
            tr = con2.BeginTransaction();
            comm2.Transaction = tr;
            Guid? perguid = null;
            try
            {
                perguid = (Guid)comm2.ExecuteScalar();
                tr.Commit();
                con2.Close();
            }
            catch (Exception e)
            {
                tr.Rollback();
                con2.Close();
                string m = module + " " +
                    e.Message;
                string t = $@"Информация для разработчика! Ошибка!";
                int b = 1;
                Message me = new Message(m, t, b);
                me.ShowDialog();
                Item_Not_Saved();
                return;
            }

            if (PD.rper == Guid.Empty)
            {
                if (PD.fam1.Text == "")
                {
                    comm2.Parameters.AddWithValue("@rpguid", Guid.Empty);
                }
                else
                {
                    var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                    SqlConnection con = new SqlConnection(connectionString1);
                    SqlCommand comm31 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                         " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                         "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                         " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                         "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                         "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                         " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con);
                    comm31.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                    comm31.Parameters.AddWithValue("@im1", PD.im1.Text);
                    comm31.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                    comm31.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                    comm31.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                    comm31.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                    comm31.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                    comm31.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                    comm31.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                    if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                    {
                        comm31.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                    }
                    else
                    {
                        comm31.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                    }
                    comm31.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                    con.Open();
                    Guid rpguid1 = (Guid)comm31.ExecuteScalar();
                    con.Close();
                    SqlCommand comm311 = new SqlCommand($@"update pol_persons set rperson_guid='{rpguid1}' where idguid='{perguid}' 
                        update pol_events set rperson_guid='{rpguid1}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where id={Vars.IdP})", con);

                    con.Open();
                    comm311.ExecuteNonQuery();
                    con.Close();
                }

            }
            else
            {
                var connectionString1 = Properties.Settings.Default.DocExchangeConnectionString;
                SqlConnection con = new SqlConnection(connectionString1);
                SqlCommand comm31 = new SqlCommand($@"update pol_persons set fam='{PD.fam1.Text}', im='{PD.im1.Text}', ot='{PD.ot1.Text}',
 dr=@dr, w={PD.pol_pr.SelectedIndex}, SROKDOVERENOSTI=@srok_d where idguid='{PD.rper}'
  update pol_documents set doctype={PD.doctype1.EditValue}, docser='{PD.docser1.Text}', docnum='{PD.docnum1.Text}', docdate='{PD.docdate1.DateTime}'
  where person_guid='{PD.rper}' 
update pol_events set rperson_guid='{PD.rper}' where person_guid='{perguid}' and idguid=(select event_guid from pol_persons where id={Vars.IdP})", con);

                comm31.Parameters.AddWithValue("@srok_d", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                {
                    comm31.Parameters.AddWithValue("@dr", "01-01-1900 00:00:00.000");
                }
                else
                {
                    comm31.Parameters.AddWithValue("@dr", PD.dr1.DateTime);
                }
                con.Open();
                comm31.ExecuteNonQuery();
                con.Close();
                //comm2.Parameters.AddWithValue("@rpguid", PD.rper);
            }

            //con2.Open();
            //comm2.ExecuteNonQuery();
            //con2.Close();
            if (PD.dop_doc == 0 && PD.ddnum.Text != "")
            {
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS(IDGUID, PERSON_GUID,  DOCTYPE, DOCSER, DOCNUM, DOCDATE,DOCEXP ,NAME_VP, event_guid,active,main)
                                values(newid(),(select idguid from pol_persons where id={Vars.IdP}),{PD.ddtype.EditValue},
'{PD.ddser.Text}','{PD.ddnum.Text}','{PD.dddate.EditValue}',@docexp1,'{PD.ddkemv.Text}',
                                (select event_guid from pol_persons where id={Vars.IdP}),1,0)
 
 
insert into pol_relation_doc_pers(PERSON_GUID, DOC_GUID, EVENT_GUID) values((select person_guid from pol_documents where id = SCOPE_IDENTITY()),
 (select idguid from pol_documents where id= SCOPE_IDENTITY()),
 (select event_guid from pol_documents where id= SCOPE_IDENTITY()) )", con);
                cmddoc.Parameters.AddWithValue("@docexp1", PD.docexp1.EditValue ?? DBNull.Value);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.dop_doc != 0)
            {
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS set   DOCTYPE={PD.ddtype.EditValue}, DOCSER='{PD.ddser.Text}', DOCNUM='{PD.ddnum.Text}', DOCDATE='{PD.dddate.EditValue}', DOCEXP=@docexp1,
NAME_VP='{PD.ddkemv.Text}' where person_guid=(select idguid from pol_persons where id={Vars.IdP}) and active=1 and main=0", con);
                con.Open();
                cmddoc.Parameters.AddWithValue("@docexp1", PD.docexp1.EditValue ?? DBNull.Value);
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.old_doc == 0 && PD.doc_num1.Text != "")
            {
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmddoc = new SqlCommand($@"insert into POL_DOCUMENTS(IDGUID, PERSON_GUID, OKSM, DOCTYPE, DOCSER, DOCNUM, DOCDATE, NAME_VP, NAME_VP_CODE, event_guid,active,main)
                                values(newid(),(select idguid from pol_persons where id={Vars.IdP}),'{PD.str_vid1.EditValue}',{PD.doc_type1.EditValue},
'{PD.doc_ser1.Text}','{PD.doc_num1.Text}','{PD.date_vid2.DateTime}','{PD.kem_vid1.Text}','{PD.kod_podr1.Text}',
                                (select event_guid from pol_oplist where person_guid=(select idguid from pol_persons where id={Vars.IdP})),0,0)
 update pol_documents set prevdocguid=(select idguid from pol_documents where id=SCOPE_IDENTITY()) where person_guid=(select idguid from pol_persons where id={Vars.IdP}) and active=1 and main=1
 
insert into pol_relation_doc_pers(PERSON_GUID, DOC_GUID, EVENT_GUID) values((select person_guid from pol_documents where id = SCOPE_IDENTITY()),
 (select idguid from pol_documents where id= SCOPE_IDENTITY()),
 (select event_guid from pol_documents where id= SCOPE_IDENTITY()) )", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();

            }
            else if (PD.old_doc != 0)
            {
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmddoc = new SqlCommand($@"update POL_DOCUMENTS set  OKSM='{PD.str_vid1.EditValue}', DOCTYPE={PD.doc_type1.EditValue}, DOCSER='{PD.doc_ser1.Text}', DOCNUM='{PD.doc_num1.Text}', DOCDATE='{PD.date_vid2.DateTime}', 
NAME_VP='{PD.kem_vid1.Text}', NAME_VP_CODE='{PD.kod_podr1.Text}', active=0,main=0 where idguid='{PD.old_doc_guid}'", con);
                con.Open();
                cmddoc.ExecuteNonQuery();
                con.Close();
            }

            if (PD.prev_persguid == Guid.Empty && PD.prev_fam.Text != "")
            {
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmdpers = new SqlCommand($@"insert into POL_PERSONS_OLD(IDGUID,PERSON_GUID, EVENT_GUID, FAM,IM,OT,W,DR,MR)
                                values(newid(),(select idguid from pol_persons where id={Vars.IdP}),(select event_guid from pol_persons where id={Vars.IdP}),'{PD.prev_fam.Text}','{PD.prev_im.Text}',
'{PD.prev_ot.Text}',{PD.prev_pol.EditValue},'{PD.prev_dr.DateTime}','{PD.prev_mr.Text}')", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else if(PD.prev_persguid != Guid.Empty && PD.prev_fam.Text != "")
            {
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmdpers = new SqlCommand($@"update POL_PERSONS set FAM='{PD.prev_fam.Text}',IM='{PD.prev_im.Text}',OT='{PD.prev_ot.Text}',W={PD.prev_pol.EditValue},DR='{PD.prev_dr.EditValue}',MR='{PD.prev_mr.Text}',ACTIVE=0
 where idguid='{PD.prev_persguid}'", con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }
            else
            {

            }

            if (PD.docexp1.EditValue == null)
            {
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmdpers =
                    new SqlCommand(
                        $@"update pol_documents set docexp = null where event_guid=(select event_guid from pol_persons where id={Vars.IdP}) and main=0",
                        con);
                con.Open();
                cmdpers.ExecuteNonQuery();
                con.Close();
            }

            string denttst = SPR.MyReader.SELECTVAIN($@"select dend from pol_polises where event_guid=(select event_guid from pol_persons where id = {Vars.IdP})", Properties.Settings.Default.DocExchangeConnectionString);

            if (string.IsNullOrEmpty(denttst))
            {
                SqlConnection con1 = new SqlConnection(connectionString);
                SqlCommand cmdpers1 =
                    new SqlCommand(
                        $@"update pol_polises set dend={PD.date_end.EditValue ?? DBNull.Value} where event_guid=(select event_guid from pol_persons where id={Vars.IdP})",
                        con1);
                con1.Open();
                cmdpers1.ExecuteNonQuery();
                con1.Close();
            }
            Item_Saved();
            PersData_Default(PD);
        }

        public static void SaveDD_bt2_prp1(MainWindow PD)
        {
            var connectionString = Properties.Settings.Default.DocExchangeConnectionString;
            SqlConnection con3 = new SqlConnection(connectionString);
            SqlCommand comm3 = new SqlCommand("update pol_persons set SROKDOVERENOSTI=@srok_doverenosti,PRIZNAKVIDACHI=@PRIZNAKVIDACHI,DATEVIDACHI=@DATEVIDACHI,ENP=@enp,FAM=@fam,IM=@im,OT=@ot,W=@w,DR=@dr,ss=@ss,mr=@mr,birth_oksm=@boksm,"
                + "c_oksm=@coksm,phone=@phone,email=@email,kateg=@kateg,dost=@dost,ddeath=@ddeath,rperson_guid=@rpguid,mo=@mo,dstart=@date_mo, DOP_COMMENT=@DOP_COMMENT, COMMENT=@COMMENT where id=@id_p " +

                "update pol_addresses set fias_l1=@FIAS_L1,fias_l3=@FIAS_L3,fias_l4=@FIAS_L4,fias_l6=@FIAS_L6,fias_l7=@FIAS_L7,fias_l90=@FIAS_L90," +
                "fias_l91=@FIAS_L91, dom=@DOM,korp=@KORP,ext=@EXT,kv=@KV,house_guid=@HOUSE_GUID where idguid=(select ADDR_GUID from pol_relation_addr_pers where addres_g=1 and person_guid=(select idguid from pol_persons where id=@id_p)) and " +
                "event_guid=(select event_guid from pol_persons where id =@id_p) " +

                "update pol_addresses set fias_l1=@FIAS_L1_1,fias_l3=@FIAS_L3_1,fias_l4=@FIAS_L4_1,fias_l6=@FIAS_L6_1,fias_l7=@FIAS_L7_1,fias_l90=@FIAS_L90_1," +
                "fias_l91=@FIAS_L91_1,  dom=@DOM_1,korp=@KORP_1,ext=@EXT_1,kv=@KV_1,house_guid=@HOUSE_GUID_1 where idguid=(select ADDR_GUID from pol_relation_addr_pers where addres_p=1 and person_guid=(select idguid from pol_persons where id=@id_p)) and " +
                "event_guid=(select event_guid from pol_persons where id =@id_p) " +

                "update pol_relation_addr_pers SET bomg=@bomg,addres_g=@addr_g,addres_p=@addr_p,dreg=@dreg where addres_g=1 and person_guid=(select idguid from pol_persons where id=@id_p) " +

                "update pol_relation_addr_pers SET bomg=@bomg,addres_g=@addr_g1,addres_p=@addr_p1,dreg=@dreg where addres_p=1 and person_guid=(select idguid from pol_persons where id=@id_p) " +

                "update pol_documents set oksm=@oksm,doctype=@doctype,docser=@docser,docnum=@docnam,docdate=@docdate,docexp=@docexp,name_vp=@name_vp,name_vp_code=@vp_code, " +
                "docmr=@docmr where person_guid=(select idguid from pol_persons where id=@id_p) and main=1" +

                "update pol_documents set doctype=@doctype1,docser=@docser1,docnum=@docnam1,docdate=@docdate1,docexp=@docexp1,name_vp=@name_vp1 " +
                 "where person_guid=(select idguid from pol_persons where id=@id_p) and main=0" +

                "update pol_polises set vpolis=@vpolis,spolis=@spolis,npolis=@npolis,dbeg=@dbeg,dend=@dend,dstop=@dstop,blank=@blank,dreceived=@dreceived " +
                "where person_guid=(select idguid from pol_persons where id=@id_p)" +

               "update pol_personsb set photo=@photo1 where person_guid=(select idguid from pol_persons where id =@id_p) and type=3" +

               "update pol_events set unload=0,tip_op=@tip_op where person_guid=(select idguid from pol_persons where id=@id_p)", con3);

            comm3.Parameters.AddWithValue("@enp", PD.enp.Text);
            comm3.Parameters.AddWithValue("@fam", PD.fam.Text);
            comm3.Parameters.AddWithValue("@im", PD.im.Text);
            comm3.Parameters.AddWithValue("@ot", PD.ot.Text);
            comm3.Parameters.AddWithValue("@w", PD.pol.SelectedIndex);
            comm3.Parameters.AddWithValue("@dr", PD.dr.DateTime);
            comm3.Parameters.AddWithValue("@mr", PD.mr2.Text);
            comm3.Parameters.AddWithValue("@boksm", PD.str_r.EditValue.ToString());
            comm3.Parameters.AddWithValue("@coksm", PD.gr.EditValue.ToString());
            comm3.Parameters.AddWithValue("@tip_op", Vars.CelVisit);
            comm3.Parameters.AddWithValue("@doctype1", PD.ddtype.EditValue);
            comm3.Parameters.AddWithValue("@docser1", PD.ddser.Text);
            comm3.Parameters.AddWithValue("@docnam1", PD.ddnum.Text);
            comm3.Parameters.AddWithValue("@docdate1", PD.dddate.EditValue ?? DBNull.Value);
            comm3.Parameters.AddWithValue("@name_vp1", PD.ddkemv.Text);
            comm3.Parameters.AddWithValue("@docexp1", PD.docexp1.EditValue ?? DBNull.Value);
            comm3.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
            comm3.Parameters.AddWithValue("@COMMENT", PD.comments.Text);
            comm3.Parameters.AddWithValue("@DOP_COMMENT", PD.Dop_comments.Text);
            if (PD.priznak_vidachi.IsChecked == true)
            {
                comm3.Parameters.AddWithValue("@PRIZNAKVIDACHI", "1");
            }
            else
            {
                comm3.Parameters.AddWithValue("@PRIZNAKVIDACHI", "0");
            }
            comm3.Parameters.AddWithValue("@DATEVIDACHI", PD.date_vidachi.DateTime);
            if (PD.mo_cmb.SelectedIndex != -1)
            {
                comm3.Parameters.AddWithValue("@mo", PD.mo_cmb.EditValue.ToString());
            }
            else
            {
                comm3.Parameters.AddWithValue("@mo", "");
            }
            //if (Convert.ToDateTime(date_mo.EditValue) == DateTime.MinValue || date_mo.EditValue == null)
            //{
            //    comm3.Parameters.AddWithValue("@date_mo", DBNull.Value);
            //}
            //else
            //{
            //    comm3.Parameters.AddWithValue("@date_mo", date_mo.EditValue);
            //}
            comm3.Parameters.AddWithValue("@date_mo", PD.date_mo.EditValue ?? DBNull.Value);
            if (Convert.ToDateTime(PD.ddeath.EditValue) == DateTime.MinValue || PD.ddeath.EditValue == null)
            {
                comm3.Parameters.AddWithValue("@ddeath", DBNull.Value);
            }
            else
            {
                comm3.Parameters.AddWithValue("@ddeath", PD.ddeath.DateTime);
            }

            comm3.Parameters.AddWithValue("@oksm", PD.str_vid.EditValue.ToString());
            comm3.Parameters.AddWithValue("@ss", PD.snils.Text);
            comm3.Parameters.AddWithValue("@phone", PD.phone.Text);
            comm3.Parameters.AddWithValue("@email", PD.email.Text);
            comm3.Parameters.AddWithValue("@kateg", PD.kat_zl.EditValue.ToString());
            comm3.Parameters.AddWithValue("@vpolis", PD.type_policy.EditValue.ToString());
            comm3.Parameters.AddWithValue("@spolis", PD.ser_blank.Text);
            comm3.Parameters.AddWithValue("@npolis", PD.num_blank.Text);
            comm3.Parameters.AddWithValue("@dbeg", PD.date_vid1.DateTime);
            if (Convert.ToDateTime(PD.date_end.EditValue) == DateTime.MinValue || PD.date_end.EditValue == null)
            {
                comm3.Parameters.AddWithValue("@dend", DBNull.Value);
            }
            else
            {
                comm3.Parameters.AddWithValue("@dend", PD.date_end.EditValue);
            }
            if (Convert.ToDateTime(PD.fakt_prekr.EditValue) == DateTime.MinValue || PD.fakt_prekr.EditValue == null)
            {
                comm3.Parameters.AddWithValue("@dstop", DBNull.Value);
            }
            else
            {
                comm3.Parameters.AddWithValue("@dstop", PD.fakt_prekr.EditValue);
            }
            comm3.Parameters.AddWithValue("@dreceived", PD.date_poluch.EditValue);
            if (PD.pustoy.IsChecked == true)
            {
                comm3.Parameters.AddWithValue("@blank", 1);
            }
            else
            {
                comm3.Parameters.AddWithValue("@blank", 0);
            }


            comm3.Parameters.AddWithValue("@doctype", PD.doc_type.EditValue);
            comm3.Parameters.AddWithValue("@docser", PD.doc_ser.Text);
            comm3.Parameters.AddWithValue("@docnam", PD.doc_num.Text);
            comm3.Parameters.AddWithValue("@docdate", PD.date_vid.DateTime);
            comm3.Parameters.AddWithValue("@docexp", PD.docexp.EditValue ?? DBNull.Value);
            comm3.Parameters.AddWithValue("@name_vp", PD.kem_vid.Text);
            comm3.Parameters.AddWithValue("@vp_code", PD.kod_podr.Text);
            comm3.Parameters.AddWithValue("@docmr", PD.mr2.Text);

            comm3.Parameters.AddWithValue("@id_p", Vars.IdP);
            if (PD.s == "" || PD.s == null)
            {
                comm3.Parameters.AddWithValue("@dost", "");
            }
            else
            {
                comm3.Parameters.AddWithValue("@dost", PD.s);
            }

            comm3.Parameters.AddWithValue("@FIAS_L1", PD.fias.reg.EditValue);
            if (PD.fias.reg_rn.EditValue == null)
            {
                comm3.Parameters.AddWithValue("@FIAS_L3", Guid.Empty);
            }
            else
            {
                comm3.Parameters.AddWithValue("@FIAS_L3", PD.fias.reg_rn.EditValue);
            }
            if (PD.fias.reg_town.EditValue == null)
            {
                comm3.Parameters.AddWithValue("@FIAS_L4", Guid.Empty);
            }
            else
            {
                comm3.Parameters.AddWithValue("@FIAS_L4", PD.fias.reg_town.EditValue);
            }

            if (PD.fias.reg_np.EditValue == null)
            {
                comm3.Parameters.AddWithValue("@FIAS_L6", Guid.Empty);
            }
            else
            {
                comm3.Parameters.AddWithValue("@FIAS_L6", PD.fias.reg_np.EditValue);
            }
            if (PD.fias.reg_ul.EditValue == null)
            {
                comm3.Parameters.AddWithValue("@FIAS_L7", Guid.Empty);
                comm3.Parameters.AddWithValue("@FIAS_L90", Guid.Empty);
                comm3.Parameters.AddWithValue("@FIAS_L91", Guid.Empty);
            }
            else
            {
                comm3.Parameters.AddWithValue("@FIAS_L7", PD.fias.reg_ul.EditValue);
                comm3.Parameters.AddWithValue("@FIAS_L90", PD.fias.reg_ul.EditValue);
                comm3.Parameters.AddWithValue("@FIAS_L91", PD.fias.reg_ul.EditValue);
            }
            if (PD.fias.reg_dom.EditValue == null)
            {
                comm3.Parameters.AddWithValue("@HOUSE_GUID", Guid.Empty);
            }
            else
            {
                comm3.Parameters.AddWithValue("@HOUSE_GUID", PD.fias.reg_dom.EditValue);
            }
            comm3.Parameters.AddWithValue("@DOM", PD.domsplit);
            comm3.Parameters.AddWithValue("@KORP", PD.fias.reg_korp.Text);
            comm3.Parameters.AddWithValue("@EXT", PD.fias.reg_str.Text);
            comm3.Parameters.AddWithValue("@KV", PD.fias.reg_kv.Text);
            if (PD.fias.bomj.IsChecked == true)
            {
                comm3.Parameters.AddWithValue("@bomg", 1);
                comm3.Parameters.AddWithValue("@addr_g", 0);

            }
            else
            {
                comm3.Parameters.AddWithValue("@bomg", 0);
                comm3.Parameters.AddWithValue("@addr_g", 1);
                if (PD.fias1.sovp_addr.IsChecked == true)
                {
                    comm3.Parameters.AddWithValue("@addr_p", 1);
                    comm3.Parameters.AddWithValue("@addr_p1", 1);
                    comm3.Parameters.AddWithValue("@addr_g1", 1);
                }
                else
                {
                    comm3.Parameters.AddWithValue("@addr_p", 0);
                    comm3.Parameters.AddWithValue("@addr_p1", 1);
                    comm3.Parameters.AddWithValue("@addr_g1", 0);
                }

            }
            if (PD.fias.reg_dr.EditValue == null)
            {
                comm3.Parameters.AddWithValue("@dreg", DBNull.Value);
            }
            else
            {
                comm3.Parameters.AddWithValue("@dreg", Convert.ToDateTime(PD.fias.reg_dr.EditValue));
            }
            if (PD.fias1.sovp_addr.IsChecked == true)
            {
                comm3.Parameters.AddWithValue("@FIAS_L1_1", PD.fias.reg.EditValue);
                if (PD.fias.reg_rn.EditValue == null)
                {
                    comm3.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
                }
                else
                {
                    comm3.Parameters.AddWithValue("@FIAS_L3_1", PD.fias.reg_rn.EditValue);
                }
                if (PD.fias.reg_town.EditValue == null)
                {
                    comm3.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
                }
                else
                {
                    comm3.Parameters.AddWithValue("@FIAS_L4_1", PD.fias.reg_town.EditValue);
                }
                if (PD.fias.reg_np.EditValue == null)
                {
                    comm3.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
                }
                else
                {
                    comm3.Parameters.AddWithValue("@FIAS_L6_1", PD.fias.reg_np.EditValue);
                }
                if (PD.fias1.reg_ul1.EditValue == null)
                {
                    comm3.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                    comm3.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                    comm3.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
                }
                else
                {
                    comm3.Parameters.AddWithValue("@FIAS_L7_1", PD.fias1.reg_ul1.EditValue);
                    comm3.Parameters.AddWithValue("@FIAS_L90_1", PD.fias1.reg_ul1.EditValue);
                    comm3.Parameters.AddWithValue("@FIAS_L91_1", PD.fias1.reg_ul1.EditValue);
                }
                if (PD.fias.reg_dom.EditValue == null)
                {
                    comm3.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
                }
                else
                {
                    comm3.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias.reg_dom.EditValue);
                }
                comm3.Parameters.AddWithValue("@DOM_1", PD.domsplit);
                comm3.Parameters.AddWithValue("@KORP_1", PD.fias.reg_korp.Text);
                comm3.Parameters.AddWithValue("@EXT_1", PD.fias.reg_str.Text);
                comm3.Parameters.AddWithValue("@KV_1", PD.fias.reg_kv.Text);
            }
            else
            {
                comm3.Parameters.AddWithValue("@FIAS_L1_1", PD.fias1.reg1.EditValue);
                if (PD.fias1.reg_rn1.EditValue == null)
                {
                    comm3.Parameters.AddWithValue("@FIAS_L3_1", Guid.Empty);
                }
                else
                {
                    comm3.Parameters.AddWithValue("@FIAS_L3_1", PD.fias1.reg_rn1.EditValue);
                }
                if (PD.fias1.reg_town1.EditValue == null)
                {
                    comm3.Parameters.AddWithValue("@FIAS_L4_1", Guid.Empty);
                }
                else
                {
                    comm3.Parameters.AddWithValue("@FIAS_L4_1", PD.fias1.reg_town1.EditValue);
                }
                if (PD.fias1.reg_np1.EditValue == null)
                {
                    comm3.Parameters.AddWithValue("@FIAS_L6_1", Guid.Empty);
                }
                else
                {
                    comm3.Parameters.AddWithValue("@FIAS_L6_1", PD.fias1.reg_np1.EditValue);
                }
                if (PD.fias1.reg_ul1.EditValue == null)
                {
                    comm3.Parameters.AddWithValue("@FIAS_L7_1", Guid.Empty);
                    comm3.Parameters.AddWithValue("@FIAS_L90_1", Guid.Empty);
                    comm3.Parameters.AddWithValue("@FIAS_L91_1", Guid.Empty);
                }
                else
                {
                    comm3.Parameters.AddWithValue("@FIAS_L7_1", PD.fias1.reg_ul1.EditValue);
                    comm3.Parameters.AddWithValue("@FIAS_L90_1", PD.fias1.reg_ul1.EditValue);
                    comm3.Parameters.AddWithValue("@FIAS_L91_1", PD.fias1.reg_ul1.EditValue);
                }
                if (PD.fias1.reg_dom1.EditValue == null)
                {
                    comm3.Parameters.AddWithValue("@HOUSE_GUID_1", Guid.Empty);
                }
                else
                {
                    comm3.Parameters.AddWithValue("@HOUSE_GUID_1", PD.fias1.reg_dom1.EditValue);
                }
                comm3.Parameters.AddWithValue("@DOM_1", PD.domsplit1);
                comm3.Parameters.AddWithValue("@KORP_1", PD.fias1.reg_korp1.Text);
                comm3.Parameters.AddWithValue("@EXT_1", PD.fias1.reg_str1.Text);
                comm3.Parameters.AddWithValue("@KV_1", PD.fias1.reg_kv1.Text);
            }




            if (PD.zl_podp.EditValue == null || PD.zl_podp.EditValue.ToString() == "")
            {
                comm3.Parameters.AddWithValue("@photo1", "");
            }
            else
            {
                comm3.Parameters.AddWithValue("@photo1", Convert.ToBase64String((byte[])PD.zl_podp.EditValue));
            }
            if (PD.pers_grid_2.SelectedItems.Count == 0 && PD.fam1.Text == "")
            {
                comm3.Parameters.AddWithValue("@rpguid", Guid.Empty);
            }
            else if (PD.pers_grid_2.SelectedItems.Count == 0 && PD.fam1.Text != "")
            {
                SqlCommand comm13 = new SqlCommand(@"select rperson_guid from pol_persons where id=@id_p", con3);
                comm13.Parameters.AddWithValue("@id_p", Vars.IdP);
                con3.Open();
                Guid rpguid1 = (Guid)comm13.ExecuteScalar();
                con3.Close();

                if (rpguid1 == Guid.Empty)
                {
                    SqlCommand comm32 = new SqlCommand("insert into pol_persons (IDGUID,parentguid,rperson_guid,FAM,IM,OT,phone,w,dr,active,SROKDOVERENOSTI)" +
                   " VALUES (newid(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',@fam1, @im1 ,@ot1,@phone1,@pol,@dr1,0,@srok_doverenosti) " +
                   "insert into pol_documents (idguid,PERSON_GUID,DOCTYPE,DOCSER,DOCNUM,DOCDATE)" +
                   " values(newid(),(select idguid from pol_persons where id=SCOPE_IDENTITY()),@doctype1,@docser1,@docnum1,@docdate1)" +
                   "insert into pol_relation_doc_pers (PERSON_GUID,DOC_GUID,DT)" +
                   "values((select PERSON_GUID from pol_documents where id=SCOPE_IDENTITY()),(select idguid from pol_documents where id=SCOPE_IDENTITY()),(select SYSDATETIME()))" +
                   " select PERSON_GUID from pol_relation_doc_pers where id =SCOPE_IDENTITY()", con3);
                    comm32.Parameters.AddWithValue("@fam1", PD.fam1.Text);
                    comm32.Parameters.AddWithValue("@im1", PD.im1.Text);
                    comm32.Parameters.AddWithValue("@ot1", PD.ot1.Text);
                    comm32.Parameters.AddWithValue("@phone1", PD.phone_p1.Text);
                    comm32.Parameters.AddWithValue("@doctype1", PD.doctype1.EditValue ?? 1);
                    comm32.Parameters.AddWithValue("@docser1", PD.docser1.Text);
                    comm32.Parameters.AddWithValue("@docnum1", PD.docnum1.Text);
                    comm32.Parameters.AddWithValue("@docdate1", PD.docdate1.DateTime);
                    comm32.Parameters.AddWithValue("@srok_doverenosti", PD.srok_doverenosti.EditValue ?? DBNull.Value);
                    if (Convert.ToDateTime(PD.dr1.EditValue) == DateTime.MinValue)
                    {
                        comm32.Parameters.AddWithValue("@dr1", "01-01-1900 00:00:00.000");
                    }
                    else
                    {
                        comm32.Parameters.AddWithValue("@dr1", PD.dr1.DateTime);
                    }
                    comm32.Parameters.AddWithValue("@pol", PD.pol_pr.SelectedIndex);
                    con3.Open();
                    Guid rpguid2 = (Guid)comm32.ExecuteScalar();

                    con3.Close();
                    rpguid1 = rpguid2;
                }
                else
                {

                }
                comm3.Parameters.AddWithValue("@rpguid", rpguid1);
            }
            else if (PD.pers_grid_2.SelectedItems.Count != 0 && PD.fam1.Text != "")
            {
                comm3.Parameters.AddWithValue("@rpguid", PD.pers_grid_2.GetFocusedRowCellValue("IDGUID").ToString());
            }


            con3.Open();
            comm3.ExecuteNonQuery();
            con3.Close();
            Item_Saved();
            //this.Close();
            //PD.pers_grid_1_Loaded(this, e);
            PersData_Default(PD);
        }

        public static void PersData_Default(MainWindow PD)
        {
            PD.prev_doc.Visibility = Visibility.Hidden;
            PD.tabs.SelectedIndex = 0;
            Vars.Btn = "1";
            PD.cel_vizita.EditValue = null;
            PD.sp_pod_z.EditValue = null;
            PD.d_obr.EditValue = DateTime.Today;
            PD.petition.IsChecked = false;
            PD.pr_pod_z_polis.EditValue = null;
            PD.form_polis.SelectedIndex = 1;
            PD.pr_pod_z_smo.SelectedIndex = -1;
            PD.fam.Text = "";
            PD.im.Text = "";
            PD.ot.Text = "";
            PD.dr.EditValue = null;
            PD.pol.EditValue = null;
            PD.doc_type.EditValue = 1;
            PD.doc_ser.Text = "";
            PD.doc_num.Text = "";
            PD.date_vid.EditValue = null;
            PD.str_vid.EditValue = null;
            PD.kem_vid.Text = "";
            PD.kod_podr.Text = "";
            PD.docexp.EditValue = null;
            PD.ddtype.EditValue = null;
            PD.ddser.Text = "";
            PD.ddnum.Text = "";
            PD.ddkemv.Text = "";
            PD.dddate.EditValue = null;
            PD.docexp1.EditValue = null;
            PD.mr2.Text = "";
            PD.str_r.EditValue = null;
            PD.gr.EditValue = null;
            PD.snils.Text = "";
            PD.enp.Text = "";
            PD.mo_cmb.EditValue = null;
            PD.date_mo.EditValue = null;
            PD.phone.Text = "";
            PD.dost1.Text = "";
            PD.ddeath.EditValue = null;
            PD.email.Text = "";
            PD.kat_zl.EditValue = null;
            PD.type_policy.EditValue = 2;
            PD.ser_blank.Text = "";
            PD.num_blank.Text = "";
            PD.date_vid1.EditValue = DateTime.Today;
            PD.date_poluch.EditValue = DateTime.Today;
            PD.date_end.EditValue = null;
            PD.fakt_prekr.EditValue = null;
            PD.fias.reg.EditValue = null;
            PD.fias.reg_dom.Text = "";
            PD.fias.reg_korp.Text = "";
            PD.fias.reg_str.Text = "";
            PD.fias.reg_kv.Text = "";
            PD.fias.reg_dr.EditValue = null;
            PD.fias1.reg1.EditValue = null;
            PD.fias1.reg_dom1.Text = "";
            PD.fias1.reg_korp1.Text = "";
            PD.fias1.reg_str1.Text = "";
            PD.fias1.reg_kv1.Text = "";
            PD.zl_photo.EditValue = null;
            PD.zl_podp.EditValue = null;
            PD.fam1.Text = "";
            PD.im1.Text = "";
            PD.ot1.Text = "";
            PD.pol_pr.EditValue = null;
            PD.dr1.EditValue = null;
            PD.phone_p1.Text = "";
            PD.doctype1.EditValue = null;
            PD.docser1.Text = "";
            PD.docnum1.Text = "";
            PD.docdate1.EditValue = null;
            PD.status_p2.SelectedIndex = -1;
            PD.doc_type1.EditValue = null;
            PD.doc_ser1.Text = "";
            PD.doc_num1.Text = "";
            PD.str_vid1.EditValue = null;
            PD.kem_vid1.Text = "";
            PD.date_vid2.EditValue = null;
            PD.kod_podr1.Text = "";
            PD.docexp2.EditValue = null;
            PD.prev_fam.Text = "";
            PD.prev_im.Text = "";
            PD.prev_ot.Text = "";
            PD.prev_pol.EditValue = null;
            PD.prev_dr.EditValue = null;
            PD.prev_mr.Text = "";
            PD.priznak_vidachi.IsChecked = false;
            PD.date_vidachi.EditValue = null;
            PD.srok_doverenosti.EditValue = null;
            PD.rper_load = 0;
            PD.rper = Guid.Empty;
            PD.pustoy_lbl.Visibility = Visibility.Hidden;
            PD.spolis_ = null;
            PD.prev_persguid = Guid.Empty;
            PD.old_doc = 0;
            PD.dop_doc = 0;
            PD.s = null;
            Vars.IdP = null;
        }

        public static void Item_Saved()
        {
            string m = "Запись успешно сохранена!";
            string t = "Сообщение!";
            int b = 1;
            Message me = new Message(m, t, b);
            me.ShowDialog();
        }
        public static void Item_Not_Saved()
        {
            string m = "Ошибка записи, указаны не все параметры! Проверьте, все ли поля заполнены! ";
            string t = "Ошибка!";
            int b = 1;
            Message me = new Message(m, t, b);
            me.ShowDialog();
        }
    }
}
