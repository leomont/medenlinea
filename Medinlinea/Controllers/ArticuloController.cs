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
using System.Linq.Expressions;

namespace Medinlinea.Controllers
{
    public class ArticuloController : Controller
    {
        private MedinlineaEntities db = new MedinlineaEntities();
        private const int PAGE_SIZE = 3;

        // GET: Articulo
        public ActionResult Index(int page = 1)
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");
            ArticulosModel model = new ArticulosModel();
            UsuarioAuth admin = (UsuarioAuth)Session["usuario"];
            if (admin != null)
            {
                if (admin.rolNombre.Equals("Especialista"))
                {
                    int idEspecialista = 0;
                    List<Especialistas> listado = db.Especialistas.Include(m => m.Login).Where(m => m.IdLogin == admin.id).ToList();
                    if (listado.Count > 0)
                    {
                        idEspecialista = listado[0].IdEspecialista;
                        var data = db.Articulos.Where(m => m.IdEspecialista == idEspecialista).OrderByDescending(u => u.IdArticulo).Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE).ToList();
                        model = new ArticulosModel
                        {
                            Articulos = data,
                            Pagination = new PaginationModel
                            {
                                CurrentPage = page,
                                ItemsPerPage = PAGE_SIZE,
                                TotalItems = db.Articulos.ToList().Count()
                            }
                        };
                    }
                }
            }
            return View(model);
        }

        // GET: Articulo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articulos articulos = db.Articulos.Find(id);
            if (articulos == null)
            {
                return HttpNotFound();
            }
            return View(articulos);
        }

        // GET: Articulo/Create
        public ActionResult Create()
        {
            ViewBag.IdEspecialista = new SelectList(db.Especialistas, "IdEspecialista", "NombreEsp");
            return View();
        }

        // POST: Articulo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdArticulo,titulo,resumen,cuerpo,etiquetas,imagenArt")] Articulos articulos)
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            if (ModelState.IsValid && Session["usuario"] != null)
            {
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
                    UsuarioAuth usuario = (UsuarioAuth)Session["usuario"];
                    List<Especialistas> especialista = db.Especialistas.ToList().Where(m => m.IdLogin == usuario.id).ToList();
                    if (especialista.Count > 0)
                    {
                        articulos.Especialistas = especialista[0];
                        articulos.ImagenArt = encodedData;
                        db.Articulos.Add(articulos);
                        db.SaveChanges();
                        lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificación", cuerpo = "Articulo registrado exitosamente" });
                        TempData["mensajes"] = lstMensajes;
                        return RedirectToAction("Index");
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
            else
            {
                lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No cuenta con privilegios, inicie sesión" });
                TempData["mensajes"] = lstMensajes;
            }
            TempData["mensajes"] = lstMensajes;
            ViewBag.IdEspecialista = new SelectList(db.Especialistas, "IdEspecialista", "NombreEsp", articulos.IdEspecialista);
            return View(articulos);
        }

        // GET: Articulo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articulos articulos = db.Articulos.Find(id);
            if (articulos == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdEspecialista = new SelectList(db.Especialistas, "IdEspecialista", "NombreEsp", articulos.IdEspecialista);
            return View(articulos);
        }

        // POST: Articulo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdArticulo,titulo,resumen,cuerpo,etiquetas,imagenArt,IdEspecialista")] Articulos articulos)
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            if (ModelState.IsValid && Session["usuario"] != null)
            {
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
                if (!string.IsNullOrWhiteSpace(encodedData))
                {
                    articulos.ImagenArt = encodedData;
                }
                try
                {
                    UsuarioAuth usuario = (UsuarioAuth)Session["usuario"];
                    List<Especialistas> especialista = db.Especialistas.ToList().Where(m => m.IdLogin == usuario.id).ToList();
                    if (especialista.Count > 0)
                    {
                        db.Entry(articulos).State = EntityState.Modified;
                        db.SaveChanges();
                        lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificación", cuerpo = "Articulo registrado exitosamente" });
                        TempData["mensajes"] = lstMensajes;
                        return RedirectToAction("Index");
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
            ViewBag.IdEspecialista = new SelectList(db.Especialistas, "IdEspecialista", "NombreEsp", articulos.IdEspecialista);
            return View(articulos);
        }

        // GET: Articulo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articulos articulos = db.Articulos.Find(id);
            if (articulos == null)
            {
                return HttpNotFound();
            }
            return View(articulos);
        }

        // POST: Articulo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Articulos articulos = db.Articulos.Find(id);
            db.Articulos.Remove(articulos);
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
