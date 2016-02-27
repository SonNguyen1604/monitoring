namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TbCapdo")]
    public partial class TbCapdo
    {
        [Key]
        [Display(Name = "Mã cấp độ")]
        public int MaCD { get; set; }

        [Display(Name = "Tên cấp độ")]
        public string TenCD { get; set; }
        [Display(Name = "Trạng thái")]
        public bool TrangThai { get; set; }
    }
}
