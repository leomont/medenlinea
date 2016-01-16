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
using System.Xml;

namespace Medinlinea.Controllers
{
    public class PublicidadController : Controller
    {
        private MedinlineaEntities db = new MedinlineaEntities();

        // GET: Publicidad
        public ActionResult Index()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");

            var publicidades = db.Publicidades.Include(p => p.Especialistas);
            return View(publicidades.ToList());
        }

        // GET: Publicidad/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publicidades publicidades = db.Publicidades.Find(id);
            if (publicidades == null)
            {
                return HttpNotFound();
            }
            return View(publicidades);
        }

        // GET: Publicidad/Create
        public ActionResult Create()
        {
            ViewBag.IdMedico = new SelectList(db.Especialistas, "IdEspecialista", "NombreEsp");
            return View();
        }

        // POST: Publicidad/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPublicidad,TipoPublicidad,ImagenPub,CantidadClicks,IdMedico")] Publicidades publicidades)
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

            try
            {

                if (ModelState.IsValid)
                {
                    UsuarioAuth admin = (UsuarioAuth)Session["usuario"];
                    if (admin != null)
                    {
                        if (admin.rolNombre.Equals("Administrador"))
                        {
                            publicidades.ImagenPub = encodedData;
                            db.Publicidades.Add(publicidades);
                            db.SaveChanges();
                            lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificacion", cuerpo = "Publicidad registrada exitosamente." });
                            TempData["mensajes"] = lstMensajes;
                            return RedirectToAction("AdicionarPublicidad", "Administrador");
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
            return RedirectToAction("AdicionarPublicidad","Administrador");
        }

        // GET: Publicidad/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publicidades publicidades = db.Publicidades.Find(id);
            if (publicidades == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdMedico = new SelectList(db.Especialistas, "IdEspecialista", "NombreEsp", publicidades.IdMedico);
            return View(publicidades);
        }

        // POST: Publicidad/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPublicidad,TipoPublicidad,ImagenPub,CantidadClicks,IdMedico")] Publicidades publicidades)
        {
            if (ModelState.IsValid)
            {
                db.Entry(publicidades).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdMedico = new SelectList(db.Especialistas, "IdEspecialista", "NombreEsp", publicidades.IdMedico);
            return View(publicidades);
        }

        // GET: Publicidad/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publicidades publicidades = db.Publicidades.Find(id);
            if (publicidades == null)
            {
                return HttpNotFound();
            }
            return View(publicidades);
        }

        // POST: Publicidad/Delete/5
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
                        Publicidades publicidades = db.Publicidades.Find(id);
                        db.Publicidades.Remove(publicidades);
                        db.SaveChanges();
                        lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificacion", cuerpo = "Publicidad eliminada exitosamente." });
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

            return RedirectToAction("EliminarPublicidad","Administrador");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult updateImagenes(string tipoPublicidad)
        {
            List<Publicidades> listado = null;
            if (!string.IsNullOrWhiteSpace(tipoPublicidad))
            {
                listado = db.Publicidades.Where(m => m.TipoPublicidad.ToUpper() == tipoPublicidad.ToUpper()).ToList();
            }
            List<Publicidades> Result = new List<Publicidades>();
            foreach (Publicidades item in listado)
            {
                Result.Add(
                    new Publicidades
                    {
                        IdMedico = item.IdMedico,
                        IdPublicidad = item.IdPublicidad,
                        ImagenPub = item.ImagenPub,
                        TipoPublicidad = item.TipoPublicidad
                    });
            }

            var resulSet = Json(new { Result}, "application/json; charset=utf-8", JsonRequestBehavior.AllowGet);
            resulSet.MaxJsonLength = 2147483647;
            return resulSet;

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
