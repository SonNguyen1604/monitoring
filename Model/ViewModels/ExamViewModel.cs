using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels
{
    public class ExamViewModel
    {
        public int MaDeThi { get; set; }
        [Display(Name = "Tên đề thi")]
        public string TenDeThi { get; set; }
        [Display(Name="Môn thi")]
        public string TenMT { get; set; }
        [Display(Name = "Cấp độ")]
        public string Capdo { get; set; }
        [Display(Name = "Đợt thi")]
        public string Dotthi { get; set; }
        [Display(Name = "Đường dẫn đề thi")]
        public string Duongdan { get; set; }
        public bool TrangThai { get; set; }
    }
}
