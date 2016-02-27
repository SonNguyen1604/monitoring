namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TbDeThi")]
    public partial class TbDeThi
    {
        [Key]
        public int MaDeThi { get; set; }
        [Display(Name = "Tên đề thi")]
        public string TenDeThi { get; set; }
        [Display(Name = "Môn thi")]
        public int? MaMT { get; set; }
        [Display(Name = "Cấp độ")]
        public int? MaCD { get; set; }
        [Display(Name = "Đợt thi")]
        public int? MaDT { get; set; }
        [Display(Name = "Đường dẫn đề thi")]
        public string DuongDanDT { get; set; }

        [Display(Name = "Trạng thái")]
        public bool TrangThai { get; set; }
    }
}
