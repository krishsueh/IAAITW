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

        [Display(Name = "組織架構")]
        [MaxLength(4000)]
        public string Organization { get; set; }

        [Display(Name = "沿革")]
        [MaxLength(4000)]
        public string History { get; set; }

        [Display(Name = "配證會員")]
        [MaxLength(4000)]
        public string LicensedMember { get; set; }

        [Display(Name = "專家介紹")]
        [MaxLength(4000)]
        public string Expert { get; set; }
    }
}