using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medinlinea.Models
{
    public class UsuarioAuth
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public long rol { get; set; }
        public string rolNombre { get; set; }


    }
}