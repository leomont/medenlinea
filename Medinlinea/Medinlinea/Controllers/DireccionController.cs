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
    public class DireccionController : Controller
    {
        private MedinlineaEntities db = new MedinlineaEntities();
        private const int PAGE_SIZE = 3;
        // GET: Direccion
        public ActionResult Index(int page = 1)
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");

            var data = db.Direcciones.Include(m => m.Especialistas).OrderBy(a => a.IdEspecialista).Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);

            DireccionesModel model = new DireccionesModel
            {
                Direcciones = data,
                Pagination = new PaginationModel
                {
                    CurrentPage = page,
                    ItemsPerPage = PAGE_SIZE,
                    TotalItems = db.Direcciones.ToList().Count()
                }
            };

            return View(model);
        }

        // GET: Direccion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Direcciones direcciones = db.Direcciones.Find(id);
            if (direcciones == null)
            {
                return HttpNotFound();
            }
            return View(direcciones);
        }

        // GET: Direccion/Create
        public ActionResult Create()
        {
            ViewBag.IdEspecialista = new SelectList(db.Especialistas, "IdEspecialista", "NombreEsp");
            ViewBag.IdCiudad = new SelectList(db.Ciudades, "IdCiudad", "NombreCiudad");
            return View();
        }



        // POST: Direccion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdDireccion,Pais,Area,Calle,TelefonoFijo,Latitud,Longitud,IdEspecialista,IdCiudad")] Direcciones direcciones)
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
                            Especialistas especialista = db.Especialistas.Find(direcciones.IdEspecialista);
                            Ciudades ciudad = db.Ciudades.Find(direcciones.IdCiudad);
                            if (especialista != null && ciudad != null)
                            {
                                Direcciones nueva = new Direcciones
                                {
                                    Calle = direcciones.Calle,
                                    Ciudades = ciudad,
                                    Especialistas = especialista,
                                    Latitud = direcciones.Latitud,
                                    Longitud = direcciones.Longitud,
                                    TelefonoFijo = direcciones.TelefonoFijo
                                };
                                db.Direcciones.Add(nueva);
                                db.SaveChanges();
                                lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificacion", cuerpo = "Consultoria registrado exitosamente." });
                                TempData["mensajes"] = lstMensajes;
                                return RedirectToAction("Index");
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

            return View(direcciones);
        }

        // GET: Direccion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Direcciones direcciones = db.Direcciones.Find(id);
            if (direcciones == null)
            {
                return HttpNotFound();
            }

            ViewBag.listado = db.Especialistas.ToList();
            ViewBag.listado2 = db.Ciudades.ToList();

            return View(direcciones);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_nuevo()
        {
            return RedirectToAction("AgregarConsultorio", "Administrador");

        }

        // POST: Direccion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdDireccion,Pais,Area,Calle,TelefonoFijo,Latitud,Longitud,IdEspecialista,IdCiudad")] Direcciones direcciones)
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
                            Direcciones direccionDb = db.Direcciones.Find(direcciones.IdDireccion);
                            Ciudades ciudad = db.Ciudades.Find(direcciones.IdCiudad);
                            if (direccionDb != null && ciudad != null)
                            {
                                direccionDb.Latitud = direcciones.Latitud;
                                direccionDb.Longitud = direcciones.Longitud;
                                direccionDb.TelefonoFijo = direcciones.TelefonoFijo;
                                direccionDb.Ciudades = ciudad;
                                direccionDb.Calle = direcciones.Calle;

                                db.Entry(direccionDb).State = EntityState.Modified;
                                db.SaveChanges();
                                lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificacion", cuerpo = "Consultorio editado exitosamente." });
                                TempData["mensajes"] = lstMensajes;
                                return RedirectToAction("Index");
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

            return View(direcciones);
        }

        // GET: Direccion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Direcciones direcciones = db.Direcciones.Find(id);
            if (direcciones == null)
            {
                return HttpNotFound();
            }
            return View(direcciones);
        }

        // POST: Direccion/Delete/5
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
                        Direcciones direcciones = db.Direcciones.Find(id);
                        db.Direcciones.Remove(direcciones);
                        db.SaveChanges();
                        lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificacion", cuerpo = "Consultorio eliminado exitosamente." });
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
