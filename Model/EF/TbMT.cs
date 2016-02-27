namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TbMT")]
    public partial class TbMT
    {
        [Key]
        public int MaMT { get; set; }

        [Required]
        [Display(Name = "Tên môn thi")]
        public string TenMT { get; set; }

        [Display(Name = "Trạng thái")]
        public bool TrangThai { get; set; }
    }
}
