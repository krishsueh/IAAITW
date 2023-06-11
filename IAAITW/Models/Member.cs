using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static IAAITW.Models.EnumList;

namespace IAAITW.Models
{
    public class Member
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "帳號")]
        public string Account { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [StringLength(100, ErrorMessage = "{0} 長度至少必須為 {2} 個字元。", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "性別")]
        public GenderType Gender { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "生日")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "申請類別")]
        public MemberType MemberTypes { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(10)]
        [Display(Name = "連絡電話(公)")]
        public string Tel { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(10)]
        [Display(Name = "手機")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "通訊處")]
        public string Address { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [EmailAddress(ErrorMessage = "{0} 格式錯誤")]
        [MaxLength(200)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }

        [Display(Name = "國際會籍")]
        public bool InternationalMembership { get; set; }

        [Display(Name = "會員證影本")]
        [MaxLength(200)]
        public string MembershipFile { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "現職單位")]
        public string CurrentEmployer { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "職稱")]
        public string JobTitle { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "最高學歷")]
        public string HighestEducation { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "服務單位")]
        public string PastEmployer1 { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "職稱")]
        public string PastJobTitle1 { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Range(1, 9999, ErrorMessage = "年份有誤")]
        [Display(Name = "起始年")]
        public int StartYear1 { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Range(1, 12, ErrorMessage = "數值介於 1 - 12")]
        [Display(Name = "起始月")]
        public int StartMonth1 { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Range(1, 9999, ErrorMessage = "年份有誤")]
        [Display(Name = "結束年")]
        public int EndYear1 { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Range(1, 12,ErrorMessage = "數值介於 1 - 12")]
        [Display(Name = "結束月")]
        public int EndMonth1 { get; set; }

        [Display(Name = "服務單位2")]
        [MaxLength(50)]
        public string PastEmployer2 { get; set; }

        [Display(Name = "職稱2")]
        [MaxLength(50)]
        public string PastJobTitle2 { get; set; }

        [Range(1, 9999, ErrorMessage = "年份有誤")]
        [Display(Name = "起始年2")]
        public int? StartYear2 { get; set; }

        [Display(Name = "起始月2")]
        [Range(1, 12, ErrorMessage = "數值介於 1 - 12")]
        public int? StartMonth2 { get; set; }

        [Range(1, 9999, ErrorMessage = "年份有誤")]
        [Display(Name = "結束年2")]
        public int? EndYear2 { get; set; }

        [Display(Name = "結束月2")]
        [Range(1, 12, ErrorMessage = "數值介於 1 - 12")]
        public int? EndMonth2 { get; set; }

        [MaxLength(50)]
        [Display(Name = "服務單位3")]
        public string PastEmployer3 { get; set; }

        [MaxLength(50)]
        [Display(Name = "職稱3")]
        public string PastJobTitle3 { get; set; }

        [Range(1, 9999, ErrorMessage = "年份有誤")]
        [Display(Name = "起始年3")]
        public int? StartYear3 { get; set; }

        [Display(Name = "起始月3")]
        [Range(1, 12, ErrorMessage = "數值介於 1 - 12")]
        public int? StartMonth3 { get; set; }

        [Range(1, 9999, ErrorMessage = "年份有誤")]
        [Display(Name = "結束年3")]
        public int? EndYear3 { get; set; }

        [Display(Name = "結束月3")]
        [Range(1, 12, ErrorMessage = "數值介於 1 - 12")]
        public int? EndMonth3 { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Range(0, 100, ErrorMessage = "數值介於 0 - 100")]
        [Display(Name = "合計年資(年)")]
        public int TotalYear { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Range(1, 12, ErrorMessage = "數值介於 1 - 12")]
        [Display(Name = "合計年資(月)")]
        public int TotalMonth { get; set; }
    }
}