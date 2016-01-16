using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Medinlinea.Models;

namespace Medinlinea.Controllers
{
    public class CurriculumController : Controller
    {
        private MedinlineaEntities db = new MedinlineaEntities();
        private const int PAGE_SIZE = 3;
        
        // GET: Curriculum
        public ActionResult Index(int page = 1)
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");
            var data = db.Curriculums.OrderBy(a => a.IdCV).Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);

            if (Session["usuario"] != null)
            {
                UsuarioAuth usuario = (UsuarioAuth)Session["usuario"];
                List<Especialistas> listado = db.Especialistas.Where(m => m.Login.IdLogin == usuario.id).ToList();
                if (listado.Count > 0)
                {
                    List<Curriculums> lst_curriculums = new List<Curriculums>();
                    if (listado[0].Curriculums != null)
                    {
                        lst_curriculums.Add(listado[0].Curriculums);
                    }
                    data = lst_curriculums.AsQueryable();
                }
                else
                {
                    lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No cuenta con privilegios para realizar esta accion, inicie sesión" });
                }
            }
            else
            {
                lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No cuenta con privilegios para realizar esta accion, inicie sesión" });
            }

            CurriculumModel model = new CurriculumModel
            {
                Curriculums = data,
                Pagination = new PaginationModel
                {
                    CurrentPage = page,
                    ItemsPerPage = PAGE_SIZE,
                    TotalItems = db.Curriculums.ToList().Count()
                }
            };
            return View(model);
        }

        // GET: Curriculum/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curriculums curriculums = db.Curriculums.Find(id);
            if (curriculums == null)
            {
                return HttpNotFound();
            }
            return View(curriculums);
        }

        // GET: Curriculum/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Curriculum/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCV,ImagenCV,DescripcionCV")] Curriculums curriculums)
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

            curriculums.ImagenCV = encodedData;

            if (ModelState.IsValid)
            {
                try
                {
                    UsuarioAuth usuario = (UsuarioAuth)Session["usuario"];
                    if (usuario != null)
                    {
                        if (usuario.rolNombre.Equals("Especialista"))
                        {
                            List<Especialistas> especialistas = db.Especialistas.Where(m => m.IdLogin == usuario.id).ToList();
                            if (especialistas.Count > 0)
                            {
                                especialistas[0].Curriculums = curriculums;
                                db.Entry(especialistas[0]).State = EntityState.Modified;
                                db.SaveChanges();
                                lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificación", cuerpo = "Curriculum registrado exitosamente" });
                                TempData["mensajes"] = lstMensajes;
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No cuenta con privilegios para realizar esta accion, inicie sesión" });
                            }
                        }
                        else
                        {
                            lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No cuenta con privilegios para realizar esta accion, inicie sesión" });
                        }
                    }
                    else
                    {
                        lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No cuenta con privilegios para realizar esta accion, inicie sesión" });
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
            TempData["mensajes"] = lstMensajes;
            return View(curriculums);
        }

        // GET: Curriculum/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curriculums curriculums = db.Curriculums.Find(id);
            if (curriculums == null)
            {
                return HttpNotFound();
            }
            return View(curriculums);
        }

        // POST: Curriculum/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCV,ImagenCV,DescripcionCV")] Curriculums curriculums)
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
                Curriculums curriculumDb = db.Curriculums.Find(curriculums.IdCV);
                if (curriculumDb != null)
                {
                    if (!string.IsNullOrWhiteSpace(encodedData))
                    {
                        curriculumDb.ImagenCV = encodedData;
                    }
                    curriculumDb.DescripcionCV = curriculums.DescripcionCV;

                    db.Entry(curriculumDb).State = EntityState.Modified;
                    db.SaveChanges();
                    lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificación", cuerpo = "Curriculum registrado exitosamente" });
                    TempData["mensajes"] = lstMensajes;
                    return RedirectToAction("Index");
                }
            }
            return View(curriculums);
        }

        // GET: Curriculum/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curriculums curriculums = db.Curriculums.Find(id);
            if (curriculums == null)
            {
                return HttpNotFound();
            }
            return View(curriculums);
        }

        // POST: Curriculum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Curriculums curriculums = db.Curriculums.Find(id);
            List<Especialistas> listado = curriculums.Especialistas.ToList();
            foreach (Especialistas obj in listado)
            {
                obj.Curriculums = null;
            }
            db.Curriculums.Remove(curriculums);
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
