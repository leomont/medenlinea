using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Medinlinea
{
    public class MvcApplication : System.Web.HttpApplication
    {
        // variable global para saber si el admin esta conectado
        public static volatile bool admOnline;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            admOnline = false;
        }
    }
}
