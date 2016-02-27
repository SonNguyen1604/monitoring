using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestPage.Models
{
    public class LoginModel
    {
        [Key]
        [Display(Name="Tên đăng nhập")]
        [Required(ErrorMessage="Vui lòng nhập tên tài khoản để đăng nhập")]
        public string UserName { get; set; }
        [Display(Name="Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu để đăng nhập")]
        public string Password { get; set; }
    }
}