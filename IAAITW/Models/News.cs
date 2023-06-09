using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static IAAITW.Models.EnumList;

namespace IAAITW.Models
{
    public class News
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "封面")]
        [MaxLength(100)]
        public string CoverImg { get; set; }

        [Display(Name = "發布日期")]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "標題")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "內文")]
        public string Content { get; set; }

        [Display(Name = "置頂")]
        public bool GoTop { get; set; } = false;
    }
}