using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medinlinea.Models
{
    public class CiudadModel
    {
        public IEnumerable<Ciudades> Ciudades { get; set; }
        public PaginationModel Pagination { get; set; }
    }
}