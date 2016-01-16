using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Medinlinea.Models;

namespace Medinlinea.Controllers
{
    public class EstadisticaController : Controller
    {
        private MedinlineaEntities db = new MedinlineaEntities();

        // GET: Estadistica
        public ActionResult Index()
        {
            var estadisticas = db.Estadisticas.Include(e => e.Especialistas);
            return View(estadisticas.ToList());
        }

        // GET: Estadistica/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estadisticas estadisticas = db.Estadisticas.Find(id);
            if (estadisticas == null)
            {
                return HttpNotFound();
            }
            return View(estadisticas);
        }

        // GET: Estadistica/Create
        public ActionResult Create()
        {
            ViewBag.IdEspecialista = new SelectList(db.Especialistas, "IdEspecialista", "NombreEsp");
            return View();
        }

        // POST: Estadistica/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdEstadistica,CantBusquedas,ClickEnPaginaWeb,ConsultasRealizadas,ClickEnCurriculum,IdEspecialista")] Estadisticas estadisticas)
        {
            if (ModelState.IsValid)
            {
                db.Estadisticas.Add(estadisticas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdEspecialista = new SelectList(db.Especialistas, "IdEspecialista", "NombreEsp", estadisticas.IdEspecialista);
            return View(estadisticas);
        }

        // GET: Estadistica/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estadisticas estadisticas = db.Estadisticas.Find(id);
            if (estadisticas == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdEspecialista = new SelectList(db.Especialistas, "IdEspecialista", "NombreEsp", estadisticas.IdEspecialista);
            return View(estadisticas);
        }

        // POST: Estadistica/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdEstadistica,CantBusquedas,ClickEnPaginaWeb,ConsultasRealizadas,ClickEnCurriculum,IdEspecialista")] Estadisticas estadisticas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estadisticas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdEspecialista = new SelectList(db.Especialistas, "IdEspecialista", "NombreEsp", estadisticas.IdEspecialista);
            return View(estadisticas);
        }

        // GET: Estadistica/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estadisticas estadisticas = db.Estadisticas.Find(id);
            if (estadisticas == null)
            {
                return HttpNotFound();
            }
            return View(estadisticas);
        }

        // POST: Estadistica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Estadisticas estadisticas = db.Estadisticas.Find(id);
            db.Estadisticas.Remove(estadisticas);
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
