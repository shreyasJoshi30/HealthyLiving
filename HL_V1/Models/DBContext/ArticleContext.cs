using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HL_V1.Models.DBContext
{
    public class ArticleContext : DbContext
    {
        public ArticleContext() :base("DefaultConnection")
        {
        }

        public DbSet<Article> Articles { get; set; }
    }
}