using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Medinlinea.Models;
using System.IO;

namespace Medinlinea.Controllers
{
    public class EspecialidadController : Controller
    {
        private MedinlineaEntities db = new MedinlineaEntities();
        private const int PAGE_SIZE = 3;
        // GET: Especialidad
        public ActionResult Index(int page=1)
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");

            var data = db.Especialidades.OrderByDescending(a => a.IdEspecialidad).Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);

            EspecialidadesModel model = new EspecialidadesModel
            {
                Especialidades = data,
                Pagination = new PaginationModel
                {
                    CurrentPage = page,
                    ItemsPerPage = PAGE_SIZE,
                    TotalItems = db.Especialidades.ToList().Count()
                }
            };
            return View(model);
        }

        // GET: Especialidad/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Especialidades especialidades = db.Especialidades.Find(id);
            if (especialidades == null)
            {
                return HttpNotFound();
            }
            return View(especialidades);
        }

        // GET: Especialidad/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Especialidad/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_nuevo()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            HttpPostedFileBase imagenM = (Request.Files.Count > 0) ? Request.Files[0] : null;
            string encodedData = string.Empty;

            string nombre = Request["Especialidad"];

            if (imagenM != null)
            {
                MemoryStream target = new MemoryStream();
                imagenM.InputStream.CopyTo(target);
                byte[] filebytes = target.ToArray();
                if (filebytes.Length > 0)
                {
                    string extension = Path.GetExtension(imagenM.FileName);
                    encodedData = "data:image/" + extension.Replace(".", "") + ";base64,";
                    encodedData += Convert.ToBase64String(filebytes, Base64FormattingOptions.None);
                }
            }

            try
            {
                UsuarioAuth admin = (UsuarioAuth)Session["usuario"];
                if (admin != null)
                {
                    if (admin.rolNombre.Equals("Administrador"))
                    {
                        List<Especialidades> listado = db.Especialidades.Where(m => m.NombreEspecialidad.ToUpper() == nombre.ToUpper()).ToList();
                        if (listado.Count == 0)
                        {
                            Especialidades especialidad = new Especialidades
                            {
                                NombreEspecialidad = nombre,
                                ImagenEspecialidad = encodedData,
                                Estado=false
                            };
                            db.Especialidades.Add(especialidad);
                            db.SaveChanges();
                            lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificacion", cuerpo = "Especialidad registrada exitosamente." });
                            TempData["mensajes"] = lstMensajes;
                        }
                        else
                        {
                            lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "Ya existe un registro con este nombre" });
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
            //return View();
            return RedirectToAction("index","Especialidad");
        }

        // GET: Especialidad/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Especialidades especialidades = db.Especialidades.Find(id);
            if (especialidades == null)
            {
                return HttpNotFound();
            }
            return View(especialidades);
        }

        // POST: Especialidad/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdEspecialidad,NombreEspecialidad,ImagenEspecialidad")] Especialidades especialidades)
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            HttpPostedFileBase imagenM = (Request.Files.Count > 0) ? Request.Files[0] : null;
            string encodedData = string.Empty;

            if (imagenM != null)
            {
                MemoryStream target = new MemoryStream();
                imagenM.InputStream.CopyTo(target);
                byte[] filebytes = target.ToArray();
                if (filebytes.Length > 0)
                {
                    string extension = Path.GetExtension(imagenM.FileName);
                    encodedData = "data:image/" + extension.Replace(".", "") + ";base64,";
                    encodedData += Convert.ToBase64String(filebytes, Base64FormattingOptions.None);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    UsuarioAuth admin = (UsuarioAuth)Session["usuario"];
                    if (admin != null)
                    {
                        if (admin.rolNombre.Equals("Administrador"))
                        {
                            Especialidades especialidadDb = db.Especialidades.Find(especialidades.IdEspecialidad);
                            if (especialidadDb != null)
                            {
                                if (!string.IsNullOrWhiteSpace(encodedData))
                                {
                                    especialidadDb.ImagenEspecialidad = encodedData;
                                }
                                especialidadDb.NombreEspecialidad = especialidades.NombreEspecialidad;
                                db.Entry(especialidadDb).State = EntityState.Modified;
                                db.SaveChanges();

                                lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificacion", cuerpo = "Especialidad registrado exitosamente." });
                                TempData["mensajes"] = lstMensajes;
                                return RedirectToAction("Index");
                            }
                            lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "Especialidad no encontrada" });
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

            return View(especialidades);
        }

        // GET: Especialidad/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Especialidades especialidades = db.Especialidades.Find(id);
            if (especialidades == null)
            {
                return HttpNotFound();
            }
            return View(especialidades);
        }

        // POST: Especialidad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Especialidades especialidades = db.Especialidades.Find(id);
            db.Especialidades.Remove(especialidades);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Carrusel()
        {
            string seleccionados = Request["seleccionados"];
            string[] listado = seleccionados.Split(',');
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];
            List<Especialidades> especialidades = db.Especialidades.ToList();
            
            foreach (Especialidades item in especialidades)
            {
                if (existArray(listado, item.IdEspecialidad))
                {
                    item.Estado = true;
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    item.Estado = false;
                    db.Entry(item).State = EntityState.Modified;
                }
            }
            db.SaveChanges();
            lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificacion", cuerpo = "Modificaciones registrado exitosamente." });
            TempData["mensajes"] = lstMensajes;
            return RedirectToAction("Index","Home");
        }


        private bool existArray(string[] listado, int valor)
        {
            foreach (string item in listado)
            {
                int idItem = 0;
                Int32.TryParse(item, out idItem);
                if (valor == idItem)
                {
                    return true;
                }
            }
            return false;
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
