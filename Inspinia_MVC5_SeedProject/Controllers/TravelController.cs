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
    public class TravelController : Controller
    {
        private FARSIMANEntities db = new FARSIMANEntities();
        public static List<tbTravelDetail> ListTravelDetails { get; set; } = new List<tbTravelDetail>();

        public static bool Modified { get; set; }


        #region ActionResult


        // GET: /Travel/

        [SessionManager("TravelHistory/Index")]
        public ActionResult Index()
        {
            var tbtravels = db.tbTravels;
            return View(tbtravels.ToList());
        }

        // GET: /Travel/Details/5

        [SessionManager("TravelHistory/Details")]
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

        // GET: /Travel/Create

        [SessionManager("TravelHistory/Create")]
        public ActionResult Create()
        {
            ViewBag.employee_ID = new SelectList(db.tbEmployees, "employee_ID", "employee_Name");
            ViewBag.subsidiary_ID = new SelectList(db.tbSubsidiaries, "subsidiary_ID", "subsidiary_Name");
            ViewBag.transporter_ID = new SelectList(db.tbTransporters, "transporter_ID", "transporter_Name");
            //ViewBag.tbEmployees = db.tbEmployees.ToList();
            return View();
        }

        // POST: /Travel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [SessionManager("TravelHistory/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="travel_ID,subsidiary_ID,transporter_ID,employee_ID,departure_Date_and_Time,distance_Kilometers,total_travel_Cost")] tbTravel tbTravel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Positions = new SelectList(db.tbPositions, "position_ID", "position_Name");
                return View(tbTravel);
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.tbTravelHistories.Add(tbTravel);

                    db.SaveChanges();


                    foreach (var employeesSubsidiary in ListTravelDetails)
                    {

                        //employeesSubsidiary.subsidiary_ID = tbTravel.subsidiary_ID;


                        db.tbTravelDetails.Add(employeesSubsidiary);
                    }

                    db.SaveChanges();

                    transaction.Commit();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View(tbTravel);
                }
            }
        }

        // GET: /Travel/Edit/5

        [SessionManager("TravelHistory/Edit")]
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

        // POST: /Travel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [SessionManager("TravelHistory/Edit")]
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

        // GET: /Travel/Delete/5
        [SessionManager("TravelHistory/Delete")]
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

        // POST: /Travel/Delete/5
        [SessionManager("TravelHistory/Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbTravel tbTravel = db.tbTravels.Find(id);
            db.tbTravels.Remove(tbTravel);
            db.SaveChanges();
            return RedirectToAction("Index");
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
