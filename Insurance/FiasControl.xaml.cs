using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;
using Yamed.Server;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace Yamed.Control.Editors
{
    
    public class FiasStreet
    {
        public Guid ID { get; set; }
        public string NAME { get; set; }
        public decimal LEVEL { get; set; }
    }

    public class FiasHouse
    {
        public Guid HOUSEGUID { get; set; }
        public string NAME { get; set; }
       
    }

    public class FiasAddress
    {
        public Guid? Region { get; set; }
        public Guid? Rn { get; set; }
        public Guid? City { get; set; }
        public Guid? CityRn { get; set; }
        public Guid? NasPun { get; set; }
        public Guid? Ul { get; set; }
        public Guid? DopUl { get; set; }
        public string House { get; set; }
        
    }

    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class FiasControl : LayoutGroup
    {
        
        public bool from_fias = false;
        private readonly string _connectionString; 
        public FiasAddress Address;
        public string btn_1;
        public FiasControl()
        {
            InitializeComponent();
            bomj.AllowUpdateTextBlockWhenPrinting = false;
            var conn = Insurance.Properties.Settings.Default.FIASConnectionString;


            _connectionString = conn;
            List<FiasStreet> regionList = new List<FiasStreet>();

            TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext(); //get UI thread context 
            var mekTask = Task.Factory.StartNew(() =>
            {
                try
                {
                    regionList = Reader2List.CustomSelect<FiasStreet>(@"
SELECT AOGUID ID, FORMALNAME + ' - ' + aot.SOCRNAME NAME, AOLEVEL [LEVEL]
  FROM [dbo].[AddressObjects] ao
  join [dbo].[AddressObjectTypes] aot on ao.SHORTNAME = aot.SCNAME and ao.AOLEVEL = aot.[LEVEL]
  where AOLEVEL = 1 AND LIVESTATUS = 1
  order by formalname", _connectionString);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
                Dispatcher.BeginInvoke((Action)delegate ()
                {

                });
            });
            mekTask.ContinueWith(x =>
            {
                reg.DataContext = regionList;
                reg.NullText = $"Регион: {regionList.Count}";
            }, uiScheduler);

        }

        private void RnLoader(Guid id)
        {
            var rnList =
                Reader2List.CustomSelect<FiasStreet>(
                    $@"
            SELECT AOGUID ID, FORMALNAME + ' - ' + aot.SOCRNAME NAME, AOLEVEL[LEVEL]
 FROM [dbo].[AddressObjects] ao
join [dbo].[AddressObjectTypes]
        aot on ao.SHORTNAME = aot.SCNAME and ao.AOLEVEL = aot.[LEVEL]
where PARENTGUID = '{
                        id}' AND LIVESTATUS = 1 and AOLEVEL = 3
  order by formalname", _connectionString);
            reg_rn.DataContext = rnList;
            reg_rn.NullText = $"Район: {rnList.Count}";
            reg_rn.IsEnabled = rnList.Any();
        }

        private void CityLoader(Guid id)
        {
            var cityList =
                Reader2List.CustomSelect<FiasStreet>(
                    $@"
            SELECT AOGUID ID, FORMALNAME + ' - ' + aot.SOCRNAME NAME, AOLEVEL[LEVEL]
 FROM [dbo].[AddressObjects] ao
join [dbo].[AddressObjectTypes]
        aot on ao.SHORTNAME = aot.SCNAME and ao.AOLEVEL = aot.[LEVEL]
where PARENTGUID = '{
                        id}' AND LIVESTATUS = 1 and AOLEVEL = 4 
  order by formalname", _connectionString);
            reg_town.DataContext = cityList;
            reg_town.NullText = $"Город: {cityList.Count}";
            reg_town.IsEnabled = cityList.Any();

        }

        private void CityRnLoader(Guid id)
        {
            var cityRnList =
                Reader2List.CustomSelect<FiasStreet>(
                    $@"
            SELECT AOGUID ID, FORMALNAME + ' - ' + aot.SOCRNAME NAME, AOLEVEL[LEVEL]
 FROM [dbo].[AddressObjects] ao
join [dbo].[AddressObjectTypes]
        aot on ao.SHORTNAME = aot.SCNAME and ao.AOLEVEL = aot.[LEVEL]
where PARENTGUID = '{
                        id}' AND LIVESTATUS = 1 and AOLEVEL = 5
  order by formalname", _connectionString);
            CityRnBoxEdit.DataContext = cityRnList;
            CityRnBoxEdit.NullText = $"Внутригородской район: {cityRnList.Count}";
            CityRnBoxEdit.IsEnabled = cityRnList.Any();

        }

        private void NasPunktLoader(Guid id)
        {
            var nasPunktnList =
                Reader2List.CustomSelect<FiasStreet>(
                    $@"
            SELECT AOGUID ID, FORMALNAME + ' - ' + aot.SOCRNAME NAME, AOLEVEL[LEVEL]
 FROM [dbo].[AddressObjects] ao
join [dbo].[AddressObjectTypes]
        aot on ao.SHORTNAME = aot.SCNAME and ao.AOLEVEL = aot.[LEVEL]
where PARENTGUID = '{
                        id}' AND LIVESTATUS = 1 and AOLEVEL in (6,65)
  order by formalname", _connectionString);
            reg_np.DataContext = nasPunktnList;
            reg_np.NullText = $"Населенный пункт: {nasPunktnList.Count}";
            reg_np.IsEnabled = nasPunktnList.Any();
        }

        private void UlLoader(Guid id)
        {
            var ulList =Reader2List.CustomSelect<FiasStreet>(
                    $@"
            SELECT AOGUID ID, FORMALNAME + ' - ' + aot.SOCRNAME NAME, AOLEVEL[LEVEL]
 FROM [dbo].[AddressObjects] ao
join [dbo].[AddressObjectTypes]
        aot on ao.SHORTNAME = aot.SCNAME and ao.AOLEVEL = aot.[LEVEL]
where PARENTGUID = '{
                        id}' AND LIVESTATUS = 1 and AOLEVEL = 7 
  order by formalname", _connectionString);
            reg_ul.DataContext = ulList;
            reg_ul.NullText = $"Улица: {ulList.Count}";
            reg_ul.IsEnabled = ulList.Any();

        }

        private void DopUlLoader(Guid id)
        {
            var dopUlList =
                Reader2List.CustomSelect<FiasStreet>(
                    $@"
            SELECT AOGUID ID, FORMALNAME + ' - ' + aot.SOCRNAME NAME, AOLEVEL[LEVEL]
 FROM [dbo].[AddressObjects] ao
join [dbo].[AddressObjectTypes]
        aot on ao.SHORTNAME = aot.SCNAME and ao.AOLEVEL = aot.[LEVEL]
where PARENTGUID = '{
                        id}' AND LIVESTATUS = 1 and AOLEVEL in (90,91)
  order by formalname", _connectionString);
            DopUlBoxEdit.DataContext = dopUlList;
            DopUlBoxEdit.NullText = $"Доп. адресообразующий элемент: {dopUlList.Count}";
            DopUlBoxEdit.IsEnabled = dopUlList.Any();
        }

        private void HouseLoader(Guid id)
        {
            var houseList =
                Reader2List.CustomSelect<FiasHouse>(
                    $@"
DECLARE @tt TABLE
(HOUSEGUID uniqueidentifier,
NAME NVARCHAR(20)
)
DECLARE @start INT
DECLARE @end INT
DECLARE @status INT

DECLARE vendor_cursor CURSOR FOR 
SELECT [INTSTART]
      ,[INTEND]      
      ,[INTSTATUS]      
FROM [dbo].[HouseIntervals]
WHERE aoguid = '{id}'

OPEN vendor_cursor
FETCH NEXT FROM vendor_cursor INTO @start, @end, @status;
WHILE @@FETCH_STATUS = 0
BEGIN
	
	WHILE @start <= @end
	BEGIN
		IF (@status = 1) OR (@status = 2)
		BEGIN
			INSERT INTO @tt (NAME) VALUES ('д. ' + CONVERT(NVARCHAR(20), @start));
			SET @start = @start + 2;
		END ELSE IF (@status = 3)
		BEGIN
			INSERT INTO @tt (NAME) VALUES ('д. ' + CONVERT(NVARCHAR(20), @start));
			SET @start = @start + 1;
		END;	
	END;	
	
	FETCH NEXT FROM vendor_cursor INTO @start, @end, @status;
END;
CLOSE vendor_cursor;
DEALLOCATE vendor_cursor;

INSERT INTO @tt (HOUSEGUID,NAME)
SELECT HOUSEGUID, ISNULL('д.' + housenum, '') + ISNULL(' корп.' + BUILDNUM , ' ') + ISNULL(' стр.' + STRUCNUM , ' ')
FROM [dbo].[Houses]
WHERE aoguid='{id}' and ENDDATE>getdate()

SELECT *
FROM @tt
GROUP BY NAME,HOUSEGUID", _connectionString);
            reg_dom.DataContext = houseList;
            reg_dom.NullText = $"Дом: {houseList.Count}";
            reg_dom.IsEnabled = houseList.Any();

        }

        private void RegionBoxEdit_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var value = ((BaseEdit)sender).EditValue;
            if (value != null)
            {
                reg_rn.EditValue = null;
                reg_town.EditValue = null;
                CityRnBoxEdit.EditValue = null;
                reg_np.EditValue = null;
                reg_ul.EditValue = null;
                DopUlBoxEdit.EditValue = null;
                reg_dom.EditValue = null;


                RnLoader((Guid)value);
                CityLoader((Guid)value);
                CityRnLoader((Guid)value);
                NasPunktLoader((Guid)value);
                UlLoader((Guid)value);
                DopUlLoader((Guid)value);
                HouseLoader((Guid)value);
            }
            else
            {
                reg_rn.EditValue = null;
                reg_rn.DataContext = null;
                reg_rn.NullText = $"Район: {0}";
                reg_town.EditValue = null;
                reg_town.DataContext = null;
                reg_town.NullText = $"Город: {0}";
                CityRnBoxEdit.EditValue = null;
                CityRnBoxEdit.DataContext = null;
                CityRnBoxEdit.NullText = $"Внутригородской район: {0}";
                reg_np.EditValue = null;
                reg_np.DataContext = null;
                reg_np.NullText = $"Населенный пункт: {0}";
                reg_ul.EditValue = null;
                reg_ul.DataContext = null;
                reg_ul.NullText = $"Улица: {0}";
                DopUlBoxEdit.EditValue = null;
                DopUlBoxEdit.DataContext = null;
                DopUlBoxEdit.NullText = $"Доп. адресообразующий элемент: {0}";
                reg_dom.EditValue = null;
                reg_dom.DataContext = null;
                reg_dom.NullText = $"Дом: {0}";


            }
        }

        private void RnBoxEdit_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var value = ((BaseEdit)sender).EditValue;
            if (value != null)
            {
                reg_town.EditValue = null;
                CityRnBoxEdit.EditValue = null;
                reg_np.EditValue = null;
                reg_ul.EditValue = null;
                DopUlBoxEdit.EditValue = null;
                reg_dom.EditValue = null;


                CityLoader((Guid)value);
                CityRnLoader((Guid)value);
                NasPunktLoader((Guid)value);
                UlLoader((Guid)value);
                DopUlLoader((Guid)value);
                HouseLoader((Guid)value);
            }
            else
            {
                reg_town.EditValue = null;
                reg_town.DataContext = null;
                reg_town.NullText = $"Город: {0}";
                CityRnBoxEdit.EditValue = null;
                CityRnBoxEdit.DataContext = null;
                CityRnBoxEdit.NullText = $"Внутригородской район: {0}";
                reg_np.EditValue = null;
                reg_np.DataContext = null;
                reg_np.NullText = $"Населенный пункт: {0}";
                reg_ul.EditValue = null;
                reg_ul.DataContext = null;
                reg_ul.NullText = $"Улица: {0}";
                DopUlBoxEdit.EditValue = null;
                DopUlBoxEdit.DataContext = null;
                DopUlBoxEdit.NullText = $"Доп. адресообразующий элемент: {0}";
                reg_dom.EditValue = null;
                reg_dom.DataContext = null;
                reg_dom.NullText = $"Дом: {0}";

            }
        }

        private void CityBoxEdit_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var value = ((BaseEdit)sender).EditValue;
            if (value != null)
            {
                CityRnBoxEdit.EditValue = null;
                reg_np.EditValue = null;
                reg_ul.EditValue = null;
                DopUlBoxEdit.EditValue = null;
                reg_dom.EditValue = null;

                CityRnLoader((Guid)value);
                NasPunktLoader((Guid)value);
                UlLoader((Guid)value);
                DopUlLoader((Guid)value);
                HouseLoader((Guid)value);
            }
            else
            {
                CityRnBoxEdit.EditValue = null;
                CityRnBoxEdit.DataContext = null;
                CityRnBoxEdit.NullText = $"Внутригородской район: {0}";
                reg_np.EditValue = null;
                reg_np.DataContext = null;
                reg_np.NullText = $"Населенный пункт: {0}";
                reg_ul.EditValue = null;
                reg_ul.DataContext = null;
                reg_ul.NullText = $"Улица: {0}";
                DopUlBoxEdit.EditValue = null;
                DopUlBoxEdit.DataContext = null;
                DopUlBoxEdit.NullText = $"Доп. адресообразующий элемент: {0}";
                reg_dom.EditValue = null;
                reg_dom.DataContext = null;
                reg_dom.NullText = $"Дом: {0}";

            }
        }

        private void CityRnBoxEdit_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var value = ((BaseEdit)sender).EditValue;
            if (value != null)
            {
                reg_np.EditValue = null;
                reg_ul.EditValue = null;
                DopUlBoxEdit.EditValue = null;
                reg_dom.EditValue = null;


                NasPunktLoader((Guid)value);
                UlLoader((Guid)value);
                DopUlLoader((Guid)value);
                HouseLoader((Guid)value);
            }
            else
            {
                reg_np.EditValue = null;
                reg_np.DataContext = null;
                reg_np.NullText = $"Населенный пункт: {0}";
                reg_ul.EditValue = null;
                reg_ul.DataContext = null;
                reg_ul.NullText = $"Улица: {0}";
                DopUlBoxEdit.EditValue = null;
                DopUlBoxEdit.DataContext = null;
                DopUlBoxEdit.NullText = $"Доп. адресообразующий элемент: {0}";
                reg_dom.EditValue = null;
                reg_dom.DataContext = null;
                reg_dom.NullText = $"Дом: {0}";

            }
        }

        private void NasPunktBoxEdit_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var value = ((BaseEdit)sender).EditValue;
            if (value != null)
            {
                reg_ul.EditValue = null;
                DopUlBoxEdit.EditValue = null;
                reg_dom.EditValue = null;

                UlLoader((Guid)value);
                DopUlLoader((Guid)value);
                HouseLoader((Guid)value);
            }
            else
            {
                reg_ul.EditValue = null;
                reg_ul.DataContext = null;
                reg_ul.NullText = $"Улица: {0}";
                DopUlBoxEdit.EditValue = null;
                DopUlBoxEdit.DataContext = null;
                DopUlBoxEdit.NullText = $"Доп. адресообразующий элемент: {0}";
                reg_dom.EditValue = null;
                reg_dom.DataContext = null;
                reg_dom.NullText = $"Дом: {0}";

            }
        }

        private void UlBoxEdit_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var value = ((BaseEdit)sender).EditValue;
            if (value != null)
            {
                DopUlBoxEdit.EditValue = null;
                reg_dom.EditValue = null;
                HouseLoader((Guid)value);
            }
            else
            {
                reg_dom.EditValue = null;
                reg_dom.DataContext = null;
                reg_dom.NullText = $"Дом: {0}";
            }
            var test = Address;

        }

        private void DopUlBoxEdit_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var value = ((BaseEdit)sender).EditValue;
            if (value != null)
            {
                reg_ul.EditValue = null;
                reg_dom.EditValue = null;
                //HouseLoader((Guid)value);
            }
            else
            {
                reg_dom.EditValue = null;
                reg_dom.DataContext = null;
                reg_dom.NullText = $"Дом: {0}";
            }
        }

        public delegate void CallMethod(bool ttab);
        public  CallMethod Method;
        public bool ttab = false;

        private void bomj_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            

            if (e.Key == Key.Enter)
            {
                //string iid = "";
                //string btn = "";
                //string ppol = "0";
                //string cel_viz = "";
                //string sppz = "";
                //string dobr = "";
                //string petit = "";
                //string prz_cod = "";

                //Insurance.Person_Data w2 = new Insurance.Person_Data();
                //w2.tabs.SelectedIndex = 3;
                //ttab = true;
                //if (Method != null)
                //{
                //    Method(ttab);
                //}
                //bomj.AllowUpdateTextBlockWhenPrinting = true;   

            }
        }

        private void reg_dr_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                bomj.AllowUpdateTextBlockWhenPrinting = true;
                
            }
        }
        

        public string[] dstrkor;
        public string domsplit="";
        private void Reg_dom_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            reg_korp.Text = "";
            reg_str.Text = "";
            dstrkor = reg_dom.Text.Replace(" ",",").Split(',');
            if (dstrkor.Length > 1)
            {
                reg_korp.Text = dstrkor[1].Replace("корп.", "");
            }
            if(dstrkor.Length>2)
            {
                reg_str.Text = dstrkor[2].Replace("стр.", "");
            }
            domsplit= dstrkor[0].Replace("д.", "");
            

        }
    }
}
