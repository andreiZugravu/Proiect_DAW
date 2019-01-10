using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proiect_DAW.Models
{
    public class Reply
    {
        [Key]
        public int ReplyId { get; set; }
        [Required, MinLength(3)]
        public string Content { get; set; }
        public string Data { get; set; }
        public int SubjectId { get; set; }
        public string UserId { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}