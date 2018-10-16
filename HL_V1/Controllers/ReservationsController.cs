using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HL_V1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace HL_V1.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reservations
        public ActionResult Index()
        {
            List<Reservation> reservationsList = new List<Reservation>();
            //var reservations = db.Reservations.Include(r => r.User1).Include(r => r.User2);

            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roles = userManager.GetRoles(User.Identity.GetUserId());
            string currentUsrId = User.Identity.GetUserId();

            if (roles.Contains("Admin"))
            {
                reservationsList = db.Reservations.ToList();
            }
            else if (roles.Contains("NUser") )
            {
                reservationsList = db.Reservations.Where(m => m.ReservedBy == currentUsrId).ToList();
            }else if (roles.Contains("Nutri"))
            {
                reservationsList = db.Reservations.Where(m => m.NutritionistID == currentUsrId).ToList();
            }


            return View(reservationsList);
        }

        // GET: Reservations/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        public ActionResult Create()
        {
            ViewBag.NutritionistID = new SelectList(db.Users.Where(u=>u.userType==ApplicationUser.userTypes.Nutritionist), "Id", "Email");
            ViewBag.ReservedBy = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReservationID,NutritionistID,ReservedDate,ReservedBy")] Reservation reservation)
        {
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roles = userManager.GetRoles(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                
                reservation.ReservationID = Guid.NewGuid();
                if (!roles.Contains("Admin"))
                {
                    reservation.ReservedBy = User.Identity.GetUserId();
                    
                }
                reservation.Reservation_status = "I";//initiate reservation



                db.Reservations.Add(reservation);
                var result=db.SaveChanges();

                //get email of the nutritionist reserved
                var nutriEmail = db.Users.Where(d => d.Id == reservation.NutritionistID).Select(d=>d.Email).FirstOrDefault();
                var userEmail = db.Users.Where(d => d.Id == reservation.ReservedBy).Select(d => d.Email).FirstOrDefault();

                var Ucontent = "This is to inform you that you have Requested for a Booking at: "+reservation.ReservedDate+".  You will be informed" +
                    "when the Nutritionist accepts your request. " +
                    "\n\n\n\n" +
                    "" +
                    "" +
                    "This is System generated Mail. Please donot reply to this mail";
                var Usubject = "Request for Booking at" + reservation.ReservedDate;

                var Ncontent = "Hi, You have a new request for Reservation at"+reservation.ReservedDate+". Please act upon the reservation from the website." +
                    "\n\n\nThis is system generated mail. Donot Reply to this mail";
                var Nsubject = "New Reservation Request at"+reservation.ReservedDate;
                if (result > 0)
                {
                    //send email to user
                    Utils.EmailSender.Send(userEmail,Usubject,Ucontent);

                    //send email to nutritionist
                    Utils.EmailSender.Send(nutriEmail, Nsubject, Ncontent);

                    ViewBag.Result = "Success! Reservation has been Requested. The nutritionist has been informed";
                }
                return RedirectToAction("Index");
            }

            ViewBag.NutritionistID = new SelectList(db.Users, "Id", "Email", reservation.NutritionistID);
            ViewBag.ReservedBy = new SelectList(db.Users, "Id", "Email", reservation.ReservedBy);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.NutritionistID = new SelectList(db.Users, "Id", "Email", reservation.NutritionistID);
            ViewBag.ReservedBy = new SelectList(db.Users, "Id", "Email", reservation.ReservedBy);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReservationID,NutritionistID,ReservedDate,ReservedBy")] Reservation reservation)
        {
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roles = userManager.GetRoles(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                
                reservation.ModifiedBy= User.Identity.GetUserId();
                reservation.ModifiedOn = DateTime.Now;

                if (!roles.Contains("Admin"))
                {
                    reservation.ReservedBy = User.Identity.GetUserId();
                    
                }
                


                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NutritionistID = new SelectList(db.Users, "Id", "Email", reservation.NutritionistID);
            ViewBag.ReservedBy = new SelectList(db.Users, "Id", "Email", reservation.ReservedBy);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
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


        public ActionResult Accept(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation= db.Reservations.Find(id);

            var userEmail = db.Users.Where(d => d.Id == reservation.ReservedBy).Select(d => d.Email).FirstOrDefault();

            var Ucontent = "This is to inform you that you have Reservation at " + reservation.ReservedDate + " has been confirmed. Plese attend the meeting on time." +
                    
                    "\n\n\n\n" +
                    "" +
                    "" +
                    "This is System generated Mail. Please donot reply to this mail";
            var Usubject = "Reservation request CONFIRMED -- " + reservation.ReservedDate;

            ViewBag.Result = "Success! Reservation has been booked. A confirmation Email has been sent.";

            if (reservation == null)
            {
                return HttpNotFound();
            }
            else
            {
                // article = db.Articles.Find(articleObject.ArticleID);
                reservation.Reservation_status = "A";
                reservation.ModifiedOn = DateTime.Now;
                reservation.ModifiedBy= User.Identity.GetUserId();

                db.Entry(reservation).State = EntityState.Modified;
               var result= db.SaveChanges();

                if (result > 0)
                {
                    //send email to user
                    Utils.EmailSender.Send(userEmail, Usubject, Ucontent);
                }
                return RedirectToAction("Index");
            }
           
        }


        public ActionResult Complete(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            else
            {
                // article = db.Articles.Find(articleObject.ArticleID);
                reservation.Reservation_status = "C";
                reservation.ModifiedOn = DateTime.Now;
                reservation.ModifiedBy = User.Identity.GetUserId();

                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //return View(article);
        }


        public ActionResult Cancel(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            else
            {
                // article = db.Articles.Find(articleObject.ArticleID);
                reservation.Reservation_status = "X";
                reservation.ModifiedOn = DateTime.Now;
                reservation.ModifiedBy = User.Identity.GetUserId();

                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //return View(article);
        }

    }
}
