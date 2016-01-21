using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medinlinea.Models
{
    public class Mensaje
    {
        public string tipo { get; set; }
        public string titulo { get; set; }

        public string cuerpo { get; set; }

        public Mensaje() { }

    }
}