namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TbStudent")]
    public partial class TbStudent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "Tên tài khoản")]

        [StringLength(50)]
        public string Username { get; set; }
        [Display(Name = "Mật khẩu")]
        [StringLength(50)]
        public string Password { get; set; }
        [StringLength(20)]
        public string GroupID { get; set; }

        [Display(Name = "Họ")]
        [StringLength(50)]
        public string Ho { get; set; }

        [Display(Name = "Tên")]
        [StringLength(50)]
        public string Ten { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày sinh")]
        public DateTime NgaySinh { get; set; }

        [Display(Name = "Giới tính")]
        public bool GioiTinh { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Active { get; set; }

        [Display(Name = "Ảnh đại diện")]
        [StringLength(250)]
        public string Avatar { get; set; }

        public bool Thi { get; set; }
    }
}
