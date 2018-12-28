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
        public string Title { get; set; }
        public string Content { get; set; }
        public string Data { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }

    public class SubjectDBContext : DbContext
    {
        public SubjectDBContext() : base("DBConnectionString") { }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}