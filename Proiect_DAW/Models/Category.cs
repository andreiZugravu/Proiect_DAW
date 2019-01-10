using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proiect_DAW.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required, MinLength(2)]
        public string Name { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }
    }
}