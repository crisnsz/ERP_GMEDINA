using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERP_GMEDINA.Attribute;
using ERP_GMEDINA.Models;

namespace ERP_GMEDINA.Controllers
{
    public class TravelHistoryController : Controller
    {
        private FARSIMANEntities db = new FARSIMANEntities();

        #region ActionResult

        // GET: /TravelHistory/

        [SessionManager("TravelHistory/Index")]
        public ActionResult Index()
        {
            var tbtravelhistories = db.tbTravelHistories.Include(t => t.tbEmployee).Include(t => t.tbSubsidiary).Include(t => t.tbTransporter);
            return View(tbtravelhistories.ToList());
        }

        // GET: /TravelHistory/Details/5

        [SessionManager("TravelHistory/Details")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbTravelHistory tbTravelHistory = db.tbTravelHistories.Find(id);
            if (tbTravelHistory == null)
            {
                return HttpNotFound();
            }
            return View(tbTravelHistory);
        }

        // GET: /TravelHistory/Create

        [SessionManager("TravelHistory/Create")]
        public ActionResult Create()
        {
            ViewBag.employee_ID = new SelectList(db.tbEmployees, "employee_ID", "employee_Name");
            ViewBag.subsidiary_ID = new SelectList(db.tbSubsidiaries, "subsidiary_ID", "subsidiary_Name");
            ViewBag.transporter_ID = new SelectList(db.tbTransporters, "transporter_ID", "transporter_Name");
            return View();
        }

        // POST: /TravelHistory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [SessionManager("TravelHistory/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "travel_ID,employee_ID,subsidiary_ID,transporter_ID,departure_Date_and_Time,travel_Cost")] tbTravelHistory tbTravelHistory)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.employee_ID = new SelectList(db.tbEmployees, "employee_ID", "employee_Name", tbTravelHistory.employee_ID);
                ViewBag.subsidiary_ID = new SelectList(db.tbSubsidiaries, "subsidiary_ID", "subsidiary_Name", tbTravelHistory.subsidiary_ID);
                ViewBag.transporter_ID = new SelectList(db.tbTransporters, "transporter_ID", "transporter_Name", tbTravelHistory.transporter_ID);
                return View(tbTravelHistory);
            }

            db.tbTravelHistories.Add(tbTravelHistory);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: /TravelHistory/Edit/5

        [SessionManager("TravelHistory/Edit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbTravelHistory tbTravelHistory = db.tbTravelHistories.Find(id);
            if (tbTravelHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.employee_ID = new SelectList(db.tbEmployees, "employee_ID", "employee_Name", tbTravelHistory.employee_ID);
            ViewBag.subsidiary_ID = new SelectList(db.tbSubsidiaries, "subsidiary_ID", "subsidiary_Name", tbTravelHistory.subsidiary_ID);
            ViewBag.transporter_ID = new SelectList(db.tbTransporters, "transporter_ID", "transporter_Name", tbTravelHistory.transporter_ID);
            return View(tbTravelHistory);
        }

        // POST: /TravelHistory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionManager("TravelHistory/Edit")]
        public ActionResult Edit([Bind(Include = "travel_ID,employee_ID,subsidiary_ID,transporter_ID,departure_Date_and_Time,travel_Cost")] tbTravelHistory tbTravelHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbTravelHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.employee_ID = new SelectList(db.tbEmployees, "employee_ID", "employee_Name", tbTravelHistory.employee_ID);
            ViewBag.subsidiary_ID = new SelectList(db.tbSubsidiaries, "subsidiary_ID", "subsidiary_Name", tbTravelHistory.subsidiary_ID);
            ViewBag.transporter_ID = new SelectList(db.tbTransporters, "transporter_ID", "transporter_Name", tbTravelHistory.transporter_ID);
            return View(tbTravelHistory);
        }

        // GET: /TravelHistory/Delete/5
        [SessionManager("TravelHistory/Delete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbTravelHistory tbTravelHistory = db.tbTravelHistories.Find(id);
            if (tbTravelHistory == null)
            {
                return HttpNotFound();
            }
            return View(tbTravelHistory);
        }

        // POST: /TravelHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionManager("TravelHistory/Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            tbTravelHistory tbTravelHistory = db.tbTravelHistories.Find(id);
            db.tbTravelHistories.Remove(tbTravelHistory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion


        #region JsonResult
        [HttpPost]
        public JsonResult GetAddress(int subsidiary_ID)
        {
            tbSubsidiary response = new tbSubsidiary();
            try
            {
                response = db.tbSubsidiaries.Where(x => x.subsidiary_ID == subsidiary_ID).FirstOrDefault();

            }
            catch (Exception Ex)
            {
                return Json(Ex.Message.ToString(), JsonRequestBehavior.DenyGet);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
