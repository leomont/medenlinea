using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medinlinea.Controllers
{
    public class VendedorController : Controller
    {
        // GET: Vendedor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InsertarEspecialista()
        {
            return View();
        }

        public ActionResult AgregarConsultorio()
        {
            return View();
        }
    }
}