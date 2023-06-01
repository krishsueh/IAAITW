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
        public string AboutUs { get; set; }

        [Display(Name = "組織架構")]
        public string Organization { get; set; }

        [Display(Name = "沿革")]
        public string History { get; set; }

        [Display(Name = "配證會員")]
        public string LicensedMember { get; set; }

        [Display(Name = "專家介紹")]
        public string Expert { get; set; }
    }
}