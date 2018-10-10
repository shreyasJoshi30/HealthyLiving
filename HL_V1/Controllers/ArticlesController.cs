using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HL_V1.Models;
using HL_V1.Models.DBContext;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace HL_V1.Controllers
{
    [ValidateInput(false)]
    [Authorize]
    public class ArticlesController : Controller
    {
        private ArticleContext db = new ArticleContext();
        

        // GET: Articles
        public ActionResult Index()
        {
            List<Article> articles = new List<Article>();
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roles = userManager.GetRoles(User.Identity.GetUserId());
            string currentUsrId = User.Identity.GetUserId();

            if (roles.Contains("Admin"))
            {
                articles = db.Articles.ToList();
            }
            else if (roles.Contains("NUser") || roles.Contains("Nutri"))
            {
                articles = db.Articles.Where(m => m.AuthorID == currentUsrId).ToList();
            }

                
            return View(articles);
           
        }

        // GET: Articles/Details/5
        public ActionResult Details(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArticleID,Title,Content,Category,ArticleStatus,ArticleCover")] Article article)
        {
            if (ModelState.IsValid)
            {
                article.PublishDate = DateTime.Now;
                article.ArticleID=Guid.NewGuid();
                article.AuthorID= User.Identity.GetUserId();
                article.ArticleStatus = "I";
                article.ModifiedOn = DateTime.Now;
                
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(article);
        }

        // GET: Articles/Edit/5
        public ActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArticleID,AuthorID,Title,Content,Category,PublishDate,ArticleStatus,ArticleCover")] Article article)
        {
            //article.PublishDate = DateTime.Now;
            if (ModelState.IsValid)
            {

                article.ModifiedBy = User.Identity.GetUserId();
                article.ModifiedOn = DateTime.Now;
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ViewArticles()
        {
            return View(db.Articles.ToList());
        }

        public ActionResult ArticleHome()
        {
            return View(db.Articles.ToList());
        }

        //// POST: Articles/Approve/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Approve([Bind(Include = "ArticleID,AuthorID,Title,Content,Category,PublishDate,ArticleStatus")] Article articleObject)
        //{

        //    Article article = db.Articles.Find(articleObject.ArticleID);
        //    article.ArticleStatus = "A";
        //    db.Entry(article).State = EntityState.Modified;
        //    db.SaveChanges();
        //    return RedirectToAction("Index");


        //}
        // GET: Articles/Edit/5
        public ActionResult Approve(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            else
            {
              // article = db.Articles.Find(articleObject.ArticleID);
                article.ArticleStatus = "A";
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //return View(article);
        }
        

    }
}
