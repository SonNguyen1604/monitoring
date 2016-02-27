namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TbBaiThi")]
    public partial class TbBaiThi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaBaiThi { get; set; }

        public int? ID { get; set; }

        public int? MaDeThi { get; set; }

        public string DuongDanBT { get; set; }

        public string DuongDanWC { get; set; }

        public string DuongDanAT { get; set; }

        public string DuongDanMH { get; set; }
    }
}
