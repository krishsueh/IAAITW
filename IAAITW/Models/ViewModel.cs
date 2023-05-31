using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IAAITW.Models
{
    public class ViewModel
    {
        public class Login
        {
            [Required(ErrorMessage = "{0}必填")]
            [EmailAddress(ErrorMessage = "{0} 格式錯誤")]
            [DataType(DataType.EmailAddress)]
            [Display(Name = "帳號")]
            public string Email { get; set; }

            [Required(ErrorMessage = "{0}必填")]
            [StringLength(200, ErrorMessage = "{0} 長度至少必須為 {2} 個字元。", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "密碼")]
            public string Password { get; set; }
        }

        public class BreadcrumbsItem
        {
            public string Text { get; set; }
            public string Url { get; set; }
        }

        public class CKEditor
        {
            [MaxLength(4000)]
            public string AboutUs { get; set; }
            public DateTime InitDate { get; set; } = DateTime.Now;
        }
    }
}