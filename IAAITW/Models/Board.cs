using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IAAITW.Models
{
    public class Board
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubjectId { get; set; }

        [Display(Name = "標題")]
        [MaxLength(50)]
        public string Subject { get; set; }

        [Display(Name = "發表人")]
        [MaxLength(50)]
        public string Author { get; set; }

        [Display(Name = "內容")]
        [MaxLength(3000)]
        public string Article { get; set; }

        [Display(Name = "發表時間")]
        public DateTime PostDate { get; set; }
        public Board()
        {
            PostDate = DateTime.Now;
        }

        public virtual ICollection<BoardReply> BoardReplies { get; set; }

    }
}