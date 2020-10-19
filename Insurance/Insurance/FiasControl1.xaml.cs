using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;
using Yamed.Server;

namespace Yamed.Control.Editors
{
    public class FiasStreet1
    {
        public Guid ID { get; set; }
        public string NAME { get; set; }
        public decimal LEVEL { get; set; }
    }

    public class FiasHouse1
    {
        public Guid HOUSEGUID { get; set; }
        public string NAME { get; set; }
    }

    public class FiasAddress1
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
    public partial class FiasControl1 : LayoutGroup
    {
        private readonly string _connectionString; 
        public FiasAddress1 Address;
        public string btn_;
        public FiasControl1()
        {
            InitializeComponent();
            sovp_addr.AllowUpdateTextBlockWhenPrinting = true;
            sovp_addr.EditValue=true;
            var conn = Insurance.Properties.Settings.Default.FIASConnectionString;


            _connectionString = conn;

           
            List<FiasStreet1> regionList = new List<FiasStreet1>();

            TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext(); //get UI thread context 
            var mekTask = Task.Factory.StartNew(() =>
            {
                try
                {
                    regionList = Reader2List.CustomSelect<FiasStreet1>(@"
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
                reg1.DataContext = regionList;
                reg1.NullText = $"Регион: {regionList.Count}";
            }, uiScheduler);

        }

        private void RnLoader(Guid id)
        {
            var rnList =
                Reader2List.CustomSelect<FiasStreet1>(
                    $@"
            SELECT AOGUID ID, FORMALNAME + ' - ' + aot.SOCRNAME NAME, AOLEVEL[LEVEL]
 FROM [dbo].[AddressObjects] ao
join [dbo].[AddressObjectTypes]
        aot on ao.SHORTNAME = aot.SCNAME and ao.AOLEVEL = aot.[LEVEL]
where PARENTGUID = '{
                        id}' AND LIVESTATUS = 1 and AOLEVEL = 3
  order by formalname", _connectionString);
            reg_rn1.DataContext = rnList;
            reg_rn1.NullText = $"Район: {rnList.Count}";
            reg_rn1.IsEnabled = rnList.Any();
        }

        private void CityLoader(Guid id)
        {
            var cityList =
                Reader2List.CustomSelect<FiasStreet1>(
                    $@"
            SELECT AOGUID ID, FORMALNAME + ' - ' + aot.SOCRNAME NAME, AOLEVEL[LEVEL]
 FROM [dbo].[AddressObjects] ao
join [dbo].[AddressObjectTypes]
        aot on ao.SHORTNAME = aot.SCNAME and ao.AOLEVEL = aot.[LEVEL]
where PARENTGUID = '{
                        id}' AND LIVESTATUS = 1 and AOLEVEL = 4
  order by formalname", _connectionString);
            reg_town1.DataContext = cityList;
            reg_town1.NullText = $"Город: {cityList.Count}";
            reg_town1.IsEnabled = cityList.Any();

        }

        private void CityRnLoader(Guid id)
        {
            var cityRnList =
                Reader2List.CustomSelect<FiasStreet1>(
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
                Reader2List.CustomSelect<FiasStreet1>(
                    $@"
            SELECT AOGUID ID, FORMALNAME + ' - ' + aot.SOCRNAME NAME, AOLEVEL[LEVEL]
 FROM [dbo].[AddressObjects] ao
join [dbo].[AddressObjectTypes]
        aot on ao.SHORTNAME = aot.SCNAME and ao.AOLEVEL = aot.[LEVEL]
where PARENTGUID = '{
                        id}' AND LIVESTATUS = 1 and AOLEVEL = 6
  order by formalname", _connectionString);
            reg_np1.DataContext = nasPunktnList;
            reg_np1.NullText = $"Населенный пункт: {nasPunktnList.Count}";
            reg_np1.IsEnabled = nasPunktnList.Any();
        }

        private void UlLoader(Guid id)
        {
            var ulList =Reader2List.CustomSelect<FiasStreet1>(
                    $@"
            SELECT AOGUID ID, FORMALNAME + ' - ' + aot.SOCRNAME NAME, AOLEVEL[LEVEL]
 FROM [dbo].[AddressObjects] ao
join [dbo].[AddressObjectTypes]
        aot on ao.SHORTNAME = aot.SCNAME and ao.AOLEVEL = aot.[LEVEL]
where PARENTGUID = '{
                        id}' AND LIVESTATUS = 1 and AOLEVEL = 7
  order by formalname", _connectionString);
            reg_ul1.DataContext = ulList;
            reg_ul1.NullText = $"Улица: {ulList.Count}";
            reg_ul1.IsEnabled = ulList.Any();

        }

        private void DopUlLoader(Guid id)
        {
            var dopUlList =
                Reader2List.CustomSelect<FiasStreet1>(
                    $@"
            SELECT AOGUID ID, FORMALNAME + ' - ' + aot.SOCRNAME NAME, AOLEVEL[LEVEL]
 FROM [dbo].[AddressObjects] ao
join [dbo].[AddressObjectTypes]
        aot on ao.SHORTNAME = aot.SCNAME and ao.AOLEVEL = aot.[LEVEL]
where PARENTGUID = '{
                        id}' AND LIVESTATUS = 1 and AOLEVEL in (90,91)
  order by formalname", _connectionString);
            DopUlBoxEdit1.DataContext = dopUlList;
            DopUlBoxEdit1.NullText = $"Доп. адресообразующий элемент: {dopUlList.Count}";
            DopUlBoxEdit1.IsEnabled = dopUlList.Any();
        }

        private void HouseLoader(Guid id)
        {
            var houseList =
                Reader2List.CustomSelect<FiasHouse1>(
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
WHERE aoguid='{id}'

SELECT *
FROM @tt
GROUP BY NAME,HOUSEGUID", _connectionString);
            reg_dom1.DataContext = houseList;
            reg_dom1.NullText = $"Дом: {houseList.Count}";
            reg_dom1.IsEnabled = houseList.Any();

        }

        private void RegionBoxEdit_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var value = ((BaseEdit)sender).EditValue;
            if (value != null)
            {
                reg_rn1.EditValue = null;
                reg_town1.EditValue = null;
                CityRnBoxEdit.EditValue = null;
                reg_np1.EditValue = null;
                reg_ul1.EditValue = null;
                DopUlBoxEdit1.EditValue = null;
                reg_dom1.EditValue = null;


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
                reg_rn1.EditValue = null;
                reg_rn1.DataContext = null;
                reg_rn1.NullText = $"Район: {0}";
                reg_town1.EditValue = null;
                reg_town1.DataContext = null;
                reg_town1.NullText = $"Город: {0}";
                CityRnBoxEdit.EditValue = null;
                CityRnBoxEdit.DataContext = null;
                CityRnBoxEdit.NullText = $"Внутригородской район: {0}";
                reg_np1.EditValue = null;
                reg_np1.DataContext = null;
                reg_np1.NullText = $"Населенный пункт: {0}";
                reg_ul1.EditValue = null;
                reg_ul1.DataContext = null;
                reg_ul1.NullText = $"Улица: {0}";
                DopUlBoxEdit1.EditValue = null;
                DopUlBoxEdit1.DataContext = null;
                DopUlBoxEdit1.NullText = $"Доп. адресообразующий элемент: {0}";
                reg_dom1.EditValue = null;
                reg_dom1.DataContext = null;
                reg_dom1.NullText = $"Дом: {0}";


            }
        }

        private void RnBoxEdit_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var value = ((BaseEdit)sender).EditValue;
            if (value != null)
            {
                reg_town1.EditValue = null;
                CityRnBoxEdit.EditValue = null;
                reg_np1.EditValue = null;
                reg_ul1.EditValue = null;
                DopUlBoxEdit1.EditValue = null;
                reg_dom1.EditValue = null;


                CityLoader((Guid)value);
                CityRnLoader((Guid)value);
                NasPunktLoader((Guid)value);
                UlLoader((Guid)value);
                DopUlLoader((Guid)value);
                HouseLoader((Guid)value);
            }
            else
            {
                reg_town1.EditValue = null;
                reg_town1.DataContext = null;
                reg_town1.NullText = $"Город: {0}";
                CityRnBoxEdit.EditValue = null;
                CityRnBoxEdit.DataContext = null;
                CityRnBoxEdit.NullText = $"Внутригородской район: {0}";
                reg_np1.EditValue = null;
                reg_np1.DataContext = null;
                reg_np1.NullText = $"Населенный пункт: {0}";
                reg_ul1.EditValue = null;
                reg_ul1.DataContext = null;
                reg_ul1.NullText = $"Улица: {0}";
                DopUlBoxEdit1.EditValue = null;
                DopUlBoxEdit1.DataContext = null;
                DopUlBoxEdit1.NullText = $"Доп. адресообразующий элемент: {0}";
                reg_dom1.EditValue = null;
                reg_dom1.DataContext = null;
                reg_dom1.NullText = $"Дом: {0}";

            }
        }

        private void CityBoxEdit_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var value = ((BaseEdit)sender).EditValue;
            if (value != null)
            {
                CityRnBoxEdit.EditValue = null;
                reg_np1.EditValue = null;
                reg_ul1.EditValue = null;
                DopUlBoxEdit1.EditValue = null;
                reg_dom1.EditValue = null;

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
                reg_np1.EditValue = null;
                reg_np1.DataContext = null;
                reg_np1.NullText = $"Населенный пункт: {0}";
                reg_ul1.EditValue = null;
                reg_ul1.DataContext = null;
                reg_ul1.NullText = $"Улица: {0}";
                DopUlBoxEdit1.EditValue = null;
                DopUlBoxEdit1.DataContext = null;
                DopUlBoxEdit1.NullText = $"Доп. адресообразующий элемент: {0}";
                reg_dom1.EditValue = null;
                reg_dom1.DataContext = null;
                reg_dom1.NullText = $"Дом: {0}";

            }
        }

        private void CityRnBoxEdit_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var value = ((BaseEdit)sender).EditValue;
            if (value != null)
            {
                reg_np1.EditValue = null;
                reg_ul1.EditValue = null;
                DopUlBoxEdit1.EditValue = null;
                reg_dom1.EditValue = null;


                NasPunktLoader((Guid)value);
                UlLoader((Guid)value);
                DopUlLoader((Guid)value);
                HouseLoader((Guid)value);
            }
            else
            {
                reg_np1.EditValue = null;
                reg_np1.DataContext = null;
                reg_np1.NullText = $"Населенный пункт: {0}";
                reg_ul1.EditValue = null;
                reg_ul1.DataContext = null;
                reg_ul1.NullText = $"Улица: {0}";
                DopUlBoxEdit1.EditValue = null;
                DopUlBoxEdit1.DataContext = null;
                DopUlBoxEdit1.NullText = $"Доп. адресообразующий элемент: {0}";
                reg_dom1.EditValue = null;
                reg_dom1.DataContext = null;
                reg_dom1.NullText = $"Дом: {0}";

            }
        }

