namespace Insurance
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AddressObjects
    {
        public int ID { get; set; }

        public Guid AOGUID { get; set; }

        [Required]
        [StringLength(120)]
        public string FORMALNAME { get; set; }

        [Required]
        [StringLength(2)]
        public string REGIONCODE { get; set; }

        [Required]
        [StringLength(1)]
        public string AUTOCODE { get; set; }

        [Required]
        [StringLength(3)]
        public string AREACODE { get; set; }

        [Required]
        [StringLength(3)]
        public string CITYCODE { get; set; }

        [Required]
        [StringLength(3)]
        public string CTARCODE { get; set; }

        [Required]
        [StringLength(3)]
        public string PLACECODE { get; set; }

        [Required]
        [StringLength(4)]
        public string STREETCODE { get; set; }

        [Required]
        [StringLength(4)]
        public string EXTRCODE { get; set; }

        [Required]
        [StringLength(3)]
        public string SEXTCODE { get; set; }

        [StringLength(120)]
        public string OFFNAME { get; set; }

        [StringLength(6)]
        public string POSTALCODE { get; set; }

        [StringLength(4)]
        public string IFNSFL { get; set; }

        [StringLength(4)]
        public string TERRIFNSFL { get; set; }

        [StringLength(4)]
        public string IFNSUL { get; set; }

        [StringLength(4)]
        public string TERRIFNSUL { get; set; }

        [StringLength(11)]
        public string OKATO { get; set; }

        [StringLength(11)]
        public string OKTMO { get; set; }

        public DateTime UPDATEDATE { get; set; }

        [Required]
        [StringLength(10)]
        public string SHORTNAME { get; set; }

        [Column(TypeName = "numeric")]
        public decimal AOLEVEL { get; set; }

        public Guid? PARENTGUID { get; set; }

        public Guid AOID { get; set; }

        public Guid? PREVID { get; set; }

        public Guid? NEXTID { get; set; }

        [StringLength(17)]
        public string CODE { get; set; }

        [StringLength(15)]
        public string PLAINCODE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal ACTSTATUS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal CENTSTATUS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal OPERSTATUS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal CURRSTATUS { get; set; }

        public DateTime STARTDATE { get; set; }

        public DateTime ENDDATE { get; set; }

        public Guid? NORMDOC { get; set; }

        [Column(TypeName = "numeric")]
        public decimal LIVESTATUS { get; set; }
    }
}
