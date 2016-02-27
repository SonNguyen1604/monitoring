namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TbDotthi")]
    public partial class TbDotthi
    {
        [Key]
        public int MaDT { get; set; }

        [Display(Name = "Tên đợt thi")]
        public string TenDT { get; set; }
        [Display(Name = "Trạng thái")]
        public bool TrangThai { get; set; }
    }
}
