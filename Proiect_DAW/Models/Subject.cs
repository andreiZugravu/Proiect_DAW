using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Proiect_DAW.Models
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }
        [Required, MinLength(3)]
        public string Title { get; set; }
        [Required, MinLength(10)]
        public string Content { get; set; }
        public string Data { get; set; }
        public int NumberOfViews { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Reply> Replies { get; set; }
    }
}