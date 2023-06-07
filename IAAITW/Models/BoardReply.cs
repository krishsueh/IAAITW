using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IAAITW.Models
{
    public class BoardReply
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReplyId { get; set; }

        [Display(Name = "標題")]
        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Board MyBoard { get; set; }

        [Display(Name = "回覆人")]
        [MaxLength(50)]
        public string Replyer { get; set; }

        [Display(Name = "內容")]
        [MaxLength(3000)]
        public string ReplyArticle { get; set; }

        [Display(Name = "回覆時間")]
        public DateTime ReplyDate { get; set; }
        public BoardReply()
        {
            ReplyDate = DateTime.Now;
        }
    }
}