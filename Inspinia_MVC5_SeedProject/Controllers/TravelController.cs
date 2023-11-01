using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.PeerToPeer;
using System.Web;
using System.Web.Mvc;
using ERP_GMEDINA.Attribute;
using ERP_GMEDINA.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace ERP_GMEDINA.Controllers
{
    public class TravelController : Controller
    {
        private FARSIMANEntities db = new FARSIMANEntities();

        public static List<tbTravelDetail> ListTravelDetails { get; set; } = new List<tbTravelDetail>();
        private static bool Modified { get; set; }

        public static List<int> ListEmployees { get; set; } = new List<int>();

        private static tbEmployee EmployeeInfo { get; set; } = new tbEmployee();

        readonly Helpers Login = new Helpers();

        #region ActionResult


        private void CleanVariables()
        {

            Modified = false;

            ListEmployees.Clear();
            ListTravelDetails.Clear();

        }

        private void GetUserInformation()
        {

            int UserId = 0;

            List<tbUser> UserInformation = Login.getUserInformation();

            foreach (tbUser user in UserInformation)
            {
                UserId = user.user_ID;
            }



            EmployeeInfo = db.tbEmployees.Find(UserId);

        }






        // GET: /Travel/

        [SessionManager("TravelHistory/Index")]
        public ActionResult Index()
        {

            CleanVariables();

            var tbtravels = db.tbTravels;
            return View(tbtravels.ToList());
        }

        // GET: /Travel/Details/5

        [SessionManager("TravelHistory/Details")]
        public ActionResult Details(int? id)
        {
            CleanVariables();

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
            CleanVariables();

            GetUserInformation();

            ViewBag.employee_ID = EmployeeInfo.employee_ID;
            ViewBag.employee_Name = EmployeeInfo.employee_Name;



            ViewBag.subsidiary_ID = new SelectList(db.tbSubsidiaries, "subsidiary_ID", "subsidiary_Name");
            ViewBag.transporter_ID = new SelectList(db.tbTransporters, "transporter_ID", "transporter_Name");

            return View();
        }

        // POST: /Travel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionManager("TravelHistory/Create")]
        public ActionResult Create([Bind(Include = "travel_ID,subsidiary_ID,transporter_ID,employee_ID,departure_Date_and_Time,distance_Kilometers,total_travel_Cost")] tbTravel tbTravel)
        {


            GetUserInformation();

            if (!ModelState.IsValid)
            {
                ViewBag.employee_ID = EmployeeInfo.employee_ID;
                ViewBag.employee_Name = EmployeeInfo.employee_Name;

                ViewBag.subsidiary_ID = new SelectList(db.tbSubsidiaries, "subsidiary_ID", "subsidiary_Name");
                ViewBag.transporter_ID = new SelectList(db.tbTransporters, "transporter_ID", "transporter_Name");

                return View(tbTravel);
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.tbTravels.Add(tbTravel);

                    db.SaveChanges();


                    if (ListTravelDetails is null)
                    {
                        return View(tbTravel);
                    }

                    //foreach (var employeesSubsidiary in ListTravelDetails)
                    //{

                    //    employeesSubsidiary.travel_ID = tbTravel.travel_ID;

                    //    db.tbTravelDetails.Add(employeesSubsidiary);
                    //} 


                    var Transportists = db.tbTransporters.Find(tbTravel.transporter_ID);


                    

                    if (Transportists == null)
                    {
                        return HttpNotFound();
                    }


                    foreach (var employee in ListEmployees)
                    {
                        var EmployeesSubsidiariesInfo = db.tbEmployeesSubsidiaries.Where(x => x.employee_ID == employee && x.subsidiary_ID == tbTravel.subsidiary_ID).SingleOrDefault();

                        if (EmployeesSubsidiariesInfo is null)
                        {
                            return View(tbTravel);
                        }

                        tbTravelDetail TravelDetail = new tbTravelDetail
                        {
                            travel_ID = tbTravel.travel_ID,
                            employee_ID = employee,
                            distance_Kilometers = (decimal)EmployeesSubsidiariesInfo.employeeSubsidiary_DistanceKM,
                            travel_Cost = Transportists.transporter_Fee * (decimal)EmployeesSubsidiariesInfo.employeeSubsidiary_DistanceKM
                        };

                        db.tbTravelDetails.Add(TravelDetail);
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
        public ActionResult Edit([Bind(Include = "travel_ID,subsidiary_ID,transporter_ID,employee_ID,departure_Date_and_Time,distance_Kilometers,total_travel_Cost")] tbTravel tbTravel)
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


        #region JsonResult

        [HttpPost]
        public JsonResult GetAddress(int? subsidiary_ID)
        {
            try
            {

                tbSubsidiary subsidiary = db.tbSubsidiaries.Where(x => x.subsidiary_ID == subsidiary_ID).FirstOrDefault();

                if (subsidiary is null)
                {
                    return Json(string.Empty);
                }


                return Json(subsidiary.subsidiary_Direction.ToString(), JsonRequestBehavior.AllowGet);

            }
            catch (Exception Ex)
            {
                return Json(Ex.Message.ToString(), JsonRequestBehavior.DenyGet);
            }

        }


        [HttpPost]
        public JsonResult GetEmployeesBySubsidiary(int subsidiary_ID)
        {
            try
            {
                var response = (from employee in db.tbEmployees
                                join employeeSubsidiary in db.tbEmployeesSubsidiaries
                                on employee.employee_ID equals employeeSubsidiary.employee_ID
                                where employeeSubsidiary.subsidiary_ID == subsidiary_ID
                                select new
                                {
                                    employee.employee_ID,
                                    employee.employee_Name,
                                    employee.employee_Direction,
                                    employee.position_ID,
                                    employeeSubsidiary.tbSubsidiary.subsidiary_Name,
                                    employeeSubsidiary.employeeSubsidiary_DistanceKM
                                }).ToList();

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(Ex.Message.ToString(), JsonRequestBehavior.DenyGet);
            }

        }



        [HttpPost]
        public JsonResult AddEmployeestoTravelT(tbTravelDetail tbTravelDetail)
        {
            try
            {
                ListTravelDetails.Add(tbTravelDetail);

                Modified = true;

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.DenyGet);
            }

        }

        [HttpPost]
        public JsonResult AddEmployeestoTravel(int Employee)
        {
            try
            {
                ListEmployees.Add(Employee);

                Modified = true;

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.DenyGet);
            }

        }



        [HttpPost]
        public JsonResult RemoveEmployeestoTravelT(tbTravelDetail tbTravelDetail)
        {
            try
            {

                ListTravelDetails.RemoveAll(item => item.employee_ID == tbTravelDetail.employee_ID);

                Modified = true;

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.DenyGet);
            }

        }

        [HttpPost]
        public JsonResult RemoveEmployeestoTravel(int Employee)
        {
            try
            {

                ListEmployees.RemoveAll(item => item == Employee);

                Modified = true;

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.DenyGet);
            }

        } 

        
        [HttpPost]
        public JsonResult GetTransporterInfo(int Transporter)
        {
            try
            {
                var response = db.tbTransporters.Where(x => x.transporter_ID == Transporter).Select(x => new 
                {
                    x.transporter_ID,
                    x.transporter_Name,
                    x.transporter_Fee
                }).FirstOrDefault();

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.DenyGet);
            }

        }



        [HttpPost]
        public JsonResult EmployeeTravelToday(int Employee)
        {
            try
            {
                bool hasRowsToday = false;
                DateTime today = DateTime.Today;

                // Query the table and check if any rows match the current date

                var query = from travelDetail in db.tbTravelDetails
                            join travel in db.tbTravels on travelDetail.travel_ID equals travel.travel_ID
                           where travelDetail.employee_ID == Employee
                              && DbFunctions.TruncateTime(travel.departure_Date_and_Time) == today
                          select travelDetail.travel_ID;

                hasRowsToday = query.Any();



                return Json(hasRowsToday, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.DenyGet);
            }

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
