using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERP_GMEDINA.Models;

namespace ERP_GMEDINA.Controllers
{
    public class tbTravelsController : Controller
    {
        private FARSIMANEntities db = new FARSIMANEntities();

        // GET: /tbTravels/
        public ActionResult Index()
        {
            var tbtravels = db.tbTravels.Include(t => t.tbEmployee).Include(t => t.tbSubsidiary).Include(t => t.tbTransporter);
            return View(tbtravels.ToList());
        }

        // GET: /tbTravels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbTravel tbTravel = db.tbTravels.Find(id);
            if (tbTravel == null)
            {
                return HttpNotFound();
            }
            return View(tbTravel);
        }

        // GET: /tbTravels/Create
        public ActionResult Create()
        {
            ViewBag.employee_ID = new SelectList(db.tbEmployees, "employee_ID", "employee_Name");
            ViewBag.subsidiary_ID = new SelectList(db.tbSubsidiaries, "subsidiary_ID", "subsidiary_Name");
            ViewBag.transporter_ID = new SelectList(db.tbTransporters, "transporter_ID", "transporter_Name");
            return View();
        }

        // POST: /tbTravels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="travel_ID,subsidiary_ID,transporter_ID,employee_ID,departure_Date_and_Time,distance_Kilometers,total_travel_Cost")] tbTravel tbTravel)
        {
            if (ModelState.IsValid)
            {
                db.tbTravels.Add(tbTravel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.employee_ID = new SelectList(db.tbEmployees, "employee_ID", "employee_Name", tbTravel.employee_ID);
            ViewBag.subsidiary_ID = new SelectList(db.tbSubsidiaries, "subsidiary_ID", "subsidiary_Name", tbTravel.subsidiary_ID);
            ViewBag.transporter_ID = new SelectList(db.tbTransporters, "transporter_ID", "transporter_Name", tbTravel.transporter_ID);
            return View(tbTravel);
        }

        // GET: /tbTravels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbTravel tbTravel = db.tbTravels.Find(id);
            if (tbTravel == null)
            {
                return HttpNotFound();
            }
            ViewBag.employee_ID = new SelectList(db.tbEmployees, "employee_ID", "employee_Name", tbTravel.employee_ID);
            ViewBag.subsidiary_ID = new SelectList(db.tbSubsidiaries, "subsidiary_ID", "subsidiary_Name", tbTravel.subsidiary_ID);
            ViewBag.transporter_ID = new SelectList(db.tbTransporters, "transporter_ID", "transporter_Name", tbTravel.transporter_ID);
            return View(tbTravel);
        }

        // POST: /tbTravels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="travel_ID,subsidiary_ID,transporter_ID,employee_ID,departure_Date_and_Time,distance_Kilometers,total_travel_Cost")] tbTravel tbTravel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbTravel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.employee_ID = new SelectList(db.tbEmployees, "employee_ID", "employee_Name", tbTravel.employee_ID);
            ViewBag.subsidiary_ID = new SelectList(db.tbSubsidiaries, "subsidiary_ID", "subsidiary_Name", tbTravel.subsidiary_ID);
            ViewBag.transporter_ID = new SelectList(db.tbTransporters, "transporter_ID", "transporter_Name", tbTravel.transporter_ID);
            return View(tbTravel);
        }

        // GET: /tbTravels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbTravel tbTravel = db.tbTravels.Find(id);
            if (tbTravel == null)
            {
                return HttpNotFound();
            }
            return View(tbTravel);
        }

        // POST: /tbTravels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbTravel tbTravel = db.tbTravels.Find(id);
            db.tbTravels.Remove(tbTravel);
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
    }
}
