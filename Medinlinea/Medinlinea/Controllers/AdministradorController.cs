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
    public class AdministradorController : Controller
    {
        private MedinlineaEntities db = new MedinlineaEntities();

        // GET: Administrador
        public ActionResult Index()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");
            return View();
        }

        public ActionResult EstadisticasEspecialista()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");
            return View();
        }

        public ActionResult EstadisticasGlobales()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");
            return View();
        }

        public ActionResult EstadisticasCalificacion()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");
            return View();
        }

        public ActionResult IngresarComercial()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");
            return View();
        }

        public ActionResult EliminarComercial()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");
            return View();
        }

        public ActionResult InsertarEspecialista()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");

            ViewBag.listado = db.Especialidades.ToList();
            ViewBag.listado2 = db.Ciudades.ToList();

            return View();
        }

        public ActionResult AgregarConsultorio()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");

            ViewBag.listado = db.Especialistas.ToList();
            ViewBag.listado2 = db.Ciudades.ToList();

            return View();
        }

        public ActionResult VerVigencia()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");
            return View();
        }

        public ActionResult GestionarSuscripcion()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");

            ViewBag.listado = db.Especialistas.ToList();
            return View();
        }

        public ActionResult AgregarCiudad()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");
            return View();
        }

        public ActionResult EliminarCiudad()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");
            return View();
        }

        public ActionResult ListarCiudad()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");
            return View();
        }

        public ActionResult AgregarEspecialidad()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");
            return View();
        }

        public ActionResult EliminarEspecialidad()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");
            return View();
        }

        public ActionResult EspecialidadCarrusel()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");

            ViewBag.listado = db.Especialidades.ToList();
            return View();
        }

        public ActionResult AdicionarPublicidad()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");

            ViewBag.listado = db.Especialistas.ToList();
            return View();
        }

        public ActionResult EliminarPublicidad()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");
            return View();
        }
        [HttpGet]
        public JsonResult getChats()
        {
            List<Chats> listado = db.Chats.OrderBy(m => m.id).Skip(Math.Max(0, db.Chats.ToList().Count - 20)).Take(10).ToList();

            var resulSet = Json(new { listado }, "application/json; charset=utf-8", JsonRequestBehavior.AllowGet);
            resulSet.MaxJsonLength = 2147483647;
            return resulSet;
        }

        [HttpGet]
        public JsonResult createChat(string nombre, string mensaje)
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];
            UsuarioAuth usuario = (UsuarioAuth)Session["usuario"];
            try
            {
                if (usuario != null)
                {
                    if (usuario.rol == 1)
                    {
                        nombre = "ADMINISTRADOR";
                    }
                }
                if (!string.IsNullOrWhiteSpace(nombre) && !string.IsNullOrWhiteSpace(nombre))
                {
                    if (usuario != null)
                    {
                        if (usuario.rol == 1)
                        {
                            db.Chats.Add(
                        new Chats
                        {
                            nombre = "ADMINISTRADOR",
                            mensaje = mensaje,
                            tipo_usuario = "ADMINISTRADOR",
                            fecha_creacion = DateTime.Now
                        }
                        );
                        }
                    }
                    else
                    {
                        db.Chats.Add(
                            new Chats
                            {
                                nombre = nombre,
                                mensaje = mensaje,
                                tipo_usuario = "USUARIO",
                                fecha_creacion = DateTime.Now
                            }
                            );
                    }
                    db.SaveChanges();
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
            List<Chats> listado = db.Chats.OrderBy(m => m.id).Skip(Math.Max(0, db.Chats.ToList().Count - 20)).Take(10).ToList();

            var resulSet = Json(new { listado }, "application/json; charset=utf-8", JsonRequestBehavior.AllowGet);
            resulSet.MaxJsonLength = 2147483647;
            return resulSet;
        }
    }
}