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
using System.Runtime.Remoting.Contexts;

namespace Medinlinea.Controllers
{
    public class EspecialistaController : Controller
    {
        private MedinlineaEntities db = new MedinlineaEntities();
        private const int PAGE_SIZE = 3;

        // GET: Especialista
        //Realizado Por: Leonardo Montes
        //Fecha: 26/12/2015
        //Nota: Se modifica el metodo index por defecto de mvc, para realizar paginacion de especialistas
        public ActionResult Index(string nombre, string ciudad, int page = 1, int idEspecialidad = 0)
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");

            int max_size = db.Especialistas.ToList().Count();

            var data = db.Especialistas.Include(e => e.Curriculums).Include(e => e.Especialidades).Include(e => e.Membresias).Where(m=> m.Activo == true && m.Membresias.Estado == true).OrderByDescending(e => e.IdEspecialista).Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            
            if (idEspecialidad > 0)
            {
                Especialidades especialidad = db.Especialidades.Find(idEspecialidad);
                data = db.Especialistas.Include(e => e.Curriculums).Include(e => e.Especialidades).Include(e => e.Membresias).Where(m => m.Especialidades.IdEspecialidad == idEspecialidad && m.Activo == true && m.Membresias.Estado == true).OrderByDescending(e => e.IdEspecialista).Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE); 
                max_size = db.Especialistas.Where(m => m.IdEspecialidad == idEspecialidad && m.Activo == true && m.Membresias.Estado == true).ToList().Count;
                if (especialidad != null)
                {
                    ViewBag.especialidad = especialidad.NombreEspecialidad;
                }
            }
            else if (!string.IsNullOrWhiteSpace(nombre) && string.IsNullOrWhiteSpace(ciudad))
            {
                data = db.Especialistas.Include(e => e.Curriculums).Include(e => e.Especialidades).Include(e => e.Membresias).Include(e => e.Login).Where(m => m.Especialidades.NombreEspecialidad.ToUpper().Contains(nombre.ToUpper()) && m.Activo == true && m.Membresias.Estado == true).OrderByDescending(e => e.IdEspecialista).Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE); 
                max_size = db.Especialistas.Include(m=> m.Especialidades).Where(m => m.Especialidades.NombreEspecialidad.ToUpper().Contains(nombre.ToUpper()) &&  m.Activo == true && m.Membresias.Estado == true).ToList().Count;
                ViewBag.especialidad = nombre;
            }
            else if (string.IsNullOrWhiteSpace(nombre) && !string.IsNullOrWhiteSpace(ciudad))
            {
                data = db.Especialistas.Include(e => e.Curriculums).Include(e => e.Especialidades).Include(e => e.Membresias).Where(m => m.Direcciones.Any(s=> s.Ciudades.NombreCiudad.ToUpper().Contains(ciudad.ToUpper()) || s.Calle.ToUpper().Contains(ciudad.ToUpper()) && m.IdEspecialista==s.IdEspecialista) && m.Activo == true && m.Membresias.Estado == true ).OrderByDescending(e => e.IdEspecialista).Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
                max_size = db.Especialistas.Include(m=> m.Direcciones ).Where(m => m.Direcciones.Any(s => s.Ciudades.NombreCiudad.ToUpper().Contains(ciudad.ToUpper()) || s.Calle.ToUpper().Contains(ciudad.ToUpper()) && m.IdEspecialista == s.IdEspecialista) && m.Activo == true && m.Membresias.Estado == true).ToList().Count;
                ViewBag.especialidad = nombre;
            }
            else if (!string.IsNullOrWhiteSpace(nombre) && !string.IsNullOrWhiteSpace(ciudad))
            {
                data = db.Especialistas.Include(e => e.Curriculums).Include(e => e.Especialidades).Include(e => e.Membresias).Where(m => m.Direcciones.Any(s => s.Ciudades.NombreCiudad.ToUpper().Contains(ciudad.ToUpper()) || s.Calle.ToUpper().Contains(ciudad.ToUpper()) && m.IdEspecialista == s.IdEspecialista) && m.Especialidades.NombreEspecialidad.ToUpper().Contains(nombre.ToUpper()) && m.Activo == true && m.Membresias.Estado == true).OrderByDescending(e => e.IdEspecialista).Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
                max_size = db.Especialistas.Include(m => m.Direcciones).Where(m => m.Direcciones.Any(s => s.Ciudades.NombreCiudad.ToUpper().Contains(ciudad.ToUpper()) || s.Calle.ToUpper().Contains(ciudad.ToUpper()) && m.IdEspecialista == s.IdEspecialista) && m.Especialidades.NombreEspecialidad.ToUpper().Contains(nombre.ToUpper()) &&  m.Activo == true && m.Membresias.Estado == true).ToList().Count;
                ViewBag.especialidad = nombre;
            }

            EspecialistasModel model = new EspecialistasModel
            {
                Especialistas = data,
                Pagination = new PaginationModel
                {
                    CurrentPage = page,
                    ItemsPerPage = PAGE_SIZE,
                    TotalItems = max_size
                }

            };


            List<Direcciones> direcciones = db.Direcciones.Include(m => m.Especialistas).ToList();
            List<Publicidades> listado1 = db.Publicidades.Where(m => m.TipoPublicidad.ToUpper() == "Fondo".ToUpper()).ToList();
            List<Publicidades> listado2 = db.Publicidades.Where(m => m.TipoPublicidad.ToUpper() == "Lateral derecha superior".ToUpper()).ToList();
            List<Publicidades> listado3 = db.Publicidades.Where(m => m.TipoPublicidad.ToUpper() == "Superior horizontal".ToUpper()).ToList();
            List<Publicidades> listado4 = db.Publicidades.Where(m => m.TipoPublicidad.ToUpper() == "Lateral derecha inferior".ToUpper()).ToList();

            ViewBag.listado = direcciones;
            ViewBag.publicidadF = listado1;
            ViewBag.publicidadDS = listado2;
            ViewBag.publicidadSH = listado3;
            ViewBag.publicidadDI = listado4;

            return View(model);
        }

        public JsonResult paginacion(int index = 1)
        {
            return null;
        }

        // GET: Especialista/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Especialistas especialistas = db.Especialistas.Find(id);
            if (especialistas == null)
            {
                return HttpNotFound();
            }
            return View(especialistas);
        }

        // GET: Especialista/Create
        public ActionResult Create()
        {
            ViewBag.IdCurriculum = new SelectList(db.Curriculums, "IdCV", "ImagenCV");
            ViewBag.IdEspecialidad = new SelectList(db.Especialidades, "IdEspecialidad", "NombreEspecialidad");
            ViewBag.IdSuscripcion = new SelectList(db.Membresias, "IdMembresia", "TipoMembresia");
            ViewBag.IdLogin = new SelectList(db.Login, "IdLogin", "UsuarioLogin");
            return View();
        }

        // POST: Especialista/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_nuevo()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            HttpPostedFileBase imagenM = (Request.Files.Count > 0) ? Request.Files[0] : null;
            string encodedData = string.Empty;

            string nombre = Request["nombre_especialista"];
            string apellidos = Request["apellido_especialista"];
            string foto = Request["foto-especialista"];
            string pagina = Request["pagina-especialista"];
            string email = Request["email_especialista"];
            string celular = Request["celular-especialista"];
            int especialidad = 0;

            Int32.TryParse(Request["idEspecialidad"], out especialidad);


            string tipo_menbresia = Request["tipo_membresia"];
            DateTime fecha_inicio;
            DateTime fecha_fin;
            DateTime.TryParse(Request["fecha_inicio_membresia"], out fecha_inicio);
            DateTime.TryParse(Request["fecha_fin_membresia"], out fecha_fin);

            string departamento = Request["Departamento-consultorio"];
            int ciudad = 0;

            Int32.TryParse(Request["idCiudad"], out ciudad);

            string direccion = Request["calle_consultorio"];
            string telefono = Request["telefono-consultorio"];
            string latitud = Request["latitud"];
            string longitud = Request["longitud"];

            string contrasena1 = Request["contraseña_especialista"];
            string contrasena2 = Request["confirmar_contraseña_especialista"];

            if (!string.IsNullOrWhiteSpace(nombre) && !string.IsNullOrWhiteSpace(apellidos) && !string.IsNullOrWhiteSpace(email)
                            && fecha_inicio != null && especialidad > 0 && fecha_fin != null && ciudad > 0
                            && !string.IsNullOrWhiteSpace(direccion) && !string.IsNullOrWhiteSpace(contrasena1) && !string.IsNullOrWhiteSpace(contrasena2))
            {
                if (contrasena1.Equals(contrasena2))
                {

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
                                List<TipoUsuario> tipo_usuario = db.TipoUsuario.ToList().Where(m => m.IdTipoUsuario == 3).ToList();
                                List<Especialidades> especialidades = db.Especialidades.ToList().Where(m => m.IdEspecialidad == especialidad).ToList();
                                if (tipo_usuario.Count > 0 && especialidades.Count > 0)
                                {
                                    Membresias menbresia = new Membresias
                                    {
                                        TipoMembresia = tipo_menbresia,
                                        FechaInicio = fecha_inicio,
                                        FechaFin = fecha_fin
                                    };

                                    Especialistas especialistas = new Especialistas
                                    {
                                        Activo = true,
                                        NombreEsp = nombre,
                                        ApellidoEsp = apellidos,
                                        PaginaWebEsp = pagina,
                                        CelularEsp = celular,
                                        EmailEsp = email,
                                        FotoEsp = encodedData,
                                        Especialidades = especialidades[0],
                                        Login = new Login
                                        {
                                            TipoUsuario = tipo_usuario[0],
                                            UsuarioLogin = email,
                                            PasswordLogin = CifradoDatos.cifrarPassword(contrasena1)
                                        },
                                        IdSuscripcion = menbresia.IdMembresia
                                    };

                                    Direcciones consultorio = new Direcciones
                                    {
                                        Calle = direccion,
                                        Ciudades = new Ciudades
                                        {
                                            IdCiudad = ciudad
                                        },
                                        TelefonoFijo = telefono,
                                        Latitud = latitud,
                                        Longitud = longitud,
                                        Especialistas = especialistas
                                    };
                                    db.Membresias.Add(menbresia);
                                    db.Especialistas.Add(especialistas);
                                    db.Direcciones.Add(consultorio);
                                    db.SaveChanges();

                                    lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificacion", cuerpo = "Especialista registrada exitosamente." });

                                    TempData["mensajes"] = lstMensajes;
                                }
                                else
                                {
                                    lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "Ocurrio un problema procesando la solicitud" });
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
            }
            return RedirectToAction("InsertarEspecialista", "Administrador");
        }

        // GET: Especialista/Edit/5
        public ActionResult Edit(int? id)
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Especialistas> especialistas = db.Especialistas.Where(m => m.Login.IdLogin == id).ToList();
            if (especialistas == null)
            {
                return HttpNotFound();
            }
            else if (especialistas.Count == 0)
            {
                return HttpNotFound();
            }

            ViewBag.listado = db.Especialidades.ToList();
            ViewBag.listado2 = db.Ciudades.ToList();
            ViewBag.modelo = especialistas[0];
            return View();
        }

        // POST: Especialista/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdEspecialista,NombreEsp,ApellidoEsp,FotoEsp,PaginaWebEsp,EmailEsp,CelularEsp,Calificacion,IdCurriculum,IdSuscripcion,IdLogin,Activo")] Especialistas especialista_aux)
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            HttpPostedFileBase imagenM = (Request.Files.Count > 0) ? Request.Files[0] : null;
            string encodedData = string.Empty;

            if (ModelState.IsValid)
            {
                UsuarioAuth usuario = (UsuarioAuth)Session["usuario"];
                List<Especialistas> especialista = db.Especialistas.ToList().Where(m => m.IdLogin == usuario.id).ToList();
                if (especialista.Count > 0)
                {
                    try
                    {
                        string tipoMembresia = Request["tipo-membresia"];
                        DateTime fecha_inicio;
                        DateTime fecha_fin;
                        DateTime.TryParse(Request["Modelo.Membresias.FechaInicio"], out fecha_inicio);
                        DateTime.TryParse(Request["Modelo.Membresias.FechaFin"], out fecha_fin);

                        string departamento = Request["Departamento-consultorio"];
                        int ciudad = 0;

                        Int32.TryParse(Request["idCiudad"], out ciudad);

                        int especialidad = 0;

                        Int32.TryParse(Request["idEspecialidad"], out especialidad);

                        string direccion = Request["calle-consultorio"];
                        string telefono = Request["telefono-consultorio"];
                        string latitud = Request["latitud"];
                        string longitud = Request["longitud"];

                        string contrasena1 = Request["contraseña-especialista"];
                        string contrasena2 = Request["confirmar-contraseña-especialista"];

                        Especialidades especialidad_Db = db.Especialidades.Find(especialidad);
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
                            especialista[0].FotoEsp = encodedData;
                        }
                        especialista[0].NombreEsp = especialista_aux.NombreEsp;
                        especialista[0].ApellidoEsp = especialista_aux.ApellidoEsp;
                        especialista[0].PaginaWebEsp = especialista_aux.PaginaWebEsp;
                        especialista[0].CelularEsp = especialista_aux.CelularEsp;
                        especialista[0].EmailEsp = especialista_aux.EmailEsp;

                        especialista[0].Especialidades = especialidad_Db;

                        especialista[0].Membresias.TipoMembresia = tipoMembresia;
                        especialista[0].Membresias.FechaInicio = fecha_inicio;
                        especialista[0].Membresias.FechaFin = fecha_fin;

                        especialista[0].Direcciones.ToList()[0].Area = departamento;
                        especialista[0].Direcciones.ToList()[0].IdCiudad = ciudad;

                        especialista[0].Direcciones.ToList()[0].Calle = direccion;
                        especialista[0].Direcciones.ToList()[0].TelefonoFijo = telefono;
                        especialista[0].Direcciones.ToList()[0].Latitud = latitud;
                        especialista[0].Direcciones.ToList()[0].Longitud = longitud;

                        if (contrasena1.Equals(contrasena2) && !contrasena1.Equals("##########") && !string.IsNullOrWhiteSpace(contrasena1))
                        {
                            especialista[0].Login.PasswordLogin = CifradoDatos.cifrarPassword(contrasena1);
                        }

                        db.Entry(especialista[0]).State = EntityState.Modified;
                        db.SaveChanges();
                        lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificacion", cuerpo = "Especialista editado exitosamente." });
                        TempData["mensajes"] = lstMensajes;
                        return RedirectToAction("Index");
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
                    lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No cuenta con privilegios para realizar esta acción, inicie sesión" });
                    TempData["mensajes"] = lstMensajes;
                }

            }
            lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "Error" + ":" + "Ocurrio un error mientras se procesaba la solicitud" });
            TempData["mensajes"] = lstMensajes;
            return View();
        }

        // GET: Especialista/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Especialistas especialistas = db.Especialistas.Find(id);
            if (especialistas == null)
            {
                return HttpNotFound();
            }
            return View(especialistas);
        }

        // POST: Especialista/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Especialistas especialistas = db.Especialistas.Find(id);
            db.Especialistas.Remove(especialistas);
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



