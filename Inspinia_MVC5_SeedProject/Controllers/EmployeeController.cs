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
            return View();
        }


        // POST: /Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [SessionManager("Employee/Details")]
        public ActionResult Create([Bind(Include="employee_ID,employee_Name,employee_Direction")] tbEmployee tbEmployee)
        {


            if (ModelState.IsValid)
            {
                db.tbEmployees.Add(tbEmployee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbEmployee);
        }

        [HttpPost]
        public JsonResult AddSubsidiary(tbSubsidiary tbSubsidiary)
        {

            var tbSubsidiarys = tbSubsidiary;
            //List<tbRolesUsuario> sessionRolesUsuario = new List<tbRolesUsuario>();
            //var list = (List<tbRolesUsuario>)Session["tbRolesUsuario"];
            //if (list == null)
            //{
            //    sessionRolesUsuario.Add(Roles);
            //    Session["tbRolesUsuario"] = sessionRolesUsuario;
            //}
            //else
            //{
            //    list.Add(Roles);
            //    Session["tbRolesUsuario"] = list;
            //}
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
        public ActionResult Edit([Bind(Include="employee_ID,employee_Name,employee_Direction")] tbEmployee tbEmployee)
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
