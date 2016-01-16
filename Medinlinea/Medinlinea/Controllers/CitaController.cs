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
using System.Text;
using System.Net.Mime;

namespace Medinlinea.Controllers
{
    public class CitaController : Controller
    {
        private MedinlineaEntities db = new MedinlineaEntities();

        // GET: Cita
        public ActionResult Index()
        {
            var citas = db.Citas.Include(c => c.Especialistas);
            return View(citas.ToList());
        }

        // GET: Cita/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Citas citas = db.Citas.Find(id);
            if (citas == null)
            {
                return HttpNotFound();
            }
            return View(citas);
        }

        // GET: Cita/Create
        public ActionResult Create()
        {
            ViewBag.IdEspecialista = new SelectList(db.Especialistas, "IdEspecialista", "NombreEsp");
            return View();
        }

        // POST: Cita/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCita,NombrePac,Sexo,FechaNacimientoPac,EmailPac,MensajeCita,Imagen1,Imagen2,Imagen3,Imagen4,IdEspecialista,TelefonoPac")] Citas citas)
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            HttpPostedFileBase imagen1 = (Request.Files.Count > 0) ? Request.Files[0] : null;
            HttpPostedFileBase imagen2 = (Request.Files.Count > 1) ? Request.Files[1] : null;
            HttpPostedFileBase imagen3 = (Request.Files.Count > 2) ? Request.Files[2] : null;
            HttpPostedFileBase imagen4 = (Request.Files.Count > 3) ? Request.Files[3] : null;
            List<ArchivoAdjunto> listado = new List<ArchivoAdjunto>();
            MemoryStream target1 = new MemoryStream();
            MemoryStream target2 = new MemoryStream();
            MemoryStream target3 = new MemoryStream();
            MemoryStream target4 = new MemoryStream();

            Especialistas especialista = db.Especialistas.Find(citas.IdEspecialista);
            if (especialista != null)
            {
                if (imagen1 != null)
                {
                    imagen1.InputStream.CopyTo(target1);

                    listado.Add(
                        new ArchivoAdjunto
                        {
                            archivo = target1,
                            tipo = imagen1.ContentType,
                            nombre = imagen1.FileName
                        }
                        );
                }
                if (imagen2 != null)
                {
                    imagen2.InputStream.CopyTo(target2);

                    listado.Add(new ArchivoAdjunto
                    {
                        archivo = target2,
                        tipo = imagen2.ContentType,
                        nombre = imagen2.FileName
                    }
                        );
                }
                if (imagen3 != null)
                {
                    imagen3.InputStream.CopyTo(target3);

                    listado.Add(new ArchivoAdjunto
                    {
                        archivo = target3,
                        tipo = imagen3.ContentType,
                        nombre = imagen3.FileName
                    }
                        );
                }
                if (imagen4 != null)
                {
                    imagen4.InputStream.CopyTo(target4);

                    listado.Add(new ArchivoAdjunto
                    {
                        archivo = target4,
                        tipo = imagen4.ContentType,
                        nombre = imagen4.FileName
                    }
                        );
                }

                //Creo corrreo con clave para enviar al usuario
                StringBuilder bodyMail = new StringBuilder();
                bodyMail.AppendLine(citas.NombrePac + " " + citas.MensajeCita + "</br>");
                /*bodyMail.AppendLine("Dirijase al siguiente enlace para continuar con el proceso. " + "<a href=\"" + informacionHost + "/Home/ValidarRestaurar?tokenCorreo=" + clave_email + "&tokenClave=" + clave_code + "\"> Restaurar contraseña perfil. </a>" + "</br>");
                bodyMail.AppendLine("Fecha:" + DateTime.Now.ToString() + "</br>");*/
                string subject = "Notificación modificación contraseña.";
                Mail mail = new Mail(especialista.EmailEsp, subject, bodyMail);

                if (mail.sendMailAttachments(listado).Result)
                {
                    if (ModelState.IsValid)
                    {
                        db.Citas.Add(citas);
                        db.SaveChanges();

                        lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificacion", cuerpo = "Consulta registrada exitosamente." });
                        TempData["mensajes"] = lstMensajes;
                        return RedirectToAction("Index", "Especialista");
                    }
                }
                else
                {
                    lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "Ocurrio un problema mientras se procesaba la solicitud" });
                    TempData["mensajes"] = lstMensajes;
                }
            }
            else
            {
                lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No se pudo encontrar el especialista solicitado" });
                TempData["mensajes"] = lstMensajes;
            }
            return RedirectToAction("Index", "Especialista");
        }

        // GET: Cita/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Citas citas = db.Citas.Find(id);
            if (citas == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdEspecialista = new SelectList(db.Especialistas, "IdEspecialista", "NombreEsp", citas.IdEspecialista);
            return View(citas);
        }

        // POST: Cita/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCita,NombrePac,Sexo,FechaNacimientoPac,EmailPac,MensajeCita,Imagen1,Imagen2,Imagen3,Imagen4,IdEspecialista,TelefonoPac")] Citas citas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(citas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdEspecialista = new SelectList(db.Especialistas, "IdEspecialista", "NombreEsp", citas.IdEspecialista);
            return View(citas);
        }

        // GET: Cita/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Citas citas = db.Citas.Find(id);
            if (citas == null)
            {
                return HttpNotFound();
            }
            return View(citas);
        }

        // POST: Cita/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Citas citas = db.Citas.Find(id);
            db.Citas.Remove(citas);
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
