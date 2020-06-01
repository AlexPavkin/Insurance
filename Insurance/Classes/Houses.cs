namespace Insurance
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Houses
    {
        public int ID { get; set; }

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

        [StringLength(20)]
        public string HOUSENUM { get; set; }

        [Column(TypeName = "numeric")]
        public decimal ESTSTATUS { get; set; }

        [StringLength(10)]
        public string BUILDNUM { get; set; }

        [StringLength(10)]
        public string STRUCNUM { get; set; }

        [Column(TypeName = "numeric")]
        public decimal STRSTATUS { get; set; }

        public Guid HOUSEID { get; set; }

        public Guid HOUSEGUID { get; set; }

        public Guid AOGUID { get; set; }

        public DateTime STARTDATE { get; set; }

        public DateTime ENDDATE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal STATSTATUS { get; set; }

        public Guid? NORMDOC { get; set; }

        [Column(TypeName = "numeric")]
        public decimal COUNTER { get; set; }
    }
}
