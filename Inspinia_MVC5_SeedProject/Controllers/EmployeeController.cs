using ERP_GMEDINA.Attribute;
using ERP_GMEDINA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ERP_GMEDINA.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly FARSIMANEntities db = new FARSIMANEntities();

        private static List<tbEmployeesSubsidiary> ListEmployeesSubsidiaries { get; set; } = new List<tbEmployeesSubsidiary>();


        public static bool Modified { get; set; }

        private void CleanVariables()
        {

            Modified = false;

            ListEmployeesSubsidiaries.Clear();

        }
        // GET: /Employee/
        [SessionManager("Employee/Index")]
        public ActionResult Index()
        {

            CleanVariables();

            return View(db.tbEmployees.ToList());
        }

        // GET: /Employee/Details/5

        [SessionManager("Employee/Details")]
        public ActionResult Details(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            CleanVariables();

            tbEmployee tbEmployee = db.tbEmployees.Find(id);
            if (tbEmployee == null)
            {
                return HttpNotFound();
            }



            ViewBag.position_ID = new SelectList(db.tbPositions, "position_ID", "position_Name", tbEmployee.position_ID);

            var tbEmployeesSubsidiaries = db.tbEmployeesSubsidiaries.Where(x => x.employee_ID == tbEmployee.employee_ID);

            ListEmployeesSubsidiaries = tbEmployeesSubsidiaries.ToList();

            ViewBag.tbEmployeesSubsidiaries = tbEmployeesSubsidiaries.ToList();

            ViewBag.tbSubsidiary = db.tbSubsidiaries.Where(x => !tbEmployeesSubsidiaries.Select(y => y.subsidiary_ID).Contains(x.subsidiary_ID)).ToList();

            return View(tbEmployee);







        }

        // GET: /Employee/Create

        [SessionManager("Employee/Create")]
        public ActionResult Create()
        {

            CleanVariables();

            ViewBag.Positions = new SelectList(db.tbPositions, "position_ID", "position_Name");

            return View();
        }

        // POST: /Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionManager("Employee/Create")]
        public ActionResult Create([Bind(Include = "employee_ID,employee_Name,employee_Direction,position_ID")] tbEmployee tbEmployee)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Positions = new SelectList(db.tbPositions, "position_ID", "position_Name");
                return View(tbEmployee);
            }
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.tbEmployees.Add(tbEmployee);

                    db.SaveChanges();


                    foreach (var employeesSubsidiary in ListEmployeesSubsidiaries)
                    {
                        employeesSubsidiary.employee_ID = tbEmployee.employee_ID;
                        db.tbEmployeesSubsidiaries.Add(employeesSubsidiary);
                    }

                    db.SaveChanges();

                    transaction.Commit();

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return View(tbEmployee);
                }
            }
        }

        // GET: /Employee/Edit/5
        [SessionManager("Employee/Edit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            CleanVariables();


            tbEmployee tbEmployee = db.tbEmployees.Find(id);


            if (tbEmployee == null)
            {
                return HttpNotFound();
            }

            ViewBag.position_ID = new SelectList(db.tbPositions, "position_ID", "position_Name", tbEmployee.position_ID);

            var tbEmployeesSubsidiaries = db.tbEmployeesSubsidiaries.Where(x => x.employee_ID == tbEmployee.employee_ID);

            ListEmployeesSubsidiaries = tbEmployeesSubsidiaries.ToList();

            ViewBag.tbEmployeesSubsidiaries = tbEmployeesSubsidiaries.ToList();

            ViewBag.tbSubsidiary = db.tbSubsidiaries.Where(x => !tbEmployeesSubsidiaries.Select(y => y.subsidiary_ID).Contains(x.subsidiary_ID)).ToList();

            return View(tbEmployee);
        }

        // POST: /Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionManager("Employee/Edit")]
        public ActionResult Edit([Bind(Include = "employee_ID,employee_Name,employee_Direction,position_ID")] tbEmployee tbEmployee)
        {
            var tbEmployeesSubsidiaries = db.tbEmployeesSubsidiaries.Where(x => x.employee_ID == tbEmployee.employee_ID);

            if (!ModelState.IsValid)
            {
                ListEmployeesSubsidiaries.Clear();
                ViewBag.position_ID = new SelectList(db.tbPositions, "position_ID", "position_Name", tbEmployee.position_ID);


                ListEmployeesSubsidiaries = tbEmployeesSubsidiaries.ToList();

                ViewBag.tbEmployeesSubsidiaries = tbEmployeesSubsidiaries.ToList();

                ViewBag.tbSubsidiary = db.tbSubsidiaries.Where(x => !tbEmployeesSubsidiaries.Select(y => y.subsidiary_ID).Contains(x.subsidiary_ID)).ToList();

                return View(tbEmployee);
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Entry(tbEmployee).State = EntityState.Modified;
                    db.SaveChanges();


                    if (Modified)
                    {
                        foreach (var employeesSubsidiary in tbEmployeesSubsidiaries)
                        {
                            var existingInList = ListEmployeesSubsidiaries.Find(x => x.employeeSubsidiary_ID == employeesSubsidiary.employeeSubsidiary_ID);

                            if (existingInList == null)
                            {
                                //Eliminar
                                db.tbEmployeesSubsidiaries.Remove(employeesSubsidiary);
                            }

                        }


                        foreach (var employeesSubsidiary in ListEmployeesSubsidiaries)
                        {
                            var existingEntity = db.tbEmployeesSubsidiaries.Find(employeesSubsidiary.employeeSubsidiary_ID);

                            if (existingEntity != null)
                            {
                                db.Entry(existingEntity).State = EntityState.Modified;
                            }
                            else
                            {

                                employeesSubsidiary.employee_ID = tbEmployee.employee_ID;

                                db.tbEmployeesSubsidiaries.Add(employeesSubsidiary);
                            }
                        }
                    }



                    db.SaveChanges();


                    transaction.Commit();

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return View(tbEmployee);
                }
            }
        }


        [HttpPost]
        public JsonResult AddSubsidiary(tbEmployeesSubsidiary tbEmployeesSubsidiary)
        {
            bool response;
            try
            {
                ListEmployeesSubsidiaries.Add(tbEmployeesSubsidiary);

                Modified = true;

                response = true;
            }
            catch (Exception)
            {
                response = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoveSubsidiary(tbEmployeesSubsidiary tbEmployeesSubsidiary)
        {
            bool response;
            try
            {
                ListEmployeesSubsidiaries.RemoveAll(item => item.subsidiary_ID == tbEmployeesSubsidiary.subsidiary_ID);
                Modified = true;
                response = true;
            }
            catch (Exception)
            {
                response = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public List<tbPosition> GetPositions()
        {
            List<tbPosition> ListPositions = db.tbPositions.ToList();

            return ListPositions;
        }

        public List<tbSubsidiary> GetSubsidiaries()
        {
            List<tbSubsidiary> SubsidiariesList = db.tbSubsidiaries.ToList();

            return SubsidiariesList;
        }

        public List<tbSubsidiary> GetSubsidiaries(int Employee_ID)
        {

            var tbEmployeesSubsidiaries = db.tbEmployeesSubsidiaries.Where(x => x.employee_ID == Employee_ID);

            List<tbSubsidiary> SubsidiariesList = db.tbSubsidiaries.Where(x => !tbEmployeesSubsidiaries.Select(y => y.subsidiary_ID).Contains(x.subsidiary_ID)).ToList();

            return SubsidiariesList;
        }

        public List<tbEmployeesSubsidiary> GetEmployeesSubsidiary()
        {
            List<tbEmployeesSubsidiary> EmployeesSubsidiaries = db.tbEmployeesSubsidiaries.ToList();

            return EmployeesSubsidiaries;
        }

        // GET: /Employee/Delete/5
        [SessionManager("Employee/Delete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbEmployee tbEmployee = db.tbEmployees.Find(id);
            if (tbEmployee == null)
            {
                return HttpNotFound();
            }
            return View(tbEmployee);
        }

        // POST: /Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbEmployee tbEmployee = db.tbEmployees.Find(id);
            db.tbEmployees.Remove(tbEmployee);
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