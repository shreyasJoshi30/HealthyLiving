using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HL_V1.Models
{
    public class Article
    {
        [Key]
        public Guid ArticleID{ get; set; }

        public string Title { get; set; }

        
        [AllowHtml]
        public string Content{ get; set; }

        public string AuthorID { get; set; }
        public string Category { get; set; }
        public DateTime PublishDate { get; set; }
        public string ArticleStatus { get; set; }

        [Display(Name = "Cover Image")]
        public string ArticleCover { get; set; }

        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class ArticleApproveViewModel
    {
        [Key]
        public Guid ArticleID { get; set; }

        public string Title { get; set; }


        [AllowHtml]
        public string Content { get; set; }

        public string AuthorID { get; set; }
        public string Category { get; set; }
        public DateTime PublishDate { get; set; }
        public string ArticleStatus { get; set; }

        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}