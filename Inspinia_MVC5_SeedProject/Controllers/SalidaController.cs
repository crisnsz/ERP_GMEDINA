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
    public class SalidaController : Controller
    {
        private ERP_ZORZALEntities db = new ERP_ZORZALEntities();

        // GET: /Salida/
        public ActionResult Index()
        {
            var tbsalida = db.tbSalida.Include(t => t.tbUsuario).Include(t => t.tbUsuario1).Include(t => t.tbBodega).Include(t => t.tbBodega1).Include(t => t.tbEstadoMovimiento).Include(t => t.tbFactura).Include(t => t.tbTipoDevolucion).Include(t => t.tbTipoSalida);
            return View(tbsalida.ToList());
        }

        // GET: /Salida/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbSalida tbSalida = db.tbSalida.Find(id);
            if (tbSalida == null)
            {
                return HttpNotFound();
            }
            return View(tbSalida);
        }

        // GET: /Salida/Create
        public ActionResult Create()
        {
            ViewBag.sal_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario");
            ViewBag.sal_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario");
            ViewBag.bod_Id = new SelectList(db.tbBodega, "bod_Id", "bod_Nombre");
            ViewBag.sal_BodDestino = new SelectList(db.tbBodega, "bod_Id", "bod_Nombre");
            ViewBag.estm_Id = new SelectList(db.tbEstadoMovimiento, "estm_Id", "estm_Descripcion");
            ViewBag.fact_Id = new SelectList(db.tbFactura, "fact_Id", "fact_Codigo");
            ViewBag.tdev_Id = new SelectList(db.tbTipoDevolucion, "tdev_Id", "tdev_Descripcion");
            ViewBag.tsal_Id = new SelectList(db.tbTipoSalida, "tsal_Id", "tsal_Descripcion");
            return View();
        }

        // POST: /Salida/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="sal_Id,bod_Id,fact_Id,sal_FechaElaboracion,estm_Id,tsal_Id,sal_BodDestino,sal_EsAnulada,tdev_Id,sal_RazonAnulada,sal_UsuarioCrea,sal_FechaCrea,sal_UsuarioModifica,sal_FechaModifica")] tbSalida tbSalida)
        {
            if (ModelState.IsValid)
            {
                db.tbSalida.Add(tbSalida);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.sal_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbSalida.sal_UsuarioModifica);
            ViewBag.sal_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbSalida.sal_UsuarioCrea);
            ViewBag.bod_Id = new SelectList(db.tbBodega, "bod_Id", "bod_Nombre", tbSalida.bod_Id);
            ViewBag.sal_BodDestino = new SelectList(db.tbBodega, "bod_Id", "bod_Nombre", tbSalida.sal_BodDestino);
            ViewBag.estm_Id = new SelectList(db.tbEstadoMovimiento, "estm_Id", "estm_Descripcion", tbSalida.estm_Id);
            ViewBag.fact_Id = new SelectList(db.tbFactura, "fact_Id", "fact_Codigo", tbSalida.fact_Id);
            ViewBag.tdev_Id = new SelectList(db.tbTipoDevolucion, "tdev_Id", "tdev_Descripcion", tbSalida.tdev_Id);
            ViewBag.tsal_Id = new SelectList(db.tbTipoSalida, "tsal_Id", "tsal_Descripcion", tbSalida.tsal_Id);
            return View(tbSalida);
        }

        // GET: /Salida/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbSalida tbSalida = db.tbSalida.Find(id);
            if (tbSalida == null)
            {
                return HttpNotFound();
            }
            ViewBag.sal_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbSalida.sal_UsuarioModifica);
            ViewBag.sal_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbSalida.sal_UsuarioCrea);
            ViewBag.bod_Id = new SelectList(db.tbBodega, "bod_Id", "bod_Nombre", tbSalida.bod_Id);
            ViewBag.sal_BodDestino = new SelectList(db.tbBodega, "bod_Id", "bod_Nombre", tbSalida.sal_BodDestino);
            ViewBag.estm_Id = new SelectList(db.tbEstadoMovimiento, "estm_Id", "estm_Descripcion", tbSalida.estm_Id);
            ViewBag.fact_Id = new SelectList(db.tbFactura, "fact_Id", "fact_Codigo", tbSalida.fact_Id);
            ViewBag.tdev_Id = new SelectList(db.tbTipoDevolucion, "tdev_Id", "tdev_Descripcion", tbSalida.tdev_Id);
            ViewBag.tsal_Id = new SelectList(db.tbTipoSalida, "tsal_Id", "tsal_Descripcion", tbSalida.tsal_Id);
            return View(tbSalida);
        }

        // POST: /Salida/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="sal_Id,bod_Id,fact_Id,sal_FechaElaboracion,estm_Id,tsal_Id,sal_BodDestino,sal_EsAnulada,tdev_Id,sal_RazonAnulada,sal_UsuarioCrea,sal_FechaCrea,sal_UsuarioModifica,sal_FechaModifica")] tbSalida tbSalida)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbSalida).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.sal_UsuarioModifica = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbSalida.sal_UsuarioModifica);
            ViewBag.sal_UsuarioCrea = new SelectList(db.tbUsuario, "usu_Id", "usu_NombreUsuario", tbSalida.sal_UsuarioCrea);
            ViewBag.bod_Id = new SelectList(db.tbBodega, "bod_Id", "bod_Nombre", tbSalida.bod_Id);
            ViewBag.sal_BodDestino = new SelectList(db.tbBodega, "bod_Id", "bod_Nombre", tbSalida.sal_BodDestino);
            ViewBag.estm_Id = new SelectList(db.tbEstadoMovimiento, "estm_Id", "estm_Descripcion", tbSalida.estm_Id);
            ViewBag.fact_Id = new SelectList(db.tbFactura, "fact_Id", "fact_Codigo", tbSalida.fact_Id);
            ViewBag.tdev_Id = new SelectList(db.tbTipoDevolucion, "tdev_Id", "tdev_Descripcion", tbSalida.tdev_Id);
            ViewBag.tsal_Id = new SelectList(db.tbTipoSalida, "tsal_Id", "tsal_Descripcion", tbSalida.tsal_Id);
            return View(tbSalida);
        }

        // GET: /Salida/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbSalida tbSalida = db.tbSalida.Find(id);
            if (tbSalida == null)
            {
                return HttpNotFound();
            }
            return View(tbSalida);
        }

        // POST: /Salida/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbSalida tbSalida = db.tbSalida.Find(id);
            db.tbSalida.Remove(tbSalida);
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
