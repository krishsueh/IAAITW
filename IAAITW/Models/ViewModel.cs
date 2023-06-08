using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static IAAITW.Models.EnumList;

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

        public class Contact
        {
            [Required(ErrorMessage = "{0}必填")]
            [MaxLength(50)]
            [Display(Name = "姓名")]
            public string Name { get; set; }

            [Required(ErrorMessage = "{0}必填")]
            [Display(Name = "性別")]
            public GenderType Gender { get; set; }

            [Required(ErrorMessage = "{0}必填")]
            [MaxLength(10)]
            [Display(Name = "聯絡電話")]
            public string Tel { get; set; }

            [Required(ErrorMessage = "{0}必填")]
            [EmailAddress(ErrorMessage = "{0} 格式錯誤")]
            [MaxLength(200)]
            [DataType(DataType.EmailAddress)]
            [Display(Name = "E-Mail")]
            public string Email { get; set; }

            [Required(ErrorMessage = "{0}必填")]
            [MaxLength(50)]
            [Display(Name = "標題")]
            public string Subject { get; set; }

            [MaxLength(1000)]
            [Display(Name = "內容")]
            public string Article { get; set; }
        }
    }
}