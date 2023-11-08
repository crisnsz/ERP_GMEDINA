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
        private readonly FARSIMANEntities db = new FARSIMANEntities();

        public static List<tbTravelDetail> ListTravelDetails { get; set; } = new List<tbTravelDetail>();
        private static bool Modified { get; set; }

        public static List<int> ListEmployees { get; set; } = new List<int>();

        private static tbEmployee EmployeeInfo { get; set; } = new tbEmployee();

        readonly Helpers Login = new Helpers();

        #region ActionResult

        public ActionResult GenerateReport(tbTravel tbTravel, string InitialDate, string FinalDate)
        {
            var vInitialDate = DateTime.SpecifyKind(Convert.ToDateTime(InitialDate), DateTimeKind.Local);
            var vFinalDate = DateTime.SpecifyKind(Convert.ToDateTime(FinalDate), DateTimeKind.Local);

            var DateOne = new DateTime(2023, 11, 1, 0, 0, 0, DateTimeKind.Local);
            var DateTwo = new DateTime(2023, 11, 10, 0, 0, 0, DateTimeKind.Local);


            var query = from travel in db.tbTravels
                        where travel.transporter_ID == tbTravel.transporter_ID &&
                              travel.departure_Date_and_Time >= vInitialDate &&
                              travel.departure_Date_and_Time <= vFinalDate
                        let totalTravelCost = db.tbTravels.Sum(t => t.total_travel_Cost)
                        from travelDetail in db.tbTravelDetails
                        where travel.travel_ID == travelDetail.travel_ID
                        from employee in db.tbEmployees
                        where travelDetail.employee_ID == employee.employee_ID
                        from subsidiary in db.tbSubsidiaries
                        where travel.subsidiary_ID == subsidiary.subsidiary_ID
                        from transporter in db.tbTransporters
                        where travel.transporter_ID == transporter.transporter_ID
                        select new TravelReportDataModel
                        {
                            Travel_ID = travel.travel_ID,
                            Departure_Date_and_Time = travel.departure_Date_and_Time,
                            Total_distance_Kilometers = travel.distance_Kilometers,
                            Total_travel_Cost_per_Date = totalTravelCost,
                            TotalTravelCost = travel.total_travel_Cost,
                            Subsidiary_Name = subsidiary.subsidiary_Name,
                            Subsidiary_Direction = subsidiary.subsidiary_Direction,
                            Employee_Name = employee.employee_Name,
                            Employee_Direction = employee.employee_Direction,
                            Distance_Kilometers = travelDetail.distance_Kilometers,
                            Transporter_Name = transporter.transporter_Name,
                            Transporter_Fee = transporter.transporter_Fee
                        };

            List<TravelReportDataModel> reportData = query.ToList();

            return View(reportData);
        }
        public List<TravelReportDataModel> GetReportData(int transporter_ID, string InitialDate, string FinalDate)
        {

            var vInitialDate = DateTime.SpecifyKind(Convert.ToDateTime(InitialDate), DateTimeKind.Local);
            var vFinalDate = DateTime.SpecifyKind(Convert.ToDateTime(FinalDate), DateTimeKind.Local);


            var query = from travel in db.tbTravels
                        where travel.transporter_ID == transporter_ID &&
                              travel.departure_Date_and_Time >= vInitialDate &&
                              travel.departure_Date_and_Time <= vFinalDate
                        let totalTravelCost = db.tbTravels.Sum(t => t.total_travel_Cost)
                        from travelDetail in db.tbTravelDetails
                        where travel.travel_ID == travelDetail.travel_ID
                        from employee in db.tbEmployees
                        where travelDetail.employee_ID == employee.employee_ID
                        from subsidiary in db.tbSubsidiaries
                        where travel.subsidiary_ID == subsidiary.subsidiary_ID
                        from transporter in db.tbTransporters
                        where travel.transporter_ID == transporter.transporter_ID
                        select new TravelReportDataModel
                        {
                            Travel_ID = travel.travel_ID,
                            Departure_Date_and_Time = travel.departure_Date_and_Time,
                            Total_distance_Kilometers = travel.distance_Kilometers,
                            Total_travel_Cost_per_Date = totalTravelCost,
                            TotalTravelCost = travel.total_travel_Cost,
                            Subsidiary_Name = subsidiary.subsidiary_Name,
                            Subsidiary_Direction = subsidiary.subsidiary_Direction,
                            Employee_Name = employee.employee_Name,
                            Employee_Direction = employee.employee_Direction,
                            Distance_Kilometers = travelDetail.distance_Kilometers,
                            Transporter_Name = transporter.transporter_Name,
                            Transporter_Fee = transporter.transporter_Fee
                        };

            List<TravelReportDataModel> reportData = query.ToList();

            return reportData;
        }

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
            GetUserInformation();

            ViewBag.employee_ID = EmployeeInfo.employee_ID;
            ViewBag.employee_Name = EmployeeInfo.employee_Name;


            ViewBag.transporter_ID = new SelectList(db.tbTransporters, "transporter_ID", "transporter_Name");


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


            tbTravel tbTravel = new tbTravel
            {
                distance_Kilometers = 0,
                total_travel_Cost = 0,

            };

            return View(tbTravel);
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

            if (!ModelState.IsValid || ListEmployees.Count <= 0)
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
                catch (Exception)
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
            CleanVariables();

            GetUserInformation();



            ViewBag.employee_ID = EmployeeInfo.employee_ID;
            ViewBag.employee_Name = EmployeeInfo.employee_Name;

            ViewBag.employee_ID = new SelectList(db.tbEmployees, "employee_ID", "employee_Name", tbTravel.employee_ID);
            ViewBag.subsidiary_ID = new SelectList(db.tbSubsidiaries, "subsidiary_ID", "subsidiary_Name", tbTravel.subsidiary_ID);
            ViewBag.transporter_ID = new SelectList(db.tbTransporters, "transporter_ID", "transporter_Name", tbTravel.transporter_ID);


            ViewBag.transporter_Fee = tbTravel.tbTransporter.transporter_Fee;


            ViewBag.subsidiaryAddress = db.tbSubsidiaries.Find(tbTravel.subsidiary_ID).subsidiary_Direction;


            var travelDetails = from detail in db.tbTravelDetails
                                where detail.travel_ID == id
                                select detail;

            ListTravelDetails = travelDetails.ToList();

            ListEmployees = travelDetails.Select(detail => detail.employee_ID ?? 0).ToList();

            var employeesAvaliable = from employeeSubsidiary in db.tbEmployeesSubsidiaries
                        .Include(es => es.tbEmployee) // Include related entity tbSubsidiary
                        .Include(es => es.tbSubsidiary)   // Include related entity tbEmployee
                                                          // Add more .Include statements for other related entities as needed
                                     join employee in db.tbEmployees on employeeSubsidiary.employee_ID equals employee.employee_ID
                                     join travelDetail in travelDetails on employee.employee_ID equals travelDetail.employee_ID into travelDetailGroup
                                     from travelDetail in travelDetailGroup.DefaultIfEmpty()
                                     join travel in db.tbTravels on employeeSubsidiary.subsidiary_ID equals travel.subsidiary_ID
                                     where travelDetail == null
                                     select employeeSubsidiary;

            ViewBag.EmployeesAvaliable = employeesAvaliable.ToList();

            var employeesAdded = (from travelDetail in db.tbTravelDetails
                                  .Include(es => es.tbEmployee) // Include related entity tbSubsidiary
                                  .Include(es => es.tbTravel) // Include related entity tbSubsidiary
                                  join employee in db.tbEmployees
                                    on travelDetail.employee_ID equals employee.employee_ID
                                  select travelDetail).ToList();




            ViewBag.EmployeesAdded = employeesAdded;


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
            GetUserInformation();

            if (!ModelState.IsValid || ListEmployees.Count <= 0)
            {
                GetUserInformation();



                ViewBag.employee_ID = EmployeeInfo.employee_ID;
                ViewBag.employee_Name = EmployeeInfo.employee_Name;

                ViewBag.employee_ID = new SelectList(db.tbEmployees, "employee_ID", "employee_Name", tbTravel.employee_ID);
                ViewBag.subsidiary_ID = new SelectList(db.tbSubsidiaries, "subsidiary_ID", "subsidiary_Name", tbTravel.subsidiary_ID);
                ViewBag.transporter_ID = new SelectList(db.tbTransporters, "transporter_ID", "transporter_Name", tbTravel.transporter_ID);


                ViewBag.transporter_Fee = tbTravel.tbTransporter.transporter_Fee;


                ViewBag.subsidiaryAddress = db.tbSubsidiaries.Find(tbTravel.subsidiary_ID).subsidiary_Direction;


                var travelDetails = from detail in db.tbTravelDetails
                                    where detail.travel_ID == tbTravel.travel_ID
                                    select detail;

                ListTravelDetails = travelDetails.ToList();

                ListEmployees = travelDetails.Select(detail => detail.employee_ID ?? 0).ToList();

                var employeesAvaliable = from employeeSubsidiary in db.tbEmployeesSubsidiaries
                            .Include(es => es.tbEmployee) // Include related entity tbSubsidiary
                            .Include(es => es.tbSubsidiary)   // Include related entity tbEmployee
                                                              // Add more .Include statements for other related entities as needed
                                         join employee in db.tbEmployees on employeeSubsidiary.employee_ID equals employee.employee_ID
                                         join travelDetail in travelDetails on employee.employee_ID equals travelDetail.employee_ID into travelDetailGroup
                                         from travelDetail in travelDetailGroup.DefaultIfEmpty()
                                         join travel in db.tbTravels on employeeSubsidiary.subsidiary_ID equals travel.subsidiary_ID
                                         where travelDetail == null
                                         select employeeSubsidiary;

                ViewBag.EmployeesAvaliable = employeesAvaliable.ToList();

                var employeesAdded = (from travelDetail in db.tbTravelDetails
                                      .Include(es => es.tbEmployee) // Include related entity tbSubsidiary
                                      .Include(es => es.tbTravel) // Include related entity tbSubsidiary
                                      join employee in db.tbEmployees
                                        on travelDetail.employee_ID equals employee.employee_ID
                                      select travelDetail).ToList();




                ViewBag.EmployeesAdded = employeesAdded;

                return View(tbTravel);
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Entry(tbTravel).State = EntityState.Modified;

                    db.SaveChanges();


                    if (Modified)
                    {

                        if (ListTravelDetails is null)
                        {
                            return View(tbTravel);
                        }


                        var Transportists = db.tbTransporters.Find(tbTravel.transporter_ID);

                        if (Transportists == null)
                        {
                            return HttpNotFound();
                        }

                        var tbTravelDetails = db.tbTravelDetails.Where(x => x.travel_ID == tbTravel.travel_ID);

                        foreach (var travelDetail in tbTravelDetails)
                        {
                            var existingInList = ListTravelDetails.Find(x => x.travel_Detail_ID == travelDetail.travel_Detail_ID);
                            //var existingInListEmp = ListEmployees.Find(x => x. == travelDetail.travel_Detail_ID);

                            if (existingInList == null)
                            {
                                //Eliminar
                                db.tbTravelDetails.Remove(travelDetail);
                            }
                        }

                        foreach (var travelDetail in ListTravelDetails)
                        {
                            var existingEntity = db.tbTravelDetails.Find(travelDetail.travel_Detail_ID);

                            if (existingEntity != null)
                            {
                                db.Entry(existingEntity).State = EntityState.Modified;
                            }
                            else
                            {
                                var EmployeesSubsidiariesInfo = db.tbEmployeesSubsidiaries.Where(x => x.employee_ID == travelDetail.employee_ID && x.subsidiary_ID == tbTravel.subsidiary_ID).SingleOrDefault();
                                //employeesSubsidiary.employee_ID = tbEmployee.employee_ID;

                                travelDetail.travel_ID = tbTravel.travel_ID;
                                travelDetail.distance_Kilometers = (decimal)EmployeesSubsidiariesInfo.employeeSubsidiary_DistanceKM;
                                db.tbTravelDetails.Add(travelDetail);
                            }
                        }
                    }

                    var th = db.tbTravelDetails.ToList();

                    db.SaveChanges();

                    transaction.Commit();


                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return View(tbTravel);
                }
            }

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
        public JsonResult GenerateReportJS(int transporter_ID, string InitialDate, string FinalDate)
        {
            try
            {



                var vInitialDate = DateTime.SpecifyKind(Convert.ToDateTime(InitialDate), DateTimeKind.Local);
                var vFinalDate = DateTime.SpecifyKind(Convert.ToDateTime(FinalDate), DateTimeKind.Local);

                var query = from travel in db.tbTravels
                            where travel.transporter_ID == transporter_ID &&
                                  travel.departure_Date_and_Time >= vInitialDate &&
                                  travel.departure_Date_and_Time <= vFinalDate
                            let totalTravelCost = db.tbTravels.Sum(t => t.total_travel_Cost)
                            from travelDetail in db.tbTravelDetails
                            where travel.travel_ID == travelDetail.travel_ID
                            from employee in db.tbEmployees
                            where travelDetail.employee_ID == employee.employee_ID
                            from subsidiary in db.tbSubsidiaries
                            where travel.subsidiary_ID == subsidiary.subsidiary_ID
                            from transporter in db.tbTransporters
                            where travel.transporter_ID == transporter.transporter_ID
                            select new TravelReportDataModel
                            {
                                Travel_ID = travel.travel_ID,
                                Departure_Date_and_Time = travel.departure_Date_and_Time,
                                Total_distance_Kilometers = travel.distance_Kilometers,
                                Total_travel_Cost_per_Date = totalTravelCost,
                                TotalTravelCost = travel.total_travel_Cost,
                                Subsidiary_Name = subsidiary.subsidiary_Name,
                                Subsidiary_Direction = subsidiary.subsidiary_Direction,
                                Employee_Name = employee.employee_Name,
                                Employee_Direction = employee.employee_Direction,
                                Distance_Kilometers = travelDetail.distance_Kilometers,
                                Transporter_Name = transporter.transporter_Name,
                                Transporter_Fee = transporter.transporter_Fee
                            };

                List<TravelReportDataModel> reportData = query.ToList();

                return Json(reportData, JsonRequestBehavior.AllowGet);

            }
            catch (Exception Ex)
            {
                return Json(Ex.Message.ToString(), JsonRequestBehavior.DenyGet);
            }

        }
        
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
        public JsonResult AddEmployeeTravel(int Employee)
        {
            try
            {
                ListEmployees.Add(Employee);

                ListTravelDetails.Add(new tbTravelDetail { employee_ID = Employee });

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
        public JsonResult RemoveEmployeeTravel(int Employee)
        {
            try
            {

                ListEmployees.RemoveAll(item => item == Employee);

                ListTravelDetails.RemoveAll(employee => employee.employee_ID == Employee);

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