        private void NasPunktBoxEdit_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var value = ((BaseEdit)sender).EditValue;
            if (value != null)
            {
                reg_ul1.EditValue = null;
                DopUlBoxEdit1.EditValue = null;
                reg_dom1.EditValue = null;

                UlLoader((Guid)value);
                DopUlLoader((Guid)value);
                HouseLoader((Guid)value);
            }
            else
            {
                reg_ul1.EditValue = null;
                reg_ul1.DataContext = null;
                reg_ul1.NullText = $"Улица: {0}";
                DopUlBoxEdit1.EditValue = null;
                DopUlBoxEdit1.DataContext = null;
                DopUlBoxEdit1.NullText = $"Доп. адресообразующий элемент: {0}";
                reg_dom1.EditValue = null;
                reg_dom1.DataContext = null;
                reg_dom1.NullText = $"Дом: {0}";

            }
        }

        private void UlBoxEdit_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var value = ((BaseEdit)sender).EditValue;
            if (value != null)
            {
                DopUlBoxEdit1.EditValue = null;
                reg_dom1.EditValue = null;
                HouseLoader((Guid)value);
            }
            else
            {
                reg_dom1.EditValue = null;
                reg_dom1.DataContext = null;
                reg_dom1.NullText = $"Дом: {0}";
            }
            var test = Address;

        }

        private void DopUlBoxEdit_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var value = ((BaseEdit)sender).EditValue;
            if (value != null)
            {
                reg_ul1.EditValue = null;
                reg_dom1.EditValue = null;
                //HouseLoader((Guid)value);
            }
            else
            {
                reg_dom1.EditValue = null;
                reg_dom1.DataContext = null;
                reg_dom1.NullText = $"Дом: {0}";
            }
        }

    

        private void sovp_addr_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key==System.Windows.Input.Key.Enter)
            {
                if(sovp_addr.IsChecked==true)
                {
                    sovp_addr.AllowUpdateTextBlockWhenPrinting = true;
                }
            }
        }

        private void sovp_addr_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            sovp_addr.AllowUpdateTextBlockWhenPrinting = false;
        }

        private void reg_kv1_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            sovp_addr.AllowUpdateTextBlockWhenPrinting = true;
        }
        public string[] dstrkor1;
        public string domsplit1="";
        private void Reg_dom1_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var eq = reg_ul1.EditValue;
            reg_korp1.Text = "";
            reg_str1.Text = "";
            dstrkor1 = reg_dom1.Text.Replace(" ",",").Split(',');
            if (dstrkor1.Length > 1)
            {
                reg_korp1.Text = dstrkor1[1].Replace("корп.", "");
            }
            if (dstrkor1.Length > 2)
            {
                reg_str1.Text = dstrkor1[2].Replace("стр.", "");
            }
            domsplit1 = dstrkor1[0].Replace("д.", "");
        }
    }
}
