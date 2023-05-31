using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IAAITW.Models
{
    public class About
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "關於我們")]
        [MaxLength(4000)]
        public string AboutUs { get; set; }

        [Display(Name = "建立時間")]
        public DateTime InitDate { get; set; }
    }
}