using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestPage.Models
{
    public class RegisterModel
    {
        [Key]
        public string ID { get; set; }
        [Required(ErrorMessage="Yêu cầu nhập tên đăng nhập.")]
        [Display(Name="Tên đăng nhập")]
        public string Username { get; set; }
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu.")]
        [StringLength(50,MinimumLength=6,ErrorMessage="Độ dài mật khẩu ít nhất 6 kí tự.")]
        public string Password { get; set; }
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu không trùng.")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Họ")]
        [Required(ErrorMessage = "Yêu cầu nhập họ.")]
        public string Ho { get; set; }
        [Display(Name = "Tên")]
        [Required(ErrorMessage = "Yêu cầu nhập tên.")]
        public string Ten { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập ngày sinh.")]
        [Display(Name = "Ngày sinh")]
        public DateTime NgaySinh { get; set; }
        [Display(Name = "Giới tính")]
        public bool GioiTinh { get; set; }
    }
}