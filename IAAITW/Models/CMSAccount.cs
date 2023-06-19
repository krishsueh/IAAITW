using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IAAITW.Models
{
    public class CMSAccount
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "使用者名稱")]
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(20)]
        public string Name { get; set; }

        [Display(Name = "個人頭像")]
        [MaxLength(200)]
        public string Headshot { get; set; }

        [Display(Name = "帳號")]
        [Required(ErrorMessage = "{0}必填")]
        [EmailAddress(ErrorMessage = "{0} 格式錯誤")]
        [MaxLength(200)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "密碼")]
        [StringLength(200, ErrorMessage = "{0} 長度至少必須為 {2} 個字元。", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "身份別")]
        [Required(ErrorMessage = "{0}必填")]
        public int IdentityId { get; set; }
        [ForeignKey("IdentityId")]
        public virtual CMSIdentity MyIdentity { get; set; }

        [Display(Name = "權限")]
        [MaxLength(500)]
        public string Permission { get; set; }
    }
}