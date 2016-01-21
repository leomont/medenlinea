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
    public class UsuarioController : Controller
    {
        private MedinlineaEntities db = new MedinlineaEntities();
        private const int PAGE_SIZE = 3;

        // GET: Usuario
        public ActionResult Index(int page = 1)
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");

            var data = db.Usuarios.Include(m => m.Login).OrderByDescending(a => a.IdUsuario).Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);

            UsuarioModel model = new UsuarioModel
            {
                Usuarios = data,
                Pagination = new PaginationModel
                {
                    CurrentPage = page,
                    ItemsPerPage = PAGE_SIZE,
                    TotalItems = db.Usuarios.ToList().Count()
                }
            };
            return View(model);
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdUsuario,NombreUsuario,ApellidoUsuario,EmailUsuario,CelularUsuario,Direccion,Activo,CedulaUsuario,UsuarioLogin")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                string contrasena1 = Request["contrasena1"];
                string contrasena2 = Request["contrasena2"];
                string username = Request["username"];
                if (!string.IsNullOrWhiteSpace(contrasena1) && !string.IsNullOrWhiteSpace(contrasena2) && contrasena1.Equals(contrasena2) && !string.IsNullOrWhiteSpace(username))
                {
                    string password = CifradoDatos.cifrarPassword(contrasena1);
                    usuarios.Login = new Login
                    {
                        UsuarioLogin = username,
                        PasswordLogin = password,
                        IdTipoUsuario = 1
                    };
                    db.Usuarios.Add(usuarios);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(usuarios);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_nuevo()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            string nombrev = Request["nombre_vendedor"];
            string apellidosv = Request["apellido_vendedor"];
            string emailv = Request["email_vendedor"];
            string celularv = Request["celular-vendedor"];
            string direccionv = Request["direccion-vendedor"];
            string cedulav = Request["cedula-vendedor"];

            string contrasena1v = Request["contrasena_vendedor"];
            string contrasena2v = Request["confirmar_contrasena_vendedor"];

            if (!string.IsNullOrWhiteSpace(nombrev) && !string.IsNullOrWhiteSpace(apellidosv)
                && !string.IsNullOrWhiteSpace(emailv) && !string.IsNullOrWhiteSpace(contrasena1v) && !string.IsNullOrWhiteSpace(contrasena2v))
            {
                if (contrasena1v.Equals(contrasena2v))
                {
                    try
                    {
                        UsuarioAuth admin = (UsuarioAuth)Session["usuario"];
                        if (admin != null)
                        {
                            if (admin.rolNombre.Equals("Administrador"))
                            {
                                List<TipoUsuario> tipo_usuario = db.TipoUsuario.ToList().Where(m => m.IdTipoUsuario == 2).ToList();
                                if (tipo_usuario.Count > 0)
                                {
                                    Usuarios usuario = new Usuarios
                                    {
                                        NombreUsuario = nombrev,
                                        ApellidoUsuario = apellidosv,
                                        EmailUsuario = emailv,
                                        CelularUsuario = celularv,
                                        Direccion = direccionv,
                                        CedulaUsuario = cedulav,
                                        Activo = true,
                                        Login = new Login
                                        {
                                            TipoUsuario = tipo_usuario[0],
                                            UsuarioLogin = emailv,
                                            PasswordLogin = CifradoDatos.cifrarPassword(contrasena1v)
                                        },
                                    };
                                    db.Usuarios.Add(usuario);
                                    db.SaveChanges();
                                    lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificacion", cuerpo = "Vendedor registrado exitosamente." });
                                    TempData["mensajes"] = lstMensajes;
                                }
                                else
                                {
                                    lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "Ocurrio un error mientras se procesaba la solicitud" });
                                    TempData["mensajes"] = lstMensajes;
                                }
                            }
                            else
                            {
                                lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No cuenta con privilegios para realizar esta accion, inicie sesión" });
                                TempData["mensajes"] = lstMensajes;
                            }
                        }
                        else
                        {
                            lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No cuenta con privilegios para realizar esta accion, inicie sesión" });
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
                else
                {
                    lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "Las contraseñas no coinciden" });
                    TempData["mensajes"] = lstMensajes;
                }
            }
            else
            {
                lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "Algunos campos son requridos" });
                TempData["mensajes"] = lstMensajes;
            }
            return RedirectToAction("IngresarComercial", "Administrador");
        }


        // GET: Usuario/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdLogin = new SelectList(db.Login, "IdLogin", "UsuarioLogin", usuarios.IdLogin);
            return View(usuarios);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdUsuario,NombreUsuario,ApellidoUsuario,EmailUsuario,CelularUsuario,Direccion,Activo,CedulaUsuario")] Usuarios usuarios)
        {
            string contrasena1 = Request["contrasena_vendedor"];
            string contrasena2 = Request["confirmar_contrasena_vendedor"];
            int idUsuario = 0;
            Int32.TryParse(Request["IdUsuario"], out idUsuario);
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
                            List<Usuarios> vendedor = db.Usuarios.Include(m => m.Login).Where(m => m.IdUsuario == idUsuario).ToList();
                            if (vendedor.Count > 0)
                            {
                                vendedor[0].ApellidoUsuario = usuarios.ApellidoUsuario;
                                vendedor[0].CedulaUsuario = usuarios.CedulaUsuario;
                                vendedor[0].CelularUsuario = usuarios.CelularUsuario;
                                vendedor[0].Direccion = usuarios.Direccion;
                                vendedor[0].NombreUsuario = usuarios.NombreUsuario;
                                vendedor[0].Activo = usuarios.Activo;

                                if (contrasena1.Equals(contrasena2) && !string.IsNullOrWhiteSpace(contrasena1) && !contrasena1.Equals("##########"))
                                {
                                    vendedor[0].Login.PasswordLogin = CifradoDatos.cifrarPassword(contrasena1);
                                }
                                if (!vendedor[0].EmailUsuario.Equals(usuarios.EmailUsuario))
                                {
                                    vendedor[0].Login.UsuarioLogin = usuarios.EmailUsuario;
                                    vendedor[0].EmailUsuario = usuarios.EmailUsuario;
                                }

                                db.Entry(vendedor[0]).State = EntityState.Modified;
                                db.SaveChanges();
                                lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificacion", cuerpo = "Vendedor editado exitosamente." });
                                TempData["mensajes"] = lstMensajes;
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "Usuario no encontrado, verfique los datos" });
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
            return View(usuarios);
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // POST: Usuario/Delete/5
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
                        Usuarios usuarios = db.Usuarios.Find(id);
                        db.Usuarios.Remove(usuarios);
                        db.SaveChanges();
                        lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificacion", cuerpo = "Vendedor eliminado exitosamente." });
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
