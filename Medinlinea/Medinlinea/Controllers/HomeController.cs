using Medinlinea.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Medinlinea.Controllers
{
    public class HomeController : Controller
    {
        private Random R = new Random();
        private MedinlineaEntities db = new MedinlineaEntities();

        // GET: Home
        public ActionResult Index()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];

            ViewBag.lstMensajes = lstMensajes;
            TempData.Remove("mensajes");

            ViewBag.listado1 =db.Publicidades.Where(m=> m.TipoPublicidad.ToUpper() == "FONDO").ToList();
            ViewBag.listado2 = db.Especialidades.Where(m=> m.Estado == true).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Autenticacion()
        {
            string username = Request["user_name"];
            string contrasena = Request["contrasena"];
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];
            try
            {
                if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(contrasena))
                {
                    //Cifrado de la contrasena
                    string password = CifradoDatos.cifrarPassword(contrasena);

                    List<Login> listado = db.Login.ToList().Where(m => m.UsuarioLogin==username && m.PasswordLogin==password).ToList();
                    if (listado.Count > 0)
                    {
                        if ((listado[0].IdTipoUsuario == 1 || listado[0].IdTipoUsuario == 2) && listado[0].Usuarios.Count > 0)
                        {
                            Session["usuario"] = new UsuarioAuth
                            {
                                id = listado[0].IdLogin,
                                nombre = listado[0].Usuarios.ToList()[0].NombreUsuario + " " + listado[0].Usuarios.ToList()[0].ApellidoUsuario,
                                rol = listado[0].TipoUsuario.IdTipoUsuario,
                                rolNombre = listado[0].TipoUsuario.NombreTipoUsuario
                            };
                            if (listado[0].IdTipoUsuario == 1)
                            {
                                MvcApplication.admOnline = true;
                            }

                            lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificacion", cuerpo = "Usuario autenticado exitosamente." });
                        }
                        else if (listado[0].IdTipoUsuario == 3 && listado[0].Especialistas.Count > 0)
                        {
                            Session["usuario"] = new UsuarioAuth
                            {
                                id = listado[0].IdLogin,
                                nombre = listado[0].Especialistas.ToList()[0].NombreEsp + " " + listado[0].Especialistas.ToList()[0].ApellidoEsp,
                                rol = listado[0].TipoUsuario.IdTipoUsuario,
                                rolNombre = listado[0].TipoUsuario.NombreTipoUsuario
                            };
                            lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificacion", cuerpo = "Usuario autenticado exitosamente." });
                        }
                        else
                        {
                            lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "Usuario o contrasena invalidos, verifique" });
                            TempData["mensajes"] = lstMensajes;
                        }
                    }
                    else
                    {
                        lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "Usuario o contrasena invalidos, verifique" });
                        TempData["mensajes"] = lstMensajes;
                    }
                }
                else
                {
                    lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No cuenta con privilegios para procesar la solicitud." });
                }

            }
            catch (Exception ex)
            {
                SystemLog log = new SystemLog();
                log.ErrorLog(ex.Message);
            }
            TempData["mensajes"] = lstMensajes;
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult CerrarSesion()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];
            if (Session["usuario"] != null)
            {
                UsuarioAuth usuario = (UsuarioAuth)Session["usuario"];
                if (usuario.rol == 1)
                {
                    MvcApplication.admOnline = false;
                }
                Session.RemoveAll();
                lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificacion", cuerpo = "Sesión finalizada." });
            }
            else
            {
                lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No cuenta con privilegios para procesar la solicitud." });
            }
            TempData["mensajes"] = lstMensajes;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult RestaurarPassword()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];            
            TempData["mensajes"] = lstMensajes;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SolicitarRestaurar()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];
            string usuario_login = Request["email"];

            if (!string.IsNullOrWhiteSpace(usuario_login))
            {
                try
                {
                    List<Login> listado = db.Login.ToList().Where(m => m.UsuarioLogin == usuario_login).ToList();

                    if (listado != null)
                    {
                        if (listado.Count > 0)
                        {
                            //Elimino solicitudes previas
                            List<Restaurar_Pass> listado_ant = db.Restaurar_Pass.ToList().Where(m => m.correo_usuario == usuario_login).ToList();
                            if(listado_ant != null)
                            {
                                foreach(Restaurar_Pass obj in listado_ant)
                                {
                                    db.Restaurar_Pass.Remove(obj);
                                }                            
                            }
                            //Genero una nueva clave
                            string clave = GetRandomString(15);
                            //Genero una clave para el correo
                            string clave_email = CifradoDatos.cifrarRSA(usuario_login);
                            string clave_code = CifradoDatos.cifrarRSA(clave);

                            db.Restaurar_Pass.Add(new Restaurar_Pass { correo_usuario = usuario_login, clave = clave });
                            db.SaveChanges();
                            //Creo corrreo con clave para enviar al usuario
                            StringBuilder bodyMail = new StringBuilder();
                            bodyMail.AppendLine("Se ha activado la restauración de contraseña para tu cuenta de usuario, " + usuario_login + "</br>");
                            string informacionHost = serverInfoCurrent();
                            bodyMail.AppendLine("Dirijase al siguiente enlace para continuar con el proceso. " + "<a href=\"" + informacionHost + "/Home/ValidarRestaurar?tokenCorreo=" + clave_email + "&tokenClave=" + clave_code + "\"> Restaurar contraseña perfil. </a>" + "</br>");
                            bodyMail.AppendLine("Fecha:" + DateTime.Now.ToString() + "</br>");
                            string subject = "Notificación modificación contraseña.";
                            Mail mail = new Mail(usuario_login, subject, bodyMail);

                            if (mail.sendMail().Result)
                            {
                                lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificación", cuerpo = "Proceso finalizado exitosamente, verifique su correo electronico." });
                            }
                            else
                            {
                                lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "Se produjo un error mientras se enviaba el correo. Correo invalido" });
                            }
                        }
                        else
                        {
                            lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "El usuario no existe o se encuentra bloqueado." });
                        }
                    }
                    else
                    {
                        lstMensajes.Add(new Mensaje{tipo="Error", titulo="Error", cuerpo="El usuario no existe o se encuentra bloqueado."});
                    }
                }
                catch (Exception ex)
                {
                    SystemLog log = new SystemLog();
                    log.ErrorLog(ex.Message);
                    lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "Se ha producido un error mientras se generaba el formulario" });
                    TempData["mensajes"] = lstMensajes;
                }
            }
            else
            {
                lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "El usuario no existe o se encuentra bloqueado." });
            }
            TempData["mensajes"] = lstMensajes;
            return RedirectToAction("Index", "Home");
        }


        public ActionResult ValidarRestaurar(string tokenCorreo, string tokenClave)
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];
            if (!string.IsNullOrWhiteSpace(tokenCorreo) && !string.IsNullOrWhiteSpace(tokenClave))
            {
                try
                {
                    string correo = CifradoDatos.descifrarRSA(tokenCorreo);
                    string clave = CifradoDatos.descifrarRSA(tokenClave);
                    //Busco la entidad que cumpla con los parametros de correo y clave
                    List<Restaurar_Pass> listado_ant = db.Restaurar_Pass.ToList().Where(m => m.correo_usuario == correo && m.clave==clave).ToList();
                    if (listado_ant != null)
                    {
                        if (listado_ant.Count > 0)
                        {
                            TempData["usuario_restore"] = correo;
                            return RedirectToAction("Restaurar", "Home");
                        }
                    }
                }
                catch (Exception ex)
                {
                    SystemLog log = new SystemLog();
                    log.ErrorLog(ex.Message);
                    lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "Se ha producido un error mientras se generaba el formulario" });
                    TempData["mensajes"] = lstMensajes;
                }
            }
            lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No posee los privilegios suficientes para ejecutar este proceso." });
            TempData["mensajes"] = lstMensajes;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Restaurar()
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];
            TempData["mensajes"] = lstMensajes;
            return View();
        }

        [HttpPost]
        public ActionResult UpdateRestorePassword(string nueva_contrasena)
        {
            List<Mensaje> lstMensajes = (((List<Mensaje>)TempData["mensajes"]) == null) ? new List<Mensaje>() : (List<Mensaje>)TempData["mensajes"];
            try
            {
                string correo_logueo = (string)TempData["usuario_restore"];
                if (!string.IsNullOrWhiteSpace(correo_logueo))
                {
                    List<Login> listado = db.Login.ToList().Where(m => m.UsuarioLogin == correo_logueo).ToList();
                    //Si el correo_electronico y la contrasena son validas subo a session la institucion
                    if (listado != null)
                    {
                        if (listado.Count > 0)
                        {
                            listado[0].PasswordLogin= CifradoDatos.cifrarPassword(nueva_contrasena);
                            db.SaveChanges();

                            //Obtengo ip y localizacion desde donde se realizo la modificacion
                            object[] infoLogin = getExternalIp();
                            //Envio email confirmacion para habilitar el perfil
                            StringBuilder bodyMail = new StringBuilder();
                            bodyMail.AppendLine("La contraseña para tu cuenta de usuario, " + listado[0].UsuarioLogin + " se ha cambiado recientemente." + "</br>");
                            bodyMail.AppendLine("Si la has cambiado tú, no necesitas realizar ninguna otra acción." + "</br>");
                            bodyMail.AppendLine("Si no la has cambiado tú, es posible que tu cuenta haya sido vulnerada. Para poder volver a iniciar sesión en tu cuenta, deberás restablecer la contraseña" + "</br>");
                            if (infoLogin != null)
                            {
                                bodyMail.AppendLine("Informacion Adicional:" + "</br>");
                                bodyMail.AppendLine("Pais:" + infoLogin[2].ToString() + "</br>");
                                bodyMail.AppendLine("Departamento:" + infoLogin[4].ToString() + "</br>");
                                bodyMail.AppendLine("Ciudad:" + infoLogin[5].ToString() + "</br>");
                                bodyMail.AppendLine("Ip Address:" + infoLogin[0].ToString() + "</br>");
                            }
                            bodyMail.AppendLine("Fecha:" + DateTime.Now.ToString() + "</br>");
                            string subject = "Notificación modificación contraseña.";
                            Mail mail = new Mail(correo_logueo, subject, bodyMail);

                            if (mail.sendMail().Result)
                            {
                                lstMensajes.Add(new Mensaje { tipo = "Notificacion", titulo = "Notificación", cuerpo = "Contraseña modificada exitosamente." });
                                TempData["mensajes"] = lstMensajes;
                                TempData.Remove("usuario_restore");
                            }
                            else
                            {
                                lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "Se produjo un error mientras se enviaba el correo. Correo invalido" });
                            }
                        }
                        else
                        {
                            lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No posee los permisos suficientes para realizar esta operación." });
                            TempData["mensajes"] = lstMensajes;
                        }
                    }
                    else
                    {
                        lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No posee los permisos suficientes para realizar esta operación." });
                        TempData["mensajes"] = lstMensajes;
                    }
                }
                else
                {
                    lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "No posee los permisos suficientes para realizar esta operación." });
                    TempData["mensajes"] = lstMensajes;
                }

            }
            catch (Exception ex)
            {
                SystemLog log = new SystemLog();
                log.ErrorLog(ex.Message);
                lstMensajes.Add(new Mensaje { tipo = "Error", titulo = "Error", cuerpo = "Se ha producido un error mientras se generaba el formulario" });
                TempData["mensajes"] = lstMensajes;
            }
            TempData["mensajes"] = lstMensajes;
            ViewBag.lstMensajes = lstMensajes;
            return RedirectToAction("Index", "Home");
        }

        private object[] getExternalIp()
        {
            string externalIP = string.Empty;
            object[] infoIpAddress = null;
            try
            {
                externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
                externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                             .Matches(externalIP)[0].ToString();

                /*-----------------------------------------------------------------------------------------*/
                WebRequest rssReq = WebRequest.Create("http://freegeoip.net/xml/" + externalIP);
                WebProxy px = new WebProxy("http://freegeoip.net/xml/" + externalIP, true);
                rssReq.Proxy = px;
                rssReq.Timeout = 5000;
                WebResponse rep = rssReq.GetResponse();
                XmlTextReader xtr = new XmlTextReader(rep.GetResponseStream());
                DataSet ds = new DataSet();
                ds.ReadXml(xtr);
                infoIpAddress = ds.Tables[0].Rows[0].ItemArray;
            }
            catch (Exception ex)
            {
                SystemLog log = new SystemLog();
                log.ErrorLog(ex.Message);
            }


            return infoIpAddress;
        }

        public string GetRandomString(int Length)
        {
            char[] ArrRandomChar = new char[Length];
            for (int i = 0; i < Length; i++)
            {
                ArrRandomChar[i] = (char)('a' + R.Next(0, 56));
            }

            return new string(ArrRandomChar);
        }

        public static string serverInfoCurrent()
        {
            string port = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            if (port == null || port == "80" || port == "443")
            {
                port = "";
            }
            else
            {
                port = ":" + port;
            }

            string protocol = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (protocol == null || protocol == "0")
            {
                protocol = "http://";
            }
            else
            {
                protocol = "https://";
            }

            return protocol + System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + port;
        }

    }
}