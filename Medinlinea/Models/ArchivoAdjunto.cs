using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;

namespace Medinlinea.Models
{
    public class ArchivoAdjunto
    {
        public MemoryStream archivo { get; set; }
        public string nombre { get; set; }
        public string tipo { get; set; }
    }
}