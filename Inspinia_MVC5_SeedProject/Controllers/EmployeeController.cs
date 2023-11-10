using ERP_GMEDINA.Attribute;
using ERP_GMEDINA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web.Mvc;
using System.Web;


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
            Session["employeesSubsidiary"] = null;
            //ListEmployeesSubsidiaries.Clear();

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
            Session["employeesSubsidiary"] = null;


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

            ViewBag.Positions = new SelectList(db.tbPositions, "position_ID", "position_Name");
            if (!ModelState.IsValid)
            {
                return View(tbEmployee);
            }
            using (TransactionScope Tran = new TransactionScope())
            {
                try
                {
                    var EmployeeInsert = db.UDP_Gral_tbEmployees_Insert(tbEmployee.employee_Name, tbEmployee.employee_Direction, tbEmployee.position_ID).FirstOrDefault();

                    var listEmployeesSubsidiarySession = (List<tbEmployeesSubsidiary>)Session["employeesSubsidiary"];

                    if (EmployeeInsert is null || EmployeeInsert.ErrorMessage.StartsWith("-1"))
                    {
                        ModelState.AddModelError("", "Error al resgistrar el empleado");
                        return View(tbEmployee);
                    }

                    var idOrError = Convert.ToInt32(EmployeeInsert.ErrorMessage);

                    if (idOrError <= 0)
                    {
                        ModelState.AddModelError("", "Error al resgistrar el empleado");
                        Session["employeesSubsidiary"] = null;
                        return View(tbEmployee);
                    }

                    if (ListEmployeesSubsidiaries != null && ListEmployeesSubsidiaries.Any())
                    {
                        foreach (tbEmployeesSubsidiary employeesSubsidiary in ListEmployeesSubsidiaries)
                        {
                            var EmployeesSubsidaries = db.UDP_Gral_tbEmployeesSubsidiaries_Insert(
                                idOrError, employeesSubsidiary.subsidiary_ID, employeesSubsidiary.employeeSubsidiary_DistanceKM)
                                .FirstOrDefault();

                            if (EmployeesSubsidaries is null || EmployeesSubsidaries.ErrorMessage.StartsWith("-1"))
                            {
                                ModelState.AddModelError("", "No se pudo agregar el registro detalle");
                                return View(tbEmployee);
                            }
                        }
                    }

                    Tran.Complete();
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
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                }

                var listEmployeesSubsidiarySession = (List<tbEmployeesSubsidiary>)Session["employeesSubsidiary"];

                if (listEmployeesSubsidiarySession == null)
                {
                    listEmployeesSubsidiarySession = new List<tbEmployeesSubsidiary>();
                    Session["employeesSubsidiary"] = listEmployeesSubsidiarySession;
                }

                listEmployeesSubsidiarySession.Add(tbEmployeesSubsidiary);
                Modified = true;

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                // logger.LogError($"Error in AddSubsidiary: {ex.Message}");

                return Json(new { success = false, error = $"No se pudo asignar el empleado a la subsidiaria {tbEmployeesSubsidiary.tbSubsidiary.subsidiary_Name}." }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult RemoveSubsidiary(tbEmployeesSubsidiary tbEmployeesSubsidiary)
        {
            try
            {
                var listEmployeesSubsidiarySession = (List<tbEmployeesSubsidiary>)Session["employeesSubsidiary"];

                if (listEmployeesSubsidiarySession != null)
                {
                    listEmployeesSubsidiarySession.RemoveAll(item => item.subsidiary_ID == tbEmployeesSubsidiary.subsidiary_ID);
                    Modified = true;
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Handle the case where the session list is null
                    return Json(new { success = false, error = "Session list is null." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                // logger.LogError($"Error in RemoveSubsidiary: {ex.Message}");

                return Json(new { success = false, error = $"No se pudo eliminar la subsidiaria asiganda: {tbEmployeesSubsidiary.tbSubsidiary.subsidiary_Name}." }, JsonRequestBehavior.AllowGet);
            }
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