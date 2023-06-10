using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IAAITW.Models
{
    public class Partner
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "圖片")]
        [MaxLength(100)]
        public string Image { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "單位名稱")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "網址")]
        [MaxLength(100)]
        public string Link { get; set; }


    }
}