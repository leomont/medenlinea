using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Medinlinea.Models;
using System.Web.Script.Serialization;

namespace Medinlinea.Controllers
{
    public class MembresiaController : Controller
    {
        private MedinlineaEntities db = new MedinlineaEntities();
        private const int PAGE_SIZE = 3;

        // GET: Membresia
        public ActionResult Index(int page = 1)
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");

            var data = db.Membresias.Include(m => m.Especialistas).OrderByDescending(a => a.IdMembresia).Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);

            MembresiasModel model = new MembresiasModel
            {
                Membresias = data,
                Pagination = new PaginationModel
                {
                    CurrentPage = page,
                    ItemsPerPage = PAGE_SIZE,
                    TotalItems = db.Membresias.ToList().Count()
                }
            };
            return View(model);
        }

        // GET: Membresia/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Especialistas> membresias = db.Especialistas.Include(m=> m.Membresias).Where(m=> m.IdSuscripcion==id).ToList();
            if (membresias.Count == 0)
            {
                return HttpNotFound();
            }
            return View(membresias[0]);
        }

        // GET: Membresia/Create
        public ActionResult Create()
        {
            ViewBag.listado= db.Especialistas.ToList();
            return View();
        }

        // POST: Membresia/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdMembresia,TipoMembresia,FechaInicio,FechaFin")] Membresias membresias)
        {
            int idEspecialista = 0;
            Int32.TryParse(Request["IdEspecialista"], out idEspecialista);
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            if (ModelState.IsValid)
            {
                try
                {
                    UsuarioAuth admin = (UsuarioAuth)Session["usuario"];
                    if (admin != null)
                    {
                        if (admin.rolNombre.Equals("Administrador"))
                        {
                            Especialistas especialista = db.Especialistas.Find(idEspecialista);
                            Membresias membresiaAnt = especialista.Membresias;
                            if (especialista != null)
                            {
                                membresias.Estado = true;
                                especialista.Membresias = membresias;
                                db.Membresias.Add(membresias);
                                db.SaveChanges();
                                if (membresiaAnt != null)
                                {
                                    db.Membresias.Remove(membresiaAnt);
                                }
                                db.SaveChanges();
                                lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificacion", cuerpo = "Membresia registrado exitosamente." });
                                TempData["mensajes"] = lstMensajes;
                                return RedirectToAction("Index");
                            }
                            else
                            {

                                lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "Especialista no encontrado" });
                                TempData["mensajes"] = lstMensajes;
                            }
                        }
                        else
                        {
                            lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No cuenta con privilegios para realizar esta acción, inicie sesión" });
                            TempData["mensajes"] = lstMensajes;
                        }
                    }
                    else
                    {

                        lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No cuenta con privilegios para realizar esta acción, inicie sesión" });
                        TempData["mensajes"] = lstMensajes;
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    var sb = new System.Text.StringBuilder();
                    foreach (var failure in ex.EntityValidationErrors)
                    {
                        sb.AppendFormat("{0} failed validation", failure.Entry.Entity.GetType());
                        foreach (var error in failure.ValidationErrors)
                        {
                            sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                            lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = error.PropertyName + ":" + error.ErrorMessage });
                            sb.AppendLine();
                        }
                    }
                    lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = sb.ToString() });
                    TempData["mensajes"] = lstMensajes;
                    SystemLog log = new SystemLog();
                    log.ErrorLog(sb.ToString());
                    throw new Exception(sb.ToString());
                }
            }

            return View(membresias);
        }

        // GET: Membresia/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Membresias membresias = db.Membresias.Find(id);
            if (membresias == null)
            {
                return HttpNotFound();
            }
            return View(membresias);
        }

        // POST: Membresia/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdMembresia,TipoMembresia,FechaInicio,FechaFin")] Membresias membresias)
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];
            
            if (ModelState.IsValid)
            {
                try
                {
                    UsuarioAuth admin = (UsuarioAuth)Session["usuario"];
                    if (admin != null)
                    {
                        if (admin.rolNombre.Equals("Administrador"))
                        {
                            Membresias membresiaDb = db.Membresias.Find(membresias.IdMembresia);
                            if (membresiaDb != null)
                            {
                                membresiaDb.TipoMembresia = membresias.TipoMembresia;
                                membresiaDb.FechaInicio = membresias.FechaInicio;
                                membresiaDb.FechaFin = membresias.FechaFin;

                                db.Entry(membresiaDb).State = EntityState.Modified;
                                db.SaveChanges();
                                lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificacion", cuerpo = "Membresia editada exitosamente." });
                                TempData["mensajes"] = lstMensajes;
                                return RedirectToAction("Index");
                            }
                            lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "La membresia solicitada no se encontro" });
                            TempData["mensajes"] = lstMensajes;
                        }
                        else
                        {
                            lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No cuenta con privilegios para realizar esta acción, inicie sesión" });
                            TempData["mensajes"] = lstMensajes;
                        }
                    }
                    else
                    {
                        lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No cuenta con privilegios para realizar esta acción, inicie sesión" });
                        TempData["mensajes"] = lstMensajes;
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    var sb = new System.Text.StringBuilder();
                    foreach (var failure in ex.EntityValidationErrors)
                    {
                        sb.AppendFormat("{0} failed validation", failure.Entry.Entity.GetType());
                        foreach (var error in failure.ValidationErrors)
                        {
                            sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                            lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = error.PropertyName + ":" + error.ErrorMessage });
                            sb.AppendLine();
                        }
                    }
                    lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = sb.ToString() });
                    TempData["mensajes"] = lstMensajes;
                    SystemLog log = new SystemLog();
                    log.ErrorLog(sb.ToString());
                    throw new Exception(sb.ToString());
                }
            }
            return View(membresias);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit_activar()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            int idEspecialista = 0;
            Int32.TryParse(Request["idEspecialista"], out idEspecialista);
            bool estado = true;
            Boolean.TryParse(Request["estadoMembresia"], out estado);
            if (ModelState.IsValid)
            {
                try
                {
                    UsuarioAuth admin = (UsuarioAuth)Session["usuario"];
                    if (admin != null)
                    {
                        if (admin.rolNombre.Equals("Administrador"))
                        {
                            Especialistas especialista = db.Especialistas.Find(idEspecialista);
                            if (especialista != null)
                            {
                                if (especialista.Membresias != null)
                                {
                                    especialista.Membresias.Estado = estado;

                                    db.Entry(especialista).State = EntityState.Modified;
                                    db.SaveChanges();
                                    lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificacion", cuerpo = "Membresia editada exitosamente." });
                                    TempData["mensajes"] = lstMensajes;
                                    return RedirectToAction("GestionarSuscripcion","Administrador");
                                }
                            }
                            lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "La membresia solicitada no se encontro" });
                            TempData["mensajes"] = lstMensajes;
                        }
                        else
                        {
                            lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No cuenta con privilegios para realizar esta acción, inicie sesión" });
                            TempData["mensajes"] = lstMensajes;
                        }
                    }
                    else
                    {
                        lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No cuenta con privilegios para realizar esta acción, inicie sesión" });
                        TempData["mensajes"] = lstMensajes;
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    var sb = new System.Text.StringBuilder();
                    foreach (var failure in ex.EntityValidationErrors)
                    {
                        sb.AppendFormat("{0} failed validation", failure.Entry.Entity.GetType());
                        foreach (var error in failure.ValidationErrors)
                        {
                            sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                            lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = error.PropertyName + ":" + error.ErrorMessage });
                            sb.AppendLine();
                        }
                    }
                    lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = sb.ToString() });
                    TempData["mensajes"] = lstMensajes;
                    SystemLog log = new SystemLog();
                    log.ErrorLog(sb.ToString());
                    throw new Exception(sb.ToString());
                }
            }
            return RedirectToAction("GestionarSuscripcion", "Administrador");
        }

        // GET: Membresia/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Especialistas> membresias = db.Especialistas.Include(m => m.Membresias).Where(m => m.IdSuscripcion == id).ToList();
            if (membresias.Count == 0)
            {
                return HttpNotFound();
            }
            return View(membresias[0]);
        }

        // POST: Membresia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];
            try
            {
                UsuarioAuth admin = (UsuarioAuth)Session["usuario"];
                if (admin != null)
                {
                    if (admin.rolNombre.Equals("Administrador"))
                    {
                        List<Especialistas> especialista = db.Especialistas.Include(m => m.Membresias).Where(m => m.Membresias.IdMembresia == id).ToList();
                        if (especialista.Count > 0)
                        {
                            especialista[0].Membresias = null;
                            db.Entry(especialista[0]).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        Membresias membresias = db.Membresias.Find(id);
                        db.Membresias.Remove(membresias);
                        db.SaveChanges();
                        lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificacion", cuerpo = "Membresia eliminada exitosamente." });
                        TempData["mensajes"] = lstMensajes;

                    }
                    else
                    {
                        lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No cuenta con privilegios para realizar esta acción, inicie sesión" });
                        TempData["mensajes"] = lstMensajes;
                    }
                }
                else
                {
                    lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No cuenta con privilegios para realizar esta acción, inicie sesión" });
                    TempData["mensajes"] = lstMensajes;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var sb = new System.Text.StringBuilder();
                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = error.PropertyName + ":" + error.ErrorMessage });
                        sb.AppendLine();
                    }
                }
                lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = sb.ToString() });
                TempData["mensajes"] = lstMensajes;
                SystemLog log = new SystemLog();
                log.ErrorLog(sb.ToString());
                throw new Exception(sb.ToString());
            }
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
