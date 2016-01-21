using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Medinlinea.Models;

namespace Medinlinea.Controllers
{
    public class ArticulosEspecialistaController : Controller
    {
        private MedinlineaEntities db = new MedinlineaEntities();
        // GET: ArticulosEspecialista
        public ActionResult Index()
        {
            return View();
        }


    }
}