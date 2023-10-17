using ERP_GMEDINA.Attribute;
using ERP_GMEDINA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ERP_GMEDINA.Controllers
{
    public class EmployeeController : Controller
    {
        private FARSIMANEntities db = new FARSIMANEntities();

        // GET: /Employee/
        [SessionManager("Employee/Index")]
        public ActionResult Index()
        {
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
            tbEmployee tbEmployee = db.tbEmployees.Find(id);
            if (tbEmployee == null)
            {
                return HttpNotFound();
            }
            return View(tbEmployee);
        }

        // GET: /Employee/Create

        [SessionManager("Employee/Create")]
        public ActionResult Create()
        {
            ViewBag.Positions = new SelectList(db.tbPositions, "position_ID", "position_Name");

            return View();
        }

        // POST: /Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionManager("Employee/Create")]
        public ActionResult Create([Bind(Include = "employee_ID,employee_Name,employee_Direction")] tbEmployee tbEmployee, int SelectedValue)
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


                    var newEmployeesPosition = new tbEmployeesPosition
                    {
                        position_ID = SelectedValue,
                        employee_ID = tbEmployee.employee_ID
                    };

                    db.tbEmployeesPositions.Add(newEmployeesPosition);

                    db.SaveChanges();

                    foreach (var employeesSubsidiary in listEmployeesSubsidiaries)
                    {
                        db.tbEmployeesSubsidiaries.Add(employeesSubsidiary);
                    }
                    db.SaveChanges();


                    transaction.Commit();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View(tbEmployee);
                }
            }
        }

        private List<tbSubsidiary> listSubsidiaries = new List<tbSubsidiary>();
        private List<tbEmployeesSubsidiary> listEmployeesSubsidiaries = new List<tbEmployeesSubsidiary>();

        [HttpPost]
        public JsonResult AddSubsidiary(tbEmployeesSubsidiary tbEmployeesSubsidiary)
        {
            bool response;
            try
            {
                //var newEmployeesSubsidiary = new tbEmployeesSubsidiary
                //{
                //    subsidiary_ID = tbSubsidiary.subsidiary_ID,

                //};
                listEmployeesSubsidiaries.Add(tbEmployeesSubsidiary);

                response = true;
            }
            catch (Exception)
            {
                response = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddSubsidiaryOLD(tbSubsidiary tbSubsidiary)
        {
            bool response;
            try
            {
                listSubsidiaries.Add(tbSubsidiary);


                var newEmployeesSubsidiary = new tbEmployeesSubsidiary
                {
                    subsidiary_ID = tbSubsidiary.subsidiary_ID,
                    
                };
                listEmployeesSubsidiaries.Add(newEmployeesSubsidiary);

                response = true;
            }
            catch (Exception)
            {
                response = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoveSubsidiary(tbSubsidiary tbSubsidiary)
        {
            listSubsidiaries.Remove(tbSubsidiary);

            return Json("Exito", JsonRequestBehavior.AllowGet);
        }

        // GET: /Employee/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: /Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "employee_ID,employee_Name,employee_Direction")] tbEmployee tbEmployee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbEmployee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbEmployee);
        }

        // GET: /Employee/Delete/5
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

        public List<tbSubsidiary> GetSubsidiaries()
        {
            List<tbSubsidiary> SubsidiariesList = new List<tbSubsidiary>();
            try
            {
                SubsidiariesList = db.tbSubsidiaries.ToList();
            }
            catch (Exception Ex)
            {
            }
            return SubsidiariesList;
        }

        public List<tbPosition> GetPositions()
        {
            List<tbPosition> ListPositions = new List<tbPosition>();

            try
            {
                ListPositions = db.tbPositions.ToList();
            }
            catch (Exception Ex)
            {
            }
            return ListPositions;
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