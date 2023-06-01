using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IAAITW.Models
{
    public class Service
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "協會業務")]
        [MaxLength(3000)]
        public string Jobs { get; set; }

        [Display(Name = "訓練認證發照")]
        [MaxLength(3000)]
        public string Licenses { get; set; }

        [Display(Name = "諮詢顧問")]
        [MaxLength(3000)]
        public string Refer { get; set; }

        [Display(Name = "縱火調查")]
        [MaxLength(3000)]
        public string Survey { get; set; }
    }
}