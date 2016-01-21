using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Medinlinea
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Por: Leonardo Montes
            //Fecha: 27/12/2015
            //Nota: Configuracion de url estaticas para la paginacion de articulos y especialistas

            routes.MapRoute(
            name: null,
            url: "Especialista/Page{page}",
            defaults: new { Controller = "Especialista", action = "Index" });

            routes.MapRoute(
            name: null,
            url: "Articulo/Page{page}",
            defaults: new { Controller = "Articulo", action = "Index" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
