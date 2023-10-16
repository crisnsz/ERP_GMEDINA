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
    public class tbUsersController : Controller
    {
        private FARSIMANEntities db = new FARSIMANEntities();

        // GET: /tbUsers/
        public ActionResult Index()
        {
            var tbusers = db.tbUsers.Include(t => t.tbEmployee);
            return View(tbusers.ToList());
        }

        // GET: /tbUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbUser tbUser = db.tbUsers.Find(id);
            if (tbUser == null)
            {
                return HttpNotFound();
            }
            return View(tbUser);
        }

        // GET: /tbUsers/Create
        public ActionResult Create()
        {
            ViewBag.employee_ID = new SelectList(db.tbEmployees, "employee_ID", "employee_Name");
            return View();
        }

        // POST: /tbUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="user_ID,employee_ID,user_Username,user_Password,user_IsActive,user_IsAdmin")] tbUser tbUser)
        {
            if (ModelState.IsValid)
            {
                db.tbUsers.Add(tbUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.employee_ID = new SelectList(db.tbEmployees, "employee_ID", "employee_Name", tbUser.employee_ID);
            return View(tbUser);
        }

        // GET: /tbUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbUser tbUser = db.tbUsers.Find(id);
            if (tbUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.employee_ID = new SelectList(db.tbEmployees, "employee_ID", "employee_Name", tbUser.employee_ID);
            return View(tbUser);
        }

        // POST: /tbUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="user_ID,employee_ID,user_Username,user_Password,user_IsActive,user_IsAdmin")] tbUser tbUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.employee_ID = new SelectList(db.tbEmployees, "employee_ID", "employee_Name", tbUser.employee_ID);
            return View(tbUser);
        }

        // GET: /tbUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbUser tbUser = db.tbUsers.Find(id);
            if (tbUser == null)
            {
                return HttpNotFound();
            }
            return View(tbUser);
        }

        // POST: /tbUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbUser tbUser = db.tbUsers.Find(id);
            db.tbUsers.Remove(tbUser);
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
